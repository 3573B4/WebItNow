using System;
using System.IO;
using System.Web;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace WebItNow_Peacock
{
    public partial class ManychatReceptor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

            // Solo ejecutar si la solicitud viene desde Manychat con POST
            if (Request.HttpMethod == "POST")
            {

                string jsonString = new StreamReader(Request.InputStream).ReadToEnd();

                try
                {
                    JObject data = JObject.Parse(jsonString);
                    string nom_completo = data["nom_completo"]?.ToString();
                    string num_telefonico = data["num_telefonico"]?.ToString();

                    // Llama a tu método para guardar en la base de datos
                    Guardar_BD(nom_completo, num_telefonico);

                    Response.StatusCode = 200;
                    Response.Write("OK");
                }
                catch (Exception ex)
                {
                    Response.StatusCode = 500;
                    Response.Write("Error: " + ex.Message);
                }

            }

            //string rutaLog = Server.MapPath("~/App_Data/log_manychat.txt");

            //LogToFile(rutaLog, "Entró al Page_Load");

            //string method = Request.HttpMethod;
            //LogToFile(rutaLog, "Tipo de solicitud: " + method);

            //if (method == "POST")
            //{
            //    LogToFile(rutaLog, "Es un POST");

            //    using (StreamReader reader = new StreamReader(Request.InputStream))
            //    {
            //        string jsonBody = reader.ReadToEnd();
            //        LogToFile(rutaLog, "JSON recibido: " + jsonBody);
            //    }
            //}
            //else
            //{
            //    LogToFile(rutaLog, "No fue un POST, fue: " + method);
            //}
        }

        private void Guardar_BD(string nom_completo, string num_telefono)
        {

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string Id_Usuario = "Martin.Baltierra";
            int IdStatus = 1;

            try
            {

                string strQuery = "INSERT INTO ITM_29 (nom_completo, num_telefono, Id_Usuario, IdStatus) VALUES (@nom_completo, @num_telefono,  @Id_Usuario,  @IdStatus)";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@nom_completo", nom_completo);
                    cmd.Parameters.AddWithValue("@num_telefono", num_telefono);
                    cmd.Parameters.AddWithValue("@Id_Usuario", Id_Usuario);
                    cmd.Parameters.AddWithValue("@IdStatus", IdStatus);
                });

                dbConn.Close();

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

        }


        private void LogToFile(string ruta, string mensaje)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(ruta, true))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + mensaje);
                }
            }
            catch (Exception ex)
            {
                // Puedes registrar errores del log aquí si quieres
                string error = ex.Message;
            }
        }

        private void LogMensaje(string mensaje)
        {
            try
            {
                string ruta = Server.MapPath("~/App_Data/log_manychat.txt");
                using (StreamWriter sw = new StreamWriter(ruta, true))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {mensaje}");
                }
            }
            catch (Exception ex)
            {
                // En caso de error al escribir el log
                string error = ex.Message;
            }
        }

    }
}