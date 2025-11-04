using System;
using System.IO;

using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

using System.Web.UI;
using System.Web.UI.WebControls;

using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace WebItNow_Peacock
{
    public partial class fwReporte_Alta_Asunto : System.Web.UI.Page
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

                    if(Variables.wPrivilegios == "2")
                    { 
                        BtnCargaDatosIA.Visible = true;
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
                // ddlProcedimiento.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
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
                                        " FROM ITM_74 " +
                                        " WHERE IdStatus = 1 ORDER BY IdFiltro";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlColumnas.DataSource = dt;

                ddlColumnas.DataValueField = "IdColumna";
                ddlColumnas.DataTextField = "Descripcion";

                ddlColumnas.DataBind();
                // ddlColumnas.Items.Insert(0, new ListItem("-- Seleccionar Columna --", "0"));
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
                                        " FROM ITM_69 " +
                                        " WHERE IdStatus = 1";

                DataTable dt = dbConn.ExecuteQuery(sqlQuery);

                ddlFiltros.DataSource = dt;

                ddlFiltros.DataValueField = "IdRespAdministrativo";
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

                ddlFiltros.DataSource = dt;

                ddlFiltros.DataValueField = "IdRespTecnico";
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

                    //strQuery = "SELECT t0.IdAsunto, t0.SubReferencia, CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END as Referencia_Sub, " +
                    //           "       t0.NumSiniestro, t0.NumPoliza, t0.NumReporte, t0.Referencia_Anterior, " +
                    //           "       STUFF(FORMAT(CAST(Fecha_Asignacion AS DATE), 'dd-MMM-yyyy'), 4, 1, UPPER(SUBSTRING(FORMAT(CAST(Fecha_Asignacion AS DATE), 'dd-MMM-yyyy'), 4, 1))) AS Fecha_Asignacion, " +
                    //           "       t0.NomActor, t0.NomDemandado, t0.NomAsegurado, " +
                    //           "       CASE WHEN t0.IdStatus = 1 THEN 'ABIERTO' ELSE 'CERRADO' END as IdStatus, t0.Referencia as Referencia, " +
                    //           "       t1.Descripcion as Tpo_Asunto, " +
                    //           "       CASE WHEN t0.IdProyecto = 0 THEN 'NINGUNO' ELSE t5.Descripcion END as NomProyecto, " +
                    //           "       CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END as Seguro_Cia, " +
                    //           "       t3.Descripcion as Resp_Tecnico, t4.Descripcion as Resp_Administrativo, " +
                    //           "       t0.IdProyecto, t0.IdSeguros, t0.IdTpoAsunto, t0.IdTpoProyecto " +
                    //           "  FROM ITM_70 t0 " +
                    //           "  JOIN ITM_66 t1 ON t0.IdTpoAsunto = t1.IdTpoAsunto " +
                    //           "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                    //           "  LEFT JOIN ITM_68 t3 ON t0.IdRespTecnico = t3.IdRespTecnico " +
                    //           "  LEFT JOIN ITM_69 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                    //           "  LEFT JOIN ITM_78 t5 ON t0.IdProyecto = t5.IdProyecto AND t0.IdSeguros = t5.IdCliente " +
                    //           " WHERE t0.IdTpoProyecto = " + iTpoProyecto + " ";

                    //           if (sIdColumna == "IdRespTecnico" || sIdColumna == "IdRespAdministrativo")
                    //            {
                    //                strQuery += $"  AND {campoBusqueda} = {valorBusqueda} ";
                    //            } else
                    //            {
                    //                strQuery += "   AND " + $"{campoBusqueda}" + " LIKE '%' + '" + $"{valorBusqueda}" + "' + '%' ";
                    //            }

                    //strQuery += " ORDER BY t0.IdAsunto DESC LIMIT 100";

                    // Consulta MySQL
                    strQuery = " SELECT t0.IdAsunto, t0.SubReferencia, " +
                               "    CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia " +
                               "     END AS Referencia_Sub, " +
                               "     t0.NumSiniestro, t0.NumPoliza, t0.NumReporte, t0.Referencia_Anterior, " +
                               " CONCAT( DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%d-'), " +
                               "   CASE DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%m') " +
                               "   WHEN '01' THEN 'Ene' WHEN '02' THEN 'Feb' WHEN '03' THEN 'Mar' " +
                               "   WHEN '04' THEN 'Abr' WHEN '05' THEN 'May' WHEN '06' THEN 'Jun' " +
                               "   WHEN '07' THEN 'Jul' WHEN '08' THEN 'Ago' WHEN '09' THEN 'Sep' " +
                               "   WHEN '10' THEN 'Oct' WHEN '11' THEN 'Nov' WHEN '12' THEN 'Dic' END, '-', " +
                               "   DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%Y') " +
                               "   ) AS Fecha_Asignacion, " +
                               "   t0.NomActor, t0.NomDemandado, t0.NomAsegurado, " +
                               "   CASE WHEN t0.IdStatus = 1 THEN 'ABIERTO' ELSE 'CERRADO' END AS IdStatus, " +
                               "   t0.Referencia AS Referencia, t1.Descripcion AS Tpo_Asunto, " +
                               "   CASE WHEN t0.IdProyecto = 0 THEN 'NINGUNO' ELSE t5.Descripcion END AS NomProyecto, " +
                               "   CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END AS Seguro_Cia, " +
                               "   t3.Descripcion AS Resp_Tecnico, t4.Descripcion AS Resp_Administrativo, t0.IdProyecto, " +
                               "   t0.IdSeguros, t0.IdTpoAsunto, t0.IdTpoProyecto " +
                               "  FROM ITM_70 t0 " +
                               "  JOIN ITM_66 t1 ON t0.IdTpoAsunto = t1.IdTpoAsunto " +
                               "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                               "  LEFT JOIN ITM_68 t3 ON t0.IdRespTecnico = t3.IdRespTecnico " +
                               "  LEFT JOIN ITM_69 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                               "  LEFT JOIN ITM_78 t5 ON t0.IdProyecto = t5.IdProyecto AND t0.IdSeguros = t5.IdCliente " +
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
                    //strQuery = "SELECT t0.IdAsunto, t0.SubReferencia, CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END as Referencia_Sub, " +
                    //           "       t0.NumSiniestro, t0.NumPoliza, t0.NumReporte, t0.Referencia_Anterior, " +
                    //           "       STUFF(FORMAT(CAST(Fecha_Asignacion AS DATE), 'dd-MMM-yyyy'), 4, 1, UPPER(SUBSTRING(FORMAT(CAST(Fecha_Asignacion AS DATE), 'dd-MMM-yyyy'), 4, 1))) AS Fecha_Asignacion, " +
                    //           "       t0.NomActor, t0.NomDemandado, t0.NomAsegurado, " +
                    //           "       CASE WHEN t0.IdStatus = 1 THEN 'ABIERTO' ELSE 'CERRADO' END as IdStatus, t0.Referencia as Referencia," +
                    //           "       t1.Descripcion as Tpo_Asunto, " +
                    //           "       CASE WHEN t0.IdProyecto = 0 THEN 'NINGUNO' ELSE t5.Descripcion END as NomProyecto, " +
                    //           "       CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END as Seguro_Cia, " +
                    //           "       t3.Descripcion as Resp_Tecnico, t4.Descripcion as Resp_Administrativo, " +
                    //           "       t0.IdProyecto, t0.IdSeguros, t0.IdTpoAsunto, t0.IdTpoProyecto " +
                    //           "  FROM ITM_70 t0 " +
                    //           "  JOIN ITM_66 t1 ON t0.IdTpoAsunto = t1.IdTpoAsunto " +
                    //           "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                    //           "  LEFT JOIN ITM_68 t3 ON t0.IdRespTecnico = t3.IdRespTecnico " +
                    //           "  LEFT JOIN ITM_69 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                    //           "  LEFT JOIN ITM_78 t5 ON t0.IdProyecto = t5.IdProyecto AND t0.IdSeguros = t5.IdCliente " +
                    //           " WHERE t0.IdStatus IN (1) AND t0.IdTpoProyecto = " + iTpoProyecto + " " +
                    //           " ORDER BY t0.IdAsunto DESC LIMIT 100";

                    strQuery = " SELECT t0.IdAsunto, t0.SubReferencia, " +
                               "    CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia " +
                               "     END AS Referencia_Sub, " +
                               "     t0.NumSiniestro, t0.NumPoliza, t0.NumReporte, t0.Referencia_Anterior, " +
                               " CONCAT( DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%d-'), " +
                               "   CASE DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%m') " +
                               "   WHEN '01' THEN 'Ene' WHEN '02' THEN 'Feb' WHEN '03' THEN 'Mar' " +
                               "   WHEN '04' THEN 'Abr' WHEN '05' THEN 'May' WHEN '06' THEN 'Jun' " +
                               "   WHEN '07' THEN 'Jul' WHEN '08' THEN 'Ago' WHEN '09' THEN 'Sep' " +
                               "   WHEN '10' THEN 'Oct' WHEN '11' THEN 'Nov' WHEN '12' THEN 'Dic' END, '-', " +
                               "   DATE_FORMAT(STR_TO_DATE(t0.Fecha_Asignacion, '%d/%m/%Y'), '%Y') " +
                               "   ) AS Fecha_Asignacion, " +
                               "   t0.NomActor, t0.NomDemandado, t0.NomAsegurado, " +
                               "   CASE WHEN t0.IdStatus = 1 THEN 'ABIERTO' ELSE 'CERRADO' END AS IdStatus, " +
                               "   t0.Referencia AS Referencia, t1.Descripcion AS Tpo_Asunto, " +
                               "   CASE WHEN t0.IdProyecto = 0 THEN 'NINGUNO' ELSE t5.Descripcion END AS NomProyecto, " +
                               "   CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END AS Seguro_Cia, " +
                               "   t3.Descripcion AS Resp_Tecnico, t4.Descripcion AS Resp_Administrativo, t0.IdProyecto, " +
                               "   t0.IdSeguros, t0.IdTpoAsunto, t0.IdTpoProyecto " +
                               "  FROM ITM_70 t0 " +
                               "  JOIN ITM_66 t1 ON t0.IdTpoAsunto = t1.IdTpoAsunto " +
                               "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                               "  LEFT JOIN ITM_68 t3 ON t0.IdRespTecnico = t3.IdRespTecnico " +
                               "  LEFT JOIN ITM_69 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                               "  LEFT JOIN ITM_78 t5 ON t0.IdProyecto = t5.IdProyecto AND t0.IdSeguros = t5.IdCliente " +
                               " WHERE t0.IdStatus IN (1) AND t0.IdTpoProyecto = " + iTpoProyecto + " " +
                               " ORDER BY t0.IdAsunto DESC LIMIT 100 ";

                }

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdAlta_Asunto.ShowHeaderWhenEmpty = true;
                    // GrdAlta_Asunto.EmptyDataText = "No hay resultados.";
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

                // Response.Redirect("fwDetalle_Asunto.aspx?Ref=" + sReferencia + "&SubRef=" + iSubReferencia +
                //                                        "&Proyecto=" + iIdProyecto + "&Seguro=" + sIdCliente + "&Asunto=" + iIdTpoAsunto, true);

                //Response.Redirect("fwGenerar_Formatos.aspx", true);

                if (iIdTpoAsunto == "4")
                {
                    // LITIGIO
                    Response.Redirect("fwBitacora_Litigio.aspx?Ref=" + sReferencia + "&SubRef=" + iSubReferencia +
                                                             "&Proyecto=" + iIdProyecto + "&Seguro=" + sIdCliente + "&Asunto=" + iIdTpoAsunto, true);
                } else
                {
                    // SIMPLE / COMPLEJO
                    Response.Redirect("fwBitacora_Asunto.aspx?Ref=" + sReferencia + "&SubRef=" + iSubReferencia +
                                                             "&Proyecto=" + iIdProyecto + "&Seguro=" + sIdCliente + "&Asunto=" + iIdTpoAsunto, true);
                }
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
                e.Row.Cells[9].Visible = false;     // Nom.Actor
                e.Row.Cells[10].Visible = false;    // Nom.Demandado
             // e.Row.Cells[11].Visible = false;    // NomAsegurado
             // e.Row.Cells[12].Visible = false;    // Resp_Tecnico
             // e.Row.Cells[13].Visible = false;    // Resp_Administrativo
                e.Row.Cells[14].Visible = false;    // Referencia_Anterior
                e.Row.Cells[15].Visible = false;    // IdStatus
                e.Row.Cells[16].Visible = false;    // Referencia
                e.Row.Cells[17].Visible = false;    // SubReferencia
                e.Row.Cells[18].Visible = false;    // IdProyecto
                e.Row.Cells[19].Visible = false;    // IdSeguros
                e.Row.Cells[20].Visible = false;    // IdTpoAsunto
                e.Row.Cells[21].Visible = false;    // IdTpoProyecto

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[5].Visible = false;     // Num.Reporte
                e.Row.Cells[9].Visible = false;     // Nom.Actor
                e.Row.Cells[10].Visible = false;    // Nom.Demandado
             // e.Row.Cells[11].Visible = false;    // NomAsegurado
             // e.Row.Cells[12].Visible = false;    // Resp_Tecnico
             // e.Row.Cells[13].Visible = false;    // Resp_Administrativo
                e.Row.Cells[14].Visible = false;    // Referencia_Anterior
                e.Row.Cells[15].Visible = false;    // IdStatus
                e.Row.Cells[16].Visible = false;    // Referencia
                e.Row.Cells[17].Visible = false;    // SubReferencia
                e.Row.Cells[18].Visible = false;    // IdProyecto
                e.Row.Cells[19].Visible = false;    // IdSeguros
                e.Row.Cells[20].Visible = false;    // IdTpoAsunto
                e.Row.Cells[21].Visible = false;    // IdTpoProyecto
            }
        }

        protected void BtnAltaAsunto_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwAlta_Asunto.aspx", true);
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
                               "       t0.NumSiniestro, t0.NumPoliza, t0.NumReporte, t0.Fecha_Asignacion, t0.NomActor, t0.NomDemandado, t0.NomAsegurado, " +
                               "       t1.Descripcion as Tpo_Asunto, " +
                               "       CASE WHEN t0.IdProyecto = 0 THEN 'NINGUNO' ELSE t5.Descripcion END as IdProyecto, " +
                               "       CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END as Seguro_Cia, " +
                               "       t3.Descripcion as Resp_Tecnico, t4.Descripcion as Resp_Administrativo " +
                               "  FROM ITM_70 t0 " +
                               "  JOIN ITM_66 t1 ON t0.IdTpoAsunto = t1.IdTpoAsunto " +
                               "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                               "  LEFT JOIN ITM_68 t3 ON t0.IdRespTecnico = t3.IdRespTecnico" +
                               "  LEFT JOIN ITM_69 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                               "  LEFT JOIN ITM_78 t5 ON t0.IdProyecto = t5.IdProyecto AND t0.IdSeguros = t5.IdCliente " +
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
                                "       t0.NumSiniestro, t0.NumPoliza, t0.NumReporte, t0.Fecha_Asignacion, t0.NomActor, t0.NomDemandado, t0.NomAsegurado, " +
                                "       t1.Descripcion as Tpo_Asunto, " +
                                "       CASE WHEN t0.IdProyecto = 0 THEN 'NINGUNO' ELSE t5.Descripcion END as IdProyecto, " +
                                "       CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END as Seguro_Cia, " +
                                "       t3.Descripcion as Resp_Tecnico, t4.Descripcion as Resp_Administrativo " +
                                "  FROM ITM_70 t0 " +
                                "  JOIN ITM_66 t1 ON t0.IdTpoAsunto = t1.IdTpoAsunto " +
                                "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                                "  LEFT JOIN ITM_68 t3 ON t0.IdRespTecnico = t3.IdRespTecnico " +
                                "  LEFT JOIN ITM_69 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                                "  LEFT JOIN ITM_78 t5 ON t0.IdProyecto = t5.IdProyecto AND t0.IdSeguros = t5.IdCliente " +
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

        protected void Agregar_Sub_Referencia(string sReferencia)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserLogon; //LblUsuario.Text;

                // Insertar registro tabla (ITM_70)
                string strQuery = "INSERT INTO ITM_70(Referencia, SubReferencia, NumSiniestro, NumPoliza, NumReporte, Referencia_Anterior, IdSeguros, IdTpoAsunto, IdProyecto, IdRegimen, " +
                                  "                   Fecha_Asignacion, NomCliente, NomActor, NomDemandado, NomAsegurado, IdRespTecnico, IdRespAdministrativo, IdConclusion, IdTpoProyecto, IdTpoJuicio, Id_Usuario, IdStatus) " +
                                  "SELECT Referencia, SubReferencia + 1, NumSiniestro, NumPoliza, NumReporte, Referencia_Anterior, IdSeguros, IdTpoAsunto, IdProyecto, IdRegimen, " +
                                  "    Fecha_Asignacion, NomCliente, NomActor, NomDemandado, NomAsegurado, IdRespTecnico, IdRespAdministrativo, IdConclusion, 0, " + ddlProcedimiento.SelectedValue + ", '" + sUsuario + "', 1" +
                                  "  FROM ITM_70" +
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

            string sReferencia = Server.HtmlDecode(Convert.ToString(GrdAlta_Asunto.Rows[index].Cells[16].Text));
            int sSubReferencia = Convert.ToInt32(GrdAlta_Asunto.Rows[index].Cells[17].Text);

            int IdentificadorBtn = Convert.ToInt32(Variables.wIdentificadorBtn);

            if (IdentificadorBtn == 0)
            {
                Eliminar_Sub_Referencia(sReferencia, sSubReferencia);
            }
            else if (IdentificadorBtn == 1)
            {
                ddlProcedimiento.Style["display"] = "none";

                Agregar_Sub_Referencia(sReferencia);
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
            int sSubReferencia = Convert.ToInt32(GrdAlta_Asunto.Rows[index].Cells[17].Text);

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

            int iIdTpoAsunto = Convert.ToInt32(GrdAlta_Asunto.Rows[Variables.wRenglon].Cells[20].Text);

            // inicializar control tipo de procedimiento
            ddlProcedimiento.SelectedIndex = 0;

            // LITIGIO = 4
            if (iIdTpoAsunto == 4)
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

            int index = row.RowIndex;

            string sReferencia = Server.HtmlDecode(Convert.ToString(GrdAlta_Asunto.Rows[index].Cells[16].Text));
            int iSubReferencia = Convert.ToInt32(GrdAlta_Asunto.Rows[index].Cells[17].Text);

            //int iIdProyecto = Convert.ToInt32(GrdAlta_Asunto.Rows[index].Cells[18].Text);
            //string sIdCliente = Server.HtmlDecode(Convert.ToString(GrdAlta_Asunto.Rows[index].Cells[19].Text));
            //int iIdTpoAsunto = Convert.ToInt32(GrdAlta_Asunto.Rows[index].Cells[20].Text);

            //Response.Redirect("fwDeliver_Document.aspx?Ref=" + sReferencia + "&SubRef=" + iSubReferencia +
            //                                         "&Proyecto=" + iIdProyecto + "&Seguro=" + sIdCliente + "&Asunto=" + iIdTpoAsunto, true);

            Response.Redirect("fwDocument_Notebook.aspx?Ref=" + sReferencia + "&SubRef=" + iSubReferencia + "&Create=" + "0", true);

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

        protected void BtnCargaDatosIA_Click(object sender, EventArgs e)
        {
            mpeNewEnvio.Show();
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            string idUsuarioFijo = Variables.wUserLogon;    // valor fijo para Id_Usuario
            int idStatusFijo = 1;                           // valor fijo para IdStatus

            LblMessageIA.Text = string.Empty;

            HttpPostedFile postedFile = Request.Files["ctl00$MainContent$oFile"];

            if (postedFile != null && postedFile.ContentLength > 0)
            {
                string extension = System.IO.Path.GetExtension(oFile.PostedFile.FileName).ToLower();
                if (extension != ".xlsx")
                {
                    LblMessage.Text = "Solo se permiten archivos .xlsx";
                    mpeMensaje.Show();

                    return;
                }

                string strFileName = Path.GetFileName(postedFile.FileName);
                string rutaExcel = Server.MapPath("~/ArchivoTemporal/" + strFileName);
               
                // Guardar archivo temporal
                postedFile.SaveAs(rutaExcel);

                // Definir estructura esperada
                var estructuraEsperada = new Dictionary<string, List<string>>()
                {
                    { "INF_POLIZA",  new List<string> { "referencia", "sub_referencia", "producto", "num_poliza", "num_endoso", "fecha_emision", "fecha_inicio_vig", "hora_inicio_vig", "fecha_fin_vig", "hora_fin_vig",     
                                                        "num_certificado", "moneda", "plan", "plazo", "canal", "num_renovacion", "giro", "forma_pago", "prima_anual", "prima_neta", "gastos_exp", "recargo_fracc", "por_iva", "monto_iva",
                                                        "prima_total", "prima_forma_pago", "pagos_subs" } },
                    { "CONTRATANTE", new List<string> { "referencia", "sub_referencia", "nombre_contrata", "rfc", "tipo_contrata", "email", "tel_part", "tel_cel", "calle", "num_ext", "num_int",
                                                        "colonia", "edo", "municipio_alcaldia", "cp", "otros" } },
                    { "ASEGURADO",   new List<string> { "referencia", "sub_referencia", "nombre_asegurado", "rfc_asegurado", "tipo_asegurado", "calle", "num_ext", "num_int", "colonia", "edo",
                                                        "municipio_alcaldia", "cp", "otros" } },
                    { "REG_POLIZA",  new List<string> { "referencia", "sub_referencia", "reg_ppaq", "fecha_ppaq", "reg_resp", "fecha_resp", "reg_cgen", "fecha_cgen", "reg_badi", "fecha_badi",
                                                        "reg_condusef", "fecha_condusef" } },
                    { "UBICACION",   new List<string> { "referencia", "sub_referencia", "detalle_asegurado", "tipo_techo", "tipo_muros", "num_pisos", "piso_inmueble" } }
                };


                // Validar estructura de TODAS las hojas
                string mensajeError;
                bool esValido = ValidarEstructuraExcel(rutaExcel, estructuraEsperada, out mensajeError);

                if (!esValido)
                {
                    // LblMessage.Text = "Error en estructura:<br/>" + mensajeError;
                    LblMessageIA.Text = "<pre style='text-align:left; font-size:inherit; font-family:inherit; white-space:pre-wrap;'>"
                                      + "Error en estructura:\r\n\r\n" + mensajeError + "</pre>";

                    mpeMensaje_2.Show();

                    // Eliminar archivo inválido
                    if (File.Exists(rutaExcel))
                    {
                        File.Delete(rutaExcel);
                    }

                    return; // Detener flujo
                }

                // Procesar archivo porque es válido
                string rutaLog = Server.MapPath("~/ArchivoTemporal/ErroresCargaExcel.csv");

                try
                {
                    CargarExcel_AI_BaseDatos(rutaExcel, "ITM_70_3", idUsuarioFijo, idStatusFijo, rutaLog, 500);

                    // Mensaje de confirmación
                    LblMessage.Text = "Archivo cargado y procesado correctamente.";
                    mpeMensaje.Show();
                }
                catch (Exception ex)
                {
                    LblMessage.Text = "Ocurrió un error al procesar el archivo: " + ex.Message;
                    mpeMensaje.Show();
                }
                finally
                {
                    // Limpiar archivo temporal después de procesar
                    if (File.Exists(rutaExcel))
                    {
                        File.Delete(rutaExcel);
                    }
                }
            }
            else
            {
                LblMessage.Text = "Por favor, selecciona un archivo antes de continuar.";
                mpeMensaje.Show();
            }
        }


        protected void BtnCloseIA_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrarIA_Click(object sender, EventArgs e)
        {

        }

        public void CargarExcel_AI_BaseDatos(string rutaExcel, string nombreTablaDefault, string idUsuarioFijo, int idStatusFijo, 
                                                              string rutaLogErrores, int batchSize = 1000)
        {
            FileInfo archivo = new FileInfo(rutaExcel);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(archivo))
            {
                foreach (var hoja in package.Workbook.Worksheets)
                {
                    try
                    {
                        // obtener tabla y mapa para la hoja
                        string nombreTabla = ObtenerNombreTabla(hoja.Name);
                        if (string.IsNullOrEmpty(nombreTabla))
                        {
                            string logLinea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{hoja.Name},0,Hoja no mapeada a ninguna tabla";
                            File.AppendAllLines(rutaLogErrores, new List<string> { logLinea });
                            continue;
                        }

                        var mapaColumnas = ObtenerMapaColumnas(hoja.Name);
                        if (mapaColumnas == null || mapaColumnas.Count == 0)
                        {
                            string logLinea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{hoja.Name},0,Hoja sin mapa de columnas configurado";
                            File.AppendAllLines(rutaLogErrores, new List<string> { logLinea });
                            continue;
                        }

                        DataTable dt = LeerExcelADataTable(rutaExcel, hoja.Name);
                        dt.TableName = hoja.Name;

                        InsertarDatosHoja(dt, nombreTabla, mapaColumnas, idUsuarioFijo, idStatusFijo, rutaLogErrores, batchSize);

                        Console.WriteLine($"Hoja '{hoja.Name}' procesada correctamente en la tabla '{nombreTabla}'.");
                    }
                    catch (Exception exHoja)
                    {
                        string logLinea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{hoja.Name},0,{exHoja.Message}";
                        File.AppendAllLines(rutaLogErrores, new List<string> { logLinea });
                    }
                }
            }

        }

        public DataTable LeerExcelADataTable(string rutaExcel, string nombreHoja)
        {
            var dt = new DataTable();

            FileInfo archivo = new FileInfo(rutaExcel);
            using (ExcelPackage package = new ExcelPackage(archivo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[nombreHoja];
                if (worksheet == null)
                    throw new Exception($"La hoja '{nombreHoja}' no existe en el archivo Excel.");

                // Leer encabezados
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    dt.Columns.Add(worksheet.Cells[1, col].Text.Trim());
                }

                // Leer filas
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    DataRow dr = dt.NewRow();
                    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                    {
                        dr[col - 1] = worksheet.Cells[row, col].Text.Trim();
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }


        public void InsertarDatosHoja(DataTable dt, string nombreTabla, Dictionary<string, (string ColumnaBD, string Tipo, int Longitud)> mapaColumnas,
                                                                                  string idUsuarioFijo, int idStatusFijo, string rutaLogErrores, int batchSize = 1000)
        {
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            MySqlCommand cmd = new MySqlCommand();

            try
            {
                dbConn.Open();
                cmd.Connection = dbConn.Connection;
                int totalFilas = dt.Rows.Count;

                if (!File.Exists(rutaLogErrores))
                    File.WriteAllText(rutaLogErrores, "FechaHora, Hoja, Fila,MensajeError" + Environment.NewLine);

                // preparar mapa insensible a mayúsculas: claveMapa -> nombreColumnaEnDT (o null si no existe)
                var excelToDtCol = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var key in mapaColumnas.Keys)
                {
                    var found = dt.Columns.Cast<DataColumn>()
                                  .FirstOrDefault(c => string.Equals(c.ColumnName.Trim(), key.Trim(), StringComparison.OrdinalIgnoreCase));
                    excelToDtCol[key] = found != null ? found.ColumnName : null;
                }

                for (int start = 0; start < totalFilas; start += batchSize)
                {
                    int end = Math.Min(start + batchSize, totalFilas);
                    var batchRows = dt.AsEnumerable().Skip(start).Take(end - start);

                    // columnas BD en orden (según mapa)
                    var columnasTabla = mapaColumnas.Values.Select(v => v.ColumnaBD).ToList();
                    columnasTabla.Add("Id_Usuario");
                    columnasTabla.Add("IdStatus");

                    List<string> listaValores = new List<string>();
                    cmd.Parameters.Clear();
                    int paramIndex = 0;

                    foreach (DataRow row in batchRows)
                    {
                        try
                        {
                            List<string> valoresFila = new List<string>();

                            foreach (var mapeo in mapaColumnas)
                            {
                                string excelKey = mapeo.Key;
                                var meta = mapeo.Value;
                                string dtColName = excelToDtCol.ContainsKey(excelKey) ? excelToDtCol[excelKey] : null;
                                string raw = dtColName != null ? (row[dtColName]?.ToString() ?? "") : "";

                                // Procesar según tipo
                                if (meta.Tipo.StartsWith("varchar", StringComparison.OrdinalIgnoreCase) ||
                                    meta.Tipo.Equals("string", StringComparison.OrdinalIgnoreCase))
                                {
                                    string valor = raw;
                                    if (meta.Longitud > 0 && valor.Length > meta.Longitud)
                                    {
                                        // truncar y registrar
                                        string logTrunc = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{dt.TableName},{dt.Rows.IndexOf(row) + 2},Valor truncado en columna {excelKey} a {meta.Longitud} caracteres";
                                        File.AppendAllLines(rutaLogErrores, new List<string> { logTrunc });
                                        valor = valor.Substring(0, meta.Longitud);
                                    }
                                    cmd.Parameters.AddWithValue($"@p{paramIndex}", valor);
                                    valoresFila.Add($"@p{paramIndex}");
                                    paramIndex++;
                                }
                                else if (meta.Tipo.Equals("int", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (string.IsNullOrWhiteSpace(raw))
                                    {
                                        cmd.Parameters.AddWithValue($"@p{paramIndex}", DBNull.Value);
                                        valoresFila.Add($"@p{paramIndex}");
                                        paramIndex++;
                                    }
                                    else if (int.TryParse(raw, out int numero))
                                    {
                                        cmd.Parameters.AddWithValue($"@p{paramIndex}", numero);
                                        valoresFila.Add($"@p{paramIndex}");
                                        paramIndex++;
                                    }
                                    else
                                    {
                                        // no es numérico: registrar y poner NULL
                                        string logLinea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{dt.TableName},{dt.Rows.IndexOf(row) + 2},Valor no numérico en {excelKey}: '{raw}'";
                                        File.AppendAllLines(rutaLogErrores, new List<string> { logLinea });
                                        cmd.Parameters.AddWithValue($"@p{paramIndex}", DBNull.Value);
                                        valoresFila.Add($"@p{paramIndex}");
                                        paramIndex++;
                                    }
                                }
                                else if (meta.Tipo.StartsWith("date", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (string.IsNullOrWhiteSpace(raw))
                                    {
                                        cmd.Parameters.AddWithValue($"@p{paramIndex}", DBNull.Value);
                                        valoresFila.Add($"@p{paramIndex}");
                                        paramIndex++;
                                    }
                                    else if (DateTime.TryParse(raw, out DateTime fecha))
                                    {
                                        cmd.Parameters.AddWithValue($"@p{paramIndex}", fecha.ToString("yyyy-MM-dd"));
                                        valoresFila.Add($"@p{paramIndex}");
                                        paramIndex++;
                                    }
                                    else
                                    {
                                        string logLinea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{dt.TableName},{dt.Rows.IndexOf(row) + 2},Fecha inválida en {excelKey}: '{raw}'";
                                        File.AppendAllLines(rutaLogErrores, new List<string> { logLinea });
                                        cmd.Parameters.AddWithValue($"@p{paramIndex}", DBNull.Value);
                                        valoresFila.Add($"@p{paramIndex}");
                                        paramIndex++;
                                    }
                                }
                                else
                                {
                                    // fallback: tratar como string
                                    string valor = raw;
                                    if (meta.Longitud > 0 && valor.Length > meta.Longitud)
                                        valor = valor.Substring(0, meta.Longitud);

                                    cmd.Parameters.AddWithValue($"@p{paramIndex}", valor);
                                    valoresFila.Add($"@p{paramIndex}");
                                    paramIndex++;
                                }
                            } // end foreach mapeo

                            // agregar Id_Usuario e IdStatus
                            cmd.Parameters.AddWithValue($"@p{paramIndex}", idUsuarioFijo);
                            valoresFila.Add($"@p{paramIndex}");
                            paramIndex++;

                            cmd.Parameters.AddWithValue($"@p{paramIndex}", idStatusFijo);
                            valoresFila.Add($"@p{paramIndex}");
                            paramIndex++;

                            listaValores.Add("(" + string.Join(",", valoresFila) + ")");
                        }
                        catch (Exception exFila)
                        {
                            string logLinea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{dt.TableName},{dt.Rows.IndexOf(row) + 2},{exFila.Message}";
                            File.AppendAllLines(rutaLogErrores, new List<string> { logLinea });
                            // continúa con la siguiente fila
                        }
                    } // end foreach row

                    if (listaValores.Count > 0)
                    {
                        var colsBackticked = columnasTabla.Select(c => $"`{c}`");

                        string sql = $"INSERT IGNORE INTO `{nombreTabla}` ({string.Join(",", colsBackticked)}) VALUES {string.Join(",", listaValores)}";

                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                    }

                } // end batches
            }
            catch (Exception ex)
            {
                string logLinea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{dt.TableName},0,{ex.Message}";
                File.AppendAllLines(rutaLogErrores, new List<string> { logLinea });
                throw new Exception("Error al insertar los datos en MySQL (batch): " + ex.Message, ex);
            }
            finally
            {
                dbConn.Close();
                cmd.Dispose();
            }
        }

        private string ObtenerNombreTabla(string nombreHoja)
        {
            // Relaciona nombre de hoja con la tabla MySQL
            switch (nombreHoja)
            {
                case "INF_POLIZA": return "ITM_70_3";
                case "CONTRATANTE": return "ITM_70_3_1";
                case "ASEGURADO": return "ITM_70_3_2";
                case "UBICACION": return "ITM_70_3_3";
                case "REG_POLIZA": return "ITM_70_3_5";

                default: return null;
            }
        }


        private Dictionary<string, (string ColumnaBD, string Tipo, int Longitud)> ObtenerMapaColumnas(string nombreHoja)
        {
            switch (nombreHoja)
            {
                case "INF_POLIZA":
                    return new Dictionary<string, (string ColumnaBD, string Tipo, int Longitud)>()
            {
                { "referencia", ("Referencia", "varchar", 15) },
                { "sub_referencia", ("SubReferencia", "int", 0) },
                { "producto", ("TpoProducto", "varchar", 25) },
                { "fecha_emision", ("Fec_Emision", "varchar", 10) },
                { "fecha_inicio_vig", ("Fec_IniVigencia", "varchar", 10) },
                { "fecha_fin_vig", ("Fec_FinVigencia", "varchar", 10) },
                { "num_certificado", ("Num_Certificado", "varchar", 25) },
                { "moneda", ("TpoMoneda", "varchar", 25) },
                { "plan", ("TpoPlan", "varchar", 25) },
                { "plazo", ("Plazo", "varchar", 25) },
                { "canal", ("CanalVentas", "varchar", 50) },
                { "num_renovacion", ("Num_Renovacion", "varchar", 25) },
                { "giro", ("Giro", "varchar", 50) },
                { "forma_pago", ("FormaPago", "varchar", 20) },
                { "prima_anual", ("PrimaTotalAnual", "varchar", 20) },
                { "prima_neta", ("PrimaNeta", "varchar", 20) },
                { "gastos_exp", ("GastosExpedicion", "varchar", 20) },
                { "recargo_fracc", ("RecargoPago", "varchar", 20) },
                { "por_iva", ("PrimaTotal", "varchar", 20) },
                { "prima_forma_pago", ("PrimaFormaPago", "varchar", 20) },
                { "pagos_subs", ("PagoSubsecuente", "varchar", 20) }

                // agrega todos los campos que correspondan
            };

                case "CONTRATANTE":
                    return new Dictionary<string, (string ColumnaBD, string Tipo, int Longitud)>()
            {
                { "referencia", ("Referencia", "varchar", 15) },
                { "sub_referencia", ("SubReferencia", "int", 0) },
                { "nombre_contrata", ("Nom_Contratante", "varchar", 80) },
                { "rfc", ("RFC_Contratante", "varchar", 15) },
                { "tipo_contrata", ("Tipo_Contratante", "varchar", 50) },
                { "email", ("Email_Contratante", "varchar", 100) },
                { "tel_part", ("Tel1_Contratante", "varchar", 15) },
                { "tel_cel", ("Tel2_Contratante", "varchar", 15) },
                { "calle", ("Calle_Contratante", "varchar", 50) },
                { "num_ext", ("Num_Exterior", "varchar", 25) },
                { "num_int", ("Num_Interior", "varchar", 25) },
                { "colonia", ("Colonia_Contratante", "varchar", 50) },
                { "edo", ("Estado_Contratante", "varchar", 5) },
                { "municipio_alcaldia", ("Municipio_Contratante", "varchar", 5) },
                { "cp", ("CPostal_Contratante", "varchar", 5) },
                { "otros", ("Otros_Contratante", "varchar", 25) }
            };

                case "ASEGURADO":
                    return new Dictionary<string, (string ColumnaBD, string Tipo, int Longitud)>()
            {
                { "referencia", ("Referencia", "varchar", 15) },
                { "sub_referencia", ("SubReferencia", "int", 0) },
                { "nombre_asegurado", ("Nom_Asegurado_1", "varchar", 80) },
                { "rfc_asegurado", ("RFC_Asegurado_1", "varchar", 15) },
                { "tipo_asegurado", ("Tipo_Asegurado_1", "varchar", 50) },
                { "calle", ("Calle_Asegurado_1", "varchar", 50) },
                { "num_ext", ("Num_Exterior_Asegurado_1", "varchar", 25) },
                { "num_int", ("Num_Interior_Asegurado_1", "varchar", 25) },
                { "colonia", ("Colonia_Asegurado_1", "varchar", 50) },
                { "edo", ("Estado_Asegurado_1", "varchar", 5) },
                { "municipio_alcaldia", ("Municipio_Asegurado_1", "varchar", 5) },
                { "cp", ("CPostal_Asegurado_1", "varchar", 5) },
                { "otros", ("Otros_Asegurado_1", "varchar", 25) }
            };

                case "REG_POLIZA":
                    return new Dictionary<string, (string ColumnaBD, string Tipo, int Longitud)>()
            {
                { "referencia", ("Referencia", "varchar", 15) },
                { "sub_referencia", ("SubReferencia", "int", 0) },
                { "reg_ppaq", ("RegistroPPAQ", "varchar", 30) },
                { "fecha_ppaq", ("FecRegistroPPAQ", "varchar", 10) },
                { "reg_resp", ("RegistroRESP", "varchar", 30) },
                { "fecha_resp", ("FecRegistroRESP", "varchar", 10) },
                { "reg_cgen", ("RegistroCGEN", "varchar", 30) },
                { "fecha_cgen", ("FecRegistroCGEN", "varchar", 10) },
                { "reg_badi", ("RegistroBADI", "varchar", 30) },
                { "fecha_badi", ("FecRegistroBADI", "varchar", 10) },
                { "reg_condusef", ("RegistroCONDUSEF", "varchar", 30) },
                { "fecha_condusef", ("FecRegistroCONDUSEF", "varchar", 10) }
            };
                case "UBICACION":
                    return new Dictionary<string, (string ColumnaBD, string Tipo, int Longitud)>()
            {
                { "referencia", ("Referencia", "varchar", 15) },
                { "sub_referencia", ("SubReferencia", "int", 0) },
                { "detalle_asegurado", ("Detalles_BienAsegurado", "varchar", 2500) },
                { "tipo_techo", ("TpoTecho_BienAsegurado", "varchar", 50) },
                { "tipo_muros", ("TpoMuro_BienAsegurado", "varchar", 50) },
                { "num_pisos", ("Pisos_BienAsegurado", "varchar", 50) },
                { "piso_inmueble", ("PisosDel_BienAsegurado", "varchar", 50) }
            };

                default:
                    return null;    // hoja no configurada
            }
        }

        private string NormalizeHeader(string header)
        {
            if (string.IsNullOrWhiteSpace(header))
                return "";

            // Normaliza a minúsculas
            string norm = header.Trim().ToLowerInvariant();

            // Quita acentos
            norm = norm.Normalize(NormalizationForm.FormD);
            norm = new string(norm.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray());

            // Reemplaza espacios, guiones, etc. por guion bajo
            norm = Regex.Replace(norm, @"[\s\-]+", "_");

            return norm;
        }

        private bool ValidarEstructuraExcel(string rutaExcel, Dictionary<string, List<string>> estructuraEsperada, out string mensajeError)
        {
            mensajeError = "";
            var errores = new List<string>();

            FileInfo fileInfo = new FileInfo(rutaExcel);

            // Obligatorio para EPPlus (licencia)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                // Recorrer cada hoja que se espera validar
                foreach (var hojaConfig in estructuraEsperada)
                {
                    string nombreHoja = hojaConfig.Key;
                    List<string> columnasEsperadas = hojaConfig.Value ?? new List<string>();

                    // Buscar la hoja
                    var worksheet = package.Workbook.Worksheets[nombreHoja];
                    if (worksheet == null)
                    {
                        errores.Add($"❌ La hoja '{nombreHoja}' no existe en el archivo.");
                        continue;
                    }

                    // Si la hoja no tiene celdas usadas
                    if (worksheet.Dimension == null || worksheet.Dimension.End.Column == 0)
                    {
                        errores.Add($"❌ La hoja '{nombreHoja}' no contiene datos (cabecera vacía).");
                        continue;
                    }

                    int colCount = worksheet.Dimension.End.Column;
                    var columnasEncontradasNorm = new List<string>();
                    var columnasEncontradasOriginal = new List<string>();
                    var duplicados = new List<string>();
                    var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    for (int col = 1; col <= colCount; col++)
                    {
                        var valorCelda = (worksheet.Cells[1, col].Text ?? "").Trim();
                        if (string.IsNullOrWhiteSpace(valorCelda))
                            continue;

                        string norm = NormalizeHeader(valorCelda);

                        columnasEncontradasOriginal.Add(valorCelda);
                        columnasEncontradasNorm.Add(norm);

                        if (!seen.Add(norm))
                            duplicados.Add(valorCelda);
                    }

                    if (duplicados.Any())
                    {
                        errores.Add($"❗ Hoja '{nombreHoja}' tiene encabezados duplicados: {string.Join(", ", duplicados.Distinct())}");
                    }

                    // Normalizar las columnas esperadas
                    var expectedNorm = columnasEsperadas.Select(c => NormalizeHeader(c)).ToList();

                    // Buscar faltantes
                    var faltantes = expectedNorm
                        .Where(c => !columnasEncontradasNorm.Contains(c, StringComparer.OrdinalIgnoreCase))
                        .ToList();

                    // Buscar sobrantes
                    var sobrantes = columnasEncontradasNorm
                        .Where(c => !expectedNorm.Contains(c, StringComparer.OrdinalIgnoreCase))
                        .ToList();

                    if (faltantes.Any() || sobrantes.Any())
                    {
                        if (faltantes.Any())
                            errores.Add($"❌ Hoja '{nombreHoja}' faltan columnas: {string.Join(", ", faltantes)}");

                        if (sobrantes.Any())
                            errores.Add($"❌ Hoja '{nombreHoja}' tiene columnas no esperadas: {string.Join(", ", sobrantes)}");
                    }
                    else
                    {
                        errores.Add($"✅ Hoja '{nombreHoja}' validada correctamente. (Columnas: {string.Join(", ", columnasEncontradasOriginal)})");
                    }
                }
            }

            // errores críticos (los que empiezan con ❌)
            var erroresCriticos = errores.Where(e => e.StartsWith("❌")).ToList();
            mensajeError = string.Join("\r\n", errores);

            return !erroresCriticos.Any();
        }

    }
}