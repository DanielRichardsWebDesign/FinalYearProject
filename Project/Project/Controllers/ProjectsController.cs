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

            if(currentUserID == null)
            {
                return RedirectToAction("Login", "Account");
            }

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

                //Add the creator to the list of Project Users
                ProjectUsers projectUser = new ProjectUsers
                { 
                    ApplicationUserID = projects.ApplicationUserID,
                    PublicID = projects.PublicID,
                    IsAdmin = true
                };
                db.ProjectUsers.Add(projectUser);
                await db.SaveChangesAsync();

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
        public async Task<ActionResult> Edit([Bind(Include = "PublicID,ProjectName,ProjectType,ProjectDescription,DateCreated,ApplicationUserID,ProjectContainerName")] Projects projects)
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
            ViewBag.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");            

            Projects project = await db.Projects.FindAsync(id);
            return View(project);
        }

        // POST: FileUploadAsync
        [HttpPost]
        public async Task<ActionResult> UploadFileAsync(ICollection<HttpPostedFileBase> formFiles, string containerName, string publicID, string userID, DateTime currentDate)
        {                                         
            try
            {
                if (formFiles == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }               
                
                //List<string> newNames                

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

                return Redirect(Request.UrlReferrer.ToString());
            }
            catch
            {
                return View("Error");
            }            
            
        }
        
        public async Task<ActionResult> ProjectUsers(int? id)
        {
            Projects project = await db.Projects.FindAsync(id);

            ViewBag.PublicID = id;

            return View(project);
        }

        public async Task<ActionResult> AddUser(int id)
        {
            ViewBag.PublicID = id;

            List<ProjectUsers> userList = db.ProjectUsers.Where(u => u.PublicID == id).ToList();

            var userId = User.Identity.GetUserId();

            var user = db.Users.AsEnumerable().Where(u => u.Id != userId && !userList.Select(pu => pu.ApplicationUserID).Contains(u.Id));

            return View(user.ToList());
        }
        
        [HttpPost]
        public async Task<ActionResult> AddUserToProject(string userID, int publicID)
        {
            try
            {
                var newProjectUser = new ProjectUsers
                {
                    ApplicationUserID = userID,
                    PublicID = publicID,
                };
                db.ProjectUsers.Add(newProjectUser);
                db.SaveChanges();

                return RedirectToAction("ProjectUsers", "Projects", new { id = publicID });
            }
            catch
            {
                return View("Error");
            }            
        }

        public async Task<ActionResult> RemoveUser(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectUsers projectUser = await db.ProjectUsers.FindAsync(id);
            if(projectUser == null)
            {
                return HttpNotFound();
            }
            return View(projectUser);
        }

        [HttpPost]
        public async Task<ActionResult> RemoveUserFromProject(string userID, int projectID)
        {
            ProjectUsers projectUser = db.ProjectUsers.Where(p => p.ApplicationUserID == userID && p.PublicID == projectID).FirstOrDefault();
            db.ProjectUsers.Remove(projectUser);
            await db.SaveChangesAsync();

            return RedirectToAction("ProjectUsers", "Projects", new { id = projectID });
        }

        public async Task<ActionResult> ViewContributions(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectUsers projectUser = db.ProjectUsers.Where(p => p.ProjectUserID == id).FirstOrDefault();
            if(projectUser == null)
            {
                return HttpNotFound();
            }
            return View(projectUser);
        }

        public async Task<ActionResult> MyFiles(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects project = await db.Projects.FindAsync(id);
            if(project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        public async Task<ActionResult> UserRequests(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var projectUserRequests = db.Projects.Where(p => p.PublicID == id).Include(u => u.ApplicationUser).First();
            if(projectUserRequests == null)
            {
                return HttpNotFound();
            }
            return View(projectUserRequests);
        }

        public async Task<ActionResult> MemberOf()
        {
            var currentUserID = User.Identity.GetUserId();

            if(currentUserID == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var projects = db.Projects.Where(p => p.ProjectUsers.Any(pm => pm.ApplicationUserID == currentUserID) && p.ApplicationUserID != currentUserID);
            return View(await projects.ToListAsync());
        }

        public async Task<ActionResult> EditUserPrivileges(int? id)
        {
            var currentUserID = User.Identity.GetUserId();
            //var project = db.Projects.Where

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (currentUserID == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ProjectUsers projectUser = db.ProjectUsers.Find(id);            

            if(projectUser == null)
            {
                return HttpNotFound();
            }

            var projectID = projectUser.PublicID;
            var project = db.Projects.Find(projectID);
            var projectMemberLoggedIn = db.ProjectUsers.Where(p => p.PublicID == projectID && p.ApplicationUserID == currentUserID).FirstOrDefault();

            if(projectMemberLoggedIn.IsAdmin == false)
            {
                return View("Unauthorised");
            }
                      
                return View(projectUser);
            
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUserPrivileges([Bind(Include = "ProjectUserID,ApplicationUserID,PublicID,IsAdmin")] ProjectUsers projectUser)
        {
            if(ModelState.IsValid)
            {
                db.Entry(projectUser).State = EntityState.Modified;
                await db.SaveChangesAsync();

                var projectID = projectUser.PublicID;

                return RedirectToAction("ProjectUsers", "Projects", new { id = projectID });
            }
            return View(projectUser);
        }

        //Tasks view
        public async Task<ActionResult> Tasks(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects project = await db.Projects.FindAsync(id);
            if(project == null)
            {
                return HttpNotFound();
            }

            ViewBag.PublicID = project.PublicID;
            ViewBag.ApplicationUserID = User.Identity.GetUserId();

            return View(project);
        }

        //Create Task
        [HttpPost]
        public async Task<ActionResult> CreateTask(int? projectID, string userID, string taskDesc)
        {
            if(projectID == null && userID == null && taskDesc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var task = new Tasks();

            if(ModelState.IsValid)
            {
                task = new Tasks 
                {
                    PublicID = Convert.ToInt32(projectID),
                    ApplicationUserID = userID,
                    TaskDescription = taskDesc
                };
                db.Tasks.Add(task);
                await db.SaveChangesAsync();

                return RedirectToAction("Tasks", "Projects", new { id = task.PublicID });
            }

            return View(task);
        }

        //Edit Task
        public async Task<ActionResult> EditTask(int? id)
        {
            var currentUserID = User.Identity.GetUserId();

            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks task = await db.Tasks.FindAsync(id);
            if(task == null)
            {
                return HttpNotFound();
            }
            var projectMemberLoggedIn = db.ProjectUsers.Where(p => p.PublicID == task.PublicID && p.ApplicationUserID == currentUserID).FirstOrDefault();
            if(projectMemberLoggedIn == null)
            {
                return View("Unauthorised");
            }

            return View(task);
        }

        //Update Task: POST
        public async Task<ActionResult> UpdateTask([Bind(Include = "TaskID,PublicID,ApplicationUserID,TaskDescription,IsComplete")] Tasks task)
        {
            if(ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Tasks", "Projects", new { id = task.PublicID });
            }

            return View(task);
        }

        //My Tasks: GET
        public async Task<ActionResult> MyTasks(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            var userId = User.Identity.GetUserId();

            ViewBag.PublicID = project.PublicID;
            ViewBag.ApplicationUserID = User.Identity.GetUserId();
            ViewBag.ProjectUserID = db.ProjectUsers.Where(p => p.PublicID == project.PublicID && p.ApplicationUserID == userId);

            if(User.Identity.GetUserId() == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(project);
        }

        //Awaiting Membership Approval Projects: GET
        public async Task<ActionResult> AwaitingApproval()
        {
            if(User.Identity.GetUserId() == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var projects = db.Projects.Include(p => p.ApplicationUser);
            var loggedInUser = User.Identity.GetUserId();

            return View(await db.Projects.Where(p => p.ProjectUserRequests.Any(r => r.ApplicationUserID == loggedInUser)).ToListAsync());
        }
    }
}
