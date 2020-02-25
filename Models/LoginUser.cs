using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityCenter.Models
{
    public class LoginUser
    {
        [Required]
        [EmailAddress]
        public string LEmail {get;set;}

        [Required]
        [DataType(DataType.Password)]
        public string LPassword {get;set;}
    }
}