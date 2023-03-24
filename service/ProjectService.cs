using domainEntities.Models;
using repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class ProjectService
    {
        private readonly IRepository<Project> projectRepo;

        public ProjectService(IRepository<Project> projectRepo)
        {
            this.projectRepo = projectRepo;
        }

        public Project[] GetProjects(string pattern)
        {
            return projectRepo.GetAll(pattern);
        }

        public Project GetProjectById(int id)
        {
            return projectRepo.Get(id);
        }

        public void InsertProject(Project project)
        {
            projectRepo.Insert(project);
        }

        public void UpdateProject(Project project)
        {
            projectRepo.Update(project);
        }

        public void DeleteProject(int id)
        {
            projectRepo.Delete(id);
        }
    }
}
