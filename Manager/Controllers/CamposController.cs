using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager.Controllers
{
    public class CamposController : Controller
    {
        HalconDBEntities db = new HalconDBEntities();
        // GET: Campos
        public ActionResult Index()
        {
            return View(db.Campos.ToList());
        }

        // GET: Campos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Campos/Create
        public ActionResult Create()
        {
            ViewData["Localidades"] = new SelectList(db.Localidades.ToList(), "IdLocalidad", "Nombre");
            ViewData["Estados"] = new SelectList(db.Estados.ToList(), "IdEstado", "Nombre");
            ViewData["Clientes"] = new SelectList(db.Clientes.ToList(), "IdCliente", "RazonSocial");
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
                objCampo.IdCampo = int.Parse(collection["IdCampo"]);
                objCampo.IdCliente = int.Parse(collection["IdCliente"]);
                objCampo.IdEstado = int.Parse(collection["IdEstado"]);
                objCampo.IdLocalidad = int.Parse(collection["IdLocalidad"]);
                objCampo.LatLong = (ConvertLatLonToDbGeography(double.Parse(collection["Latitud"]), double.Parse(collection["Longitud"])));
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
                objCampo.IdCampo = int.Parse(collection["IdCampo"]);
                objCampo.IdCliente = int.Parse(collection["IdCliente"]);
                objCampo.IdEstado = int.Parse(collection["IdEstado"]);
                objCampo.IdLocalidad = int.Parse(collection["IdLocalidad"]);
                objCampo.LatLong = (ConvertLatLonToDbGeography(double.Parse(collection["Latitud"]), double.Parse(collection["Longitud"])));
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
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public static System.Data.Entity.Spatial.DbGeography ConvertLatLonToDbGeography(double longitude, double latitude)
        {
            var point = string.Format("POINT({1} {0})", latitude, longitude);
            return System.Data.Entity.Spatial.DbGeography.FromText(point);
        }
    }
}
