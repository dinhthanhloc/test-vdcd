using System.ComponentModel.DataAnnotations;

namespace TodoApi.Entities;

public class TodoItem
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public bool IsDone { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}