using domainEntities.Exceptions;
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
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    UPDATE employees
                    SET deleted_at = @date
                    WHERE employee_id = @id AND deleted_at IS NULL
                ";
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@id", id);
                if (command.ExecuteNonQuery() < 1)
                    throw new NotFoundException("employee");
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }

        public Employee Get(int id)
        {
            try
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
                        Hours = reader.GetFloat("hours"),
                        Email = reader.GetString("email"),
                        Status = (Employee.EmployeeStatus)reader.GetInt32("status"),
                        Role = (Employee.EmployeeRole)reader.GetInt32("role")
                    };
                else
                    throw new NotFoundException("employee");
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }

        public Employee[] GetAll()
        {
            try
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
                        Hours = reader.GetFloat("hours"),
                        Email = reader.GetString("email"),
                        Status = (Employee.EmployeeStatus)reader.GetInt32("status"),
                        Role = (Employee.EmployeeRole)reader.GetInt32("role")
                    });
                return result.ToArray();
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }

        public void Insert(Employee item)
        {
            try
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
            catch (MySqlException ex)
            {
                switch ((MySqlErrorCode)ex.Number)
                {
                    case MySqlErrorCode.DuplicateKeyEntry:
                        throw new BadRequestException($"Employe with username {item.Username} already exists.");
                    default:
                        throw new InternalServerException("Unable to successfully connect to database.");
                }
            }
        }

        public void Update(Employee item)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    UPDATE employees
                    SET name = @name, username = @username, hours = @hours,
                    email = @email, status = @status, role = @role
                    WHERE employee_id = @id AND deleted_at IS NULL
                ";
                command.Parameters.AddWithValue("@name", item.Name);
                command.Parameters.AddWithValue("@username", item.Username);
                command.Parameters.AddWithValue("@hours", item.Hours);
                command.Parameters.AddWithValue("@email", item.Email);
                command.Parameters.AddWithValue("@status", (int)item.Status);
                command.Parameters.AddWithValue("@role", (int)item.Role);
                command.Parameters.AddWithValue("@id", item.Id);
                if (command.ExecuteNonQuery() < 1)
                    throw new NotFoundException("employee");
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }

        public void UpdatePassword(string username, string password)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    UPDATE employees
                    SET password = @password
                    WHERE username = @username AND deleted_at IS NULL
                ";
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@username", username);
                if (command.ExecuteNonQuery() < 1)
                    throw new NotFoundException("employee");
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }

        public Employee GetByUsername(string username)
        {
            try
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
                    throw new NotFoundException("employee");
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }
    }
}
