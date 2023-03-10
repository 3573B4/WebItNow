using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;

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

                getProcesos(Convert.ToInt32(Session["Proceso"]));

                //ddlSubProceso.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                getSubProcesos(Convert.ToInt32(Session["Proceso"]));

                //ddlProceso.SelectedValue = Convert.ToString(Session["Proceso"]);
                //ddlSubProceso.SelectedValue = Convert.ToString(Session["SubProceso"]);

                GetTpoDocumento(Convert.ToInt32(Session["Proceso"]), Convert.ToInt32(Session["SubProceso"]));
            }

            //* * Agrega THEAD y TBODY a GridView.
            GrdTpoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;
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
            //int result = ValidarCheckBox();

            if (ValidarCheckBox())
            {
                int Referencia_Existe = ValReferenciaTransaccion(TxtReferencia.Text);
                string sqlQuery = string.Empty;

                if (Referencia_Existe == 0)
                {
                    ObtenerDatosGrid_Insert();
                }
                else
                {
                    ObtenerDatosGrid_Update();
                }

                // Validar que los campos hayan sido capturados
                if (TxtReferencia.Text != "" && TxtNom.Text != "" && TxtEmail.Text != "")
                {
                    //  Insertar Registo Tabla ITM_11 (Solicitud Documento)
                    //  int result = Add_Solicitud(TxtNom.Text, TxtEmail.Text, TxtReferencia.Text, "02");
                    //  int result = Add_Solicitud(TxtNom.Text, TxtEmail.Text, TxtReferencia.Text, ddlTpoDocumento.SelectedValue);

                    int result = 0;

                    if (result == 0)
                    {

                        Session["Asunto"] = "Solicitud Documento";
                        Session["Referencia"] = TxtReferencia.Text;
                        Session["Email"] = TxtEmail.Text.Trim();

                        Response.Redirect("Page_Message.aspx");

                        //var email = new EnvioEmail();
                        //int Envio_Ok = email.EnvioMensaje(TxtNom.Text.Trim(), TxtEmail.Text.Trim(), "Solicitud Documento", string.Empty);

                        //if (Envio_Ok == 0)
                        //{
                        //    LblMessage.Text = "Solicitud enviada correctamente ";
                        //    this.mpeMensaje.Show();
                        //}

                        Limpia(this.Controls);

                        Lbl_Message.Visible = false;
                    }
                }
                else
                {
                    Lbl_Message.Visible = true;
                    Lbl_Message.Text = "* Estos campos son obligatorios";
                }
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
                sqlQuery = "Select IdTpoDocumento, Descripcion, IdProceso, IdSubProceso, 'false' as IdStatus " +
                           "  From ITM_06 " +
                           " Where IdProceso = " + iIdProceso + "" +
                           "   And IdSubProceso = " + iIdSubProceso + "" +
                           "   And Status = '1'";
            }
            else
            {
                sqlQuery = "Select tp.IdTpoDocumento, tp.Descripcion, tp.IdProceso, tp.IdSubProceso, tr.IdStatus " +
                            "  From ITM_06 tp, ITM_15 tr " +
                            " Where tp.IdTpoDocumento = tr.IdTpoDocumento And tp.IdProceso = tr.IdProceso And tp.IdSubProceso = tr.IdSubProceso" +
                            "   And tp.IdProceso = " + iIdProceso + "" +
                            "   And tp.IdSubProceso = " + iIdSubProceso + "" +
                            "   And tr.Referencia = '" + TxtReferencia.Text + "'" +
                            "   And tp.Status = '1'";

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

            GrdTpoDocumento.DataSource = dt;
            GrdTpoDocumento.DataBind();

            //* * Agrega THEAD y TBODY a GridView.
            GrdTpoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;

            Conecta.Cerrar();

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

        public void ObtenerDatosGrid_Insert()
        {
            // DBCC CHECKIDENT (ITM_15, RESEED, 0) -- Inicializar el campo [IdTransaccion] = 0
            string sReferencia = TxtReferencia.Text;
            //bool valorcol0 = false;
            int valorcol0;

            foreach (GridViewRow row in GrdTpoDocumento.Rows)
            {

                var chkbox = row.FindControl("chkTpoDocumento") as CheckBox;
                if (chkbox.Checked == true)
                {
                    valorcol0 = 1;
                }
                else
                {
                    valorcol0 = 0;
                }

                string valorcol1 = row.Cells[1].Text;
                string valorcol2 = row.Cells[2].Text;
                string valorcol3 = row.Cells[3].Text;
                string valorcol4 = row.Cells[4].Text;

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

            }
        }

        public void ObtenerDatosGrid_Update()
        {
            // DBCC CHECKIDENT (ITM_15, RESEED, 0) -- Inicializar el campo [IdTransaccion] = 0
            string sReferencia = TxtReferencia.Text;
            //  bool valorcol0 = false;
            int valorcol0;

            foreach (GridViewRow row in GrdTpoDocumento.Rows)
            {

                var chkbox = row.FindControl("chkTpoDocumento") as CheckBox;
                if (chkbox.Checked == true)
                {
                    valorcol0 = 1;
                }
                else
                {
                    valorcol0 = 0;
                }

                string valorcol1 = row.Cells[1].Text;
                string valorcol2 = row.Cells[2].Text;
                string valorcol3 = row.Cells[3].Text;
                string valorcol4 = row.Cells[4].Text;

                // Insertar tabla ITM_15
                ConexionBD Conecta = new ConexionBD();
                NewMethod(Conecta);

                try
                {

                    // Actualizar en tabla ITM_15
                    //SqlCommand cmd1 = new SqlCommand("sp_tbTransaccion_Update", Conecta.ConectarBD);
                    //cmd1.CommandType = CommandType.StoredProcedure;

                    //cmd1.Parameters.AddWithValue("@idstatus", valorcol0);
                    //cmd1.Parameters.AddWithValue("@referencia", sReferencia);
                    //cmd1.Parameters.AddWithValue("@idtpodocumento", valorcol1);
                    //cmd1.Parameters.AddWithValue("@idproceso", valorcol3);
                    //cmd1.Parameters.AddWithValue("@idsubproceso", valorcol4);

                    //SqlDataReader dr1 = cmd1.ExecuteReader();

                    //if (dr1.Read())
                    //{

                    //    // return dr1.GetInt32(0);

                    //}

                    //cmd1.Dispose();
                    //dr1.Dispose();

                    //Conecta.Cerrar();

                    //return 0;

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

            }
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

                        Insert_Transaccion(sTpoDocumento, iIdProceso, iIdSubProceso);
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
    }
}