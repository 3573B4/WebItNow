using System;
using System.IO;
using System.Runtime.InteropServices;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;

using System.Threading;

namespace WebItNow
{

	public partial class Review_Document : System.Web.UI.Page
	{
        // NYWVH-HT4XC-R2WYW-9Y3CM-X4V3Y
        private static Guid FolderDownloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHGetKnownFolderPath(ref Guid id, int flags, IntPtr token, out IntPtr path);

        protected void Page_Load(object sender, EventArgs e)
		{
            if (!Page.IsPostBack)
            {
                TxtPathDownload.Text = GetDownloadsPath();

                // Carga GridView
                GetEstadoDocumentos(); 
            }
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
            TxtUsu.Text = grdEstadoDocumento.SelectedRow.Cells[1].Text;
            TxtTpoDocumento.Text = grdEstadoDocumento.SelectedRow.Cells[3].Text;

            // string varGalleryFolder = System.Web.HttpContext.Current.Server.MapPath("~/Directorio/");

            // string IdUsuario = grdEstadoDocumento.SelectedRow.Cells[1].Text;

            // varGalleryFolder = varGalleryFolder + grdEstadoDocumento.SelectedRow.Cells[1].Text;

            // GetFiles(varGalleryFolder + "\\" + grdEstadoDocumento.SelectedRow.Cells[2].Text);

            // Carga GridView
            // GetEstadoDocumentos();

            // GetEstadoDocumentos(grdEstadoDocumento.SelectedRow.Cells[1].Text);

        }

        protected void grdEstadoDocumento_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                //
                // Se obtiene indice de la row seleccionada
                //
                int index = Convert.ToInt32(e.CommandArgument);

                //
                // Obtengo el id de la entidad que se esta editando
                // en este caso de la entidad Person
                //
                int id = Convert.ToInt32(grdEstadoDocumento.DataKeys[index].Value);

            }

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

        protected void GetEstadoDocumentos()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string strQuery = "SELECT ed.IdUsuario, ed.IdTipoDocumento, td.Descripcion, s.Descripcion as Desc_Status  " +
                                  "  FROM tbEstadoDocumento ed, tbTpoDocumento td, tbStatus s " +
                                  " WHERE ed.IdStatus = s.IdStatus And ed.IdTipoDocumento = td.IdTpoDocumento ";
                //                "   AND IdUsuario = '" + StrUser + "'";

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

		protected void BtnUnLoad_Click(object send, EventArgs e)
        {

            /// <param name="strURLFile"> URL del archivo que se desea descargar </param>
            /// <param name="strPathToSave"> Ruta donde se desea almacenar el archivo </param>

            // string strURLFile = Server.MapPath("~/Directorio/") + "live8.jpg";
            // string strPathToSave = TxtPathDownload.Text + "/live8.jpg";

            //string strURLFile = Server.MapPath("~/Directorio/") + "manual.pdf";
            //string strPathToSave = TxtPathDownload.Text + "/" + "manual.pdf";

            string strURLFile = Server.MapPath("~/Directorio/USUARIO5/OTR/") + "Captura de pantalla (3).png";
            string strPathToSave = TxtPathDownload.Text + "/" + "Captura de pantalla (3).png";

            downloadFileToSpecificPath(strURLFile, strPathToSave);

            //string file = Request.Params["file"];
            //file = "";

            //if (!string.IsNullOrEmpty(file))
            //{
            //    string downloads = "~/Directorio/";

            //    file = Path.Combine(Server.MapPath(downloads), Path.GetFileName(file));
            //    Response.Clear();
            //    Response.ContentType = "application/octect-stream";
            //    Response.AddHeader("Content–Disposition", "attachment; filename=foo.xyz");
            //    Response.TransmitFile(file);
            //    Response.End();
            //}
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
            IntPtr _handle;
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

        protected void imgDownload_Click(object sender, ImageClickEventArgs e)
        {
            Thread thdSyncRead = new Thread(new ThreadStart(openfolder));
            thdSyncRead.SetApartmentState(ApartmentState.STA);
            thdSyncRead.Start();

            //using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            //{ 
            //    if (dialog.ShowDialog() == DialogResult.OK)
            //    {
            //        TxtPathDownload.Text = dialog.SelectedPath;
            //    }
            //}

            //using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            //{
            //    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            //    if (result == DialogResult.OK)
            //    {
            //        //string ruta = dialog.SelectedPath;
            //        TxtPathDownload.Text = dialog.SelectedPath;
            //    }
            //}

            //using (var fd = new FolderBrowserDialog())
            //{
            //    if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fd.SelectedPath))
            //    {
            //        TxtPathDownload.Text = fd.SelectedPath;
            //    }
            //}


            //Process.Start("explorer.exe");

            //var fileContent = string.Empty;
            //var filePath = string.Empty;

            //Page page = HttpContext.Current.CurrentHandler as Page;
            //ScriptManager.RegisterStartupScript(page, page.GetType(), "OpenModalDialog", "<script type=text/javascript>window.showModalDialog('SaveNewDialogWindow.aspx', null, 'dialogHeight:100px;dialogWidth:280px;status:no'); </script>", false);

            //using (OpenFileDialog openFileDialog = new OpenFileDialog())
            //{
            //    openFileDialog.InitialDirectory = "c:\\";
            //    openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            //    openFileDialog.FilterIndex = 2;
            //    openFileDialog.RestoreDirectory = true;

            //    if (openFileDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        //Get the path of specified file
            //        filePath = openFileDialog.FileName;

            //        //Read the contents of the file into a stream
            //        var fileStream = openFileDialog.OpenFile();

            //        using (StreamReader reader = new StreamReader(fileStream))
            //        {
            //            fileContent = reader.ReadToEnd();
            //        }
            //    }
            //}
        }

        public void openfolder()
        {
            try
            {

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();

            string selectedfolder = fbd.SelectedPath;

            TxtPathDownload.Text = fbd.SelectedPath;
            
            //string[] files = Directory.GetFiles(fbd.SelectedPath);
            //System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");

            }
            catch (Exception ex)
            {
                Lbl_Message.Text = ex.Message;
            }
        }

    }
}