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
    public class Place
    {
        [Key]
        public int PlaceId {get;set;}

        //[Required]
        public string Name {get;set;}

        //[Required]
        public string Location {get;set;}

        //[Required]
        public string Description {get;set;}
        
        // public List<Review> Reviews {get;set;}

        public int UserId {get;set;}
        

        [ForeignKey("UserId")]
        public User Uploader {get;set;}

        public List<Like> Liked {get;set;}


        [Required]
        [DataType(DataType.Upload)]
        [NotMapped]
        public IFormFile ImageFile {get;set;}
        public string PicUrl {get;set;} 
   


    }
}