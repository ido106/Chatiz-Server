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



        public Message GetLast(string username, string contact)
        {
            if(username == null || contact == null || _userService.Get(username) == null || _userService.GetContact(username, contact) == null)
            {
                return null;
            } 

            Contact c = _userService.Get(username).Contacts.Find(x => x.ContactUsername == contact);
            int id = c.Messages.Max(x => x.Id);
            return c.Messages.FirstOrDefault(x => x.Id == id);
        }

        public Message Get(string username, string contact, int id)
        {
            if (username == null || contact == null || _userService.Get(username) == null || _userService.GetContact(username, contact) == null)
            {
                return null;
            }

            return _userService.GetContact(username, contact).Messages.FirstOrDefault(m => m.Id == id);
        }

            private bool addMessageHelper(string username, string contacat, string data, bool isMine)
        {
            if (_userService.Get(username) == null) return false;
            Contact c = _userService.GetContact(username, contacat);
            if (c == null) return false;
            int id = c.Messages.Max(x => x.Id) + 1;
            Message message = new Message(id, "text", data, isMine);
            c.Messages.Add(message);
            return true;
        }

        public bool Add(string username, string contact, string data)
        {
            bool temp = addMessageHelper(username, contact, data, true) && addMessageHelper(contact,username, data, false);
            if (temp) _context.SaveChanges();
            return temp;
        }

        public bool Update(string id,int id2, string username, string newData)
        {
            if (username == null || newData == null) return false;
            Contact c = _userService.GetContact(username, id);
            if (c == null) return false;
            Message m = c.Messages.FirstOrDefault(m => m.Id == id2);
            m.Data = newData;
            _context.SaveChanges();
            return true;
        } 
    }
}
