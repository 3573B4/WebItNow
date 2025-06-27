using System;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using System.Linq;

using System.Collections.Generic;

namespace WebItNow_Peacock
{
    public partial class fwPV_Bitacora_Asunto : System.Web.UI.Page
    {

        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Session["DownloadsPath"] = GetDownloadFolderPath();

            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdUsuario"] == null || Session["UsPassword"] == null)
            {
                Response.Redirect("Login.aspx", true);
            }

            if (!Page.IsPostBack)
            {
                string sReferencia = Request.QueryString["Ref"];
                string SubReferencia = Request.QueryString["SubRef"];
                string CveCliente = Request.QueryString["Seguro"];

                Variables.wRef = sReferencia;
                Variables.wSubRef = Convert.ToInt32(SubReferencia);
                Variables.wPrefijo_Aseguradora = CveCliente;
                Variables.wExiste = false;

                inhabilitar(this.Controls);

                habilitar_control_general();
                
                // inhabilitar control Crear Cuaderno
                if (Convert.ToString(Session["UsPrivilegios"]) == "0")
                {
                    ////BtnCrear_Cuaderno.Enabled = false;
                }

                GetTpoFolio();
                GetEstados();

                ddlTpoFolio.Enabled = true;
                btnEditarPnl1.Enabled = false;

                // Obtener datos generales
                GetConsulta_Datos_Generales(sReferencia, SubReferencia);
                GetFolios(sReferencia, SubReferencia);

                // Inhabilitar controles si ya existe un Alta de Cuaderno
                if (Convert.ToString(Session["UsPrivilegios"]) != "0")
                {
                    ////Validar_Alta_Notebook();
                }

            }
            else
            {

            }
        }

        protected void GetEstados()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT DISTINCT " +
                                  "  CASE WHEN c_estado = 05 THEN 'Coahuila' " +
                                  "       WHEN c_estado = 16 THEN 'Michoacán' " +
                                  "       WHEN c_estado = 30 THEN 'Veracruz' " +
                                  "       ELSE d_estado " +
                                  "   END AS d_estado, c_estado " +
                                  " FROM ITM_75 " +
                                  "ORDER BY d_estado ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlEstado.DataSource = dt;

                ddlEstado.DataValueField = "c_estado";
                ddlEstado.DataTextField = "d_estado";

                ddlEstado.DataBind();
                ddlEstado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetMunicipios(string pEstado)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT DISTINCT D_mnpio, c_mnpio " +
                                  " FROM ITM_75 " +
                                  "WHERE c_estado = '" + pEstado + "'" +
                                  "ORDER BY D_mnpio";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlMunicipios.DataSource = dt;

                ddlMunicipios.DataValueField = "c_mnpio";
                ddlMunicipios.DataTextField = "D_mnpio";

                ddlMunicipios.DataBind();
                ddlMunicipios.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        protected void GetTpoFolio()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdTpoFolio, Descripcion " +
                                  "  FROM ITM_19 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlTpoFolio.DataSource = dt;

                ddlTpoFolio.DataValueField = "IdTpoFolio";
                ddlTpoFolio.DataTextField = "Descripcion";

