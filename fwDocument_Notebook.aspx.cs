using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MySql.Data.MySqlClient;

namespace WebItNow_Peacock
{
    public partial class fwDocument_Notebook : System.Web.UI.Page
    {

        string Url_OneDrive = string.Empty;

        protected void Page_PreInit(object sender, EventArgs e)
        {

            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdUsuario"] == null || Session["UsPassword"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!Page.IsPostBack)
            {
                try
                {
                    // Obtener la Ruta de OneDrive
                    Session["Url_OneDrive"] = Get_Url_OneDrive();

                    DesHabilitar_Controles();

                    Variables.wUserName = Convert.ToString(Session["IdUsuario"]);
                    Variables.wPassword = Convert.ToString(Session["UsPassword"]);
                    Variables.wEmail = Convert.ToString(Session["UsEmail"]);

                    if (Variables.wUserName == "" || Variables.wPassword == "")
                    {
                        Response.Redirect("Login.aspx", true);
                        return;
                    }

                    // Habilitar Controles
                    TxtRef.Enabled = true;
                    ImgBusReference.Enabled = true;

                    GetConclusion();
                    GetRegimen();
                    GetTpoDocumento();

                    GetArchSolicitados(string.Empty, "Referencia");

                    string sReferencia = Request.QueryString["Ref"];
                    string SubReferencia = Request.QueryString["SubRef"];

                    if (sReferencia == string.Empty || sReferencia == null)
                    {
                        return;
                    }

                    int iCreate = Convert.ToInt32(Request.QueryString["Create"]);

                    Variables.wRef = sReferencia;
                    Variables.wSubRef = Convert.ToInt32(SubReferencia);

                    if(sReferencia != string.Empty)
                    {
                        string sValor = string.Empty;

                        if (SubReferencia != "0")
                        {
                            sReferencia += "-" + SubReferencia;
                        }

                        
                        Variables.wCrear = 0;
                        int bAltaAsunto = GetBuscador_ITM_70(sReferencia, "Referencia");

                        if (bAltaAsunto == 1)
                        {
                            GetBuscador(TxtRef.Text, "Referencia");
                            
                            if (Variables.wCrear == 1 && iCreate == 1)
                            {
                                Crear_Cuaderno();
                            }
                        }

                    }
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

        protected void Page_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Variables.wUserName = string.Empty;
            Variables.wPassword = string.Empty;
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
                                  "       t2.Url_Archivo, t2.Nom_Archivo, t2.Fec_Entrega, t2.IdDescarga, t2.IdStatus," +
                                  "       t2.IdSeccion AS IdSeccion, t2.IdCategoria AS IdCategoria " +
                                  "  FROM ITM_03 t0, ITM_88 t1, ITM_47 t2 " +
                                  " WHERE (CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.UsReferencia, '-', t0.SubReferencia) ELSE t0.UsReferencia END) = '" + sValor + "' " +
                                  "   AND t0.UsReferencia = t2.UsReferencia AND t0.SubReferencia = t2.SubReferencia " +
                                  "   AND t1.IdCliente = t2.IdAseguradora AND t1.IdProyecto = t2.IdProyecto AND t1.IdTpoAsunto = t2.IdTpoAsunto " +
                                  "   AND t1.IdSeccion = t2.IdSeccion AND t1.IdCategoria = t2.IdCategoria " +
                                  "   AND t1.IdDocumento = t2.IdDocumento AND t1.IdProyecto = t2.IdProyecto " +
                                  " UNION ALL " +
                                  "SELECT t0.UsReferencia, t0.SubReferencia, t1.TpoArchivo, t1.Descripcion, " +
                                  "  t2.IdConclusion, t2.IdTpoDocumento, t2.IdDocumento, t2.IdUsuario, t2.Id_Directorio, t2.Url_Archivo, t2.Nom_Archivo, " +
                                  "  t2.Fec_Entrega, t2.IdDescarga, t2.IdStatus, t2.IdSeccion AS IdSeccion, t2.IdCategoria AS IdCategoria " +
                                  "  FROM ITM_03 t0, ITM_46 t1, ITM_47 t2 " +
                                  " WHERE (CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.UsReferencia, '-', t0.SubReferencia) ELSE t0.UsReferencia END) = '" + sValor + "' " +
                                  "   AND t0.IdConclusion = t1.IdConclusion " +
                                  "   AND t0.IdConclusion = t2.IdConclusion " +
                                  "   AND t0.UsReferencia = t2.UsReferencia " +
                                  "   AND t0.SubReferencia = t2.SubReferencia " +
                                  "   AND t1.IdAseguradora = t2.IdAseguradora " +
                                  "   AND t1.IdTpoDocumento = t2.IdTpoDocumento " +
                                  "   AND t1.IdDocumento = t2.IdDocumento " +
                                  "   AND t1.IdTpoDocumento IN (1) " +
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

        protected void GetConclusion()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                //string strQuery = "SELECT IdConclusion, Descripcion " +
                //                        " FROM ITM_10 " +
                //                        " WHERE IdStatus = 1 ";

                string strQuery = "SELECT IdDocumento, Descripcion " +
                                        " FROM ITM_83 " +
                                        " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlConclusion.DataSource = dt;

                ddlConclusion.DataValueField = "IdDocumento";
                ddlConclusion.DataTextField = "Descripcion";

                ddlConclusion.DataBind();
                ddlConclusion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
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
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdDocumento, Descripcion " +
                                        " FROM ITM_81 " +
                                        " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlRegimen.DataSource = dt;

                ddlRegimen.DataValueField = "IdDocumento";
                ddlRegimen.DataTextField = "Descripcion";

                ddlRegimen.DataBind();
                ddlRegimen.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetTpoDocumento()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdTpoDocumento, Descripcion " +
                                        " FROM ITM_60 " +
                                        " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlSegmento.DataSource = dt;

                ddlSegmento.DataValueField = "IdTpoDocumento";
                ddlSegmento.DataTextField = "Descripcion";

                ddlSegmento.DataBind();
                ddlSegmento.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void DesHabilitar_Controles()
        {
            TxtRef.Enabled = false;
            ImgBusReference.Enabled = false;

            TxtNomProyecto.Text = string.Empty;
            TxtNomProyecto.Enabled = false;

            TxtNomCliente.Text = string.Empty;
            TxtNomCliente.Enabled = false;

            TxtTpoAsunto.Text = string.Empty;
            TxtTpoAsunto.Enabled = false;

            TxtNomAsegurado.Text = string.Empty;
            TxtNomAsegurado.Enabled = false;

            ddlConclusion.ClearSelection();
            ddlConclusion.Enabled = false;

            ddlRegimen.ClearSelection();
            ddlRegimen.Enabled = false;

            //btnCrearRef.Enabled = false;
            //btnEliminarRef.Enabled = false;

            TxtNomArch.Text = string.Empty;
            TxtNomArch.Enabled = false;

            //oFileArch.Disabled = true;

            BtnCorresp_Solicitados.Enabled = false;

            BtnGenerarArch.Enabled = false;

        }

        protected void BtnGenerarArch_Click(object sender, EventArgs e)
        {
            int iIdProyecto = Variables.wIdProyecto;
            Response.Redirect("fwDocument_Notebook_1.aspx?Ref=" + Variables.wRef + "&SubRef=" + Variables.wSubRef + "&Proyecto=" + iIdProyecto, true);
        }

        protected void ImgBusReference_Click(object sender, ImageClickEventArgs e)
        {
            // inicializar controles
            ddlConclusion.ClearSelection();
            ddlRegimen.ClearSelection();

            TxtNomProyecto.Text = string.Empty;
            TxtNomCliente.Text = string.Empty;
            TxtTpoAsunto.Text = string.Empty;
            TxtNomAsegurado.Text = string.Empty;

            ddlConclusion.Enabled = false;
            ddlRegimen.Enabled = false;

            BtnGenerarArch.Enabled = false;

            GetArchSolicitados(string.Empty, "Referencia");

            if (TxtRef.Text != string.Empty || TxtSiniestro.Text != string.Empty)
            {

                TxtSiniestro.Text = string.Empty;

                mpeBuscador.Show();

                // bAltaAsunto = 0 -- No esta dado de Alta
                // bAltaAsunto = 1 -- Si esta dado de Alta
                int bAltaAsunto = GetBuscador_Referencia();

            }
            else
            {
                // GetArchSolicitados(string.Empty, "Referencia");

                //btnCrearRef.Enabled = false;
                //btnEliminarRef.Enabled = false;
            }
        }

        public int GetBuscador_Referencia()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                //string strQuery = "SELECT CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END AS Referencia, t0.NumSiniestro " +
                //                  "  FROM ITM_70 t0 " +
                //                  " WHERE t0.IdStatus IN (1); ";

                //if (TxtRef.Text != string.Empty)
                //{
                //    strQuery += "   AND (CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END) LIKE '%' + '" + TxtRef.Text.Trim() + "' + '%' ";
                //}
                //if (TxtSiniestro.Text != string.Empty)
                //{
                //    strQuery += "   AND (t0.NumSiniestro LIKE '%' + '" + TxtSiniestro.Text.Trim() + "' + '%' AND t0.NumSiniestro != '')";
                //}

                string strQuery = "SELECT CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END AS Referencia, t0.NumSiniestro " +
                                  "  FROM ITM_70 t0 " +
                                  " WHERE t0.IdStatus IN (1) ";

                if (TxtRef.Text != string.Empty)
                {
                    strQuery += "   AND ( CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END) LIKE CONCAT('%' , '" + TxtRef.Text.Trim() + "' , '%'); ";
                }
                if (TxtSiniestro.Text != string.Empty)
                {
                    strQuery += "   AND ( t0.NumSiniestro LIKE CONCAT ( '%' , '" + TxtSiniestro.Text.Trim() + "' , '%' ) AND t0.NumSiniestro != '')";
                }

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdBuscador.ShowHeaderWhenEmpty = true;
                    GrdBuscador.EmptyDataText = "No hay resultados.";
                }

                GrdBuscador.DataSource = dt;
                GrdBuscador.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdBuscador.HeaderRow.TableSection = TableRowSection.TableHeader;

                dbConn.Close();

                return 1;
            }

            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

            return 0;

        }


