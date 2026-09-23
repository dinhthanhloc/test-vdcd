using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Entities;

namespace TodoApi.Services;

public class TodoService : ITodoService
{
    private readonly AppDbContext _context;

    public TodoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResultDto<TodoResponseDto>> GetTodosAsync(bool? isDone, int page, int pageSize)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var query = _context.Todos.AsNoTracking();

        if (isDone.HasValue)
        {
            query = query.Where(t => t.IsDone == isDone.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TodoResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                IsDone = t.IsDone,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();

        return new PagedResultDto<TodoResponseDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<TodoResponseDto?> GetByIdAsync(int id)
    {
        var todo = await _context.Todos.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (todo == null) return null;

        return new TodoResponseDto
        {
            Id = todo.Id,
            Title = todo.Title,
            IsDone = todo.IsDone,
            CreatedAt = todo.CreatedAt
        };
    }

    public async Task<TodoResponseDto> CreateAsync(CreateTodoDto dto)
    {
        var entity = new TodoItem
        {
            Title = dto.Title,
            IsDone = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Todos.Add(entity);
        await _context.SaveChangesAsync();

        return new TodoResponseDto
        {
            Id = entity.Id,
            Title = entity.Title,
            IsDone = entity.IsDone,
            CreatedAt = entity.CreatedAt
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateTodoDto dto)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null) return false;

        todo.Title = dto.Title;
        todo.IsDone = dto.IsDone;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null) return false;

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
        return true;
    }
}