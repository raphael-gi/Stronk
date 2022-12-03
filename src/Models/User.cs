using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stronk.Models;

[Table("tbl_User")]
public class User
{
    [Key]
    public int Id { get; set; }
    [Required] [MaxLength(50)]
    public string Username { get; set; }
    [Required] [MaxLength(90)]
    public string Password { get; set; }
}