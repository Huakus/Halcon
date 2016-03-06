using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Manager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public void Application_AuthenticateRequest(Object src, EventArgs e)
        {
            if (!(HttpContext.Current.User == null))
            {
                if (HttpContext.Current.User.Identity.AuthenticationType == "Forms")
                {
                    Manager.Controllers.RolesController objRolesController = new Controllers.RolesController();

                    Manager.Models.UsuarioAutenticado objUsuario = new JavaScriptSerializer().Deserialize<Manager.Models.UsuarioAutenticado>(FormsAuthentication.Decrypt(Request.Cookies[".ASPXAUTH"].Values[0]).UserData);

                    System.Web.Security.FormsIdentity id;
                    id = (System.Web.Security.FormsIdentity)HttpContext.Current.User.Identity;
                    Models.Roles objRol = new Models.Roles();
                    //objRol = objRolesController.Obtener(id);


                    String[] myRoles = new String[1];
                    myRoles[0] = objUsuario.Rol;
                    //myRoles[1] = "Admin";
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, myRoles);
                }
            }
        }
    }
}
