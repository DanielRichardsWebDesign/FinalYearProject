using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class UserTasks
    {
        [Key]
        [Column(Order = 1)]
        public int TaskID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ProjectUserID { get; set; }

        public virtual Tasks Tasks { get; set; }
        public virtual ProjectUsers ProjectUsers { get; set; }
    }
}