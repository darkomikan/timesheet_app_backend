using domainEntities.Exceptions;
using domainEntities.Models;
using repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class EmployeeService
    {
        private readonly IRepository<Employee> employeeRepo;
        private readonly AuthService authService;

        public EmployeeService(IRepository<Employee> employeeRepo, AuthService authService)
        {
            this.employeeRepo = employeeRepo;
            this.authService = authService;
        }

        public Employee[] GetEmployees(string pattern)
        {
            return employeeRepo.GetAll(pattern);
        }

        public Employee GetEmployeeById(int id)
        {
            return employeeRepo.Get(id);
        }

        public void InsertEmployee(Employee employee)
        {
            employee.Password = authService.GetHashAndSalt(employee.Password);
            employeeRepo.Insert(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            employeeRepo.Update(employee);
        }

        public void UpdateEmployeePassword(string username, string password)
        {
            ((EmployeeRepo)employeeRepo).UpdatePassword(username, authService.GetHashAndSalt(password));
        }

        public void DeleteEmployee(int id)
        {
            employeeRepo.Delete(id);
        }

        public string Login(string username, string password)
        {
            try
            {
                Employee emp = ((EmployeeRepo)employeeRepo).GetByUsername(username);
                if (authService.VerifyHash(password, emp.Password))
                    return authService.GenerateJSONWebToken(emp);
                else
                    throw new UnauthorizedException("Password is not correct.");
            }
            catch (NotFoundException)
            {
                throw new UnauthorizedException($"Username {username} does not exist.");
            }
        }
    }
}
