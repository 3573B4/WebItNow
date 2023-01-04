﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace WebItNow
{
    public class ConexionBD
    {
    //  string connString = "Data Source = DESKTOP-EF6AQB3; Initial Catalog = Itnow ; User ID=sa; Password=e22j22";
        string connString = "Server=tcp:codice1.database.windows.net,1433;Initial Catalog=Itnow;Persist Security Info=False; User ID=DB_Codice; Password=Itnow2023; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

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