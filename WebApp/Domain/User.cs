using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class User
    {
        [Key]
        public string Username { get; set; }

        [Required]
        public string Nickname { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Server { get; set; }

        public DateTime LastSeen { get; set; }

        public string ImgSrc { get; set; }

        public List<Contact> Contacts { get; set; }

    }
}
