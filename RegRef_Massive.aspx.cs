using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;

using System.IO;
using SpreadsheetLight;
using System.Configuration;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

namespace WebItNow
{
    public partial class RegRef_Massive : System.Web.UI.Page
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

            }
        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }

        protected void BtnCargaExcel_Click(object sender, EventArgs e)
        {
            string directorio = "~/itnowstorage/";
            //string directorioURL = Server.MapPath(directorio);

            string nomFile = FileUpload1.FileName;
            string fileName = System.IO.Path.GetFileName(FileUpload1.FileName);

            int tamArchivo = FileUpload1.PostedFile.ContentLength;
            string extensionFile = Path.GetExtension(FileUpload1.FileName);


            if (FileUpload1.HasFile)
            {
                modGeneral modGeneral1 = new modGeneral();

                if (modGeneral1.validarCaracter(nomFile))
                {

                    if ((extensionFile == ".xlsx") || (extensionFile == ".xls"))
                    {
                        if (tamArchivo <= 70000000)
                        {
                            string filepath = Server.MapPath(directorio + FileUpload1.FileName);
                            FileUpload1.SaveAs(filepath);
                            string sPath = Path.GetDirectoryName(filepath) + "/" + nomFile;

                            System.Threading.Thread.Sleep(3000);
                            this.Cargar_Excel(sPath, nomFile);

                            // Eliminar archivo .xlsx del repositorio (solucion).
                            File.Delete(sPath);
                        }
                    }
                    else
                    {
                        LblMessage.Text = "El archivo tiene que ser un formato .xlsx";
                        mpeMensaje.Show();
                    }
                }
                else
                {
                    LblMessage.Text = "Nombre del archivo, no debe contener caracteres especiales";
                    mpeMensaje.Show();
                }
            }
            else
            {
                LblMessage.Text = "Debe seleccionar un archivo";
                mpeMensaje.Show();
            }

        }

        //protected void BtnEnviar_Click(object sender, EventArgs e)
        //{
        //    //* Validar si el usuario existe o es nuevo
        //    if (TxtRef.Text != "" && TxtEmail.Text != "")
        //    {

        //        // Insertar Registo Tabla tbUsuarios (UploadFiles)
        //        int result = Add_tbUsuarios(TxtRef.Text, TxtEmail.Text, string.Empty, 3, "Insert");

        //        if (result == 0)
        //        {
        //            // Insertar Registros Tabla tbEstadoDocumento [ITM_04]
        //            int idStatus = 1;
        //            int valor = Add_tbEstadoDocumento(TxtRef.Text, idStatus);

        //            var email = new EnvioEmail();
        //            int Envio_Ok = email.EnvioMensaje(TxtRef.Text.Trim(), TxtEmail.Text.Trim(), "Registro Referencia ", string.Empty);

        //            //EnvioMensaje(TxtUsu.Text.Trim(), TxtEmail.Text.Trim());

        //            if (Envio_Ok == 0)
        //            {
        //                LblMessage.Text = "Usuario fue insertado correctamente ";
        //                this.mpeMensaje.Show();
        //            }

        //            Limpia(this.Controls);

        //            // Response.Redirect("Login.aspx");
        //            Lbl_Message.Visible = false;
        //        }
        //    }
        //    else
        //    {
        //        Lbl_Message.Visible = true;
        //        Lbl_Message.Text = "* Estos campos son obligatorios";
        //    }
        //}

        protected void imgExcel_Click(object sender, EventArgs e)
        {
            try
            {

                Session["Filename"] = "Carga_Referencia.xlsx";

                string sFilename = "Carga_Referencia.xlsx";
                string sSubdirectorio = "Plantillas/";

                System.Threading.Thread.Sleep(2000);
                DownloadFromAzure(sFilename, sSubdirectorio);

                string mensaje = "window.open('Descargas.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenWindow", mensaje, true);

            } catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        protected void DownloadFromAzure(string sFilename, string sSubdirectorio)
        {

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

                using (FileStream stream = File.OpenWrite(directorioURL))
                {

                    //                              32768  
                    download.Content.CopyTo(stream, 327680);
                    stream.Flush();
                    stream.Close();
                }

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

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        public int Add_tbUsuarios(String pUsuarios, String pUsEmail, String pUsPassword, int pUsPrivilegios, string pStatementType)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbUsuario_StatementType", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@usuario", pUsuarios);
                cmd1.Parameters.AddWithValue("@email", pUsEmail);
                cmd1.Parameters.AddWithValue("@password", pUsPassword);
                cmd1.Parameters.AddWithValue("@privilegios", pUsPrivilegios);
                cmd1.Parameters.AddWithValue("@StatementType", pStatementType);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                if (dr1.Read())
                {

                    return dr1.GetInt32(0);

                }

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

                return 0;

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

            return -1;
        }

        public int Add_tbEstadoDocumento(String pReferencia, int pProceso, int pSubProceso, int pIdStatus)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Consulta a la tabla Tipo de Documento
                //string strQuery = "Select IdTpoDocumento, Descripcion From ITM_06 Where Status = " + pIdStatus + "";

                // Consulta a la tabla Tipo de Documento
                string strQuery = "Select IdTpoDocumento, Descripcion From ITM_06 " +
                                  " Where IdProceso = " + pProceso + "" +
                                  "   And IdSubProceso = " + pSubProceso + " And Status = " + pIdStatus + "";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

            //  SqlDataReader dr = cmd.ExecuteReader();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {

                    string IdTpoDocumento = Convert.ToString(row[0]);

                    int iIdStatus = 0;
                    int iIdDescarga = 0;
                    // Insert en la tabla Estado de Documento
                    SqlCommand cmd1 = new SqlCommand("Insert into ITM_04 (Referencia, IdTipoDocumento, IdStatus, IdDescarga) " +
                                        "Values ('" + pReferencia + "', '" + IdTpoDocumento + "', " + iIdStatus + ", " + iIdDescarga + ")", Conecta.ConectarBD);

                    SqlDataReader dr1 = cmd1.ExecuteReader();

                    cmd1.Dispose();
                    dr1.Dispose();
                }

                Conecta.Cerrar();

                return 0;

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

            return -1;
        }

        public int ValidaUser(String pUsuarios, int pUsPrivilegios)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbUsuario", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@usuario", pUsuarios);
                cmd1.Parameters.AddWithValue("@privilegios", pUsPrivilegios);
                SqlDataReader dr1 = cmd1.ExecuteReader();

                if (dr1.Read())
                {

                    return dr1.GetInt32(0);

                }

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

                return 0;

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

            return -1;
        }

        public int ValidaEmail(String pEmail, int pUsPrivilegios)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbEmail", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@email", pEmail);
                cmd1.Parameters.AddWithValue("@privilegios", pUsPrivilegios);
                SqlDataReader dr1 = cmd1.ExecuteReader();

                if (dr1.Read())
                {

                    return dr1.GetInt32(0);

                }

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

                return 0;

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

            return -1;
        }

        //protected void TxtRef_TextChanged(object sender, EventArgs e)
        //{
        //    // Validar si existe Usuario en la tabla ITM_02 (tbUsuarios)
        //    Variables.wPrivilegios = "3";
        //    int Usuario_Existe = ValidaUser(TxtRef.Text, Int32.Parse(Variables.wPrivilegios));

        //    if (Usuario_Existe == 1)
        //    {
        //        TxtRef.Text = string.Empty;
        //        TxtEmail.Focus();

        //        LblMessage.Text = "El nombre de usuario ya existe";
        //        this.mpeMensaje.Show();
        //    }
        //    else
        //    {
        //        TxtEmail.Focus();
        //    }
        //}

        //protected void TxtEmail_TextChanged(object sender, EventArgs e)
        //{
        //    // Validar si existe Email en la tabla ITM_02 (tbUsuarios)
        //    Variables.wPrivilegios = "3";
        //    int Email_Existe = ValidaEmail(TxtEmail.Text, Int32.Parse(Variables.wPrivilegios));

        //        if (Email_Existe == 1)
        //    {
        //        TxtEmail.Text = string.Empty;
        //        TxtEmail.Focus();

        //        LblMessage.Text = "El Correo electrónico ya existe";
        //        this.mpeMensaje.Show();
        //    }
        //    else
        //    {
        //        TxtEmail.Focus();
        //    }
        //}

        public void Limpia(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
                else if (control is DropDownList)
                    ((DropDownList)control).Items.Clear();
                else if (control is RadioButtonList)
                    ((RadioButtonList)control).ClearSelection();
                else if (control is CheckBoxList)
                    ((CheckBoxList)control).ClearSelection();
                else if (control is RadioButton)
                    ((RadioButton)control).Checked = false;
                else if (control is CheckBox)
                    ((CheckBox)control).Checked = false;
                else if (control.HasControls())
                    //Esta linea detécta un Control que contenga otros Controles
                    //Así ningún control se quedará sin ser limpiado.
                    Limpia(control.Controls);
            }

        }

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }

        private void Cargar_Excel(string directorio, string nomFile)
        {

           // string directorioURL = HttpContext.Current.Server.MapPath(directorio + nomFile);

            SLDocument sl = new SLDocument(directorio);
            SLWorksheetStatistics propiedades = sl.GetWorksheetStatistics();
            int numeroColumnas = propiedades.NumberOfColumns;
            int numeroCeldas = propiedades.NumberOfCells;

            if (numeroColumnas <= 13)
            {
                try
                {
                    int ultFila = numeroCeldas / numeroColumnas;
                //  int ultimaFila = propiedades.EndRowIndex;

                    ConexionBD conectar = new ConexionBD();
                    conectar.Abrir();

                    //string error = "";

                    for (int x = 2; x <= ultFila; x++)
                    {
                        string sReferencia = sl.GetCellValueAsString("A" + x);

                        if (sReferencia != string.Empty)
                        {

                            if (existeProducto(sReferencia))
                            {
                                LblMessage.Text += "ya existe referencia" + sReferencia + "\n";
                                mpeMensaje.Show();
                            }
                            else
                            {
                                int iProceso = Convert.ToInt32(sl.GetCellValueAsString("L" + x));
                                int iSubProceso = Convert.ToInt32(sl.GetCellValueAsString("M" + x));

                                string sqlString = "INSERT INTO ITM_02 (UsReferencia, UsEmail, Aseguradora, UsAsegurado, Tipo_Asegurado, Nombre_Contacto, " +
                                                                        "Apellidos_Contacto, UsTelefono, RFC_Contacto, Perfil_Contacto, Siniestro, Id_Proceso, Id_SubProceso ) " +
                                                " VALUES (@referencia, @email, @aseguradora, @asegurado, @tpoasegurado, @nomcontacto, @apecontacto, " +
                                                " @telefono, @rfc, @perfil, @siniestro, @proceso, @subproceso )";
                                try
                                {

                                    SqlCommand cmd = new SqlCommand(sqlString, conectar.ConectarBD);

                                    cmd.Parameters.AddWithValue("@referencia", sl.GetCellValueAsString("A" + x));
                                    cmd.Parameters.AddWithValue("@email", sl.GetCellValueAsString("B" + x));
                                    cmd.Parameters.AddWithValue("@aseguradora", sl.GetCellValueAsString("C" + x));
                                    cmd.Parameters.AddWithValue("@asegurado", sl.GetCellValueAsString("D" + x));
                                    cmd.Parameters.AddWithValue("@tpoasegurado", sl.GetCellValueAsString("E" + x));
                                    cmd.Parameters.AddWithValue("@nomcontacto", sl.GetCellValueAsString("F" + x));
                                    cmd.Parameters.AddWithValue("@apecontacto", sl.GetCellValueAsString("G" + x));
                                    cmd.Parameters.AddWithValue("@telefono", sl.GetCellValueAsString("H" + x));
                                    cmd.Parameters.AddWithValue("@rfc", sl.GetCellValueAsString("I" + x));
                                    cmd.Parameters.AddWithValue("@perfil", sl.GetCellValueAsString("J" + x));
                                    cmd.Parameters.AddWithValue("@siniestro", sl.GetCellValueAsString("K" + x));
                                    cmd.Parameters.AddWithValue("@proceso", sl.GetCellValueAsString("L" + x));
                                    cmd.Parameters.AddWithValue("@subproceso", sl.GetCellValueAsString("M" + x));


                                    int result =  cmd.ExecuteNonQuery();

                                    // Insertar Registros Tabla tbEstadoDocumento [ITM_04]
                                    int idStatus = 1;
                                    int valor = Add_tbEstadoDocumento(sReferencia, iProceso, iSubProceso, idStatus);
                                }
                                catch (Exception ex)
                                {
                                    LblMessage.Text = ex.Message;
                                    mpeMensaje.Show();
                                }

                            }

                        }
                    }

                    LblMessage.Text = "Documento Excel Cargado";
                    mpeMensaje.Show();

                } catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    mpeMensaje.Show();
                }
            }
            else
            {
                LblMessage.Text = "Formato del documento es incorrecto ";
                mpeMensaje.Show();
            }
        }

        private bool existeProducto(string pReferencia)
        {
            try
            {

                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                string queryString = "SELECT UsReferencia FROM ITM_02 WHERE UsReferencia LIKE '%' + '" + pReferencia + "'  + '%'";
                SqlCommand cmd2 = new SqlCommand(queryString, conectar.ConectarBD);

                int num = Convert.ToInt32(cmd2.ExecuteScalar());

                if (num > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

            return false;
        }

    }
}