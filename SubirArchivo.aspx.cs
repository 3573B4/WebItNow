
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

    public partial class SubirArchivo : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //string valor = ddlDocs.SelectedValue;
            //Image1.ImageUrl = "~/Images/icono-Subir-Archivo-morado.png";
            if (!IsPostBack)
            {
                BtnEnviar.Enabled = true;
                getDocRequeridos();

                // * * Obtener Descripcion breve documento
                string IdDoc = ddlDocs.SelectedValue.ToString();
                LblDescrpBrev.Text = TpoDocumento_DescrpBrev(IdDoc, 1);

                getDocsUsuario();
                checarStatusDoc();
                string userId = Convert.ToString(Session["IdUsuario"]);
                lblUsuario.Text = userId;
            }

            //* * Agrega THEAD y TBODY a GridView.
            gvEstadoDocs.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        public void checarStatusDoc()
        {
            string user = Convert.ToString(Session["IdUsuario"]);
            string tpoDoc = ddlDocs.SelectedValue;
            ConexionBD connect = new ConexionBD();
            connect.Abrir();

            // Consulta a la tabla Estado de Documento
            string edoQuery = " SELECT IdUsuario, IdTipoDocumento, IdStatus " +
                                "FROM ITM_04 " +
                                "WHERE IdUsuario = '" + user + "' " +
                                "AND IdTipoDocumento = '" + tpoDoc + "'";

            SqlCommand cmd = new SqlCommand(edoQuery, connect.ConectarBD);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                BtnEnviar.Enabled = false;

                LblMessage.Text = "No existe tipo de documento, para este usuario.";
                mpeMensaje.Show();
            }
            else
            {
                 Variables.wEdoDoc = dt.Rows[0]["IdStatus"].ToString();

                if (Variables.wEdoDoc == "0" || Variables.wEdoDoc == "1" )
                {
                    BtnEnviar.Enabled = true;
                }
                else
                {
                    BtnEnviar.Enabled = false;
                }
            }
        }

        public void getDocRequeridos()
        {
            ConexionBD conectar = new ConexionBD();
            conectar.Abrir();

            // Consulta a la tabla Tipo de Documento
            string sqlQuery = "SELECT IdTpoDocumento, Descripcion " +
                                "FROM ITM_06 " +
                                "WHERE Status = '1'";

            SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlDocs.DataSource = dt;

            ddlDocs.DataValueField = "IdTpoDocumento";
            ddlDocs.DataTextField = "Descripcion";

            ddlDocs.DataBind();

            //cmd.Dispose();
            //da.Dispose();
            //conectar.Cerrar();

            //ListItem i;
            //foreach(DataRow row in dt.Rows)
            //{
            //    i = new ListItem(row["Descripcion"].ToString(), row["IdTpoDocumento"].ToString());
            //    ddlDocs.Items.Add(i);
            //}

        }

        public void getDocsUsuario()
        {
            string user = /*"USUARIO4"*/ Convert.ToString(Session["IdUsuario"]);
            ConexionBD Conectar = new ConexionBD();
            Conectar.Abrir();

            //string sqlQuery = "SELECT IdTipoDocumento, IdStatus " +
            //                    "FROM ITM_04 " +
            //                    "WHERE IdUsuario = '"+ user +"'";

            // Consulta a las tablas : Estado de Documento (Expediente) = ITM_04
            // Tipo de Documento = ITM_06
            // Status de Documento = ITM_07

            string sqlQuery = "SELECT ed.IdUsuario, ed.IdTipoDocumento, ed.Nom_Imagen, td.Descripcion, s.Descripcion as Desc_Status  " +
                                  "  FROM ITM_04 ed, ITM_06 td, ITM_07 s " +
                                  " WHERE ed.IdStatus = s.IdStatus And ed.IdTipoDocumento = td.IdTpoDocumento " +
                                  "   AND IdUsuario = '" + user + "'";

            SqlCommand ejecucion = new SqlCommand(sqlQuery, Conectar.ConectarBD);
            SqlDataAdapter da = new SqlDataAdapter(ejecucion);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                gvEstadoDocs.ShowHeaderWhenEmpty = true;
                gvEstadoDocs.EmptyDataText = "No hay resultados.";
            }

            gvEstadoDocs.DataSource = dt;
            gvEstadoDocs.DataBind();

        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                //string directorio = "https://itnowtech18-my.sharepoint.com/:f:/g/personal/llg_peacock_claims/Ekb4AdD2Id1KgMI9CRoIAU4BdP795N2YLyTJxmlMmIfWUA?e=CCtbfK" + "/";
                string directorio = "~/itnowstorage/";
                string user = Convert.ToString(Session["IdUsuario"]);
                string folderName = ddlDocs.SelectedValue;
                string directFinal = directorio + user + "/" + folderName + "/";
                string UrlFinal = user + "/" + folderName + "/";
                string directorioURL = Server.MapPath(directFinal);
                string nomFile = FileUpload1.FileName;
                int tamArchivo = FileUpload1.PostedFile.ContentLength;

                if (FileUpload1.HasFile)
                {

                    if (tamArchivo == 0 )
                    {
                        LblMessage.Text = "El archivo esta dañado";
                        mpeMensaje.Show();
                        return;
                    }

                    if (tamArchivo <= 40000000)
                    {

                        ConexionBD Conectar = new ConexionBD();
                        Conectar.Abrir();

                        // Actualizar la tabla Estado de Documento
                        string sqlUpDate = "UPDATE ITM_04 " +
                                           "    SET IdStatus = '2'," +
                                                " Url_Imagen = '" + UrlFinal + "'," +
                                                " Nom_Imagen = '" + nomFile + "'," +
                                                "  Fec_Envio = GETDATE() " +
                                            " WHERE IdUsuario = '" + user + "'" +
                                            " AND IdTipoDocumento = '" + folderName + "'";

                        SqlCommand cmd = new SqlCommand(sqlUpDate, Conectar.ConectarBD);


                        //if (!Directory.Exists(directorioURL))
                        //{
                        //    Directory.CreateDirectory(directorioURL);
                        //}
                        //if (File.Exists(directorioURL + nomFile))
                        //{
                        //    //Console.WriteLine("El documento sI existe");
                        //    LblMessage.Text = "El documento ya existe";
                        //    mpeMensaje.Show();
                        //}
                        //else
                        //{

                        string filepath = Server.MapPath(directorio + FileUpload1.FileName);
                        FileUpload1.SaveAs(filepath);
                        string sPath = System.IO.Path.GetDirectoryName(filepath) + "/" + nomFile;

                        UploadToAzure(nomFile, sPath);

                        // DELETE AL ARCHIVO --> (sPath)
                        File.Delete(sPath);

                        //   FileUpload1.SaveAs(Server.MapPath(directFinal /*+ folderName + "-"*/ + FileUpload1.FileName));
                        
                        cmd.ExecuteReader();

                        getDocsUsuario();
                        checarStatusDoc();

                        var email = new EnvioEmail();
                        string sEmail = email.CorreoElectronico(user);
                        int Envio_Ok = email.EnvioMensaje(user, sEmail, "Documento Enviado");


                        //}

                    }
                    
                    else
                    {
                    LblMessage.Text = "El documento Exede los 40 MB";
                    mpeMensaje.Show();

                    }

                }
                

                else
                {
                    LblMessage.Text = "Debe seleccionar un archivo";
                    mpeMensaje.Show();


                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void ddlDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            string IdDoc = ddlDocs.SelectedValue.ToString();

            LblDescrpBrev.Text = TpoDocumento_DescrpBrev(IdDoc, 1);

            checarStatusDoc();

            //ConexionBD Conectar = new ConexionBD();
            //Conectar.Abrir();

            //// Consulta a la tabla Tipo de Documento
            //string sqlQuery = "SELECT IdTpoDocumento, DescrpBrev " +
            //                    "FROM ITM_06 " +
            //                    "WHERE Status = '1'";
            //SqlCommand cmd1 = new SqlCommand(sqlQuery, Conectar.ConectarBD);

            //SqlDataAdapter da = new SqlDataAdapter(cmd1);
            //DataTable dt = new DataTable();

            //da.Fill(dt);

        }

        public string TpoDocumento_DescrpBrev(String pIdTpoDocumento, int pIdStatus)
        {
            try
            {
                string DescrpBrev = string.Empty;

                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Consulta a la tabla Tipo de Documento
                string strQuery = "Select IdTpoDocumento, DescrpBrev From ITM_06 Where IdTpoDocumento = '" + pIdTpoDocumento + "' And Status = " + pIdStatus + "";

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

        protected void gvEstadoDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            // int var = 0;
        }

        public void UploadToAzure(string sFilename, string sPath)
        {
            try
            {

                string ConnectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");
                string AccountName = ConfigurationManager.AppSettings.Get("StorageAccountName");

                // Name of the directory, and file
                string dirName = "itnowstorage";
                string fileName = sFilename;

                // Get a reference from our share 
                ShareClient share = new ShareClient(ConnectionString, AccountName);

                // Get a reference from our directory - directory located on root level
                ShareDirectoryClient directory = share.GetDirectoryClient(dirName);

                string sUsuario = Convert.ToString(Session["IdUsuario"]);
                string sTpoDocumento = ddlDocs.SelectedValue;

                //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                //CloudFileClient fileclient = storageAccount.CreateCloudFileClient();
                //CloudFileShare share1 = fileclient.GetShareReference("vault");

                //CloudFileDirectory rootdir = share1.GetRootDirectoryReference();
                //var dir = rootdir.GetDirectoryReference("itnowstorage/USUARIO2");

                //dir.CreateIfNotExists();


                if (Variables.wEdoDoc == "0")
                {
                    // CreateDirectory
                    directory.CreateSubdirectory(sUsuario);
                    directory = directory.GetSubdirectoryClient(sUsuario);
                    directory = directory.CreateSubdirectory(sTpoDocumento);
                }
                else
                {
                    // Get a reference to a subdirectory not located on root level
                    directory = directory.GetSubdirectoryClient(sUsuario);
                    directory = directory.GetSubdirectoryClient(sTpoDocumento);
                }

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
                    const int uploadLimit = 40000000;

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

                            LblMessage.Text = "El documento se envio exitosamente";
                            mpeMensaje.Show();
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


            } catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }
    }
    }
