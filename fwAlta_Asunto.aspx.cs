using System;
using System.IO;

using System.Data;
using System.Data.SqlClient;

using System.Web.UI;
using System.Web.UI.WebControls;

using System.Linq;
using OfficeOpenXml;
using MySql.Data.MySqlClient;

namespace WebItNow_Peacock
{
    public partial class fwAlta_Asunto : System.Web.UI.Page
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

                    // Labels
                    lblTitulo_Alta_Asunto.Text = GetGlobalResourceObject("GlobalResources", "lblTitulo_Alta_Asunto").ToString();

                    BtnEnviar.Enabled = true;

                    Variables.wContinuar = true;
                    Variables.wPoliza = false;
                    Variables.wAsegurado = false;

                    GetCiaSeguros();
                    GetProyecto();
                    GetTipoAsunto();
                    GetResponsableTec();
                    GetResponsableAdmin();

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
                //ConexionBD Conecta = new ConexionBD();
                //Conecta.Abrir();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdSeguros, Descripcion " +
                                  "  FROM ITM_67 " +
                                  " WHERE IdStatus = 1 " +
                                  "   AND IdSeguros <> 'OTR' " +
                                  " ORDER BY IdOrden";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //da.Fill(dt);

                ddlCiaSeguros.DataSource = dt;

                ddlCiaSeguros.DataValueField = "IdSeguros";
                ddlCiaSeguros.DataTextField = "Descripcion";

                ddlCiaSeguros.DataBind();
                ddlCiaSeguros.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();

                //Conecta.Cerrar();
                //cmd.Dispose();
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
                                  "  FROM ITM_78 " +
                                  " WHERE IdCliente = '" + sCliente + "'" +
                                  "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlProyecto.DataSource = dt;

                ddlProyecto.DataValueField = "IdProyecto";
                ddlProyecto.DataTextField = "Descripcion";

