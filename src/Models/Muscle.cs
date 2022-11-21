﻿using System.ComponentModel.DataAnnotations;

namespace Stronk.Models;

public class Muscle
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}