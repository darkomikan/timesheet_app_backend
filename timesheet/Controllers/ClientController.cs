using domainEntities.Models;
using Microsoft.AspNetCore.Mvc;
using service;

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

        [HttpGet]
        public Client Get(int id) 
        {
            return clientService.GetClientById(id);
        }

        [HttpGet]
        public Client[] GetAll()
        {
            return clientService.GetClients();
        }

        [HttpPost]
        public void Add(Client client)
        {
            clientService.InsertClient(client);
        }

        [HttpPut]
        public void Update(Client client)
        {
            clientService.UpdateClient(client);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            clientService.DeleteClient(id);
        }
    }
}
