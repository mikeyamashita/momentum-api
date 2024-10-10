public class Goal
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<Habit>? Habits { get; set; }
    public List<Milestone>? Milestones { get; set; }
    public bool IsComplete { get; set; }

}