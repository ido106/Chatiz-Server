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


        public void Add(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
        }

        public bool Exist(string username)
        {
            var q = _context.User.Where(x => x.Username.Equals(username));
            if (q.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public List<Contact> GetContacts(User user)
        {
            throw new NotImplementedException();
        }

        public Contact GetContact(User user, string contact_name)
        {
            throw new NotImplementedException();
        }

        public User getContactByUser(User user, string contact_name)
        {
            throw new NotImplementedException();
        }

        public void AddContact(User user, string contact_name)
        {
            throw new NotImplementedException();
        }

        public void DeleteContact(User user, string contact_name)
        {
            throw new NotImplementedException();
        }

        public User Get(string username)
        {
            throw new NotImplementedException();
        }
    }
}
