using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;

namespace WebItNow
{
    public partial class Request_Document : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }
        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            // Validar que los campos hayan sido capturados
            if (TxtNom.Text != "" && TxtEmail.Text != "" && TxtReferencia.Text != "")
            {
                // Insertar Registo Tabla ITM_11 (Solicitud Documento)
                int result = Add_Solicitud(TxtNom.Text, TxtEmail.Text, TxtReferencia.Text, TxtTpoDocumento.Text);

                if (result == 0)
                {

                    Session["IdUsuario"] = TxtNom.Text;
                    Session["Asunto"] = "Solicitud Documento";
                    Session["Email"] = TxtEmail.Text.Trim();

                    Response.Redirect("Page_Message.aspx");

                    //var email = new EnvioEmail();
                    //int Envio_Ok = email.EnvioMensaje(TxtNom.Text.Trim(), TxtEmail.Text.Trim(), "Solicitud Documento", string.Empty);

                    //if (Envio_Ok == 0)
                    //{
                    //    LblMessage.Text = "Solicitud enviada correctamente ";
                    //    this.mpeMensaje.Show();
                    //}

                    Limpia(this.Controls);

                    Lbl_Message.Visible = false;
                }
            }
            else
            {
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "* Estos campos son obligatorios";
            }
        }

        public int Add_Solicitud(String pNombre, String pUsEmail, String pReferencia, String pTpoDocumento)
        {
            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            try
            {

                SqlCommand cmd1 = new SqlCommand("sp_tbSolicitud", Conecta.ConectarBD);
                cmd1.CommandType = CommandType.StoredProcedure;

                cmd1.Parameters.AddWithValue("@nombre", pNombre);
                cmd1.Parameters.AddWithValue("@email", pUsEmail);
                cmd1.Parameters.AddWithValue("@referencia", pReferencia);
                cmd1.Parameters.AddWithValue("@TpoDocumento", pTpoDocumento);

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

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }

    }
}