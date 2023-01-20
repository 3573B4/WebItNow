using System;
using System.IO;

using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Azure.Storage.Files.Shares;


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
                    //string sFilename = (string)Session["Filename"];
                    //string sSubdirectorio = (string)Session["Subdirectorio"];

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
    }
}