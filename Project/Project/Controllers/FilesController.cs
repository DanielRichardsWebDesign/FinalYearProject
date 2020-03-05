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

        // GET: Files/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Files files = await db.Files.FindAsync(id);
            if (files == null)
            {
                return HttpNotFound();
            }
            return View(files);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            //Get file object
            Files files = await db.Files.FindAsync(id);

            //Get file path and container from entity in database
            var filePath = files.FilePath;
            var projectContainer = files.Projects.ProjectContainerName;

            try
            {
                //Delete file from Azure storage
                await azureBlobService.DeleteAsync(projectContainer, filePath);

                //Delete from database
                db.Files.Remove(files);
                await db.SaveChangesAsync();

                return RedirectToAction("WorkStation", "Projects", new { id = files.PublicID });
            }
            catch
            {
                return View("Error");
            }           
            
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
