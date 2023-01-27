using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow
{
    public partial class PagMessage : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userId = Convert.ToString(Session["IdUsuario"]);
                lblUsuario.Text = userId;
            }
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                string sMensajeTextArea = TxtAreaMensaje.Value;

                string user = Convert.ToString(Session["IdUsuario"]);

                var email = new EnvioEmail();
                string sEmail = email.CorreoElectronico(user);
                int Envio_Ok = email.EnvioMensaje(user, sEmail, "Documento Motivo");

                Response.Redirect("Upload_Files_1.aspx");
            }
            catch(Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {

        }
    }
}