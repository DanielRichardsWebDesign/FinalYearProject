using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Project.Models
{
    public class ProjectRoles
    {
        public virtual string RoleId { get; set; }
        public virtual string PublicID { get; set; }

        public virtual ApplicationRole Role { get; set; }
        public virtual Projects Projects { get; set; }
    }
}