using Google.Protobuf.WellKnownTypes;

namespace RolESBackend.Modelos;

public class Usuario
{
    public int id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Senha { get; init; } = string.Empty;
}