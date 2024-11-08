public class Goal
{
    public string? Name { get; set; }
    public string? Description { get; set; }
<<<<<<< HEAD
    public DateTime? StartDate { get; set;}
    public DateTime? EndDate { get; set;}
=======
    public DateTime? Startdate { get; set; }
    public DateTime? Enddate { get; set; }
>>>>>>> 49d8af9651387045d54a1640cb897a18be5da4fd
    public List<Habit>? Habits { get; set; }
    public List<Milestone>? Milestones { get; set; }
    public bool IsComplete { get; set; }

}