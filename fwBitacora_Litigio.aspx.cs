using System;
using System.Data;
using System.Data.SqlClient;

using System.Web.UI;
using System.Web.UI.WebControls;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;

using System.Collections.Generic;


namespace WebItNow_Peacock
{
    public partial class fwBitacora_Litigio : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Session["DownloadsPath"] = GetDownloadFolderPath();

            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // string sReferencia = Request.QueryString["Referencia"];
            if (Session["IdUsuario"] == null || Session["UsPassword"] == null)
            {
                Response.Redirect("Login.aspx", true);
            }

            if (!Page.IsPostBack)
            {
                string sReferencia = Request.QueryString["Ref"];
                string SubReferencia = Request.QueryString["SubRef"];
                string IdProyecto = Request.QueryString["Proyecto"];
                string CveCliente = Request.QueryString["Seguro"];
                string IdTpoAsunto = Request.QueryString["Asunto"];

                Variables.wRef = sReferencia;
                Variables.wSubRef = Convert.ToInt32(SubReferencia);
                Variables.wIdProyecto = Convert.ToInt32(IdProyecto);
                Variables.wPrefijo_Aseguradora = CveCliente;
                Variables.wIdTpoAsunto = Convert.ToInt32(IdTpoAsunto);
                Variables.wExiste = false;

                if (Variables.wSubRef == 0 && Variables.wIdProyecto > 1)
                {
                    divSiniestro_Proyect.Visible = true;
                    divFecAviso_Proyect.Visible = true;
                } else
                {
                    divSiniestro_Proyect.Visible = false;
                    divFecAviso_Proyect.Visible = false;
                }

                // BindRepeater();

                inhabilitar(this.Controls);

                // inhabilitar control Crear Cuaderno
                if (Convert.ToString(Session["UsPrivilegios"]) == "0")
                {
                    BtnCrear_Cuaderno.Enabled = false;
                }

                // GetConsulta_Datos(sReferencia);

                GetProcedimiento();
                GetEstados();
                GetConclusion();
                GetRegimen();

                //// GetCoberturas();

                string flechaHaciaAbajo = "\u25BC";
                string flechaHaciaArriba = "\u25B2";

                btnShowPanel0.Text = flechaHaciaAbajo;   // Flecha hacia arriba
                btnShowPanel1.Text = flechaHaciaArriba;  // Flecha hacia arriba

                //// btnShowPanel2.Text = flechaHaciaAbajo;   // Flecha hacia abajo
                //// btnShowPanel3.Text = flechaHaciaAbajo;   // Flecha hacia abajo
                //// btnShowPanel4.Text = flechaHaciaAbajo;   // Flecha hacia abajo
                //// btnShowPanel5.Text = flechaHaciaArriba;  // Flecha hacia arriba
                //// btnShowPanel6.Text = flechaHaciaAbajo;   // Flecha hacia abajo
                //// btnShowPanel7.Text = flechaHaciaAbajo;   // Flecha hacia abajo
                //// btnShowPanel8.Text = flechaHaciaAbajo;   // Flecha hacia abajo
                //// btnShowPanel9.Text = flechaHaciaAbajo;   // Flecha hacia abajo
                //// btnShowPanel10.Text = flechaHaciaAbajo;  // Flecha hacia abajo
                //// btnShowPanel11.Text = flechaHaciaAbajo;  // Flecha hacia abajo
                //// btnShowPanel12.Text = flechaHaciaAbajo;  // Flecha hacia abajo
                //// btnShowPanel13.Text = flechaHaciaAbajo;  // Flecha hacia abajo
                //// btnShowPanel14.Text = flechaHaciaAbajo;  // Flecha hacia abajo
                //// btnShowPanel15.Text = flechaHaciaAbajo;  // Flecha hacia abajo
                //// btnShowPanel16.Text = flechaHaciaAbajo;  // Flecha hacia abajo
                //// btnShowPanel17.Text = flechaHaciaAbajo;  // Flecha hacia abajo

                ddlTpoAsegurado.Enabled = true;
                ddlConclusion.Enabled = true;
                // ddlCoberturas.Enabled = true;

                // TxtDomSiniestro.Enabled = true;

                GetSeccion_2();     // RIESGOS
                GetSeccion_4();     // BIENES
                GetSeccion_5();     // OTROS DETALLES

                // Obtener datos generales
                GetConsulta_Datos_Generales(sReferencia, SubReferencia);

                // Obtener datos coberturas tabla ITM_70_3_4
                // GetAltaCoberturas();
                // GetConsulta_Datos_Coberturas(sReferencia, SubReferencia);

                // inhabilitar controles panel coberturas
                // SetControlsEnabled(false);

                // Obtener los documentos de las categorias solicitados
                GetDocumentos(TxtSubReferencia.Text);


                // Inhabilitar controles si ya existe un Alta de Cuaderno
                if (Convert.ToString(Session["UsPrivilegios"]) != "0")
                {
                    Validar_Alta_Notebook();
                }

                // GetCategorias();

                // Actualizar CheckBox Seleccionados
                // Select_ITM_91(Variables.wRef, Variables.wSubRef);

            }

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

        protected void GetConclusion()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT C.IdDocumento, C.Descripcion " +
                                  "  FROM ITM_86 AS A, ITM_87 AS B, ITM_83 AS C " +
                                  " WHERE A.IdSeccion = B.IdCategoria " +
                                  "   AND A.IdDocumento = C.IdDocumento " +
                                  "   AND IdProyecto = " + Variables.wIdProyecto + " AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND IdSeccion = 3 ";

                if (Variables.wIdProyecto != 0)
                {
                    // strQuery += "   AND A.[IdTpoAsunto] = " + Variables.wIdTpoAsunto + " ";
                }

                strQuery += "   AND bSeleccion = 1 AND A.IdStatus = 1 " +
                            " ORDER BY C.IdDocumento ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlConclusion.DataSource = dt;

                ddlConclusion.DataValueField = "IdDocumento";
                ddlConclusion.DataTextField = "Descripcion";

                ddlConclusion.DataBind();
                ddlConclusion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

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
                ddlProcedimiento.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetRegimen()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT C.IdDocumento, C.Descripcion " +
                                  "  FROM ITM_86 AS A, ITM_87 AS B, ITM_81 AS C " +
                                  " WHERE A.IdSeccion = B.IdCategoria " +
                                  "   AND A.IdDocumento = C.IdDocumento " +
                                  "   AND IdProyecto = " + Variables.wIdProyecto + " AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND IdSeccion = 1 ";

                if (Variables.wIdProyecto != 0)
                {
                    // strQuery += "   AND A.IdTpoAsunto = " + Variables.wIdTpoAsunto + " ";
                }

                strQuery += "   AND bSeleccion = 1 AND A.IdStatus = 1 " +
                            " ORDER BY C.IdDocumento ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlTpoAsegurado.DataSource = dt;

                ddlTpoAsegurado.DataValueField = "IdDocumento";
                ddlTpoAsegurado.DataTextField = "Descripcion";

                ddlTpoAsegurado.DataBind();
                ddlTpoAsegurado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetCategorias()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdProyecto, IdCliente, IdSeccion, A.IdDocumento, B.IdCategoria, " +
                                  "       C.Descripcion AS DescCategoria, B.Descripcion AS DescSeccion " +
                                  "  FROM ITM_86 AS A, ITM_87 AS B, ITM_82 AS C " +
                                  " WHERE A.IdSeccion = B.IdCategoria " +
                                  "   AND A.IdDocumento = C.IdDocumento " +
                                  "   AND IdProyecto = " + Variables.wIdProyecto + " AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND IdSeccion = 2 ";

                if (Variables.wIdProyecto != 0)
                {
                    strQuery += "   AND A.IdTpoAsunto = " + Variables.wIdTpoAsunto + " ";
                }

                strQuery += "   AND bSeleccion = 1 AND A.IdStatus = 1 " +
                            "UNION ALL " +
                            "SELECT IdProyecto, IdCliente, IdSeccion, A.IdDocumento, B.IdCategoria, " +
                            "       C.Descripcion AS DescCategoria, B.Descripcion AS DescSeccion " +
                            "   FROM ITM_86 AS A, ITM_87 AS B, ITM_84 AS C " +
                            " WHERE A.IdSeccion = B.IdCategoria " +
                            "   AND A.IdDocumento = C.IdDocumento " +
                            "   AND IdProyecto = " + Variables.wIdProyecto + " AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                            "   AND IdSeccion = 4 ";

                if (Variables.wIdProyecto != 0)
                {
                    strQuery += "   AND A.IdTpoAsunto = " + Variables.wIdTpoAsunto + " ";
                }

                strQuery += "   AND bSeleccion = 1 AND A.IdStatus = 1 " +
                                "UNION ALL " +
                                "SELECT IdProyecto, IdCliente, IdSeccion, A.IdDocumento, B.IdCategoria, " +
                                "       C.Descripcion AS DescCategoria, B.Descripcion AS DescSeccion " +
                                "  FROM ITM_86 AS A, ITM_87 AS B, ITM_85 AS C " +
                                " WHERE A.IdSeccion = B.IdCategoria " +
                                "   AND A.IdDocumento = C.IdDocumento " +
                                "   AND IdProyecto = " + Variables.wIdProyecto + " AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                "   AND IdSeccion = 5 ";

