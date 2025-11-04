using System;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwGM_Catalog_Document : System.Web.UI.Page
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
                GetCarpetas();
                GetDocumentos();
            }
        }

        protected void GetCarpetas()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT Id_Carpeta, Nom_Carpeta " +
                                  "  FROM ITM_80 " +
                                  " WHERE Id_Carpeta NOT IN (1, 5)" +
                                  "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlCarpetas.DataSource = dt;

                ddlCarpetas.DataValueField = "Id_Carpeta";
                ddlCarpetas.DataTextField = "Nom_Carpeta";

                ddlCarpetas.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ddlCarpetas.Items.Insert(0, new ListItem("-- No Hay Carpeta(s) --", "0"));
                }
                else
                {
                    ddlCarpetas.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                }

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetDocumentos()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_20
                string strQuery = "SELECT d.Descripcion, d.DescripBrev, d.IdConsecutivo, d.IdCarpeta, d.DocInterno, " +
                                  "  CASE WHEN d.DocInterno = 1 THEN 'INTERNO' ELSE '' END as TpoInterno " +
                                  "  FROM ITM_80 as p, ITM_20 as d " +
                                  " WHERE p.Id_Carpeta = d.IdCarpeta " +
                                  "   AND d.IdCarpeta = " + ddlCarpetas.SelectedItem.Value + "";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdDocumentos.ShowHeaderWhenEmpty = true;
                    GrdDocumentos.EmptyDataText = "No hay resultados.";
                }

                GrdDocumentos.DataSource = dt;
                GrdDocumentos.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdDocumentos.HeaderRow.TableSection = TableRowSection.TableHeader;
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

        protected void ddlTpoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdDocumentos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdDocumentos.PageIndex = e.NewPageIndex;
                GetDocumentos();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdDocumentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdDocumentos_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdDocumentos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdDocumentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdDocumentos, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = Unit.Pixel(1100);    // Descripcion
                e.Row.Cells[5].Width = Unit.Pixel(150);     // Tpo. Documento
                e.Row.Cells[6].Width = Unit.Pixel(25);      // Editar
                e.Row.Cells[7].Width = Unit.Pixel(25);      // Eliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;     // DescripBrev
                e.Row.Cells[2].Visible = false;     // IdConsecutivo
                e.Row.Cells[3].Visible = false;     // IdCarpeta
                e.Row.Cells[4].Visible = false;     // DocInterno
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;     // DescripBrev
                e.Row.Cells[2].Visible = false;     // IdConsecutivo
                e.Row.Cells[3].Visible = false;     // IdCarpeta
                e.Row.Cells[4].Visible = false;     // DocInterno
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {

                if (ddlCarpetas.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar Tipo de carpeta";
                    mpeMensaje.Show();
                    return;
                }
                else if (TxtDescripBrev.Text == "" || TxtDescripBrev.Text == null)
                {
                    LblMessage.Text = "Capturar Descripción del documento (Corta)";
                    mpeMensaje.Show();
                    return;
                }
                else if (TxtDescripcion.Text == "" || TxtDescripcion.Text == null)
                {
                    LblMessage.Text = "Capturar Descripción del documento (Larga)";
                    mpeMensaje.Show();
                    return;
                }

                int iConsecutivo = GetIdConsecutivoMax();

                int iIdCarpeta = Convert.ToInt32(ddlCarpetas.SelectedValue);

                int iDocInterno = 0;

                if (chkDocInterno.Checked)
                {
                    iDocInterno = 1;
                }

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_20)
                string strQuery = "INSERT INTO ITM_20 (IdConsecutivo, IdCarpeta, Descripcion, DescripBrev, DocInterno, IdStatus) " +
                                  "VALUES (" + iConsecutivo + ", " + iIdCarpeta + ", '" + TxtDescripcion.Text.Trim() + "', " +
                                  "'" + TxtDescripBrev.Text.Trim() + "', " + iDocInterno + ", 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                GetDocumentos();

                LblMessage.Text = "Se agrego documento, correctamente";
                mpeMensaje.Show();

                // Inicializar Controles
                TxtDescripBrev.Text = string.Empty;
                TxtDescripcion.Text = string.Empty;

                chkDocInterno.Checked = false;

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

            string strQuery = "SELECT COALESCE(MAX(IdConsecutivo), 0) + 1 IdConsecutivo " +
                                " FROM ITM_20 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            dbConn.Close();

            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdConsecutivo"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            LblMessage_1.Text = "¿Desea eliminar el documento?";
            mpeMensaje_1.Show();
        }

        protected void Eliminar_ITM_90()
        {
            try
            {
                int index = Variables.wRenglon;

                int iConsecutivo = Convert.ToInt32(GrdDocumentos.Rows[index].Cells[2].Text); ;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_20)
                string strQuery = "DELETE FROM ITM_20 " +
                                  " WHERE IdConsecutivo = " + iConsecutivo + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino documento, correctamente";
                mpeMensaje.Show();

                GetDocumentos();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Actualizar_ITM_90()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int index = Variables.wRenglon;

                int iConsecutivo = Convert.ToInt32(GrdDocumentos.Rows[index].Cells[2].Text); ;

                int iDocInterno = 0;

                if (chkDocInterno.Checked)
                {
                    iDocInterno = 1;
                }

                // Actualizar registro(s) tablas (ITM_20)
                string strQuery = "UPDATE ITM_20 " +
                                  "   SET Descripcion = '" + TxtDescripcion.Text.Trim() + "', " +
                                  "       DescripBrev = '" + TxtDescripBrev.Text.Trim() + "', " +
                                  "       DocInterno = " + iDocInterno + " " +
                                  " WHERE IdConsecutivo = " + iConsecutivo + " ";


                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizo documento, correctamente";
                mpeMensaje.Show();

                GetDocumentos();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            Eliminar_ITM_90();

            // inicializar controles.
            TxtDescripBrev.Text = string.Empty;
            TxtDescripcion.Text = string.Empty;
            chkDocInterno.Checked = false;

        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void ddlCarpetas_SelectedIndexChanged(object sender, EventArgs e)
        {
            // inicializar controles.
            TxtDescripBrev.Text = string.Empty;
            TxtDescripcion.Text = string.Empty;
            chkDocInterno.Checked = false;

            TxtDescripBrev.ReadOnly = false;
            TxtDescripcion.ReadOnly = false;
            chkDocInterno.Enabled = true;

            GetDocumentos();
        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            TxtDescripcion.Text = Server.HtmlDecode(Convert.ToString(GrdDocumentos.Rows[index].Cells[0].Text));
            TxtDescripBrev.Text = Server.HtmlDecode(Convert.ToString(GrdDocumentos.Rows[index].Cells[1].Text));
            chkDocInterno.Checked = GrdDocumentos.Rows[index].Cells[4].Text == "1";

            // chkDocInterno.Checked = Convert.ToBoolean(GrdDocumentos.Rows[index].Cells[4].Text);

            TxtDescripBrev.ReadOnly = true;
            TxtDescripcion.ReadOnly = true;
            chkDocInterno.Enabled = false;

            ddlCarpetas.Enabled = false;

            BtnAnular.Visible = true;
            btnEditar.Enabled = true;
            BtnAgregar.Enabled = false;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            TxtDescripBrev.ReadOnly = false;
            TxtDescripcion.ReadOnly = false;
            chkDocInterno.Enabled = true;

            btnEditar.Visible = false;
            BtnGrabar.Visible = true;

        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {

            if (TxtDescripBrev.Text == "" || TxtDescripBrev.Text == null)
            {
                LblMessage.Text = "Capturar Descripción del documento (Corta)";
                mpeMensaje.Show();
                return;
            }

            if (TxtDescripcion.Text == "" || TxtDescripcion.Text == null)
            {
                LblMessage.Text = "Capturar Descripción del documento (Larga)";
                mpeMensaje.Show();
                return;
            }

            Actualizar_ITM_90();

            // inicializar controles.
            TxtDescripBrev.Text = string.Empty;
            TxtDescripcion.Text = string.Empty;
            chkDocInterno.Checked = false;

            ddlCarpetas.Enabled = true;

            TxtDescripBrev.ReadOnly = false;
            TxtDescripcion.ReadOnly = false;
            chkDocInterno.Enabled = true;

            btnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            btnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnAnular_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            ddlCarpetas.Enabled = true;

            TxtDescripBrev.Text = string.Empty;
            TxtDescripcion.Text = string.Empty;
            chkDocInterno.Checked = false;

            TxtDescripBrev.ReadOnly = false;
            TxtDescripcion.ReadOnly = false;
            chkDocInterno.Enabled = true;

            btnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            btnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }
    }
}