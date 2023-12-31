﻿using System;
using System.Configuration;
using System.IO;

using Microsoft.AspNet.Web;
using System.Runtime.InteropServices;

using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

using System.Data;
using System.Data.SqlClient;

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using System.IO.Compression;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Security.Principal;
using System.Reflection;

using Azure.Storage;
using Azure.Storage.Files;
using Microsoft.WindowsAzure.Storage.File;

namespace WebItNow
{
    public partial class Review_Document : System.Web.UI.Page
    {
        public string Descripcion { get; set; }

        // NYWVH-HT4XC-R2WYW-9Y3CM-X4V3Y
        // private static Guid FolderDownloads = new Guid("374DE290-123F-4565-9164-39C4925E467B");

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]

        private static extern int SHGetKnownFolderPath(ref Guid id, int flags, IntPtr token, out IntPtr path);

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";

            // Server.ScriptTimeout = 3600;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            // Carga GridView
            GetEstadoDocumentos();

            if (!Page.IsPostBack)
            {

                //    string script = "$(document).ready(function () { $('[id*=btnSubmit]').click(); });";
                //    ClientScript.RegisterStartupScript(this.GetType(), "load", script, true);


                ////* * Agrega THEAD y TBODY a GridView.
                //GrdEstadoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (Variables.wDownload == true)
            {
                TxtRef.Text = string.Empty;
                TxtTpoDocumento.Text = string.Empty;
                TxtNomArchivo.Text = string.Empty;
                TxtUrl_Imagen.Text = string.Empty;

                Variables.wRef = string.Empty;
                Variables.wTpoDocumento = string.Empty;
                Variables.wFileName = string.Empty;
                Variables.wURL_Imagen = string.Empty;

                imgDescarga.Enabled = false;
                Variables.wDownload = false;
            }


        }

        protected void GrdEstadoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtRef.Text = Server.HtmlDecode(GrdEstadoDocumento.SelectedRow.Cells[2].Text); 
            TxtNomArchivo.Text = Server.HtmlDecode(GrdEstadoDocumento.SelectedRow.Cells[5].Text);
            TxtUrl_Imagen.Text = Server.HtmlDecode(GrdEstadoDocumento.SelectedRow.Cells[6].Text);
            TxtTpoDocumento.Text = Server.HtmlDecode(GrdEstadoDocumento.SelectedRow.Cells[7].Text);
            TxtAsegurado.Text = Server.HtmlDecode(GrdEstadoDocumento.SelectedRow.Cells[9].Text);

            // Habilitar el boton de Descargas
            imgDescarga.Enabled = true;

            Variables.wRef = Server.HtmlDecode(GrdEstadoDocumento.SelectedRow.Cells[2].Text);
            Variables.wFileName = Server.HtmlDecode(GrdEstadoDocumento.SelectedRow.Cells[5].Text);
            Variables.wURL_Imagen = Server.HtmlDecode(GrdEstadoDocumento.SelectedRow.Cells[6].Text);
            Variables.wTpoDocumento = Server.HtmlDecode(GrdEstadoDocumento.SelectedRow.Cells[7].Text);


