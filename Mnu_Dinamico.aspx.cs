using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class Mnu_Dinamico : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string UsPrivilegios = Convert.ToString(Session["UsPrivilegios"]);

                if (UsPrivilegios == "3")
                {
                    // Opción 1: Remover el href para deshabilitar el enlace
                    lnkAlta_Proyecto.Attributes.Remove("href");

                    // Opción 2: Usar JavaScript para deshabilitar el enlace
                    lnkAlta_Proyecto.Attributes["onclick"] = "return false;";

                    // Opción 3: Cambiar la apariencia y deshabilitar interacciones
                    lnkAlta_Proyecto.Attributes.CssStyle.Add("pointer-events", "none");
                    lnkAlta_Proyecto.Attributes.CssStyle.Add("opacity", "0.5");

                    // Opción 1: Remover el href para deshabilitar el enlace
                    lnkAlta_Inspeccion.Attributes.Remove("href");

                    // Opción 2: Usar JavaScript para deshabilitar el enlace
                    lnkAlta_Inspeccion.Attributes["onclick"] = "return false;";

                    // Opción 3: Cambiar la apariencia y deshabilitar interacciones
                    lnkAlta_Inspeccion.Attributes.CssStyle.Add("pointer-events", "none");
                    lnkAlta_Inspeccion.Attributes.CssStyle.Add("opacity", "0.5");
                }
            }
        }

    }
}