        protected void BtnPnlMsgAceptar_Click(object sender, EventArgs e)
        {
            int iDocInterno = 0;

            if (ddlSegmento.SelectedValue == "2")       // CORRESPONDENCIA 
            {
                iDocInterno = 1;
            }

            //if (ddlSegmento.SelectedValue == "0")
            //{
            //    DocumentacionSoporte(iDocInterno);

            //    TxtNomArch.Text = string.Empty;
            //    GetArchSolicitados(TxtRef.Text, "Referencia");
            //}

            DocumentacionSoporte(iDocInterno);

            TxtNomArch.Text = string.Empty;
            GetArchSolicitados(TxtRef.Text, "Referencia");

        }

        protected void DocumentacionSoporte(int iDocInterno)
        {

            string idTpoDoc = string.Empty;
            string sTpoArchivo = "PDF";

            string IdAseguradora = Variables.wPrefijo_Aseguradora;     // Convert.ToInt32(ddlAseguradora.SelectedValue);
            int IdConclusion = Variables.wIdConclusion;             // Convert.ToInt32(ddlConclusion.SelectedValue);
            int IdTpoAsunto = Variables.wIdTpoAsunto; // Convert.ToInt32(ddlTpoAsunto.SelectedValue);

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT MAX(IdDocumento) + 1 IdDocumento FROM ITM_47 t0 " +
                              " WHERE (CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.UsReferencia, '-', t0.SubReferencia) ELSE t0.UsReferencia END) = '" + TxtRef.Text + "'" +
                              "   AND t0.IdAseguradora = '" + IdAseguradora + "' " +
                              "   AND t0.IdConclusion = " + IdConclusion + " " +
                              "   AND t0.IdTpoDocumento = 1";

            MySqlDataReader reader = dbConn.ExecuteReaderQuery(strQuery);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    idTpoDoc = reader["IdDocumento"].ToString().Trim();

                    //if (idTpoDoc.Length < 2)
                    //{
                    //    idTpoDoc = "0" + idTpoDoc;
                    //}
                }
            }

            // Cerrar el MySqlDataReader
            reader.Close();
            // Cerrar la conexión a la base de datos
            dbConn.Close();

