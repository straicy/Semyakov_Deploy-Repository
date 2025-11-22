using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models;
using MyWebApi.Services;

namespace MyWebApi.Controllers;

[ApiController]
[Route("api/cars")]
public class CarsController : ControllerBase
{
    private readonly ICarService _service;

    public CarsController(ICarService service) => _service = service;

    [HttpGet] 
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")] 
    public async Task<IActionResult> GetById(string id) => await _service.GetByIdAsync(id) is Car car ? Ok(car) : NotFound();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Car car)
    {
        // 1. ПЕРЕВІРКА: Валідація моделі (виправляє помилку 400 коли Price негативний)
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        await _service.CreateAsync(car);
        return CreatedAtAction(nameof(GetById), new { id = car.Id }, car);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Car car)
    {
        // 1. ПЕРЕВІРКА: Валідація моделі (виправляє помилку 400 коли Year недійсний)
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // 2. ПЕРЕВІРКА: Чи існує автомобіль (виправляє помилку 404)
        if (await _service.GetByIdAsync(id) == null)
        {
            return NotFound(); 
        }

        car.Id = id;
        await _service.UpdateAsync(car);
        // Згідно з тестами, якщо об'єкт знайдено і оновлено, має бути 204
        return NoContent(); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        // 1. ПЕРЕВІРКА: Чи існує автомобіль (виправляє помилку 404)
        if (await _service.GetByIdAsync(id) == null)
        {
            return NotFound(); 
        }

        await _service.DeleteAsync(id);
        // Згідно з HTTP-стандартом та тестами, 204 є коректним для успішного видалення
        return NoContent();
    }
}