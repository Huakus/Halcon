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
        }

        public JsonResult InsectosDataMes(int Año)
        {
            DataLayer objDL = new DataLayer();
            DataTable objTabla = new DataTable();
            objTabla = objDL.SP_GETLecturasInsectosByMes(Año);

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
                        string strMes = new DateTime(Año, int.Parse(objTabla.Rows[iFilas][iColumnas].ToString()), 1).ToString("MMMM", objCultura);
                        sCeldas[iColumnas] = strMes; //objTabla.Rows[iFilas][iColumnas].ToString();
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

        public JsonResult InsectosDataDia(int Año, string Mes)
        {
            DataLayer objDL = new DataLayer();
            DataTable objTabla = new DataTable();
            int NumeroMes = DateTime.ParseExact(Mes, "MMMM", objCultura).Month;
            objTabla = objDL.SP_GETLecturasInsectosByDia(Año, NumeroMes);

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
                        string strDia = new DateTime(Año, NumeroMes, int.Parse(objTabla.Rows[iFilas][iColumnas].ToString())).ToString("dd", objCultura);
                        sCeldas[iColumnas] = strDia; //objTabla.Rows[iFilas][iColumnas].ToString();
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
            */
        }


        public JsonResult InsectosDataHora(int Año, string Mes, int Dia)
        {
            DataLayer objDL = new DataLayer();
            DataTable objTabla = new DataTable();
            int NumeroMes = DateTime.ParseExact(Mes, "MMMM", objCultura).Month;
            objTabla = objDL.SP_GETLecturasInsectosByHora(Año, NumeroMes, Dia);

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
                        string strHora = new DateTime(Año, NumeroMes, Dia, int.Parse(objTabla.Rows[iFilas][iColumnas].ToString()), 0, 0).ToString("hh", objCultura);
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

            /*
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
            */
        }
    }
}