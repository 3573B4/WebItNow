using System;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.IO.IsolatedStorage;

using System.Runtime.InteropServices;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using System.Net;
using System.Text;

using System.Diagnostics;
using System.Reflection;
//using System.Windows.Forms;

using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;

using System.Threading;
using Azure.Storage.Files.Shares;
using Microsoft.WindowsAzure.Storage.File;

namespace WebItNow
{

	public partial class Review_Document : System.Web.UI.Page
	{
        public string Descripcion { get; set; }

        // NYWVH-HT4XC-R2WYW-9Y3CM-X4V3Y
        private static Guid FolderDownloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");
        private object isoStore;
        private object share;

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
               // TxtPathDownload.Text = GetDownloadsPath();

                // Carga GridView
                GetEstadoDocumentos();
                // itnowstorage - Nombre de la cuenta de almacenamiento Azure
            }

            if (Variables.wDownload == true)
            {
                TxtUsu.Text = string.Empty;
                TxtTpoDocumento.Text = string.Empty;
                TxtNomArchivo.Text = string.Empty;
                TxtUrl_Imagen.Text = string.Empty;

                Variables.wDownload = false;
            }


            //* * Agrega THEAD y TBODY a GridView.
            grdEstadoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        public static string GetDownloadsPath()
        {
            if (Environment.OSVersion.Version.Major < 6) throw new NotSupportedException();

            IntPtr pathPtr = IntPtr.Zero;

            try
            {
                SHGetKnownFolderPath(ref FolderDownloads, 0, IntPtr.Zero, out pathPtr);
                return Marshal.PtrToStringUni(pathPtr);
            }
            finally
            {
                Marshal.FreeCoTaskMem(pathPtr);
            }
        }

        protected void grdEstadoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtUsu.Text = grdEstadoDocumento.SelectedRow.Cells[2].Text;
            TxtTpoDocumento.Text = grdEstadoDocumento.SelectedRow.Cells[4].Text;
            TxtNomArchivo.Text = grdEstadoDocumento.SelectedRow.Cells[5].Text;
            TxtUrl_Imagen.Text = grdEstadoDocumento.SelectedRow.Cells[6].Text;

