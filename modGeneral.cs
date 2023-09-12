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

        public bool valTextGrande(string textGrande)
        {
            char[] a = { '{', '}', '/', '[', ']', '`', '^', '~', '+', '*', '´', '¨', '¿', '¡', '?', '\\', '=', ')', '(', '&', '%', '$', '#', '"', '!', '|', '°', '¬', '-', '_' };

            for (int i = 0; i < textGrande.Length; i++)
            {
                for (int j = 0; j < a.Length; j++)
                {
                    if (textGrande[i] == a[j])
                    {

                        return false;
                        //break;
                    }
                }
            }
            return true;
        }

        public void CopyFileAzure(string sReferencia, string sFileName, string sTpoDocumento, int iConsecutivo)
        {
            try
            {

                // Get the connection string from app settings
                string ConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
                string AccountName = ConfigurationManager.AppSettings.Get("StorageAccountName");
                //string sDirName = "itnowstorage";

                // Obtener una referencia de nuestra parte.
                ShareClient share = new ShareClient(ConnectionString, AccountName);

                // Obtener una referencia de nuestro directorio - directorio ubicado en el nivel raíz.
                ShareDirectoryClient directory = share.GetDirectoryClient(AccountName);

                // Obtener una referencia a un subdirectorio que no se encuentra en el nivel raíz
                directory = directory.GetSubdirectoryClient(sReferencia);

                // Obtener una referencia a nuestro archivo.
                ShareFileClient file = directory.GetFileClient(sFileName);

                // Get a reference to the file we created previously
                ShareFileClient sourceFile = new ShareFileClient(ConnectionString, AccountName, file.Path);

                // Ensure that the source file exists
                if (sourceFile.Exists())
                {
                    string AccountCodice = ConfigurationManager.AppSettings.Get("StorageAccountCodice");
                //  string sDirName_CODISE = "codisestorage";

                    var dateAndTime = DateTime.Now;
                    var Date = dateAndTime.ToShortDateString();

                    DateTime myDateTime = DateTime.Parse(Date);

                    string año = Convert.ToString(myDateTime.Year);
                    string mes = myDateTime.ToString("MM");
                    string dia = myDateTime.ToString("dd");

                    // Obtener una referencia de nuestra parte.
                    ShareClient share_codise = new ShareClient(ConnectionString, AccountCodice);

                    // Validar si la carpeta del dia existe
                    ShareDirectoryClient directory_codise = share_codise.GetDirectoryClient(AccountCodice);

                    directory_codise = directory_codise.GetSubdirectoryClient(año);
                    directory_codise = directory_codise.GetSubdirectoryClient(mes);
                    directory_codise = directory_codise.GetSubdirectoryClient(dia);

                    string extencion = Path.GetExtension(sFileName);
                    string sFileName_New = mes + dia + "_" + iConsecutivo + extencion;

                    if (!directory_codise.Exists())
                    {
                        directory_codise = directory_codise.GetSubdirectoryClient("../");
                        // CreateDirectory - Día
                        directory_codise.CreateSubdirectory(dia);
                        directory_codise = directory_codise.GetSubdirectoryClient(dia);
                    }

                    // CreateDirectory - Consecutivo
                    directory_codise.CreateSubdirectory(Convert.ToString(iConsecutivo));

                    string destFilePath = AccountCodice + "/" + año + "/" + mes + "/" + dia + "/" + iConsecutivo + "/" + sFileName_New;

                    // Get a reference to the destination file
                    ShareFileClient destFile = new ShareFileClient(ConnectionString, AccountCodice, destFilePath);

                    // Start the copy operation
                    destFile.StartCopy(sourceFile.Uri);

                    // Actualiza columnas Url_Path_Final - Nom_Imagen_Final
                    string Url_Path_Final = año + "/" + mes + "/" + dia + "/" + iConsecutivo + "/";
                    Actualiza_ITM_04(sReferencia, Url_Path_Final, sFileName_New, sTpoDocumento);

                    if (destFile.Exists())
                    {
                        Console.WriteLine($"{sourceFile.Uri} copied to {destFile.Uri}");
                    }

                }
            }
            catch (Exception ex)
            {
                string sError = ex.Message;
            }
        }

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }

        private void Actualiza_ITM_04(string sReferencia, string pUrl_Path, string pNom_Imagen, string sTpoDocumento)
        {
            // Insertar tabla ITM_15
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {
                string strQuery = "Update ITM_04 " +
                                   "   Set Url_Path_Final = '" + pUrl_Path + "'," +
                                   "       Nom_Imagen_Final = '" + pNom_Imagen + "' " +
                                   " Where Referencia = '" + sReferencia + "'" +
                                   "   And IdTipoDocumento = '" + sTpoDocumento + "'";


                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                cmd.ExecuteReader();

            }
            catch (Exception ex)
            {
                string sError = ex.Message;
            }
            finally
            {

            }
        }

    }
}