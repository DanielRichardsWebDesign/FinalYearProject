using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Project.Models
{
    public class Tasks
    {
        [Key]
        public int TaskID { get; set; }
        [Required]
        public int PublicID { get; set; }
        
        public string ApplicationUserID { get; set; }
        [Required]
        public string TaskDescription { get; set; }
        public bool IsComplete { get; set; }

        //Virtual objects for retrieving relevant data
        public virtual Projects Projects { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        //Collection of users assigned to task
        public virtual ICollection<UserTasks> UserTasks { get; set; }
    }
}