using domainEntities.Models;
using repository;
using service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IRepository<Category>, CategoryRepo>();
builder.Services.AddScoped<IRepository<Client>, ClientRepo>();
builder.Services.AddScoped<IRepository<Employee>, EmployeeRepo>();
builder.Services.AddScoped<IRepository<Project>, ProjectRepo>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
