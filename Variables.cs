using System;
using System.Web;

namespace WebItNow_Peacock
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
        public static string wRef;
        public static int wSubRef;
        public static string wFileName;
        public static string wTpoDocumento;
        public static string wURL_Imagen;
        public static string wIdCarpeta;
        public static string wIdColumna;
        public static string wNomAsegurado;
        public static string wDesc_Documento;
        public static string wEmail;
        public static string wRef_Proyecto;
        public static string wPrefijo_Aseguradora;
        public static string wDesc_Aseguradora;

        public static string wCve;
        public static string wPass;
        public static string wQuery;
        public static string wPrivilegios;
        public static string wEdoDoc;
        public static string wTabla;
        public static string wPrefijo;

        public static Boolean wDownload = true;
        public static Boolean wExiste = true;

        public static Boolean wContinuar = false;
        public static Boolean wPoliza = false;
        public static Boolean wAsegurado = false;

        public static Boolean wICD = false;
        public static Boolean wCPT = false;
        public static Boolean wEstudios = false;
        public static Boolean wServicios = false;
        public static Boolean wProveedor = false;
        public static Boolean wPaquete = false;

        public static int wProceso;
        public static int wIdNota;
        public static int wRenglon;
        public static int wIdAsunto;
        public static int wIdAseguradora;
        public static int wIdTpoDocumento;
        public static int wIdProyecto;
        public static int wIdTpoProyecto;
        public static int wIdConclusion;
        public static int wIdRegimen;
        public static int wIdDocumento;
        public static int wSeccion;
        public static int wIdConsecutivo;
        public static int wCrear;
        public static int wEliminar;

        public static int wIdTpoAsunto;

        public static int wIdentificadorBtn;
        public static int wIdArchivo;

        public static string wUserName;         // Conexion BD
        public static string wPassword;         // Conexion BD
        public static string wUserLogon;
        public static string wPassLogon;

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
        public static int wCasos_C;
        public static decimal wAportaciones_B;
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
