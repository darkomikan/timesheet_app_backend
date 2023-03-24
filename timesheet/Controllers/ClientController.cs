using domainEntities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using service;
using System.Data;

namespace timesheet.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ClientController : Controller
    {
        private readonly ClientService clientService;

        public ClientController(ClientService clientService)
        {
            this.clientService = clientService;
        }

        [Authorize(Roles = "Admin,Worker")]
        [HttpGet]
        public Client Get(int id) 
        {
            return clientService.GetClientById(id);
        }

        [Authorize(Roles = "Admin,Worker")]
        [HttpGet]
        public Client[] GetAll(string pattern)
        {
            return clientService.GetClients(pattern);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public void Add(Client client)
        {
            clientService.InsertClient(client);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public void Update(Client client)
        {
            clientService.UpdateClient(client);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public void Delete(int id)
        {
            clientService.DeleteClient(id);
        }
    }
}
