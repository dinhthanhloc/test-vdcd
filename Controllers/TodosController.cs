using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodosController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    // GET: /api/todos?isDone=true&page=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<TodoResponseDto>>> GetTodos(
        [FromQuery] bool? isDone,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _todoService.GetTodosAsync(isDone, page, pageSize);
        return Ok(result);
    }

    // GET: /api/todos/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TodoResponseDto>> GetById(int id)
    {
        var result = await _todoService.GetByIdAsync(id);
        if (result == null) return NotFound();

        return Ok(result);
    }

    // POST: /api/todos
    [HttpPost]
    public async Task<ActionResult<TodoResponseDto>> Create([FromBody] CreateTodoDto dto)
    {
        // ASP.NET Core tự động validate Model state dựa trên DataAnnotations ([Required], [MaxLength])
        // Trả về 400 Bad Request nếu không hợp lệ.

        var result = await _todoService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result); // Trả về 201 Created
    }

    // PUT: /api/todos/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoDto dto)
    {
        var updated = await _todoService.UpdateAsync(id, dto);
        if (!updated) return NotFound();

        return Ok(); // Hoặc NoContent() 204 tùy convention, ở đây trả về 200 OK với body cập nhật thành công
    }

    // DELETE: /api/todos/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _todoService.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent(); // Trả về 204 No Content
    }
}