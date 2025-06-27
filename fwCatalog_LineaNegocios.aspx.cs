using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwCatalog_LineaNegocios : System.Web.UI.Page
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
                    // GetContactos();

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

        public void GetLineaNegocios()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Linea de Negocios = ITM_58
                string strQuery = "SELECT IdLinea, IdSeguros, NomLineaNegocio " +
                                  "  FROM ITM_58 " +
                                  " WHERE IdSeguros = '" + ddlCliente.SelectedValue + "' " +
                                  "   AND IdStatus = 1 ORDER BY IdLinea";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdLineaNegocios.ShowHeaderWhenEmpty = true;
                    GrdLineaNegocios.EmptyDataText = "No hay resultados.";
                }

                GrdLineaNegocios.DataSource = dt;
                GrdLineaNegocios.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdLineaNegocios.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdLineaNegocios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdLineaNegocios.PageIndex = e.NewPageIndex;
                GetLineaNegocios();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdLineaNegocios_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdLineaNegocios_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdLineaNegocios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdLineaNegocios, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Width = Unit.Pixel(100);     // IdSeguros
                e.Row.Cells[2].Width = Unit.Pixel(1100);    // Nombre Linea de Negocio
                e.Row.Cells[4].Width = Unit.Pixel(50);      // Eliminar
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;    // IdLinea
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;    // IdLinea
            }
        }

        protected void GrdLineaNegocios_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            Eliminar_ITM_58();

            // ddlCliente.SelectedIndex = 0;
            TxtLineaNegocio.Text = string.Empty;

        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }


        protected void BtnAnular_Click(object sender, EventArgs e)
        {

            // inicializar controles.
            // ddlCliente.SelectedIndex = 0;
            ddlCliente.Enabled = true;

            TxtLineaNegocio.Text = string.Empty;
            TxtLineaNegocio.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            ddlCliente.Enabled = false;
            TxtLineaNegocio.ReadOnly = false;

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
            if (TxtLineaNegocio.Text == "" || TxtLineaNegocio.Text == null)
            {
                LblMessage.Text = "Capturar Nombre de Linea Negocios";
                mpeMensaje.Show();
                return;
            }

            Actualizar_ITM_58();

            // inicializar controles.
            ddlCliente.Enabled = true;
            // ddlCliente.SelectedIndex = 0;

            TxtLineaNegocio.Text = string.Empty;
            TxtLineaNegocio.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;

        }

        protected void Actualizar_ITM_58()
        {
            try
            {
                int index = Variables.wRenglon;

                int IdLinea = Convert.ToInt32(GrdLineaNegocios.Rows[index].Cells[0].Text); ;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // IdContacto, IdSeguros, NomContacto, EmailContacto
                // Actualizar registro(s) tablas (ITM_58)
                string strQuery = "UPDATE ITM_58 " +
                                  "   SET NomLineaNegocio = '" + TxtLineaNegocio.Text.Trim() + "' " +
                                  " WHERE IdLinea = " + IdLinea + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizo linea de negocios, correctamente";
                mpeMensaje.Show();

                GetLineaNegocios();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Eliminar_ITM_58()
        {
            try
            {
                int index = Variables.wRenglon;

                int IdLinea = Convert.ToInt32(GrdLineaNegocios.Rows[index].Cells[0].Text);
                string IdSeguros = Convert.ToString(GrdLineaNegocios.Rows[index].Cells[1].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_58)
                string strQuery = "DELETE FROM ITM_58 " +
                                  " WHERE IdLinea = " + IdLinea + " " +
                                  "   AND IdSeguros = '" + IdSeguros + "' ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino linea de negocios, correctamente";
                mpeMensaje.Show();

                GetLineaNegocios();
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
                if (TxtLineaNegocio.Text == "" || TxtLineaNegocio.Text == null)
                {
                    LblMessage.Text = "Capturar Nombre de Linea Negocios";
                    mpeMensaje.Show();
                    return;
                }

                int iIdLinea = GetIdConsecutivoMax();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_58)
                string strQuery = "INSERT INTO ITM_58 (IdLinea, IdSeguros, NomLineaNegocio, IdStatus) " +
                                  "VALUES (" + iIdLinea + ", '" + ddlCliente.SelectedValue + "', '" + TxtLineaNegocio.Text.Trim() + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se agrego linea de negocio, correctamente";
                mpeMensaje.Show();

                // Inicializar Controles
                // ddlCliente.SelectedIndex = 0;
                TxtLineaNegocio.Text = string.Empty;

                GetLineaNegocios();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public int GetIdConsecutivoMax()
        {

            int IdConsecutivoMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX(IdLinea), 0) + 1 IdLinea " +
                                " FROM ITM_58 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);


            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdLinea"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            ddlCliente.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdLineaNegocios.Rows[index].Cells[1].Text));
            TxtLineaNegocio.Text = Server.HtmlDecode(Convert.ToString(GrdLineaNegocios.Rows[index].Cells[2].Text));

            ddlCliente.Enabled = false;
            TxtLineaNegocio.ReadOnly = true;

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

            LblMessage_1.Text = "¿Desea eliminar linea de negocios?";
            mpeMensaje_1.Show();
        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetLineaNegocios();
        }

    }

}