using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwCatalog_Contacts : System.Web.UI.Page
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

                    GetCiaSeguros();
                    GetSeccion_1();

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
                                  " WHERE IdSeguros <> 'OTR'" +
                                  "   AND IdStatus = 1 " +
                                  " ORDER BY IdOrden";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlCliente.DataSource = dt;

                ddlCliente.DataValueField = "IdSeguros";
                ddlCliente.DataTextField = "Descripcion";

                ddlCliente.DataBind();
                ddlCliente.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetContactos()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_59
                string strQuery = "SELECT IdContacto, IdSeguros, NomContacto, EmailContacto, Telefono, TelExtencion, TelCelular " +
                                  "  FROM ITM_59 " +
                                  " WHERE IdSeguros = '" + ddlCliente.SelectedValue + "' " +
                                  "   AND IdStatus = 1 ORDER BY IdContacto";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdContactos.ShowHeaderWhenEmpty = true;
                    GrdContactos.EmptyDataText = "No hay resultados.";
                }

                GrdContactos.DataSource = dt;
                GrdContactos.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdContactos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            Eliminar_ITM_59();

            // ddlCliente.SelectedIndex = 0;
            ddlCliente.Enabled = true;

            TxtNomContacto.Text = string.Empty;
            TxtCorreoContacto.Text = string.Empty;
            TxtTelefono.Text = string.Empty;
            TxtExtencionTel.Text = string.Empty;
            TxtTelMovil.Text = string.Empty;
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void GrdContactos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdContactos.PageIndex = e.NewPageIndex;
                GetContactos();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdContactos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdContactos_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdContactos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdContactos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdContactos, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Width = Unit.Pixel(50);      // IdSeguros
                e.Row.Cells[2].Width = Unit.Pixel(600);     // Nombre de Contacto
                e.Row.Cells[3].Width = Unit.Pixel(600);     // Correo de Contacto
                e.Row.Cells[4].Width = Unit.Pixel(250);     // Telefono
                e.Row.Cells[5].Width = Unit.Pixel(250);     // Extencion Telefono
                e.Row.Cells[6].Width = Unit.Pixel(250);     // Telefono Movil
                e.Row.Cells[7].Width = Unit.Pixel(600);     // Linea de Negocios
                e.Row.Cells[9].Width = Unit.Pixel(50);      // Eliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;    // IdContacto
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;    // IdContacto
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtener el IdContacto de la fila
                int idContacto = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "IdContacto"));

                // Consulta SQL para obtener las líneas de negocio asociadas con el IdContacto
                string query = @" SELECT cl.NomLineaNegocio FROM itm_58 cl
                                   INNER JOIN itm_56 l ON cl.IdLinea = l.IdLinea
                                   WHERE l.IdContacto = @IdContacto";

                // Llamada para obtener los datos de la BD
                DataTable dt = Obtener_LineaNegocios(query, idContacto); // Método para obtener las líneas de negocio

                // Obtener el control Repeater dentro de la fila
                Repeater rptLineasNegocio = (Repeater)e.Row.FindControl("rptLineasNegocio");

                // Asignar los datos al Repeater y enlazar
                if (rptLineasNegocio != null)
                {
                    rptLineasNegocio.DataSource = dt;
                    rptLineasNegocio.DataBind();
                }
            }

        }

        private DataTable Obtener_LineaNegocios(string query, int idContacto)
        {
            DataTable dt = new DataTable();

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            // Acceder a la conexión directamente desde la instancia dbConn
            MySqlConnection conn = dbConn.Connection;

            try
            {
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    // Agregar parámetro a la consulta
                    command.Parameters.AddWithValue("@IdContacto", idContacto);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                // Puedes registrar el error o lanzar una excepción personalizada
                Console.WriteLine("Error al obtener las líneas de negocio: " + ex.Message);
            }

            return dt;
        }


        protected void BtnAnular_Click(object sender, EventArgs e)
        {

            // inicializar controles.
            // ddlCliente.SelectedIndex = 0;
            ddlCliente.Enabled = true;

            TxtNomContacto.Text = string.Empty;
            TxtNomContacto.ReadOnly = false;

            TxtCorreoContacto.Text = string.Empty;
            TxtCorreoContacto.ReadOnly = false;

            TxtTelefono.Text = string.Empty;
            TxtTelefono.ReadOnly = false;

            TxtExtencionTel.Text = string.Empty;
            TxtExtencionTel.ReadOnly = false;

            TxtTelMovil.Text = string.Empty;
            TxtTelMovil.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;

            DesactivarCheckBoxes(grdSeccion_1, true);

            GetSeccion_1();
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            ddlCliente.Enabled = false;
            TxtNomContacto.ReadOnly = false;
            TxtCorreoContacto.ReadOnly = false;
            TxtTelefono.ReadOnly = false;
            TxtExtencionTel.ReadOnly = false;
            TxtTelMovil.ReadOnly = false;

            DesactivarCheckBoxes(grdSeccion_1, true);

            BtnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (ddlCliente.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Compañia de Seguros";
                mpeMensaje.Show();
                return;
            }
            if (TxtNomContacto.Text == "" || TxtNomContacto.Text == null)
            {
                LblMessage.Text = "Capturar Nombre del Contacto";
                mpeMensaje.Show();
                return;
            }
            else if (TxtCorreoContacto.Text == "" || TxtCorreoContacto.Text == null)
            {
                LblMessage.Text = "Capturar Correo del Contacto";
                mpeMensaje.Show();
                return;
            }


            Actualizar_ITM_59();

            Delete_ITM_56();

            int IdContacto = Convert.ToInt32(GrdContactos.Rows[Variables.wRenglon].Cells[0].Text);
            string IdSeguros = Server.HtmlDecode(Convert.ToString(GrdContactos.Rows[Variables.wRenglon].Cells[1].Text));

            Obtener_Valores_ChBox(grdSeccion_1, "ChBoxSeccion_1", IdContacto, IdSeguros);

            // inicializar controles.
            // ddlCliente.SelectedIndex = 0;
            ddlCliente.Enabled = true;

            TxtNomContacto.Text = string.Empty;
            TxtNomContacto.ReadOnly = false;

            TxtCorreoContacto.Text = string.Empty;
            TxtCorreoContacto.ReadOnly = false;

            TxtTelefono.Text = string.Empty;
            TxtTelefono.ReadOnly = false;

            TxtExtencionTel.Text = string.Empty;
            TxtExtencionTel.ReadOnly = false;

            TxtTelMovil.Text = string.Empty;
            TxtTelMovil.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;

            DesactivarCheckBoxes(grdSeccion_1, true);

            GetContactos();

            GetSeccion_1();
        }

        protected void Actualizar_ITM_59()
        {
            try
            {
                int index = Variables.wRenglon;

                int IdContacto = Convert.ToInt32(GrdContactos.Rows[index].Cells[0].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // IdContacto, IdSeguros, NomContacto, EmailContacto
                // Actualizar registro(s) tablas (ITM_59)
                string strQuery = "UPDATE ITM_59 " +
                                  "   SET NomContacto = '" + TxtNomContacto.Text.Trim() + "', " +
                                  "       EmailContacto = '" + TxtCorreoContacto.Text.Trim() + "', " +
                                  "       Telefono = '" + TxtTelefono.Text.Trim() + "', " +
                                  "       TelExtencion = '" + TxtExtencionTel.Text.Trim() + "', " +
                                  "       TelCelular = '" + TxtTelMovil.Text.Trim() + "' " +
                                  " WHERE IdContacto = " + IdContacto + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizo contacto, correctamente";
                mpeMensaje.Show();

                GetContactos();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Eliminar_ITM_59()
        {
            try
            {
                int index = Variables.wRenglon;

                int IdContacto = Convert.ToInt32(GrdContactos.Rows[index].Cells[0].Text);
                string IdSeguros = Convert.ToString(GrdContactos.Rows[index].Cells[1].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_59)
                string strQuery = "DELETE FROM ITM_59 " +
                                  " WHERE IdContacto = " + IdContacto + " " +
                                  "   AND IdSeguros = '" + IdSeguros + "' ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino cliente, correctamente";
                mpeMensaje.Show();

                GetContactos();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlCliente.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar Compañia de Seguros";
                    mpeMensaje.Show();
                    return;
                }
                if (TxtNomContacto.Text == "" || TxtNomContacto.Text == null)
                {
                    LblMessage.Text = "Capturar Nombre del Contacto";
                    mpeMensaje.Show();
                    return;
                }
                else if (TxtCorreoContacto.Text == "" || TxtCorreoContacto.Text == null)
                {
                    LblMessage.Text = "Capturar Correo del Contacto";
                    mpeMensaje.Show();
                    return;
                }

                int iIdContacto = GetIdConsecutivoMax();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_59)
                string strQuery = "INSERT INTO ITM_59 (IdContacto, IdSeguros, NomContacto, EmailContacto, Telefono, TelExtencion, TelCelular, IdStatus) " +
                                  "VALUES (" + iIdContacto + ", '" + ddlCliente.SelectedValue + "', '" + TxtNomContacto.Text.Trim() + "', '" + TxtCorreoContacto.Text.Trim() + "', " +
                                  " '" + TxtTelefono.Text.Trim() + "', '" + TxtExtencionTel.Text.Trim() + "', '" + TxtTelMovil.Text.Trim() + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                Obtener_Valores_ChBox(grdSeccion_1, "ChBoxSeccion_1", iIdContacto, ddlCliente.SelectedValue);

                GetSeccion_1();

                LblMessage.Text = "Se agrego contacto, correctamente";
                mpeMensaje.Show();

                // Inicializar Controles
                // ddlCliente.SelectedIndex = 0;
                ddlCliente.Enabled = true;

                TxtNomContacto.Text = string.Empty;
                TxtCorreoContacto.Text = string.Empty;
                TxtTelefono.Text = string.Empty;
                TxtExtencionTel.Text = string.Empty;
                TxtTelMovil.Text = string.Empty;

                GetContactos();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public int GetIdCntLineaNegMax()
        {
            int IdConsecutivoMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX(IdCntLineaNeg), 0) + 1 IdCntLineaNeg " +
                                " FROM ITM_56 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);


            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdCntLineaNeg"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;
        }

        public int GetIdConsecutivoMax()
        {

            int IdConsecutivoMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX(IdContacto), 0) + 1 IdContacto " +
                                " FROM ITM_59 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);


            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdContacto"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void ImgCheckList_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            ddlCliente.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdContactos.Rows[index].Cells[1].Text));
            TxtNomContacto.Text = Server.HtmlDecode(Convert.ToString(GrdContactos.Rows[index].Cells[2].Text));
            TxtCorreoContacto.Text = Server.HtmlDecode(Convert.ToString(GrdContactos.Rows[index].Cells[3].Text));
            TxtTelefono.Text = Server.HtmlDecode(Convert.ToString(GrdContactos.Rows[index].Cells[4].Text));
            TxtExtencionTel.Text = Server.HtmlDecode(Convert.ToString(GrdContactos.Rows[index].Cells[5].Text));
            TxtTelMovil.Text = Server.HtmlDecode(Convert.ToString(GrdContactos.Rows[index].Cells[6].Text));

            GetSeccion_1_1();

            ddlCliente.Enabled = false;
            TxtNomContacto.ReadOnly = true;
            TxtCorreoContacto.ReadOnly = true;
            TxtTelefono.ReadOnly = true;
            TxtExtencionTel.ReadOnly = true;
            TxtTelMovil.ReadOnly = true;

            DesactivarCheckBoxes(grdSeccion_1, false);

            BtnAnular.Visible = true;
            BtnEditar.Enabled = true;
            BtnAgregar.Enabled = false;
        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            LblMessage_1.Text = "¿Desea eliminar el contacto?";
            mpeMensaje_1.Show();
        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetSeccion_1();
            GetContactos();
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

        protected void GetSeccion_1()
        {
            try
            {
                Variables.wPrefijo_Aseguradora = ddlCliente.SelectedValue;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "  WITH CTE AS ( SELECT ITM_58.NomLineaNegocio AS NomLineaNegocio_58, ITM_58.IdLinea, " +
                                  "                ROW_NUMBER() OVER (ORDER BY ITM_58.IdLinea) AS RowNumber " +
                                  "  FROM ITM_58 " +
                                  " WHERE ITM_58.IdStatus = 1 AND ITM_58.IdSeguros = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  " ) " +
                                  "SELECT COALESCE(CTE1.NomLineaNegocio_58, '') AS Columna1, " +
                                  "           0 AS ChBoxSeccion_1_1, " +
                                  "       COALESCE(CTE2.NomLineaNegocio_58, '') AS Columna2, " +
                                  "           0 AS ChBoxSeccion_1_2, " +
                                  "       COALESCE(CTE3.NomLineaNegocio_58, '') AS Columna3, " +
                                  "           0 AS ChBoxSeccion_1_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
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

        protected void GetSeccion_1_1()
        {
            try
            {
                Variables.wPrefijo_Aseguradora = ddlCliente.SelectedValue;
                int IdContacto = Convert.ToInt32(GrdContactos.Rows[Variables.wRenglon].Cells[0].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "  WITH CTE AS ( SELECT ITM_58.NomLineaNegocio AS NomLineaNegocio_58, ITM_58.IdLinea, " +
                                  "                ROW_NUMBER() OVER (ORDER BY ITM_58.IdLinea) AS RowNumber " +
                                  "  FROM ITM_58 " +
                                  " WHERE ITM_58.IdStatus = 1 AND ITM_58.IdSeguros = '" + Variables.wPrefijo_Aseguradora + "' ), " +
                                  "Seleccion_56 AS ( SELECT IdLinea, bSeleccion FROM ITM_56 WHERE IdSeguros = '" + Variables.wPrefijo_Aseguradora + "' AND IdContacto = " + IdContacto + " ) " +
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

        protected void Obtener_Valores_ChBox(GridView gridView, string checkBoxID, int IdContacto, string IdSeguros)
        {

            int IdCntLineaNeg = GetIdCntLineaNegMax();

            // int IdContacto = Convert.ToInt32(GrdContactos.Rows[Variables.wRenglon].Cells[0].Text);
            // string IdSeguros = Server.HtmlDecode(Convert.ToString(GrdContactos.Rows[Variables.wRenglon].Cells[1].Text));

            string IdUsuario = Variables.wUserLogon;

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

            }

            ActualizarDatosEnTabla(IdCntLineaNeg, IdSeguros, IdContacto, IdUsuario, selectedLabels);
        }

        // Método para actualizar los datos en la tabla ITM_86
        private void ActualizarDatosEnTabla(int IdCntLineaNeg, string IdSeguros, int IdContacto, string IdUsuario, List<string> selectedLabels)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Obtener el IdDocumento de ITM_82, ITM_84, ITM_85  para cada label
                Dictionary<string, int> idDocumentos = new Dictionary<string, int>();

                foreach (var label in selectedLabels)
                {
                    int idDocumento = GetIdDocumento(label, IdSeguros);
                    idDocumentos[label] = idDocumento;
                }

                // Implementa la lógica de la función aquí
                foreach (var kvp in idDocumentos)
                {
                    string label = kvp.Key;
                    int IdLinea = kvp.Value;

                    // Crear el comando SQL para la inserción
                    string strQuery = "INSERT INTO ITM_56 (IdCntLineaNeg, IdSeguros, IdContacto, IdLinea, bSeleccion, IdUsuario )" +
                                      " VALUES (@IdCntLineaNeg, @IdSeguros, @IdContacto, @IdLinea, 1, @IdUsuario) ";

                    //using (SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD))
                    int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                    {
                        // Agregar los parámetros y sus valores
                        cmd.Parameters.AddWithValue("@IdCntLineaNeg", IdCntLineaNeg);
                        cmd.Parameters.AddWithValue("@IdSeguros", IdSeguros);
                        cmd.Parameters.AddWithValue("@IdContacto", IdContacto);
                        cmd.Parameters.AddWithValue("@IdLinea", IdLinea);
                        cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
                    });

                    IdCntLineaNeg += 1;
                }

                dbConn.Close();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public int GetIdDocumento(string label, string IdSeguros)
        {
            int idDocumento = 0;

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Crear la consulta SQL para recuperar IdDocumento de ITM_82
                string query = $"SELECT IdLinea FROM ITM_58 WHERE NomLineaNegocio = @Label AND IdSeguros = @IdSeguros";

                int rowsAffected = dbConn.ExecuteNonQueryWithParameters(query, cmd =>
                {
                    cmd.Parameters.AddWithValue("@Label", label);
                    cmd.Parameters.AddWithValue("@IdSeguros", IdSeguros);

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

        protected void Delete_ITM_56()
        {

            int IdContacto = Convert.ToInt32(GrdContactos.Rows[Variables.wRenglon].Cells[0].Text);
            string IdSeguros = Server.HtmlDecode(Convert.ToString(GrdContactos.Rows[Variables.wRenglon].Cells[1].Text));

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_56)
                string strQuery = "DELETE FROM ITM_56 " +
                                  " WHERE IdSeguros = '" + IdSeguros + "' " +
                                  "   AND IdContacto = " + IdContacto + " ";

                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
            }
            catch (System.Exception ex)
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