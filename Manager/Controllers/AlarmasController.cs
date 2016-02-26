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
    public class AlarmasController : Controller
    {
        private HalconDBEntities db = new HalconDBEntities();

        // GET: Alarmas
        public ActionResult Index()
        {
            var alarmas = db.Alarmas.Include(a => a.Relevamientos).Include(a => a.Umbrales);
            return View(alarmas.ToList());
        }

        // GET: Alarmas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alarmas alarmas = db.Alarmas.Find(id);
            if (alarmas == null)
            {
                return HttpNotFound();
            }
            return View(alarmas);
        }

        // GET: Alarmas/Create
        public ActionResult Create()
        {
            ViewBag.IdRelevamiento = new SelectList(db.Relevamientos, "IdRelevamiento", "Observaciones");
            ViewBag.IdUmbral = new SelectList(db.Umbrales, "IdUmbral", "Observaciones");
            return View();
        }

        // POST: Alarmas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAlarma,IdRelevamiento,IdUmbral,Tipo,Observaciones,Cantidad,IdInsecto,ValorMaximo,IdProvincia,IdMes")] Alarmas alarmas)
        {
            if (ModelState.IsValid)
            {
                db.Alarmas.Add(alarmas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdRelevamiento = new SelectList(db.Relevamientos, "IdRelevamiento", "Observaciones", alarmas.IdRelevamiento);
            ViewBag.IdUmbral = new SelectList(db.Umbrales, "IdUmbral", "Observaciones", alarmas.IdUmbral);
            return View(alarmas);
        }

        // GET: Alarmas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alarmas alarmas = db.Alarmas.Find(id);
            if (alarmas == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdRelevamiento = new SelectList(db.Relevamientos, "IdRelevamiento", "Observaciones", alarmas.IdRelevamiento);
            ViewBag.IdUmbral = new SelectList(db.Umbrales, "IdUmbral", "Observaciones", alarmas.IdUmbral);
            return View(alarmas);
        }

        // POST: Alarmas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAlarma,IdRelevamiento,IdUmbral,Tipo,Observaciones,Cantidad,IdInsecto,ValorMaximo,IdProvincia,IdMes")] Alarmas alarmas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alarmas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdRelevamiento = new SelectList(db.Relevamientos, "IdRelevamiento", "Observaciones", alarmas.IdRelevamiento);
            ViewBag.IdUmbral = new SelectList(db.Umbrales, "IdUmbral", "Observaciones", alarmas.IdUmbral);
            return View(alarmas);
        }

        // GET: Alarmas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alarmas alarmas = db.Alarmas.Find(id);
            if (alarmas == null)
            {
                return HttpNotFound();
            }
            return View(alarmas);
        }

        // POST: Alarmas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Alarmas alarmas = db.Alarmas.Find(id);
            db.Alarmas.Remove(alarmas);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Confirmar(int? id)
        {
            Alarmas alarmas = db.Alarmas.Find(id);
            alarmas.IdEstado = 1;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Desestimar(int? id)
        {
            Alarmas alarmas = db.Alarmas.Find(id);
            alarmas.IdEstado = 2;
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
