using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stronk.Models;

[Table("tbl_Workout_Exercise")]
public class WorkoutExercise
{
    [Key]
    public int Id { get; set; }
    public int WorkoutId { get; set; }
    public int ExerciseId { get; set; }
    public Workout Workout { get; set; }
    public Exercise Exercise { get; set; }
}