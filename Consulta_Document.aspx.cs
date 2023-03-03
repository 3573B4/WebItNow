using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow
{
    public partial class Consulta_Document : System.Web.UI.Page
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
                getStatus();


                string sReferencia = Convert.ToString(Session["IdUsuario"]);
                lblRef.Text = "Bienvenido: " + sReferencia;
            }

            getCamposGV();
            grdConsultaDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        public void getStatus()
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdStatus, Descripcion " +
                                    "FROM ITM_07 WHERE IdStatus != 0 ";

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlStatusDocumento.DataSource = dt;

                ddlStatusDocumento.DataValueField = "IdStatus";
                ddlStatusDocumento.DataTextField = "Descripcion";

                ddlStatusDocumento.DataBind();
            }catch(Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        public void getCamposGV()
        {
            try
            {
                //la opcion seleccionada en el ddl
                string sValorStatus = ddlStatusDocumento.SelectedValue;

                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Estado de Documento
                string sqlQuery = "SELECT ed.Referencia, " +
                                         "td.Descripcion, " +
                                         "ed.Url_Imagen, " +
                                         "ed.Nom_Imagen, " +
                                         "s.Descripcion AS Desc_Status, " +
                                         "ed.Fec_Envio, ed.Fec_Aceptado, ed.Fec_Rechazado, " +
                                         "ed.IdDescarga " +
                                  "FROM ITM_04 ed, ITM_06 td, ITM_07 s " +
                                  "WHERE ed.IdStatus = s.IdStatus AND ed.IdTipoDocumento = td.IdTpoDocumento ";
                if (sValorStatus != "0")
                {
                    sqlQuery = sqlQuery + "AND ed.IdStatus = '" + sValorStatus + "'";
                }

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    grdConsultaDocumento.ShowHeaderWhenEmpty = true;
                    grdConsultaDocumento.EmptyDataText = "no hay resultados.";
                }

                grdConsultaDocumento.DataSource = dt;
                grdConsultaDocumento.DataBind();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Acceso.aspx");

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }

        protected void grdConsultaDocumento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdConsultaDocumento.PageIndex = e.NewPageIndex;

            getCamposGV();
        }

        public IEnumerable<int> GetPages()
        {
            return Enumerable.Range(1, grdConsultaDocumento.PageCount);
        }
    }
}