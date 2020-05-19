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
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public async Task<ActionResult> Index()
        {
            var comments = db.Comments.Include(c => c.ApplicationUser).Include(c => c.Files);
            return View(await comments.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = await db.Comments.FindAsync(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "Email");
            ViewBag.FileID = new SelectList(db.Files, "FileID", "FileName");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CommentID,Comment,DateCommented,FileID,ApplicationUserID")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comments);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "Email", comments.ApplicationUserID);
            ViewBag.FileID = new SelectList(db.Files, "FileID", "FileName", comments.FileID);
            return View(comments);
        }

        // GET: Comments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = await db.Comments.FindAsync(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "Email", comments.ApplicationUserID);
            ViewBag.FileID = new SelectList(db.Files, "FileID", "FileName", comments.FileID);
            return View(comments);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CommentID,Comment,DateCommented,FileID,ApplicationUserID")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comments).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserID = new SelectList(db.Users, "Id", "Email", comments.ApplicationUserID);
            ViewBag.FileID = new SelectList(db.Files, "FileID", "FileName", comments.FileID);
            return View(comments);
        }

        // GET: Comments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = await db.Comments.FindAsync(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Comments comments = await db.Comments.FindAsync(id);
            var fileID = comments.FileID;

            db.Comments.Remove(comments);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Files", new { id = fileID });
        }

        // POST COMMENT
        [HttpPost]
        public async Task<ActionResult> PostComment(string comment, int fileID, string userID)
        {
            try
            {
                if(comment == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var currentDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                var newComment = new Comments()
                {
                    Comment = comment,
                    DateCommented = Convert.ToDateTime(currentDateTime),
                    FileID = fileID,
                    ApplicationUserID = userID,
                };
                db.Comments.Add(newComment);
                await db.SaveChangesAsync();

                string message = "Comment Posted Successfully!";
                TempData["Message"] = message;
                return Redirect(Request.UrlReferrer.ToString());
            }
            catch
            {
                return View("Error");
            }
        }

        //EDIT COMMENT
        [HttpPost]
        public async Task<ActionResult> EditComment(string comment, int commentID, int fileID, string userID)
        {
            if(ModelState.IsValid)
            {
                var editComment = db.Comments.FirstOrDefault(c => c.CommentID == commentID);

                //Set new properties
                editComment.Comment = comment;

                //Save changes
                db.Entry(editComment).State = EntityState.Modified;
                db.SaveChanges();

                string message = "Comment Modified Successfully!";
                TempData["Message"] = message;
                return Redirect(Request.UrlReferrer.ToString());
            }
            return Redirect(Request.UrlReferrer.ToString());
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
