using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheWall.Models
{
    public class Message
    {
        [Key]
        public int MessageId {get;set;}

        [Required]
        [Display(Name = "Post a message")]
        public string Body {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        // Foreign Keys
        public int UserId {get;set;}

        // Navigation Properties
        public User Author {get;set;}
        public List<Comment> MessageComments {get;set;}

        // Methods
        [NotMapped]
        public string DisplayCreatedAt
        {
            get
            {
                return this.CreatedAt.ToString("d");
            }
        }
    }
}