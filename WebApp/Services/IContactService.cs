using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Services
{
    public interface IContactService
    {
        public Message GetLast(string username, string contact);
        
        public Message Get(int id);

        public void Add(string data);
        public void Update(string data);



    }
}
