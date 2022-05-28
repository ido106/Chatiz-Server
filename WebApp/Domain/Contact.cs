using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Contact
    {
        [Key]
        [Required]
        public string ContactUsername { get; set; }

        [Required]
        public User User { get; set; }

        public List <Message> Messages { get; set; }

        public string LastMessage { get; set; }
    }
}