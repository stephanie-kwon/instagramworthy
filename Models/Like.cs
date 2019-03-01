using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace InstagramWorthy.Models
{
    public class Like
    {
        [Key]
        public int LikeId {get;set;}

        public int UserId {get;set;}
        public User liker {get;set;}

        [InverseProperty("Likes")]
        public int PlaceId {get;set;}

        public Place place {get;set;}

    }
}