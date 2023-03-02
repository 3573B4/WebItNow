using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;

namespace WebItNow
{
    public partial class Request_Document : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Llena_grdRef("**", "Referencia" );

                //DataTable dt = (DataTable)Session["dt"];
                //// dt.Clear();

                //if (dt is null)
                //{
                //    GrdRef.ShowHeaderWhenEmpty = true;
                //    GrdRef.EmptyDataText = "No hay resultados.";
                //}

                //GrdRef.DataSource = dt;
                //GrdRef.DataBind();
            }

            //* * Agrega THEAD y TBODY a GridView.
            GrdRef.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnNewReference_Click(object sender, EventArgs e)
        {
            TxtRef.Text = string.Empty;
            Session["Referencia"] = TxtRef.Text;
            Response.Redirect("Request_Document_1.aspx");
        }

        protected void BtnExistReference_Click(object sender, EventArgs e)
        {
            //Session["Referencia"] = "00195-AFI21";
            //Session["Email"] = "martin.baltierra@mail.com" ;
            //Session["Aseguradora"] = "Seguros Afirme S.A. de C.V. Afirme Grupo Financiero - AFI";

            //Response.Redirect("Request_Document_1.aspx");

        }

        public void Limpia(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
                else if (control is DropDownList)
                    ((DropDownList)control).Items.Clear();
                else if (control is RadioButtonList)
                    ((RadioButtonList)control).ClearSelection();
                else if (control is CheckBoxList)
                    ((CheckBoxList)control).ClearSelection();
                else if (control is RadioButton)
                    ((RadioButton)control).Checked = false;
                else if (control is CheckBox)
                    ((CheckBox)control).Checked = false;
                else if (control.HasControls())
                    //Esta linea detécta un Control que contenga otros Controles
                    //Así ningún control se quedará sin ser limpiado.
                    Limpia(control.Controls);
            }

        }

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }

        protected void GrdRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
            // Se obtiene Columna Referencia de fila seleccionada del gridview
            //
            string sReferencia = GrdRef.SelectedRow.Cells[0].Text;

        }

        protected void GrdRef_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdRef, "Select$" + e.Row.RowIndex.ToString()) + ";");
        }

        public void Llena_grdRef(string sValor, string sColumna)
        {
            string strQuery = string.Empty;

            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            if (sColumna == "Referencia")
            {
                strQuery = "Select UsReferencia, Aseguradora, Siniestro, UsEmail " +
                           "  From ITM_02 Where UsReferencia Like '" + "%" + sValor + "%" + "'";

            } else if (sColumna == "Aseguradora")
            {
                strQuery = "Select UsReferencia, Aseguradora, Siniestro, UsEmail " +
                           "  From ITM_02 Where Aseguradora Like '" + "%" + sValor + "%" + "'";
            }
            
            SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                GrdRef.ShowHeaderWhenEmpty = true;
                GrdRef.EmptyDataText = "No hay resultados.";
            }

            GrdRef.DataSource = dt;
            GrdRef.DataBind();

            //* * Agrega THEAD y TBODY a GridView.
            GrdRef.HeaderRow.TableSection = TableRowSection.TableHeader;

        }

        protected void ImgBusReference_Click(object sender, ImageClickEventArgs e)
        {
            if (TxtRef.Text != string.Empty)
            {
                Llena_grdRef(TxtRef.Text, "Referencia");
            }
        }

        protected void ImgBusCustomer_Click(object sender, ImageClickEventArgs e)
        {
            if (TxtCliente.Text != string.Empty)
            {
                Llena_grdRef(TxtCliente.Text, "Aseguradora");
            }
        }

        protected void ImgSelect_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            string sReferencia = GrdRef.Rows[index].Cells[1].Text;
            string sAseguradora = Server.HtmlDecode(GrdRef.Rows[index].Cells[2].Text);
            string sSiniestro = GrdRef.Rows[index].Cells[3].Text;
            string sEmail = GrdRef.Rows[index].Cells[4].Text;

            Session["Referencia"] = sReferencia;
            Session["Aseguradora"] = sAseguradora;
            Session["Email"] = sEmail;

            Response.Redirect("Request_Document_1.aspx");
        }
    }
}