using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Project.Models
{
    public class Projects
    {
        [Key]
        public int PublicID { get; set; }

        [MaxLength(25)]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} characters long", MinimumLength = 4)]
        [Display(Name = "Project Name")]
        [Required]
        public string ProjectName { get; set; }

        [MaxLength(50)]
        [Required]
        public string ProjectType { get; set; }

        [MaxLength(250)]
        [StringLength(250, ErrorMessage = "The {0} must be at least {2} characters long", MinimumLength = 10)]
        [Display(Name = "Project Description")]
        [Required]
        public string ProjectDescription { get; set; }
        //True for private. False for public
        //public bool ProjectPrivacy { get; set; }
        
        //Name to set for Blob storage container for this Project
        [MaxLength(63)]
        [StringLength(63, ErrorMessage = "The {0} must be at least {2} characters long", MinimumLength = 3)]
        [Display(Name = "Project Container Name")]
        [Required]
        public string ProjectContainerName { get; set; }

        //Define the certain date/time format to use 
        [DataType(DataType.Date), Column(TypeName = "Date"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        //Reference ApplicationUser_Id from Identity framework
        public string ApplicationUserID { get; set; }
        
        public virtual ApplicationUser ApplicationUser { get; set; }

        //Collection of files on Project
        public virtual ICollection<Files> Files { get; set; }

        //Collection of users on Project
        public virtual ICollection<ProjectUsers> ProjectUsers { get; set; }

        //Collection of User requests on Project
        public virtual ICollection<ProjectUserRequests> ProjectUserRequests { get; set; }

        //Collection of tasks on Project
        public virtual ICollection<Tasks> Tasks { get; set; }
    }

    public class ProjectDBContext : DbContext
    {
        public virtual DbSet<Projects> Projects { get; set; }        
    }
}