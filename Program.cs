using Wilmert_P1_P4_Final.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<AutoresServices>();

var app = builder.Build();

// TEMPORAL: muestra el mensaje real del error en la respuesta (borrar al terminar)
app.Use(async (ctx, next) =>
{
    try { await next(); }
    catch (Exception ex)
    {
        ctx.Response.StatusCode = 500;
        await ctx.Response.WriteAsync(ex.ToString());
    }
});

using (var scope = app.Services.CreateScope())
{
    var autoresService = scope.ServiceProvider.GetRequiredService<AutoresServices>();
    await autoresService.InitializeAsync();
}

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


