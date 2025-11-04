using System;
using System.Collections.Generic;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;
using MySql.Data.MySqlClient;

namespace WebItNow_Peacock
{
    public partial class fwConfiguracion_Proyect : Page
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

                    string flechaHaciaArriba = "\u25B2";
                    btnShowPanel1.Text = flechaHaciaArriba; // Flecha hacia arriba

                    string IdProyecto = Request.QueryString["Proyecto"];
                    int proyectoId;

                    Variables.wIdProyecto = Convert.ToInt32(IdProyecto);

                    GetSeccion_1();
                    GetSeccion_2();
                    GetSeccion_3();
                    GetSeccion_4();
                    GetSeccion_5();

                    if (int.TryParse(IdProyecto, out proyectoId) && proyectoId >= 0)
                    {
                        BtnAgregarDatos.Visible = false;
                        btnEditar.Visible = true;

                        TxtCliente.Text = (string)Session["Cliente"];
                        TxtNomProyecto.Text = (string)Session["Proyecto"];
                        TxtTpoAsunto.Text = (string)Session["TpoAsunto"];

                        // Variables.wIdProyecto = Convert.ToInt32(IdProyecto);

                        // Consulta de datos secciones
                        // GetDatosSeccion(grdSeccion_1, "ChBoxSeccion_1", 1);
                        // GetDatosSeccion(grdSeccion_2, "ChBoxSeccion_2", 2);
                        // GetDatosSeccion(grdSeccion_3, "ChBoxSeccion_3", 3);
                        // GetDatosSeccion(grdSeccion_4, "ChBoxSeccion_4", 4);
                        // GetDatosSeccion(grdSeccion_5, "ChBoxSeccion_5", 5);

                        // Desactivar los CheckBoxes
                        DesactivarCheckBoxes(grdSeccion_1, false);
                        DesactivarCheckBoxes(grdSeccion_2, false);
                        DesactivarCheckBoxes(grdSeccion_3, false);
                        DesactivarCheckBoxes(grdSeccion_4, false);
                        DesactivarCheckBoxes(grdSeccion_5, false);

                        // Desactivar los Buttons
                        BtnSeccion1.Enabled = false;
                        BtnSeccion2.Enabled = false;
                        BtnSeccion3.Enabled = false;
                        BtnSeccion4.Enabled = false;
                        BtnSeccion5.Enabled = false;

                        BtnPnl1Seleccionar_1.Enabled = false;
                        BtnPnl1Seleccionar_2.Enabled = false;
                        BtnPnl1Seleccionar_3.Enabled = false;
                        BtnPnl1Seleccionar_4.Enabled = false;
                        BtnPnl1Seleccionar_5.Enabled = false;

                    }
                    else
                    {
                        TxtCliente.Text = (string)Session["Cliente"];
                        TxtTpoAsunto.Text = (string)Session["TpoAsunto"];
                        TxtNomProyecto.Text = (string)Session["Proyecto"];

                        Variables.wIdProyecto = (int)Session["IdProyecto"];

                    }

                    // GetCiaSeguros();
                    // GetProyecto();
                    // GetTipoAsunto();
                    // GetTipoEstatus();
                    // GetCarpetas();

                    // Forzar el refresh del cliente para asegurarse de que los datos se muestren
                    // ClientScript.RegisterStartupScript(this.GetType(), "refresh", "location.reload();", true);

                    UpdatePanel8.Update();
                    UpdatePanel10.Update();
                    UpdatePanel11.Update();
                    UpdatePanel12.Update();
                    UpdatePanel13.Update();

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
                                        " FROM ITM_67 " +
                                        " WHERE IdSeguros <> 'OTR'" +
                                        "   AND IdStatus = 1 " +
                                        "ORDER BY IdOrden";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                //ddlCliente.DataSource = dt;

                //ddlCliente.DataValueField = "IdSeguros";
                //ddlCliente.DataTextField = "Descripcion";

                //ddlCliente.DataBind();
                //ddlCliente.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

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
                //string IdCliente = ddlCliente.SelectedValue;

                string IdCliente = string.Empty;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdProyecto, Descripcion " +
                                  "  FROM ITM_78 " +
                                  " WHERE IdCliente = '" + IdCliente + "'" +
                                  "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                //ddlProyecto.DataSource = dt;

                //ddlProyecto.DataValueField = "IdProyecto";
                //ddlProyecto.DataTextField = "Descripcion";

                //ddlProyecto.DataBind();
                //ddlProyecto.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

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
                int IdProyecto = Variables.wIdProyecto;
                string IdCliente = (string)Session["IdCliente"];

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT a.IdTpoAsunto, a.Descripcion " +
                                        " FROM ITM_66 as a, ITM_78 as b " +
                                        " WHERE a.IdTpoAsunto = b.IdTpoAsunto " +
                                        "  AND IdProyecto = " + IdProyecto + " " +
                                        "  AND IdCliente = '" + IdCliente + "' " +
                                        "  AND a.IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count != 0)
                {
                    TxtTpoAsunto.Text = dt.Rows[0].ItemArray[1].ToString();
                }

                // ddlTpoAsunto.DataSource = dt;

                // ddlTpoAsunto.DataValueField = "IdTpoAsunto";
                // ddlTpoAsunto.DataTextField = "Descripcion";

                // ddlTpoAsunto.DataBind();
                // ddlTpoAsunto.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        protected void GetTipoEstatus()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                //string sCliente = ddlCliente.SelectedValue;
                //int iProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);

                int IdProyecto = (int)Session["IdProyecto"]; ;
                string IdCliente = (string)Session["IdCliente"];

                string strQuery = "SELECT a.IdEstatus, a.Descripcion, b.Descripcion As IdCliente, c.Descripcion As IdProyecto " +
                                  "  FROM ITM_76 as a, ITM_67 as b, ITM_78 as c " +
                                  " WHERE a.IdCliente = b.IdSeguros " +
                                  "   AND a.IdProyecto = c.IdProyecto " +
                                  "   AND a.IdCliente = '" + IdCliente + "'" +
                                  "   AND a.IdProyecto = " + IdProyecto + "";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                //ddlTpoEstatus.DataSource = dt;

                //ddlTpoEstatus.DataValueField = "IdEstatus";
                //ddlTpoEstatus.DataTextField = "Descripcion";

                //ddlTpoEstatus.DataBind();
                //ddlTpoEstatus.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetCarpetas()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT Id_Carpeta, Nom_Carpeta " +
                                        " FROM ITM_80 " +
                                        " WHERE IdStatus = 1 "
                                        /*+ " WHERE IdAseguradora = " + ddlAseguradora.SelectedValue*/;

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                // ddlCarpetas.DataSource = dt;

                //ddlCarpetas.DataValueField = "Id_Carpeta";
                //ddlCarpetas.DataTextField = "Nom_Carpeta";

                //ddlCarpetas.DataBind();

                //if (dt.Rows.Count == 0)
                //{
                //    ddlCarpetas.Items.Insert(0, new ListItem("-- No Hay Carpeta(s) --", "0"));
                //}
                //else
                //{
                //    ddlCarpetas.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                //}

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetSeccion_1()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string IdTpoAsunto = (string)Session["IdTpoAsunto"];
                string IdCliente = (string)Session["IdCliente"];

                string strQuery = " WITH CTE AS ( SELECT  ITM_81.Descripcion AS Descripcion_81, " +
                                  "  ROW_NUMBER() OVER (ORDER BY ITM_86.IdPosicion) AS RowNumber " +
                                  " FROM ITM_81 " +
                                  "INNER JOIN ITM_86 ON ITM_81.IdDocumento = ITM_86.IdDocumento " +
                                  "WHERE ITM_81.IdStatus = 1 AND ITM_81.IdTpoAsunto = 0 " +
                                  "  AND ITM_86.IdProyecto = " + Variables.wIdProyecto + " AND ITM_86.IdSeccion = 1 " +
                                  "  AND ITM_86.IdTpoAsunto = " + IdTpoAsunto + " AND ITM_86.IdCliente = '" + IdCliente + "' )," +
                                  "      Documentos AS ( SELECT IdDocumento, bSeleccion, " +
                                  "  ROW_NUMBER() OVER (ORDER BY IdPosicion) AS RowNumber " +
                                  " FROM ITM_86 " +
                                  "WHERE IdProyecto = " + Variables.wIdProyecto + " AND IdSeccion = 1 AND IdTpoAsunto = " + IdTpoAsunto + " " +
                                  "  AND IdCliente = '" + IdCliente + "' ) " +
                                  "SELECT COALESCE(CTE1.Descripcion_81, '') AS Columna1, " +
                                  "       COALESCE(D1.bSeleccion, 0) AS ChBoxSeccion_1_1, " +
                                  "       COALESCE(CTE2.Descripcion_81, '') AS Columna2, " +
                                  "       COALESCE(D2.bSeleccion, 0) AS ChBoxSeccion_1_2, " +
                                  "       COALESCE(CTE3.Descripcion_81, '') AS Columna3, " +
                                  "       COALESCE(D3.bSeleccion, 0) AS ChBoxSeccion_1_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN Documentos D1 ON CTE1.RowNumber = D1.RowNumber " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN Documentos D2 ON CTE2.RowNumber = D2.RowNumber " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                  "  LEFT JOIN Documentos D3 ON CTE3.RowNumber = D3.RowNumber " +
                                  " ORDER BY D1.RowNumber ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                dbConn.Close();

                if (dt.Rows.Count == 0)
                {
                    grdSeccion_1.ShowHeaderWhenEmpty = true;
                    grdSeccion_1.EmptyDataText = "No hay resultados.";
                }

                grdSeccion_1.DataSource = dt;
                grdSeccion_1.DataBind();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetSeccion_2()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string IdTpoAsunto = (string)Session["IdTpoAsunto"];
                string IdCliente = (string)Session["IdCliente"];

