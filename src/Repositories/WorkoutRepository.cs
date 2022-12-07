using Microsoft.EntityFrameworkCore;
using Stronk.Models;

namespace Stronk.Data;

public class WorkoutRepository
{
    private readonly DatabaseContext _databaseContext;

    public WorkoutRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    public async Task<List<int>> GetSelectedExercises(int id)
    {
        return await _databaseContext.WorkoutsExercises.Where(we => we.WorkoutId == id).Select(we => we.ExerciseId).ToListAsync();
    }
    public async Task<List<Workout>> GetWorkouts()
    {
        return await _databaseContext.Workouts
            .Include(w => w.WorkoutExercises)
            .ThenInclude(we => we.Exercise)
            .ToListAsync();
    }
    public async Task<Workout> GetWorkout(int id)
    {
        return await _databaseContext.Workouts.FindAsync(id);
    }
    public async Task<bool> CreateWorkout(Workout workout, int[] exercises)
    {
        List<WorkoutExercise> workoutExercises = new List<WorkoutExercise>();
        foreach (int exercise in exercises)
        {
            workoutExercises.Add(new WorkoutExercise
            {
                ExerciseId = exercise
            });
        }
        workout.WorkoutExercises = workoutExercises;
        await _databaseContext.Workouts.AddAsync(workout);
        return await _databaseContext.SaveChangesAsync() > 0;
    }
    public async Task<bool> EditWorkout(Workout workout, int[] exercises)
    {
        Workout oldWorkout = await _databaseContext.Workouts
            .Where(w => w.Id == workout.Id)
            .Include(w => w.WorkoutExercises)
            .FirstAsync();
        List<WorkoutExercise> workoutExercises = new List<WorkoutExercise>();
        foreach (int exercise in exercises)
        {
            workoutExercises.Add(new WorkoutExercise
            {
                WorkoutId = workout.Id,
                ExerciseId = exercise
            });
        }
        oldWorkout.Name = workout.Name;
        oldWorkout.WorkoutExercises = workoutExercises;
        return await _databaseContext.SaveChangesAsync() > 0;
    }
    public async Task<bool> DeleteWorkout(int id)
    {
        Workout workout = await _databaseContext.Workouts
            .Include(w => w.WorkoutExercises)
            .FirstAsync(e => e.Id == id);
        _databaseContext.Workouts.Remove(workout);
        return await _databaseContext.SaveChangesAsync() > 0;
    }
}