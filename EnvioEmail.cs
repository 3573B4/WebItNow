using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;


namespace WebItNow_Peacock
{
    public class EnvioEmail
    {
        public string CorreoElectronico(string pReferencia)
        {
            string sEmail = string.Empty;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            try
            {
                // Acceder a la conexión directamente desde la instancia dbConn
                MySqlConnection conn = dbConn.Connection;

                // Consulta en la tabla Usuarios
                string query = "SELECT UsEmail FROM ITM_03 WHERE UsReferencia LIKE CONCAT('%', @pReferencia, '%')";
                using (MySqlCommand cmd1 = new MySqlCommand(query, conn))
                {
                    // Agregar parámetro
                    cmd1.Parameters.AddWithValue("@pReferencia", pReferencia);

                    using (MySqlDataReader dr1 = cmd1.ExecuteReader())
                    {
                        if (dr1.HasRows)
                        {
                            while (dr1.Read())
                            {
                                sEmail = dr1["UsEmail"].ToString().Trim();
                            }
                        }
                    }
                }
            }
            finally
            {
                // Cerrar la conexión
                dbConn.Close();
            }

            // Asegurar que siempre se retorna un valor
            return sEmail;
        }

        public string CorreoElectronico_User(string pUsuario)
        {
            string sEmail = string.Empty;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            try
            {
                // Acceder a la conexión directamente desde la instancia dbConn
                MySqlConnection conn = dbConn.Connection;

                // Consulta en la tabla Usuarios
                string query = "Select UsEmail From ITM_02 Where IdUsuario = '" + pUsuario + "'";
                using (MySqlCommand cmd1 = new MySqlCommand(query, conn))
                {
                    // Agregar parámetro
                    cmd1.Parameters.AddWithValue("@pUsuario", pUsuario);

                    using (MySqlDataReader dr1 = cmd1.ExecuteReader())
                    {
                        if (dr1.HasRows)
                        {
                            while (dr1.Read())
                            {
                                sEmail = dr1["UsEmail"].ToString().Trim();
                            }
                        }
                    }
                }
            }
            finally
            {
                // Cerrar la conexión
                dbConn.Close();
            }

            // Asegurar que siempre se retorna un valor
            return sEmail;
        }

        public int EnvioMensaje_bk(string pEmail, string pAsunto, string pBody, string wAdjunto)
        {

            try
            {
                string smtpServer = "smtp.office365.com";
                int smtpPort = 587;
                string emailFrom = "desarrollo@peacock.claims";     // Cambia por tu correo
                string password = "SVflc055";                       // Cambia por tu contraseña o contraseña de aplicación

                string emailTo = pEmail;

                MailMessage mail = new MailMessage(emailFrom, emailTo)
                {
                    Subject = pAsunto,
                    Body = pBody,
                    IsBodyHtml = true
                };

                SmtpClient smtp = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(emailFrom, password),
                    EnableSsl = true // Asegúrate de que SSL esté habilitado
                };

                // Agregar el archivo adjunto
                Attachment attachment = new Attachment(wAdjunto);
                mail.Attachments.Add(attachment);

                smtp.Send(mail);
                Console.WriteLine("Correo enviado correctamente.");

                // Liberar recursos después de enviar el correo
                attachment.Dispose(); // Cerrar y liberar el archivo adjunto

                return 0;
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"Error SMTP: {smtpEx.Message}");
                Console.WriteLine($"Estado SMTP: {smtpEx.StatusCode}");
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                return 1;
            }

        }

        public int EnvioMensaje(string pEmail, string pAsunto, string pBody, string wAdjunto)
        {
            // string wDe = "archivo@peacock.claims";
            string wDe = "desarrollo@peacock.claims";
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

            // Agregar el archivo adjunto
            Attachment attachment = new Attachment(wAdjunto);
            correo.Attachments.Add(attachment);

            correo.IsBodyHtml = true;
            correo.Priority = System.Net.Mail.MailPriority.Normal;
            //
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

            //
            //---------------------------------------------
            // Servidor de correo , Usuario, Password
            //---------------------------------------------
            smtp.Host = "smtp.office365.com";
            // smtp.Host = "smtp-mail.outlook.com";
            smtp.Port = 587; //465; //25

            //tOaf$p2403#
            // smtp.Credentials = new System.Net.NetworkCredential("archivo@peacock.claims", "CVbnM0p24");
            smtp.Credentials = new System.Net.NetworkCredential("desarrollo@peacock.claims", "SVflc055");
            smtp.EnableSsl = true;

            // tAafjp2304#
            try
            {
                smtp.Send(correo);
                // System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;

                return 0;
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"Error SMTP: {smtpEx.Message}");
                Console.WriteLine($"Estado SMTP: {smtpEx.StatusCode}");
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                return 1;
            }
        }

    }
}