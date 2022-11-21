using System.ComponentModel.DataAnnotations;

namespace Stronk.Models;

public class Workout
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public int UserId { get; set; }
    
    public virtual ICollection<Exercise> Exercises { get; set; }
}