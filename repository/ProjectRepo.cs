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
        private readonly IRepository<Client> clientRepo;
        private readonly IRepository<Employee> employeeRepo;
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public ProjectRepo(IConfiguration configuration, IRepository<Client> clientRepo, IRepository<Employee> employeeRepo)
        {
            this.clientRepo = clientRepo;
            this.employeeRepo = employeeRepo;
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
                UPDATE projects
                SET deleted_at = @date
                WHERE project_id = @id
            ";
            command.Parameters.AddWithValue("@date", DateTime.Now);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public Project? Get(int id)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT *
                FROM projects
                WHERE project_id = @id AND deleted_at IS NULL
            ";
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
                return new Project
                {
                    Id = reader.GetInt32("project_id"),
                    Client = clientRepo.Get(reader.GetInt32("client_id")),
                    Lead = employeeRepo.Get(reader.GetInt32("lead_id")),
                    Name = reader.GetString("name"),
                    Description = reader.GetString("description"),
                    Status = (Project.ProjectStatus)reader.GetInt32("status")
                };
            else
                return null;
        }

        public Project[] GetAll()
        {
            List<Project> result = new List<Project>();
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT * FROM projects
                WHERE deleted_at IS NULL
            ";
            using var reader = command.ExecuteReader();
            while (reader.Read())
                result.Add(new Project
                {
                    Id = reader.GetInt32("project_id"),
                    Client = clientRepo.Get(reader.GetInt32("client_id")),
                    Lead = employeeRepo.Get(reader.GetInt32("lead_id")),
                    Name = reader.GetString("name"),
                    Description = reader.GetString("description"),
                    Status = (Project.ProjectStatus)reader.GetInt32("status")
                });
            return result.ToArray();
        }

        public void Insert(Project item)
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

        public void Update(Project item)
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
            command.ExecuteNonQuery();
        }
    }
}
