using Microsoft.AspNetCore.Mvc;
using Parcial1_P4_Wilmert.Models;
using Parcial1_P4_Wilmert.Services;

namespace Parcial1_P4_Wilmert.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private readonly AutoresService _autoresService;

    public AutoresController(AutoresService autoresService)
    {
        _autoresService = autoresService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Autor), StatusCodes.Status201Created)]
    public async Task<IActionResult> Crear([FromBody] Autor autor)
    {
        var guardado = await _autoresService.SaveAsync(autor);
        return CreatedAtAction(nameof(GetById), new { id = guardado.Id }, guardado);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Autor), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(int id)
    {
        var autor = await _autoresService.GetByIdAsync(id);
        return autor is null ? NotFound() : Ok(autor);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Autor>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetList()
    {
        var autores = await _autoresService.GetListAsync();
        return Ok(autores);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] Autor autor)
    {
        var existente = await _autoresService.GetByIdAsync(id);
        if (existente is null)
            return NotFound($"No existe el autor con Id {id}");

        autor.Id = id;
        await _autoresService.UpdateAsync(autor);
        return Ok(autor);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var existente = await _autoresService.GetByIdAsync(id);
        if (existente is null)
            return NotFound($"No existe el autor con Id {id}");

        await _autoresService.DeleteAsync(id);
        return NoContent();
    }
}