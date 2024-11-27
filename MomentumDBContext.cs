using Microsoft.EntityFrameworkCore;

class MomentumDBContext : DbContext
{

    public required DbSet<GoalDoc> GoalDocs { get; set; }
    public required DbSet<HabitGridDoc> HabitGridDocs { get; set; }

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

        modelBuilder.Entity<HabitGridDoc>()
            .OwnsOne(c => c.HabitGrid, d =>
            {
                d.ToJson();
            });

    }
}