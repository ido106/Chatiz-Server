using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Contact
    {
        public Contact(string contact_username)
        {
            ContactUsername = contact_username;
            Messages = new List<Message>();
        }

        [Key]
        public string ContactUsername { get; set; }

        public List<Message> Messages { get; set; }
    }
}