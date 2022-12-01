using Microsoft.EntityFrameworkCore;
using Stronk.Models;

namespace Stronk.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<Muscle> Muscles { get; set; }
    public DbSet<ExerciseMuscle> ExercisesMuscles { get; set; }
    public DbSet<Workout> Workouts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Exercise_Muscle>()
        //    .HasKey(em => new { em.ExerciseId, em.MusclesId });
        //
        //modelBuilder.Entity<Exercise_Muscle>()
        //    .HasOne(e => e.Exercise)
        //    .WithMany(em => em.ExerciseMuscles)
        //    .HasForeignKey(e => e.ExerciseId);
        //modelBuilder.Entity<Exercise_Muscle>()
        //    .HasOne(m => m.Muscle)
        //    .WithMany(em => em.ExerciseMuscles)
        //    .HasForeignKey(m => m.MusclesId);
    }
}