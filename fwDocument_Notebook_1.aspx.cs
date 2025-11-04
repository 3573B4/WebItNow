using System;
using System.IO;
using System.IO.Compression;

using System.Data;
using System.Data.SqlClient;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MySql.Data.MySqlClient;

using iTextSharp.text.pdf;

namespace WebItNow_Peacock
{
    public partial class fwDocument_Notebook_1 : System.Web.UI.Page
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
                Variables.wUserName = Convert.ToString(Session["IdUsuario"]);
                Variables.wPassword = Convert.ToString(Session["UsPassword"]);

                if (Variables.wUserName == "" || Variables.wPassword == "")
                {
                    Response.Redirect("Login.aspx", true);
                    return;
                }

                // Labels
                lblTitulo_Generar_Cuaderno.Text = GetGlobalResourceObject("GlobalResources", "lblTitulo_Generar_Cuaderno").ToString();

                // Obtener la Ruta de OneDrive
                Session["Url_OneDrive"] = Get_Url_OneDrive();

                if (Session["sReferencia"] != null)
                {
                    string sReferencia = (string)Session["sReferencia"];
                    TxtRef.Text = sReferencia;

                }
                else
                {
                    string sReferencia = Request.QueryString["Ref"];
                    string sSubReferencia = Request.QueryString["SubRef"];
                    string sIdProyecto = Request.QueryString["Proyecto"];

                    TxtRef.Text = sReferencia;
                    
                    Variables.wRef = sReferencia;
                    Variables.wSubRef = Convert.ToInt32(sSubReferencia);
                    Variables.wIdProyecto = Convert.ToInt32(sIdProyecto);
                }

