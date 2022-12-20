
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
                string userId = Convert.ToString(Session["IdUsuario"]);
            }
        }
        public void getDocRequeridos()
        {
            ConexionBD conectar = new ConexionBD();
            conectar.Abrir();

            string sqlQuery = "SELECT IdTpoDocumento, Descripcion " +
                                "FROM tbTpoDocumento";

            SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);

            ddlDocs.DataSource = cmd.ExecuteReader();
            ddlDocs.DataValueField = "IdTpoDocumento";
            ddlDocs.DataTextField = "Descripcion";
            ddlDocs.DataBind();

            
        }
        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            string user = /*"USUARIO4"*/ Convert.ToString(Session["IdUsuario"]);
            string folderName = ddlDocs.SelectedValue;
            ConexionBD Conectar = new ConexionBD();
            Conectar.Abrir();

            string sqlUpDate = "UPDATE tbEstadoDocumento " +
                                "SET IdStatus = '2'" +
                                "WHERE IdUsuario = '" + user + "'" +
                                "AND IdTipoDocumento = '" + folderName + "'";

            SqlCommand cmd = new SqlCommand(sqlUpDate, Conectar.ConectarBD);

            string directorio = "./Directorio/";
            
            string directFinal = directorio + user + "/" + folderName + "/";
            string directorioURL = Server.MapPath(directFinal);
            string nomFile = folderName + "-" + FileUpload1.FileName;

            if (FileUpload1.HasFile)
            {
                if (System.IO.Directory.Exists(directorioURL))
                {
                    if (File.Exists(directorioURL + nomFile))
                    {
                        //Console.WriteLine("El documento sI existe");
                        Lbl_Message.Text = "El documento ya existe";
                    }
                    else
                    {
                        
                        FileUpload1.SaveAs(Server.MapPath(directFinal + folderName + "-" + FileUpload1.FileName));
                        //Lbl_Message.Text = "EL archivo se subio exitosamente";
                        cmd.ExecuteReader();
                        this.mpeMensaje.Show();
                    }

                }
                else
                {
                    System.IO.Directory.CreateDirectory(directorioURL);
                    FileUpload1.SaveAs(Server.MapPath(directFinal + folderName + "-" + FileUpload1.FileName));
                    //Lbl_Message.Text = "EL archivo se subio exitosamente";
                    cmd.ExecuteReader();
                    this.mpeMensaje.Show();
                }
            }
            else
            {
                Lbl_Message.Text = "Debe seleccionar un archivo";
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
    }
}
