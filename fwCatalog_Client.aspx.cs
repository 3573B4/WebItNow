using System;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwCatalog_Client : System.Web.UI.Page
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

                    GetClientes();

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
                                        " FROM ITM_67 " +
                                        " WHERE IdStatus = 1 ORDER BY IdOrden";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdCliente.ShowHeaderWhenEmpty = true;
                    GrdCliente.EmptyDataText = "No hay resultados.";
                }

                GrdCliente.DataSource = dt;
                GrdCliente.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdCliente.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdCliente_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdCliente_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdCliente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdCliente.PageIndex = e.NewPageIndex;
                GetClientes();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdCliente_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdCliente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdCliente, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Width = Unit.Pixel(100);     // IdSeguros
                e.Row.Cells[3].Width = Unit.Pixel(1100);    // Descripcion
                e.Row.Cells[4].Width = Unit.Pixel(50);      // Eliminar
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[2].Visible = false;    // IdOrden
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Visible = false;    // IdOrden
            }
        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            LblMessage_1.Text = "¿Desea eliminar el cliente?";
            mpeMensaje_1.Show();
        }

        protected void Eliminar_ITM_67()
        {
            try
            {
                int index = Variables.wRenglon;

                int iIdOrden = Convert.ToInt32(GrdCliente.Rows[index].Cells[2].Text); ;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_67)
                string strQuery = "DELETE FROM ITM_67 " +
                                  " WHERE IdOrden = " + iIdOrden + "";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino cliente, correctamente";
                mpeMensaje.Show();

                GetClientes();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Actualizar_ITM_67()
        {
            try
            {
                int index = Variables.wRenglon;

                int iIdOrden = Convert.ToInt32(GrdCliente.Rows[index].Cells[2].Text); ;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Actualizar registro(s) tablas (ITM_67)
                string strQuery = "UPDATE ITM_67 " +
                                  "   SET IdSeguros = '" + TxtCveCliente.Text.Trim() + "', " +
                                  "       Descripcion = '" + TxtNomCliente.Text.Trim() + "' " +
                                  " WHERE IdOrden = " + iIdOrden + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizo cliente, correctamente";
                mpeMensaje.Show();

                GetClientes();
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
                if (TxtCveCliente.Text == "" || TxtCveCliente.Text == null)
                {
                    LblMessage.Text = "Capturar Clave del Cliente";
                    mpeMensaje.Show();
                    return;
                }
                else if (TxtNomCliente.Text == "" || TxtNomCliente.Text == null)
                {
                    LblMessage.Text = "Capturar Nombre del Cliente";
                    mpeMensaje.Show();
                    return;
                }

                int iIdOrden = GetIdConsecutivoMax();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_67)
                string strQuery = "INSERT INTO ITM_67 (IdSeguros, IdOrden, Descripcion, IdStatus) " +
                                  "VALUES ('" + TxtCveCliente.Text.Trim() + "', " + iIdOrden + ", '" + TxtNomCliente.Text.Trim() + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se agrego cliente, correctamente";
                mpeMensaje.Show();

                // Inicializar Controles
                TxtCveCliente.Text = string.Empty;
                TxtNomCliente.Text = string.Empty;

                GetClientes();

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

            string strQuery = "SELECT COALESCE(MAX(IdOrden), 0) + 1 IdOrden " +
                                " FROM ITM_67 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);


            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdOrden"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            Eliminar_ITM_67();

            TxtCveCliente.Text = string.Empty;
            TxtNomCliente.Text = string.Empty;
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
            TxtCveCliente.Text = string.Empty;
            TxtCveCliente.ReadOnly = false;

            TxtNomCliente.Text = string.Empty;
            TxtNomCliente.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            TxtCveCliente.ReadOnly = false;
            TxtNomCliente.ReadOnly = false;

            BtnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (TxtCveCliente.Text == "" || TxtCveCliente.Text == null)
            {
                LblMessage.Text = "Capturar Clave del Cliente";
                mpeMensaje.Show();
                return;
            }
            else if (TxtNomCliente.Text == "" || TxtNomCliente.Text == null)
            {
                LblMessage.Text = "Capturar Nombre del Cliente";
                mpeMensaje.Show();
                return;
            }

            Actualizar_ITM_67();

            // inicializar controles.
            TxtCveCliente.Text = string.Empty;
            TxtCveCliente.ReadOnly = false;

            TxtNomCliente.Text = string.Empty;
            TxtNomCliente.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            TxtCveCliente.Text = Server.HtmlDecode(Convert.ToString(GrdCliente.Rows[index].Cells[1].Text));
            TxtNomCliente.Text = Server.HtmlDecode(Convert.ToString(GrdCliente.Rows[index].Cells[3].Text));

            TxtCveCliente.ReadOnly = true;
            TxtNomCliente.ReadOnly = true;

            BtnAnular.Visible = true;
            BtnEditar.Enabled = true;
            BtnAgregar.Enabled = false;
        }

        protected void ImgCheckList_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);

            int index = row.RowIndex;

            string sClave = Server.HtmlDecode(Convert.ToString(GrdCliente.Rows[index].Cells[1].Text));
            string sDescripcion = Server.HtmlDecode(Convert.ToString(GrdCliente.Rows[index].Cells[3].Text));

            Response.Redirect("fwConfiguracion_Aseguradora.aspx?Cve=" + sClave + "&Desc=" + sDescripcion, true);

        }

        protected void BtnProyecto_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwAlta_Proyecto.aspx", true);
        }
    }
}