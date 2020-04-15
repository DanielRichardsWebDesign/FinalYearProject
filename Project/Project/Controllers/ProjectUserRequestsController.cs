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
    public class ProjectUserRequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // POST: RequestMembership
        public async Task<ActionResult> RequestMembership(int projectID, string userID)
        {
            if(userID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if(ModelState.IsValid)
            {
                ProjectUserRequests projectUserRequest = new ProjectUserRequests
                {
                    PublicID = projectID,
                    ApplicationUserID = userID
                };
                db.ProjectUserRequests.Add(projectUserRequest);
                await db.SaveChangesAsync();
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        // GET: ProjectUserRequests/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectUserRequests projectUserRequests = await db.ProjectUserRequests.FindAsync(id);
            if (projectUserRequests == null)
            {
                return HttpNotFound();
            }
            return View(projectUserRequests);
        }

        // POST: ProjectUserRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProjectUserRequests projectUserRequests = await db.ProjectUserRequests.FindAsync(id);
            db.ProjectUserRequests.Remove(projectUserRequests);
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
