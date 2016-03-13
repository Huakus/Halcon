using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Manager.Controllers
{
    [Authorize(Roles = "Resp. Relevamiento")]
    public class UsuariosController : Controller
    {
        HalconDBEntities db = new HalconDBEntities();
        // GET: Usuarios
        public ActionResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        // GET: Usuarios/Details/5
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

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            ViewData["Estados"] = new SelectList(db.Estados.ToList(), "IdEstado", "Nombre");
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                Usuarios objUsuario = new Usuarios();
                objUsuario.Apellido = collection["Apellido"];
                objUsuario.DNI = long.Parse(collection["DNI"]);
                objUsuario.Email = collection["Email"];
                objUsuario.IdEstado = int.Parse(collection["IdEstado"]);
                objUsuario.IdRol = int.Parse(collection["IdRol"]);
                objUsuario.Nombre = collection["Nombre"];
                objUsuario.NombreUsuario = collection["NombreUsuario"];
                objUsuario.Observaciones = collection["Observaciones"];
                objUsuario.Password = collection["Password"];
                db.Usuarios.Add(objUsuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["Estados"] = new SelectList(db.Estados.ToList(), "IdEstado", "Nombre");
            ViewData["Roles"] = new SelectList(db.Roles.ToList(), "IdRol", "Nombre");
            var objUsuario = (from obj in db.Usuarios where obj.IdUsuario == id select obj).First();
            return View(objUsuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var objUsuario = (from obj in db.Usuarios where obj.IdUsuario == id select obj).First();
                objUsuario.Apellido = collection["Apellido"];
                objUsuario.DNI = int.Parse(collection["DNI"]);
                objUsuario.Email = collection["Email"];
                objUsuario.IdEstado = int.Parse(collection["IDEstado"]);
                objUsuario.IdRol = int.Parse(collection["IdRol"]);
                objUsuario.Nombre = collection["Nombre"];
                objUsuario.NombreUsuario = collection["NombreUsuario"];
                objUsuario.Observaciones = collection["Observaciones"];
                objUsuario.Password = collection["Password"];
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int id)
        {
            var objUsuario = (from obj in db.Usuarios where obj.IdUsuario == id select obj).First();
            return View(objUsuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var objUsuario = (from obj in db.Usuarios where obj.IdUsuario == id select obj).First();
                db.Usuarios.Remove(objUsuario);
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
