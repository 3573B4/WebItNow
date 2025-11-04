using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwCatalog_StSiniestro : System.Web.UI.Page
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

                    // Labels
                    lblTitulo_Cat_Estatus.Text = GetGlobalResourceObject("GlobalResources", "lblTitulo_Cat_Estatus").ToString();

                    Inicializar_GrdStSiniestro();

                    GetStSiniestro();

                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

            }
        }

        private void Inicializar_GrdStSiniestro()
        {
            // Crea un DataTable vacío con la estructura necesaria
            DataTable dt = CrearDataTableVacio();

            // Verifica si el DataTable tiene filas
            if (dt.Rows.Count == 0)
            {
                // Mostrar el mensaje de "No hay resultados"
                GrdStSiniestro.ShowHeaderWhenEmpty = true;
                GrdStSiniestro.EmptyDataText = GetGlobalResourceObject("GlobalResources", "msg_NoResults").ToString();
                
                // GrdStSiniestro.EmptyDataText = "No hay resultados.";
            }

            // Enlaza el DataTable (vacío o lleno) al GridView
            GrdStSiniestro.DataSource = dt;
            GrdStSiniestro.DataBind();
        }

        private DataTable CrearDataTableVacio()
        {
            DataTable dt = new DataTable();

            // Define las columnas del DataTable
            dt.Columns.Add("IdEstStatus", typeof(string));
            dt.Columns.Add("Cve_EstStatus", typeof(string));
            dt.Columns.Add("Descripcion", typeof(string));

            // Agrega más columnas según sea necesario

            return dt;
        }

        public void GetStSiniestro()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_52
                string strQuery = "SELECT IdEstStatus, Cve_EstStatus, Descripcion " +
                                        " FROM ITM_52 " +
                                        " WHERE IdStatus = 1";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdStSiniestro.ShowHeaderWhenEmpty = true;
                    GrdStSiniestro.EmptyDataText = GetGlobalResourceObject("GlobalResources", "msg_NoResults").ToString();
                    // GrdStSiniestro.EmptyDataText = "No hay resultados.";
                }

                GrdStSiniestro.DataSource = dt;
                GrdStSiniestro.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdStSiniestro.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdStSiniestro_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdStSiniestro_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdStSiniestro_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdStSiniestro.PageIndex = e.NewPageIndex;
                GetStSiniestro();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdStSiniestro_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdStSiniestro_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdStSiniestro, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Width = Unit.Pixel(100);     // Cve_EstStatus
                e.Row.Cells[2].Width = Unit.Pixel(1200);    // Descripcion
                e.Row.Cells[3].Width = Unit.Pixel(50);      // Editar
                e.Row.Cells[4].Width = Unit.Pixel(50);      // Eliminar
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;    // IdEstStatus
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;    // IdEstStatus
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

            // LblMessage_1.Text = "¿Desea eliminar el estatus?";
            LblMessage_1.Text = GetGlobalResourceObject("GlobalResources", "msg_Confirmar_Delete_Estatus").ToString();
            mpeMensaje_1.Show();
        }

        protected void Eliminar_ITM_52()
        {
            try
            {
                int index = Variables.wRenglon;

                int IdEstStatus = Convert.ToInt32(GrdStSiniestro.Rows[index].Cells[0].Text); ;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_52)
                string strQuery = "DELETE FROM ITM_52 " +
                                  " WHERE IdEstStatus = " + IdEstStatus + "";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                //LblMessage.Text = "Se elimino estatus, correctamente";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Estatus_Eliminado").ToString();
                mpeMensaje.Show();

                GetStSiniestro();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Actualizar_ITM_52()
        {
            try
            {
                int index = Variables.wRenglon;

                int IdEstStatus = Convert.ToInt32(GrdStSiniestro.Rows[index].Cells[0].Text); ;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Actualizar registro(s) tablas (ITM_52)
                string strQuery = "UPDATE ITM_52 " +
                                  "   SET Descripcion = '" + TxtNomEstatus.Text.Trim() + "' " +
                                  " WHERE IdEstStatus = " + IdEstStatus + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                //LblMessage.Text = "Se actualizo estatus, correctamente";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Estatus_Actualizado").ToString();
                mpeMensaje.Show();

                GetStSiniestro();
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
                if (TxtNomEstatus.Text == "" || TxtNomEstatus.Text == null)
                {
                    // LblMessage.Text = "Capturar Nombre del Estatus";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Capturar_NomEstatus").ToString();
                    mpeMensaje.Show();
                    return;
                }

                int iConsecutivo = GetIdConsecutivoMax();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_52)
                string strQuery = "INSERT INTO ITM_52 (IdEstStatus, Cve_EstStatus, Descripcion, IdStatus) " +
                                  "VALUES (" + iConsecutivo + ", CONCAT('ESS-', LPAD(" + iConsecutivo + ", 4, '0')), '" + TxtNomEstatus.Text.Trim() + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                // LblMessage.Text = "Se agrego estatus, correctamente";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Estatus_Agregado").ToString();
                mpeMensaje.Show();

                // Inicializar Controles
                TxtNomEstatus.Text = string.Empty;

                GetStSiniestro();

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

            string strQuery = "SELECT COALESCE(MAX(IdEstStatus), 0) + 1 IdEstStatus " +
                                " FROM ITM_52 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);


            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdEstStatus"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            Eliminar_ITM_52();

            TxtNomEstatus.Text = string.Empty;
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
            TxtNomEstatus.Text = string.Empty;
            TxtNomEstatus.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            TxtNomEstatus.ReadOnly = false;

            BtnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (TxtNomEstatus.Text == "" || TxtNomEstatus.Text == null)
            {
                // LblMessage.Text = "Capturar Nombre del Estatus";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Capturar_NomEstatus").ToString();
                mpeMensaje.Show();
                return;
            }

            Actualizar_ITM_52();

            // inicializar controles.
            TxtNomEstatus.Text = string.Empty;
            TxtNomEstatus.ReadOnly = false;

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

            TxtNomEstatus.Text = Server.HtmlDecode(Convert.ToString(GrdStSiniestro.Rows[index].Cells[2].Text));

            TxtNomEstatus.ReadOnly = true;

            BtnAnular.Visible = true;
            BtnEditar.Enabled = true;
            BtnAgregar.Enabled = false;
        }

    }
}