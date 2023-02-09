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
                if (Session["Filename"] != null)
                {
                    string sFilename = (string)Session["Filename"];

                 // System.Threading.Thread.Sleep(10000);
                    DownloadFromAzure(sFilename);
                }

            }
        }

        protected void DownloadFromAzure(string sFilename)
        {
            try
            {
                string directorioURL = Server.MapPath("~/itnowstorage/" + sFilename);

                string extension = Path.GetExtension(sFilename);
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

                        Response.End();
                    //  HttpContext.Current.ApplicationInstance.CompleteRequest();
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