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
    public partial class Document_Files : System.Web.UI.Page
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
                Variables.wProceso = Convert.ToInt32(Session["Proceso"]);

                GetTpoDocumentos();
            }

        }

        protected void GetTpoDocumentos()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_37

                string strQuery = "SELECT da.Entregado, td.Descripcion, da.Descripcion as Desc_Documento, da.Fec_Entrega, td.IdTpoDocumento, da.Url_Imagen, da.Nom_Imagen " +
                                  "  FROM ITM_08 td, ITM_37 da" +
                                  " WHERE td.IdTpoDocumento = da.IdTpoDocumento" +
                                  "   AND td.IdStatus = 1" +
                                  "  And da.Referencia = '" + TxtReferencia.Text + "'";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    GrdTpoDocumento.ShowHeaderWhenEmpty = true;
                    GrdTpoDocumento.EmptyDataText = "No hay resultados.";
                }

                GrdTpoDocumento.DataSource = dt;
                GrdTpoDocumento.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdTpoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                Lbl_Message.Text = FnErrorMessage(ex.Message);
            }
        }

        protected void GetBusqProceso()
        {
            try
            {
                int iProceso = Convert.ToInt32(Session["Proceso"]);
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_12
                string strQuery = "SELECT p.Descripcion as Desc_Proceso, d.Descripcion " +
                                  "  FROM ITM_08 as p, ITM_12 as d " +
                                  " WHERE p.IdTpoDocumento = d.IdTpoDocumento " +
                                  "   AND d.IdProceso = "+ iProceso +" " +
                                  "   AND d.IdTpoDocumento = 12 " +
                                  "   AND d.Descripcion like '%" + txtPnlBusqProceso.Text + "%'";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    grdPnlBusqProceso.ShowHeaderWhenEmpty = true;
                    grdPnlBusqProceso.EmptyDataText = "No hay resultados.";
                }

                grdPnlBusqProceso.DataSource = dt;
                grdPnlBusqProceso.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                grdPnlBusqProceso.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetCatalog_Documento()
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdTpoDocumento, Descripcion " +
                                    "FROM ITM_08 " +
                                    "WHERE IdTpoDocumento IN (09,10,11,12,13,14,15)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlTpoDocumento.DataSource = dt;

                ddlTpoDocumento.DataValueField = "IdTpoDocumento";
                ddlTpoDocumento.DataTextField = "Descripcion";

                ddlTpoDocumento.DataBind();
                ddlTpoDocumento.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

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

        protected void BtnDocument_Proceso_Click(object sender, EventArgs e)
        {
            try
            {
                GetBusqProceso();
                mpeNewProceso.Show();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ImgEnvioArchivo_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wTpoDocumento = GrdTpoDocumento.Rows[index].Cells[6].Text;

            mpeNewEnvio.Show();
        }

        protected void ImgVerArchivo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
                int index = row.RowIndex;

                Variables.wTpoDocumento = GrdTpoDocumento.Rows[index].Cells[6].Text;
                Variables.wURL_Imagen = GrdTpoDocumento.Rows[index].Cells[7].Text;
                Variables.wFileName = GrdTpoDocumento.Rows[index].Cells[8].Text;

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

        protected void GrdTpoDocumento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GrdTpoDocumento_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdTpoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            Variables.wTpoDocumento = Server.HtmlDecode(GrdTpoDocumento.SelectedRow.Cells[6].Text);
            this.hdfValorGrid.Value = this.GrdTpoDocumento.SelectedValue.ToString();
        }

        protected void GrdTpoDocumento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdTpoDocumento, "Select$" + e.Row.RowIndex.ToString()) + ";");
        }

        public void UploadToAzure(string sFilename, string sDirName, string sTpoDocumento)
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
                    string sqlUpDate = "UPDATE ITM_37 " +
                                        "    SET Entregado = '1'," +
                                            " Url_Imagen = '" + sUrl_Imagen + "'," +
                                            " Nom_Imagen = '" + sFilename + "'," +
                                            "  Fec_Entrega = GETDATE() " +
                                        " WHERE Referencia LIKE '%' + '" + sReferencia + "'  + '%' " +
                                        " AND IdTpoDocumento = '" + Variables.wTpoDocumento + "'";

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
                    if(Variables.wTpoDocumento == "10")
                    {
                        this.UploadToAzure(nomFile, sReferencia, "Correos");
                    }else if (Variables.wTpoDocumento == "14")
                    {
                        this.UploadToAzure(nomFile, sReferencia, "Polizas");
                    }
                }
                else
                {
                    LblMessage.Text = "Debe seleccionar un archivo";
                    mpeMensaje.Show();
                    return;
                }

                GetTpoDocumentos();
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

        protected void BtnDocument_Nuevo_Click(object sender, EventArgs e)
        {
            GetCatalog_Documento();
            mpeNewDocumento.Show();
        }

        protected void BtnAceptar_New_Click(object sender, EventArgs e)
        {
            string sReferencia = TxtReferencia.Text;
            string sIdTpoDocumento = ddlTpoDocumento.SelectedValue;
            string sDescripcion = TxtDescripcion.Text;

            int Envio_Ok = Add_tbDetalleArchivos(sReferencia, sIdTpoDocumento, sDescripcion);

            if (Envio_Ok == 0)
            {
                GetTpoDocumentos();

                //LblMessage.Text = "Su registro fue creado exitosamente";
                //this.mpeMensaje.Show();
            }
        }

        protected void BtnCerrar_New_Click(object sender, EventArgs e)
        {

        }

        protected void ddlTpoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sIdTpoDocumento = ddlTpoDocumento.SelectedValue;
        }

        public int Add_tbDetalleArchivos(string pReferencia, string pIdTpoDocumento, string pDescripcion)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string strQuery = "Insert into ITM_37 (Referencia, IdTpoDocumento, Descripcion, Url_Imagen, Url_Path_Final, Nom_Imagen, Nom_Imagen_Final, Fec_Entrega, Entregado)" +
                                  "Values ('" + pReferencia + "', '" + pIdTpoDocumento + "', '" + pDescripcion + "', Null, Null, Null, Null, Null, 0)" + "\n \n";

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

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdPnlBusqProceso.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {

                    var chkbox = row.FindControl("ChBoxRow") as CheckBox;

                    if (chkbox.Checked)
                    {
                        string Descripcion = Server.HtmlDecode(Convert.ToString(row.Cells[2].Text));
                        ConexionBD Conecta = new ConexionBD();
                        Conecta.Abrir();

                        string strQuery = "INSERT INTO ITM_37 (Referencia, IdTpoDocumento, Descripcion, Entregado) " +
                                          " VALUES ('" + TxtReferencia.Text + "', '12', '" + Descripcion + "', 0)";

                        SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                        cmd.ExecuteReader();
                    }
                }
            }

            GetTpoDocumentos();
        }

        protected void btnClose_Proceso_Click(object sender, EventArgs e)
        {

        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            GetBusqProceso();
            mpeNewProceso.Show();
        }
    }
}