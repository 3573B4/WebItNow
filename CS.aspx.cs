using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    [WebMethod]
    public static List<string> SearchCustomers(string prefixText, int count)
    {
        using (SqlConnection conn = new SqlConnection())
        {

            string connString = "Server=tcp:codice1.database.windows.net,1433;Initial Catalog=Itnow_Test;Persist Security Info=False;User ID=DB_Codice;Password=Itnow2023;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            conn.ConnectionString = connString;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT Nombre FROM ITM_19 WHERE Nombre like @SearchText +'%'";
                cmd.Parameters.AddWithValue("@SearchText", prefixText);
                cmd.Connection = conn;
                conn.Open();

                List<string> customers = new List<string>();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        customers.Add(sdr["Nombre"].ToString());
                    }
                }
                conn.Close();

                return customers;
            }
        }
    }
}

