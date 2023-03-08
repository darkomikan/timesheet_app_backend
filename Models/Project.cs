namespace timesheet.Models;

public class Project
{
    public enum ProjectStatus
    {
        Inactive,
        Active,
        Archive
    }

    public int Project_id { get; set; }
    public int Client_id { get; set; }
    public int Lead_id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ProjectStatus Status { get; set; }
}
