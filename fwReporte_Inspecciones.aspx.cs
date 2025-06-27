using System;
using System.Data;
using System.Data.SqlClient;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwReporte_Inspecciones : System.Web.UI.Page
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

                    GetResponsableInspeccion();
                    GetAltaInspecciones("*", "*");

                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

                //* * Agrega THEAD y TBODY a GridView.
                GrdAlta_Inspeccion.HeaderRow.TableSection = TableRowSection.TableHeader;
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

                ddlResponsable.DataSource = dt;

                ddlResponsable.DataValueField = "IdResponsable_Inspeccion";
                ddlResponsable.DataTextField = "Nom_Responsable";

                ddlResponsable.DataBind();
                ddlResponsable.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }


        protected void GetAltaInspecciones(string sValor, string sIdInspecciones)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = string.Empty;


                if (sValor == string.Empty)
                {

                    //strQuery = "SET LANGUAGE Spanish " +
                    //            "SELECT TOP (50) t0.Id_Inspecciones, " +
                    //            "       UPPER(DATENAME(dw, Fecha_Programada)) As Dia_Semana, t0.Fecha_Programada, " +
                    //            "       FORMAT(eventstart, 'HH:mm') As Hora_Minutos, t0.Lugar_Inspeccion," +
                    //            "       t0.Ref_Siniestro, t2.Nom_Responsable As IdResponsable_Inspeccion, t1.NomTpo_Inspeccion as IdTpo_Inspeccion, " +
                    //            "       t0.Ref_Siniestro + '" + '/' + "' + t0.Nom_Asegurado + '" + '/' + "' + t0.Riesgo + '" + '/' + "' + t0.Calle + ' ' + t0.Num_Exterior + '" + ' ' + "' + 'C.P. ' + t0.Codigo_Postal + '" + ' ' + "' + t0.Colonia As Observaciones, t0.IdStatus " +
                    //            "  FROM ITM_61 t0, ITM_63 t1, ITM_64 t2 " +
                    //            " WHERE t0.IdResponsable_Inspeccion = t2.IdResponsable_Inspeccion " +
                    //            "   AND t0.IdTpo_Inspeccion = t1.IdTpo_Inspeccion " +
                    //            "   AND t0.IdResponsable_Inspeccion = '" + sIdInspecciones + "'" +
                    //            " ORDER BY t0.Id_Inspecciones DESC";

                    // Consulta MySQL
                    strQuery = "SELECT t0.Id_Inspecciones, " +
                               "       UPPER(DAYNAME(STR_TO_DATE(t0.Fecha_Programada, '%d/%m/%Y'))) AS Dia_Semana, t0.Fecha_Programada, " +
                               "       DATE_FORMAT(t0.eventstart, '%H:%i') AS Hora_Minutos, t0.Lugar_Inspeccion, " +
                               "       t0.Ref_Siniestro, t2.Nom_Responsable AS IdResponsable_Inspeccion, t1.NomTpo_Inspeccion AS IdTpo_Inspeccion, " +
                               "       CONCAT(t0.Ref_Siniestro, '/', t0.Nom_Asegurado, '/', t0.Riesgo, '/', t0.Calle, ' ', t0.Num_Exterior, ' C.P. ', t0.Codigo_Postal, ' ', t0.Colonia) AS Observaciones, " +
                               "       t0.IdStatus " +
                               "  FROM ITM_61 t0 " +
                               "  JOIN ITM_63 t1 ON t0.IdTpo_Inspeccion = t1.IdTpo_Inspeccion " +
                               "  JOIN ITM_64 t2 ON t0.IdResponsable_Inspeccion = t2.IdResponsable_Inspeccion " +
                               "   AND t0.IdResponsable_Inspeccion = '" + sIdInspecciones + "' " +
                               " ORDER BY t0.Id_Inspecciones DESC LIMIT 100;";
                }
                else
                {
                    //strQuery = "SET LANGUAGE Spanish " +
                    //            "SELECT TOP (50) t0.Id_Inspecciones, " +
                    //            "       UPPER(DATENAME(dw, Fecha_Programada)) As Dia_Semana, t0.Fecha_Programada, " +
                    //            "       FORMAT(eventstart, 'HH:mm') As Hora_Minutos, t0.Lugar_Inspeccion," +
                    //            "       t0.Ref_Siniestro, t2.Nom_Responsable As IdResponsable_Inspeccion, t1.NomTpo_Inspeccion as IdTpo_Inspeccion, " +
                    //            "       t0.Ref_Siniestro + '" + '/' + "' + t0.Nom_Asegurado + '" + '/' + "' + t0.Riesgo + '" + '/' + "' + t0.Calle + ' ' + t0.Num_Exterior + '" + ' ' + "' + 'C.P. ' + t0.Codigo_Postal + '" + ' ' + "' + t0.Colonia As Observaciones, t0.IdStatus " +
                    //            "  FROM ITM_61 t0, ITM_63 t1, ITM_64 t2 " +
                    //            " WHERE t0.IdResponsable_Inspeccion = t2.IdResponsable_Inspeccion " +
                    //            "   AND t0.IdTpo_Inspeccion = t1.IdTpo_Inspeccion " +
                    //            " ORDER BY t0.Id_Inspecciones DESC";

                    // Consulta MySQL
                    strQuery = "SELECT t0.Id_Inspecciones, " +
                               "       UPPER(DAYNAME(STR_TO_DATE(t0.Fecha_Programada, '%d/%m/%Y'))) AS Dia_Semana, t0.Fecha_Programada, " +
                               "       DATE_FORMAT(t0.eventstart, '%H:%i') AS Hora_Minutos, t0.Lugar_Inspeccion, " +
                               "       t0.Ref_Siniestro, t2.Nom_Responsable AS IdResponsable_Inspeccion, t1.NomTpo_Inspeccion AS IdTpo_Inspeccion, " +
                               "       CONCAT(t0.Ref_Siniestro, '/', t0.Nom_Asegurado, '/', t0.Riesgo, '/', t0.Calle, ' ', t0.Num_Exterior, ' C.P. ', t0.Codigo_Postal, ' ', t0.Colonia) AS Observaciones, " +
                               "       t0.IdStatus " +
                               "  FROM ITM_61 t0 " +
                               "  JOIN ITM_63 t1 ON t0.IdTpo_Inspeccion = t1.IdTpo_Inspeccion " +
                               "  JOIN ITM_64 t2 ON t0.IdResponsable_Inspeccion = t2.IdResponsable_Inspeccion " +
                               " ORDER BY t0.Id_Inspecciones DESC LIMIT 100;";
                }

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdAlta_Inspeccion.ShowHeaderWhenEmpty = true;
                    GrdAlta_Inspeccion.EmptyDataText = "No hay resultados.";
                }

                GrdAlta_Inspeccion.DataSource = dt;
                GrdAlta_Inspeccion.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdAlta_Inspeccion.HeaderRow.TableSection = TableRowSection.TableHeader;

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
                //Lbl_Message.Text = FnErrorMessage(ex.Message);
            }
        }

        protected void GrdAlta_Inspeccion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdAlta_Inspeccion.PageIndex = e.NewPageIndex;
            GetAltaInspecciones("*", "*");
        }

        protected void GrdAlta_Inspeccion_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdAlta_Inspeccion_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdAlta_Inspeccion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdAlta_Inspeccion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdAlta_Inspeccion, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[8].Visible = false;    // IdStatus
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[8].Visible = false;    // IdStatus
            }
        }

        protected void BtnAltaInspeccion_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwAlta_Inspecciones.aspx", true);
        }

        protected void BtnExportarExcel_Click(object sender, EventArgs e)
        {

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void ddlResponsable_SelectedIndexChanged(object sender, EventArgs e)
        {
            string IdInspecciones = ddlResponsable.SelectedValue;

            if(IdInspecciones == "0")
            {
                GetAltaInspecciones("*", "*");
            } 
            else
            {
                GetAltaInspecciones(string.Empty, IdInspecciones);
            }
        }

        protected void BtnAgendaInspecciones_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwAgenda_Inspecciones.aspx", true);
        }

    }
}