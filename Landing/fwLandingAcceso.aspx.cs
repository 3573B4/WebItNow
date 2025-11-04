using System;
using System.Xml;

using MySql.Data.MySqlClient;


namespace WebItNow_Peacock.Landing
{
    public partial class fwLandingAcceso : System.Web.UI.Page
    {

        // Guardamos la referencia del siniestro o token en ViewState
        protected string TokenActual
        {
            get { return ViewState["TokenActual"]?.ToString(); }
            set { ViewState["TokenActual"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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

                string token = Request.QueryString["token"];

                if (string.IsNullOrEmpty(token))
                {
                    LblMensaje.Text = "Token inválido o faltante.";
                    PnlLogin.Visible = false;
                    return;
                }

                TokenActual = token;

                if (ValidarToken(token))
                {
                    PnlLogin.Visible = true;
                    PnlFormulario.Visible = false;
                    LblMensaje.Text = "Ingrese la contraseña temporal enviada por correo.";
                }
                else
                {
                    LblMensaje.Text = "El enlace ha expirado o es inválido.";
                    PnlLogin.Visible = false;
                    PnlFormulario.Visible = false;
                }
            }
        }

        // Validar token en la base de datos
        private bool ValidarToken(string token)
        {
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();
            
            bool esValido = false;

            try
            {
                string strQuery = @"SELECT COUNT(*) FROM ITM_Tokens 
                                     WHERE Token=@Token AND FechaExpira > NOW() AND Usado=0";

                using (MySqlCommand cmd = new MySqlCommand(strQuery, dbConn.Connection))
                {
                    cmd.Parameters.AddWithValue("@Token", token);

                    // Ejecutar y validar
                    object result = cmd.ExecuteScalar();

                    // Validar que result no sea NULL
                    if (result != null && result != DBNull.Value)
                        esValido = Convert.ToInt32(result) > 0;
                }
            }
            finally
            {
                dbConn.Close();
            }

            return esValido;
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string passwordIngresada = TxtPassword.Text.Trim();

            int idAsunto = ValidarPassword(TokenActual, passwordIngresada);

            if (idAsunto > 0)
            {
                PnlLogin.Visible = false;
                // PnlFormulario.Visible = true;
                // LblMensaje.Text = "Bienvenido. Ahora puede cargar la información de su siniestro.";

                // Response.Redirect("fwLandingDocument.aspx?Ref=" + Variables.wRef + "&SubRef=" + Variables.wSubRef + "&Proyecto=" + iIdProyecto, true);

                Session["IdAsunto"] = idAsunto;
                Response.Redirect("fwLandingDocument.aspx");
            }
            else
            {
                LblMensaje.Text = "Contraseña incorrecta. Intente de nuevo.";
            }
        }

        private int ValidarPassword(string token, string password)
        {
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            int idAsunto = 0;

            try
            {
                
                //string query = @"SELECT COUNT(*) FROM ITM_Tokens 
                //                 WHERE Token=@Token AND PasswordTemporal=@Password
                //                   AND FechaExpira > NOW() AND Usado=0";

                string strQuery = @"SELECT IdAsunto FROM ITM_Tokens WHERE Token = @Token 
                                       AND PasswordTemporal = @Password 
                                       AND FechaExpira > NOW() AND Usado = 0 LIMIT 1;";

                using (MySqlCommand cmd = new MySqlCommand(strQuery, dbConn.Connection))
                {
                    cmd.Parameters.AddWithValue("@Token", token);
                    cmd.Parameters.AddWithValue("@Password", password);

                    //esCorrecta = Convert.ToInt32(cmd.ExecuteScalar()) > 0;

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        idAsunto = Convert.ToInt32(result);

                        // 🔹 Marcar como usado después de una validación exitosa
                        string updateQuery = @"UPDATE ITM_Tokens SET Usado = 1 WHERE Token = @Token;";
                        using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, dbConn.Connection))
                        {
                            updateCmd.Parameters.AddWithValue("@Token", token);
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            finally
            {
                dbConn.Close();
            }

            return idAsunto;
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {

        }
    }
}