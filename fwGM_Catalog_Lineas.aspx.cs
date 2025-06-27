using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwGM_Catalog_Lineas : System.Web.UI.Page
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

                    GetLineas();
                    GetEstaciones(0);

                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

            }
        }


        public void GetEstaciones(int IdLinea)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_15
                string strQuery = "SELECT IdEstacion, IdLinea, NomEstacion " +
                                  "  FROM ITM_15 as b " +
                                  " WHERE IdLinea = " + IdLinea + " " +
                                  "   AND b.IdStatus = 1 ORDER BY IdEstacion";

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


        protected void GetLineas()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdLinea, NomLinea " +
                                  "  FROM ITM_16 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlProtocolos.DataSource = dt;

                ddlProtocolos.DataValueField = "IdLinea";
                ddlProtocolos.DataTextField = "NomLinea";

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
                Eliminar_ITM_15();
            }
            else
            {
                // Eliminar protocolos (SLA)
                Eliminar_ITM_16();
            }

            TxtTarea.Text = string.Empty;

            Variables.wEliminar = 0;

            MostrarPanel(false);
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }


        protected void BtnAnular_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            ddlProtocolos.Enabled = true;

            TxtTarea.Text = string.Empty;
            TxtTarea.ReadOnly = false;

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
                LblMessage.Text = "Seleccionar Línea de Ocurrencia";
                mpeMensaje.Show();
                return;
            }
            if (TxtTarea.Text == "" || TxtTarea.Text == null)
            {
                LblMessage.Text = "Capturar Descripción Estación";
                mpeMensaje.Show();
                return;
            }

            Actualizar_ITM_15();

            // inicializar controles.
            //ddlCliente.Enabled = true;
            ddlProtocolos.Enabled = true;
            // ddlCliente.SelectedIndex = 0;

            TxtTarea.Text = string.Empty;
            TxtTarea.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void Actualizar_ITM_15()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int index = Variables.wRenglon;

                int IdEstacion = Convert.ToInt32(GrdTasks.Rows[index].Cells[1].Text);
                int IdLinea = Convert.ToInt32(ddlProtocolos.SelectedValue);

                // Actualizar registro(s) tablas (ITM_15)
                string strQuery = "UPDATE ITM_15 " +
                                "   SET NomEstacion = '" + TxtTarea.Text.Trim() + "' " +
                                " WHERE IdEstacion = " + IdEstacion + " " +
                                "   AND IdLinea = " + IdLinea + " ";


                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizo estación, correctamente";
                mpeMensaje.Show();

                GetEstaciones(Convert.ToInt32(ddlProtocolos.SelectedValue));
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }


        protected void Eliminar_ITM_15()
        {
            try
            {
                int index = Variables.wRenglon;

                int IdEstacion = Convert.ToInt32(GrdTasks.Rows[index].Cells[1].Text); ;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_15)
                string strQuery = "DELETE FROM ITM_15 " +
                                  " WHERE IdEstacion = " + IdEstacion + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino estación, correctamente";
                mpeMensaje.Show();

                GetEstaciones(Convert.ToInt32(ddlProtocolos.SelectedValue));
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Eliminar_ITM_16()
        {
            try
            {
                int index = Variables.wRenglon;

                int IdLinea = Convert.ToInt32(GrdProtocolos.Rows[index].Cells[0].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_15)
                string strQuery = "DELETE FROM ITM_15 " +
                                  " WHERE IdLinea = '" + IdLinea + "'; ";

                strQuery += Environment.NewLine;

                // Eliminar registro(s) tablas (ITM_16)
                strQuery += "DELETE FROM ITM_16 " +
                                  " WHERE IdLinea = " + IdLinea + " ";


                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino estación, correctamente";
                mpeMensaje.Show();

                GetEstaciones(Convert.ToInt32(ddlProtocolos.SelectedValue));
                GetLineas();

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
                    LblMessage.Text = "Seleccionar Linea de Ocurrencia";
                    mpeMensaje.Show();
                    return;
                }
                if (TxtTarea.Text == "" || TxtTarea.Text == null)
                {
                    LblMessage.Text = "Capturar Nombre de la Estación";
                    mpeMensaje.Show();
                    return;
                }

                int iIdEstacion = GetIdConsecutivoMax();


                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_15)
                string strQuery = "INSERT INTO ITM_15 (IdEstacion, IdLinea, NomEstacion, IdStatus) " +
                                  "VALUES (" + iIdEstacion + ", " + ddlProtocolos.SelectedValue + ", '" + TxtTarea.Text.Trim() + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();


                GetEstaciones(Convert.ToInt32(ddlProtocolos.SelectedValue));

                LblMessage.Text = "Se agrego estación, correctamente";
                mpeMensaje.Show();

                // Inicializar Controles
                TxtTarea.Text = string.Empty;

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

            string strQuery = "SELECT COALESCE(MAX(IdEstacion), 0) + 1 IdEstacion " +
                                " FROM ITM_15 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);


            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdEstacion"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        public int GetIdLineaMax()
        {

            int IdConsecutivoMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX(IdLinea), 0) + 1 IdLinea " +
                                " FROM ITM_16 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);


            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdLinea"].ToString().Trim());
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

            ddlProtocolos.Enabled = false;
            TxtTarea.ReadOnly = true;

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

            LblMessage_1.Text = "¿Desea eliminar la estación?";
            mpeMensaje_1.Show();
        }

        protected void ddlProtocolos_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetEstaciones(Convert.ToInt32(ddlProtocolos.SelectedValue));
        }

        protected void BtnCancelProtocolo_Click(object sender, EventArgs e)
        {
            MostrarPanel(false);
        }

        protected void BtnAddProtocolo_Click(object sender, EventArgs e)
        {
            Insert_ITM_16();

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
                GetLineas();
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
                e.Row.Cells[0].Visible = false;    // IdLinea
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;    // IdLinea
            }
        }

        public void Insert_ITM_16()
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
                    LblMessage.Text = "Capturar Nombre Línea de Ocurrencia";
                    mpeMensaje.Show();
                    return;
                }

                int iIdLinea = GetIdLineaMax();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_16)
                string strQuery = "INSERT INTO ITM_16 (IdLinea, NomLinea, IdStatus) " +
                                  "VALUES (" + iIdLinea + ", '" + TxtSLA_Protocolo.Text.Trim() + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                GetLineas();

                LblMessage.Text = "Se agrego Línea de ocurrencia, correctamente";
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

                // Consulta a las tablas : Protocolos (SLA) = ITM_16
                string strQuery = "SELECT IdLinea, NomLinea " +
                                  "  FROM ITM_16 " +
                                  " WHERE IdStatus = 1 ORDER BY IdLinea";

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

            LblMessage_1.Text = "¿Desea eliminar Línea de Ocurrencia?";
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
                GetEstaciones(Convert.ToInt32(ddlProtocolos.SelectedValue));
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
                e.Row.Cells[0].Width = Unit.Pixel(1100);    // NomTarea
                e.Row.Cells[2].Width = Unit.Pixel(25);      // Editar
                e.Row.Cells[3].Width = Unit.Pixel(25);      // Eliminar

            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;    // IdEstacion
                }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;    // IdEstacion
                }
        }

        protected void ddlUnidadTiempo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}