using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MySql.Data.MySqlClient;

namespace WebItNow_Peacock.Landing
{
    public partial class fwLandingDocument : System.Web.UI.Page
    {
        string Url_OneDrive = string.Empty;

        protected void Page_PreInit(object sender, EventArgs e)
        {

            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    // Obtener la Ruta de OneDrive
                    Session["Url_OneDrive"] = Get_Url_OneDrive();

                    Variables.wIdAsunto = Convert.ToInt32(Session["IdAsunto"]);

                    // ✅ Obtener datos del asunto desde ITM_70
                    DataTable dtAsunto = ObtenerDatosAsunto(Variables.wIdAsunto);

                    if (dtAsunto.Rows.Count > 0)
                    {
                        TxtRef.Text = dtAsunto.Rows[0]["Referencia"].ToString();
                        TxtNomCliente.Text = dtAsunto.Rows[0]["Desc_IdSeguros"].ToString();

                        Variables.wPrefijo_Aseguradora = dtAsunto.Rows[0]["IdSeguros"].ToString();
                        Variables.wDesc_Aseguradora = dtAsunto.Rows[0]["DescripBrev"].ToString();
                        Variables.wIdTpoAsunto = Convert.ToInt32(dtAsunto.Rows[0]["IdTpoAsunto"].ToString());
                        Variables.wIdConclusion = Convert.ToInt32(dtAsunto.Rows[0]["IdConclusion"].ToString());
                        Variables.wIdRegimen = Convert.ToInt32(dtAsunto.Rows[0]["IdRegimen"].ToString());


                    }

                    GetArchSolicitados(TxtRef.Text, "Referencia");
                }

                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

            }

            //* * Agrega THEAD y TBODY a GridView.
            GrdArch_Solicitados.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        private DataTable ObtenerDatosAsunto(int idAsunto)
        {
            string strQuery = @"SELECT t0.Referencia, CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END AS Referencia_Sub, " +
                              "        t1.IdSeguros, t0.IdTpoAsunto, t0.IdRegimen, t0.IdConclusion, " +
                              "        t1.Descripcion AS Desc_IdSeguros, t2.Descripcion AS Desc_TpoAsunto, t1.DescripBrev " +
                              "  FROM  ITM_70 t0 " +
                              "  JOIN ITM_66 t2 ON t0.IdTpoAsunto = t2.IdTpoAsunto " +
                              "  JOIN ITM_67 t1 ON t0.IdSeguros = t1.IdSeguros " +
                              " WHERE t0.IdAsunto = @IdAsunto " +
                              "   AND t0.IdStatus IN (1)";


            DataTable dt = new DataTable();

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);