                string strQuery = " WITH CTE AS ( SELECT  ITM_82.Descripcion AS Descripcion_82, " +
                                  "  ROW_NUMBER() OVER (ORDER BY ITM_86.IdPosicion) AS RowNumber " +
                                  " FROM ITM_82 " +
                                  "INNER JOIN ITM_86 ON ITM_82.IdDocumento = ITM_86.IdDocumento " +
                                  "WHERE ITM_82.IdStatus = 1 AND ITM_82.IdTpoAsunto = 0 " +
                                  "  AND ITM_86.IdProyecto = " + Variables.wIdProyecto + " AND ITM_86.IdSeccion = 2 " +
                                  "  AND ITM_86.IdTpoAsunto = " + IdTpoAsunto + " AND ITM_86.IdCliente = '" + IdCliente + "' )," +
                                  "      Documentos AS ( SELECT IdDocumento, bSeleccion, " +
                                  "  ROW_NUMBER() OVER (ORDER BY IdPosicion) AS RowNumber " +
                                  " FROM ITM_86 " +
                                  "WHERE IdProyecto = " + Variables.wIdProyecto + " AND IdSeccion = 2 AND IdTpoAsunto = " + IdTpoAsunto + " " +
                                  "  AND IdCliente = '" + IdCliente + "' ) " +
                                  "SELECT COALESCE(CTE1.Descripcion_82, '') AS Columna1, " +
                                  "       COALESCE(D1.bSeleccion, 0) AS ChBoxSeccion_2_1, " +
                                  "       COALESCE(CTE2.Descripcion_82, '') AS Columna2, " +
                                  "       COALESCE(D2.bSeleccion, 0) AS ChBoxSeccion_2_2, " +
                                  "       COALESCE(CTE3.Descripcion_82, '') AS Columna3, " +
                                  "       COALESCE(D3.bSeleccion, 0) AS ChBoxSeccion_2_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN Documentos D1 ON CTE1.RowNumber = D1.RowNumber " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN Documentos D2 ON CTE2.RowNumber = D2.RowNumber " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                  "  LEFT JOIN Documentos D3 ON CTE3.RowNumber = D3.RowNumber " +
                                  " ORDER BY D1.RowNumber ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                dbConn.Close();

                if (dt.Rows.Count == 0)
                {
                    grdSeccion_2.ShowHeaderWhenEmpty = true;
                    grdSeccion_2.EmptyDataText = "No hay resultados.";
                }

                grdSeccion_2.DataSource = dt;
                grdSeccion_2.DataBind();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetSeccion_3()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string IdTpoAsunto = (string)Session["IdTpoAsunto"];
                string IdCliente = (string)Session["IdCliente"];

                string strQuery = " WITH CTE AS ( SELECT  ITM_83.Descripcion AS Descripcion_83, " +
                                  "  ROW_NUMBER() OVER (ORDER BY ITM_86.IdPosicion) AS RowNumber " +
                                  " FROM ITM_83 " +
                                  "INNER JOIN ITM_86 ON ITM_83.IdDocumento = ITM_86.IdDocumento " +
                                  "WHERE ITM_83.IdStatus = 1 AND ITM_83.IdTpoAsunto = 0 " +
                                  "  AND ITM_86.IdProyecto = " + Variables.wIdProyecto + " AND ITM_86.IdSeccion = 3 " +
                                  "  AND ITM_86.IdTpoAsunto = " + IdTpoAsunto + " AND ITM_86.IdCliente = '" + IdCliente + "' )," +
                                  "      Documentos AS ( SELECT IdDocumento, bSeleccion, " +
                                  "  ROW_NUMBER() OVER (ORDER BY IdPosicion) AS RowNumber " +
                                  " FROM ITM_86 " +
                                  "WHERE IdProyecto = " + Variables.wIdProyecto + " AND IdSeccion = 3 AND IdTpoAsunto = " + IdTpoAsunto + " " +
                                  "  AND IdCliente = '" + IdCliente + "' ) " +
                                  "SELECT COALESCE(CTE1.Descripcion_83, '') AS Columna1, " +
                                  "       COALESCE(D1.bSeleccion, 0) AS ChBoxSeccion_3_1, " +
                                  "       COALESCE(CTE2.Descripcion_83, '') AS Columna2, " +
                                  "       COALESCE(D2.bSeleccion, 0) AS ChBoxSeccion_3_2, " +
                                  "       COALESCE(CTE3.Descripcion_83, '') AS Columna3, " +
                                  "       COALESCE(D3.bSeleccion, 0) AS ChBoxSeccion_3_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN Documentos D1 ON CTE1.RowNumber = D1.RowNumber " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN Documentos D2 ON CTE2.RowNumber = D2.RowNumber " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                  "  LEFT JOIN Documentos D3 ON CTE3.RowNumber = D3.RowNumber " +
                                  " ORDER BY D1.RowNumber ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                dbConn.Close();

                if (dt.Rows.Count == 0)
                {
                    grdSeccion_3.ShowHeaderWhenEmpty = true;
                    grdSeccion_3.EmptyDataText = "No hay resultados.";
                }

                grdSeccion_3.DataSource = dt;
                grdSeccion_3.DataBind();

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

                string IdTpoAsunto = (string)Session["IdTpoAsunto"];
                string IdCliente = (string)Session["IdCliente"];

                string strQuery = " WITH CTE AS ( SELECT  ITM_84.Descripcion AS Descripcion_84, " +
                                  "  ROW_NUMBER() OVER (ORDER BY ITM_86.IdPosicion) AS RowNumber " +
                                  " FROM ITM_84 " +
                                  "INNER JOIN ITM_86 ON ITM_84.IdDocumento = ITM_86.IdDocumento " +
                                  "WHERE ITM_84.IdStatus = 1 AND ITM_84.IdTpoAsunto = 0 " +
                                  "  AND ITM_86.IdProyecto = " + Variables.wIdProyecto + " AND ITM_86.IdSeccion = 4 " +
                                  "  AND ITM_86.IdTpoAsunto = " + IdTpoAsunto + " AND ITM_86.IdCliente = '" + IdCliente + "' )," +
                                  "      Documentos AS ( SELECT IdDocumento, bSeleccion, " +
                                  "  ROW_NUMBER() OVER (ORDER BY IdPosicion) AS RowNumber " +
                                  " FROM ITM_86 " +
                                  "WHERE IdProyecto = " + Variables.wIdProyecto + " AND IdSeccion = 4 AND IdTpoAsunto = " + IdTpoAsunto + " " +
                                  "  AND IdCliente = '" + IdCliente + "' ) " +
                                  "SELECT COALESCE(CTE1.Descripcion_84, '') AS Columna1, " +
                                  "       COALESCE(D1.bSeleccion, 0) AS ChBoxSeccion_4_1, " +
                                  "       COALESCE(CTE2.Descripcion_84, '') AS Columna2, " +
                                  "       COALESCE(D2.bSeleccion, 0) AS ChBoxSeccion_4_2, " +
                                  "       COALESCE(CTE3.Descripcion_84, '') AS Columna3, " +
                                  "       COALESCE(D3.bSeleccion, 0) AS ChBoxSeccion_4_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN Documentos D1 ON CTE1.RowNumber = D1.RowNumber " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN Documentos D2 ON CTE2.RowNumber = D2.RowNumber " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                  "  LEFT JOIN Documentos D3 ON CTE3.RowNumber = D3.RowNumber " +
                                  " ORDER BY D1.RowNumber ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                dbConn.Close();

                if (dt.Rows.Count == 0)
                {
                    grdSeccion_4.ShowHeaderWhenEmpty = true;
                    grdSeccion_4.EmptyDataText = "No hay resultados.";
                }

                grdSeccion_4.DataSource = dt;
                grdSeccion_4.DataBind();

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

                string IdTpoAsunto = (string)Session["IdTpoAsunto"];
                string IdCliente = (string)Session["IdCliente"];

                string strQuery = " WITH CTE AS ( SELECT  ITM_85.Descripcion AS Descripcion_85, " +
                                  "  ROW_NUMBER() OVER (ORDER BY ITM_86.IdPosicion) AS RowNumber " +
                                  " FROM ITM_85 " +
                                  "INNER JOIN ITM_86 ON ITM_85.IdDocumento = ITM_86.IdDocumento " +
                                  "WHERE ITM_85.IdStatus = 1 AND ITM_85.IdTpoAsunto = 0 " +
                                  "  AND ITM_86.IdProyecto = " + Variables.wIdProyecto + " AND ITM_86.IdSeccion = 5 " +
                                  "  AND ITM_86.IdTpoAsunto = " + IdTpoAsunto + " AND ITM_86.IdCliente = '" + IdCliente + "' )," +
                                  "      Documentos AS ( SELECT IdDocumento, bSeleccion, " +
                                  "  ROW_NUMBER() OVER (ORDER BY IdPosicion) AS RowNumber " +
                                  " FROM ITM_86 " +
                                  "WHERE IdProyecto = " + Variables.wIdProyecto + " AND IdSeccion = 5 AND IdTpoAsunto = " + IdTpoAsunto + " " +
                                  "  AND IdCliente = '" + IdCliente + "' ) " +
                                  "SELECT COALESCE(CTE1.Descripcion_85, '') AS Columna1, " +
                                  "       COALESCE(D1.bSeleccion, 0) AS ChBoxSeccion_5_1, " +
                                  "       COALESCE(CTE2.Descripcion_85, '') AS Columna2, " +
                                  "       COALESCE(D2.bSeleccion, 0) AS ChBoxSeccion_5_2, " +
                                  "       COALESCE(CTE3.Descripcion_85, '') AS Columna3, " +
                                  "       COALESCE(D3.bSeleccion, 0) AS ChBoxSeccion_5_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN Documentos D1 ON CTE1.RowNumber = D1.RowNumber " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN Documentos D2 ON CTE2.RowNumber = D2.RowNumber " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                  "  LEFT JOIN Documentos D3 ON CTE3.RowNumber = D3.RowNumber " +
                                  " ORDER BY D1.RowNumber ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                dbConn.Close();

                if (dt.Rows.Count == 0)
                {
                    grdSeccion_5.ShowHeaderWhenEmpty = true;
                    grdSeccion_5.EmptyDataText = "No hay resultados.";
                }

