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
    public class ProjectUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProjectUsers
        public async Task<ActionResult> Index()
        {
            return View(await db.ProjectUsers.ToListAsync());
        }

        // GET: ProjectUsers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectUsers projectUsers = await db.ProjectUsers.FindAsync(id);
            if (projectUsers == null)
            {
                return HttpNotFound();
            }
            return View(projectUsers);
        }

        // GET: ProjectUsers/Create
        public ActionResult AddUser(int projectID)
        {
            var projectUsers = db.ProjectUsers.Select(p => p.ApplicationUserID);

            var returnTo = db.Users.Where(u => !projectUsers.Contains(u.Id)).ToList();

            return View(returnTo);
        }

        // POST: ProjectUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ApplicationUserID,PublicID")] ProjectUsers projectUsers)
        {
            if (ModelState.IsValid)
            {
                db.ProjectUsers.Add(projectUsers);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(projectUsers);
        }

        // GET: ProjectUsers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectUsers projectUsers = await db.ProjectUsers.FindAsync(id);
            if (projectUsers == null)
            {
                return HttpNotFound();
            }
            return View(projectUsers);
        }

        // POST: ProjectUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ApplicationUserID,PublicID")] ProjectUsers projectUsers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectUsers).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(projectUsers);
        }

        // GET: ProjectUsers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectUsers projectUsers = await db.ProjectUsers.FindAsync(id);
            if (projectUsers == null)
            {
                return HttpNotFound();
            }
            return View(projectUsers);
        }

        // POST: ProjectUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ProjectUsers projectUsers = await db.ProjectUsers.FindAsync(id);
            db.ProjectUsers.Remove(projectUsers);
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
