using domainEntities.Models;
using repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class CategoryService
    {
        private readonly IRepository<Category> categoryRepo;

        public CategoryService(IRepository<Category> categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }

        public Category[] GetCategories(string pattern)
        {
            return categoryRepo.GetAll(pattern);
        }

        public Category GetCategoryById(int id)
        {
            return categoryRepo.Get(id);
        }

        public void InsertCategory(Category category)
        {
            categoryRepo.Insert(category);
        }

        public void UpdateCategory(Category category)
        {
            categoryRepo.Update(category);
        }

        public void DeleteCategory(int id)
        {
            categoryRepo.Delete(id);
        }
    }
}
