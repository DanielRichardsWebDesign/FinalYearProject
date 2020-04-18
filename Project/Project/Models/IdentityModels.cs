using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Project.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //Virtual DbSet to interact with the generated code created by Identity
        public virtual ICollection<Projects> Projects { get; set; }
        public virtual ICollection<Files> Files { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<ProjectUserRequests> ProjectUserRequests { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    //Application Role class
    public class ApplicationRole : IdentityRole
    {

    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{            
                   
        //}

        public virtual DbSet<Projects> Projects { get; set; }
        //public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Project.Models.Files> Files { get; set; }

        public System.Data.Entity.DbSet<Project.Models.ProjectUsers> ProjectUsers { get; set; }

        public System.Data.Entity.DbSet<Project.Models.Comments> Comments { get; set; }

        public System.Data.Entity.DbSet<Project.Models.ProjectUserRequests> ProjectUserRequests { get; set; }

        public System.Data.Entity.DbSet<Project.Models.Tasks> Tasks { get; set; }

        //public System.Data.Entity.DbSet<Project.Models.ApplicationUser> ApplicationUsers { get; set; }

        //public System.Data.Entity.DbSet<Project.Models.ApplicationUser> ApplicationUsers { get; set; }

        //public System.Data.Entity.DbSet<Project.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}