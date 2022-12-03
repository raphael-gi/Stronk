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

    public List<Exercise> Exercises()
    {
        return _databaseContext.Exercises.ToList();
    }
    public async Task<List<Workout>> GetWorkouts()
    {
        return await _databaseContext.Workouts.Select(w => new Workout
        {
            Id = w.Id,
            Name = w.Name,
            WorkoutExercises = _databaseContext.WorkoutsExercises.Where(we => we.WorkoutId == w.Id).Select(we => new WorkoutExercise
            {
                WorkoutId = we.WorkoutId,
                ExerciseId = we.ExerciseId,
                Exercise = _databaseContext.Exercises.First(e => e.Id == we.ExerciseId)
            }).ToList()
        }).ToListAsync();
    }
    public async Task<bool> CreateWorkout(Workout workout, int[] exercises)
    {
        await _databaseContext.Workouts.AddAsync(workout);
        if (await _databaseContext.SaveChangesAsync() < 1)
        {
            return false;
        }
        Console.WriteLine(workout.Id);
        List<WorkoutExercise> workoutExercises = new List<WorkoutExercise>();
        foreach (int exercise in exercises)
        {
            workoutExercises.Add(new WorkoutExercise()
            {
                WorkoutId = workout.Id,
                ExerciseId = exercise
            });
        }
        await _databaseContext.WorkoutsExercises.AddRangeAsync(workoutExercises);
        if (await _databaseContext.SaveChangesAsync() < 1)
        {
            return false;
        }
        return true;
    }
}