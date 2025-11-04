using System;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;
namespace WebItNow_Peacock
{
    public partial class fwGM_Catalog_Categorias : System.Web.UI.Page
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

                    GetCategorias();

                    Inicializar_GrdCategorias();

                    // GetTipoAsunto();
                    // GetCategorias("ITM_21");


                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

            }
        }

        protected void GetTipoAsunto()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdTpoAsunto, Descripcion " +
                                  "  FROM ITM_66 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        protected void GetCategorias()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdCategoria, Descripcion " +
                                  "  FROM ITM_87 " +
                                  " WHERE IdStatus = 1 AND IdCategoria NOT IN (4, 6) " +
                                  " ORDER BY IdOrden";

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

        public void GetCategorias(string Tabla)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_21, ITM_22, ITM_23, ITM_24
                string strQuery = $"SELECT IdDocumento, Descripcion " +
                                  $"  FROM { Tabla } " +
                                  //$" WHERE IdTpoAsunto = " + ddlTpoAsunto.SelectedValue + "" +
                                  $" WHERE IdStatus = 1 " +
                                  $" ORDER BY IdDocumento";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdCategorias.ShowHeaderWhenEmpty = true;
                    GrdCategorias.EmptyDataText = "No hay resultados.";
                }

                GrdCategorias.DataSource = dt;
                GrdCategorias.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdCategorias.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        private void Inicializar_GrdCategorias()
        {
            // Crea un DataTable vacío con la estructura necesaria
            DataTable dt = CrearDataTableVacio();

            // Verifica si el DataTable tiene filas
            if (dt.Rows.Count == 0)
            {
                // Mostrar el mensaje de "No hay resultados"
                GrdCategorias.ShowHeaderWhenEmpty = true;
                GrdCategorias.EmptyDataText = "No hay resultados.";
            }

            // Enlaza el DataTable (vacío o lleno) al GridView
            GrdCategorias.DataSource = dt;
            GrdCategorias.DataBind();
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

        public int Add_tbDocumentos(string Tabla, string pDescripcion)
        {
            try
            {
                int iConsecutivo = GetIdConsecutivoMax(Tabla);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = $"INSERT INTO { Tabla } (IdDocumento, Descripcion, DescripBrev, IdTpoAsunto, IdStatus) " +
                                  $"VALUES (" + iConsecutivo + ", '" + pDescripcion + "', Null, 0, 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se agrego categoría, correctamente";
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

        public int GetIdConsecutivoMax(string Tabla)
        {

            int IdConsecutivoMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = $"SELECT COALESCE(MAX(IdDocumento), 0) + 1 IdDocumento " +
                              $"  FROM { Tabla } ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdDocumento"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void Eliminar_tbDocumentos(string Tabla, int iIdDocumento, int iIdTpoAsunto)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName; //LblUsuario.Text;

                // Eliminar registro tabla
                string strQuery = $"DELETE FROM { Tabla } " +
                                  $" WHERE IdDocumento = " + iIdDocumento + " ";
                //$"   AND IdTpoAsunto = " + iIdTpoAsunto + "";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino categoría, correctamente";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    LblMessage.Text = "Categoria, se encuentra relacionada a un Asunto";
                }
                else
                {
                    LblMessage.Text = Convert.ToString(ex.Message);
                }

                mpeMensaje.Show();
            }
        }

        protected void Actualizar_tbDocumentos(string Tabla, int iIdDocumento, int iIdTpoAsunto)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName; //LblUsuario.Text;

                // Eliminar registro tabla
                string strQuery = $"UPDATE { Tabla } " +
                                  $"   SET Descripcion = '" + TxtNomCategoria.Text.Trim() + "' " +
                                  $" WHERE IdDocumento = " + iIdDocumento + " ";
                //$"   AND IdTpoAsunto = " + iIdTpoAsunto + "";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizo categoría, correctamente";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    LblMessage.Text = "Categoria, se encuentra relacionada a un Asunto";
                }
                else
                {
                    LblMessage.Text = Convert.ToString(ex.Message);
                }

                mpeMensaje.Show();
            }
        }

        protected void GrdCategorias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdCategorias.PageIndex = e.NewPageIndex;
                GetCategorias(Variables.wTabla);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdCategorias_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdCategorias_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdCategorias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdCategorias, "Select$" + e.Row.RowIndex.ToString()) + ";");

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

            if (ddlSecciones.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Categorías Por";
                mpeMensaje.Show();
                return;
            }
            else if (TxtNomCategoria.Text == "" || TxtNomCategoria.Text == null)
            {
                LblMessage.Text = "Capturar Descripción de Categoría";
                mpeMensaje.Show();
                return;
            }

            string sDescripcion = TxtNomCategoria.Text;
            string sTabla = Variables.wTabla;

            int Envio_Ok = Add_tbDocumentos(sTabla, sDescripcion);

            if (Envio_Ok == 0)
            {

                // inicializar controles
                TxtNomCategoria.Text = string.Empty;

                GetCategorias(sTabla);
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

            LblMessage_1.Text = "¿Desea eliminar la categoria ?";
            mpeMensaje_1.Show();

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {

            int index = Variables.wRenglon;

            int iIdDocumento = Convert.ToInt32(GrdCategorias.Rows[index].Cells[0].Text);
            //int iIdTpoAsunto = Convert.ToInt32(GrdCategorias.Rows[index].Cells[2].Text);
            int iIdTpoAsunto = 0;

            Eliminar_tbDocumentos(Variables.wTabla, iIdDocumento, iIdTpoAsunto);

            GetCategorias(Variables.wTabla);
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void ddlTpoAsunto_SelectedIndexChanged(object sender, EventArgs e)
        {
            //switch (ddlSecciones.SelectedValue)
            //{
            //    case "1":
            //        GetCategorias("ITM_21");
            //        Variables.wTabla = "ITM_21";

            //        break;
            //    case "2":
            //        GetCategorias("ITM_22");
            //        Variables.wTabla = "ITM_22";

            //        break;
            //    case "3":
            //        GetCategorias("ITM_23");
            //        Variables.wTabla = "ITM_23";

            //        break;
            //    case "4":

            //        break;
            //    case "5":
            //        GetCategorias("ITM_24");
            //        Variables.wTabla = "ITM_24";

            //        break;
            //    default:
            //        break;
            //}
        }

        protected void ddlSecciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ddlTpoAsunto.ClearSelection();
            switch (ddlSecciones.SelectedValue)
            {
                case "1":
                    GetCategorias("ITM_21");
                    Variables.wTabla = "ITM_21";

                    break;
                case "2":
                    GetCategorias("ITM_22");
                    Variables.wTabla = "ITM_22";

                    break;
                case "3":
                    GetCategorias("ITM_23");
                    Variables.wTabla = "ITM_23";

                    break;
                case "4":
                    // GetCategorias("ITM_84");
                    // Variables.wTabla = "ITM_84";

                    break;
                case "5":
                    GetCategorias("ITM_24");
                    Variables.wTabla = "ITM_24";

                    break;
                case "6":
                    // GetCategorias("ITM_92");
                    // Variables.wTabla = "ITM_92";

                    break;
                default:
                    Inicializar_GrdCategorias();
                    break;
            }

        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            TxtNomCategoria.Text = Server.HtmlDecode(Convert.ToString(GrdCategorias.Rows[index].Cells[1].Text));

            TxtNomCategoria.ReadOnly = true;
            TxtNomCategoria.ReadOnly = true;

            ddlSecciones.Enabled = false;

            BtnAnular.Visible = true;
            btnEditar.Enabled = true;
            BtnAgregar.Enabled = false;
        }

        protected void BtnAnular_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            ddlSecciones.Enabled = true;

            TxtNomCategoria.Text = string.Empty;
            TxtNomCategoria.ReadOnly = false;

            btnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            btnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            TxtNomCategoria.ReadOnly = false;

            btnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (TxtNomCategoria.Text == "" || TxtNomCategoria.Text == null)
            {
                LblMessage.Text = "Capturar Descripción de Categoría";
                mpeMensaje.Show();
                return;
            }

            int index = Variables.wRenglon;

            int iIdDocumento = Convert.ToInt32(GrdCategorias.Rows[index].Cells[0].Text);
            int iIdTpoAsunto = 0;

            Actualizar_tbDocumentos(Variables.wTabla, iIdDocumento, iIdTpoAsunto);

            GetCategorias(Variables.wTabla);

            // inicializar controles.
            ddlSecciones.Enabled = true;

            TxtNomCategoria.Text = string.Empty;
            TxtNomCategoria.ReadOnly = false;

            btnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            btnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }
    }
}