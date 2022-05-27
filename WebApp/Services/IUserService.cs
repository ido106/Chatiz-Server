using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Services
{
    public interface IUserService
    {
        public void Add(User usern);
        public bool Exist(string username);
        public User Get(string username);
        public List<Contact> GetContacts(string username);
        public Contact GetContact(string username, string contact_name);
        public bool AddContact(string username, string contact_name);
        public bool DeleteContact(string username, string contact_name);
        
    }
}
