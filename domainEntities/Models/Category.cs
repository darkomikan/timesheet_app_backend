﻿namespace domainEntities.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime? DeletedAt { get; set; }
}
