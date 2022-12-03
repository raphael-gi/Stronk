using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stronk.Models;

[Table("tbl_Muscle")]
public class Muscle
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }

    public ICollection<ExerciseMuscle> ExerciseMuscles { get; set; }
}