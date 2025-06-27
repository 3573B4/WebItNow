using System;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwGM_Configuracion_StSiniestro : System.Web.UI.Page
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

                    GetCiaSeguros();
                    GetProyecto();
                    GetCarpetas();
                    GetDocumentos(string.Empty, 0, 0);

                    // Inicializar controles
                    ddlEstSiniestro.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                    BtnEtapas.Enabled = false;

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

                ddlCliente.DataSource = dt;

                ddlCliente.DataValueField = "IdSeguros";
                ddlCliente.DataTextField = "Descripcion";

                ddlCliente.DataBind();
                ddlCliente.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetProyecto()
        {
            try
            {
                string IdCliente = ddlCliente.SelectedValue;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdProyecto, Descripcion " +
                                  "  FROM ITM_26 " +
                                  " WHERE IdCliente = '" + IdCliente + "'" +
                                  "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlProyecto.DataSource = dt;

                ddlProyecto.DataValueField = "IdProyecto";
                ddlProyecto.DataTextField = "Descripcion";

                ddlProyecto.DataBind();
                ddlProyecto.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetCarpetas()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT Id_Carpeta, Nom_Carpeta " +
                                        " FROM ITM_80 " +
                                        " WHERE Id_Carpeta = 4 AND IdStatus = 1";

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

        protected void GetTipoAsunto()
        {
            try
            {
                int IdProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT a.IdTpoEvento, a.Descripcion " +
                                        " FROM ITM_11 as a, ITM_26 as b " +
                                        " WHERE a.IdTpoEvento = b.IdTpoAsunto " +
                                        "  AND IdProyecto = " + IdProyecto + " " +
                                        "  AND b.IdCliente = '" + ddlCliente.SelectedValue + "' " +
                                        "  AND a.IdStatus IN ( 0, 1 ) ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count != 0)
                {
                    Variables.wIdTpoAsunto = Convert.ToInt16(dt.Rows[0].ItemArray[0].ToString());
                    TxtTpoAsunto.Text = dt.Rows[0].ItemArray[1].ToString();
                }

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }
        protected void GetStSiniestro()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();


                string strQuery = "SELECT IdEstStatus, Descripcion " +
                                  "  FROM ITM_52 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlEstSiniestro.DataSource = dt;

                ddlEstSiniestro.DataValueField = "IdEstStatus";
                ddlEstSiniestro.DataTextField = "Descripcion";

                ddlEstSiniestro.DataBind();
                ddlEstSiniestro.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdTareas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdTareas.PageIndex = e.NewPageIndex;
                string IdCliente = ddlCliente.SelectedValue;
                int IdProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);
                int IdEstatus = Convert.ToInt32(ddlEstSiniestro.SelectedValue);

                GetDocumentos(IdCliente, IdProyecto, IdEstatus);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdTareas_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdTareas_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdTareas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdTareas, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = Unit.Pixel(1200);    // Descripcion
                e.Row.Cells[1].Width = Unit.Pixel(100);     // IdEstStatus
                e.Row.Cells[4].Width = Unit.Pixel(50);      // Eliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;     // IdEtapa_Estatus
                e.Row.Cells[2].Visible = false;     // IdProyecto
                e.Row.Cells[3].Visible = false;     // IdEstatus
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;     // IdEtapa_Estatus
                e.Row.Cells[2].Visible = false;     // IdProyecto
                e.Row.Cells[3].Visible = false;     // IdEstatus
            }
        }

        protected void GrdTareas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnEtapas_Click(object sender, EventArgs e)
        {
            ddlCarpetas.SelectedIndex = 0;

            GetBusqProceso(0);
            mpeNewProceso.Show();
        }

        protected void GetBusqProceso(int IdSeccion)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                Variables.wPrefijo_Aseguradora = ddlCliente.SelectedValue;
                Variables.wIdProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);

                string strQuery = "SELECT C.IdDocumento, C.Descripcion " +
                                  "  FROM ITM_25 AS A, ITM_87 AS B, ITM_23 AS C " +
                                  " WHERE A.IdSeccion = B.IdCategoria " +
                                  "   AND A.IdDocumento = C.IdDocumento " +
                                  "   AND IdProyecto = " + Variables.wIdProyecto + " AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND IdSeccion = " + IdSeccion + " " +
                                  "   AND bSeleccion = 1 AND A.IdStatus = 1 " +
                                  " ORDER BY C.IdDocumento ";


                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdEtapas.ShowHeaderWhenEmpty = true;
                    GrdEtapas.EmptyDataText = "No hay resultados.";
                }

                GrdEtapas.DataSource = dt;
                GrdEtapas.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdEtapas.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
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

            LblMessage_1.Text = "¿Desea eliminar la etapa?";
            mpeMensaje_1.Show();
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            Eliminar_ITM_53();
        }

        protected void Add_ITM_53()
        {
            try
            {
                string IdCliente = string.Empty;
                int IdProyecto = 0;
                int IdEstatus = 0;
                int IdTpoAsunto = 0;

                foreach (GridViewRow row in GrdEtapas.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        var chkbox = row.FindControl("ChBoxRow") as CheckBox;

                        if (chkbox.Checked)
                        {
                            string IdEtapa = Server.HtmlDecode(Convert.ToString(row.Cells[1].Text));
                            string NomEtapa = Server.HtmlDecode(Convert.ToString(row.Cells[2].Text));

                            IdCliente = ddlCliente.SelectedValue;
                            IdProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);
                            IdTpoAsunto = Variables.wIdTpoAsunto;
                            IdEstatus = Convert.ToInt32(ddlEstSiniestro.SelectedValue);

                            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                            dbConn.Open();

                            IdEstatus = Convert.ToInt32(ddlEstSiniestro.SelectedValue);
                            string IdUsuario = Variables.wUserLogon;

                            string strQuery = @"INSERT INTO ITM_28 (IdCliente, IdProyecto, IdTpoAsunto, IdEstatus, IdEtapa, NomEtapa, IdUsuario, IdStatus) " +
                                               "SELECT '" + IdCliente + "', " + IdProyecto + ", " + IdTpoAsunto + ", " + IdEstatus + ", " + IdEtapa + ", '" + NomEtapa + "', '" + IdUsuario + "', 1 " +
                                               " WHERE NOT EXISTS ( " +
                                               "SELECT 1 FROM ITM_28 " +
                                               " WHERE IdCliente = '" + IdCliente + "' " +
                                               "   AND IdProyecto = " + IdProyecto + " " +
                                               "   AND IdTpoAsunto = " + IdTpoAsunto + " " +
                                               "   AND IdEstatus = " + IdEstatus + " " +
                                               "   AND IdEtapa = " + IdEtapa + " " +
                                               "   AND NomEtapa = '" + NomEtapa + "' " +
                                               "   AND IdUsuario = '" + IdUsuario + "' ); ";

                            int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                            dbConn.Close();
                        }
                    }
                }

                GetDocumentos(IdCliente, IdProyecto, IdEstatus);

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void Eliminar_ITM_53()
        {
            try
            {
                int index = Variables.wRenglon;

                string IdCliente = ddlCliente.SelectedValue;
                int IdEtapa_Estatus = Convert.ToInt32(GrdTareas.Rows[index].Cells[1].Text);
                int IdProyecto = Convert.ToInt32(GrdTareas.Rows[index].Cells[2].Text);
                int IdEstatus = Convert.ToInt32(GrdTareas.Rows[index].Cells[3].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_28)
                string strQuery = "DELETE FROM ITM_28 " +
                                  " WHERE IdEtapa_Estatus = " + IdEtapa_Estatus + " " +
                                  "   AND IdCliente = '" + IdCliente + "' " +
                                  "   AND IdProyecto = " + IdProyecto + " " +
                                  "   AND IdEstatus = " + IdEstatus + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino la etapa, correctamente";
                mpeMensaje.Show();

                GetDocumentos(IdCliente, IdProyecto, IdEstatus);
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

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Add_ITM_53();
        }

        protected void btnClose_Proceso_Click(object sender, EventArgs e)
        {

        }

        protected void GrdEtapas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdEtapas, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;     // IdSLA
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;     // IdSLA
            }
        }

        protected void GrdEtapas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdEtapas.PageIndex = e.NewPageIndex;
                GetBusqProceso(3);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetDocumentos(string IdCliente, int IdProyecto, int IdEstatus)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_28
                string strQuery = "SELECT a.IdEtapa_Estatus, a.IdCliente, a.IdProyecto, a.IdTpoAsunto, a.IdEstatus, a.IdEtapa, b.Descripcion " +
                                  "  FROM ITM_28 as a, ITM_23 as b " +
                                  " WHERE b.IdDocumento = a.IdEtapa " +
                                  "   AND a.IdCliente = '" + IdCliente + "' " +
                                  "   AND a.IdProyecto = " + IdProyecto + " " +
                                  "   AND a.IdEstatus = " + IdEstatus + " ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdTareas.ShowHeaderWhenEmpty = true;
                    GrdTareas.EmptyDataText = "No hay resultados.";
                }

                GrdTareas.DataSource = dt;
                GrdTareas.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdTareas.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ddlEstSiniestro_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnEtapas.Enabled = true;

            string IdCliente = ddlCliente.SelectedValue;
            int IdProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);
            int IdEstatus = Convert.ToInt32(ddlEstSiniestro.SelectedValue);

            GetDocumentos(IdCliente, IdProyecto, IdEstatus);
        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnEtapas.Enabled = false;

            // Inicializar controles
            GetDocumentos(string.Empty, 0, 0);

            TxtTpoAsunto.Text = string.Empty;

            ddlProyecto.ClearSelection();
            ddlEstSiniestro.Items.Clear();
            ddlCarpetas.SelectedIndex = 0;

            ddlEstSiniestro.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

            GetProyecto();
        }

        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTipoAsunto();

            // Inicializar controles
            GetDocumentos(string.Empty, 0, 0);

            TxtTpoAsunto.Text = string.Empty;

            ddlEstSiniestro.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

            BtnEtapas.Enabled = false;

            GetStSiniestro();

        }

        protected void ddlCarpetas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCarpetas.SelectedValue == "0")
            {
                GetBusqProceso(0);
            }
            else
            {
                GetBusqProceso(3);
            }
        }

    }
}