using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace WebItNow_Peacock
{
    public static class EmailSender
    {
        private static readonly string smtpHost = ConfigurationManager.AppSettings["SMTP_HOST"];
        private static readonly int smtpPort = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"]);
        private static readonly string smtpUser = ConfigurationManager.AppSettings["SMTP_USER"];
        private static readonly string smtpPass = ConfigurationManager.AppSettings["SMTP_PASS"];
        private static readonly string fromEmail = ConfigurationManager.AppSettings["FROM_EMAIL"];

        public static void EnviarContrasena(string paraEmail, string passwordTemporal, string referencia)
        {
            string subject = "Contraseña temporal para subir documentación";

            string body = $@"Hola, Tu contraseña temporal para subir la información del siniestro (referencia: {referencia}) es: 
            {passwordTemporal}
            Este código expira en 48 horas. No compartas esta contraseña por favor.
            Saludos.";

            using (var msg = new MailMessage(fromEmail, paraEmail, subject, body))
            {
                using (var client = new SmtpClient(smtpHost, smtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    client.Send(msg);
                }
            }
        }
    }
}