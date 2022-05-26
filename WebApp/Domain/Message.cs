using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Data { get; set; }

        [Required]
        public DateTime TimeSent { get; set; }

        [Required]
        public bool IsMine { get; set; }
    }
}