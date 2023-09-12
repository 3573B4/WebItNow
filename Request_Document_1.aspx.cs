using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace WebItNow
{
    public partial class Request_Document_1 : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                TxtReferencia.Text = Convert.ToString(Session["Referencia"]);
                TxtEmail.Text = Convert.ToString(Session["Email"]);
                TxtNom.Text = Convert.ToString(Session["Aseguradora"]);

                // Validar que los documentos existan en la tabla de transacciones
                ValidarTpoDoc_Transaccion();

                // Carga los Tipos de documentos que se pueden anexar a un asunto
                GrdTpoDocumentNuevo();

                getProcesos(Convert.ToInt32(Session["Proceso"]));

                getSubProcesos(Convert.ToInt32(Session["Proceso"]));

                GetTpoDocumento(Convert.ToInt32(Session["Proceso"]), Convert.ToInt32(Session["SubProceso"]));
            }

            //* * Agrega THEAD y TBODY a GridView.
            GrdTpoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;
            //* * Agrega THEAD y TBODY a GridView.
            GrdTpoDocumentNew.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Request_Document.aspx", true);
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            if (ValidarCheckBox())
            {
                // Leer datos del gridview

                foreach (GridViewRow row in GrdTpoDocumento.Rows)
                {
                    // IdStatus
                    int valorcol0;

                    string sTpoDocumento = row.Cells[2].Text;
                    string sDescripcion =  row.Cells[3].Text;
                    string sDescrpBrev = row.Cells[4].Text;
                    int sIdProceso = Convert.ToInt32(row.Cells[5].Text);
                    int sIdSubProceso = Convert.ToInt32(row.Cells[6].Text);

                    var chkbox = row.FindControl("chkTpoDocumento") as CheckBox;
                    if (chkbox.Checked == true)
                    {
                        valorcol0 = 1;
                    }
                    else
                    {
                        valorcol0 = 0;
                    }

                    int Referencia_Existe = ValidaExistTransaccion(TxtReferencia.Text, sTpoDocumento, sIdProceso, sIdSubProceso);

                    string sqlQuery = string.Empty;

                    if (Referencia_Existe == 0)
                    {
                        // Insertar tablas ITM_04, ITM_06
                        Insert_Row_ITM_04(sTpoDocumento);

                        // Insert_Row_ITM_06(sTpoDocumento, sDescripcion, sDescrpBrev, sIdProceso, sIdSubProceso);

                        // Insertar tablas ITM_15
                        Insert_Row_ITM_15(valorcol0,sTpoDocumento,sIdProceso, sIdSubProceso);
                    }
                    else
                    {
                        Update_Row_ITM_15(valorcol0, sTpoDocumento, sIdProceso, sIdSubProceso);
                    }

                }

                //string sAsunto = "IMPORTANTE: Se solicita documentación para la atención de su reporte " + TxtReferencia.Text;

                //string sBody = "Estimado Cliente. \n Enviamos este mensaje para hacer de su conocimiento que es necesario nos remita a la brevedad, \n " +
                //    "la documentación correspondiente al reporte: " + TxtReferencia.Text + " \n " +
                //    "Para completar este proceso es necesario ingresar a nuestro sitio de carga segura de información. Dando clic a continuación: \n " +
                //    "bit.ly/3n1Ck2q \n" +
                //    "En caso de cualquier duda, puede ingresar a este vínculo, donde podrá buscar y encontrar lo que requiera: \n " +
                //    "https://peacock.zendesk.com/hc/es-419" + " \n " + " \n ";
                //sBody += "Alternativamente, puede contactarnos en cualquiera de los siguientes medios, donde con gusto lo atenderemos: \n " +
                //    "* Asistente virtual en " + "www.peacock.claims \n " +
                //    "* WhatsApp: + 52 55-9035-4806 \n " +
                //    "* Correo electrónico: " + "contacto@peacock.claims \n " +
                //    "* Vía Teléfono: + 5255-8525-7200 y +52 55-8932-4700 \n " + " \n " +
                //    "Agradecemos de antemano su confianza y preferencia. Esperamos que su experiencia de servicio sea satisfactoria.";

                //var email = new EnvioEmail();
                //int Envio_Ok = email.EnvioMensaje(TxtReferencia.Text.Trim(), TxtEmail.Text.Trim(), sAsunto, sBody);

                Session["Asunto"] = "Solicitud Documento";
                Session["Referencia"] = TxtReferencia.Text;
                Session["Email"] = TxtEmail.Text.Trim();

                Response.Redirect("Page_Message.aspx");

                Limpia(this.Controls);
                Lbl_Message.Visible = false;

            }
            else
            {
                LblMessage.Text = "Debe seleccionar minimo 1 documento";
                this.mpeMensaje.Show();
            }
        }

        public void OnTextChanged(object sender, EventArgs e)
        {
            Variables.wPrivilegios = System.Web.HttpContext.Current.Session["UsPrivilegios"] as string;
            //Reference the TextBox.
            TextBox textBox = sender as TextBox;

            if (TxtReferencia.Text != "")
            {

                Lbl_Message.Visible = true;

                // Insertar Registo Usuario Cargas
                int result = ValidarReferencia(TxtReferencia.Text, Int32.Parse(Variables.wPrivilegios));

                if (result == 0)
                {
                    TxtReferencia.Text = string.Empty;
                    TxtEmail.Text = string.Empty;
                    TxtNom.Text = string.Empty;

                    LblMessage.Text = "Ingrese una referencia valida";
                    this.mpeMensaje.Show();
                }
                else
                {
                    Lbl_Message.Text = "";
                    Lbl_Message.Visible = false;
                    TxtReferencia.Focus();
                }

            }
            else
            {
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "* Este campo es obligatorio";
            }

        }

        public int ValidarReferencia(String pReferencia, int pUsPrivilegios)
        {

            try
            {

                ConexionBD Conecta = new ConexionBD();
                NewMethod(Conecta);

                // Consulta a la tabla : Usuarios = ITM_02
                string strQuery = "SELECT UsReferencia, UsEmail, UsAsegurado " +
                                  "  FROM ITM_02 ed " +
                                  " WHERE UsReferencia LIKE '%' + '" + pReferencia + "'  + '%' ";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    TxtEmail.Text = Convert.ToString(row[1]);
                    TxtNom.Text = Convert.ToString(row[2]);

                    return 1;
                }

                cmd.Dispose();
                Conecta.Cerrar();

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

            return 0;
        }

        public int Add_Solicitud(String pNombre, String pUsEmail, String pReferencia, String pTpoDocumento)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbSolicitud", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@nombre", pNombre);
                cmd1.Parameters.AddWithValue("@email", pUsEmail);
                cmd1.Parameters.AddWithValue("@referencia", pReferencia);
                cmd1.Parameters.AddWithValue("@TpoDocumento", pTpoDocumento);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                if (dr1.Read())
                {

                    return dr1.GetInt32(0);

                }

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

                return 0;

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

            return -1;
        }

        public void Limpia(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
                else if (control is DropDownList)
                    ((DropDownList)control).Items.Clear();
                else if (control is RadioButtonList)
                    ((RadioButtonList)control).ClearSelection();
                else if (control is CheckBoxList)
                    ((CheckBoxList)control).ClearSelection();
                else if (control is RadioButton)
                    ((RadioButton)control).Checked = false;
                else if (control is CheckBox)
                    ((CheckBox)control).Checked = false;
                else if (control.HasControls())
                    //Esta linea detécta un Control que contenga otros Controles
                    //Así ningún control se quedará sin ser limpiado.
                    Limpia(control.Controls);
            }

        }

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }

        public void GetTpoDocumento(int iIdProceso, int iIdSubProceso)
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            // Validar si la referencia ya tiene transacciones

            int Referencia_Existe = ValReferenciaTransaccion(TxtReferencia.Text);
            string sqlQuery = string.Empty;

            if (Referencia_Existe == 0)
            {
                // Consulta a la tabla Tipo de Documento
                sqlQuery = "Select IdTpoDocumento, Descripcion, DescripBrev, IdProceso, IdSubProceso, 'false' as IdStatus " +
                           "  From ITM_06 " +
                           " Where IdProceso = " + iIdProceso + "" +
                           "   And IdSubProceso = " + iIdSubProceso + "" +
                           "   And IdStatus = '1'";
            }
            else
            {
                //sqlQuery = "Select tp.IdTpoDocumento, tp.Descripcion,  DescripBrev, tp.IdProceso, tp.IdSubProceso, tr.IdStatus " +
                //            "  From ITM_06 tp, ITM_15 tr " +
                //            " Where tp.IdTpoDocumento = tr.IdTpoDocumento And tp.IdProceso = tr.IdProceso And tp.IdSubProceso = tr.IdSubProceso" +
                //            "   And tp.IdProceso = " + iIdProceso + "" +
                //            "   And tp.IdSubProceso = " + iIdSubProceso + "" +
                //            "   And tr.Referencia = '" + TxtReferencia.Text + "'" +
                //            "   And tp.IdStatus = '1'";

                sqlQuery = "Select tp.IdTpoDocumento, tp.Descripcion,  tp.DescripBrev, tr.IdProceso, tr.IdSubProceso, tr.IdStatus " +
                           "  From ITM_08 tp, ITM_15 tr" +
                           " Where tp.IdTpoDocumento = tr.IdTpoDocumento" +
                           "   And tr.Referencia = '" + TxtReferencia.Text + "'";

            }

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                GrdTpoDocumento.ShowHeaderWhenEmpty = true;
                GrdTpoDocumento.EmptyDataText = "No hay resultados.";
            }

            Session["data"] = dt;

            GrdTpoDocumento.DataSource = dt;
            GrdTpoDocumento.DataBind();

            //* * Agrega THEAD y TBODY a GridView.
            GrdTpoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;

            Conecta.Cerrar();

        }

        public void GrdTpoDocumentNuevo()
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            // Consulta a la tabla Tipo de Documento
            string sqlQuery = "Select IdTpoDocumento, Descripcion, DescripBrev " +
                              "  From ITM_08 " +
                              " Where IdStatus = '1'";

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                GrdTpoDocumentNew.ShowHeaderWhenEmpty = true;
                GrdTpoDocumentNew.EmptyDataText = "No hay resultados.";
            }

            GrdTpoDocumentNew.DataSource = dt;
            GrdTpoDocumentNew.DataBind();

            //* * Agrega THEAD y TBODY a GridView.
            GrdTpoDocumentNew.HeaderRow.TableSection = TableRowSection.TableHeader;

            Conecta.Cerrar();
        }

        public int ValidaExistTransaccion(String pReferencia, string pTpoDocumento, int pIdProceso, int pIdSubproceso)
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbExisteTransaccion", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@referencia", pReferencia);
                cmd1.Parameters.AddWithValue("@tpodocumento", pTpoDocumento);
                cmd1.Parameters.AddWithValue("@idproceso", pIdProceso);
                cmd1.Parameters.AddWithValue("@idsubproceso", pIdSubproceso);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                if (dr1.Read())
                {

                    return dr1.GetInt32(0);

                }

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

                return 0;

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

            return -1;
        }

        public int ValReferenciaTransaccion(String pReferencia)
        {

            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbValidaTransaccion", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@referencia", pReferencia);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                if (dr1.Read())
                {

                    return dr1.GetInt32(0);

                }

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

                return 0;

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

            return -1;
        }

        protected void getProcesos(int iProceso)
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            string sqlQuery = "SELECT IdProceso, Nombre " +
                                    " FROM ITM_16 " +
                                    " WHERE IdProceso = " + iProceso + "" +
                                    "   AND IdStatus = 1 ";

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);
            SqlDataReader dt = cmd.ExecuteReader();

            if (dt.HasRows)
            {
                while (dt.Read())
                {
                    TxtProceso.Text = dt["Nombre"].ToString().Trim();
                }
            }

            //ddlProceso.DataSource = dt;

            //ddlProceso.DataValueField = "IdProceso";
            //ddlProceso.DataTextField = "Nombre";

            //ddlProceso.DataBind();
            //ddlProceso.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

        }

        protected void getSubProcesos(int iProceso)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdSubProceso, Descripcion " +
                                    " FROM ITM_14 " +
                                    " WHERE IdProceso = " + iProceso;


                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);
                SqlDataReader dt = cmd.ExecuteReader();

                if (dt.HasRows)
                {
                    while (dt.Read())
                    {
                        TxtSubProceso.Text = dt["Descripcion"].ToString().Trim();
                    }
                }
                //ddlSubProceso.DataSource = dt;

                //ddlSubProceso.DataValueField = "IdSubProceso";
                //ddlSubProceso.DataTextField = "Descripcion";

                //ddlSubProceso.DataBind();
                //ddlSubProceso.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public bool ValidarCheckBox()
        {
            int cont = 0;

            foreach (GridViewRow row in GrdTpoDocumento.Rows)
            {
                var chkbox = row.FindControl("chkTpoDocumento") as CheckBox;
                if (chkbox.Checked == true)
                {
                    cont++;
                }
            }

            if (cont >= 1)
            {
                return true;
            }

            return false;

        }

        public void Insert_Row_ITM_04(string valorcol1)
        {
            try
            {

                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string sReferencia = TxtReferencia.Text;

                // Insertar en la tabla Detalle Referencias ó Asuntos ITM_04
                string sqlQuery = "INSERT INTO ITM_04 (Referencia, IdTipoDocumento, IdStatus, IdDescarga ) " +
                                             " VALUES ('" + sReferencia + "', '" + valorcol1 + "', 0, 0)";

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);
                SqlDataReader dt = cmd.ExecuteReader();

                Conecta.Cerrar();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        public void Insert_Row_ITM_06(string valorcol1, string valorcol2, string valorcol3, int valorcol4, int valorcol5)
        {
            try
            {

                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string sReferencia = TxtReferencia.Text;

                // Insertar en la tabla de Transacciones ITM_06
                string sqlQuery = "INSERT INTO ITM_06 (IdTpoDocumento, Descripcion, DescripBrev, IdStatus, IdProceso, IdSubProceso)" +
                              "VALUES ('" + valorcol1 + "', '" + valorcol2 + "', '" + valorcol3 + "', 1, " + valorcol4 + ", " + valorcol5 + ")";

                sqlQuery += Environment.NewLine;

                // Insertar en la tabla Detalle Referencias ó Asuntos ITM_04
                sqlQuery += "INSERT INTO ITM_04 (Referencia, IdTipoDocumento, IdStatus, IdDescarga ) " +
                                             " VALUES ('" + sReferencia + "', '" + valorcol1 + "', 0, 0)";

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);
                SqlDataReader dt = cmd.ExecuteReader();

                Conecta.Cerrar();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        public void Insert_Row_ITM_15(int valorcol0, string valorcol1, int valorcol3, int valorcol4)
        {
            // DBCC CHECKIDENT (ITM_15, RESEED, 0) -- Inicializar el campo [IdTransaccion] = 0
            string sReferencia = TxtReferencia.Text;
            //bool valorcol0 = false;
            //int valorcol0;

            // Insertar tabla ITM_15
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                // Insertar en tabla ITM_15
                SqlCommand cmd1 = new SqlCommand("sp_tbTransaccion", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@idstatus", valorcol0);
                cmd1.Parameters.AddWithValue("@referencia", sReferencia);
                cmd1.Parameters.AddWithValue("@idtpodocumento", valorcol1);
                cmd1.Parameters.AddWithValue("@idproceso", valorcol3);
                cmd1.Parameters.AddWithValue("@idsubproceso", valorcol4);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                if (dr1.Read())
                {

                    // return dr1.GetInt32(0);

                }

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

                //return 0;

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

            //foreach (GridViewRow row in GrdTpoDocumento.Rows)
            //{

            //    //var chkbox = row.FindControl("chkTpoDocumento") as CheckBox;
            //    //if (chkbox.Checked == true)
            //    //{
            //    //    valorcol0 = 1;
            //    //}
            //    //else
            //    //{
            //    //    valorcol0 = 0;
            //    //}

            //    //string valorcol1 = row.Cells[1].Text;
            //    //string valorcol2 = row.Cells[2].Text;
            //    //string valorcol3 = row.Cells[3].Text;
            //    //string valorcol4 = row.Cells[4].Text;

            //}
        }

        public void Update_Row_ITM_15(int valorcol0, string valorcol1, int valorcol3, int valorcol4)
        {
            // Inicializar tabla ITM_15
            // DBCC CHECKIDENT (ITM_15, RESEED, 0) -- Inicializar el campo [IdTransaccion] = 0

            string sReferencia = TxtReferencia.Text;
            // bool valorcol0 = false;
            // int valorcol0;

            // Insertar tabla ITM_15
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                string strQuery = "Update ITM_15 " +
                                   "   Set IdStatus = " + valorcol0 + "" +
                                   " Where Referencia = '" + sReferencia + "'" +
                                   "   And IdTpoDocumento = '" + valorcol1 + "'" +
                                   "   And IdProceso = " + valorcol3 + "" +
                                   "   And IdSubProceso = " + valorcol4;

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                cmd.ExecuteReader();

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

            //foreach (GridViewRow row in GrdTpoDocumento.Rows)
            //{

            //    var chkbox = row.FindControl("chkTpoDocumento") as CheckBox;
            //    if (chkbox.Checked == true)
            //    {
            //        valorcol0 = 1;
            //    }
            //    else
            //    {
            //        valorcol0 = 0;
            //    }

            //    string valorcol1 = row.Cells[2].Text;
            //    string valorcol2 = row.Cells[4].Text;
            //    string valorcol3 = row.Cells[5].Text;

            //}
        }

        public void ValidarTpoDoc_Transaccion()
        {
            string sqlQuery = string.Empty;
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                sqlQuery = "SELECT td.IdTpoDocumento,td.IdProceso, td.IdSubProceso " +
                           "  FROM [dbo].[ITM_06] AS td " +
                           " WHERE NOT EXISTS (SELECT td.IdTpoDocumento,td.IdProceso, td.IdSubProceso " +
                           "  FROM [dbo].[ITM_15] AS tr WHERE td.IdTpoDocumento = tr.IdTpoDocumento)";

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);
                SqlDataReader dt = cmd.ExecuteReader();

                if (dt.HasRows)
                {
                    while (dt.Read())
                    {
                        string sTpoDocumento = dt["IdTpoDocumento"].ToString().Trim();
                        string iIdProceso = dt["IdProceso"].ToString().Trim();
                        string iIdSubProceso = dt["IdSubProceso"].ToString().Trim();

                        if ( Convert.ToString(Session["Proceso"]) == iIdProceso)
                        {
                            Insert_Transaccion(sTpoDocumento, iIdProceso, iIdSubProceso);
                        }

                    }
                }

                Conecta.Cerrar();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();

            }
        }

        public void Insert_Transaccion(string sTpoDocumento, string iIdProceso, string iIdSubProceso)
        {
            int iIdStatus = 0;
            int iIdDescarga = 0;

            try
            {

                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string sReferencia = TxtReferencia.Text;

                // Insertar en la tabla de Transacciones ITM_15
                string sqlQuery = "INSERT INTO ITM_15 (IdStatus, Referencia, IdTpoDocumento, IdProceso, IdSubProceso) " +
                              "VALUES (0, '" + sReferencia + "', '" + sTpoDocumento + "', " + iIdProceso + ", " + iIdSubProceso + ") ";

                sqlQuery += Environment.NewLine;

                // Insertar en la tabla Detalle Referencias ó Asuntos ITM_04
                sqlQuery += "INSERT INTO ITM_04 (Referencia, IdTipoDocumento, IdStatus, IdDescarga ) " +
                                             " VALUES ('" + sReferencia + "', '" + sTpoDocumento + "', " + iIdStatus + ", " + iIdDescarga + ")";

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);
                SqlDataReader dt = cmd.ExecuteReader();

                Conecta.Cerrar();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

        }

        protected void GrdTpoDocumentNew_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdTpoDocumentNew, "Select$" + e.Row.RowIndex.ToString()) + ";");
        }

        protected void ImgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            bool bExiste = false;

            try
            {
                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
                int index = row.RowIndex;

                string sTipoDocumento = GrdTpoDocumentNew.Rows[index].Cells[1].Text;
                string sDescripcion = Server.HtmlDecode(GrdTpoDocumentNew.Rows[index].Cells[2].Text);
                string sDescrpBrev = Server.HtmlDecode(GrdTpoDocumentNew.Rows[index].Cells[3].Text);
                int iProceso = Convert.ToInt32(Session["Proceso"]);
                int iSubProceso = Convert.ToInt32(Session["SubProceso"]);
            //  int iIdStatus = 1;
                bool bIdStatus = true;
                DataTable dt = (DataTable)Session["data"];

                DataColumnCollection columns = dt.Columns;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (sTipoDocumento == dt.Rows[i]["IdTpoDocumento"].ToString())
                    {
                        bExiste = true;
                    }
                }

                if (!bExiste)
                {
                    DataRow workRow = dt.NewRow();

                    workRow[0] = sTipoDocumento;
                    workRow[1] = sDescripcion;
                    workRow[2] = sDescrpBrev;
                    workRow[3] = iProceso;
                    workRow[4] = iSubProceso;
                    workRow[5] = bIdStatus;

                    dt.Rows.Add(workRow);
                    //dt.Rows.Add(new Object[] { "01", "Smith", 1, 1, 0 });
                }


                //Guardo los nuevos valores
                Session["data"] = dt;

                GrdTpoDocumento.DataSource = dt;
                GrdTpoDocumento.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdTpoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
                int index = row.RowIndex;

                string sTipoDocumento = GrdTpoDocumento.Rows[index].Cells[2].Text;
                int iProceso = Convert.ToInt32(Session["Proceso"]);
                int iSubProceso = Convert.ToInt32(Session["SubProceso"]);

                DataTable dt = (DataTable)Session["data"];

                // validar que el renglon a eliminar sea mayor a 1
                if (GrdTpoDocumento.Rows.Count > 1)
                {
                    dt.Rows.RemoveAt(index);
                }

                //Guardo los nuevos valores
                Session["data"] = dt;

                GrdTpoDocumento.DataSource = dt;
                GrdTpoDocumento.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdTpoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void GrdTpoDocumento_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = (DataTable)Session["data"];
            dt.Rows.RemoveAt(e.RowIndex);

            //Guardo los nuevos valores
            Session["data"] = dt;

            GrdTpoDocumento.DataSource = dt;
            GrdTpoDocumento.DataBind();

            //* * Agrega THEAD y TBODY a GridView.
            GrdTpoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;

        }

        protected void GrdTpoDocumento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string item = e.Row.Cells[0].Text;
                foreach (Button button in e.Row.Cells[6].Controls.OfType<Button>())
                {
                    if (button.CommandName == "Delete")
                    {
                        button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
                    }
                }
            }
        }

        protected void chkTpoDocumento_CheckedChanged(object sender, EventArgs e)
        {
            int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
            CheckBox cb = (CheckBox)GrdTpoDocumento.Rows[selRowIndex].FindControl("chkTpoDocumento");

            DataTable dt = (DataTable)Session["data"];
            DataRow workRow = dt.NewRow();

            workRow.BeginEdit();
            workRow[4] = cb.Checked;
            workRow.EndEdit();
        }
    }
}