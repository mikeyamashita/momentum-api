using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://192.168.50.15:8100", "http://192.168.1.64:8100", "http://137.186.195.140:8100", "capacitor://localhost")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var Configuration = builder.Configuration;

// Db Connection
var isDevelopment = builder.Environment.IsDevelopment();
if (isDevelopment)
{
    builder.Services.AddDbContext<MomentumDBContext>(options =>
        options.UseNpgsql(Configuration.GetConnectionString("momentumDB")));
}
else
{
    builder.Services.AddDbContext<MomentumDBContext>(options =>
        options.UseNpgsql(Configuration.GetConnectionString("momentumDB")));
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "MomentumAPI";
    config.Title = "MomentumAPI v1";
    config.Version = "v1";
});

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine("Connection string is: " + connString);

var app = builder.Build();

// Swagger
// if (app.Environment.IsDevelopment())
// {
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.DocumentTitle = "MomentumAPI";
    config.Path = "/swagger";
    config.DocumentPath = "/swagger/{documentName}/swagger.json";
    config.DocExpansion = "list";
});
// }

// using (var scope = app.Services.CreateScope())
// {
//     var db = scope.ServiceProvider.GetRequiredService<MomentumDBContext>();
//     db.Database.Migrate(); // 👈 this runs pending migrations automatically
// }


// GoalsDoc Endpoints
app.MapGet("/api/goals", async (MomentumDBContext db) =>
    await db.GoalDocs.OrderBy(x => x.Id).ToListAsync());

app.MapGet("/api/goal/{id}", async (int id, MomentumDBContext db) =>
    await db.GoalDocs.FindAsync(id)
        is GoalDoc goal
            ? Results.Ok(goal)
            : Results.NotFound());

app.MapPost("/api/goal", async (GoalDoc goal, MomentumDBContext db) =>
{
    db.GoalDocs.Add(goal);
    await db.SaveChangesAsync();

    return Results.Created($"/api/goal/{goal.Id}", goal);
});

app.MapPut("/api/goal/{id}", async (int id, GoalDoc inputGoalDoc, MomentumDBContext db) =>
{
    var goaldoc = await db.GoalDocs.FindAsync(id);

    if (goaldoc is null) return Results.NotFound();

    goaldoc.Goal = inputGoalDoc.Goal;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/api/goal/{id}", async (int id, MomentumDBContext db) =>
{
    if (await db.GoalDocs.FindAsync(id) is GoalDoc goal)
    {
        db.GoalDocs.Remove(goal);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

// HabitsGridDoc Endpoints
app.MapGet("/api/habitgrid", async (MomentumDBContext db) =>
    await db.HabitGridDocs.ToListAsync());

// app.MapGet("/api/habitgrid/{date}", async (string date, MomentumDBContext db) =>
//     await db.HabitGridDocs.FindAsync(date)
//         is HabitGridDoc habitgrid
//             ? Results.Ok(habitgrid)
//             : Results.NotFound());

// app.MapGet("/api/habitgrid/{id}", async (int id, MomentumDBContext db) =>
//     await db.HabitGridDocs.FindAsync(id)
//         is HabitGridDoc habitgrid
//             ? Results.Ok(habitgrid)
//             : Results.NotFound());

app.MapPost("/api/habitgrid", async (HabitGridDoc habitgrid, MomentumDBContext db) =>
{
    db.HabitGridDocs.Add(habitgrid);
    await db.SaveChangesAsync();

    return Results.Created($"/api/habitgrid/{habitgrid.Id}", habitgrid);
});

app.MapPut("/api/habitgrid/{id}", async (int id, HabitGridDoc inputHabitGridDoc, MomentumDBContext db) =>
{
    var habitgrid = await db.HabitGridDocs.FindAsync(id);

    if (habitgrid is null) return Results.NotFound();

    habitgrid.HabitGrid = inputHabitGridDoc.HabitGrid;
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/api/habitgrid/{id}", async (int id, MomentumDBContext db) =>
{
    if (await db.HabitGridDocs.FindAsync(id) is HabitGridDoc habitgrid)
    {
        db.HabitGridDocs.Remove(habitgrid);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.UseCors();

app.Run();