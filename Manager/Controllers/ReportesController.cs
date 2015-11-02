using Manager.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace Manager.Controllers
{
    public class ReportesController : Controller
    {
        CultureInfo objCultura = new CultureInfo("es-AR");
        DateTimeFormatInfo objDateTimeFormat = new DateTimeFormatInfo();
        TextInfo objTextInfo = new CultureInfo("es-AR").TextInfo;
        // GET: Reportes
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult InsectosDataAño()
        {

            DataLayer objDL = new DataLayer();
            DataTable objTabla = new DataTable();
            objTabla = objDL.SP_GETLecturasInsectosByAño();

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
                        sCeldas[iColumnas] = objTabla.Rows[iFilas][iColumnas].ToString();
                    }
                    else
                    {
                        sCeldas[iColumnas] = int.Parse(objTabla.Rows[iFilas][iColumnas].ToString());
                    }
                }
                sData[iFilas + 1] = sCeldas;
            }

            return new JsonResult { Data = sData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            /*
            using (HalconDBEntities dc = new HalconDBEntities())
            {
                var v = (from a in dc.Lecturas
                         group a by a.FechaLectura.Year into g
                         select new
                         {
                             Año = g.Key,
                             Cantidad = g.Count()
                         });
                if (v != null)
                {
                    var chartData = new object[v.Count() + 1];
                    chartData[0] = new object[]
                    {
                        "Año",
                        "Cantidad"
                    };
                    var AñoData = v.ToList();
                    int j = 0;
                    foreach (var i in AñoData)
                    {
                        j++;
                        chartData[j] = new object[] { i.Año.ToString(), i.Cantidad };
                    }
                    return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            */
        }

        public JsonResult InsectosDataMes(int Año)
        {
            using (HalconDBEntities dc = new HalconDBEntities())
            {
                var v = (from a in dc.Lecturas
                         where a.FechaLectura.Year.Equals(Año)
                         group a by a.FechaLectura.Month into g
                         select new
                         {
                             Mes = g.Key,
                             Cantidad = g.Count()
                         });
                if (v != null)
                {
                    var chartData = new object[12 + 1];
                    chartData[0] = new object[]
                    {
                        "Mes",
                        "Cantidad"
                    };
                    var MesData = v.ToList();
                    for (int i = 1; i <= 12; i++)
                    {
                        string strMes = new DateTime(Año, i, 1).ToString("MMMM", objCultura);
                        foreach (var objFila in MesData)
                        {
                            if (objFila.Mes == i)
                            {
                                chartData[i] = new object[] { objTextInfo.ToTitleCase(strMes), objFila.Cantidad };
                            }
                            else
                            {
                                chartData[i] = new object[] { objTextInfo.ToTitleCase(strMes), 0 };
                            }
                        }
                    }
                    return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult InsectosDataDia(int Año, string Mes)
        {
            int NumeroMes = DateTime.ParseExact(Mes, "MMMM", objCultura).Month;
            int Dias = DateTime.DaysInMonth(Año, NumeroMes);
            using (HalconDBEntities dc = new HalconDBEntities())
            {
                var v = (from a in dc.Lecturas
                         where a.FechaLectura.Year.Equals(Año) && a.FechaLectura.Month.Equals(NumeroMes)
                         group a by a.FechaLectura.Day into g
                         select new
                         {
                             Dia = g.Key,
                             Cantidad = g.Count()
                         });
                if (v != null)
                {
                    var chartData = new object[Dias + 1];
                    chartData[0] = new object[]
                    {
                        "Dia",
                        "Cantidad"
                    };
                    var DiaData = v.ToList();
                    for (int i = 1; i <= Dias; i++)
                    {
                        foreach (var objFila in DiaData)
                        {
                            if (objFila.Dia == i)
                            {
                                chartData[i] = new object[] { i, objFila.Cantidad };
                            }
                            else
                            {
                                chartData[i] = new object[] { i, 0 };
                            }
                        }
                    }
                    return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult InsectosDataHora(int Año, string Mes, int Dia)
        {
            int NumeroMes = DateTime.ParseExact(Mes, "MMMM", objCultura).Month;
            using (HalconDBEntities dc = new HalconDBEntities())
            {
                var v = from a in dc.Lecturas
                        where a.FechaLectura.Year.Equals(Año) && a.FechaLectura.Month.Equals(NumeroMes) && a.FechaLectura.Day.Equals(Dia)
                        group a by a.FechaLectura.Hour into g
                        select new
                        {
                            Hora = g.Key,
                            Cantidad = g.Count()
                        };
                if (v != null)
                {
                    var chartData = new object[24 + 1];
                    chartData[0] = new object[]
                    {
                        "Dia",
                        "Cantidad"
                    };
                    var HoraData = v.ToList();
                    for (int i = 1; i <= 24; i++)
                    {
                        foreach (var objFila in HoraData)
                        {
                            if (objFila.Hora == i)
                            {
                                chartData[i] = new object[] { i, objFila.Cantidad };
                            }
                            else
                            {
                                chartData[i] = new object[] { i, 0 };
                            }
                        }
                    }
                    return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}