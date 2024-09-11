public class Goal
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<Habit>? Habits { get; set; }
    public bool IsComplete { get; set; }

}