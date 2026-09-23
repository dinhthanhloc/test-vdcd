using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs;

public class CreateTodoDto
{
    [Required(ErrorMessage = "Title là bắt buộc.")]
    public string Title { get; set; } = string.Empty;
}

public class UpdateTodoDto
{
    [Required(ErrorMessage = "Title là bắt buộc.")]
    [MaxLength(200, ErrorMessage = "Title tối đa 200 ký tự.")]
    public string Title { get; set; } = string.Empty;

    public bool IsDone { get; set; }
}

public class TodoResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsDone { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PagedResultDto<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}