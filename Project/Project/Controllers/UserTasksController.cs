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
    public class UserTasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserTasks
        public async Task<ActionResult> Index()
        {
            var userTasks = db.UserTasks.Include(u => u.ProjectUsers).Include(u => u.Tasks);
            return View(await userTasks.ToListAsync());
        }

        // GET: UserTasks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTasks userTasks = await db.UserTasks.FindAsync(id);
            if (userTasks == null)
            {
                return HttpNotFound();
            }
            return View(userTasks);
        }

        // GET: UserTasks/Create
        public ActionResult Create()
        {
            ViewBag.ProjectUserID = new SelectList(db.ProjectUsers, "ProjectUserID", "ApplicationUserID");
            ViewBag.TaskID = new SelectList(db.Tasks, "TaskID", "ApplicationUserID");
            return View();
        }

        // POST: UserTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TaskID,ProjectUserID")] UserTasks userTasks)
        {
            if (ModelState.IsValid)
            {
                db.UserTasks.Add(userTasks);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectUserID = new SelectList(db.ProjectUsers, "ProjectUserID", "ApplicationUserID", userTasks.ProjectUserID);
            ViewBag.TaskID = new SelectList(db.Tasks, "TaskID", "ApplicationUserID", userTasks.TaskID);
            return View(userTasks);
        }

        // GET: UserTasks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTasks userTasks = await db.UserTasks.FindAsync(id);
            if (userTasks == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectUserID = new SelectList(db.ProjectUsers, "ProjectUserID", "ApplicationUserID", userTasks.ProjectUserID);
            ViewBag.TaskID = new SelectList(db.Tasks, "TaskID", "ApplicationUserID", userTasks.TaskID);
            return View(userTasks);
        }

        // POST: UserTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TaskID,ProjectUserID")] UserTasks userTasks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userTasks).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectUserID = new SelectList(db.ProjectUsers, "ProjectUserID", "ApplicationUserID", userTasks.ProjectUserID);
            ViewBag.TaskID = new SelectList(db.Tasks, "TaskID", "ApplicationUserID", userTasks.TaskID);
            return View(userTasks);
        }

        // GET: UserTasks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTasks userTasks = await db.UserTasks.FindAsync(id);
            if (userTasks == null)
            {
                return HttpNotFound();
            }
            return View(userTasks);
        }

        // POST: UserTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UserTasks userTasks = await db.UserTasks.FindAsync(id);
            db.UserTasks.Remove(userTasks);
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

        //Assign Users view
        public async Task<ActionResult> AssignUsers(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            //var userTask = new UserTasks();
            var task = db.Tasks.FirstOrDefault(t => t.TaskID == id);            
            if(task == null)
            {
                return HttpNotFound();
            }

            //ViewBag for selecting users to assign
            ViewBag.TaskID = task.TaskID;
            ViewBag.TaskDescription = task.TaskDescription;
            ViewBag.ProjectUsers = db.ProjectUsers.Where(p => p.PublicID == task.PublicID && !db.UserTasks.Any(t => t.ProjectUserID == p.ProjectUserID)).Include(p => p.User).ToList();
            ViewBag.AssignedUsers = db.UserTasks.Where(p => p.TaskID == task.TaskID).Include(u => u.ProjectUsers.User).ToList();

            return View();

        }

        //Add Users to Task post method
        [HttpPost]
        public async Task<ActionResult> AddUserToTask(int? TaskID, int? []projectUserIDs, int? []assignedUserIDs)
        {
            Tasks task = await db.Tasks.FindAsync(TaskID);
            if(task == null)
            {
                return HttpNotFound();
            }

            if(projectUserIDs != null)
            {
                //Loop through each UserIDs and add to UserTasks table
                foreach (var projectUserID in projectUserIDs)
                {
                    UserTasks userTask = new UserTasks();
                    userTask.TaskID = Convert.ToInt32(TaskID);
                    userTask.ProjectUserID = Convert.ToInt32(projectUserID);

                    //Query for checking if entity exists
                    var userTaskQuery = db.UserTasks.SingleOrDefault(u => u.TaskID == userTask.TaskID && u.ProjectUserID == userTask.ProjectUserID);

                    //Checks if entity exists in UserTasks table and removes it if it does
                    if (userTaskQuery != null)
                    {
                        db.UserTasks.Remove(userTaskQuery);
                    }

                    //Add to database
                    db.UserTasks.Add(userTask);
                    db.SaveChanges();
                }
            }            

            if(assignedUserIDs != null)
            {
                //Loop through each UserID for AssignedUsers array and remove the checked ones
                foreach (var projectUserID in assignedUserIDs)
                {
                    UserTasks userTask = new UserTasks();
                    userTask.TaskID = Convert.ToInt32(TaskID);
                    userTask.ProjectUserID = Convert.ToInt32(projectUserID);

                    //Check if entity exists in UserTasks
                    var assignedUserQuery = db.UserTasks.SingleOrDefault(u => u.TaskID == userTask.TaskID && u.ProjectUserID == userTask.ProjectUserID);

                    if (assignedUserQuery != null)
                    {
                        db.UserTasks.Remove(assignedUserQuery);
                    }
                    db.SaveChanges();
                }
            }            

            return RedirectToAction("Tasks", "Projects", new { id = task.PublicID });
        }      

    }
}
