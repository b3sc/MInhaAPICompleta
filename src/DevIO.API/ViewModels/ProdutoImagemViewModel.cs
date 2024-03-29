﻿using DevIO.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevIO.API.ViewModels;


//apenas para exemplificar uma solução para resolver a questão de upload das imagens

//[ModelBinder(BinderType = typeof(JsonWithFilesFormDataModelBinder))]
// retornar com o nome produto, basicamente um alias para ser utilizado como key
[ModelBinder(typeof(JsonWithFilesFormDataModelBinder), Name = "produto")]
public class ProdutoImagemViewModel
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]

    public Guid FornecedorId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string Descricao { get; set; }

    /* IFormFile
        faz o Stream desse objeto (pequenas fatias), 
        sem que ultrapasse o limite de dados que pode ser ultrapassado dentro da aplicação
    */
    public IFormFile ImagemUpload { get; set; }

    public string Imagem { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public decimal Valor { get; set; }

    [ScaffoldColumn(false)]
    public DateTime DataCadastro { get; set; }

    public bool Ativo { get; set; }


    [ScaffoldColumn(false)]
    public string NomeFornecedor { get; set; }
}