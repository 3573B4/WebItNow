
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

            string sReferencia = Convert.ToString(Session["Referencia"]);
            lblUsuario.Text = "Referencia: " + sReferencia;
            //Variables.wExiste = true;
            
            if (!IsPostBack)
            {

                BtnEnviar.Enabled = true;
                string sTpoDocumento = Convert.ToString(Session["TpoDocumento"]);

                GetDocRequeridos(sTpoDocumento);

                //// * * Obtener Descripcion breve documento
                // string IdDoc = ddlDocs.SelectedValue.ToString();

                // TxtDescrpBrev.Text = TpoDocumento_DescrpBrev(IdDoc, 1);

                ChecarStatusDoc();

                //string sReferencia = Convert.ToString(Session["Referencia"]);
                //lblUsuario.Text = "Referencia: " + sReferencia;
            }
        }

        protected void BtnEnviar_Click_BK(object sender, EventArgs e)
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

                //Access the File using the Name of HTML INPUT File.
                HttpPostedFile postedFile = Request.Files["oFile"];

                string nomFile = postedFile.FileName;
                string fileName = System.IO.Path.GetFileName(postedFile.FileName);
                int tamArchivo = postedFile.ContentLength;
                string extensionFile = Path.GetExtension(postedFile.FileName);

                //string nomFile = FileUpload1.FileName;
                //string fileName = System.IO.Path.GetFileName(FileUpload1.FileName);
                //int tamArchivo = FileUpload1.PostedFile.ContentLength;
                //string extensionFile = Path.GetExtension(FileUpload1.FileName);

                if (postedFile.FileName != "")
                //if (FileUpload1.HasFile)
                {
                    if (tamArchivo >= 70000000)
                    {
                        LblMessage.Text = "El documento Excede los 70 MB";
                        mpeMensaje.Show();
                        return;
                    }

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
                        return;
                    }
                    else
                    {
                        // 64 mb es = a 67,108,864 bytes
                        if (tamArchivo <= 70000000)
                        {

                            //ConexionBD Conectar = new ConexionBD();
                            //Conectar.Abrir();

                            //// Actualizar la tabla Estado de Documento
                            //string sqlUpDate = "UPDATE ITM_04 " +
                            //                   "    SET IdStatus = '2'," +
                            //                        " Url_Imagen = '" + UrlFinal + "'," +
                            //                        " Nom_Imagen = '" + nomFile + "'," +
                            //                        "  Fec_Envio = GETDATE() " +
                            //                    " WHERE Referencia LIKE '%' + '" + sReferencia + "'  + '%' " +
                            //                    " AND IdTipoDocumento = '" + folderName + "'";

                            //SqlCommand cmd = new SqlCommand(sqlUpDate, Conectar.ConectarBD);

                            string filepath = Server.MapPath(directorio + postedFile.FileName);
                            postedFile.SaveAs(filepath);
                            string sPath = System.IO.Path.GetDirectoryName(filepath) + "\\" + nomFile;

                            //string filepath = Server.MapPath(directorio + FileUpload1.FileName);
                            //FileUpload1.SaveAs(filepath);
                            //string sPath = System.IO.Path.GetDirectoryName(filepath) + "\\" + nomFile;

                            System.Threading.Thread.Sleep(10000);
                            this.UploadToAzure(nomFile, sPath);

                            //UploadToAzure(nomFile, sPath);

                            if(Variables.wExiste)
                            {

                                // DELETE AL ARCHIVO --> (sPath)
                                File.Delete(sPath);

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

                                cmd.ExecuteReader();

                                ChecarStatusDoc();

                                Session["Referencia"] = sReferencia;
                                Session["Asunto"] = "Documento Recibido";

                                Response.Redirect("Page_Message.aspx", true);
                            }
                            else
                            {
                                //Response.Redirect(Request.RawUrl);
                                Variables.wExiste = true;

                                LblMessage.Text = "El documento ya existe";
                                mpeMensaje.Show();
                                return;
                                // HttpContext.Current.ApplicationInstance.CompleteRequest();
                            }


                         // Server.Transfer("Page_Message.aspx");

                            //var email = new EnvioEmail();
                            //string sEmail = email.CorreoElectronico(user);
                            //int Envio_Ok = email.EnvioMensaje(user, sEmail, "Documento Enviado");

                        }

                        else
                        {
                            LblMessage.Text = "El documento Excede los 70 MB";
                            mpeMensaje.Show();
                            return;
                        }
                    }
                }
                else
                {
                    LblMessage.Text = "Debe seleccionar un archivo";
                    mpeMensaje.Show();
                    return;


                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
                return;
            }
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                string sReferencia = Convert.ToString(Session["Referencia"]);

                //Access the File using the Name of HTML INPUT File.
                HttpPostedFile postedFile = Request.Files["oFile"];

                string nomFile = postedFile.FileName;

                if (postedFile.FileName != "")
                {
                    this.UploadToAzure(nomFile, sReferencia);
                }
                else
                {
                    LblMessage.Text = "Debe seleccionar un archivo";
                    mpeMensaje.Show();
                    return;
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
                return;
            }
        }

        public void GetDocRequeridos(string sTpoDocumento)
        {
            ConexionBD conectar = new ConexionBD();
            conectar.Abrir();

            // Consulta a la tabla Tipo de Documento
            //string sqlQuery = "SELECT IdTpoDocumento, Descripcion, DescripBrev " +
            //                    "FROM ITM_06 " +
            //                    "WHERE IdTpoDocumento = '" + sTpoDocumento + "' " +
            //                    "  AND IdStatus = '1'";

            string sqlQuery = "SELECT IdTpoDocumento, Descripcion, DescripBrev " +
                                "FROM ITM_08 " +
                                "WHERE IdTpoDocumento = '" + sTpoDocumento + "' " +
                                "  AND IdStatus = '1'";

            SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
            SqlDataReader dt = cmd.ExecuteReader();

            if (dt.HasRows)
            {
                while (dt.Read())
                {
                    LblDescripcion.Text = dt["Descripcion"].ToString().Trim();
                    TxtDescrpBrev.Text = dt["DescripBrev"].ToString().Trim();
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
            // Response.Redirect(Request.RawUrl);
            // Response.Redirect(Page.Request.Url.ToString(), true);

            //Page.Response.Redirect(Page.Request.Url.ToString(), false);
            //Context.ApplicationInstance.CompleteRequest();

            Response.Redirect(Page.Request.Path);
        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Upload_Files.aspx");
        }

        public void UploadToAzure(string sFilename, string sDirName)
        {

            string sReferencia = sDirName;

            string ConnectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");
            string AccountName = ConfigurationManager.AppSettings.Get("StorageAccountName");

            try
            {
                // Get a reference to a share and then create it
                ShareClient share = new ShareClient(ConnectionString, AccountName);

                // Get a reference to a directory and create it
                ShareDirectoryClient directory = share.GetDirectoryClient(AccountName);

                // Get a reference to a subdirectory not located on root level
                directory = directory.GetSubdirectoryClient(sDirName);

                if (!directory.Exists())
                {
                    directory = directory.GetSubdirectoryClient("../");
                    directory.CreateSubdirectory(sDirName);
                    directory = directory.GetSubdirectoryClient(sDirName);
                }

                // Get a reference to a file and upload it
                ShareFileClient file = directory.GetFileClient(sFilename);

                if (file.Exists())
                {
                    //Variables.wExiste = true;

                    LblMessage.Text = "El documento ya existe";
                    mpeMensaje.Show();
                    return;
                }
                else
                {
                    //Access the File using the Name of HTML INPUT File.
                    HttpPostedFile postedFile = Request.Files["oFile"];

                    file.Create(postedFile.ContentLength);

                    int blockSize = 64 * 1024;
                    long offset = 0;    // Definir desplazamiento de rango http.

                    BinaryReader reader = new BinaryReader(postedFile.InputStream);
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

                    // DELETE AL ARCHIVO --> (sPath)
                    // File.Delete(sPath);

                    string folderName = Convert.ToString(Session["TpoDocumento"]);

                    ConexionBD Conectar = new ConexionBD();
                    Conectar.Abrir();

                    // Actualizar la tabla Estado de Documento
                    string sqlUpDate = "UPDATE ITM_04 " +
                                        "    SET IdStatus = '2'," +
                                            " Url_Imagen = '" + sDirName + "'," +
                                            " Nom_Imagen = '" + sFilename + "'," +
                                            "  Fec_Envio = GETDATE() " +
                                        " WHERE Referencia LIKE '%' + '" + sReferencia + "'  + '%' " +
                                        " AND IdTipoDocumento = '" + folderName + "'";

                    SqlCommand cmd = new SqlCommand(sqlUpDate, Conectar.ConectarBD);

                    cmd.ExecuteReader();

                    ChecarStatusDoc();

                    Session["Referencia"] = sReferencia;
                    Session["Asunto"] = "Documento Recibido";

                    Response.Redirect("Page_Message.aspx", true);
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