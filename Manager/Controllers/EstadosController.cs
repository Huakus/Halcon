using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager.Controllers
{
    [Authorize(Roles = "Resp. Relevamiento")]
    public class EstadosController : Controller
    {
        HalconDBEntities db = new HalconDBEntities();

        // GET: Estados
        public ActionResult Index()
        {
            return View(db.Estados.ToList());
        }

        // GET: Estados/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Estados/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Estados/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                Estados objEstado = new Estados();
                objEstado.Nombre = collection["Nombre"];
                objEstado.Descripcion = collection["Descripcion"];

                db.Estados.Add(objEstado);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Estados/Edit/5
        public ActionResult Edit(int id)
        {
            var objEstado = (from obj in db.Estados where obj.IdEstado == id select obj).First();
            return View(objEstado);
        }

        // POST: Estados/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var objEstado = (from obj in db.Estados where obj.IdEstado == id select obj).First();
                objEstado.Descripcion = collection["Descripcion"].ToString();
                objEstado.Nombre = collection["Nombre"].ToString();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Estados/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Estados/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
