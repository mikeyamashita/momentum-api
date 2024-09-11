using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Db Connection
// var isDevelopment = builder.Environment.IsDevelopment();
// if (isDevelopment)
// {


builder.Services.AddDbContext<MomentumDBContext>(options =>
{
    options.UseNpgsql(Configuration.GetConnectionString("momentumDB"));

    // var dataSourceBuilder = new NpgsqlDataSourceBuilder(Configuration.GetConnectionString("momentumDB"));
    // dataSourceBuilder.EnableDynamicJson();
    // options.UseNpgsql(dataSourceBuilder.Build());
});

// }
// else
// {
//     builder.Services.AddDbContext<MomentumDBContext>(options =>
//         options.UseNpgsql(Configuration.GetConnectionString("momentumDBProd")));
// }

// builder.Services.AddDbContext<MomentumDBContext>(opt => opt.UseInMemoryDatabase("MomentumDb"));

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


app.MapGet("/goal", async (MomentumDBContext db) =>
    await db.GoalDocs.ToListAsync());

app.MapGet("/goal/{id}", async (int id, MomentumDBContext db) =>
    await db.GoalDocs.FindAsync(id)
        is GoalDoc goal
            ? Results.Ok(goal)
            : Results.NotFound());

app.MapPost("/goal", async (GoalDoc goal, MomentumDBContext db) =>
{
    db.GoalDocs.Add(goal);
    await db.SaveChangesAsync();

    return Results.Created($"/goal/{goal.Id}", goal);
});

app.MapPut("/goal/{id}", async (int id, GoalDoc inputGoalDoc, MomentumDBContext db) =>
{
    var goal = await db.GoalDocs.FindAsync(id);

    if (goal is null) return Results.NotFound();

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/goal/{id}", async (int id, MomentumDBContext db) =>
{
    if (await db.GoalDocs.FindAsync(id) is GoalDoc goal)
    {
        db.GoalDocs.Remove(goal);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();