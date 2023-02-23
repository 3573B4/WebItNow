using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

using System.Data;
using System.Data.SqlClient;



namespace WebItNow
{
    public class modGeneral
    {
        public bool validarCaracter(string nomFile)
        {
            char[] a = { '{', '}', ',', '/', '[', ']', '`', '^', '~', '+', '*', '´', '¨', '¿', '¡' };
            string sA = new string(a);

            for (int i = 0; i < nomFile.Length; i++)
            {
                for (int j = 0; j < a.Length; j++)
                {
                    if (nomFile[i] == a[j])
                    {

                        return false;
                        //break;
                    }
                }
            }
            return true;
        }

        public void CopyFileAzure(string sReferencia, string sFileName, int iConsecutivo)
        {
            try
            {

                // Get the connection string from app settings
                string ConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
                string AccountName = ConfigurationManager.AppSettings.Get("StorageAccountName");
                string sDirName = "itnowstorage";

                // Obtener una referencia de nuestra parte.
                ShareClient share = new ShareClient(ConnectionString, AccountName);

                // Obtener una referencia de nuestro directorio - directorio ubicado en el nivel raíz.
                ShareDirectoryClient directory = share.GetDirectoryClient(sDirName);

                // Obtener una referencia a un subdirectorio que no se encuentra en el nivel raíz
                directory = directory.GetSubdirectoryClient(sReferencia);

                // Obtener una referencia a nuestro archivo.
                ShareFileClient file = directory.GetFileClient(sFileName);

                // Get a reference to the file we created previously
                ShareFileClient sourceFile = new ShareFileClient(ConnectionString, AccountName, file.Path);

                // Ensure that the source file exists
                if (sourceFile.Exists())
                {
                    string sDirName_CODISE = "codisestorage";

                    var dateAndTime = DateTime.Now;
                    var Date = dateAndTime.ToShortDateString();

                    DateTime myDateTime = DateTime.Parse(Date);

                    string año = Convert.ToString(myDateTime.Year);
                    string mes = myDateTime.ToString("MM");
                    string dia = myDateTime.ToString("dd");

                    // Obtener una referencia de nuestra parte.
                    ShareClient share_codise = new ShareClient(ConnectionString, sDirName_CODISE);

                    // Validar si la carpeta del dia existe
                    ShareDirectoryClient directory_codise = share_codise.GetDirectoryClient(sDirName_CODISE);

                    directory_codise = directory_codise.GetSubdirectoryClient(año);
                    directory_codise = directory_codise.GetSubdirectoryClient(mes);
                    directory_codise = directory_codise.GetSubdirectoryClient(dia);
                    //directory_codise.CreateSubdirectory("prueba");

                    if (!directory_codise.Exists())
                    {
                        directory_codise = directory_codise.GetSubdirectoryClient("../");
                        // CreateDirectory - Día
                        directory_codise.CreateSubdirectory(dia);
                        directory_codise = directory_codise.GetSubdirectoryClient(dia);
                    }

                    // CreateDirectory - Consecutivo
                    directory_codise.CreateSubdirectory(Convert.ToString(iConsecutivo));

                    string destFilePath = sDirName_CODISE + "/" + año + "/" + mes +  "/" + dia + "/" + iConsecutivo + "/" + sFileName;

                    // Get a reference to the destination file
                    ShareFileClient destFile = new ShareFileClient(ConnectionString, sDirName_CODISE, destFilePath);

                    // Start the copy operation
                    destFile.StartCopy(sourceFile.Uri);

                    if (destFile.Exists())
                    {
                        Console.WriteLine($"{sourceFile.Uri} copied to {destFile.Uri}");
                    }
                }
            } catch (Exception ex)
            {
                string sError = ex.Message;
            }
        }

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }
    }
}