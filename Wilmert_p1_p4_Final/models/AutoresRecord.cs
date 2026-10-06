namespace Wilmert_P1_P4_Final.Models;

public record AutoresRecord(
    int Idautor,
    string nombre,
    string nacionalidad,
    DateTime Fecha,
    int Numero,
    float sueldo)
{

    public AutoresRecord() : this(0, "", "", default, 0, 0) { }
}
