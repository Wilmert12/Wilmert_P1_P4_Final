using Microsoft.AspNetCore.Mvc;
using Wilmert_P1_P4_Final.Models;
using Wilmert_P1_P4_Final.Services;

namespace Wilmert_P1_P4_Final.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
	private readonly AutoresServices _autoresService;

	public AutoresController(AutoresServices autoresService)
	{
		_autoresService = autoresService;
	}

	[HttpGet]
	public async Task<IActionResult> ObtenerAutores()
	{
		return Ok(await _autoresService.GetListAsync());
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> ObtenerAutor(int id)
	{
		var autor = await _autoresService.GetByIdAsync(id);
		return autor is null ? NotFound() : Ok(autor);
	}

	[HttpPost]
	public async Task<IActionResult> CrearAutor([FromBody] AutoresRecord autor)
	{
		if (autor is null)
			return BadRequest();

		await _autoresService.CreateAsync(autor);
		return Ok();
	}

	[HttpPut("{id:int}")]
	public async Task<IActionResult> ActualizarAutor(int id, [FromBody] AutoresRecord autor)
	{
		if (autor is null)
			return BadRequest();

		var autorActualizado = autor with { Idautor = id };
		var actualizado = await _autoresService.UpdateAsync(autorActualizado);
		return actualizado ? NoContent() : NotFound();
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> EliminarAutor(int id)
	{
		var eliminado = await _autoresService.DeleteAsync(id);
		return eliminado ? NoContent() : NotFound();
	}
}
