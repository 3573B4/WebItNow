using System;
using Microsoft.Exchange.WebServices.Data;

using System.Data.SqlClient;

namespace WebItNow
{
    public class EnvioEmail
    {
        public string CorreoElectronico(string pReferencia)
        {
            string sEmail = string.Empty;

            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            // Consulta en la tabla Usuarios
            SqlCommand cmd1 = new SqlCommand("Select UsEmail From ITM_02 Where UsReferencia LIKE '%' + '" + pReferencia + "'  + '%'", Conecta.ConectarBD);
            SqlDataReader dr1 = cmd1.ExecuteReader();

            if (dr1.HasRows)
            {

                while (dr1.Read())
                {
                    sEmail = dr1["UsEmail"].ToString().Trim();
                }
            }
            else
            {
                return sEmail;
            }

            cmd1.Dispose();
            dr1.Dispose();

            Conecta.Cerrar();

            return sEmail;
        }

        public string CorreoElectronico_User(string pUsuario)
        {
            string sEmail = string.Empty;

            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            // Consulta en la tabla Usuarios
            SqlCommand cmd1 = new SqlCommand("Select UsEmail From ITM_01 Where IdUsuario = '" + pUsuario + "'", Conecta.ConectarBD);
            SqlDataReader dr1 = cmd1.ExecuteReader();

            if (dr1.HasRows)
            {

                while (dr1.Read())
                {
                    sEmail = dr1["UsEmail"].ToString().Trim();
                }
            }
            else
            {
                return sEmail;
            }

            cmd1.Dispose();
            dr1.Dispose();

            Conecta.Cerrar();

            return sEmail;
        }

        public int EnvioMensaje(String pUsuarios, String pEmail, string pAsunto, string pBody)
        {
            string wDe = "archivo@peacock.claims";
            string wPara = pEmail;
            //  string wCC = "martin.baltierra@itnow.mx";
            string wAsunto = pAsunto;
            //string wMensaje = pBody + " \n Observaciones registradas : \n";
            string wMensaje = pBody;

            System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
            correo.From = new System.Net.Mail.MailAddress(wDe);
            correo.To.Add(wPara);
            //  correo.CC.Add(wCC);
            correo.Subject = wAsunto;
            wMensaje += "\n\nFecha y hora GMT: " +
                DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss");
            correo.Body = wMensaje;
            correo.IsBodyHtml = false;
            correo.Priority = System.Net.Mail.MailPriority.Normal;
            //
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

            //
            //---------------------------------------------
            // Servidor de correo , Usuario, Password
            //---------------------------------------------
            //smtp.Host = "outlook.office365.com";
            //smtp.Port = 25; //465; //25

            smtp.Host = "smtp.office365.com";
            smtp.Port = 587; //465; //25

            smtp.Credentials = new System.Net.NetworkCredential("archivo@peacock.claims", "ArcpT2022#");
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(correo);
                // System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;

                return 0;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return 1;
            }
        }


        public int EnvioExchange(String pUsuarios, String pEmail, string pAsunto, string pBody)
        {
            try
            {
                string wPara = pEmail;
                string wAsunto = pAsunto;
                string wMensaje = "Observaciones registradas :  \n" + pBody;

                wMensaje += "\n\nFecha y hora GMT: " +
                    DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss");

                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2007_SP1);
                
                service.Credentials = new WebCredentials("sistemas@peacock.claims", "System1623#");
                service.TraceEnabled = true;
                service.TraceFlags = TraceFlags.All;
                service.AutodiscoverUrl("sistemas@peacock.claims", RedirectionUrlValidationCallback);
                
                EmailMessage email = new EmailMessage(service);

                email.ToRecipients.Add(wPara);
                email.Subject = wAsunto;
                email.Body = new MessageBody(wMensaje);
                email.Send();

                return 0;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return 1;
            }
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // El valor predeterminado para la devolución de llamada de validación es rechazar la URL.
            bool result = false;
            Uri redirectionUri = new Uri(redirectionUrl);
            // Valida el contenido de la URL de redirección. 
            // En esta simple validación devolución de llamada, la URL de redirección se considera válida si utiliza HTTPS
            // para cifrar las credenciales de autenticación. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }

            return result;
        }
    }
}