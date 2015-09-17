using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text.RegularExpressions;

namespace Manager.Controllers
{
    public class RelevamientosController : Controller
    {
        HalconDBEntities db = new HalconDBEntities();

        // GET: Relevamientos
        public ActionResult Index()
        {
            return View(db.Relevamientos);
        }

        // GET: Relevamientos/Details/5
        public ActionResult Details(int id)
        {
            var objRelevamiento = (from obj in db.Relevamientos where obj.IdRelevamiento == id select obj).First();
            return View(objRelevamiento);
        }

        // GET: Relevamientos/Create
        public ActionResult Create()
        {
            ViewData["Trampas"] = new SelectList(db.Trampas.ToList(), "IdTrampa", "IdTrampa");
            ViewData["Estados"] = new SelectList(db.Estados.ToList(), "IdEstado", "Nombre");
            return View();
        }

        // POST: Relevamientos/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, HttpPostedFileBase Archivo)
        {
            try
            {
                if (Archivo != null && Archivo.ContentLength > 0)
                {
                    var reader = new BinaryReader(Archivo.InputStream);
                    string result = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(Archivo.ContentLength));
                    string[] arrLineas = Regex.Split(result, "\r\n");

                    Relevamientos objRelevamiento = new Relevamientos();
                    objRelevamiento.Observaciones = collection["Observaciones"];
                    objRelevamiento.FechaInicio = DateTime.Parse(arrLineas[0].Split(';')[0].ToString());
                    objRelevamiento.FechaFinal = DateTime.Parse(arrLineas[arrLineas.Length - 1].Split(';')[0].ToString());
                    objRelevamiento.IdEstado = 1;
                    objRelevamiento.IdTrampa = int.Parse(arrLineas[0].Split(';')[2].ToString());
                    db.Relevamientos.Add(objRelevamiento);
                    db.SaveChanges();


                    Lecturas objLecturas = new Lecturas();
                    
                    Monitoreos objMonitoreos = new Monitoreos();
                    var objRelevamiento2 = (from obj in db.Relevamientos select obj).OrderByDescending(i => i.IdRelevamiento).First();
                    foreach (string Linea in arrLineas)
                    {
                        string[] arrDatos = Linea.Split(';');
                        if (arrDatos.Length > 1 && arrDatos[1].ToString() == "LECTURA")
                        {
                            objLecturas.IdEstado = 1;
                            objLecturas.IdRelevamiento = objRelevamiento2.IdRelevamiento;
                            objLecturas.Frecuencia = double.Parse(arrDatos[2]);
                            objLecturas.Aleteos = int.Parse(arrDatos[3]);
                            objLecturas.FechaLectura = DateTime.Parse(arrDatos[0]);
                            db.Lecturas.Add(objLecturas);
                        }
                        else if(arrDatos.Length > 1 && arrDatos[1].ToString() == "ESTADO")
                        {
                            objMonitoreos.IdEstado = 1;
                            objMonitoreos.IdRelevamiento = objRelevamiento2.IdRelevamiento;
                            objMonitoreos.Humedad = double.Parse(arrDatos[3]);
                            objMonitoreos.Temperatura = double.Parse(arrDatos[2]);
                            objMonitoreos.Bateria = double.Parse(arrDatos[4]);
                            objMonitoreos.FechaMonitoreo = DateTime.Parse(arrDatos[0]);
                            db.Monitoreos.Add(objMonitoreos);
                        }
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Relevamientos/Edit/5
        public ActionResult Edit(int id)
        {
            ViewData["Trampas"] = new SelectList(db.Trampas.ToList(), "IdTrampa", "IdTrampa");
            ViewData["Estados"] = new SelectList(db.Estados.ToList(), "IdEstado", "Nombre");
            var objRelevamiento = (from obj in db.Relevamientos where obj.IdRelevamiento == id select obj).First();
            return View(objRelevamiento);
        }

        // POST: Relevamientos/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                Relevamientos objRelevamiento = new Relevamientos();
                objRelevamiento.FechaFinal = DateTime.Parse(collection["FechaFinal"]);
                objRelevamiento.FechaInicio = DateTime.Parse(collection["FechaInicio"]);
                objRelevamiento.IdEstado = int.Parse(collection["IdEstado"]);
                objRelevamiento.IdTrampa = int.Parse(collection["IdTrampa"]);
                objRelevamiento.Observaciones = collection["Observaciones"];
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Relevamientos/Delete/5
        public ActionResult Delete(int id)
        {
            var objRelevamiento = (from obj in db.Relevamientos where obj.IdRelevamiento == id select obj).First();
            return View(objRelevamiento);
        }

        // POST: Relevamientos/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var objRelevamiento = (from obj in db.Relevamientos where obj.IdRelevamiento == id select obj).First();
                db.Relevamientos.Remove(objRelevamiento);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
