using TodoApi.DTOs;

namespace TodoApi.Services;

public interface ITodoService
{
    Task<PagedResultDto<TodoResponseDto>> GetTodosAsync(bool? isDone, int page, int pageSize);
    Task<TodoResponseDto?> GetByIdAsync(int id);
    Task<TodoResponseDto> CreateAsync(CreateTodoDto dto);
    Task<bool> UpdateAsync(int id, UpdateTodoDto dto);
    Task<bool> DeleteAsync(int id);
}