            this.hdfValorGrid.Value = this.GrdEstadoDocumento.SelectedValue.ToString();
        }

        protected void GrdEstadoDocumento_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdEstadoDocumento.PageIndex = e.NewPageIndex;
            GetEstadoDocumentos();
        }

        protected void GrdEstadoDocumento_PageIndexChanged(Object sender, EventArgs e)
        {
            // Call a helper method to display the current page number 
            // when the user navigates to a different page.
            DisplayCurrentPage();
        }

        protected void GrdEstadoDocumento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdEstadoDocumento, "Select$" + e.Row.RowIndex.ToString()) + ";");

            string IdDescarga = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "IdDescarga"));

            if (IdDescarga == "1")
            {
                (e.Row.FindControl("imgAceptado") as ImageButton).Enabled = true;
                (e.Row.FindControl("imgRechazado") as ImageButton).Enabled = true;
            //  (e.Row.FindControl("imgDescarga") as ImageButton).Enabled = true;
            }
        }

        protected void GrdEstadoDocumento_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //// Actualizar en la tabla [ITM_04] (IdDescarga = 1)
            //Update_ITM_04(sReferencia, sTipoDocumento, 1);

            //// Actualizar controles
            //GetEstadoDocumentos();

            //System.Threading.Thread.Sleep(10000);
            //this.DownloadFromAzure(sFilename, sSubdirectorio);

            //if (e.CommandName == "cmdpost")
            //{
            //    int rowIndex = Convert.ToInt32(e.CommandArgument);
            //    GridViewRow row = GrdEstadoDocumento.Rows[rowIndex];

            //    string sReferencia = row.Cells[3].Text;
            //    string sNomArchivo = row.Cells[6].Text;
            //    string sSubdirectorio = row.Cells[7].Text;
            //    string sTipoDocumento = row.Cells[8].Text;

            //    Variables.wDownload = true;

            //    // Actualizar en la tabla [ITM_04] (IdDescarga = 1)
            //    Update_ITM_04(sReferencia, sTipoDocumento, 1);

            //    // Actualizar controles
            //    GetEstadoDocumentos();

            //    Session["Filename"] = sNomArchivo;

            //    System.Threading.Thread.Sleep(10000);
            //    DownloadFromAzure(sNomArchivo, sSubdirectorio);

            //    // Descargar el archivo.
            //    string mensaje = "window.open('Descargas.aspx');";
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", mensaje, true);

            //}
        }

        protected void DisplayCurrentPage()
        {
            // Calculate the current page number.
            // int currentPage = grdEstadoDocumento.PageIndex + 1;

            // Display the current page number. 
            // Message.Text = "Page " + currentPage.ToString() + " of " grdEstadoDocumento.PageCount.ToString() + ".";
        }

        protected void GetEstadoDocumentos()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_04
                // Tipo de Documento = ITM_08
                // Status de Documento = ITM_07

                string strQuery = "SELECT ed.Referencia, ed.Nom_Imagen, td.Descripcion, ed.IdTipoDocumento, " +
                                  "       s.Descripcion as Desc_Status, ed.Url_Imagen, ed.IdDescarga, tr.Aseguradora " +
                                  "  FROM ITM_02 tr, ITM_04 ed, ITM_08 td, ITM_07 s, ITM_15 t " +
                                  " WHERE tr.UsReferencia = ed.Referencia " +
                                  "   AND ed.IdStatus = s.IdStatus " + 
                                  "   AND ed.IdTipoDocumento = td.IdTpoDocumento " +
                                  "   AND t.Referencia = ed.Referencia " +
                                  "   AND t.IdTpoDocumento = ed.IdTipoDocumento " +
                                  //"   AND t.IdProceso = td.IdProceso " +
                                  //"   AND t.IdSubProceso = td.IdSubProceso " +
                                  "   AND t.IdStatus = 1 " +
                                  "   AND ed.IdStatus IN (2) ";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    GrdEstadoDocumento.ShowHeaderWhenEmpty = true;
                    GrdEstadoDocumento.EmptyDataText = "No hay resultados.";
                }

                GrdEstadoDocumento.DataSource = dt;
                GrdEstadoDocumento.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdEstadoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                Lbl_Message.Text = FnErrorMessage(ex.Message);
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
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon();

            // Response.Redirect("Menu.aspx", true);
            Response.Redirect("Mnu_Dinamico.aspx", true);
        }

        protected void ChkAceptado_OnCheckedChanged(object sender, EventArgs e)
        {
         // GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.CheckBox)sender).NamingContainer);
         // int index = row.RowIndex;
        }

        protected void BtnUnLoad_Click(object send, EventArgs e)
        {

            /// <param name="strURLFile"> URL del archivo que se desea descargar </param>
            /// <param name="strPathToSave"> Ruta donde se desea almacenar el archivo </param>

            //string strURLFile = Server.MapPath("~/Directorio/") + "USUARIO5/OTR/" + "Captura de pantalla (3).png";
            //string strPathToSave = TxtPathDownload.Text + "\\" + "Captura de pantalla (3).png";

            //  string strURLFile = Server.MapPath("~/Directorio/") + "wordpress-6.1.1.zip";
            //  string strPathToSave = TxtPathDownload.Text + "/" + "wordpress-6.1.1.zip";

            //  downloadFileToSpecificPath(strURLFile, strPathToSave);

            //  imgDownload.Enabled = false;
        }

        protected void ImgAceptado_Click(object sender, EventArgs e)
        {
            try
            {

                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
                int index = row.RowIndex;

                string sReferencia = GrdEstadoDocumento.Rows[index].Cells[2].Text;
                string sFileName = Server.HtmlDecode(GrdEstadoDocumento.Rows[index].Cells[5].Text);
                string sTipoDocumento = GrdEstadoDocumento.Rows[index].Cells[7].Text;

                GrdEstadoDocumento.Rows[index].Cells[3].Text = "Revisado";

                // Actualizar en la tabla tbEstadoDocumento (Status = 3)
                Update_tbEstadoDocumento(sReferencia, sTipoDocumento, 3);

                // Actualizar en la tabla [ITM_04] (IdDescarga = 1)
                Update_ITM_04(sReferencia, sTipoDocumento, 1);

                int iConsecutivo = 0;
                var dateAndTime = DateTime.Now;
                var Date = dateAndTime.ToShortDateString();

                // Obtener valor ultimo consecutivo por dia.
                iConsecutivo = sp_Consecutivo(iConsecutivo, DateTime.Parse(Date), "Select");
                
                if (iConsecutivo == 0)
                {
                    // Inserta el registro con el primer consecutivo del dia
                    iConsecutivo = sp_Consecutivo(0, DateTime.Parse(Date), "Insert");
                    iConsecutivo += 1;
                }
                else
                {
                    iConsecutivo += 1;
                }

                // Copiar el archivo al almacenamiento (codisestorage)
                modGeneral modGral = new modGeneral();
                modGral.CopyFileAzure(sReferencia, sFileName, sTipoDocumento, iConsecutivo);

                iConsecutivo = sp_Consecutivo(iConsecutivo, DateTime.Parse(Date), "Update");


                DeleteFromAzure(sFileName, sReferencia);

                Session["Referencia"] = sReferencia;
                Session["Asunto"] = "Documentación Aceptada";

                Response.Redirect("Page_Message.aspx");

                //var email = new EnvioEmail();

                //// Consultar de la tabla [tbUsuarios] el [UsEmail]
                //string sEmail = email.CorreoElectronico(sReferencia);
                //int Envio_Ok = email.EnvioMensaje(sReferencia, sEmail, "Documento Aceptado ");

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

        protected void ImgRechazado_Click(object sender, EventArgs e)
        {
            try
            {

                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
                int index = row.RowIndex;


                string sReferencia = GrdEstadoDocumento.Rows[index].Cells[2].Text;
                string sNom_Archivo = GrdEstadoDocumento.Rows[index].Cells[5].Text;
                string sUrl_Imagen = GrdEstadoDocumento.Rows[index].Cells[6].Text;
                string sTipoDocumento = GrdEstadoDocumento.Rows[index].Cells[7].Text;

                string strURLFile = Server.MapPath("~/Directorio/") + sUrl_Imagen + sNom_Archivo;

                // Actualizar en la tabla [tbEstadoDocumento] (Status = 1)
                Update_tbEstadoDocumento(sReferencia, sTipoDocumento, 1);

                // Actualizar en la tabla [ITM_04] (IdDescarga = 0)
                Update_ITM_04(sReferencia, sTipoDocumento, 0);

                GrdEstadoDocumento.Rows[index].Cells[3].Text = "Solicitado";

                // Eliminar el archivo de Server.MapPath("~/Directorio/")
                // File.Delete(strURLFile);

                DeleteFromAzure(sNom_Archivo, sUrl_Imagen);

                // Refrescar GridView
                GetEstadoDocumentos();

                Session["Referencia"] = sReferencia;
                Session["Asunto"] = "Documento Rechazado";

                Response.Redirect("Page_Message.aspx");

                //var email = new EnvioEmail();

                //// Consultar de la tabla [tbUsuarios] el [UsEmail]
                //string sEmail = email.CorreoElectronico(sReferencia);
                //int Envio_Ok = email.EnvioMensaje(sReferencia, sEmail, "Documento Rechazado");

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
                //GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
                //int index = row.RowIndex;

                //string sReferencia = GrdEstadoDocumento.Rows[index].Cells[3].Text;
                //string sNomArchivo = GrdEstadoDocumento.Rows[index].Cells[6].Text;
                //string sSubdirectorio = GrdEstadoDocumento.Rows[index].Cells[7].Text;
                //string sTipoDocumento = GrdEstadoDocumento.Rows[index].Cells[8].Text;

                //string sReferencia = TxtRef.Text;
                //string sTipoDocumento = TxtTpoDocumento.Text;
                //string Filename = TxtNomArchivo.Text;
                //string Subdirectorio = TxtUrl_Imagen.Text;

                Variables.wDownload = true;

                // Actualizar en la tabla [ITM_04] (IdDescarga = 1)
                Update_ITM_04(Variables.wRef, Variables.wTpoDocumento, 1);

                // Actualizar controles
                GetEstadoDocumentos();

                string sFileName = Variables.wFileName;
                string sSubdirectorio = Variables.wURL_Imagen;

                HttpContext.Current.Session["sFileName"] = Variables.wFileName;
                HttpContext.Current.Session["Subdirectorio"] = Variables.wURL_Imagen;

                System.Threading.Thread.Sleep(1000);
                //this.DownloadFromAzure(sFileName, sSubdirectorio);

                this.DescargaFromAzure_(sFileName, sSubdirectorio);

                //var result =  DownloadAsync.DownloadAzureAsync(sFileName, sSubdirectorio);

                string mensaje = "window.open('Descargas.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", mensaje, true);

                // imgDescarga.Enabled = false;

                //// Actualizar en la tabla [ITM_04] (IdDescarga = 1)
                //Update_ITM_04(sReferencia, sTipoDocumento, 1);

                //// Actualizar controles
                //GetEstadoDocumentos();

                //Session["Filename"] = sNomArchivo;

                //System.Threading.Thread.Sleep(10000);
                //DownloadFromAzure(sNomArchivo, sSubdirectorio);

                // Descargar el archivo.
                //string mensaje = "window.open('Descargas.aspx');";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", mensaje, true);

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

        protected void DeleteFromAzure(string sFilename, string sSubdirectorio)
        {
            try
            {

                // Name of the share, directory, and file
                string ConnectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");
                string AccountName = ConfigurationManager.AppSettings.Get("StorageAccountName");
             // string sDirName = "itnowstorage";

                // Obtener una referencia de nuestra parte.
                ShareClient share = new ShareClient(ConnectionString, AccountName);

                // Obtener una referencia de nuestro directorio - directorio ubicado en el nivel raíz.
                ShareDirectoryClient directory = share.GetDirectoryClient(AccountName);

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

            string sNomArchivo = GrdEstadoDocumento.Rows[index].Cells[6].Text;
            string sSubdirectorio = GrdEstadoDocumento.Rows[index].Cells[7].Text;

            // string filePath = Server.MapPath("~/Directorio/") + sSubdirectorio + sNomArchivo;
            // Variables.wDownload = true;

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
        public static void DownloadFileToSpecificPath(string strURLFile, string strPathToSave)
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

        public void Update_tbEstadoDocumento(string pReferencia, string pIdTipoDocumento, int pIdStatus)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Actualizar en la tabla Estado de Documento
                if (pIdStatus == 1)
                {
                    Variables.wQuery = "Update ITM_04 Set IdStatus = " + pIdStatus + ", Nom_Imagen = Null, Fec_Rechazado = GETDATE()" +
                                    " Where Referencia LIKE '%' + '" + pReferencia + "'  + '%' And IdTipoDocumento = '" + pIdTipoDocumento + "'";
                }
                else
                {
                    Variables.wQuery = "Update ITM_04 Set IdStatus = " + pIdStatus + ", Fec_Aceptado = GETDATE() Where Referencia LIKE '%' + '" + pReferencia + "'  + '%' And IdTipoDocumento = '" + pIdTipoDocumento + "'";
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

        public void Update_ITM_04(string pReferencia, string pIdTipoDocumento, int pIdDescarga)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                using (SqlCommand cmd1 = new SqlCommand("sp_UpDescarga", Conecta.ConectarBD))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.AddWithValue("@referencia", pReferencia);
                    cmd1.Parameters.AddWithValue("@IdTipoDocumento", pIdTipoDocumento);
                    cmd1.Parameters.AddWithValue("@IdDescarga", pIdDescarga);

                    SqlDataReader dr1 = cmd1.ExecuteReader();

                    cmd1.Dispose();
                    dr1.Dispose();
                }

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

        public int sp_Consecutivo(int pConsecutivo, DateTime pFecha, string pStatementType)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {
                using (SqlCommand cmd1 = new SqlCommand("sp_Consecutivo_StatementType", Conecta.ConectarBD))
                {
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.AddWithValue("@idconsecutivo", pConsecutivo);
                    cmd1.Parameters.AddWithValue("@dfecha", pFecha);
                    cmd1.Parameters.AddWithValue("@StatementType", pStatementType);

                    SqlDataReader dr1 = cmd1.ExecuteReader();

                    if (dr1.Read())
                    {

                        return dr1.GetInt32(0);

                    }

                    cmd1.Dispose();
                    dr1.Dispose();
                }

                Conecta.Cerrar();

                return 0;

            }
            catch (Exception ex)
            {
                // Show(ex.Message);
                // LblMessage.Text = ex.Message;
                // this.mpeMensaje.Show();
                Console.WriteLine($"{ex.Message}");
            }
            finally
            {

            }

            return -1;
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

        public static string ReturnFiletype(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }

        protected void RefreshPage()
        {
            Response.AppendHeader("Refresh", "0;URL=" + HttpContext.Current.Request.Url.AbsoluteUri);
        }

        protected void BtnDescargas_Click(object sender, EventArgs e)
        {

            //string sReferencia = TxtRef.Text;
            //string sTipoDocumento = TxtTpoDocumento.Text;
            //string sFilename = TxtNomArchivo.Text;
            //string sSubdirectorio = TxtUrl_Imagen.Text;

            //Variables.wRef = GrdEstadoDocumento.SelectedRow.Cells[2].Text;
            //Variables.wFileName = GrdEstadoDocumento.SelectedRow.Cells[5].Text;
            //Variables.wURL_Imagen = GrdEstadoDocumento.SelectedRow.Cells[6].Text;
            //Variables.wTpoDocumento = GrdEstadoDocumento.SelectedRow.Cells[7].Text;
            imgDescarga.Enabled = false;

            // Actualizar en la tabla [ITM_04] (IdDescarga = 1)
            Update_ITM_04(Variables.wRef, Variables.wTpoDocumento, 1);

            // Actualizar controles
            GetEstadoDocumentos();

            Session["Filename"] = Variables.wFileName;

            string sFilename = Variables.wFileName;
            string sSubdirectorio = Variables.wURL_Imagen;

            System.Threading.Thread.Sleep(10000);
            DownloadFromAzure(sFilename, sSubdirectorio);

            string mensaje = "window.open('Descargas.aspx');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", mensaje, true);

        }

        protected void DownloadFromAzure(string sFilename, string sSubdirectorio)
        {

            //  long tamaño = 0;
            try
            {
                // Name of the share, directory, and file
                string ConnectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");
                string AccountName = ConfigurationManager.AppSettings.Get("StorageAccountName");

             // string sDirName = "itnowstorage";
                string directorioURL = Server.MapPath("~/itnowstorage/" + sFilename);

                // Obtener una referencia de nuestra parte.
                ShareClient share = new ShareClient(ConnectionString, AccountName);

                // Obtener una referencia de nuestro directorio - directorio ubicado en el nivel raíz.
                ShareDirectoryClient directory = share.GetDirectoryClient(AccountName);

                // Obtener una referencia a un subdirectorio que no se encuentra en el nivel raíz
                directory = directory.GetSubdirectoryClient(sSubdirectorio);

                // Obtener una referencia a nuestro archivo.
                ShareFileClient file = directory.GetFileClient(sFilename);

                // Descargar el archivo.
                ShareFileDownloadInfo download = file.Download();

                // Int32 tamaño = file.Path.Length;

                using (FileStream stream = File.OpenWrite(directorioURL))
                {
                    // tamaño = stream.Length;

                    //                              32768  
                    download.Content.CopyTo(stream, 327680);
                    stream.Flush();
                    stream.Close();
                }

                ////while (true)
                ////{                   
                ////    if (File.Exists(directorioURL))
                ////        break;
                ////}

                //string extension = Path.GetExtension(file.Path);
                //var strMimeType = "";

                //if (extension != null)
                //{
                //    switch (extension.ToLower())
                //    {
                //        case ".htm":
                //        case ".html":
                //            strMimeType = "text/HTML";
                //            break;
                //        case ".txt":
                //            strMimeType = "text/plain";
                //            break;
                //        case ".doc":

                //        case ".docx":

                //        case ".rtf":
                //            strMimeType = "Application/msword";
                //            break;
                //        case ".pdf":
                //            strMimeType = "application/pdf";
                //            break;
                //        case ".zip":
                //            strMimeType = "application/zip";
                //            break;
                //        case ".xlsx":
                //            strMimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //            break;
                //        case "xls":
                //            strMimeType = "application/vnd.ms-excel";
                //            break;

                //    }

                //}

                //if (extension == ".xlsx")
                //{
                //    string direccion = directorioURL;
                //    System.IO.FileStream fs = null;

                //    fs = System.IO.File.Open(direccion, System.IO.FileMode.Open);
                //    byte[] btFile = new byte[fs.Length];
                //    fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
                //    fs.Close();

                //    Response.AddHeader("Content-disposition", "attachment; filename=" + Path.GetFileName(directorioURL));
                //    Response.ContentType = "application/octet-stream";
                //    Response.BinaryWrite(btFile);
                ////  Response.Flush();
                //    Response.End();
                ////  HttpContext.Current.ApplicationInstance.CompleteRequest();
                //}
                //else
                //{

                //    if (File.Exists(directorioURL))
                //    {
                //        Response.Clear();
                //        Response.Buffer = true;
                //        Response.ContentType = strMimeType;
                //        Response.ContentEncoding = System.Text.Encoding.UTF8;
                //        Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(directorioURL));
                //    //  Response.TransmitFile(directorioURL, 0, tamaño);
                //        Response.TransmitFile(directorioURL);
                //        Response.Flush();
                //        Response.End();

                //    //  HttpContext.Current.ApplicationInstance.CompleteRequest();
                //    }
                //    else
                //    {
                //        Response.End();
                //    }
                //}

                ////  System.Threading.Thread.Sleep(5000);
                ////  File.Delete(directorioURL);
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

        protected void DownloadFromAzure_New(string sFilename, string sSubdirectorio)
        {
            try
            {

                string ConnectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");
                string shareName = ConfigurationManager.AppSettings.Get("StorageAccountName");
                string dirName = "/itnowstorage-desarrollo/" + sSubdirectorio ;

                // Name of the share, directory, and file we'll download from
                // string shareName = "sample-share";
                // string dirName = "sample-dir";
                // string fileName = "sample-file";

                // Path to the save the downloaded file
                // string localFilePath = @"<path_to_local_file>";

                // Get a reference to the file
                ShareClient share = new ShareClient(ConnectionString, shareName);
                ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
                ShareFileClient file = directory.GetFileClient(sFilename);

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

    }
}