using Wilmert_P1_P4_Final.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<AutoresServices>();

var app = builder.Build();

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
