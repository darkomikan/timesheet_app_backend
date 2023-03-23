using domainEntities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;
using System.Data;

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

        [Authorize(Roles = "Admin,Worker")]
        [HttpGet]
        public Project Get(int id)
        {
            return projectService.GetProjectById(id);
        }

        [Authorize(Roles = "Admin,Worker")]
        [HttpGet]
        public Project[] GetAll()
        {
            return projectService.GetProjects();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void Add(Project project)
        {
            projectService.InsertProject(project);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public void Update(Project project)
        {
            projectService.UpdateProject(project);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public void Delete(int id)
        {
            projectService.DeleteProject(id);
        }
    }
}
