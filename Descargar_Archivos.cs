using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using System.IO;

namespace WebItNow
{
    public class Descargar_Archivos
    {


        /// <summary>
        /// Descargar un archivo.
        /// </summary>
        /// <param name="connectionString">
        /// Una cadena de conexión a su cuenta de Azure Storage.
        /// </param>
        /// <param name="shareName">
        /// El nombre del recurso compartido desde el que descargar.
        /// </param>
        /// <param name="localFilePath">
        /// Ruta para descargar el archivo local.
        /// </param>
        public async Task<bool> Archivos_Descarga(string localFilePath)
        {
            var ConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
            var AccountName = CloudConfigurationManager.GetSetting("StorageAccountName");
            //  var AccountKey = ConfigurationManager.AppSettings["StorageAccountKey"];

            //string dirName = "sample-dir";
            //string fileName = "sample-file";

            string dirName = "itnowstorage/USUARIO2/OTR/";
            string fileName = "Martin Baltierra Firma.jpg";
            string localFilePath1 = "C:/Users/Dell/Downloads";

            // Obtener una referencia al archivo
            ShareClient share = new ShareClient(ConnectionString, AccountName);
            ShareDirectoryClient directory = share.GetDirectoryClient(dirName);
            ShareFileClient file = directory.GetFileClient(fileName);

            // Download the file
            ShareFileDownloadInfo download = await file.DownloadAsync();
            using (FileStream stream = File.OpenWrite(localFilePath1))
            {
                await download.Content.CopyToAsync(stream);
            }

            return true;

        }

    }

}