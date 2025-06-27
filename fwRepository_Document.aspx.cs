using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwRepository_Document : System.Web.UI.Page
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
                    // GetAccion();
                    // GetTipoAsunto();
                    GetTipoEstatus();
                    GetDocumentos("*");


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
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string sqlQuery = "SELECT IdSeguros, Descripcion " +
                                        " FROM ITM_67 " +
                                        " WHERE IdSeguros <> 'OTR'" +
                                        "   AND IdStatus = 1 " +
                                        "ORDER BY IdOrden";
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlCliente.DataSource = dt;

                ddlCliente.DataValueField = "IdSeguros";
                ddlCliente.DataTextField = "Descripcion";

                ddlCliente.DataBind();
                ddlCliente.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                Conecta.Cerrar();
                cmd.Dispose();
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
                string sCliente = ddlCliente.SelectedValue;

                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string sqlQuery = "SELECT IdProyecto, Descripcion " +
                                  "  FROM ITM_78 " +
                                  " WHERE IdCliente = '" + sCliente + "'" +
                                  "   AND IdStatus = 1 ";

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlProyecto.DataSource = dt;

                ddlProyecto.DataValueField = "IdProyecto";
                ddlProyecto.DataTextField = "Descripcion";

                ddlProyecto.DataBind();
                ddlProyecto.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                Conecta.Cerrar();
                cmd.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetAccion()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string sqlQuery = "SELECT IdAccion, Descripcion " +
                                        " FROM ITM_77 " +
                                        " WHERE IdStatus = 1 ";
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                //ddlAccion.DataSource = dt;

                //ddlAccion.DataValueField = "IdAccion";
                //ddlAccion.DataTextField = "Descripcion";

                //ddlAccion.DataBind();
                //ddlAccion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                Conecta.Cerrar();
                cmd.Dispose();
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
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string sqlQuery = "SELECT a.IdTpoAsunto, a.Descripcion " +
                                        " FROM ITM_66 as a, ITM_78 as b " +
                                        " WHERE a.IdTpoAsunto = b.IdTpoAsunto " +
                                        "  AND IdProyecto = " + Convert.ToInt32(ddlProyecto.SelectedValue) + " " +
                                        "  AND a.IdStatus = 1 ";

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count != 0)
                {
                    TxtTpoAsunto.Text = dt.Rows[0].ItemArray[1].ToString();
                }

                // ddlTpoAsunto.DataSource = dt;

                // ddlTpoAsunto.DataValueField = "IdTpoAsunto";
                // ddlTpoAsunto.DataTextField = "Descripcion";

                // ddlTpoAsunto.DataBind();
                // ddlTpoAsunto.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                Conecta.Cerrar();
                cmd.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        protected void GetTipoEstatus()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string sqlQuery = "SELECT IdEstatus, Descripcion " +
                                        " FROM ITM_76 " +
                                        " WHERE IdStatus = 1 ";
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlTpoEstatus.DataSource = dt;

                ddlTpoEstatus.DataValueField = "IdEstatus";
                ddlTpoEstatus.DataTextField = "Descripcion";

                ddlTpoEstatus.DataBind();
                ddlTpoEstatus.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                Conecta.Cerrar();
                cmd.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetDocumentos(string sReferencia)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string strQuery = string.Empty;

                strQuery = "SELECT t0.Id_CheckList, Descripcion, t0.Fec_Entrega, t0.Entregado, t0.IdStatus " +
                           "  FROM ITM_73 as t0 " +
                           " WHERE t0.Referencia = '" + sReferencia + "'" +
                           "   AND t0.IdStatus = 1";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    GrdDocumentos.ShowHeaderWhenEmpty = true;
                    GrdDocumentos.EmptyDataText = "No hay resultados.";
                }

                GrdDocumentos.DataSource = dt;
                GrdDocumentos.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdDocumentos.HeaderRow.TableSection = TableRowSection.TableHeader;

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

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlProyecto.ClearSelection();
            TxtTpoAsunto.Text = string.Empty;

            GetProyecto();
        }

        protected void ddlTpoAsunto_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTipoAsunto();
        }

        protected void ddlAccion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnDocument_Proceso_Click(object sender, EventArgs e)
        {
            try
            {
                GetBusqProceso();
                mpeNewProceso.Show();
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
                int iProceso = 6;       // Convert.ToInt32(Session["Proceso"]);
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_12
                string strQuery = "SELECT p.Descripcion as Desc_Proceso, d.Descripcion " +
                                  "  FROM ITM_08 as p, ITM_12 as d " +
                                  " WHERE p.IdTpoDocumento = d.IdTpoDocumento " +
                                  "   AND d.IdProceso = " + iProceso + " " +
                                  "   AND d.IdTpoDocumento = 12 " +
                                  "   AND d.Descripcion like '%" + txtPnlBusqProceso.Text + "%'";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    grdPnlBusqProceso.ShowHeaderWhenEmpty = true;
                    grdPnlBusqProceso.EmptyDataText = "No hay resultados.";
                }

                grdPnlBusqProceso.DataSource = dt;
                grdPnlBusqProceso.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                grdPnlBusqProceso.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ddlTpoStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnAddDocument_Click(object sender, EventArgs e)
        {
            GetCatalog_Documento();
            mpeNewDocumento.Show();
        }

        public void GetCatalog_Documento()
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdTpoDocumento, Descripcion " +
                                    "FROM ITM_08 " +
                                    "WHERE IdTpoDocumento IN (12)";

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlTpoDocumento.DataSource = dt;

                ddlTpoDocumento.DataValueField = "IdTpoDocumento";
                ddlTpoDocumento.DataTextField = "Descripcion";

                ddlTpoDocumento.DataBind();
                ddlTpoDocumento.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ChkEntregado_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void GrdDocumentos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

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

        }

        protected void btnClose_Proceso_Click(object sender, EventArgs e)
        {
            txtPnlBusqProceso.Text = string.Empty;
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            GetBusqProceso();
            mpeNewProceso.Show();
        }

        protected void ddlTpoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_New_Click(object sender, EventArgs e)
        {
            if (ddlTpoDocumento.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Tipo de documento";
                mpeMensaje.Show();
                return;
            }

            if (TxtDescripcion.Text == "" || TxtDescripcion.Text == null)
            {
                LblMessage.Text = "Capturar Descripción del documento";
                mpeMensaje.Show();
                return;
            }

            int Envio_Ok = 0;

            // string sReferencia = TxtReferencia.Text;
            int sIdTpoDocumento = Convert.ToInt32(ddlTpoDocumento.SelectedValue);
            string sDescripcion = TxtDescripcion.Text;

            // int Envio_Ok = Add_tbDetalleArchivos(sReferencia, sIdTpoDocumento, sDescripcion);

            if (Envio_Ok == 0)
            {
                GetDocumentos("*");

                // Inicializar Controles
                ddlTpoDocumento.ClearSelection();
                TxtDescripcion.Text = string.Empty;

                LblMessage.Text = "Se agrego documento, correctamente";
                mpeMensaje.Show();

                // LblMessage.Text = "Su registro fue creado exitosamente";
                // this.mpeMensaje.Show();
            }
        }

        protected void BtnCerrar_New_Click(object sender, EventArgs e)
        {

        }


    }
}