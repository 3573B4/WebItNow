using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow
{
    public partial class Document_Notas : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                TxtReferencia.Text = Convert.ToString(Session["Referencia"]);
                TxtAsegurado.Text = Convert.ToString(Session["Asegurado"]);

                GetNotas();
            }

        }

        protected void GetNotas()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Consulta a las tablas : Notas del Documento (Expediente) = ITM_38
                string strQuery = "SELECT t1.Descripcion as Desc_Nota, t2.IdTpoNota, t2.Descripcion, t2.Fec_Entrega, t2.IdUsuario, t2.Url_Imagen, t2.Nom_Imagen, t2.IdNota" +
                                  "  FROM ITM_40 t1, ITM_38 t2" +
                                  " WHERE t1.IdTpoNota = t2.IdTpoNota" +
                                  "  And t2.Referencia = '" + TxtReferencia.Text + "'";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    GrdNotas.ShowHeaderWhenEmpty = true;
                    GrdNotas.EmptyDataText = "No hay resultados.";
                }

                GrdNotas.DataSource = dt;
                GrdNotas.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdNotas.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                Lbl_Message.Text = FnErrorMessage(ex.Message);
            }
        }

        public void GetCatalog_Notas()
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdTpoNota, Descripcion " +
                                    "FROM ITM_40 ";

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlTpoNota.DataSource = dt;

                ddlTpoNota.DataValueField = "IdTpoNota";
                ddlTpoNota.DataTextField = "Descripcion";

                ddlTpoNota.DataBind();
                ddlTpoNota.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        public string FnErrorMessage(string prmMessage)
        {
            return ("<span style=\"color:Red;\">" +
                    "<img src = \"images/icons16/error.png\" height=\"16\" width=\"16\" alt=\"Error\" />&nbsp;" +
                    prmMessage + "</span>");
        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            //System.Web.Security.FormsAuthentication.SignOut();
            //Session.Abandon();

            Response.Redirect("Expediente.aspx", true);
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void GrdNotas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GrdNotas_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdNotas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdNotas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdNotas, "Select$" + e.Row.RowIndex.ToString()) + ";");
        }

        protected void ImgEnvioArchivo_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wIdNota = Convert.ToInt32(GrdNotas.Rows[index].Cells[8].Text);

            mpeNewEnvio.Show();
        }

        protected void ImgVerArchivo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
                int index = row.RowIndex;

                Variables.wURL_Imagen = GrdNotas.Rows[index].Cells[6].Text;
                Variables.wFileName = GrdNotas.Rows[index].Cells[7].Text;
                Variables.wIdNota = Convert.ToInt32(GrdNotas.Rows[index].Cells[8].Text);

                string sFileName = Variables.wFileName;
                string sSubdirectorio = Variables.wURL_Imagen;

                HttpContext.Current.Session["sFileName"] = Variables.wFileName;
                HttpContext.Current.Session["Subdirectorio"] = Variables.wURL_Imagen;

                System.Threading.Thread.Sleep(1000);
                this.DescargaFromAzure_(sFileName, sSubdirectorio);

                string mensaje = "window.open('Descargas.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", mensaje, true);

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {
                RefreshPage();
            }
        }

        protected void RefreshPage()
        {
            Response.AppendHeader("Refresh", "0;URL=" + HttpContext.Current.Request.Url.AbsoluteUri);
        }

        public void UploadToAzure(string sFilename, string sDirName, string sTpoDocumento, int iIdNota)
        {

            string ConnectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");
            string AccountName = ConfigurationManager.AppSettings.Get("StorageAccountName");

            try
            {
                // Get a reference to a share and then create it
                ShareClient share = new ShareClient(ConnectionString, AccountName);

                // Get a reference to a directory and create it
                ShareDirectoryClient directory = share.GetDirectoryClient(AccountName);

                // Get a reference to a subdirectory not located on root level
                directory = directory.GetSubdirectoryClient(sDirName + "/" + sTpoDocumento);

                if (!directory.Exists())
                {
                    directory = directory.GetSubdirectoryClient("../");
                    directory.CreateSubdirectory(sTpoDocumento);
                    directory = directory.GetSubdirectoryClient(sTpoDocumento);
                }

                // Get a reference to a file and upload it
                ShareFileClient file = directory.GetFileClient(sFilename);

                if (file.Exists())
                {
                    //Variables.wExiste = true;

                    LblMessage.Text = "El documento ya existe";
                    mpeMensaje.Show();
                    return;
                }
                else
                {

                    //Access the File using the Name of HTML INPUT File.
                    HttpPostedFile postedFile = Request.Files["oFile"];

                    file.Create(postedFile.ContentLength);

                    int blockSize = 64 * 1024;
                    long offset = 0;    // Definir desplazamiento de rango http.

                    BinaryReader reader = new BinaryReader(postedFile.InputStream);
                    while (true)
                    {
                        byte[] buffer = reader.ReadBytes(blockSize);
                        if (buffer.Length == 0)
                            break;

                        MemoryStream uploadChunk = new MemoryStream();
                        uploadChunk.Write(buffer, 0, buffer.Length);
                        uploadChunk.Position = 0;

                        HttpRange httpRange = new HttpRange(offset, buffer.Length);
                        var resp = file.UploadRange(httpRange, uploadChunk);
                        offset += buffer.Length;    // Cambia el desplazamiento por el número de bytes ya escritos.
                    }

                    reader.Close();

                    string sUrl_Imagen = sDirName + "/" + sTpoDocumento;
                    string sReferencia = TxtReferencia.Text;

                    ConexionBD Conectar = new ConexionBD();
                    Conectar.Abrir();

                    // Actualizar la tabla Documentos Archivo
                    string sqlUpDate = "UPDATE ITM_38 " +
                                        "    SET Url_Imagen = '" + sUrl_Imagen + "'," +
                                            "    Nom_Imagen = '" + sFilename + "'," +
                                            "   Fec_Entrega = GETDATE(), " +
                                            "   Entregado = '1'" +
                                        " WHERE Referencia LIKE '%' + '" + sReferencia + "'  + '%'" +
                                        "   AND IdNota = " + iIdNota + "";

                    SqlCommand cmd = new SqlCommand(sqlUpDate, Conectar.ConectarBD);

                    cmd.ExecuteReader();
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void DescargaFromAzure_(string sFileName, string sSubdirectorio)
        {

            try
            {

                string ConnectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");
                string shareName = ConfigurationManager.AppSettings.Get("StorageAccountName");
                string dirName = "/itnowstorage-desarrollo/" + sSubdirectorio;

                // Get a reference to the file
                ShareClient share = new ShareClient(ConnectionString, shareName);
                ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
                ShareFileClient file = directory.GetFileClient(sFileName);

                // Download the file
                ShareFileDownloadInfo download = file.Download();

                var memoryStream = new MemoryStream();
                download.Content.CopyTo(memoryStream);

                memoryStream.Flush();
                memoryStream.Close();

                HttpContext.Current.Session["Array"] = memoryStream.ToArray();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();

                return;
            }
            finally
            {

            }
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                string sReferencia = TxtReferencia.Text;

                // Acceda al archivo usando el nombre del archivo de entrada HTML.
                HttpPostedFile postedFile = Request.Files["oFile"];

                string nomFile = postedFile.FileName;

                if (postedFile.FileName != "")
                {
                    this.UploadToAzure(nomFile, sReferencia, "Notas", Variables.wIdNota);
                }
                else
                {
                    LblMessage.Text = "Debe seleccionar un archivo";
                    mpeMensaje.Show();
                    return;
                }

                GetNotas();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                // mpeMensaje.Show();
                return;
            }
        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void ddlTpoNota_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_New_Click(object sender, EventArgs e)
        {
            string sReferencia = TxtReferencia.Text;
            string sIdTpoNota = ddlTpoNota.SelectedValue;
            string sDescripcion = TxtDescNota.Text;
            string sUsuario = "";

            int Envio_Ok = Add_tbDetalleNotas(sReferencia, sIdTpoNota, sDescripcion, sUsuario);

            if (Envio_Ok == 0)
            {
                GetNotas();
            }
        }

        public int Add_tbDetalleNotas(string pReferencia, string pIdTpoDocumento, string pDescripcion, string pUsuario)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string strQuery = "Insert into ITM_38 (Referencia, IdTpoNota, Descripcion, IdUsuario, Url_Imagen, Url_Path_Final, Nom_Imagen, Nom_Imagen_Final, Fec_Entrega, Entregado)" +
                                  "Values ('" + pReferencia + "', '" + pIdTpoDocumento + "', '" + pDescripcion + "', '" + pUsuario + "', Null, Null, Null, Null, Null, 0)" + "\n \n";

                // Insert en la tabla Estado de Documento
                SqlCommand cmd1 = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

                return 0;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }
            return -1;
        }

        protected void BtnCerrar_New_Click(object sender, EventArgs e)
        {

        }

        protected void BtnDocument_Nuevo_Click(object sender, EventArgs e)
        {
            GetCatalog_Notas();
            mpeNewNota.Show();
        }


    }
}