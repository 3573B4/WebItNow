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
			if (TxtUsu.Text != "" && TxtPass.Text != "")
			    {

                    // Insertar Registo Usuario Cargas
                    int result = Registrar(TxtUsu.Text, TxtPass.Text, 3, "Insert");

                    if (result == 0)
                    {
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

        public int Registrar(String pUsuarios, String pUsPassword, int pUsPrivilegios, string pStatementType)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbUsuario_StatementType", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@usuario", pUsuarios);
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            //* Validar si el usuario existe o es nuevo
            if (TxtUsu.Text != "" && TxtPass.Text != "")
            {

                // Insertar Registo Usuario Cargas
                int result = Registrar(TxtUsu.Text, TxtPass.Text, 3, "Insert");

                if (result == 0)
                {
                    //   LblMessage.Text = "Usuario fue insertado correctamente ";
                    //   this.mpeMensaje.Show();

                    string script = @"<script type='text/javascript'>
                            alert('Usuario fue agregado correctamente'); </script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);


                    //string script = "alert('Usuario fue insertado correctamente');";
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, true);

                    Limpia(this.Controls);

                    // Response.Redirect("Acceso.aspx");
                }

            }
            else
            {
                LblMessage.Text = "Debes captura Usuario / Password.";
                this.mpeMensaje.Show();
            }
        }
    }
}