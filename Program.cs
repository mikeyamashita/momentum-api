using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "http://192.168.50.173:4200", "http://192.168.50.142:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

var Configuration = builder.Configuration;

// Db Connection
builder.Services.AddDbContext<MomentumDBContext>(options =>
{
    options.UseNpgsql(Configuration.GetConnectionString("momentumDB"));
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "MomentumAPI";
    config.Title = "MomentumAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

// Swagger
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

// endpoints
app.MapGet("/api/goals", async (MomentumDBContext db) =>
    await db.GoalDocs.ToListAsync());

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
    var goal = await db.GoalDocs.FindAsync(id);

    if (goal is null) return Results.NotFound();

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

app.UseCors();

app.Run();