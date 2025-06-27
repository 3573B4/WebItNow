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
    public partial class fwDetalle_Asunto : System.Web.UI.Page
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
                string SubReferencia = Request.QueryString["SubRef"];
                //string IdProyecto = Request.QueryString["Proyecto"];
                //string CveCliente = Request.QueryString["Seguro"];
                //string IdTpoAsunto = Request.QueryString["Asunto"];

                Variables.wRef = sReferencia;
                Variables.wSubRef = Convert.ToInt32(SubReferencia);
                //Variables.wIdProyecto = Convert.ToInt32(IdProyecto);
                //Variables.wPrefijo_Aseguradora = CveCliente;
                //Variables.wIdTpoAsunto = Convert.ToInt32(IdTpoAsunto);

                inhabilitar(this.Controls);

                //GetConclusion();
                //GetRegimen();

                //string flechaHaciaAbajo = "\u25BC";
                //btnShowPanel1.Text = flechaHaciaAbajo; // Flecha hacia abajo

                //// habiliar control(es)
                //ddlTpoAsegurado.Enabled = true;
                //ddlConclusion.Enabled = true;

                GetConsulta_Datos(sReferencia, SubReferencia);
                // GetCategorias();

                // Actualizar CheckBox Seleccionados
                // Select_ITM_91(Variables.wRef, Variables.wSubRef);

            }
        }

        protected void GetConclusion()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string sqlQuery = "SELECT C.[IdDocumento], C.Descripcion " +
                             "  FROM ITM_86 AS A, ITM_87 AS B, ITM_83 AS C " +
                             " WHERE A.IdSeccion = B.IdCategoria " +
                             "   AND A.[IdDocumento] = C.[IdDocumento] " +
                             "   AND [IdProyecto] = " + Variables.wIdProyecto + " AND [IdCliente] = '" + Variables.wPrefijo_Aseguradora + "' " +
                             "   AND [IdSeccion] = 3 ";

                    if (Variables.wIdProyecto != 0)
                    {
                       sqlQuery += "   AND A.[IdTpoAsunto] = " + Variables.wIdTpoAsunto + " ";
                    }

                       sqlQuery += "   AND [bSeleccion] = 1 AND A.[IdStatus] = 1 " +
                                   " ORDER BY C.[IdDocumento] ";

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                //ddlConclusion.DataSource = dt;

                //ddlConclusion.DataValueField = "IdDocumento";
                //ddlConclusion.DataTextField = "Descripcion";

                //ddlConclusion.DataBind();
                //ddlConclusion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                Conecta.Cerrar();
                cmd.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetRegimen()
        {
            try
            {

                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                 string sqlQuery = "SELECT C.[IdDocumento], C.Descripcion " +
                                   "  FROM ITM_86 AS A, ITM_87 AS B, ITM_81 AS C " +
                                   " WHERE A.IdSeccion = B.IdCategoria " +
                                   "   AND A.[IdDocumento] = C.[IdDocumento] " +
                                   "   AND [IdProyecto] = " + Variables.wIdProyecto + " AND [IdCliente] = '" + Variables.wPrefijo_Aseguradora + "' " +
                                   "   AND [IdSeccion] = 1 ";
                    
                    if (Variables.wIdProyecto != 0)
                    {
                       sqlQuery += "   AND A.[IdTpoAsunto] = " + Variables.wIdTpoAsunto + " ";
                    }

                       sqlQuery += "   AND [bSeleccion] = 1 AND A.[IdStatus] = 1 " +
                                   " ORDER BY C.[IdDocumento] ";

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                //ddlTpoAsegurado.DataSource = dt;

                //ddlTpoAsegurado.DataValueField = "IdDocumento";
                //ddlTpoAsegurado.DataTextField = "Descripcion";

                //ddlTpoAsegurado.DataBind();
                //ddlTpoAsegurado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                Conecta.Cerrar();
                cmd.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public int GetConsulta_Datos(string pReferencia, string pSubReferencia)
        {

            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string strQuery = "SET LANGUAGE Spanish " +
                           "SELECT TOP (50) t0.IdAsunto, t0.SubReferencia, CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END as Referencia_Sub, " +
                           "       t0.NumSiniestro, t0.NumPoliza, t0.NumReporte, t0.Referencia_Anterior, " +
                           "       STUFF(FORMAT(CAST(Fecha_Asignacion AS DATE), 'dd-MMM-yyyy'), 4, 1, UPPER(SUBSTRING(FORMAT(CAST(Fecha_Asignacion AS DATE), 'dd-MMM-yyyy'), 4, 1))) AS Fecha_Asignacion, " +
                           "       t0.NomActor, t0.NomDemandado, t0.NomAsegurado, " +
                           "       CASE WHEN t0.IdStatus = 1 THEN 'ABIERTO' ELSE 'CERRADO' END as IdStatus, t0.Referencia as Referencia," +
                           "       t1.Descripcion as Tpo_Asunto, " +
                           "       CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END as Seguro_Cia, " +
                           "       t3.Descripcion as Resp_Tecnico, t4.Descripcion as Resp_Administrativo, t5.Descripcion as IdProyecto, " +
                           "       t0.IdRegimen, t0.IdConclusion " +
                           "  FROM ITM_70 t0 " +
                           "  JOIN ITM_66 t1 ON t0.IdTpoAsunto = t1.IdTpoAsunto " +
                           "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                           "  LEFT JOIN ITM_68 t3 ON t0.IdRespTecnico = t3.IdRespTecnico " +
                           "  LEFT JOIN ITM_69 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                           "  LEFT JOIN ITM_78 t5 ON t0.IdProyecto = t5.IdProyecto " +
                           " WHERE t0.IdStatus IN (1) " +
                           "   AND t0.Referencia = '" + pReferencia + "'" +
                           "   AND t0.SubReferencia = '" + pSubReferencia + "'" +
                           " ORDER BY t0.IdAsunto DESC";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    // Informacion General
                    TxtSubReferencia.Text = Convert.ToString(row[2]);
                    TxtNumSiniestro.Text = Convert.ToString(row[3]);
                    TxtNumPoliza.Text = Convert.ToString(row[4]);
                    TxtNumReporte.Text = Convert.ToString(row[5]);
                 // TxtRef_Anterior.Text = Convert.ToString(row[6]);
                    TxtFecha_Asignacion.Text = Convert.ToString(row[7]);
                    TxtNomActor.Text = Convert.ToString(row[8]);
                    TxtNomDemandado.Text = Convert.ToString(row[9]);
                    TxtNomAsegurado.Text = Convert.ToString(row[10]);
                    TxtEstatus.Text = Convert.ToString(row[11]);
                 // TxtReferencia.Text = Convert.ToString(row[12]);
                    TxtTpo_Asunto.Text = Convert.ToString(row[13]);
                    TxtSeguro_Cia.Text = Convert.ToString(row[14]);
                    TxtResp_Tecnico.Text = Convert.ToString(row[15]);
                    TxtResp_Administrativo.Text = Convert.ToString(row[16]);
                    TxtProyecto.Text = Convert.ToString(row[17]);
                    //ddlTpoAsegurado.SelectedValue = Convert.ToString(row[18]);
                    //ddlConclusion.SelectedValue = Convert.ToString(row[19]);

                    return 0;
                }

                cmd.Dispose();
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

        public void inhabilitar(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Enabled = false;
                else if (control is DropDownList)
                    ((DropDownList)control).Enabled = false;
                else if (control is RadioButtonList)
                    ((RadioButtonList)control).Enabled = false;
                else if (control is CheckBoxList)
                    ((CheckBoxList)control).Enabled = false;
                else if (control is RadioButton)
                    ((RadioButton)control).Enabled = false;
                else if (control is CheckBox)
                    ((CheckBox)control).Enabled = false;
                else if (control.HasControls())
                    //Esta linea detécta un Control que contenga otros Controles
                    //Así ningún control se quedará sin ser limpiado.
                    inhabilitar(control.Controls);
            }

        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            //Variables.wRef = string.Empty;
            //Variables.wSubRef = 0;
            //Variables.wIdProyecto = 0;
            //Variables.wPrefijo_Aseguradora = string.Empty;
            //Variables.wIdTpoAsunto = 0;

            Response.Redirect("fwReporte_Alta_Asunto.aspx", true);
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void ddlTpoAsegurado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdCategorias_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Width = Unit.Pixel(150);     // DescCategoria
                e.Row.Cells[4].Width = Unit.Pixel(150);     // DescSeccion
                e.Row.Cells[5].Width = Unit.Pixel(25);      // ChBoxRow
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;     // IdDocumento
                e.Row.Cells[1].Visible = false;     // IdSeccion
                e.Row.Cells[2].Visible = false;     // IdCategoria
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;     // IdDocumento
                e.Row.Cells[1].Visible = false;     // IdSeccion
                e.Row.Cells[2].Visible = false;     // IdCategoria
            }
        }

        protected void GrdCategorias_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                // Eliminar registros
                // Delete_ITM_91();
                // Actualizar campo IdRegimen
                // Update_ITM_70();

                // Insertar en ITM_91 (Tipo de Asegurado)
                // Insert_ITM_91();

                int IdProyecto = Variables.wIdProyecto;
                string IdCliente = Variables.wPrefijo_Aseguradora;
                int IdTpoAsunto = Variables.wIdTpoAsunto;

                string IdUsuario = Variables.wUserName;

                //foreach (GridViewRow row in GrdCategorias.Rows)
                //{
                //    if (row.RowType == DataControlRowType.DataRow)
                //    {
                //        var chkbox = row.FindControl("ChBoxRow") as CheckBox;

                //        if (chkbox.Checked)
                //        {
                //            string IdDocumento = Server.HtmlDecode(Convert.ToString(row.Cells[0].Text));
                //            string IdSeccion = Server.HtmlDecode(Convert.ToString(row.Cells[1].Text));
                //            string IdCategoria = Server.HtmlDecode(Convert.ToString(row.Cells[2].Text));

                //            ConexionBD Conecta = new ConexionBD();
                //            Conecta.Abrir();

                //            string strQuery = "INSERT INTO ITM_91 (Referencia, SubReferencia, IdProyecto, IdCliente, IdTpoAsunto, IdSeccion, IdCategoria, IdDocumento, bSeleccion, IdUsuario) " +
                //                              "VALUES ('" + Variables.wRef + "', " + Variables.wSubRef + ", " + IdProyecto + ", '" + IdCliente + "', " + IdTpoAsunto + ", " + IdSeccion + ", " +
                //                              "" + IdCategoria + ", " + IdDocumento + ", 1, '" + IdUsuario + "')" + "\n \n";

                //            SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //            cmd.ExecuteReader();
                //        }
                //    }
                //}

                LblMessage.Text = "Se han aplicado los cambios, correctamente";
                this.mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ddlConclusion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnShowPanel1_Click(object sender, EventArgs e)
        {
            //pnl1.Visible = !pnl1.Visible;   // Cambia la visibilidad del Panel 1 al contrario de su estado actual

            //if (pnl1.Visible)
            //{
            //    string flechaHaciaArriba = "\u25B2";
            //    btnShowPanel1.Text = flechaHaciaArriba; // Flecha hacia arriba
            //    pnl1.Visible = true;
            //}
            //else
            //{
            //    string flechaHaciaAbajo = "\u25BC";
            //    btnShowPanel1.Text = flechaHaciaAbajo; // Flecha hacia abajo
            //    pnl1.Visible = false;
            //}
        }
    }
}