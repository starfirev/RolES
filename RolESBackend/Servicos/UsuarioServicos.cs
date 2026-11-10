using MySql.Data.MySqlClient;
using RolESBackend.Dados;
using RolESBackend.DTOs;

namespace RolESBackend.Servicos;

public class UsuarioServicos(BdConexao conexao)
{
    public async Task<int> AdicionarAsync(CadastroUsuarioDto modelo)
    {
        var sql =
            "INSERT INTO usuarios(nome, email, nascimento, senha, cargo) VALUES(@nome, @email, @nascimento, @senha, 'comum')";

        var mySqlComando = new MySqlCommand(sql, await conexao.CriarConexaoAsync());

        mySqlComando.Parameters.AddWithValue("@nome", modelo.Nome);
        mySqlComando.Parameters.AddWithValue("@email", modelo.Email);
        mySqlComando.Parameters.AddWithValue("@nascimento", modelo.Nascimento.ToString("yyyy-MM-dd"));
        mySqlComando.Parameters.AddWithValue("@senha", modelo.Senha);

        var linhasAfetadas = await mySqlComando.ExecuteNonQueryAsync();

        return linhasAfetadas;

    }
}