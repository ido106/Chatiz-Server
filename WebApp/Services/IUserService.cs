using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Services
{
    interface IUserService
    {
        public void Add(User user);
        public bool Exist(string username);
        public User Get(string username);
        public List<Contact> GetContacts(User user);
        public Contact GetContact(User user, string contact_name);
        public User getContactByUser(User user, string contact_name);
        public void AddContact(User user, string contact_name);
        public void DeleteContact(User user, string contact_name);
        
    }
}
