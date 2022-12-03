using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stronk.Models;

[Table("tbl_Exercise_Muscle")]
public class ExerciseMuscle
{
    [Key]
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public int MuscleId { get; set; }
    public Exercise Exercise { get; set; }
    public Muscle Muscle { get; set; }
}