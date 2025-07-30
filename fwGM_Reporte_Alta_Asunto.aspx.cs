using System;
using System.IO;

using System.Data;
using System.Data.SqlClient;

using System.Web.UI;
using System.Web.UI.WebControls;

using OfficeOpenXml;

namespace WebItNow_Peacock.GastosMedicos
{
    public partial class fwGM_Reporte_Alta_Asunto : System.Web.UI.Page
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
                ddlProcedimiento.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

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
                ddlColumnas.Items.Insert(0, new ListItem("-- Seleccionar Columna --", "0"));

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
                ddlFiltros.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

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
                ddlFiltros.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

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

                string strQuery = "SELECT IdTpoEvento, Descripcion " +
                                        " FROM ITM_11 " +
                                        " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlFiltros.DataSource = dt;

                ddlFiltros.DataValueField = "IdTpoEvento";
                ddlFiltros.DataTextField = "Descripcion";

                ddlFiltros.DataBind();
                ddlFiltros.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
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
                ddlFiltros.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();

            }
            catch (System.Exception ex)
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
                ddlFiltros.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

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
                                        " FROM ITM_26 " +
                                        " WHERE IdStatus = 1 ";


                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlFiltros.DataSource = dt;

                ddlFiltros.DataValueField = "IdProyecto";
                ddlFiltros.DataTextField = "Descripcion";

                ddlFiltros.DataBind();
                ddlFiltros.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));

                dbConn.Close();

            }
            catch (Exception ex)
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
                if (rbProyectos.Checked)
                {
                    iTpoProyecto = 1;
                }

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
                               "    CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia " +
                               "     END AS Referencia_Sub, " +
                               "     t0.NumSiniestro, t0.NumPoliza, t0.NumReporte, " +
                               " CONCAT( DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%d-'), " +
                               "   CASE DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%m') " +
                               "   WHEN '01' THEN 'Ene' WHEN '02' THEN 'Feb' WHEN '03' THEN 'Mar' " +
                               "   WHEN '04' THEN 'Abr' WHEN '05' THEN 'May' WHEN '06' THEN 'Jun' " +
                               "   WHEN '07' THEN 'Jul' WHEN '08' THEN 'Ago' WHEN '09' THEN 'Sep' " +
                               "   WHEN '10' THEN 'Oct' WHEN '11' THEN 'Nov' WHEN '12' THEN 'Dic' END, '-', " +
                               "   DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%Y') " +
                               "   ) AS Fecha_Asignacion,  t0.Hora_Asignacion, " +
                               "   t0.NomActor, t0.NomDemandado, t0.NomAjustador, " +
                               "   CASE WHEN t0.IdStatus = 1 THEN 'ABIERTO' ELSE 'CERRADO' END AS IdStatus, " +
                               "   t0.Referencia AS Referencia, t1.Descripcion AS Tpo_Evento, " +
                               "   CASE WHEN t0.IdProyecto = 0 THEN 'NINGUNO' ELSE t5.Descripcion END AS NomProyecto, " +
                               "   CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END AS Seguro_Cia, " +
                               "   t3.Descripcion AS Resp_Tecnico, t4.Descripcion AS Resp_Administrativo, t0.IdProyecto, " +
                               "   t0.IdSeguros, t0.IdTpoEvento, t0.IdTpoProyecto, " +
                               "   CASE WHEN t0.IdEstatusCaso = 1 THEN 'Hospitalizado' WHEN t0.IdEstatusCaso = 2 THEN 'En Seguimiento' WHEN t0.IdEstatusCaso = 3 THEN 'Alta / Cerrado' END AS Estatus_Caso " +
                               "  FROM ITM_73 t0 " +
                               "  JOIN ITM_11 t1 ON t0.IdTpoEvento = t1.IdTpoEvento " +
                               "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                               "  LEFT JOIN ITM_12 t3 ON t0.IdRespMedico = t3.IdRespMedico " +
                               "  LEFT JOIN ITM_14 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                               "  LEFT JOIN ITM_26 t5 ON t0.IdProyecto = t5.IdProyecto AND t0.IdSeguros = t5.IdCliente " +
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
                               "     END AS Referencia_Sub, " +
                               "     t0.NumSiniestro, t0.NumPoliza, t0.NumReporte, " +
                               " CONCAT( DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%d-'), " +
                               "   CASE DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%m') " +
                               "   WHEN '01' THEN 'Ene' WHEN '02' THEN 'Feb' WHEN '03' THEN 'Mar' " +
                               "   WHEN '04' THEN 'Abr' WHEN '05' THEN 'May' WHEN '06' THEN 'Jun' " +
                               "   WHEN '07' THEN 'Jul' WHEN '08' THEN 'Ago' WHEN '09' THEN 'Sep' " +
                               "   WHEN '10' THEN 'Oct' WHEN '11' THEN 'Nov' WHEN '12' THEN 'Dic' END, '-', " +
                               "   DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%Y') " +
                               "   ) AS Fecha_Asignacion, t0.Hora_Asignacion, " +
                               "   t0.NomActor, t0.NomDemandado, t0.NomAjustador, " +
                               "   CASE WHEN t0.IdStatus = 1 THEN 'ABIERTO' ELSE 'CERRADO' END AS IdStatus, " +
                               "   t0.Referencia AS Referencia, t1.Descripcion AS Tpo_Evento, " +
                               "   CASE WHEN t0.IdProyecto = 0 THEN 'NINGUNO' ELSE t5.Descripcion END AS NomProyecto, " +
                               "   CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END AS Seguro_Cia, " +
                               "   t3.Descripcion AS Resp_Tecnico, t4.Descripcion AS Resp_Administrativo, t0.IdProyecto, " +
                               "   t0.IdSeguros, t0.IdTpoEvento, t0.IdTpoProyecto, " +
                               "   CASE WHEN t0.IdEstatusCaso = 1 THEN 'Hospitalizado' WHEN t0.IdEstatusCaso = 2 THEN 'En Seguimiento' WHEN t0.IdEstatusCaso = 3 THEN 'Alta / Cerrado' END AS Estatus_Caso " +
                               "  FROM ITM_73 t0 " +
                               "  JOIN ITM_11 t1 ON t0.IdTpoEvento = t1.IdTpoEvento " +
                               "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                               "  LEFT JOIN ITM_12 t3 ON t0.IdRespMedico = t3.IdRespMedico " +
                               "  LEFT JOIN ITM_14 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                               "  LEFT JOIN ITM_26 t5 ON t0.IdProyecto = t5.IdProyecto AND t0.IdSeguros = t5.IdCliente " +
                               " WHERE t0.IdStatus IN (1) AND t0.IdTpoProyecto = " + iTpoProyecto + " " +
                               " ORDER BY t0.IdAsunto DESC LIMIT 100 ";

                }

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdAlta_Asunto.ShowHeaderWhenEmpty = true;
                    GrdAlta_Asunto.EmptyDataText = "No hay resultados.";
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

        public string FnErrorMessage(string prmMessage)
        {
            return ("<span style=\"color:Red;\">" +
                    "<img src = \"images/icons16/error.png\" height=\"16\" width=\"16\" alt=\"Error\" />&nbsp;" +
                    prmMessage + "</span>");
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {

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

        protected void GrdAlta_Asunto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectReferencia")
            {
                string[] arguments = e.CommandArgument.ToString().Split('|');

                string sReferencia = arguments[1];
                string iSubReferencia = arguments[2];
                string iIdProyecto = arguments[3];
                string sIdCliente = arguments[4];
                string iIdTpoAsunto = arguments[5];


                //if (iIdTpoAsunto == "4")
                //{
                //    // LITIGIO
                //    //Response.Redirect("fwBitacora_Litigio.aspx?Ref=" + sReferencia + "&SubRef=" + iSubReferencia +
                //    //                                         "&Proyecto=" + iIdProyecto + "&Seguro=" + sIdCliente + "&Asunto=" + iIdTpoAsunto, true);
                //}
                //else
                //{
                //    // SIMPLE / COMPLEJO
                //    Response.Redirect("fwGM_Bitacora_Asunto.aspx?Ref=" + sReferencia + "&SubRef=" + iSubReferencia +
                //                                             "&Proyecto=" + iIdProyecto + "&Seguro=" + sIdCliente + "&Asunto=" + iIdTpoAsunto, true);
                //}

                // AMBULATORIO / TRASLADO
                Response.Redirect("fwGM_Bitacora_Asunto.aspx?Ref=" + sReferencia + "&SubRef=" + iSubReferencia +
                                                         "&Proyecto=" + iIdProyecto + "&Seguro=" + sIdCliente + "&Asunto=" + iIdTpoAsunto, true);
            }
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
                e.Row.Cells[4].Width = Unit.Pixel(100);         // IdProyecto
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Visible = false;     // Num.Reporte
                e.Row.Cells[10].Visible = false;    // Nom.Actor
                e.Row.Cells[11].Visible = false;    // Nom.Demandado
             // e.Row.Cells[12].Visible = false;    // NomAjustador
             // e.Row.Cells[13].Visible = false;    // Resp_Medico
             // e.Row.Cells[14].Visible = false;    // Resp_Administrativo
                e.Row.Cells[15].Visible = false;    // IdStatus
                e.Row.Cells[16].Visible = false;    // Referencia
                e.Row.Cells[17].Visible = false;    // SubReferencia
                e.Row.Cells[18].Visible = false;    // IdProyecto
                e.Row.Cells[19].Visible = false;    // IdSeguros
                e.Row.Cells[20].Visible = false;    // IdTpoEvento
                e.Row.Cells[21].Visible = false;    // IdTpoProyecto

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[5].Visible = false;     // Num.Reporte
                e.Row.Cells[10].Visible = false;    // Nom.Actor
                e.Row.Cells[11].Visible = false;    // Nom.Demandado
             // e.Row.Cells[12].Visible = false;    // NomAjustador
             // e.Row.Cells[13].Visible = false;    // Resp_Medico
             // e.Row.Cells[14].Visible = false;    // Resp_Administrativo
                e.Row.Cells[15].Visible = false;    // IdStatus
                e.Row.Cells[16].Visible = false;    // Referencia
                e.Row.Cells[17].Visible = false;    // SubReferencia
                e.Row.Cells[18].Visible = false;    // IdProyecto
                e.Row.Cells[19].Visible = false;    // IdSeguros
                e.Row.Cells[20].Visible = false;    // IdTpoEvento
                e.Row.Cells[21].Visible = false;    // IdTpoProyecto
            }
        }

        protected void BtnAltaAsunto_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwGM_Alta_Asunto.aspx", true);
        }

        protected void GrdAlta_Asunto_PreRender(object sender, EventArgs e)
        {
            // Ajustar el estilo de las filas en general antes de renderizar
            foreach (GridViewRow row in GrdAlta_Asunto.Rows)
            {
                row.Style["height"] = "20px"; // Establecer la altura de las filas
            }

        }

        protected void BtnExportarExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string fechaActual = DateTime.Now.ToString("ddMMyy");
                string fileName = "Reporte_AltaAsunto";
                string extension = "xlsx";

                string nombreArchivoCompleto = $"{fileName}_{fechaActual}.{extension}";

                string filePath = Server.MapPath("~/itnowstorage/" + nombreArchivoCompleto);

                string IdColumna = ddlColumnas.SelectedValue;

                if (IdColumna == "0" && TxtRef.Text == "")
                {
                    ExportToExcel(filePath, "*", string.Empty);
                }
                else
                {
                    if (IdColumna == "0")
                    {
                        LblMessage.Text = "Seleccionar Columna, donde hacer la busqueda";
                        mpeMensaje.Show();
                        return;
                    }

                    ExportToExcel(filePath, string.Empty, IdColumna);
                }


                // Eliminar el archivo después de descargarlo
                File.Delete(filePath);

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void ExportToExcel(string filePath, string sValor, string sIdColumna)
        {
            try
            {

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

                int iTpoProyecto = 0;

                // Tipo de Consulta Asuntos / Proyectos
                if (rbProyectos.Checked)
                {
                    iTpoProyecto = 1;
                }

                string fechaActual = DateTime.Now.ToString("ddMMyy");
                string fileName = "Reporte_AltaAsunto";
                string extension = "xlsx";

                string nombreArchivoCompleto = $"{fileName}_{fechaActual}.{extension}";

                // Establecer la propiedad LicenseContext
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                if (sValor == string.Empty)
                {

                    // Consulta MySQL
                    strQuery = "SELECT CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END as Referencia, " +
                               "       t0.NumSiniestro, t0.NumPoliza, t0.NumReporte, t0.Fecha_Asignacion, t0.Hora_Asignacion, t0.NomActor, t0.NomDemandado, t0.NomAjustador, " +
                               "       t1.Descripcion as Tpo_Asunto, " +
                               "       CASE WHEN t0.IdProyecto = 0 THEN 'NINGUNO' ELSE t5.Descripcion END as IdProyecto, " +
                               "       CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END as Seguro_Cia, " +
                               "       t3.Descripcion as Resp_Tecnico, t4.Descripcion as Resp_Administrativo, " +
                               "       CASE WHEN t0.IdEstatusCaso = 1 THEN 'Hospitalizado' WHEN t0.IdEstatusCaso = 2 THEN 'En Seguimiento' WHEN t0.IdEstatusCaso = 3 THEN 'Alta / Cerrado' END AS Estatus_Caso " +
                               "  FROM ITM_73 t0 " +
                               "  JOIN ITM_11 t1 ON t0.IdTpoEvento = t1.IdTpoEvento " +
                               "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                               "  LEFT JOIN ITM_12 t3 ON t0.IdRespMedico = t3.IdRespMedico" +
                               "  LEFT JOIN ITM_14 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                               "  LEFT JOIN ITM_26 t5 ON t0.IdProyecto = t5.IdProyecto AND t0.IdSeguros = t5.IdCliente " +
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
                    strQuery = "SELECT CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END as Referencia, " +
                                "       t0.NumSiniestro, t0.NumPoliza, t0.NumReporte, t0.Fecha_Asignacion, t0.Hora_Asignacion, t0.NomActor, t0.NomDemandado, t0.NomAjustador, " +
                                "       t1.Descripcion as Tpo_Asunto, " +
                                "       CASE WHEN t0.IdProyecto = 0 THEN 'NINGUNO' ELSE t5.Descripcion END as IdProyecto, " +
                                "       CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END as Seguro_Cia, " +
                                "       t3.Descripcion as Resp_Tecnico, t4.Descripcion as Resp_Administrativo, " +
                                "       CASE WHEN t0.IdEstatusCaso = 1 THEN 'Hospitalizado' WHEN t0.IdEstatusCaso = 2 THEN 'En Seguimiento' WHEN t0.IdEstatusCaso = 3 THEN 'Alta / Cerrado' END AS Estatus_Caso " +
                                "  FROM ITM_73 t0 " +
                                "  JOIN ITM_11 t1 ON t0.IdTpoEvento = t1.IdTpoEvento " +
                                "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                                "  LEFT JOIN ITM_12 t3 ON t0.IdRespMedico = t3.IdRespMedico " +
                                "  LEFT JOIN ITM_14 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                                "  LEFT JOIN ITM_26 t5 ON t0.IdProyecto = t5.IdProyecto AND t0.IdSeguros = t5.IdCliente " +
                                " WHERE t0.IdStatus IN (1) AND t0.IdTpoProyecto = " + iTpoProyecto + " " +
                                " ORDER BY t0.IdAsunto DESC LIMIT 100 ";
                }

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                // Exportar a Excel con EPPlus
                using (ExcelPackage pck = new ExcelPackage())
                {
                    // Crear hoja de Excel
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Datos");

                    // Llenar la hoja con los datos de la DataTable
                    ws.Cells["A1"].LoadFromDataTable(dt, true);

                    // Guardar el archivo Excel
                    byte[] excelFile = pck.GetAsByteArray();

                    //string fileName = "Reporte_AltaAsunto.xlsx";
                    //File.WriteAllBytes(Server.MapPath("~/itnowstorage/" + fileName), excelFile);

                    File.WriteAllBytes(filePath, excelFile);
                }

                dbConn.Close();

                // Descargar el archivo
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", $"attachment; filename={nombreArchivoCompleto}");
                Response.TransmitFile(filePath);
                Response.End();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void ImgBusReference_Click(object sender, ImageClickEventArgs e)
        {
            string IdColumna = ddlColumnas.SelectedValue;

            if (IdColumna == "0" && TxtRef.Text == "")
            {
                GetAltaAsunto("*", string.Empty);
            }
            else
            {
                if (IdColumna == "0")
                {
                    LblMessage.Text = "Seleccionar Columna, donde hacer la busqueda";
                    mpeMensaje.Show();
                    return;
                }

                GetAltaAsunto(string.Empty, IdColumna);
            }
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

                case "IdTpoEvento":

                    GetTpoAsunto();

                    LblFiltros.Text = "Tipo de Evento";
                    LblFiltros.Visible = true;
                    ddlFiltros.Visible = true;

                    TxtRef.Text = string.Empty;

                    break;

                case "IdRespMedico":

                    GetResponsableTec();

                    LblFiltros.Text = "Responsable Médico";
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

        protected void Agregar_Sub_Referencia(string sReferencia)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserLogon; //LblUsuario.Text;

                // Insertar registro tabla (ITM_70)
                string strQuery = "INSERT INTO ITM_73 (Referencia, SubReferencia, NumSiniestro, NumPoliza, NumReporte, Referencia_Anterior, IdSeguros, IdTpoAsunto, IdProyecto, IdRegimen, " +
                                  "                   Fecha_Asignacion, NomCliente, NomActor, NomDemandado, NomAsegurado, IdRespTecnico, IdRespAdministrativo, IdConclusion, IdTpoProyecto, IdTpoJuicio, Id_Usuario, IdStatus) " +
                                  "SELECT Referencia, SubReferencia + 1, NumSiniestro, NumPoliza, NumReporte, Referencia_Anterior, IdSeguros, IdTpoAsunto, IdProyecto, IdRegimen, " +
                                  "    Fecha_Asignacion, NomCliente, NomActor, NomDemandado, NomAsegurado, IdRespTecnico, IdRespAdministrativo, IdConclusion, 0, " + ddlProcedimiento.SelectedValue + ", '" + sUsuario + "', 1" +
                                  "  FROM ITM_73" +
                                  " WHERE Referencia = '" + sReferencia + "'" +
                                  " ORDER BY SubReferencia DESC LIMIT 1 ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage_1.Text = "Se agrego sub-referencia, correctamente";
                mpeMensaje_1.Show();

                ddlProcedimiento.SelectedIndex = 0;

                BtnAceptar.Visible = false;
                BtnCancelar.Visible = false;
                BtnCerrar.Visible = true;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        protected void Eliminar_Sub_Referencia(string sReferencia, int iSubReferencia)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName; //LblUsuario.Text;

                // Eliminar registro tabla (ITM_70)
                string strQuery = "DELETE FROM ITM_70 " +
                                  " WHERE Referencia = '" + sReferencia + "'" +
                                  "   AND SubReferencia = " + iSubReferencia + "";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage_1.Text = "Se elimino sub-referencia, correctamente";
                mpeMensaje_1.Show();

                BtnAceptar.Visible = false;
                BtnCancelar.Visible = false;
                BtnCerrar.Visible = true;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            int index = Variables.wRenglon;

            // string sReferencia = Server.HtmlDecode(Convert.ToString(GrdAlta_Asunto.Rows[index].Cells[16].Text));
            // int sSubReferencia = Convert.ToInt32(GrdAlta_Asunto.Rows[index].Cells[17].Text);

            int IdentificadorBtn = Convert.ToInt32(Variables.wIdentificadorBtn);

            if (IdentificadorBtn == 0)
            {
                // Eliminar_Sub_Referencia(sReferencia, sSubReferencia);
            }
            else if (IdentificadorBtn == 1)
            {
                ddlProcedimiento.Style["display"] = "none";

                // Agregar_Sub_Referencia(sReferencia);
            }

            GetAltaAsunto("*", string.Empty);
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void ImgSubRef_Del_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);

            int index = row.RowIndex;
            int sSubReferencia = Convert.ToInt32(GrdAlta_Asunto.Rows[index].Cells[16].Text);

            Variables.wRenglon = row.RowIndex;
            Variables.wIdentificadorBtn = 0;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            if (sSubReferencia > 0)
            {
                LblMessage_1.Text = "¿Desea eliminar sub-referencia?";
                mpeMensaje_1.Show();
            }
            else
            {
                BtnAceptar.Visible = false;
                BtnCancelar.Visible = false;
                BtnCerrar.Visible = true;

                LblMessage_1.Text = "El registro que desea eliminar no es una sub-referencia";
                mpeMensaje_1.Show();
            }



        }

        protected void ImgSubRef_Add_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            Variables.wRenglon = row.RowIndex;
            Variables.wIdentificadorBtn = 1;

            int iIdTpoEvento = Convert.ToInt32(GrdAlta_Asunto.Rows[Variables.wRenglon].Cells[19].Text);

            // inicializar control tipo de procedimiento
            ddlProcedimiento.SelectedIndex = 0;

            // LITIGIO = 4
            if (iIdTpoEvento == 4)
            {
                ddlProcedimiento.Style["display"] = "block";
            }
            else
            {
                ddlProcedimiento.Style["display"] = "none";
            }

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            LblMessage_1.Text = "¿Desea agregar sub-referencia?";
            mpeMensaje_1.Show();

        }

        protected void ImgCheckList_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);

            // int index = row.RowIndex;

            // string sReferencia = Server.HtmlDecode(Convert.ToString(GrdAlta_Asunto.Rows[index].Cells[16].Text));
            // int iSubReferencia = Convert.ToInt32(GrdAlta_Asunto.Rows[index].Cells[17].Text);

            // Response.Redirect("fwDocument_Notebook.aspx?Ref=" + sReferencia + "&SubRef=" + iSubReferencia + "&Create=" + "0", true);

        }

        protected void ddlFiltros_SelectedIndexChanged(object sender, EventArgs e)
        {
            string IdColumna = ddlColumnas.SelectedValue;

            GetAltaAsunto(string.Empty, IdColumna);

        }

        protected void BtnCerrar_New_Click(object sender, EventArgs e)
        {

        }

        protected void rbAsuntos_CheckedChanged(object sender, EventArgs e)
        {
            GetAltaAsunto("*", "*");
        }

        protected void rbProyectos_CheckedChanged(object sender, EventArgs e)
        {
            GetAltaAsunto("*", "*");
        }

        protected void ddlTpoJuicio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}