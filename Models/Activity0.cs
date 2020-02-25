using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ActivityCenter.Models
{
    public class Activity0
    {
        [Key]
        public int ActivityId {get;set;}

        [Required(ErrorMessage="Title is required")]
        public string Title {get;set;}

        [Required(ErrorMessage="Date is required")]
        [DataType(DataType.Date)]
        public DateTime Date {get;set;}

        [Required(ErrorMessage="Time is required")]
        [DataType(DataType.Time)]
        public DateTime Time {get;set;}

        [Required(ErrorMessage="Duration is required")]
        public int Duration {get;set;}

        [Required(ErrorMessage="Description is required")]
        public string Description {get;set;}
        public User Coordinator {get;set;}
        public int CoordinatorId {get;set;}
        
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public List<Participant> ActivityParticipant {get;set;}
        public Activity0()
        {
            ActivityParticipant = new List<Participant>();
        }

    }
}