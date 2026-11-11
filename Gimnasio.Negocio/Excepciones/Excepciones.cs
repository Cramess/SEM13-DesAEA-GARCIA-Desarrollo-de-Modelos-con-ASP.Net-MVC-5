namespace Gimnasio.Negocio.Excepciones;
public class ReglaNegocioException : Exception
{
    public ReglaNegocioException(string mensaje) : base(mensaje) { }
}
public class DatoInvalidoException : Exception
{
    public string Campo { get; }
    public DatoInvalidoException(string campo, string mensaje) : base(mensaje)
    {
        Campo = campo;
    }
}
