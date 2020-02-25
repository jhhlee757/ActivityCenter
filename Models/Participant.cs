using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ActivityCenter.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantId {get;set;}

        public int UserId {get;set;}
        public int ActivityId {get;set;}
        public User JoiningUser {get;set;}
        public Activity0 JoiningActivity {get;set;}
        
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}