using System;
using System.Collections.Generic;

using System.Data;
using System.Data.SqlClient;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwAlta_Inspecciones : System.Web.UI.Page
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
                try
                {
                    // Labels
                    lblTitulo_Alta_Inspecciones.Text = GetGlobalResourceObject("GlobalResources", "lblTitulo_Alta_Inspecciones").ToString();

                    TxtRef_Siniestro.Text = string.Empty;
                    TxtNomAsegurado.Text = string.Empty;

                    TxtNomAsegurado.Enabled = false;

                    GetHoraProgramada();
                    GetMinutos();
                    GetEstados();
                    GetTipoInspeccion();
                    GetResponsableInspeccion();

                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

            }
        }

        protected void GetHoraProgramada()
        {
            try
            {
                //ConexionBD Conecta = new ConexionBD();
                //Conecta.Abrir();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdHora_Programada, Hora " +
                                        " FROM ITM_62 " +
                                        " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlHoraProgramada.DataSource = dt;

                ddlHoraProgramada.DataValueField = "IdHora_Programada";
                ddlHoraProgramada.DataTextField = "Hora";

                ddlHoraProgramada.DataBind();
                ddlHoraProgramada.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        protected void GetMinutos()
        {
            // Crear una lista de valores
            List<string> valores = new List<string> { "00", "15", "30", "45" };

            // Asignar la lista al DropDownList
            ddlMinutes.DataSource = valores;
            ddlMinutes.DataBind();

            // Puedes agregar un elemento por defecto si lo necesitas
            ddlMinutes.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
        }

        protected void GetEstados()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT DISTINCT " +
                                  "  CASE WHEN c_estado = 05 THEN 'Coahuila' " +
                                  "       WHEN c_estado = 16 THEN 'Michoacán' " +
                                  "       WHEN c_estado = 30 THEN 'Veracruz' " +
                                  "       ELSE d_estado " +
                                  "   END AS d_estado, c_estado " +
                                  " FROM ITM_75 " +
                                  "ORDER BY d_estado ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlEstado.DataSource = dt;

                ddlEstado.DataValueField = "c_estado";
                ddlEstado.DataTextField = "d_estado";

                ddlEstado.DataBind();
                ddlEstado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetMunicipios(string pEstado) 
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT DISTINCT D_mnpio, c_mnpio " +
                                  " FROM ITM_75 " +
                                  "WHERE c_estado = '" + pEstado + "'" +
                                  "ORDER BY D_mnpio";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //da.Fill(dt);

                ddlMunicipios.DataSource = dt;

                ddlMunicipios.DataValueField = "c_mnpio";
                ddlMunicipios.DataTextField = "D_mnpio";

                ddlMunicipios.DataBind();
                ddlMunicipios.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetTipoInspeccion()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdTpo_Inspeccion, NomTpo_Inspeccion " +
                                        " FROM ITM_63 " +
                                        " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlTipoInspeccion.DataSource = dt;

                ddlTipoInspeccion.DataValueField = "IdTpo_Inspeccion";
                ddlTipoInspeccion.DataTextField = "NomTpo_Inspeccion";

                ddlTipoInspeccion.DataBind();
                ddlTipoInspeccion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        protected void GetResponsableInspeccion()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdResponsable_Inspeccion, Nom_Responsable " +
                                        " FROM ITM_64 " +
                                        " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlResponsableInspeccion.DataSource = dt;

                ddlResponsableInspeccion.DataValueField = "IdResponsable_Inspeccion";
                ddlResponsableInspeccion.DataTextField = "Nom_Responsable";

                ddlResponsableInspeccion.DataBind();
                ddlResponsableInspeccion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        protected void GetCodigoPostal(string sEstado, string sMunicipio)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = string.Empty;

                strQuery = "SELECT d_codigo, d_asenta, d_tipo_asenta " +
                           "  FROM ITM_75 " +
                           " WHERE c_estado = '" + sEstado + "'" +
                           "   AND c_mnpio = '" + sMunicipio + "'";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    grdCodigoPostal.ShowHeaderWhenEmpty = true;
                    grdCodigoPostal.EmptyDataText = "No hay resultados.";
                }

                grdCodigoPostal.DataSource = dt;
                grdCodigoPostal.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                grdCodigoPostal.HeaderRow.TableSection = TableRowSection.TableHeader;

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
                //Lbl_Message.Text = FnErrorMessage(ex.Message);
            }
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {

            if (Page.IsValid)
            {
                try
                {
                    ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                    dbConn.Open();

                    //  string sUsuario = string.Empty;
                    string sUsuario = Variables.wUserLogon;
                    string sHoraProgramada = ddlHoraProgramada.SelectedValue;
                    string sMinutes = ddlMinutes.SelectedValue;
                    string sEstado = ddlEstado.SelectedValue;
                    string sMunicipio = ddlMunicipios.SelectedValue;
                    string sTipoInspeccion = ddlTipoInspeccion.SelectedValue;
                    string sResponsableInspeccion = ddlResponsableInspeccion.SelectedValue;

                    DateTime fecha = DateTime.ParseExact(TxtFechaInput.Text, "yyyy-MM-dd", null);
                    string fechaFormateada = fecha.ToString("dd/MM/yyyy");

                    DateTime hora = DateTime.ParseExact(ddlHoraProgramada.SelectedItem.ToString(), "HH:mm", null);

                    int year = fecha.Year;
                    int month = fecha.Month;
                    int day = fecha.Day;

                    int time_ini = hora.Hour;
                    int time_fin = hora.Hour + 1;

                    // Insertar registro tabla (ITM_61)
                    string strQuery = "INSERT INTO ITM_61 (Ref_Siniestro, Nom_Asegurado, Riesgo, Fecha_Programada, eventstart, eventend, Calle, Num_Exterior, Num_Interior, Estado, Municipio, Codigo_Postal, Colonia, Lugar_Inspeccion, IdHora_Programada, IdTpo_Inspeccion, IdResponsable_Inspeccion, Id_Usuario, IdStatus) " +
                    "VALUES('" + TxtRef_Siniestro.Text + "', '" + TxtNomAsegurado.Text + "', '" + TxtRiesgo.Text + "', '" + fechaFormateada + "', " +
                    //" SMALLDATETIMEFROMPARTS(" + year + ", " + month + ", " + day + ", " + time_ini + ", 0), " +
                    //" SMALLDATETIMEFROMPARTS(" + year + ", " + month + ", " + day + ", " + time_fin + " , 0), " +
                    " STR_TO_DATE('" + year + "-" + month + "-" + day + " " + time_ini + ":00', '%Y-%m-%d %H:%i:%s'), " +
                    " STR_TO_DATE('" + year + "-" + month + "-" + day + " " + time_fin + ":00', '%Y-%m-%d %H:%i:%s'), " +
                    "'" + TxtCalle.Text + "', '" + TxtNumExt.Text + "', '" + TxtNumInt.Text + "', '" + sEstado + "', '" + sMunicipio + "', '" + TxtCodigoPostal.Text + "', '" + TxtColonia.Text + "', " +
                    "'" + TxtObservacionesDomicilio.Text + "', '" + sHoraProgramada + "', '" + sTipoInspeccion + "', '" + sResponsableInspeccion + "', '" + sUsuario + "', 1)";

                    int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                    dbConn.Close();

                    //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                    //int affectedRows = cmd.ExecuteNonQuery();

                    // Inicializar controles
                    Limpia(this.Controls);

                    rfvRef_Siniestro.Display = ValidatorDisplay.None;
                    // rfvAsegurado.Display = ValidatorDisplay.None;
                    rfvRiesgo.Display = ValidatorDisplay.None;
                    rfvFechaInput.Display = ValidatorDisplay.None;
                    cvHoraProgramada.Display = ValidatorDisplay.None;
                    rfvCalle.Display = ValidatorDisplay.None;
                    rfvNumExt.Display = ValidatorDisplay.None;
                    // rfvNumInt.Display = ValidatorDisplay.None;
                    cvEstado.Display = ValidatorDisplay.None;
                    cvMunicipios.Display = ValidatorDisplay.None;
                    rfvCodigoPostal.Display = ValidatorDisplay.None;
                    rfvColonia.Display = ValidatorDisplay.None;
                    // rfvObservacionesDomicilio.Display = ValidatorDisplay.None;
                    cvTipoInspeccion.Display = ValidatorDisplay.None;
                    cvResponsableInspeccion.Display = ValidatorDisplay.None;

                    LblMessage.Text = "Se agendo la cita, correctamente";
                    mpeMensaje.Show();

                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    mpeMensaje.Show();
                }

            }
        }

        public int Valida_DatosObligatorios()
        {
            if (TxtRef_Siniestro.Text == "")
            {
                LblMessage.Text = "Capturar Referencia";
                mpeMensaje.Show();
                return -1;
            }
            //else if (TxtNomAsegurado.Text == "")
            //{
            //    LblMessage.Text = "Capturar Nombre del Asegurado";
            //    mpeMensaje.Show();
            //    return -1;
            //}
            //else if (TxtSiniestro.Text == "")
            //{
            //    LblMessage.Text = "Capturar Numero de Siniestro";
            //    mpeMensaje.Show();
            //    return -1;
            //}
            else if (TxtRiesgo.Text == "")
            {
                LblMessage.Text = "Capturar Riesgo";
                mpeMensaje.Show();
                return -1;
            }
            else if (TxtFechaInput.Text == "")
            {
                LblMessage.Text = "Seleccione Fecha Programada";
                mpeMensaje.Show();
                return -1;
            }
            else if (ddlHoraProgramada.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccione Hora Programada";
                mpeMensaje.Show();
                return -1;
            }
            //else if (TxtObservacionesDomicilio.Text == "")
            //{
            //    LblMessage.Text = "Capturar Lugar de la Inspección";
            //    mpeMensaje.Show();
            //    return -1;
            //}
            else if (ddlTipoInspeccion.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccione Tipo de Inspección";
                mpeMensaje.Show();
                return -1;
            }
            else if (ddlResponsableInspeccion.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccione Responsable de la Inspección";
                mpeMensaje.Show();
                return -1;
            }

            return 0;
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

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwControl_Inspecciones.aspx", true);
        }

        protected void cvHoraProgramada_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlHoraProgramada.SelectedValue != "0";
        }

        protected void cvTipoInspeccion_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlTipoInspeccion.SelectedValue != "0";
        }

        protected void cvResponsableInspeccion_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlResponsableInspeccion.SelectedValue != "0";
        }

        protected void cvMinutes_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlMinutes.SelectedValue != "0";
        }

        protected void ImgBusReference_Click(object sender, ImageClickEventArgs e)
        {
            if (TxtRef_Siniestro.Text != string.Empty)
            {
                // bAltaAsunto = 0 -- No esta dado de Alta
                // bAltaAsunto = 1 -- Si esta dado de Alta
                int bAltaAsunto = GetBuscador_ITM_70(TxtRef_Siniestro.Text, "Referencia");

                if (bAltaAsunto == 1)
                {
                    // GetBuscador(TxtRef.Text, "Referencia");
                }

            }

        }

        public int GetBuscador_ITM_70(string sValor, string sColumna)
        {
            try
            {
                // DesHabilitar_Controles();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t0.Referencia, " +
                                  "  CASE WHEN t0.SubReferencia >= 0 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END AS Referencia_Sub," +
                                  "            t0.NomAsegurado, t0.IdTpoAsunto, t1.IdAseguradora,  " +
                                  "  CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE '' END AS NomCliente,  " +
                                  "            t2.Descripcion," +
                                  "            t0.IdStatus " +
                                  "  FROM  ITM_70 t0 " +
                                  "  JOIN ITM_48 t1 ON t0.IdSeguros = t1.IdSeguros " +
                                  "  JOIN ITM_66 t2 ON t0.IdTpoAsunto = t2.IdTpoAsunto" +
                                  " WHERE (CASE WHEN t0.SubReferencia >= 0 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END) = '" + sValor + "' " +
                                  "   AND t0.IdStatus IN (1)";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    // Inicializar controles
                    TxtRef_Siniestro.Text = string.Empty;
                    TxtNomAsegurado.Text = string.Empty;

                    LblMessage_1.Text = "La referencia no se encuentra registrada";
                    mpeMensaje_1.Show();

                }
                else
                {

                    TxtRef_Siniestro.Enabled = true;
                    ImgBusReference.Enabled = true;

                    //btnCrearRef.Enabled = true;
                    //btnEliminarRef.Enabled = false;

                    // Guardar el valor del nombre del asegurado
                    Variables.wNomAsegurado = dt.Rows[0].ItemArray[2].ToString();

                    TxtNomAsegurado.Text = dt.Rows[0].ItemArray[2].ToString();

                    return 1;
                }

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

            return 0;
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwAlta_Asunto.aspx", true);
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void cvEstado_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlEstado.SelectedValue != "0";
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sEstado = ddlEstado.SelectedValue;
            GetMunicipios(sEstado);
        }

        protected void ddlMunicipios_SelectedIndexChanged(object sender, EventArgs e)
        {

            BtnAddCodigoPostal.Enabled = true;
        }

        protected void cvMunicipios_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlMunicipios.SelectedValue != "0";
        }

        protected void BtnAddCodigoPostal_Click(object sender, EventArgs e)
        {
            PnlCodigoPostal.Visible = true;

            string sEstado = ddlEstado.SelectedValue;
            string sMunicipio = ddlMunicipios.SelectedValue;

            GetCodigoPostal(sEstado, sMunicipio);

            mpeNewProceso.Show();
        }

        protected void grdCodigoPostal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Add_Codigo_Postal();
        }

        protected void Add_Codigo_Postal()
        {
            try
            {

                foreach (GridViewRow row in grdCodigoPostal.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        var chkbox = row.FindControl("ChBoxRow") as CheckBox;

                        if (chkbox.Checked)
                        {
                            TxtCodigoPostal.Text = Server.HtmlDecode(Convert.ToString(row.Cells[1].Text));
                            TxtColonia.Text = Server.HtmlDecode(Convert.ToString(row.Cells[2].Text));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void btnClose_Proceso_Click(object sender, EventArgs e)
        {
           // PnlCodigoPostal.Visible = false;
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

        }

        protected void lnkCPostal_Click(object sender, EventArgs e)
        {
            LinkButton lnkReferencia = (LinkButton)sender;
            string[] argumentParts = lnkReferencia.CommandArgument.Split('|');
            string CodigoPostal = argumentParts[0];
            string Asentamiento = argumentParts[1];

            TxtCodigoPostal.Text = CodigoPostal;
            TxtColonia.Text = Asentamiento;

            // PnlCodigoPostal.Visible = false;
        }

        protected void grdCodigoPostal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkCodigoPostal = (LinkButton)e.Row.FindControl("lnkCPostal");

                // Recupera el valor de la segunda columna (NombreProducto) para esta fila
                string asentamiento = DataBinder.Eval(e.Row.DataItem, "d_asenta").ToString();

                // Establece el CommandArgument del LinkButton como la referencia y el nombre del producto separados por un delimitador (por ejemplo, "|")
                lnkCodigoPostal.CommandArgument = $"{lnkCodigoPostal.CommandArgument}|{asentamiento}";

                //string referencia = lnkCodigoPostal.CommandArgument;
                //lnkCodigoPostal.CommandName = "MostrarDatos";        // Puedes usar cualquier nombre de comando aquí
                //lnkCodigoPostal.CommandArgument = referencia;
            }
        }

        protected void grdCodigoPostal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectPostalCode")
            {
                string[] arguments = e.CommandArgument.ToString().Split('|');
                string CodigoPostal = arguments[0];
                string Asentamiento = arguments[1];

                TxtCodigoPostal.Text = CodigoPostal;
                TxtColonia.Text = Asentamiento;

                // Mostrar o ocultar el panel según sea necesario
                // PnlCodigoPostal.Visible = false;
            }
        }

    }
}