using System.ComponentModel.DataAnnotations;
using System;
namespace Domain
{
    public class Message
    {
        public Message(int id, string type, string data, bool isMine)
        {
            Id = id;
            Type = type;
            TimeSent = DateTime.Now;
            Data = data;
            IsMine = isMine;
        }
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