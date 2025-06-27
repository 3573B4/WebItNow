using System;
using System.IO;

using System.Data;
using System.Data.SqlClient;

using System.Web.UI;
using System.Web.UI.WebControls;

using DocumentFormat.OpenXml.Packaging;

namespace WebItNow_Peacock
{
    public partial class fwGenerar_Formatos_1 : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Session["DownloadsPath"] = GetDownloadFolderPath();

            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string sReferencia = Request.QueryString["Referencia"];

            if (!Page.IsPostBack)
            {
                inhabilitar(this.Controls);
                GetConsulta_Datos(sReferencia);
            }
        }

        public int GetConsulta_Datos(string pReferencia)
        {

            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string strQuery = "SELECT t0.Referencia, t0.Num_Reporte, t0.Num_Siniestro, CONVERT(VARCHAR, t0.Fec_Ocurrencia, 103) as Fec_Ocurrencia, CONVERT(VARCHAR, t0.Fec_Reporte, 103) as Fec__Reporte, " +
                                  "CONVERT(VARCHAR, t0.Fec_Asignacion, 103) as Fec_Asignacion, t0.Riesgo, t0.Det_Reporte, t0.Tpo_Contratante, t0.Lugar_Siniestro, " +
                                  "t0.Asegurado_1, t0.Asegurado_2, t0.Asegurado_3, t0.Tel_Reporte, t0.Tel_Ofi_Reporte, t0.Tel_Cel_Reporte, t0.Tel_Ase_Poliza, " +
                                  "t0.Nom_Contratante, t0.Tel_Contratante, t0.Mail_Contratante, t0.Cel_Contratante, t0.Beneficiario, " +
                                  "t0.Calle_Bien_Asegurado, t0.Colonia_Bien_Asegurado, t0.Poblacion_Bien_Asegurado, t0.Estado_Bien_Asegurado, " +
                                  "t0.Municipio_Bien_Asegurado, t0.CPostal_Bien_Asegurado, t0.Tpo_Producto, t0.Tpo_Plan, t0.Tpo_Moneda, t0.Num_Poliza," +
                                  "t0.Num_Certificado,  CONVERT(VARCHAR, t0.Fec_Emision, 103) as Fec_Emision, CONVERT(VARCHAR, t0.Fec_Ini_Vigencia, 103) as Fec_Ini_Vigencia, " +
                                  "CONVERT(VARCHAR, t0.Fec_Fin_Vigencia, 103) as Fec_Fin_Vigencia, t0.Plazo, t0.Canal_Ventas," +
                                  "t0.Calle_Asegurado, t0.Colonia_Asegurado, t0.Poblacion_Asegurado, t0.Estado_Asegurado, t0.Municipio_Asegurado, " +
                                  "t0.CPostal_Asegurado, t0.Calle_Contratante, t0.Colonia_Contratante, t0.Poblacion_Contratante, " +
                                  "t0.Estado_Contratante, t0.Municipio_Contratante, t0.CPostal_Contratante, " +
                                  "t0.Tpo_Techo, t0.Tpo_Vivienda, t0.Tpo_Muro, t0.Pisos_Bien_Asegurado, t0.Pisos_Del_Bien_Asegurado, " +
                                  "t0.Locales_Comerciales, t0.Detalles_Bien_Asegurado, t0.Regla_Suma_Asegurada " +
                                  "  FROM ITM_Temporal as t0 " +
                                  " WHERE t0.Referencia = '" + pReferencia + "'";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    // Informacion General
                    TxtReferencia.Text = Convert.ToString(row[0]);
                    TxtNumReporte.Text = Convert.ToString(row[1]);
                    TxtNumSiniestro.Text = Convert.ToString(row[2]);

                    TxtFechaOcurrencia.Text = row[3] != DBNull.Value && row[3] != null ? Convert.ToString(row[3]) : string.Empty;
                    TxtFechaReporte.Text = row[4] != DBNull.Value && row[4] != null ? Convert.ToString(row[4]) : string.Empty;
                    TxtFechaAsignacion.Text = row[5] != DBNull.Value && row[5] != null ? Convert.ToString(row[5]) : string.Empty;
                    
                    TxtRiesgo.Text = Convert.ToString(row[6]);
                    TxtDetalleReporte.Text = Convert.ToString(row[7]);
                    TxtTpoContratante.Text = Convert.ToString(row[8]);
                    TxtLugarSiniestro.Text = Convert.ToString(row[9]);

                    // Datos Personales
                    TxtAsegurado_1.Text = Convert.ToString(row[10]);
                    TxtAsegurado_2.Text = Convert.ToString(row[11]);
                    TxtAsegurado_3.Text = Convert.ToString(row[12]);
                    TxtTelReporte.Text = Convert.ToString(row[13]);
                    TxtTelOficina.Text = Convert.ToString(row[14]);
                    TxtCelReporte.Text = Convert.ToString(row[15]);
                    TxtTelAsegPoliza.Text = Convert.ToString(row[16]);
                    TxtNomContratante.Text = Convert.ToString(row[17]);
                    TxtTelContratante.Text = Convert.ToString(row[18]);
                    TxtMailContratante.Text = Convert.ToString(row[19]);
                    TxtCelContratante.Text = Convert.ToString(row[20]);
                    TxtBeneficiario.Text = Convert.ToString(row[21]);

                    // Informacion General
                    TxtCalleBienAsegurado.Text = Convert.ToString(row[22]);
                    TxtColoniaBienAsegurado.Text = Convert.ToString(row[23]);
                    TxtPoblacionBienAsegurado.Text = Convert.ToString(row[24]);
                    TxtEstadoBienAsegurado.Text = Convert.ToString(row[25]);
                    TxtMunicipioBienAsegurado.Text = Convert.ToString(row[26]);
                    TxtCPostalBienAsegurado.Text = Convert.ToString(row[27]);

                    TxtTpoProducto.Text = Convert.ToString(row[28]);
                    TxtTpoPlan.Text = Convert.ToString(row[29]);
                    TxtTpoMoneda.Text = Convert.ToString(row[30]);
                    TxtNumPoliza.Text = Convert.ToString(row[31]);
                    TxtNumCertificado.Text = Convert.ToString(row[32]);

                    TxtFechaEmision.Text = row[33] != DBNull.Value && row[32] != null ? Convert.ToString(row[32]) : string.Empty;
                    TxtFechaIniVigencia.Text = row[34] != DBNull.Value && row[33] != null ? Convert.ToString(row[33]) : string.Empty;
                    TxtFechaFinVigencia.Text = row[35] != DBNull.Value && row[34] != null ? Convert.ToString(row[34]) : string.Empty;

                    TxtPlazo.Text = Convert.ToString(row[36]);
                    TxtCanalVentas.Text = Convert.ToString(row[37]);

                    TxtCalleAsegurado.Text = Convert.ToString(row[38]);
                    TxtColoniaAsegurado.Text = Convert.ToString(row[39]);
                    TxtPoblacionAsegurado.Text = Convert.ToString(row[40]);
                    TxtEstadoAsegurado.Text = Convert.ToString(row[41]);
                    TxtMunicipioAsegurado.Text = Convert.ToString(row[42]);
                    TxtCPostalAsegurado.Text = Convert.ToString(row[43]);

                    TxtCalleContratante.Text = Convert.ToString(row[44]);
                    TxtColoniaContratante.Text = Convert.ToString(row[45]);
                    TxtPoblacionContratante.Text = Convert.ToString(row[46]);
                    TxtEstadoContratante.Text = Convert.ToString(row[47]);
                    TxtMunicipioContratante.Text = Convert.ToString(row[48]);
                    TxtCPostalContratante.Text = Convert.ToString(row[49]);

                    TxtTpoTecho.Text = Convert.ToString(row[50]);
                    TxtTpoVivienda.Text = Convert.ToString(row[51]);
                    TxtTpoMuro.Text = Convert.ToString(row[52]);
                    TxtPisosBienAsegurado.Text = Convert.ToString(row[53]);
                    TxtPisosDelBienAsegurado.Text = Convert.ToString(row[54]);
                    TxtLocalesComerciales.Text = Convert.ToString(row[55]);
                    TxtDetallesBienAsegurado.Text = Convert.ToString(row[56]);
                    TxtReglaSumaAsegurada.Text = Convert.ToString(row[57]);

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

        public void Update_Consulta_Datos(string pNum_Reporte)
        {
            // Actualizar ITM_Temporal
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string fechaOcurrencia = !string.IsNullOrEmpty(TxtFechaOcurrencia.Text) ? $"CONVERT(smalldatetime, '{TxtFechaOcurrencia.Text.Trim()}', 103)" : "Null";
                string fechaReporte = !string.IsNullOrEmpty(TxtFechaReporte.Text) ? $"CONVERT(smalldatetime, '{TxtFechaReporte.Text.Trim()}', 103)" : "Null";
                string fechaAsignacion = !string.IsNullOrEmpty(TxtFechaAsignacion.Text) ? $"CONVERT(smalldatetime, '{TxtFechaAsignacion.Text.Trim()}', 103)" : "Null";

                string fechaEmisionValue = !string.IsNullOrEmpty(TxtFechaEmision.Text) ? $"CONVERT(smalldatetime, '{TxtFechaEmision.Text.Trim()}', 103)" : "Null";
                string fechaIniVigenciaValue = !string.IsNullOrEmpty(TxtFechaIniVigencia.Text) ? $"CONVERT(smalldatetime, '{TxtFechaIniVigencia.Text.Trim()}', 103)" : "Null";
                string fechaFinVigenciaValue = !string.IsNullOrEmpty(TxtFechaFinVigencia.Text) ? $"CONVERT(smalldatetime, '{TxtFechaFinVigencia.Text.Trim()}', 103)" : "Null";

                string strQuery = "UPDATE ITM_Temporal " +
                                  " Set Num_Siniestro = '" + TxtNumSiniestro.Text.Trim() + "', Fec_Ocurrencia  = " + fechaOcurrencia + ", " +
                                  " Fec_Reporte = " + fechaReporte + ", Fec_Asignacion = " + fechaAsignacion + ", " +
                                  " Riesgo = '" + TxtRiesgo.Text.Trim() + "', Det_Reporte = '" + TxtDetalleReporte.Text.Trim() + "', " +
                                  " Tpo_Contratante = '" + TxtTpoContratante.Text + "', Lugar_Siniestro = '" + TxtLugarSiniestro.Text.Trim() + "', " +
                                  " Asegurado_1 = '" + TxtAsegurado_1.Text.Trim() + "', Asegurado_2 = '" + TxtAsegurado_2.Text.Trim() + "', " +
                                  " Asegurado_3 = '" + TxtAsegurado_3.Text.Trim() + "', Tel_Reporte = '" + TxtTelReporte.Text.Trim() + "', " +
                                  " Tel_Ofi_Reporte = '" + TxtTelOficina.Text.Trim() + "', Tel_Cel_Reporte = '" + TxtCelReporte.Text.Trim() + "', " +
                                  " Tel_Ase_Poliza = '" + TxtTelAsegPoliza.Text.Trim() + "', Nom_Contratante = '" + TxtNomContratante.Text.Trim() + "', " +
                                  " Tel_Contratante = '" + TxtTelContratante.Text.Trim() + "', Mail_Contratante = '" + TxtMailContratante.Text.Trim() + "', " +
                                  " Cel_Contratante = '" + TxtCelContratante.Text.Trim() + "', Beneficiario = '" + TxtBeneficiario.Text.Trim() + "'," +
                                  " Calle_Bien_Asegurado = '" + TxtCalleBienAsegurado.Text.Trim() + "', Colonia_Bien_Asegurado = '" + TxtColoniaBienAsegurado.Text.Trim() + "'," +
                                  " Poblacion_Bien_Asegurado = '" + TxtPoblacionBienAsegurado.Text.Trim() + "',  Estado_Bien_Asegurado = '" + TxtEstadoBienAsegurado.Text.Trim() + "', " +
                                  " Municipio_Bien_Asegurado = '" + TxtMunicipioBienAsegurado.Text.Trim() + "', CPostal_Bien_Asegurado = '" + TxtCPostalBienAsegurado.Text.Trim() + "'," +
                                  " Tpo_Producto = '" + TxtTpoProducto.Text.Trim() + "', Tpo_Plan = '" + TxtTpoPlan.Text.Trim() + "'," +
                                  " Tpo_Moneda = '" + TxtTpoMoneda.Text.Trim() + "', Num_Poliza = '" + TxtNumPoliza.Text.Trim() + "'," +
                                  " Num_Certificado = '" + TxtNumCertificado.Text.Trim() + "', Fec_Emision = " + fechaEmisionValue + ", " +
                                  " Fec_Ini_Vigencia = " + fechaIniVigenciaValue + ", Fec_Fin_Vigencia = " + fechaFinVigenciaValue + "," +
                                  " Plazo = '" + TxtPlazo.Text.Trim() + "', Canal_Ventas = '" + TxtCanalVentas.Text.Trim() + "', " +
                                  " Calle_Asegurado = '" + TxtCalleAsegurado.Text.Trim() + "', Colonia_Asegurado = '" + TxtColoniaAsegurado.Text.Trim() + "'," +
                                  " Poblacion_Asegurado = '" + TxtPoblacionAsegurado.Text.Trim() + "', Estado_Asegurado = '" + TxtEstadoAsegurado.Text.Trim() + "', " +
                                  " Municipio_Asegurado = '" + TxtMunicipioAsegurado.Text.Trim() + "', CPostal_Asegurado = '" + TxtCPostalAsegurado.Text.Trim() + "'," +
                                  " Calle_Contratante = '" + TxtCalleContratante.Text.Trim() + "', Colonia_Contratante = '" + TxtColoniaContratante.Text.Trim() + "'," +
                                  " Poblacion_Contratante = '" + TxtPoblacionContratante.Text.Trim() + "', Estado_Contratante = '" + TxtEstadoContratante.Text.Trim() + "', " +
                                  " Municipio_Contratante = '" + TxtMunicipioContratante.Text.Trim() + "', CPostal_Contratante = '" + TxtCPostalContratante.Text.Trim() + "', " +
                                  " Tpo_Techo = '" + TxtTpoTecho.Text.Trim() + "', Tpo_Vivienda = '" + TxtTpoVivienda.Text.Trim() + "'," +
                                  " Tpo_Muro = '" + TxtTpoMuro.Text.Trim() + "', Pisos_Bien_Asegurado = '" + TxtPisosBienAsegurado.Text.Trim() + "', " +
                                  " Pisos_Del_Bien_Asegurado = '" + TxtPisosDelBienAsegurado.Text.Trim() + "', Locales_Comerciales = '" + TxtLocalesComerciales.Text.Trim() + "', " +
                                  " Detalles_Bien_Asegurado = '" + TxtDetallesBienAsegurado.Text.Trim() + "', Regla_Suma_Asegurada = '" + TxtReglaSumaAsegurada.Text.Trim() + "' " +
                                  " WHERE Num_Reporte = '" + pNum_Reporte + "'";

                // Actualizar en la tabla temporal
                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataReader dr = cmd.ExecuteReader();

                cmd.Dispose();
                dr.Dispose();

                Conecta.Cerrar();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        public void habilitar(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Enabled = true;
                else if (control is DropDownList)
                    ((DropDownList)control).Enabled = true;
                else if (control is RadioButtonList)
                    ((RadioButtonList)control).Enabled = true;
                else if (control is CheckBoxList)
                    ((CheckBoxList)control).Enabled = true;
                else if (control is RadioButton)
                    ((RadioButton)control).Enabled = true;
                else if (control is CheckBox)
                    ((CheckBox)control).Enabled = true;
                else if (control.HasControls())
                    //Esta linea detécta un Control que contenga otros Controles
                    //Así ningún control se quedará sin ser limpiado.
                    habilitar(control.Controls);
            }

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
            Response.Redirect("fwGenerar_Formatos.aspx", true);
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            habilitar(this.Controls);

            TxtNumReporte.Enabled = false;
            BtnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnGeneraDocumento_Click(object sender, EventArgs e)
        {
            try
            {
                Llenar_Plantilla();

            } catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        private void ReplaceText(DocumentFormat.OpenXml.Wordprocessing.Body body, string placeholder, string newText)
        {
            foreach (var textElement in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
            {
                if (textElement.Text.Trim().Contains(placeholder))
                {
                    textElement.Text = textElement.Text.Replace(placeholder, newText);
                }
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            Update_Consulta_Datos(TxtNumReporte.Text);

            LblMessage.Text = "Se han aplicado los cambios, correctamente";
            this.mpeMensaje.Show();

            inhabilitar(this.Controls);
            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
        }

        private void Llenar_Plantilla()
        {
            try 
            { 
                string plantillaPath = Server.MapPath("~/itnowstorage/IP_CALZA_SIDER.docx");
                string documentoGeneradoPath = Server.MapPath("~/itnowstorage/DocumentoGenerado.docx");

                DateTime fecOcurrencia = DateTime.ParseExact(TxtFechaOcurrencia.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaOcurrencia = fecOcurrencia.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecAsignacion = DateTime.ParseExact(TxtFechaAsignacion.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaAsignacion = fecAsignacion.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                // Copiar la plantilla a un nuevo documento
                System.IO.File.Copy(plantillaPath, documentoGeneradoPath, true);

                // Abrir el documento generado
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documentoGeneradoPath, true))
                {
                    // Obtener el cuerpo del documento
                    DocumentFormat.OpenXml.Wordprocessing.Body body = wordDoc.MainDocumentPart.Document.Body;

                    // Buscar y reemplazar marcadores de posición con datos reales
                    ReplaceText(body, "Num_Reporte", TxtNumReporte.Text);
                    ReplaceText(body, "Num_Siniestro", TxtNumSiniestro.Text);
                    ReplaceText(body, "Fec_Ocurrencia", FechaOcurrencia);
                    ReplaceText(body, "Fec_Asignacion", FechaAsignacion);

                    // Guardar los cambios
                    wordDoc.MainDocumentPart.Document.Save();
                }

                LblMessage.Text = "El Documento Word, se a generado correctamente";
                this.mpeMensaje.Show();

                // Descargar el documento generado
                Session["sFileName"] = "DocumentoGenerado.docx";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AbrirDescarga", string.Format("window.open('Descargas.aspx');"), true);

                // Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                // Response.AddHeader("Content-Disposition", "attachment; filename=DocumentoGenerado.docx");
                // Response.TransmitFile(documentoGeneradoPath);
                // Response.End();

                // Mostrar mensaje en el lado del cliente después de la descarga
                // ClientScript.RegisterStartupScript(this.GetType(), "downloadComplete", "alert('El Documento Word, se a generado correctamente.');", true);

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }
    }
}