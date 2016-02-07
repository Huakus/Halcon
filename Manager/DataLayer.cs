using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ApplicationBlocks.Data;

namespace Manager
{
    public class DataLayer
    {
        string sCnn = "";
        public DataLayer()
        {
            sCnn = ConfigurationManager.ConnectionStrings["HalconDBEntitiesAB"].ToString();
        }

        public DataTable SP_GETLecturasInsectosByAño()
        {
            return SqlHelper.ExecuteDataset(sCnn, CommandType.StoredProcedure, "SP_GETLecturasInsectosByAño").Tables[0];
        }

        public DataTable SP_GETLecturasInsectosByMes(int Año)
        {
            return SqlHelper.ExecuteDataset(sCnn, "SP_GETLecturasInsectosByMes", Año).Tables[0];
        }

        public DataTable SP_GETLecturasInsectosByDia(int Año, int Mes)
        {
            return SqlHelper.ExecuteDataset(sCnn, "SP_GETLecturasInsectosByDia", Año, Mes).Tables[0];
        }

        public DataTable SP_GETLecturasInsectosByHora(int Año, int Mes, int Dia)
        {
            return SqlHelper.ExecuteDataset(sCnn, "SP_GETLecturasInsectosByHora", Año, Mes, Dia).Tables[0];
        }

        public DataTable SP_GETLecturasInsectosByRelevamiento(int idRelevamiento)
        {
            return SqlHelper.ExecuteDataset(sCnn, "SP_GETLecturasInsectosByRelevamiento", idRelevamiento).Tables[0];
        }
    }
}