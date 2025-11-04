using System;
using System.IO;

using System.Data;
using System.Data.SqlClient;

using System.Web.UI;
using System.Web.UI.WebControls;

using OfficeOpenXml;

namespace WebItNow_Peacock
{
    public partial class fwPV_Reporte_Alta_Asunto : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Session["DownloadsPath"] = GetDownloadFolderPath();

            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdUsuario"] == null || Session["UsPassword"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!Page.IsPostBack)
            {
                try
                {
                    Variables.wUserName = Convert.ToString(Session["IdUsuario"]);
                    Variables.wPassword = Convert.ToString(Session["UsPassword"]);
                    Variables.wPrivilegios = Convert.ToString(Session["UsPrivilegios"]);

                    if (Variables.wUserName == "" || Variables.wPassword == "")
                    {
                        Response.Redirect("Login.aspx", true);
                        return;
                    }

                    // GetCiaSeguros();
                    GetProcedimiento();

                    GetColumnas();
                    GetAltaAsunto("*", "*");
                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

                //* * Agrega THEAD y TBODY a GridView.
                GrdAlta_Asunto.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void habilitar_controles()
        {
            TxtRef.Enabled = true;
            ddlColumnas.Enabled = true;
            BtnExportarExcel.Enabled = true;
        }

        protected void deshabilitar_controles()
        {
            TxtRef.Enabled = false;
            ddlColumnas.Enabled = false;
            BtnExportarExcel.Enabled = false;
        }

        protected void GetEstatus()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdDocumento, Descripcion " +
                                        " FROM ITM_83 " +
                                        " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlFiltros.DataSource = dt;

                ddlFiltros.DataValueField = "IdDocumento";
                ddlFiltros.DataTextField = "Descripcion";

                ddlFiltros.DataBind();
                // ddlFiltros.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlFiltros.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetTpoAsunto()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdTpoAsunto, Descripcion " +
                                        " FROM ITM_66 " +
                                        " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlFiltros.DataSource = dt;

                ddlFiltros.DataValueField = "IdTpoAsunto";
                ddlFiltros.DataTextField = "Descripcion";

                ddlFiltros.DataBind();
                // ddlFiltros.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlFiltros.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetProyectos()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT DISTINCT(IdProyecto), Descripcion " +
                                        " FROM ITM_78 " +
                                        " WHERE IdStatus = 1 ";


                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlFiltros.DataSource = dt;

                ddlFiltros.DataValueField = "IdProyecto";
                ddlFiltros.DataTextField = "Descripcion";

                ddlFiltros.DataBind();
                // ddlFiltros.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));
                ddlFiltros.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "-1"));

                dbConn.Close();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetCiaSeguros()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdSeguros, Descripcion " +
                                        " FROM ITM_67 " +
                                        " WHERE IdStatus = 1 ORDER BY IdOrden";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlFiltros.DataSource = dt;

                ddlFiltros.DataValueField = "IdSeguros";
                ddlFiltros.DataTextField = "Descripcion";

                ddlFiltros.DataBind();
                // ddlFiltros.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlFiltros.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetProcedimiento()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdProcedimiento, Descripcion " +
                                  "  FROM ITM_05 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlProcedimiento.DataSource = dt;

                ddlProcedimiento.DataValueField = "IdProcedimiento";
                ddlProcedimiento.DataTextField = "Descripcion";

                ddlProcedimiento.DataBind();
                //ddlProcedimiento.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlProcedimiento.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetColumnas()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdColumna, Descripcion " +
                                        " FROM ITM_76 " +
                                        " WHERE IdStatus = 1 ORDER BY IdFiltro";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlColumnas.DataSource = dt;

                ddlColumnas.DataValueField = "IdColumna";
                ddlColumnas.DataTextField = "Descripcion";

                ddlColumnas.DataBind();
                //ddlColumnas.Items.Insert(0, new ListItem("-- Seleccionar Columna --", "0"));
                ddlColumnas.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select_Col").ToString(), "0"));

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

                string sqlQuery = "SELECT IdRespAdministrativo, Descripcion " +
                                        " FROM ITM_14 " +
                                        " WHERE IdStatus = 1";

                DataTable dt = dbConn.ExecuteQuery(sqlQuery);

                ddlFiltros.DataSource = dt;

