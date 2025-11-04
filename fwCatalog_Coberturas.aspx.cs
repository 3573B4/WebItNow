using System;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwCatalog_Coberturas : System.Web.UI.Page
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

                    // Labels
                    lblTitulo_Cat_Coberturas.Text = GetGlobalResourceObject("GlobalResources", "lblTitulo_Cat_Coberturas").ToString();

                    GetCiaSeguros();
                    // GetSecciones();

                    ddlProducto.Items.Clear();
                    ddlProducto.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                    Inicializar_GrdCoberturas();

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

                //SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //da.Fill(dt);

                ddlCliente.DataSource = dt;

                ddlCliente.DataValueField = "IdSeguros";
                ddlCliente.DataTextField = "Descripcion";

                ddlCliente.DataBind();
                //ddlCliente.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlCliente.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                //Conecta.Cerrar();
                //cmd.Dispose();
                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }


        protected void GetProductos()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdProducto, Descripcion " +
                                        " FROM ITM_39 " +
                                        " WHERE IdSeguros = '" + ddlCliente.SelectedValue + "' " +
                                        "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                //SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //da.Fill(dt);

                ddlProducto.DataSource = dt;

                ddlProducto.DataValueField = "IdProducto";
                ddlProducto.DataTextField = "Descripcion";

                ddlProducto.DataBind();
                //ddlProducto.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlProducto.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                //Conecta.Cerrar();
                //cmd.Dispose();
                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetSecciones()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_44
                string strQuery = "SELECT IdSeccion, Descripcion " +
                                        " FROM ITM_44 " +
                                        " WHERE IdStatus = 1";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlSecciones.DataSource = dt;

                ddlSecciones.DataValueField = "IdSeccion";
                ddlSecciones.DataTextField = "Descripcion";

                ddlSecciones.DataBind();
                //ddlSecciones.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlSecciones.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        private void Inicializar_GrdCoberturas()
        {
            // Crea un DataTable vacío con la estructura necesaria
            DataTable dt = CrearDataTableVacio();

            // Verifica si el DataTable tiene filas
            if (dt.Rows.Count == 0)
            {
                // Mostrar el mensaje de "No hay resultados"
                GrdCoberturas.ShowHeaderWhenEmpty = true;
                GrdCoberturas.EmptyDataText = GetGlobalResourceObject("GlobalResources", "msg_NoResults").ToString();

                //GrdCoberturas.EmptyDataText = "No hay resultados.";
            }

            // Enlaza el DataTable (vacío o lleno) al GridView
            GrdCoberturas.DataSource = dt;
            GrdCoberturas.DataBind();
        }

        private DataTable CrearDataTableVacio()
        {
            DataTable dt = new DataTable();

            // Define las columnas del DataTable
            dt.Columns.Add("IdCobertura", typeof(string));
            dt.Columns.Add("IdSeguros", typeof(string));
            dt.Columns.Add("IdProducto", typeof(string));
            dt.Columns.Add("IdSeccion", typeof(string));
            dt.Columns.Add("Cve_Cobertura", typeof(string));
            dt.Columns.Add("Descripcion", typeof(string));
            // Agrega más columnas según sea necesario

            return dt;
        }

        public void GetCoberturas(string sIdSeguro, int iIdProducto, int iIdSeccion)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a la tabla : Coberturas  = ITM_94
                string strQuery = "SELECT IdCobertura, IdSeguros, IdProducto, IdSeccion, Cve_Cobertura, Descripcion " +
                                  "  FROM ITM_94 " +
                                  " WHERE IdStatus = 1 " +
                                  "   AND IdSeguros = '" + sIdSeguro + "' " +
                                  "   AND IdProducto =  " + iIdProducto + " " +
                                  "   AND IdSeccion =  " + iIdSeccion + " " +
                                  " ORDER BY IdCobertura";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdCoberturas.ShowHeaderWhenEmpty = true;
                    GrdCoberturas.EmptyDataText = GetGlobalResourceObject("GlobalResources", "msg_NoResults").ToString();

                    //GrdCoberturas.EmptyDataText = "No hay resultados.";
                }

                GrdCoberturas.DataSource = dt;
                GrdCoberturas.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdCoberturas.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdCoberturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdCoberturas.PageIndex = e.NewPageIndex;
                int iIdSeccion = 0;
                GetCoberturas(ddlCliente.SelectedValue, Convert.ToInt16(ddlProducto.SelectedValue), iIdSeccion);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdCoberturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdCoberturas_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdCoberturas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdCoberturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdCoberturas, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Width = Unit.Pixel(100);     // Cve_Cobertura
                e.Row.Cells[5].Width = Unit.Pixel(1100);    // Descripcion
                e.Row.Cells[6].Width = Unit.Pixel(25);      // ImgEditar
                e.Row.Cells[7].Width = Unit.Pixel(25);      // ImgEliminar
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;         // IdCobertura
                e.Row.Cells[1].Visible = false;         // IdSeguros
                e.Row.Cells[2].Visible = false;         // IdProducto
                e.Row.Cells[3].Visible = false;         // IdSeccion
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;         // IdCobertura
                e.Row.Cells[1].Visible = false;         // IdSeguros
                e.Row.Cells[2].Visible = false;         // IdProducto
                e.Row.Cells[3].Visible = false;         // IdSeccion
            }
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {

            int index = Variables.wRenglon;

            int iIdCobertura = Convert.ToInt32(GrdCoberturas.Rows[index].Cells[0].Text);
            int iIdProducto = Convert.ToInt32(GrdCoberturas.Rows[index].Cells[2].Text);
            int iIdSeccion = Convert.ToInt32(GrdCoberturas.Rows[index].Cells[3].Text);

            Eliminar_tbDocumentos(ddlCliente.SelectedValue, iIdCobertura, iIdProducto, iIdSeccion);

            GetCoberturas(ddlCliente.SelectedValue, Convert.ToInt16(ddlProducto.SelectedValue), iIdSeccion);
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {

            // ddlSecciones.SelectedValue = "0";
            Inicializar_GrdCoberturas();

            GetProductos();
        }

        protected void BtnAnular_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            ddlCliente.Enabled = true;
            ddlProducto.Enabled = true;
            // ddlSecciones.Enabled = true;

            TxtNomCobertura.Text = string.Empty;
            TxtNomCobertura.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            TxtNomCobertura.ReadOnly = false;

            BtnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (TxtNomCobertura.Text == "" || TxtNomCobertura.Text == null)
            {
                // LblMessage.Text = "Capturar Descripción de Cobertura";  
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Capturar_DescCobertura").ToString();
                mpeMensaje.Show();
                return;
            }

            int index = Variables.wRenglon;

            int iIdCobertura = Convert.ToInt16(GrdCoberturas.Rows[index].Cells[0].Text);
            int iIdProducto = Convert.ToInt16(GrdCoberturas.Rows[index].Cells[2].Text);
            int iIdSeccion = Convert.ToInt16(GrdCoberturas.Rows[index].Cells[3].Text);

            string sIdSeguros = ddlCliente.SelectedValue;

            Actualizar_tbDocumentos(iIdCobertura, sIdSeguros, iIdProducto, iIdSeccion);

            GetCoberturas(ddlCliente.SelectedValue, Convert.ToInt16(ddlProducto.SelectedValue), iIdSeccion);

            // inicializar controles.
            ddlCliente.Enabled = true;
            ddlProducto.Enabled = true;
            // ddlSecciones.Enabled = true;

            TxtNomCobertura.Text = string.Empty;
            TxtNomCobertura.ReadOnly = false;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void Actualizar_tbDocumentos(int iIdCobertura, string sIdSeguros, int iIdProducto, int iIdSeccion)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName;

                // Eliminar registro tabla
                string strQuery = "UPDATE ITM_94 " +
                                  "   SET Descripcion = '" + TxtNomCobertura.Text.Trim() + "' " +
                                  " WHERE IdCobertura = " + iIdCobertura + " " +
                                  "   AND IdSeguros = '" + sIdSeguros + "' " +
                                  "   AND IdProducto = " + iIdProducto + " " +
                                  "   AND IdSeccion = " + iIdSeccion + "";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                // LblMessage.Text = "Se actualizo cobertura, correctamente";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Cobertura_Actualizada").ToString();
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    // LblMessage.Text = "Cobertura, se encuentra relacionada a un Asunto";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Cobertura_Relacionada").ToString();
                }
                else
                {
                    LblMessage.Text = Convert.ToString(ex.Message);
                }

                mpeMensaje.Show();
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {

            if (ddlCliente.SelectedValue == "0")
            {
                // LblMessage.Text = "Seleccionar Coberturas Por";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_CiaSeguros").ToString();
                mpeMensaje.Show();
                return;
            }
            else if (ddlProducto.SelectedValue == "0")
            {
                // LblMessage.Text = "Seleccionar un producto";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Producto").ToString();
                mpeMensaje.Show();
                return;
            }
            else if (TxtNomCobertura.Text == "" || TxtNomCobertura.Text == null)
            {
                // LblMessage.Text = "Capturar Descripción de Cobertura";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Capturar_DescCobertura").ToString();
                mpeMensaje.Show();
                return;
            }

            string sDescripcion = TxtNomCobertura.Text;
            int iIdSeccion = 0;

            int Envio_Ok = Add_tbDocumentos(ddlCliente.SelectedValue, Convert.ToInt16(ddlProducto.SelectedValue), iIdSeccion, sDescripcion);

            if (Envio_Ok == 0)
            {

                // inicializar controles
                TxtNomCobertura.Text = string.Empty;

                GetCoberturas(ddlCliente.SelectedValue, Convert.ToInt16(ddlProducto.SelectedValue), iIdSeccion);
            }
        }

        public int Add_tbDocumentos(string sIdSeguros, int iIdProducto, int iIdSeccion,  string pDescripcion)
        {
            try
            {
                int iConsecutivo = GetIdConsecutivoMax();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "INSERT INTO ITM_94 (IdCobertura, IdSeguros, IdProducto, IdSeccion, Cve_Cobertura, Descripcion, DescripBrev, IdStatus) " +
                                  "VALUES (" + iConsecutivo + ", '" + sIdSeguros + "', " + iIdProducto + ", " + iIdSeccion + ", CONCAT('CBR-', LPAD(" + iConsecutivo + ", 4, '0')), '" + pDescripcion + "', Null, 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                // LblMessage.Text = "Se agrego cobertura, correctamente";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Cobertura_Agregado").ToString();
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

            string strQuery = "SELECT COALESCE(MAX(IdCobertura), 0) + 1 IdCobertura " +
                              "  FROM ITM_94 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdCobertura"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void Eliminar_tbDocumentos(string sIdSeguros, int iIdCobertura, int iIdProducto, int iIdSeccion)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName; //LblUsuario.Text;

                // Eliminar registro tabla
                string strQuery = "DELETE FROM ITM_94 " +
                                  " WHERE IdCobertura = " + iIdCobertura + " " +
                                  "   AND IdSeguros = '" + sIdSeguros + "' " +
                                  "   AND IdProducto = " + iIdProducto + " " +
                                  "   AND IdSeccion = " + iIdSeccion + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                // LblMessage.Text = "Se elimino cobertura, correctamente";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Cobertura_Eliminada").ToString();
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    // LblMessage.Text = "Cobertura, se encuentra relacionada a un Asunto";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Cobertura_Relacionada").ToString();
                }
                else
                {
                    LblMessage.Text = Convert.ToString(ex.Message);
                }

                mpeMensaje.Show();
            }
        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            TxtNomCobertura.Text = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[5].Text));

            TxtNomCobertura.ReadOnly = true;

            ddlCliente.Enabled = false;
            ddlProducto.Enabled = false;
            // ddlSecciones.Enabled = false;

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

            // LblMessage_1.Text = "¿Desea eliminar la cobertura ?";
            LblMessage_1.Text = GetGlobalResourceObject("GlobalResources", "msg_Confirmar_Delete_Cobertura").ToString();
            mpeMensaje_1.Show();
        }

        protected void ddlProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            Inicializar_GrdCoberturas();
            // ddlSecciones.SelectedValue = "0";
            int iIdSeccion = 0;
            GetCoberturas(ddlCliente.SelectedValue, Convert.ToInt16(ddlProducto.SelectedValue), iIdSeccion);
        }

        protected void ddlSecciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            // GetCoberturas(ddlCliente.SelectedValue, Convert.ToInt16(ddlProducto.SelectedValue), Convert.ToInt16(ddlSecciones.SelectedValue));
        }
    }
}