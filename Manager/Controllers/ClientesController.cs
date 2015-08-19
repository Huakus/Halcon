using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager.Controllers
{
    public class ClientesController : Controller
    {
        HalconDBEntities db = new HalconDBEntities();
        // GET: Clientes
        public ActionResult Index()
        {
            return View(db.Clientes.ToList());
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            ViewData["Estados"] = new SelectList(db.Estados.ToList(), "IdEstado", "Nombre");
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                Clientes objCliente = new Clientes();
                objCliente.Apellido = collection["Apellido"];
                objCliente.DNI = int.Parse(collection["DNI"]);
                objCliente.Email = collection["Email"];
                objCliente.IdEstado = int.Parse(collection["IdEstado"]);
                objCliente.Nombre = collection["Nombre"];
                objCliente.Observaciones = collection["Observaciones"];
                db.Clientes.Add(objCliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["Estados"] = new SelectList(db.Estados.ToList(), "IdEstado", "Nombre");
            var objCliente = (from obj in db.Clientes where obj.IdCliente == id select obj).First();
            return View(objCliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var objCliente = (from obj in db.Clientes where obj.IdCliente == id select obj).First();
                objCliente.Apellido = collection["Apellido"];
                objCliente.DNI = int.Parse(collection["DNI"]);
                objCliente.Email = collection["Email"];
                objCliente.IdEstado = int.Parse(collection["IdEstado"]);
                objCliente.Nombre = collection["Nombre"];
                objCliente.Observaciones = collection["Observaciones"];
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int id)
        {
            var objCliente = (from obj in db.Clientes where obj.IdCliente == id select obj).First();
            return View(objCliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var objCliente = (from obj in db.Clientes where obj.IdCliente == id select obj).First();
                db.Clientes.Remove(objCliente);
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
