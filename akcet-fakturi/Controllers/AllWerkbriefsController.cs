using Data;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace akcet_fakturi.Controllers
{
    [Authorize]
    public class AllWerkbriefsController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: AllWerkbriefs
        public ActionResult AllWerkbriefs()
        {
            var model = db.Werkbriefs.ToList();

            return View(model);
        }


        public ActionResult AllUserWerkbriefs()
        {
            var userId = User.Identity.GetUserId();
            var model = db.Werkbriefs.Where(w => w.UserId == userId).ToList();

            return View(model);
        }

        public ActionResult GetWerkbrief(int IdWerkbrief)
        {
            var html = db.Werkbriefs.Find(IdWerkbrief).WerkbriefHTML;
            return Json(html);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var werkbrief = db.Werkbriefs.Find(id);
            if (werkbrief == null)
            {
                return HttpNotFound();
            }
            return View(werkbrief);
        }


        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var werkbrief = db.Werkbriefs.Find(id);

            db.Werkbriefs.Remove(werkbrief);
            db.SaveChanges();
            return RedirectToAction("AllWerkbriefs");
        }
    }
}