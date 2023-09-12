using AjaxControlToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace WebItNow
{
    /// <summary>
    /// Descripción breve de WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }

        [WebMethod(true)]
        [ScriptMethod]
        public string[] GetCompletionList(string prefixText)
        {

            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            string sqlQuery = "SELECT UsReferencia " +
                                    " FROM ITM_02 " +
                                    " WHERE UsReferencia like '%" + prefixText + "%'";

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            string[] cntName = new string[dt.Rows.Count];
            int i = 0;
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    cntName.SetValue(row["UsReferencia"].ToString(), i);
                    i++;
                }
            }
            catch { }
            finally
            {
                Conecta.Cerrar();
            }

            return cntName;
        }

        [WebMethod(true)]
        [ScriptMethod]
        public string[] GetSuggestions(string prefixText)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string sqlQuery = "SELECT Referencia, IdAsegurado " +
                                        " FROM ITM_33 " +
                                        " WHERE Referencia like @Referencia";


                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                cmd.Parameters.AddWithValue("@Referencia", prefixText + "%");
                Conecta.Abrir();

                SqlDataReader dr = cmd.ExecuteReader();

                List<string> custList = new List<string>();
                string custItem = string.Empty;

                while (dr.Read())
                {
                    custItem = AutoCompleteExtender.CreateAutoCompleteItem(dr[0].ToString(), dr[1].ToString());
                    custList.Add(custItem);

                }

                Conecta.Cerrar();
                dr.Close();

                return custList.ToArray();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
