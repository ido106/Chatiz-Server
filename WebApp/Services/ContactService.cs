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



        public async Task<Message> GetLast(string username, string contact)
        {
            if(username == null || contact == null || await _userService.Get(username) == null || await _userService.GetContact(username, contact) == null)
            {
                return null;
            } 

            Contact c =  (await _userService.Get(username)).Contacts.Find(x => x.ContactUsername == contact);
            return c.Messages.Last();
        }
        public async Task<Message> Get(string username, string contact, int id)
        {
            if (username == null || contact == null || await _userService.Get(username) == null || await _userService.GetContact(username, contact) == null)
            {
                return null;
            }

            return (await _userService.GetContact(username, contact)).Messages.FirstOrDefault(m => m.Id == id);
        }

            private async Task<bool> addMessageHelper(string username, string contacat, string data, bool isMine)
        {
            if (_userService.Get(username) == null) return false;
            Contact c = await _userService.GetContact(username, contacat);
            if (c == null) return false;
            int id = c.Messages.Max(x => x.Id) + 1;
            Message message = new(id, "text", data, isMine);
            c.Messages.Add(message);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Add(string username, string contact, string data)
        {
            //maybe need to check if contact is in this server, if he is - add the message to his list as well, if he isnt - send a request to the other server.
            bool temp = await addMessageHelper(username, contact, data, true);
            if (temp) await _context.SaveChangesAsync();
            return temp;
        }

        public async Task<bool> Update(string id,int id2, string username, string newData)
        {
            if (username == null || newData == null) return false;
            Contact c = await _userService.GetContact(username, id);
            if (c == null) return false;
            Message m = c.Messages.FirstOrDefault(m => m.Id == id2);
            m.Data = newData;
            await _context.SaveChangesAsync();
            return true;
        } 
    }
}
