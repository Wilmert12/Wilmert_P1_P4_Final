namespace Wilmert_P1_P4_Final.Models;

public record AutoresRecord(
    int IdAutor,
    string Nombres,
    string Nacionalidad,
    DateTime FechaNacimiento,
    float Sueldo)
{

    public AutoresRecord() : this(0, "", "", default, 0) { }
}