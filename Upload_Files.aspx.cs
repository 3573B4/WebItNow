using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebItNow
{
    public partial class Upload_Files : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                // * * Obtener Descripcion breve documento
                getDocsUsuario();

                string sReferencia = Convert.ToString(Session["Referencia"]);
                lblReferencia.Text = "Referencia: " + sReferencia;
            }

            //* * Agrega THEAD y TBODY a GridView.
            grdEstadoDocs.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        public void getDocsUsuario()
        {
            string sReferencia = Convert.ToString(Session["Referencia"]);
            ConexionBD Conectar = new ConexionBD();
            Conectar.Abrir();

            string sqlQuery = "SELECT ed.Referencia, ed.IdTipoDocumento, ed.Nom_Imagen, td.Descripcion, s.Descripcion as Desc_Status  " +
                                  "  FROM ITM_04 ed, ITM_06 td, ITM_07 s " +
                                  " WHERE ed.IdStatus = s.IdStatus And ed.IdTipoDocumento = td.IdTpoDocumento " +
                                  "   AND Referencia = '" + sReferencia + "'";

            SqlCommand ejecucion = new SqlCommand(sqlQuery, Conectar.ConectarBD);
            SqlDataAdapter da = new SqlDataAdapter(ejecucion);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                grdEstadoDocs.ShowHeaderWhenEmpty = true;
                grdEstadoDocs.EmptyDataText = "No hay resultados.";
            }

            grdEstadoDocs.DataSource = dt;
            grdEstadoDocs.DataBind();

        }

        protected void grdEstadoDocs_PageIndexChanged(Object sender, EventArgs e)
        {
            // Call a helper method to display the current page number 
            // when the user navigates to a different page.
            DisplayCurrentPage();
        }

        protected void grdEstadoDocs_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdEstadoDocs.PageIndex = e.NewPageIndex;
            getDocsUsuario();
        }

        protected void grdEstadoDocs_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.grdEstadoDocs, "Select$" + e.Row.RowIndex.ToString()) + ";");

            string IdStatus = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Desc_status"));

            if (IdStatus == "Revision" || IdStatus == "Completo")
            {
                (e.Row.FindControl("BtnCargaDocumento") as Button).Enabled = false;

            }
        }

        protected void DisplayCurrentPage()
        {
            // Calculate the current page number.
            int currentPage = grdEstadoDocs.PageIndex + 1;

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Acceso.aspx");
        }

        protected void BtnCargaDocumento_Click(object sender, EventArgs e)
        {
            Response.Redirect("Upload_Files_2.aspx");
        }

    }
}