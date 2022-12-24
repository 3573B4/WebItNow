using System;
using System.IO;
using System.Runtime.InteropServices;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;


namespace WebItNow
{

	public partial class Review_Document : System.Web.UI.Page
	{
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
            string varGalleryFolder = System.Web.HttpContext.Current.Server.MapPath("~/Directorio/");

            // string IdUsuario = grdEstadoDocumento.SelectedRow.Cells[1].Text;

            varGalleryFolder = varGalleryFolder + grdEstadoDocumento.SelectedRow.Cells[1].Text;

            //GetFiles(varGalleryFolder + "\\" + grdEstadoDocumento.SelectedRow.Cells[2].Text);

            // Carga GridView
            GetEstadoDocumentos();
            //GetEstadoDocumentos(grdEstadoDocumento.SelectedRow.Cells[1].Text);

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

        }

		protected void BtnClose_Click(object send, EventArgs e)
        {

        }

        protected void imgDownload_Click(object sender, ImageClickEventArgs e)
        {

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

    }
}