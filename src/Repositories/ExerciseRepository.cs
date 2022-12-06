using Microsoft.EntityFrameworkCore;
using Stronk.Models;
using Stronk.Data;

namespace Stronk.Repositories;

public class ExerciseRepository
{
    private readonly DatabaseContext _databaseContext;

    public ExerciseRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    public async Task<List<Exercise>> GetExercises(int userId)
    {
        return await _databaseContext.Exercises
            .Include(e => e.ExerciseMuscles)
            .ThenInclude(em => em.Muscle)
            .Where(e => e.UserId == userId)
            .ToListAsync();
    }
    public async Task<Exercise> GetExercise(int id)
    {
        return await _databaseContext.Exercises.FindAsync(id);
    }
    public async Task<Exercise> GetMusclesByExercise(int id)
    {
        return await _databaseContext.Exercises
            .Where(e => e.Id == id)
            .Include(e => e.ExerciseMuscles)
            .ThenInclude(em => em.Muscle)
            .FirstAsync();
    }
    public async Task<bool> CreateExercise(Exercise exercise, int[] muscles)
    {
        List<ExerciseMuscle> exerciseMuscles = new List<ExerciseMuscle>();
        foreach (int muscle in muscles)
        {
            exerciseMuscles.Add(new ExerciseMuscle()
            {
                MuscleId = muscle
            });
        }

        exercise.ExerciseMuscles = exerciseMuscles;
        await _databaseContext.Exercises.AddAsync(exercise);
        if (await _databaseContext.SaveChangesAsync() < 1)
        {
            return false;
        }
        return true;
    }
    public async Task<bool> EditExercise(Exercise exercise, int[] muscles)
    {
        Exercise oldExercise = await _databaseContext.Exercises.FindAsync(exercise.Id);
        List<ExerciseMuscle> exerciseMuscles = new List<ExerciseMuscle>();
        foreach (int muscle in muscles)
        {
            exerciseMuscles.Add(new ExerciseMuscle
            {
                MuscleId = muscle
            });
        }
        oldExercise.Name = exercise.Name;
        oldExercise.Description = exercise.Description;
        if (await _databaseContext.SaveChangesAsync() < 1)
        {
            return false;
        }
        
        return true;
    }
}