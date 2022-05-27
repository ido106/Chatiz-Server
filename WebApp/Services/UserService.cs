using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Repository;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly WebAppContext _context;

        public UserService(WebAppContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.User.ToList();
        }

        public User Get(string username)
        {
            return _context.User.FirstOrDefault(x => x.Username == username);
        }

        public void Add(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
        }

        public bool Exist(string username)
        {
           var q = _context.User.Where(x => x.Username.Equals(username));
           return q.Count() > 0;
        }

        public List<Contact> GetContacts(string username)
        {
            User user = Get(username);
            if (user == null) return null;
            return user.Contacts;
        }

        public Contact GetContact(string username, string contact_name)
        {
            List<Contact> all_contacts = GetContacts(username);
            if (all_contacts == null) return null;
            Contact contact = all_contacts.FirstOrDefault(x => x.ContactUsername == contact_name);
            return contact;
        }

        public bool AddContact(string username, string contact_name)
        {
            User user = Get(username);
            if(user == null) return false;

            if(Get(contact_name) == null) return false;

            Contact contact = new (contact_name);

            user.Contacts.Add(contact);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteContact(string username, string contact_name)
        {
            User user = Get(username);
            if(user == null || username.Equals(contact_name)) return false;
            Contact contact = GetContact(username, contact_name);
            if(contact == null) return false;
            user.Contacts.Remove(contact);
            _context.SaveChanges();

            return true;
        }

    
    }
}
