using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Repository;

namespace Services
{
    public class UserService
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
            _context.User.Find(x => x.Username == username);
        }

        public void Edit(String username)
        {

        }

        public void Add(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
        }
    }
}