                ddlFiltros.DataValueField = "IdRespAdministrativo";
                ddlFiltros.DataTextField = "Descripcion";

                ddlFiltros.DataBind();
                //ddlFiltros.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlFiltros.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

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

                string strQuery = "SELECT IdRespMedico, Descripcion " +
                                        " FROM ITM_12 " +
                                        " WHERE IdStatus = 1";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlFiltros.DataSource = dt;

                ddlFiltros.DataValueField = "IdRespMedico";
                ddlFiltros.DataTextField = "Descripcion";

                ddlFiltros.DataBind();
                //ddlFiltros.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlFiltros.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                dbConn.Close();


            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetAltaAsunto(string sValor, string sIdColumna)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int iTpoProyecto = 0;

                // Tipo de Consulta Asuntos / Proyectos
                //if (rbProyectos.Checked)
                //{
                //    iTpoProyecto = 1;
                //}

                string valorBusqueda = string.Empty;
                string strQuery = string.Empty;

                string campoBusqueda = "t0." + sIdColumna;

                if (TxtRef.Text != string.Empty)
                {
                    valorBusqueda = TxtRef.Text;
                }
                else if (ddlFiltros.SelectedValue != "-1")
                {
                    valorBusqueda = ddlFiltros.SelectedValue;
                }

                Console.WriteLine($"Búsqueda por {campoBusqueda} = {valorBusqueda}");

