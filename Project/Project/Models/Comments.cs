using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Project.Models
{
    public class Comments
    {
        [Key]
        public int CommentID { get; set; }
        [Required]
        public string Comment { get; set; }
        [DataType(DataType.DateTime), Display(Name = "Date Commented"), Column(TypeName = "DateTime"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateCommented { get; set; }

        //Get specific File by ID
        public int FileID { get; set; }

        //Get specific user by ID
        public string ApplicationUserID { get; set; }

        //Virtual Objects to access User and Project properties
        public virtual Files Files { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}