using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;

namespace WebItNow
{
    public partial class Request_Document_1 : System.Web.UI.Page
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
                if (Convert.ToString(Session["Referencia"]) == string.Empty)
                {
                    TxtReferencia.ReadOnly = false;
                    TxtEmail.ReadOnly = false;
                    TxtNom.ReadOnly = false;
                }
                else
                {
                    TxtReferencia.ReadOnly = true;
                    TxtEmail.ReadOnly = true;
                    TxtNom.ReadOnly = true;

                    TxtReferencia.Text = Convert.ToString(Session["Referencia"]);
                    TxtEmail.Text = Convert.ToString(Session["Email"]);
                    TxtNom.Text = Convert.ToString(Session["Aseguradora"]);

                }
                GetTpoDocumento();
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
            if (TxtReferencia.Text != "" && TxtNom.Text != "" && TxtEmail.Text != "" )
            {
                // Insertar Registo Tabla ITM_11 (Solicitud Documento)
                int result = Add_Solicitud(TxtNom.Text, TxtEmail.Text, TxtReferencia.Text, ddlTpoDocumento.SelectedValue);

                if (result == 0)
                {

                    Session["Referencia"] = TxtReferencia.Text;
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

        public void OnTextChanged(object sender, EventArgs e)
        {

            Variables.wPrivilegios = System.Web.HttpContext.Current.Session["UsPrivilegios"] as string;
            //Reference the TextBox.
            TextBox textBox = sender as TextBox;

            if (TxtReferencia.Text != "")
            {

                Lbl_Message.Visible = true;

                // Insertar Registo Usuario Cargas
                int result = ValidarReferencia(TxtReferencia.Text, Int32.Parse(Variables.wPrivilegios));

                if (result == 0)
                {
                    TxtReferencia.Text = string.Empty;
                    TxtEmail.Text = string.Empty;
                    TxtNom.Text = string.Empty;

                    LblMessage.Text = "Ingrese una referencia valida";
                    this.mpeMensaje.Show();
                }
                else
                {
                    Lbl_Message.Text = "";
                    Lbl_Message.Visible = false;
                    TxtReferencia.Focus();
                }

            }
            else
            {
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "* Este campo es obligatorio";
            }

        }

        public int ValidarReferencia(String pReferencia, int pUsPrivilegios)
        {

            try
            {

                ConexionBD Conecta = new ConexionBD();
                NewMethod(Conecta);

                // Consulta a la tabla : Usuarios = ITM_02
                string strQuery = "SELECT UsReferencia, UsEmail, UsAsegurado " +
                                  "  FROM ITM_02 ed " +
                                  " WHERE UsReferencia LIKE '%' + '" + pReferencia + "'  + '%' ";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    TxtEmail.Text = Convert.ToString(row[1]);
                    TxtNom.Text = Convert.ToString(row[2]);

                    return 1;
                }

                cmd.Dispose();
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

            return 0;
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

        protected void ddlTpoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void GetTpoDocumento()
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            // Consulta a la tabla Tipo de Documento
            string sqlQuery = "SELECT IdTpoDocumento, Descripcion " +
                                "FROM ITM_06 " +
                                "WHERE Status = '1'";

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlTpoDocumento.DataSource = dt;

            ddlTpoDocumento.DataValueField = "IdTpoDocumento";
            ddlTpoDocumento.DataTextField = "Descripcion";

            ddlTpoDocumento.DataBind();

            cmd.Dispose();
            da.Dispose();

            Conecta.Cerrar();

        }

    }
}