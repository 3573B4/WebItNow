using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace WebItNow
{
    public partial class Forgot_Password : System.Web.UI.Page
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
                this.Form.Attributes.Add("autocomplete", "off");
                TxtEmail.Focus();
            }

        }

        public void OnTextChanged(object sender, EventArgs e)
        {

            Variables.wPrivilegios = System.Web.HttpContext.Current.Session["UsPrivilegios"] as string;
            //Reference the TextBox.
            TextBox textBox = sender as TextBox;

            if (TxtEmail.Text != "")
            {

                Lbl_Message.Visible = true;

                // Insertar Registo Usuario Cargas
                int result = ValidarEmail(TxtEmail.Text, Int32.Parse(Variables.wPrivilegios));

                if (result == 0)
                {
                    TxtEmail.Text = string.Empty;
                    LblMessage.Text = "Ingrese un correo electrónico valido";
                    this.mpeMensaje.Show();
                }
                else
                {
                    Lbl_Message.Text = "";
                    Lbl_Message.Visible = false;
                    TxtPass.Focus();
                }

            }
            else
            {
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "* Este campo es obligatorio";
            }

        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void BtnClose_Click(object sender, EventArgs e) 
        {
        
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            if (TxtPass.Text == "" || TxtConfPass.Text == "")
            {
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "* Estos campos son obligatorios";
            }
            else
            {

                if (TxtPass.Text == TxtConfPass.Text)
                {

                    string sUsuario = Recuperar_Usuario(TxtEmail.Text);

                    var email = new EnvioEmail();   

                    // Consultar de la tabla [tbUsuarios] el [UsEmail]
                    string sEmail = email.CorreoElectronico(sUsuario);
                    int Envio_Ok = email.EnvioMensaje(sUsuario, sEmail, "Cambio Contraseña",string.Empty);

                    if (Envio_Ok == 0)
                    {
                        int result = UpdatePassword(sUsuario, sEmail, TxtPass.Text, Int32.Parse(Variables.wPrivilegios), "Update");
                        if (result == 0)
                        {
                            LblMessage.Text = "Contraseña se actualizo correctamente ";
                            this.mpeMensaje.Show();

                            Limpia(this.Controls);
                        }
                        Lbl_Message.Visible = false;
                    }
                }
                else 
                {
                    Lbl_Message.Visible = true;
                    Lbl_Message.Text = "* Contraseñas son diferentes";
                }
            }
        }

        public int ValidaUser(String pUsuarios, int pUsPrivilegios)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbUsuario", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@usuario", pUsuarios);
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

        public int ValidarEmail(String pUsuarios, int pUsPrivilegios)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbEmail", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@email", pUsuarios);
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

        public string Recuperar_Usuario (String pEmail)
        {

            try
            {

                ConexionBD Conecta = new ConexionBD();
                NewMethod(Conecta);

                SqlCommand cmd1 = new SqlCommand("Select IdUsuario From ITM_02 Where UsEmail = '" + pEmail + "'", Conecta.ConectarBD);
                SqlDataReader dr1 = cmd1.ExecuteReader();

                while (dr1.Read())
                {
                    return dr1["IdUsuario"].ToString().Trim();
                }

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

                return String.Empty;

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

            return String.Empty;
        }

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }

        public int UpdatePassword(String pUsuarios, String pEmail, String pUsPassword, int pUsPrivilegios, string pStatementType)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbUsuario_StatementType", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@usuario", pUsuarios);
                cmd1.Parameters.AddWithValue("@email", pEmail);
                cmd1.Parameters.AddWithValue("@password", pUsPassword);
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

      }
}