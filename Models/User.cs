using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ActivityCenter.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required]
        [MinLength (2, ErrorMessage="Name must be atleast 2 characters")]
        public string Name {get;set;}


        [EmailAddress]
        [Required]
        public string Email {get;set;}

        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage="Password must be atleast 8 characters long")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage = "Password must contain 1 letter, 1 number and 1 special character")]
        public string Password {get;set;}
        
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        [NotMapped]
        [Compare("Password", ErrorMessage="Passwords must match")]
        [DataType(DataType.Password)]
        public string Confirm {get;set;}  
        public List<Participant> UserParticipant {get;set;}
        public User()
        {
            UserParticipant = new List<Participant>();
        }
    }
}