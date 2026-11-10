
using MySql.Data.MySqlClient;
using RolESBackend.Dados;
using RolESBackend.DTOs;
using RolESBackend.Servicos;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

// Cadastro de serviço Customizados 
builder.Services.AddSingleton<BdConexao>();
builder.Services.AddScoped<UsuarioServicos>();

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

app.MapPost("/usuario", async (CadastroUsuarioDto requestBody, UsuarioServicos uServicos) =>
{

    try
    {
        var linhasAfetadas = await uServicos.AdicionarAsync(requestBody);
        
        if (linhasAfetadas > 0) return Results.Created();

        return Results.Conflict(new
            { Mensagem = "Houve um erro ao tentar criar o seu usuário. Tente novamente mais tarde." });
    }

    catch (MySqlException exception) // Dá erro ao se cadastrar com as mesmas informações.
    {
        if (exception.Number == 1062)
            return Results.BadRequest(new
            {
                Mensagem = "O e-mail do usuário já consta cadastrado."
            });
        throw;
    }
    catch
    {
        return Results.InternalServerError(); // O usuario recebe o erro de forma mais clara.
    }
});

app.Run();

