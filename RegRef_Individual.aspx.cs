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

            }
        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            //* Validar si el usuario existe o es nuevo
            if (TxtRef.Text != "" && TxtEmail.Text != "")
            {

                // Insertar Registo Tabla tbUsuarios (UploadFiles)
                int result = Add_tbReferencia(TxtRef.Text, TxtEmail.Text, TxtAsegurado.Text, TxtTelefono.Text, 3, "Insert");

                if (result == 0)
                {
                    // Insertar Registros Tabla tbEstadoDocumento [ITM_04]
                    int idStatus = 1;
                    int valor = Add_tbEstadoDocumento(TxtRef.Text, idStatus);

                    var email = new EnvioEmail();
                    int Envio_Ok = email.EnvioMensaje(TxtRef.Text.Trim(), TxtEmail.Text.Trim(), "Registro Referencia ", string.Empty);

                    //EnvioMensaje(TxtUsu.Text.Trim(), TxtEmail.Text.Trim());

                    if (Envio_Ok == 0)
                    {
                        LblMessage.Text = "Alta de Asunto, agregada correctamente ";
                        this.mpeMensaje.Show();
                    }

                    Limpia(this.Controls);

                    // Response.Redirect("Login.aspx");
                    Lbl_Message.Visible = false;
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

        public int Add_tbReferencia(String pReferencia, String pUsEmail, string pAsegurado, String pTelefono, int pUsPrivilegios, string pStatementType)
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

        public int Add_tbEstadoDocumento(String pReferencia, int pIdStatus)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Consulta a la tabla Tipo de Documento
                string strQuery = "Select IdTpoDocumento, Descripcion From ITM_06 Where Status = " + pIdStatus + "";

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

        public int ValidaEmail(String pEmail, int pUsPrivilegios)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbEmail", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@email", pEmail);
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
            int Usuario_Existe = ValidaReferencia(TxtRef.Text, Int32.Parse(Variables.wPrivilegios));

            if (Usuario_Existe == 1)
            {
                TxtRef.Text = string.Empty;
                TxtEmail.Focus();

                LblMessage.Text = "La referencia ya se encuentra registrada";
                this.mpeMensaje.Show();
            }
            else
            {
                TxtEmail.Focus();
            }
        }

        protected void TxtEmail_TextChanged(object sender, EventArgs e)
        {
            //// Validar si existe Email en la tabla ITM_02 (tbReferencia)
            //Variables.wPrivilegios = "3";
            //int Email_Existe = ValidaEmail(TxtEmail.Text, Int32.Parse(Variables.wPrivilegios));

            //if (Email_Existe == 1)
            //{
            //    TxtEmail.Text = string.Empty;
            //    TxtEmail.Focus();

            //    LblMessage.Text = "El Correo electrónico ya existe";
            //    this.mpeMensaje.Show();
            //}
            //else
            //{
            //    TxtEmail.Focus();
            //}
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

    }
}