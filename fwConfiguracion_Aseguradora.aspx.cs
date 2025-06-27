using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwConfiguracion_Aseguradora : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
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

                    if (Variables.wUserName == "" || Variables.wPassword == "")
                    {
                        Response.Redirect("Login.aspx", true);
                        return;
                    }

                    string CveCliente = Request.QueryString["Cve"];
                    Variables.wPrefijo_Aseguradora = CveCliente;

                    GetClientes();
                    GetContactos();

                    // GetProyecto();
                    // GetLineasNegocio();
                    // GetProtocolo();

                    BtnAgregarDatos.Visible = false;
                    BtnEditar.Visible = true;

                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

            }
        }

        public void GetClientes()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_67
                string strQuery = "SELECT IdSeguros, IdOrden, Descripcion " +
                                  "  FROM ITM_67 " +
                                  " WHERE IdSeguros = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND IdStatus = 1 ORDER BY IdOrden";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count != 0)
                {
                    TxtNomCliente.Text = dt.Rows[0].ItemArray[2].ToString();
                }

                dbConn.Close();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetProyecto()
        {
            try
            {
                string sCliente = Variables.wPrefijo_Aseguradora;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdProyecto, Descripcion " +
                                  "  FROM ITM_78 " +
                                  " WHERE IdCliente = '" + sCliente + "'" +
                                  "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                //ddlProyecto.DataSource = dt;

                //ddlProyecto.DataValueField = "IdProyecto";
                //ddlProyecto.DataTextField = "Descripcion";

                //ddlProyecto.DataBind();
                //ddlProyecto.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetProtocolo()
        {
            try
            {
                string sCliente = Variables.wPrefijo_Aseguradora;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdSLA, NomProtocolo " +
                                  "  FROM ITM_55 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                //ddlProtocolos.DataSource = dt;

                //ddlProtocolos.DataValueField = "IdSLA";
                //ddlProtocolos.DataTextField = "NomProtocolo";

                //ddlProtocolos.DataBind();
                //ddlProtocolos.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetContactos()
        {
            try
            {
                string sCliente = Variables.wPrefijo_Aseguradora;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdContacto, NomContacto " +
                                  "  FROM ITM_59 " +
                                  " WHERE IdSeguros = '" + sCliente + "'" +
                                  "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlContactos.DataSource = dt;

                ddlContactos.DataValueField = "IdContacto";
                ddlContactos.DataTextField = "NomContacto";

                ddlContactos.DataBind();
                ddlContactos.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetLineasNegocio()
        {
            try
            {
                string sCliente = Variables.wPrefijo_Aseguradora;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdLinea, NomLineaNegocio " +
                                  "  FROM ITM_58 " +
                                  " WHERE IdSeguros = '" + sCliente + "'" +
                                  "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                //ddlLineasNegocio.DataSource = dt;

                //ddlLineasNegocio.DataValueField = "IdLinea";
                //ddlLineasNegocio.DataTextField = "NomLineaNegocio";

                //ddlLineasNegocio.DataBind();
                //ddlLineasNegocio.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAnular_Click(object sender, EventArgs e)
        {

        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwCatalog_Client.aspx", true);
        }

        protected void ddlLineasNegocio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlSLA_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        protected void GetSeccion_1()
        {
            try
            {
                int IdContacto = Convert.ToInt32(ddlContactos.SelectedValue);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "  WITH CTE AS ( SELECT ITM_58.NomLineaNegocio AS NomLineaNegocio_58, ITM_58.IdLinea, " +
                                  "                ROW_NUMBER() OVER (ORDER BY ITM_58.IdLinea) AS RowNumber " +
                                  "  FROM ITM_58 " +
                                  " WHERE ITM_58.IdStatus = 1 AND ITM_58.IdSeguros = '" + Variables.wPrefijo_Aseguradora + "' ), " +
                                  "Seleccion_56 AS ( SELECT IdLinea, bSeleccion FROM ITM_56 " +
                                  "                   WHERE IdSeguros = '" + Variables.wPrefijo_Aseguradora + "' AND IdContacto = " + IdContacto + " ) " +
                                  "SELECT COALESCE(CTE1.NomLineaNegocio_58, '') AS Columna1, " +
                                  "       COALESCE(S56_1.bSeleccion, 0) AS ChBoxSeccion_1_1, " +
                                  "       COALESCE(CTE2.NomLineaNegocio_58, '') AS Columna2, " +
                                  "       COALESCE(S56_2.bSeleccion, 0) AS ChBoxSeccion_1_2, " +
                                  "       COALESCE(CTE3.NomLineaNegocio_58, '') AS Columna3, " +
                                  "       COALESCE(S56_3.bSeleccion, 0) AS ChBoxSeccion_1_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN Seleccion_56 S56_1 ON CTE1.IdLinea = S56_1.IdLinea " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN Seleccion_56 S56_2 ON CTE2.IdLinea = S56_2.IdLinea " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                  "  LEFT JOIN Seleccion_56 S56_3 ON CTE3.IdLinea = S56_3.IdLinea " +
                                  " ORDER BY CTE1.RowNumber ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    grdSeccion_1.ShowHeaderWhenEmpty = true;
                    grdSeccion_1.EmptyDataText = "No hay resultados.";
                }

                grdSeccion_1.DataSource = dt;
                grdSeccion_1.DataBind();

                dbConn.Close();
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

                string strQuery = "  WITH CTE AS ( SELECT ITM_55.NomProtocolo AS NomProtocolo_55, ITM_55.IdSLA, " +
                                  "                ROW_NUMBER() OVER (ORDER BY ITM_55.IdSLA) AS RowNumber " +
                                  "  FROM ITM_55 " +
                                  " WHERE ITM_55.IdStatus = 1 " +
                                  " ) " +
                                  "SELECT COALESCE(CTE1.NomProtocolo_55, '') AS Columna1, " +
                                  "           0 AS ChBoxSeccion_2_1, " +
                                  "       COALESCE(CTE2.NomProtocolo_55, '') AS Columna2, " +
                                  "           0 AS ChBoxSeccion_2_2, " +
                                  "       COALESCE(CTE3.NomProtocolo_55, '') AS Columna3, " +
                                  "           0 AS ChBoxSeccion_2_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                  " ORDER BY CTE1.RowNumber ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                //if (dt.Rows.Count == 0)
                //{
                //    grdSeccion_2.ShowHeaderWhenEmpty = true;
                //    grdSeccion_2.EmptyDataText = "No hay resultados.";
                //}

                //grdSeccion_2.DataSource = dt;
                //grdSeccion_2.DataBind();

                dbConn.Close();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnAgregarDatos_Click(object sender, EventArgs e)
        {

        }

        protected void ddlContactos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            {
                GetSeccion_1();
                // GetSeccion_2();

                // Desactivar los CheckBoxes
                DesactivarCheckBoxes(grdSeccion_1, false);
                // DesactivarCheckBoxes(grdSeccion_2, true);

            } catch (Exception ex)
            {

                LblMessage.Text = ex.Message;
                mpeMensaje.Show();

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

    }
}