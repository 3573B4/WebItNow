using System;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace WebItNow_Peacock
{
    public class TokenManager
    {

        public (string token, string passwordTemporal) GenerarYGuardarToken(int idAsunto, int horasValidez = 24)
        {

            // Abrir conexión utilizando tu clase personalizada
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            // Token seguro (64 hex chars)
            byte[] tokenData = new byte[32];
            
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(tokenData);

            string token = BitConverter.ToString(tokenData).Replace("-", "").ToLower();

            // Contraseña temporal de 8 dígitos
            string passwordTemporal = GenerarPasswordTemporal(8);
            //string passwordTemporal = new Random().Next(100000, 999999).ToString();

            try
            {
                dbConn.Open();

                string strQuery = @"INSERT INTO ITM_Tokens (IdAsunto, Token, PasswordTemporal, FechaExpira, Usado)
                                    VALUES (@IdAsunto, @Token, @Password, DATE_ADD(NOW(), INTERVAL @Horas HOUR), 0);";

                using (MySqlCommand cmd = new MySqlCommand(strQuery, dbConn.Connection))
                {
                    cmd.Parameters.AddWithValue("@IdAsunto", idAsunto);
                    cmd.Parameters.AddWithValue("@Token", token);
                    cmd.Parameters.AddWithValue("@Password", passwordTemporal);
                    cmd.Parameters.AddWithValue("@Horas", horasValidez);

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                dbConn.Close();
            }
            return (token, passwordTemporal);
        }

        // Generar contraseña temporal segura
        private string GenerarPasswordTemporal(int longitud = 10)
        {
            const string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789#$";
            Random random = new Random();
            char[] password = new char[longitud];

            for (int i = 0; i < longitud; i++)
            {
                password[i] = caracteres[random.Next(caracteres.Length)];
            }

            return new string(password);
        }

        // Método para marcar token como usado (cuando el asegurado finaliza)
        public void MarcarTokenUsado(string token)
        {
            // Abrir conexión utilizando tu clase personalizada
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            try
            {
                dbConn.Open();

                string query = "UPDATE ITM_Tokens SET Usado = 1 WHERE Token = @Token";

                using (var cmd = new MySqlCommand(query, dbConn.Connection))
                {
                    cmd.Parameters.AddWithValue("@Token", token);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                dbConn.Close();
            }
        }
    }
}