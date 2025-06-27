using System;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwGM_Catalog_Responsable : System.Web.UI.Page
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

                    GetResponsables();

                    Inicializar_GrdResponsable();

                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

            }
        }

        protected void GetResponsables()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdResponsable, Descripcion " +
                                  "  FROM ITM_89 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlResponsables.DataSource = dt;

                ddlResponsables.DataValueField = "IdResponsable";
                ddlResponsables.DataTextField = "Descripcion";

                ddlResponsables.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ddlResponsables.Items.Insert(0, new ListItem("-- No Hay Carpeta(s) --", "0"));
                }
                else
                {
                    ddlResponsables.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                }

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetResponsables(string IdColumna, string Tabla)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_68, ITM_69, ITM_79
                string strQuery = $"SELECT { IdColumna } as IdDocumento, Descripcion " +
                                  $"  FROM { Tabla } " +
                                  $" WHERE IdStatus = 1 " +
                                  $" ORDER BY { IdColumna }";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdResponsables.ShowHeaderWhenEmpty = true;
                    GrdResponsables.EmptyDataText = "No hay resultados.";
                }

                GrdResponsables.DataSource = dt;
                GrdResponsables.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdResponsables.HeaderRow.TableSection = TableRowSection.TableHeader;
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
                GrdResponsables.ShowHeaderWhenEmpty = true;
                GrdResponsables.EmptyDataText = "No hay resultados.";
            }

            // Enlaza el DataTable (vacío o lleno) al GridView
            GrdResponsables.DataSource = dt;
            GrdResponsables.DataBind();
        }

        private DataTable CrearDataTableVacio()
        {
            DataTable dt = new DataTable();

            // Define las columnas del DataTable
            dt.Columns.Add("IdDocumento", typeof(string));
            dt.Columns.Add("Descripcion", typeof(string));
            // Agrega más columnas según sea necesario

            return dt;
        }

        public int Add_tbDocumentos(string IdColumna, string Tabla, string pDescripcion)
        {
            try
            {
                int iConsecutivo = GetIdConsecutivoMax(IdColumna, Tabla);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = $"INSERT INTO { Tabla } ({ IdColumna }, Descripcion, DescripBrev, IdStatus) " +
                                  $"VALUES (" + iConsecutivo + ", '" + pDescripcion + "', Null, 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se agrego responsable, correctamente";
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

        public int GetIdConsecutivoMax(string IdColumna, string Tabla)
        {

            int IdConsecutivoMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = $"SELECT COALESCE(MAX( { IdColumna } ), 0) + 1 IdDocumento " +
                              $"  FROM { Tabla } ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdDocumento"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void Eliminar_tbDocumentos(string IdColumna, string Tabla, int iIdDocumento, int iIdTpoAsunto)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName; //LblUsuario.Text;

                // Eliminar registro tabla
                string strQuery = $"DELETE FROM { Tabla } " +
                                  $" WHERE { IdColumna } = " + iIdDocumento + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino responsable, correctamente";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    LblMessage.Text = "Responsable, se encuentra relacionado a un Asunto";
                }
                else
                {
                    LblMessage.Text = Convert.ToString(ex.Message);
                }

                mpeMensaje.Show();
            }
        }

        protected void Actualizar_tbDocumentos(string IdColumna, string Tabla, int iIdDocumento, int iIdTpoAsunto)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName; //LblUsuario.Text;

                // Eliminar registro tabla
                string strQuery = $"UPDATE { Tabla } " +
                                  $"   SET Descripcion = '" + TxtNomResponsable.Text.Trim() + "' " +
                                  $" WHERE { IdColumna } = " + iIdDocumento + " ";
                //$"   AND IdTpoAsunto = " + iIdTpoAsunto + "";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizo responsable, correctamente";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    LblMessage.Text = "Responsable, se encuentra relacionado a un Asunto";
                }
                else
                {
                    LblMessage.Text = Convert.ToString(ex.Message);
                }

                mpeMensaje.Show();
            }
        }

        protected void GrdResponsables_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdResponsables.PageIndex = e.NewPageIndex;
                GetResponsables(Variables.wIdColumna, Variables.wTabla);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdResponsables_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdResponsables_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdResponsables_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdResponsables_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdResponsables, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Width = Unit.Pixel(1100);    // Descripcion
                e.Row.Cells[2].Width = Unit.Pixel(25);      // ImgEditar
                e.Row.Cells[3].Width = Unit.Pixel(25);      // ImgEliminar
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;         // IdDocumento
             // e.Row.Cells[2].Visible = false;         // IdTpoAsunto
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;         // IdDocumento
             // e.Row.Cells[2].Visible = false;         // IdTpoAsunto
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {

            if (ddlResponsables.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar tipo de Responsable";
                mpeMensaje.Show();
                return;
            }
            else if (TxtNomResponsable.Text == "" || TxtNomResponsable.Text == null)
            {
                LblMessage.Text = "Capturar Nombre de Responsable";
                mpeMensaje.Show();
                return;
            }

            string sDescripcion = TxtNomResponsable.Text;
            string sTabla = Variables.wTabla;

            int Envio_Ok = Add_tbDocumentos(Variables.wIdColumna, sTabla, sDescripcion);

            if (Envio_Ok == 0)
            {

                // inicializar controles
                TxtNomResponsable.Text = string.Empty;

                GetResponsables(Variables.wIdColumna, sTabla);
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

            LblMessage_1.Text = "¿Desea eliminar el responsable ?";
            mpeMensaje_1.Show();

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {

            int index = Variables.wRenglon;

            int iIdDocumento = Convert.ToInt32(GrdResponsables.Rows[index].Cells[0].Text);
            //int iIdTpoAsunto = Convert.ToInt32(GrdResponsables.Rows[index].Cells[2].Text);
            int iIdTpoAsunto = 0;

            Eliminar_tbDocumentos(Variables.wIdColumna, Variables.wTabla, iIdDocumento, iIdTpoAsunto);

            GetResponsables(Variables.wIdColumna, Variables.wTabla);
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

            TxtNomResponsable.Text = Server.HtmlDecode(Convert.ToString(GrdResponsables.Rows[index].Cells[1].Text));

            TxtNomResponsable.ReadOnly = true;
            TxtNomResponsable.ReadOnly = true;

            ddlResponsables.Enabled = false;

            BtnAnular.Visible = true;
            BtnEditar.Enabled = true;
            BtnAgregar.Enabled = false;
        }

        protected void BtnAnular_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            ddlResponsables.Enabled = true;

            TxtNomResponsable.Text = string.Empty;
            TxtNomResponsable.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            TxtNomResponsable.ReadOnly = false;
            TxtNomResponsable.ReadOnly = false;

            BtnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (TxtNomResponsable.Text == "" || TxtNomResponsable.Text == null)
            {
                LblMessage.Text = "Capturar Nombre de Responsable";
                mpeMensaje.Show();
                return;
            }

            int index = Variables.wRenglon;

            int iIdDocumento = Convert.ToInt32(GrdResponsables.Rows[index].Cells[0].Text);
            int iIdTpoAsunto = 0;

            Actualizar_tbDocumentos(Variables.wIdColumna, Variables.wTabla, iIdDocumento, iIdTpoAsunto);

            GetResponsables(Variables.wIdColumna, Variables.wTabla);

            // inicializar controles.
            ddlResponsables.Enabled = true;

            TxtNomResponsable.Text = string.Empty;
            TxtNomResponsable.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void ddlResponsables_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlResponsables.SelectedValue)
            {
                case "1":
                    // Administrativos
                    Variables.wIdColumna = "IdRespAdministrativo";
                    Variables.wTabla = "ITM_14";

                    GetResponsables(Variables.wIdColumna, Variables.wTabla);

                    break;
                case "2":
                    // Tecnicos
                    Variables.wIdColumna = "IdRespMedico";
                    Variables.wTabla = "ITM_12";

                    GetResponsables(Variables.wIdColumna, Variables.wTabla);

                    break;
                default:
                    Inicializar_GrdResponsable();
                    break;
            }
        }
    }
}