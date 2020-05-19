using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Runtime.Serialization;

namespace Project.Models
{
    
    public class Files
    {
        [Key]
        public int FileID { get; set; }

        [Required]
        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Required]
        [Display(Name = "File Type")]
        public string FileType { get; set; }

        [Required]
        [Display(Name = "File Size")]
        public string FileSize { get; set; }

        //This will link to an Azure Blob which holds the file, allowing for download.
        [Required]
        public string FilePath { get; set; }

        //Gets the specific date and time a file has been uploaded
        [DataType(DataType.DateTime), Display(Name = "Date Uploaded"), Column(TypeName = "DateTime"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateUploaded { get; set; }

        [DataType(DataType.DateTime), Display(Name = "Date Modified"), Column(TypeName = "DateTime"), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateModified { get; set; }

        //Get specific Project by ID
        public int PublicID { get; set; }

        //Get specific User by ID
        public string ApplicationUserID { get; set; }        

        //Virtual Objects to access User and Project properties
        [JsonIgnore]
        public virtual Projects Projects { get; set; }
        
        public virtual ApplicationUser ApplicationUser { get; set; }

        //Collection of comments for many relationship with comments
        [JsonIgnore]
        public virtual ICollection<Comments> Comments { get; set; }
    }
}