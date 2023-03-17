using domainEntities.Models;
using repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class EmployeeService
    {
        private const int keySize = 64;
        private const int iterations = 350000;
        private readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        private readonly IRepository<Employee> employeeRepo;

        public EmployeeService(IRepository<Employee> employeeRepo)
        {
            this.employeeRepo = employeeRepo;
        }

        public Employee[] GetEmployees()
        {
            return employeeRepo.GetAll();
        }

        public Employee? GetEmployeeById(int id)
        {
            return employeeRepo.Get(id);
        }

        public void InsertEmployee(Employee employee)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(employee.Password), salt, iterations, hashAlgorithm, keySize);
            employee.Password = Convert.ToHexString(hash) + Convert.ToHexString(salt);
            employeeRepo.Insert(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            employeeRepo.Update(employee);
        }

        public void DeleteEmployee(int id)
        {
            employeeRepo.Delete(id);
        }

        public bool VerifyPassword(string username, string password)
        {
            Employee? emp = ((EmployeeRepo)employeeRepo).GetByUsername(username);
            if (emp != null)
            {
                var newHash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password),
                    Convert.FromHexString(emp.Password.Substring(128)), iterations, hashAlgorithm, keySize);
                return newHash.SequenceEqual(Convert.FromHexString(emp.Password.Substring(0, 128)));
            }
            return false;
        }
    }
}
