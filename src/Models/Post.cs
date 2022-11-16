using System.ComponentModel.DataAnnotations;

namespace Stronk.Models;

public class Post
{
    public int Id { get; set; }
    [Required] public string? Title { get; set; }
    public string Message { get; set; }
    [Required] public DateTime date { get; set; }
}