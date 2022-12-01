using System.ComponentModel.DataAnnotations;

namespace Stronk.Models;

public class Exercise
{
    [Key]
    public int Id { get; set; }
    [Required] [MaxLength(50)]
    public string Name { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public IEnumerable<ExerciseMuscle>? ExerciseMuscles { get; set; }
}