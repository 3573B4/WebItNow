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

             // lblUsuario.Text = sUsuario;
                BtnRegresar.Visible = false;

                if (sAsunto == "Documento Enviado")
                {
                    BtnRegresar.Visible = false;
                    //LblMotivo.Text = "Motivo de Enviado";
                    LblMotivo.Text = "Deseas agregar algún comentario o instrucción adicional al documento.";
                }
                else if (sAsunto == "Solicitud Documento")
                {
                    BtnRegresar.Visible = false;
                    LblMotivo.Text = "Agregar comentario o instrucción especifica:";
                }

                if (sAsunto == "Documento Aceptado")
                {
                    //LblMotivo.Text = "Motivo de Aceptado";
                    LblMotivo.Text = "Agregar observación o comentario.";

                }
                else if (sAsunto == "Documento Rechazado")
                {
                    //LblMotivo.Text = "Motivo de Rechazo";
                    LblMotivo.Text = "Indique el motivo por el cual fue rechazado el archivo.";
                }

            }
        }

        protected void BtnEnviarWhats_Click(object sender, EventArgs e)
        {

        }

        protected void BtnEnviarEmail_Click(object sender, EventArgs e)
        {
            try
            {
                string sMensajeTextArea = TxtAreaMensaje.Value;

                string sUsuario = Convert.ToString(Session["IdUsuario"]);
                string sAsunto = Convert.ToString(Session["Asunto"]);
                string sPredeterminado = string.Empty;

                if (sAsunto == "Documento Aceptado")
                {
                    sPredeterminado = "Estimado cliente su archivo fue aprobado por nuestro personal en nuestra plataforma de carga segura. Gracias.";
                }
                else if (sAsunto == "Documento Rechazado")
                {
                    sPredeterminado = "Estimado cliente. Lamentamos informarle que el archivo cargado en nuestro sistema no cumple con los parametros establecidos, por favor reintente subir su archivo en nuestro sistema de carga segura.";
                }
                else if (sAsunto == "Documento Enviado")
                {
                    sPredeterminado = "Estimado cliente. Su archivo fue cargado exitosamente en nuestra plataforma de carga segura. A la brevedad sera revisado y validado por alguno de nuestros operadores";
                }




                    var email = new EnvioEmail();


                TxtAreaMensaje.Value = sPredeterminado + TxtAreaMensaje.Value;

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