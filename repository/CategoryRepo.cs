using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using domainEntities.Models;
using domainEntities.Exceptions;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace repository
{
    public class CategoryRepo : IRepository<Category>
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public CategoryRepo(IConfiguration configuration) 
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
                    UPDATE categories
                    SET deleted_at = @date
                    WHERE category_id = @id
                ";
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@id", id);
                if (command.ExecuteNonQuery() < 1)
                    throw new NotFoundException("category");
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }

        public Category Get(int id)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT category_id, name
                    FROM categories
                    WHERE category_id = @id AND deleted_at IS NULL
                ";
                command.Parameters.AddWithValue("@id", id);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                    return new Category { Id = reader.GetInt32("category_id"), Name = reader.GetString("name") };
                else
                    throw new NotFoundException("category");
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }

        public Category[] GetAll()
        {
            try
            {
                List<Category> result = new List<Category>();
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT * FROM categories
                    WHERE deleted_at IS NULL
                ";
                using var reader = command.ExecuteReader();
                while (reader.Read())
                    result.Add(new Category { Id = reader.GetInt32("category_id"), Name = reader.GetString("name") });
                return result.ToArray();
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }

        public void Insert(Category item)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    INSERT INTO categories (name) VALUES (@name)
                ";
                command.Parameters.AddWithValue("@name", item.Name);
                command.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }

        public void Update(Category item)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    UPDATE categories
                    SET name = @name
                    WHERE category_id = @id
                ";
                command.Parameters.AddWithValue("@name", item.Name);
                command.Parameters.AddWithValue("@id", item.Id);
                if (command.ExecuteNonQuery() < 1)
                    throw new NotFoundException("category");
            }
            catch (MySqlException)
            {
                throw new InternalServerException("Unable to successfully connect to database.");
            }
        }
    }
}
