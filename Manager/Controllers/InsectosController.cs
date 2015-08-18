﻿using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager.Controllers
{
    public class InsectosController : Controller
    {
        HalconDBEntities db = new HalconDBEntities();

        // GET: Insectos
        public ActionResult Index()
        {
            return View(db.Insectos.ToList());
        }

        // GET: Insectos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Insectos/Create
        public ActionResult Create()
        {
            ViewData["Generos"] = new SelectList(db.Generos.ToList(), "IdGenero", "Nombre");
            ViewData["Estados"] = new SelectList(db.Estados.ToList(), "IdEstado", "Nombre");
            return View();
        }

        // POST: Insectos/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                Insectos objInsecto = new Insectos();
                objInsecto.Ancho = int.Parse(collection["Ancho"]);
                objInsecto.IdEstado = int.Parse(collection["IdEstado"]);
                objInsecto.IdGenero = int.Parse(collection["IdGenero"]);
                objInsecto.Largo = int.Parse(collection["Largo"]);
                objInsecto.NombreCientifico = collection["NombreCientifico"];
                objInsecto.NombreVulgar = collection["NombreVulgar"];
                objInsecto.Observaciones = collection["Observaciones"];

                db.Insectos.Add(objInsecto);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Insectos/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["Generos"] = new SelectList(db.Generos.ToList(), "IdGenero", "Nombre");
            ViewData["Estados"] = new SelectList(db.Estados.ToList(), "IdEstado", "Nombre");
            var objInsecto = (from obj in db.Insectos where obj.IdInsecto == id select obj).First();
            return View(objInsecto);
        }

        // POST: Insectos/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var objInsecto = (from obj in db.Insectos where obj.IdInsecto == id select obj).First();
                objInsecto.Ancho = int.Parse(collection["Ancho"]);
                objInsecto.IdEstado = int.Parse(collection["IdEstado"]);
                objInsecto.IdGenero = int.Parse(collection["IdGenero"]);
                objInsecto.Largo = int.Parse(collection["Largo"]);
                objInsecto.NombreCientifico = collection["NombreCientifico"];
                objInsecto.NombreVulgar = collection["NombreVulgar"];
                objInsecto.Observaciones = collection["Observaciones"];
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Insectos/Delete/5
        public ActionResult Delete(int id)
        {
            var objInsecto = (from obj in db.Insectos where obj.IdInsecto == id select obj).First();
            return View(objInsecto);
        }

        // POST: Insectos/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var objInsecto = (from obj in db.Insectos where obj.IdInsecto == id select obj).First();
                db.Insectos.Remove(objInsecto);
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