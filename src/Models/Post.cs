using System.ComponentModel.DataAnnotations;

namespace Stronk.Models;

public class Post
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string Message { get; set; }
    public DateTime date { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
}