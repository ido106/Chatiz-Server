using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Contact
    {
        [Key]
        public string ContactUsername { get; set; }

        [Required]
        public string Nickname { get; set; }

        public DateTime LastSeen { get; set; }

        [Required]
        public string Server { get; set; }

        public string ImgSrc { get; set; }

        public List<Message> Messages { get; set; }
    }
}