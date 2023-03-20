using domainEntities.Models;
using Microsoft.AspNetCore.Mvc;
using service;

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

        [HttpGet]
        public Category? Get(int id)
        {
            return categoryService.GetCategoryById(id);
        }

        [HttpGet]
        public Category[] GetAll()
        {
            return categoryService.GetCategories();
        }

        [HttpPost]
        public void Add(Category category)
        {
            categoryService.InsertCategory(category);
        }

        [HttpPut] 
        public void Update(Category category)
        {
            categoryService.UpdateCategory(category);
        }

        [HttpDelete] 
        public void Delete(int id)
        {
            categoryService.DeleteCategory(id);
        }
    }
}
