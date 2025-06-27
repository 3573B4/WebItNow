using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwCatalog_Tasks : System.Web.UI.Page
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
                GetTareas();
            }
        }

        public void GetTareas()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_54
                string strQuery = "SELECT IdTarea, NomTarea, Plazo, DocInterno, " +
                                  "  CASE WHEN DocInterno = 1 THEN 'INTERNO' ELSE '' END as TpoInterno " +
                                        " FROM ITM_54 " +
                                        " WHERE IdStatus = 1 ORDER BY IdTarea";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdTasks.ShowHeaderWhenEmpty = true;
                    GrdTasks.EmptyDataText = "No hay resultados.";
                }

                GrdTasks.DataSource = dt;
                GrdTasks.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdTasks.HeaderRow.TableSection = TableRowSection.TableHeader;
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
            Eliminar_ITM_54();

            // inicializar controles.
            TxtTarea.Text = string.Empty;
            TxtPlazo.Text = string.Empty;
            chkTaskInterno.Checked = false;
        }


        protected void Eliminar_ITM_54()
        {
            try
            {
                int index = Variables.wRenglon;

                int IdTarea = Convert.ToInt32(GrdTasks.Rows[index].Cells[1].Text); ;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_54)
                string strQuery = "DELETE FROM ITM_54 " +
                                  " WHERE IdTarea = " + IdTarea + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino tarea, correctamente";
                mpeMensaje.Show();

                GetTareas();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Actualizar_ITM_54()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int index = Variables.wRenglon;

                int IdTarea = Convert.ToInt32(GrdTasks.Rows[index].Cells[1].Text); ;

                int iDocInterno = 0;

                if (chkTaskInterno.Checked)
                {
                    iDocInterno = 1;
                }

                // Actualizar registro(s) tablas (ITM_54)
                string strQuery = "UPDATE ITM_54 " +
                                  "   SET NomTarea = '" + TxtTarea.Text.Trim() + "', " +
                                  "       Plazo = '" + TxtPlazo.Text.Trim() + "', " +
                                  "       DocInterno = " + iDocInterno + " " +
                                  " WHERE IdTarea = " + IdTarea + " ";


                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizo tarea, correctamente";
                mpeMensaje.Show();

                GetTareas();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void GrdTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdTasks, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = Unit.Pixel(1100);    // Tarea
                e.Row.Cells[3].Width = Unit.Pixel(1100);    // Plazo
                e.Row.Cells[4].Width = Unit.Pixel(150);     // Tpo. Documento
                e.Row.Cells[5].Width = Unit.Pixel(25);      // Editar
                e.Row.Cells[6].Width = Unit.Pixel(25);      // Eliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;     // IdTarea
                e.Row.Cells[2].Visible = false;     // IdConsecutivo
                //e.Row.Cells[3].Visible = false;     // IdCarpeta
                //e.Row.Cells[4].Visible = false;     // DocInterno
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;     // DescripBrev
                e.Row.Cells[2].Visible = false;     // IdConsecutivo
                //e.Row.Cells[3].Visible = false;     // IdCarpeta
                //e.Row.Cells[4].Visible = false;     // DocInterno
            }
        }

        protected void GrdTasks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdTasks_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdTasks_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdTasks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdTasks.PageIndex = e.NewPageIndex;
                GetTareas();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            TxtTarea.Text = Server.HtmlDecode(Convert.ToString(GrdTasks.Rows[index].Cells[0].Text));
            TxtPlazo.Text = Server.HtmlDecode(Convert.ToString(GrdTasks.Rows[index].Cells[3].Text));
            chkTaskInterno.Checked = GrdTasks.Rows[index].Cells[2].Text == "1";

            // chkDocInterno.Checked = Convert.ToBoolean(GrdDocumentos.Rows[index].Cells[4].Text);

            TxtTarea.ReadOnly = true;
            TxtPlazo.ReadOnly = true;
            chkTaskInterno.Enabled = false;

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

            LblMessage_1.Text = "¿Desea eliminar la tarea?";
            mpeMensaje_1.Show();
        }

        protected void BtnAnular_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            TxtTarea.Text = string.Empty;
            TxtPlazo.Text = string.Empty;
            chkTaskInterno.Checked = false;

            TxtTarea.ReadOnly = false;
            TxtPlazo.ReadOnly = false;
            chkTaskInterno.Enabled = true;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            TxtTarea.ReadOnly = false;
            TxtPlazo.ReadOnly = false;
            chkTaskInterno.Enabled = true;

            BtnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (TxtTarea.Text == "" || TxtTarea.Text == null)
            {
                LblMessage.Text = "Capturar Descripción de la Tarea";
                mpeMensaje.Show();
                return;
            }

            if (TxtPlazo.Text == "" || TxtPlazo.Text == null)
            {
                LblMessage.Text = "Capturar Descripción del Plazo";
                mpeMensaje.Show();
                return;
            }

            Actualizar_ITM_54();

            // inicializar controles.
            TxtTarea.Text = string.Empty;
            TxtPlazo.Text = string.Empty;
            chkTaskInterno.Checked = false;

            TxtTarea.ReadOnly = false;
            TxtPlazo.ReadOnly = false;
            chkTaskInterno.Enabled = true;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {

                if (TxtTarea.Text == "" || TxtTarea.Text == null)
                {
                    LblMessage.Text = "Capturar Descripción de la Tarea";
                    mpeMensaje.Show();
                    return;
                }
                else if (TxtPlazo.Text == "" || TxtPlazo.Text == null)
                {
                    LblMessage.Text = "Capturar Descripción del Plazo";
                    mpeMensaje.Show();
                    return;
                }

                int iConsecutivo = GetIdConsecutivoMax();

                int iDocInterno = 0;

                if (chkTaskInterno.Checked)
                {
                    iDocInterno = 1;
                }

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_54)
                string strQuery = "INSERT INTO ITM_54 (IdTarea, NomTarea, Plazo, DocInterno, IdStatus) " +
                                  "VALUES (" + iConsecutivo + ", '" + TxtTarea.Text.Trim() + "', " +
                                  "'" + TxtPlazo.Text.Trim() + "', " + iDocInterno + ", 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                GetTareas();

                LblMessage.Text = "Se agrego tarea, correctamente";
                mpeMensaje.Show();

                // Inicializar Controles
                TxtTarea.Text = string.Empty;
                TxtPlazo.Text = string.Empty;

                chkTaskInterno.Checked = false;

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

            string strQuery = "SELECT COALESCE(MAX(IdTarea), 0) + 1 IdTarea " +
                                " FROM ITM_54 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            dbConn.Close();

            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdTarea"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }


    }
}