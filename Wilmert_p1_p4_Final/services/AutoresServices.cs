using Dapper;
using Microsoft.Data.Sqlite;
using Parcial1_P4_Wilmert.Models;

namespace Parcial1_P4_Wilmert.Services;

public class NumbersService(IConfiguration configuration)
{
    private readonly string _connectionString =
        configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "Falta 'DefaultConnection' en appsettings.json");

    private SqliteConnection CreateConnection() => new(_connectionString);

    public async Task InitializeAsync()
    {
        const string sql = @"
            CREATE TABLE IF NOT EXISTS NumberRecords (
                Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                Fecha     TEXT    NOT NULL,
                Numero    INTEGER NOT NULL,
                Resultado INTEGER NOT NULL
            );";

        await using var connection = CreateConnection();
        await connection.ExecuteAsync(sql);
    }

    public async Task<NumberRecord> SaveAsync(NumberRecord record)
    {
        const string sql = @"
            INSERT INTO NumberRecords (Fecha, Numero, Resultado)
            VALUES (@Fecha, @Numero, @Resultado);
            SELECT last_insert_rowid();";

        await using var connection = CreateConnection();
        var nuevoId = await connection.ExecuteScalarAsync<long>(sql, record);

        return record with { Id = (int)nuevoId };
    }

    public async Task<bool> UpdateAsync(NumberRecord record)
    {
        const string sql = @"
            UPDATE NumberRecords
            SET Fecha = @Fecha, Numero = @Numero, Resultado = @Resultado
            WHERE Id = @Id;";

        await using var connection = CreateConnection();
        var filasAfectadas = await connection.ExecuteAsync(sql, record);
        return filasAfectadas > 0;
    }

    public async Task<NumberRecord?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT Id, Fecha, Numero, Resultado
            FROM NumberRecords
            WHERE Id = @Id;";

        await using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<NumberRecord>(sql, new { Id = id });
    }

    public async Task<List<NumberRecord>> GetListAsync()
    {
        const string sql = @"
            SELECT Id, Fecha, Numero, Resultado
            FROM NumberRecords
            ORDER BY Fecha DESC;";

        await using var connection = CreateConnection();
        var result = await connection.QueryAsync<NumberRecord>(sql);
        return result.ToList();
    }
}