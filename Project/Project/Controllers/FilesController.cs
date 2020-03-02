using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class FilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Files
        public async Task<ActionResult> Index()
        {
            var files = db.Files.Include(f => f.ApplicationUser).Include(f => f.Projects);
            return View(await files.ToListAsync());
        }        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
