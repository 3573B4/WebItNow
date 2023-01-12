
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.IO.Compression;

namespace WebItNow
{

    public partial class SubirArchivo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string valor = ddlDocs.SelectedValue;
            //Image1.ImageUrl = "~/Images/icono-Subir-Archivo-morado.png";
            if (!IsPostBack)
            {
                BtnEnviar.Enabled = true;
                getDocRequeridos();

                // * * Obtener Descripcion breve documento
                string IdDoc = ddlDocs.SelectedValue.ToString();
                LblDescrpBrev.Text = TpoDocumento_DescrpBrev(IdDoc, 1);

                getDocsUsuario();
                checarStatusDoc();
                string userId = Convert.ToString(Session["IdUsuario"]);
                lblUsuario.Text = userId;                
            }
        }

        public void checarStatusDoc()
        {
            string user = Convert.ToString(Session["IdUsuario"]);
            string tpoDoc = ddlDocs.SelectedValue;
            ConexionBD connect = new ConexionBD();
            connect.Abrir();

            // Consulta a la tabla Estado de Documento
            string edoQuery = " SELECT IdUsuario, IdTipoDocumento, IdStatus " +
                                "FROM ITM_04 " +
                                "WHERE IdUsuario = '"+ user +"' " +
                                "AND IdTipoDocumento = '"+ tpoDoc +"'";

            SqlCommand cmd = new SqlCommand(edoQuery, connect.ConectarBD);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                BtnEnviar.Enabled = false;

                LblMessage.Text = "No existe tipo de documento, para este usuario.";
                mpeMensaje.Show();
            }
            else
            {
                string edoDoc = dt.Rows[0]["IdStatus"].ToString();

                if(edoDoc == "1")
                {
                    BtnEnviar.Enabled = true;
                }
                else
                {
                    BtnEnviar.Enabled = false;
                }
            }
        }

        public void getDocRequeridos()
        {
            ConexionBD conectar = new ConexionBD();
            conectar.Abrir();

            // Consulta a la tabla Tipo de Documento
            string sqlQuery = "SELECT IdTpoDocumento, Descripcion " +
                                "FROM ITM_06 " +
                                "WHERE Status = '1'";

            SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlDocs.DataSource = dt;

            ddlDocs.DataValueField = "IdTpoDocumento";
            ddlDocs.DataTextField = "Descripcion";

            ddlDocs.DataBind();
            
            //cmd.Dispose();
            //da.Dispose();
            //conectar.Cerrar();

            //ListItem i;
            //foreach(DataRow row in dt.Rows)
            //{
            //    i = new ListItem(row["Descripcion"].ToString(), row["IdTpoDocumento"].ToString());
            //    ddlDocs.Items.Add(i);
            //}

        }

        public void getDocsUsuario()
        {
            string user = /*"USUARIO4"*/ Convert.ToString(Session["IdUsuario"]);
            ConexionBD Conectar = new ConexionBD();
            Conectar.Abrir();

            //string sqlQuery = "SELECT IdTipoDocumento, IdStatus " +
            //                    "FROM ITM_04 " +
            //                    "WHERE IdUsuario = '"+ user +"'";

            // Consulta a las tablas : Estado de Documento (Expediente) = ITM_04
            // Tipo de Documento = ITM_06
            // Status de Documento = ITM_07

            string sqlQuery = "SELECT ed.IdUsuario, ed.IdTipoDocumento, ed.Nom_Imagen, td.Descripcion, s.Descripcion as Desc_Status  " +
                                  "  FROM ITM_04 ed, ITM_06 td, ITM_07 s " +
                                  " WHERE ed.IdStatus = s.IdStatus And ed.IdTipoDocumento = td.IdTpoDocumento " +
                                  "   AND IdUsuario = '" + user + "'";

            SqlCommand ejecucion = new SqlCommand(sqlQuery, Conectar.ConectarBD);
            SqlDataAdapter da = new SqlDataAdapter(ejecucion);
            DataTable dt = new DataTable();
            da.Fill(dt);
            
            if(dt.Rows.Count == 0)
            {
                gvEstadoDocs.ShowHeaderWhenEmpty = true;
                gvEstadoDocs.EmptyDataText = "No hay resultados.";
            }
            gvEstadoDocs.DataSource = dt;
            gvEstadoDocs.DataBind();

        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                //string directorio = "https://itnowtech18-my.sharepoint.com/:f:/g/personal/llg_peacock_claims/Ekb4AdD2Id1KgMI9CRoIAU4BdP795N2YLyTJxmlMmIfWUA?e=CCtbfK" + "/";
                string directorio = "~/Directorio/";
                string user = Convert.ToString(Session["IdUsuario"]);
                string folderName = ddlDocs.SelectedValue;
                string directFinal = directorio + user + "/" + folderName + "/";
                string UrlFinal = user + "/" + folderName + "/";
                string directorioURL = Server.MapPath(directFinal);
                string nomFile = FileUpload1.FileName;

                int tamArchivo = FileUpload1.PostedFile.ContentLength;
                if (tamArchivo <= 40000000)
                {

                    ConexionBD Conectar = new ConexionBD();
                    Conectar.Abrir();

                    // Actualizar la tabla Estado de Documento
                    string sqlUpDate = "UPDATE ITM_04 " +
                                        "SET IdStatus = '2'," +
                                            " Url_Imagen = '" + UrlFinal + "'," +
                                            " Nom_Imagen = '" + nomFile + "'" +
                                        " WHERE IdUsuario = '" + user + "'" +
                                        " AND IdTipoDocumento = '" + folderName + "'";

                    SqlCommand cmd = new SqlCommand(sqlUpDate, Conectar.ConectarBD);

                    if (FileUpload1.HasFile)
                    {
                        
                        if (!Directory.Exists(directorioURL))
                        {
                            Directory.CreateDirectory(directorioURL);
                        }
                        if (File.Exists(directorioURL + nomFile))
                        {
                            //Console.WriteLine("El documento sI existe");
                            LblMessage.Text = "El documento ya existe";
                            mpeMensaje.Show();
                        }
                        else
                        {
                            FileUpload1.SaveAs(Server.MapPath(directFinal /*+ folderName + "-"*/ + FileUpload1.FileName));
                            cmd.ExecuteReader();

                            getDocsUsuario();
                            checarStatusDoc();

                            var email = new EnvioEmail();
                            string sEmail = email.CorreoElectronico(user);
                            int Envio_Ok = email.EnvioMensaje(user, sEmail, "Documento Enviado");

                            LblMessage.Text = "El documento se envio exitosamente";
                            mpeMensaje.Show();
                        }
                    }else if (tamArchivo==0)
                    {
                        LblMessage.Text = "El archivo esta dañado";
                        mpeMensaje.Show();
                    }
                    else
                    {
                        LblMessage.Text = "Debe seleccionar un archivo";
                        mpeMensaje.Show();


                        // ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                }
                else
                {
                    LblMessage.Text = "El documento Exede los 4 MB";
                    mpeMensaje.Show();

                }
            }
            catch(Exception ex)
            {
                Lbl_Message.Text = ex.Message;

            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
            
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void ddlDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            string IdDoc = ddlDocs.SelectedValue.ToString();

            LblDescrpBrev.Text = TpoDocumento_DescrpBrev(IdDoc, 1);

            checarStatusDoc();

            //ConexionBD Conectar = new ConexionBD();
            //Conectar.Abrir();

            //// Consulta a la tabla Tipo de Documento
            //string sqlQuery = "SELECT IdTpoDocumento, DescrpBrev " +
            //                    "FROM ITM_06 " +
            //                    "WHERE Status = '1'";
            //SqlCommand cmd1 = new SqlCommand(sqlQuery, Conectar.ConectarBD);

            //SqlDataAdapter da = new SqlDataAdapter(cmd1);
            //DataTable dt = new DataTable();

            //da.Fill(dt);

        }

        public string TpoDocumento_DescrpBrev(String pIdTpoDocumento, int pIdStatus)
        {
            try
            {
                string DescrpBrev = string.Empty;

                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Consulta a la tabla Tipo de Documento
                string strQuery = "Select IdTpoDocumento, DescrpBrev From ITM_06 Where IdTpoDocumento = '" + pIdTpoDocumento + "' And Status = " + pIdStatus + "";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    DescrpBrev = Convert.ToString(row[1]);
                }

                Conecta.Cerrar();

                return DescrpBrev;

            }
            catch (Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }

            return string.Empty;
        }

        protected void gvEstadoDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // int var = 0;
        }
    }
}
