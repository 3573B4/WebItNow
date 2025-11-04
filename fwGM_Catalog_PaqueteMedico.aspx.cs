using System;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwGM_Catalog_PaqueteMedico : System.Web.UI.Page
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
                GetInstituciones();

                Inicializar_GrdPaquetes();
                GetPaqueteMedico(Convert.ToInt32(ddlInstituciones.SelectedValue));

            }
        }

        private void Inicializar_GrdPaquetes()
        {
            // Crea un DataTable vacío con la estructura necesaria
            DataTable dt = CrearDataTableVacio();

            // Verifica si el DataTable tiene filas
            if (dt.Rows.Count == 0)
            {
                // Mostrar el mensaje de "No hay resultados"
                GrdPaquetes.ShowHeaderWhenEmpty = true;
                GrdPaquetes.EmptyDataText = "No hay resultados.";
            }

            // Enlaza el DataTable (vacío o lleno) al GridView
            GrdPaquetes.DataSource = dt;
            GrdPaquetes.DataBind();
        }

        private DataTable CrearDataTableVacio()
        {
            DataTable dt = new DataTable();

            // Define las columnas del DataTable (ITM_)
            dt.Columns.Add("Id_Paquete", typeof(string));
            dt.Columns.Add("Nom_Institucion", typeof(string));
            dt.Columns.Add("Nom_Paquete", typeof(string));
            dt.Columns.Add("Monto_Minimo", typeof(string));
            dt.Columns.Add("Monto_Maximo", typeof(string));

            // Agrega más columnas según sea necesario

            return dt;
        }


        protected void GetInstituciones()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT Id_Institucion, Descripcion " +
                                  "  FROM ITM_13 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlInstituciones.DataSource = dt;

                ddlInstituciones.DataValueField = "Id_Institucion";
                ddlInstituciones.DataTextField = "Descripcion";

                ddlInstituciones.DataBind();
                ddlInstituciones.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetPaqueteMedico(int Id_Institucion)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Proveedores tipo de servicio = ITM_37
                string strQuery = "SELECT Id_Paquete, Id_Institucion, Nom_Paquete, Monto_Minimo, Monto_Maximo " +
                                  "  FROM ITM_37 " +
                                  " WHERE Id_Institucion = " + Id_Institucion + " " +
                                  "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdPaquetes.ShowHeaderWhenEmpty = true;
                    GrdPaquetes.EmptyDataText = "No hay resultados.";
                }

                GrdPaquetes.DataSource = dt;
                GrdPaquetes.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdPaquetes.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }


        protected void GrdPaquetes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdPaquetes.PageIndex = e.NewPageIndex;
                GetPaqueteMedico(Convert.ToInt32(ddlInstituciones.SelectedValue));
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdPaquetes_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdPaquetes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdPaquetes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdPaquetes, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Width = Unit.Pixel(800);     // Nom_Paquete
                e.Row.Cells[3].Width = Unit.Pixel(200);     // Monto_Minimo
                e.Row.Cells[4].Width = Unit.Pixel(200);     // Monto_Maximo
                e.Row.Cells[5].Width = Unit.Pixel(50);      // Editar
                e.Row.Cells[6].Width = Unit.Pixel(50);      // Eliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;     // Id_Paquete
                e.Row.Cells[1].Visible = false;     // Id_Institucion
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;     // Id_Paquete
                e.Row.Cells[1].Visible = false;     // Id_Institucion
            }
        }

        protected void GrdPaquetes_PreRender(object sender, EventArgs e)
        {

        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {

                if (ddlInstituciones.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar Institución Hospitalaria";
                    mpeMensaje.Show();
                    return;
                }
                else if (TxtNomPaquete.Text == "" || TxtNomPaquete.Text == null)
                {
                    LblMessage.Text = "Capturar Nombre del paquete";
                    mpeMensaje.Show();
                    return;
                }
                else if (TxtMontoMinimo.Text == "" || TxtMontoMinimo.Text == null)
                {
                    LblMessage.Text = "Capturar Monto Minimo";
                    mpeMensaje.Show();
                    return;
                }

                else if (TxtMontoMaximo.Text == "" || TxtMontoMaximo.Text == null)
                {
                    LblMessage.Text = "Capturar Monto Maximo";
                    mpeMensaje.Show();
                    return;
                }

                decimal MontoMinimo = 0;

                if (!string.IsNullOrWhiteSpace(TxtMontoMinimo.Text.Trim()))
                {
                    decimal.TryParse(TxtMontoMinimo.Text.Trim(), out MontoMinimo);
                }

                decimal MontoMaximo = 0;

                if (!string.IsNullOrWhiteSpace(TxtMontoMaximo.Text.Trim()))
                {
                    decimal.TryParse(TxtMontoMaximo.Text.Trim(), out MontoMaximo);
                }

                int Id_Paquete = GetIdConsecutivoMax();
                int Id_Institucion = Convert.ToInt32(ddlInstituciones.SelectedValue);

                string Id_Usuario = Variables.wUserLogon;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_37)
                string strQuery = "INSERT INTO ITM_37 (Id_Paquete, Id_Institucion, Nom_Paquete, Monto_Minimo, Monto_Maximo," +
                                  " Id_Usuario, IdStatus) " +
                                  "VALUES (" + Id_Paquete + ", " + Id_Institucion + ", '" + TxtNomPaquete.Text.Trim() + "', " +
                                  "" + MontoMinimo + ", " + MontoMaximo + ", '" + Id_Usuario + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                GetPaqueteMedico(Convert.ToInt32(ddlInstituciones.SelectedValue));

                LblMessage.Text = "Se agregó paquete médico correctamente";
                mpeMensaje.Show();

                // Inicializar Controles
                // ddlInstituciones.SelectedValue = "0";
                TxtNomPaquete.Text = string.Empty;
                TxtMontoMinimo.Text = string.Empty;
                TxtMontoMaximo.Text = string.Empty;

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

            string strQuery = "SELECT COALESCE(MAX(Id_Paquete), 0) + 1 Id_Paquete " +
                                " FROM ITM_37 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            dbConn.Close();

            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["Id_Paquete"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void Eliminar_ITM_37()
        {
            try
            {
                int index = Variables.wRenglon;

                int Id_Paquete = Convert.ToInt32(GrdPaquetes.Rows[index].Cells[0].Text);
                int Id_Institucion = Convert.ToInt32(GrdPaquetes.Rows[index].Cells[1].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_37)
                string strQuery = "DELETE FROM ITM_37 " +
                                  " WHERE Id_Paquete = " + Id_Paquete + " " +
                                  "   AND Id_Institucion = " + Id_Institucion + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se eliminó correctamente el paquete médico";
                mpeMensaje.Show();

                GetPaqueteMedico(Convert.ToInt32(ddlInstituciones.SelectedValue));
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Actualizar_ITM_37()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int index = Variables.wRenglon;

                int Id_Paquete = Convert.ToInt32(GrdPaquetes.Rows[index].Cells[0].Text);
                int Id_Institucion = Convert.ToInt32(GrdPaquetes.Rows[index].Cells[1].Text);

                decimal MontoMinimo = 0;

                if (!string.IsNullOrWhiteSpace(TxtMontoMinimo.Text.Trim()))
                {
                    decimal.TryParse(TxtMontoMinimo.Text.Trim(), out MontoMinimo);
                }

                decimal MontoMaximo = 0;

                if (!string.IsNullOrWhiteSpace(TxtMontoMaximo.Text.Trim()))
                {
                    decimal.TryParse(TxtMontoMaximo.Text.Trim(), out MontoMaximo);
                }

                // Actualizar registro(s) tablas (ITM_37)
                string strQuery = "UPDATE ITM_37 " +
                                  "   SET Nom_Paquete = '" + TxtNomPaquete.Text.Trim() + "', " +
                                  "       Monto_Minimo = " + MontoMinimo + ", " +
                                  "       Monto_Maximo = " + MontoMaximo + " " +
                                  " WHERE Id_Paquete = " + Id_Paquete + " " +
                                  "   AND Id_Institucion = " + Id_Institucion + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizó correctamente el paquete médico";
                mpeMensaje.Show();

                GetPaqueteMedico(Convert.ToInt32(ddlInstituciones.SelectedValue));
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (TxtNomPaquete.Text == "" || TxtNomPaquete.Text == null)
            {
                LblMessage.Text = "Capturar Nombre del paquete";
                mpeMensaje.Show();
                return;
            }
            else if (TxtMontoMinimo.Text == "" || TxtMontoMinimo.Text == null)
            {
                LblMessage.Text = "Capturar Monto Minimo";
                mpeMensaje.Show();
                return;
            }
            else if (TxtMontoMaximo.Text == "" || TxtMontoMaximo.Text == null)
            {
                LblMessage.Text = "Capturar Monto Maximo";
                mpeMensaje.Show();
                return;
            }

            Actualizar_ITM_37();

            // inicializar controles.
            // ddlInstituciones.SelectedValue = "0";
            ddlInstituciones.Enabled = true;

            TxtNomPaquete.Text = string.Empty;
            TxtMontoMinimo.Text = string.Empty;
            TxtMontoMaximo.Text = string.Empty;

            btnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            btnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            TxtNomPaquete.ReadOnly = false;
            TxtMontoMinimo.ReadOnly = false;
            TxtMontoMaximo.ReadOnly = false;

            btnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnAnular_Click(object sender, EventArgs e)
        {
            // Inicializar Controles
            // ddlInstituciones.SelectedValue = "0";

            TxtNomPaquete.Text = string.Empty;
            TxtMontoMinimo.Text = string.Empty;
            TxtMontoMaximo.Text = string.Empty;

            ddlInstituciones.Enabled = true;

            TxtNomPaquete.ReadOnly = false;
            TxtMontoMinimo.ReadOnly = false;
            TxtMontoMaximo.ReadOnly = false;

            btnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            btnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            ddlInstituciones.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdPaquetes.Rows[index].Cells[1].Text));
            TxtNomPaquete.Text = Server.HtmlDecode(Convert.ToString(GrdPaquetes.Rows[index].Cells[2].Text));
            TxtMontoMinimo.Text = Server.HtmlDecode(Convert.ToString(GrdPaquetes.Rows[index].Cells[3].Text));
            TxtMontoMaximo.Text = Server.HtmlDecode(Convert.ToString(GrdPaquetes.Rows[index].Cells[4].Text));

            TxtNomPaquete.ReadOnly = true;
            TxtMontoMinimo.ReadOnly = true;
            TxtMontoMaximo.ReadOnly = true;

            ddlInstituciones.Enabled = false;

            BtnAnular.Visible = true;
            btnEditar.Enabled = true;
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

            LblMessage_1.Text = "¿Desea eliminar el paquete médico?";
            mpeMensaje_1.Show();
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            Eliminar_ITM_37();

            // inicializar controles.
            // ddlInstituciones.SelectedValue = "0";

            TxtNomPaquete.Text = string.Empty;
            TxtMontoMinimo.Text = string.Empty;
            TxtMontoMaximo.Text = string.Empty;
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

        protected void ddlInstituciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetPaqueteMedico(Convert.ToInt32(ddlInstituciones.SelectedValue));
        }
    }
}