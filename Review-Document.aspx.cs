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
        public string Descripcion { get; set; }

        // NYWVH-HT4XC-R2WYW-9Y3CM-X4V3Y
        private static Guid FolderDownloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHGetKnownFolderPath(ref Guid id, int flags, IntPtr token, out IntPtr path);

        protected void Page_Load(object sender, EventArgs e)
		{


            if (!Page.IsPostBack)
            {
               // TxtPathDownload.Text = GetDownloadsPath();

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
            TxtUsu.Text = grdEstadoDocumento.SelectedRow.Cells[0].Text;
            TxtTpoDocumento.Text = grdEstadoDocumento.SelectedRow.Cells[1].Text;
            TxtNomArchivo.Text = grdEstadoDocumento.SelectedRow.Cells[2].Text;
            TxtUrl_Imagen.Text = grdEstadoDocumento.SelectedRow.Cells[4].Text;

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

                string strQuery = "SELECT ed.IdUsuario, ed.Nom_Imagen, td.Descripcion, ed.IdTipoDocumento, " +
                                  "s.Descripcion as Desc_Status, ed.Url_Imagen  " +
                                  "  FROM tbEstadoDocumento ed, tbTpoDocumento td, tbStatus s " +
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

        protected void BtnAceptado_Click(object sender, EventArgs e)
        {
            try
            {

                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.Button)sender).NamingContainer);
                int index = row.RowIndex;
                
                string sUsuario = grdEstadoDocumento.Rows[index].Cells[0].Text;

                // Consultar de la tabla [tbUsuarios] el [UsEmail]
                string sEmail = "esteban.trejo@itnow.mx";

                grdEstadoDocumento.Rows[index].Cells[3].Text = "Completo";

                // Actualizar en la tabla tbEstadoDocumento (Status = 3)
                Update_tbEstadoDocumento(sUsuario, 3);

                var email = new EnvioEmail();
                int Envio_Ok =email.EnvioMensaje(sUsuario, sEmail, "Documento Aceptado ");

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

        protected void BtnRechazado_Click(object sender, EventArgs e)
        {
            try
            {

                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.Button)sender).NamingContainer);
                int index = row.RowIndex;

                string sUsuario = grdEstadoDocumento.Rows[index].Cells[0].Text;
                string sNom_Archivo = grdEstadoDocumento.Rows[index].Cells[2].Text;
                string sUrl_Imagen = grdEstadoDocumento.Rows[index].Cells[4].Text;

                // Consultar de la tabla [tbUsuarios] el [UsEmail]
                string sEmail = "esteban.trejo@itnow.mx";

                string strURLFile = Server.MapPath("~/Directorio/") + sUrl_Imagen + sNom_Archivo;

                // Actualizar en la tabla tbEstadoDocumento (Status = 1)
                Update_tbEstadoDocumento(sUsuario, 1);

                grdEstadoDocumento.Rows[index].Cells[3].Text = "Faltante";

                // Eliminar el archivo de Server.MapPath("~/Directorio/")
                File.Delete(strURLFile);

                // Refrescar GridView
                GetEstadoDocumentos();

                var email = new EnvioEmail();

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
              //  imgDownload.Enabled = false;

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

        protected void DownloadFile(object sender, EventArgs e)
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
                    Response.ContentType = ContentType;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                    Response.WriteFile(filePath);
                    Response.End();

                    TxtUsu.Text = string.Empty;
                    TxtTpoDocumento.Text = string.Empty;
                    TxtNomArchivo.Text = string.Empty;
                    TxtUrl_Imagen.Text = string.Empty;

                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
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

        public static void ThreadMethod()
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    myStream.Close();
                }
            }
        }

        public static void AbrirCarpeta()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }

            MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
        }

        public void OpenFolder()
        {

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string selectedfolder = fbd.SelectedPath;
              //TxtPathDownload.Text = selectedfolder;
                fbd.Dispose();

            }

        }

        public void Update_tbEstadoDocumento(string pUsuarios, int pIdStatus)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();


                if (pIdStatus == 1)
                {
                    Variables.wQuery = "Update tbEstadoDocumento Set IdStatus = " + pIdStatus + ", Nom_Imagen = Null " +
                                    " Where IdUsuario = '" + pUsuarios + "'";
                }
                else
                {
                    Variables.wQuery = "Update tbEstadoDocumento Set IdStatus = " + pIdStatus + " Where IdUsuario = '" + pUsuarios + "'";
                }

                // Update tabla tbEstadoDocumento
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


    }
}