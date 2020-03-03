using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using Project.Models;
using Project.Services;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Project.Controllers
{
    public class FilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IAzureBlobService azureBlobService;

        public FilesController() :this(new AzureBlobService())
        {

        }

        public FilesController(IAzureBlobService azureBlobService)
        {
            this.azureBlobService = azureBlobService;
        }

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
