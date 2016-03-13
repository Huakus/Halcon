using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager.Controllers
{
    [Authorize(Roles = "Resp. Relevamiento, Resp. Trampas")]
    public class TrampasController : Controller
    {
        HalconDBEntities db = new HalconDBEntities();
        // GET: Trampas
        public ActionResult Index()
        {
            return View(db.Trampas.ToList());
        }

        public ActionResult IndexByCampo(int idCampo)
        {
            var objTrampas = (from obj in db.Trampas where obj.IdCampo == idCampo select obj).ToList<Trampas>();
            if (objTrampas == null)
            {
                return HttpNotFound();
            }
            return View(objTrampas);
        }

        // GET: Trampas/Details/5
        public ActionResult Details(int id)
        {
            var objTrampa = (from obj in db.Trampas where obj.IdTrampa == id select obj).First();
            return View(objTrampa);
        }

        // GET: Trampas/Create
        public ActionResult Create()
        {
            ViewData["Campos"] = new SelectList(db.Campos.ToList(), "IdCampo", "Calle");
            ViewData["Estados"] = new SelectList(db.Estados.ToList(), "IdEstado", "Nombre");
            return View();
        }

        // POST: Trampas/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                Trampas objTrampa = new Trampas();
                objTrampa.IdCampo = int.Parse(collection["IdCampo"]);
                objTrampa.IdEstado = int.Parse(collection["IdEstado"]);
                objTrampa.BateriaFElab = DateTime.Parse(collection["BateriaFElab"]);
                objTrampa.BateriaFVenc = DateTime.Parse(collection["BateriaFVenc"]);
                db.Trampas.Add(objTrampa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Trampas/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["Campos"] = new SelectList(db.Campos.ToList(), "IdCampo", "Calle");
            ViewData["Estados"] = new SelectList(db.Estados.ToList(), "IdEstado", "Nombre");
            ViewData["Usuarios"] = new SelectList(db.Usuarios.ToList(), "IdUsuario", "Nombre");
            var objTrampa = (from obj in db.Trampas where obj.IdTrampa == id select obj).First();
            return View(objTrampa);
        }

        // POST: Trampas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var objTrampa = (from obj in db.Trampas where obj.IdTrampa == id select obj).First();
                objTrampa.IdCampo = int.Parse(collection["IdCampo"]);
                objTrampa.IdEstado = int.Parse(collection["IdEstado"]);
                objTrampa.IdUsuario = int.Parse(collection["IdUsuario"]);
                objTrampa.BateriaFElab = DateTime.Parse(collection["BateriaFElab"]);
                objTrampa.BateriaFVenc = DateTime.Parse(collection["BateriaFVenc"]);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Trampas/Delete/5
        public ActionResult Delete(int id)
        {
            var objTrampa = (from obj in db.Trampas where obj.IdTrampa == id select obj).First();
            return View(objTrampa);
        }

        // POST: Trampas/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var objTrampa = (from obj in db.Trampas where obj.IdTrampa == id select obj).First();
                db.Trampas.Remove(objTrampa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Export(int id)
        {
            try
            {
                var objTrampa = (from obj in db.Trampas where obj.IdTrampa == id select obj).First();
                //Exporta archivo de configuración de trampa
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