            // string pathDir = TxtRef.Text + "_" + TxtNomAsegurado.Text + "\\";

            string pathDir = TxtRef.Text + "\\";
            string Url_Arch = "\\" + "1.- Documentación Soporte";

            try
            {

                // Insertar registro tablas (ITM_46)-(ITM_47)
                string sqlQuery = "INSERT INTO ITM_46 (UsReferencia, SubReferencia, IdDocumento, IdAseguradora, IdConclusion, IdTpoDocumento, TpoArchivo, IdRegimen, IdTpoAsunto, Descripcion, DocInterno, IdStatus) " +
                                  "              VALUES('" + TxtRef.Text + "', " + Variables.wSubRef + ", '" + idTpoDoc + "', '" + IdAseguradora + "', " + IdConclusion + ", 1, 'PDF', NULL, " + IdTpoAsunto + ", '" + TxtNomArch.Text + "', " + iDocInterno + ", 1); ";

                sqlQuery += Environment.NewLine;

                sqlQuery += "INSERT INTO ITM_47 (UsReferencia, SubReferencia, IdProyecto, IdAseguradora, IdConclusion, IdTpoAsunto, IdTpoDocumento, TpoArchivo, IdDocumento, IdUsuario, Id_Directorio, Url_Archivo, Nom_Archivo, Fec_Entrega, Fec_Rechazo, IdDescarga, IdStatus) " +
                            "            VALUES('" + TxtRef.Text + "', " + Variables.wSubRef + ", " + Variables.wIdProyecto + ", '" + IdAseguradora + "', " + IdConclusion + ", " + IdTpoAsunto + ", 1, '" + sTpoArchivo + "', '" + idTpoDoc + "', NULL, 1, '" + Url_Arch + "', NULL, NULL, NULL, 0, 9); ";

                int affectedRows = dbConn.ExecuteNonQuery(sqlQuery);


            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

            // Acceda al archivo usando el nombre del archivo de entrada HTML.
            //HttpPostedFile postedFile = Request.Files["ctl00$MainContent$oFileArch"];

            //if (postedFile.FileName != "")
            //{

            //    string strExtencion = Path.GetExtension(postedFile.FileName).Substring(1);

            //    // string nomFile = postedFile.FileName;
            //    // string sNomFile = TxtRef.Text + "_" + TxtNomAsegurado.Text + "_" + TxtNomArch.Text + "." + strExtencion;
            //    string sNomFile = TxtRef.Text + "_" + TxtNomArch.Text + "." + strExtencion;

            //    Url_OneDrive = Url_OneDrive_Path();
            //    string strFolder = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + pathDir + Url_Arch + "\\");

            //    // Guardar el archivo cargado en el servidor.
            //    string strFilePath = strFolder + sNomFile;

            //    if (File.Exists(strFilePath))
            //    {

            //        LblMessage.Text = "El documento ya existe";
            //        mpeMensaje.Show();

            //        TxtNomArch.Text = string.Empty;
            //        return;
            //    }
            //    else
            //    {
            //        try
            //        {
            //            // se ha subido con éxito.
            //            postedFile.SaveAs(strFilePath);

            //            // Insertar registro tablas (ITM_46)-(ITM_47)
            //            string sqlQuery = "INSERT INTO ITM_46 (UsReferencia, SubReferencia, IdDocumento, IdAseguradora, IdConclusion, IdTpoDocumento, TpoArchivo, IdRegimen, IdTpoAsunto, Descripcion, DocInterno, IdStatus) " +
            //                              "              VALUES('" + TxtRef.Text + "', " + Variables.wSubRef + ", '" + idTpoDoc + "', '" + IdAseguradora + "', " + IdConclusion + ", 1, 'PDF', NULL, " + IdTpoAsunto + ", '" + TxtNomArch.Text + "', " + iDocInterno + ", 1); ";

            //            sqlQuery += Environment.NewLine;

            //            sqlQuery += "INSERT INTO ITM_47 (UsReferencia, SubReferencia, IdProyecto, IdAseguradora, IdConclusion, IdTpoAsunto, IdTpoDocumento, TpoArchivo, IdDocumento, IdUsuario, Id_Directorio, Url_Archivo, Nom_Archivo, Fec_Entrega, Fec_Rechazo, IdDescarga, IdStatus) " +
            //                        "            VALUES('" + TxtRef.Text + "', " + Variables.wSubRef + ", " + Variables.wIdProyecto + ", '" + IdAseguradora + "', " + IdConclusion + ", " + IdTpoAsunto + ", 1, '" + sTpoArchivo + "', '" + idTpoDoc + "', NULL, 1, '" + Url_Arch + "', '" + sNomFile + "', NOW(), NULL, 1, 9); ";

            //            int affectedRows = dbConn.ExecuteNonQuery(sqlQuery);


            //        }
            //        catch (Exception ex)
            //        {
            //            LblMessage.Text = ex.Message;
            //            mpeMensaje.Show();
            //        }

            //    }

            //    dbConn.Close();

            //    //TxtNomArch.Text = string.Empty;
            //    //GetArchConclusion(TxtRef.Text, "Referencia");

            //}   
            //else
            //{
            //    LblMessage.Text = "Debe seleccionar un archivo";
            //    mpeMensaje.Show();
            //    return;
            //}

        }

