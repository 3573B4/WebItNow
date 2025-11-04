using System;
using System.Web;
using System.IO;

namespace WebItNow_Peacock   // <- Agregar el namespace correcto
{
    public class fwFileHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                // 1. Obtener ruta desde querystring
                string filePath = context.Request.QueryString["path"];
                if (string.IsNullOrEmpty(filePath))
                {
                    context.Response.StatusCode = 400;
                    context.Response.Write("Falta parámetro 'path'.");
                    return;
                }

                // 2. Decodificar URL
                filePath = HttpUtility.UrlDecode(filePath);

                // 3. Validar existencia del archivo
                if (!File.Exists(filePath))
                {
                    context.Response.StatusCode = 404;
                    context.Response.Write("Archivo no encontrado.");
                    return;
                }

                string fileName = Path.GetFileName(filePath);
                string extension = Path.GetExtension(filePath).ToLower();

                // 4. Definir Content-Type y disposition según extensión
                string contentType;
                string disposition = "inline"; // por defecto abrir en navegador

                switch (extension)
                {
                    case ".pdf":
                        contentType = "application/pdf";
                        disposition = "inline";
                        break;
                    case ".jpg":
                    case ".jpeg":
                        contentType = "image/jpeg";
                        disposition = "inline";
                        break;
                    case ".png":
                        contentType = "image/png";
                        disposition = "inline";
                        break;
                    case ".doc":
                        contentType = "application/msword";
                        disposition = "attachment";
                        break;
                    case ".docx":
                        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        disposition = "attachment";
                        break;
                    case ".xls":
                        contentType = "application/vnd.ms-excel";
                        disposition = "attachment";
                        break;
                    case ".xlsx":
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        disposition = "attachment";
                        break;
                    default:
                        contentType = "application/octet-stream";
                        disposition = "attachment";
                        break;
                }

                // 5. Enviar archivo al cliente
                context.Response.Clear();
                context.Response.ContentType = contentType;
                context.Response.AddHeader("Content-Disposition", $"{disposition}; filename=\"{fileName}\"");
                context.Response.TransmitFile(filePath);
                context.Response.Flush();

                // 6. Intentar eliminar archivo temporal después de enviar
                try { File.Delete(filePath); } catch { /* ignorar errores si está en uso */ }

                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write("Error interno: " + ex.Message);
            }
        }

        public bool IsReusable { get { return false; } }
    }
}


