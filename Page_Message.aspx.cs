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

                if (sAsunto == "Documento Enviado")
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

                if (sAsunto == "Documento Aceptado")
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

                if (sAsunto == "Documento Aceptado")
                {
                    sPredeterminado = "Estimado cliente su archivo fue aprobado por nuestro personal en nuestra plataforma de carga segura. Gracias. \n ";
                }
                else if (sAsunto == "Documento Rechazado")
                {
                    sPredeterminado = "Estimado cliente. Lamentamos informarle que el archivo cargado en nuestro sistema no cumple con los parametros establecidos, \n " + "por favor reintente subir su archivo en nuestro sistema de carga segura. \n";
                }
                else if (sAsunto == "Documento Enviado")
                {
                    sPredeterminado = "Estimado cliente. Su archivo fue cargado exitosamente en nuestra plataforma de carga segura. \n " + "A la brevedad sera revisado y validado por alguno de nuestros operadores. Muchas gracias. \n";
                }
                else if (sAsunto == "Solicitud Documento" )
                {
                    string sListado = getListaDocumentos();

                    sPredeterminado = "Le informamos que para la atención de su siniestro es necesario nos proporciones la siguiente información: \n \n " +
                        sListado + " \n " +
                        "Para realizar lo anterior es necesario acceder a nuestro sitio seguro: \n " +
                        "https://codice1.azurewebsites.net/Access \n " +
                        "Sus datos de acceso son su correo electrónico y la siguiente referencia " + sReferencia;
                }

                var email = new EnvioEmail();

                TxtAreaMensaje.Value = sPredeterminado + TxtAreaMensaje.Value;

                string sEmail = email.CorreoElectronico(sReferencia);
                int Envio_Ok = email.EnvioMensaje(sReferencia, sEmail, sAsunto, TxtAreaMensaje.Value);

                //if (sAsunto == "Solicitud Documento")
                //{
                //    string sEmail = email.CorreoElectronico(sReferencia);
                //    int Envio_Ok = email.EnvioMensaje(sReferencia, sEmail, sAsunto, TxtAreaMensaje.Value);
                //}
                //else
                //{
                //    string sEmail = Convert.ToString(Session["Email"]);
                //    int Envio_Ok = email.EnvioMensaje(sReferencia, sEmail, sAsunto, TxtAreaMensaje.Value);
                //}

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
                System.Web.Security.FormsAuthentication.SignOut();
                Session.Abandon();

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