using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace InstagramWorthy.Models
{
    public class LoginUser
    {
        [Key]
        public int LoginUserId {get;set;}

        [Required]
        [EmailAddress]
        public string login_Email {get;set;}

        [Required]
        public string login_Password {get;set;}

    }
}