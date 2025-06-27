using System;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwCatalog_Productos : System.Web.UI.Page
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
                if (Session["IdUsuario"] == null || Session["UsPassword"] == null)
                {
                    Response.Redirect("Login.aspx");
                }

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

                    Inicializar_GrdProductos();

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

                //SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //da.Fill(dt);

                ddlCliente.DataSource = dt;

                ddlCliente.DataValueField = "IdSeguros";
                ddlCliente.DataTextField = "Descripcion";

                ddlCliente.DataBind();
                ddlCliente.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                //Conecta.Cerrar();
                //cmd.Dispose();
                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        private void Inicializar_GrdProductos()
        {
            // Crea un DataTable vacío con la estructura necesaria
            DataTable dt = CrearDataTableVacio();

            // Verifica si el DataTable tiene filas
            if (dt.Rows.Count == 0)
            {
                // Mostrar el mensaje de "No hay resultados"
                GrdProductos.ShowHeaderWhenEmpty = true;
                GrdProductos.EmptyDataText = "No hay resultados.";
            }

            // Enlaza el DataTable (vacío o lleno) al GridView
            GrdProductos.DataSource = dt;
            GrdProductos.DataBind();
        }

        private DataTable CrearDataTableVacio()
        {
            DataTable dt = new DataTable();

            // Define las columnas del DataTable
            dt.Columns.Add("IdProducto", typeof(string));
            dt.Columns.Add("Descripcion", typeof(string));
            dt.Columns.Add("Num_Condusef", typeof(string));
            // Agrega más columnas según sea necesario

            return dt;
        }

        public void GetProductos(string sIdSeguro)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a la tabla : Coberturas  = ITM_39
                string strQuery = "SELECT IdProducto, Descripcion, Num_Condusef " +
                                  "  FROM ITM_39 " +
                                  " WHERE IdStatus = 1 " +
                                  "   AND IdSeguros = '" + sIdSeguro + "' " +
                                  " ORDER BY IdProducto";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdProductos.ShowHeaderWhenEmpty = true;
                    GrdProductos.EmptyDataText = "No hay resultados.";
                }

                GrdProductos.DataSource = dt;
                GrdProductos.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdProductos.HeaderRow.TableSection = TableRowSection.TableHeader;
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

            int iIdProducto = Convert.ToInt32(GrdProductos.Rows[index].Cells[0].Text);

            Eliminar_tbProducto(ddlCliente.SelectedValue, iIdProducto);

            GetProductos(ddlCliente.SelectedValue);
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            LblMessage_1.Text = "¿Desea eliminar el producto ?";
            mpeMensaje_1.Show();
        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            TxtNomProducto.Text = Server.HtmlDecode(Convert.ToString(GrdProductos.Rows[index].Cells[1].Text));
            TxtNumCondusef.Text = Server.HtmlDecode(Convert.ToString(GrdProductos.Rows[index].Cells[2].Text));

            TxtNomProducto.ReadOnly = true;
            TxtNumCondusef.ReadOnly = true;

            ddlCliente.Enabled = false;

            BtnAnular.Visible = true;
            BtnEditar.Enabled = true;
            BtnAgregar.Enabled = false;
        }

        protected void GrdProductos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdProductos, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Width = Unit.Pixel(550);     // Descripcion
                e.Row.Cells[2].Width = Unit.Pixel(550);     // Num_Condusef
                e.Row.Cells[3].Width = Unit.Pixel(25);      // ImgEditar
                e.Row.Cells[4].Width = Unit.Pixel(25);      // ImgEliminar
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;         // IdProducto
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;         // IdProducto
            }
        }

        protected void GrdProductos_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdProductos.PageIndex = e.NewPageIndex;
                GetProductos(ddlCliente.SelectedValue);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public int Add_tbProducto(string sIdSeguros, string pDescripcion, string pNumCondusef)
        {
            try
            {
                int iConsecutivo = GetIdConsecutivoMax();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "INSERT INTO ITM_39 (IdProducto, IdSeguros, Descripcion, Num_Condusef, IdStatus) " +
                                  "VALUES (" + iConsecutivo + ", '" + sIdSeguros + "', '" + pDescripcion + "', '" + pNumCondusef + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se agrego producto, correctamente";
                mpeMensaje.Show();

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

        public int GetIdConsecutivoMax()
        {

            int IdConsecutivoMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX(IdProducto), 0) + 1 IdProducto " +
                              "  FROM ITM_39 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdProducto"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void Eliminar_tbProducto(string sIdSeguros, int iIdCobertura)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName; //LblUsuario.Text;

                // Eliminar registro tabla
                string strQuery = "DELETE FROM ITM_39 " +
                                  " WHERE IdProducto = " + iIdCobertura + " " +
                                  "   AND IdSeguros = '" + sIdSeguros + "' ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino producto, correctamente";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    LblMessage.Text = "Producto, se encuentra relacionado a un Asunto";
                }
                else
                {
                    LblMessage.Text = Convert.ToString(ex.Message);
                }

                mpeMensaje.Show();
            }
        }
        protected void BtnAnular_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            ddlCliente.Enabled = true;

            TxtNomProducto.Text = string.Empty;
            TxtNomProducto.ReadOnly = false;

            TxtNumCondusef.Text = string.Empty;
            TxtNumCondusef.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            TxtNomProducto.ReadOnly = false;
            TxtNumCondusef.ReadOnly = false;

            BtnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (TxtNomProducto.Text == "" || TxtNomProducto.Text == null)
            {
                LblMessage.Text = "Capturar Descripción de Producto";
                mpeMensaje.Show();
                return;
            }

            int index = Variables.wRenglon;

            int iIdProducto = Convert.ToInt32(GrdProductos.Rows[index].Cells[0].Text);
            string sIdSeguros = ddlCliente.SelectedValue;

            Actualizar_tbProducto(sIdSeguros, iIdProducto);

            GetProductos(ddlCliente.SelectedValue);

            // inicializar controles.
            ddlCliente.Enabled = true;

            TxtNomProducto.Text = string.Empty;
            TxtNomProducto.ReadOnly = false;

            TxtNumCondusef.Text = string.Empty;
            TxtNumCondusef.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void Actualizar_tbProducto(string sIdSeguros, int iIdProducto)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName;

                // Eliminar registro tabla
                string strQuery = "UPDATE ITM_39 " +
                                  "   SET Descripcion = '" + TxtNomProducto.Text.Trim() + "', " +
                                  "       Num_Condusef = '" + TxtNumCondusef.Text.Trim() + "' " +
                                  " WHERE IdProducto = " + iIdProducto + " " +
                                  "   AND IdSeguros = '" + sIdSeguros + "' ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizo producto, correctamente";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    LblMessage.Text = "Cobertura, se encuentra relacionada a un Asunto";
                }
                else
                {
                    LblMessage.Text = Convert.ToString(ex.Message);
                }

                mpeMensaje.Show();
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (ddlCliente.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Compañia de Seguros";
                mpeMensaje.Show();
                return;
            }
            else if (TxtNomProducto.Text == "" || TxtNomProducto.Text == null)
            {
                LblMessage.Text = "Capturar Descripción de Producto";
                mpeMensaje.Show();
                return;
            }

            string sDescripcion = TxtNomProducto.Text;
            string sNumCondusef = TxtNumCondusef.Text;

            int Envio_Ok = Add_tbProducto(ddlCliente.SelectedValue, sDescripcion, sNumCondusef);

            if (Envio_Ok == 0)
            {

                // inicializar controles
                TxtNomProducto.Text = string.Empty;
                TxtNumCondusef.Text = string.Empty;

                GetProductos(ddlCliente.SelectedValue);
            }
        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetProductos(ddlCliente.SelectedValue);
        }
    }
}