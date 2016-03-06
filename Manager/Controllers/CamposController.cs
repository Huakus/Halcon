using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Spatial;

namespace Manager.Controllers
{
    [Authorize(Roles = "Resp. Relevamiento")]
    public class CamposController : Controller
    {
        HalconDBEntities db = new HalconDBEntities();
        double Latitud = 0;
        double Longitud = 0;

        // GET: Campos
        public ActionResult Index()
        {
            Latitud = 0;
            Longitud = 0;
            return View(db.Campos.ToList());
        }

        // GET: Campos/Details/5
        public ActionResult Details(int id)
        {
            var objCampo = (from obj in db.Campos where obj.IdCampo == id select obj).First();
            return View(objCampo);
        }

        // GET: Campos/Create
        public ActionResult Create()
        {
            ViewData["Clientes"] = new SelectList(db.Clientes.ToList(), "IdCliente", "Nombre");
            ViewData["Estados"] = new SelectList(db.Estados.ToList(), "IdEstado", "Nombre");
            ViewData["Localidades"] = new SelectList(db.Localidades.ToList(), "IdLocalidad", "Nombre");
            return View();
        }

        // POST: Campos/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                Campos objCampo = new Campos();

                objCampo.Calle = collection["Calle"];
                objCampo.IdCliente = int.Parse(collection["IdCliente"]);
                objCampo.IdEstado = int.Parse(collection["IdEstado"]);
                objCampo.IdLocalidad = int.Parse(collection["IdLocalidad"]);
                objCampo.LatLong = DbGeography.FromText(string.Format("POINT({0} {1})", collection["LatLong.Latitude"], collection["LatLong.Longitude"]));
                objCampo.Numero = int.Parse(collection["Numero"]);
                objCampo.Observaciones = collection["Observaciones"];
                db.Campos.Add(objCampo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Campos/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["Clientes"] = new SelectList(db.Clientes.ToList(), "IdCliente", "Nombre");
            ViewData["Estados"] = new SelectList(db.Estados.ToList(), "IdEstado", "Nombre");
            ViewData["Localidades"] = new SelectList(db.Localidades.ToList(), "IdLocalidad", "Nombre");
            ViewBag.Latitud = Latitud;
            ViewBag.Longitud = Longitud;
            var objCampo = (from obj in db.Campos where obj.IdCampo == id select obj).First();
            return View(objCampo);
        }

        // POST: Campos/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var objCampo = (from obj in db.Campos where obj.IdCampo == id select obj).First();
                objCampo.Calle = collection["Calle"];
                objCampo.IdCliente = int.Parse(collection["IdCliente"]);
                objCampo.IdEstado = int.Parse(collection["IdEstado"]);
                objCampo.IdLocalidad = int.Parse(collection["IdLocalidad"]);
                objCampo.LatLong = DbGeography.FromText(string.Format("POINT({0} {1})", collection["LatLong.Longitude"], collection["LatLong.Latitude"]));
                objCampo.Numero = int.Parse(collection["Numero"]);
                objCampo.Observaciones = collection["Observaciones"];
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Campos/Delete/5
        public ActionResult Delete(int id)
        {
            var objCampo = (from obj in db.Campos where obj.IdCampo == id select obj).First();
            return View(objCampo);
        }

        // POST: Campos/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var objCampo = (from obj in db.Campos where obj.IdCampo == id select obj).First();
                db.Campos.Remove(objCampo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
