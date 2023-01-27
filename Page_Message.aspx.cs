using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow
{
    public partial class Page_Message : System.Web.UI.Page
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
                string sUsuario = Convert.ToString(Session["IdUsuario"]);
                lblUsuario.Text = sUsuario;
            }
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                string sMensajeTextArea = TxtAreaMensaje.Value;

                string sUsuario = Convert.ToString(Session["IdUsuario"]);
                string sAsunto = Convert.ToString(Session["Asunto"]);

                var email = new EnvioEmail();
                string sEmail = email.CorreoElectronico(sUsuario);
                int Envio_Ok = email.EnvioMensaje(sUsuario, sEmail, sAsunto);

                if (sAsunto == "Documento Enviado")
                { 
                    Response.Redirect("Upload_Files.aspx");
                } 
                else if (sAsunto == "Documento Aceptado" || sAsunto == "Documento Rechazado")
                {
                    Response.Redirect("Review_Document.aspx");
                }

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