using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Contact
    {
        [Key]
        [Required]
        public string ContactUsername { get; set; }

        public string Nickname { get; set; }

        public string  Server { get; set; }

        public DateTime LastSeen { get; set; }

        public string LastMessage { get; set; }

        [JsonIgnore]
        public string ImgSrc { get; set; }

        [JsonIgnore]
        public List<Message> Messages { get; set; }

        /*[Required]
        public User User { get; set; }*/
    }
}