using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using Project.Models;
using Project.Services;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Cors;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

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

        // GET: Files/Details/5        
        public async Task<ActionResult> Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Files files = await db.Files.FindAsync(id);
            if(files == null)
            {
                return HttpNotFound();
            }
            return View(files);
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

        // DOWNLOAD        
        public async Task<ActionResult> DownloadFile(int id)
        {
            //Get file object
            Files file = db.Files.Find(id);           

            //Get file from blob container
            var blobStream = await azureBlobService.DownloadAsync(file.FileName, file.Projects.ProjectContainerName);            

            //Return file to browser - Will appear as download.
            return File(blobStream, file.FileType, file.FileName);
        }

        // DOWNLOAD REPOSITORY
        public async Task<ActionResult> DownloadRepository(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Get repository container
            Projects project = db.Projects.Find(id);            
            if(project == null)
            {
                return HttpNotFound();
            }

            var containerName = project.ProjectContainerName;

            //Download repository Azure method
            var blobList = await azureBlobService.DownloadRepositoryAsync(containerName);

            //Create zip file for all the blobs in the repository
            using(var zipOutputStream = new ZipOutputStream(Response.OutputStream))
            {
                foreach(var blob in blobList)
                {
                    zipOutputStream.SetLevel(0);
                    var entry = new ZipEntry(blob.Name);
                    zipOutputStream.PutNextEntry(entry);
                    blob.DownloadToStream(zipOutputStream);
                }
                zipOutputStream.Finish();
                zipOutputStream.Close();
            }
            Response.BufferOutput = false;
            Response.AddHeader("Content-Disposition", "attatchment; filename= " + project.ProjectName + ".zip");
            Response.Flush();
            Response.End();

            return null;



            //return File(zipStream, "application/zip", project.ProjectName);
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