        protected void BtnClousePnlAdd_Click(object sender, EventArgs e)
        {

        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                int iIdStatus = Convert.ToInt32(GrdArch_Solicitados.Rows[Variables.wRenglon].Cells[13].Text);

                string sDesc_Documento = string.Empty;

                string sReferencia = TxtRef.Text;
                string sNomAsegurado = TxtNomAsegurado.Text;

                if (iIdStatus != 9)
                {
                    sDesc_Documento = Obtener_DescBrev_Doc(Variables.wIdDocumento);
                } else
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

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void btnClose_Proceso_Click(object sender, EventArgs e)
        {

        }

        protected void BtnDocSoporte_Click(object sender, EventArgs e)
        {

        }

        protected void btnCrearRef_Click(object sender, EventArgs e)
        {
            int iOk = Campos_Obligatorios();

            if (iOk != 0)
            {
                return;
            }

            string sReferencia = Variables.wRef;
            int iSubReferencia = Variables.wSubRef;
            string sNomAsegurado = TxtNomAsegurado.Text;
            string IdAseguradora = Variables.wPrefijo_Aseguradora;
            string sEmail = Variables.wEmail;
            int IdConclusion = Convert.ToInt32(ddlConclusion.SelectedValue);
            int IdRegimen = Convert.ToInt32(ddlRegimen.SelectedValue);
            int IdTpoAsunto = Variables.wIdTpoAsunto;
            int IdSituacion = 1;
            int IdStatus = 1;

            int iOk_Personalizacion = Personalizacion_Cuadernos(IdAseguradora, IdConclusion);

            if (iOk_Personalizacion == 0)
            {
                // Inicializar controles
                TxtSiniestro.Text = string.Empty;
                TxtReporte.Text = string.Empty;
                TxtNomProyecto.Text = string.Empty;
                TxtNomCliente.Text = string.Empty;
                TxtTpoAsunto.Text = string.Empty;
                TxtNomAsegurado.Text = string.Empty;

                // TxtTpoAsunto.Visible = false;
                // LblNomCliente.Visible = false;
                // TxtNomCliente.Visible = false;

                ddlConclusion.ClearSelection();
                ddlRegimen.ClearSelection();

                LblMessage.Text = "Es necesario solicitar la personalización del cuaderno";
                mpeMensaje.Show();

                return;
            }

            try
            {
                int iCorrecto = Add_tbReferencia(sReferencia, iSubReferencia, sNomAsegurado, sEmail, IdAseguradora, string.Empty, 0, 0, 3, IdConclusion, IdRegimen, IdTpoAsunto, IdSituacion, IdStatus, "Insert");

                if (iCorrecto == 0)
                {
                    // Agregar registros con los datos de cada cuaderno a cargar
                    Add_tbDetalleCuadernos(sReferencia, iSubReferencia, IdAseguradora, IdConclusion, IdRegimen) ;

                    // string strFolder = Url_OneDrive + TxtRef.Text + "_" + TxtNomAsegurado.Text + "\\";
                    // Variables.wPrefijo_Aseguradora

                    // string nombreAsegurado = TxtNomAsegurado.Text;
                    // nombreAsegurado = nombreAsegurado.Substring(0, 35);

                    Url_OneDrive = Url_OneDrive_Path();
                    // string strFolder = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "_" + nombreAsegurado + "\\");
                    string strFolder = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "\\");

                    // Crear la carpeta si no existe.
                    if (!Directory.Exists(strFolder))
                    {
                        // Crear estructura de Directorios
                        CreateDirectory(strFolder);
                    }

                    // Generar el archivo .TXT
                    Generar_Arch_TXT();

                    GetArchSolicitados(TxtRef.Text, "Referencia");

                    Bitacora_Registrados();

                    // Deshabilitar botones
                    TxtNomAsegurado.Enabled = false;

                    ddlConclusion.Enabled = false;
                    ddlRegimen.Enabled = false;

                    //btnCrearRef.Enabled = false;
                    //btnEliminarRef.Enabled = true;

                    // Habilitar botones
                    TxtNomArch.Enabled = true;
                    //oFileArch.Disabled = false;

                    BtnCorresp_Solicitados.Enabled = true;
                    BtnGenerarArch.Enabled = true;
                }
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


        protected void Generar_Arch_TXT()
        {
            // Definir la ruta y el nombre del archivo
            Url_OneDrive = Url_OneDrive_Path();
            string sDirName = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "\\");

            string fileName = "DatosGenerales.txt";
            string fullPath = Path.Combine(sDirName, fileName);

            // Asegúrese de que el directorio exista
            if (!Directory.Exists(sDirName))
            {
                Directory.CreateDirectory(sDirName);
            }

            // Escribir contenido en el archivo
            using (StreamWriter writer = new StreamWriter(fullPath, false))
            {
                writer.WriteLine("Número de Siniestro : " + TxtSiniestro.Text);
                writer.WriteLine("Número de Reporte   : " + TxtReporte.Text);
                writer.WriteLine("Nombre Asegurado    : " + TxtNomAsegurado.Text);
                writer.WriteLine("Nombre Proyecto     : " + TxtNomProyecto.Text);
                writer.WriteLine("Tipo de Asunto      : " + TxtTpoAsunto.Text);
            }

        }

        public int Campos_Obligatorios()
        {
            try
            {

                if (TxtRef.Text == "" || TxtRef.Text == null)
                {
                    LblMessage.Text = "Capturar la Referencia";
                    mpeMensaje.Show();
                    return -1;
                }
                if (TxtNomAsegurado.Text == "" || TxtNomAsegurado.Text == null)
                {
                    LblMessage.Text = "Capturar Nombre del Asegurado";
                    mpeMensaje.Show();
                    return -1;
                }
                if (ddlConclusion.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar Tipo de Cuaderno";
                    mpeMensaje.Show();
                    return -1;
                }
                if (ddlRegimen.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar Tipo de Asegurado";
                    mpeMensaje.Show();
                    return -1;
                }

                return 0;
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
            finally
            {

            }

            return -1;
        }


        public int Personalizacion_Cuadernos(string IdAseguradora, int IdConclusion)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT COALESCE(COUNT(DISTINCT IdCliente), 0) AS Result " +
                                  "  FROM ITM_88 " +
                                  " WHERE IdCliente = '" + IdAseguradora + "' " +
                                  "   AND IdProyecto = " + Variables.wIdProyecto + "";

                MySqlDataReader reader = dbConn.ExecuteReaderQuery(strQuery);

                if (reader.Read())
                {

                    return reader.GetInt32(0);
                }

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
            finally
            {

            }

            return -1;
        }


        public int Add_tbReferencia(String pReferencia, int pSubReferencia, String pNomAsegurado, String pUsEmail, string pIdAseguradora, String pTelefono, int pProceso, int pSubProceso, int pUsPrivilegios, int pIdConclusion, int pIdRegimen, int pIdTpoAsunto, int pIdSituacion, int pIdStatus, string pStatementType)
        {
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            try
            {

                MySqlCommand cmd = new MySqlCommand("sp_tbRef_Cuadernos", dbConn.Connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@referencia", pReferencia);
                cmd.Parameters.AddWithValue("@subreferencia", pSubReferencia);
                cmd.Parameters.AddWithValue("@email", pUsEmail);
                cmd.Parameters.AddWithValue("@aseguradora", pIdAseguradora);
                cmd.Parameters.AddWithValue("@nomasegurado", pNomAsegurado);
                cmd.Parameters.AddWithValue("@telefono", pTelefono);
                cmd.Parameters.AddWithValue("@proceso", pProceso);
                cmd.Parameters.AddWithValue("@subproceso", pSubProceso);
                cmd.Parameters.AddWithValue("@privilegios", pUsPrivilegios);
                cmd.Parameters.AddWithValue("@conclusion", pIdConclusion);
                cmd.Parameters.AddWithValue("@regimen", pIdRegimen);
                cmd.Parameters.AddWithValue("@asunto", pIdTpoAsunto);
                cmd.Parameters.AddWithValue("@situacion", pIdSituacion);
                cmd.Parameters.AddWithValue("@status", pIdStatus);
                cmd.Parameters.AddWithValue("@StatementType", pStatementType);

                MySqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {

                    return dr.GetInt32(0);

                }

                dbConn.Close();

                return 0;

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

            return -1;
        }


        public void Add_tbDetalleCuadernos(String pReferencia, int pSubReferencia, string pIdAseguradora, int pIdConclusion, int pIdRegimen)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Obtener los registros que corresponden al Tpo. de Cuaderno
                string strQuery = "SELECT t1.IdDoc_Categoria, t1.IdCliente, t1.IdProyecto, t1.IdTpoAsunto, t1.IdSeccion, " +
                                  "       t1.IdCategoria, t1.IdDocumento, t1.NomArchivo, t1.IdUsuario, t1.IdStatus " +
                                  " FROM ITM_88 AS t1, ITM_91 AS t2 " +
                                  " WHERE t2.Referencia = '" + pReferencia + "' " +
                                  "   AND t2.SubReferencia = " + pSubReferencia + " " +
                                  "   AND t1.IdProyecto = " + Variables.wIdProyecto + "" +
                                  "   AND t1.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND t1.IdProyecto = t2.IdProyecto " +
                                  "   AND t1.IdSeccion = t2.IdSeccion " +
                                  "   AND t1.IdCliente = t2.IdCliente " +
                                  "   AND t1.IdCategoria = t2.IdCategoria ";
                // "   AND t1.IdTpoAsunto = t2.IdTpoAsunto ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                foreach (DataRow row in dt.Rows)
                {
                    int IdTpoAsunto = Convert.ToInt32(row[3]);      // ITM_88
                    int IdSeccion = Convert.ToInt32(row[4]);        // ITM_88
                    int IdCategoria = Convert.ToInt32(row[5]);        // ITM_88
                    int IdDocumento = Convert.ToInt32(row[6]);      // ITM_88

                    int IdConclusion = Convert.ToInt32(ddlConclusion.SelectedValue);
                    int IdTpoDocumento = 1;
                    string sTpoArchivo = "PDF";     // PDF, MSG, XML
                    int IdDirectorio = 1;           // Pendiente

                    int IdDescarga = 0;
                    int IdStatus = 1;
                    int IdProyecto = Variables.wIdProyecto;

                    string sqlQuery = "INSERT INTO ITM_47 (UsReferencia, SubReferencia, IdProyecto, IdAseguradora, IdConclusion, IdTpoAsunto, IdTpoDocumento, TpoArchivo, IdSeccion, IdCategoria, IdDocumento, IdUsuario, Id_Directorio, Url_Archivo, Fec_Entrega, Fec_Rechazo, IdDescarga, IdStatus ) " +
                                      "VALUES ('" + pReferencia + "', " + pSubReferencia + ", " + IdProyecto + ", '" + pIdAseguradora + "', " + IdConclusion + ", " + IdTpoAsunto + ", " + IdTpoDocumento + ",  '" + sTpoArchivo + "', " + IdSeccion + ", " + IdCategoria +", " + IdDocumento + ", NULL, " + IdDirectorio + ", NULL, NULL, NULL, " + IdDescarga + ", " + IdStatus + "); ";

                    // Insert en la tabla Estado de Documento
                    // MySqlDataReader reader = dbConn.ExecuteReaderQuery(sqlQuery);
                    int affectedRows = dbConn.ExecuteNonQuery(sqlQuery);
                }

                dbConn.Close();
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


        public string Url_OneDrive_Path()
        {
            try
            {
                string Url_OneDrive = string.Empty;

                switch (TxtTpoAsunto.Text)
                {
                    case "SIMPLE":
                        Url_OneDrive = (string)Session["Url_OneDrive"] + "1.1 AJUSTE SIMPLE" + "\\";

                        if (Variables.wIdTpoProyecto == 1)
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


        public void CreateDirectory(string strFolder)
        {
            try
            {

                // Tabla ITM_51 (Directorios)
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = string.Empty;

                strQuery = "SELECT Nom_Directorio FROM ITM_51 t0 " +
                            " WHERE t0.IdStatus IN (1,2) ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                // string nombreAsegurado = TxtNomAsegurado.Text;
                // nombreAsegurado = nombreAsegurado.Substring(0, 35);

                Url_OneDrive = Url_OneDrive_Path();
                // string sDirName = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "_" + nombreAsegurado + "\\");
                string sDirName = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "\\");

                // crear una matriz de rutas de directorios
                string[] filesArray = new string[dt.Rows.Count];

                int cont = -1;

                foreach (DataRow row in dt.Rows)
                {
                    cont++;
                    filesArray[cont] = sDirName + Convert.ToString(row[0]);
                }

                dbConn.Close();

                // Crear estructura de Directorios
                Directory.CreateDirectory(strFolder);

                for (int i = 0; i < filesArray.Length; i++)
                {
                    string sFolder = filesArray[i];

                    // Crear Directorio
                    Directory.CreateDirectory(sFolder);
                }

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
                return;
            }
        }


        protected void Bitacora_Registrados()
        {
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string sReferencia = TxtRef.Text;
            string sIdUsuario = Variables.wUserName; // LblUsuario.Text;
            string sTipo_Operacion = "REGISTRO";
            string sDet_Operacion = string.Empty;

            // Insertar registro tabla (ITM_65)
            string strQuery = "INSERT INTO ITM_65 (UsReferencia, SubReferencia, IdUsuario, Tipo_Operacion, Fec_Operacion, Det_Operacion) " +
                              "            VALUES ('" + sReferencia + "', " + Variables.wSubRef + ", '" + sIdUsuario + "', '" + sTipo_Operacion + "', NOW(), '" + sDet_Operacion + "')";

            int affectedRows = dbConn.ExecuteNonQuery(strQuery);

            dbConn.Close();
        }


        protected void btnEliminarRef_Click(object sender, EventArgs e)
        {

        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            TxtNomAsegurado.Enabled = true;

            ImgEditar.Visible = false;
            ImgAceptar.Visible = true;
            ImgCancelar.Visible = true;
        }

        protected void ImgAceptar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                // Url_OneDrive = Url_OneDrive_Path();
                // string strFolder = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "\\");

                // string sourceDirName = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "_" + Variables.wNomAsegurado);
                // string destDirName = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "_" + TxtNomAsegurado.Text);

                // Directory.Move(sourceDirName, destDirName);

                TxtNomAsegurado.Enabled = false;
                Variables.wNomAsegurado = TxtNomAsegurado.Text;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Actualizar registro(s) tablas (ITM_70)
                string strQuery = "UPDATE ITM_70 " +
                                  "   SET NomAsegurado = '" + TxtNomAsegurado.Text + "' " +
                                  " WHERE Referencia = '" + TxtRef.Text + "' ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                ImgEditar.Visible = true;
                ImgAceptar.Visible = false;
                ImgCancelar.Visible = false;
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
            finally
            {

            }
        }

        protected void ImgCancelar_Click(object sender, ImageClickEventArgs e)
        {
            TxtNomAsegurado.Text = Variables.wNomAsegurado;
            TxtNomAsegurado.Enabled = false;

            ImgEditar.Visible = true;
            ImgAceptar.Visible = false;
            ImgCancelar.Visible = false;
        }

        protected void ddlAseguradora_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlTpoAsunto_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlConclusion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlRegimen_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnCorresp_Solicitados_Click(object sender, EventArgs e)
        {
            ddlSegmento.SelectedValue = "0";
            mpeAddArchivo.Show();
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
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            Variables.wRenglon = row.RowIndex;

            LblMessage_2.Text = "¿Desea eliminar documento, seleccionado?";
            mpeMensaje_2.Show();
        }

        protected void BtnAceptar_Del_Doc_Click(object sender, EventArgs e)
        {
            try
            {

                int index = Variables.wRenglon;


                string sReferencia = TxtRef.Text;
                // string sDesc_Documento = GrdArch_Obligatorios.Rows[index].Cells[3].Text;
                string sNom_Archivo = GrdArch_Solicitados.Rows[index].Cells[5].Text.Replace("&nbsp;", "").Trim();

                string sUrl_Imagen = Server.HtmlDecode(GrdArch_Solicitados.Rows[index].Cells[7].Text);

                // string nombreAsegurado = TxtNomAsegurado.Text;
                // nombreAsegurado = nombreAsegurado.Substring(0, 35);

                // string sDirName = TxtRef.Text + "_" + nombreAsegurado;
                string sDirName = TxtRef.Text;

                int iIdTpoDocumento = Convert.ToInt32(GrdArch_Solicitados.Rows[index].Cells[9].Text);
                int iIdConclusion = Convert.ToInt32(GrdArch_Solicitados.Rows[index].Cells[10].Text);
                int iIdDocumento = Convert.ToInt32(GrdArch_Solicitados.Rows[index].Cells[11].Text);
                int iIdStatus = Convert.ToInt32(GrdArch_Solicitados.Rows[index].Cells[13].Text);

                string iIdAseguradora = Variables.wPrefijo_Aseguradora;

                if (sNom_Archivo != "")
                {

                    // string strURLFile = Server.MapPath("~/itnowstorage/") + sUrl_Imagen + sNom_Archivo;
                    Url_OneDrive = Url_OneDrive_Path();
                    string strURLFile = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + sDirName + "\\" + sUrl_Imagen + "\\" + sNom_Archivo);

                    // Eliminar el archivo de Server.MapPath("~/itnowstorage/")
                    File.Delete(strURLFile);

                }

                if (iIdStatus != 9)
                {
                    // Actualizar en la tabla [tbArchivosCuadernos] (IdStatus = 0)
                    Update_ITM_47(sReferencia, iIdTpoDocumento, iIdAseguradora, iIdConclusion, iIdDocumento, 0);
                }
                else
                {
                    // Eliminar registro Tablas ITM_46 y ITM_47
                    Delete_Arch_Externo(sReferencia, iIdTpoDocumento, iIdAseguradora, iIdConclusion, iIdDocumento, 0);
                }


                // Refrescar GridView
                GetArchSolicitados(TxtRef.Text, "Referencia");

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnCancelar_Del_Doc_Click(object sender, EventArgs e)
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

        protected void GrdArch_Solicitados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdArch_Solicitados.PageIndex = e.NewPageIndex;
            
            GetArchSolicitados(TxtRef.Text, "Referencia");
        }

        protected void GrdArch_Solicitados_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdArch_Solicitados_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdBuscador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdBuscador.PageIndex = e.NewPageIndex;

                int bAltaAsunto = GetBuscador_Referencia();

            } catch(Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdBuscador_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectBuscador")
            {
                string[] arguments = e.CommandArgument.ToString().Split('|');
                string Referencia = arguments[0];

                int bAltaAsunto = GetBuscador_ITM_70(Referencia, "Referencia");

                if (bAltaAsunto == 1)
                {
                    GetBuscador(TxtRef.Text, "Referencia");
                }

                PnlBuscador.Style["display"] = "none";
                ViewState["PanelVisible"] = false;

                // Mostrar o ocultar el panel según sea necesario
                // PnlBuscador.Visible = false;
            }
        }

        public int GetBuscador_ITM_70(string sValor, string sColumna)
        {
            try
            {
                // DesHabilitar_Controles();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t0.Referencia, t0.NumSiniestro, " +
                                  "  CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END AS Referencia_Sub, " +
                                  "            t0.NomAsegurado, t1.IdSeguros, t0.IdTpoAsunto, t0.IdProyecto,  " +
                                  "       t1.Descripcion AS Desc_IdSeguros, t2.Descripcion AS Desc_TpoAsunto, t3.Descripcion AS NomProyecto, t0.IdProyecto, " +
                                  "       t1.DescripBrev, t0.IdConclusion, t0.IdRegimen, t0.IdTpoProyecto, t0.NumPoliza, t0.NumReporte, t0.SubReferencia, t0.IdStatus " +
                                  "  FROM  ITM_70 t0 " +
                                  "  JOIN ITM_66 t2 ON t0.IdTpoAsunto = t2.IdTpoAsunto " +
                                  "  JOIN ITM_67 t1 ON t0.IdSeguros = t1.IdSeguros " +
                                  "  JOIN ITM_78 t3 ON t0.IdProyecto = t3.IdProyecto " +
                                  " WHERE (CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END) = '" + sValor + "' " +
                                  "   AND t0.IdStatus IN (1)";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    // Inicializar controles
                    TxtRef.Text = string.Empty;
                    TxtSiniestro.Text = string.Empty;
                    TxtReporte.Text = string.Empty;
                    TxtNomProyecto.Text = string.Empty;
                    TxtNomCliente.Text = string.Empty;
                    TxtTpoAsunto.Text = string.Empty;
                    TxtNomAsegurado.Text = string.Empty;

                    ddlConclusion.ClearSelection();
                    ddlRegimen.ClearSelection();

                    ddlConclusion.Enabled = false;
                    ddlRegimen.Enabled = false;

                    //btnCrearRef.Enabled = false;
                    //btnEliminarRef.Enabled = false;

                    BtnCorresp_Solicitados.Enabled = false;

                    LblMessage_1.Text = "La referencia no se encuentra registrada";
                    mpeMensaje_1.Show();

                }
                else
                {

                    TxtRef.Enabled = true;
                    ImgBusReference.Enabled = true;

                    Variables.wCrear = 1;

                    //btnCrearRef.Enabled = true;
                    //btnEliminarRef.Enabled = false;

                    Variables.wRef = dt.Rows[0].ItemArray[0].ToString();
                    Variables.wSubRef = Convert.ToInt32(dt.Rows[0].ItemArray[17].ToString());

                    TxtSiniestro.Text = dt.Rows[0].ItemArray[1].ToString();
                    TxtRef.Text = dt.Rows[0].ItemArray[2].ToString();

                    // Guardar el valor del nombre del asegurado
                    Variables.wNomAsegurado = dt.Rows[0].ItemArray[3].ToString();
                    TxtNomAsegurado.Text = dt.Rows[0].ItemArray[3].ToString();

                    Variables.wPrefijo_Aseguradora = dt.Rows[0].ItemArray[4].ToString();
                    Variables.wIdTpoAsunto = Convert.ToInt32(dt.Rows[0].ItemArray[5].ToString());

                    TxtNomCliente.Text = dt.Rows[0].ItemArray[7].ToString();
                    TxtTpoAsunto.Text = dt.Rows[0].ItemArray[8].ToString();
                    TxtNomProyecto.Text = dt.Rows[0].ItemArray[9].ToString();

                    Variables.wIdProyecto = Convert.ToInt32(dt.Rows[0].ItemArray[10].ToString());
                    Variables.wDesc_Aseguradora = dt.Rows[0].ItemArray[11].ToString();

                    Variables.wIdConclusion = Convert.ToInt32(dt.Rows[0].ItemArray[12].ToString());
                    Variables.wIdRegimen = Convert.ToInt32(dt.Rows[0].ItemArray[13].ToString());
                    Variables.wIdTpoProyecto = Convert.ToInt32(dt.Rows[0].ItemArray[14].ToString());

                    // t0.NumPoliza, t0.NumReporte
                    TxtReporte.Text = dt.Rows[0].ItemArray[16].ToString();

                    ddlConclusion.ClearSelection();
                    ddlRegimen.ClearSelection();

                    ddlConclusion.SelectedValue = Convert.ToString(Variables.wIdConclusion);
                    ddlRegimen.SelectedValue = Convert.ToString(Variables.wIdRegimen);

                    ddlConclusion.Enabled = false;  // true
                    ddlRegimen.Enabled = false;     // true

                    return 1;
                }

                GetArchSolicitados(string.Empty, "Referencia");

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

            return 0;
        }

        protected void GetBuscador(string sValor, string sColumna)
        {
            try
            {
                // DesHabilitar_Controles();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t1.UsReferencia, t1.Aseguradora, t1.UsAsegurado, t1.IdConclusion, t1.IdRegimen, t1.IdTpoAsunto, t1.IdSituacion, t1.IdStatus " +
                                  "  FROM  ITM_03 t1 " +
                                  " WHERE (CASE WHEN t1.SubReferencia >= 1 THEN CONCAT(t1.UsReferencia, '-', t1.SubReferencia) ELSE t1.UsReferencia END) = '" + sValor + "'" +
                                  "   AND t1.IdStatus IN (0, 1)";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 1)
                {
                    if (dt.Rows[0].ItemArray[7].ToString() == "0")
                    {
                        TxtRef.Enabled = true;
                        ImgBusReference.Enabled = true;

                        TxtNomProyecto.Text = string.Empty;
                        TxtNomProyecto.Enabled = false;

                        TxtNomCliente.Text = string.Empty;
                        TxtNomCliente.Enabled = false;

                        TxtTpoAsunto.Text = string.Empty;
                        TxtTpoAsunto.Enabled = false;

                        TxtNomAsegurado.Text = string.Empty;
                        TxtNomAsegurado.Enabled = false;

                        ddlConclusion.ClearSelection();
                        ddlConclusion.Enabled = false;

                        ddlRegimen.ClearSelection();
                        ddlRegimen.Enabled = false;

                        Variables.wCrear = 0;

                        //btnCrearRef.Enabled = false;
                        //btnEliminarRef.Enabled = false;

                        LblMessage.Text = "La referencia ya se encuentra cerrada";
                        mpeMensaje.Show();
                    }
                    else
                    {
                        TxtRef.Enabled = true;
                        ImgBusReference.Enabled = true;

                        Variables.wCrear = 0;

                        //btnCrearRef.Enabled = false;
                        //btnEliminarRef.Enabled = true;

                        TxtNomArch.Enabled = true;
                        //oFileArch.Disabled = false;

                        BtnCorresp_Solicitados.Enabled = true;

                        BtnGenerarArch.Enabled = true;

                        TxtNomAsegurado.Text = dt.Rows[0].ItemArray[2].ToString();

                        // Guardar el valor del nombre del asegurado
                        Variables.wNomAsegurado = dt.Rows[0].ItemArray[2].ToString();

                        ddlConclusion.SelectedValue = dt.Rows[0].ItemArray[3].ToString();
                        ddlRegimen.SelectedValue = dt.Rows[0].ItemArray[4].ToString();

                        GetArchSolicitados(TxtRef.Text, "Referencia");

                    }
                }

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
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

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);

                //DataTable dt = new DataTable();

                //da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    sNom_Directorio = Convert.ToString(row[0]);
                }

                dbConn.Close();

                return sNom_Directorio;

            }
            catch (Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }

            return string.Empty;
        }

