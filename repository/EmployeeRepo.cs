using domainEntities.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repository
{
    public class EmployeeRepo : IRepository<Employee>
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public EmployeeRepo(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration.GetConnectionString("timesheet_db")!;
        }

        public void Delete(int id)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE employees
                SET deleted_at = @date
                WHERE employee_id = @id
            ";
            command.Parameters.AddWithValue("@date", DateTime.Now);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public Employee? Get(int id)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT *
                FROM employees
                WHERE employee_id = @id AND deleted_at IS NULL
            ";
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
                return new Employee
                {
                    Id = reader.GetInt32("employee_id"),
                    Name = reader.GetString("name"),
                    Username = reader.GetString("username"),
                    Password = reader.GetString("password"),
                    Hours = reader.GetFloat("hours"),
                    Email = reader.GetString("email"),
                    Status = (Employee.EmployeeStatus)reader.GetInt32("status"),
                    Role = (Employee.EmployeeRole)reader.GetInt32("role")
                };
            else
                return null;
        }

        public Employee[] GetAll()
        {
            List<Employee> result = new List<Employee>();
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT * FROM employees
                WHERE deleted_at IS NULL
            ";
            using var reader = command.ExecuteReader();
            while (reader.Read())
                result.Add(new Employee
                {
                    Id = reader.GetInt32("employee_id"),
                    Name = reader.GetString("name"),
                    Username = reader.GetString("username"),
                    Password = reader.GetString("password"),
                    Hours = reader.GetFloat("hours"),
                    Email = reader.GetString("email"),
                    Status = (Employee.EmployeeStatus)reader.GetInt32("status"),
                    Role = (Employee.EmployeeRole)reader.GetInt32("role")
                });
            return result.ToArray();
        }

        public void Insert(Employee item)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO employees (name, username, password, hours, email, status, role)
                VALUES (@name, @username, @password, @hours, @email, @status, @role)
            ";
            command.Parameters.AddWithValue("@name", item.Name);
            command.Parameters.AddWithValue("@username", item.Username);
            command.Parameters.AddWithValue("@password", item.Password);
            command.Parameters.AddWithValue("@hours", item.Hours);
            command.Parameters.AddWithValue("@email", item.Email);
            command.Parameters.AddWithValue("@status", (int)item.Status);
            command.Parameters.AddWithValue("@role", (int)item.Role);
            command.ExecuteNonQuery();
        }

        public void Update(Employee item)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE employees
                SET name = @name, username = @username, password = @password, hours = @hours,
                email = @email, status = @status, role = @role
                WHERE employee_id = @id
            ";
            command.Parameters.AddWithValue("@name", item.Name);
            command.Parameters.AddWithValue("@username", item.Username);
            command.Parameters.AddWithValue("@password", item.Password);
            command.Parameters.AddWithValue("@hours", item.Hours);
            command.Parameters.AddWithValue("@email", item.Email);
            command.Parameters.AddWithValue("@status", (int)item.Status);
            command.Parameters.AddWithValue("@role", (int)item.Role);
            command.Parameters.AddWithValue("@id", item.Id);
            command.ExecuteNonQuery();
        }

        public Employee? GetByUsername(string username)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT *
                FROM employees
                WHERE username = @username AND deleted_at IS NULL
            ";
            command.Parameters.AddWithValue("@username", username);
            using var reader = command.ExecuteReader();
            if (reader.Read())
                return new Employee
                {
                    Id = reader.GetInt32("employee_id"),
                    Name = reader.GetString("name"),
                    Username = reader.GetString("username"),
                    Password = reader.GetString("password"),
                    Hours = reader.GetFloat("hours"),
                    Email = reader.GetString("email"),
                    Status = (Employee.EmployeeStatus)reader.GetInt32("status"),
                    Role = (Employee.EmployeeRole)reader.GetInt32("role")
                };
            else
                return null;
        }
    }
}
