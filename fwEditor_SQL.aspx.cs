using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace WebItNow_Peacock
{
    public partial class fwEditor_SQL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnExecute_Click(object sender, EventArgs e)
        {

            string query = txtQuery.Text.Trim();

            // Validación de palabras clave restringidas
            // string[] restrictedKeywords = { "DROP", "TRUNCATE", "ALTER", "GRANT", "REVOKE" };
            // foreach (var keyword in restrictedKeywords)
            // {
            //     if (query.ToUpper().Contains(keyword))
            //     {
            //         // Mostrar alerta si se detecta una palabra clave restringida
            //         ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Operación no permitida: {keyword}.');", true);
            //         return; // Salir del método sin ejecutar la consulta
            //     }
            // }

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = txtQuery.Text.Trim();

            try
            {
                using (MySqlCommand command = new MySqlCommand(strQuery, dbConn.Connection))
                {
                    if (query.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                    {
                        // Ejecutar SELECT y llenar el GridView
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            gvResults.DataSource = dt;
                            gvResults.DataBind();
                        }
                    }
                    else
                    {
                        // Ejecutar comandos de tipo INSERT, UPDATE, DELETE
                        int rowsAffected = command.ExecuteNonQuery();
                        gvResults.DataSource = null;
                        gvResults.DataBind();

                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Query ejecutado. Filas afectadas: {rowsAffected}');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('Error: {ex.Message}');", true);
            }
        }



    }
}