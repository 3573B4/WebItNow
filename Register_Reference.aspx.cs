using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;

using OfficeOpenXml;
using System.IO;
using SpreadsheetLight;

namespace WebItNow
{
    public partial class Register_Reference : System.Web.UI.Page
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
            string nomFile = Upload.FileName;
         // string fileName = System.IO.Path.GetFileName(Upload.FileName);

            int tamArchivo = Upload.PostedFile.ContentLength;
            string extensionFile = Path.GetExtension(Upload.FileName);

            if (Upload.HasFile)
            {
                if ((extensionFile == ".xlsx") || (extensionFile == ".xls"))
                {
                    if (tamArchivo <= 70000000)
                    {
                        string filepath = Server.MapPath(directorio + Upload.FileName);
                        Upload.SaveAs(filepath);
                        string sPath = Path.GetDirectoryName(filepath) + "/" + nomFile;

                        System.Threading.Thread.Sleep(3000);
                        this.Cargar_Excel(directorio, nomFile);

                        // Eliminar archivo .xlsx del repositorio (solucion).
                        File.Delete(sPath);
                    }
                }
                else
                {
                    LblMessage.Text = "El archivo tiene  que ser un formato .xlsx";
                    mpeMensaje.Show();
                }

            }
            else
            {
                LblMessage.Text = "Debe seleccionar un archivo";
                mpeMensaje.Show();
            }

        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            //* Validar si el usuario existe o es nuevo
            if (TxtRef.Text != "" && TxtEmail.Text != "")
            {

                // Insertar Registo Tabla tbUsuarios (UploadFiles)
                int result = Add_tbUsuarios(TxtRef.Text, TxtEmail.Text, string.Empty, 3, "Insert");

                if (result == 0)
                {
                    // Insertar Registros Tabla tbEstadoDocumento [ITM_04]
                    int idStatus = 1;
                    int valor = Add_tbEstadoDocumento(TxtRef.Text, idStatus);

                    var email = new EnvioEmail();
                    int Envio_Ok = email.EnvioMensaje(TxtRef.Text.Trim(), TxtEmail.Text.Trim(), "Registro Referencia ", string.Empty);

                    //EnvioMensaje(TxtUsu.Text.Trim(), TxtEmail.Text.Trim());

                    if (Envio_Ok == 0)
                    {
                        LblMessage.Text = "Usuario fue insertado correctamente ";
                        this.mpeMensaje.Show();
                    }

                    Limpia(this.Controls);

                    // Response.Redirect("Login.aspx");
                    Lbl_Message.Visible = false;
                }
            }
            else
            {
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "* Estos campos son obligatorios";
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

        public int Add_tbEstadoDocumento(String pReferencia, int pIdStatus)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Consulta a la tabla Tipo de Documento
                string strQuery = "Select IdTpoDocumento, Descripcion From ITM_06 Where Status = " + pIdStatus + "";

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

        protected void TxtRef_TextChanged(object sender, EventArgs e)
        {
            // Validar si existe Usuario en la tabla ITM_02 (tbUsuarios)
            Variables.wPrivilegios = "3";
            int Usuario_Existe = ValidaUser(TxtRef.Text, Int32.Parse(Variables.wPrivilegios));

            if (Usuario_Existe == 1)
            {
                TxtRef.Text = string.Empty;
                TxtEmail.Focus();

                LblMessage.Text = "El nombre de usuario ya existe";
                this.mpeMensaje.Show();
            }
            else
            {
                TxtEmail.Focus();
            }
        }

        protected void TxtEmail_TextChanged(object sender, EventArgs e)
        {
            // Validar si existe Email en la tabla ITM_02 (tbUsuarios)
            Variables.wPrivilegios = "3";
            int Email_Existe = ValidaEmail(TxtEmail.Text, Int32.Parse(Variables.wPrivilegios));

                if (Email_Existe == 1)
            {
                TxtEmail.Text = string.Empty;
                TxtEmail.Focus();

                LblMessage.Text = "El Correo electrónico ya existe";
                this.mpeMensaje.Show();
            }
            else
            {
                TxtEmail.Focus();
            }
        }

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

            string directorioURL = HttpContext.Current.Server.MapPath(directorio + nomFile);

            SLDocument sl = new SLDocument(directorioURL);
            SLWorksheetStatistics propiedades = sl.GetWorksheetStatistics();
            int ultimaFila = propiedades.EndRowIndex;

            ConexionBD conectar = new ConexionBD();
            conectar.Abrir();

            //string error = "";

            for (int x = 2; x <= ultimaFila; x++)
            {
                string sReferencia = sl.GetCellValueAsString("A" + x);

                if (existeProducto(sReferencia))
                {
                    LblMessage.Text += "ya existe referencia" + sReferencia + "\n";
                    mpeMensaje.Show();
                }
                else
                {

                    string sqlString = "INSERT INTO ITM_02 (UsReferencia, UsEmail, Aseguradora, UsAsegurado, Tipo_Asegurado, Nombre_Contacto, " +
                                                            "Apellidos_Contacto, UsTelefono, RFC_Contacto, Perfil_Contacto, Siniestro ) " +
                                    " VALUES (@referencia, @email, @aseguradora, @asegurado, @tpoasegurado, @nomcontacto, @apecontacto, " +
                                    " @telefono, @rfc, @perfil, @siniestro )";
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


                        int result =  cmd.ExecuteNonQuery();

                        // Insertar Registros Tabla tbEstadoDocumento [ITM_04]
                        int idStatus = 1;
                        int valor = Add_tbEstadoDocumento(sReferencia, idStatus);
                    }
                    catch (Exception ex)
                    {
                        LblMessage.Text = ex.Message;
                        mpeMensaje.Show();
                    }

                }
            }
            LblMessage.Text = "Documento Excel Cargado";
            mpeMensaje.Show();
        }

        private bool existeProducto(string sReferencia)
        {
            ConexionBD conectar = new ConexionBD();
            conectar.Abrir();

            string queryString = "SELECT UsReferencia FROM ITM_02 WHERE UsReferencia='" + sReferencia + "'";
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

    }
}