using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Manager.Models;
using System.Web.Routing;

namespace Manager.Controllers
{
    [Authorize(Roles = "Resp. Relevamiento")]
    public class AlarmasController : Controller
    {
        private HalconDBEntities db = new HalconDBEntities();

        // GET: Alarmas
        public ActionResult Index()
        {
            var alarmas = db.Alarmas.Include(a => a.Relevamientos).Include(a => a.Umbrales);
            return View(alarmas.ToList());
        }

        public ActionResult IndexByUmbral(int IdUmbral)
        {
            var objAlarma = (from obj in db.Alarmas where obj.IdUmbral == IdUmbral select obj).ToList<Alarmas>();
            if (objAlarma == null)
            {
                return HttpNotFound();
            }
            return View(objAlarma);
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
        public ActionResult Create([Bind(Include = "IdAlarma,IdRelevamiento,IdUmbral,ValorMaximo,Observaciones")] Alarmas alarmas)
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
        public ActionResult Edit([Bind(Include = "IdAlarma,IdRelevamiento,IdUmbral,ValorMaximo,Observaciones")] Alarmas alarmas)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GenerarAlarmas(int IdRelevamiento)
        {
            var objAlarmasViejas = (from objAlarmas in db.Alarmas where objAlarmas.IdRelevamiento == IdRelevamiento select objAlarmas);

            foreach (Alarmas objAlarma in objAlarmasViejas)
            {
                db.Alarmas.Remove(objAlarma);
            }


            var objAlarmasManuales =
                (from objLecturasManuales in db.LecturasManuales
                 from objUmbrales in db.Umbrales
                 where objLecturasManuales.IdInsecto == objUmbrales.IdInsecto
                 && objLecturasManuales.Cantidad > objUmbrales.ValorMaximo && objLecturasManuales.IdRelevamiento == IdRelevamiento
                 select new AlarmasMemoria()
                 {
                     IdRelevamiento = objLecturasManuales.IdRelevamiento,
                     IdUmbral = objUmbrales.IdUmbral,
                     IdInsecto = objLecturasManuales.IdInsecto,
                     ValorMaximo = objUmbrales.ValorMaximo,
                     IdProvincia = objUmbrales.IdProvincia,
                     IdMes = objUmbrales.IdMes,
                     Cantidad = objLecturasManuales.Cantidad,
                     Tipo = "MANUAL",
                     IdEstado = 2,
                     Observaciones = ""
                 }).ToList<AlarmasMemoria>();

            foreach (AlarmasMemoria obj in objAlarmasManuales)
            {
                Alarmas objAlarma = new Alarmas();
                objAlarma.IdRelevamiento = obj.IdRelevamiento;
                objAlarma.IdUmbral = obj.IdUmbral;
                objAlarma.IdInsecto = obj.IdInsecto;
                objAlarma.ValorMaximo = obj.ValorMaximo;
                objAlarma.IdProvincia = obj.IdProvincia;
                objAlarma.IdMes = obj.IdMes;
                objAlarma.Cantidad = obj.Cantidad;
                objAlarma.Tipo = obj.Tipo;
                objAlarma.IdEstado = obj.IdEstado;
                objAlarma.Observaciones = obj.Observaciones;

                db.Alarmas.Add(objAlarma);
            }
            db.SaveChanges();

            return RedirectToAction("Index");

            //Redirect("https://localhost:44300/Relevamientos/Validar/" + IdRelevamiento.ToString());
            //return RedirectToAction("Validar", new RouteValueDictionary(new { controller = "Relevamientos", action = "Validar", IdRelevamiento = IdRelevamiento }));
            //return RedirectToAction("Validar", "Relevamientos", new { IdRelevamiento = IdRelevamiento });
        }

        public class AlarmasMemoria
        {
            public int IdRelevamiento { get; set; }
            public int IdUmbral { get; set; }
            public int IdInsecto { get; set; }
            public long ValorMaximo { get; set; }
            public int IdProvincia { get; set; }
            public int IdMes { get; set; }
            public long Cantidad { get; set; }
            public string Tipo { get; set; }
            public int IdEstado { get; set; }
            public string Observaciones { get; set; }
        }
    }
}
