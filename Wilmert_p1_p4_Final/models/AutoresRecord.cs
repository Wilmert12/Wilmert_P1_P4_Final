namespace Parcial1_P4_Wilmert.Models;

public record AutoresRecord(int Idautor,String nombre,string nacionalidad, DateTime Fecha, int Numero, float sueldo)
{
       public AutoresRecord() : this(0, default, default, default, 0, 0) { }

}