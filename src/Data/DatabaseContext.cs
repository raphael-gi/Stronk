using Microsoft.EntityFrameworkCore;
using Stronk.Models;

namespace Stronk.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<Muscle> Muscles { get; set; }
    public DbSet<Workout> Workouts { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<ExerciseMuscle> ExercisesMuscles { get; set; }
    public DbSet<WorkoutExercise> WorkoutsExercises { get; set; }
    public DbSet<PostWorkout> PostsWorkouts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<ExerciseMuscle>()
        //    .HasKey(em => new { em.ExerciseId, em.MuscleId });
        modelBuilder.Entity<ExerciseMuscle>()
            .HasOne(e => e.Exercise)
            .WithMany(em => em.ExerciseMuscles)
            .HasForeignKey(e => e.ExerciseId);
        modelBuilder.Entity<ExerciseMuscle>()
            .HasOne(m => m.Muscle)
            .WithMany(em => em.ExerciseMuscles)
            .HasForeignKey(m => m.MuscleId);

        //modelBuilder.Entity<WorkoutExercise>()
        //    .HasKey(we => new { we.WorkoutId, we.ExerciseId });
        modelBuilder.Entity<WorkoutExercise>()
            .HasOne(w => w.Workout)
            .WithMany(we => we.WorkoutExercises)
            .HasForeignKey(w => w.WorkoutId);
        modelBuilder.Entity<WorkoutExercise>()
            .HasOne(e => e.Exercise)
            .WithMany(we => we.WorkoutExercises)
            .HasForeignKey(e => e.ExerciseId);

        //modelBuilder.Entity<PostWorkout>()
        //    .HasKey(pw => new { pw.PostId, pw.WorkoutId });
        modelBuilder.Entity<PostWorkout>()
            .HasOne(p => p.Post)
            .WithMany(pw => pw.PostWorkout)
            .HasForeignKey(p => p.PostId);
        modelBuilder.Entity<PostWorkout>()
            .HasOne(w => w.Workout)
            .WithMany(pw => pw.PostWorkout)
            .HasForeignKey(w => w.WorkoutId);
    }
}