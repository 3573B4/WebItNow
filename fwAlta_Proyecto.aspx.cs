using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwAlta_Proyecto : System.Web.UI.Page
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
                try
                {
                    Variables.wUserName = Convert.ToString(Session["IdUsuario"]);
                    Variables.wPassword = Convert.ToString(Session["UsPassword"]);

                    if (Variables.wUserName == "" || Variables.wPassword == "")
                    {
                        Response.Redirect("Login.aspx", true);
                        return;
                    }

                    // Labels
                    lblTitulo_Alta_Proyecto.Text = GetGlobalResourceObject("GlobalResources", "lblTitulo_Alta_Proyecto").ToString();

                    GetCiaSeguros();
                    GetTipoAsunto();
                    GetGerentes();
                    GetProyectos("*");
                    GetTpoAsegurado();

                    // inicializar controles
                    ddlTpoAsunto.SelectedValue = "0";
                    rbGestionCasos2.Checked = true;

                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }

            }
        }

        protected void GetCiaSeguros()
        {
            try
            {
                //ConexionBD Conecta = new ConexionBD();
                //Conecta.Abrir();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdSeguros, Descripcion " +
                                        " FROM ITM_67 " +
                                        " WHERE IdSeguros <> 'OTR'" +
                                        "   AND IdStatus = 1 " +
                                        "ORDER BY IdOrden";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //da.Fill(dt);

                ddlCliente.DataSource = dt;

                ddlCliente.DataValueField = "IdSeguros";
                ddlCliente.DataTextField = "Descripcion";

                ddlCliente.DataBind();
                //ddlCliente.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlCliente.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                dbConn.Close();

                //Conecta.Cerrar();
                //cmd.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetTipoAsunto()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdTpoAsunto, Descripcion " +
                                        " FROM ITM_66 " +
                                        " WHERE IdStatus IN (0, 1) ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlTpoAsunto.DataSource = dt;

                ddlTpoAsunto.DataValueField = "IdTpoAsunto";
                ddlTpoAsunto.DataTextField = "Descripcion";

                ddlTpoAsunto.DataBind();
                //ddlTpoAsunto.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));
                ddlTpoAsunto.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "-1"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        protected void GetGerentes()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdGerente, Descripcion " +
                                        " FROM ITM_79 " +
                                        " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlGerentes.DataSource = dt;

                ddlGerentes.DataValueField = "IdGerente";
                ddlGerentes.DataTextField = "Descripcion";

                ddlGerentes.DataBind();
                //ddlGerentes.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlGerentes.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetProyectos(string sCliente)
        {
            try
            {
                // string sCliente = ddlCliente.SelectedValue;
                string strQuery = string.Empty;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_12
                if (sCliente == "0")
                {
                    strQuery = "SELECT a.IdProyecto, a.Descripcion, b.Descripcion As NomCliente, a.IdCliente, c.Descripcion As DescAsunto, c.IdTpoAsunto, NumPoliza " +
                               "  FROM ITM_78 as a, ITM_67 as b, ITM_66 as c " +
                               " WHERE a.IdCliente = b.IdSeguros " +
                               "   AND a.IdTpoAsunto = c.IdTpoAsunto " +
                               "   AND a.IdStatus <> 9";
                }
                else
                {

                   strQuery = "SELECT a.IdProyecto, a.Descripcion, b.Descripcion As NomCliente, a.IdCliente, c.Descripcion As DescAsunto, c.IdTpoAsunto, NumPoliza " +
                              "  FROM ITM_78 as a, ITM_67 as b, ITM_66 as c " +
                              " WHERE a.IdCliente = b.IdSeguros " +
                              "   AND a.IdTpoAsunto = c.IdTpoAsunto " +
                              "   AND a.IdCliente = '" + sCliente + "' " +
                              "   AND a.IdStatus <> 9";
                }

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdProyectos.ShowHeaderWhenEmpty = true;
                    GrdProyectos.EmptyDataText = GetGlobalResourceObject("GlobalResources", "msg_NoResults").ToString();
                    //GrdProyectos.EmptyDataText = "No hay resultados.";
                }

                GrdProyectos.DataSource = dt;
                GrdProyectos.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdProyectos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetTpoAsegurado()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdRegimen, Descripcion " +
                                  "  FROM ITM_49 " +
                                  " WHERE IdRegimen <> 4 " +
                                  "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlTpoAsegurado.DataSource = dt;

                ddlTpoAsegurado.DataValueField = "IdRegimen";
                ddlTpoAsegurado.DataTextField = "Descripcion";

                ddlTpoAsegurado.DataBind();
                //ddlTpoAsegurado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlTpoAsegurado.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sCliente = ddlCliente.SelectedValue;

            if (sCliente == "0")
            {
                GetProyectos("*");
            } else
            {
                GetProyectos(sCliente);
            }

        }


        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            Eliminar_ITM_78();
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void ddlTpoAsunto_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public int obtenerConsecutivo(string IdProceso)
        {
            try
            {
                int IdArchivoMax = 0;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdConsecutivo FROM ITM_71" +
                                  " WHERE IdProceso = '" + IdProceso + "'";

                MySqlDataReader reader = dbConn.ExecuteReaderQuery(strQuery);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        IdArchivoMax = Convert.ToInt32(reader["IdConsecutivo"].ToString().Trim());
                    }
                }

                dbConn.Close();

                return IdArchivoMax;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

            return -1;
        }

        public int GetIdConsecutivoMax()
        {

            int IdConsecutivoMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX(IdProyecto), 0) + 1 IdProyecto " +
                                " FROM ITM_78 ";

            MySqlDataReader reader = dbConn.ExecuteReaderQuery(strQuery);

            //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

            //SqlDataReader dr = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    IdConsecutivoMax = Convert.ToInt32(reader["IdProyecto"].ToString().Trim());
                }
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void Eliminar_ITM_78()
        {
            try
            {
                int index = Variables.wRenglon;

                int iConsecutivo = Convert.ToInt32(GrdProyectos.Rows[index].Cells[4].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_78)
                // string strQuery = "DELETE ITM_78 " +
                //                   " WHERE IdProyecto = " + iConsecutivo + "";

                string strQuery = "UPDATE ITM_78 " +
                                  "   SET IdStatus = 9 " +
                                  " WHERE IdProyecto = " + iConsecutivo + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //int affectedRows = cmd.ExecuteNonQuery();

                // LblMessage.Text = "Se elimino proyecto, correctamente";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Proyecto_Eliminado").ToString();
                mpeMensaje.Show();

                string sCliente = ddlCliente.SelectedValue;
                GetProyectos(sCliente);
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void GrdProyectos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdProyectos.PageIndex = e.NewPageIndex;

                string sCliente = ddlCliente.SelectedValue;
                GetProyectos(sCliente);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdProyectos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectProyecto")
            {
                string[] arguments = e.CommandArgument.ToString().Split('|');
                string iProyecto = arguments[1];

                Session["Proyecto"] = arguments[0];
                Session["IdProyecto"] = arguments[1];
                Session["Cliente"] = arguments[2];
                Session["TpoAsunto"] = arguments[3];
                Session["IdCliente"] = arguments[4];
                Session["IdTpoAsunto"] = arguments[5];

                Response.Redirect("fwConfiguracion_Proyect.aspx?Proyecto=" + iProyecto, true);
            }
        }

        protected void GrdProyectos_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdProyectos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdProyectos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdProyectos, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = Unit.Pixel(500);     // Proyecto
                e.Row.Cells[1].Width = Unit.Pixel(500);     // NomCliente
                e.Row.Cells[2].Width = Unit.Pixel(500);     // IdTpoAsunto
                e.Row.Cells[4].Width = Unit.Pixel(50);      // Eliminar
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[3].Visible = false;    // NumPoliza
                e.Row.Cells[4].Visible = false;    // IdProyecto
                e.Row.Cells[5].Visible = false;    // IdCliente
                e.Row.Cells[6].Visible = false;    // IdTpoAsunto
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Visible = false;    // NumPoliza
                e.Row.Cells[4].Visible = false;    // IdProyecto
                e.Row.Cells[5].Visible = false;    // IdCliente
                e.Row.Cells[6].Visible = false;    // IdTpoAsunto
            }
        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            LblMessage_1.Text = "¿Desea eliminar el proyecto?";
            mpeMensaje_1.Show();
        }

        protected void rbGestionCasos1_CheckedChanged(object sender, EventArgs e)
        {
            PnlGestionCasos.Visible = rbGestionCasos1.Checked;
        }

        protected void rbGestionCasos2_CheckedChanged(object sender, EventArgs e)
        {
            // inicializar controles
            TxtNumPoliza.Text = string.Empty;
            TxtNomAsegurado.Text = string.Empty;
            ddlTpoAsegurado.ClearSelection();
            TxtIniVigencia.Text = string.Empty;
            TxtFinVigencia.Text = string.Empty;

            PnlGestionCasos.Visible = false;
        }

        protected void ddlTpoAsegurado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlGerentes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnSiguiente_Click(object sender, EventArgs e)
        {

            if (ddlCliente.SelectedValue == "0")
            {
                //LblMessage.Text = "Seleccionar Nombre del Cliente";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_CiaSeguros").ToString();
                mpeMensaje.Show();
                return;
            }
            else if (ddlTpoAsunto.SelectedValue == "-1")
            {
                //LblMessage.Text = "Seleccionar Tipo de Asunto";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_TpoAsunto").ToString();
                mpeMensaje.Show();
                return;
            }
            else if (TxtNomProyecto.Text == "" || TxtNomProyecto.Text == null)
            {
                //LblMessage.Text = "Capturar Nombre del proyecto";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Capturar_NomProyecto").ToString();
                mpeMensaje.Show();
                return;
            }
            else if (ddlGerentes.SelectedValue == "0")
            {
                //LblMessage.Text = "Seleccionar Gerente Responsable";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Gerente").ToString();
                mpeMensaje.Show();
                return;
            }

            int OkExiste = Valida_Proyecto_Existe();

            if(OkExiste == 1)
            {
                //LblMessage.Text = "El Proyecto, ya se encuentra registrado";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Proyecto_Registrado").ToString();
                mpeMensaje.Show();
                return;
            }

            Session["IdTpoGestion"] = 0;
            Session["TpoAsegurado"] = 0;

            Session["Cliente"] = ddlCliente.SelectedItem.Text;
            Session["IdCliente"] = ddlCliente.SelectedItem.Value;

            if (TxtNomProyecto.Text == "NINGUNO")
            {
                ddlTpoAsunto.SelectedValue = "0";

            }

            Session["TpoAsunto"] = ddlTpoAsunto.SelectedItem.Text;
            Session["IdTpoAsunto"] = ddlTpoAsunto.SelectedItem.Value;

            Session["Proyecto"] = TxtNomProyecto.Text.Trim();

            if(TxtNomProyecto.Text != "NINGUNO")
            {
                Session["IdProyecto"] = GetIdConsecutivoMax();
            } else
            {

                Session["IdProyecto"] = 0;
            }

            Session["IdGerente"] = ddlGerentes.SelectedItem.Value;

            Session["DescProyecto"] = TxtDescProyecto.Text.Trim();

            if (rbGestionCasos1.Checked)
            {
                if (TxtNumPoliza.Text == "" || TxtNumPoliza.Text == null)
                {
                    //LblMessage.Text = "Capturar Número de Póliza";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Capturar_NumPoliza").ToString();
                    mpeMensaje.Show();
                    return;
                }
                else if (TxtNomAsegurado.Text == "" || TxtNomAsegurado.Text == null)
                {
                    //LblMessage.Text = "Capturar Nombre del Asegurado";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Capturar_NomAsegurado").ToString();
                    mpeMensaje.Show();
                    return;
                }
                else if (ddlTpoAsegurado.SelectedValue == "0")
                {
                    //LblMessage.Text = "Seleccionar Tipo de Asegurado";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_TpoAsegurado").ToString();
                    mpeMensaje.Show();
                    return;
                }
                else if (TxtIniVigencia.Text == "" || TxtIniVigencia.Text == null)
                {
                    //LblMessage.Text = "Seleccionar Fecha Inicio Vigencia";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_FecIniVigencia").ToString();
                    mpeMensaje.Show();
                    return;
                }
                else if (TxtFinVigencia.Text == "" || TxtFinVigencia.Text == null)
                {
                    //LblMessage.Text = "Seleccionar Fecha Fin Vigencia";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_FecFinVigencia").ToString();
                    mpeMensaje.Show();
                    return;
                }

                Session["IdTpoGestion"] = 1;

                Session["Poliza"] = TxtNumPoliza.Text.Trim();
                Session["NomAsegurado"] = TxtNomAsegurado.Text.Trim();
                Session["TpoAsegurado"] = ddlTpoAsegurado.SelectedItem.Value;
                Session["IniVigencia"] = TxtIniVigencia.Text.Trim();
                Session["FinVigencia"] = TxtFinVigencia.Text.Trim();

                // fechaActualFormateada = TxtIniVigencia.Text;

            }

            Response.Redirect("fwConfiguracion_Proyect.aspx", true);
            return;

        }


        protected int Valida_Proyecto_Existe()
        {
            int Proyecto_Existe = 0;


            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT IdProyecto, Descripcion, IdCliente " +
                              "  FROM ITM_78 " +
                              " WHERE Descripcion = '" + TxtNomProyecto.Text.Trim() + "' " +
                              "   AND IdCliente = '" + ddlCliente.SelectedValue + "' ";

            MySqlDataReader reader = dbConn.ExecuteReaderQuery(strQuery);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Proyecto_Existe = 1;
                }
            }

            dbConn.Close();

            return Proyecto_Existe;
        }

    }
}