using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow
{
    public partial class Menu : System.Web.UI.Page
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

                // Permisos Usuario
                string UsPrivilegios = System.Web.HttpContext.Current.Session["UsPrivilegios"] as String;

                if (UsPrivilegios == "User")
                {

                    MenuItem item1 = Menu1.FindItem("Configuracion/Usuarios");
                    item1.Enabled = false;
                    MenuItem item2 = Menu1.FindItem("Configuracion/Bitacora");
                    item2.Enabled = false;
                    MenuItem item3 = Menu1.FindItem("Fonac/Totales para desincorporados");
                 // item3.Parent.ChildItems.Remove(item3);
                    item3.Enabled = false;

                }
            }

        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }


        protected void MyMenu_MenuItemClick(object sender, MenuEventArgs e)
        {

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

    }
}