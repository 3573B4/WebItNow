using System;
using System.IO;

using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

namespace WebItNow
{
    public partial class Descargas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["Subdirectorio"] != null)
                {
                    string sFilename = (string)Session["Filename"];
                    string sSubdirectorio = (string)Session["Subdirectorio"];

                    DownloadFromAzure(sFilename, sSubdirectorio);

                    //// Name of the share, directory, and file
                    //string ConnectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");
                    //string AccountName = ConfigurationManager.AppSettings.Get("StorageAccountName");
                    //string sDirName = "itnowstorage";

                    //// Obtener una referencia de nuestra parte.
                    //ShareClient share = new ShareClient(ConnectionString, AccountName);

                    //// Obtener una referencia de nuestro directorio - directorio ubicado en el nivel raíz.
                    //ShareDirectoryClient directory = share.GetDirectoryClient(sDirName);

                    //// Obtener una referencia a un subdirectorio que no se encuentra en el nivel raíz
                    //directory = directory.GetSubdirectoryClient(sSubdirectorio);

                    //// Obtener una referencia a nuestro archivo.
                    //ShareFileClient rutaFile = directory.GetFileClient(sFilename);

                    //Response.ContentType = ContentType;
                    //Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(rutaFile.Path));
                    //Response.WriteFile(rutaFile.Path);
                    //Response.End();
                }
                
            }
        }

        protected void DownloadFromAzure(string sFilename, string sSubdirectorio)
        {
            // long tamaño = 0;
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

                // Int32 tamaño = file.Path.Length;

                using (FileStream stream = File.OpenWrite(directorioURL))
                {
                    // tamaño = stream.Length;

                    //                              32768  
                    download.Content.CopyTo(stream, 327680);
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

                    if (File.Exists(directorioURL))
                    {
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = strMimeType;
                        Response.ContentEncoding = System.Text.Encoding.UTF8;
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(directorioURL));
                    //  Response.TransmitFile(directorioURL, 0, tamaño);
                        Response.TransmitFile(directorioURL);
                        Response.Flush();

                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        Response.End();
                    }
                }

            }
            finally
            {

            }
        }
    }
}