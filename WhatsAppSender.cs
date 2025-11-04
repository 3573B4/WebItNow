using System;
using System.Configuration;

using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

using System.Threading.Tasks;

using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


namespace WebItNow_Peacock
{
    public class WhatsAppSender
    {
        private static readonly string token = ConfigurationManager.AppSettings["WHATSAPP_TOKEN"];
        private static readonly string phoneNumberId = ConfigurationManager.AppSettings["PHONE_NUMBER_ID"];
        private static readonly string apiVersion = ConfigurationManager.AppSettings["WHATSAPP_API_VERSION"];
        private static readonly string apiUrl = $"https://graph.facebook.com/{apiVersion}/{phoneNumberId}/messages";

        private const string accountSid = "AC0dc719b93d0beb278eb8d6faefbc436b";         // "TU_ACCOUNT_SID"
        private const string authToken = "456e2f667c16dd4fa19e8234cfd942da";            // "TU_AUTH_TOKEN"

        private const string fromWhatsAppNumber = "whatsapp:+14155238886";              // Número oficial Twilio
        
        // private const string fromWhatsAppNumber = "whatsapp:+19782073046";           // Número oficial Twilio
        // +19782073046
        // private static bool isInitialized = false;

        public static async Task<string> EnviarLinkAsync(string telefonoDestino, string urlConToken)
        {
            using (var client = new HttpClient())
            {
                // Autenticación con token de WhatsApp
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                // Construcción del payload
                var payload = new
                {
                    messaging_product = "whatsapp",
                    to = telefonoDestino,
                    type = "text",
                    text = new
                    {
                        body = $"Hola, por favor sube la información de tu siniestro aquí: {urlConToken}"
                    }
                };

                // Serializar a JSON
                string json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar petición
                var response = await client.PostAsync(apiUrl, content);
                string respBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"WhatsApp API error: {response.StatusCode} - {respBody}");

                return respBody;
            }
        }

        public static void SendMessage(string to, string body)
        {
            const string accountSid = "AC0dc719b93d0beb278eb8d6faefbc436b";     // "TU_ACCOUNT_SID"
            const string authToken = "456e2f667c16dd4fa19e8234cfd942da";        // "TU_AUTH_TOKEN"

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                from: new PhoneNumber("whatsapp:+14155238886"),                 // Sandbox Twilio
                to: new PhoneNumber("whatsapp:" + to),
                body: body
            );

            Console.WriteLine(message.Sid);
        }

        public static void EnviarWhatsApp(string telefonoDestino, string urlConToken)
        {
            // 1. Inicializa las credenciales
            TwilioClient.Init(accountSid, authToken);

            try
            {
                // 2. Envía el mensaje
                var message = MessageResource.Create(
                from: new PhoneNumber(fromWhatsAppNumber),
                to: new PhoneNumber("whatsapp:" + telefonoDestino),
                body: $"Hola, por favor sube la documentación de tu siniestro en el siguiente link: {urlConToken}"
                );

                //Console.WriteLine(message.Sid);
                //Console.WriteLine("Status del mensaje: " + message.Status);
                //Console.WriteLine("ErrorCode: " + message.ErrorCode);
                //Console.WriteLine("ErrorMessage: " + message.ErrorMessage);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar WhatsApp: " + ex.Message);
            }

        }

        public static async Task<string> EnviarWhatsAppMeta(string telefonoDestino, string mensaje)
        {
            string token = "EAAVGmSYNA8QBPuVwxGw9HXzRFTzqiYB6CBdq4SPrM2ndw5YmoLnUKJjKiJHXp39jWZCNZBFiqnoEjODjPFIYnrkraKjeJPIBQnO" +
                           "GIbACC3gv3BbUILK9qhtKMLvuGC4ZCYO5ushz8zZC7mLTDPPBQdOoNOtCOGGWKGzcjVzVbNL82aX8Q5BZBgf4NG9F5IFq8WgZDZD";   // De Business Manager

            string phoneNumberId = "804953145675418";    // ID de tu número Business en Meta

            string apiUrl = $"https://graph.facebook.com/v21.0/{phoneNumberId}/messages";


            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var payload = new
                {
                    messaging_product = "whatsapp",
                    to = telefonoDestino,
                    type = "text",
                    text = new { body = mensaje }
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                return await response.Content.ReadAsStringAsync();
            }

        }

    }
}