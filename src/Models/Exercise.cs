using System.ComponentModel.DataAnnotations;

namespace Stronk.Models;

public class Exercise
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Description { get; set; }
    public Muscle Muscle { get; set; }
    public User User { get; set; }
    
    public virtual ICollection<Muscle> Muscles { get; set; }
}