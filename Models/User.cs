using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;



namespace InstagramWorthy.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}

        [Required]
        [MinLength(3)]
        public string FirstName {get;set;}

        [Required]
        [MinLength(3)]
        public string LastName {get;set;}

        [Required]
        [EmailAddress]
        public string Email {get;set;}

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        public string Password {get;set;}

        [Required]
        public string Location {get;set;}

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}
        public DateTime Created_at {get;set;} = DateTime.Now;
        public DateTime Updated_at {get;set;} = DateTime.Now;

        [InverseProperty("Uploader")]
        public List<Place> Places {get;set;}

        public List<Review> Reviews {get;set;}

        public List<Like> Likes {get;set;}

        public User(){
            Places = new List<Place>();
            Reviews = new List<Review>();
            Likes = new List<Like>();
        }

        [DataType(DataType.Upload)]
        [NotMapped]
        public IFormFile ImageFile {get;set;}
        public string ProfileUrl {get;set;} 


    }
}