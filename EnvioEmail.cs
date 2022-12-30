using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebItNow
{
    public class EnvioEmail
    {
        public int EnvioMensaje(String pUsuarios, String pEmail, string pMensaje)
        {
            string wDe = "martin.baltierra@itnow.mx";
            string wPara = pEmail;
            string wCC = "martin.baltierra@itnow.mx";
            string wAsunto = pMensaje;
            string wMensaje = "Mensaje de Registro";

            System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
            correo.From = new System.Net.Mail.MailAddress(wDe);
            correo.To.Add(wPara);
            correo.CC.Add(wCC);
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
            smtp.Host = "smtp.ionos.mx";
            smtp.Port = 587; //465; //25
            smtp.Credentials = new System.Net.NetworkCredential("martin.baltierra@itnow.mx", "Baltierra2022#");
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(correo);
             // Lbl_Message.Text = "Mensaje enviado satisfactoriamente";

                return 0;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
              // Lbl_Message.Text = "ERROR: " + ex.Message;
                return 1;
            }
        }
    }
}