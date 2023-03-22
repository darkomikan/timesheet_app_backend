using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domainEntities.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string itemName) : base($"The requested {itemName} was not found.") { }
    }
}
