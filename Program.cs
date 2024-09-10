using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MomentumDB>(opt => opt.UseInMemoryDatabase("HabitList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "MomentumAPI";
    config.Title = "MomentumAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "MomentumAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/habititems", async (MomentumDB db) =>
    await db.Habits.ToListAsync());

app.MapGet("/habititems/complete", async (MomentumDB db) =>
    await db.Habits.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/habititems/{id}", async (int id, MomentumDB db) =>
    await db.Habits.FindAsync(id)
        is Habit habit
            ? Results.Ok(habit)
            : Results.NotFound());

app.MapPost("/habititems", async (Habit habit, MomentumDB db) =>
{
    db.Habits.Add(habit);
    await db.SaveChangesAsync();

    return Results.Created($"/habititems/{habit.Id}", habit);
});

app.MapPut("/habititems/{id}", async (int id, Habit inputHabit, MomentumDB db) =>
{
    var habit = await db.Habits.FindAsync(id);

    if (habit is null) return Results.NotFound();

    habit.Name = inputHabit.Name;
    habit.IsComplete = inputHabit.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/habititems/{id}", async (int id, MomentumDB db) =>
{
    if (await db.Habits.FindAsync(id) is Habit habit)
    {
        db.Habits.Remove(habit);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();