using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stronk.Models;

[Table("tbl_Exercise")]
public class Exercise
{
    [Key]
    public int Id { get; set; }
    [Required] [MaxLength(50)]
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<ExerciseMuscle> ExerciseMuscles { get; set; }
    public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
}