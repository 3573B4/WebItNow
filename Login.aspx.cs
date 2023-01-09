using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Windows.Forms;

using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace WebItNow
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.Form.Attributes.Add("autocomplete", "off");

                // Permisos Usuario
                System.Web.HttpContext.Current.Session["UsPrivilegios"] = "3";
            }
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            if (TxtUsu.Text == "" || TxtPass.Text == "" || txtVerificationCode.Text == "")
            {
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "* Estos campos son obligatorios";
                lblCaptchaMessage.Text = "";
                //LblMessage.Text = "Debes capturar Usuario / Clave / " + "<br/>" + "Código de verificación.";
                //this.mpeMensaje.Show();
            }
            else if (txtVerificationCode.Text.ToLower() != Session["CaptchaVerify"].ToString())
            {
                lblCaptchaMessage.Text = "Por favor ingrese el captcha correcto";
                lblCaptchaMessage.ForeColor = System.Drawing.Color.Red;
            }
            else
            {

                string result = Autenticar(TxtUsu.Text, TxtPass.Text);

                if (result != null)
                {

                    // IdUsuario
                    System.Web.HttpContext.Current.Session["IdUsuario"] = TxtUsu.Text;
                    // Permisos Usuario
                    System.Web.HttpContext.Current.Session["UsPrivilegios"] = result;

                    // Permisos Usuario
                    // System.Web.HttpContext.Current.Session["UsPrivilegios"] = dr1["UsPrivilegios"].ToString().Trim();

                    string UsPrivilegios = Convert.ToString(Session["UsPrivilegios"]);

                    if (UsPrivilegios == "3")
                    {
                        Response.Redirect("SubirArchivo.aspx");
                    }
                    else
                    {
                        Response.Redirect("menu.aspx");
                    }

                    Lbl_Message.Visible = false;

                }
                else
                {
                    // "Usuario y/o Contraseña Incorrectos";
                    LblMessage.Text = "No fue posible iniciar sesión." + "<br/>" + "Confirme su nombre de usuario y contraseña.";
                    this.mpeMensaje.Show();
                }
            }
        }

        protected void BtnRegistrarse_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }

        public string Autenticar(String pUsuarios, String pContrasena)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_Login", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@usuario", pUsuarios);
                cmd1.Parameters.AddWithValue("@password", pContrasena);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                while (dr1.Read())
                {
                    return dr1["UsPrivilegios"].ToString().Trim();
                }
                    //if (dr1.Read())
                    //{

                    //    return dr1.GetInt32(0);

                    //}

                cmd1.Dispose();
                dr1.Dispose();

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

            return null;
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

    }
}