                ddlTpoFolio.DataBind();
                ddlTpoFolio.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }


        public int GetConsulta_Datos_Generales(string pReferencia, string pSubReferencia)
        {

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t0.IdAsunto, t0.SubReferencia, CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END as Referencia_Sub, " +
                           "       t0.NumSiniestro, t0.NumPoliza, t0.NumReporte, t0.NomCliente, t0.NomAsegurado, " +
                           "       CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END as Seguro_Cia, " +
                           "       A.Tpo_Siniestro, A.Fec_Ocurrencia, A.Fec_Contacto, A.Fec_Inspeccion, A.Hora_Inspeccion, " +
                           "       A.Detalle_Reporte, A.Calle, A.Num_Exterior, A.Num_Interior, A.Estado, A.Delegacion, A.Colonia, A.Codigo_Postal, " +
                           "       B.Email_Contacto, B.Tel1_Contacto, B.Tel2_Contacto " +
                        // "       t0.IdEstStatus, t0.IdRegimen, t0.IdConclusion " +
                           "  FROM ITM_77 t0 " +
                           "  LEFT JOIN ITM_77_1 A ON t0.Referencia = A.Referencia AND t0.SubReferencia = A.SubReferencia " +
                           "  LEFT JOIN ITM_77_2 B ON t0.Referencia = B.Referencia AND t0.SubReferencia = B.SubReferencia " +
                           "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                           "  LEFT JOIN ITM_68 t3 ON t0.IdRespTecnico = t3.IdRespTecnico " +
                           "  LEFT JOIN ITM_69 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                           " WHERE t0.IdStatus IN (1) " +
                           "   AND t0.Referencia = '" + pReferencia + "'" +
                           "   AND t0.SubReferencia = '" + pSubReferencia + "'" +
                           " ORDER BY t0.IdAsunto DESC LIMIT 100";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                foreach (DataRow row in dt.Rows)
                {
                    // Informacion General
                    TxtSubReferencia.Text = Convert.ToString(row[2]);
                    TxtNumSiniestro.Text = Convert.ToString(row[3]);
                    TxtNumPoliza.Text = Convert.ToString(row[4]);
                    TxtNumReporte.Text = Convert.ToString(row[5]);
                    TxtNomCliente.Text = Convert.ToString(row[6]);
                    TxtNomAsegurado.Text = Convert.ToString(row[7]);
                    TxtSeguro_Cia.Text = Convert.ToString(row[8]);

                    TxtTpoSiniestro.Text = Convert.ToString(row[9]);
                    TxtFechaOcurrencia.Text = Convert.ToString(row[10]);
                    TxtFechaContacto.Text = Convert.ToString(row[11]);
                    TxtFechaInspeccion.Text = Convert.ToString(row[12]);
                    TxtHoraInspeccion.Text = string.IsNullOrEmpty(Convert.ToString(row[13])) ? "00:00" : Convert.ToString(row[13]);
                    TxtDetalleReporte.Text = Convert.ToString(row[14]);
                    TxtCalle.Text = Convert.ToString(row[15]);
                    TxtNumExterior.Text = Convert.ToString(row[16]);
                    TxtNumInterior.Text = Convert.ToString(row[17]);
                    ddlEstado.Text = Convert.ToString(row[18]);

                    // Disparar el evento SelectedIndexChanged manualmente
                    ddlEstado_SelectedIndexChanged(ddlEstado, EventArgs.Empty);

                    ddlMunicipios.Text = Convert.ToString(row[19]);
                    TxtColonia.Text = Convert.ToString(row[20]);
                    TxtCodigoPostal.Text = Convert.ToString(row[21]);

                    TxtEmailContacto1.Text = Convert.ToString(row[22]);
                    TxtTel1_Contacto1.Text = Convert.ToString(row[23]);
                    TxtTel2_Contacto1.Text = Convert.ToString(row[24]);

                    return 0;
                }

                dbConn.Close();

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

        public void GetFolios(string pReferencia, string pSubReferencia)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Folios = ITM_77_3
                string strQuery = "SELECT IdConsecutivo, " +
                               "     CASE WHEN t0.SubFolio >= 1 THEN CONCAT(t0.Folio, '-', t0.SubFolio) ELSE t0.Folio END AS Foliar, " +
                                  "       Equipo, Marca, Serie, Modelo, Condiciones, Contraseña, Folio, SubFolio, TpoFolio " +
                                  "  FROM ITM_77_3 t0" +
                                  " WHERE Referencia = '" + pReferencia + "' " +
                                  "   AND SubReferencia = '" + pSubReferencia + "' " +
                                  "   AND IdStatus = 1 ORDER BY Folio";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdFolios.ShowHeaderWhenEmpty = true;
                    GrdFolios.EmptyDataText = "No hay resultados.";
                }

                GrdFolios.DataSource = dt;
                GrdFolios.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdFolios.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }


        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            int index = Variables.wRenglon;

            string sFolio = Server.HtmlDecode(Convert.ToString(GrdFolios.Rows[index].Cells[9].Text));
            int iSubFolio = Convert.ToInt32(GrdFolios.Rows[index].Cells[10].Text);

            Eliminar_Sub_Referencia(sFolio, iSubFolio);

            Inicializar_Datos_Generales();

            GetFolios(Variables.wRef, Convert.ToString(Variables.wSubRef));
        }

        protected void Eliminar_Sub_Referencia(string sFolio, int iSubFolio)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName; //LblUsuario.Text;

                // Eliminar registro tabla (ITM_70)
                string strQuery = "DELETE FROM ITM_77_3 " +
                                  " WHERE Folio = '" + sFolio + "'" +
                                  "   AND SubFolio = " + iSubFolio + "";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage_1.Text = "Se elimino el folio, correctamente";
                mpeMensaje_1.Show();

                BtnAceptar.Visible = false;
                BtnCancelar.Visible = false;
                BtnCerrar.Visible = true;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Del_Doc_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCancelar_Del_Doc_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_CrearCuaderno_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCancelar_CrearCuaderno_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAnularPnl2_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            inhabilitar(this.Controls);

            habilitar_control_general();

            // habilitar (Configuracion Siniestro)
            // habilitar_Config_Siniestro();

            btnEditarPnl2.Visible = true;
            btnActualizarPnl2.Visible = false;
            BtnAnularPnl2.Visible = false;
        }

        protected void btnEditarPnl2_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl2
            TxtNumSiniestro.Enabled = true;
            TxtNumReporte.Enabled = true;
            TxtNumPoliza.Enabled = true;
            TxtNomCliente.Enabled = true;
            TxtNomAsegurado.Enabled = true;
            TxtTpoSiniestro.Enabled = true;
            TxtFechaOcurrencia.Enabled = true;
            TxtFechaContacto.Enabled = true;
            TxtFechaInspeccion.Enabled = true;
            TxtHoraInspeccion.Enabled = true;
            TxtDetalleReporte.Enabled = true;
            TxtCalle.Enabled = true;
            TxtNumExterior.Enabled = true;
            TxtNumInterior.Enabled = true;
            ddlEstado.Enabled = true;
            ddlMunicipios.Enabled = true;
            TxtColonia.Enabled = true;
            TxtCodigoPostal.Enabled = true;

            TxtEmailContacto1.Enabled = true;
            TxtTel1_Contacto1.Enabled = true;
            TxtTel2_Contacto1.Enabled = true;

            btnEditarPnl2.Visible = false;
            btnActualizarPnl2.Visible = true;
            BtnAnularPnl2.Visible = true;
        }

        protected void btnActualizarPnl2_Click(object sender, EventArgs e)
        {

            string input = TxtHoraInspeccion.Text;
            if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^(?:[01]\d|2[0-3]):[0-5]\d$"))
            {
                // Maneja la entrada inválida.
                // Por ejemplo, muestra un mensaje de error al usuario.
                LblMessage.Text = "Formato Hora inválido. Use hh:mm.";
                this.mpeMensaje.Show();
            }
            else
            {
                Actualizar_ITM_77();

                Actualizar_Datos_Generales();

                Actualizar_Datos_Contacto();

                inhabilitar(this.Controls);
                habilitar_control_general();

                // habilitar (Configuracion Siniestro)
                // habilitar_Config_Siniestro();

                btnEditarPnl2.Visible = true;
                btnActualizarPnl2.Visible = false;
                BtnAnularPnl2.Visible = false;

                LblMessage.Text = "Se han aplicado los cambios, correctamente";
                this.mpeMensaje.Show();
            }
        }

        protected void Actualizar_ITM_77()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Actualizar registro tabla (ITM_77)
                string strQuery = "UPDATE ITM_77 " +
                                  "   SET NumSiniestro = '" + TxtNumSiniestro.Text.Trim() + "', " +
                                  "       NumPoliza =  '" + TxtNumPoliza.Text.Trim() + "', " +
                                  "       NumReporte =  '" + TxtNumReporte.Text.Trim() + "', " +
                                  "       NomCliente  = '" + TxtNomCliente.Text.Trim() + "', " +
                                  "       NomAsegurado = '" + TxtNomAsegurado.Text.Trim() + "' " +
                                  " WHERE Referencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = " + Variables.wSubRef + " ";

                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }


        protected void Actualizar_Datos_Generales()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Informacion General
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;
                string Tpo_Siniestro = TxtTpoSiniestro.Text;
                string Fec_Ocurrencia = TxtFechaOcurrencia.Text;
                string Fec_Contacto = TxtFechaContacto.Text;
                string Fec_Inspeccion = TxtFechaInspeccion.Text;
                string Hora_Inspeccion = TxtHoraInspeccion.Text;
                string Detalle_Reporte = TxtDetalleReporte.Text;
                string Calle = TxtCalle.Text;
                string Num_Exterior = TxtNumExterior.Text;
                string Num_Interior = TxtNumInterior.Text;
                string Estado = ddlEstado.SelectedValue;
                string Delegacion = ddlMunicipios.SelectedValue;
                string Colonia = TxtColonia.Text;
                string Codigo_Postal = TxtCodigoPostal.Text;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @" INSERT INTO ITM_77_1 (Referencia, SubReferencia, Tpo_Siniestro, Fec_Ocurrencia, Fec_Contacto, Fec_Inspeccion, Hora_Inspeccion, " +
                                    "                       Detalle_Reporte, Calle, Num_Exterior, Num_Interior, Estado, Delegacion, Colonia, Codigo_Postal, " +
                                    "                       Id_Usuario, IdStatus)" +
                                    " VALUES (@Referencia, @SubReferencia, @Tpo_Siniestro, @Fec_Ocurrencia, @Fec_Contacto, @Fec_Inspeccion, @Hora_Inspeccion, " +
                                    "        @Detalle_Reporte, @Calle, @Num_Exterior, @Num_Interior, @Estado, @Delegacion, @Colonia, @Codigo_Postal, " +
                                    "        @Id_Usuario, @IdStatus)" +
                                    " ON DUPLICATE KEY UPDATE " +
                                    "    Tpo_Siniestro = VALUES(Tpo_Siniestro), " +
                                    "    Fec_Ocurrencia = VALUES(Fec_Ocurrencia), " +
                                    "    Fec_Contacto = VALUES(Fec_Contacto), " +
                                    "    Fec_Inspeccion = VALUES(Fec_Inspeccion), " +
                                    "    Hora_Inspeccion = VALUES(Hora_Inspeccion), " +
                                    "    Detalle_Reporte = VALUES(Detalle_Reporte), " +
                                    "    Calle = VALUES(Calle), " +
                                    "    Num_Exterior = VALUES(Num_Exterior), " +
                                    "    Num_Interior = VALUES(Num_Interior), " +
                                    "    Estado = VALUES(Estado), " +
                                    "    Delegacion = VALUES(Delegacion), " +
                                    "    Colonia = VALUES(Colonia), " +
                                    "    Codigo_Postal = VALUES(Codigo_Postal), " +
                                    "    Id_Usuario = VALUES(Id_Usuario), " +
                                    "    IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@Tpo_Siniestro", Tpo_Siniestro);
                    cmd.Parameters.AddWithValue("@Fec_Ocurrencia", Fec_Ocurrencia);
                    cmd.Parameters.AddWithValue("@Fec_Contacto", Fec_Contacto);
                    cmd.Parameters.AddWithValue("@Fec_Inspeccion", Fec_Inspeccion);
                    cmd.Parameters.AddWithValue("@Hora_Inspeccion", Hora_Inspeccion);
                    cmd.Parameters.AddWithValue("@Detalle_Reporte", Detalle_Reporte);
                    cmd.Parameters.AddWithValue("@Calle", Calle);
                    cmd.Parameters.AddWithValue("@Num_Exterior", Num_Exterior);
                    cmd.Parameters.AddWithValue("@Num_Interior", Num_Interior);
                    cmd.Parameters.AddWithValue("@Estado", Estado);
                    cmd.Parameters.AddWithValue("@Delegacion", Delegacion);
                    cmd.Parameters.AddWithValue("@Colonia", Colonia);
                    cmd.Parameters.AddWithValue("@Codigo_Postal", Codigo_Postal);
                    cmd.Parameters.AddWithValue("@Id_Usuario", Id_Usuario);
                    cmd.Parameters.AddWithValue("@IdStatus", IdStatus);
                });

                dbConn.Close();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Actualizar_Datos_Contacto()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Informacion General
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;
                string Email_Contacto = TxtEmailContacto1.Text;
                string Tel1_Contacto = TxtTel1_Contacto1.Text;
                string Tel2_Contacto = TxtTel2_Contacto1.Text;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @" INSERT INTO ITM_77_2 (Referencia, SubReferencia, Email_Contacto, Tel1_Contacto, Tel2_Contacto, " +
                                    "                       Id_Usuario, IdStatus)" +
                                    " VALUES (@Referencia, @SubReferencia, @Email_Contacto, @Tel1_Contacto, @Tel2_Contacto, " +
                                    "        @Id_Usuario, @IdStatus)" +
                                    " ON DUPLICATE KEY UPDATE " +
                                    "    Email_Contacto = VALUES(Email_Contacto), " +
                                    "    Tel1_Contacto = VALUES(Tel1_Contacto), " +
                                    "    Tel2_Contacto = VALUES(Tel2_Contacto), " +
                                    "    Id_Usuario = VALUES(Id_Usuario), " +
                                    "    IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@Email_Contacto", Email_Contacto);
                    cmd.Parameters.AddWithValue("@Tel1_Contacto", Tel1_Contacto);
                    cmd.Parameters.AddWithValue("@Tel2_Contacto", Tel2_Contacto);
                    cmd.Parameters.AddWithValue("@Id_Usuario", Id_Usuario);
                    cmd.Parameters.AddWithValue("@IdStatus", IdStatus);
                });

                dbConn.Close();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Actualizar_ITM_77_3()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int index = Variables.wRenglon;

                string Folio = GrdFolios.Rows[index].Cells[9].Text;
                int SubFolio = Convert.ToInt16(GrdFolios.Rows[index].Cells[10].Text);
                int TpoFolio = Convert.ToInt16(GrdFolios.Rows[index].Cells[11].Text);

                // Actualizar registro(s) tablas (ITM_77_3)
                string strQuery = "UPDATE ITM_77_3 " +
                                  "   SET Equipo = '" + TxtEquipo.Text.Trim() + "', " +
                                  "       Marca = '" + TxtMarca.Text.Trim() + "', " +
                                  "       Serie = '" + TxtSerie.Text.Trim() + "', " +
                                  "       Modelo = '" + TxtModelo.Text.Trim() + "', " +
                                  "       Condiciones = '" + TxtCondiciones.Text.Trim() + "'," +
                                  "       Contraseña = '" + TxtContraseña.Text.Trim() + "' " +
                                  " WHERE Folio = '" + Folio + "' " +
                                  "   AND SubFolio = " + SubFolio + " " +
                                  "   AND TpoFolio = " + TpoFolio + " ";
                                    


                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizo documento, correctamente";
                mpeMensaje.Show();

                GetFolios(Variables.wRef, Convert.ToString(Variables.wSubRef));
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Actualizar_Datos_Folio()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Informacion General
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                string sTpoFolio = string.Empty;

                if (ddlTpoFolio.SelectedValue == "1")
                {
                    sTpoFolio = "RPS-";

                } else if (ddlTpoFolio.SelectedValue == "2")
                {
                    sTpoFolio = "REP-";
                }

                string Folio = sTpoFolio + TxtSubReferencia.Text.Split('-')[1];
                int SubFolio = Obtener_SubFolio(Folio);
                string Equipo = TxtEquipo.Text;
                string Marca = TxtMarca.Text;
                string Serie = TxtSerie.Text;
                string Modelo = TxtModelo.Text;
                string Condiciones = TxtCondiciones.Text;
                string Contraseña = TxtContraseña.Text;
                string TpoFolio = ddlTpoFolio.SelectedValue;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @" INSERT INTO ITM_77_3 (Referencia, SubReferencia, Folio, SubFolio, Equipo, Marca, Serie, " +
                                    "                      Modelo, Condiciones, Contraseña, TpoFolio, Id_Usuario, IdStatus) " +
                                    " VALUES (@Referencia, @SubReferencia, @Folio, @SubFolio, @Equipo, @Marca, @Serie, " +
                                    "         @Modelo, @Condiciones, @Contraseña, @TpoFolio, @Id_Usuario, @IdStatus)" +
                                    " ON DUPLICATE KEY UPDATE " +
                                    "    Folio = VALUES(Folio), " +
                                    "    SubFolio = VALUES(SubFolio), " +
                                    "    Equipo = VALUES(Equipo), " +
                                    "    Marca = VALUES(Marca), " +
                                    "    Serie = VALUES(Serie), " +
                                    "    Modelo = VALUES(Modelo), " +
                                    "    Condiciones = VALUES(Condiciones), " +
                                    "    Contraseña = VALUES(Contraseña), " +
                                    "    TpoFolio = VALUES(TpoFolio), " +
                                    "    Id_Usuario = VALUES(Id_Usuario), " +
                                    "    IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@Folio", Folio);
                    cmd.Parameters.AddWithValue("@SubFolio", SubFolio);
                    cmd.Parameters.AddWithValue("@Equipo", Equipo);
                    cmd.Parameters.AddWithValue("@Marca", Marca);
                    cmd.Parameters.AddWithValue("@Serie", Serie);
                    cmd.Parameters.AddWithValue("@Modelo", Modelo);
                    cmd.Parameters.AddWithValue("@Condiciones", Condiciones);
                    cmd.Parameters.AddWithValue("@Contraseña", Contraseña);
                    cmd.Parameters.AddWithValue("@TpoFolio", TpoFolio);
                    cmd.Parameters.AddWithValue("@Id_Usuario", Id_Usuario);
                    cmd.Parameters.AddWithValue("@IdStatus", IdStatus);
                });

                dbConn.Close();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        public int Obtener_SubFolio(string folio)
        {
            int nextSubFolio = 1;

            string strQuery = @"SELECT CASE WHEN COUNT(*) = 0 THEN 1 ELSE MAX(SubFolio) + 1 END AS NextSubFolio 
                                  FROM itm_77_3 
                                 WHERE Folio = @Folio 
                                   AND Referencia = @Referencia 
                                   AND SubReferencia = @SubReferencia";

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);

            DataTable dt = dbConn.ExecuteQueryWithParameters(strQuery, cmd =>
            {
                cmd.Parameters.AddWithValue("@Folio", folio);
                cmd.Parameters.AddWithValue("@Referencia", Variables.wRef);
                cmd.Parameters.AddWithValue("@SubReferencia", Variables.wSubRef);
            });

            if (dt.Rows.Count > 0)
            {
                nextSubFolio = Convert.ToInt32(dt.Rows[0]["NextSubFolio"]);
            }

            return nextSubFolio;
        }


        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Variables.wRef = string.Empty;
            Variables.wSubRef = 0;
            Variables.wPrefijo_Aseguradora = string.Empty;

            Response.Redirect("fwPV_Reporte_Alta_Asunto.aspx", true);
        }

        public void habilitar_control_general()
        {
            TxtEquipo.Enabled = true;
            TxtMarca.Enabled = true;
            TxtSerie.Enabled = true;
            TxtModelo.Enabled = true;
            TxtCondiciones.Enabled = true;
            TxtContraseña.Enabled = true;
            ddlTpoFolio.Enabled = true;

        }

        public void Inicializar_Datos_Generales()
        {
            TxtEquipo.Text = string.Empty;
            TxtMarca.Text = string.Empty;
            TxtSerie.Text = string.Empty;
            TxtModelo.Text = string.Empty;
            TxtCondiciones.Text = string.Empty;
            TxtContraseña.Text = string.Empty;
            ddlTpoFolio.SelectedValue = "0";
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
                //else if (control is CheckBox)
                //    ((CheckBox)control).Enabled = false;
                else if (control.HasControls())
                    //Esta linea detécta un Control que contenga otros Controles
                    //Así ningún control se quedará sin ser limpiado.
                    inhabilitar(control.Controls);
            }
        }

        protected void ddlTpoFolio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnNvoFolio_Click(object sender, EventArgs e)
        {

            if (TxtEquipo.Text == "" || TxtEquipo.Text == null)
            {
                LblMessage.Text = "Capturar Descripción del Equipo";
                mpeMensaje.Show();
                return;
            }
            else if (TxtMarca.Text == "" || TxtMarca.Text == null)
            {
                LblMessage.Text = "Capturar Descripción de la Marca";
                mpeMensaje.Show();
                return;
            }
            else if (TxtSerie.Text == "" || TxtSerie.Text == null)
            {
                LblMessage.Text = "Capturar Descripción de la Serie";
                mpeMensaje.Show();
                return;
            }
            else if (TxtModelo.Text == "" || TxtModelo.Text == null)
            {
                LblMessage.Text = "Capturar Descripción del Modelo";
                mpeMensaje.Show();
                return;
            }
            else if (TxtCondiciones.Text == "" || TxtCondiciones.Text == null)
            {
                LblMessage.Text = "Capturar Condiciones del Equipo";
                mpeMensaje.Show();
                return;
            }
            else if (TxtContraseña.Text == "" || TxtContraseña.Text == null)
            {
                LblMessage.Text = "Capturar Descripción Contraseña";
                mpeMensaje.Show();
                return;
            }
            else if (ddlTpoFolio.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Tipo de folio";
                mpeMensaje.Show();
                return;
            }

            Actualizar_Datos_Folio();

            Inicializar_Datos_Generales();

            GetFolios(Variables.wRef, Convert.ToString(Variables.wSubRef));
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sEstado = ddlEstado.SelectedValue;
            GetMunicipios(sEstado);
        }

        protected void ddlMunicipios_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdFolios_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdFolios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdFolios, "Select$" + e.Row.RowIndex.ToString()) + ";");


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = Unit.Pixel(150);         // Folio
                e.Row.Cells[1].Width = Unit.Pixel(150);         // Equipo
                e.Row.Cells[2].Width = Unit.Pixel(150);         // Marca
                e.Row.Cells[3].Width = Unit.Pixel(150);         // Serie
                e.Row.Cells[4].Width = Unit.Pixel(150);         // Modelo
                e.Row.Cells[5].Width = Unit.Pixel(750);         // Condiciones
                e.Row.Cells[6].Width = Unit.Pixel(150);         // Contraseña
                e.Row.Cells[7].Width = Unit.Pixel(50);          // Modificar
                e.Row.Cells[8].Width = Unit.Pixel(50);          // Eliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[9].Visible = false;     // Folio
                e.Row.Cells[10].Visible = false;    // SubFolio
                e.Row.Cells[11].Visible = false;    // TpoFolio
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[9].Visible = false;     // Folio
                e.Row.Cells[10].Visible = false;    // SubFolio
                e.Row.Cells[11].Visible = false;    // TpoFolio
            }
        }

        protected void GrdFolios_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdFolios_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdFolios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void ImgSubRef_Add_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            TxtEquipo.Text = Server.HtmlDecode(Convert.ToString(GrdFolios.Rows[index].Cells[1].Text));
            TxtMarca.Text = Server.HtmlDecode(Convert.ToString(GrdFolios.Rows[index].Cells[2].Text));
            TxtSerie.Text = Server.HtmlDecode(Convert.ToString(GrdFolios.Rows[index].Cells[3].Text));
            TxtModelo.Text = Server.HtmlDecode(Convert.ToString(GrdFolios.Rows[index].Cells[4].Text));
            TxtCondiciones.Text = Server.HtmlDecode(Convert.ToString(GrdFolios.Rows[index].Cells[5].Text));
            TxtContraseña.Text = Server.HtmlDecode(Convert.ToString(GrdFolios.Rows[index].Cells[6].Text));
            ddlTpoFolio.SelectedValue = Convert.ToString(GrdFolios.Rows[index].Cells[11].Text);

            TxtEquipo.ReadOnly = true;
            TxtMarca.ReadOnly = true;
            TxtSerie.ReadOnly = true;
            TxtModelo.ReadOnly = true;
            TxtCondiciones.ReadOnly = true;
            TxtContraseña.ReadOnly = true;

            ddlTpoFolio.Enabled = false;
            BtnNvoFolio.Enabled = false;

            BtnAnularPnl1.Visible = true;
            btnEditarPnl1.Enabled = true;
            btnActualizarPnl1.Enabled = false;
        }

        protected void ImgSubRef_Del_Click(object sender, ImageClickEventArgs e)
        {

            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            LblMessage_1.Text = "¿Desea eliminar el folio ?";
            mpeMensaje_1.Show();

        }

        protected void BtnAnularPnl1_Click(object sender, EventArgs e)
        {

            TxtEquipo.Text = string.Empty;
            TxtMarca.Text = string.Empty;
            TxtSerie.Text = string.Empty;
            TxtModelo.Text = string.Empty;
            TxtCondiciones.Text = string.Empty;
            TxtContraseña.Text = string.Empty;

            TxtEquipo.ReadOnly = false;
            TxtMarca.ReadOnly = false;
            TxtSerie.ReadOnly = false;
            TxtModelo.ReadOnly = false;
            TxtCondiciones.ReadOnly = false;
            TxtContraseña.ReadOnly = false;

            ddlTpoFolio.Enabled = true;
            ddlTpoFolio.SelectedValue = "0";
            
            BtnNvoFolio.Enabled = true;

            btnEditarPnl1.Visible = true;
            btnEditarPnl1.Enabled = false;

            btnActualizarPnl1.Visible = false;
            BtnAnularPnl1.Visible = false;
        }

        protected void btnEditarPnl1_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl3
            TxtEquipo.ReadOnly = false;
            TxtMarca.ReadOnly = false;
            TxtSerie.ReadOnly = false;
            TxtModelo.ReadOnly = false;
            TxtCondiciones.ReadOnly = false;
            TxtContraseña.ReadOnly = false;
            ddlTpoFolio.Enabled = false;

            BtnNvoFolio.Enabled = false;

            btnEditarPnl1.Visible = false;
            btnActualizarPnl1.Visible = true;
            btnActualizarPnl1.Enabled = true;
            BtnAnularPnl1.Visible = true;
        }

        protected void btnActualizarPnl1_Click(object sender, EventArgs e)
        {
            Actualizar_ITM_77_3();

            // inicializar (Descripcion General)
            Inicializar_Datos_Generales();

            ddlTpoFolio.Enabled = true;
            BtnNvoFolio.Enabled = true;

            btnEditarPnl1.Visible = true;
            btnActualizarPnl1.Visible = false;
            BtnAnularPnl1.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios, correctamente";
            this.mpeMensaje.Show();
        }

        protected void BtnDeclaracion_Click(object sender, EventArgs e)
        {
            try
            {

                Dictionary<string, TextBox> fechas = new Dictionary<string, TextBox>
                    {
                        { "Fecha de Ocurrencia", TxtFechaOcurrencia },
                        { "Fecha de Contacto", TxtFechaContacto }
                    };

                foreach (var fecha in fechas)
                {
                    if (string.IsNullOrEmpty(fecha.Value.Text))
                    {
                        string mensaje = $"Por favor, complete {fecha.Key}.";

                        LblMessage.Text = mensaje;
                        this.mpeMensaje.Show();

                        return;
                    }
                }

                Declaracion_Recoleccion();

                // Crear instancia de la clase EnvioEmail
                EnvioEmail envioEmail = new EnvioEmail();

                string sPredeterminado = string.Empty;

                // Llamar al método CorreoElectronico
                string sEmail = envioEmail.CorreoElectronico(Variables.wRef);
                string sUsuario = envioEmail.CorreoElectronico_User(Variables.wUserName);

                // string Url_OneDrive = @"\Users\Dell\OneDrive - INSURANCE CLAIMS & RISKS MEXICO\";
                Variables.wDesc_Aseguradora = "1.1 AJUSTE SIMPLE\\ZURICH-SANTANDER";

                DateTime fecOcurrencia = DateTime.ParseExact(TxtFechaOcurrencia.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaOcurrencia = fecOcurrencia.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecContacto = DateTime.ParseExact(TxtFechaContacto.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaContacto = fecContacto.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                string sAsunto = "SOLICITUD DE DOCUMENTOS | NOTIFICACIÓN " + TxtNumSiniestro.Text + " | " + TxtNomAsegurado.Text + " | " + Variables.wRef;

                //sPredeterminado = "Estimado Ajustador: \n \n" +
                //                  "Por el presente estamos asignándole el Siniestro Tradicional que nos fue instruido \n" +
                //                  FechaAsignacion + ", cuyos detalles se encuentran a continuación: \n \n" +
                //                  "• " + Variables.wRef + " \n\n";

                sPredeterminado = @"
                                    <html>
                                    <body>
                                        <p>Estimado Ajustador:</p>
                                        <p>
                                            Por el presente estamos asignándole el Siniestro Tradicional que nos fue instruido <br />
                                            " + FechaOcurrencia + @", cuyos detalles se encuentran a continuación:
                                        </p>
                                        <p>
                                            • &nbsp;&nbsp; " + Variables.wRef + @"
                                        </p>
                                    </body>
                                    </html>";

                //sPredeterminado += "1.            DATOS DEL SINIESTRO \n \n" +
                //                   "• Fecha de reporte: " + FechaReporte + " \n" +
                //                   "• Aseguradora: " + TxtSeguro_Cia.Text + " \n" +
                //                   "• Asegurado: " + TxtNomAsegurado.Text + " \n" +
                //                   "• Notificación: " + TxtNumSiniestro.Text + " \n" +
                //                   "• POLIZA: " + TxtNumPoliza.Text + " \n \n";

                sPredeterminado += @"
                                    <html>
                                    <body>
                                        <p><b>1.            DATOS DEL SINIESTRO</b></p>
                                        <p>
                                            • &nbsp;&nbsp; Fecha de Ocurrencia: " + FechaOcurrencia + @"<br />
                                            • &nbsp;&nbsp; Aseguradora: " + TxtSeguro_Cia.Text + @"<br />
                                            • &nbsp;&nbsp; Asegurado: " + TxtNomAsegurado.Text + @"<br />
                                            • &nbsp;&nbsp; Notificación: " + TxtNumSiniestro.Text + @"<br />
                                            • &nbsp;&nbsp; POLIZA: " + TxtNumPoliza.Text + @"<br />
                                        </p>
                                    </body>
                                    </html>";

                //sPredeterminado += "2.            DESCRIPCIÓN DEL EVENTO: \n \n" +
                //                   "• SE DESCONOCE \n \n";

                sPredeterminado += @"
                                    <html>
                                    <body>
                                        <p><b>2.            DESCRIPCIÓN DEL EVENTO:</b></p>
                                        <p>• &nbsp;&nbsp; SE DESCONOCE</p>
                                    </body>
                                    </html>";

                //sPredeterminado += "3.            DATOS DE CONTACTO \n \n" +
                //                   "• " + TxtNomAsegurado.Text + " \n" +
                //                   "• " + TxtTel1_Contacto1.Text + " \n" +
                //                   "• " + TxtEmailContacto1.Text + " \n \n";


                sPredeterminado += @"
                                    <html>
                                    <body>
                                        <p><b>3.            DATOS DE CONTACTO</b></p>
                                        <p>
                                            • &nbsp;&nbsp; " + TxtNomAsegurado.Text + @"<br />
                                            • &nbsp;&nbsp; " + TxtTel1_Contacto1.Text + @"<br />
                                            • &nbsp;&nbsp; " + TxtEmailContacto1.Text + @"<br />
                                        </p>
                                    </body>
                                    </html>";

                //sPredeterminado += "Quedamos atentos a cualquier duda o comentario y nos reiteramos a tus órdenes. \n \n" +
                //                   "Saludos. \n";

                sPredeterminado += @"
                                    <html>
                                    <body>
                                        <p>Quedamos atentos a cualquier duda o comentario y nos reiteramos a tus órdenes.</p>
                                        <p>Saludos.</p>
                                    </body>
                                    </html>";

                string sBody = sPredeterminado;

                // *** Envio de correo electronico ***
                //string Nom_Documento = "CSD_" + Variables.wRef + ".docx";
                //string wAdjunto = Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + Variables.wRef + "\\" + Nom_Documento;

                //envioEmail.EnvioMensaje(TxtEmailContacto1.Text, sAsunto, sBody, wAdjunto);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        private void Declaracion_Recoleccion()
        {
            try
            {
                string plantillaPath = Server.MapPath("~/itnowstorage/DECLARACION_RECOLECCION.docx");

                string sReferencia = Variables.wRef.Replace("/", "-");

                string Nom_Documento = "DR_" + sReferencia + ".docx";
                string documentoGeneradoPath = Server.MapPath("~/itnowstorage/" + Nom_Documento);

                DateTime fecOcurrencia = DateTime.ParseExact(TxtFechaOcurrencia.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaOcurrencia = fecOcurrencia.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecContacto = DateTime.ParseExact(TxtFechaContacto.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaContacto = fecContacto.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecInspeccion = DateTime.ParseExact(TxtFechaInspeccion.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaInspeccion = fecInspeccion.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                string abrev_Colonia = "Colonia";
                string abrev_CodigoP = "C.P.";

                string Dom_Inspeccion = TxtCalle.Text.Trim() + " " + TxtNumExterior.Text.Trim() + " " + TxtNumInterior.Text.Trim() + ", " + abrev_Colonia + " " + TxtColonia.Text.Trim()
                                      + ", " + ddlMunicipios.SelectedItem + ", " + ddlEstado.SelectedItem + ", " + abrev_CodigoP + " " + TxtCodigoPostal.Text.Trim();

                // Declaración de variables
                string Folio_1 = string.Empty, Equipo_1 = string.Empty, Condiciones_1 = string.Empty, Marca_1 = string.Empty, Serie_1 = string.Empty, Modelo_1 = string.Empty, Contraseña_1 = string.Empty;
                string Folio_2 = string.Empty, Equipo_2 = string.Empty, Condiciones_2 = string.Empty, Marca_2 = string.Empty, Serie_2 = string.Empty, Modelo_2 = string.Empty, Contraseña_2 = string.Empty;
                string Folio_3 = string.Empty, Equipo_3 = string.Empty, Condiciones_3 = string.Empty, Marca_3 = string.Empty, Serie_3 = string.Empty, Modelo_3 = string.Empty, Contraseña_3 = string.Empty;
                string Folio_4 = string.Empty, Equipo_4 = string.Empty, Condiciones_4 = string.Empty, Marca_4 = string.Empty, Serie_4 = string.Empty, Modelo_4 = string.Empty, Contraseña_4 = string.Empty;
                string Folio_5 = string.Empty, Equipo_5 = string.Empty, Condiciones_5 = string.Empty, Marca_5 = string.Empty, Serie_5 = string.Empty, Modelo_5 = string.Empty, Contraseña_5 = string.Empty;

                // Obtener Coberturas
                // Iterar sobre las filas del GridView
                for (int i = 0; i < GrdFolios.Rows.Count && i < 5; i++) // Máximo 5 filas
                {
                    GridViewRow row = GrdFolios.Rows[i];

                    // Recuperar valores de las celdas
                    string folio = Server.HtmlDecode(Convert.ToString(row.Cells[0].Text));         // Folio
                    string equipo = Server.HtmlDecode(Convert.ToString(row.Cells[1].Text));         // Equipo
                    string marca = Server.HtmlDecode(Convert.ToString(row.Cells[2].Text));          // Marca
                    string serie = Server.HtmlDecode(Convert.ToString(row.Cells[3].Text));          // Serie
                    string modelo = Server.HtmlDecode(Convert.ToString(row.Cells[4].Text));         // Modelo
                    string condiciones = Server.HtmlDecode(Convert.ToString(row.Cells[5].Text));    // Condiciones
                    string contraseña = Server.HtmlDecode(Convert.ToString(row.Cells[6].Text));     // Contraseña

                    // Asignar valores a las variables según el renglón
                    switch (i)
                    {
                        case 0:
                            Folio_1 = folio;
                            Equipo_1 = equipo;
                            Condiciones_1 = condiciones;
                            Marca_1 = marca;
                            Serie_1 = serie;
                            Modelo_1 = modelo;
                            Contraseña_1 = contraseña;
                            break;
                        case 1:
                            Folio_2 = folio;
                            Equipo_2 = equipo;
                            Condiciones_2 = condiciones;
                            Marca_2 = marca;
                            Serie_2 = serie;
                            Modelo_2 = modelo;
                            Contraseña_2 = contraseña;
                            break;
                        case 2:
                            Folio_3 = folio;
                            Equipo_3 = equipo;
                            Condiciones_3 = condiciones;
                            Marca_3 = marca;
                            Serie_3 = serie;
                            Modelo_3 = modelo;
                            Contraseña_3 = contraseña;
                            break;
                        case 3:
                            Folio_4 = folio;
                            Equipo_4 = equipo;
                            Condiciones_4 = condiciones;
                            Marca_4 = marca;
                            Serie_4 = serie;
                            Modelo_4 = modelo;
                            Contraseña_4 = contraseña;
                            break;
                        case 4:
                            Folio_5 = folio;
                            Equipo_5 = equipo;
                            Condiciones_5 = condiciones;
                            Marca_5 = marca;
                            Serie_5 = serie;
                            Modelo_5 = modelo;
                            Contraseña_5 = contraseña;
                            break;
                    }
                }

                // Copiar la plantilla a un nuevo documento
                System.IO.File.Copy(plantillaPath, documentoGeneradoPath, true);

                // Abrir el documento generado
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documentoGeneradoPath, true))
                {
                    // Obtener el cuerpo del documento
                    DocumentFormat.OpenXml.Wordprocessing.Body body = wordDoc.MainDocumentPart.Document.Body;

                    // Buscar y reemplazar marcadores de posición con datos reales
                    ReplaceText(body, "Fec_Ocurrencia", FechaOcurrencia);
                    ReplaceText(body, "Fec_Contacto", FechaContacto);
                    ReplaceText(body, "Fec_Inspeccion", FechaInspeccion);
                    ReplaceText(body, "Hora_Inspeccion", TxtHoraInspeccion.Text);

                    // ReplaceText(body, "Num_Referencia", TxtSubReferencia.Text);
                    ReplaceText(body, "Nom_Aseguradora", TxtSeguro_Cia.Text);
                    ReplaceText(body, "Num_Siniestro", TxtNumSiniestro.Text);
                    ReplaceText(body, "Nom_Asegurado", TxtNomAsegurado.Text);
                    ReplaceText(body, "Num_Poliza", TxtNumPoliza.Text);
                    // ReplaceText(body, "Num_Reporte", TxtNumReporte.Text);

                    ReplaceText(body, "Num_Telefono", TxtTel1_Contacto1.Text + " | " + TxtTel2_Contacto1.Text);
                    ReplaceText(body, "Correo_Email", TxtEmailContacto1.Text);

                    ReplaceText(body, "Dom_Inspeccion", Dom_Inspeccion);
                    ReplaceText(body, "Tpo_Siniestro", TxtTpoSiniestro.Text);
                    ReplaceText(body, "Detalle_Reporte", TxtDetalleReporte.Text);

                    ReplaceText(body, "Folio_1", Folio_1);
                    ReplaceText(body, "Equipo_1", Equipo_1);
                    ReplaceText(body, "Condiciones_1", Condiciones_1);
                    ReplaceText(body, "Marca_1", Marca_1);
                    ReplaceText(body, "Serie_1", Serie_1);
                    ReplaceText(body, "Modelo_1", Modelo_1);
                    ReplaceText(body, "Contraseña_1", Contraseña_1);

                    ReplaceText(body, "Folio_2", Folio_2);
                    ReplaceText(body, "Equipo_2", Equipo_2);
                    ReplaceText(body, "Condiciones_2", Condiciones_2);
                    ReplaceText(body, "Marca_2", Marca_2);
                    ReplaceText(body, "Serie_2", Serie_2);
                    ReplaceText(body, "Modelo_2", Modelo_2);
                    ReplaceText(body, "Contraseña_2", Contraseña_2);

                    ReplaceText(body, "Folio_3", Folio_3);
                    ReplaceText(body, "Equipo_3", Equipo_3);
                    ReplaceText(body, "Condiciones_3", Condiciones_3);
                    ReplaceText(body, "Marca_3", Marca_3);
                    ReplaceText(body, "Serie_3", Serie_3);
                    ReplaceText(body, "Modelo_3", Modelo_3);
                    ReplaceText(body, "Contraseña_3", Contraseña_3);

                    ReplaceText(body, "Folio_4", Folio_4);
                    ReplaceText(body, "Equipo_4", Equipo_4);
                    ReplaceText(body, "Condiciones_4", Condiciones_4);
                    ReplaceText(body, "Marca_4", Marca_4);
                    ReplaceText(body, "Serie_4", Serie_4);
                    ReplaceText(body, "Modelo_4", Modelo_4);
                    ReplaceText(body, "Contraseña_4", Contraseña_4);
                    
                    ReplaceText(body, "Folio_5", Folio_5);
                    ReplaceText(body, "Equipo_5", Equipo_5);
                    ReplaceText(body, "Condiciones_5", Condiciones_5);
                    ReplaceText(body, "Marca_5", Marca_5);
                    ReplaceText(body, "Serie_5", Serie_5);
                    ReplaceText(body, "Modelo_5", Modelo_5);
                    ReplaceText(body, "Contraseña_5", Contraseña_5);

                    // Guardar los cambios
                    wordDoc.MainDocumentPart.Document.Save();
                }

                LblMessage.Text = "Declaración Recolección, se a generado correctamente";
                this.mpeMensaje.Show();

                Session["sFileName"] = Nom_Documento;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AbrirDescarga", string.Format("window.open('Descargas.aspx');"), true);

            }
            catch (Exception ex)
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

        private void ReplaceTextWithNewLines(DocumentFormat.OpenXml.Wordprocessing.Body body, string placeholder, List<string> values, string fontFamily, int fontSize)
        {
            foreach (var paragraph in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>())
            {
                foreach (var text in paragraph.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
                {
                    if (text.Text.Contains(placeholder))
                    {
                        // Crear un nuevo Run para reemplazar el texto del marcador de posición
                        DocumentFormat.OpenXml.Wordprocessing.Run run = new DocumentFormat.OpenXml.Wordprocessing.Run();

                        // Crear propiedades de estilo para el Run
                        DocumentFormat.OpenXml.Wordprocessing.RunProperties runProperties = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();

                        // Asignar tipo de letra (font family)
                        DocumentFormat.OpenXml.Wordprocessing.RunFonts runFonts = new DocumentFormat.OpenXml.Wordprocessing.RunFonts() { Ascii = fontFamily, HighAnsi = fontFamily };
                        runProperties.Append(runFonts);

                        // Asignar tamaño de fuente
                        DocumentFormat.OpenXml.Wordprocessing.FontSize fontSizeValue = new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = $"{fontSize * 2}" };    // Multiplicar por 2 para el formato de OpenXML
                        runProperties.AppendChild(fontSizeValue);

                        // Añadir las propiedades de estilo al Run
                        run.Append(runProperties);

                        // Añadir cada valor en la lista con un salto de línea
                        foreach (var value in values)
                        {
                            // Crear un nuevo Text para cada valor con su estilo
                            run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(value));
                            run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Break()); // Añadir un salto de línea
                            run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Break()); // Añadir un salto de línea
                        }

                        // Reemplazar el marcador de posición con el nuevo Run
                        text.Text = string.Empty; // Limpiar el texto del marcador
                        text.Parent.InsertAfterSelf(run); // Insertar el nuevo Run después del marcador
                    }
                }
            }
        }


    }
}