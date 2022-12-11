using Microsoft.EntityFrameworkCore;
using Stronk.Data;
using Stronk.Models;

namespace Stronk.Repositories;

public class PostRepository
{
    private readonly DatabaseContext _databaseContext;

    public PostRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    public async Task<List<Post>> GetPosts()
    {
        return await _databaseContext.Posts
            .OrderByDescending(p => p.Date)
            .ThenByDescending(p => p.Id)
            .Include(p => p.User)
            .Include(p => p.PostWorkout)
            .ThenInclude(pw => pw.Workout)
            .ThenInclude(w => w.WorkoutExercises)
            .ThenInclude(we => we.Exercise)
            .ToListAsync();
    }
    public async Task<List<Workout>> GetWorkouts(int userId)
    {
        return await _databaseContext.Workouts
            .Where(w => w.UserId == userId)
            .ToListAsync();
    }
    public async Task<int> GetPostsAmount(int userId)
    {
        return await _databaseContext.Posts.CountAsync(p => p.UserId == userId);
    }
    public async Task<bool> CreatePost(Post post, int[] workouts)
    {
        post.Date = DateTime.Now;
        List<PostWorkout> postWorkouts = new List<PostWorkout>();
        foreach (int workout in workouts)
        {
            postWorkouts.Add(new PostWorkout
            {
                WorkoutId = workout
            });
        }
        post.PostWorkout = postWorkouts;
        await _databaseContext.Posts.AddAsync(post);
        return await _databaseContext.SaveChangesAsync() > 0;
    }
    public async Task<bool> DeletePost(int id)
    {
        Post post = await _databaseContext.Posts
            .Include(p => p.PostWorkout)
            .FirstAsync(p => p.Id == id);
        _databaseContext.Posts.Remove(post);
        return await _databaseContext.SaveChangesAsync() > 0;
    }
    public async Task<bool> Copy(int id, int userId)
    {
        Post post = await _databaseContext.Posts
            .Where(p => p.Id == id)
            .Include(p => p.PostWorkout)
            .ThenInclude(pw => pw.Workout)
            .ThenInclude(w => w.WorkoutExercises)
            .ThenInclude(we => we.Exercise)
            .ThenInclude(e => e.ExerciseMuscles)
            .FirstAsync();
        List<Workout> workouts = new List<Workout>();
        foreach (PostWorkout postWorkout in post.PostWorkout)
        {
            workouts.Add(new Workout
            {
                Name = postWorkout.Workout.Name,
                UserId = userId,
                WorkoutExercises = postWorkout.Workout.WorkoutExercises
            });
        }
        foreach (var variable in workouts)
        {
            Console.WriteLine(variable.Name);
            foreach (var test in variable.WorkoutExercises)
            {
                Console.WriteLine(test.Exercise.Name);
            }
        }
        await _databaseContext.Workouts.AddRangeAsync(workouts);
        
        return await _databaseContext.SaveChangesAsync() > 0;
    }
}