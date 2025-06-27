using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwGM_Catalog_Hospitales : System.Web.UI.Page
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


        public void GetEstaciones(int IdZona)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_17
                string strQuery = "SELECT IdHospital, IdZona, NomHospital " +
                                  "  FROM ITM_17 as b " +
                                  " WHERE IdZona = " + IdZona + " " +
                                  "   AND b.IdStatus = 1 ORDER BY IdHospital";

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

                string strQuery = "SELECT IdZona, NomZona " +
                                  "  FROM ITM_18 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlProtocolos.DataSource = dt;

                ddlProtocolos.DataValueField = "IdZona";
                ddlProtocolos.DataTextField = "NomZona";

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
                Eliminar_ITM_17();
            }
            else
            {
                // Eliminar protocolos (SLA)
                Eliminar_ITM_18();
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

            Actualizar_ITM_17();

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

        protected void Actualizar_ITM_17()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int index = Variables.wRenglon;

                int IdHospital = Convert.ToInt32(GrdTasks.Rows[index].Cells[1].Text);
                int IdZona = Convert.ToInt32(ddlProtocolos.SelectedValue);

                // Actualizar registro(s) tablas (ITM_17)
                string strQuery = "UPDATE ITM_17 " +
                                "   SET NomHospital = '" + TxtTarea.Text.Trim() + "' " +
                                " WHERE IdHospital = " + IdHospital + " " +
                                "   AND IdZona = " + IdZona + " ";


                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizo hospital, correctamente";
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


        protected void Eliminar_ITM_17()
        {
            try
            {
                int index = Variables.wRenglon;

                int IdHospital = Convert.ToInt32(GrdTasks.Rows[index].Cells[1].Text); ;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_17)
                string strQuery = "DELETE FROM ITM_17 " +
                                  " WHERE IdHospital = " + IdHospital + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino hospital, correctamente";
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

        protected void Eliminar_ITM_18()
        {
            try
            {
                int index = Variables.wRenglon;

                int IdZona = Convert.ToInt32(GrdProtocolos.Rows[index].Cells[0].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_17)
                string strQuery = "DELETE FROM ITM_17 " +
                                  " WHERE IdZona = '" + IdZona + "'; ";

                strQuery += Environment.NewLine;

                // Eliminar registro(s) tablas (ITM_18)
                strQuery += "DELETE FROM ITM_18 " +
                                  " WHERE IdZona = " + IdZona + " ";


                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino zona, correctamente";
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
                    LblMessage.Text = "Capturar Nombre del Hospital";
                    mpeMensaje.Show();
                    return;
                }

                int iIdHospital = GetIdConsecutivoMax();


                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_17)
                string strQuery = "INSERT INTO ITM_17 (IdHospital, IdZona, NomHospital, IdStatus) " +
                                  "VALUES (" + iIdHospital + ", " + ddlProtocolos.SelectedValue + ", '" + TxtTarea.Text.Trim() + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();


                GetEstaciones(Convert.ToInt32(ddlProtocolos.SelectedValue));

                LblMessage.Text = "Se agrego hospital, correctamente";
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

            string strQuery = "SELECT COALESCE(MAX(IdHospital), 0) + 1 IdHospital " +
                                " FROM ITM_17 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);


            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdHospital"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        public int GetIdLineaMax()
        {

            int IdConsecutivoMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX(IdZona), 0) + 1 IdZona " +
                                " FROM ITM_18 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);


            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdZona"].ToString().Trim());
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

            LblMessage_1.Text = "¿Desea eliminar hospital?";
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
            Insert_ITM_18();

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

        public void Insert_ITM_18()
        {
            try
            {

                if (TxtSLA_Protocolo.Text == "" || TxtSLA_Protocolo.Text == null)
                {
                    LblMessage.Text = "Capturar Nombre Zona";
                    mpeMensaje.Show();
                    return;
                }

                int iIdZona = GetIdLineaMax();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_18)
                string strQuery = "INSERT INTO ITM_18 (IdZona, NomZona, IdStatus) " +
                                  "VALUES (" + iIdZona + ", '" + TxtSLA_Protocolo.Text.Trim() + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                GetLineas();

                LblMessage.Text = "Se agrego zona, correctamente";
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

                // Consulta a las tablas : Protocolos (SLA) = ITM_18
                string strQuery = "SELECT IdZona, NomZona " +
                                  "  FROM ITM_18 " +
                                  " WHERE IdStatus = 1 ORDER BY IdZona";

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

            LblMessage_1.Text = "¿Desea eliminar la zona?";
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

    }
}