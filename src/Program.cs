using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Stronk.Data;
using Stronk.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(c =>
    {
        c.LoginPath = "/Login";
        c.AccessDeniedPath = "/Login/Denied";
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<MuscleRepository>();
builder.Services.AddScoped<ExerciseRepository>();
builder.Services.AddScoped<WorkoutRepository>();
builder.Services.AddScoped<PostRepository>();
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();