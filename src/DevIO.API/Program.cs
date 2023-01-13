using DevIO.API.Configuration;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// *** Configurando serviços no container ***

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MeuDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Extension Method de configuração do Identity
builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddApiConfig();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    // suprimindo a forma de validação automática do model.state
    options.SuppressModelStateInvalidFilter = true;

});


builder.Services.ResolveDependencies();


var app = builder.Build();

// *** Configurando o resquest dos serviços no pipeline ***

app.UseApiConfig(app.Environment);


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Autenticacao e autorização (Identity)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
