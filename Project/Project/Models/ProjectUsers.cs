using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Project.Models
{
    public class ProjectUsers
    {
        [Key]
        [Required]
        public virtual string ApplicationUserID { get; set; }

        [Required]
        public virtual string PublicID { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Projects Projects { get; set; }

        //Collection of roles for users
        public virtual ICollection<ProjectRoles> ProjectRoles { get; set; }
    }
}