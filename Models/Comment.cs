using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheWall.Models
{
    public class Comment
    {
        [Key]
        public int CommentId {get;set;}

        [Required]
        [Display(Name = "Post a comment")]
        public string CommentBody {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        // Foreign Keys
        public int MessageId {get;set;}
        public int UserId {get;set;}

        // Navigation Properties
        public User User {get;set;}
        public Message Message {get;set;}

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