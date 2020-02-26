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

        // GET: Files/Details/5
        public async Task<ActionResult> Details(int? id)
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

        // GET: Files/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserID = new SelectList(db.ApplicationUsers, "Id", "Email");
            ViewBag.PublicID = new SelectList(db.Projects, "PublicID", "ProjectName");
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FileID,FileName,FileType,FileSize,FilePath,DateUploaded,DateModified,PublicID,ApplicationUserID")] Files files)
        {
            if (ModelState.IsValid)
            {
                db.Files.Add(files);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserID = new SelectList(db.ApplicationUsers, "Id", "Email", files.ApplicationUserID);
            ViewBag.PublicID = new SelectList(db.Projects, "PublicID", "ProjectName", files.PublicID);
            return View(files);
        }

        // GET: Files/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.ApplicationUserID = new SelectList(db.ApplicationUsers, "Id", "Email", files.ApplicationUserID);
            ViewBag.PublicID = new SelectList(db.Projects, "PublicID", "ProjectName", files.PublicID);
            return View(files);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "FileID,FileName,FileType,FileSize,FilePath,DateUploaded,DateModified,PublicID,ApplicationUserID")] Files files)
        {
            if (ModelState.IsValid)
            {
                db.Entry(files).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserID = new SelectList(db.ApplicationUsers, "Id", "Email", files.ApplicationUserID);
            ViewBag.PublicID = new SelectList(db.Projects, "PublicID", "ProjectName", files.PublicID);
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
            Files files = await db.Files.FindAsync(id);
            db.Files.Remove(files);
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
