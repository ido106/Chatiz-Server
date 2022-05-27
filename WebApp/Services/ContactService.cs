using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ContactService : IContactService
    {
        private readonly WebAppContext _context;
        private readonly IUserService _userService;

        public ContactService(WebAppContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public Message getLast(string username, string contact)
        {
            throw new NotImplementedException();
        }
    }
}
