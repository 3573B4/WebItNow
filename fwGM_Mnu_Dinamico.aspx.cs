using System;
using System.Web.UI;

namespace WebItNow_Peacock.GastosMedicos
{
    public partial class fwGM_Mnu_Dinamico : System.Web.UI.Page
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
                }
            }
        }
    }
}