﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stronk.Models;

[Table("tbl_Post")]
public class Post
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string Message { get; set; }
    public DateTime date { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public ICollection<PostWorkout> PostWorkout { get; set; }
}