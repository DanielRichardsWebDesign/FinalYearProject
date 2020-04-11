using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Project.Models
{
    public class ProjectUserRequests
    {
        [Key]
        public int ProjectUserRequestID { get; set; }

        //Get specific project id
        public int PublicID { get; set; }

        //Get specific user id
        public string ApplicationUserID { get; set; }

        //Virtual objects to set user and project properties
        public Projects Projects { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}