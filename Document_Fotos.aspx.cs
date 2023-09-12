using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Collections;

namespace WebItNow
{
    public partial class Document_Fotos : System.Web.UI.Page
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

                string sReferencia = TxtReferencia.Text;
                Variables.wFileName = "Fotos.zip";

                DownloadFromAzure(sReferencia);

                string directorioURL = Server.MapPath("~/itnowstorage/" + sReferencia + "/" + "Fotos");

                if (Directory.Exists(directorioURL))
                {
                    string directorio = "~/itnowstorage/" + sReferencia + "/" + "Fotos";

                    string[] filePaths = Directory.GetFiles(Server.MapPath(directorio));

                    List<ListItem> files = new List<ListItem>();

                    foreach (string filePath in filePaths)
                    {
                        string fileName = Path.GetFileName(filePath);
                        files.Add(new ListItem("", directorio + "/" + fileName));
                    }

                    DataList1.DataSource = files;
                    DataList1.DataBind();

                    //ArrayList Lista = new ArrayList();
                    //string[] Archivos = Directory.GetFiles(Server.MapPath(directorio));

                    //foreach(string archivo in Archivos)
                    //{
                    //    Lista.Add(directorio + "/" + Path.GetFileName(archivo));
                    //}

                    //DataList1.DataSource = Lista;
                    //DataList1.DataBind();

                }

            }

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

        protected void btnSubir_Click(object sender, EventArgs e)
        {
            try
            {
                mpeSubirArchivo.Show();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
            }
        }

        protected void btnPnlSubir_Click(object sender, EventArgs e)
        {
            try
            {
                string filepath = Server.MapPath("\\Upload");

                HttpFileCollection uploadedFiles = Request.Files;

                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    HttpPostedFile userPostedFile = uploadedFiles[i];

                    try
                    {
                        if (userPostedFile.ContentLength > 0)
                        {

                            string nomFile = userPostedFile.FileName;
                            string sReferencia = TxtReferencia.Text;

                            this.UploadToAzure(nomFile, sReferencia, "Fotos", userPostedFile);
                        }
                    }
                    catch (Exception Ex)
                    {
                        lblPnlMensageError.Text += "Error: <br>" + Ex.Message;
                    }
                }

            }
            catch (Exception ex)
            {
                lblPnlMensageError.Text = ex.Message;
            }

        }

        public void UploadToAzure(string sFilename, string sDirName, string sTpoDocumento, HttpPostedFile userPostedFile)
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
                    LblMessage.Text = "El documento ya existe";
                    mpeMensaje.Show();

                    return;
                }
                else
                {

                    //Access the File using the Name of HTML INPUT File.
                    //HttpPostedFile postedFile = Request.Files["oFile"];

                    file.Create(userPostedFile.ContentLength);

                    int blockSize = 64 * 1024;
                    long offset = 0;    // Definir desplazamiento de rango http.

                    BinaryReader reader = new BinaryReader(userPostedFile.InputStream);
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

                    string sReferencia = TxtReferencia.Text;
                    string sUsuario = (string)Session["IdUsuario"];
                    string sUrl_Imagen = sDirName + "/" + sTpoDocumento;

                    Add_tbDetalleFotos(sReferencia, sUsuario, sUrl_Imagen, sFilename);

                    //ConexionBD Conectar = new ConexionBD();
                    //Conectar.Abrir();

                    //// Actualizar la tabla Documentos Archivo
                    //string sqlUpDate = "UPDATE ITM_39" +
                    //                    "    SET IdStatus = '1'," +
                    //                        " Url_Imagen = '" + sUrl_Imagen + "'," +
                    //                        " Nom_Imagen = '" + sFilename + "'," +
                    //                        "  Fec_Entrega = GETDATE() " +
                    //                    " WHERE Referencia LIKE '%' + '" + sReferencia + "'  + '%' " +
                    //                    " AND IdTpoDocumento = '" + Variables.wTpoDocumento + "'";

                    //SqlCommand cmd = new SqlCommand(sqlUpDate, Conectar.ConectarBD);

                    //cmd.ExecuteReader();
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void btnClose_FotosUnload_Click(object sender, EventArgs e)
        {

        }

        protected void btnDescargarZip_Click(object sender, EventArgs e)
        {
            try
            {
                string sReferencia = TxtReferencia.Text;
                string sSubdirectorio = "Fotos";

                Variables.wFileName = "Fotos.zip";

                DownloadFromAzure(sReferencia);
                DownloadFilesZip(sReferencia, sSubdirectorio);

             // azCopy_FromDirectory(sReferencia, sSubdirectorio);
             // this.DownloadFiles(sReferencia, sSubdirectorio);
             // this.DownloadFromAzure(sFilename, "PBA-270323-1");

                HttpContext.Current.Session["sFileName"] = Variables.wFileName;
                HttpContext.Current.Session["Subdirectorio"] = Variables.wURL_Imagen;

                //string mensaje = "window.open('Descargas.aspx');";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", mensaje, true);

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();

                return;
            }
        }

        protected void DownloadFromAzure(string sReferencia)
        {
            try
            {
                // string sFileName = "Cancelar.jpg";
                string sFileName = string.Empty;

                string sDirectory = "/" + sReferencia + "/Fotos/";
                string sMapDirectory = "/itnowstorage" + sDirectory;

                // Name of the share, directory, and file
                string ConnectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");
                string AccountName = ConfigurationManager.AppSettings.Get("StorageAccountName");

                string sDirName = "itnowstorage-desarrollo";

                string directorioURL = Server.MapPath("~" + sMapDirectory);

                // Obtener una referencia de nuestra parte.
                ShareClient share = new ShareClient(ConnectionString, AccountName);

                // Obtener una referencia de nuestro directorio - directorio ubicado en el nivel raíz.
                ShareDirectoryClient directory = share.GetDirectoryClient(sDirName);

                // Obtener una referencia a un subdirectorio que no se encuentra en el nivel raíz
                directory = directory.GetSubdirectoryClient(sReferencia);
                directory = directory.GetSubdirectoryClient("Fotos");

                ConexionBD Conecta = new ConexionBD();
                NewMethod(Conecta);

                // Consulta a la tabla : Usuarios = ITM_02
                string strQuery = "SELECT IdFoto, Referencia, Nom_Imagen " +
                                  "  FROM ITM_39 ed " +
                                  " WHERE Referencia LIKE '%' + '" + sReferencia + "'  + '%' ";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    sFileName = Convert.ToString(row[2]);

                    // Obtener una referencia a nuestro archivo.
                    ShareFileClient file = directory.GetFileClient(sFileName);

                    // Descargar el archivo.
                    ShareFileDownloadInfo download = file.Download();

                    Int32 tamaño = file.Path.Length;
                    if (!Directory.Exists(directorioURL))
                    {
                        Directory.CreateDirectory(directorioURL);
                    }

                    using (FileStream stream = File.OpenWrite(directorioURL + sFileName))
                    {
                        //                              32768  
                        download.Content.CopyTo(stream, 327680);
                        stream.Flush();
                        stream.Close();
                    }
                }

                cmd.Dispose();
                Conecta.Cerrar();

                //// Obtener una referencia a nuestro archivo.
                //ShareFileClient file = directory.GetFileClient(sFileName);

                //// Descargar el archivo.
                //ShareFileDownloadInfo download = file.Download();

                //Int32 tamaño = file.Path.Length;
                //if (!Directory.Exists(directorioURL))
                //{
                //    Directory.CreateDirectory(directorioURL);
                //}


                //using (FileStream stream = File.OpenWrite(directorioURL + sFileName))
                //{
                //    //                              32768  
                //    download.Content.CopyTo(stream, 327680);
                //    stream.Flush();
                //    stream.Close();
                //}
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();

            }
        }

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }

        public void DownloadFilesZip(string sReferencia, string sSubdirectorio)
        {

            string directorio = "~/itnowstorage/";

            string startPath = Server.MapPath(directorio) + sReferencia + "\\" + sSubdirectorio;
            string zipPath = Server.MapPath(directorio + Variables.wFileName);

            string directorioURL = Server.MapPath(directorio + Variables.wFileName);

            try
            {
                // DELETE AL ARCHIVO --> (sPath)
                File.Delete(zipPath);

                ZipFile.CreateFromDirectory(startPath, zipPath);

                if (File.Exists(directorioURL))
                {
                    string strMimeType = "application/zip";

                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = strMimeType;
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(directorioURL));
                    Response.TransmitFile(directorioURL);
                    Response.Flush();

                    HttpContext.Current.ApplicationInstance.CompleteRequest();

                }
                else
                {
                    Response.End();
                }

                //DELETE AL ARCHIVO-- > (sPath)
                File.Delete(zipPath);

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

        public int Add_tbDetalleFotos(string pReferencia, string pUsuario, string pUrl_Imagen, string pNom_Imagen)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string strQuery = "Insert into ITM_39 (Referencia, IdUsuario, Url_Imagen, Url_Path_Final, Nom_Imagen, Nom_Imagen_Final, Fec_Entrega, IdStatus)" +
                                  "Values ('" + pReferencia + "', '" + pUsuario + "', '" + pUrl_Imagen + "', Null, '" + pNom_Imagen + "', Null, GETDATE(), 1)" + "\n \n";

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

        public void azCopy_FromDirectory(string sReferencia, string sSubdirectorio)
        {
            try
            {

                // Parámetros
                string storageAccountName = "itnowstorage";
                string fileShareName = ConfigurationManager.AppSettings.Get("StorageAccountName");
                string directoryPath = "/itnowstorage-desarrollo/" + sReferencia + "/" + sSubdirectorio;
                string sasToken = "sp=rcwdl&st=2023-05-08T16:18:45Z&se=2023-05-09T16:18:45Z&sip=187.190.190.0-187.190.190.255&spr=https&sv=2022-11-02&sig=xefrCK07v5quKswqSkXAWaJ%2B8gA%2FU%2B8rowWuE8bNSOM%3D&sr=s";
                string localDirectoryPath = Server.MapPath("~/itnowstorage/");

                // Comando de azcopy
                string azCopyCommand = $"copy 'https://{storageAccountName}.file.core.windows.net/{fileShareName}/{directoryPath}?{sasToken}' '{localDirectoryPath}' -- recursive = true";

                // Iniciar proceso de línea de comando
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();

                startInfo.FileName = "azcopy";              // Nombre del ejecutable de azcopy
                startInfo.Arguments = azCopyCommand;        // Argumentos para el comando de azcopy
                startInfo.RedirectStandardOutput = true;
                startInfo.UseShellExecute = false;
                
                process.StartInfo = startInfo;
                process.Start();

                // Esperar a que termine el proceso
                process.WaitForExit();

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

    }
}