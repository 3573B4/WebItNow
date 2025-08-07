using System;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwGM_Catalog_Adjusters : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
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

                    Inicializar_GrdResponsable();
                    GetAjustadores();

                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

            }
        }


        public void GetAjustadores()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_45
                string strQuery = "SELECT IdAjustador, NomAjustador, Email_Ajustador, Tel_Ajustador " +
                                  "  FROM ITM_45 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdAjustadores.ShowHeaderWhenEmpty = true;
                    GrdAjustadores.EmptyDataText = "No hay resultados.";
                }

                GrdAjustadores.DataSource = dt;
                GrdAjustadores.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdAjustadores.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        private void Inicializar_GrdResponsable()
        {
            // Crea un DataTable vacío con la estructura necesaria
            DataTable dt = CrearDataTableVacio();

            // Verifica si el DataTable tiene filas
            if (dt.Rows.Count == 0)
            {
                // Mostrar el mensaje de "No hay resultados"
                GrdAjustadores.ShowHeaderWhenEmpty = true;
                GrdAjustadores.EmptyDataText = "No hay resultados.";
            }

            // Enlaza el DataTable (vacío o lleno) al GridView
            GrdAjustadores.DataSource = dt;
            GrdAjustadores.DataBind();
        }

        private DataTable CrearDataTableVacio()
        {
            DataTable dt = new DataTable();

            // Define las columnas del DataTable
            dt.Columns.Add("IdAjustador", typeof(string));
            dt.Columns.Add("NomAjustador", typeof(string));
            dt.Columns.Add("Email_Ajustador", typeof(string));
            dt.Columns.Add("Tel_Ajustador", typeof(string));
            // Agrega más columnas según sea necesario

            return dt;
        }

        public int Add_tbDocumentos()
        {
            try
            {
                int iConsecutivo = GetIdConsecutivoMax();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "INSERT INTO ITM_45 (IdAjustador, NomAjustador, Email_Ajustador, Tel_Ajustador, IdStatus) " +
                                  "VALUES (" + iConsecutivo + ", '" + TxtNomAjustador.Text.Trim() + "', '" + TxtEmail.Text.Trim() + "', '" + TxtTelCelular.Text.Trim() + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se agrego ajustador, correctamente";
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

            string strQuery = $"SELECT COALESCE(MAX( IdAjustador ), 0) + 1 IdAjustador " +
                              $"  FROM ITM_45 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdAjustador"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void Eliminar_tbDocumentos(int iIdAjustador)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName; //LblUsuario.Text;

                // Eliminar registro tabla
                string strQuery = "DELETE FROM ITM_45 " +
                                  " WHERE IdAjustador = " + iIdAjustador + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino ajustador, correctamente";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    LblMessage.Text = "Ajustador, se encuentra relacionado a un Asunto";
                }
                else
                {
                    LblMessage.Text = Convert.ToString(ex.Message);
                }

                mpeMensaje.Show();
            }
        }

        protected void Actualizar_tbDocumentos(int iIdAjustador)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName; //LblUsuario.Text;

                // Eliminar registro tabla
                string strQuery = "UPDATE ITM_45 " +
                                  "   SET NomAjustador = '" + TxtNomAjustador.Text.Trim() + "'," +
                                  "       Email_Ajustador = '" + TxtEmail.Text.Trim() + "'," +
                                  "       Tel_Ajustador = '" + TxtTelCelular.Text.Trim() + "' " +
                                  " WHERE IdAjustador = " + iIdAjustador + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizo ajustador, correctamente";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    LblMessage.Text = "Ajustador, se encuentra relacionado a un Asunto";
                }
                else
                {
                    LblMessage.Text = Convert.ToString(ex.Message);
                }

                mpeMensaje.Show();
            }
        }

        protected void GrdAjustadores_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdAjustadores_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdAjustadores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdAjustadores.PageIndex = e.NewPageIndex;
                GetAjustadores();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdAjustadores_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdAjustadores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdAjustadores, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Width = Unit.Pixel(450);     // NomAjustador
                e.Row.Cells[2].Width = Unit.Pixel(350);     // Email_Ajustador
                e.Row.Cells[3].Width = Unit.Pixel(300);     // Tel_Ajustador
                e.Row.Cells[4].Width = Unit.Pixel(25);      // ImgEditar
                e.Row.Cells[5].Width = Unit.Pixel(25);      // ImgEliminar
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;             // IdAjustador
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;             // IdAjustador
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {

            if (TxtNomAjustador.Text == "" || TxtNomAjustador.Text == null)
            {
                LblMessage.Text = "Capturar Nombre de Ajustador";
                mpeMensaje.Show();
                return;
            }

            int Envio_Ok = Add_tbDocumentos();

            if (Envio_Ok == 0)
            {

                // inicializar controles
                TxtNomAjustador.Text = string.Empty;
                TxtEmail.Text = string.Empty;
                TxtTelCelular.Text = string.Empty;

                GetAjustadores();
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

            LblMessage_1.Text = "¿Desea eliminar el ajustador ?";
            mpeMensaje_1.Show();

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {

            int index = Variables.wRenglon;

            int iIdAjustador = Convert.ToInt32(GrdAjustadores.Rows[index].Cells[0].Text);

            Eliminar_tbDocumentos(iIdAjustador);

            GetAjustadores();
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            TxtNomAjustador.Text = Server.HtmlDecode(Convert.ToString(GrdAjustadores.Rows[index].Cells[1].Text));
            TxtEmail.Text = Server.HtmlDecode(Convert.ToString(GrdAjustadores.Rows[index].Cells[2].Text));
            TxtTelCelular.Text = Server.HtmlDecode(Convert.ToString(GrdAjustadores.Rows[index].Cells[3].Text));

            TxtNomAjustador.ReadOnly = true;
            TxtEmail.ReadOnly = true;
            TxtTelCelular.ReadOnly = true;

            BtnAnular.Visible = true;
            BtnEditar.Enabled = true;
            BtnAgregar.Enabled = false;
        }

        protected void BtnAnular_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            TxtNomAjustador.Text = string.Empty;
            TxtNomAjustador.ReadOnly = false;

            TxtEmail.Text = string.Empty;
            TxtEmail.ReadOnly = false;

            TxtTelCelular.Text = string.Empty;
            TxtTelCelular.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            TxtNomAjustador.ReadOnly = false;
            TxtEmail.ReadOnly = false;
            TxtTelCelular.ReadOnly = false;

            BtnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (TxtNomAjustador.Text == "" || TxtNomAjustador.Text == null)
            {
                LblMessage.Text = "Capturar Nombre de Ajustador";
                mpeMensaje.Show();
                return;
            }

            int index = Variables.wRenglon;

            int iIdAjustador = Convert.ToInt32(GrdAjustadores.Rows[index].Cells[0].Text);

            Actualizar_tbDocumentos(iIdAjustador);

            GetAjustadores();

            // inicializar controles.
            TxtNomAjustador.Text = string.Empty;
            TxtNomAjustador.ReadOnly = false;

            TxtEmail.Text = string.Empty;
            TxtEmail.ReadOnly = false;

            TxtTelCelular.Text = string.Empty;
            TxtTelCelular.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

    }
}