using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwCatalog_SLA_Protocolos : System.Web.UI.Page
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

                    MostrarPanel(false); // Mostrar el panel principal por defecto

                    //GetCiaSeguros();
                    GetProtocolos();
                    GetTareas(0);
                    GetUnidadTiempo();

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

                //ddlCliente.DataSource = dt;

                //ddlCliente.DataValueField = "IdSeguros";
                //ddlCliente.DataTextField = "Descripcion";

                //ddlCliente.DataBind();
                //ddlCliente.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }


        public void GetTareas(int IdSLA)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_54
                string strQuery = "SELECT IdTarea, IdSLA, NomTarea, a.Descripcion as Descripcion, Plazo, DocInterno, " +
                                  //"  CASE WHEN UnidadTiempo = 1 THEN 'Minutos' ELSE 'Horas' END as UnidadTiempo,  " +
                                  "  CASE WHEN DocInterno = 1 THEN 'INTERNO' ELSE '' END as TpoInterno, b.UnidadTiempo " +
                                  "  FROM ITM_50 as a, ITM_54 as b " +
                                  " WHERE IdSLA = " + IdSLA + " " +
                                  "   AND a.IdUnidadTiempo = b.UnidadTiempo" +
                                  "   AND b.IdStatus = 1 ORDER BY IdTarea";

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


        protected void GetProtocolos()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdSLA, NomProtocolo " +
                                  "  FROM ITM_55 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlProtocolos.DataSource = dt;

                ddlProtocolos.DataValueField = "IdSLA";
                ddlProtocolos.DataTextField = "NomProtocolo";

                ddlProtocolos.DataBind();
                ddlProtocolos.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetUnidadTiempo()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdUnidadTiempo, Descripcion " +
                                  "  FROM ITM_50 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlUnidadTiempo.DataSource = dt;

                ddlUnidadTiempo.DataValueField = "IdUnidadTiempo";
                ddlUnidadTiempo.DataTextField = "Descripcion";

                ddlUnidadTiempo.DataBind();
                ddlUnidadTiempo.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }


        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            if (Variables.wEliminar == 0)
            {
                // Eliminar Tareas
                Eliminar_ITM_54();
            } 
            else
            {
                // Eliminar protocolos (SLA)
                Eliminar_ITM_55();
            }

            TxtTarea.Text = string.Empty;
            TxtPlazo.Text = string.Empty;
            chkTaskInterno.Checked = false;

            Variables.wEliminar = 0;

            MostrarPanel(false);
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void GrdProcesos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GrdProcesos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdProcesos_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdProcesos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdProcesos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void BtnAnular_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            ddlProtocolos.Enabled = true;

            TxtTarea.Text = string.Empty;
            TxtPlazo.Text = string.Empty;
            ddlUnidadTiempo.SelectedIndex = 0;
            chkTaskInterno.Checked = false;

            TxtTarea.ReadOnly = false;
            TxtPlazo.ReadOnly = false;
            ddlUnidadTiempo.Enabled = true;
            chkTaskInterno.Enabled = true;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            // ddlCliente.Enabled = false;
            ddlProtocolos.Enabled = false;

            TxtTarea.ReadOnly = false;
            TxtPlazo.ReadOnly = false;
            ddlUnidadTiempo.Enabled = true;
            chkTaskInterno.Enabled = true;

            BtnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            //if (ddlCliente.SelectedValue == "0")
            //{
            //    LblMessage.Text = "Seleccionar Compañia de Seguros";
            //    mpeMensaje.Show();
            //    return;
            //}
            if (ddlProtocolos.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Protocolo (SLA)";
                mpeMensaje.Show();
                return;
            }
            if (TxtTarea.Text == "" || TxtTarea.Text == null)
            {
                LblMessage.Text = "Capturar Descripción de la Tarea";
                mpeMensaje.Show();
                return;
            }
            if (TxtPlazo.Text == "" || TxtPlazo.Text == null || TxtPlazo.Text.Trim() == "")
            {
                LblMessage.Text = "Capturar Descripción del Plazo";
                mpeMensaje.Show();
                return;
            }
            if (ddlUnidadTiempo.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Unidad de Tiempo";
                mpeMensaje.Show();
                return;
            }

            Actualizar_ITM_54();

            // inicializar controles.
            //ddlCliente.Enabled = true;
            ddlProtocolos.Enabled = true;
            // ddlCliente.SelectedIndex = 0;

            TxtTarea.Text = string.Empty;
            TxtPlazo.Text = string.Empty;
            ddlUnidadTiempo.SelectedIndex = 0;
            chkTaskInterno.Checked = false;

            TxtTarea.ReadOnly = false;
            TxtPlazo.ReadOnly = false;
            ddlUnidadTiempo.Enabled = true;
            chkTaskInterno.Enabled = true;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void Actualizar_ITM_54()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int index = Variables.wRenglon;

                int IdTarea = Convert.ToInt32(GrdTasks.Rows[index].Cells[1].Text);
                int IdSLA = Convert.ToInt32(ddlProtocolos.SelectedValue);

                int iDocInterno = 0;

                if (chkTaskInterno.Checked)
                {
                    iDocInterno = 1;
                }

                // Actualizar registro(s) tablas (ITM_54)
                string strQuery = "UPDATE ITM_54 " +
                                  "   SET NomTarea = '" + TxtTarea.Text.Trim() + "', " +
                                  "       Plazo = '" + TxtPlazo.Text.Trim() + "', " +
                                  "       UnidadTiempo = " + ddlUnidadTiempo.SelectedValue + ", " +
                                  "       DocInterno = " + iDocInterno + " " +
                                  " WHERE IdTarea = " + IdTarea + " " +
                                  "   AND IdSLA = " + IdSLA + " ";


                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizo tarea, correctamente";
                mpeMensaje.Show();

                GetTareas(Convert.ToInt32(ddlProtocolos.SelectedValue));
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
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

                GetTareas(Convert.ToInt32(ddlProtocolos.SelectedValue));
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Eliminar_ITM_55()
        {
            try
            {
                int index = Variables.wRenglon;

                int IdSLA = Convert.ToInt32(GrdProtocolos.Rows[index].Cells[0].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_54)
                string strQuery = "DELETE FROM ITM_54 " +
                                  " WHERE IdSLA = '" + IdSLA + "'; ";

                strQuery += Environment.NewLine;

                // Eliminar registro(s) tablas (ITM_55)
                strQuery += "DELETE FROM ITM_55 " +
                                  " WHERE IdSLA = " + IdSLA + " ";


                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino protocolo, correctamente";
                mpeMensaje.Show();

                GetTareas(Convert.ToInt32(ddlProtocolos.SelectedValue));
                GetProtocolos();

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
                if (ddlProtocolos.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar Protocolo (SLA)";
                    mpeMensaje.Show();
                    return;
                }
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
                else if (ddlUnidadTiempo.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar Unidad de Tiempo";
                    mpeMensaje.Show();
                    return;
                }

                int iIdTarea = GetIdConsecutivoMax();

                int iDocInterno = 0;

                if (chkTaskInterno.Checked)
                {
                    iDocInterno = 1;
                }

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_54)
                string strQuery = "INSERT INTO ITM_54 (IdTarea, IdSLA, NomTarea, Plazo, UnidadTiempo, DocInterno, IdStatus) " +
                                  "VALUES (" + iIdTarea + ", " + ddlProtocolos.SelectedValue + ", '" + TxtTarea.Text.Trim() + "', " +
                                  "'" + TxtPlazo.Text.Trim() + "', " + ddlUnidadTiempo.SelectedValue + ", " + iDocInterno + ", 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();


                GetTareas(Convert.ToInt32(ddlProtocolos.SelectedValue));

                LblMessage.Text = "Se agrego tarea, correctamente";
                mpeMensaje.Show();

                // Inicializar Controles
                TxtTarea.Text = string.Empty;
                TxtPlazo.Text = string.Empty;
                ddlUnidadTiempo.SelectedIndex = 0;
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


            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdTarea"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        public int GetIdSLAMax()
        {

            int IdConsecutivoMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX(IdSLA), 0) + 1 IdSLA " +
                                " FROM ITM_55 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);


            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdSLA"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            TxtTarea.Text = Server.HtmlDecode(Convert.ToString(GrdTasks.Rows[index].Cells[0].Text));
            TxtPlazo.Text = Server.HtmlDecode(Convert.ToString(GrdTasks.Rows[index].Cells[3].Text));
            ddlUnidadTiempo.SelectedValue = Convert.ToString(GrdTasks.Rows[index].Cells[6].Text);
            chkTaskInterno.Checked = GrdTasks.Rows[index].Cells[2].Text == "1";

            ddlProtocolos.Enabled = false;
            TxtTarea.ReadOnly = true;
            TxtPlazo.ReadOnly = true;
            ddlUnidadTiempo.Enabled = false;
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

        protected void ddlProtocolos_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTareas(Convert.ToInt32(ddlProtocolos.SelectedValue));
        }

        protected void BtnCancelProtocolo_Click(object sender, EventArgs e)
        {
            MostrarPanel(false);
        }

        protected void BtnAddProtocolo_Click(object sender, EventArgs e)
        {
            Insert_ITM_55();

            MostrarPanel(false);
        }

        protected void btnMostrarPanel_Click(object sender, EventArgs e)
        {
            GetProSLA();
            MostrarPanel(true);
        }

        private void MostrarPanel(bool mostrarAgregarProtocolo)
        {
            PanelAgregarProtocolo.Visible = mostrarAgregarProtocolo;
            MainPanel.Visible = !mostrarAgregarProtocolo;
        }

        protected void GrdProtocolos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdProtocolos.PageIndex = e.NewPageIndex;
                GetProtocolos();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdProtocolos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdProtocolos_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdProtocolos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdProtocolos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdProtocolos, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Width = Unit.Pixel(1100);     // Nom Protocolo
                e.Row.Cells[2].Width = Unit.Pixel(50);      // Eliminar
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;    // IdSLA
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;    // IdSLA
            }
        }

        public void Insert_ITM_55()
        {
            try
            {

                //if (ddlCliente.SelectedValue == "0")
                //{
                //    LblMessage.Text = "Seleccionar Compañia de Seguros";
                //    mpeMensaje.Show();
                //    return;
                //}
                if (TxtSLA_Protocolo.Text == "" || TxtSLA_Protocolo.Text == null)
                {
                    LblMessage.Text = "Capturar Nombre del Protocolo (SLA)";
                    mpeMensaje.Show();
                    return;
                }

                int iIdSLA = GetIdSLAMax();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_55)
                string strQuery = "INSERT INTO ITM_55 (IdSLA, NomProtocolo, IdStatus) " +
                                  "VALUES (" + iIdSLA + ", '" + TxtSLA_Protocolo.Text.Trim() + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                GetProtocolos();

                LblMessage.Text = "Se agrego Protocolo (SLA), correctamente";
                mpeMensaje.Show();

                // Inicializar Controles
                TxtSLA_Protocolo.Text = string.Empty;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetProSLA()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string IdSeguros = string.Empty;

                // Consulta a las tablas : Protocolos (SLA) = ITM_55
                string strQuery = "SELECT IdSLA, NomProtocolo " +
                                  "  FROM ITM_55 " +
                                  " WHERE IdStatus = 1 ORDER BY IdSLA";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdProtocolos.ShowHeaderWhenEmpty = true;
                    GrdProtocolos.EmptyDataText = "No hay resultados.";
                }

                GrdProtocolos.DataSource = dt;
                GrdProtocolos.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdProtocolos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ImgEliminarSLA_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;
            Variables.wEliminar = 1;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            LblMessage_1.Text = "¿Desea eliminar el protocolo?";
            mpeMensaje_1.Show();
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
                GetTareas(Convert.ToInt32(ddlProtocolos.SelectedValue));
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdTasks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdTasks, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = Unit.Pixel(1000);    // NomTarea
                e.Row.Cells[3].Width = Unit.Pixel(400);     // Plazo
                e.Row.Cells[4].Width = Unit.Pixel(400);     // Descripcion
                e.Row.Cells[5].Width = Unit.Pixel(100);     // TpoInterno
                e.Row.Cells[7].Width = Unit.Pixel(50);      // Editar
                e.Row.Cells[8].Width = Unit.Pixel(50);      // Eliminar

            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;    // IdTarea
                e.Row.Cells[2].Visible = false;    // DocInterno
                e.Row.Cells[6].Visible = false;    // Unidad de Tiempo
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;    // IdTarea
                e.Row.Cells[2].Visible = false;    // DocInterno
                e.Row.Cells[6].Visible = false;    // Unidad de Tiempo
            }
        }

        protected void ddlUnidadTiempo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}