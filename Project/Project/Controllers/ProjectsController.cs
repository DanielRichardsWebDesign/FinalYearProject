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
using Project.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;

namespace Project.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IAzureBlobService azureBlobService;

        //Create an instance of AzureBlobService class, to access the interface methods
        public ProjectsController() :this(new AzureBlobService())
        {

        }

        public ProjectsController(IAzureBlobService azureBlobService)
        {
            this.azureBlobService = azureBlobService;
        }
                
        // GET: Projects
        public async Task<ActionResult> Index()
        {

            var currentUserID = User.Identity.GetUserId();
            var projects = db.Projects.Include(p => p.ApplicationUser);
            return View(await projects.Where(p => p.ApplicationUserID.Equals(currentUserID)).ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = await db.Projects.FindAsync(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.UserID = User.Identity.GetUserId().ToString();
            ViewBag.CurrentDate = DateTime.Today.ToString("dd/MM/yyyy");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PublicID,ProjectName,ProjectType,ProjectDescription,DateCreated,ApplicationUserID,ProjectContainerName")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(projects);
                await db.SaveChangesAsync();

                //Create Azure Blob Container with ProjectContainerName
                string containerName = projects.ProjectContainerName;                

                //Create Container with ProjectContainerName
                await azureBlobService.CreateBlobContainer(containerName);

                return RedirectToAction("Index");
            }

            ViewBag.UserID = User.Identity.GetUserId().ToString();
            ViewBag.CurrentDate = DateTime.Now.ToString();
            return View(projects);
        }

        // GET: Projects/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = await db.Projects.FindAsync(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "Email", projects.ApplicationUserID);
            ViewBag.DateCreated = db.Projects.Find(id).DateCreated.ToString("dd/MM/yyyy");
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PublicID,ProjectName,ProjectType,ProjectDescription,DateCreated,ApplicationUserID")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projects).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "Email", projects.ApplicationUserID);
            return View(projects);
        }

        // GET: Projects/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = await db.Projects.FindAsync(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Projects projects = await db.Projects.FindAsync(id);
            db.Projects.Remove(projects);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Projects WorkStation Controller
        public async Task<ActionResult> WorkStation(int? id)
        {
            ViewBag.PublicID = id.ToString();           
            ViewBag.UserID = User.Identity.GetUserId();
            ViewBag.CurrentDate = DateTime.Today.ToString("dd/MM/yyyy HH:mm");

            Projects project = await db.Projects.FindAsync(id);
            return View(project);
        }

        // POST: FileUploadAsync
        [HttpPost]
        public async Task<ActionResult> UploadFileAsync(List<HttpPostedFileBase> formFiles, string containerName, string publicID, string userID, DateTime currentDate)
        {                                         
            try
            {
                if (formFiles == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //Upload to Azure Blob Container
                await azureBlobService.UploadAsync(formFiles, containerName);

                //Create a new file entity for each form file in formFiles list
                foreach (var formFile in formFiles)
                {
                    string blobUri = await azureBlobService.GetBlobUri(formFile.FileName, containerName);

                    //Create new file entity
                    var file = new Files()
                    {
                        FileName = formFile.FileName,
                        FileType = formFile.ContentType,
                        FileSize = formFile.ContentLength.ToString(),
                        FilePath = blobUri,
                        DateUploaded = currentDate,
                        DateModified = currentDate,
                        PublicID = Int32.Parse(publicID),
                        ApplicationUserID = userID
                    };
                    db.Files.Add(file);
                    await db.SaveChangesAsync();
                }

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View("Error");
            }            
            
        }       
        
    }
}
