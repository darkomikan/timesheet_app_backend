namespace domainEntities.Models;

public class Project
{
    public enum ProjectStatus
    {
        Inactive,
        Active,
        Archive
    }

    public int Id { get; set; }
    public Client Client { get; set; }
    public Employee Lead { get; set; }
    public List<Employee> Employees { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ProjectStatus Status { get; set; }
    public DateTime? DeletedAt { get; set; }
}
