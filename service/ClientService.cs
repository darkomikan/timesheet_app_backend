using domainEntities.Models;
using repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class ClientService
    {
        private readonly IRepository<Client> clientRepo;

        public ClientService(IRepository<Client> clientRepo)
        {
            this.clientRepo = clientRepo;
        }

        public Client[] GetClients()
        {
            return clientRepo.GetAll();
        }

        public Client GetClientById(int id)
        {
            return clientRepo.Get(id);
        }

        public void InsertClient(Client client)
        {
            clientRepo.Insert(client);
        }

        public void UpdateClient(Client client)
        {
            clientRepo.Update(client);
        }

        public void DeleteClient(int id)
        {
            clientRepo.Delete(id);
        }
    }
}
