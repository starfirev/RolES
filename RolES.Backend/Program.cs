
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
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
        {   Mensagem ="Servidor está online.", 
            Data = dataResposta
            
        });
});

app.MapPost("/usuario", async (CadastroUsuarioDto requestBody, UsuarioServicos uServicos) => // O 'app' servidor'Map' é o localiza o servidor e o 'usuário é a rota.
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

app.MapPost("/login", async (LoginUsuarioDto corpoRequisicao, UsuarioServicos uServicos) => 
{

    try
    {
        var usuarioEncontrado  = await uServicos.BuscarPorEmailAsync(corpoRequisicao.Email);

        if (usuarioEncontrado == null) return Results.NotFound(new
        {
            Mensagem = "O usuário não foi encontrado."
        });
        if (corpoRequisicao.Senha != usuarioEncontrado.Senha)
            return Results.BadRequest(new
            {
                Mensagem = "Email/senha estão incorretos."
            });
        return Results.Ok();
    }

    catch
    {
        return Results.InternalServerError();
    }
});




app.Run();

