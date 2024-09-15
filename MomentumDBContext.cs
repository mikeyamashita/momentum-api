using Microsoft.EntityFrameworkCore;

class MomentumDBContext : DbContext
{

    public DbSet<GoalDoc> GoalDocs { get; set; }

    public MomentumDBContext(DbContextOptions<MomentumDBContext> options)
        : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GoalDoc>()
            .OwnsOne(c => c.Goal, d =>
            {
                d.ToJson();
                d.OwnsMany(d => d.Habits);
                d.OwnsMany(d => d.Milestones);
            });
    }
}