            try
            {
                dbConn.Open();

                using (MySqlCommand cmd = new MySqlCommand(strQuery, dbConn.Connection))
                {
                    cmd.Parameters.AddWithValue("@IdAsunto", idAsunto);
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            finally
            {
                dbConn.Close();
            }

            return dt;
        }

        public void GetArchSolicitados(string sValor, string sColumna)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Documentos conclusion (Cuadernos) = ITM_46
                //string strQuery = "SELECT * FROM ( " +
                //                  "SELECT t0.UsReferencia, t0.SubReferencia, t2.TpoArchivo, t1.NomArchivo AS Descripcion, " +
                //                  "       t2.IdConclusion, t2.IdTpoDocumento, t2.IdDocumento, t2.IdUsuario, t2.Id_Directorio, " +
                //                  "       t2.Url_Archivo, t2.Nom_Archivo, t2.Fec_Entrega, t2.IdDescarga, t2.IdStatus," +
                //                  "       t2.IdSeccion AS IdSeccion, t2.IdCategoria AS IdCategoria " +
                //                  "  FROM ITM_03 t0, ITM_88 t1, ITM_47 t2, ITM_90 t3 " +
                //                  " WHERE (CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.UsReferencia, '-', t0.SubReferencia) ELSE t0.UsReferencia END) = '" + sValor + "' " +
                //                  "   AND t0.UsReferencia = t2.UsReferencia AND t0.SubReferencia = t2.SubReferencia " +
                //                  "   AND t1.IdCliente = t2.IdAseguradora AND t1.IdProyecto = t2.IdProyecto AND t1.IdTpoAsunto = t2.IdTpoAsunto " +
                //                  "   AND t1.IdSeccion = t2.IdSeccion AND t1.IdCategoria = t2.IdCategoria " +
                //                  "   AND t1.IdDocumento = t2.IdDocumento AND t1.IdProyecto = t2.IdProyecto " +
                //                  "   AND t1.IdDocumento = t3.IdConsecutivo AND DocInterno = 0 " +
                //                  " UNION ALL " +
                //                  "SELECT t0.UsReferencia, t0.SubReferencia, t1.TpoArchivo, t1.Descripcion, " +
                //                  "  t2.IdConclusion, t2.IdTpoDocumento, t2.IdDocumento, t2.IdUsuario, t2.Id_Directorio, t2.Url_Archivo, t2.Nom_Archivo, " +
                //                  "  t2.Fec_Entrega, t2.IdDescarga, t2.IdStatus, t2.IdSeccion AS IdSeccion, t2.IdCategoria AS IdCategoria " +
                //                  "  FROM ITM_03 t0, ITM_46 t1, ITM_47 t2 " +
                //                  " WHERE (CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.UsReferencia, '-', t0.SubReferencia) ELSE t0.UsReferencia END) = '" + sValor + "' " +
                //                  "   AND t0.IdConclusion = t1.IdConclusion " +
                //                  "   AND t0.IdConclusion = t2.IdConclusion " +
                //                  "   AND t0.UsReferencia = t2.UsReferencia " +
                //                  "   AND t0.SubReferencia = t2.SubReferencia " +
                //                  "   AND t1.IdAseguradora = t2.IdAseguradora " +
                //                  "   AND t1.IdTpoDocumento = t2.IdTpoDocumento " +
                //                  "   AND t1.IdDocumento = t2.IdDocumento " +
                //                  "   AND t1.IdTpoDocumento IN (1) " +
                //                  "  ) AS OrderedPart " +
                //                  " ORDER BY IdSeccion, IdCategoria ";

                string strQuery = "SELECT t0.UsReferencia, t0.SubReferencia, t2.TpoArchivo, t1.NomArchivo AS Descripcion, " +
                                  "       t2.IdConclusion, t2.IdTpoDocumento, t2.IdDocumento, t2.IdUsuario, t2.Id_Directorio, " +
                                  "       t2.Url_Archivo, t2.Nom_Archivo, t2.Fec_Entrega, t2.IdDescarga, t2.IdStatus," +
                                  "       t2.IdSeccion AS IdSeccion, t2.IdCategoria AS IdCategoria " +
                                  "  FROM ITM_03 t0, ITM_88 t1, ITM_47 t2, ITM_90 t3 " +
                                  " WHERE (CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.UsReferencia, '-', t0.SubReferencia) ELSE t0.UsReferencia END) = '" + sValor + "' " +
                                  "   AND t0.UsReferencia = t2.UsReferencia AND t0.SubReferencia = t2.SubReferencia " +
                                  "   AND t1.IdCliente = t2.IdAseguradora AND t1.IdProyecto = t2.IdProyecto AND t1.IdTpoAsunto = t2.IdTpoAsunto " +
                                  "   AND t1.IdSeccion = t2.IdSeccion AND t1.IdCategoria = t2.IdCategoria " +
                                  "   AND t1.IdDocumento = t2.IdDocumento AND t1.IdProyecto = t2.IdProyecto " +
                                  "   AND t1.IdDocumento = t3.IdConsecutivo AND DocInterno = 0 " +
                                  "   AND t2.IdDescarga = 0" +
                                  " ORDER BY IdSeccion, IdCategoria ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdArch_Solicitados.ShowHeaderWhenEmpty = true;
                    GrdArch_Solicitados.EmptyDataText = "No hay resultados.";
                }

                GrdArch_Solicitados.DataSource = dt;
                GrdArch_Solicitados.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdArch_Solicitados.HeaderRow.TableSection = TableRowSection.TableHeader;

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public string Get_Url_OneDrive()
        {
            string urlOneDrive = string.Empty;
            string strQuery = "SELECT Url_OneDrive FROM ITM_01 WHERE Id = @Valor";

            try
            {
                // Abrir conexión utilizando tu clase personalizada
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                using (MySqlCommand command = new MySqlCommand(strQuery, dbConn.Connection))    // Usar la conexión de dbConn
                {
                    // Agregar el parámetro a la consulta
                    command.Parameters.AddWithValue("@Valor", "01");

                    // Ejecutar la consulta
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        urlOneDrive = result.ToString();
                    }
                }

                dbConn.Close(); // Asegúrate de cerrar la conexión
            }
            catch (Exception ex)
            {
                // Manejo de errores
                urlOneDrive = "Error: " + ex.Message;
            }

