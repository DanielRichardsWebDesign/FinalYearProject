using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Project.Models
{
    public class ProjectUsers
    {
        [Key]
        public virtual int ProjectUserID { get; set; }
        
        [Required]
        public virtual string ApplicationUserID { get; set; }

        [Required]
        public virtual int PublicID { get; set; }

        //True or false for admin status
        public virtual bool IsAdmin { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Projects Projects { get; set; }

        //Collection of roles for users
        //public virtual ICollection<ProjectRoles> ProjectRoles { get; set; }

        //Collection of tasks assigned to user
        public virtual ICollection<UserTasks> UserTasks { get; set; }
    }
}