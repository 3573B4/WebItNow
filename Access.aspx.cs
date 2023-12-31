﻿using System;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;

namespace WebItNow
{
	public partial class Access : System.Web.UI.Page
	{
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Session["DownloadsPath"] = GetDownloadFolderPath();

            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.Form.Attributes.Add("autocomplete", "off");

                // Permisos Usuario
                System.Web.HttpContext.Current.Session["UsPrivilegios"] = "3";

                //LblMessage.Text = (string)Session["DownloadsPath"];
                //this.mpeMensaje.Show();
            }
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            if (TxtRef.Text == "" || TxtEmail.Text == "" || txtVerificationCode.Text == "")
            {
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "* Estos campos son obligatorios";

                lblCaptchaMessage.Text = "";
                //LblMessage.Text = "Debes capturar Usuario / Clave / " + "<br/>" + "Código de verificación.";
                //this.mpeMensaje.Show();
            }
            else if (txtVerificationCode.Text.ToLower() != Session["CaptchaVerify"].ToString())
            {
                lblCaptchaMessage.Text = "Por favor ingrese el captcha correcto";
                lblCaptchaMessage.ForeColor = System.Drawing.Color.Red;
            }
            else
            {

                string result = Autenticar(TxtRef.Text, TxtEmail.Text);

                if (result != null)
                {

                    // IdUsuario
                    System.Web.HttpContext.Current.Session["Referencia"] = TxtRef.Text;
                    // Permisos Usuario
                    System.Web.HttpContext.Current.Session["UsPrivilegios"] = result;

                    // Permisos Usuario
                    // System.Web.HttpContext.Current.Session["UsPrivilegios"] = dr1["UsPrivilegios"].ToString().Trim();

                    string UsPrivilegios = Convert.ToString(Session["UsPrivilegios"]);

                    if (UsPrivilegios == "3")
                    {
                        Response.Redirect("Upload_Files.aspx");
                    }
                    else
                    {
                        // Response.Redirect("menu.aspx");
                    }

                    Lbl_Message.Visible = false;

                }
                else
                {
                    // "Usuario y/o Contraseña Incorrectos";
                    LblMessage.Text = "No fue posible iniciar sesión." + "<br/>" + "Confirme su Correo electrónico ó Referencia.";
                    this.mpeMensaje.Show();
                }
            }
        }

        protected void BtnRegistrarse_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }

        public string Autenticar(String pReferencia, String pEmail)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_Acceso", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@referencia", pReferencia);
                cmd1.Parameters.AddWithValue("@email", pEmail);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                while (dr1.Read())
                {
                    return dr1["UsPrivilegios"].ToString().Trim();
                }
                //if (dr1.Read())
                //{

                //    return dr1.GetInt32(0);

                //}

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

            }
            catch (Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }

            return null;
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        public static string GetHomePath()
        {
            // Not in .NET 2.0
            // System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                return System.Environment.GetEnvironmentVariable("HOME");

            return System.Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        }

        public static string GetDownloadFolderPath()
        {
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                string pathDownload = System.IO.Path.Combine(GetHomePath(), "Downloads");
                return pathDownload;
            }

            return System.Convert.ToString(
                Microsoft.Win32.Registry.GetValue(
                     @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders"
                    , "{374DE290-123F-4565-9164-39C4925E467B}"
                    , String.Empty
                )
            );
        }
    }
}