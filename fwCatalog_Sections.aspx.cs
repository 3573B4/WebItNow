using System;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwCatalog_Sections : System.Web.UI.Page
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

                    // GetSecciones();
                    Inicializar_GrdSeccion();

                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

            }
        }

        public void GetSecciones()
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
                    GrdSeccion.ShowHeaderWhenEmpty = true;
                    GrdSeccion.EmptyDataText = "No hay resultados.";
                }

                GrdSeccion.DataSource = dt;
                GrdSeccion.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdSeccion.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        private void Inicializar_GrdSeccion()
        {
            // Crea un DataTable vacío con la estructura necesaria
            DataTable dt = CrearDataTableVacio();

            // Verifica si el DataTable tiene filas
            if (dt.Rows.Count == 0)
            {
                // Mostrar el mensaje de "No hay resultados"
                GrdSeccion.ShowHeaderWhenEmpty = true;
                GrdSeccion.EmptyDataText = "No hay resultados.";
            }

            // Enlaza el DataTable (vacío o lleno) al GridView
            GrdSeccion.DataSource = dt;
            GrdSeccion.DataBind();
        }

        private DataTable CrearDataTableVacio()
        {
            DataTable dt = new DataTable();

            // Define las columnas del DataTable (ITM_)
            dt.Columns.Add("IdSeccion", typeof(string));
            dt.Columns.Add("Descripcion", typeof(string));

            // Agrega más columnas según sea necesario

            return dt;
        }

        protected void GrdSeccion_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdSeccion_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdSeccion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdSeccion.PageIndex = e.NewPageIndex;
                GetSecciones();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdSeccion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdSeccion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdSeccion, "Select$" + e.Row.RowIndex.ToString()) + ";");

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

            LblMessage_1.Text = "¿Desea eliminar la sección?";
            mpeMensaje_1.Show();
        }

        protected void Eliminar_ITM_67()
        {
            try
            {
                int index = Variables.wRenglon;

                int iIdOrden = Convert.ToInt32(GrdSeccion.Rows[index].Cells[2].Text); ;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_67)
                string strQuery = "DELETE FROM ITM_67 " +
                                  " WHERE IdOrden = " + iIdOrden + "";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino sección, correctamente, revisar";
                mpeMensaje.Show();

                GetSecciones();
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

                int iIdOrden = Convert.ToInt32(GrdSeccion.Rows[index].Cells[2].Text); ;
                string IdSeguros = "ZSA";

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Actualizar registro(s) tablas (ITM_67)
                string strQuery = "UPDATE ITM_67 " +
                                  "   SET IdSeguros = '" + IdSeguros + "', " +
                                  "       Descripcion = '" + TxtNomSeccion.Text.Trim() + "' " +
                                  " WHERE IdOrden = " + iIdOrden + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizo sección, correctamente";
                mpeMensaje.Show();

                GetSecciones();
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

                if (TxtNomSeccion.Text == "" || TxtNomSeccion.Text == null)
                {
                    LblMessage.Text = "Capturar Nombre de la Sección";
                    mpeMensaje.Show();
                    return;
                }

                int iIdOrden = GetIdConsecutivoMax();
                string IdSeguros = "ZSA";

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_67)
                string strQuery = "INSERT INTO ITM_67 (IdSeguros, IdOrden, Descripcion, IdStatus) " +
                                  "VALUES ('" + IdSeguros + "', " + iIdOrden + ", '" + TxtNomSeccion.Text.Trim() + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se agrego sección, correctamente";
                mpeMensaje.Show();

                // Inicializar Controles
                TxtNomSeccion.Text = string.Empty;

                GetSecciones();

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

            TxtNomSeccion.Text = string.Empty;
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
            TxtNomSeccion.Text = string.Empty;
            TxtNomSeccion.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            TxtNomSeccion.ReadOnly = false;

            BtnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (TxtNomSeccion.Text == "" || TxtNomSeccion.Text == null)
            {
                LblMessage.Text = "Capturar Nombre de la Sección";
                mpeMensaje.Show();
                return;
            }

            Actualizar_ITM_67();

            // inicializar controles.
            TxtNomSeccion.Text = string.Empty;
            TxtNomSeccion.ReadOnly = false;

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

            TxtNomSeccion.Text = Server.HtmlDecode(Convert.ToString(GrdSeccion.Rows[index].Cells[3].Text));

            TxtNomSeccion.ReadOnly = true;

            BtnAnular.Visible = true;
            BtnEditar.Enabled = true;
            BtnAgregar.Enabled = false;
        }

    }
}