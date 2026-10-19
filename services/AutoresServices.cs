using Dapper;
using Microsoft.Data.Sqlite;
using Wilmert_P1_P4_Final.Models;

namespace Wilmert_P1_P4_Final.Services;

public class AutoresServices(IConfiguration configuration)
{
    private readonly string _connectionString = BuildConnectionString(configuration);

    private static string BuildConnectionString(IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Falta 'DefaultConnection' en appsettings.json");

       
        var enAzure = !string.IsNullOrEmpty(
            Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
        var home = Environment.GetEnvironmentVariable("HOME");

        if (enAzure && !string.IsNullOrEmpty(home))
        {
            var dir = Path.Combine(home, "data");
            Directory.CreateDirectory(dir);
            return $"Data Source={Path.Combine(dir, "autores.db")}";
        }

        return cs;
    }

    private SqliteConnection CreateConnection() => new(_connectionString);

    public async Task InitializeAsync()
    {
        const string sql = @"
            CREATE TABLE IF NOT EXISTS Autores (
                Idautor         INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombre          TEXT NOT NULL,
                Nacionalidad    TEXT NOT NULL,
                FechaNacimiento TEXT NOT NULL,
                Sueldo          REAL NOT NULL
            );";

        await using var connection = CreateConnection();
        await connection.ExecuteAsync(sql);
    }

    public async Task<int> CreateAsync(AutoresRecord autor)
    {
        const string sql = @"
            INSERT INTO Autores (Nombre, Nacionalidad, FechaNacimiento, Sueldo)
            VALUES (@Nombre, @Nacionalidad, @FechaNacimiento, @Sueldo);
            SELECT last_insert_rowid();";

        await using var connection = CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            Nombre = autor.Nombres,
            autor.Nacionalidad,
            FechaNacimiento = autor.FechaNacimiento.ToString("yyyy-MM-dd HH:mm:ss"),
            autor.Sueldo
        });
    }

    public async Task<bool> UpdateAsync(AutoresRecord autor)
    {
        const string sql = @"
            UPDATE Autores
            SET Nombre          = @Nombre,
                Nacionalidad    = @Nacionalidad,
                FechaNacimiento = @FechaNacimiento,
                Sueldo          = @Sueldo
            WHERE Idautor = @Idautor;";

        await using var connection = CreateConnection();
        var filasAfectadas = await connection.ExecuteAsync(sql, new
        {
            Idautor = autor.IdAutor,
            Nombre = autor.Nombres,
            autor.Nacionalidad,
            FechaNacimiento = autor.FechaNacimiento.ToString("yyyy-MM-dd HH:mm:ss"),
            autor.Sueldo
        });
        return filasAfectadas > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Autores WHERE Idautor = @Id;";

        await using var connection = CreateConnection();
        var filasAfectadas = await connection.ExecuteAsync(sql, new { Id = id });
        return filasAfectadas > 0;
    }

    public async Task<AutoresRecord?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT Idautor AS IdAutor, Nombre AS Nombres, Nacionalidad, FechaNacimiento, Sueldo
            FROM Autores
            WHERE Idautor = @Id;";

        await using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<AutoresRecord>(sql, new { Id = id });
    }

    public async Task<List<AutoresRecord>> GetListAsync()
    {
        const string sql = @"
            SELECT Idautor AS IdAutor, Nombre AS Nombres, Nacionalidad, FechaNacimiento, Sueldo
            FROM Autores
            ORDER BY Nombre;";

        await using var connection = CreateConnection();
        var result = await connection.QueryAsync<AutoresRecord>(sql);
        return result.ToList();
    }
}