                if (sValor == string.Empty)
                {

                    // Consulta MySQL
                    strQuery = " SELECT t0.IdAsunto, t0.SubReferencia, " +
                               "   CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia " +
                               "    END AS Referencia_Sub, t0.NumSiniestro, t0.NumPoliza, t0.NumReporte, " +
                               " CONCAT( DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%d-'), " +
                               "   CASE DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%m') " +
                               "   WHEN '01' THEN 'Ene' WHEN '02' THEN 'Feb' WHEN '03' THEN 'Mar' " +
                               "   WHEN '04' THEN 'Abr' WHEN '05' THEN 'May' WHEN '06' THEN 'Jun' " +
                               "   WHEN '07' THEN 'Jul' WHEN '08' THEN 'Ago' WHEN '09' THEN 'Sep' " +
                               "   WHEN '10' THEN 'Oct' WHEN '11' THEN 'Nov' WHEN '12' THEN 'Dic' END, '-', " +
                               "   DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%Y') ) AS Fecha_Asignacion, " +
                               "   t0.NomCliente, t0.NomAsegurado, " +
                               "   CASE WHEN t0.IdStatus = 1 THEN 'ABIERTO' ELSE 'CERRADO' END AS IdStatus, " +
                               "   t0.Referencia AS Referencia, " +
                               "   CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END AS Seguro_Cia, " +
                               "   t3.Descripcion AS Resp_Tecnico, t4.Descripcion AS Resp_Administrativo, " +
                               "   t0.IdSeguros, t0.IdTpoProyecto " +
                               "  FROM ITM_77 t0 " +
                               "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                               "  LEFT JOIN ITM_68 t3 ON t0.IdRespTecnico = t3.IdRespTecnico " +
                               "  LEFT JOIN ITM_69 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                               " WHERE t0.IdStatus IN (1) AND t0.IdTpoProyecto = " + iTpoProyecto + " ";

                    if (sIdColumna == "IdRespTecnico" || sIdColumna == "IdRespAdministrativo")
                    {
                        strQuery += $"  AND {campoBusqueda} = {valorBusqueda} ";
                    }
                    else
                    {
                        //strQuery += "   AND " + $"{campoBusqueda}" + " LIKE '%' + '" + $"{valorBusqueda}" + "' + '%' ";
                        strQuery += "   AND " + $"{campoBusqueda}" + " LIKE CONCAT('%', '" + $"{valorBusqueda}" + "', '%') ";
                    }

                    strQuery += " ORDER BY t0.IdAsunto DESC LIMIT 100 ";

                }
                else
                {
                    strQuery = " SELECT t0.IdAsunto, t0.SubReferencia, " +
                               "    CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia " +
                               "     END AS Referencia_Sub, t0.NumSiniestro, t0.NumPoliza, t0.NumReporte, " +
                               " CONCAT( DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%d-'), " +
                               "   CASE DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%m') " +
                               "   WHEN '01' THEN 'Ene' WHEN '02' THEN 'Feb' WHEN '03' THEN 'Mar' " +
                               "   WHEN '04' THEN 'Abr' WHEN '05' THEN 'May' WHEN '06' THEN 'Jun' " +
                               "   WHEN '07' THEN 'Jul' WHEN '08' THEN 'Ago' WHEN '09' THEN 'Sep' " +
                               "   WHEN '10' THEN 'Oct' WHEN '11' THEN 'Nov' WHEN '12' THEN 'Dic' END, '-', " +
                               "   DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%Y') ) AS Fecha_Asignacion, " +
                               "   t0.NomCliente, t0.NomAsegurado, " +
                               "   CASE WHEN t0.IdStatus = 1 THEN 'ABIERTO' ELSE 'CERRADO' END AS IdStatus, " +
                               "   t0.Referencia AS Referencia, " +
                               "   CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END AS Seguro_Cia, " +
                               "   t3.Descripcion AS Resp_Tecnico, t4.Descripcion AS Resp_Administrativo, " +
                               "   t0.IdSeguros, t0.IdTpoProyecto " +
                               "  FROM ITM_77 t0 " +
                               "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                               "  LEFT JOIN ITM_68 t3 ON t0.IdRespTecnico = t3.IdRespTecnico " +
                               "  LEFT JOIN ITM_69 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                               " WHERE t0.IdStatus IN (1) AND t0.IdTpoProyecto = " + iTpoProyecto + " " +
                               " ORDER BY t0.IdAsunto DESC LIMIT 100 ";

                }

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdAlta_Asunto.ShowHeaderWhenEmpty = true;
                    //GrdAlta_Asunto.EmptyDataText = "No hay resultados.";
                    GrdAlta_Asunto.EmptyDataText = GetGlobalResourceObject("GlobalResources", "msg_NoResults").ToString();
                }

                GrdAlta_Asunto.DataSource = dt;
                GrdAlta_Asunto.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdAlta_Asunto.HeaderRow.TableSection = TableRowSection.TableHeader;

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
                //Lbl_Message.Text = FnErrorMessage(ex.Message);
            }
        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAltaAsunto_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwPV_Alta_Asunto.aspx", true);
        }

        protected void BtnExportarExcel_Click(object sender, EventArgs e)
        {

        }

        protected void ImgSubRef_Add_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void ImgSubRef_Del_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void rbAsuntos_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void rbProyectos_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void ddlFiltros_SelectedIndexChanged(object sender, EventArgs e)
        {
            string IdColumna = ddlColumnas.SelectedValue;

            GetAltaAsunto(string.Empty, IdColumna);
        }

        protected void ddlColumnas_SelectedIndexChanged(object sender, EventArgs e)
        {
            string IdColumna = ddlColumnas.SelectedValue;

            // Inicializar DropDownList de busqueda
            ddlFiltros.ClearSelection();

            switch (IdColumna)
            {
                case "IdSeguros":

                    GetCiaSeguros();

                    LblFiltros.Text = "Compañia de Seguros";
                    LblFiltros.Visible = true;
                    ddlFiltros.Visible = true;

                    TxtRef.Text = string.Empty;

                    break;

                case "IdProyecto":

                    GetProyectos();

                    LblFiltros.Text = "Nombre de Proyecto";
                    LblFiltros.Visible = true;
                    ddlFiltros.Visible = true;

                    TxtRef.Text = string.Empty;

                    break;

                case "IdTpoAsunto":

                    GetTpoAsunto();

                    LblFiltros.Text = "Tipo de Asunto";
                    LblFiltros.Visible = true;
                    ddlFiltros.Visible = true;

                    TxtRef.Text = string.Empty;

                    break;

                case "IdRespTecnico":

                    GetResponsableTec();

                    LblFiltros.Text = "Responsable Técnico";
                    LblFiltros.Visible = true;
                    ddlFiltros.Visible = true;

                    TxtRef.Text = string.Empty;

                    break;

                case "IdRespAdministrativo":

                    GetResponsableAdmin();

                    LblFiltros.Text = "Responsable Administrativo";
                    LblFiltros.Visible = true;
                    ddlFiltros.Visible = true;

                    TxtRef.Text = string.Empty;

                    break;

                case "IdStatus":

                    GetEstatus();

                    LblFiltros.Text = "Estatus";
                    LblFiltros.Visible = true;
                    ddlFiltros.Visible = true;

                    TxtRef.Text = string.Empty;

                    break;

                default:

                    LblFiltros.Text = "Filtros";
                    LblFiltros.Visible = false;
                    ddlFiltros.Visible = false;

                    if (IdColumna != "0")
                    {
                        GetAltaAsunto(string.Empty, IdColumna);
                    }
                    else
                    {
                        GetAltaAsunto("*", "*");
                    }

                    break;
            }
        }

        protected void ImgBusReference_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void ImgCheckList_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void GrdAlta_Asunto_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdAlta_Asunto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string privilegios = Convert.ToString(Session["UsPrivilegios"]);

            if (!string.IsNullOrEmpty(privilegios) && privilegios == "0")
            {
                var imgCheckList = e.Row.FindControl("ImgCheckList") as ImageButton;
                if (imgCheckList != null)
                {
                    imgCheckList.Enabled = false;
                }
            }


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Width = Unit.Pixel(110);         // Referencia
                e.Row.Cells[2].Width = Unit.Pixel(100);         // Num.Siniestro
                e.Row.Cells[3].Width = Unit.Pixel(100);         // Num.Poliza
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[4].Visible = false;     // Num.Reporte
                e.Row.Cells[7].Visible = false;     // NomAsegurado
             // e.Row.Cells[8].Visible = false;     // NomCliente
                e.Row.Cells[9].Visible = false;     // Resp_Tecnico
             // e.Row.Cells[10].Visible = false;    // Resp_Administrativo
                e.Row.Cells[11].Visible = false;    // IdStatus
                e.Row.Cells[12].Visible = false;    // Referencia
                e.Row.Cells[13].Visible = false;    // SubReferencia
                e.Row.Cells[14].Visible = false;    // IdSeguros
                e.Row.Cells[15].Visible = false;    // IdTpoProyecto
                e.Row.Cells[16].Visible = false;    // ImgSubRef_Add
                e.Row.Cells[17].Visible = false;    // ImgSubRef_Del

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Visible = false;     // Num.Reporte
                e.Row.Cells[7].Visible = false;     // NomAsegurado
             // e.Row.Cells[8].Visible = false;     // NomCliente
                e.Row.Cells[9].Visible = false;     // Resp_Tecnico
             // e.Row.Cells[10].Visible = false;    // Resp_Administrativo
                e.Row.Cells[11].Visible = false;    // IdStatus
                e.Row.Cells[12].Visible = false;    // Referencia
                e.Row.Cells[13].Visible = false;    // SubReferencia
                e.Row.Cells[14].Visible = false;    // IdSeguros
                e.Row.Cells[15].Visible = false;    // IdTpoProyecto
                e.Row.Cells[16].Visible = false;    // ImgSubRef_Add
                e.Row.Cells[17].Visible = false;    // ImgSubRef_Del
            }
        }

        protected void GrdAlta_Asunto_PreRender(object sender, EventArgs e)
        {
            // Ajustar el estilo de las filas en general antes de renderizar
            foreach (GridViewRow row in GrdAlta_Asunto.Rows)
            {
                row.Style["height"] = "20px"; // Establecer la altura de las filas
            }
        }

        protected void GrdAlta_Asunto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectReferencia")
            {
                string[] arguments = e.CommandArgument.ToString().Split('|');

                string sReferencia = arguments[1];
                string iSubReferencia = arguments[2];
                string sIdCliente = arguments[3];

                Response.Redirect("fwPV_Bitacora_Asunto.aspx?Ref=" + sReferencia + "&SubRef=" + iSubReferencia +
                                                                 "&Seguro=" + sIdCliente, true);

            }
        }

        protected void GrdAlta_Asunto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string IdColumna = ddlColumnas.SelectedValue;

            GrdAlta_Asunto.PageIndex = e.NewPageIndex;

            if (IdColumna != "0")
            {
                GetAltaAsunto(string.Empty, IdColumna);
            }
            else
            {
                GetAltaAsunto("*", "Referencia");
            }
        }

        protected void ddlProcedimiento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwPV_Mnu_Dinamico.aspx", true);
            return;
        }
    }
}