            return urlOneDrive;
        }

        protected void ImgAccept_Solicitados_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wDesc_Documento = Server.HtmlDecode(Convert.ToString(GrdArch_Solicitados.Rows[index].Cells[3].Text));
            Variables.wIdTpoDocumento = Convert.ToInt32(GrdArch_Solicitados.Rows[index].Cells[9].Text);
            Variables.wIdConclusion = Convert.ToInt32(GrdArch_Solicitados.Rows[index].Cells[10].Text);
            Variables.wIdDocumento = Convert.ToInt32(GrdArch_Solicitados.Rows[index].Cells[11].Text);

            Variables.wIdCarpeta = Obtener_Carpeta(Convert.ToInt32(GrdArch_Solicitados.Rows[index].Cells[12].Text));
            Variables.wRenglon = index;

            mpeNewEnvio.Show();
        }

        protected void ImgDecline_Solicitados_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected string ObtenerDatosTokens(int wIdAsunto)
        {
            try
            {
                // Variable donde se almacenará el token recuperado de la base de datos.
                string sToken = string.Empty;

                // Crea una nueva instancia de la clase de conexión a MySQL,
                // utilizando las credenciales almacenadas en las variables globales.
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);

                // Abre la conexión a la base de datos.
                dbConn.Open();

                // Variable que contendrá el query SQL a ejecutar.
                string strQuery = string.Empty;

                // Construcción de la consulta SQL:
                // Se obtiene el campo 'Token' de la tabla ITM_TOKENS
                // donde el IdAsunto sea igual al proporcionado y el token aún no haya sido usado (Usado = 0).
                strQuery = "SELECT Token FROM ITM_TOKENS t0 " +
                           " WHERE t0.IdAsunto = " + wIdAsunto + "" +
                           "   AND t0.Usado = 1";

                // Ejecuta la consulta y obtiene los resultados en un DataTable.
                DataTable dt = dbConn.ExecuteQuery(strQuery);

                // Recorre todas las filas devueltas por la consulta.
                foreach (DataRow row in dt.Rows)
                {
                    // Convierte el valor de la primera columna (Token) a string y lo asigna a la variable sToken.
                    // Si hay varios registros, el último valor sobrescribirá los anteriores.
                    sToken = Convert.ToString(row[0]);
                }

                // Cierra la conexión a la base de datos.
                dbConn.Close();

                // Devuelve el token encontrado.
                return sToken;

            }
            catch (Exception ex)
            {
                // Si ocurre un error, muestra el mensaje de la excepción en un label.
                LblMessage.Text = ex.Message;

                // Muestra un modal de mensaje para informar al usuario.
                this.mpeMensaje.Show();
            }
            finally
            {
                // Aquí puedes agregar limpieza de recursos si es necesario.
            }

            // Si no se encontró ningún token o hubo un error, se devuelve un string vacío.
            return string.Empty;
        }

        protected string Obtener_Carpeta(int wIdDirectorio)
        {
            // Tabla ITM_51 (Directorios)
            try
            {

                string sNom_Directorio = string.Empty;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = string.Empty;

                strQuery = "SELECT Nom_Directorio FROM ITM_51 t0 " +
                           " WHERE t0.Id_Directorio = " + wIdDirectorio + "" +
                           "   AND t0.IdStatus IN (1, 2)";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                foreach (DataRow row in dt.Rows)
                {
                    sNom_Directorio = Convert.ToString(row[0]);
                }

                dbConn.Close();

                return sNom_Directorio;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }

            return string.Empty;
        }

        protected void GrdArch_Solicitados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GrdArch_Solicitados_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdArch_Solicitados_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdArch_Solicitados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdArch_Solicitados, "Select$" + e.Row.RowIndex.ToString()) + ";");

            string IdDescarga = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "IdDescarga"));
            string IdStatus = DataBinder.Eval(e.Row.DataItem, "IdStatus")?.ToString();

            if (IdDescarga == "0" && IdStatus == "1")
            {
                (e.Row.FindControl("ImgAccept_Solicitados") as ImageButton).Enabled = true;
                (e.Row.FindControl("ImgDecline_Solicitados") as ImageButton).Enabled = false;
            }
            else if (IdDescarga == "1" && IdStatus == "1")
            {
                (e.Row.FindControl("ImgAccept_Solicitados") as ImageButton).Enabled = false;
                (e.Row.FindControl("ImgDecline_Solicitados") as ImageButton).Enabled = true;
            }
            else if (IdDescarga == "0" || IdDescarga == "1" && IdStatus == "9")
            {
                (e.Row.FindControl("ImgAccept_Solicitados") as ImageButton).Enabled = true;
                (e.Row.FindControl("ImgDecline_Solicitados") as ImageButton).Enabled = true;
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
                e.Row.Cells[12].Visible = false;
                e.Row.Cells[13].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chBoxRow = (CheckBox)e.Row.FindControl("ChBoxRow");
                if (chBoxRow != null)
                {
                    // Añadir atributo onclick para prevenir cambios
                    chBoxRow.Attributes["onclick"] = "return false;";
                }
            }
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCloseWindows_Click(object sender, EventArgs e)
        {
            // Cerrar sesión
            Session.Abandon();
            Session.Clear();


            // ✅ Obtener datos del asunto desde ITM_TOKENS
            string token = ObtenerDatosTokens(Variables.wIdAsunto);

            string baseUrl = System.Configuration.ConfigurationManager.AppSettings["APP_BASE_URL"].TrimEnd('/');
            string link = $"{baseUrl}/fwLandingAcceso.aspx?token={token}";

            // Response.Redirect("fwLandingAcceso.aspx?token=A9F8D6C4B3E21A7C9F6D4B2E1A0C", true); // Página con mensaje de sesión cerrada
            Response.Redirect(link, true); // Página con mensaje de sesión cerrada
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                int iIdStatus = Convert.ToInt32(GrdArch_Solicitados.Rows[Variables.wRenglon].Cells[13].Text);

                string sDesc_Documento = string.Empty;

                string sReferencia = TxtRef.Text;
                // string sNomAsegurado = TxtNomAsegurado.Text;

                if (iIdStatus != 9)
                {
                    sDesc_Documento = Obtener_DescBrev_Doc(Variables.wIdDocumento);
                }
                else
                {
                    sDesc_Documento = Variables.wDesc_Documento;
                }

                // string sDirName = TxtRef.Text + "_" + TxtNomAsegurado.Text;
                string sDirName = TxtRef.Text;

                // int iConclusion = Convert.ToInt32(ddlConclusion.SelectedValue);
                // int iAseguradora = Variables.wIdAseguradora;

                int iConclusion = Variables.wIdConclusion;
                string iAseguradora = Variables.wPrefijo_Aseguradora;
                int iRegimen = Variables.wIdRegimen;      // Convert.ToInt32(ddlRegimen.SelectedValue);

                int iDocumento = Variables.wIdDocumento;

                // Acceda al archivo usando el nombre del archivo de entrada HTML.
                HttpPostedFile postedFile = Request.Files["ctl00$MainContent$oFile"];

                // string nomFile = postedFile.FileName;
                string strExtencion = Path.GetExtension(postedFile.FileName).Substring(1);

                // string nombreAsegurado = TxtNomAsegurado.Text;
                // nombreAsegurado = nombreAsegurado.Substring(0, 35);

                // string sNomFile = TxtRef.Text + "_" + nombreAsegurado + "_" + sDesc_Documento + "." + strExtencion;
                string sNomFile = TxtRef.Text + "_" + sDesc_Documento + "." + strExtencion;

                if (postedFile.FileName != "")
                {
                    this.UploadTo_ITNOW(sNomFile, sDirName, "PDF", iAseguradora, iConclusion, iDocumento, Variables.wIdTpoDocumento);
                }
                else
                {
                    LblMessage.Text = "Debe seleccionar un archivo";
                    mpeMensaje.Show();
                    return;
                }

                GetArchSolicitados(TxtRef.Text, "Referencia");

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
                return;
            }
        }

        public void UploadTo_ITNOW(string sFileName, string sDirName, string sTpoDocumento, string pIdAseguradora, int pIdConclusion, int piDocumento, int pIdTpoDocumento)
        {
            string strFileName;
            string strFilePath;
            string strFolder;
            string strDirectorio;

            strDirectorio = Variables.wIdCarpeta;

            // Acceda al archivo usando el nombre del archivo de entrada HTML.
            HttpPostedFile postedFile = Request.Files["ctl00$MainContent$oFile"];

            // strFolder = Server.MapPath("~/itnowstorage/" + sDirName + "/");

            // Variables.wDesc_Aseguradora = "ZURICH-SANTANDER";

            Url_OneDrive = Url_OneDrive_Path();
            strFolder = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + sDirName + "\\" + strDirectorio + "\\");

            // strFolder = Url_OneDrive + sDirName + "\\";

            // Recuperar el nombre del archivo que se publica.
            strFileName = postedFile.FileName;
            strFileName = Path.GetFileName(strFileName);

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            try
            {
                // Guardar el archivo cargado en el servidor.
                strFilePath = strFolder + sFileName;

                if (File.Exists(strFilePath))
                {
                    LblMessage.Text = "El documento ya existe";
                    mpeMensaje.Show();
                    return;
                }
                else
                {
                    // se ha subido con éxito.
                    postedFile.SaveAs(strFilePath);
                }

                string sUrl_Imagen = "\\" + strDirectorio;
                string sIdUsuario = Variables.wUserName; // LblUsuario.Text;
                string sReferencia = TxtRef.Text;

                // Actualizar la tabla Documentos Archivos
                string strQuery = "UPDATE ITM_47 " +
                                  " SET IdUsuario = '" + sIdUsuario + "', " +
                                  "    Url_Archivo = '" + sUrl_Imagen + "'," +
                                  "    Nom_Archivo = '" + sFileName + "'," +
                                  "    Fec_Entrega = NOW(), " +             //  GETDATE()
                                  "     IdDescarga = 1 " +
                                  " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END)  = '" + sReferencia + "'" +
                                  "   AND IdConclusion = " + pIdConclusion + " " +
                                  "   AND IdAseguradora = '" + pIdAseguradora + "' " +
                                  "   AND IdTpoDocumento = " + pIdTpoDocumento + " " +
                                  "   AND IdDocumento = " + piDocumento + "";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                // dr.Dispose();
                // cmd.Dispose();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {
                dbConn.Close();
            }
        }

        public string Url_OneDrive_Path()
        {
            try
            {
                string Url_OneDrive = string.Empty;

                // string TpoAsunto = "SIMPLE";

                switch (Variables.wIdTpoAsunto)
                {
                    case 1:    // NOTIFICACION
                        Url_OneDrive = (string)Session["Url_OneDrive"];

                        break;

                    case 2:     // SIMPLE
                        Url_OneDrive = (string)Session["Url_OneDrive"] + "1.1 AJUSTE SIMPLE" + "\\";

                        if (Variables.wIdTpoProyecto == 1)
                        {
                            Url_OneDrive = (string)Session["Url_OneDrive"] + "1.1 AJUSTE SIMPLE" + "\\" + "PROYECTOS ESPECIALES" + "\\";
                        }

                        break;

                    case 3:    // COMPLEJO
                        Url_OneDrive = (string)Session["Url_OneDrive"] + "1.2 AJUSTE - COMPLEX" + "\\";

                        if (Variables.wIdTpoProyecto == 1)
                        {
                            Url_OneDrive = (string)Session["Url_OneDrive"] + "1.2 AJUSTE - COMPLEX" + "\\" + "PROYECTOS ESPECIALES" + "\\";
                        }

                        break;

                    case 4:     // LITIGIO
                        Url_OneDrive = (string)Session["Url_OneDrive"] + "2. LITIGIO" + "\\";

                        if (Variables.wIdTpoProyecto == 1)
                        {
                            Url_OneDrive = (string)Session["Url_OneDrive"] + "2. LITIGIO" + "\\" + "PROYECTOS ESPECIALES" + "\\";
                        }

                        break;

                    default:
                        // code block
                        break;
                }

                return Url_OneDrive;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

            return string.Empty;
        }

        public string Obtener_DescBrev_Doc(int sIdDocumento)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT DescripBrev " +
                                  "  FROM ITM_90 " +
                                  " WHERE IdConsecutivo = " + sIdDocumento + " ";

                MySqlDataReader reader = dbConn.ExecuteReaderQuery(strQuery);

                if (reader.Read())
                {

                    return reader.GetString(0);
                }

                dbConn.Close();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

            return string.Empty;
        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }
    }
}