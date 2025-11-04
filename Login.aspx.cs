using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Xml;

using MySql.Data.MySqlClient;

namespace WebItNow_Peacock
{
    public partial class Login : System.Web.UI.Page
    {
        // Desarrollo : Martin Baltierra Gonzalez
        // Empresa    : Itnow Technologies de México

        protected void Page_PreInit(object sender, EventArgs e)
        {
            string idioma = "es-MX"; // por defecto

            // Verificar si ya hay sesión
            if (Session["Idioma"] != null)
            {
                idioma = Session["Idioma"].ToString();
            }
            // Si no hay sesión, verificar cookie persistente
            else if (Request.Cookies["IdiomaUsuario"] != null)
            {
                idioma = Request.Cookies["IdiomaUsuario"].Value;
                Session["Idioma"] = idioma; // también guardamos en sesión
            }
            // Si no hay sesión ni cookie, detectar automáticamente idioma del navegador
            else
            {
                try
                {
                    string navLang = Request.UserLanguages != null && Request.UserLanguages.Length > 0
                        ? Request.UserLanguages[0]
                        : "es-MX";

                    if (navLang.StartsWith("en-US", StringComparison.OrdinalIgnoreCase))
                        idioma = "en-US";
                    else if (navLang.StartsWith("pt-BR", StringComparison.OrdinalIgnoreCase))
                        idioma = "pt-BR";
                    else
                        idioma = "es-MX";

                    // Guardar en sesión
                    Session["Idioma"] = idioma;

                    // Guardar cookie persistente para futuras visitas
                    HttpCookie cookieIdioma = new HttpCookie("IdiomaUsuario", idioma);
                    cookieIdioma.Expires = DateTime.Now.AddYears(10);
                    Response.Cookies.Add(cookieIdioma);
                }
                catch
                {
                    idioma = "es-MX";
                }
            }

            // Aplica cultura
            Thread.CurrentThread.CurrentCulture = new CultureInfo(idioma);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(idioma);
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            // Detectar si se hizo un cambio de idioma
            string idioma = Request["__EVENTARGUMENT"];

            if (!string.IsNullOrEmpty(idioma))
            {
                // Actualizar sesión
                Session["Idioma"] = idioma;

                // Actualizar cookie persistente
                HttpCookie cookieIdioma = new HttpCookie("IdiomaUsuario", idioma);
                cookieIdioma.Expires = DateTime.Now.AddYears(15);
                Response.Cookies.Add(cookieIdioma);

                // Recargar página para aplicar nuevo idioma
                Response.Redirect(Request.RawUrl, false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            // Opcional: si no hay cambio, aplicar idioma desde sesión o cookie
            string idiomaActual = Session["Idioma"] != null ? Session["Idioma"].ToString()
                                : (Request.Cookies["IdiomaUsuario"] != null ? Request.Cookies["IdiomaUsuario"].Value
                                : "es-MX");

            Thread.CurrentThread.CurrentCulture = new CultureInfo(idiomaActual);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(idiomaActual);

            //if (!Page.IsPostBack || Session["Idioma"] != null)
            if (!Page.IsPostBack)
            {

                //string idiomaSeleccionado = Request["__EVENTTARGET"] == "IdiomaDropdown" ? Request["__EVENTARGUMENT"] : null;
                //if (!string.IsNullOrEmpty(idiomaSeleccionado))
                //{
                //    Session["Idioma"] = idiomaSeleccionado;
                //    Response.Redirect(Request.RawUrl); // recarga la página con el idioma
                //}

                this.Form.Attributes.Add("autocomplete", "off");

                string filePath = Server.MapPath("~/App_Data/BD_MySQL.xml");

                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                System.Web.HttpContext.Current.Session["Server"] = doc.SelectSingleNode("/DatabaseConfig/Server").InnerText;
                System.Web.HttpContext.Current.Session["Port"] = doc.SelectSingleNode("/DatabaseConfig/Port").InnerText;
                System.Web.HttpContext.Current.Session["Database"] = doc.SelectSingleNode("/DatabaseConfig/Database").InnerText;
                System.Web.HttpContext.Current.Session["User"] = doc.SelectSingleNode("/DatabaseConfig/User").InnerText;
                System.Web.HttpContext.Current.Session["Password"] = doc.SelectSingleNode("/DatabaseConfig/Password").InnerText;

                // Labels
                lblTitulo.Text = GetGlobalResourceObject("GlobalResources", "lblTitulo").ToString();
                lblUsuario.Text = GetGlobalResourceObject("GlobalResources", "lblUsuario").ToString();
                LblPass.Text = GetGlobalResourceObject("GlobalResources", "lblContraseña").ToString();
                lblVerificacion.Text = GetGlobalResourceObject("GlobalResources", "lblCodigoVerificacion").ToString();

                // Placeholders
                TxtUsu.Attributes["placeholder"] = GetGlobalResourceObject("GlobalResources", "lblUsuario").ToString();
                TxtPass.Attributes["placeholder"] = GetGlobalResourceObject("GlobalResources", "lblContraseña").ToString();
                txtVerificationCode.Attributes["placeholder"] = GetGlobalResourceObject("GlobalResources", "lblCodigoVerificacion").ToString();
                BtnAceptar.Text = GetGlobalResourceObject("GlobalResources", "btnIniciarSesion").ToString();

            }
        }

            
        protected void BtnRegistrarse_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register_User.aspx");
        }

        public (string IdPrivilegio, string IdModulo) Autenticar(String pUsuarios, String pContrasena, String pIdioma)
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
                    cmd.Parameters.AddWithValue("@idioma", pIdioma);


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

                    string sIdioma = Convert.ToString(Session["Idioma"]);

                    // string result = Autenticar(TxtUsu.Text, TxtPass.Text);
                    (string idPrivilegio, string idModulo) = Autenticar(TxtUsu.Text, TxtPass.Text, sIdioma);

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

        protected void btnIngles_Click(object sender, ImageClickEventArgs e)
        {
            Session["Idioma"] = "en-US";
            Response.Redirect(Request.RawUrl);
        }

        protected void btnPortugues_Click(object sender, ImageClickEventArgs e)
        {
            // Guardas el idioma en sesión
            Session["Idioma"] = "pt-BR";

            // Recarga la página para aplicar recursos
            Response.Redirect(Request.RawUrl);
        }

        protected void btnEspañol_Click(object sender, ImageClickEventArgs e)
        {
            Session["Idioma"] = "es-MX";
            Response.Redirect(Request.RawUrl);
        }

        protected void ddlIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Guardar idioma en sesión y recargar la página
            //Session["Idioma"] = ddlIdioma.SelectedValue;
            //Response.Redirect(Request.RawUrl);
        }

        protected override void RaisePostBackEvent(IPostBackEventHandler sourceControl, string eventArgument)
        {
            string idioma = Request.Form["__EVENTARGUMENT"];
            if (!string.IsNullOrEmpty(idioma))
            {
                Session["Idioma"] = idioma;
                Response.Redirect(Request.RawUrl);
            }
            base.RaisePostBackEvent(sourceControl, eventArgument);
        }

    }
}