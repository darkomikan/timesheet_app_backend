using domainEntities.Exceptions;
using domainEntities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;
using System;

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
            try
            {
                string token = employeeService.Login(username, password);
                return Ok(token);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
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
