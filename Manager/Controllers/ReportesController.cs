using Manager.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager.Controllers
{
    public class ReportesController : Controller
    {
        // GET: Reportes
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult InsectosDataAño()
        {
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
                    int j = 0;
                    foreach (var i in v)
                    {
                        j++;
                        chartData[j] = new object[] { i.Año.ToString(), i.Cantidad };
                    }
                    return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
                    for (int i = 1; i <= 12; i++)
                    {
                        string MesName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                        var MesData = v.Where(a => a.Mes.Equals(i)).FirstOrDefault();
                        if (MesData != null)
                        {
                            chartData[i] = new object[] { MesName, MesData.Cantidad };
                        }
                        else
                        {
                            chartData[i] = new object[] { MesName, 0 };
                        }
                    }
                    return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult InsectosDataDia(int Año, string Mes)
        {
            int NumeroMes = 0;
            switch (Mes)
            {
                case "enero":
                    NumeroMes = 1;
                    break;
                case "febrero":
                    NumeroMes = 2;
                    break;
                case "marzo":
                    NumeroMes = 3;
                    break;
                case "abril":
                    NumeroMes = 4;
                    break;
                case "mayo":
                    NumeroMes = 5;
                    break;
                case "junio":
                    NumeroMes = 6;
                    break;
                case "julio":
                    NumeroMes = 7;
                    break;
                case "agosto":
                    NumeroMes = 8;
                    break;
                case "septiembre":
                    NumeroMes = 9;
                    break;
                case "octubre":
                    NumeroMes = 10;
                    break;
                case "noviembre":
                    NumeroMes = 11;
                    break;
                case "diciembre":
                    NumeroMes = 12;
                    break;
                default:
                    NumeroMes = 1;
                    break;
            }
            //int NumeroMes = DateTime.ParseExact(Mes, "MMMM", CultureInfo.CurrentCulture).Month;
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
                    for (int i = 1; i <= Dias; i++)
                    {
                        var DiaData = v.Where(a => a.Dia.Equals(i)).FirstOrDefault();
                        if (DiaData != null)
                        {
                            chartData[i] = new object[] { i, DiaData.Cantidad };
                        }
                        else
                        {
                            chartData[i] = new object[] { i, 0 };
                        }
                    }
                    return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult InsectosDataHora(int Año, string Mes, int Dia)
        {
            int NumeroMes = 0;
            switch (Mes)
            {
                case "enero":
                    NumeroMes = 1;
                    break;
                case "febrero":
                    NumeroMes = 2;
                    break;
                case "marzo":
                    NumeroMes = 3;
                    break;
                case "abril":
                    NumeroMes = 4;
                    break;
                case "mayo":
                    NumeroMes = 5;
                    break;
                case "junio":
                    NumeroMes = 6;
                    break;
                case "julio":
                    NumeroMes = 7;
                    break;
                case "agosto":
                    NumeroMes = 8;
                    break;
                case "septiembre":
                    NumeroMes = 9;
                    break;
                case "octubre":
                    NumeroMes = 10;
                    break;
                case "noviembre":
                    NumeroMes = 11;
                    break;
                case "diciembre":
                    NumeroMes = 12;
                    break;
                default:
                    NumeroMes = 1;
                    break;
            }
            //int NumeroMes = DateTime.ParseExact(Mes, "MMMM", CultureInfo.CurrentCulture).Month;
            using (HalconDBEntities dc = new HalconDBEntities())
            {
                var v = (from a in dc.Lecturas
                         where a.FechaLectura.Year.Equals(Año) && a.FechaLectura.Month.Equals(NumeroMes) && a.FechaLectura.Day.Equals(Dia)
                         group a by a.FechaLectura.Hour into g
                         select new
                         {
                             Hora = g.Key,
                             Cantidad = g.Count()
                         });
                if (v != null)
                {
                    var chartData = new object[24 + 1];
                    chartData[0] = new object[]
                    {
                        "Dia",
                        "Cantidad"
                    };
                    for (int i = 1; i <= 24; i++)
                    {
                        var DiaData = v.Where(a => a.Hora.Equals(i)).FirstOrDefault();
                        if (DiaData != null)
                        {
                            chartData[i] = new object[] { i, DiaData.Cantidad };
                        }
                        else
                        {
                            chartData[i] = new object[] { i, 0 };
                        }
                    }
                    return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}