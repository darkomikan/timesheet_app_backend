using domainEntities.Models;
using Microsoft.AspNetCore.Mvc;
using service;

namespace timesheet.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProjectController : Controller
    {
        private readonly ProjectService projectService;

        public ProjectController(ProjectService projectService)
        {
            this.projectService = projectService;
        }

        [HttpGet]
        public Project Get(int id)
        {
            return projectService.GetProjectById(id);
        }

        [HttpGet]
        public Project[] GetAll()
        {
            return projectService.GetProjects();
        }

        [HttpPost]
        public void Add(Project project)
        {
            projectService.InsertProject(project);
        }

        [HttpPut]
        public void Update(Project project)
        {
            projectService.UpdateProject(project);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            projectService.DeleteProject(id);
        }
    }
}