                ddlProyecto.DataBind();
                ddlProyecto.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));
                // ddlProyecto.Items.Insert(1, new ListItem("NINGUNO", "-1"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetTipoAsunto()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdTpoAsunto, Descripcion " +
                                        " FROM ITM_66 " +
                                        " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlTipoAsunto.DataSource = dt;

                ddlTipoAsunto.DataValueField = "IdTpoAsunto";
                ddlTipoAsunto.DataTextField = "Descripcion";

                ddlTipoAsunto.DataBind();
                ddlTipoAsunto.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        protected void GetResponsableTec()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdRespTecnico, Descripcion " +
                                        " FROM ITM_68 " +
                                        " WHERE IdStatus = 1";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlRespTecnico.DataSource = dt;

                ddlRespTecnico.DataValueField = "IdRespTecnico";
                ddlRespTecnico.DataTextField = "Descripcion";

                ddlRespTecnico.DataBind();
                ddlRespTecnico.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

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

                //string strQuery = "SELECT IdRespAdministrativo, Descripcion " +
                //                        " FROM ITM_69 " +
                //                        " WHERE IdStatus = 1";

                // DataTable dt = dbConn.ExecuteQuery(strQuery);

                int IdPrivilegio = Convert.ToInt32(Session["UsPrivilegios"]);
                string IdUsuario = Convert.ToString(Session["IdUsuario"]);

                string strQuery = "SELECT t1.IdRespAdministrativo, t1.Descripcion " +
                                  "  FROM ITM_69 t1 " +
                                  "  LEFT JOIN ITM_02 t0 ON t1.Descripcion LIKE CONCAT('%', REPLACE(t0.IdUsuario, '.', ' '), '%') " +
                                  " WHERE t1.IdStatus = 1 " +
                                  "   AND ( (@IdPrivilegio = 3 AND t0.IdUsuario = @IdUsuario) " +       // Filtra por usuario específico si el privilegio es 3
                                  "    OR  @IdPrivilegio = 2 ); ";                                      // Trae todos los registros si el privilegio es 2

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

                        ddlTipoAsunto.SelectedValue = Convert.ToString(row[1]);
                        ddlTipoAsunto_SelectedIndexChanged(ddlTipoAsunto, EventArgs.Empty);

                        TxtNumPoliza.Text = Convert.ToString(row[2]);
                        TxtNomAsegurado.Text = Convert.ToString(row[3]);
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

        public int Validar_NumPoliza(string pNumPoliza)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT NumPoliza, NomAsegurado FROM ITM_70" +
                                  " WHERE NumPoliza = '" + pNumPoliza + "'";

                MySqlDataReader reader = dbConn.ExecuteReaderQuery(strQuery);

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //SqlDataReader dr = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    return 1;   // Duplicado encontrado
                }

                dbConn.Close();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

            return 0;
        }

        public int Validar_Asegurado(string pAsegurado)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT NumPoliza, NomAsegurado FROM ITM_70" +
                                  " WHERE NomAsegurado = '" + pAsegurado + "'";

                MySqlDataReader reader = dbConn.ExecuteReaderQuery(strQuery);

                if (reader.HasRows)
                {
                    return 1;   // Duplicado encontrado
                }

                dbConn.Close();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

            return 0;
        }

        public int Validar_Duplicado(string pNumPoliza, string pAsegurado)
        {

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT NumPoliza, NomAsegurado FROM ITM_70" +
                                  " WHERE NumPoliza = @NumPoliza";

                MySqlCommand cmd = new MySqlCommand(strQuery, dbConn.Connection);

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                cmd.Parameters.AddWithValue("@NumPoliza", pNumPoliza);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        return 1;   // Duplicado encontrado

                    }
                }

                dbConn.Close();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

            return 0;
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            Insertar_ITM_70();
        }

        protected void Insertar_ITM_70()
        {

            if (Page.IsValid)
            {
                //int iDuplicado = Validar_Duplicado(TxtNumPoliza.Text, TxtNomAsegurado.Text);

                //if (iDuplicado == 0)
                //{
                //    LblMessage.Text = "Número de Póliza ó Nombre del Asegurado, duplicados";
                //    mpeMensaje.Show();

                //    return;
                //}

                rfvFechaInput.Display = ValidatorDisplay.None;
                cvCiaSeguros.Display = ValidatorDisplay.None;
                cvProyecto.Display = ValidatorDisplay.None;
                rfvCliente.Display = ValidatorDisplay.None;
                cvTipoAsunto.Display = ValidatorDisplay.None;

                rfvEstOcurrencia.Display = ValidatorDisplay.Dynamic;
                rfvDescMote.Display = ValidatorDisplay.Dynamic;

                rfvNomActor.Display = ValidatorDisplay.None;
                rfvDemandado.Display = ValidatorDisplay.None;
                rfvNumSiniestro.Display = ValidatorDisplay.None;
                rfvNumPoliza.Display = ValidatorDisplay.None;
                rfvNumReporte.Display = ValidatorDisplay.Dynamic;
                rfvNomAsegurado.Display = ValidatorDisplay.Dynamic;
                cvTecResponsable.Display = ValidatorDisplay.Dynamic;
                cvAdminResponsable.Display = ValidatorDisplay.Dynamic;

                int IdTpoAsunto = Convert.ToInt32(ddlTipoAsunto.SelectedValue);

                int iPoliza = Validar_NumPoliza(TxtNumPoliza.Text);

                if (iPoliza == 0) { Variables.wPoliza = true; }

                if (iPoliza == 1 && Variables.wPoliza == false && (IdTpoAsunto == 2 || IdTpoAsunto == 3))
                {

                    Variables.wPoliza = true;

                    LblMessage_1.Text = "Se esta duplicando el Número de Póliza";
                    mpeMensaje_1.Show();

                    return;

                }
                else
                {
                    int iAsegurado = Validar_Asegurado(TxtNomAsegurado.Text);

                    if (iAsegurado == 0) { Variables.wAsegurado = true; }

                    if (iAsegurado == 1 && Variables.wAsegurado == false && (IdTpoAsunto == 2 || IdTpoAsunto == 3))
                    {
                        Variables.wAsegurado = true;

                        LblMessage_1.Text = "Se esta duplicando el Nombre del Asegurado";
                        mpeMensaje_1.Show();

                        return;
                    }
                }

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

                    string sReferencia = string.Empty;
                    string sRef_Actual = Variables.wRef_Proyecto;

                    // string referencia = Variables.wRef_Proyecto;
                    string añoActual = DateTime.Now.Year.ToString().Substring(2);   // Año Actual

                 // if (sRef_Actual != null)
                    if (!string.IsNullOrEmpty(sRef_Actual) && sRef_Actual.Length > 2)
                    {
                        // Si tiene más de 2 caracteres, quitamos los últimos 2 y concatenamos el año
                        sReferencia = sRef_Actual.Substring(0, sRef_Actual.Length - 2) + añoActual;

                    } else
                    {
                        sReferencia = Variables.wRef_Proyecto;

                    }

                    int sSubRefencia = Obtener_Sub_Referencia(sReferencia);

                    DateTime fecha = DateTime.ParseExact(TxtFechaInput.Text, "yyyy-MM-dd", null);
                    string fechaFormateada = fecha.ToString("dd/MM/yyyy");

                    // int year = fecha.Year;
                    // int month = fecha.Month;
                    // int day = fecha.Day;

                    string sIdSeguros = ddlCiaSeguros.SelectedValue;


                    //int iConsecutivo = Obtener_Consecutivo("fwAlta_Asunto");


                    // if (ddlTipoAsunto.SelectedValue == "1")
                    // {
                    //     sReferencia = "T-" + iConsecutivo + "-" + sIdSeguros + fecha.ToString("yy");
                    // }
                    // else if (ddlTipoAsunto.SelectedValue == "2")
                    // {
                    //     sReferencia = "SS-" + iConsecutivo + "-" + sIdSeguros + fecha.ToString("yy");
                    // }
                    // else if (ddlTipoAsunto.SelectedValue == "3")
                    // {
                    //     sReferencia = "SC-" + iConsecutivo + "-" + sIdSeguros + fecha.ToString("yy");
                    // }
                    // else if (ddlTipoAsunto.SelectedValue == "4")
                    // {
                    //     sReferencia = "L-" + iConsecutivo + "-" + sIdSeguros + fecha.ToString("yy");
                    // }

                    string sNumSiniestro = TxtNumSiniestro.Text;
                    string sNumPoliza = TxtNumPoliza.Text;
                    string sNumReporte = TxtNumReporte.Text;
                    string sNomCliente = TxtNomCliente.Text;
                    string sNomActor = TxtNomActor.Text;
                    string sNomDemandado = TxtDemandado.Text;
                    string sNomAsegurado = TxtNomAsegurado.Text;
                    string sEstOcurrencia = TxtEstOcurrencia.Text;
                    string sDescMote = TxtDescMote.Text;

                    int iIdProyecto;
                    int iConsecutivo = 0;

                    if (ddlProyecto.SelectedValue == "0")
                    {
                        iIdProyecto = 0;
                        sSubRefencia = 0;

                        iConsecutivo = Obtener_Consecutivo("fwAlta_Asunto");

                        if (ddlTipoAsunto.SelectedValue == "1")
                        {
                            sReferencia = "T-" + iConsecutivo + "-" + sIdSeguros + fecha.ToString("yy");
                        }
                        else if (ddlTipoAsunto.SelectedValue == "2")
                        {
                            sReferencia = "SS-" + iConsecutivo + "-" + sIdSeguros + fecha.ToString("yy");
                        }
                        else if (ddlTipoAsunto.SelectedValue == "3")
                        {
                            sReferencia = "SC-" + iConsecutivo + "-" + sIdSeguros + fecha.ToString("yy");
                        }
                        else if (ddlTipoAsunto.SelectedValue == "4")
                        {
                            sReferencia = "L-" + iConsecutivo + "-" + sIdSeguros + fecha.ToString("yy");
                        }

                    }
                    else
                    {
                        iIdProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);
                    }

                    int iIdTpoAsunto = Convert.ToInt32(ddlTipoAsunto.SelectedValue);
                    int iIdRespTecnico = Convert.ToInt32(ddlRespTecnico.SelectedValue);
                    int iIdRespAdministrativo = Convert.ToInt32(ddlRespAdministrativo.SelectedValue);

                    // Insertar registro tabla (ITM_70)
                    string strQuery = "INSERT INTO ITM_70 (Referencia, SubReferencia, NumSiniestro,  NumPoliza, NumReporte, IdSeguros, IdTpoAsunto, IdProyecto, IdRegimen, NomCliente, NomActor, NomDemandado, NomAsegurado, IdRespTecnico, IdRespAdministrativo, " +
                        "Fecha_Asignacion, IdConclusion, IdTpoProyecto, EstOcurrencia, DescMote, Id_Usuario, IdStatus ) " +
                        "VALUES('" + sReferencia + "', '" + sSubRefencia + "', '" + sNumSiniestro + "', '" + sNumPoliza + "', '" + sNumReporte + "', '" + sIdSeguros + "', " + iIdTpoAsunto + ", " + iIdProyecto + ", 1, '" + sNomCliente + "'," +
                        "'" + sNomActor + "', '" + sNomDemandado + "', '" + sNomAsegurado + "', " + iIdRespTecnico + ", " + iIdRespAdministrativo + "," +
                        "'" + fechaFormateada + "', 1, 0, '" + sEstOcurrencia + "', '" + sDescMote + "', '" + sUsuario + "', 1); ";

                    strQuery += Environment.NewLine;

                    if (ddlProyecto.SelectedValue == "0")
                    {
                        iConsecutivo++;
                        strQuery += "UPDATE ITM_71 SET IdConsecutivo = " + iConsecutivo + " WHERE IdProceso = 'fwAlta_Asunto'";
                    }

                    int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                    dbConn.Close();

                    //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                    //int affectedRows = cmd.ExecuteNonQuery();

                    DateTime fechaConvertida = DateTime.ParseExact(TxtFechaInput.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    string sFecAsignacion = fechaConvertida.ToString("dd/MM/yyyy");

                    Response.Write(sFecAsignacion);

                    string sCiaSeguros = ddlCiaSeguros.SelectedItem.Text;
                    string sRespAdministrativo = ddlRespAdministrativo.SelectedItem.Text;
                    string sRespTecnico = ddlRespTecnico.SelectedItem.Text;

                    // Insertar renglon datos excel onedrive
                    // InsertarDatosEnTablaExcel(sReferencia, sFecAsignacion, sNumSiniestro, sCiaSeguros, sRespAdministrativo, sRespTecnico);

                    LblMessage.Text = "Se agrego nuevo asunto, correctamente " + "<br />" + "Num. Referencia : " + sReferencia + "-" + sSubRefencia;
                    mpeMensaje.Show();

                    // Inicializar controles
                    Limpia(this.Controls);

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

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //SqlDataReader dr = cmd.ExecuteReader();

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

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwReporte_Alta_Asunto.aspx", true);
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
            args.IsValid = ddlRespTecnico.SelectedValue != "0";
        }

        protected void cvProyecto_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlProyecto.SelectedValue != "-1";
        }

        protected void cvTipoAsunto_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlTipoAsunto.SelectedValue != "0";
        }

        protected void ddlCiaSeguros_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetProyecto();

            ddlProyecto.ClearSelection();
            ddlTipoAsunto.ClearSelection();

            TxtNumPoliza.Text = string.Empty;
            TxtNomAsegurado.Text = string.Empty;

            bool condicionCumplida = (ddlCiaSeguros.SelectedValue == "OTR");

            divCliente.Visible = (ddlCiaSeguros.SelectedValue == "OTR");
            rfvCliente.Enabled = condicionCumplida;

            // Verificar si la empresa seleccionada es "INB"
            bool esINB = ddlCiaSeguros.SelectedValue == "INB";

            // Activar o desactivar los validadores según la empresa seleccionada
            rfvEstOcurrencia.Enabled = esINB;
            rfvDescMote.Enabled = esINB;

            // Desactivar validadores si la empresa es "INB", de lo contrario activarlos
            rfvNumReporte.Enabled = !esINB;
            rfvNomAsegurado.Enabled = !esINB;

        }

        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtNumPoliza.Text = string.Empty;
            TxtNomAsegurado.Text = string.Empty;

            GetDatos_Proyecto();

            if (ddlProyecto.SelectedValue == "0")
            {
                ddlTipoAsunto.SelectedIndex = 0;
            }
        }

        protected void ddlTipoAsunto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoAsunto.SelectedValue == "0")
            {
                // Inicializar.
                //divCampo1.Visible = false;
                //divCampo2.Visible = false;

                divNomActor.Visible = false;
                divDemandado.Visible = false;
                divNumSiniestro.Visible = false;
                divNumPoliza.Visible = false;
                divNumReporte.Visible = false;
                divNomAsegurado.Visible = false;
            }
            else if (ddlTipoAsunto.SelectedValue == "1")
            {
                // NOTIFICACION
                //divCampo1.Visible = true;
                //divCampo2.Visible = true;

                divNomActor.Visible = false;
                divDemandado.Visible = false;
                divNumSiniestro.Visible = false;
                divNumPoliza.Visible = false;
                divNumReporte.Visible = true;
                divNomAsegurado.Visible = true;
                divTecResponsable.Visible = false;
            }
            else if (ddlTipoAsunto.SelectedValue == "2")
            {
                // SIMPLE
                //divCampo1.Visible = false;
                //divCampo2.Visible = false;

                divNomActor.Visible = false;
                divDemandado.Visible = false;
                divNumSiniestro.Visible = true;
                divNumPoliza.Visible = true;
                divNumReporte.Visible = false;
                divNomAsegurado.Visible = true;
                divTecResponsable.Visible = true;
            }
            else if (ddlTipoAsunto.SelectedValue == "3")
            {
                // COMPLEJO
                //divCampo1.Visible = false;
                //divCampo2.Visible = false;

                divNomActor.Visible = false;
                divDemandado.Visible = false;
                divNumSiniestro.Visible = true;
                divNumPoliza.Visible = true;
                divNumReporte.Visible = false;
                divNomAsegurado.Visible = true;
                divTecResponsable.Visible = true;
            }
            else if (ddlTipoAsunto.SelectedValue == "4")
            {
                // LITIGIO
                //divCampo1.Visible = false;
                //divCampo2.Visible = false;

                divNomActor.Visible = true;
                divDemandado.Visible = true;
                divNumSiniestro.Visible = false;
                divNumPoliza.Visible = false;
                divNumReporte.Visible = false;
                divNomAsegurado.Visible = false;
                divTecResponsable.Visible = true;
            }

            ValidarCamposPorEmpresa();
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
                hojaTrabajo.Cells[ultimaFila, 1].Value = "REF123";  // Referencia
                hojaTrabajo.Cells[ultimaFila, 2].Value = DateTime.Now.ToString("dd/MM/yyyy");  // Fecha de Asignación
                hojaTrabajo.Cells[ultimaFila, 3].Value = "SIN12345";  // Número de Siniestro
                hojaTrabajo.Cells[ultimaFila, 4].Value = "Compañía XYZ";  // Compañía de Seguros
                hojaTrabajo.Cells[ultimaFila, 5].Value = "Juan Pérez";  // Responsable Administrativo
                hojaTrabajo.Cells[ultimaFila, 6].Value = "María López";  // Responsable Técnico
                hojaTrabajo.Cells[ultimaFila, 7].Value = "Tipo de caso";  // Indique si se trata de

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

        private void ValidarCamposPorEmpresa()
        {
            string empresaSeleccionada = ddlCiaSeguros.SelectedValue; // Obtiene el valor de la empresa seleccionada

            if (empresaSeleccionada == "INB" && ddlTipoAsunto.SelectedValue == "1")
            {
                // Mostrar campos obligatorios para INB
                divCampo1.Visible = true;
                divCampo2.Visible = true;
                rfvEstOcurrencia.Enabled = true;
                rfvDescMote.Enabled = true;

                // Ocultar campos no obligatorios para INB
                divNumReporte.Visible = false;
                divNomAsegurado.Visible = false;
                rfvNumReporte.Enabled = false;
                rfvNomAsegurado.Enabled = false;
            }
            else
            {
                // Ocultar campos obligatorios cuando NO es INB
                divCampo1.Visible = false;
                divCampo2.Visible = false;
                rfvEstOcurrencia.Enabled = false;
                rfvDescMote.Enabled = false;

                //// Mostrar campos no obligatorios
                //divNumReporte.Visible = true;
                //divNomAsegurado.Visible = true;
                //rfvNumReporte.Enabled = true;
                //rfvNomAsegurado.Enabled = true;
            }
        }

    }
}