﻿using domainEntities.Exceptions;
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
    public class ProjectRepo : IRepository<Project>
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public ProjectRepo(IConfiguration configuration)
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
                    UPDATE projects
                    SET deleted_at = @date
                    WHERE project_id = @id
                ";
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@id", id);
                if (command.ExecuteNonQuery() < 1)
                    throw new NotFoundException("project");
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }

        public Project Get(int id)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT project_id, projects.client_id AS client_id, lead_id, projects.name AS pname, projects.description AS pdescription,
                    projects.status AS pstatus, projects.deleted_at AS deleted_at,
                    clients.name AS cname, clients.address AS caddress, clients.city AS ccity, clients.zip AS czip, clients.country AS ccountry,
                    employees.name AS ename, employees.username AS eusername, employees.hours AS ehours,
                    employees.email AS eemail, employees.status AS estatus, employees.role AS erole

                    FROM projects LEFT JOIN (clients, employees)
                    ON (clients.client_id = projects.client_id AND employees.employee_id = projects.lead_id)
                    WHERE project_id = @id AND projects.deleted_at IS NULL
                ";
                command.Parameters.AddWithValue("@id", id);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                    return new Project
                    {
                        Id = reader.GetInt32("project_id"),
                        Client = new Client
                        {
                            Id = reader.GetInt32("client_id"),
                            Name = reader.GetString("cname"),
                            Address = reader.GetString("caddress"),
                            City = reader.GetString("ccity"),
                            Country = reader.GetString("ccountry"),
                            Zip = reader.GetString("czip")
                        },
                        Lead = new Employee
                        {
                            Id = reader.GetInt32("lead_id"),
                            Name = reader.GetString("ename"),
                            Username = reader.GetString("eusername"),
                            Hours = reader.GetFloat("ehours"),
                            Email = reader.GetString("eemail"),
                            Status = (Employee.EmployeeStatus)reader.GetInt32("estatus"),
                            Role = (Employee.EmployeeRole)reader.GetInt32("erole")
                        },
                        Name = reader.GetString("pname"),
                        Description = reader.GetString("pdescription"),
                        Status = (Project.ProjectStatus)reader.GetInt32("pstatus")
                    };
                else
                    throw new NotFoundException("project");
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }

        public Project[] GetAll()
        {
            try
            {
                List<Project> result = new List<Project>();
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT project_id, projects.client_id AS client_id, lead_id, projects.name AS pname, projects.description AS pdescription,
                    projects.status AS pstatus, projects.deleted_at AS deleted_at,
                    clients.name AS cname, clients.address AS caddress, clients.city AS ccity, clients.zip AS czip, clients.country AS ccountry,
                    employees.name AS ename, employees.username AS eusername, employees.hours AS ehours,
                    employees.email AS eemail, employees.status AS estatus, employees.role AS erole

                    FROM projects LEFT JOIN (clients, employees)
                    ON (clients.client_id = projects.client_id AND employees.employee_id = projects.lead_id)
                    WHERE projects.deleted_at IS NULL
                ";
                using var reader = command.ExecuteReader();
                while (reader.Read())
                    result.Add(new Project
                    {
                        Id = reader.GetInt32("project_id"),
                        Client = new Client
                        {
                            Id = reader.GetInt32("client_id"),
                            Name = reader.GetString("cname"),
                            Address = reader.GetString("caddress"),
                            City = reader.GetString("ccity"),
                            Country = reader.GetString("ccountry"),
                            Zip = reader.GetString("czip")
                        },
                        Lead = new Employee
                        {
                            Id = reader.GetInt32("lead_id"),
                            Name = reader.GetString("ename"),
                            Username = reader.GetString("eusername"),
                            Hours = reader.GetFloat("ehours"),
                            Email = reader.GetString("eemail"),
                            Status = (Employee.EmployeeStatus)reader.GetInt32("estatus"),
                            Role = (Employee.EmployeeRole)reader.GetInt32("erole")
                        },
                        Name = reader.GetString("pname"),
                        Description = reader.GetString("pdescription"),
                        Status = (Project.ProjectStatus)reader.GetInt32("pstatus")
                    });
                return result.ToArray();
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }

        public void Insert(Project item)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    INSERT INTO projects (client_id, lead_id, name, description, status)
                    VALUES (@client_id, @lead_id, @name, @description, @status)
                ";
                command.Parameters.AddWithValue("@client_id", item.Client.Id);
                command.Parameters.AddWithValue("@lead_id", item.Lead.Id);
                command.Parameters.AddWithValue("@name", item.Name);
                command.Parameters.AddWithValue("@description", item.Description);
                command.Parameters.AddWithValue("@status", (int)item.Status);
                command.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }

        public void Update(Project item)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    UPDATE projects
                    SET name = client_id = @client_id, lead_id = @lead_id, name = @name,
                    description = @description, status = @status
                    WHERE project_id = @id
                ";
                command.Parameters.AddWithValue("@client_id", item.Client.Id);
                command.Parameters.AddWithValue("@lead_id", item.Lead.Id);
                command.Parameters.AddWithValue("@name", item.Name);
                command.Parameters.AddWithValue("@description", item.Description);
                command.Parameters.AddWithValue("@status", (int)item.Status);
                command.Parameters.AddWithValue("@id", item.Id);
                if (command.ExecuteNonQuery() < 1)
                    throw new NotFoundException("project");
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }
    }
}
