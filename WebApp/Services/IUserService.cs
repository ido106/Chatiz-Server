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
        public Task<bool> Exist(string username);
        public Task<User> Get(string username);
        public Task<List<Contact>> GetContacts(string username);
        public Task<Contact> GetContact(string username, string contact_name);
        public Task<bool> AddContact(string username, string contact_name,string nickName, string server);
        public Task<bool> DeleteContact(string username, string contact_name);
        
       //public Task<bool> GetContactsInfo(string username);
    }
}
