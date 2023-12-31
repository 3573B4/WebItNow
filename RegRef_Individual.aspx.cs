﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;

namespace WebItNow
{
    public partial class RegRef_Individual : System.Web.UI.Page
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
                getProcesos();
                ddlSubProceso.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
            }
        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            // Response.Redirect("Menu.aspx", true);
            Response.Redirect("Mnu_Dinamico.aspx", true);
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            //* Validar si el usuario existe o es nuevo
            if (TxtRef.Text != "" && TxtEmail.Text != "" && TxtAsegurado.Text != "")
            {
                if (ddlProceso.SelectedValue != "0")
                {
                    if (ddlSubProceso.SelectedValue != "0")
                    {
                    
                        // Insertar Registo Tabla tbReferencia (Referencias Individuales)
                        int result = Add_tbReferencia(TxtRef.Text, TxtEmail.Text, TxtAsegurado.Text, TxtTelefono.Text, Convert.ToInt32(ddlProceso.SelectedValue), Convert.ToInt32(ddlSubProceso.SelectedValue), 3, "Insert");

                        if (result == 0)
                        {
                            // Insertar Registros Tabla tbEstadoDocumento [ITM_04]
                            int idStatus = 1;
                            int valor = Add_tbEstadoDocumento(TxtRef.Text, Convert.ToInt32(ddlProceso.SelectedValue), Convert.ToInt32(ddlSubProceso.SelectedValue), idStatus);

                            string sAsunto = "Creación de Reporte: " + TxtRef.Text;
                            string sBody = "Estimado Cliente: \n " + " \n ";

                            sBody += "Enviamos este mensaje para hacer de su conocimiento que hemos iniciado la atención del reporte: " + TxtRef.Text + " \n " +
                            "Le informamos que a la brevedad alguno de nuestros colaboradores se estará poniendo en contacto con usted, o en su defecto, \n " +
                            "le estaremos solicitando la información necesaria a este correo electrónico. \n " +
                            "En caso de cualquier duda, puede ingresar a este vínculo, donde podrá buscar y encontrar lo que requiera: \n " +
                            "https://peacock.zendesk.com/hc/es-419" + "\n \n ";
                            sBody += "Alternativamente, puede contactarnos en cualquiera de los siguientes medios, donde con gusto lo atenderemos: \n " + " \n " +
                            "* Asistente virtual en " + "https://www.peacock.claims \n " +
                            "* WhatsApp: + 52 55-9035-4806 \n " +
                            "* Correo electrónico: " + "contacto@peacock.claims \n " +
                            "* Vía Teléfono: + 5255-8525-7200 y +52 55-8932-4700 \n " + " \n ";
                            sBody += "Agradecemos de antemano su confianza y preferencia. Esperamos que su experiencia de servicio sea satisfactoria.";

                            var email = new EnvioEmail();
                            int Envio_Ok = email.EnvioMensaje(TxtRef.Text.Trim(), TxtEmail.Text.Trim(), sAsunto, sBody);

                            //EnvioMensaje(TxtUsu.Text.Trim(), TxtEmail.Text.Trim());

                            if (Envio_Ok == 0)
                            {
                                LblMessage.Text = "Su registro fue creado exitosamente";
                                this.mpeMensaje.Show();
                            }

                            Limpia(this.Controls);

                            // Response.Redirect("Login.aspx");
                            Lbl_Message.Visible = false;
                            
                            RequiredFieldValidator1.Display = ValidatorDisplay.None;
                            RequiredFieldValidator2.Display = ValidatorDisplay.None;
                            RequiredFieldValidator3.Display = ValidatorDisplay.None;
                            RequiredFieldValidator4.Display = ValidatorDisplay.None;
                            RequiredFieldValidator5.Display = ValidatorDisplay.None;

                        }
                    
                    }
                    else
                    {
                        Lbl_Message.Visible = true;
                        Lbl_Message.Text = "* Seleccione un SubProceso";
                    }
                }
                else
                {
                    Lbl_Message.Visible = true;
                    Lbl_Message.Text = "* Seleccione un Proceso";
               }
            }
            else
            {
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "* Estos campos son obligatorios";
            }
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        public int Add_tbReferencia(String pReferencia, String pUsEmail, string pAsegurado, String pTelefono, int pProceso, int pSubProceso, int pUsPrivilegios, string pStatementType)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbReferencia", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@referencia", pReferencia);
                cmd1.Parameters.AddWithValue("@email", pUsEmail);
                cmd1.Parameters.AddWithValue("@asegurado", pAsegurado);
                cmd1.Parameters.AddWithValue("@telefono", pTelefono);
                cmd1.Parameters.AddWithValue("@proceso", pProceso);
                cmd1.Parameters.AddWithValue("@subproceso", pSubProceso);
                cmd1.Parameters.AddWithValue("@privilegios", pUsPrivilegios);
                cmd1.Parameters.AddWithValue("@StatementType", pStatementType);

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

        public int Add_tbEstadoDocumento(String pReferencia, int pProceso, int pSubProceso, int pIdStatus)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Consulta a la tabla Tipo de Documento
                string strQuery = "Select IdTpoDocumento, Descripcion From ITM_06 " +
                                  " Where IdProceso = " + pProceso + "" +
                                  "   And IdSubProceso = " + pSubProceso + " And IdStatus = " + pIdStatus + "";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //  SqlDataReader dr = cmd.ExecuteReader();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {

                    string IdTpoDocumento = Convert.ToString(row[0]);

                    int iIdStatus = 0;
                    int iIdDescarga = 0;
                    // Insert en la tabla Estado de Documento
                    SqlCommand cmd1 = new SqlCommand("Insert into ITM_04 (Referencia, IdTipoDocumento, IdStatus, IdDescarga) " +
                                        "Values ('" + pReferencia + "', '" + IdTpoDocumento + "', " + iIdStatus + ", " + iIdDescarga + ")", Conecta.ConectarBD);

                    SqlDataReader dr1 = cmd1.ExecuteReader();

                    cmd1.Dispose();
                    dr1.Dispose();
                }

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

        public int ValidaReferencia(String pReferencia, int pUsPrivilegios)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbValidaReferencia", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@referencia", pReferencia);
                cmd1.Parameters.AddWithValue("@privilegios", pUsPrivilegios);
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

        protected void TxtRef_TextChanged(object sender, EventArgs e)
        {
            // Validar si existe Usuario en la tabla ITM_02 (tbReferencia)
            Variables.wPrivilegios = "3";
            int Referencia_Existe = ValidaReferencia(TxtRef.Text, Int32.Parse(Variables.wPrivilegios));

            if (Referencia_Existe == 1)
            {
                TxtRef.Text = string.Empty;
                TxtRef.Focus();

                LblMessage.Text = "La referencia ya se encuentra registrada";
                this.mpeMensaje.Show();
            }
            else
            {
                TxtEmail.Focus();
            }
        }

        public void Limpia(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
             // else if (control is DropDownList)
             //     ((DropDownList)control).Items.Clear();
                else if (control is DropDownList)
                    ((DropDownList)control).SelectedIndex = 0;
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

        protected void ddlProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iProceso = Convert.ToInt32(ddlProceso.SelectedValue);
            getSubProcesos(iProceso);
        }

        protected void ddlSubProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetTpoDoc();
        }

        protected void getProcesos()
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();
            string sqlQuery = "SELECT IdProceso, Nombre " +
                                    " FROM ITM_16 " +
                                    " WHERE IdStatus = 1 ";
            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlProceso.DataSource = dt;

            ddlProceso.DataValueField = "IdProceso";
            ddlProceso.DataTextField = "Nombre";

            ddlProceso.DataBind();
            ddlProceso.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

        }

        protected void getSubProcesos(int iProceso)
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdSubProceso, Descripcion " +
                                    " FROM ITM_14 " +
                                    " WHERE IdProceso = " + iProceso;

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlSubProceso.DataSource = dt;

                ddlSubProceso.DataValueField = "IdSubProceso";
                ddlSubProceso.DataTextField = "Descripcion";

                ddlSubProceso.DataBind();
                ddlSubProceso.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

    }
}