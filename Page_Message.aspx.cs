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
                string sAsunto = Convert.ToString(Session["Asunto"]);

                lblUsuario.Text = sUsuario;
                BtnRegresar.Visible = false;

                if (sAsunto == "Documento Enviado")
                {
                    BtnRegresar.Visible = false;
                    LblMotivo.Text = "Motivo de Enviado";
                }
                else if (sAsunto == "Solicitud Documento")
                {
                    BtnRegresar.Visible = false;
                    LblMotivo.Text = "Motivo de Solicitud";
                }

                if (sAsunto == "Documento Aceptado")
                {
                    LblMotivo.Text = "Motivo de Aceptado";
                }
                else if (sAsunto == "Documento Rechazado")
                {
                    LblMotivo.Text = "Motivo de Rechazo";
                }

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
                if (sAsunto != "Solicitud Documento")
                {
                    string sEmail = email.CorreoElectronico(sUsuario);
                    int Envio_Ok = email.EnvioMensaje(sUsuario, sEmail, sAsunto, TxtAreaMensaje.Value);
                }
                else
                {
                    string sEmail = Convert.ToString(Session["Email"]);
                    int Envio_Ok = email.EnvioMensaje(sUsuario, sEmail, sAsunto, TxtAreaMensaje.Value);
                }


                if (sAsunto == "Documento Enviado")
                { 
                    Response.Redirect("Upload_Files.aspx");
                } 
                else if (sAsunto == "Documento Aceptado" || sAsunto == "Documento Rechazado")
                {
                    Response.Redirect("Review_Document.aspx");
                }
                else if (sAsunto == "Solicitud Documento")
                {
                    Response.Redirect("Request_Document.aspx");
                }
                    

            }
            catch(Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            string sAsunto = Convert.ToString(Session["Asunto"]);

            if (sAsunto == "Documento Aceptado" || sAsunto == "Documento Rechazado")
            {
                Response.Redirect("Review_Document.aspx");
            }
            else if (sAsunto == "Solicitud Documento")
            {
                Response.Redirect("Request_Document.aspx");
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {

        }
    }
}