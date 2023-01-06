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
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                //Session["UserId"] = 1;

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

                 // BtnCarga_Nomina.Enabled = false;
                }
            }

            //if (Session["UserId"] == null)
            //{
            //    //OnClientClick="acceso(); return false;"
            //    LblMessage.Text = "La sesión ha expirado.";
            //    this.mpeMensaje.Show();

            //    Response.Redirect("Acceso.aspx",false);
            //    Context.ApplicationInstance.CompleteRequest();
            //}

        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void BtnCarga_Nomina_Click(object sender, EventArgs e)
        {
            Response.Redirect("Carga_Nomina.aspx");
        }

        protected void BtnConsulta_Click(object sender, EventArgs e)
        {
            Response.Redirect("Consulta.aspx");
        }

        protected void BtnCorrespondencia_Click(object sender, EventArgs e)
        {
            Response.Redirect("Correspondencia.aspx", false);
            // OnClientClick = "correspondencia(); return false;"
        }

        protected void MyMenu_MenuItemClick(object sender, MenuEventArgs e)
        {

        }
        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

    }
}