using domainEntities.Models;
using Microsoft.AspNetCore.Mvc;
using service;

namespace timesheet.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeService employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet]
        public Employee Get(int id)
        {
            return employeeService.GetEmployeeById(id);
        }

        [HttpGet]
        public Employee[] GetAll()
        {
            return employeeService.GetEmployees();
        }

        [HttpGet]
        public bool Login(string username, string password)
        {
            return employeeService.VerifyPassword(username, password);
        }

        [HttpPost]
        public void Add(Employee employee)
        {
            employeeService.InsertEmployee(employee);
        }

        [HttpPut]
        public void Update(Employee employee)
        {
            employeeService.UpdateEmployee(employee);
        }

        [HttpPut]
        public void ChangePassword(string username, string password)
        {
            employeeService.UpdateEmployeePassword(username, password);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            employeeService.DeleteEmployee(id);
        }
    }
}
