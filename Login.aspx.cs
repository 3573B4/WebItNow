using System;
using System.Web.UI;
using System.Xml;

using MySql.Data.MySqlClient;

namespace WebItNow_Peacock
{
    public partial class Login : System.Web.UI.Page
    {
        // Desarrollo : Martin Baltierra Gonzalez
        // Empresa    : Itnow Technologies de México
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.Form.Attributes.Add("autocomplete", "off");

                string filePath = Server.MapPath("~/App_Data/BD_MySQL.xml");

                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                System.Web.HttpContext.Current.Session["Server"] = doc.SelectSingleNode("/DatabaseConfig/Server").InnerText;
                System.Web.HttpContext.Current.Session["Port"] = doc.SelectSingleNode("/DatabaseConfig/Port").InnerText;
                System.Web.HttpContext.Current.Session["Database"] = doc.SelectSingleNode("/DatabaseConfig/Database").InnerText;
                System.Web.HttpContext.Current.Session["User"] = doc.SelectSingleNode("/DatabaseConfig/User").InnerText;
                System.Web.HttpContext.Current.Session["Password"] = doc.SelectSingleNode("/DatabaseConfig/Password").InnerText;

            }
        }


        protected void BtnRegistrarse_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register_User.aspx");
        }

        public (string IdPrivilegio, string IdModulo) Autenticar(String pUsuarios, String pContrasena)
        {
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            try
            {
                using (MySqlCommand cmd = new MySqlCommand("sp_Login", dbConn.Connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@usuario", pUsuarios);
                    cmd.Parameters.AddWithValue("@password", pContrasena);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            System.Web.HttpContext.Current.Session["UsEmail"] = dr["UsEmail"].ToString().Trim();
                            return (dr["IdPrivilegio"].ToString().Trim(), dr["IdModulo"].ToString().Trim());

                            // return dr["IdPrivilegio"].ToString().Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Mostrar el mensaje de error (asegúrate de adaptar esto a tu método de mostrar errores)
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {
                dbConn.Close();
            }

            //return null;
            return (string.Empty, string.Empty); // Retorna valores vacíos en lugar de null
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        public static string GetHomePath()
        {
            // Not in .NET 2.0
            // System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                return System.Environment.GetEnvironmentVariable("HOME");

            return System.Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        }

        public static string GetDownloadFolderPath()
        {
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                string pathDownload = System.IO.Path.Combine(GetHomePath(), "Downloads");
                return pathDownload;
            }

            return System.Convert.ToString(
                Microsoft.Win32.Registry.GetValue(
                     @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders"
                    , "{374DE290-123F-4565-9164-39C4925E467B}"
                    , String.Empty
                )
            );
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            try
            {

                //if (TxtUsu.Text == "" || TxtPass.Text == "" || txtVerificationCode.Text == "")
                if (string.IsNullOrEmpty(TxtUsu.Text) || string.IsNullOrEmpty(TxtPass.Text) || string.IsNullOrEmpty(txtVerificationCode.Text))
                {
                    Lbl_Message.Visible = true;
                    Lbl_Message.Text = "* Estos campos son obligatorios";

                    lblCaptchaMessage.Text = string.Empty;

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
                    // Variables.wUserName = TxtUsu.Text;
                    // Variables.wPassword = TxtPass.Text;

                    Variables.wUserLogon = TxtUsu.Text;
                    Variables.wPassLogon = TxtPass.Text;

                    Variables.wUserName = Convert.ToString(Session["User"]);        // Parametro obtenido de BD_MySQL.xml
                    Variables.wPassword = Convert.ToString(Session["Password"]);    // Parametro obtenido de BD_MySQL.xml

                    // string result = Autenticar(TxtUsu.Text, TxtPass.Text);
                    (string idPrivilegio, string idModulo) = Autenticar(TxtUsu.Text, TxtPass.Text);

                    // if (result != null)
                    if (!string.IsNullOrEmpty(idPrivilegio) && !string.IsNullOrEmpty(idModulo))
                    {
                        // IdUsuario
                        System.Web.HttpContext.Current.Session["IdUsuario"] = TxtUsu.Text;
                        // UsPassword
                        System.Web.HttpContext.Current.Session["UsPassword"] = TxtPass.Text; ;
                        // Permisos Usuario
                        System.Web.HttpContext.Current.Session["UsPrivilegios"] = idPrivilegio;
                        // Acceso Modulo
                        System.Web.HttpContext.Current.Session["ModuloAcceso"] = idModulo;

                        // Permisos Usuario
                        // System.Web.HttpContext.Current.Session["UsPrivilegios"] = dr1["UsPrivilegios"].ToString().Trim();

                        string UsPrivilegios = Convert.ToString(Session["UsPrivilegios"]);

                        string ModuloAcceso = Convert.ToString(Session["ModuloAcceso"]);

                        if (UsPrivilegios == "4")
                        {
                         // Response.Redirect("Upload_Files.aspx");
                        }
                        else
                        {
                            if (ModuloAcceso == "1")
                            {
                                // Siniestros
                                Response.Redirect("Mnu_Dinamico.aspx", false);
                            } 
                            else if (ModuloAcceso == "2")
                            {
                                // GastosMedicos
                                Response.Redirect("fwGM_Mnu_Dinamico.aspx", false);
                            }

                            
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
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

    }
}