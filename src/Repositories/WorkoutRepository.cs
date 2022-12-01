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
    public List<Workout> Workouts()
    {
        return _databaseContext.Workouts.ToList();
    }
}