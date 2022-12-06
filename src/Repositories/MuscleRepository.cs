using Microsoft.EntityFrameworkCore;
using Stronk.Data;
using Stronk.Models;

namespace Stronk.Repositories;

public class MuscleRepository
{
    private readonly DatabaseContext _databaseContext;

    public MuscleRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    public async Task<List<Muscle>> GetMuscles()
    {
        return await _databaseContext.Muscles.ToListAsync();
    }
    public async Task<List<int>> GetSelectedMuscles(int id)
    {
        return await _databaseContext.ExercisesMuscles
            .Where(em => em.ExerciseId == id)
            .Select(em => em.MuscleId)
            .ToListAsync();
    }
}