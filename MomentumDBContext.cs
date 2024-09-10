using Microsoft.EntityFrameworkCore;

class MomentumDB : DbContext
{
    public MomentumDB(DbContextOptions<MomentumDB> options)
        : base(options) { }

    public DbSet<Habit> Habits => Set<Habit>();
}