                grdSeccion_5.DataSource = dt;
                grdSeccion_5.DataBind();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetDatosSeccion(GridView gridView, string checkBoxID, int IdSeccion)
        {
            try
            {
                int IdProyecto = Variables.wIdProyecto;
                string IdCliente = (string)Session["IdCliente"];
                string IdTpoAsunto = (string)Session["IdTpoAsunto"];

                // int IdCarpeta = Convert.ToInt32(ddlCarpetas.SelectedValue);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // obtener datos de la tabla ITM_86. para la Seccion : Asegurados a los que aplica(n) este/os Documentos(s).
                string strQuery = "SELECT IdPosicion, IdDocumento, bSeleccion" +
                                  "  FROM ITM_86 WHERE IdProyecto = " + IdProyecto + " " +
                                  "   AND IdCliente = '" + IdCliente + "' " +
                                  "   AND IdSeccion = " + IdSeccion + " " +
                                  "   AND IdTpoAsunto = " + IdTpoAsunto + " " +
                                  "   AND IdStatus = 1 ";

                MySqlDataReader dr = dbConn.ExecuteReaderQuery(strQuery);

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //SqlDataReader dr = cmd.ExecuteReader();

                // Diccionario para mapear IdPosicion a fila y columna en los CheckBoxes
                Dictionary<int, Tuple<int, int>> posicionMap = new Dictionary<int, Tuple<int, int>>();

                int fila = 0;
                int columna = 0; // Inicializar columna
                string checkBoxID_Columna = ""; // Inicializar checkBoxID_Columna fuera del bucle

                // Procesar los datos del SqlDataReader y llenar los CheckBoxes
                while (dr.Read())
                {
                    int idPosicion = dr.GetInt32(0);        // Obtener el IdPosicion de la fila actual
                    int bSeleccionInt = dr.GetInt32(2);     // Obtener el valor entero de bSeleccion

                    // Convertir el valor entero a un valor booleano (1=true, 0=false)
                    bool bSeleccion = bSeleccionInt == 1;

                    // Calcular la fila y columna correspondientes para esta IdPosicion
                    fila = (idPosicion - 1) / 3;        // Calcular la fila (el índice de fila comienza en 0)
                    columna = (idPosicion - 1) % 3;     // Calcular la columna (el índice de columna comienza en 0)

                    // Actualizar checkBoxID_Columna solo si es el comienzo de una nueva serie de filas
                    if (fila % 6 == 0) // Comienza una nueva serie de filas
                    {
                        checkBoxID_Columna = $"{checkBoxID}_{columna + 1}";
                    }

                    // Verificar si la fila está dentro del rango del GridView
                    if (fila >= 0 && fila < gridView.Rows.Count)
                    {
                        // Encontrar el CheckBox en la fila y columna correspondientes
                        CheckBox checkBox = (CheckBox)gridView.Rows[fila].FindControl(checkBoxID_Columna);

                        // Verificar si el CheckBox se encontró correctamente
                        if (checkBox != null)
                        {
                            // Establecer el valor del CheckBox según bSeleccion
                            checkBox.Checked = bSeleccion;
                        }
                    }

                    // Agregar la relación IdPosicion -> fila y columna al diccionario
                    posicionMap.Add(idPosicion, new Tuple<int, int>(fila, columna));

                    // Incrementar fila después de procesar los datos de esta fila
                    fila++;
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void Obtener_Valores_ChBox(GridView gridView, string checkBoxID, int IdSeccion)
        {
            int IdProyecto = Variables.wIdProyecto;

            string IdCliente = (string)Session["IdCliente"];
            string IdTpoAsunto = (string)Session["IdTpoAsunto"];

            bool[] valoresDocs = new bool[24];      // Creamos un array de booleanos para almacenar los valores de los documentos

            // Iterar sobre las filas de la GridView
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                // Obtener los CheckBoxes de cada fila
                GridViewRow row = gridView.Rows[i];

                CheckBox chkBox1 = (CheckBox)row.FindControl(checkBoxID + "_1");
                CheckBox chkBox2 = (CheckBox)row.FindControl(checkBoxID + "_2");
                CheckBox chkBox3 = (CheckBox)row.FindControl(checkBoxID + "_3");

                // Determinar las columnas correspondientes en la base de datos
                int columnaBaseDatos = i / 8;   // Cada conjunto de 6 filas corresponde a una columna en la base de datos
                int offset = i % 8;             // Determina el desplazamiento dentro de cada conjunto de 6 filas

                // Almacenar los valores de los CheckBoxes en las columnas de la base de datos correspondientes
                valoresDocs[columnaBaseDatos * 8 + offset] = chkBox1.Checked;
                valoresDocs[columnaBaseDatos * 8 + offset + 8] = chkBox2.Checked;
                valoresDocs[columnaBaseDatos * 8 + offset + 16] = chkBox3.Checked;

                // Almacenar los valores de los CheckBoxes en el array
                // valoresDocs[i * 3] = chkBox1.Checked;
                // valoresDocs[i * 3 + 1] = chkBox2.Checked;
                // valoresDocs[i * 3 + 2] = chkBox3.Checked;
            }

            ActualizarDatosEnTabla(IdProyecto, IdCliente, IdSeccion, IdTpoAsunto, valoresDocs);
        }

        // Método para actualizar los datos en la tabla ITM_86
        private void ActualizarDatosEnTabla(int idProyecto, string idCliente, int idSeccion, string idTpoAsunto, bool[] valoresDocs)
        {
            try
            {

                //ConexionBD Conecta = new ConexionBD();
                //Conecta.Abrir();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                StringBuilder posiciones = new StringBuilder(); // StringBuilder para construir la cadena

                // Recorremos el arreglo para encontrar los valores true y sus posiciones
                for (int i = 0; i < valoresDocs.Length; i++)
                {
                    if (valoresDocs[i]) // Si el valor en esta posición es true
                    {
                        if (posiciones.Length > 0)
                        {
                            posiciones.Append(", ");    // Agrega una coma y un espacio si ya hay elementos en la cadena
                        }
                        posiciones.Append(i + 1);       // Agrega la posición a la cadena
                    }
                }

                // Inicializar datos tabla ITM_86

                string strQuery = $@"UPDATE ITM_86 
                                            SET bSeleccion = 0 
                                          WHERE IdProyecto = { idProyecto }
                                            AND IdCliente = '{ idCliente }'
                                            AND IdSeccion = { idSeccion }
                                        AND IdTpoAsunto = { idTpoAsunto } ;";

                if (posiciones.Length != 0)
                {

                    string posicionesStr = $"({posiciones})";

                    //int idPosicion = 1;

                    //// Generar la consulta SQL UPDATE
                    ///
                    //string strQuery = $@"UPDATE ITM_86 
                    //                        SET bSeleccion = 0 
                    //                      WHERE [IdProyecto] = { idProyecto }
                    //                        AND [IdCliente] = '{ idCliente }'
                    //                        AND [IdSeccion] = { idSeccion }
                    //                    AND [IdTpoAsunto] = { idTpoAsunto } ";

                    strQuery += Environment.NewLine;

                    strQuery += $@"UPDATE ITM_86 
                                      SET bSeleccion = 1 
                                    WHERE IdPosicion IN { posicionesStr}
                                      AND IdProyecto = { idProyecto }
                                      AND IdCliente = '{ idCliente }'
                                      AND IdSeccion = { idSeccion }
                                      AND IdTpoAsunto = { idTpoAsunto } ";

                    //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                    //int affectedRows = cmd.ExecuteNonQuery();

                }

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //int affectedRows = cmd.ExecuteNonQuery();

            } catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ddlProyecto.ClearSelection();

            TxtTpoAsunto.Text = string.Empty;
            // ddlTpoEstatus.ClearSelection();
            // ddlCarpetas.ClearSelection();

            BtnAgregarDatos.Enabled = false;

            // btnEliminarDatos.Enabled = false;
            // btnEditarDatos.Enabled = false;

            // pnl1.Visible = false;

            InicializarCheckBoxes();

            // GetProyecto();
        }

        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTipoAsunto();
            GetTipoEstatus();

            //ddlCarpetas.ClearSelection();

            BtnAgregarDatos.Enabled = false;
            //btnEliminarDatos.Enabled = false;
            //btnEditarDatos.Enabled = false;

            // pnl1.Visible = false;

            InicializarCheckBoxes();

        }

        protected void ddlCarpetas_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iExiste = ExisteCarpeta();

            if (iExiste == 1)
            {
                // Consulta de datos secciones
                GetDatosSeccion(grdSeccion_1, "ChBoxSeccion_1", 1);
                GetDatosSeccion(grdSeccion_2, "ChBoxSeccion_2", 2);
                GetDatosSeccion(grdSeccion_3, "ChBoxSeccion_3", 3);
                GetDatosSeccion(grdSeccion_4, "ChBoxSeccion_4", 4);
                GetDatosSeccion(grdSeccion_5, "ChBoxSeccion_5", 5);

                pnl1.Visible = true;

                BtnAgregarDatos.Enabled = false;
                //btnEliminarDatos.Enabled = true;
                //btnEditarDatos.Enabled = true;
            }
            else
            {
                // Inicializar Secciones
                GetSeccion_1();
                GetSeccion_2();
                GetSeccion_3();
                GetSeccion_4();
                GetSeccion_5();

                pnl1.Visible = false;

                BtnAgregarDatos.Enabled = true;
                //btnEliminarDatos.Enabled = false;
                //btnEditarDatos.Enabled = false;

                LblMessage.Text = "No existe carpeta, para este proyecto";
                mpeMensaje.Show();

            }

        }

        private void InicializarCheckBoxes()
        {
            // Iterar sobre las filas de la GridView
            for (int i = 0; i < grdSeccion_1.Rows.Count; i++)
            {
                // Obtener los CheckBoxes de cada fila
                GridViewRow row = grdSeccion_1.Rows[i];
                CheckBox chkBox1 = (CheckBox)row.FindControl("ChBoxSeccion_1_1");
                CheckBox chkBox2 = (CheckBox)row.FindControl("ChBoxSeccion_1_2");
                CheckBox chkBox3 = (CheckBox)row.FindControl("ChBoxSeccion_1_3");

                // Inicializar los CheckBoxes
                chkBox1.Checked = false;
                chkBox2.Checked = false;
                chkBox3.Checked = false;
            }

            for (int i = 0; i < grdSeccion_2.Rows.Count; i++)
            {
                // Obtener los CheckBoxes de cada fila
                GridViewRow row = grdSeccion_2.Rows[i];
                CheckBox chkBox1 = (CheckBox)row.FindControl("ChBoxSeccion_2_1");
                CheckBox chkBox2 = (CheckBox)row.FindControl("ChBoxSeccion_2_2");
                CheckBox chkBox3 = (CheckBox)row.FindControl("ChBoxSeccion_2_3");

                // Inicializar los CheckBoxes
                chkBox1.Checked = false;
                chkBox2.Checked = false;
                chkBox3.Checked = false;
            }

            for (int i = 0; i < grdSeccion_3.Rows.Count; i++)
            {
                // Obtener los CheckBoxes de cada fila
                GridViewRow row = grdSeccion_3.Rows[i];
                CheckBox chkBox1 = (CheckBox)row.FindControl("ChBoxSeccion_3_1");
                CheckBox chkBox2 = (CheckBox)row.FindControl("ChBoxSeccion_3_2");
                CheckBox chkBox3 = (CheckBox)row.FindControl("ChBoxSeccion_3_3");

                // Inicializar los CheckBoxes
                chkBox1.Checked = false;
                chkBox2.Checked = false;
                chkBox3.Checked = false;
            }

            for (int i = 0; i < grdSeccion_4.Rows.Count; i++)
            {
                // Obtener los CheckBoxes de cada fila
                GridViewRow row = grdSeccion_4.Rows[i];
                CheckBox chkBox1 = (CheckBox)row.FindControl("ChBoxSeccion_4_1");
                CheckBox chkBox2 = (CheckBox)row.FindControl("ChBoxSeccion_4_2");
                CheckBox chkBox3 = (CheckBox)row.FindControl("ChBoxSeccion_4_3");

                // Inicializar los CheckBoxes
                chkBox1.Checked = false;
                chkBox2.Checked = false;
                chkBox3.Checked = false;
            }

            for (int i = 0; i < grdSeccion_5.Rows.Count; i++)
            {
                // Obtener los CheckBoxes de cada fila
                GridViewRow row = grdSeccion_5.Rows[i];
                CheckBox chkBox1 = (CheckBox)row.FindControl("ChBoxSeccion_5_1");
                CheckBox chkBox2 = (CheckBox)row.FindControl("ChBoxSeccion_5_2");
                CheckBox chkBox3 = (CheckBox)row.FindControl("ChBoxSeccion_5_3");

                // Inicializar los CheckBoxes
                chkBox1.Checked = false;
                chkBox2.Checked = false;
                chkBox3.Checked = false;
            }

        }

