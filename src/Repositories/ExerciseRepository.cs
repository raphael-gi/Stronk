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

    public List<Exercise> GetExercises()
    {
        return _databaseContext.Exercises
            .Where(e => e.UserId == 0)
            .ToList();
    }

    public Exercise GetExercise(int id)
    {
        return _databaseContext.Exercises.Find(id);
    }

    public List<Muscle> GetSelectedMuscles(int id)
    {
        List<int> ids = _databaseContext.ExercisesMuscles.Where(em => em.ExerciseId == id).Select(em => em.MusclesId).ToList();
        return _databaseContext.Muscles.Where(m => ids.Contains(m.Id)).ToList();
    }

    public List<Exercise> GetExercisesByMuscle(int id)
    {
        return _databaseContext.Exercises.ToList();
    }
    public List<Muscle> GetMuscles()
    {
        return _databaseContext.Muscles.ToList();
    }
    public bool Add(Exercise exercise, string[] muscles)
    {
        List<int> results = _databaseContext.Exercises.Where(e => e.Name == exercise.Name && e.UserId == 0).Select(e => e.Id).ToList();
        if (results.Any())
        {
            return false;
        }
        _databaseContext.Exercises.Add(exercise);
        _databaseContext.SaveChanges();
        List<int> id = _databaseContext.Exercises.Where(e => e.Name == exercise.Name).Select(e => e.Id).ToList();
        foreach (var muscle in muscles)
        {
            ExerciseMuscle exerciseMuscle = new ExerciseMuscle
            {
                ExerciseId = id[0],
                MusclesId = int.Parse(muscle)
            };
            _databaseContext.ExercisesMuscles.Add(exerciseMuscle);
        }
        
        return _databaseContext.SaveChanges() > 0;
    }

    public bool Update(Exercise exercise, string[] muscles)
    {
        _databaseContext.Exercises.Update(exercise);
        return true;
    }
}