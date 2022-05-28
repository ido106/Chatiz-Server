using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Contact
    {
        public Contact(string contact_username,string nickName, string server)
        {
            ContactUsername = contact_username;
            Messages = new List<Message>();
            NickName = nickName;
            Server = server;
        }

        [Key]
        public string ContactUsername { get; set; }

        [Required]
        public string NickName { get; set; }

        [Required]
        public string Server { get; set; }

        [Required]
        public string Last { get; set; }

        [Required]
        public DateTime LastDate { get; set; }

        [Required]
        public List<Message> Messages { get; set; }
    }
}