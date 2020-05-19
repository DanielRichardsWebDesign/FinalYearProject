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
    public class TasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tasks
        public async Task<ActionResult> Index()
        {
            var tasks = db.Tasks.Include(t => t.ApplicationUser).Include(t => t.Projects);
            return View(await tasks.ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = await db.Tasks.FindAsync(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "Email");
            ViewBag.PublicID = new SelectList(db.Projects, "PublicID", "ProjectName");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TaskID,PublicID,ApplicationUserID,TaskDescription,IsComplete")] Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(tasks);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "Email", tasks.ApplicationUserID);
            ViewBag.PublicID = new SelectList(db.Projects, "PublicID", "ProjectName", tasks.PublicID);
            return View(tasks);
        }

        // GET: Tasks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = await db.Tasks.FindAsync(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "Email", tasks.ApplicationUserID);
            ViewBag.PublicID = new SelectList(db.Projects, "PublicID", "ProjectName", tasks.PublicID);
            return View(tasks);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TaskID,PublicID,ApplicationUserID,TaskDescription,IsComplete")] Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tasks).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "Email", tasks.ApplicationUserID);
            ViewBag.PublicID = new SelectList(db.Projects, "PublicID", "ProjectName", tasks.PublicID);
            return View(tasks);
        }

        // GET: Tasks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = await db.Tasks.FindAsync(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tasks tasks = await db.Tasks.FindAsync(id);
            db.Tasks.Remove(tasks);
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
    }
}
