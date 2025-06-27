using System;
using System.IO;

using System.Data;
using System.Data.SqlClient;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwGenerar_Formatos : System.Web.UI.Page
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

                    // GetCiaSeguros();
                    GetConsulta_Datos("*", "*");
                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

                //* * Agrega THEAD y TBODY a GridView.
                GrdConsulta_Datos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void habilitar_controles()
        {
            TxtReporte.Enabled = true;
            // ddlCiaSeguros.Enabled = true;
        }

        protected void deshabilitar_controles()
        {
            TxtReporte.Enabled = false;
            // ddlCiaSeguros.Enabled = false;
        }

        //protected void GetCiaSeguros()
        //{
        //    try
        //    {
        //        ConexionBD Conecta = new ConexionBD();
        //        Conecta.Abrir();

        //        string sqlQuery = "SELECT IdSeguros, Descripcion " +
        //                                " FROM ITM_67 " +
        //                                " WHERE IdStatus = 1 ORDER BY IdOrden";
        //        SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);

        //        ddlCiaSeguros.DataSource = dt;

        //        ddlCiaSeguros.DataValueField = "IdSeguros";
        //        ddlCiaSeguros.DataTextField = "Descripcion";

        //        ddlCiaSeguros.DataBind();
        //        ddlCiaSeguros.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

        //        Conecta.Cerrar();
        //        cmd.Dispose();
        //    }
        //    catch (System.Exception ex)
        //    {
        //        LblMessage.Text = ex.Message;
        //        mpeMensaje.Show();
        //    }
        //}

        protected void GetConsulta_Datos(string sValor, string sIdReporte)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string strQuery = string.Empty;

                if (sValor == string.Empty)
                {
                    strQuery = "SELECT TOP (50) t0.Id_Expediente, t0.Referencia, t0.Num_Reporte, t0.Num_Siniestro, t0.Fec_Asignacion, t0.Riesgo, t0.Det_Reporte, t0.Asegurado_1, t0.IdStatus " +
                               "  FROM ITM_Temporal t0 " +
                               " WHERE   t0.IdStatus IN (1) " +
                               "   AND ( t0.Num_Reporte LIKE '%' + '" + TxtReporte.Text + "' + '%' OR t0.Num_Siniestro LIKE '%' + '" + TxtReporte.Text + "' + '%' " +
                               "    OR   t0.Num_Siniestro = '" + sIdReporte + "' )" +
                               " ORDER BY t0.Id_Expediente DESC";
                }
                else
                {
                    strQuery = "SELECT TOP (50) t0.Id_Expediente, t0.Referencia, t0.Num_Reporte, t0.Num_Siniestro, t0.Fec_Asignacion, t0.Riesgo, t0.Det_Reporte, t0.Asegurado_1, t0.IdStatus " +
                               "  FROM ITM_Temporal t0 " +
                               " WHERE t0.IdStatus IN (1) " +
                               " ORDER BY t0.Id_Expediente DESC";
                }

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    GrdConsulta_Datos.ShowHeaderWhenEmpty = true;
                    GrdConsulta_Datos.EmptyDataText = "No hay resultados.";
                }

                GrdConsulta_Datos.DataSource = dt;
                GrdConsulta_Datos.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdConsulta_Datos.HeaderRow.TableSection = TableRowSection.TableHeader;

                Conecta.Cerrar();
                cmd.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
                //Lbl_Message.Text = FnErrorMessage(ex.Message);
            }
        }

        public string FnErrorMessage(string prmMessage)
        {
            return ("<span style=\"color:Red;\">" +
                    "<img src = \"images/icons16/error.png\" height=\"16\" width=\"16\" alt=\"Error\" />&nbsp;" +
                    prmMessage + "</span>");
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {

        }

        protected void GrdConsulta_Datos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdConsulta_Datos.PageIndex = e.NewPageIndex;
            GetConsulta_Datos("*", "Reporte");
        }

        protected void GrdConsulta_Datos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdConsulta_Datos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdConsulta_Datos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdConsulta_Datos, "Select$" + e.Row.RowIndex.ToString()) + ";");


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = Unit.Pixel(120);         // Referencia Siniestro
                e.Row.Cells[1].Width = Unit.Pixel(100);         // Num.Reporte
                e.Row.Cells[2].Width = Unit.Pixel(100);         // Num.Siniestro
                e.Row.Cells[3].Width = Unit.Pixel(100);         // Fecha de Asignación
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Visible = false;    // Detalles del Reporte
                e.Row.Cells[7].Visible = false;    // IdStatus

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[5].Visible = false;    // Detalles del Reporte
                e.Row.Cells[7].Visible = false;    // IdStatus
            }
        }

        protected void GrdConsulta_Datos_PreRender(object sender, EventArgs e)
        {
            // Ajustar el estilo de las filas en general antes de renderizar
            foreach (GridViewRow row in GrdConsulta_Datos.Rows)
            {
                row.Style["height"] = "20px"; // Establecer la altura de las filas
            }

        }

        //protected void ddlCiaSeguros_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string IdSeguro = ddlCiaSeguros.SelectedValue;

        //    if (IdSeguro != "0")
        //    {
        //        GetConsulta_Datos(string.Empty, IdSeguro);
        //    }
        //    else
        //    {
        //        GetConsulta_Datos("*", "*");
        //    }

        //}


        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void ImgPagina_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);

            int index = row.RowIndex;
            string sReferencia = Server.HtmlDecode(Convert.ToString(GrdConsulta_Datos.Rows[index].Cells[0].Text));

            Response.Redirect("fwGenerar_Formatos_1.aspx?Referencia=" + sReferencia, true);
        }

        protected void ImgBusReporte_Click(object sender, ImageClickEventArgs e)
        {
            string IdReporte = TxtReporte.Text.Trim();

            if (IdReporte == "")
            {
                GetConsulta_Datos("*", string.Empty);
            }
            else
            {
                GetConsulta_Datos(string.Empty, IdReporte);
            }
        }
    }
}