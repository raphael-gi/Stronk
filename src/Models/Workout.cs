﻿using System.ComponentModel.DataAnnotations;

namespace Stronk.Models;

public class Workout
{
    [Key]
    public int Id { get; set; }
    [Required] [MaxLength(50)]
    public string Name { get; set; }
}