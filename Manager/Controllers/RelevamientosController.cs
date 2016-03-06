using Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;

namespace Manager.Controllers
{
    
    //[Authorize(Roles = "Resp. Relevamiento")]
    public class RelevamientosController : Controller
    {
        HalconDBEntities db = new HalconDBEntities();

        // GET: Relevamientos
        [Authorize(Roles = "Resp. Relevamiento, Resp. Conteo")]
        public ActionResult Index()
        {
            return View(db.Relevamientos);
        }

        // GET: Relevamientos/Create
        [Authorize(Roles = "Resp. Relevamiento, Resp. Conteo")]
        public ActionResult Create()
        {
            ViewData["Trampas"] = new SelectList(db.Trampas.ToList(), "IdTrampa", "IdTrampa");
            ViewData["Estados"] = new SelectList(db.Estados.ToList(), "IdEstado", "Nombre");
            return View();
        }

        // POST: Relevamientos/Create
        [HttpPost]
        [Authorize(Roles = "Resp. Relevamiento, Resp. Conteo")]
        public ActionResult Create(FormCollection collection, HttpPostedFileBase ArchAuto, HttpPostedFileBase ArchManual)
        {
            try
            {
                if (ArchAuto != null &&
                    ArchAuto.ContentLength > 0 &&
                    ArchAuto.FileName.EndsWith("txt") &&
                    ArchManual != null &&
                    ArchManual.ContentLength > 0 &&
                    (ArchManual.FileName.EndsWith("xls") || ArchManual.FileName.EndsWith("xlsx"))
                    )
                {
                    string pathArchManual = "";

                    if (!Directory.Exists(Server.MapPath("Excels"))) Directory.CreateDirectory(Server.MapPath("Excels"));

                    var readerArchAuto = new BinaryReader(ArchAuto.InputStream);
                    string resultArchAuto = System.Text.Encoding.UTF8.GetString(readerArchAuto.ReadBytes(ArchAuto.ContentLength));
                    string[] lineasArchAuto = Regex.Split(resultArchAuto, "\r\n");

                    Relevamientos objRelevamiento = new Relevamientos();
                    objRelevamiento.Observaciones = collection["Observaciones"];
                    objRelevamiento.FechaInicio = DateTime.Parse(lineasArchAuto[0].Split(';')[0].ToString());
                    objRelevamiento.FechaFinal = DateTime.Parse(lineasArchAuto[lineasArchAuto.Length - 1].Split(';')[0].ToString());
                    objRelevamiento.IdEstado = 1;
                    objRelevamiento.IdTrampa = int.Parse(lineasArchAuto[0].Split(';')[2].ToString());
                    db.Relevamientos.Add(objRelevamiento);
                    //db.SaveChanges();

                    pathArchManual = Server.MapPath("Excels") + @"\" + ArchManual.FileName;
                    if (System.IO.File.Exists(pathArchManual)) System.IO.File.Delete(pathArchManual);

                    ArchManual.SaveAs(pathArchManual);

                    string cnnStr = "";
                    if (pathArchManual.EndsWith(".xlsx"))
                    {
                        //Excel 2007
                        cnnStr = "Provider=Microsoft.ACE.OLEDB.12.0;Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'";
                        cnnStr += ";Data Source=" + pathArchManual + ";";
                    }
                    else
                    {
                        //Excel 97-2003
                        //http://www.connectionstrings.com/excel (leer sobre la clave de registro TypeGuessRows)
                        cnnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
                        cnnStr += ";Data Source=" + pathArchManual + ";";
                    }

                    System.Data.OleDb.OleDbConnection oCnn = new System.Data.OleDb.OleDbConnection(cnnStr);
                    System.Data.OleDb.OleDbDataAdapter oDa = null;
                    DataTable dtArchManual = new DataTable();

                    try
                    {
                        oCnn.Open();
                        //Obtenemos los nombres de las hojas del Excel.
                        DataTable dtHojas = oCnn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                        if (dtHojas.Rows.Count > 0)
                        {
                            string firstSheet = dtHojas.Rows[0]["TABLE_NAME"].ToString().Trim();

                            string selectCmd = "select * from [" + firstSheet + "]";
                            oDa = new System.Data.OleDb.OleDbDataAdapter(selectCmd, oCnn);
                            oDa.Fill(dtArchManual);
                        }
                        oCnn.Close();

                        dtArchManual.Columns.Add("Fecha");

                        foreach (DataRow drFila in dtArchManual.Rows)
                        {
                            int iAño = 0;
                            int iMes = 0;
                            int iDia = 0;

                            int.TryParse(drFila[0].ToString(), out iAño);
                            int.TryParse(drFila[1].ToString(), out iMes);
                            int.TryParse(drFila[2].ToString(), out iDia);

                            if (iAño > 0 && iMes > 0 && iDia > 0)
                            {
                                DateTime tFecha = new DateTime(iAño, iMes, iDia);
                                drFila["Fecha"] = tFecha.Date.ToShortDateString();
                            }

                            if (drFila["Fecha"].ToString() == objRelevamiento.FechaFinal.ToShortDateString())
                            {
                                foreach (DataColumn dcColumna in dtArchManual.Columns)
                                {
                                    if (dcColumna.Ordinal > 2)
                                    {
                                        //CORREGIR SELECCION DE INSECTO
                                        Insectos objInsecto = (from obj in db.Insectos where obj.NombreCientifico == dcColumna.ColumnName select obj).FirstOrDefault();
                                        if (objInsecto != null)
                                        {
                                            int Cantidad = 0;
                                            int.TryParse(drFila[dcColumna.Ordinal].ToString(), out Cantidad);
                                            if (Cantidad > 0)
                                            {
                                                LecturasManuales objLecturasManuales = new LecturasManuales();
                                                objLecturasManuales.IdRelevamiento = objRelevamiento.IdRelevamiento;
                                                objLecturasManuales.IdInsecto = objInsecto.IdInsecto;
                                                objLecturasManuales.Cantidad = Cantidad;
                                                objLecturasManuales.IdEstado = 1;

                                                db.LecturasManuales.Add(objLecturasManuales);
                                            }
                                        }
                                    }
                                }
                                //db.SaveChanges();
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (oCnn.State == ConnectionState.Open) oCnn.Close();
                    }

                    if (oDa != null) { oDa.Dispose(); }
                    if (oCnn != null) { oCnn.Dispose(); }








                    //var objRelevamiento2 = (from obj in db.Relevamientos select obj).OrderByDescending(i => i.IdRelevamiento).First();
                    foreach (string Linea in lineasArchAuto)
                    {
                        string[] arrDatos = Linea.Split(';');
                        if (arrDatos.Length > 1 && arrDatos[1].ToString() == "LECTURA")
                        {
                            Lecturas objLecturas = new Lecturas();
                            objLecturas.IdEstado = 1;
                            objLecturas.IdRelevamiento = objRelevamiento.IdRelevamiento;
                            objLecturas.Frecuencia = double.Parse(arrDatos[2], System.Globalization.CultureInfo.InvariantCulture);
                            objLecturas.Aleteos = int.Parse(arrDatos[3]);
                            objLecturas.FechaLectura = DateTime.Parse(arrDatos[0]);
                            db.Lecturas.Add(objLecturas);
                        }
                        else if (arrDatos.Length > 1 && arrDatos[1].ToString() == "ESTADO")
                        {
                            Monitoreos objMonitoreos = new Monitoreos();
                            objMonitoreos.IdEstado = 1;
                            objMonitoreos.IdRelevamiento = objRelevamiento.IdRelevamiento;
                            objMonitoreos.Humedad = double.Parse(arrDatos[3], System.Globalization.CultureInfo.InvariantCulture);
                            objMonitoreos.Temperatura = double.Parse(arrDatos[2], System.Globalization.CultureInfo.InvariantCulture);
                            objMonitoreos.Bateria = double.Parse(arrDatos[4], System.Globalization.CultureInfo.InvariantCulture);
                            objMonitoreos.FechaMonitoreo = DateTime.Parse(arrDatos[0]);
                            db.Monitoreos.Add(objMonitoreos);
                        }
                        //db.SaveChanges();
                    }
                    db.SaveChanges();

                    //GenerarAlarmas(id);

                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Relevamientos/Edit/5
        [Authorize(Roles = "Resp. Relevamiento")]
        public ActionResult Editar(int id)
        {
            ViewData["Trampas"] = new SelectList(db.Trampas.ToList(), "IdTrampa", "IdTrampa");
            ViewData["Estados"] = new SelectList(db.Estados.ToList(), "IdEstado", "Nombre");
            Relevamientos objRelevamiento = (from obj in db.Relevamientos where obj.IdRelevamiento == id select obj).First();
            return View(objRelevamiento);
        }

        // POST: Relevamientos/Edit/5
        [HttpPost]
        [Authorize(Roles = "Resp. Relevamiento")]
        public ActionResult Editar(int id, FormCollection collection)
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

        [Authorize(Roles = "Resp. Relevamiento")]
        public ActionResult Validar(int id)
        {
            try
            {
                Relevamientos objRelevamiento = (from obj in db.Relevamientos where obj.IdRelevamiento == id select obj).First();
                var objCantAuto = from objLecturas in db.Lecturas
                                  from objInsectos in db.Insectos
                                  where objLecturas.Frecuencia <= objInsectos.FrecuenciaMax && objLecturas.Frecuencia >= objInsectos.FrecuenciaMin
                                  group objInsectos by objInsectos.IdInsecto into objGrupo
                                  select new
                                  {
                                      IdInsecto = objGrupo.Key,
                                      CantAuto = objGrupo.Count()
                                  };

                var objCantMan = from objLecturasManuales in db.LecturasManuales
                                 from objInsectos in db.Insectos
                                 where objLecturasManuales.IdInsecto == objInsectos.IdInsecto
                                 select new
                                 {
                                     objInsectos.IdInsecto,
                                     CantMan = objLecturasManuales.Cantidad
                                 };

                var objTablaTotales = (from objIN in db.Insectos
                                       join objCA in objCantAuto on objIN.IdInsecto equals objCA.IdInsecto into temporalCA
                                       join objCM in objCantMan on objIN.IdInsecto equals objCM.IdInsecto into temporalCM
                                       from tmpCA in temporalCA.DefaultIfEmpty()
                                       from tmpCM in temporalCM.DefaultIfEmpty()
                                       where tmpCA.CantAuto > 0 || tmpCM.CantMan > 0
                                       select new TablaTotales()
                                       {
                                           Insecto = objIN.NombreCientifico,
                                           CantidadAutomatica = (tmpCA == null ? 0 : tmpCA.CantAuto),
                                           CantidadManual = (tmpCM == null ? 0 : tmpCM.CantMan)
                                       }).ToList<TablaTotales>();

                var objAlarmasTotales = (from objAlarmas in db.Alarmas
                                         from objInsectos in db.Insectos
                                         where objAlarmas.IdRelevamiento == id
                                         && objAlarmas.IdInsecto == objInsectos.IdInsecto
                                         select new TablaAlarmas()
                                         {
                                             Insecto = objInsectos.NombreCientifico,
                                             CantidadMaxima = objAlarmas.ValorMaximo,
                                             CantidadReal = objAlarmas.Cantidad,
                                             Tipo = objAlarmas.Tipo,
                                             Estado = objAlarmas.Estados.Nombre
                                         }).ToList<TablaAlarmas>();

                DatosParaValidar objDatosParaValidar = new DatosParaValidar();
                objDatosParaValidar.Relevamiento = objRelevamiento;
                objDatosParaValidar.Totales = objTablaTotales;
                objDatosParaValidar.Alarmas = objAlarmasTotales;



                return View(objDatosParaValidar);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [Authorize(Roles = "Resp. Relevamiento")]
        public ActionResult Validar1(int id)
        {
            var objRelevamiento = (from obj in db.Relevamientos where obj.IdRelevamiento == id select obj).First();
            objRelevamiento.IdEstado = 1;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Resp. Relevamiento")]
        public ActionResult Validar2(int id)
        {
            var objRelevamiento = (from obj in db.Relevamientos where obj.IdRelevamiento == id select obj).First();
            objRelevamiento.IdEstado = 2;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Resp. Relevamiento, Resp. Conteo")]
        public JsonResult InsectosDataHora(int idRelevamiento)
        {
            var objRelevamiento = (from obj in db.Relevamientos where obj.IdRelevamiento == idRelevamiento select obj).First();
            System.Globalization.CultureInfo objCultura = new System.Globalization.CultureInfo("es-AR");
            System.Globalization.DateTimeFormatInfo objDateTimeFormat = new System.Globalization.DateTimeFormatInfo();
            System.Globalization.TextInfo objTextInfo = new System.Globalization.CultureInfo("es-AR").TextInfo;
            DataLayer objDL = new DataLayer();
            System.Data.DataTable objTabla = new System.Data.DataTable();
            objTabla = objDL.SP_GETLecturasInsectosByRelevamiento(idRelevamiento);

            var sData = new object[objTabla.Rows.Count + 1];
            string[] sCabeceras = new string[objTabla.Columns.Count];
            for (int iColumnas = 0; iColumnas < objTabla.Columns.Count; iColumnas++)
            {
                sCabeceras[iColumnas] = objTabla.Columns[iColumnas].ToString();
            }
            sData[0] = sCabeceras;

            for (int iFilas = 0; iFilas < objTabla.Rows.Count; iFilas++)
            {
                var sCeldas = new object[objTabla.Columns.Count];
                for (int iColumnas = 0; iColumnas < objTabla.Columns.Count; iColumnas++)
                {
                    if (iColumnas == 0)
                    {
                        string strHora = new DateTime(objRelevamiento.FechaInicio.Year, objRelevamiento.FechaInicio.Month, objRelevamiento.FechaInicio.Day, int.Parse(objTabla.Rows[iFilas][iColumnas].ToString()), 0, 0).ToString("hh", objCultura);
                        sCeldas[iColumnas] = strHora; //objTabla.Rows[iFilas][iColumnas].ToString();
                    }
                    else
                    {
                        sCeldas[iColumnas] = int.Parse(objTabla.Rows[iFilas][iColumnas].ToString());
                    }
                }
                sData[iFilas + 1] = sCeldas;
            }

            return new JsonResult { Data = sData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }

    public class DatosParaValidar : System.Collections.IEnumerable
    {
        public Relevamientos Relevamiento { get; set; }
        public List<TablaTotales> Totales { get; set; }
        public List<TablaAlarmas> Alarmas { get; set; }

        public System.Collections.IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class TablaTotales
    {
        public string Insecto { get; set; }
        public double CantidadAutomatica { get; set; }
        public double CantidadManual { get; set; }
    }

    public class TablaAlarmas
    {
        public string Insecto { get; set; }
        public string Tipo { get; set; }
        public long CantidadMaxima { get; set; }
        public long CantidadReal { get; set; }
        public string Estado { get; set; }
    }
}
