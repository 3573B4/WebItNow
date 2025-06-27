using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwDeliver_Document : System.Web.UI.Page
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
                string sReferencia = Request.QueryString["Ref"];
                int iSubReferencia = Convert.ToInt32(Request.QueryString["SubRef"]);

                GetDocumentos_Asunto("*");
                GetDatosAsunto(sReferencia, iSubReferencia);
            }
        }

        protected void GetDatosAsunto(string sReferencia, int iSubReferencia)
        {
            TxtReferencia.Text = string.Empty;
            TxtSiniestro.Text = string.Empty;
            TxtPoliza.Text = string.Empty;
            TxtAsegurado.Text = string.Empty;

            try
            {
                // DesHabilitar_Controles();

                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT t0.Referencia, t0.NumSiniestro, t0.NumPoliza, t0.NomAsegurado " +
                                  "  FROM  ITM_70 t0 " +
                                  " WHERE t0.Referencia = '" + sReferencia + "'" +
                                  "   AND t0.SubReferencia = " + iSubReferencia + "" +
                                  "   AND t0.IdStatus = 1";

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {

                }
                else
                {
                    if (iSubReferencia == 0)
                    {
                        TxtReferencia.Text = dt.Rows[0].ItemArray[0].ToString();
                    } 
                    else
                    {
                        TxtReferencia.Text = dt.Rows[0].ItemArray[0].ToString() + "-" + iSubReferencia;
                    }
                    
                    TxtSiniestro.Text = dt.Rows[0].ItemArray[1].ToString();
                    TxtPoliza.Text = dt.Rows[0].ItemArray[2].ToString();
                    TxtAsegurado.Text = dt.Rows[0].ItemArray[3].ToString();

                    GetDocumentos_Asunto(TxtReferencia.Text);

                }

                Conecta.Cerrar();
                cmd.Dispose();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetDocumentos_Asunto(string sReferencia)
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

                //strQuery = "SELECT Columna, Descripcion, '' as Fec_Entrega, CAST(Entregado AS bit) as Entregado FROM ( " +
                //            "       SELECT " +
                //            "       Documento_1, Documento_2, Documento_3, Documento_4, " +
                //            "       Documento_5, Documento_6, Documento_7, Documento_8, " +
                //            "       Documento_9, Documento_10, " +
                //            "       ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS rn " +
                //            "  FROM ITM_Documentos_Check " +
                //            " WHERE Referencia = '" + sReferencia + "' ) AS tbl " +
                //            " CROSS APPLY ( VALUES  " +
                //            "    ('Documento_1', CONVERT(VARCHAR(225), Documento_1), '', 0)," +
                //            "    ('Documento_2', CONVERT(VARCHAR(225), Documento_2), '', 0)," +
                //            "    ('Documento_3', CONVERT(VARCHAR(225), Documento_3), '', 0)," +
                //            "    ('Documento_4', CONVERT(VARCHAR(225), Documento_4), '', 1)," +
                //            "    ('Documento_5', CONVERT(VARCHAR(225), Documento_5), '', 0)," +
                //            "    ('Documento_8', CONVERT(VARCHAR(225), Documento_6), '', 0)," +
                //            "    ('Documento_7', CONVERT(VARCHAR(225), Documento_7), '', 1)," +
                //            "    ('Documento_8', CONVERT(VARCHAR(225), Documento_8), '', 0)," +
                //            "    ('Documento_9', CONVERT(VARCHAR(225), Documento_9), '', 0)," +
                //            "    ('Documento_10', CONVERT(VARCHAR(225), Documento_10), '', 0)" +
                //            " ) AS unpivoted(Columna, Descripcion, Fec_Entrega, Entregado) ORDER BY rn";

                //strQuery = "SELECT (CEILING((COUNT(CASE WHEN IdStatus = 1 THEN 1 END) * 100.0 / COUNT(*)) / 10) * 10) AS Porcentaje " +
                //           "  FROM ITM_Temporal" +

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    GrdCheck_List.ShowHeaderWhenEmpty = true;
                    GrdCheck_List.EmptyDataText = "No hay resultados.";
                }

                GrdCheck_List.DataSource = dt;
                GrdCheck_List.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdCheck_List.HeaderRow.TableSection = TableRowSection.TableHeader;

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


        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwReporte_Alta_Asunto.aspx", true);
        }

        protected void GrdCheck_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GrdCheck_List_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdCheck_List_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdCheck_List_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdCheck_List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdCheck_List, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = Unit.Pixel(1050);        // Descripcion
                e.Row.Cells[1].Width = Unit.Pixel(100);         // Fecha_Entrega
                e.Row.Cells[2].Width = Unit.Pixel(100);         // Entregado
                e.Row.Cells[5].Width = Unit.Pixel(50);          // Eliminar
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[3].Visible = false;    // IdStatus
                e.Row.Cells[4].Visible = false;    // Id_CheckList
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Visible = false;    // IdStatus
                e.Row.Cells[4].Visible = false;    // Id_CheckList
            }
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

        protected void BtnAceptar_New_Click(object sender, EventArgs e)
        {

            if (ddlTpoDocumento.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Tipo de documento";
                mpeMensaje.Show();
                return ;
            }

            if (TxtDescripcion.Text == "" || TxtDescripcion.Text == null)
            {
                LblMessage.Text = "Capturar Descripción del documento";
                mpeMensaje.Show();
                return ;
            }

            string sReferencia = TxtReferencia.Text;
            int sIdTpoDocumento = Convert.ToInt32(ddlTpoDocumento.SelectedValue);
            string sDescripcion = TxtDescripcion.Text;

            int Envio_Ok = Add_tbDetalleArchivos(sReferencia, sIdTpoDocumento, sDescripcion);

            if (Envio_Ok == 0)
            {
                GetDocumentos_Asunto(TxtReferencia.Text);

                // Inicializar Controles
                ddlTpoDocumento.ClearSelection();
                TxtDescripcion.Text = string.Empty;

                LblMessage.Text = "Se agrego documento, correctamente";
                mpeMensaje.Show();

                // LblMessage.Text = "Su registro fue creado exitosamente";
                // this.mpeMensaje.Show();
            }
        }

        public int Add_tbDetalleArchivos(string pReferencia, int pIdTpoDocumento, string pDescripcion)
        {
            try
            {
                int iConsecutivo = GetIdConsecutivoMax();
                int iProceso = 6;

                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string strQuery = "INSERT INTO ITM_73 (Referencia, Id_Documento, Descripcion, Fec_Entrega, Entregado, IdStatus) " +
                                  "VALUES ('" + pReferencia + "', " + pIdTpoDocumento + ", '" + pDescripcion + "', Null, 0, 1)" + "\n \n";

                strQuery += Environment.NewLine;

                strQuery += "INSERT INTO ITM_12 (IdConsecutivo, IdProceso, IdTpoDocumento, Descripcion) " +
                            "VALUES (" + iConsecutivo + ", " + iProceso + ", " + pIdTpoDocumento + ", '" + pDescripcion + "')" + "\n \n";

                // Insert en la tabla Estado de Documento
                SqlCommand cmd1 = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

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

        protected void BtnCerrar_New_Click(object sender, EventArgs e)
        {

        }

        protected void ddlTpoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sIdTpoDocumento = ddlTpoDocumento.SelectedValue;
        }

        protected void Delete_ITM_73(int iIdCheckList)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string sUsuario = Variables.wUserName;

                // Eliminar registro tabla (ITM_73)
                string sqlQuery = "DELETE FROM ITM_73 " +
                                  " WHERE Referencia = '" + TxtReferencia.Text + "'" +
                                  "   AND Id_CheckList = " + iIdCheckList + "";

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                int affectedRows = cmd.ExecuteNonQuery();

                Conecta.Cerrar();
                cmd.Dispose();

                LblMessage.Text = "Se elimino documento, correctamente";
                mpeMensaje.Show();

                GetDocumentos_Asunto(TxtReferencia.Text);
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
            Variables.wRenglon = row.RowIndex;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            LblMessage_1.Text = "¿Desea eliminar el documento?";
            mpeMensaje_1.Show();

        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            GetBusqProceso();
            mpeNewProceso.Show();
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

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Add_ITM_73();
        }

        protected void btnClose_Proceso_Click(object sender, EventArgs e)
        {

        }

        protected void Add_ITM_73()
        {
            try
            {

                int pIdTpoDocumento = 12;

                foreach (GridViewRow row in grdPnlBusqProceso.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        var chkbox = row.FindControl("ChBoxRow") as CheckBox;

                        if (chkbox.Checked)
                        {
                            string Descripcion = Server.HtmlDecode(Convert.ToString(row.Cells[2].Text));

                            ConexionBD Conecta = new ConexionBD();
                            Conecta.Abrir();

                            string strQuery = "INSERT INTO ITM_73 (Referencia, Id_Documento, Descripcion, Fec_Entrega, Entregado, IdStatus) " +
                                              "VALUES ('" + TxtReferencia.Text + "', " + pIdTpoDocumento + ", '" + Descripcion + "', Null, 0, 1)" + "\n \n";

                            SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                            cmd.ExecuteReader();
                        }
                    }
                }

                GetDocumentos_Asunto(TxtReferencia.Text);

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

            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();
            string sqlQuery = "SELECT COALESCE(MAX(IdConsecutivo), 0) + 1 IdConsecutivo " +
                                " FROM ITM_12 ";

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    IdConsecutivoMax = Convert.ToInt32(dr["IdConsecutivo"].ToString().Trim());
                }
            }

            Conecta.Cerrar();
            cmd.Dispose();
            dr.Dispose();

            return IdConsecutivoMax;

        }
        protected void ChkEntregado_CheckedChanged(object sender, EventArgs e)
        {

            // Obtén el CheckBox que ha causado el cambio
            CheckBox chkEntregado = (CheckBox)sender;

            // Obtén la fila actual del GridView
            GridViewRow row = (GridViewRow)chkEntregado.NamingContainer;

            int index = row.RowIndex;
            int iIdCheckList = Convert.ToInt32(GrdCheck_List.Rows[index].Cells[4].Text);

            CheckBox chkEntregadoEnFila = (CheckBox)row.FindControl("ChkEntregado");

            if (chkEntregadoEnFila != null)
            {
                // Puedes acceder directamente al valor Checked del CheckBox
                bool valorEntregado = chkEntregadoEnFila.Checked;

                ActualizarEntregado(valorEntregado, iIdCheckList);
            }
                // Puedes obtener el valor de la columna deseada utilizando DataBinder.Eval
                // int idRegistro = (int)DataBinder.Eval(row.DataItem, "ID"); // Asegúrate de reemplazar "ID" con el nombre de la columna que contiene el identificador único de tu registro

                // Aquí puedes realizar la actualización en tu base de datos o cualquier lógica que necesites
                // Por ejemplo, podrías llamar a un método en tu capa de acceso a datos para actualizar el valor en la base de datos
                // Ejemplo hipotético: MiCapaDatos.ActualizarEntregado(idRegistro, chkEntregado.Checked);

                // Después de realizar la actualización, puedes recargar tus datos si es necesario
                // Ejemplo hipotético: CargarDatos();
                GetDocumentos_Asunto(TxtReferencia.Text);
        }

        protected void ActualizarEntregado(bool valorEntregado, int iIdCheckList)
        {

            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string sUsuario = Variables.wUserName;
                string sqlQuery = string.Empty;

                // Eliminar registro tabla (ITM_73)
                if (valorEntregado == true)
                {
                    sqlQuery = "UPDATE ITM_73 " +
                               "   SET Entregado = '" + valorEntregado + "', " +
                               "       Fec_Entrega = GETDATE() " +
                               " WHERE Referencia = '" + TxtReferencia.Text + "'" +
                               "   AND Id_CheckList = " + iIdCheckList + "";
                } 
                else
                {
                    sqlQuery = "UPDATE ITM_73 " +
                               "   SET Entregado = '" + valorEntregado + "', " +
                               "       Fec_Entrega = NULL " +
                               " WHERE Referencia = '" + TxtReferencia.Text + "'" +
                               "   AND Id_CheckList = " + iIdCheckList + "";
                }

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                int affectedRows = cmd.ExecuteNonQuery();

                Conecta.Cerrar();
                cmd.Dispose();

                LblMessage.Text = "Se actualizo documento, correctamente";
                mpeMensaje.Show();

                GetDocumentos_Asunto(TxtReferencia.Text);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            int index = Variables.wRenglon;
            int iIdCheckList = Convert.ToInt32(GrdCheck_List.Rows[index].Cells[4].Text);

            Delete_ITM_73(iIdCheckList);
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void grdPnlBusqProceso_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdPnlBusqProceso.PageIndex = e.NewPageIndex;
                GetBusqProceso();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
    }
}