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
    public partial class Forgot_Password : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.Form.Attributes.Add("autocomplete", "off");
                TxtUsu.Focus();
            }

        }

        public void OnTextChanged(object sender, EventArgs e)
        {
            string wPrivilegios = System.Web.HttpContext.Current.Session["UsPrivilegios"] as string;
            //Reference the TextBox.
            TextBox textBox = sender as TextBox;

            //Get the ID of the TextBox.
            //string id = textBox.ID;

            //Display the Text of TextBox.
            // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + textBox.Text + "');", true);
            // ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "alert('" + Lbl_Message.Text + "');", true);
            // TxtPass.Text = Server.HtmlEncode(TxtUsu.Text);

            if (TxtUsu.Text != "")
            {

                Lbl_Message.Visible = true;

                // Insertar Registo Usuario Cargas
                int result = ValidaUser(TxtUsu.Text, Int32.Parse(wPrivilegios));

                if (result == 0)
                {
                    TxtUsu.Text = string.Empty;
                    //Lbl_Message.Text = "* El nombre de usuario es incorrecto";
                    LblMessage.Text = "El nombre de usuario es incorrecto";
                    this.mpeMensaje.Show();
                }
                else
                {
                    Lbl_Message.Text = "";
                    Lbl_Message.Visible = false;
                    TxtPass.Focus();
                }

            }
            else
            {
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "* Este campo es obligatorio";
            }

        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            if (TxtPass.Text == "" || TxtConfPass.Text == "")
            {
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "* Estos campos son obligatorios";
            }
            else
            {

                if (TxtPass.Text == TxtConfPass.Text)
                {
                    int result = UpdatePassword(TxtUsu.Text, TxtPass.Text, 3, "Update");
                    if (result == 0)
                    {
                        LblMessage.Text = "Contraseña se actualizo correctamente ";
                        this.mpeMensaje.Show();

                        Limpia(this.Controls);
                    }
                    Lbl_Message.Visible = false;
                }
                else 
                {
                    Lbl_Message.Visible = true;
                    Lbl_Message.Text = "* Contraseñas son diferentes";
                }
            }
        }

        public int ValidaUser(String pUsuarios, int pUsPrivilegios)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbUsuario", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@usuario", pUsuarios);
                cmd1.Parameters.AddWithValue("@privilegios", pUsPrivilegios);
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

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }

        public int UpdatePassword(String pUsuarios, String pUsPassword, int pUsPrivilegios, string pStatementType)
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

    }
}