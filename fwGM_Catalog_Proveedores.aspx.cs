using System;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwGM_Catalog_Proveedores : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Session["DownloadsPath"] = GetDownloadFolderPath();

            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdUsuario"] == null || Session["UsPassword"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!Page.IsPostBack)
            {
                GetServicios();
                GetEstados();

                // Disparar el evento SelectedIndexChanged manualmente
                ddlEstado_SelectedIndexChanged(ddlEstado, EventArgs.Empty);

                Inicializar_GrdProveedores();

                GetProveedores();
            }
        }

        private void Inicializar_GrdProveedores()
        {
            // Crea un DataTable vacío con la estructura necesaria
            DataTable dt = CrearDataTableVacio();

            // Verifica si el DataTable tiene filas
            if (dt.Rows.Count == 0)
            {
                // Mostrar el mensaje de "No hay resultados"
                GrdProveedores.ShowHeaderWhenEmpty = true;
                GrdProveedores.EmptyDataText = "No hay resultados.";
            }

            // Enlaza el DataTable (vacío o lleno) al GridView
            GrdProveedores.DataSource = dt;
            GrdProveedores.DataBind();
        }

        private DataTable CrearDataTableVacio()
        {
            DataTable dt = new DataTable();

            // Define las columnas del DataTable (ITM_34)
            dt.Columns.Add("Id_Proveedor", typeof(string));
            dt.Columns.Add("Tpo_Servicio", typeof(string));
            dt.Columns.Add("Nom_Empresa", typeof(string));
            dt.Columns.Add("Nom_Representante", typeof(string));
            dt.Columns.Add("Nacionalidad_Empresa", typeof(string));
            dt.Columns.Add("RFC_Empresa", typeof(string));

            dt.Columns.Add("Calle", typeof(string));
            dt.Columns.Add("Num_Exterior", typeof(string));
            dt.Columns.Add("Num_Interior", typeof(string));
            dt.Columns.Add("Estado", typeof(string));
            dt.Columns.Add("Delegacion", typeof(string));
            dt.Columns.Add("Colonia", typeof(string));
            dt.Columns.Add("Codigo_Postal", typeof(string));

            dt.Columns.Add("Email_Empresa", typeof(string));
            dt.Columns.Add("Tel_Contacto_1", typeof(string));
            dt.Columns.Add("Tel_Contacto_2", typeof(string));
            dt.Columns.Add("Nom_Responsable", typeof(string));
            dt.Columns.Add("Contraseña_QR", typeof(string));

            // Agrega más columnas según sea necesario

            return dt;
        }

        protected void GetServicios()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT Id_Servicio, Nom_Servicio " +
                                        " FROM ITM_08 " +
                                        " WHERE IdStatus = 1 ";
                /*+ " WHERE IdAseguradora = " + ddlAseguradora.SelectedValue*/

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlTpoServicio.DataSource = dt;

                ddlTpoServicio.DataValueField = "Id_Servicio";
                ddlTpoServicio.DataTextField = "Nom_Servicio";

                ddlTpoServicio.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ddlTpoServicio.Items.Insert(0, new ListItem("-- No Hay Carpeta(s) --", "0"));
                }
                else
                {
                    ddlTpoServicio.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                }

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetProveedores()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Proveedores tipo de servicio = ITM_34
                string strQuery = "SELECT Id_Proveedor, Tpo_Servicio, Nom_Empresa, Nom_Representante, Nacionalidad_Empresa, " +
                                  " RFC_Empresa, Calle, Num_Exterior, Num_Interior, Estado, Delegacion, Colonia, Codigo_Postal, " +
                                  " Email_Empresa, Tel_Contacto_1, Tel_Contacto_2, Nom_Responsable, Contraseña_QR " +
                                  "  FROM ITM_34 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdProveedores.ShowHeaderWhenEmpty = true;
                    GrdProveedores.EmptyDataText = "No hay resultados.";
                }

                GrdProveedores.DataSource = dt;
                GrdProveedores.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdProveedores.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetEstados()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT DISTINCT " +
                                  "  CASE WHEN c_estado = 05 THEN 'Coahuila' " +
                                  "       WHEN c_estado = 16 THEN 'Michoacán' " +
                                  "       WHEN c_estado = 30 THEN 'Veracruz' " +
                                  "       ELSE d_estado " +
                                  "   END AS d_estado, c_estado " +
                                  " FROM ITM_75 " +
                                  "ORDER BY d_estado ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlEstado.DataSource = dt;

                ddlEstado.DataValueField = "c_estado";
                ddlEstado.DataTextField = "d_estado";

                ddlEstado.DataBind();
                ddlEstado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetMunicipios(string pEstado)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT DISTINCT D_mnpio, c_mnpio " +
                                  " FROM ITM_75 " +
                                  "WHERE c_estado = '" + pEstado + "'" +
                                  "ORDER BY D_mnpio";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlMunicipios.DataSource = dt;

                ddlMunicipios.DataValueField = "c_mnpio";
                ddlMunicipios.DataTextField = "D_mnpio";

                ddlMunicipios.DataBind();
                ddlMunicipios.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }


        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            Eliminar_ITM_34();

            // inicializar controles.
            ddlTpoServicio.SelectedValue = "0";

            TxtNomEmpresa.Text = string.Empty;
            TxtNomRepresentante.Text = string.Empty;
            TxtNacionalidad_Empresa.Text = string.Empty;
            TxtRFC.Text = string.Empty;

            TxtCalleProveedor.Text = string.Empty;
            TxtNumExtProveedor.Text = string.Empty;
            TxtNumIntProveedor.Text = string.Empty;

            ddlEstado.SelectedValue = "0";
            ddlMunicipios.SelectedValue = "0";

            TxtColoniaProveedor.Text = string.Empty;
            TxtCPostalProveedor.Text = string.Empty;

            TxtEmail.Text = string.Empty;
            TxtTelContacto1.Text = string.Empty;
            TxtTelContacto2.Text = string.Empty;
            TxtResponsable.Text = string.Empty;
            TxtContraseñaQR.Text = string.Empty;
        }

        protected void Eliminar_ITM_34()
        {
            try
            {
                int index = Variables.wRenglon;

                int Id_Proveedor = Convert.ToInt32(GrdProveedores.Rows[index].Cells[0].Text); ;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_34)
                string strQuery = "DELETE FROM ITM_34 " +
                                  " WHERE Id_Proveedor = " + Id_Proveedor + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se eliminó correctamente el proveedor";
                mpeMensaje.Show();

                GetProveedores();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Actualizar_ITM_34()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int index = Variables.wRenglon;

                int Id_Proveedor = Convert.ToInt32(GrdProveedores.Rows[index].Cells[0].Text); ;

                // Actualizar registro(s) tablas (ITM_34)
                string strQuery = "UPDATE ITM_34 " +
                                  "   SET Nom_Empresa = '" + TxtNomEmpresa.Text.Trim() + "', " +
                                  "       Nom_Representante = '" + TxtNomRepresentante.Text.Trim() + "', " +
                                  "       Nacionalidad_Empresa = '" + TxtNacionalidad_Empresa.Text.Trim() + "', " +
                                  "       RFC_Empresa = '" + TxtRFC.Text.Trim() + "', " +
                                  "       Calle = '" + TxtCalleProveedor.Text.Trim() + "', " +
                                  "       Num_Exterior = '" + TxtNumExtProveedor.Text.Trim() + "', " +
                                  "       Num_Interior = '" + TxtNumIntProveedor.Text.Trim() + "', " +
                                  "       Estado = '" + ddlEstado.SelectedValue + "', " +
                                  "       Delegacion = '" + ddlMunicipios.SelectedValue + "', " +
                                  "       Colonia = '" + TxtColoniaProveedor.Text.Trim() + "', " +
                                  "       Codigo_Postal = '" + TxtCPostalProveedor.Text.Trim() + "', " +
                                  "       Email_Empresa = '" + TxtEmail.Text.Trim() + "', " +
                                  "       Tel_Contacto_1 = '" + TxtTelContacto1.Text.Trim() + "', " +
                                  "       Tel_Contacto_2 = '" + TxtTelContacto2.Text.Trim() + "', " +
                                  "       Nom_Responsable = '" + TxtResponsable.Text.Trim() + "', " +
                                  "       Contraseña_QR = '" + TxtContraseñaQR.Text.Trim() + "' " +
                                  " WHERE Id_Proveedor = " + Id_Proveedor + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizó correctamente el proveedor";
                mpeMensaje.Show();

                GetProveedores();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void ddlTpoServicio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnAnular_Click(object sender, EventArgs e)
        {

            // Inicializar Controles
            ddlTpoServicio.SelectedValue = "0";

            TxtNomEmpresa.Text = string.Empty;
            TxtNomRepresentante.Text = string.Empty;
            TxtNacionalidad_Empresa.Text = string.Empty;
            TxtRFC.Text = string.Empty;
            
            TxtCalleProveedor.Text = string.Empty;
            TxtNumExtProveedor.Text = string.Empty;
            TxtNumIntProveedor.Text = string.Empty;

            ddlEstado.SelectedValue = "0";
            ddlMunicipios.SelectedValue = "0";

            TxtColoniaProveedor.Text = string.Empty;
            TxtCPostalProveedor.Text = string.Empty;

            TxtEmail.Text = string.Empty;
            TxtTelContacto1.Text = string.Empty;
            TxtTelContacto2.Text = string.Empty;
            TxtResponsable.Text = string.Empty;
            TxtContraseñaQR.Text = string.Empty;

            ddlTpoServicio.Enabled = true;

            TxtNomEmpresa.ReadOnly = false;
            TxtNomRepresentante.ReadOnly = false;
            TxtNacionalidad_Empresa.ReadOnly = false;
            TxtRFC.ReadOnly = false;

            TxtCalleProveedor.ReadOnly = false;
            TxtNumExtProveedor.ReadOnly = false;
            TxtNumIntProveedor.ReadOnly = false;

            ddlEstado.Enabled = true;
            ddlMunicipios.Enabled = true;

            TxtColoniaProveedor.ReadOnly = false;
            TxtCPostalProveedor.ReadOnly = false;

            TxtEmail.ReadOnly = false;
            TxtTelContacto1.ReadOnly = false;
            TxtTelContacto2.ReadOnly = false;
            TxtResponsable.ReadOnly = false;
            TxtContraseñaQR.ReadOnly = false;

            btnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            btnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {

            TxtNomEmpresa.ReadOnly = false;
            TxtNomRepresentante.ReadOnly = false;
            TxtNacionalidad_Empresa.ReadOnly = false;
            TxtRFC.ReadOnly = false;
            
            TxtCalleProveedor.ReadOnly = false;
            TxtNumExtProveedor.ReadOnly = false;
            TxtNumIntProveedor.ReadOnly = false;

            ddlEstado.Enabled = true;
            ddlMunicipios.Enabled = true;

            TxtColoniaProveedor.ReadOnly = false;
            TxtCPostalProveedor.ReadOnly = false;

            TxtEmail.ReadOnly = false;
            TxtTelContacto1.ReadOnly = false;
            TxtTelContacto2.ReadOnly = false;
            TxtResponsable.ReadOnly = false;
            TxtContraseñaQR.ReadOnly = false;

            btnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {

            if (TxtNomEmpresa.Text == "" || TxtNomEmpresa.Text == null)
            {
                LblMessage.Text = "Capturar Nombre de la empresa";
                mpeMensaje.Show();
                return;
            }
            else if (TxtNomRepresentante.Text == "" || TxtNomRepresentante.Text == null)
            {
                LblMessage.Text = "Capturar Nombre del representante";
                mpeMensaje.Show();
                return;
            }

            Actualizar_ITM_34();

            // inicializar controles.
            ddlTpoServicio.SelectedValue = "0";

            TxtNomEmpresa.Text = string.Empty;
            TxtNomRepresentante.Text = string.Empty;
            TxtNacionalidad_Empresa.Text = string.Empty;
            TxtRFC.Text = string.Empty;
            
            TxtCalleProveedor.Text = string.Empty;
            TxtNumExtProveedor.Text = string.Empty;
            TxtNumIntProveedor.Text = string.Empty;

            ddlEstado.SelectedValue = "0";
            ddlMunicipios.SelectedValue = "0";

            TxtColoniaProveedor.Text = string.Empty;
            TxtCPostalProveedor.Text = string.Empty;

            TxtEmail.Text = string.Empty;
            TxtTelContacto1.Text = string.Empty;
            TxtTelContacto2.Text = string.Empty;
            TxtResponsable.Text = string.Empty;
            TxtContraseñaQR.Text = string.Empty;

            ddlTpoServicio.Enabled = true;

            TxtNomEmpresa.ReadOnly = false;
            TxtNomRepresentante.ReadOnly = false;
            TxtNacionalidad_Empresa.ReadOnly = false;
            TxtRFC.ReadOnly = false;
            
            TxtCalleProveedor.ReadOnly = false;
            TxtNumExtProveedor.ReadOnly = false;
            TxtNumIntProveedor.ReadOnly = false;

            ddlEstado.Enabled = true;
            ddlMunicipios.Enabled = true;

            TxtColoniaProveedor.ReadOnly = false;
            TxtCPostalProveedor.ReadOnly = false;

            TxtEmail.ReadOnly = false;
            TxtTelContacto1.ReadOnly = false;
            TxtTelContacto2.ReadOnly = false;
            TxtResponsable.ReadOnly = false;
            TxtContraseñaQR.ReadOnly = false;

            btnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            btnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {

                if (ddlTpoServicio.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar Tipo de servicio";
                    mpeMensaje.Show();
                    return;
                }
                else if (TxtNomEmpresa.Text == "" || TxtNomEmpresa.Text == null)
                {
                    LblMessage.Text = "Capturar Nombre de la empresa";
                    mpeMensaje.Show();
                    return;
                }
                else if (TxtNomRepresentante.Text == "" || TxtNomRepresentante.Text == null)
                {
                    LblMessage.Text = "Capturar Nombre del representante";
                    mpeMensaje.Show();
                    return;
                }

                int iConsecutivo = GetIdConsecutivoMax();
                int Tpo_Servicio = Convert.ToInt32(ddlTpoServicio.SelectedValue);

                string Id_Usuario = Variables.wUserLogon;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_34)
                string strQuery = "INSERT INTO ITM_34 (Id_Proveedor, Tpo_Servicio, Nom_Empresa, Nom_Representante, Nacionalidad_Empresa, " +
                                  "RFC_Empresa, Calle, Num_Exterior, Num_Interior, Estado, Delegacion, Colonia, Codigo_Postal, " +
                                  "Email_Empresa, Tel_Contacto_1, Tel_Contacto_2, Nom_Responsable, Contraseña_QR, Id_Usuario, IdStatus ) " +
                                  "VALUES (" + iConsecutivo + ", " + Tpo_Servicio + ", '" + TxtNomEmpresa.Text.Trim() + "', " +
                                  "'" + TxtNomRepresentante.Text.Trim() + "', '" + TxtNacionalidad_Empresa.Text.Trim() + "', " +
                                  "'" + TxtRFC.Text.Trim() + "', '" + TxtCalleProveedor.Text.Trim() + "', '" + TxtNumExtProveedor.Text.Trim() + "', '" + TxtNumIntProveedor.Text.Trim() + "'," +
                                  "'" + ddlEstado.SelectedValue + "', '" + ddlMunicipios.SelectedValue + "', '" + TxtColoniaProveedor.Text.Trim() + "','" + TxtCPostalProveedor.Text.Trim() + "', " +
                                  "'" + TxtEmail.Text.Trim() + "', '" + TxtTelContacto1.Text.Trim() + "', '" + TxtTelContacto2.Text.Trim() + "', " +
                                  "'" + TxtResponsable.Text.Trim() + "', '" + TxtContraseñaQR.Text.Trim() + "', '" + Id_Usuario + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                GetProveedores();

                LblMessage.Text = "Se agrego proveedor correctamente";
                mpeMensaje.Show();

                // Inicializar Controles
                ddlTpoServicio.SelectedValue = "0";
                TxtNomEmpresa.Text = string.Empty;
                TxtNomRepresentante.Text = string.Empty;
                TxtNacionalidad_Empresa.Text = string.Empty;
                TxtRFC.Text = string.Empty;
                TxtCalleProveedor.Text = string.Empty;
                TxtNumExtProveedor.Text = string.Empty;
                TxtNumIntProveedor.Text = string.Empty;

                ddlEstado.SelectedValue = "0";
                ddlMunicipios.SelectedValue = "0";

                TxtColoniaProveedor.Text = string.Empty;
                TxtCPostalProveedor.Text = string.Empty;

                TxtEmail.Text = string.Empty;
                TxtTelContacto1.Text = string.Empty;
                TxtTelContacto2.Text = string.Empty;
                TxtResponsable.Text = string.Empty;
                TxtContraseñaQR.Text = string.Empty;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public int GetIdConsecutivoMax()
        {

            int IdConsecutivoMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX(Id_Proveedor), 0) + 1 Id_Proveedor " +
                                " FROM ITM_34 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            dbConn.Close();

            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["Id_Proveedor"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void GrdProveedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdProveedores.PageIndex = e.NewPageIndex;
                GetProveedores();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdProveedores_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdProveedores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdProveedores, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Width = Unit.Pixel(300);     // Nom_Empresa
                e.Row.Cells[3].Width = Unit.Pixel(300);     // Nom_Representante
                e.Row.Cells[4].Width = Unit.Pixel(200);     // Nom_Representante
                e.Row.Cells[5].Width = Unit.Pixel(200);     // RFC
                e.Row.Cells[13].Width = Unit.Pixel(200);    // Email_Empresa
                e.Row.Cells[14].Width = Unit.Pixel(150);    // Tel_Contacto_1
                e.Row.Cells[15].Width = Unit.Pixel(150);    // Tel_Contacto_2
                e.Row.Cells[18].Width = Unit.Pixel(50);     // Editar
                e.Row.Cells[19].Width = Unit.Pixel(50);     // Eliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;     // Id_Proveedor
                e.Row.Cells[1].Visible = false;     // Tpo_Servicio
                e.Row.Cells[6].Visible = false;     // Calle
                e.Row.Cells[7].Visible = false;     // Num_Exterior
                e.Row.Cells[8].Visible = false;     // Num_Interior
                e.Row.Cells[9].Visible = false;     // Estado
                e.Row.Cells[10].Visible = false;    // Delegacion
                e.Row.Cells[11].Visible = false;    // Colonia
                e.Row.Cells[12].Visible = false;    // Codigo_Postal
                e.Row.Cells[16].Visible = false;    // Nom_Responsable
                e.Row.Cells[17].Visible = false;    // Contraseña_QR
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;     // Id_Proveedor
                e.Row.Cells[1].Visible = false;     // Tpo_Servicio
                e.Row.Cells[6].Visible = false;     // Calle
                e.Row.Cells[7].Visible = false;     // Num_Exterior
                e.Row.Cells[8].Visible = false;     // Num_Interior
                e.Row.Cells[9].Visible = false;     // Estado
                e.Row.Cells[10].Visible = false;    // Delegacion
                e.Row.Cells[11].Visible = false;    // Colonia
                e.Row.Cells[12].Visible = false;    // Codigo_Postal
                e.Row.Cells[16].Visible = false;    // Nom_Responsable
                e.Row.Cells[17].Visible = false;    // Contraseña_QR
            }
        }

        protected void GrdProveedores_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdProveedores_PreRender(object sender, EventArgs e)
        {

        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            ddlTpoServicio.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[1].Text));
            TxtNomEmpresa.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[2].Text));
            TxtNomRepresentante.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[3].Text));

            TxtNacionalidad_Empresa.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[4].Text));
            TxtRFC.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[5].Text));

            TxtCalleProveedor.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[6].Text));
            TxtNumExtProveedor.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[7].Text));
            TxtNumIntProveedor.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[8].Text));
            ddlEstado.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[9].Text));
            
            // Disparar el evento SelectedIndexChanged manualmente
            ddlEstado_SelectedIndexChanged(ddlEstado, EventArgs.Empty);

            ddlMunicipios.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[10].Text));
            TxtColoniaProveedor.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[11].Text));
            TxtCPostalProveedor.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[12].Text));

            TxtEmail.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[13].Text));
            TxtTelContacto1.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[14].Text));
            TxtTelContacto2.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[15].Text));
            TxtResponsable.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[16].Text));
            TxtContraseñaQR.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[17].Text));

            TxtNomEmpresa.ReadOnly = true;
            TxtNomRepresentante.ReadOnly = true;
            TxtNacionalidad_Empresa.ReadOnly = true;
            TxtRFC.ReadOnly = true;
            TxtCalleProveedor.ReadOnly = true;
            TxtNumExtProveedor.ReadOnly = true;
            TxtNumIntProveedor.ReadOnly = true;

            ddlEstado.Enabled = false;
            ddlMunicipios.Enabled = false;

            TxtColoniaProveedor.ReadOnly = true;
            TxtCPostalProveedor.ReadOnly = true;

            TxtEmail.ReadOnly = true;
            TxtTelContacto1.ReadOnly = true;
            TxtTelContacto2.ReadOnly = true;
            TxtResponsable.ReadOnly = true;
            TxtContraseñaQR.ReadOnly = true;

            ddlTpoServicio.Enabled = false;

            BtnAnular.Visible = true;
            btnEditar.Enabled = true;
            BtnAgregar.Enabled = false;
        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            LblMessage_1.Text = "¿Desea eliminar el proveedor?";
            mpeMensaje_1.Show();
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sEstado = ddlEstado.SelectedValue;
            GetMunicipios(sEstado);
        }

        protected void ddlMunicipios_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}