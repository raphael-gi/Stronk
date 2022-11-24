using System.ComponentModel.DataAnnotations;

namespace Stronk.Models;

public class Exercise
{
    [Key]
    public int Id { get; set; }
    [Required] public string? Name { get; set; }
    public string Description { get; set; }
    public List<Muscle> Muscles { get; set; }
}