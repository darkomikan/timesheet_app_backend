namespace timesheet.Models;

public class Record
{
    public int Record_id { get; set; }
    public int Client_id { get; set; }
    public int Project_id { get; set; }
    public int Category_id { get; set; }
    public int Employee_id { get; set; }
    public DateTime Date_created { get; set; }
    public string? Description { get; set; }
    public float Hours { get; set; }
    public float? Overtime { get; set; }
}
