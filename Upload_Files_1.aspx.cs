
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.IO.Compression;
using Azure.Storage.Files.Shares;
using Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.File;

namespace WebItNow
{
    public partial class Upload_Files_1 : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                BtnEnviar.Enabled = true;
                string sTpoDocumento = Convert.ToString(Session["TpoDocumento"]);

                GetDocRequeridos(sTpoDocumento);

                //// * * Obtener Descripcion breve documento
                // string IdDoc = ddlDocs.SelectedValue.ToString();

                // TxtDescrpBrev.Text = TpoDocumento_DescrpBrev(IdDoc, 1);

                ChecarStatusDoc();

                string sReferencia = Convert.ToString(Session["Referencia"]);
                lblUsuario.Text = "Referencia: " + sReferencia;
            }
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                string directorio = "~/itnowstorage/";
                string sReferencia = Convert.ToString(Session["Referencia"]);
            //  string folderName = ddlDocs.SelectedValue;
                string folderName = Convert.ToString(Session["TpoDocumento"]);
                //string directFinal = directorio + sReferencia + "/" + folderName + "/";
                //string UrlFinal = sReferencia + "/" + folderName + "/";
                string directFinal = directorio + sReferencia + "/" ;
                string UrlFinal = sReferencia + "/" ;
                string directorioURL = Server.MapPath(directFinal);

                string nomFile = FileUpload1.FileName;
                string fileName = System.IO.Path.GetFileName(FileUpload1.FileName);

                int tamArchivo = FileUpload1.PostedFile.ContentLength;
                string extensionFile = Path.GetExtension(FileUpload1.FileName);

                if (FileUpload1.HasFile)
                {
                    if (tamArchivo == 0)
                    {
                        LblMessage.Text = "El archivo esta dañado";
                        mpeMensaje.Show();
                        return;
                    }
                    if ((extensionFile == ".exe") || (extensionFile == ".js") || (extensionFile == ".jar") || (extensionFile == ".jdk"))
                    {
                        LblMessage.Text = "El archivo tiene un formato invalido";
                        mpeMensaje.Show();
                    }
                    else
                    {
                        
                        // 64 mb es = a 67,108,864 bytes
                        if (tamArchivo <= 70000000)
                        {

                            ConexionBD Conectar = new ConexionBD();
                            Conectar.Abrir();

                            // Actualizar la tabla Estado de Documento
                            string sqlUpDate = "UPDATE ITM_04 " +
                                               "    SET IdStatus = '2'," +
                                                    " Url_Imagen = '" + UrlFinal + "'," +
                                                    " Nom_Imagen = '" + nomFile + "'," +
                                                    "  Fec_Envio = GETDATE() " +
                                                " WHERE Referencia LIKE '%' + '" + sReferencia + "'  + '%' " +
                                                " AND IdTipoDocumento = '" + folderName + "'";

                            SqlCommand cmd = new SqlCommand(sqlUpDate, Conectar.ConectarBD);

                            string filepath = Server.MapPath(directorio + FileUpload1.FileName);
                            FileUpload1.SaveAs(filepath);
                            string sPath = System.IO.Path.GetDirectoryName(filepath) + "\\" + nomFile;

                            System.Threading.Thread.Sleep(10000);
                            this.UploadToAzure(nomFile, sPath);

                            //UploadToAzure(nomFile, sPath);

                            // DELETE AL ARCHIVO --> (sPath)
                            File.Delete(sPath);

                            cmd.ExecuteReader();

                            ChecarStatusDoc();

                            Session["Referencia"] = sReferencia;
                            Session["Asunto"] = "Documento Enviado";

                            Response.Redirect("Page_Message.aspx",true);
                         // Server.Transfer("Page_Message.aspx");

                            //var email = new EnvioEmail();
                            //string sEmail = email.CorreoElectronico(user);
                            //int Envio_Ok = email.EnvioMensaje(user, sEmail, "Documento Enviado");

                        }

                        else
                        {
                            LblMessage.Text = "El documento Excede los 70 MB";
                            mpeMensaje.Show();

                        }
                    }
                }
                else
                {
                    LblMessage.Text = "Debe seleccionar un archivo";
                    mpeMensaje.Show();

                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetDocRequeridos(string sTpoDocumento)
        {
            ConexionBD conectar = new ConexionBD();
            conectar.Abrir();

            // Consulta a la tabla Tipo de Documento
            string sqlQuery = "SELECT IdTpoDocumento, Descripcion, DescrpBrev " +
                                "FROM ITM_06 " +
                                "WHERE IdTpoDocumento = '" + sTpoDocumento + "' " +
                                "  AND IdStatus = '1'";

            SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
            SqlDataReader dt = cmd.ExecuteReader();

            if (dt.HasRows)
            {
                while (dt.Read())
                {
                    LblDescripcion.Text = dt["Descripcion"].ToString().Trim();
                    TxtDescrpBrev.Text = dt["DescrpBrev"].ToString().Trim();
                }
            }

        }

        public string TpoDocumento_DescrpBrev(String pIdTpoDocumento, int pIdStatus)
        {
            try
            {
                string DescrpBrev = string.Empty;

                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Consulta a la tabla Tipo de Documento
                string strQuery = "Select IdTpoDocumento, DescrpBrev From ITM_06 Where IdTpoDocumento = '" + pIdTpoDocumento + "' And IdStatus = " + pIdStatus + "";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    DescrpBrev = Convert.ToString(row[1]);
                }

                Conecta.Cerrar();

                return DescrpBrev;

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

            return string.Empty;
        }

        public void ChecarStatusDoc()
        {
            string sReferencia = Convert.ToString(Session["Referencia"]);
        //  string tpoDoc = ddlDocs.SelectedValue;
            string tpoDoc = Convert.ToString(Session["TpoDocumento"]);

            ConexionBD connect = new ConexionBD();
            connect.Abrir();

            // Consulta a la tabla Estado de Documento
            string edoQuery = " SELECT IdTipoDocumento, IdStatus " +
                                "FROM ITM_04 " +
                                "WHERE Referencia LIKE '%' + '" + sReferencia + "'  + '%' " +
                                "AND IdTipoDocumento = '" + tpoDoc + "'";

            SqlCommand cmd = new SqlCommand(edoQuery, connect.ConectarBD);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                BtnEnviar.Enabled = false;


                //LblMessage.Text = "No existe tipo de documento, para esta referencia.";
                //mpeMensaje.Show();
            }
            else
            {
                Variables.wEdoDoc = dt.Rows[0]["IdStatus"].ToString();

                if (Variables.wEdoDoc == "0" || Variables.wEdoDoc == "1")
                {
                    BtnEnviar.Enabled = true;
                }
                else
                {
                    //  BtnEnviar.Enabled = false;
                }
            }
        }

        protected void DdlDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
        //  string IdDoc = ddlDocs.SelectedValue.ToString();
            string IdDoc = Convert.ToString(Session["TpoDocumento"]);
            TxtDescrpBrev.Text = TpoDocumento_DescrpBrev(IdDoc, 1);

            ChecarStatusDoc();
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Upload_Files.aspx");
        }

        public void UploadToAzure(string sFilename, string sPath)
        {
            try
            {

                string ConnectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");
                string AccountName = ConfigurationManager.AppSettings.Get("StorageAccountName");

                // Name of the directory, and file
                // string dirName = "itnowstorage";
                string fileName = sFilename;

                // Get a reference from our share 
                ShareClient share = new ShareClient(ConnectionString, AccountName);

                // Get a reference from our directory - directory located on root level
                ShareDirectoryClient directory = share.GetDirectoryClient(AccountName);

                string sReferencia = Convert.ToString(Session["Referencia"]);
            //  string sTpoDocumento = ddlDocs.SelectedValue;
                string sTpoDocumento = Convert.ToString(Session["TpoDocumento"]);

                // Get a reference to a subdirectory not located on root level
                directory = directory.GetSubdirectoryClient(sReferencia);

                if (!directory.Exists())
                {
                    directory = directory.GetSubdirectoryClient("../");
                    // CreateDirectory - Día
                    directory.CreateSubdirectory(sReferencia);
                    directory = directory.GetSubdirectoryClient(sReferencia);
                }

                //if (Variables.wEdoDoc == "0")
                //{
                //    // CreateDirectory
                //    directory.CreateSubdirectory(sReferencia);
                //    directory = directory.GetSubdirectoryClient(sReferencia);
                //    directory = directory.CreateSubdirectory(sTpoDocumento);
                //}
                //else
                //{
                //    // Get a reference to a subdirectory not located on root level
                //    directory = directory.GetSubdirectoryClient(sReferencia);
                //    directory = directory.GetSubdirectoryClient(sTpoDocumento);
                //}

                // Get a reference to our file
                ShareFileClient file = directory.GetFileClient(fileName);

                if (file.Exists())
                {
                    LblMessage.Text = "El documento ya existe";
                    mpeMensaje.Show();
                }
                else
                {
                    // Si el documento no existe
                    // Max. 4MB (4194304 Bytes in binary) allowed
                    const int uploadLimit = 70000000;

                    // Upload the file
                    using (FileStream stream = File.OpenRead(sPath))
                    {
                        file.Create(stream.Length);
                        if (stream.Length <= uploadLimit)
                        {
                            //file.UploadRange(
                            //new HttpRange(0, stream.Length),
                            //stream);

                            int blockSize = 32 * 1024;
                            long offset = 0;    // Definir desplazamiento de rango http.
                            BinaryReader reader = new BinaryReader(stream);
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

                            //LblMessage.Text = "El documento se envio exitosamente";
                            //mpeMensaje.Show();
                        }
                        else
                        {
                            int bytesRead;
                            long index = 0;
                            byte[] buffer = new byte[uploadLimit];
                            // La transmisión es más grande que el límite, por lo que debemos cargarla en fragmentos
                            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                // Cree un flujo de memoria para que el búfer lo cargue
                                MemoryStream ms = new MemoryStream(buffer, 0, bytesRead);
                                file.UploadRange(new HttpRange(index, ms.Length), ms);
                                index += ms.Length; // increment the index to the account for bytes already written
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

    }
}