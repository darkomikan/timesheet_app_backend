using domainEntities.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin,Worker")]
        [HttpGet]
        public Employee Get(int id)
        {
            return employeeService.GetEmployeeById(id);
        }

        [Authorize(Roles = "Admin,Worker")]
        [HttpGet]
        public Employee[] GetAll()
        {
            return employeeService.GetEmployees();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            string token = employeeService.VerifyPassword(username, password);
            if (token != string.Empty)
                return Ok(token);
            else
                return Unauthorized();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void Add(Employee employee)
        {
            employeeService.InsertEmployee(employee);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public void Update(Employee employee)
        {
            employeeService.UpdateEmployee(employee);
        }

        [Authorize(Roles = "Admin,Worker")]
        [HttpPut]
        public void ChangePassword(string username, string password)
        {
            employeeService.UpdateEmployeePassword(username, password);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public void Delete(int id)
        {
            employeeService.DeleteEmployee(id);
        }
    }
}
