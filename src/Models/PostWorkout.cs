using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stronk.Models;

[Table("tbl_Post_Workout")]
public class PostWorkout
{
    [Key]
    public int Id { get; set; }
    public int PostId { get; set; }
    public int WorkoutId { get; set; }
    public Post Post { get; set; }
    public Workout Workout { get; set; }
}