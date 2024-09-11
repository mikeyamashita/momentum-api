using Microsoft.EntityFrameworkCore;

class MomentumDBContext : DbContext
{

    public DbSet<GoalDoc> GoalDocs { get; set; }

    public MomentumDBContext(DbContextOptions<MomentumDBContext> options)
        : base(options) { }
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<GoalDoc>()
    //         .Property(b => b.Goal)
    //         .HasColumnType("json");
    // }

    // private static readonly JsonSerializerOptions defaultOptions
    //     = new JsonSerializerOptions
    //     {
    //         DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    //         PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    //     };

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<GoalDoc>()
    //     .Property(e => e.Goal)
    //     .HasColumnType("json");
    //     // .UseJsonSerializerOptions(new JsonSerializerOptions
    //     // {
    //     //     DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    //     //     PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    //     // });
    // }
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     var dataSourceBuilder = new NpgsqlDataSourceBuilder(Configuration.GetConnectionString("momentumDB"));
    //     dataSourceBuilder.EnableDynamicJson();
    //     options.UseNpgsql(dataSourceBuilder.Build());

    //     optionsBuilder
    //         .UseNpgsql(dataSourceBuilder)
    //         .UseNpgsqlJsonSerializerOptions();
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GoalDoc>()
            .OwnsOne(c => c.Goal, d =>
            {
                d.ToJson();
                d.OwnsMany(d => d.Habits);
            });
    }
}