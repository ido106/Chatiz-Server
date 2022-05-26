using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class User
    {
        [Key]
        public string Username { get; set; }

        public string Nickname { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Server { get; set; }

        [Required]
        public DateTime LastSeen { get; set; }

        public string ImgSrc { get; set; }

        public List<Contact> Contacts { get; set; }

    }
}
