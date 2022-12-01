using System.ComponentModel.DataAnnotations;

namespace Stronk.Models;

public class Muscle
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }

    public ICollection<ExerciseMuscle> ExerciseMuscles;
}