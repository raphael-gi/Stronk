using System.ComponentModel.DataAnnotations;

namespace Stronk.Models;

public class ExerciseMuscle
{
    [Key]
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public int MusclesId { get; set; }
}