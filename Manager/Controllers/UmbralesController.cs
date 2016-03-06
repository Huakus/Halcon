using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Manager.Models;

namespace Manager.Controllers
{
    [Authorize(Roles = "Resp. Relevamiento")]
    public class UmbralesController : Controller
    {
        private HalconDBEntities db = new HalconDBEntities();

        // GET: Umbrales
        public ActionResult Index()
        {
            var umbrales = db.Umbrales.Include(u => u.Insectos);
            return View(umbrales.ToList());
        }

        // GET: Umbrales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umbrales umbrales = db.Umbrales.Find(id);
            if (umbrales == null)
            {
                return HttpNotFound();
            }
            return View(umbrales);
        }

        // GET: Umbrales/Create
        public ActionResult Create()
        {
            ViewBag.IdInsecto = new SelectList(db.Insectos, "IdInsecto", "NombreCientifico");
            return View();
        }

        // POST: Umbrales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUmbral,IdInsecto,ValorMaximo")] Umbrales umbrales)
        {
            if (ModelState.IsValid)
            {
                db.Umbrales.Add(umbrales);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdInsecto = new SelectList(db.Insectos, "IdInsecto", "NombreCientifico", umbrales.IdInsecto);
            return View(umbrales);
        }

        // GET: Umbrales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umbrales umbrales = db.Umbrales.Find(id);
            if (umbrales == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdInsecto = new SelectList(db.Insectos, "IdInsecto", "NombreCientifico", umbrales.IdInsecto);
            return View(umbrales);
        }

        // POST: Umbrales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUmbral,IdInsecto,ValorMaximo")] Umbrales umbrales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(umbrales).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdInsecto = new SelectList(db.Insectos, "IdInsecto", "NombreCientifico", umbrales.IdInsecto);
            return View(umbrales);
        }

        // GET: Umbrales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Umbrales umbrales = db.Umbrales.Find(id);
            if (umbrales == null)
            {
                return HttpNotFound();
            }
            return View(umbrales);
        }

        // POST: Umbrales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Umbrales umbrales = db.Umbrales.Find(id);
            db.Umbrales.Remove(umbrales);
            db.SaveChanges();
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
