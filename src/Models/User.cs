using System.ComponentModel.DataAnnotations;

namespace Stronk.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required] [MaxLength(50)]
    public string Username { get; set; }
    [Required] [MaxLength(90)]
    public string Password { get; set; }
}