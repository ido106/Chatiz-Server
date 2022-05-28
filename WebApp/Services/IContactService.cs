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
        public Task<Message> GetLast(string username, string contact);
        public Task<Message> Get(string username, string contact, int id);
        public Task<bool> Add(string username, string contact, string data);
        public Task<bool> Update(string id, int id2, string username, string newData);

    }
}
