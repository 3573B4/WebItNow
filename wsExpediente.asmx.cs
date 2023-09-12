using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;

namespace WebItNow
{

    public class wsExpediente : System.Web.Services.WebService
    {
        //public string HelloWorld()
        //{
        //    return "Hola a todos";
        //}

        //public static string[] GetCompletionList(string prefixText, int count, string contextKey)
        //{
        //    List<String> referencia = SearchReference(prefixText, count);
        //    return referencia.ToArray();
        //}

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static List<string> SearchReference(string prefixText, int count)
        {

            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            string strQuery = "SELECT a.Referencia FROM ITM_33 as a WHERE a.Referencia like @SearchText + '%'";
            SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

            cmd.Parameters.AddWithValue("@SearchText", prefixText);
            cmd.Connection = Conecta.ConectarBD;
            Conecta.ConectarBD.Open();

            List<string> referencia = new List<string>();

            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                while (sdr.Read())
                {
                    referencia.Add(sdr["ContactName"].ToString());
                }
            }

            Conecta.Cerrar();

            return referencia;
        }

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }
    }
}