        protected void ddlTpoStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ddlCarpetas.ClearSelection();

            BtnAgregarDatos.Enabled = false;
            //btnEliminarDatos.Enabled = false;
            //btnEditarDatos.Enabled = false;
        }

        protected void Add_tbCarpeta()
        {
            try
            {
                // if (ddlCliente.SelectedValue == "0")
                // {
                //     LblMessage.Text = "Seleccionar Nombre del Cliente";
                //     mpeMensaje.Show();
                //     return;
                // }
                // else if (ddlProyecto.SelectedValue == "0")
                // {
                //     LblMessage.Text = "Seleccionar Nombre del Proyecto";
                //     mpeMensaje.Show();
                //     return;
                // }
                // else if (ddlTpoEstatus.SelectedValue == "0")
                // {
                //     LblMessage.Text = "Seleccionar Tipo de Estatus";
                //     mpeMensaje.Show();
                //     return;
                // }
                // else if (ddlCarpetas.SelectedValue == "0")
                // {
                //     LblMessage.Text = "Seleccionar Nombre de la Carpeta";
                //     mpeMensaje.Show();
                //     return;
                // }


                string sUsuario = Variables.wUserLogon;

                string sReferencia = string.Empty;

                DateTime fechaActual = DateTime.Today;
                string fechaActualFormateada = fechaActual.ToString("dd/MM/yyyy");

                string IdTpoAsunto = (string)Session["IdTpoAsunto"];

                int iConsecutivo = 0;

                if (TxtNomProyecto.Text != "NINGUNO")
                {

                    // DateTime fechaActual = DateTime.Today;
                    // fechaActualFormateada = fechaActual.ToString("yyyy-MM-dd");
                    DateTime fecha = DateTime.ParseExact(fechaActualFormateada, "dd/MM/yyyy", null);

                    string sIdSeguros = (string)Session["IdCliente"];
                    iConsecutivo = obtenerConsecutivo("fwAlta_Asunto");

                    // IdTpoAsunto = (string)Session["IdTpoAsunto"];

                    if (IdTpoAsunto == "1")
                    {
                        sReferencia = "T-" + iConsecutivo + "-" + sIdSeguros + fecha.ToString("yy");
                    }
                    else if (IdTpoAsunto == "2")
                    {
                        sReferencia = "SS-" + iConsecutivo + "-" + sIdSeguros + fecha.ToString("yy");
                    }
                    else if (IdTpoAsunto == "3")
                    {
                        sReferencia = "SC-" + iConsecutivo + "-" + sIdSeguros + fecha.ToString("yy");
                    }
                    else if (IdTpoAsunto == "4")
                    {
                        sReferencia = "L-" + iConsecutivo + "-" + sIdSeguros + fecha.ToString("yy");
                    }

                }
                // string sIdCliente = ddlCliente.SelectedValue;
                // int iProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);
                // int iIdEstatus = Convert.ToInt32(ddlTpoEstatus.SelectedValue);
                // int iIdCarpeta = Convert.ToInt32(ddlCarpetas.SelectedValue);

                string sIdCliente = (string)Session["IdCliente"];
                // int iProyecto = (int)Session["IdProyecto"];
                int iProyecto = Variables.wIdProyecto;
                string iGerente = (string)Session["IdGerente"];
                int iTpoGestion = (int)Session["IdTpoGestion"];
                string sDescProyecto = (string)Session["DescProyecto"];

                string sNumPoliza = (string)Session["Poliza"];
                string sNomAsegurado = (string)Session["NomAsegurado"];
                int iTpoAsegurado = (int)Session["TpoAsegurado"];
                string sIniVigencia = (string)Session["IniVigencia"];
                string sFinVigencia = (string)Session["FinVigencia"];


                //ConexionBD Conecta = new ConexionBD();
                //Conecta.Abrir();
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_78)
                string strQuery = "INSERT INTO ITM_78 (IdProyecto, Referencia, SubReferencia, Descripcion, IdCliente, IdTpoAsunto, Descripcion_Proyecto, Gerente_Responsable, IdTpoGestion, NumPoliza, " +
                                  " NomAsegurado, TpoAsegurado, FecIni_Vigencia, FecFin_Vigencia, Id_Usuario, IdStatus) " +
                                  " VALUES (" + iProyecto + ", '" + sReferencia + "', 0, '" + TxtNomProyecto.Text.Trim() + "', '" + sIdCliente + "', '" + IdTpoAsunto + "', " +
                                  "'" + sDescProyecto + "', '" + iGerente + "', " + iTpoGestion + ", '" + sNumPoliza + "', " +
                                  "'" + sNomAsegurado + "', " + iTpoAsegurado + ", '" + sIniVigencia + "', '" + sFinVigencia + "', '" + sUsuario + "', 1);" + "\n \n";

                if (TxtNomProyecto.Text != "NINGUNO")
                {
                    strQuery += Environment.NewLine;

                    iConsecutivo++;
                    strQuery += "UPDATE ITM_71 SET IdConsecutivo = " + iConsecutivo + " WHERE IdProceso = 'fwAlta_Asunto' ;";

                }

                strQuery += Environment.NewLine;

                strQuery += "INSERT INTO ITM_70 (Referencia, SubReferencia, NumPoliza, IdSeguros, IdTpoAsunto, IdProyecto, IdRegimen, Fecha_Asignacion, " +
                            " NomAsegurado, IdConclusion, IdTpoProyecto, Id_Usuario, IdStatus) " +
                            "  VALUES ('" + sReferencia + "', 0, '" + sNumPoliza + "', '" + sIdCliente + "', '" + IdTpoAsunto + "', " +
                            " " + iProyecto + ", 1, '" + fechaActualFormateada + "', '" + sNomAsegurado + "', 4, 1, '" + sUsuario + "', 1)";


                //strQuery += Environment.NewLine;

                //// Insertar registro tabla (ITM_86)
                //strQuery += "INSERT INTO ITM_86 (IdProyecto, IdCliente, IdSeccion, IdEstatus, IdCarpeta, " +
                //                  " Id_Usuario, IdStatus) " +
                //                  " VALUES (" + iProyecto + ", '" + sIdCliente + "', 1, 0, 0, " +
                //                  "'" + sUsuario + "', 1)" + "\n \n";

                //strQuery += Environment.NewLine;

                //strQuery += "INSERT INTO ITM_86 (IdProyecto, IdCliente, IdSeccion, IdEstatus, IdCarpeta, " +
                //                  " Id_Usuario, IdStatus) " +
                //                  " VALUES (" + iProyecto + ", '" + sIdCliente + "', 2,  0, 0, " +
                //                  "'" + sUsuario + "', 1)" + "\n \n";

                //strQuery += Environment.NewLine;

                //strQuery += "INSERT INTO ITM_86 (IdProyecto, IdCliente, IdSeccion, IdEstatus, IdCarpeta, " +
                //                  " Id_Usuario, IdStatus) " +
                //                  " VALUES (" + iProyecto + ", '" + sIdCliente + "', 3,  0, 0, " +
                //                  "'" + sUsuario + "', 1)" + "\n \n";

                //strQuery += Environment.NewLine;

                //strQuery += "INSERT INTO ITM_86 (IdProyecto, IdCliente, IdSeccion, IdEstatus, IdCarpeta, " +
                //                  " Id_Usuario, IdStatus) " +
                //                  " VALUES (" + iProyecto + ", '" + sIdCliente + "', 4,  0, 0, " +
                //                  "'" + sUsuario + "', 1)" + "\n \n";

                //strQuery += Environment.NewLine;

                //strQuery += "INSERT INTO ITM_86 (IdProyecto, IdCliente, IdSeccion, IdEstatus, IdCarpeta, " +
                //                  " Id_Usuario, IdStatus) " +
                //                  " VALUES (" + iProyecto + ", '" + sIdCliente + "', 5,  0, 0, " +
                //                  "'" + sUsuario + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //int affectedRows = cmd.ExecuteNonQuery();

                //Conecta.Cerrar();
                //cmd.Dispose();

                // LblMessage.Text = "Se agrego nueva, carpeta para proyecto, correctamente ";
                // mpeMensaje.Show();
                if (TxtNomProyecto.Text != "NINGUNO")
                {
                    LblMessage.Text = "Se agrego nuevo proyecto, correctamente " + "<br />" + "Num. Referencia : " + sReferencia;
                } else
                {
                    LblMessage.Text = "Se agrego nuevo proyecto, correctamente ";
                }

                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public int obtenerConsecutivo(string IdProceso)
        {
            try
            {
                int IdArchivoMax = 0;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdConsecutivo FROM ITM_71" +
                                  " WHERE IdProceso = '" + IdProceso + "'";

                MySqlDataReader dr = dbConn.ExecuteReaderQuery(strQuery);

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        IdArchivoMax = Convert.ToInt32(dr["IdConsecutivo"].ToString().Trim());
                    }
                }

                dbConn.Close();

                //Conecta.Cerrar();
                //cmd.Dispose();
                //dr.Dispose();

                return IdArchivoMax;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

            return -1;
        }

        protected void Renumerar_ITM_86()
        {
            try
            {
                int IdProyecto = Variables.wIdProyecto;
                string IdCliente = (string)Session["IdCliente"];
                string IdTpoAsunto = (string)Session["IdTpoAsunto"];

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "";

                strQuery = "  WITH Renumbered AS ( SELECT IdPosicion, " +
                           "       ROW_NUMBER() OVER (ORDER BY IdPosicion) AS NewId " +
                           "  FROM ITM_86 " +
                           " WHERE IdProyecto = " + IdProyecto + " AND IdCliente = '" + IdCliente + "' " +
                           "   AND IdSeccion = " + Variables.wSeccion + " AND IdTpoAsunto = " + IdTpoAsunto + " ) " +
                           "UPDATE ITM_86 " +
                           "   SET IdPosicion = NewId " +
                           "  FROM ITM_86 " +
                           "  JOIN Renumbered ON ITM_86.IdPosicion = Renumbered.IdPosicion " +
                           "   AND IdProyecto = " + IdProyecto + " AND IdCliente = '" + IdCliente + "' " +
                           "   AND IdSeccion = " + Variables.wSeccion + " AND IdTpoAsunto = " + IdTpoAsunto + " ";

                // Insert en la tabla Estado de Documento
                MySqlDataReader dr = dbConn.ExecuteReaderQuery(strQuery);

                dbConn.Close();
                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //SqlDataReader dr = cmd.ExecuteReader();

                //Conecta.Cerrar();

                //cmd.Dispose();
                //dr.Dispose();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        protected void Del_tbCarpeta(int iIdDocumento)
        {
            try
            {
                int IdProyecto = Variables.wIdProyecto ;
                string IdCliente = (string)Session["IdCliente"];
                string IdTpoAsunto = (string)Session["IdTpoAsunto"];

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "DELETE FROM ITM_86 " +
                                  " WHERE IdStatus = 1 " +
                                  "   AND IdProyecto = " + IdProyecto + " " +
                                  "   AND IdCliente = '" + IdCliente + "' " +
                                  "   AND IdSeccion = " + Variables.wSeccion + " " +
                                  "   AND IdTpoAsunto = " + IdTpoAsunto + " " +
                                  "   AND IdDocumento = " + iIdDocumento + " ;";

                strQuery += Environment.NewLine;

                strQuery += "  WITH Renumbered AS ( " +
                            "SELECT IdPosicion, ROW_NUMBER() OVER (ORDER BY IdPosicion) AS NewId " +
                            "  FROM ITM_86 " +
                            " WHERE IdProyecto = " + IdProyecto + " " +
                            "   AND IdCliente = '" + IdCliente + "' " +
                            "   AND IdSeccion = " + Variables.wSeccion + " " +
                            "   AND IdTpoAsunto = " + IdTpoAsunto + " ) " +
                            "UPDATE ITM_86 SET IdPosicion = ( SELECT Renumbered.NewId FROM Renumbered WHERE Renumbered.IdPosicion = ITM_86.IdPosicion ) " +
                            "WHERE ITM_86.IdProyecto = " + IdProyecto + " " +
                            "  AND ITM_86.IdCliente = '" + IdCliente + "' " +
                            "  AND ITM_86.IdSeccion = " + Variables.wSeccion + " " +
                            "  AND ITM_86.IdTpoAsunto = " + IdTpoAsunto + "; ";

                //strQuery += "; WITH Renumbered AS ( SELECT IdPosicion, " +
                //            "       ROW_NUMBER() OVER (ORDER BY IdPosicion) AS NewId " +
                //            "  FROM ITM_86 " +
                //            " WHERE IdProyecto = " + IdProyecto + " AND IdCliente = '" + IdCliente + "' " +
                //            "   AND IdSeccion = " + Variables.wSeccion + " AND IdTpoAsunto = " + IdTpoAsunto + " ) " +
                //            "UPDATE ITM_86 " +
                //            "   SET IdPosicion = NewId " +
                //            "  FROM ITM_86 " +
                //            "  JOIN Renumbered ON ITM_86.IdPosicion = Renumbered.IdPosicion " +
                //            "   AND IdProyecto = " + IdProyecto + " AND IdCliente = '" + IdCliente + "' " +
                //            "   AND IdSeccion = " + Variables.wSeccion + " AND IdTpoAsunto = " + IdTpoAsunto + " ";



                // Insert en la tabla Estado de Documento
                MySqlDataReader dr = dbConn.ExecuteReaderQuery(strQuery);

                dbConn.Close();

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //SqlDataReader dr = cmd.ExecuteReader();

                //Conecta.Cerrar();

                //cmd.Dispose();
                //dr.Dispose();

            }

            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnCerrar_New_Click(object sender, EventArgs e)
        {

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

        protected void btnCrear_Click(object sender, EventArgs e)
        {
            //var email = new EnvioEmail();
            //string sPredeterminado = string.Empty;

            //sPredeterminado = "Estimado Cliente. \n" +
            //                  "Enviamos este mensaje para hacer de su conocimiento que es necesario nos remita a la brevedad, \n";

            //string sBody = sPredeterminado + " \n Observaciones registradas : \n" + "Pruebas";

            //string sCuerpo = "IMPORTANTE: Se solicita documentación para la atención de su reporte " + "XXX";
            //int Envio_Ok = email.EnvioMensaje("XXX", "martin.baltierra@itnow.mx", sCuerpo, sBody);
        }

        public int ExisteCarpeta()
        {
            try
            {
                int IdProyecto = (int)Session["IdProyecto"]; ;
                string IdCliente = (string)Session["IdCliente"];


                // int IdProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);
                // string IdCliente = ddlCliente.SelectedValue;
                // int IdCarpeta = Convert.ToInt32(ddlCarpetas.SelectedValue);
                // int iIdEstatus = Convert.ToInt32(ddlTpoEstatus.SelectedValue);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdProyecto, IdCliente, IdCarpeta " +
                                  "  FROM ITM_86 t0 " +
                                  " WHERE t0.IdStatus = 1 " +
                                  "   AND IdProyecto = " + IdProyecto + " " +
                                  "   AND IdCliente = '" + IdCliente + "' ";
                // "   AND IdCarpeta = " + IdCarpeta + " " +
                // "   AND IdEstatus = " + iIdEstatus + " ";


                // Insert en la tabla Estado de Documento
                MySqlDataReader dr = dbConn.ExecuteReaderQuery(strQuery);

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        return 1;
                    }
                }

                dbConn.Close();

                //Conecta.Cerrar();

                //cmd.Dispose();
                //dr.Dispose();

            }

            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

            return 0;
        }

        protected void BtnPnl1Seleccionar_1_Click(object sender, EventArgs e)
        {
            // Recorre cada fila del GridView
            foreach (GridViewRow row in grdSeccion_1.Rows)
            {
                // Busca los CheckBoxes dentro de la fila actual
                CheckBox chkBox1 = (CheckBox)row.FindControl("ChBoxSeccion_1_1");
                CheckBox chkBox2 = (CheckBox)row.FindControl("ChBoxSeccion_1_2");
                CheckBox chkBox3 = (CheckBox)row.FindControl("ChBoxSeccion_1_3");

                // Verifica si los CheckBoxes se encontraron y establece su propiedad Checked como true para seleccionarlos
                if (chkBox1 != null)
                {
                    chkBox1.Checked = true;
                }

                if (chkBox2 != null)
                {
                    chkBox2.Checked = true;
                }

                if (chkBox3 != null)
                {
                    chkBox3.Checked = true;
                }
            }
        }

        protected void BtnPnl1Seleccionar_2_Click(object sender, EventArgs e)
        {
            // Recorre cada fila del GridView
            foreach (GridViewRow row in grdSeccion_2.Rows)
            {
                // Busca los CheckBoxes dentro de la fila actual
                CheckBox chkBox1 = (CheckBox)row.FindControl("ChBoxSeccion_2_1");
                CheckBox chkBox2 = (CheckBox)row.FindControl("ChBoxSeccion_2_2");
                CheckBox chkBox3 = (CheckBox)row.FindControl("ChBoxSeccion_2_3");

                // Verifica si los CheckBoxes se encontraron y establece su propiedad Checked como true para seleccionarlos
                if (chkBox1 != null)
                {
                    chkBox1.Checked = true;
                }

                if (chkBox2 != null)
                {
                    chkBox2.Checked = true;
                }

                if (chkBox3 != null)
                {
                    chkBox3.Checked = true;
                }
            }
        }

        protected void BtnPnl1Seleccionar_3_Click(object sender, EventArgs e)
        {
            // Recorre cada fila del GridView
            foreach (GridViewRow row in grdSeccion_3.Rows)
            {
                // Busca los CheckBoxes dentro de la fila actual
                CheckBox chkBox1 = (CheckBox)row.FindControl("ChBoxSeccion_3_1");
                CheckBox chkBox2 = (CheckBox)row.FindControl("ChBoxSeccion_3_2");
                CheckBox chkBox3 = (CheckBox)row.FindControl("ChBoxSeccion_3_3");

                // Verifica si los CheckBoxes se encontraron y establece su propiedad Checked como true para seleccionarlos
                if (chkBox1 != null)
                {
                    chkBox1.Checked = true;
                }

                if (chkBox2 != null)
                {
                    chkBox2.Checked = true;
                }

                if (chkBox3 != null)
                {
                    chkBox3.Checked = true;
                }
            }
        }

        protected void BtnPnl1Seleccionar_4_Click(object sender, EventArgs e)
        {
            // Recorre cada fila del GridView
            foreach (GridViewRow row in grdSeccion_4.Rows)
            {
                // Busca los CheckBoxes dentro de la fila actual
                CheckBox chkBox1 = (CheckBox)row.FindControl("ChBoxSeccion_4_1");
                CheckBox chkBox2 = (CheckBox)row.FindControl("ChBoxSeccion_4_2");
                CheckBox chkBox3 = (CheckBox)row.FindControl("ChBoxSeccion_4_3");

                // Verifica si los CheckBoxes se encontraron y establece su propiedad Checked como true para seleccionarlos
                if (chkBox1 != null)
                {
                    chkBox1.Checked = true;
                }

                if (chkBox2 != null)
                {
                    chkBox2.Checked = true;
                }

                if (chkBox3 != null)
                {
                    chkBox3.Checked = true;
                }
            }
        }

        protected void BtnPnl1Seleccionar_5_Click(object sender, EventArgs e)
        {
            // Recorre cada fila del GridView
            foreach (GridViewRow row in grdSeccion_5.Rows)
            {
                // Busca los CheckBoxes dentro de la fila actual
                CheckBox chkBox1 = (CheckBox)row.FindControl("ChBoxSeccion_5_1");
                CheckBox chkBox2 = (CheckBox)row.FindControl("ChBoxSeccion_5_2");
                CheckBox chkBox3 = (CheckBox)row.FindControl("ChBoxSeccion_5_3");

                // Verifica si los CheckBoxes se encontraron y establece su propiedad Checked como true para seleccionarlos
                if (chkBox1 != null)
                {
                    chkBox1.Checked = true;
                }

                if (chkBox2 != null)
                {
                    chkBox2.Checked = true;
                }

                if (chkBox3 != null)
                {
                    chkBox3.Checked = true;
                }
            }
        }


        protected void BtnSeccion1_Click(object sender, EventArgs e)
        {
            Variables.wSeccion = 1;
            Variables.wTabla = "ITM_81";

            GetBusqProceso(Variables.wTabla);
            mpeNewProceso.Show();
        }

        protected void BtnSeccion2_Click(object sender, EventArgs e)
        {
            Variables.wSeccion = 2;
            Variables.wTabla = "ITM_82";

            GetBusqProceso(Variables.wTabla);
            mpeNewProceso.Show();
        }

        protected void BtnSeccion3_Click(object sender, EventArgs e)
        {
            Variables.wSeccion = 3;
            Variables.wTabla = "ITM_83";

            GetBusqProceso(Variables.wTabla);
            mpeNewProceso.Show();
        }

        protected void BtnSeccion4_Click(object sender, EventArgs e)
        {
            Variables.wSeccion = 4;
            Variables.wTabla = "ITM_84";

            GetBusqProceso(Variables.wTabla);
            mpeNewProceso.Show();
        }

        protected void BtnSeccion5_Click(object sender, EventArgs e)
        {
            Variables.wSeccion = 5;
            Variables.wTabla = "ITM_85";

            GetBusqProceso(Variables.wTabla);
            mpeNewProceso.Show();
        }

        protected void GetBusqProceso(string Tabla)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string aliasTabla = "a";
                string IdTpoAsunto = (string)Session["IdTpoAsunto"];

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_81, ITM_82, ITM_83, ITM_84, ITM_85
                string strQuery = $"SELECT {aliasTabla}.IdDocumento, {aliasTabla}.Descripcion, {aliasTabla}.IdTpoAsunto " +
                                  $"  FROM { Tabla } AS {aliasTabla} " +
                                  $" WHERE {aliasTabla}.IdStatus = 1 " +
                                  $"   AND {aliasTabla}.IdTpoAsunto = 0 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                dbConn.Close();

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    grdPnlBusqProceso.ShowHeaderWhenEmpty = true;
                    grdPnlBusqProceso.EmptyDataText = "No hay resultados.";
                }

                grdPnlBusqProceso.DataSource = dt;
                grdPnlBusqProceso.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                grdPnlBusqProceso.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void btnClose_Proceso_Click(object sender, EventArgs e)
        {

            // Limpia tu variable en el servidor
            Variables.wTabla = string.Empty;
        }

        protected void btnAgregar_Proceso_Click(object sender, EventArgs e)
        {

            //if (TxtDescripcion.Text == "" || TxtDescripcion.Text == null)
            //{
            //    LblMessage.Text = "Capturar Descripción del documento";
            //    mpeMensaje.Show();
            //    return;
            //}

            string sDescripcion = TxtDescripcion.Text;

            string sTabla = Variables.wTabla;

            int Envio_Ok = Add_tbDocumentos(sTabla, sDescripcion);

            if (Envio_Ok == 0)
            {
                if (Variables.wSeccion == 1)
                {
                    GetSeccion_1();
                    //GetDatosSeccion(grdSeccion_1, "ChBoxSeccion_1", Variables.wSeccion);

                }
                else if (Variables.wSeccion == 2)
                {
                    GetSeccion_2();
                    //GetDatosSeccion(grdSeccion_2, "ChBoxSeccion_2", Variables.wSeccion);
                }
                else if (Variables.wSeccion == 3)
                {
                    GetSeccion_3();
                    //GetDatosSeccion(grdSeccion_3, "ChBoxSeccion_3", Variables.wSeccion);
                }
                else if (Variables.wSeccion == 4)
                {
                    GetSeccion_4();
                    //GetDatosSeccion(grdSeccion_4, "ChBoxSeccion_4", Variables.wSeccion);
                }
                else if (Variables.wSeccion == 5)
                {
                    GetSeccion_5();
                    //GetDatosSeccion(grdSeccion_5, "ChBoxSeccion_5", Variables.wSeccion);
                }

                // GetSeccion_1();
                // GetSeccion_2();
                // GetSeccion_3();
                // GetSeccion_4();
            }

            // Inicializar Controles
            TxtDescripcion.Text = string.Empty;
        }

        public int Add_tbDocumentos_bk(string Tabla, string pDescripcion)
        {
            try
            {
                int iConsecutivo = GetIdConsecutivoMax(Tabla);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = $"INSERT INTO { Tabla } (IdDocumento, Descripcion, DescripBrev, IdStatus) " +
                                  $"VALUES (" + iConsecutivo + ", '" + pDescripcion + "', Null, 1)" + "\n \n";


                // Insert en la tabla Estado de Documento
                MySqlDataReader dr = dbConn.ExecuteReaderQuery(strQuery);
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

        public int Add_tbDocumentos(string Tabla, string pDescripcion)
        {
            try
            {

                string IdTpoAsunto = (string)Session["IdTpoAsunto"];
                string strQuery = string.Empty;

                int rowsAffected = 0;

                foreach (GridViewRow row in grdPnlBusqProceso.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        var chkbox = row.FindControl("ChBoxRow") as CheckBox;

                        if (chkbox.Checked)
                        {
                            string IdDocumento = Server.HtmlDecode(Convert.ToString(row.Cells[1].Text));
                            string IdCliente = (string)Session["IdCliente"];

                            int IdPosicion = Ultimo_IdPosicion_ITM_86();

                            if (IdPosicion > 24) { return -1; }

                            // Abrir la conexión
                            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                            dbConn.Open();

                            strQuery = "INSERT INTO ITM_86 (IdPosicion, IdProyecto, IdCliente, IdSeccion, IdTpoAsunto, IdDocumento, bSeleccion, IdStatus) " +
                                       "SELECT @IdPosicion, @IdProyecto, @IdCliente, @IdSeccion, @IdTpoAsunto, @IdDocumento, 0, 1 " +
                                    // "  FROM DUAL " +
                                       " WHERE NOT EXISTS ( " +
                                       "SELECT 1 FROM ITM_86 " +
                                       " WHERE IdProyecto = @IdProyecto AND IdCliente = @IdCliente " +
                                       "   AND IdSeccion = @IdSeccion AND IdTpoAsunto = @IdTpoAsunto " +
                                       "   AND IdDocumento = @IdDocumento);";


                            // Crear y configurar el comando SQL
                            using (MySqlConnection conn = dbConn.Connection)
                            {
                                using (MySqlCommand cmd = new MySqlCommand(strQuery, conn))
                                {
                                    // Agregar parámetros para evitar inyección SQL y mejorar la legibilidad del código
                                    cmd.Parameters.AddWithValue("@IdPosicion", IdPosicion);
                                    cmd.Parameters.AddWithValue("@IdProyecto", Variables.wIdProyecto);
                                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                                    cmd.Parameters.AddWithValue("@IdSeccion", Variables.wSeccion);
                                    cmd.Parameters.AddWithValue("@IdTpoAsunto", IdTpoAsunto);
                                    cmd.Parameters.AddWithValue("@IdDocumento", IdDocumento);

                                    // Ejecutar el comando y obtener el número de filas afectadas
                                    rowsAffected = cmd.ExecuteNonQuery();

                                }

                                // La conexión se cerrará automáticamente al salir del bloque using
                                // dbConn.Close();
                            }

                        }
                    }
                }

                if (rowsAffected > 0)
                {
                    LblMessage.Text = "Se agrego categoría, correctamente";
                    mpeMensaje.Show();
                }

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

        public int Ultimo_IdPosicion_ITM_86()
        {
            int IdPosicionMax = 0;
            string IdCliente = (string)Session["IdCliente"];
            string IdTpoAsunto = (string)Session["IdTpoAsunto"];

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX(IdPosicion), 0) + 1 IdPosicion " +
                              "  FROM ITM_86 " +
                              " WHERE IdProyecto = " + Variables.wIdProyecto + " " +
                              "   AND IdCliente = '" + IdCliente + "'" +
                              "   AND IdSeccion = " + Variables.wSeccion + " " +
                              "   AND IdTpoAsunto = " + IdTpoAsunto + " ";

            MySqlDataReader dr = dbConn.ExecuteReaderQuery(strQuery);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    IdPosicionMax = Convert.ToInt32(dr["IdPosicion"].ToString().Trim());
                }
            }

            dbConn.Close();

            return IdPosicionMax;
        }

        public int GetIdConsecutivoMax(string Tabla)
        {

            int IdConsecutivoMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = $"SELECT COALESCE(MAX(IdDocumento), 0) + 1 IdConsecutivo " +
                              $"  FROM { Tabla } ";

            MySqlDataReader dr = dbConn.ExecuteReaderQuery(strQuery);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    IdConsecutivoMax = Convert.ToInt32(dr["IdConsecutivo"].ToString().Trim());
                }
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void Eliminar_tbDocumentos(string Tabla, int iIdDocumento)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName; //LblUsuario.Text;

                // Eliminar registro tabla
                string strQuery = $"DELETE FROM  { Tabla } " +
                                  $" WHERE IdDocumento = " + iIdDocumento + "";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino documento, correctamente";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ImgDocumento_Del_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);

            int iIdDocumento = Convert.ToInt32(grdPnlBusqProceso.Rows[row.RowIndex].Cells[0].Text);

            Eliminar_tbDocumentos(Variables.wTabla, iIdDocumento);

            if (Variables.wSeccion == 1)
            {
                GetSeccion_1();
                GetDatosSeccion(grdSeccion_1, "ChBoxSeccion_1", Variables.wSeccion);

            }
            else if (Variables.wSeccion == 2)
            {
                GetSeccion_2();
                GetDatosSeccion(grdSeccion_2, "ChBoxSeccion_2", Variables.wSeccion);
            }
            else if (Variables.wSeccion == 3)
            {
                GetSeccion_3();
                GetDatosSeccion(grdSeccion_3, "ChBoxSeccion_3", Variables.wSeccion);
            }
            else if (Variables.wSeccion == 4)
            {
                GetSeccion_4();
                GetDatosSeccion(grdSeccion_4, "ChBoxSeccion_4", Variables.wSeccion);
            }
            else if (Variables.wSeccion == 5)
            {
                GetSeccion_5();
                GetDatosSeccion(grdSeccion_5, "ChBoxSeccion_5", Variables.wSeccion);
            }

            // GetSeccion_1();
            // GetSeccion_2();
            // GetSeccion_3();
            // GetSeccion_4();

        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

        }

        //protected void grdPnlBusqProceso_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    try
        //    {
        //        grdPnlBusqProceso.PageIndex = e.NewPageIndex;
        //        GetBusqProceso(Variables.wTabla);
        //    }
        //    catch (Exception ex)
        //    {
        //        LblMessage.Text = ex.Message;
        //        mpeMensaje.Show();
        //    }
        //}

        protected void btnEditarDatos_Click(object sender, EventArgs e)
        {
            // Obtener_Valores_ChBox(grdSeccion_1, "ChBoxSeccion_1", 1);
            // Obtener_Valores_ChBox(grdSeccion_2, "ChBoxSeccion_2", 2);
            // Obtener_Valores_ChBox(grdSeccion_3, "ChBoxSeccion_3", 3);
            // Obtener_Valores_ChBox(grdSeccion_4, "ChBoxSeccion_4", 4);

            // LblMessage.Text = "Se ha actualizado, el registro, correctamente";
            // this.mpeMensaje.Show();
        }

        protected void BtnAgregarDatos_Click(object sender, EventArgs e)
        {
            Add_tbCarpeta();

            // Actualizar en la tabla ITM_86 los valores de CheckBox seleccionados. 
            Obtener_Valores_ChBox(grdSeccion_1, "ChBoxSeccion_1", 1);
            Obtener_Valores_ChBox(grdSeccion_2, "ChBoxSeccion_2", 2);
            Obtener_Valores_ChBox(grdSeccion_3, "ChBoxSeccion_3", 3);
            Obtener_Valores_ChBox(grdSeccion_4, "ChBoxSeccion_4", 4);
            Obtener_Valores_ChBox(grdSeccion_5, "ChBoxSeccion_5", 5);

            // Inicializar Controles
            BtnAgregarDatos.Enabled = false;

            // Desactivar los CheckBoxes
            DesactivarCheckBoxes(grdSeccion_1, false);
            DesactivarCheckBoxes(grdSeccion_2, false);
            DesactivarCheckBoxes(grdSeccion_3, false);
            DesactivarCheckBoxes(grdSeccion_4, false);
            DesactivarCheckBoxes(grdSeccion_5, false);

            BtnSeccion1.Enabled = false;
            BtnSeccion2.Enabled = false;
            BtnSeccion3.Enabled = false;
            BtnSeccion4.Enabled = false;
            BtnSeccion5.Enabled = false;

            BtnPnl1Seleccionar_1.Enabled = false;
            BtnPnl1Seleccionar_2.Enabled = false;
            BtnPnl1Seleccionar_3.Enabled = false;
            BtnPnl1Seleccionar_4.Enabled = false;
            BtnPnl1Seleccionar_5.Enabled = false;

            //btnEliminarDatos.Enabled = true;
            //btnEditarDatos.Enabled = true;
        }

        protected void btnEliminarDatos_Click(object sender, EventArgs e)
        {
            // Del_tbCarpeta();

            LblMessage.Text = "Se ha eliminado, el registro, correctamente";
            this.mpeMensaje.Show();

            // Inicializar Controles

            // ddlCliente.ClearSelection();
            // ddlProyecto.ClearSelection();
            // ddlTpoEstatus.ClearSelection();
            // ddlCarpetas.ClearSelection();

            TxtTpoAsunto.Text = string.Empty;

            BtnAgregarDatos.Enabled = false;
            //btnEliminarDatos.Enabled = false;
            //btnEditarDatos.Enabled = false;

            InicializarCheckBoxes();

        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            // Inicializar variables de sesión
            Session["Cliente"] = string.Empty;
            Session["Proyecto"] = string.Empty;
            Session["TpoAsunto"] = string.Empty;
            Session["IdCliente"] = string.Empty;
            Session["IdProyecto"] = 0;
            Session["IdGerente"] = string.Empty;
            Session["IdTpoGestion"] = 0;
            Session["DescProyecto"] = string.Empty;
            Session["Poliza"] = string.Empty;
            Session["NomAsegurado"] = string.Empty;
            Session["TpoAsegurado"] = string.Empty;
            Session["IniVigencia"] = string.Empty;
            Session["FinVigencia"] = string.Empty;

            Variables.wIdProyecto = -1;

            Response.Redirect("fwAlta_Proyecto.aspx", true);
            return;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            btnEditar.Visible = false;
            BtnGrabar.Visible = true;

            // Desactivar los CheckBoxes
            DesactivarCheckBoxes(grdSeccion_1, true);
            DesactivarCheckBoxes(grdSeccion_2, true);
            DesactivarCheckBoxes(grdSeccion_3, true);
            DesactivarCheckBoxes(grdSeccion_4, true);
            DesactivarCheckBoxes(grdSeccion_5, true);

            BtnSeccion1.Enabled = true;
            BtnSeccion2.Enabled = true;
            BtnSeccion3.Enabled = true;
            BtnSeccion4.Enabled = true;
            BtnSeccion5.Enabled = true;

            BtnPnl1Seleccionar_1.Enabled = true;
            BtnPnl1Seleccionar_2.Enabled = true;
            BtnPnl1Seleccionar_3.Enabled = true;
            BtnPnl1Seleccionar_4.Enabled = true;
            BtnPnl1Seleccionar_5.Enabled = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            btnEditar.Visible = true;
            BtnGrabar.Visible = false;

            // Actualizar en la tabla ITM_86 los valores de CheckBox seleccionados. 
            Obtener_Valores_ChBox(grdSeccion_1, "ChBoxSeccion_1", 1);
            Obtener_Valores_ChBox(grdSeccion_2, "ChBoxSeccion_2", 2);
            Obtener_Valores_ChBox(grdSeccion_3, "ChBoxSeccion_3", 3);
            Obtener_Valores_ChBox(grdSeccion_4, "ChBoxSeccion_4", 4);
            Obtener_Valores_ChBox(grdSeccion_5, "ChBoxSeccion_5", 5);

            // Desactivar los CheckBoxes
            DesactivarCheckBoxes(grdSeccion_1, false);
            DesactivarCheckBoxes(grdSeccion_2, false);
            DesactivarCheckBoxes(grdSeccion_3, false);
            DesactivarCheckBoxes(grdSeccion_4, false);
            DesactivarCheckBoxes(grdSeccion_5, false);

            BtnSeccion1.Enabled = false;
            BtnSeccion2.Enabled = false;
            BtnSeccion3.Enabled = false;
            BtnSeccion4.Enabled = false;
            BtnSeccion5.Enabled = false;

            BtnPnl1Seleccionar_1.Enabled = false;
            BtnPnl1Seleccionar_2.Enabled = false;
            BtnPnl1Seleccionar_3.Enabled = false;
            BtnPnl1Seleccionar_4.Enabled = false;
            BtnPnl1Seleccionar_5.Enabled = false;

            LblMessage.Text = "Se han actualizado los datos, correctamente";
            this.mpeMensaje.Show();
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
                                    // Elimina la clase para habilitar visualmente los CheckBoxes
                                    checkBox.CssClass = checkBox.CssClass.Replace("disabled-checkbox", ""); 
                                }
                                else
                                {
                                    // Agrega la clase para deshabilitar visualmente los CheckBoxes
                                    checkBox.CssClass += " disabled-checkbox"; 
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void btnReadExcel_Click(object sender, EventArgs e)
        {
            // string filePath = "C:\\inetpub\\wwwroot\\WebItNow - Peacock\\itnowstorage\\Carga_Ref.xlsx";

            //if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath) && Path.GetExtension(filePath) == ".xlsx")
            //{
            //    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            //    {
            //        IWorkbook workbook = new XSSFWorkbook(stream); // Cargar el libro de trabajo desde el archivo

            //        ISheet sheet = workbook.GetSheetAt(0); // Obtener la primera hoja

            //        string connectionString = "Data Source = 82.165.211.179; Initial Catalog = Itnow_Test ; User ID=sa; Password=System1623#"; // Reemplaza con tu cadena de conexión a SQL Server

            //        using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            //        {
            //            sqlConnection.Open();

            //            // Recorrer las filas de la hoja
            //            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            //            {
            //                IRow row = sheet.GetRow(i); // Obtener la fila actual

            //                if (row != null)
            //                {
            //                    try
            //                    {
            //                        string fechaFormateada = string.Empty;

            //                        // Obtener los valores de las celdas y asignarlos a variables
            //                        string valor1 = row.GetCell(0)?.ToString();
            //                        string valor2 = row.GetCell(1)?.ToString();
            //                        string valor3 = row.GetCell(2)?.ToString();
            //                        string valor4 = row.GetCell(3)?.ToString();
            //                        string valor5 = row.GetCell(4)?.ToString();
            //                        string valor6 = row.GetCell(5)?.ToString();
            //                        string valor7 = row.GetCell(6)?.ToString();

            //                        // Manejar la fecha si no está vacía
            //                        if (!string.IsNullOrEmpty(valor3))
            //                        {
            //                            string fechaExcelString = valor3;

            //                            // Dividir la cadena de fecha en sus componentes
            //                            string[] partesFecha = fechaExcelString.Split('-');

            //                            if (partesFecha.Length == 3)
            //                            {
            //                                string dia = partesFecha[0];
            //                                string mes = partesFecha[1];
            //                                string año = partesFecha[2];

            //                                string mesNumerico;
            //                                switch (mes.ToLower())
            //                                {
            //                                    case "ene.": mesNumerico = "01"; break;
            //                                    case "feb.": mesNumerico = "02"; break;
            //                                    case "mar.": mesNumerico = "03"; break;
            //                                    case "abr.": mesNumerico = "04"; break;
            //                                    case "may.": mesNumerico = "05"; break;
            //                                    case "jun.": mesNumerico = "06"; break;
            //                                    case "jul.": mesNumerico = "07"; break;
            //                                    case "ago.": mesNumerico = "08"; break;
            //                                    case "sep.": mesNumerico = "09"; break;
            //                                    case "oct.": mesNumerico = "10"; break;
            //                                    case "nov.": mesNumerico = "11"; break;
            //                                    case "dic.": mesNumerico = "12"; break;
            //                                    default: mesNumerico = ""; break;
            //                                }

            //                                if (!string.IsNullOrEmpty(mesNumerico))
            //                                {
            //                                    fechaFormateada = $"{dia}/{mesNumerico}/{año}";
            //                                }
            //                            }
            //                        }

            //                        // Insertar los valores en la base de datos
            //                        string insertQuery = "INSERT INTO Carga_Referencia_1 (REFERENCIA, SINIESTRO, FECHA, ASEGURADORA, ADMINISTRATIVO, TECNICO, ESTATUS) " +
            //                                            " VALUES (@Valor1, @Valor2, @Valor3, @Valor4, @Valor5, @Valor6, @Valor7)";

            //                        using (SqlCommand sqlCommand = new SqlCommand(insertQuery, sqlConnection))
            //                        {
            //                            if (valor1 != null)
            //                                sqlCommand.Parameters.AddWithValue("@Valor1", valor1);
            //                            else
            //                                sqlCommand.Parameters.AddWithValue("@Valor1", DBNull.Value);

            //                            if (valor2 != null)
            //                                sqlCommand.Parameters.AddWithValue("@Valor2", valor2);
            //                            else
            //                                sqlCommand.Parameters.AddWithValue("@Valor2", DBNull.Value);

            //                            //if (!string.IsNullOrEmpty(fechaFormateada))
            //                            if (valor3 != null)
            //                                sqlCommand.Parameters.AddWithValue("@Valor3", valor3);
            //                            else
            //                                sqlCommand.Parameters.AddWithValue("@Valor3", DBNull.Value);

            //                            if (valor4 != null)
            //                                sqlCommand.Parameters.AddWithValue("@Valor4", valor4);
            //                            else
            //                                sqlCommand.Parameters.AddWithValue("@Valor4", DBNull.Value);

            //                            if (valor5 != null)
            //                                sqlCommand.Parameters.AddWithValue("@Valor5", valor5);
            //                            else
            //                                sqlCommand.Parameters.AddWithValue("@Valor5", DBNull.Value);

            //                            if (valor6 != null)
            //                                sqlCommand.Parameters.AddWithValue("@Valor6", valor6);
            //                            else
            //                                sqlCommand.Parameters.AddWithValue("@Valor6", DBNull.Value);

            //                            if (valor7 != null)
            //                                sqlCommand.Parameters.AddWithValue("@Valor7", valor7);
            //                            else
            //                                sqlCommand.Parameters.AddWithValue("@Valor7", DBNull.Value);

            //                            sqlCommand.ExecuteNonQuery();
            //                        }
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        LblMessage.Text = ex.Message;
            //                        mpeMensaje.Show();
            //                    }
            //                }
            //            }
            //        }

            //        workbook.Close();
            //    }
            //}
        }

        protected void grdPnlBusqProceso_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Quitar")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());

                // Aquí puedes acceder a la fila específica que fue seleccionada y ejecutar tu función
                GridViewRow row = grdPnlBusqProceso.Rows[rowIndex];
                int IdDocumento = int.Parse(row.Cells[1].Text);

                // Ejecutar tu función aquí
                Del_tbCarpeta(IdDocumento);

                // Renumerar indice tabla ITM_86
                // Renumerar_ITM_86();

                // Inicializar_IdDoc(IdDocumento);

                if (Variables.wSeccion == 1)
                {
                    GetSeccion_1();
                    // GetDatosSeccion(grdSeccion_1, "ChBoxSeccion_1", Variables.wSeccion);

                }
                else if (Variables.wSeccion == 2)
                {
                    GetSeccion_2();
                    // GetDatosSeccion(grdSeccion_2, "ChBoxSeccion_2", Variables.wSeccion);
                }
                else if (Variables.wSeccion == 3)
                {
                    GetSeccion_3();
                    // GetDatosSeccion(grdSeccion_3, "ChBoxSeccion_3", Variables.wSeccion);
                }
                else if (Variables.wSeccion == 4)
                {
                    GetSeccion_4();
                    // GetDatosSeccion(grdSeccion_4, "ChBoxSeccion_4", Variables.wSeccion);
                }
                else if (Variables.wSeccion == 5)
                {
                    GetSeccion_5();
                    // GetDatosSeccion(grdSeccion_5, "ChBoxSeccion_5", Variables.wSeccion);
                }
            }
        }

        protected void Inicializar_IdDoc(int IdDocumento)
        {
            try
            {
                string IdTpoAsunto = (string)Session["IdTpoAsunto"];

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Iterar sobre las columnas IdDoc_1 a IdDoc_24
                for (int i = 1; i <= 24; i++)
                {
                    // Construir el nombre de la columna dinámicamente
                    string columnName = "IdDoc_" + i;

                    // Construir la consulta dinámicamente
                    string strQuery = "UPDATE ITM_89 SET ";

                    // Actualizar la columna si su valor es igual a IdDocumento
                    strQuery += columnName + " = CASE WHEN " + columnName + " = @IdDocumento THEN 0 ELSE " + columnName + " END";

                    strQuery += " WHERE IdProyecto = @IdProyecto AND IdTpoAsunto = @IdTpoAsunto AND IdSeccion = @IdSeccion";

                    MySqlCommand cmd_1 = new MySqlCommand(strQuery, dbConn.Connection);

                    //SqlCommand cmd_1 = new SqlCommand(strQuery, Conecta.ConectarBD);

                    // Asignar valores de parámetro
                    cmd_1.Parameters.AddWithValue("@IdDocumento", IdDocumento);
                    cmd_1.Parameters.AddWithValue("@IdProyecto", Variables.wIdProyecto);
                    cmd_1.Parameters.AddWithValue("@IdTpoAsunto", IdTpoAsunto);
                    cmd_1.Parameters.AddWithValue("@IdSeccion", Variables.wSeccion);

                    cmd_1.ExecuteNonQuery(); // Ejecutar la consulta de actualización
                }

                dbConn.Close();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

        }

        protected void grdPnlBusqProceso_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //    e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.grdPnlBusqProceso, "Select$" + e.Row.RowIndex.ToString()) + ";");

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Cells[0].Width = Unit.Pixel(500);     // Proyecto
            //    e.Row.Cells[1].Width = Unit.Pixel(500);     // NomCliente
            //    e.Row.Cells[2].Width = Unit.Pixel(500);     // IdTpoAsunto
            //    e.Row.Cells[4].Width = Unit.Pixel(50);      // Eliminar
            //}

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;    // IdDocumento
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;    // IdDocumento
            }

        }

        protected void grdSeccion_1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Encuentra los CheckBoxes dentro de la fila
                CheckBox chBoxSeccion_1_1 = (CheckBox)e.Row.FindControl("ChBoxSeccion_1_1");
                CheckBox chBoxSeccion_1_2 = (CheckBox)e.Row.FindControl("ChBoxSeccion_1_2");
                CheckBox chBoxSeccion_1_3 = (CheckBox)e.Row.FindControl("ChBoxSeccion_1_3");

                // Obtén los datos de la fila actual
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                // Obtén los valores de las columnas y conviértelos a tipo entero
                string valorColumna1 = Convert.ToString(rowView["Columna1"]);
                string valorColumna2 = Convert.ToString(rowView["Columna2"]);
                string valorColumna3 = Convert.ToString(rowView["Columna3"]);

                // Obtén el valor de la columna Columna1
                int valorChBoxSeccion_1_1 = Convert.ToInt32(rowView["ChBoxSeccion_1_1"]);
                int valorChBoxSeccion_1_2 = Convert.ToInt32(rowView["ChBoxSeccion_1_2"]);
                int valorChBoxSeccion_1_3 = Convert.ToInt32(rowView["ChBoxSeccion_1_3"]);

                // Verifica si la columna Columna1 tiene datos y muestra u oculta el CheckBox
                if (!string.IsNullOrEmpty(valorColumna1))
                {
                    chBoxSeccion_1_1.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_1_1.Checked = valorChBoxSeccion_1_1 == 1;
                }
                else
                {
                    chBoxSeccion_1_1.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna2))
                {
                    chBoxSeccion_1_2.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_1_2.Checked = valorChBoxSeccion_1_2 == 1;
                }
                else
                {
                    chBoxSeccion_1_2.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna3))
                {
                    chBoxSeccion_1_3.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_1_3.Checked = valorChBoxSeccion_1_3 == 1;
                }
                else
                {
                    chBoxSeccion_1_3.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
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

        protected void grdSeccion_3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Encuentra los CheckBoxes dentro de la fila
                CheckBox chBoxSeccion_3_1 = (CheckBox)e.Row.FindControl("ChBoxSeccion_3_1");
                CheckBox chBoxSeccion_3_2 = (CheckBox)e.Row.FindControl("ChBoxSeccion_3_2");
                CheckBox chBoxSeccion_3_3 = (CheckBox)e.Row.FindControl("ChBoxSeccion_3_3");

                // Obtén los datos de la fila actual
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                // Obtén los valores de las columnas y conviértelos a tipo entero
                string valorColumna1 = Convert.ToString(rowView["Columna1"]);
                string valorColumna2 = Convert.ToString(rowView["Columna2"]);
                string valorColumna3 = Convert.ToString(rowView["Columna3"]);

                // Obtén el valor de la columna Columna1
                int valorChBoxSeccion_3_1 = Convert.ToInt32(rowView["ChBoxSeccion_3_1"]);
                int valorChBoxSeccion_3_2 = Convert.ToInt32(rowView["ChBoxSeccion_3_2"]);
                int valorChBoxSeccion_3_3 = Convert.ToInt32(rowView["ChBoxSeccion_3_3"]);

                // Verifica si la columna Columna1 tiene datos y muestra u oculta el CheckBox
                if (!string.IsNullOrEmpty(valorColumna1))
                {
                    chBoxSeccion_3_1.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_3_1.Checked = valorChBoxSeccion_3_1 == 1;
                }
                else
                {
                    chBoxSeccion_3_1.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna2))
                {
                    chBoxSeccion_3_2.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_3_2.Checked = valorChBoxSeccion_3_2 == 1;
                }
                else
                {
                    chBoxSeccion_3_2.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna3))
                {
                    chBoxSeccion_3_3.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_3_3.Checked = valorChBoxSeccion_3_3 == 1;
                }
                else
                {
                    chBoxSeccion_3_3.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
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

    }
}