using domainEntities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;
using System.Data;

namespace timesheet.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CategoryController : Controller
    {
        private readonly CategoryService categoryService;

        public CategoryController(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [Authorize(Roles = "Admin,Worker")]
        [HttpGet]
        public Category Get(int id)
        {
            return categoryService.GetCategoryById(id);
        }

        [Authorize(Roles = "Admin,Worker")]
        [HttpGet]
        public Category[] GetAll(string pattern)
        {
            return categoryService.GetCategories(pattern);
        }

        [Authorize(Roles = "Admin,Worker")]
        [HttpPost]
        public void Add(Category category)
        {
            categoryService.InsertCategory(category);
        }

        [Authorize(Roles = "Admin,Worker")]
        [HttpPut] 
        public void Update(Category category)
        {
            categoryService.UpdateCategory(category);
        }

        [Authorize(Roles = "Admin,Worker")]
        [HttpDelete] 
        public void Delete(int id)
        {
            categoryService.DeleteCategory(id);
        }
    }
}
