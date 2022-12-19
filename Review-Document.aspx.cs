using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using WebGrease.Css;

namespace WebItNow
{
    public partial class Fotos : System.Web.UI.Page
    {
        //string varGalleryFolder = "C:\\inetpub\\wwwroot\\WebItNow\\images\\Gallery";
        string varGalleryFolder = System.Web.HttpContext.Current.Server.MapPath("~/Directorio/");
        
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                // Valor del usuario viene de la seccion login
                string IdUsuario = Convert.ToString(Session["IdUsuario"]);
                
                varGalleryFolder = varGalleryFolder + "USUARIO3";

                if (!Page.IsPostBack)
                {
                    // Cargar DropDownList
                    GetTpoDocumento();

                    // Carga GridView
                    GetEstadoDocumentos();

                    //GetPhotos(cboTpoDocumento.SelectedValue.ToString());
                    //GetFolders(varGalleryFolder);
                    //GetFiles(varGalleryFolder + "\\" + cboTpoDocumento.SelectedValue.ToString());
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = fnErrorMessage(ex.Message);
            }
        }

        protected void GetTpoDocumento()
        {
            try
            {
                //DataSet dsTpoDocumento = new DataSet("PhotoGallery");
                //dsTpoDocumento.ReadXml(Server.MapPath("App_Data\\Gallery.xml"));

                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                SqlCommand cmd = new SqlCommand("SELECT IdTpoDocumento, Descripcion FROM tbTpoDocumento", Conecta.ConectarBD);
                SqlDataReader dr = cmd.ExecuteReader();

                cboTpoDocumento.DataSource = dr;
                cboTpoDocumento.DataTextField = "Descripcion";
                cboTpoDocumento.DataValueField = "IdTpoDocumento";
                cboTpoDocumento.DataBind();

                // dlsTpoDocumento.DataBind();
                // dsTpoDocumento.Dispose();

                cmd.Dispose();
                dr.Dispose();

                Conecta.Cerrar();

            }
            catch (Exception ex)
            {
                lblMessage.Text = fnErrorMessage(ex.Message);
            }
        }

        protected void GetEstadoDocumentos() 
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            string strQuery = "SELECT IdUsuario, IdTipoDocumento, s.Descripcion " +
                              "  FROM tbEstadoDocumento ed, tbStatus s " +
                              " WHERE ed.IdStatus = s.IdStatus";

            SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            grdEstadoDocumento.DataSource = dt;
            grdEstadoDocumento.DataBind();

        }

        protected void grdEstadoDocumento_PageIndexChanged(Object sender, EventArgs e)
        {
            // Call a helper method to display the current page number 
            // when the user navigates to a different page.
            DisplayCurrentPage();
        }

        protected void DisplayCurrentPage()
        {
            // Calculate the current page number.
            int currentPage = grdEstadoDocumento.PageIndex + 1;

            // Display the current page number. 
            //Message.Text = "Page " + currentPage.ToString() + " of " grdEstadoDocumento.PageCount.ToString() + ".";
        }

        protected string fnFilePath(string prmPath, string prmFileName)
        {
            return ("Directorio/USUARIO3/" + prmPath + "/" + prmFileName);
        }

        protected void GetFolders(string prmFolder)
        {
            try
            {
                if (Directory.Exists(prmFolder))
                {
                    string[] arrFolders;
                    arrFolders = Directory.GetDirectories(prmFolder);
                    foreach (string dir in arrFolders)
                    {
                        ListItem lst = new ListItem(dir.Substring(prmFolder.Length + 1),
                        dir.Substring(prmFolder.Length + 1));
                        cboTpoDocumento.Items.Add(lst);
                    }
                }
                else
                {
                    lblMessage.Text = fnErrorMessage("No existe el directorio de imágenes");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = fnErrorMessage(ex.Message);
            }
        }

        protected void GetFiles(string prmFolder)
        {
            try
            {
                string[] arrFiles;
                arrFiles = Directory.GetFiles(prmFolder, "*.jpg");

                DataSet dsDocument = new DataSet("dsDocument");
                DataTable taFile;
                DataRow rwFile;
                
                //DataColumn colFile;
                taFile = dsDocument.Tables.Add("Files");
                taFile.Columns.Add("IdUsuario", varGalleryFolder.GetType());
                taFile.Columns.Add("FileName", varGalleryFolder.GetType());
                taFile.Columns.Add("FileDescription", varGalleryFolder.GetType());

                foreach (string file in arrFiles)
                {
                    //Aqui llenamos el Dataset
                    rwFile = taFile.NewRow();
                    rwFile["IdUsuario"] = prmFolder.Substring(varGalleryFolder.Length + 1); ;
                    rwFile["FileName"] = file.Substring(prmFolder.Length + 1);
                    rwFile["FileDescription"] = file.Substring(prmFolder.Length + 1);
                    taFile.Rows.Add(rwFile);
                }

                dblstTpoDocumento.DataSource = dsDocument.Tables["Files"];

                if (dsDocument.Tables["Files"].Rows.Count == 0)
                {
                    lblMessage.Text = fnErrorMessage("No se encontraron imágenes");
                }
                else
                {
                   // lblMessage.Text = fnInfoMessage("Se encontraron " dsDocument.Tables["Files"].Rows.Count.ToString() + " imágenes");
                }

                dblstTpoDocumento.DataBind();
                dsDocument.Dispose();
            }
            catch (Exception ex)
            {
                lblMessage.Text = fnErrorMessage(ex.Message);
            }
        }

        protected void GetPhotos(string prmGallery)
        {
            try
            {
                DataSet dsDocument = new DataSet("PhotoGallery");
                dsDocument.ReadXml(Server.MapPath("App_Data\\Gallery.xml"));
                DataView dvGallery = dsDocument.Tables["Picture"].DefaultView;
                dvGallery.RowFilter = "IdUsuario='" + prmGallery + "'";
                dblstTpoDocumento.DataSource = dvGallery;

                if (dvGallery.Count == 0)
                {
                    lblMessage.Text = fnErrorMessage("No se encontraron imágenes");
                }
                else
                {
                    lblMessage.Text = fnInfoMessage("Se encontraron " + dvGallery.Count.ToString() +
                        " imágenes");
                }
                dblstTpoDocumento.DataBind();

                dsDocument.Dispose();
            }
            catch (Exception ex)
            {
                lblMessage.Text = fnErrorMessage(ex.Message);
            }
        }

        protected void cboTpoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            //GetPhotos(cboGallery.SelectedValue.ToString());
            GetFiles(varGalleryFolder + "\\" + cboTpoDocumento.SelectedValue.ToString());
        }

        protected void dblstTpoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public string fnErrorMessage(string prmMessage)
        {
            return ("<span style=\"color:Red;\">" +
                    "<img src = \"images/icons16/error.png\" height=\"16\" width=\"16\" alt=\"Error\" />&nbsp;" +
                    prmMessage + "</span>");
        }

        public string fnInfoMessage(string prmMessage)
        {
            return ("<span style=\"color:Blue;\">" +
                    "<img src = \"images/icons16/information.png\" height=\"16\" width=\"16\" alt=\"Información\" />&nbsp;" +
                    prmMessage + "</span>");
        }

        public string fnNormalMessage(string prmMessage)
        {
            return ("<span style=\"vertical-align:center\">" +
                    "<img src = \"images/icons16/information.png\" height=\"16\" width=\"16\" alt=\"Información\" />&nbsp;" +
                    prmMessage + "</span>");
        }

        public static string SafeSqlLikeClauseLiteral(string prmSQLString)
        {
            string s = prmSQLString;
            s = s.Replace("'", "''");
            s = s.Replace("[", "[[]");
            s = s.Replace("%", "[%]");
            s = s.Replace("_", "[_]");
            return (s);
        }

        public static string fnYESNO(bool prmValue)
        {
            if (prmValue)
            {
                return ("SI");
            }
            else
            {
                return ("NO");
            }
        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }

        protected void grdEstadoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            string varGalleryFolder = System.Web.HttpContext.Current.Server.MapPath("~/Directorio/");

           // string IdUsuario = grdEstadoDocumento.SelectedRow.Cells[1].Text;

            varGalleryFolder = varGalleryFolder + grdEstadoDocumento.SelectedRow.Cells[1].Text;

            GetFiles(varGalleryFolder + "\\" + grdEstadoDocumento.SelectedRow.Cells[2].Text);

            // Carga GridView
            GetEstadoDocumentos();

        }

        }
}