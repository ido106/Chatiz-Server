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
        public Message Get(string username, string contact, int id);
        public bool Add(string username, string contact, string data);
        public bool Update(string id, int id2, string username, string newData);



    }
}
