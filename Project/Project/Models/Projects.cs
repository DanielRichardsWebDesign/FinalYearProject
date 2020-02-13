using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Projects
    {
        [Key]
        public int PublicID { get; set; }
        [MaxLength(25)]
        [Required]
        public string ProjectName { get; set; }
        [MaxLength(50)]
        [Required]
        public string ProjectType { get; set; }
        [MaxLength(250)]
        [Required]
        public string ProjectDescription { get; set; }
        //True for private. False for public
        //public bool ProjectPrivacy { get; set; }      

        //Reference ApplicationUser_Id from Identity framework
        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}