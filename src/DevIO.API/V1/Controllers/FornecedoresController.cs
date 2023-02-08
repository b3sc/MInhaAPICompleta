using AutoMapper;
using DevIO.API.Controllers;
using DevIO.API.Extensions;
using DevIO.API.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.API.V1.Controllers;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class FornecedoresController : MainController
{
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly IFornecedorService _fornecedorService;
    private readonly IEnderecoRepository _enderecoRepository;
    private readonly IMapper _mapper;

    public FornecedoresController(IFornecedorRepository fornecedorRepository,
                                    IMapper mapper,
                                    IFornecedorService fornecedorService,
                                    IEnderecoRepository enderecoRepository,
                                    INotificador notificador,
                                    IUser user) : base(notificador, user)
    {
        _fornecedorRepository = fornecedorRepository;
        _fornecedorService = fornecedorService;
        _enderecoRepository = enderecoRepository;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IEnumerable<FornecedorViewModel>> ObterTodos()
    {
        var fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
        return fornecedores;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
    {
        var fornecedor = await ObterFornecedorProdutosEndereco(id);

        if (fornecedor == null) return NotFound();

        return fornecedor;
    }

    [ClaimsAuthorize("Fornecedor", "Adicionar")]
    [HttpPost]
    public async Task<ActionResult<FornecedorViewModel>> Adicionar(FornecedorViewModel fornecedorViewModel)
    {
        if (!ModelState.IsValid) return CustomResponse(ModelState);

        await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorViewModel));

        return CustomResponse(fornecedorViewModel);

        //if (!result) return BadRequest();
        //return Ok(_mapper.Map<FornecedorViewModel>(fornecedor));
    }

    [ClaimsAuthorize("Fornecedor", "Atualizar")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
    {
        //if(id != fornecedorViewModel.Id) return BadRequest();
        if (id != fornecedorViewModel.Id)
        {
            NotificarErro("Id informado não é o mesmo passado na query");
            return CustomResponse(fornecedorViewModel);
        }

        if (!ModelState.IsValid) return CustomResponse(ModelState);

        await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));

        return CustomResponse(fornecedorViewModel);

    }

    [ClaimsAuthorize("Fornecedor", "Excluir")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
    {
        var fornecedorViewModel = await ObterFornecedorEndereco(id);
        if (fornecedorViewModel == null) return NotFound();

        await _fornecedorService.Remover(id);

        return CustomResponse();

    }

    [HttpGet("obter-endereco/{id:guid}")]
    public async Task<EnderecoViewModel> ObterEnderecoId(Guid id)
    {
        return _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterPorId(id));
    }

    [ClaimsAuthorize("Fornecedor", "Atualizar")]
    [HttpPost("atualizar-endereco/{id:guid}")]
    public async Task<IActionResult> AtualizarEndereco(Guid id, EnderecoViewModel enderecoViewModel)
    {
        if (id != enderecoViewModel.Id) return BadRequest();

        if (!ModelState.IsValid) return CustomResponse(ModelState);

        await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(enderecoViewModel));

        return CustomResponse(enderecoViewModel);

    }

    private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
    {
        return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
    }

    private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
    {
        return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
    }

    private ActionResult<FornecedorViewModel> ResultServices(FornecedorViewModel fornecedor, bool result)
    {
        if (!result) return BadRequest();

        return Ok(fornecedor);
    }

}