using System;
using System.Configuration;
using System.IO;

using System.Runtime.InteropServices;

using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

using System.Data;
using System.Data.SqlClient;

using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using System.Windows;
using System.Windows.Forms;

namespace WebItNow
{
    public partial class Review_Document : System.Web.UI.Page
    {
        public string Descripcion { get; set; }

        // NYWVH-HT4XC-R2WYW-9Y3CM-X4V3Y
        private static Guid FolderDownloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]

        private static extern int SHGetKnownFolderPath(ref Guid id, int flags, IntPtr token, out IntPtr path);

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Carga GridView
                GetEstadoDocumentos();
            }

            //* * Agrega THEAD y TBODY a GridView.
            grdEstadoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void grdEstadoDocumento_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEstadoDocumento.PageIndex = e.NewPageIndex;
            GetEstadoDocumentos();
        }

        protected void grdEstadoDocumento_PageIndexChanged(Object sender, EventArgs e)
        {
            // Call a helper method to display the current page number 
            // when the user navigates to a different page.
            DisplayCurrentPage();
        }

        protected void grdEstadoDocumento_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.grdEstadoDocumento, "Select$" + e.Row.RowIndex.ToString()) + ";");

            string IdDescarga = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "IdDescarga"));

            if (IdDescarga == "1")
            {
                (e.Row.FindControl("imgAceptado") as ImageButton).Enabled = true;
                (e.Row.FindControl("imgRechazado") as ImageButton).Enabled = true;
            //  (e.Row.FindControl("imgDescargas") as ImageButton).Enabled = true;
            }
        }

        protected void DisplayCurrentPage()
        {
            // Calculate the current page number.
            int currentPage = grdEstadoDocumento.PageIndex + 1;

            // Display the current page number. 
            //Message.Text = "Page " + currentPage.ToString() + " of " grdEstadoDocumento.PageCount.ToString() + ".";
        }

        protected void GetEstadoDocumentos()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_04
                // Tipo de Documento = ITM_06
                // Status de Documento = ITM_07

                string strQuery = "SELECT ed.IdUsuario, ed.Nom_Imagen, td.Descripcion, ed.IdTipoDocumento, " +
                                  "       s.Descripcion as Desc_Status, ed.Url_Imagen, ed.IdDescarga " +
                                  "  FROM ITM_04 ed, ITM_06 td, ITM_07 s " +
                                  " WHERE ed.IdStatus = s.IdStatus And ed.IdTipoDocumento = td.IdTpoDocumento " +
                                  "   AND ed.IdStatus IN (2,3) ";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    grdEstadoDocumento.ShowHeaderWhenEmpty = true;
                    grdEstadoDocumento.EmptyDataText = "No hay resultados.";
                }

                grdEstadoDocumento.DataSource = dt;
                grdEstadoDocumento.DataBind();


            }
            catch (Exception ex)
            {
                Lbl_Message.Text = fnErrorMessage(ex.Message);
            }
        }

        public string fnErrorMessage(string prmMessage)
        {
            return ("<span style=\"color:Red;\">" +
                    "<img src = \"images/icons16/error.png\" height=\"16\" width=\"16\" alt=\"Error\" />&nbsp;" +
                    prmMessage + "</span>");
        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }

        protected void chkAceptado_OnCheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.CheckBox)sender).NamingContainer);
            int index = row.RowIndex;
        }

        protected void BtnUnLoad_Click(object send, EventArgs e)
        {

            /// <param name="strURLFile"> URL del archivo que se desea descargar </param>
            /// <param name="strPathToSave"> Ruta donde se desea almacenar el archivo </param>

            //string strURLFile = Server.MapPath("~/Directorio/") + "USUARIO5/OTR/" + "Captura de pantalla (3).png";
            //string strPathToSave = TxtPathDownload.Text + "\\" + "Captura de pantalla (3).png";

            string strURLFile = Server.MapPath("~/Directorio/") + "wordpress-6.1.1.zip";
            //   string strPathToSave = TxtPathDownload.Text + "/" + "wordpress-6.1.1.zip";

            //    downloadFileToSpecificPath(strURLFile, strPathToSave);

            // imgDownload.Enabled = false;
        }

        protected void imgAceptado_Click(object sender, EventArgs e)
        {
            try
            {

                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
                int index = row.RowIndex;

                string sUsuario = grdEstadoDocumento.Rows[index].Cells[3].Text;
                string sTipoDocumento = grdEstadoDocumento.Rows[index].Cells[8].Text;

                grdEstadoDocumento.Rows[index].Cells[4].Text = "Completo";

                // Actualizar en la tabla tbEstadoDocumento (Status = 3)
                Update_tbEstadoDocumento(sUsuario, sTipoDocumento, 3);

                // Actualizar en la tabla [ITM_04] (IdDescarga = 1)
                Update_ITM_04(sUsuario, sTipoDocumento, 1);

                Session["IdUsuario"] = sUsuario;
                Session["Asunto"] = "Documento Aceptado";

                Response.Redirect("Page_Message.aspx");

                //var email = new EnvioEmail();

                //// Consultar de la tabla [tbUsuarios] el [UsEmail]
                //string sEmail = email.CorreoElectronico(sUsuario);
                //int Envio_Ok = email.EnvioMensaje(sUsuario, sEmail, "Documento Aceptado ");

                //if (Envio_Ok == 0)
                //{
                //    Lbl_Message.Text = "* El documento ha sido aceptado.";
                //}
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        protected void imgRechazado_Click(object sender, EventArgs e)
        {
            try
            {

                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
                int index = row.RowIndex;


                string sUsuario = grdEstadoDocumento.Rows[index].Cells[3].Text;
                string sNom_Archivo = grdEstadoDocumento.Rows[index].Cells[6].Text;
                string sUrl_Imagen = grdEstadoDocumento.Rows[index].Cells[7].Text;
                string sTipoDocumento = grdEstadoDocumento.Rows[index].Cells[8].Text;

                string strURLFile = Server.MapPath("~/Directorio/") + sUrl_Imagen + sNom_Archivo;

                // Actualizar en la tabla [tbEstadoDocumento] (Status = 1)
                Update_tbEstadoDocumento(sUsuario, sTipoDocumento, 1);

                // Actualizar en la tabla [ITM_04] (IdDescarga = 0)
                Update_ITM_04(sUsuario, sTipoDocumento, 0);

                grdEstadoDocumento.Rows[index].Cells[4].Text = "Faltante";

                // Eliminar el archivo de Server.MapPath("~/Directorio/")
                // File.Delete(strURLFile);

                DeleteFromAzure(sNom_Archivo, sUrl_Imagen, sUsuario, sTipoDocumento);

                // Refrescar GridView
                GetEstadoDocumentos();

                Session["IdUsuario"] = sUsuario;
                Session["Asunto"] = "Documento Rechazado";

                Response.Redirect("Page_Message.aspx");

                //var email = new EnvioEmail();

                //// Consultar de la tabla [tbUsuarios] el [UsEmail]
                //string sEmail = email.CorreoElectronico(sUsuario);
                //int Envio_Ok = email.EnvioMensaje(sUsuario, sEmail, "Documento Rechazado");

                //if (Envio_Ok == 0)
                //{
                //    Lbl_Message.Text = "* El documento ha sido rechazado.";
                //}

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ImgDescarga_Click(object sender, EventArgs e)
        {
            try
            {

                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
                int index = row.RowIndex;

                string sUsuario = grdEstadoDocumento.Rows[index].Cells[3].Text;
                string sNomArchivo = grdEstadoDocumento.Rows[index].Cells[6].Text;
                string sSubdirectorio = grdEstadoDocumento.Rows[index].Cells[7].Text;
                string sTipoDocumento = grdEstadoDocumento.Rows[index].Cells[8].Text;

                Variables.wDownload = true;

                // Actualizar en la tabla [ITM_04] (IdDescarga = 1)
                Update_ITM_04(sUsuario, sTipoDocumento, 1);

                // Descargar el archivo.
                DownloadFromAzure(sNomArchivo, sSubdirectorio);

                //  imgDescarga.Enabled = false;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

        }

        protected void DeleteFromAzure(string sFilename, string sSubdirectorio, string sUsuario, string sTipoDocumento)
        {
            try
            {

                // Name of the share, directory, and file
                string ConnectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");
                string AccountName = ConfigurationManager.AppSettings.Get("StorageAccountName");
                string sDirName = "itnowstorage";

                // Obtener una referencia de nuestra parte.
                ShareClient share = new ShareClient(ConnectionString, AccountName);

                // Obtener una referencia de nuestro directorio - directorio ubicado en el nivel raíz.
                ShareDirectoryClient directory = share.GetDirectoryClient(sDirName);

                // Obtener una referencia a un subdirectorio que no se encuentra en el nivel raíz
                directory = directory.GetSubdirectoryClient(sSubdirectorio);

                // Obtener una referencia a nuestro archivo.
                ShareFileClient file = directory.GetFileClient(sFilename);

                file.Delete();

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
        }

        protected void DownloadFromAzure(string sFilename, string sSubdirectorio)
        {
            try
            {
                // Name of the share, directory, and file
                string ConnectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");
                string AccountName = ConfigurationManager.AppSettings.Get("StorageAccountName");
                string sDirName = "itnowstorage";
                string directorioURL = Server.MapPath("~/itnowstorage/" + sFilename);

                // Obtener una referencia de nuestra parte.
                ShareClient share = new ShareClient(ConnectionString, AccountName);

                // Obtener una referencia de nuestro directorio - directorio ubicado en el nivel raíz.
                ShareDirectoryClient directory = share.GetDirectoryClient(sDirName);

                // Obtener una referencia a un subdirectorio que no se encuentra en el nivel raíz
                directory = directory.GetSubdirectoryClient(sSubdirectorio);

                // Obtener una referencia a nuestro archivo.
                ShareFileClient file = directory.GetFileClient(sFilename);

                // Descargar el archivo.
                ShareFileDownloadInfo download = file.Download();
                using (FileStream stream = File.OpenWrite(directorioURL))
                {
                    download.Content.CopyTo(stream);
                    stream.Flush();
                    stream.Close();
                }

                string extension = Path.GetExtension(file.Path);
                var strMimeType = "";

                if (extension != null)
                {
                    switch (extension.ToLower())
                    {
                        case ".htm":
                        case ".html":
                            strMimeType = "text/HTML";
                            break;
                        case ".txt":
                            strMimeType = "text/plain";
                            break;
                        case ".doc":

                        case ".docx":

                        case ".rtf":
                            strMimeType = "Application/msword";
                            break;
                        case ".pdf":
                            strMimeType = "application/pdf";
                            break;
                        case ".zip":
                            strMimeType = "application/zip";
                            break;
                        case ".xlsx":
                            strMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            break;
                        case "xls":
                            strMimeType = "application/vnd.ms-excel";
                            break;

                    }

                }

                if (extension == ".xlsx")
                {
                    string direccion = directorioURL;
                    System.IO.FileStream fs = null;

                    fs = System.IO.File.Open(direccion, System.IO.FileMode.Open);
                    byte[] btFile = new byte[fs.Length];
                    fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                    fs.Close();

                    Response.AddHeader("Content-disposition", "attachment; filename=" + Path.GetFileName(directorioURL));
                    Response.ContentType = "application/octet-stream";
                    Response.BinaryWrite(btFile);
                //  Response.Flush();
                    Response.End();
                //  HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {

                    Response.Buffer = true;
                    Response.ContentType = strMimeType;
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(directorioURL));
                    Response.TransmitFile(directorioURL);
                    Response.Flush();

                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

                // Actualizar controles
                GetEstadoDocumentos();

                //  System.Threading.Thread.Sleep(5000);
                //  File.Delete(directorioURL);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }
        }

        protected void DeleteDirectorios(string sDirectorio, string sFilename)
        {
            // Name of the share, directory, and file
            string ConnectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");
            string AccountName = ConfigurationManager.AppSettings.Get("StorageAccountName");

            try
            {

                ShareClient share = new ShareClient(ConnectionString, AccountName);

                // Realizar un seguimiento de los directorios restantes,
                // comenzando desde la raíz..
                var remaining = new Queue<ShareDirectoryClient>();
                remaining.Enqueue(share.GetRootDirectoryClient());

                while (remaining.Count > 0)
                {
                    // Obtener todos los archivos y subdirectorios del siguiente directorio
                    ShareDirectoryClient dir = remaining.Dequeue();
                    foreach (ShareFileItem item in dir.GetFilesAndDirectories())
                    {
                        // Directorios
                        if (item.IsDirectory)
                        {
                            remaining.Enqueue(dir.GetSubdirectoryClient(item.Name));
                            if (item.Name == sDirectorio)
                            {
                                dir.DeleteSubdirectory(item.Name);
                            }
                        }
                        else
                        {
                            if (item.Name == sFilename)
                            {
                                dir.DeleteFile(sFilename);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void DownloadFile(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            string sNomArchivo = grdEstadoDocumento.Rows[index].Cells[6].Text;
            string sSubdirectorio = grdEstadoDocumento.Rows[index].Cells[7].Text;

            string filePath = Server.MapPath("~/Directorio/") + sSubdirectorio + sNomArchivo;
            Variables.wDownload = true;

            // DescargaArch(filePath);

            Session["FilePath"] = Server.MapPath("~/Directorio/") + sSubdirectorio + sNomArchivo;
            // Response.Redirect("Descargas.aspx", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AbrirDescarga", string.Format("window.open('Descargas.aspx');"), true);

        }

        public void DescargaArch(string filePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            // Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Método que descarga un archivo de Internet en la ruta indicada.
        /// </summary>
        /// <param name="strURLFile"> URL del archivo que se desea descargar</param>
        /// <param name="strPathToSave"> Ruta donde se desea almacenar el archivo</param>
        public static void downloadFileToSpecificPath(string strURLFile, string strPathToSave)
        {
            // Se encierra el código dentro de un bloque try-catch.
            try
            {
                // Se valida que la URL no esté en blanco.
                if (String.IsNullOrEmpty(strURLFile))
                {
                    // Se retorna un mensaje de error al usuario.
                    throw new ArgumentNullException("La dirección URL del documento es nula o se encuentra en blanco.");
                }// Fin del if que valida que la URL no esté en blanco.

                // Se valida que la ruta física no esté en blanco.
                if (String.IsNullOrEmpty(strPathToSave))
                {
                    // Se retorna un mensaje de error al usuario.
                    throw new ArgumentNullException("La ruta para almacenar el documento es nula o se encuentra en blanco.");
                }// Fin del if que valida que la ruta física no esté en blanco.

                // Se descargar el archivo indicado en la ruta específicada.
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    client.DownloadFile(strURLFile, strPathToSave);
                }// Fin del using para descargar archivos.
            }// Fin del try.
            catch (Exception ex)
            {
                // Se retorna la excepción al cliente.
                throw ex;
            }   // Fin del catch.

        }   // Fin del método downloadFileToSpecificPath.

        protected void BtnClose_Click(object send, EventArgs e)
        {

        }

        private class OldWindow : System.Windows.Forms.IWin32Window
        {
            readonly IntPtr _handle;
            public OldWindow(IntPtr handle)
            {
                _handle = handle;
            }

            #region IWin32Window Members 
            IntPtr System.Windows.Forms.IWin32Window.Handle
            {
                get { return _handle; }
            }
            #endregion
        }

        public void Update_tbEstadoDocumento(string pUsuarios, string pIdTipoDocumento, int pIdStatus)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Actualizar en la tabla Estado de Documento
                if (pIdStatus == 1)
                {
                    Variables.wQuery = "Update ITM_04 Set IdStatus = " + pIdStatus + ", Nom_Imagen = Null, Fec_Rechazado = GETDATE()" +
                                    " Where IdUsuario = '" + pUsuarios + "' And IdTipoDocumento = '" + pIdTipoDocumento + "'";
                }
                else
                {
                    Variables.wQuery = "Update ITM_04 Set IdStatus = " + pIdStatus + ", Fec_Aceptado = GETDATE() Where IdUsuario = '" + pUsuarios + "' And IdTipoDocumento = '" + pIdTipoDocumento + "'";
                }

                SqlCommand cmd1 = new SqlCommand(Variables.wQuery, Conecta.ConectarBD);
                SqlDataReader dr1 = cmd1.ExecuteReader();

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

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
        }

        public void Update_ITM_04(string pUsuarios, string pIdTipoDocumento, int pIdDescarga)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_UpDescarga", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@usuario", pUsuarios);
                cmd1.Parameters.AddWithValue("@IdTipoDocumento", pIdTipoDocumento);
                cmd1.Parameters.AddWithValue("@IdDescarga", pIdDescarga);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

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

        }

        public bool DownloadFile(string UrlString, string DescFilePath)
        {
            string fileName = System.IO.Path.GetFileName(UrlString);
            string descFilePathAndName = System.IO.Path.Combine(DescFilePath, fileName);
            try
            {
                WebRequest myre = WebRequest.Create(UrlString);
            }
            catch
            {
                return false;
            }
            try
            {
                byte[] fileData;
                using (WebClient client = new WebClient())
                {
                    fileData = client.DownloadData(UrlString);
                }
                using (FileStream fs = new FileStream(descFilePathAndName, FileMode.OpenOrCreate))
                {
                    fs.Write(fileData, 0, fileData.Length);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("download field", ex.InnerException);
            }
        }

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }

    }
}