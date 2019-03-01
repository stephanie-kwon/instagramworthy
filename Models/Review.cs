using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace InstagramWorthy.Models
{
    public class Review
    {
        [Key]
        public int ReviewId {get;set;}

        [Required]
        public string Name {get;set;}

        [Required]
        public string Location {get;set;}

        [Required]
        public string Description {get;set;}

        public string PicUrl {get;set;}

        public User reviewuploader {get;set;}

        public Place review {get;set;}

    }
}