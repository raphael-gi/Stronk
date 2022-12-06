using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stronk.Models;

[Table("tbl_Workout")]
public class Workout
{
    [Key]
    public int Id { get; set; }
    [Required] [MaxLength(50)]
    public string Name { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
    public ICollection<PostWorkout> PostWorkout { get; set; }
}