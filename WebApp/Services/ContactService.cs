using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ContactService : IMessageService
    {
        private readonly WebAppContext _context;

        public ContactService(WebAppContext context)
        {
            _context = context;
        }
    }
}
