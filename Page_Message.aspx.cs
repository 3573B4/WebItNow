using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                string sReferencia = Convert.ToString(Session["Referencia"]);
                string sAsunto = Convert.ToString(Session["Asunto"]);

             // lblUsuario.Text = sUsuario;
                BtnRegresar.Visible = false;

                if (sAsunto == "Documento Recibido")
                {
                    BtnRegresar.Visible = false;
                    //LblMotivo.Text = "Motivo de Enviado";
                    lblHeader.Text = "Tu documento se encuentra procesado.";
                    LblMotivo.Text = "Deseas agregar algún comentario o instrucción adicional al documento.";
                }
                else if (sAsunto == "Solicitud Documento")
                {
                    BtnRegresar.Visible = false;
                    lblHeader.Text = "Tu solicitud de archivo se encuentra en proceso.";
                    LblMotivo.Text = "Agregar comentario o instrucción especifica:";
                }

                if (sAsunto == "Documentación Aceptada")
                {
                    //LblMotivo.Text = "Motivo de Aceptado";
                    lblHeader.Text = "Documento aprobado exitosamente.";
                    LblMotivo.Text = "Agregar observación o comentario.";

                }
                else if (sAsunto == "Documento Rechazado")
                {
                    //LblMotivo.Text = "Motivo de Rechazo";
                    lblHeader.Text = "Archivo no aprobado.";
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

                string sReferencia = Convert.ToString(Session["Referencia"]);
                string sAsunto = Convert.ToString(Session["Asunto"]);
                string sPredeterminado = string.Empty;

                if (sAsunto == "Documentación Aceptada")
                {
                    sPredeterminado = "Estimado cliente. \n " +
                        "Hacemos de su conocimiento que su documentación fue aprobada por nuestro personal en nuestra plataforma de carga segura. \n \n" +
                        "Atentamente \n " +
                        "El equipo de Peackok \n";
                }
                else if (sAsunto == "Documento Rechazado")
                {
                    sPredeterminado = "Estimado cliente. \n " +
                        "Lamentamos informarle que el archivo cargado en nuestro sistema no cumple con los parámetros establecidos, \n" + "por favor intente nuevamente subir su archivo en nuestro sistema de carga segura. \n" +
                        "Para completar este proceso es necesario ingresar a nuestro sitio de carga segura de información. Dando clic a continuación: \n " +
                        "https://bit.ly/3n1Ck2q" + "\n " +
                        "(Si no está habilitado este enlace, favor de copiarlo y pegarlo en su navegador) \n \n" +
                        "Para poder ingresar es necesario proporcionar el siguiente número de reporte/referencia: " + sReferencia + " \n" +
                        "Así como el correo electrónico que nos proporcionó para el seguimiento de su reporte. \n" +
                        "En caso de cualquier duda, puede ingresar a este vínculo, donde podrá buscar y encontrar lo que requiera: \n" +
                        "https://peacock.zendesk.com/hc/es-419" + "\n \n";
                    sPredeterminado += "Alternativamente, puede contactarnos en cualquiera de los siguientes medios, donde con gusto lo atenderemos: \n \n" +
                        "* Asistente virtual en " + "https://www.peacock.claims \n " +
                        "* WhatsApp: + 52 55-9035-4806 \n " +
                        "* Correo electrónico: " + "contacto@peacock.claims \n" +
                        "* Vía Teléfono: + 5255-8525-7200 y +52 55-8932-4700 \n \n" +
                        "Agradecemos de antemano su confianza y preferencia. Esperamos que su experiencia de servicio sea satisfactoria. \n";
                }
                else if (sAsunto == "Documento Recibido")
                {
                    sPredeterminado = "Estimado cliente. \n " +
                        "Le informamos que su archivo fue cargado exitosamente en nuestra plataforma de carga segura. \n " + "A la brevedad será revisado y validado por alguno de nuestros operadores.  \n Muchas gracias. \n";
                }
                else if (sAsunto == "Solicitud Documento" )
                {
                    //string sListado = getListaDocumentos();
                    //sListado + " \n " +
                    sPredeterminado = "Estimado Cliente. \n" +
                        "Enviamos este mensaje para hacer de su conocimiento que es necesario nos remita a la brevedad, \n" +
                        "la documentación correspondiente al reporte: " + sReferencia + " \n" +
                        "Para completar este proceso es necesario ingresar a nuestro sitio de carga segura de información. Dando clic a continuación: \n" +
                        "https://bit.ly/3n1Ck2q" + "\n " +
                        "(Si no está habilitado este enlace, favor de copiarlo y pegarlo en su navegador) \n \n" +
                        "En caso de cualquier duda, puede ingresar a este vínculo, donde podrá buscar y encontrar lo que requiera: \n " +
                        "https://peacock.zendesk.com/hc/es-419" + "\n \n";
                    sPredeterminado += "Alternativamente, puede contactarnos en cualquiera de los siguientes medios, donde con gusto lo atenderemos: \n \n" +
                        "* Asistente virtual en " + "https://www.peacock.claims \n " +
                        "* WhatsApp: + 52 55-9035-4806 \n " +
                        "* Correo electrónico: " + "contacto@peacock.claims \n " +
                        "* Vía Teléfono: + 5255-8525-7200 y +52 55-8932-4700 \n \n" +
                        "Agradecemos de antemano su confianza y preferencia. Esperamos que su experiencia de servicio sea satisfactoria. \n";
                }

                var email = new EnvioEmail();

                string sEmail = email.CorreoElectronico(sReferencia);
                string sBody = sPredeterminado + " \n Observaciones registradas : \n" + TxtAreaMensaje.Value;

                //TxtAreaMensaje.Value = sPredeterminado + TxtAreaMensaje.Value;
                //string sEmail = email.CorreoElectronico(sReferencia);
                //int Envio_Ok = email.EnvioMensaje(sReferencia, sEmail, sAsunto, sBody);


                if (sAsunto == "Solicitud Documento")
                {
                    string sCuerpo = "IMPORTANTE: Se solicita documentación para la atención de su reporte " + sReferencia;
                    int Envio_Ok = email.EnvioMensaje(sReferencia, sEmail, sCuerpo, sBody);
                }
                else
                {
                    int Envio_Ok = email.EnvioMensaje(sReferencia, sEmail, sAsunto, sBody);
                }

                if (sAsunto == "Documento Recibido")
                { 
                    Response.Redirect("Upload_Files.aspx");
                } 
                else if (sAsunto == "Documentación Aceptada" || sAsunto == "Documento Rechazado")
                {
                    Response.Redirect("Review_Document.aspx");
                }
                else if (sAsunto == "Solicitud Documento")
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    Session.Abandon();

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
                //System.Web.Security.FormsAuthentication.SignOut();
                //Session.Abandon();

                Response.Redirect("Request_Document.aspx");
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {

        }

        protected string getListaDocumentos()
        {
            string listado = string.Empty;
            string sReferencia = Convert.ToString(Session["Referencia"]);

            ConexionBD Conectar = new ConexionBD();
            Conectar.Abrir();
            string sqlQuery = "SELECT td.Descripcion " +
                               " FROM ITM_06 td, ITM_15 t " +
                              " WHERE t.Referencia = '" + sReferencia + "' " +
                                " AND t.IdProceso = td.IdProceso " +
                                " AND t.IdSubProceso = td.IdSubProceso" +
                                " AND t.IdTpoDocumento = td.IdTpoDocumento " +
                                " AND t.IdStatus = 1";

            SqlCommand ejecucion = new SqlCommand(sqlQuery, Conectar.ConectarBD);
            SqlDataReader dr1 = ejecucion.ExecuteReader();

            if (dr1.HasRows)
            {

                while (dr1.Read())
                {
                    listado = listado + dr1["Descripcion"].ToString().Trim() + " \n ";
                }
            }
            else
            {
                return listado;
            }

            return listado;
        }
    }
}