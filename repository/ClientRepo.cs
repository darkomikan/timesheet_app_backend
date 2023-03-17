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
    public class ClientRepo : IRepository<Client>
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public ClientRepo(IConfiguration configuration)
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
                UPDATE clients
                SET deleted_at = @date
                WHERE client_id = @id
            ";
            command.Parameters.AddWithValue("@date", DateTime.Now);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public Client? Get(int id)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT *
                FROM clients
                WHERE client_id = @id AND deleted_at IS NULL
            ";
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
                return new Client
                {
                    Id = reader.GetInt32("client_id"),
                    Name = reader.GetString("name"),
                    Address = reader.GetString("address"),
                    City = reader.GetString("city"),
                    Country = reader.GetString("country"),
                    Zip = reader.GetString("zip")
                };
            else
                return null;
        }

        public Client[] GetAll()
        {
            List<Client> result = new List<Client>();
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT * FROM clients
                WHERE deleted_at IS NULL
            ";
            using var reader = command.ExecuteReader();
            while (reader.Read())
                result.Add(new Client
                {
                    Id = reader.GetInt32("client_id"),
                    Name = reader.GetString("name"),
                    Address = reader.GetString("address"),
                    City = reader.GetString("city"),
                    Country = reader.GetString("country"),
                    Zip = reader.GetString("zip")
                });
            return result.ToArray();
        }

        public void Insert(Client item)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO clients (address, city, country, name, zip)
                VALUES (@address, @city, @country, @name, @zip)
            ";
            command.Parameters.AddWithValue("@address", item.Address);
            command.Parameters.AddWithValue("@city", item.City);
            command.Parameters.AddWithValue("@country", item.Country);
            command.Parameters.AddWithValue("@name", item.Name);
            command.Parameters.AddWithValue("@zip", item.Zip);
            command.ExecuteNonQuery();
        }

        public void Update(Client item)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE clients
                SET address = @address, city = @city, country = @country, name = @name, zip = @zip
                WHERE client_id = @id
            ";
            command.Parameters.AddWithValue("@address", item.Address);
            command.Parameters.AddWithValue("@city", item.City);
            command.Parameters.AddWithValue("@country", item.Country);
            command.Parameters.AddWithValue("@name", item.Name);
            command.Parameters.AddWithValue("@zip", item.Zip);
            command.Parameters.AddWithValue("@id", item.Id);
            command.ExecuteNonQuery();
        }
    }
}