                if (Variables.wSubRef != 0)
                {
                    TxtRef.Text += "-" + Variables.wSubRef;
                }
            }

            GetBuscador(TxtRef.Text, "Referencia");


            //* * Agrega THEAD y TBODY a GridView.
            GrdArch_Solicitados.HeaderRow.TableSection = TableRowSection.TableHeader;

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

        protected void GetBuscador(string sValor, string sColumna)
        {
            try
            {
                // DesHabilitar_Controles();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t1.UsReferencia, t1.SubReferencia, t3.Descripcion, t1.UsAsegurado, t2.Descripcion, t4.Descripcion, " +
                                  "       t5.Descripcion, t1.IdSituacion, t3.DescripBrev, t6.Descripcion, t1.IdStatus " +
                                  "  FROM  ITM_03 t1, ITM_83 t2, ITM_48 t3, ITM_49 t4, ITM_66 t5, ITM_78 t6 " +
                                  " WHERE (CASE WHEN t1.SubReferencia >= 1 THEN CONCAT(t1.UsReferencia, '-', t1.SubReferencia) ELSE t1.UsReferencia END) = '" + sValor + "' " +
                                  "   AND t1.Aseguradora = t3.IdSeguros " +
                                  "   AND t1.IdConclusion = t2.IdDocumento " +
                                  "   AND t1.IdRegimen = t4.IdRegimen " +
                                  "   AND t1.IdTpoAsunto = t5.IdTpoAsunto " +
                                  "   AND t6.IdProyecto = " + Variables.wIdProyecto + " " +
                                  "   AND t1.IdStatus = 1";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {

                }
                else
                {

                    Variables.wDesc_Aseguradora = dt.Rows[0].ItemArray[2].ToString();
                    TxtNomAsegurado.Text = dt.Rows[0].ItemArray[3].ToString();

                    // Guardar el valor del nombre del asegurado
                    Variables.wNomAsegurado = dt.Rows[0].ItemArray[3].ToString();

                    TxtConclusion.Text = dt.Rows[0].ItemArray[4].ToString();
                    TxtRegimen.Text = dt.Rows[0].ItemArray[5].ToString();
                    TxtTpoAsunto.Text = dt.Rows[0].ItemArray[6].ToString();
                    TxtNomCliente.Text = dt.Rows[0].ItemArray[8].ToString();
                    TxtNomProyecto.Text = dt.Rows[0].ItemArray[9].ToString();

                    //if (TxtConclusion.Text == "INDEMNIZADO")
                    //{
                    //    // Ocultar Divisiones por nivel.
                    //    DivSoporte.Visible = true;
                    //    GrdArch_Regimen.Visible = true;
                    //    DivBtnRegimen.Visible = true;
                    //}
                    //else
                    //{
                    //    DivSoporte.Visible = false;
                    //    GrdArch_Regimen.Visible = false;
                    //    DivBtnRegimen.Visible = false;
                    //}

                    GetArchSolicitados(TxtRef.Text, "Referencia");

                }

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        public void GetArchSolicitados(string sValor, string sColumna)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Documentos conclusion (Cuadernos) = ITM_46
                string strQuery = "SELECT * FROM ( " +
                                  "SELECT t0.UsReferencia, t0.SubReferencia, t2.TpoArchivo, t1.NomArchivo AS Descripcion, " +
                                  "       t2.IdConclusion, t2.IdTpoDocumento, t2.IdDocumento, t2.IdUsuario, t2.Id_Directorio, " +
                                  "       t2.Url_Archivo, t2.Nom_Archivo, t2.Fec_Entrega, t2.IdDescarga, t2.IdStatus, " +
                                  "       t2.IdSeccion AS IdSeccion, t2.IdCategoria AS IdCategoria " +
                                  "  FROM ITM_03 t0, ITM_88 t1, ITM_47 t2 " +
                                  " WHERE (CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.UsReferencia, '-', t0.SubReferencia) ELSE t0.UsReferencia END) = '" + sValor + "' " +
                                  "   AND t0.UsReferencia = t2.UsReferencia AND t0.SubReferencia = t2.SubReferencia AND t1.IdCliente = t2.IdAseguradora " +
                                  "   AND t1.IdSeccion = t2.IdSeccion AND t1.IdCategoria = t2.IdCategoria " +
                                  "   AND t1.IdDocumento = t2.IdDocumento AND t1.IdProyecto = t2.IdProyecto " +
                                  " UNION " +
                                  "SELECT t0.UsReferencia, t0.SubReferencia, t1.TpoArchivo, t1.Descripcion, " +
                                  "       t2.IdConclusion, t2.IdTpoDocumento, t2.IdDocumento, t2.IdUsuario, t2.Id_Directorio, " +
                                  "       t2.Url_Archivo, t2.Nom_Archivo, t2.Fec_Entrega, t2.IdDescarga, t2.IdStatus, " +
                                  "       t2.IdSeccion AS IdSeccion, t2.IdCategoria AS IdCategoria " +
                                  "  FROM ITM_03 t0, ITM_46 t1, ITM_47 t2 " +
                                  " WHERE (CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.UsReferencia, '-', t0.SubReferencia) ELSE t0.UsReferencia END) = '" + sValor + "'" +
                                  "   AND t0.IdConclusion = t1.IdConclusion" +
                                  "   AND t0.IdConclusion = t2.IdConclusion" +
                                  "   AND t0.UsReferencia = t2.UsReferencia " +
                                  "   AND t0.SubReferencia = t2.SubReferencia" +
                                  "   AND t1.IdTpoDocumento = t2.IdTpoDocumento" +
                                  "   AND t1.IdDocumento = t2.IdDocumento" +
                                  "   AND t1.IdTpoDocumento IN (1)" +
                                  "  ) AS OrderedPart " +
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

        protected void GrdArch_Solicitados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[5].Visible = false;
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
                e.Row.Cells[10].Visible = false;
            }
        }

        protected void BtnArch_ZIP_Click(object sender, EventArgs e)
        {
            if (chkSolicitados.Checked == false)
            {
                string sOpcion = "COMPLETO";
                // Convert_ZIP(sOpcion);

                Convert_ZIP_Archivos(sOpcion, 0);
            }
            else
            {
                CheckBox_True_ZIP(this.Controls);
            }

            // Inicializar CheckBox
            chkSolicitados.Checked = false;
        }

        protected void BtnArch_PDF_Click(object sender, EventArgs e)
        {
            try
            {

                if (chkSolicitados.Checked == false)
                {
                    // Concatenar todos los archivos excepto correspondencia en 1 solo archivo .pdf
                    Convert_PDF("COMPLETO", 0);

                    // Concatenar toda la correspondencia en 1 solo archivo .pdf
                    Convert_PDF_Correspondencia("CORRESPONDENCIA", 0);

                }
                else
                {
                    CheckBox_True_PDF(this.Controls);
                }


                // Inicializar CheckBox
                chkSolicitados.Checked = false;


            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnCloseNotebook_Click(object sender, EventArgs e)
        {
            // Se elimina la Referencia de manera fisica y se mueve a la carpeta elimnados la estructura de Directorios
            try
            {
                string Dir_Cerrados = string.Empty;

                if (Variables.wDesc_Aseguradora == "ZURICH-SANTANDER")
                {
                    Dir_Cerrados = "0-CASOS CERRADOS SANTANDER";
                }
                else if (Variables.wDesc_Aseguradora == "ZURICH")
                {
                    Dir_Cerrados = "0-CASOS CERRADOS ZURICH";
                }
                else if (Variables.wDesc_Aseguradora == "AFIRME")
                {
                    Dir_Cerrados = "0-CASOS CERRADOS AFIRME";
                }

                // Mover carpeta(s) a \\Directorio de Cerrados
                Url_OneDrive = Url_OneDrive_Path();

                string sourceDirectory = Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text ;
                string destinationDirectory = Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + Dir_Cerrados + "\\" + TxtRef.Text ;

                try
                {
                    Directory.Move(sourceDirectory, destinationDirectory);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_03)-(ITM_46_1)-(ITM_47)
                string sqlQuery = "UPDATE ITM_46 " +
                                  "   SET IdStatus = 0 " +
                                  " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END) = '" + TxtRef.Text + "' ;";

                sqlQuery += Environment.NewLine;

                sqlQuery += "UPDATE ITM_47 " +
                            "   SET IdStatus = 0 " +
                            " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END) = '" + TxtRef.Text + "' ;";

                sqlQuery += Environment.NewLine;

                sqlQuery += "UPDATE ITM_03 " +
                            "   SET IdStatus = 0 " +
                            " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END) = '" + TxtRef.Text + "' ;";

                int affectedRows = dbConn.ExecuteNonQuery(sqlQuery);

                dbConn.Close();

                // Registro de cuarderno cerrado en tabla bitacora ITM_65
                Bitacora_Cerrados();

                TxtRef.Text = string.Empty;
                TxtNomAsegurado.Text = string.Empty;
                TxtNomCliente.Text = string.Empty;
                TxtConclusion.Text = string.Empty;
                TxtRegimen.Text = string.Empty;
                TxtTpoAsunto.Text = string.Empty;

                GetArchSolicitados(string.Empty, "Referencia");

                BtnArch_PDF.Enabled = false;
                BtnArch_ZIP.Enabled = false;
                BtnCloseNotebook.Enabled = false;

                LblMessage.Text = "El cuaderno ha sido cerrado correctamente";
                mpeMensaje.Show();

            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Bitacora_Cerrados()
        {
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string sReferencia = TxtRef.Text;
            string sIdUsuario = Variables.wUserLogon; // LblUsuario.Text;
            string sTipo_Operacion = "CERRADO";
            string sDet_Operacion = string.Empty;

            // Insertar registro tabla (ITM_65)
            string sqlQuery = "INSERT INTO ITM_65 (UsReferencia, SubReferencia, IdUsuario, Tipo_Operacion, Fec_Operacion, Det_Operacion) " +
                                "              VALUES('" + sReferencia + "', " + Variables.wSubRef + ", '" + sIdUsuario + "', '" + sTipo_Operacion + "', NOW(), '" + sDet_Operacion + "')";

            int affectedRows = dbConn.ExecuteNonQuery(sqlQuery);

            dbConn.Close();

        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Variables.wIdProyecto = -1;
            //Response.Redirect("fwDocument_Notebook.aspx", true);
            Response.Redirect("fwDocument_Notebook.aspx?Ref=" + Variables.wRef + "&SubRef=" + Variables.wSubRef + "&Create=" + "0", true);
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        public void CheckBox_True_PDF(ControlCollection controles)
        {
            // string y;

            foreach (Control ctrl in controles)
            {
                if (ctrl.GetType() == typeof(CheckBox))
                {
                    using (CheckBox chk = (CheckBox)ctrl)
                    {
                        if (chk.Checked)
                            switch (chk.ID)
                            {
                                case "chkSolicitados":
                                    Convert_PDF("SOLICITADOS", 1);
                                    break;
                                default:
                                    // Statement;
                                    break;
                            }
                    }
                }

                //Esta linea detécta un Control que contenga otros Controles
                //Así ningún control se quedará sin ser limpiado.
                CheckBox_True_PDF(ctrl.Controls);
            }
        }

        public void CheckBox_True_ZIP(ControlCollection controles)
        {
            foreach (Control ctrl in controles)
            {
                if (ctrl.GetType() == typeof(CheckBox))
                {
                    using (CheckBox chk = (CheckBox)ctrl)
                    {
                        if (chk.Checked)
                            switch (chk.ID)
                            {
                                case "chkSolicitados":
                                    Convert_ZIP_Archivos("SOLICITADOS", 1);
                                    break;

                                default:
                                    // Statement;
                                    break;
                            }
                    }
                }

                //Esta linea detécta un Control que contenga otros Controles
                //Así ningún control se quedará sin ser limpiado.
                CheckBox_True_ZIP(ctrl.Controls);
            }
        }

        protected void Convert_ZIP(string sOpcion)
        {
            try
            {
                // sDirName va la Referencia + Nombre Asegurado
                string sDirName = TxtRef.Text + "_" + TxtNomAsegurado.Text;

                string strFolder = string.Empty;
                // Path o ruta donde se va a guardar el archivo con el nombre de Referencia
                string PathZip = string.Empty;

                Url_OneDrive = Url_OneDrive_Path();

                switch (sOpcion)
                {
                    case "SOLICITADOS":
                        strFolder = Server.HtmlDecode(Url_OneDrive + sDirName + "\\" + "1.- Documentación Soporte");
                        PathZip = Server.HtmlDecode(Url_OneDrive + sDirName + "_" + sOpcion + ".zip");
                        break;

                    //case "DOCUMENTACION SOPORTE":
                    //    strFolder = Server.HtmlDecode(Url_OneDrive + sDirName + "\\" + "2.- Documentación Soporte");
                    //    PathZip = Server.HtmlDecode(Url_OneDrive + sDirName + "_" + sOpcion + ".zip");
                    //    break;

                    //case "REGIMEN FISCAL":
                    //    strFolder = Server.HtmlDecode(Url_OneDrive + sDirName + "\\" + "2.- Documentación Soporte");
                    //    PathZip = Server.HtmlDecode(Url_OneDrive + sDirName + "_" + sOpcion + ".zip");
                    //    break;

                    //case "ASEGURADORA":
                    //    strFolder = Server.HtmlDecode(Url_OneDrive + sDirName + "\\" + "2.- Documentación Soporte");
                    //    PathZip = Server.HtmlDecode(Url_OneDrive + sDirName + "_" + sOpcion + ".zip");
                    //    break;

                    //case "COMPLETO":
                    //    strFolder = Server.HtmlDecode(Url_OneDrive + TxtRef.Text + "_" + TxtNomAsegurado.Text);
                    //    PathZip = Server.HtmlDecode(Url_OneDrive + sDirName + "_" + sOpcion + ".zip");
                    //    break;

                    default:
                        // Statement;
                        break;
                }

                // Esta es la clase para que se haga el zip
                ZipFile.CreateFromDirectory(strFolder, PathZip);

                LblMessage.Text = "Se genero archivo .ZIP" /*+ "\n" + PathZip*/;
                mpeMensaje.Show();

            }
            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }
        }

        protected void Convert_ZIP_Archivos(string sOpcion, int iIdTpoDocumento)
        {

            // Concatenar Archivos a .zip
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = string.Empty;

                if (sOpcion == "COMPLETO")
                {
                    strQuery = "SELECT Url_Archivo, Nom_Archivo FROM ITM_47 t0 " +
                               " WHERE t0.UsReferencia = '" + TxtRef.Text + "' " +
                               "   AND t0.Url_Archivo IS NOT NULL";

                }
                else if (sOpcion == "SOLICITADOS" )
                {
                    strQuery = "SELECT Url_Archivo, Nom_Archivo FROM ITM_47 t0 " +
                               " WHERE t0.UsReferencia = '" + TxtRef.Text + "' " +
                               "   AND IdTpoDocumento = " + iIdTpoDocumento + " " +
                               "   AND t0.Url_Archivo IS NOT NULL";
                }

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    dbConn.Close();

                    return;
                }

                Url_OneDrive = Url_OneDrive_Path();
                // string sDirName = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "_" + TxtNomAsegurado.Text);
                string sDirName = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "\\");

                // crear una matriz de rutas de archivos PDF
                string[] filesArray = new string[dt.Rows.Count];

                int cont = -1;

                foreach (DataRow row in dt.Rows)
                {
                    string extension = Path.GetExtension(Convert.ToString(row[1]));

                    cont++;
                    filesArray[cont] = sDirName + Convert.ToString(row[0]) + "\\" + Convert.ToString(row[1]);
                }

                // string startPath = Url_OneDrive + TxtAseguradora.Text + "\\" + TxtRef.Text + "_" + TxtNomAsegurado.Text + "\\" + "5.- Cuadernos" + "\\";
                // string zipFilePath = Server.HtmlDecode(startPath + TxtRef.Text + "_" + TxtNomAsegurado.Text + "_" + sOpcion + ".zip");

                string startPath = Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "\\" + "5.- Cuadernos" + "\\";
                string zipFilePath = Server.HtmlDecode(startPath + TxtRef.Text + "_" + sOpcion + ".zip");

                // Ruta donde se guardará el archivo comprimido vacío
                string rutaArchivoComprimido = zipFilePath;

                // Crear el archivo comprimido vacío
                CrearArchivoComprimidoVacio(rutaArchivoComprimido);

                // Agregar los archivos al archivo ZIP
                using (ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Update))
                {
                    foreach (string filePath in filesArray)
                    {
                        string entryName = Path.GetFileName(filePath);
                        ZipArchiveEntry entry = archive.CreateEntry(entryName);

                        using (FileStream sourceStream = File.OpenRead(filePath))
                        using (Stream entryStream = entry.Open())
                        {
                            sourceStream.CopyTo(entryStream);
                        }
                    }
                }

                dbConn.Close();

                LblMessage.Text = "Se genero archivo .ZIP" /*+ "\n" + PathZip*/;
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

        }

        protected void CrearArchivoComprimidoVacio(string rutaArchivoComprimido)
        {
            // Utiliza un FileStream para crear el archivo comprimido
            using (FileStream archivoComprimido = new FileStream(rutaArchivoComprimido, FileMode.Create))
            {
                // Crea un archivo ZIP vacío
                using (ZipArchive zip = new ZipArchive(archivoComprimido, ZipArchiveMode.Create))
                {
                    // No es necesario agregar ningún archivo
                    // El archivo ZIP estará vacío
                }
            }
        }

        protected void Convert_PDF(string sOpcion, int iIdTpoDocumento)
        {
            // Convertir (concatenar) Archivos de formato .pdf a .pdf
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = string.Empty;

                if (sOpcion == "COMPLETO")
                {
                    strQuery = "SELECT Url_Archivo, Nom_Archivo FROM ITM_47 t0 " +
                               " WHERE t0.UsReferencia = '" + TxtRef.Text + "' " +
                               "   AND TpoArchivo = 'PDF' " +
                               "   AND RIGHT(Nom_Archivo,3) = 'pdf' " +
                               "   AND t0.Url_Archivo IS NOT NULL";

                }
                else if (sOpcion == "SOLICITADOS")
                {
                    strQuery = "SELECT Url_Archivo, Nom_Archivo FROM ITM_47 t0 " +
                               " WHERE t0.UsReferencia = '" + TxtRef.Text + "' " +
                               "   AND IdTpoDocumento = " + iIdTpoDocumento + " " +
                               "   AND TpoArchivo = 'PDF' " +
                               "   AND RIGHT(Nom_Archivo,3) = 'pdf' " +
                               "   AND t0.Url_Archivo IS NOT NULL";
                }
                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    dbConn.Close();

                    return;
                }

                Url_OneDrive = Url_OneDrive_Path();
                // string sDirName = Server.HtmlDecode(Url_OneDrive + TxtAseguradora.Text + "\\" + TxtRef.Text + "_" + TxtNomAsegurado.Text);
                string sDirName = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "\\");


                // crear una matriz de rutas de archivos PDF
                string[] filesArray = new string[dt.Rows.Count];

                int cont = -1;

                foreach (DataRow row in dt.Rows)
                {
                    string extension = (Path.GetExtension(Convert.ToString(row[1])).ToUpper());

                    if (extension == ".PDF")
                    {
                        cont++;
                        filesArray[cont] = sDirName + Convert.ToString(row[0]) + "\\" + Convert.ToString(row[1]);
                    }

                }

                dbConn.Close();

                // crear objeto PdfFileEditor
                // PdfFileEditor pdfEditor = new PdfFileEditor();

                Url_OneDrive = Url_OneDrive_Path();

                // string carpetaSalida = Url_OneDrive + TxtAseguradora.Text + "\\" + TxtRef.Text + "_" + TxtNomAsegurado.Text + "\\" + "5.- Cuadernos" + "\\";
                // string archivoSalida = carpetaSalida + TxtRef.Text + "_" + TxtNomAsegurado.Text + "_" + sOpcion + ".pdf";

                string carpetaSalida = Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "\\" + "5.- Cuadernos" + "\\";
                string archivoSalida = carpetaSalida + TxtRef.Text + "_" + sOpcion + ".pdf";

                using (FileStream fs = new FileStream(archivoSalida, FileMode.Create))
                {
                    iTextSharp.text.Document doc = new iTextSharp.text.Document();

                    PdfCopy copy = new PdfCopy(doc, fs);
                    doc.Open();

                    foreach (string archivo in filesArray)
                    {
                        PdfReader reader = new PdfReader(archivo);
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            PdfImportedPage page = copy.GetImportedPage(reader, i);
                            copy.AddPage(page);
                        }

                        reader.Close();
                    }

                    doc.Close();
                }

                LblMessage.Text = "Se genero archivo .PDF" /*+ "\n" + PathZip*/;
                mpeMensaje.Show();

            }
            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

        }

        protected void Convert_PDF_Correspondencia(string sOpcion, int iIdTpoDocumento)
        {

            try
            {
                // Convertir (concatenar) Archivos de formato .pdf a .pdf

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = string.Empty;

                if (sOpcion == "CORRESPONDENCIA")
                {
                    strQuery = "SELECT Url_Archivo, Nom_Archivo FROM ITM_47 t0 " +
                               " WHERE t0.UsReferencia = '" + TxtRef.Text + "' " +
                               "   AND TpoArchivo = 'MSG' " +
                               "   AND RIGHT(Nom_Archivo,3) = 'pdf' " +
                               "   AND t0.Url_Archivo IS NOT NULL";
                }

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    dbConn.Close();

                    return;
                }

                Url_OneDrive = Url_OneDrive_Path();
                // string sDirName = Server.HtmlDecode(Url_OneDrive + TxtAseguradora.Text + "\\" + TxtRef.Text + "_" + TxtNomAsegurado.Text);
                string sDirName = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "\\");

                // crear una matriz de rutas de archivos PDF
                string[] filesArray = new string[dt.Rows.Count];

                int cont = -1;

                foreach (DataRow row in dt.Rows)
                {
                    string extension = Path.GetExtension(Convert.ToString(row[1]));

                    if (extension == ".pdf" || extension == ".PDF")
                    {
                        cont++;
                        filesArray[cont] = sDirName + Convert.ToString(row[0]) + "\\" + Convert.ToString(row[1]);
                    }

                }

                dbConn.Close();

                // crear objeto PdfFileEditor
                // PdfFileEditor pdfEditor = new PdfFileEditor();

                Url_OneDrive = Url_OneDrive_Path();

                //string carpetaSalida = Url_OneDrive + TxtAseguradora.Text + "\\" + TxtRef.Text + "_" + TxtNomAsegurado.Text + "\\" + "5.- Cuadernos" + "\\";
                //string archivoSalida = carpetaSalida + TxtRef.Text + "_" + TxtNomAsegurado.Text + "_" + sOpcion + ".pdf";

                string carpetaSalida = Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "\\" + "5.- Cuadernos" + "\\";
                string archivoSalida = carpetaSalida + TxtRef.Text + "_" + sOpcion + ".pdf";

                using (FileStream fs = new FileStream(archivoSalida, FileMode.Create))
                {
                    iTextSharp.text.Document doc = new iTextSharp.text.Document();

                    PdfCopy copy = new PdfCopy(doc, fs);
                    doc.Open();

                    foreach (string archivo in filesArray)
                    {
                        PdfReader reader = new PdfReader(archivo);
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            PdfImportedPage page = copy.GetImportedPage(reader, i);
                            copy.AddPage(page);
                        }
                        reader.Close();
                    }

                    doc.Close();
                }

                //// fusionar archivos
                //pdfEditor.Concatenate(filesArray, sNomArchivo);

                LblMessage.Text = "Se genero archivo .PDF" /*+ "\n" + PathZip*/;
                mpeMensaje.Show();

            }
            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

        }

        public string Url_OneDrive_Path()
        {
            try
            {
                string Url_OneDrive = string.Empty;

                switch (TxtTpoAsunto.Text)
                {
                    case "SIMPLE":
                        Url_OneDrive = (string)Session["Url_OneDrive"] + "1.1 AJUSTE SIMPLE" + "\\";

                        if(Variables.wIdTpoProyecto == 1)
                        {
                            Url_OneDrive = (string)Session["Url_OneDrive"] + "1.1 AJUSTE SIMPLE" + "\\" + "PROYECTOS ESPECIALES" + "\\";
                        }

                        break;

                    case "COMPLEJO":
                        Url_OneDrive = (string)Session["Url_OneDrive"] + "1.2 AJUSTE - COMPLEX" + "\\";

                        if (Variables.wIdTpoProyecto == 1)
                        {
                            Url_OneDrive = (string)Session["Url_OneDrive"] + "1.2 AJUSTE - COMPLEX" + "\\" + "PROYECTOS ESPECIALES" + "\\";
                        }

                        break;

                    case "LITIGIO":
                        Url_OneDrive = (string)Session["Url_OneDrive"] + "2. LITIGIO" + "\\";

                        if (Variables.wIdTpoProyecto == 1)
                        {
                            Url_OneDrive = (string)Session["Url_OneDrive"] + "2. LITIGIO" + "\\" + "PROYECTOS ESPECIALES" + "\\";
                        }

                        break;

                    case "NOTIFICACION":
                        Url_OneDrive = (string)Session["Url_OneDrive"];

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

        protected void GrdArch_Solicitados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdArch_Solicitados.PageIndex = e.NewPageIndex;

            GetArchSolicitados(TxtRef.Text, "Referencia");
        }
    }
}