using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

using MySql.Data.MySqlClient;
using System.Web.Security;
using System.Web.SessionState;

namespace WebItNow_Peacock
{
    public class Global : HttpApplication
    {
        // string Url_OneDrive = "\\Users\\Administrator\\OneDrive - INSURANCE CLAIMS & RISKS MEXICO\\";

        // string Url_OneDrive = "\\Users\\Administrator\\OneDrive - Peacock\\";
        // string Url_OneDrive_Eliminados = "\\Users\\Administrator\\OneDrive - Peacock\\CUADERNOS ELIMINADOS\\";

        void Application_Start(object sender, EventArgs e)
        {
            // Código que se ejecuta al iniciar la aplicación
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // Registrar bundles
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            string path = HttpContext.Current.Request.Path.ToLower();
            if (path == "/manychatreceptor")
            {
                HttpContext.Current.RewritePath("/ManychatReceptor.aspx");
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Session["Url_OneDrive"] = Url_OneDrive;
            // Session["Url_OneDrive_Eliminados"] = Url_OneDrive_Eliminados;
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["Idioma"] != null)
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                    new System.Globalization.CultureInfo(HttpContext.Current.Session["Idioma"].ToString());
                System.Threading.Thread.CurrentThread.CurrentCulture =
                    new System.Globalization.CultureInfo(HttpContext.Current.Session["Idioma"].ToString());
            }
            else
            {
                // Idioma por defecto: español
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                    new System.Globalization.CultureInfo("es-MX");
                System.Threading.Thread.CurrentThread.CurrentCulture =
                    new System.Globalization.CultureInfo("es-MX");
            }
        }

    }
}