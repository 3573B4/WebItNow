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

    }
}