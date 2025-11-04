using System;
using System.Data;
using MySql.Data.MySqlClient; // Asegúrate de instalar MySql.Data desde NuGet
using System.Configuration;

namespace WebItNow_Peacock
{
    public class ConexionBD_MySQL : System.Web.UI.Page
    {
        private MySqlConnection connection;
        private readonly string connectionString;

        protected ErrorManager errorManager = new ErrorManager();

        public ConexionBD_MySQL(string username, string password)
        {
            string server = Convert.ToString(Session["Server"]);
            string port = Convert.ToString(Session["Port"]);
            string database = Convert.ToString(Session["Database"]);
            string usuario = Convert.ToString(Session["User"]);
            string contraseña = Convert.ToString(Session["Password"]);

            // Ajusta la base de datos según el idioma
            switch (Session["Idioma"].ToString())
            {
                case "es-MX":
                    // No cambia: Español por defecto
                    break;
                case "pt-BR":
                    database += "_pt";
                    break;
                case "en-US":
                    database += "_en";
                    break;
            }

            connectionString = $"Server={server};Port={port};Database={database};User ID={usuario};Password={contraseña};";
            connection = new MySqlConnection(connectionString);
        }

        public MySqlConnection Connection => connection;    // Propiedad para obtener la conexión
        public void Open()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();

                    // Establecer lc_time_names en español
                    using (MySqlCommand comando = new MySqlCommand("SET lc_time_names = 'es_ES'", connection))
                    {
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                errorManager.LogError("Error de MySQL: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                errorManager.LogError("Operación inválida: " + ex.Message);
            }
            catch (Exception ex)
            {
                errorManager.LogError("Ocurrió un error: " + ex.Message);
            }

            if (errorManager.HasError)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "showError", $"showError('{errorManager.ErrorMessage}');", true);
            }
        }

        public void Close()
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                errorManager.LogError("Error de MySQL: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                errorManager.LogError("Operación inválida: " + ex.Message);
            }
            catch (Exception ex)
            {
                errorManager.LogError("Ocurrió un error: " + ex.Message);
            }

            if (errorManager.HasError)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "showError", $"showError('{errorManager.ErrorMessage}');", true);
            }
        }

        public MySqlCommand CreateCommand(string query)
        {
            return new MySqlCommand(query, connection);
        }

        public DataTable ExecuteQuery(string query)
        {
            Open();
            DataTable dt = new DataTable();
            using (MySqlCommand cmd = CreateCommand(query))
            {
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            Close();
            return dt;
        }

        public int ExecuteNonQuery(string query)
        {
            Open();
            using (MySqlCommand cmd = CreateCommand(query))
            {
                int result = cmd.ExecuteNonQuery();
                Close();

                return result;
            }
        }

        public int ExecuteNonQueryWithParameters(string query, Action<MySqlCommand> addParameters)
        {
            Open();
            using (MySqlCommand cmd = CreateCommand(query))
            {
                addParameters(cmd);

                int result = cmd.ExecuteNonQuery();
                Close();

                return result;
            }
        }

        public DataTable ExecuteQueryWithParameters(string query, Action<MySqlCommand> addParameters)
        {
            Open();
            using (MySqlCommand cmd = CreateCommand(query))
            {
                addParameters(cmd);

                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    Close();
                    return dt;
                }
            }
        }

        public MySqlDataReader ExecuteReaderQuery(string strQuery)
        {
            MySqlDataReader dr = null;

            try
            {
                // Usar el método Open() para abrir la conexión
                Open();

                // Crear el comando con la consulta y la conexión
                MySqlCommand cmd = new MySqlCommand(strQuery, connection);

                // Ejecutar la consulta y obtener el MySqlDataReader
                dr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                errorManager.LogError("Error al ejecutar la consulta: " + ex.Message);
            }

            return dr; // Devolver el MySqlDataReader
        }

        internal void AddParameter(string v, string wPrefijo_Aseguradora)
        {
            throw new NotImplementedException();
        }
    }
}