            this.hdfValorGrid.Value = this.grdEstadoDocumento.SelectedValue.ToString();
        }

        //protected void grdEstadoDocumento_RowDataBound(object sender, GridViewCommandEventArgs e)
        //{

        //    if (e.CommandName == "Select")
        //    {
        //        //
        //        // Se obtiene indice de la row seleccionada
        //        //
        //        int index = Convert.ToInt32(e.CommandArgument);

        //        //
        //        // Obtengo el id de la entidad que se esta editando
        //        // en este caso de la entidad Person
        //        //
        //        int id = Convert.ToInt32(grdEstadoDocumento.DataKeys[index].Value);

        //    }

        //}

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
            }
        }

        protected void grdEstadoDocumento_DataBound(object sender, EventArgs e)
        {
            //grdEstadoDocumento = ((GridView)sender);
            //for (int i = 0; i < grdEstadoDocumento.Columns.Count; i++)
            //{
            //    grdEstadoDocumento.Columns[i].ItemStyle.Width = Unit.Percentage(100 / (grdEstadoDocumento.Columns.Count));
            //}
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

        protected void imgDownload_Click(object sender, ImageClickEventArgs e)
        {
            //Thread thdSyncRead = new Thread(new ThreadStart(OpenFolder));
            //thdSyncRead.SetApartmentState(ApartmentState.STA);
            //thdSyncRead.Start();

            try
            {
                /// <param name="strURLFile"> URL del archivo que se desea descargar </param>
                /// <param name="strPathToSave"> Ruta donde se desea almacenar el archivo </param>

                // System.Web.HttpContext.Current.Session["FileName"] = TxtNomArchivo.Text;
                // string dlDir = "";

                string strURLFile = Server.MapPath("~/Directorio/") + TxtUrl_Imagen.Text + TxtNomArchivo.Text;
             //   string strPathToSave = TxtPathDownload.Text + "\\"  + TxtNomArchivo.Text;

             // downloadFileToSpecificPath(strURLFile, strPathToSave);
             // imgDownload.Enabled = false;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
            }
            finally
            {
                LblMessage.Text = "El documento se descargo exitosamente";
                this.mpeMensaje.Show();
            }

        }

        protected void ImgDescarga_Click(object send, EventArgs e)
        {
            if (TxtNomArchivo.Text == "")
            {
                LblMessage.Text = "Seleccione el archivo a descargar";
                this.mpeMensaje.Show();
            }
            else
            {
                string sFilename = TxtNomArchivo.Text;
                string sSubdirectorio = TxtUrl_Imagen.Text;
                Variables.wDownload = true;

                // Actualizar en la tabla [ITM_04] (IdDescarga = 1)
                string sTipoDocumento = "OTR";
                Update_ITM_04(TxtUsu.Text, sTipoDocumento, 1);

                // Descargar el archivo.
                DownloadFromAzure(sFilename, sSubdirectorio);

                //string filePath = Server.MapPath("~/Directorio/") + TxtUrl_Imagen.Text + TxtNomArchivo.Text;

                //bool fileExist = File.Exists(filePath);

                //if (fileExist)
                //{
                //  DescargaArch(filePath);
                //}
                //else
                //{
                //    LblMessage.Text = "El archivo no se encuentra en el repositorio";
                //    this.mpeMensaje.Show();
                //}
            }
        }

        protected void imgAceptado_Click(object sender, EventArgs e)
        {
            try
            {

                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
                int index = row.RowIndex;

                string sUsuario = grdEstadoDocumento.Rows[index].Cells[2].Text;
                string sTipoDocumento = grdEstadoDocumento.Rows[index].Cells[7].Text;

                grdEstadoDocumento.Rows[index].Cells[3].Text = "Completo";

                // Actualizar en la tabla tbEstadoDocumento (Status = 3)
                Update_tbEstadoDocumento(sUsuario, sTipoDocumento, 3);

                // Actualizar en la tabla [ITM_04] (IdDescarga = 1)
                Update_ITM_04(sUsuario, sTipoDocumento, 1);

                var email = new EnvioEmail();

                // Consultar de la tabla [tbUsuarios] el [UsEmail]
                string sEmail = email.CorreoElectronico(sUsuario);
                int Envio_Ok = email.EnvioMensaje(sUsuario, sEmail, "Documento Aceptado ");

                if (Envio_Ok == 0)
                {
                    /// Envio de correo Ok.
                    Lbl_Message.Text = "* El documento ha sido aceptado.";
                }
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

                string sUsuario = grdEstadoDocumento.Rows[index].Cells[2].Text;
                string sNom_Archivo = grdEstadoDocumento.Rows[index].Cells[5].Text;
                string sUrl_Imagen = grdEstadoDocumento.Rows[index].Cells[6].Text;
                string sTipoDocumento = grdEstadoDocumento.Rows[index].Cells[7].Text;

                string strURLFile = Server.MapPath("~/Directorio/") + sUrl_Imagen + sNom_Archivo;

                // Actualizar en la tabla [tbEstadoDocumento] (Status = 1)
                Update_tbEstadoDocumento(sUsuario, sTipoDocumento, 1);

                // Actualizar en la tabla [ITM_04] (IdDescarga = 0)
                Update_ITM_04(sUsuario, sTipoDocumento, 0);

                grdEstadoDocumento.Rows[index].Cells[3].Text = "Faltante";

                // Eliminar el archivo de Server.MapPath("~/Directorio/")
                // File.Delete(strURLFile);

                DeleteFromAzure(sNom_Archivo, sUrl_Imagen, sUsuario, sTipoDocumento);

                // Refrescar GridView
                GetEstadoDocumentos();

                var email = new EnvioEmail();

                // Consultar de la tabla [tbUsuarios] el [UsEmail]
                string sEmail = email.CorreoElectronico(sUsuario);
                int Envio_Ok = email.EnvioMensaje(sUsuario, sEmail, "Documento Rechazado ");

                if (Envio_Ok == 0)
                {
                    /// Envio de correo Ok.
                    Lbl_Message.Text = "* El documento ha sido rechazado.";
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
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

                directory.Delete();

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

                // Obtener una referencia de nuestra parte.
                ShareClient share = new ShareClient(ConnectionString, AccountName);

                // Obtener una referencia de nuestro directorio - directorio ubicado en el nivel raíz.
                ShareDirectoryClient directory = share.GetDirectoryClient(sDirName);

                // Obtener una referencia a un subdirectorio que no se encuentra en el nivel raíz
                directory = directory.GetSubdirectoryClient(sSubdirectorio);

                // Obtener una referencia a nuestro archivo.
                ShareFileClient file = directory.GetFileClient(sFilename);

                // Descargar el archivo.
                Response.ContentType = ContentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(file.Path));
                Response.WriteFile(file.Path);
                HttpContext.Current.ApplicationInstance.CompleteRequest();

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

        protected void DownloadFile(object send, EventArgs e)
        {
            if (TxtNomArchivo.Text == "")
            {
                LblMessage.Text = "Seleccione el archivo a descargar";
                this.mpeMensaje.Show();
            }
            else
            {
                string filePath = Server.MapPath("~/Directorio/") + TxtUrl_Imagen.Text + TxtNomArchivo.Text;
                Variables.wDownload = true;

                // DescargaArch(filePath);

                Session["FilePath"] = Server.MapPath("~/Directorio/") + TxtUrl_Imagen.Text + TxtNomArchivo.Text;
                // Response.Redirect("Descargas.aspx", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AbrirDescarga", string.Format("window.open('Descargas.aspx');"), true);

            }
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
                                    " Where IdUsuario = '" + pUsuarios + "' And IdTipoDocumento = '"+ pIdTipoDocumento + "'";
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

        protected void DownloadFile_bk(object sender, EventArgs e)
        {
            try
            {
                if (TxtNomArchivo.Text == "")
                {
                    LblMessage.Text = "Seleccione el archivo a descargar";
                    this.mpeMensaje.Show();
                }
                else
                {

                    // string filePath = (sender as LinkButton).CommandArgument;
                    string filePath = Server.MapPath("~/Directorio/") + TxtUrl_Imagen.Text + TxtNomArchivo.Text;
                    Variables.wDownload = true;

                    DescargaArch(filePath);

                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void grdEstadoDocumento_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEstadoDocumento.PageIndex = e.NewPageIndex;
            GetEstadoDocumentos();
        }

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }

    }

}
