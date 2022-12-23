using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace WebItNow
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }
        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
			//* Validar si el usuario existe o es nuevo
			if (TxtUsu.Text != "" && TxtEmail.Text != "" && TxtPass.Text != "")
			    {

                //validar si existe Usuario en la tabla tbUsuarios


                // Insertar Registo Tabla tbUsuarios (UploadFiles)
                int result = Add_tbUsuarios(TxtUsu.Text, TxtEmail.Text, TxtPass.Text, 3, "Insert");
                    
                    if (result == 0)
                    {
                    // Insertar Registros Tabla tbEstadoDocumento
                    int idStatus = 1;
                    int valor = Add_tbEstadoDocumento(TxtUsu.Text, idStatus);

                    EnvioMensaje(TxtUsu.Text.Trim(), TxtEmail.Text.Trim());

                    LblMessage.Text = "Usuario fue insertado correctamente ";
                    this.mpeMensaje.Show();

                    //string script = @"<script type='text/javascript'>
                    //        alert('Usuario fue agregado correctamente'); </script>";

                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);

                    //string script = "alert('Usuario fue insertado correctamente');";
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, true);

                    Limpia(this.Controls);

                     // Response.Redirect("Login.aspx");
                    }

                    Lbl_Message.Visible = false;
            }
				else
				{
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "* Estos campos son obligatorios";
                   // LblMessage.Text = "Debes captura Usuario / Password.";
                   // this.mpeMensaje.Show();
				}
			}

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        public void Limpia(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
                else if (control is DropDownList)
                    ((DropDownList)control).Items.Clear();
                else if (control is RadioButtonList)
                    ((RadioButtonList)control).ClearSelection();
                else if (control is CheckBoxList)
                    ((CheckBoxList)control).ClearSelection();
                else if (control is RadioButton)
                    ((RadioButton)control).Checked = false;
                else if (control is CheckBox)
                    ((CheckBox)control).Checked = false;
                else if (control.HasControls())
                    //Esta linea detécta un Control que contenga otros Controles
                    //Así ningún control se quedará sin ser limpiado.
                    Limpia(control.Controls);
            }

        }

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }

        public int Add_tbUsuarios(String pUsuarios, String pUsEmail, String pUsPassword, int pUsPrivilegios, string pStatementType)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbUsuario_StatementType", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@usuario", pUsuarios);
                cmd1.Parameters.AddWithValue("@email", pUsEmail);
                cmd1.Parameters.AddWithValue("@password", pUsPassword);
                cmd1.Parameters.AddWithValue("@privilegios", pUsPrivilegios);
                cmd1.Parameters.AddWithValue("@StatementType", pStatementType);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                if (dr1.Read())
                {

                    return dr1.GetInt32(0);

                }

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

                return 0;

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

            return -1;
        }

        public int Add_tbEstadoDocumento(String pUsuarios, int pIdStatus)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string strQuery = "SELECT IdTpoDocumento, Descripcion FROM tbTpoDocumento";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

              //SqlDataReader dr = cmd.ExecuteReader();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {

                    string IdTpoDocumento = Convert.ToString(row[0]);

                    // Insert tabla tbEstadoDocumento
                    SqlCommand cmd1 = new SqlCommand("Insert into tbEstadoDocumento (IdUsuario, IdTipoDocumento, IdStatus) " +
                                        "Values ('" + pUsuarios + "', '" + IdTpoDocumento + "', " + pIdStatus + ")", Conecta.ConectarBD);

                    SqlDataReader dr1 = cmd1.ExecuteReader();

                    cmd1.Dispose();
                    dr1.Dispose();
                }

                Conecta.Cerrar();

                return 0;

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

            return -1;
        }

        public int EnvioMensaje(String pUsuarios, String pEmail)
        {
            string wDe = "martin.baltierra@itnow.mx";
            string wPara = pEmail;
            string wCC = "martin.baltierra@itnow.mx";
            string wAsunto = "Registro Usuario " + pUsuarios;
            string wMensaje = "Mensaje de Registro";

            System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
            correo.From = new System.Net.Mail.MailAddress(wDe);
            correo.To.Add(wPara);
            correo.CC.Add(wCC);
            correo.Subject = wAsunto;
            wMensaje += "\n\nFecha y hora GMT: " +
                DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss");
            correo.Body = wMensaje;
            correo.IsBodyHtml = false;
            correo.Priority = System.Net.Mail.MailPriority.Normal;
            //
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

            //
            //---------------------------------------------
            // Servidor de correo , Usuario, Password
            //---------------------------------------------
            smtp.Host = "smtp.ionos.mx";
            smtp.Port = 587; //465; //25
            smtp.Credentials = new System.Net.NetworkCredential("martin.baltierra@itnow.mx", "Baltierra2022#");
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(correo);
                Lbl_Message.Text = "Mensaje enviado satisfactoriamente";

                return 0;
            }
            catch (Exception ex)
            {
                Lbl_Message.Text = "ERROR: " + ex.Message;

                return 1;
            }
        }
    }
}