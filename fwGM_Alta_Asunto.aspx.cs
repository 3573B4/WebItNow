using System;
using System.IO;

using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

using System.Linq;
using OfficeOpenXml;
using MySql.Data.MySqlClient;

namespace WebItNow_Peacock.GastosMedicos
{
    public partial class fwGM_Alta_Asunto : System.Web.UI.Page
    {

        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Session["DownloadsPath"] = GetDownloadFolderPath();

            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    Variables.wUserName = Convert.ToString(Session["IdUsuario"]);
                    Variables.wPassword = Convert.ToString(Session["UsPassword"]);

                    if (Variables.wUserName == "" || Variables.wPassword == "")
                    {
                        Response.Redirect("Login.aspx", true);
                        return;
                    }

                    BtnEnviar.Enabled = true;

                    Variables.wContinuar = true;
                    Variables.wPoliza = false;
                    Variables.wAsegurado = false;

                    GetCiaSeguros();
                    GetProyecto();
                    GetTipoEvento();
                    GetRespMedico();
                    GetResponsableAdmin();

                    TxtHoraAsignacion.Text = "00:00";

                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

            }
        }

        protected void GetCiaSeguros()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdSeguros, Descripcion " +
                                  "  FROM ITM_67 " +
                                  " WHERE IdStatus = 1 " +
                                  "   AND IdSeguros <> 'OTR' " +
                                  " ORDER BY IdOrden";

                DataTable dt = dbConn.ExecuteQuery(strQuery);


                ddlCiaSeguros.DataSource = dt;

                ddlCiaSeguros.DataValueField = "IdSeguros";
                ddlCiaSeguros.DataTextField = "Descripcion";

                ddlCiaSeguros.DataBind();
                ddlCiaSeguros.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetProyecto()
        {
            try
            {
                string sCliente = ddlCiaSeguros.SelectedValue;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdProyecto, Descripcion " +
                                  "  FROM ITM_26 " +
                                  " WHERE IdCliente = '" + sCliente + "'" +
                                  "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlProyecto.DataSource = dt;

                ddlProyecto.DataValueField = "IdProyecto";
                ddlProyecto.DataTextField = "Descripcion";

                ddlProyecto.DataBind();
                ddlProyecto.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetTipoEvento()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdTpoEvento, Descripcion " +
                                  "  FROM ITM_11 " +
                                  " WHERE IdTpoEvento IN (4,5) " +
                                  "   AND  IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlTipoEvento.DataSource = dt;

                ddlTipoEvento.DataValueField = "IdTpoEvento";
                ddlTipoEvento.DataTextField = "Descripcion";

                ddlTipoEvento.DataBind();
                ddlTipoEvento.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        protected void GetRespMedico()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdRespMedico, Descripcion " +
                                        " FROM ITM_12 " +
                                        " WHERE IdStatus = 1";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlRespMedico.DataSource = dt;

                ddlRespMedico.DataValueField = "IdRespMedico";
                ddlRespMedico.DataTextField = "Descripcion";

                ddlRespMedico.DataBind();
                ddlRespMedico.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetResponsableAdmin()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int IdPrivilegio = Convert.ToInt32(Session["UsPrivilegios"]);
                string IdUsuario = Convert.ToString(Session["IdUsuario"]);

                string strQuery = "SELECT t1.IdRespAdministrativo, t1.Descripcion " +
                                  "  FROM ITM_14 t1 " +
                                  "  LEFT JOIN ITM_02 t0 ON t1.Descripcion LIKE CONCAT('%', REPLACE(t0.IdUsuario, '.', ' '), '%') " +
                                  " WHERE t1.IdStatus = 1 " +
                                  "   AND ( (@IdPrivilegio = 3 AND t0.IdUsuario = @IdUsuario) " +               // Filtra por usuario específico si el privilegio es 3
                                  "    OR  @IdPrivilegio = 1 OR @IdPrivilegio = 2 ); ";                         // Trae todos los registros si el privilegio es 2


                // Usar la nueva función para ejecutar la consulta
                DataTable dt = dbConn.ExecuteQueryWithParameters(strQuery, cmd =>
                {
                    cmd.Parameters.AddWithValue("@IdPrivilegio", IdPrivilegio);
                    cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                });

                ddlRespAdministrativo.DataSource = dt;

                ddlRespAdministrativo.DataValueField = "IdRespAdministrativo";
                ddlRespAdministrativo.DataTextField = "Descripcion";

                ddlRespAdministrativo.DataBind();
                ddlRespAdministrativo.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetDatos_Proyecto()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sCliente = ddlCiaSeguros.SelectedValue;
                int iProyecto = int.Parse(ddlProyecto.SelectedValue);

                string strQuery = "SELECT Referencia, IdTpoAsunto, NumPoliza, NomAsegurado " +
                                  " FROM ITM_78 " +
                                  " WHERE IdCliente = '" + sCliente + "' " +
                                  "   AND IdProyecto = " + iProyecto + " " +
                                  "   AND IdStatus = 1";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                foreach (DataRow row in dt.Rows)
                {
                    if (ddlProyecto.SelectedValue != "0")
                    {
                        Variables.wRef_Proyecto = Convert.ToString(row[0]);

                        ddlTipoEvento.SelectedValue = Convert.ToString(row[1]);
                        ddlTipoEvento_SelectedIndexChanged(ddlTipoEvento, EventArgs.Empty);

                    }
                }

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            Insertar_ITM_73();
        }

        protected void Insertar_ITM_73()
        {

            if (Page.IsValid)
            {

                rfvFechaInput.Display = ValidatorDisplay.None;
                cvCiaSeguros.Display = ValidatorDisplay.None;
                cvProyecto.Display = ValidatorDisplay.None;
                rfvCliente.Display = ValidatorDisplay.None;
                cvTipoEvento.Display = ValidatorDisplay.None;

                cvRespMedico.Display = ValidatorDisplay.Dynamic;
                cvAdminResponsable.Display = ValidatorDisplay.Dynamic;

                 if (Variables.wContinuar == false)
                {
                    return;
                }

                try
                {
                    ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                    dbConn.Open();

                    //  string sUsuario = string.Empty;
                    string sUsuario = Variables.wUserLogon;  // LblUsuario.Text;

                    string sReferencia = Variables.wRef_Proyecto;

                    int sSubRefencia = Obtener_Sub_Referencia(Variables.wRef_Proyecto);

                    DateTime fecha = DateTime.ParseExact(TxtFechaInput.Text, "yyyy-MM-dd", null);
                    string fechaFormateada = fecha.ToString("dd/MM/yyyy");
                    string sHoraAsignacion = TxtHoraAsignacion.Text;

                    string sIdSeguros = ddlCiaSeguros.SelectedValue;

                    string sNumSiniestro = string.Empty;
                    string sNumPoliza = string.Empty;
                    string sNumReporte = string.Empty;
                    string sNomCliente = TxtNomCliente.Text;
                    string sNomActor = string.Empty;
                    string sNomDemandado = string.Empty;
                    // string sNomAjustador = string.Empty;
                    string sEstOcurrencia = string.Empty;
                    string sDescMote = string.Empty;

                    int iIdProyecto;
                    int iConsecutivo = 0;

                    //if (ddlProyecto.SelectedValue == "0")
                    //{
                    //    iIdProyecto = 0;
                    //    sSubRefencia = 0;

                    //    iConsecutivo = Obtener_Consecutivo("fwAlta_Asunto_GM");

                    //    if (ddlTipoEvento.SelectedValue == "1")
                    //    {
                    //        sReferencia = "ES-" + iConsecutivo + "-" + sIdSeguros + fecha.ToString("yy");
                    //    }
                    //    else if (ddlTipoEvento.SelectedValue == "2")
                    //    {
                    //        sReferencia = "EM-" + iConsecutivo + "-" + sIdSeguros + fecha.ToString("yy");
                    //    }
                    //    else if (ddlTipoEvento.SelectedValue == "3")
                    //    {
                    //        sReferencia = "EC-" + iConsecutivo + "-" + sIdSeguros + fecha.ToString("yy");
                    //    }

                    //}
                    //else
                    //{
                    //    iIdProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);
                    //}

                    iIdProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);
                    sSubRefencia = 0;

                    iConsecutivo = Obtener_Consecutivo("fwAlta_Asunto_GM");

                    if (ddlTipoEvento.SelectedValue == "4")
                    {
                        sReferencia = "AM-" + iConsecutivo.ToString("D4") + "-" + sIdSeguros + fecha.ToString("yy");
                    }
                    else if (ddlTipoEvento.SelectedValue == "5")
                    {
                        sReferencia = "TR-" + iConsecutivo.ToString("D4") + "-" + sIdSeguros + fecha.ToString("yy");
                    }

                    int iIdTpoAsunto = Convert.ToInt32(ddlTipoEvento.SelectedValue);
                    int iIdAjustador = 0;
                    int iIdRespMedico = Convert.ToInt32(ddlRespMedico.SelectedValue);
                    int iIdRespAdministrativo = Convert.ToInt32(ddlRespAdministrativo.SelectedValue);

                    // Insertar registro tabla (ITM_73)
                    string strQuery = "INSERT INTO ITM_73 (Referencia, SubReferencia, NumSiniestro,  NumPoliza, NumReporte, IdSeguros, IdTpoEvento, IdProyecto, IdRegimen, NomCliente, NomActor, NomDemandado, IdAjustador, IdRespMedico, IdRespAdministrativo, " +
                        "Fecha_Asignacion, Hora_Asignacion, IdConclusion, IdTpoProyecto, Id_Usuario, IdStatus ) " +
                        "VALUES('" + sReferencia + "', '" + sSubRefencia + "', '" + sNumSiniestro + "', '" + sNumPoliza + "', '" + sNumReporte + "', '" + sIdSeguros + "', " + iIdTpoAsunto + ", " + iIdProyecto + ", 1, '" + sNomCliente + "'," +
                        "'" + sNomActor + "', '" + sNomDemandado + "', " + iIdAjustador + ", " + iIdRespMedico + ", " + iIdRespAdministrativo + "," +
                        "'" + fechaFormateada + "', '" + sHoraAsignacion + "', 1, 0, '" + sUsuario + "', 1); ";

                    strQuery += Environment.NewLine;

                    //if (ddlProyecto.SelectedValue == "0")
                    //{
                    //    iConsecutivo++;
                    //    strQuery += "UPDATE ITM_71 SET IdConsecutivo = " + iConsecutivo + " WHERE IdProceso = 'fwAlta_Asunto_GM'";
                    //}

                    iConsecutivo++;
                    strQuery += "UPDATE ITM_71 SET IdConsecutivo = " + iConsecutivo + " WHERE IdProceso = 'fwAlta_Asunto_GM'";

                    int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                    dbConn.Close();

                    //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                    //int affectedRows = cmd.ExecuteNonQuery();

                    DateTime fechaConvertida = DateTime.ParseExact(TxtFechaInput.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    string sFecAsignacion = fechaConvertida.ToString("dd/MM/yyyy");

                    Response.Write(sFecAsignacion);

                    string sCiaSeguros = ddlCiaSeguros.SelectedItem.Text;
                    string sRespAdministrativo = ddlRespAdministrativo.SelectedItem.Text;
                    string sRespTecnico = ddlRespMedico.SelectedItem.Text;

                    // Insertar renglon datos excel onedrive
                    // InsertarDatosEnTablaExcel(sReferencia, sFecAsignacion, sNumSiniestro, sCiaSeguros, sRespAdministrativo, sRespTecnico);

                    LblMessage.Text = "Se agrego nuevo asunto, correctamente " + "<br />" + "Num. Referencia : " + sReferencia + "-" + sSubRefencia;
                    mpeMensaje.Show();

                    // Inicializar controles
                    Limpia(this.Controls);

                    TxtHoraAsignacion.Text = "00:00";

                    Variables.wPoliza = false;
                    Variables.wAsegurado = false;

                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    mpeMensaje.Show();
                }

            }
        }

        public int Obtener_Sub_Referencia(string sReferencia)
        {
            try
            {
                int SubReferencia = 0;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT MAX(SubReferencia) + 1 as SubReferencia FROM ITM_70" +
                                  " WHERE Referencia = '" + sReferencia + "'";

                MySqlDataReader reader = dbConn.ExecuteReaderQuery(strQuery);

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //SqlDataReader dr = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SubReferencia = Convert.ToInt32(reader["SubReferencia"].ToString().Trim());
                    }
                }

                dbConn.Close();

                return SubReferencia;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

            return -1;
        }

        public int Obtener_Consecutivo(string IdProceso)
        {
            try
            {
                int IdArchivoMax = 0;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdConsecutivo FROM ITM_71" +
                                  " WHERE IdProceso = '" + IdProceso + "'";

                MySqlDataReader reader = dbConn.ExecuteReaderQuery(strQuery);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        IdArchivoMax = Convert.ToInt32(reader["IdConsecutivo"].ToString().Trim());
                    }
                }

                dbConn.Close();

                return IdArchivoMax;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

            return -1;
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        public void Limpia(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
                // else if (control is DropDownList)
                //     ((DropDownList)control).Items.Clear();
                else if (control is DropDownList)
                    ((DropDownList)control).SelectedIndex = 0;
                else if (control is RadioButtonList)
                    ((RadioButtonList)control).ClearSelection();
                else if (control is CheckBoxList)
                    ((CheckBoxList)control).ClearSelection();
                else if (control is RadioButton)
                    ((RadioButton)control).Checked = false;
                else if (control is CheckBox)
                    ((CheckBox)control).Checked = false;
                else if (control.HasControls())
                    // Esta linea detécta un Control que contenga otros Controles
                    // Así ningún control se quedará sin ser limpiado.
                    Limpia(control.Controls);
            }

        }

        protected void cvCiaSeguros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlCiaSeguros.SelectedValue != "0";
        }

        protected void cvAdminResponsable_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlRespAdministrativo.SelectedValue != "0";
        }

        protected void cvTecResponsable_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlRespMedico.SelectedValue != "0";
        }

        protected void cvProyecto_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlProyecto.SelectedValue != "-1";
        }

        protected void cvTipoAsunto_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlTipoEvento.SelectedValue != "0";
        }

        protected void ddlCiaSeguros_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetProyecto();

            ddlProyecto.ClearSelection();
            ddlTipoEvento.ClearSelection();

            bool condicionCumplida = (ddlCiaSeguros.SelectedValue == "OTR");

            divCliente.Visible = (ddlCiaSeguros.SelectedValue == "OTR");
            rfvCliente.Enabled = condicionCumplida;


        }

        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetDatos_Proyecto();

            if (ddlProyecto.SelectedValue == "0")
            {
                ddlTipoEvento.SelectedIndex = 0;
            }
        }

        protected void BtnContinuar_Click(object sender, EventArgs e)
        {
            Variables.wContinuar = true;
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            if (Variables.wPoliza && Variables.wAsegurado == false)
            {
                Variables.wPoliza = false;
            }
            else if (Variables.wAsegurado && Variables.wPoliza)
            {
                Variables.wAsegurado = false;
            }
        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        public void InsertarDatosEnExcel()
        {
            string rutaArchivo = (string)Session["Url_OneDrive"] + "CONTROL OPERATIVO" + "\\" + "ARCHIVOS DAÑADOS" + "\\" + "ALTA DE REFERENCIAS APP.xlsx";
            string nombreHoja = "Respuestas de formulario";

            FileInfo archivo = new FileInfo(rutaArchivo);

            // Asegúrate de que EPPlus pueda trabajar con archivos Excel
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(archivo))
            {
                // Obtenemos la hoja de trabajo por su nombre
                ExcelWorksheet hojaTrabajo = package.Workbook.Worksheets[nombreHoja];

                if (hojaTrabajo == null)
                {
                    throw new Exception($"La hoja '{nombreHoja}' no existe en el archivo.");
                }

                // Determinamos la siguiente fila disponible
                int ultimaFila = hojaTrabajo.Dimension.End.Row + 1; // La fila siguiente al último dato insertado

                // Insertamos los datos en la nueva fila
                hojaTrabajo.Cells[ultimaFila, 1].Value = "REF123";          // Referencia
                hojaTrabajo.Cells[ultimaFila, 2].Value = DateTime.Now.ToString("dd/MM/yyyy");  // Fecha de Asignación
                hojaTrabajo.Cells[ultimaFila, 3].Value = "SIN12345";        // Número de Siniestro
                hojaTrabajo.Cells[ultimaFila, 4].Value = "Compañía XYZ";    // Compañía de Seguros
                hojaTrabajo.Cells[ultimaFila, 5].Value = "Juan Pérez";      // Responsable Administrativo
                hojaTrabajo.Cells[ultimaFila, 6].Value = "María López";     // Responsable Técnico
                hojaTrabajo.Cells[ultimaFila, 7].Value = "Tipo de caso";    // Indique si se trata de

                // Guardamos los cambios en el archivo Excel
                package.Save();
            }
        }

        public void InsertarDatosEnTablaExcel(string numReferencia, string fecAsignacion, string numSiniestro, string ciaSeguros, string respAdmin, string respTec)
        {
            string rutaArchivo = (string)Session["Url_OneDrive"] + "CONTROL OPERATIVO" + "\\" + "ARCHIVOS DAÑADOS" + "\\" + "ALTA DE REFERENCIAS APP.xlsx";

            string nombreHoja = "Respuestas de formulario";
            string nombreTabla = "CopiaRespuestas";

            // Verifica si el archivo existe
            if (!File.Exists(rutaArchivo))
            {
                throw new FileNotFoundException($"El archivo en la ruta '{rutaArchivo}' no existe.");
            }

            FileInfo archivo = new FileInfo(rutaArchivo);

            // Configura EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(archivo))
            {
                // Obtiene la hoja por nombre
                ExcelWorksheet hojaTrabajo = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == nombreHoja);

                if (hojaTrabajo == null)
                {
                    throw new Exception($"La hoja '{nombreHoja}' no existe en el archivo.");
                }

                // Guarda el estado original de visibilidad de la hoja
                var estadoOriginalHoja = hojaTrabajo.Hidden;

                // Si la hoja está oculta, hazla visible temporalmente
                if (estadoOriginalHoja != eWorkSheetHidden.Visible)
                {
                    hojaTrabajo.Hidden = eWorkSheetHidden.Visible;
                }

                // Obtiene la tabla por nombre
                var tabla = hojaTrabajo.Tables[nombreTabla];

                if (tabla == null)
                {
                    throw new Exception($"La tabla '{nombreTabla}' no existe en la hoja '{nombreHoja}'.");
                }

                // Agrega una nueva fila a la tabla
                var nuevaFila = tabla.Address.End.Row + 1;
                tabla.AddRow();

                // Agrega los datos a la nueva fila dentro de la tabla
                hojaTrabajo.Cells[nuevaFila, tabla.Address.Start.Column].Value = numReferencia;             // Referencia
                hojaTrabajo.Cells[nuevaFila, tabla.Address.Start.Column + 1].Value = fecAsignacion;         // Fecha de Asignación
                hojaTrabajo.Cells[nuevaFila, tabla.Address.Start.Column + 2].Value = numSiniestro;          // Número de Siniestro
                hojaTrabajo.Cells[nuevaFila, tabla.Address.Start.Column + 3].Value = ciaSeguros;            // Compañía de Seguros
                hojaTrabajo.Cells[nuevaFila, tabla.Address.Start.Column + 4].Value = respAdmin;             // Responsable Administrativo
                hojaTrabajo.Cells[nuevaFila, tabla.Address.Start.Column + 5].Value = respTec;               // Responsable Técnico
                hojaTrabajo.Cells[nuevaFila, tabla.Address.Start.Column + 6].Value = "NUEVO SINIESTRO";     // Indique si se trata de

                // Restaurar el estado original de visibilidad de la hoja
                hojaTrabajo.Hidden = estadoOriginalHoja;

                // Guarda los cambios en el archivo Excel existente
                package.Save();
            }
        }

        protected void ddlTipoEvento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void cvTipoEvento_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }

        protected void cvRespMedico_ServerValidate(object source, ServerValidateEventArgs args)
        {

        }
    }
}