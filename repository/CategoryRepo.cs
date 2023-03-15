﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using domainEntities.Models;
using MySql.Data.MySqlClient;

namespace repository
{
    public class CategoryRepo : IRepository<Category>
    {
        private readonly AppContext appContext;

        public CategoryRepo(AppContext appContext) 
        {
            this.appContext = appContext;
        }

        public void Delete(int id)
        {
            var command = appContext.Connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE categories
                SET deleted_at = @date
                WHERE category_id = @id
            ";
            command.Parameters.AddWithValue("@date", DateTime.Now);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        public Category Get(int id)
        {
            var command = appContext.Connection.CreateCommand();
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
                return null;
        }

        public Category[] GetAll()
        {
            List<Category> result = new List<Category>();
            var command = appContext.Connection.CreateCommand();
            command.CommandText =
            @"
                SELECT * FROM categories
                WHERE deleted_at IS NULL
            ";
            using var reader = command.ExecuteReader();
            while (reader.Read()) 
                result.Add(new Category { Id = reader.GetInt32("category_id"), Name  = reader.GetString("name") });
            return result.ToArray();
        }

        public void Insert(Category item)
        {
            var command = appContext.Connection.CreateCommand();
            command.CommandText = 
            @"
                INSERT INTO categories (name) VALUES (@name)
            ";
            command.Parameters.AddWithValue("@name", item.Name);
            command.ExecuteNonQuery();
        }

        public void Update(Category item)
        {
            var command = appContext.Connection.CreateCommand();
            command.CommandText =
            @"
                UPDATE categories
                SET name = @name
                WHERE category_id = @id
            ";
            command.Parameters.AddWithValue("@name", item.Name);
            command.Parameters.AddWithValue("@id", item.Id);
            command.ExecuteNonQuery();
        }
    }
}
