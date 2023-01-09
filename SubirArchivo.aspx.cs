
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

            string edoQuery = " SELECT IdUsuario, IdTipoDocumento, IdStatus " +
                                "FROM tbEstadoDocumento " +
                                "WHERE IdUsuario = '"+ user +"' " +
                                "AND IdTipoDocumento = '"+ tpoDoc +"'";
            SqlCommand cmd = new SqlCommand(edoQuery, connect.ConectarBD);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

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
        public void getDocRequeridos()
        {
            ConexionBD conectar = new ConexionBD();
            conectar.Abrir();

            string sqlQuery = "SELECT IdTpoDocumento, Descripcion " +
                                "FROM tbTpoDocumento " +
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
            //                    "FROM tbEstadoDocumento " +
            //                    "WHERE IdUsuario = '"+ user +"'";

            string sqlQuery = "SELECT ed.IdUsuario, ed.IdTipoDocumento, td.Descripcion, s.Descripcion as Desc_Status  " +
                                  "  FROM tbEstadoDocumento ed, tbTpoDocumento td, tbStatus s " +
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
                string User = /*"USUARIO4"*/ Convert.ToString(Session["IdUsuario"]);
                string folderName = ddlDocs.SelectedValue;
                string directFinal = directorio + User + "/" + folderName + "/";
                string UrlFinal = User + "/" + folderName + "/";
                string directorioURL = Server.MapPath(directFinal);
                string nomFile = /*folderName + "-" +*/ FileUpload1.FileName;

                int tamArchivo = FileUpload1.PostedFile.ContentLength;
                if (tamArchivo <= 40000000)
                {

                    ConexionBD Conectar = new ConexionBD();
                    Conectar.Abrir();

                    string sqlUpDate = "UPDATE tbEstadoDocumento " +
                                        "SET IdStatus = '2'," +
                                            " Url_Imagen = '" + UrlFinal + "'," +
                                            " Nom_Imagen = '" + nomFile + "'" +
                                        " WHERE IdUsuario = '" + User + "'" +
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
                            //Lbl_Message.Text = "EL archivo se subio exitosamente";
                            cmd.ExecuteReader();
                            getDocsUsuario();

                            var email = new EnvioEmail();

                            // Consultar de la tabla [tbUsuarios] el [UsEmail]
                            string sEmail = email.CorreoElectronico(User);
                            int Envio_Ok = email.EnvioMensaje(User, sEmail, "Documento Enviado ");

                            LblMessage.Text = "El documento se subio exitosamente";
                            mpeMensaje.Show();

                            //BtnEnviar.Enabled = false;
                        }
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

            ConexionBD Conectar = new ConexionBD();
            Conectar.Abrir();

            string sqlQuery = "SELECT IdTpoDocumento, DescrpBrev " +
                                "FROM tbTpoDocumento " +
                                "WHERE Status = '1'";
            SqlCommand cmd1 = new SqlCommand(sqlQuery, Conectar.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();

            da.Fill(dt);


        }

        protected void gvEstadoDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int var = 0;
        }
    }
}
