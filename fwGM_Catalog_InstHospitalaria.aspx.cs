using System;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwGM_Catalog_InstHospitalaria : System.Web.UI.Page
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

                    GetInstituciones();

                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

            }
        }

        public void GetInstituciones()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Instituciones Hospitalarias = ITM_13
                string strQuery = "SELECT Id_Institucion, Descripcion " +
                                        " FROM ITM_13 " +
                                        " WHERE IdStatus = 1";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdInstitucion.ShowHeaderWhenEmpty = true;
                    GrdInstitucion.EmptyDataText = "No hay resultados.";
                }

                GrdInstitucion.DataSource = dt;
                GrdInstitucion.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdInstitucion.HeaderRow.TableSection = TableRowSection.TableHeader;
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

            LblMessage_1.Text = "¿Desea eliminar Institución Hospitalaria?";
            mpeMensaje_1.Show();
        }

        protected void Eliminar_ITM_13()
        {
            try
            {
                int index = Variables.wRenglon;

                int Id_Institucion = Convert.ToInt32(GrdInstitucion.Rows[index].Cells[0].Text); ;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_13)
                string strQuery = "DELETE FROM ITM_13 " +
                                  " WHERE Id_Institucion = " + Id_Institucion + "";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino Institución Hospitalaria correctamente";
                mpeMensaje.Show();

                GetInstituciones();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Actualizar_ITM_13()
        {
            try
            {
                int index = Variables.wRenglon;

                int Id_Institucion = Convert.ToInt32(GrdInstitucion.Rows[index].Cells[0].Text); ;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Actualizar registro(s) tablas (ITM_13)
                string strQuery = "UPDATE ITM_13 " +
                                  "   SET Descripcion = '" + TxtNomInstitucion.Text.Trim() + "' " +
                                  " WHERE Id_Institucion = " + Id_Institucion + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizó institución hospitalaria correctamente";
                mpeMensaje.Show();

                GetInstituciones();
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
                if (TxtNomInstitucion.Text == "" || TxtNomInstitucion.Text == null)
                {
                    LblMessage.Text = "Capturar Nombre de la Institución Hospitalaria";
                    mpeMensaje.Show();
                    return;
                }

                int Id_Institucion = GetIdConsecutivoMax();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_13)
                string strQuery = "INSERT INTO ITM_13 (Id_Institucion, Descripcion, IdStatus) " +
                                  "VALUES (" + Id_Institucion + ", '" + TxtNomInstitucion.Text.Trim() + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se agrego Institución Hospitalaria correctamente";
                mpeMensaje.Show();

                // Inicializar Controles
                TxtNomInstitucion.Text = string.Empty;

                GetInstituciones();

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

            string strQuery = "SELECT COALESCE(MAX(Id_Institucion), 0) + 1 Id_Institucion " +
                                " FROM ITM_13 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);


            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["Id_Institucion"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            Eliminar_ITM_13();

            TxtNomInstitucion.Text = string.Empty;
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
            TxtNomInstitucion.Text = string.Empty;
            TxtNomInstitucion.ReadOnly = false;

            btnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            btnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            TxtNomInstitucion.ReadOnly = false;

            btnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            if (TxtNomInstitucion.Text == "" || TxtNomInstitucion.Text == null)
            {
                LblMessage.Text = "Capturar Nombre de la Institución Hospitalaria";
                mpeMensaje.Show();
                return;
            }

            Actualizar_ITM_13();

            // inicializar controles.
            TxtNomInstitucion.Text = string.Empty;
            TxtNomInstitucion.ReadOnly = false;

            btnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            btnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            TxtNomInstitucion.Text = Server.HtmlDecode(Convert.ToString(GrdInstitucion.Rows[index].Cells[1].Text));
            TxtNomInstitucion.ReadOnly = true;

            BtnAnular.Visible = true;
            btnEditar.Enabled = true;
            BtnAgregar.Enabled = false;
        }

        protected void BtnProyecto_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwAlta_Proyecto.aspx", true);
        }

        protected void GrdInstitucion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdInstitucion.PageIndex = e.NewPageIndex;
                GetInstituciones();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdInstitucion_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdInstitucion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdInstitucion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdInstitucion, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Width = Unit.Pixel(1200);    // Descripcion
                e.Row.Cells[2].Width = Unit.Pixel(50);      // Editar
                e.Row.Cells[3].Width = Unit.Pixel(50);      // Eliminar
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;    // Id_Institucion
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;    // Id_Institucion
            }
        }

        protected void GrdInstitucion_PreRender(object sender, EventArgs e)
        {

        }
    }
}