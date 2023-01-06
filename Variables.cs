using System;
using System.Web;

namespace WebItNow
{
    public class Variables : IHttpModule
    {
        /// <summary>
        /// Deberá configurar este módulo en el archivo Web.config de su
        ///  web y registrarlo en IIS para poder usarlo. Para obtener más información
        /// consulte el vínculo siguiente: https://go.microsoft.com/?linkid=8101007
        /// </summary>
        /// 

        public static string wUsu;
        public static string wCve;
        public static string wPass;
        public static string wFileName;
        public static string wQuery;
        public static Boolean wDownload = true;

      //  public static int wPrivilegios;

        public struct UsPrivilegios
        {
            // 1 - Administrator
            // 2 - Consultant
            // 3 - UploadFiles
            public const int Administrator = 1;
            public const int Consultant = 2;
            public const int UploadFiles = 3;
        }

        public static int wTipo_Plaza;
        public static int wEstatus;

        public static float wFactor;

        // Carga Nomina
        public static int wNum_Emp;
        public static string wCodigo;
        public static string wAgrup;
        public static decimal wD_21_SDO;

        // Carga Totales Nomina
        public static int wId_Carga;
        public static int wCasos_B;
        public static decimal wAportaciones_B;
        public static int wCasos_C;
        public static decimal wAportaciones_C;


        #region Miembros de IHttpModule

        public void Dispose()
        {
            //Ponga aquí el código de limpieza.
        }

        public void Init(HttpApplication context)
        {
            // El siguiente es un ejemplo de cómo se puede controlar el evento LogRequest y proporcionar 
            // una implementación de registro personalizado para él
            context.LogRequest += new EventHandler(OnLogRequest);
        }

        #endregion

        public class Variables_Extraordinaria
        {
            public static string wNomina;
            public static string wQuincena;
            public static string wAño;
            public static string wDocumento;

            public static int wSubramo;
            public static int wCheque;
        }

        public void OnLogRequest(Object source, EventArgs e)
        {
            //Aquí puede poner la lógica de registro personalizado
        }

    }
}
