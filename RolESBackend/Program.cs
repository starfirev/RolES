using System.Data;
using RolESBackend.Dados;
using RolESBackend.DTOs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

// Cadastro de serviço Customizados 
builder.Services.AddSingleton<BdConexao>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();    
}

app.MapGet("/", () => // Metodo sem nome  
{
    var dataResposta = DateTime.UtcNow;
    return Results.Ok(new 
        {   Mensagem ="", 
            Data = dataResposta
            
        });
});

app.MapPost("/usuario", (CadastroUsuarioDto requestBody) =>
{
    
    return Results.Ok(requestBody);
});

app.Run();

