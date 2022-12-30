
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
                getDocRequeridos();
                getDocsUsuario();
                string userId = Convert.ToString(Session["IdUsuario"]);
                lblUsuario.Text = userId;
            }
        }
        public void getDocRequeridos()
        {
            ConexionBD conectar = new ConexionBD();
            conectar.Abrir();

            string sqlQuery = "SELECT * " +
                                "FROM tbTpoDocumento " +
                                "WHERE Status = '1'";

            SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);

            ddlDocs.DataSource = cmd.ExecuteReader();
            ddlDocs.DataValueField = "IdTpoDocumento";
            ddlDocs.DataTextField = "Descripcion";
            ddlDocs.DataBind();

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

            string directorio = "~/Directorio/";
            string user = /*"USUARIO4"*/ Convert.ToString(Session["IdUsuario"]);
            string folderName = ddlDocs.SelectedValue;
            string directFinal = directorio + user + "/" + folderName + "/";
            string UrlFinal = user + "/" + folderName + "/";
            string directorioURL = Server.MapPath(directFinal);
            string nomFile = /*folderName + "-" +*/ FileUpload1.FileName;
            int tamArchivo = FileUpload1.PostedFile.ContentLength;

            if (tamArchivo <= 40960000)
            {

                ConexionBD Conectar = new ConexionBD();
                Conectar.Abrir();

                string sqlUpDate = "UPDATE tbEstadoDocumento " +
                                    "SET IdStatus = '2'," +
                                        " Url_Imagen = '" + UrlFinal + "'," +
                                        " Nom_Imagen = '" + nomFile + "'" +
                                    " WHERE IdUsuario = '" + user + "'" +
                                    " AND IdTipoDocumento = '" + folderName + "'";

                SqlCommand cmd = new SqlCommand(sqlUpDate, Conectar.ConectarBD);

                if (FileUpload1.HasFile)
                {
                    if (System.IO.Directory.Exists(directorioURL))
                    {
                        if (File.Exists(directorioURL + nomFile))
                        {
                            //Console.WriteLine("El documento sI existe");
                            LblMessage.Text = "El documento ya existe";
                            this.mpeMensaje.Show();
                        }
                        else
                        {
                            FileUpload1.SaveAs(Server.MapPath(directFinal /*+ folderName + "-"*/ + FileUpload1.FileName));
                            //Lbl_Message.Text = "EL archivo se subio exitosamente";
                            cmd.ExecuteReader();

                            LblMessage.Text = "El documento se subio exitosamente";
                            this.mpeMensaje.Show();
                        }

                    }
                    else
                    {
                        System.IO.Directory.CreateDirectory(directorioURL);
                        FileUpload1.SaveAs(Server.MapPath(directFinal /*+ folderName + "-"*/ + FileUpload1.FileName));
                        //Lbl_Message.Text = "EL archivo se subio exitosamente";
                        cmd.ExecuteReader();

                        LblMessage.Text = "El documento se subio exitosamente";
                        this.mpeMensaje.Show();
                    }
                }
                else
                {
                    Lbl_Message.Text = "Debe seleccionar un archivo";
                }
            }
            else
            {
                LblMessage.Text = "El documento excede de 40 MB";
                this.mpeMensaje.Show();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
            
        }

        protected void ddlDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nombre = ddlDocs.SelectedValue.ToString();
        }

        protected void FileUpload1_Click(object sender, EventArgs e)
        {
            FileUpload fileUpload1 = new FileUpload();

            if (FileUpload1.PostedFile != null)
            {
                int tamArchivo = FileUpload1.PostedFile.ContentLength;

                if (tamArchivo <= 40960000)
                {

                }
                else
                {
                    LblMessage.Text = "El documento excede de 40 MB";
                    this.mpeMensaje.Show();
                }
            }
        }
    }
}
