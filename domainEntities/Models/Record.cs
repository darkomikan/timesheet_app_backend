namespace timesheet.Models;

public class Record
{
    public int Id { get; set; }
    public Client Client { get; set; }
    public Project Project { get; set; }
    public Category Category { get; set; }
    public Employee Employee { get; set; }
    public DateTime DateCreated { get; set; }
    public string? Description { get; set; }
    public float Hours { get; set; }
    public float? Overtime { get; set; }
}