        public void Update_ITM_47(string pReferencia, int pIdTpoDocumento, string pIdAseguradora, int pIdConclusion, int pIdDocumento, int piIdDescarga)
        {
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            try
            {
                // Actualizar en la tabla Estado de Documento
                string strQuery = "UPDATE ITM_47 Set IdDescarga = " + piIdDescarga + ", IdUsuario = Null, Url_Archivo = Null, Nom_Archivo = Null, Fec_Entrega = Null, Fec_Rechazo = NOW()" +
                                  " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END)  = '" + pReferencia + "' " +
                                  "   AND IdTpoDocumento = " + pIdTpoDocumento + " And IdAseguradora = '" + pIdAseguradora + "' " +
                                  "   AND IdConclusion = " + pIdConclusion + " And IdDocumento = " + pIdDocumento + "";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {
                dbConn.Close();
            }
        }

        public void Delete_Arch_Externo(string pReferencia, int pIdTpoDocumento, string pIdAseguradora, int pIdConclusion, int pIdDocumento, int piIdDescarga)
        {
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            try
            {
                // Actualizar en la tabla Estado de Documento
                string strQuery = "DELETE FROM ITM_46 " +
                                  " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END)  = '" + pReferencia + "' " +
                                  "   AND IdTpoDocumento = " + pIdTpoDocumento + " And IdAseguradora = '" + pIdAseguradora + "' " +
                                  "   AND IdConclusion = " + pIdConclusion + " And IdDocumento = " + pIdDocumento + "; ";

                       strQuery += "DELETE FROM ITM_47 " +
                                  " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END)  = '" + pReferencia + "' " +
                                  "   AND IdTpoDocumento = " + pIdTpoDocumento + " And IdAseguradora = '" + pIdAseguradora + "' " +
                                  "   AND IdConclusion = " + pIdConclusion + " And IdDocumento = " + pIdDocumento + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {
                dbConn.Close();
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

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Variables.wRef = string.Empty;
            Variables.wSubRef = 0;
            Variables.wIdProyecto = 0;
            Variables.wPrefijo_Aseguradora = string.Empty;
            Variables.wIdTpoAsunto = 0;

            Response.Redirect("fwReporte_Alta_Asunto.aspx", true);
        }

        protected void Crear_Cuaderno()
        {
            //int iOk = Campos_Obligatorios();

            //if (iOk != 0)
            //{
            //    return;
            //}

            string sReferencia = Variables.wRef;
            int iSubReferencia = Variables.wSubRef;
            string sNomAsegurado = TxtNomAsegurado.Text;
            string IdAseguradora = Variables.wPrefijo_Aseguradora;
            string sEmail = Variables.wEmail;
            int IdConclusion = Convert.ToInt32(ddlConclusion.SelectedValue);
            int IdRegimen = Convert.ToInt32(ddlRegimen.SelectedValue);
            int IdTpoAsunto = Variables.wIdTpoAsunto;
            int IdSituacion = 1;
            int IdStatus = 1;

            int iOk_Personalizacion = Personalizacion_Cuadernos(IdAseguradora, IdConclusion);

            if (iOk_Personalizacion == 0)
            {
                // Inicializar controles
                TxtSiniestro.Text = string.Empty;
                TxtReporte.Text = string.Empty;
                TxtNomProyecto.Text = string.Empty;
                TxtNomCliente.Text = string.Empty;
                TxtTpoAsunto.Text = string.Empty;
                TxtNomAsegurado.Text = string.Empty;

                ddlConclusion.ClearSelection();
                ddlRegimen.ClearSelection();

                LblMessage.Text = "Es necesario solicitar la personalización del cuaderno";
                mpeMensaje.Show();

                return;
            }

            try
            {
                int iCorrecto = Add_tbReferencia(sReferencia, iSubReferencia, sNomAsegurado, sEmail, IdAseguradora, string.Empty, 0, 0, 3, IdConclusion, IdRegimen, IdTpoAsunto, IdSituacion, IdStatus, "Insert");

                if (iCorrecto == 0)
                {
                    // Agregar registros con los datos de cada cuaderno a cargar
                    // Add_tbDetalleCuadernos(sReferencia, iSubReferencia, IdAseguradora, IdConclusion, IdRegimen);

                    Url_OneDrive = Url_OneDrive_Path();
                    string strFolder = Server.HtmlDecode(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtRef.Text + "\\");

                    // Crear la carpeta si no existe.
                    if (!Directory.Exists(strFolder))
                    {
                        // Crear estructura de Directorios
                        CreateDirectory(strFolder);
                    }

                    // Generar el archivo .TXT
                    Generar_Arch_TXT();

                    GetArchSolicitados(TxtRef.Text, "Referencia");

                    Bitacora_Registrados();

                    // Deshabilitar botones
                    TxtNomAsegurado.Enabled = false;

                    ddlConclusion.Enabled = false;
                    ddlRegimen.Enabled = false;

                    // Habilitar botones
                    TxtNomArch.Enabled = true;
                    //oFileArch.Disabled = false;

                    BtnCorresp_Solicitados.Enabled = true;
                    BtnGenerarArch.Enabled = true;
                }
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


    }
}