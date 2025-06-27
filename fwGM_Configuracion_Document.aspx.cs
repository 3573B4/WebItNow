using System;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwGM_Configuracion_Document : System.Web.UI.Page
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
                    GetDocumentos(0, 0, 0);

                    // Inicializar controles
                    ddlSecciones.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                    ddlCategorias.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                    BtnDocumentos.Enabled = false;

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

        protected void GetTipoAsunto()
        {
            try
            {
                int IdProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT a.IdTpoAsunto, a.Descripcion " +
                                        " FROM ITM_66 as a, ITM_26 as b " +
                                        " WHERE a.IdTpoAsunto = b.IdTpoAsunto " +
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

        protected void GetSecciones()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdCategoria, Descripcion " +
                                  "  FROM ITM_87 " +
                                  " WHERE IdStatus = 1 AND IdCategoria NOT IN (4, 6) " +
                                  " ORDER BY IdOrden ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlSecciones.DataSource = dt;

                ddlSecciones.DataValueField = "IdCategoria";
                ddlSecciones.DataTextField = "Descripcion";

                ddlSecciones.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ddlSecciones.Items.Insert(0, new ListItem("-- No Hay Carpeta(s) --", "0"));
                }
                else
                {
                    ddlSecciones.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                }

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetCategorias(int IdSeccion, string IdCliente, string Tabla)
        {
            try
            {
                int IdTpoAsunto = Variables.wIdTpoAsunto;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = $"SELECT { Tabla }.IdDocumento, { Tabla }.Descripcion " +
                                  $"  FROM { Tabla } , ITM_25 " +
                                  $" WHERE { Tabla }.IdDocumento = ITM_25.IdDocumento" +
                                  "    AND ITM_25.IdProyecto = " + ddlProyecto.SelectedItem.Value + " " +
                                  "    AND ITM_25.IdSeccion = " + IdSeccion + " " +
                                  "    AND ITM_25.IdCliente = '" + IdCliente + "' " +
                                  "    AND ITM_25.bSeleccion = 1 ";

                if (ddlProyecto.SelectedItem.Value != "0")
                {
                    strQuery += "    AND ITM_25.IdTpoAsunto = '" + IdTpoAsunto + "' ";
                }

                strQuery += $"    ORDER BY { Tabla }.IdDocumento ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlCategorias.DataSource = dt;

                ddlCategorias.DataValueField = "IdDocumento";
                ddlCategorias.DataTextField = "Descripcion";

                ddlCategorias.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ddlCategorias.Items.Insert(0, new ListItem("-- No Hay Categoría(s) --", "0"));
                }
                else
                {
                    ddlCategorias.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                }

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetDocumentos(int IdProyecto, int IdCategoria, int IdSeccion)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_27
                string strQuery = "SELECT a.IdDoc_Categoria, a.IdCliente, a.IdProyecto, a.IdTpoAsunto, a.IdSeccion, a.IdCategoria, a.IdDocumento, b.Descripcion " +
                                  "  FROM ITM_27 as a, ITM_20 as b " +
                                  " WHERE b.IdConsecutivo = a.IdDocumento " +
                                  "   AND a.IdProyecto = " + IdProyecto + " " +
                                  "   AND a.IdSeccion = " + IdSeccion + " " +
                                  "   AND a.IdCategoria = " + IdCategoria + " ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdDocumentos.ShowHeaderWhenEmpty = true;
                    GrdDocumentos.EmptyDataText = "No hay resultados.";
                }

                GrdDocumentos.DataSource = dt;
                GrdDocumentos.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdDocumentos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetDocumentos_Carpeta()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_20
                string strQuery = "SELECT d.IdConsecutivo, d.IdCarpeta, d.Descripcion " +
                                  "  FROM ITM_80 as p, ITM_20 as d " +
                                  " WHERE p.Id_Carpeta = d.IdCarpeta " +
                                  "   AND d.IdCarpeta = " + ddlCarpetas.SelectedItem.Value + "";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdDocCarpeta.ShowHeaderWhenEmpty = true;
                    GrdDocCarpeta.EmptyDataText = "No hay resultados.";
                }

                GrdDocCarpeta.DataSource = dt;
                GrdDocCarpeta.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdDocCarpeta.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetBusqProceso()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_20
                string strQuery = "SELECT d.IdConsecutivo, d.IdCarpeta, d.Descripcion " +
                                  "  FROM ITM_80 as p, ITM_20 as d " +
                                  " WHERE p.Id_Carpeta = d.IdCarpeta " +
                                  "   AND d.IdCarpeta = " + ddlCarpetas.SelectedItem.Value + "";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdDocCarpeta.ShowHeaderWhenEmpty = true;
                    GrdDocCarpeta.EmptyDataText = "No hay resultados.";
                }

                GrdDocCarpeta.DataSource = dt;
                GrdDocCarpeta.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdDocCarpeta.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnDocumentos.Enabled = false;

            // Inicializar controles
            GetDocumentos(0, 0, 0);

            TxtTpoAsunto.Text = string.Empty;

            ddlProyecto.ClearSelection();
            ddlSecciones.Items.Clear();
            ddlCategorias.Items.Clear();

            ddlSecciones.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
            ddlCategorias.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

            GetProyecto();
        }

        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTipoAsunto();

            // Inicializar controles
            GetDocumentos(0, 0, 0);

            TxtTpoAsunto.Text = string.Empty;

            ddlSecciones.Items.Clear();
            ddlCategorias.Items.Clear();

            ddlSecciones.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
            ddlCategorias.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

            BtnDocumentos.Enabled = false;

            GetSecciones();
        }

        protected void ddlSecciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            // inicializar controles
            string IdCliente = ddlCliente.SelectedValue;

            ddlCategorias.Items.Clear();
            BtnDocumentos.Enabled = false;

            GetDocumentos(0, 0, 0);

            if (ddlSecciones.SelectedValue == "1")
            {
                GetCategorias(1, IdCliente, "ITM_21");

            }
            else if (ddlSecciones.SelectedValue == "2")
            {
                GetCategorias(2, IdCliente, "ITM_22");

            }
            else if (ddlSecciones.SelectedValue == "3")
            {
                GetCategorias(3, IdCliente, "ITM_23");

            }
            else if (ddlSecciones.SelectedValue == "4")
            {
                // GetCategorias(4, IdCliente, "ITM_84");

            }
            else if (ddlSecciones.SelectedValue == "5")
            {
                GetCategorias(5, IdCliente, "ITM_24");

            }
            else if (ddlSecciones.SelectedValue == "6")
            {
                // GetCategorias(6, IdCliente, "ITM_92");
            }

        }

        protected void ddlCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnDocumentos.Enabled = true;
            int IdProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);
            int IdCategoria = Convert.ToInt32(ddlCategorias.SelectedValue);
            int IdSeccion = Convert.ToInt32(ddlSecciones.SelectedValue);

            GetDocumentos(IdProyecto, IdCategoria, IdSeccion);
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }
        protected void BtnDocumentos_Click(object sender, EventArgs e)
        {
            ddlCarpetas.SelectedIndex = 0;

            GetBusqProceso();
            mpeNewProceso.Show();
        }

        protected void GrdDocumentos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdDocumentos.PageIndex = e.NewPageIndex;
                int IdProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);
                int IdCategoria = Convert.ToInt32(ddlCategorias.SelectedValue);
                int IdSeccion = Convert.ToInt32(ddlSecciones.SelectedValue);

                GetDocumentos(IdProyecto, IdCategoria, IdSeccion);
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
                e.Row.Cells[0].Width = Unit.Pixel(1200);    // Descripcion
                e.Row.Cells[1].Width = Unit.Pixel(100);     // IdDoc_Categoria
                e.Row.Cells[5].Width = Unit.Pixel(50);      // Eliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;     // IdDoc_Categoria
                e.Row.Cells[2].Visible = false;     // IdProyecto
                e.Row.Cells[3].Visible = false;     // IdSeccion
                e.Row.Cells[4].Visible = false;     // IdCategoria
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;     // IdDoc_Categoria
                e.Row.Cells[2].Visible = false;     // IdProyecto
                e.Row.Cells[3].Visible = false;     // IdSeccion
                e.Row.Cells[4].Visible = false;     // IdCategoria
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

            LblMessage_1.Text = "¿Desea eliminar el documento?";
            mpeMensaje_1.Show();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Add_ITM_88();
        }

        protected void btnClose_Proceso_Click(object sender, EventArgs e)
        {

        }

        protected void GrdDocCarpeta_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdDocCarpeta.PageIndex = e.NewPageIndex;
                GetBusqProceso();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ddlCarpetas_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetBusqProceso();
        }

        protected void Add_ITM_88()
        {
            try
            {
                int IdProyecto = 0;
                int IdCategoria = 0;
                int IdSeccion = 0;

                foreach (GridViewRow row in GrdDocCarpeta.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        var chkbox = row.FindControl("ChBoxRow") as CheckBox;

                        if (chkbox.Checked)
                        {
                            string IdDocumento = Server.HtmlDecode(Convert.ToString(row.Cells[1].Text));
                            string Descripcion = Server.HtmlDecode(Convert.ToString(row.Cells[2].Text));

                            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                            dbConn.Open();

                            string IdCliente = ddlCliente.SelectedValue;
                            IdProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);
                            int IdTpoAsunto = Variables.wIdTpoAsunto;
                            IdSeccion = Convert.ToInt32(ddlSecciones.SelectedValue);
                            IdCategoria = Convert.ToInt32(ddlCategorias.SelectedValue);
                            string IdUsuario = Variables.wUserLogon;

                            string strQuery = "INSERT INTO ITM_27 (IdCliente, IdProyecto, IdTpoAsunto, IdSeccion, IdCategoria, IdDocumento, NomArchivo, IdUsuario, IdStatus) " +
                                              "VALUES ('" + IdCliente + "', " + IdProyecto + ", " + IdTpoAsunto + ", " + IdSeccion + ", " + IdCategoria + ", " + IdDocumento + ", '" + Descripcion + "', '" + IdUsuario + "', 1)" + "\n \n";

                            int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                            dbConn.Close();
                        }
                    }
                }

                GetDocumentos(IdProyecto, IdCategoria, IdSeccion);

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void Eliminar_ITM_88()
        {
            try
            {
                int index = Variables.wRenglon;

                int IdDoc_Categoria = Convert.ToInt32(GrdDocumentos.Rows[index].Cells[1].Text);
                int IdProyecto = Convert.ToInt32(GrdDocumentos.Rows[index].Cells[2].Text);
                int IdSeccion = Convert.ToInt32(GrdDocumentos.Rows[index].Cells[3].Text);
                int IdCategoria = Convert.ToInt32(GrdDocumentos.Rows[index].Cells[4].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_27)
                string strQuery = "DELETE FROM ITM_27 " +
                                  " WHERE IdDoc_Categoria = " + IdDoc_Categoria + " " +
                                  "   AND IdProyecto = " + IdProyecto + " " +
                                  "   AND IdSeccion = " + IdSeccion + " " +
                                  "   AND IdCategoria = " + IdCategoria + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino documento, correctamente";
                mpeMensaje.Show();

                GetDocumentos(IdProyecto, IdCategoria, IdSeccion);
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
            Eliminar_ITM_88();
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void GrdDocCarpeta_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdDocCarpeta, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;     // IdConsecutivo
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;     // IdConsecutivo
            }
        }

    }
}