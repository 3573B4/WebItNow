using System;
using System.IO;

using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

using System.Linq;
using OfficeOpenXml;
using MySql.Data.MySqlClient;

namespace WebItNow_Peacock
{
    public partial class fwPV_Alta_Asunto : System.Web.UI.Page
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

        protected void GetResponsableAdmin()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

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

        protected void cvCiaSeguros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlCiaSeguros.SelectedValue != "0";
        }

        protected void cvAdminResponsable_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = ddlRespAdministrativo.SelectedValue != "0";
        }

        protected void ddlCiaSeguros_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnContinuar_Click(object sender, EventArgs e)
        {
            Variables.wContinuar = true;
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            Insertar_ITM_77();
        }

        protected void Insertar_ITM_77()
        {

            if (Page.IsValid)
            {

                rfvFechaInput.Display = ValidatorDisplay.None;
                cvCiaSeguros.Display = ValidatorDisplay.None;
                rfvCliente.Display = ValidatorDisplay.None;

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
                    string sUsuario = Variables.wUserLogon;

                    string sReferencia = Variables.wRef_Proyecto;
                    int sSubRefencia = 0;

                    DateTime fecha = DateTime.ParseExact(TxtFechaInput.Text, "yyyy-MM-dd", null);
                    string fechaFormateada = fecha.ToString("dd/MM/yyyy");

                    string sIdSeguros = ddlCiaSeguros.SelectedValue;

                    string sNumSiniestro = string.Empty;
                    string sNumPoliza = string.Empty;
                    string sNumReporte = string.Empty;
                    string sNomCliente = TxtNomCliente.Text;
                    string sNomAsegurado = string.Empty;
                    string sNomDemandado = string.Empty;
                    string sNomAjustador = string.Empty;
                    string sEstOcurrencia = string.Empty;
                    string sDescMote = string.Empty;

                    int iConsecutivo = 0;

                    iConsecutivo = Obtener_Consecutivo("fwAlta_Asunto_PV");
                    sReferencia = "RET-" + iConsecutivo.ToString("D4") + "/" + fecha.ToString("yy");

                    int iIdRespAdministrativo = Convert.ToInt32(ddlRespAdministrativo.SelectedValue);

                    // Insertar registro tabla (ITM_77)
                    string strQuery = "INSERT INTO ITM_77 (Referencia, SubReferencia, NumSiniestro,  NumPoliza, NumReporte, IdSeguros, IdRegimen, NomCliente, NomAsegurado, IdRespTecnico, IdRespAdministrativo, " +
                        "Fecha_Asignacion, IdConclusion, IdTpoProyecto, Id_Usuario, IdStatus ) " +
                        "VALUES('" + sReferencia + "', '" + sSubRefencia + "', '" + sNumSiniestro + "', '" + sNumPoliza + "', '" + sNumReporte + "', '" + sIdSeguros + "', 1, '" + sNomCliente + "', " +
                        "'" + sNomAsegurado + "',  0, " + iIdRespAdministrativo + ", " +
                        "'" + fechaFormateada + "', 1, 0, '" + sUsuario + "', 1); ";

                    strQuery += Environment.NewLine;

                    iConsecutivo++;
                    strQuery += "UPDATE ITM_71 SET IdConsecutivo = " + iConsecutivo + " WHERE IdProceso = 'fwAlta_Asunto_PV'";

                    int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                    dbConn.Close();

                    DateTime fechaConvertida = DateTime.ParseExact(TxtFechaInput.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    string sFecAsignacion = fechaConvertida.ToString("dd/MM/yyyy");

                    Response.Write(sFecAsignacion);

                    string sCiaSeguros = ddlCiaSeguros.SelectedItem.Text;
                    string sRespAdministrativo = ddlRespAdministrativo.SelectedItem.Text;

                    // Insertar renglon datos excel onedrive
                    // InsertarDatosEnTablaExcel(sReferencia, sFecAsignacion, sNumSiniestro, sCiaSeguros, sRespAdministrativo, sRespTecnico);

                    LblMessage.Text = "Se agrego nuevo asunto, correctamente " + "<br />" + "Num. Referencia : " + sReferencia;
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
            Response.Redirect("fwPV_Mnu_Dinamico.aspx", true);
            return;
        }
    }
}