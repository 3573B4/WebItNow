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
            //importReadExcelFile();
            Cargar_Excel();


            // Eliminar archivo .xlsx del repositorio (solucion).
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

        public int Add_tbEstadoDocumento(String pUsuarios, int pIdStatus)
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
                    SqlCommand cmd1 = new SqlCommand("Insert into ITM_04 (IdUsuario, IdTipoDocumento, IdStatus, IdDescarga) " +
                                        "Values ('" + pUsuarios + "', '" + IdTpoDocumento + "', " + iIdStatus + ", " + iIdDescarga + ")", Conecta.ConectarBD);

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

        protected void importReadExcelFile()
        {

            if (IsPostBack && Upload.HasFile)
            {
                if (Path.GetExtension(Upload.FileName).Equals(".xlsx"))
                {

                    // Subir el archivo al repositorio (solucion) - wwwroot

                    //var excel = new ExcelPackage(Upload.FileContent);
                    //var dt = excel.ToDataTable();

                    var dt = ExcelDataToDataTable("Carga_Referencia.xlsx", "Hoja1", true);
                    var table = "ITM_02";

                    using (var conn = new SqlConnection("Server=tcp:codice1.database.windows.net,1433;Initial Catalog=Itnow;Persist Security Info=False; User ID=DB_Codice; Password=Itnow2023; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                    {
                        var bulkCopy = new SqlBulkCopy(conn);
                        bulkCopy.DestinationTableName = table;
                        conn.Open();
                        var schema = conn.GetSchema("Columns", new[] { null, null, table, null });
                        try
                        {

                            foreach (DataColumn sourceColumn in dt.Columns)
                            {
                                foreach (DataRow row in schema.Rows)
                                {
                                    if (string.Equals(sourceColumn.ColumnName, (string)row["COLUMN_NAME"], StringComparison.OrdinalIgnoreCase))
                                    {
                                        bulkCopy.ColumnMappings.Add(sourceColumn.ColumnName, (string)row["COLUMN_NAME"]);
                                        break;
                                    }
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            LblMessage.Text = ex.Message;
                            this.mpeMensaje.Show();
                        }

                            bulkCopy.WriteToServer(dt);
                    }
                }
            }

        }

        public static DataTable ExcelDataToDataTable(string filePath, string sheetName, bool hasHeader = true)
        {
            string directorioURL = HttpContext.Current.Server.MapPath("~/itnowstorage/" + filePath);
            var dt = new DataTable();
            var fi = new FileInfo(directorioURL);

            // Check if the file exists
            if (!fi.Exists)
                throw new Exception("File " + filePath + " Does Not Exists");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var xlPackage = new ExcelPackage(fi);
            // get the first worksheet in the workbook
            var worksheet = xlPackage.Workbook.Worksheets[sheetName];

            dt = worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column].ToDataTable(c =>
            {
                c.FirstRowIsColumnNames = true;
            });

            return dt;
        }

        private void Cargar_Excel()
        {

            string directorioURL = HttpContext.Current.Server.MapPath("~/itnowstorage/" + "Carga_Referencia.xlsx");

            SLDocument sl = new SLDocument(directorioURL);
            SLWorksheetStatistics propiedades = sl.GetWorksheetStatistics();
            int ultimaFila = propiedades.EndRowIndex;

            ConexionBD conectar = new ConexionBD();
            conectar.Abrir();

            //string error = "";

            for (int x = 2; x <= ultimaFila; x++)
            {
                string referencia = sl.GetCellValueAsString("A" + x);

                if (existeProducto(referencia))
                {
                    LblMessage.Text += "ya existe referencia" + referencia + "\n";
                    mpeMensaje.Show();
                }
                else
                {

                    string sqlString = "INSERT INTO ITM_02 (UsReferencia, UsEmail, Aseguradora, UsAsegurado, Tipo_Asegurado, Nombre_Contacto, " +
                                                            "Apellidos_Contacto, UsTelefono, RFC_Contacto, Perfil_Contacto ) " +
                                    " VALUES (@referencia, @email, @aseguradora, @asegurado, @tpoasegurado, @nomcontacto, @apecontacto, " +
                                    " @telefono, @rfc, @perfil )";
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


                        int result =  cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        LblMessage.Text = ex.Message;
                        mpeMensaje.Show();
                    }

                }
            }
            LblMessage.Text = "Plantilla Cargada";
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