                if (Variables.wIdProyecto != 0)
                {
                    strQuery += "   AND A.IdTpoAsunto = " + Variables.wIdTpoAsunto + " ";
                }

                strQuery += "   AND bSeleccion = 1 AND A.IdStatus = 1 " +
                            " ORDER BY IdSeccion, A.IdDocumento ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    //GrdCategorias.ShowHeaderWhenEmpty = true;
                    //GrdCategorias.EmptyDataText = "No hay resultados.";
                }

                //GrdCategorias.DataSource = dt;
                //GrdCategorias.DataBind();

                dbConn.Close();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }

        }

        protected void GetSeccion_2()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "  WITH CTE AS ( SELECT ITM_82.Descripcion AS Descripcion_82, ITM_86.IdDocumento, " +
                                  "                ROW_NUMBER() OVER (ORDER BY ITM_86.IdPosicion) AS RowNumber " +
                                  "  FROM ITM_82 " +
                                  " INNER JOIN ITM_86 ON ITM_82.IdDocumento = ITM_86.IdDocumento " +
                                  " WHERE ITM_82.IdStatus = 1 AND ITM_86.IdProyecto =  " + Variables.wIdProyecto + " AND ITM_86.bSeleccion = 1 " +
                                  "   AND ITM_86.IdSeccion = 2 AND ITM_86.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  " ), " +
                                  " Seleccion_91 AS ( SELECT IdCategoria, bSeleccion " +
                                  "  FROM ITM_91 " +
                                  " WHERE IdProyecto = " + Variables.wIdProyecto + " AND IdSeccion = 2 AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND Referencia = '" + Variables.wRef + "' AND SubReferencia = " + Variables.wSubRef + " ) " +
                                  "SELECT COALESCE(CTE1.Descripcion_82, '') AS Columna1, " +
                                  "       COALESCE(S91_1.bSeleccion, 0) AS ChBoxSeccion_2_1, " +
                                  "       COALESCE(CTE2.Descripcion_82, '') AS Columna2, " +
                                  "       COALESCE(S91_2.bSeleccion, 0) AS ChBoxSeccion_2_2," +
                                  "       COALESCE(CTE3.Descripcion_82, '') AS Columna3, " +
                                  "       COALESCE(S91_3.bSeleccion, 0) AS ChBoxSeccion_2_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN Seleccion_91 S91_1 ON CTE1.IdDocumento = S91_1.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN Seleccion_91 S91_2 ON CTE2.IdDocumento = S91_2.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                  "  LEFT JOIN Seleccion_91 S91_3 ON CTE3.IdDocumento = S91_3.IdCategoria " +
                                  " ORDER BY CTE1.RowNumber ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    grdSeccion_2.ShowHeaderWhenEmpty = true;
                    grdSeccion_2.EmptyDataText = "No hay resultados.";
                }

                grdSeccion_2.DataSource = dt;
                grdSeccion_2.DataBind();

                dbConn.Close();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetSeccion_4()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "  WITH CTE AS ( SELECT ITM_84.Descripcion AS Descripcion_84, ITM_86.IdDocumento, " +
                                  "                ROW_NUMBER() OVER (ORDER BY ITM_86.IdPosicion) AS RowNumber " +
                                  "  FROM ITM_84 " +
                                  " INNER JOIN ITM_86 ON ITM_84.IdDocumento = ITM_86.IdDocumento " +
                                  " WHERE ITM_84.IdStatus = 1 AND ITM_86.IdProyecto =  " + Variables.wIdProyecto + " AND ITM_86.bSeleccion = 1 " +
                                  "   AND ITM_86.IdSeccion = 4 AND ITM_86.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  " ), " +
                                  " Seleccion_91 AS ( SELECT IdCategoria, bSeleccion " +
                                  "  FROM ITM_91 " +
                                  " WHERE IdProyecto = " + Variables.wIdProyecto + " AND IdSeccion = 4 AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND Referencia = '" + Variables.wRef + "' AND SubReferencia = " + Variables.wSubRef + " ) " +
                                  "SELECT COALESCE(CTE1.Descripcion_84, '') AS Columna1, " +
                                  "       COALESCE(S91_1.bSeleccion, 0) AS ChBoxSeccion_4_1, " +
                                  "       COALESCE(CTE2.Descripcion_84, '') AS Columna2, " +
                                  "       COALESCE(S91_2.bSeleccion, 0) AS ChBoxSeccion_4_2," +
                                  "       COALESCE(CTE3.Descripcion_84, '') AS Columna3, " +
                                  "       COALESCE(S91_3.bSeleccion, 0) AS ChBoxSeccion_4_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN Seleccion_91 S91_1 ON CTE1.IdDocumento = S91_1.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN Seleccion_91 S91_2 ON CTE2.IdDocumento = S91_2.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                  "  LEFT JOIN Seleccion_91 S91_3 ON CTE3.IdDocumento = S91_3.IdCategoria " +
                                  " ORDER BY CTE1.RowNumber ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    grdSeccion_4.ShowHeaderWhenEmpty = true;
                    grdSeccion_4.EmptyDataText = "No hay resultados.";
                }

                grdSeccion_4.DataSource = dt;
                grdSeccion_4.DataBind();

                dbConn.Close();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetSeccion_5()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "  WITH CTE AS ( SELECT ITM_85.Descripcion AS Descripcion_85, ITM_86.IdDocumento, " +
                                  "                ROW_NUMBER() OVER (ORDER BY ITM_86.IdPosicion) AS RowNumber " +
                                  "  FROM ITM_85 " +
                                  " INNER JOIN ITM_86 ON ITM_85.IdDocumento = ITM_86.IdDocumento " +
                                  " WHERE ITM_85.IdStatus = 1 AND ITM_86.IdProyecto =  " + Variables.wIdProyecto + " AND ITM_86.bSeleccion = 1 " +
                                  "   AND ITM_86.IdSeccion = 5 AND ITM_86.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  " ), " +
                                  " Seleccion_91 AS ( SELECT IdCategoria, bSeleccion " +
                                  "  FROM ITM_91 " +
                                  " WHERE IdProyecto = " + Variables.wIdProyecto + " AND IdSeccion = 5 AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND Referencia = '" + Variables.wRef + "' AND SubReferencia = " + Variables.wSubRef + " ) " +
                                  "SELECT COALESCE(CTE1.Descripcion_85, '') AS Columna1, " +
                                  "       COALESCE(S91_1.bSeleccion, 0) AS ChBoxSeccion_5_1, " +
                                  "       COALESCE(CTE2.Descripcion_85, '') AS Columna2, " +
                                  "       COALESCE(S91_2.bSeleccion, 0) AS ChBoxSeccion_5_2," +
                                  "       COALESCE(CTE3.Descripcion_85, '') AS Columna3, " +
                                  "       COALESCE(S91_3.bSeleccion, 0) AS ChBoxSeccion_5_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN Seleccion_91 S91_1 ON CTE1.IdDocumento = S91_1.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN Seleccion_91 S91_2 ON CTE2.IdDocumento = S91_2.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                  "  LEFT JOIN Seleccion_91 S91_3 ON CTE3.IdDocumento = S91_3.IdCategoria " +
                                  " ORDER BY CTE1.RowNumber ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    grdSeccion_5.ShowHeaderWhenEmpty = true;
                    grdSeccion_5.EmptyDataText = "No hay resultados.";
                }

                grdSeccion_5.DataSource = dt;
                grdSeccion_5.DataBind();

                dbConn.Close();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void grdSeccion_2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Encuentra los CheckBoxes dentro de la fila
                CheckBox chBoxSeccion_2_1 = (CheckBox)e.Row.FindControl("ChBoxSeccion_2_1");
                CheckBox chBoxSeccion_2_2 = (CheckBox)e.Row.FindControl("ChBoxSeccion_2_2");
                CheckBox chBoxSeccion_2_3 = (CheckBox)e.Row.FindControl("ChBoxSeccion_2_3");

                // Obtén los datos de la fila actual
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                // Obtén los valores de las columnas y conviértelos a tipo entero
                string valorColumna1 = Convert.ToString(rowView["Columna1"]);
                string valorColumna2 = Convert.ToString(rowView["Columna2"]);
                string valorColumna3 = Convert.ToString(rowView["Columna3"]);

                // Obtén el valor de la columna Columna1
                int valorChBoxSeccion_2_1 = Convert.ToInt32(rowView["ChBoxSeccion_2_1"]);
                int valorChBoxSeccion_2_2 = Convert.ToInt32(rowView["ChBoxSeccion_2_2"]);
                int valorChBoxSeccion_2_3 = Convert.ToInt32(rowView["ChBoxSeccion_2_3"]);

                // Verifica si la columna Columna1 tiene datos y muestra u oculta el CheckBox
                if (!string.IsNullOrEmpty(valorColumna1))
                {
                    chBoxSeccion_2_1.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_2_1.Checked = valorChBoxSeccion_2_1 == 1;
                }
                else
                {
                    chBoxSeccion_2_1.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna2))
                {
                    chBoxSeccion_2_2.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_2_2.Checked = valorChBoxSeccion_2_2 == 1;
                }
                else
                {
                    chBoxSeccion_2_2.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna3))
                {
                    chBoxSeccion_2_3.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_2_3.Checked = valorChBoxSeccion_2_3 == 1;
                }
                else
                {
                    chBoxSeccion_2_3.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
            }
        }

        protected void grdSeccion_4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Encuentra los CheckBoxes dentro de la fila
                CheckBox chBoxSeccion_4_1 = (CheckBox)e.Row.FindControl("ChBoxSeccion_4_1");
                CheckBox chBoxSeccion_4_2 = (CheckBox)e.Row.FindControl("ChBoxSeccion_4_2");
                CheckBox chBoxSeccion_4_3 = (CheckBox)e.Row.FindControl("ChBoxSeccion_4_3");

                // Obtén los datos de la fila actual
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                // Obtén los valores de las columnas y conviértelos a tipo entero
                string valorColumna1 = Convert.ToString(rowView["Columna1"]);
                string valorColumna2 = Convert.ToString(rowView["Columna2"]);
                string valorColumna3 = Convert.ToString(rowView["Columna3"]);

                // Obtén el valor de la columna Columna1
                int valorChBoxSeccion_4_1 = Convert.ToInt32(rowView["ChBoxSeccion_4_1"]);
                int valorChBoxSeccion_4_2 = Convert.ToInt32(rowView["ChBoxSeccion_4_2"]);
                int valorChBoxSeccion_4_3 = Convert.ToInt32(rowView["ChBoxSeccion_4_3"]);

                // Verifica si la columna Columna1 tiene datos y muestra u oculta el CheckBox
                if (!string.IsNullOrEmpty(valorColumna1))
                {
                    chBoxSeccion_4_1.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_4_1.Checked = valorChBoxSeccion_4_1 == 1;
                }
                else
                {
                    chBoxSeccion_4_1.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna2))
                {
                    chBoxSeccion_4_2.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_4_2.Checked = valorChBoxSeccion_4_2 == 1;
                }
                else
                {
                    chBoxSeccion_4_2.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna3))
                {
                    chBoxSeccion_4_3.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_4_3.Checked = valorChBoxSeccion_4_3 == 1;
                }
                else
                {
                    chBoxSeccion_4_3.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
            }
        }

        protected void grdSeccion_5_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Encuentra los CheckBoxes dentro de la fila
                CheckBox chBoxSeccion_5_1 = (CheckBox)e.Row.FindControl("ChBoxSeccion_5_1");
                CheckBox chBoxSeccion_5_2 = (CheckBox)e.Row.FindControl("ChBoxSeccion_5_2");
                CheckBox chBoxSeccion_5_3 = (CheckBox)e.Row.FindControl("ChBoxSeccion_5_3");

                // Obtén los datos de la fila actual
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                // Obtén los valores de las columnas y conviértelos a tipo entero
                string valorColumna1 = Convert.ToString(rowView["Columna1"]);
                string valorColumna2 = Convert.ToString(rowView["Columna2"]);
                string valorColumna3 = Convert.ToString(rowView["Columna3"]);

                // Obtén el valor de la columna Columna1
                int valorChBoxSeccion_5_1 = Convert.ToInt32(rowView["ChBoxSeccion_5_1"]);
                int valorChBoxSeccion_5_2 = Convert.ToInt32(rowView["ChBoxSeccion_5_2"]);
                int valorChBoxSeccion_5_3 = Convert.ToInt32(rowView["ChBoxSeccion_5_3"]);

                // Verifica si la columna Columna1 tiene datos y muestra u oculta el CheckBox
                if (!string.IsNullOrEmpty(valorColumna1))
                {
                    chBoxSeccion_5_1.Visible = true;    // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_5_1.Checked = valorChBoxSeccion_5_1 == 1;
                }
                else
                {
                    chBoxSeccion_5_1.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna2))
                {
                    chBoxSeccion_5_2.Visible = true;    // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_5_2.Checked = valorChBoxSeccion_5_2 == 1;
                }
                else
                {
                    chBoxSeccion_5_2.Visible = false;   // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna3))
                {
                    chBoxSeccion_5_3.Visible = true;    // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_5_3.Checked = valorChBoxSeccion_5_3 == 1;
                }
                else
                {
                    chBoxSeccion_5_3.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
            }
        }

        public int GetConsulta_Datos_Generales(string pReferencia, string pSubReferencia)
        {

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t0.IdAsunto, CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END as Referencia, " +
                           "       t0.NomActor, t0.NomDemandado, " +
                           "       t0.NumSiniestro, A.NumSiniestro_Proyecto, A.Expediente_Proyecto, A.NumPoliza, " +
                           "       A.Fec_Reporte, A.Fec_Aviso, A.Vigencia, A.NomAjustador, A.Calle, A.Num_Exterior, A.Num_Interior, A.Estado, " +
                           "       A.Delegacion, A.Colonia, A.Codigo_Postal, A.NomAfectado, A.Tribunal, A.ExpLitigio, t0.IdTpoJuicio, " +
                           "       CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END as Seguro_Cia, " +
                           "       t0.IdRegimen, t0.IdConclusion," +
                           "       CASE WHEN t0.IdProyecto = 0 THEN 'NINGUNO' ELSE t5.Descripcion END AS NomProyecto," +
                           "       t0.Referencia_Anterior " +
                           "  FROM ITM_70 t0 " +
                           "  LEFT JOIN ITM_72 A ON t0.Referencia = A.Referencia AND t0.SubReferencia = A.SubReferencia " +
                           "  JOIN ITM_66 t1 ON t0.IdTpoAsunto = t1.IdTpoAsunto " +
                           "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                           "  LEFT JOIN ITM_68 t3 ON t0.IdRespTecnico = t3.IdRespTecnico " +
                           "  LEFT JOIN ITM_69 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                           "  LEFT JOIN ITM_78 t5 ON t0.IdProyecto = t5.IdProyecto " +
                           " WHERE t0.IdStatus IN (1) " +
                           "   AND t0.Referencia = '" + pReferencia + "'" +
                           "   AND t0.SubReferencia = '" + pSubReferencia + "'" +
                           " ORDER BY t0.IdAsunto DESC ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                foreach (DataRow row in dt.Rows)
                {
                    // Informacion General
                    TxtSubReferencia.Text = Convert.ToString(row[1]);
                    TxtNomActor.Text = Convert.ToString(row[2]);
                    TxtNomDemandado.Text = Convert.ToString(row[3]);
                    TxtNumSiniestro.Text = Convert.ToString(row[4]);
                    TxtSiniestro_Proyect.Text = Convert.ToString(row[5]);
                    TxtExpediente_Proyect.Text = Convert.ToString(row[6]);
                    TxtNumPoliza.Text = Convert.ToString(row[7]);
                    TxtFechaReporte.Text = Convert.ToString(row[8]);
                    TxtFechaAviso.Text = Convert.ToString(row[9]);
                    TxtVigencia.Text = Convert.ToString(row[10]);
                    TxtNomAjustador.Text = Convert.ToString(row[11]);
                    TxtCalle.Text = Convert.ToString(row[12]);
                    TxtNumExterior.Text = Convert.ToString(row[13]);
                    TxtNumInterior.Text = Convert.ToString(row[14]);
                    
                    ddlEstado.SelectedValue = Convert.ToString(row[15]);

                    // Disparar el evento SelectedIndexChanged manualmente
                    ddlEstado_SelectedIndexChanged(ddlEstado, EventArgs.Empty);

                    ddlMunicipios.SelectedValue = Convert.ToString(row[16]);

                    TxtColonia.Text = Convert.ToString(row[17]);
                    TxtCodigoPostal.Text = Convert.ToString(row[18]);
                    TxtNomAfectado.Text = Convert.ToString(row[19]);

                    TxtTribunal.Text = Convert.ToString(row[20]);
                    TxtExpLitigio.Text = Convert.ToString(row[21]);
                    ddlProcedimiento.SelectedValue = Convert.ToString(row[22]);

                    TxtSeguro_Cia.Text = Convert.ToString(row[23]);
                    ddlTpoAsegurado.SelectedValue = Convert.ToString(row[24]);
                    ddlConclusion.SelectedValue = Convert.ToString(row[25]);

                    if (Variables.wSubRef == 0 && Variables.wIdProyecto > 1)
                    {
                        string NomProyecto = Convert.ToString(row[26]);

                        // Datos Litigio solo para referencia proyecto
                        LblSiniestro_Proyect.Text = LblSiniestro_Proyect.Text + " " + NomProyecto;
                        LblExpediente_Proyect.Text = LblExpediente_Proyect.Text + " " + NomProyecto;
                    }

                    TxtAntReferencia.Text = Convert.ToString(row[27]);

                    return 0;
                }

                dbConn.Close();

                return 0;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }

            return -1;

        }

        public void inhabilitar(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Enabled = false;
                else if (control is DropDownList)
                    ((DropDownList)control).Enabled = false;
                else if (control is RadioButtonList)
                    ((RadioButtonList)control).Enabled = false;
                else if (control is CheckBoxList)
                    ((CheckBoxList)control).Enabled = false;
                else if (control is RadioButton)
                    ((RadioButton)control).Enabled = false;
                //else if (control is CheckBox)
                //    ((CheckBox)control).Enabled = false;
                else if (control.HasControls())
                    //Esta linea detécta un Control que contenga otros Controles
                    //Así ningún control se quedará sin ser limpiado.
                    inhabilitar(control.Controls);
            }
        }

        public void habilitar(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Enabled = true;
                else if (control is DropDownList)
                    ((DropDownList)control).Enabled = true;
                else if (control is RadioButtonList)
                    ((RadioButtonList)control).Enabled = true;
                else if (control is CheckBoxList)
                    ((CheckBoxList)control).Enabled = true;
                else if (control is RadioButton)
                    ((RadioButton)control).Enabled = true;
                else if (control is CheckBox)
                    ((CheckBox)control).Enabled = true;
                else if (control.HasControls())
                    //Esta linea detécta un Control que contenga otros Controles
                    //Así ningún control se quedará sin ser limpiado.
                    habilitar(control.Controls);
            }

        }

        public void habilitar_Config_Siniestro()
        {
            if (Variables.wExiste)
            {
                ddlTpoAsegurado.Enabled = false;
                ddlConclusion.Enabled = true;

                BtnCrear_Cuaderno.Enabled = false;
            }
            else
            {
                ddlTpoAsegurado.Enabled = true;
                ddlConclusion.Enabled = true;

                BtnCrear_Cuaderno.Enabled = true;
                // BtnGraba_Categorias.Enabled = false;

                // Desactivar los CheckBoxes
                DesactivarCheckBoxes(grdSeccion_2, true);
                DesactivarCheckBoxes(grdSeccion_4, true);
                DesactivarCheckBoxes(grdSeccion_5, true);

                UpdatePanel8.Update();
                UpdatePanel10.Update();
                UpdatePanel12.Update();
            }

        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Variables.wRef = string.Empty;
            Variables.wSubRef = 0;
            Variables.wIdProyecto = 0;
            Variables.wPrefijo_Aseguradora = string.Empty;
            Variables.wIdTpoAsunto = 0;

            Response.Redirect("fwReporte_Alta_Asunto.aspx", true);
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            habilitar(this.Controls);

            // TxtNumReporte.Enabled = false;
            // BtnEditar.Visible = false;
            // BtnGrabar.Visible = true;
        }

        protected void BtnCartaSolicitud_Click(object sender, EventArgs e)
        {
            try
            {
                Carta_Solicitud();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnCrear_Cuaderno_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwDocument_Notebook.aspx?Ref=" + Variables.wRef + "&SubRef=" + Variables.wSubRef + "&Create=" + "1", true);
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sEstado = ddlEstado.SelectedValue;
            GetMunicipios(sEstado);
        }

        protected void ddlMunicipios_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlProcedimiento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnAddCodigoPostal_Click(object sender, EventArgs e)
        {

        }

        private void Carta_Solicitud()
        {
            try
            {
                string plantillaPath = Server.MapPath("~/itnowstorage/CARTA_SOLICITUD.docx");
                string documentoGeneradoPath = Server.MapPath("~/itnowstorage/CartaSolicitud.docx");

                // Obtener la fecha del día
                DateTime fechaActual = DateTime.Now;

                string DomSiniestro = TxtCalle.Text.Trim() + " " + TxtNumExterior.Text.Trim() + " " + TxtNumInterior.Text.Trim() + ", " + TxtColonia.Text.Trim()
                                      + ", " + ddlMunicipios.SelectedItem + ", " + TxtCodigoPostal.Text.Trim() + ", " + ddlEstado.SelectedItem;

                // Formatear la fecha
                string Fec_Solicitud = fechaActual.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                List<string> documentosSolicitados = ObtenerDocumentosSolicitados();

                string Doc_Solicitados = string.Empty;

                // Concatenar los valores en la variable Doc_Solicitados
                foreach (var doc in documentosSolicitados)
                {
                    // Doc_Solicitados += doc + Environment.NewLine;
                    Doc_Solicitados += string.Join(Environment.NewLine, documentosSolicitados);
                }

                List<string> elementosSeleccionados = ObtenerElementosSeleccionados();

                string Categorias_Seleccionadas = string.Join(", ", elementosSeleccionados);

                // Copiar la plantilla a un nuevo documento
                System.IO.File.Copy(plantillaPath, documentoGeneradoPath, true);

                // Abrir el documento generado
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documentoGeneradoPath, true))
                {

                    // Obtener el cuerpo del documento
                    DocumentFormat.OpenXml.Wordprocessing.Body body = wordDoc.MainDocumentPart.Document.Body;

                    // Verificar si el cuerpo existe, si no, lanzar una excepción
                    if (body == null)
                    {
                        throw new InvalidOperationException("La plantilla no tiene un cuerpo definido.");
                    }

                    // Buscar y reemplazar marcadores de posición con datos reales
                    ReplaceText(body, "Fec_Solicitud", Fec_Solicitud);
                    ReplaceText(body, "Num_Poliza", TxtNumPoliza.Text);
                    ReplaceText(body, "Num_Siniestro", TxtNumSiniestro.Text);
                    ReplaceText(body, "Nom_Asegurado", TxtNomAfectado.Text);

                    ReplaceText(body, "Dom_Registrado", DomSiniestro);
                    ReplaceText(body, "Nom_Categorias", Categorias_Seleccionadas);

                    ReplaceTextWithNewLines(body, "Doc_Solicitados", documentosSolicitados, "Skeena", 10);

                    // ReplaceText(body, "Doc_Solicitados", Doc_Solicitados);

                    // Guardar los cambios
                    wordDoc.MainDocumentPart.Document.Save();
                }

                LblMessage.Text = "Carta Solicitud, se a generado correctamente";
                this.mpeMensaje.Show();

                // Descargar el documento generado
                Session["sFileName"] = "CartaSolicitud.docx";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AbrirDescarga", string.Format("window.open('Descargas.aspx');"), true);

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        private void ReplaceText(DocumentFormat.OpenXml.Wordprocessing.Body body, string placeholder, string newText)
        {
            foreach (var textElement in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
            {
                if (textElement.Text.Trim().Contains(placeholder))
                {
                    textElement.Text = textElement.Text.Replace(placeholder, newText);
                }
            }
        }

        private void ReplaceTextWithNewLines(DocumentFormat.OpenXml.Wordprocessing.Body body, string placeholder, List<string> values, string fontFamily, int fontSize)
        {
            foreach (var paragraph in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>())
            {
                foreach (var text in paragraph.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
                {
                    if (text.Text.Contains(placeholder))
                    {
                        // Crear un nuevo Run para reemplazar el texto del marcador de posición
                        DocumentFormat.OpenXml.Wordprocessing.Run run = new DocumentFormat.OpenXml.Wordprocessing.Run();

                        // Crear propiedades de estilo para el Run
                        DocumentFormat.OpenXml.Wordprocessing.RunProperties runProperties = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();

                        // Asignar tipo de letra (font family)
                        DocumentFormat.OpenXml.Wordprocessing.RunFonts runFonts = new DocumentFormat.OpenXml.Wordprocessing.RunFonts() { Ascii = fontFamily, HighAnsi = fontFamily };
                        runProperties.Append(runFonts);

                        // Asignar tamaño de fuente
                        DocumentFormat.OpenXml.Wordprocessing.FontSize fontSizeValue = new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = $"{fontSize * 2}" };    // Multiplicar por 2 para el formato de OpenXML
                        runProperties.AppendChild(fontSizeValue);

                        // Añadir las propiedades de estilo al Run
                        run.Append(runProperties);

                        // Añadir cada valor en la lista con un salto de línea
                        foreach (var value in values)
                        {
                            // Crear un nuevo Text para cada valor con su estilo
                            run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(value));
                            run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Break()); // Añadir un salto de línea
                        }

                        // Reemplazar el marcador de posición con el nuevo Run
                        text.Text = string.Empty; // Limpiar el texto del marcador
                        text.Parent.InsertAfterSelf(run); // Insertar el nuevo Run después del marcador
                    }
                }
            }
        }

        private List<string> ObtenerDocumentosSolicitados()
        {
            List<string> documentosSolicitados = new List<string>();

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT CONCAT(ROW_NUMBER() OVER (ORDER BY t1.NomArchivo), '. ', t1.NomArchivo) AS Descripcion " +
                                  "  FROM ITM_88 AS t1 " +
                                  "  LEFT JOIN ITM_90 AS t4 ON t4.DocInterno = 0 AND t1.IdDocumento = t4.IdConsecutivo " +
                                  "  JOIN ITM_91 AS t2 ON t1.IdProyecto = t2.IdProyecto AND t1.IdSeccion = t2.IdSeccion " +
                                  "   AND t1.IdCliente = t2.IdCliente AND t1.IdCategoria = t2.IdCategoria " +
                                  "  LEFT JOIN ITM_47 AS t3 " +
                                  "    ON (CASE WHEN t3.SubReferencia >= 1 THEN CONCAT(t3.UsReferencia, '-', t3.SubReferencia) ELSE t3.UsReferencia END) = '" + Variables.wRef + "' " +
                                  "   AND t1.IdCliente = t3.IdAseguradora AND t1.IdTpoAsunto = t3.IdTpoAsunto " +
                                  "   AND t1.IdSeccion = t3.IdSeccion  AND t1.IdDocumento = t3.IdDocumento " +
                                  " WHERE (CASE WHEN t2.SubReferencia >= 1 THEN CONCAT(t2.Referencia, '-', t2.SubReferencia) ELSE t2.Referencia END) = '" + Variables.wRef + "' " +
                                  "   AND t1.IdProyecto = " + Variables.wIdProyecto + " " +
                                  "   AND t1.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                foreach (DataRow row in dt.Rows)
                {
                    documentosSolicitados.Add(row["Descripcion"].ToString());
                }

                dbConn.Close();
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

            return documentosSolicitados;
        }

        private List<string> ObtenerElementosSeleccionados()
        {
            List<string> seleccionados = new List<string>();

            try
            {

                foreach (GridViewRow row in grdSeccion_2.Rows)
                {
                    // Accede a cada CheckBox en la fila
                    CheckBox chkBox1 = (CheckBox)row.FindControl("ChBoxSeccion_2_1");
                    CheckBox chkBox2 = (CheckBox)row.FindControl("ChBoxSeccion_2_2");
                    CheckBox chkBox3 = (CheckBox)row.FindControl("ChBoxSeccion_2_3");

                    // Accede a los valores de las columnas según tu lógica
                    //string columna1 = ((DataBoundLiteralControl)row.Cells[0].Controls[0]).Text.Trim();
                    //string columna2 = ((DataBoundLiteralControl)row.Cells[2].Controls[0]).Text.Trim();
                    //string columna3 = ((DataBoundLiteralControl)row.Cells[4].Controls[0]).Text.Trim();

                    string columna1 = row.Cells[0].Text.Trim() == "&nbsp;" ? "" : Server.HtmlDecode(row.Cells[0].Text.Trim());  // Asumiendo que el texto está en la primera celda (Columna1)
                    string columna2 = row.Cells[2].Text.Trim() == "&nbsp;" ? "" : Server.HtmlDecode(row.Cells[2].Text.Trim());  // Asumiendo que el texto está en la tercera celda (Columna2)
                    string columna3 = row.Cells[4].Text.Trim() == "&nbsp;" ? "" : Server.HtmlDecode(row.Cells[4].Text.Trim());  // Asumiendo que el texto está en la quinta celda (Columna3)

                    if (chkBox1 != null && chkBox1.Checked)
                    {
                        seleccionados.Add(columna1);
                    }
                    if (chkBox2 != null && chkBox2.Checked)
                    {
                        seleccionados.Add(columna2);
                    }
                    if (chkBox3 != null && chkBox3.Checked)
                    {
                        seleccionados.Add(columna3);
                    }
                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

            return seleccionados;
        }

        protected void btnShowPanel0_Click(object sender, EventArgs e)
        {
            pnl0.Visible = !pnl0.Visible;   // Cambia la visibilidad del Panel 1 al contrario de su estado actual

            if (pnl0.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel0.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl0.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel0.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl0.Visible = false;
            }
        }

        protected void btnShowPanel1_Click(object sender, EventArgs e)
        {
            pnl1.Visible = !pnl1.Visible;   // Cambia la visibilidad del Panel 1 al contrario de su estado actual

            if (pnl1.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel1.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl1.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel1.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl1.Visible = false;
            }
        }

        protected void ddlTpoAsegurado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlConclusion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdCategorias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Width = Unit.Pixel(150);     // DescCategoria
                e.Row.Cells[4].Width = Unit.Pixel(150);     // DescSeccion
                e.Row.Cells[5].Width = Unit.Pixel(25);      // ChBoxRow
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;     // IdDocumento
                e.Row.Cells[1].Visible = false;     // IdSeccion
                e.Row.Cells[2].Visible = false;     // IdCategoria
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;     // IdDocumento
                e.Row.Cells[1].Visible = false;     // IdSeccion
                e.Row.Cells[2].Visible = false;     // IdCategoria
            }
        }


        protected void GrdDocumentos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //GrdDocumentos.PageIndex = e.NewPageIndex;
            //GetDocumentos(TxtSubReferencia.Text);
        }

        protected void GrdDocumentos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdDocumentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void GrdDocumentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chBoxRow = (CheckBox)e.Row.FindControl("ChBoxRow");
                if (chBoxRow != null)
                {
                    // Añadir atributo onclick para prevenir cambios
                    chBoxRow.Attributes["onclick"] = "return false;";
                }
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[3].Visible = false;     // DocInterno
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Visible = false;     // DocInterno
            }
        }

        public void Validar_Alta_Notebook()
        {

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT UsReferencia FROM ITM_03 " +
                                  " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END) = '" + TxtSubReferencia.Text + "' ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count > 0)
                {
                    ddlTpoAsegurado.Enabled = false;
                    BtnCrear_Cuaderno.Enabled = false;
                    // BtnGraba_Categorias.Enabled = false;

                    // Desactivar los CheckBoxes
                    DesactivarCheckBoxes(grdSeccion_2, false);
                    DesactivarCheckBoxes(grdSeccion_4, false);
                    DesactivarCheckBoxes(grdSeccion_5, false);

                    UpdatePanel8.Update();
                    UpdatePanel10.Update();
                    UpdatePanel12.Update();

                    Variables.wExiste = true;
                }

                dbConn.Close();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

        }

        public void DesactivarCheckBoxes(GridView gridView, bool habilitar)
        {
            foreach (GridViewRow row in gridView.Rows)
            {
                // Verifica si la fila es una fila de datos
                if (row.RowType == DataControlRowType.DataRow)
                {
                    // Itera sobre las celdas de la fila
                    foreach (TableCell cell in row.Cells)
                    {
                        // Verifica si la celda contiene un control CheckBox
                        foreach (Control control in cell.Controls)
                        {
                            CheckBox checkBox = control as CheckBox;
                            if (checkBox != null)
                            {
                                // Desactivar o activar los CheckBoxes según el parámetro "desactivar"
                                // checkBox.Enabled = habilitar;

                                if (habilitar)
                                {
                                    checkBox.CssClass = checkBox.CssClass.Replace("disabled-checkbox", "");     // Elimina la clase para habilitar visualmente los CheckBoxes
                                }
                                else
                                {
                                    checkBox.CssClass += " disabled-checkbox";      // Agrega la clase para deshabilitar visualmente los CheckBoxes
                                }
                            }
                        }
                    }
                }
            }
        }

        public void GetDocumentos(string sValor)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT * FROM (" +
                                  "SELECT t1.IdDoc_Categoria, t1.NomArchivo AS Descripcion, t3.TpoArchivo, t3.Fec_Entrega, " +
                                  "       (SELECT DocInterno FROM ITM_90 t4 WHERE t4.IdConsecutivo = t1.IdDocumento) AS DocInterno, " +
                                  "       COALESCE(t3.IdDescarga, 0) AS IdDescarga, " +
                                  "       t1.IdSeccion AS IdSeccion, t1.IdCategoria AS IdCategoria" +
                                  "  FROM ITM_88 AS t1 " +
                                  "  JOIN ITM_91 AS t2 ON t1.IdProyecto = t2.IdProyecto AND t1.IdSeccion = t2.IdSeccion " +
                                  "   AND t1.IdCliente = t2.IdCliente AND t1.IdCategoria = t2.IdCategoria " +
                                  "  LEFT JOIN ITM_47 AS t3 " +
                                  "    ON (CASE WHEN t3.SubReferencia >= 1 THEN CONCAT(t3.UsReferencia, '-', t3.SubReferencia) ELSE t3.UsReferencia END) = '" + sValor + "' " +
                                  "   AND t1.IdCliente = t3.IdAseguradora AND t1.IdTpoAsunto = t3.IdTpoAsunto " +
                                  "   AND t1.IdSeccion = t3.IdSeccion  AND t1.IdDocumento = t3.IdDocumento " +
                                  " WHERE (CASE WHEN t2.SubReferencia >= 1 THEN CONCAT(t2.Referencia, '-', t2.SubReferencia) ELSE t2.Referencia END) = '" + sValor + "' " +
                                  "   AND t1.IdProyecto = " + Variables.wIdProyecto + " " +
                                  "   AND t1.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "UNION ALL " +
                                  "SELECT t1.IdDocumento, t2.Descripcion, t1.TpoArchivo, t1.Fec_Entrega, t2.DocInterno, " +
                                  "COALESCE(t1.IdDescarga, 0) AS IdDescarga, t1.IdSeccion AS IdSeccion, t1.IdCategoria AS IdCategoria " +
                                  "  FROM ITM_47 AS t1 JOIN ITM_46 AS t2 " +
                                  "    ON (CASE WHEN t1.SubReferencia >= 1 THEN CONCAT(t1.UsReferencia, '-', t1.SubReferencia) ELSE t1.UsReferencia END) = '" + sValor + "' " +
                                  "   AND (CASE WHEN t2.SubReferencia >= 1 THEN CONCAT(t2.UsReferencia, '-', t2.SubReferencia) ELSE t2.UsReferencia END) = '" + sValor + "' " +
                                  "   AND t1.IdDocumento = t2.IdDocumento " +
                                  "  ) AS OrderedPart " +
                                  " ORDER BY IdSeccion, IdCategoria ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdDocumentos.ShowHeaderWhenEmpty = true;
                    GrdDocumentos.EmptyDataText = "No hay resultados.";
                }

                GrdDocumentos.DataSource = dt;
                GrdDocumentos.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdDocumentos.HeaderRow.TableSection = TableRowSection.TableHeader;

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnAnularPnl2_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            inhabilitar(this.Controls);

            // habilitar (Configuracion Siniestro)
            habilitar_Config_Siniestro();

            btnEditarPnl2.Visible = true;
            btnActualizarPnl2.Visible = false;
            BtnAnularPnl2.Visible = false;
        }

        protected void btnEditarPnl2_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl2
            TxtAntReferencia.Enabled = true;
            TxtNumSiniestro.Enabled = true;
            TxtSiniestro_Proyect.Enabled = true;
            TxtExpediente_Proyect.Enabled = true;
            TxtNumPoliza.Enabled = true;

            TxtFechaReporte.Enabled = true;
            TxtFechaAviso.Enabled = true;

            TxtVigencia.Enabled = true;
            TxtNomAjustador.Enabled = true;
            
            TxtCalle.Enabled = true;
            TxtNumExterior.Enabled = true;
            TxtNumInterior.Enabled = true;
            ddlEstado.Enabled = true;
            ddlMunicipios.Enabled = true;
            TxtColonia.Enabled = true;
            TxtCodigoPostal.Enabled = true;
            
            TxtNomAfectado.Enabled = true;
            TxtNomActor.Enabled = true;
            TxtNomDemandado.Enabled = true;

            TxtTribunal.Enabled = true;
            TxtExpLitigio.Enabled = true;
            ddlProcedimiento.Enabled = true;

            btnEditarPnl2.Visible = false;
            btnActualizarPnl2.Visible = true;
            BtnAnularPnl2.Visible = true;
        }

        protected void btnActualizarPnl2_Click(object sender, EventArgs e)
        {
            //if (Page.IsValid)
            //{

            //}

            string input = "11:15";   // TxtHoraAsignacion.Text
            if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^(?:[01]\d|2[0-3]):[0-5]\d$"))
            {
                // Maneja la entrada inválida.
                // Por ejemplo, muestra un mensaje de error al usuario.
                LblMessage.Text = "Formato Hora inválido. Use hh:mm.";
                this.mpeMensaje.Show();
            }
            else
            {
                Actualizar_ITM_70();

                Actualizar_Datos_Generales();

                inhabilitar(this.Controls);

                // habilitar (Configuracion Siniestro)
                habilitar_Config_Siniestro();

                btnEditarPnl2.Visible = true;
                btnActualizarPnl2.Visible = false;
                BtnAnularPnl2.Visible = false;

                LblMessage.Text = "Se han aplicado los cambios, correctamente";
                this.mpeMensaje.Show();
            }

        }

        protected void Actualizar_ITM_70()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Actualizar registro tabla (ITM_70)
                string strQuery = "UPDATE ITM_70 " +
                                  "   SET Referencia_Anterior = '" + TxtAntReferencia.Text.Trim() + "', " +
                                  "       NumSiniestro = '" + TxtNumSiniestro.Text.Trim() + "', " +
                                  "       NumPoliza = '" + TxtNumPoliza.Text.Trim() + "'," +
                                  "       NomActor = '" + TxtNomActor.Text.Trim() + "', " +
                                  "       NomDemandado = '" + TxtNomDemandado.Text.Trim() + "', " +
                                  "       NomAsegurado = '" + TxtNomAfectado.Text.Trim() + "', " +
                                  "       IdTpoJuicio  = " + ddlProcedimiento.SelectedValue + " " +
                                  " WHERE Referencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = " + Variables.wSubRef + " ";

                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Actualizar_Datos_Generales()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Informacion General
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;
                string NumSiniestro = TxtNumSiniestro.Text;
                string NumSiniestro_Proyecto = TxtSiniestro_Proyect.Text;
                string Expediente_Proyecto = TxtExpediente_Proyect.Text;
                string NumPoliza = TxtNumPoliza.Text;

                string Fec_Reporte = TxtFechaReporte.Text;
                string Fec_Aviso = TxtFechaAviso.Text;
                string Vigencia = TxtVigencia.Text;
                string NomAjustador = TxtNomAjustador.Text;

                string Calle = TxtCalle.Text;
                string Num_Exterior = TxtNumExterior.Text;
                string Num_Interior = TxtNumInterior.Text;
                string Estado = ddlEstado.SelectedValue;
                string Delegacion = ddlMunicipios.SelectedValue;
                string Colonia = TxtColonia.Text;
                string Codigo_Postal = TxtCodigoPostal.Text;

                string NomAfectado = TxtNomAfectado.Text;
                string NomActor = TxtNomActor.Text;
                string NomDemandado = TxtNomDemandado.Text;
                string Tribunal = TxtTribunal.Text;
                string ExpLitigio = TxtExpLitigio.Text;
                string TpoJuicio = ddlProcedimiento.SelectedValue;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @" INSERT INTO ITM_72 (Referencia, SubReferencia, NumSiniestro, NumSiniestro_Proyecto, Expediente_Proyecto, NumPoliza, " +
                                    "                    Fec_Reporte, Fec_Aviso, Vigencia, NomAjustador, Calle, Num_Exterior, Num_Interior, Estado, " +
                                    "                    Delegacion, Colonia, Codigo_Postal, NomAfectado, NomActor, NomDemandado, Tribunal, ExpLitigio, " +
                                    "                    TpoJuicio, Id_Usuario, IdStatus)" +
                                    " VALUES (@Referencia, @SubReferencia, @NumSiniestro, @NumSiniestro_Proyecto, @Expediente_Proyecto, @NumPoliza, " +
                                    "         @Fec_Reporte, @Fec_Aviso, @Vigencia, @NomAjustador, @Calle, @Num_Exterior, @Num_Interior, @Estado, " +
                                    "         @Delegacion, @Colonia, @Codigo_Postal, @NomAfectado, @NomActor, @NomDemandado, @Tribunal, @ExpLitigio, " +
                                    "         @TpoJuicio, @Id_Usuario, @IdStatus)" +
                                    " ON DUPLICATE KEY UPDATE " +
                                    "    NumSiniestro = VALUES(NumSiniestro), " +
                                    "    NumSiniestro_Proyecto = VALUES(NumSiniestro_Proyecto), " +
                                    "    Expediente_Proyecto = VALUES(Expediente_Proyecto), " +
                                    "    NumPoliza = VALUES(NumPoliza), " +
                                    "    Fec_Reporte = VALUES(Fec_Reporte), " +
                                    "    Fec_Aviso = VALUES(Fec_Aviso), " +
                                    "    Vigencia = VALUES(Vigencia), " +
                                    "    NomAjustador = VALUES(NomAjustador), " +
                                    "    Calle = VALUES(Calle), " +
                                    "    Num_Exterior = VALUES(Num_Exterior), " +
                                    "    Num_Interior = VALUES(Num_Interior), " +
                                    "    Estado = VALUES(Estado), " +
                                    "    Delegacion = VALUES(Delegacion), " +
                                    "    Colonia = VALUES(Colonia), " +
                                    "    Codigo_Postal = VALUES(Codigo_Postal), " +
                                    "    NomAfectado = VALUES(NomAfectado), " +
                                    "    NomActor = VALUES(NomActor), " +
                                    "    NomDemandado = VALUES(NomDemandado), " +
                                    "    Tribunal = VALUES(Tribunal), " +
                                    "    ExpLitigio = VALUES(ExpLitigio), " +
                                    "    TpoJuicio = VALUES(TpoJuicio), " +
                                    "    Id_Usuario = VALUES(Id_Usuario), " +
                                    "    IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@NumSiniestro", NumSiniestro);
                    cmd.Parameters.AddWithValue("@NumSiniestro_Proyecto", NumSiniestro_Proyecto);
                    cmd.Parameters.AddWithValue("@Expediente_Proyecto", Expediente_Proyecto);
                    cmd.Parameters.AddWithValue("@NumPoliza", NumPoliza);
                    cmd.Parameters.AddWithValue("@Fec_Reporte", Fec_Reporte);
                    cmd.Parameters.AddWithValue("@Fec_Aviso", Fec_Aviso);
                    cmd.Parameters.AddWithValue("@Vigencia", Vigencia);
                    cmd.Parameters.AddWithValue("@NomAjustador", NomAjustador);

                    cmd.Parameters.AddWithValue("@Calle", Calle);
                    cmd.Parameters.AddWithValue("@Num_Exterior", Num_Exterior);
                    cmd.Parameters.AddWithValue("@Num_Interior", Num_Interior);
                    cmd.Parameters.AddWithValue("@Estado", Estado);

                    cmd.Parameters.AddWithValue("@Delegacion", Delegacion);
                    cmd.Parameters.AddWithValue("@Colonia", Colonia);
                    cmd.Parameters.AddWithValue("@Codigo_Postal", Codigo_Postal);

                    cmd.Parameters.AddWithValue("@NomAfectado", NomAfectado);
                    cmd.Parameters.AddWithValue("@NomActor", NomActor);
                    cmd.Parameters.AddWithValue("@NomDemandado", NomDemandado);
                    cmd.Parameters.AddWithValue("@Tribunal", Tribunal);
                    cmd.Parameters.AddWithValue("@ExpLitigio", ExpLitigio);
                    cmd.Parameters.AddWithValue("@TpoJuicio", TpoJuicio);

                    cmd.Parameters.AddWithValue("@Id_Usuario", Id_Usuario);
                    cmd.Parameters.AddWithValue("@IdStatus", IdStatus);

                });

                dbConn.Close();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnGraba_Categorias_Click(object sender, EventArgs e)
        {
            try
            {
                // Eliminar registros
                Delete_ITM_91();
                // Actualizar campo IdRegimen, IdConclusion, DomSiniestro 
                Update_ITM_70();
                // Insertar en ITM_91 (Tipo de Asegurado / Tpo de Estatus)
                Insert_ITM_91();

                // Agregar registros con los datos de cada cuaderno a cargar
                string sReferencia = Variables.wRef;
                int iSubReferencia = Variables.wSubRef;
                string IdAseguradora = Variables.wPrefijo_Aseguradora;
                int IdConclusion = Convert.ToInt32(ddlConclusion.SelectedValue);
                int IdRegimen = Convert.ToInt32(ddlTpoAsegurado.SelectedValue); ;

                if (Variables.wExiste == false)
                {
                    Add_tbDetalleCuadernos(sReferencia, iSubReferencia, IdAseguradora, IdConclusion, IdRegimen);
                }

                Obtener_Valores_ChBox(grdSeccion_2, "ChBoxSeccion_2", 2, "ITM_82");
                Obtener_Valores_ChBox(grdSeccion_4, "ChBoxSeccion_4", 4, "ITM_84");
                Obtener_Valores_ChBox(grdSeccion_5, "ChBoxSeccion_5", 5, "ITM_85");

                GetDocumentos(TxtSubReferencia.Text);

                LblMessage.Text = "Se han aplicado los cambios, correctamente";
                this.mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }


        protected void Update_ITM_70()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string DomSiniestro = string.Empty;

                // Eliminar registro(s) tablas (ITM_70)
                string strQuery = "UPDATE ITM_03 " +
                                  "   SET IdRegimen = " + ddlTpoAsegurado.SelectedValue + "," +
                                  "       IdConclusion = " + ddlConclusion.SelectedValue + " " +
                                  " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END) = '" + TxtSubReferencia.Text + "' ; ";

                strQuery += Environment.NewLine;

                strQuery += "UPDATE ITM_46 " +
                            "   SET IdConclusion = " + ddlConclusion.SelectedValue + " " +
                            " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END) = '" + TxtSubReferencia.Text + "' ; ";

                strQuery += Environment.NewLine;

                strQuery += "UPDATE ITM_47 " +
                            "   SET IdConclusion = " + ddlConclusion.SelectedValue + " " +
                            " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END) = '" + TxtSubReferencia.Text + "' ; ";

                strQuery += Environment.NewLine;

                strQuery += "UPDATE ITM_70 " +
                            "   SET IdRegimen = " + ddlTpoAsegurado.SelectedValue + "," +
                            "       IdConclusion = " + ddlConclusion.SelectedValue + ", " +
                            "       DomSiniestro = '" + DomSiniestro + "' " +
                            " WHERE Referencia = '" + Variables.wRef + "' " +
                            "   AND SubReferencia = " + Variables.wSubRef + " ";

                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        public void Add_tbDetalleCuadernos(String pReferencia, int pSubReferencia, string pIdAseguradora, int pIdConclusion, int pIdRegimen)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Obtener los registros que corresponden al Tpo. de Cuaderno
                string strQuery = "SELECT t1.IdDoc_Categoria, t1.IdCliente, t1.IdProyecto, t1.IdTpoAsunto, t1.IdSeccion, " +
                                  "       t1.IdCategoria, t1.IdDocumento, t1.NomArchivo, t1.IdUsuario, t1.IdStatus " +
                                  " FROM ITM_88 AS t1, ITM_91 AS t2 " +
                                  " WHERE t2.Referencia = '" + pReferencia + "' " +
                                  "   AND t2.SubReferencia = " + pSubReferencia + " " +
                                  "   AND t1.IdProyecto = " + Variables.wIdProyecto + "" +
                                  "   AND t1.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND t1.IdSeccion = 3 AND t1.IdCategoria = '" + pIdConclusion + "' " +
                                  "   AND t1.IdProyecto = t2.IdProyecto AND t1.IdCliente = t2.IdCliente " +
                                  "   AND t1.IdSeccion = t2.IdSeccion AND t1.IdCategoria = t2.IdCategoria ";
                // "   AND t1.IdTpoAsunto = t2.IdTpoAsunto ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                foreach (DataRow row in dt.Rows)
                {
                    int IdTpoAsunto = Convert.ToInt32(row[3]);      // ITM_88
                    int IdSeccion = Convert.ToInt32(row[4]);        // ITM_88
                    int IdCategoria = Convert.ToInt32(row[5]);        // ITM_88
                    int IdDocumento = Convert.ToInt32(row[6]);      // ITM_88

                    int IdConclusion = Convert.ToInt32(ddlConclusion.SelectedValue);
                    int IdTpoDocumento = 1;
                    string sTpoArchivo = "PDF";     // PDF, MSG, XML
                    int IdDirectorio = 1;           // Pendiente

                    int IdDescarga = 0;
                    int IdStatus = 1;
                    int IdProyecto = Variables.wIdProyecto;

                    string sqlQuery = "INSERT INTO ITM_47 (UsReferencia, SubReferencia, IdProyecto, IdAseguradora, IdConclusion, IdTpoAsunto, IdTpoDocumento, TpoArchivo, IdSeccion, IdCategoria, IdDocumento, IdUsuario, Id_Directorio, Url_Archivo, Fec_Entrega, Fec_Rechazo, IdDescarga, IdStatus ) " +
                                      "VALUES ('" + pReferencia + "', " + pSubReferencia + ", " + IdProyecto + ", '" + pIdAseguradora + "', " + IdConclusion + ", " + IdTpoAsunto + ", " + IdTpoDocumento + ",  '" + sTpoArchivo + "', " + IdSeccion + ", " + IdCategoria + ", " + IdDocumento + ", NULL, " + IdDirectorio + ", NULL, NULL, NULL, " + IdDescarga + ", " + IdStatus + "); ";

                    // Insert en la tabla Estado de Documento
                    // MySqlDataReader reader = dbConn.ExecuteReaderQuery(sqlQuery);
                    int affectedRows = dbConn.ExecuteNonQuery(sqlQuery);
                }

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }
        }

        public int Validar_Existe_Categoria(int IdSeccion, int IdCategoria)
        {

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT Referencia, SubReferencia, IdSeccion, IdCategoria " +
                              "  FROM ITM_91 t0" +
                              " WHERE t0.Referencia = '" + Variables.wRef + "'" +
                              "   AND t0.SubReferencia = '" + Variables.wSubRef + "' " +
                              "   AND IdSeccion =  " + IdSeccion + " " +
                              "   AND IdCategoria = " + IdCategoria + " ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            dbConn.Close();

            if (dt.Rows.Count > 0)
            {
                return 1;
            }

            return 0;

        }

        protected void Insert_ITM_91()
        {
            try
            {

                int IdProyecto = Variables.wIdProyecto;
                string IdCliente = Variables.wPrefijo_Aseguradora;

                int IdTpoAsunto = Variables.wIdTpoAsunto;
                string IdUsuario = Variables.wUserLogon;

                int IdDocumento = 1;
                string strQuery = string.Empty;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                if (ddlTpoAsegurado.SelectedValue != "0")
                {

                    // Tipo de Asegurado
                    strQuery = "INSERT INTO ITM_91 (Referencia, SubReferencia, IdProyecto, IdCliente, IdTpoAsunto, IdSeccion, IdCategoria, IdDocumento, bSeleccion, IdUsuario) " +
                               "VALUES ('" + Variables.wRef + "', " + Variables.wSubRef + ", " + IdProyecto + ", '" + IdCliente + "', " + IdTpoAsunto + ", 1, " +
                               " " + ddlTpoAsegurado.SelectedValue + ", " + IdDocumento + ", 1, '" + IdUsuario + "'); " + "\n \n";

                    strQuery += Environment.NewLine;

                }

                if (ddlConclusion.SelectedValue != "0")
                {
                    Variables.wExiste = false;

                    int IdSeccion = 3;
                    int IdCategoria = Convert.ToInt32(ddlConclusion.SelectedValue);

                    // Validar si el Tpo. de Estatus existe no se inserta
                    int iExiste = Validar_Existe_Categoria(IdSeccion, IdCategoria);

                    if (iExiste == 0)   // No Existe
                    {
                        // Estatus
                        strQuery += "INSERT INTO ITM_91 (Referencia, SubReferencia, IdProyecto, IdCliente, IdTpoAsunto, IdSeccion, IdCategoria, IdDocumento, bSeleccion, IdUsuario) " +
                                    "VALUES ('" + Variables.wRef + "', " + Variables.wSubRef + ", " + IdProyecto + ", '" + IdCliente + "', " + IdTpoAsunto + ", 3, " +
                                    " " + ddlConclusion.SelectedValue + ", " + IdDocumento + ", 1, '" + IdUsuario + "'); " + "\n \n";
                    }
                    else
                    {
                        Variables.wExiste = true;
                    }

                }

                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

        }

        protected void Delete_ITM_91()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_91)
                string strQuery = "DELETE FROM ITM_91 " +
                                  " WHERE Referencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = " + Variables.wSubRef + " " +
                                  "   AND IdSeccion NOT IN (3) ";

                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Delete_Doc_Conclusion(int IdSeccion, int IdCategoria)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Verificar si hay registros en ITM_47 con IdDescarga = 0
                string checkQuery = "SELECT COUNT(*) FROM ITM_47 " +
                                    " WHERE UsReferencia = '" + Variables.wRef + "' " +
                                    "   AND SubReferencia = " + Variables.wSubRef + " " +
                                    "   AND IdSeccion = " + IdSeccion + " " +
                                    "   AND IdCategoria = " + IdCategoria + " " +
                                    "   AND IdDescarga = 1;";

                int count = 0;

                using (var reader = dbConn.ExecuteReaderQuery(checkQuery))
                {
                    if (reader.Read()) // Lee el resultado del COUNT
                    {
                        count = reader.GetInt32(0);
                    }

                    // Cerrar el DataReader
                    reader.Close();
                }

                if (count == 0)
                {

                    // Eliminar registro(s) tablas (ITM_91)
                    string strQuery = "DELETE FROM ITM_91 " +
                                    " WHERE Referencia = '" + Variables.wRef + "' " +
                                    "   AND SubReferencia = " + Variables.wSubRef + " " +
                                    "   AND IdSeccion = " + IdSeccion + " " +
                                    "   AND IdCategoria = " + IdCategoria + "; ";

                    strQuery += Environment.NewLine;

                    // Eliminar registro(s) tablas (ITM_47)
                    strQuery += "DELETE FROM ITM_47 " +
                                        " WHERE UsReferencia = '" + Variables.wRef + "' " +
                                        "   AND SubReferencia = " + Variables.wSubRef + " " +
                                        "   AND IdSeccion = " + IdSeccion + " " +
                                        "   AND IdCategoria = " + IdCategoria + " ";

                    int result = dbConn.ExecuteNonQuery(strQuery);

                    dbConn.Close();

                    LblMessage.Text = "Se elimino documento(s), correctamente";
                    mpeMensaje.Show();
                }
                else
                {
                    LblMessage.Text = "No se elimino documento(s), ya existen archivo(s)";
                    mpeMensaje.Show();
                }

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Obtener_Valores_ChBox(GridView gridView, string checkBoxID, int IdSeccion, string sTabla)
        {
            //int IdProyecto = Variables.wIdProyecto;

            //string IdCliente = (string)Session["IdCliente"];
            //string IdTpoAsunto = (string)Session["IdTpoAsunto"];

            string Referencia = Variables.wRef;
            int SubReferencia = Variables.wSubRef;

            int IdProyecto = Variables.wIdProyecto;
            string IdTpoAsunto = Convert.ToString(Variables.wIdTpoAsunto);

            string IdCliente = Variables.wPrefijo_Aseguradora;
            string IdUsuario = Variables.wUserLogon;

            //// bool[] valoresDocs = new bool[18];      // Creamos un array de booleanos para almacenar los valores de los documentos
            // string[] valoresDocs = new string[18];

            // // Inicializar el array con cadenas vacías
            // for (int j = 0; j < valoresDocs.Length; j++)
            // {
            //     valoresDocs[j] = string.Empty;
            // }

            // Declarar una lista para almacenar los textos de los labels asociados a los CheckBoxes seleccionados
            List<string> selectedLabels = new List<string>();

            // Iterar sobre las filas de la GridView
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                // Obtener los CheckBoxes de cada fila
                GridViewRow row = gridView.Rows[i];

                CheckBox chkBox1 = (CheckBox)row.FindControl(checkBoxID + "_1");
                CheckBox chkBox2 = (CheckBox)row.FindControl(checkBoxID + "_2");
                CheckBox chkBox3 = (CheckBox)row.FindControl(checkBoxID + "_3");

                // Recuperar el texto de las celdas asociadas a cada CheckBox y reemplazar &nbsp; con una cadena vacía
                string labelText1 = row.Cells[0].Text.Trim() == "&nbsp;" ? "" : Server.HtmlDecode(row.Cells[0].Text.Trim());  // Asumiendo que el texto está en la primera celda (Columna1)
                string labelText2 = row.Cells[2].Text.Trim() == "&nbsp;" ? "" : Server.HtmlDecode(row.Cells[2].Text.Trim());  // Asumiendo que el texto está en la tercera celda (Columna2)
                string labelText3 = row.Cells[4].Text.Trim() == "&nbsp;" ? "" : Server.HtmlDecode(row.Cells[4].Text.Trim());  // Asumiendo que el texto está en la quinta celda (Columna3)

                // Almacenar los valores de los CheckBoxes en las columnas de la base de datos correspondientes solo si están seleccionados
                if (chkBox1 != null && chkBox1.Checked)
                {
                    selectedLabels.Add(labelText1);
                }
                if (chkBox2 != null && chkBox2.Checked)
                {
                    selectedLabels.Add(labelText2);
                }
                if (chkBox3 != null && chkBox3.Checked)
                {
                    selectedLabels.Add(labelText3);
                }

                //// Determinar las columnas correspondientes en la base de datos
                //int columnaBaseDatos = i / 6;   // Cada conjunto de 6 filas corresponde a una columna en la base de datos
                //int offset = i % 6;             // Determina el desplazamiento dentro de cada conjunto de 6 filas

                //// Almacenar los valores de los CheckBoxes en las columnas de la base de datos correspondientes
                //valoresDocs[columnaBaseDatos * 6 + offset] = row.Cells[0].Text.Trim() == "&nbsp;" ? "" : row.Cells[0].Text.Trim();            // chkBox1.Checked;
                //valoresDocs[columnaBaseDatos * 6 + offset + 6] = row.Cells[2].Text.Trim() == "&nbsp;" ? "" : row.Cells[2].Text.Trim();        // chkBox2.Checked;
                //valoresDocs[columnaBaseDatos * 6 + offset + 12] = row.Cells[4].Text.Trim() == "&nbsp;" ? "" : row.Cells[4].Text.Trim();       // chkBox3.Checked;

            }

            ActualizarDatosEnTabla(Referencia, SubReferencia, IdProyecto, IdCliente, IdSeccion, IdTpoAsunto, IdUsuario, sTabla, selectedLabels);
        }

        private void ActualizarDatosEnTabla(string Referencia, int SubReferencia, int idProyecto, string idCliente, int idSeccion, string idTpoAsunto, string IdUsuario, string sTabla, List<string> selectedLabels)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Obtener el IdDocumento de ITM_82, ITM_84, ITM_85  para cada label
                Dictionary<string, int> idDocumentos = new Dictionary<string, int>();

                foreach (var label in selectedLabels)
                {
                    int idDocumento = GetIdDocumento(label, sTabla);
                    idDocumentos[label] = idDocumento;
                }

                // Implementa la lógica de la función aquí
                foreach (var kvp in idDocumentos)
                {
                    string label = kvp.Key;
                    int idDocumento = kvp.Value;

                    // Crear el comando SQL para la inserción
                    string strQuery = "INSERT INTO ITM_91 (Referencia, SubReferencia, IdCliente, IdProyecto, IdTpoAsunto, IdSeccion, " +
                                      "                    IdCategoria, IdDocumento, bSeleccion, IdUsuario )" +
                                      " VALUES (@Referencia, @SubReferencia, @idCliente, @idProyecto, @idTpoAsunto, @idSeccion, @IdCategoria, 1, 1, @IdUsuario) ";

                    //using (SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD))
                    int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                    {
                        // Agregar el parámetro y su valor
                        // Agregar los parámetros y sus valores
                        cmd.Parameters.AddWithValue("@Referencia", Referencia);
                        cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                        cmd.Parameters.AddWithValue("@idCliente", idCliente);
                        cmd.Parameters.AddWithValue("@idProyecto", idProyecto);
                        cmd.Parameters.AddWithValue("@idTpoAsunto", idTpoAsunto);
                        cmd.Parameters.AddWithValue("@idSeccion", idSeccion);
                        cmd.Parameters.AddWithValue("@IdCategoria", idDocumento);
                        cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);

                        // Ejecutar el comando
                        // cmd.ExecuteNonQuery();
                    });
                }

                dbConn.Close();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public int GetIdDocumento(string label, string Tabla)
        {
            int idDocumento = 0;

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Crear la consulta SQL para recuperar IdDocumento de ITM_82
                string query = $"SELECT IdDocumento FROM { Tabla } WHERE Descripcion = @Label";

                int rowsAffected = dbConn.ExecuteNonQueryWithParameters(query, cmd =>
                {
                    cmd.Parameters.AddWithValue("@Label", label);

                    // Ejecutar la consulta y obtener el resultado
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        idDocumento = Convert.ToInt32(result);
                    }

                });

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

            return idDocumento;
        }

        protected void BtnAceptar_Del_Doc_Click(object sender, EventArgs e)
        {
            int IdSeccion = 3;
            int IdCategoria = Convert.ToInt32(ddlConclusion.SelectedValue);

            Delete_Doc_Conclusion(IdSeccion, IdCategoria);

            GetDocumentos(TxtSubReferencia.Text);
        }

        protected void BtnCancelar_Del_Doc_Click(object sender, EventArgs e)
        {

        }

        protected void ImgDel_Documento_Click(object sender, ImageClickEventArgs e)
        {
            LblMessage_2.Text = "¿Desea eliminar documento(s) estatus ?";
            mpeMensaje_2.Show();
        }

    }
}