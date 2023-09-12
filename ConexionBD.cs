using System;
using System.Data.SqlClient;

namespace WebItNow
{
    public class ConexionBD
    {
        // * *  Autenticacion de SQL (Itnow_Test - Local)
        string connString = "Data Source = ITNW5CD8185HPF; Initial Catalog = NivelArchivos ; User ID=sa; Password=0408";

        // * *  Autenticacion de SQL (Itnow_Test - Azure)
        //string connString = "Server=tcp:codice1.database.windows.net,1433;Initial Catalog=Itnow_Test;Persist Security Info=False;User ID=DB_Codice;Password=Itnow2023;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public SqlConnection ConectarBD = new SqlConnection();

        public ConexionBD()
        {
            ConectarBD.ConnectionString = connString;
        }

        public void Abrir()
        {
            try
            {
                ConectarBD.Open();
                Console.WriteLine("Conexion abierta");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al abrir la BD" + ex.Message);
            }
        }

        public void Cerrar()
        {
            ConectarBD.Close();
        }
    }
}