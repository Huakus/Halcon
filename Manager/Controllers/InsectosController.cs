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
            return View(db.Estados.ToList());
        }

        // GET: Insectos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Insectos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insectos/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

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
            return View();
        }

        // POST: Insectos/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

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
            return View();
        }

        // POST: Insectos/Delete/5
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
