namespace domainEntities.Models;

public class Employee
{
    public enum EmployeeStatus
    {
        Inactive,
        Active
    }

    public enum EmployeeRole
    {
        Worker,
        Admin
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public float Hours { get; set; }
    public string Email { get; set; }
    public EmployeeStatus Status { get; set; }
    public EmployeeRole Role { get; set; }
}
