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
    public class MantenimientosController : Controller
    {
        private HalconDBEntities db = new HalconDBEntities();

        // GET: Mantenimientos
        public ActionResult Index()
        {
            var mantenimientos = db.Mantenimientos.Include(m => m.Trampas).Include(m => m.Usuarios);
            return View(mantenimientos.ToList());
        }

        // GET: Mantenimientos/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mantenimientos mantenimientos = db.Mantenimientos.Find(id);
            if (mantenimientos == null)
            {
                return HttpNotFound();
            }
            return View(mantenimientos);
        }

        // GET: Mantenimientos/Create
        public ActionResult Create()
        {
            ViewBag.IdTrampa = new SelectList(db.Trampas, "IdTrampa", "IdTrampa");
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre");
            return View();
        }

        // POST: Mantenimientos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdMantenimiento,IdTrampa,IdUsuario,IdTipoMantenimiento,FechaMantenimiento,Observaciones")] Mantenimientos mantenimientos)
        {
            if (ModelState.IsValid)
            {
                db.Mantenimientos.Add(mantenimientos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdTrampa = new SelectList(db.Trampas, "IdTrampa", "IdTrampa", mantenimientos.IdTrampa);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", mantenimientos.IdUsuario);
            return View(mantenimientos);
        }

        // GET: Mantenimientos/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mantenimientos mantenimientos = db.Mantenimientos.Find(id);
            if (mantenimientos == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdTrampa = new SelectList(db.Trampas, "IdTrampa", "IdTrampa", mantenimientos.IdTrampa);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", mantenimientos.IdUsuario);
            return View(mantenimientos);
        }

        // POST: Mantenimientos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMantenimiento,IdTrampa,IdUsuario,IdTipoMantenimiento,FechaMantenimiento,Observaciones")] Mantenimientos mantenimientos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mantenimientos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdTrampa = new SelectList(db.Trampas, "IdTrampa", "IdTrampa", mantenimientos.IdTrampa);
            ViewBag.IdUsuario = new SelectList(db.Usuarios, "IdUsuario", "Nombre", mantenimientos.IdUsuario);
            return View(mantenimientos);
        }

        // GET: Mantenimientos/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mantenimientos mantenimientos = db.Mantenimientos.Find(id);
            if (mantenimientos == null)
            {
                return HttpNotFound();
            }
            return View(mantenimientos);
        }

        // POST: Mantenimientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Mantenimientos mantenimientos = db.Mantenimientos.Find(id);
            db.Mantenimientos.Remove(mantenimientos);
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
