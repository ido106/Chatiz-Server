using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
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
