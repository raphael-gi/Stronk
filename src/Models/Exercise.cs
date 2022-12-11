using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stronk.Models;

[Table("tbl_Exercise")]
public class Exercise
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public ICollection<ExerciseMuscle> ExerciseMuscles { get; set; }
    public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
}