using System;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using MySql.Data.MySqlClient;

namespace WebItNow_Peacock
{
    public partial class fwGM_Bitacora_Asunto : System.Web.UI.Page
    {

        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Session["DownloadsPath"] = GetDownloadFolderPath();

            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // string sReferencia = Request.QueryString["Referencia"];
            if (Session["IdUsuario"] == null || Session["UsPassword"] == null)
            {
                Response.Redirect("Login.aspx", true);
            }

            if (!Page.IsPostBack)
            {
                string sReferencia = Request.QueryString["Ref"];
                string SubReferencia = Request.QueryString["SubRef"];
                string IdProyecto = Request.QueryString["Proyecto"];
                string CveCliente = Request.QueryString["Seguro"];
                string IdTpoAsunto = Request.QueryString["Asunto"];

                Variables.wRef = sReferencia;
                Variables.wSubRef = Convert.ToInt32(SubReferencia);
                Variables.wIdProyecto = Convert.ToInt32(IdProyecto);
                Variables.wPrefijo_Aseguradora = CveCliente;
                Variables.wIdTpoAsunto = Convert.ToInt32(IdTpoAsunto);
                Variables.wExiste = false;

                // BindRepeater();

                inhabilitar(this.Controls);
                habilitar_controles();

                ////chkCopiarDatos.Enabled = false;
                ////pnlEstadoOcurrencia.Visible = false;        // Ocultar por defecto

                if (IdTpoAsunto == "1")
                {
                    // Aplica solo para NOTIFICACION
                    ////pnlEstadoOcurrencia.Visible = true;     // Vizualizar por defecto
                }

                // inhabilitar control Crear Cuaderno
                if (Convert.ToString(Session["UsPrivilegios"]) == "0")
                {
                    ////BtnCrear_Cuaderno.Enabled = false;
                }

                GetEstatusCaso();
                GetTipoEvento();
                GetServicios();

                // GetEstados();
                GetEstados_Proveedor();
                GetEstados_Atencion();

                GetEstadosLesionado();
                GetEstadosResponsable();

                GetLineas();
                GetZonas();

                GetStSiniestro();
                GetConclusion();
                GetRegimen();

                GetTipoServicio();

                GetInstituciones();
                GetTpoAtencion();

                habilitar_control_proveedores();
                habilitar_control_servicios();
                habilitar_control_paquetes();

                Inicializar_GrdProveedores();
                Inicializar_GrdEstudios();
                Inicializar_GrdServicios();
                Inicializar_GrdPaquetes();

                string flechaHaciaAbajo = "\u25BC";
                string flechaHaciaArriba = "\u25B2";

                btnShowPanel0.Text = flechaHaciaAbajo;      // Flecha hacia abajo
             // btnShowPanel1.Text = flechaHaciaArriba;     // Flecha hacia arriba
                btnShowPanel3.Text = flechaHaciaArriba;     // Flecha hacia arriba  (INFORMACIÓN GENERAL DEL LESIONADO)
                btnShowPanel4.Text = flechaHaciaArriba;     // Flecha hacia arriba  (INFORMACIÓN GENERAL DEL RESPONSABLE)

                btnShowPanel6.Text = flechaHaciaArriba;     // Flecha hacia abajo   (LINEA / ESTACIÓN DE OCURRENCIA)
                btnShowPanel7.Text = flechaHaciaAbajo;      // Flecha hacia abajo
                btnShowPanel8.Text = flechaHaciaAbajo;      // Flecha hacia abajo   (INFORMACIÓN MEDICA)

                btnShowPanel9.Text = flechaHaciaAbajo;      // Flecha hacia abajo   (PROVEEDOR 1)

                btnShowPanel10.Text = flechaHaciaAbajo;     // Flecha hacia abajo   (Diagnostico Final)
                btnShowPanel11.Text = flechaHaciaAbajo;     // Flecha hacia abajo   (Tratamientos Realizados)
                btnShowPanel12.Text = flechaHaciaAbajo;     // Flecha hacia abajo   (Estudios Realizados)
             // btnShowPanel13.Text = flechaHaciaAbajo;     // Flecha hacia abajo   (Comentarios Médicos)

                btnShowPanel14.Text = flechaHaciaAbajo;     // Flecha hacia abajo   (DATOS DE ATENCIÓN)

                btnShowPanel15.Text = flechaHaciaAbajo;     // Flecha hacia abajo   (SERVICIO AUTORIZADO) 

                btnShowPanel16.Text = flechaHaciaAbajo;     // Flecha hacia abajo   (PAQUETES MEDICOS)
                ////btnShowPanel17.Text = flechaHaciaAbajo; // Flecha hacia abajo

                // TxtDomSiniestro.Enabled = true;

                // GetSeccion_2();     // RIESGOS
                // GetSeccion_5();     // OTROS DETALLES

                // Obtener datos generales
                GetConsulta_Datos_Generales(sReferencia, SubReferencia);

                // Obtener datos coberturas tabla ITM_70_3_4
                ////GetAltaCoberturas();

                // Obtener los documentos de las categorias solicitados
                ////GetDocumentos(TxtSubReferencia.Text);

                // Obtener Datos de Proveedor
                GetDatos_Proveedor();

                // Obtener Datos Estudios Realizados
                GetDatos_Estudios();

                // Obtener Servicios autorizados
                GetDatos_Servicios();

                // Obtener Datos Paquetes Médicos
                GetDatos_Paquetes();

                // Obtener las claves de ICD / CPT
                GetDiagnosticoFinal(TxtSubReferencia.Text);
                GetTratamientosRealizados(TxtSubReferencia.Text);


                // Inhabilitar controles si ya existe un Alta de Cuaderno
                if (Convert.ToString(Session["UsPrivilegios"]) != "0")
                {
                    ////Validar_Alta_Notebook();
                }

            }
            else
            {
                // habilitar_controles();
            }

        }

        protected void GetServicios()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdServicio, Descripcion " +
                                  "  FROM ITM_06 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlServicios.DataSource = dt;

                ddlServicios.DataValueField = "IdServicio";
                ddlServicios.DataTextField = "Descripcion";

                ddlServicios.DataBind();
                ddlServicios.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        protected void GetTipoServicio()
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

        protected void GetTipoEvento()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdTpoEvento, Descripcion " +
                                  "  FROM ITM_11 " +
                                  " WHERE IdTpoEvento NOT IN (4,5) " +
                                  "   AND  IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlTipoEvento.DataSource = dt;

                ddlTipoEvento.DataValueField = "IdTpoEvento";
                ddlTipoEvento.DataTextField = "Descripcion";

                ddlTipoEvento.DataBind();
                ddlTipoEvento.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
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

                //ddlEstado.DataSource = dt;

                //ddlEstado.DataValueField = "c_estado";
                //ddlEstado.DataTextField = "d_estado";

                //ddlEstado.DataBind();
                //ddlEstado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetEstados_Proveedor()
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

                ddlEstadoProveedor.DataSource = dt;

                ddlEstadoProveedor.DataValueField = "c_estado";
                ddlEstadoProveedor.DataTextField = "d_estado";

                ddlEstadoProveedor.DataBind();
                ddlEstadoProveedor.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetEstadosLesionado()
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

                ddlEstadoLesionado.DataSource = dt;

                ddlEstadoLesionado.DataValueField = "c_estado";
                ddlEstadoLesionado.DataTextField = "d_estado";

                ddlEstadoLesionado.DataBind();
                ddlEstadoLesionado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetEstadosResponsable()
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

                ddlEstadoResponsable.DataSource = dt;

                ddlEstadoResponsable.DataValueField = "c_estado";
                ddlEstadoResponsable.DataTextField = "d_estado";

                ddlEstadoResponsable.DataBind();
                ddlEstadoResponsable.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));


                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetEstados_Atencion()
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

                ddlEstadoAtencion.DataSource = dt;

                ddlEstadoAtencion.DataValueField = "c_estado";
                ddlEstadoAtencion.DataTextField = "d_estado";

                ddlEstadoAtencion.DataBind();
                ddlEstadoAtencion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));


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

                //ddlMunicipios.DataSource = dt;

                //ddlMunicipios.DataValueField = "c_mnpio";
                //ddlMunicipios.DataTextField = "D_mnpio";

                //ddlMunicipios.DataBind();
                //ddlMunicipios.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetMunicipiosProveedor(string pEstado)
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

                ddlMunicipioProveedor.DataSource = dt;

                ddlMunicipioProveedor.DataValueField = "c_mnpio";
                ddlMunicipioProveedor.DataTextField = "D_mnpio";

                ddlMunicipioProveedor.DataBind();
                ddlMunicipioProveedor.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetMunicipiosLesionado(string pEstado)
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

                ddlMunicipiosLesionado.DataSource = dt;

                ddlMunicipiosLesionado.DataValueField = "c_mnpio";
                ddlMunicipiosLesionado.DataTextField = "D_mnpio";

                ddlMunicipiosLesionado.DataBind();
                ddlMunicipiosLesionado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetMunicipiosResponsable(string pEstado)
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

                ddlMunicipiosResponsable.DataSource = dt;

                ddlMunicipiosResponsable.DataValueField = "c_mnpio";
                ddlMunicipiosResponsable.DataTextField = "D_mnpio";

                ddlMunicipiosResponsable.DataBind();
                ddlMunicipiosResponsable.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetMunicipios_Atencion(string pEstado)
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

                ddlMunicipiosAtencion.DataSource = dt;

                ddlMunicipiosAtencion.DataValueField = "c_mnpio";
                ddlMunicipiosAtencion.DataTextField = "D_mnpio";

                ddlMunicipiosAtencion.DataBind();
                ddlMunicipiosAtencion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetLineas()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdLinea, NomLinea " +
                                  " FROM ITM_16 " +
                                  "ORDER BY IdLinea ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlLineaOcurrencia.DataSource = dt;

                ddlLineaOcurrencia.DataValueField = "IdLinea";
                ddlLineaOcurrencia.DataTextField = "NomLinea";

                ddlLineaOcurrencia.DataBind();
                ddlLineaOcurrencia.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetEstaciones(int iLinea)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdEstacion, NomEstacion " +
                                  "  FROM ITM_15 " +
                                  " WHERE IdLinea = " + iLinea + " " +
                                  "ORDER BY IdEstacion ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlEstacionOcurrencia.DataSource = dt;

                ddlEstacionOcurrencia.DataValueField = "IdEstacion";
                ddlEstacionOcurrencia.DataTextField = "NomEstacion";

                ddlEstacionOcurrencia.DataBind();
                ddlEstacionOcurrencia.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetZonas()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdZona, NomZona " +
                                  " FROM ITM_18 " +
                                  "ORDER BY IdZona ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlZonas.DataSource = dt;

                ddlZonas.DataValueField = "IdZona";
                ddlZonas.DataTextField = "NomZona";

                ddlZonas.DataBind();
                ddlZonas.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetConclusion()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_23
                string strQuery = "SELECT a.IdEtapa, b.Descripcion " +
                                  "  FROM ITM_28 as a, ITM_23 as b " +
                                  " WHERE b.IdDocumento = a.IdEtapa " +
                                  "   AND a.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND a.IdProyecto = " + Variables.wIdProyecto + " " +
                                  "   AND a.IdEstatus = " + Convert.ToInt32(ddlEstSiniestro.SelectedValue) + " ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlConclusion.DataSource = dt;

                ddlConclusion.DataValueField = "IdEtapa";
                ddlConclusion.DataTextField = "Descripcion";

                ddlConclusion.DataBind();
                ddlConclusion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetRegimen()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT C.IdDocumento, C.Descripcion " +
                                  "  FROM ITM_25 AS A, ITM_87 AS B, ITM_21 AS C " +
                                  " WHERE A.IdSeccion = B.IdCategoria " +
                                  "   AND A.IdDocumento = C.IdDocumento " +
                                  "   AND IdProyecto = " + Variables.wIdProyecto + " AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND IdSeccion = 1 ";

                if (Variables.wIdProyecto != 0)
                {
                    // strQuery += "   AND A.IdTpoAsunto = " + Variables.wIdTpoAsunto + " ";
                }

                strQuery += "   AND bSeleccion = 1 AND A.IdStatus = 1 " +
                            " ORDER BY C.IdDocumento ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlTpoAsegurado.DataSource = dt;

                ddlTpoAsegurado.DataValueField = "IdDocumento";
                ddlTpoAsegurado.DataTextField = "Descripcion";

                ddlTpoAsegurado.DataBind();
                ddlTpoAsegurado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetStSiniestro()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();


                string strQuery = "SELECT IdEstStatus, Descripcion " +
                                  "  FROM ITM_52 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlEstSiniestro.DataSource = dt;

                ddlEstSiniestro.DataValueField = "IdEstStatus";
                ddlEstSiniestro.DataTextField = "Descripcion";

                ddlEstSiniestro.DataBind();
                ddlEstSiniestro.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetHospitales(int iZona)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdHospital, NomHospital " +
                                  "  FROM ITM_17 " +
                                  " WHERE IdZona = " + iZona + " " +
                                  "ORDER BY IdHospital ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlHospitales.DataSource = dt;

                ddlHospitales.DataValueField = "IdHospital";
                ddlHospitales.DataTextField = "NomHospital";

                ddlHospitales.DataBind();
                ddlHospitales.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetTpoAtencion()
        {
            try
            {

                ddlTpoAtencion.Items.Clear();
                ddlTpoAtencion.Items.Add(new ListItem("-- Seleccionar--", "0"));
                ddlTpoAtencion.Items.Add(new ListItem("Ambulatoria", "1"));
                ddlTpoAtencion.Items.Add(new ListItem("Hospitalaria", "2"));

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetEstatusCaso()
        {
            try
            {

                ddlEstatusCaso.Items.Clear();
                ddlEstatusCaso.Items.Add(new ListItem("-- Seleccionar--", "0"));
                ddlEstatusCaso.Items.Add(new ListItem("Hospitalizado", "1"));
                ddlEstatusCaso.Items.Add(new ListItem("En Seguimiento", "2"));
                ddlEstatusCaso.Items.Add(new ListItem("Alta / Cerrado", "3"));

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetInstituciones()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT Id_Institucion, Descripcion " +
                                  "  FROM ITM_13 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlInstituciones.DataSource = dt;

                ddlInstituciones.DataValueField = "Id_Institucion";
                ddlInstituciones.DataTextField = "Descripcion";

                ddlInstituciones.DataBind();
                ddlInstituciones.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetPaqueteMedico(int Id_Institucion)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT Id_Paquete, Nom_Paquete " +
                                  "  FROM ITM_37 " +
                                  " WHERE Id_Institucion = " + Id_Institucion + " " +
                                  "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlPaquetes_MD.DataSource = dt;

                ddlPaquetes_MD.DataValueField = "Id_Paquete";
                ddlPaquetes_MD.DataTextField = "Nom_Paquete";

                ddlPaquetes_MD.DataBind();
                ddlPaquetes_MD.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        
        protected void GetPaqueteMontos(int Id_Paquete)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT Monto_Minimo, Monto_Maximo " +
                                  "  FROM ITM_37 " +
                                  " WHERE Id_Paquete = " + Id_Paquete + " " +
                                  "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                foreach (DataRow row in dt.Rows)
                {
                    // Informacion General
                    TxtMontoMinimo.Text = Convert.ToString(row[0]);
                    TxtMontoMaximo.Text = Convert.ToString(row[1]);
                }

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        private void Inicializar_GrdPaquetes()
        {
            // Crea un DataTable vacío con la estructura necesaria
            DataTable dt = DataTableVacio_Paquetes();

            // Verifica si el DataTable tiene filas
            if (dt.Rows.Count == 0)
            {
                // Mostrar el mensaje de "No hay resultados"
                GrdPaqueteMedico.ShowHeaderWhenEmpty = true;
                GrdPaqueteMedico.EmptyDataText = "No hay resultados.";
            }

            // Enlaza el DataTable (vacío o lleno) al GridView
            GrdPaqueteMedico.DataSource = dt;
            GrdPaqueteMedico.DataBind();
        }
                
        private DataTable DataTableVacio_Paquetes()
        {
            DataTable dt = new DataTable();

            // Define las columnas del DataTable
            dt.Columns.Add("Id_Consecutivo", typeof(string));
            dt.Columns.Add("Id_Institucion", typeof(string));
            dt.Columns.Add("Id_Paquete_Medico", typeof(string));
            dt.Columns.Add("Descripcion", typeof(string));
            dt.Columns.Add("ID_Paquete", typeof(string));
            dt.Columns.Add("Monto_Minimo", typeof(string));
            dt.Columns.Add("Monto_Maximo", typeof(string));
            dt.Columns.Add("Monto_Utilizado", typeof(string));
            dt.Columns.Add("Monto_Restante", typeof(string));
            dt.Columns.Add("Monto_Superado", typeof(string));
            dt.Columns.Add("Observaciones", typeof(string));
            // Agrega más columnas según sea necesario

            return dt;
        }

        private void Inicializar_GrdServicios()
        {
            // Crea un DataTable vacío con la estructura necesaria
            DataTable dt = DataTableVacio_Servicios();

            // Verifica si el DataTable tiene filas
            if (dt.Rows.Count == 0)
            {
                // Mostrar el mensaje de "No hay resultados"
                GrdServicios.ShowHeaderWhenEmpty = true;
                GrdServicios.EmptyDataText = "No hay resultados.";
            }

            // Enlaza el DataTable (vacío o lleno) al GridView
            GrdServicios.DataSource = dt;
            GrdServicios.DataBind();
        }

        private DataTable DataTableVacio_Servicios()
        {
            DataTable dt = new DataTable();

            // Define las columnas del DataTable
            dt.Columns.Add("IdConsecutivo", typeof(string));
            dt.Columns.Add("IdServicio", typeof(string));
            dt.Columns.Add("Desc_Servicio", typeof(string));
            // Agrega más columnas según sea necesario

            return dt;
        }
                
        private void Inicializar_GrdProveedores()
        {
            // Crea un DataTable vacío con la estructura necesaria
            DataTable dt = DataTableVacio_Proveedores();

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
                
        private DataTable DataTableVacio_Proveedores()
        {
            DataTable dt = new DataTable();

            // Define las columnas del DataTable (ITM_35)
            dt.Columns.Add("Id_Proveedor", typeof(string)); 
            dt.Columns.Add("Tpo_Servicio", typeof(string));
            dt.Columns.Add("Nom_Empresa", typeof(string));
            dt.Columns.Add("Email_Empresa", typeof(string));

            dt.Columns.Add("Calle", typeof(string));
            dt.Columns.Add("Num_Exterior", typeof(string));
            dt.Columns.Add("Num_Interior", typeof(string));
            dt.Columns.Add("Estado", typeof(string));
            dt.Columns.Add("Delegacion", typeof(string));
            dt.Columns.Add("Colonia", typeof(string));
            dt.Columns.Add("Codigo_Postal", typeof(string));

            dt.Columns.Add("Tel_Contacto_1", typeof(string));
            dt.Columns.Add("Tel_Contacto_2", typeof(string));
            dt.Columns.Add("Hora_Solicitud", typeof(string));
            dt.Columns.Add("Num_Unidad", typeof(string));
            dt.Columns.Add("Responsable", typeof(string));

            // Agrega más columnas según sea necesario

            return dt;
        }

        private void Inicializar_GrdEstudios()
        {
            // Crea un DataTable vacío con la estructura necesaria
            DataTable dt = DataTableVacio_Estudios();

            // Verifica si el DataTable tiene filas
            if (dt.Rows.Count == 0)
            {
                // Mostrar el mensaje de "No hay resultados"
                GrdEstudios.ShowHeaderWhenEmpty = true;
                GrdEstudios.EmptyDataText = "No hay resultados.";
            }

            // Enlaza el DataTable (vacío o lleno) al GridView
            GrdEstudios.DataSource = dt;
            GrdEstudios.DataBind();
        }

        private DataTable DataTableVacio_Estudios()
        {
            DataTable dt = new DataTable();

            // Define las columnas del DataTable
            dt.Columns.Add("IdEstudios", typeof(string));
            dt.Columns.Add("Desc_Estudios", typeof(string));
            // Agrega más columnas según sea necesario

            return dt;
        }

        protected void GrdDocumentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GrdDocumentos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdDocumentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            if (Variables.wICD)
            {
                Eliminar_ITM_32();
            } else if (Variables.wCPT)
            {
                Eliminar_ITM_33();
            } else if (Variables.wEstudios)
            {
                Eliminar_ITM_36();
            } else if (Variables.wServicios)
            {
                Eliminar_ITM_07();
            } else if (Variables.wProveedor)
            {
                Eliminar_ITM_35();
            } else if(Variables.wPaquete)
            {
                Eliminar_ITM_38();
            }

        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Del_Doc_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCancelar_Del_Doc_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_CrearCuaderno_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCancelar_CrearCuaderno_Click(object sender, EventArgs e)
        {

        }


        protected void BtnAnularPnl2_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            inhabilitar(this.Controls);
            habilitar_controles();

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            btnEditarPnl2.Visible = true;
            btnActualizarPnl2.Visible = false;
            BtnAnularPnl2.Visible = false;
        }

        protected void btnEditarPnl2_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl2
            TxtNumSiniestro.Enabled = true;
            TxtNumPoliza.Enabled = true;
            ddlEstatusCaso.Enabled = true;
            TxtNomAjustador.Enabled = true;
            TxtEmailAjustador.Enabled = true;
            TxtTelAjustador.Enabled = true;
            TxtFechaIniVigencia.Enabled = true;
            TxtFechaFinVigencia.Enabled = true;
            TxtFechaOcurrencia.Enabled = true;
            TxtHoraOcurrencia.Enabled = true;
            TxtHoraRecepcion.Enabled = true;
            TxtDetalleReporte.Enabled = true;
            //TxtCalle.Enabled = true;
            //TxtNumExterior.Enabled = true;
            //TxtNumInterior.Enabled = true;
            //ddlEstado.Enabled = true;
            //ddlMunicipios.Enabled = true;
            //TxtColonia.Enabled = true;
            //TxtCodigoPostal.Enabled = true;

            TxtNomLesionado.Enabled = true;
            TxtFecNacimiento.Enabled = true;
            TxtSexo.Enabled = true;
            TxtEmailLesionado.Enabled = true;
            TxtTelLesionado.Enabled = true;
            TxtEdadLesionado.Enabled = true;
            TxtNomResponsable.Enabled = true;
            TxtTelResponsable.Enabled = true;
            TxtEmailResponsable.Enabled = true;

            btnEditarPnl2.Visible = false;
            btnActualizarPnl2.Visible = true;
            BtnAnularPnl2.Visible = true;
        }

        protected void btnActualizarPnl2_Click(object sender, EventArgs e)
        {

            string input = TxtHoraOcurrencia.Text;
            if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^(?:[01]\d|2[0-3]):[0-5]\d$"))
            {
                // Maneja la entrada inválida.
                // Por ejemplo, muestra un mensaje de error al usuario.
                LblMessage.Text = "Formato Hora inválido. Use hh:mm.";
                this.mpeMensaje.Show();
            }
            else
            {
                Actualizar_ITM_73();

                Insertar_ITM_73_1();

                inhabilitar(this.Controls);
                habilitar_controles();

                // ddlTpoAsegurado.Enabled = true;
                // ddlEstSiniestro.Enabled = true;
                // ddlConclusion.Enabled = true;

                btnEditarPnl2.Visible = true;
                btnActualizarPnl2.Visible = false;
                BtnAnularPnl2.Visible = false;

                LblMessage.Text = "Se han aplicado los cambios correctamente";
                this.mpeMensaje.Show();
            }
        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Variables.wRef = string.Empty;
            Variables.wSubRef = 0;
            Variables.wIdProyecto = 0;
            Variables.wPrefijo_Aseguradora = string.Empty;
            Variables.wIdTpoAsunto = 0;

            Response.Redirect("fwGM_Reporte_Alta_Asunto.aspx", true);
        }

        protected void Actualizar_ITM_73()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Actualizar registro tabla (ITM_73)
                string strQuery = "UPDATE ITM_73 " +
                                  "   SET NumSiniestro = '" + TxtNumSiniestro.Text.Trim() + "', " +
                                  "       NumPoliza =  '" + TxtNumPoliza.Text.Trim() + "', " +
                                  "       NomAjustador = '" + TxtNomAjustador.Text.Trim() + "', " +
                                  "       IdEstatusCaso = " + ddlEstatusCaso.SelectedValue + " " +
                                  " WHERE Referencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = " + Variables.wSubRef + " ";

                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Insertar_ITM_73_1()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Informacion General
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;
                string Email_Ajustador = TxtEmailAjustador.Text;
                string Tel_Ajustador = TxtTelAjustador.Text;
                string Fec_IniVigencia = TxtFechaIniVigencia.Text;
                string Fec_FinVigencia = TxtFechaFinVigencia.Text;
                string Fec_Ocurrencia = TxtFechaOcurrencia.Text;
                string Hora_Ocurrencia = TxtHoraOcurrencia.Text;
                string Hora_Recepcion = TxtHoraRecepcion.Text;
                string Detalle_Reporte = TxtDetalleReporte.Text;
                string Calle = string.Empty;            //TxtCalle.Text;
                string Num_Exterior = string.Empty;     // TxtNumExterior.Text;
                string Num_Interior = string.Empty;     //TxtNumInterior.Text;
                string Estado = string.Empty;           //ddlEstado.SelectedValue;
                string Delegacion = string.Empty;       //ddlMunicipios.SelectedValue;
                string Colonia = string.Empty;          //TxtColonia.Text;
                string Codigo_Postal = string.Empty;    //TxtCodigoPostal.Text;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @" INSERT INTO ITM_73_1 (Referencia, SubReferencia, Email_Ajustador, Tel_Ajustador, Fec_IniVigencia, Fec_FinVigencia, Fec_Ocurrencia, Hora_Recepcion, Hora_Ocurrencia, " +
                                    "                       Detalle_Reporte, Calle, Num_Exterior, Num_Interior, Estado, Delegacion, Colonia, Codigo_Postal, " +
                                    "                       Id_Usuario, IdStatus)" +
                                    " VALUES (@Referencia, @SubReferencia, @Email_Ajustador, @Tel_Ajustador, @Fec_IniVigencia, @Fec_FinVigencia, @Fec_Ocurrencia, @Hora_Recepcion, @Hora_Ocurrencia, " +
                                    "        @Detalle_Reporte, @Calle, @Num_Exterior, @Num_Interior, @Estado, @Delegacion, @Colonia, @Codigo_Postal, " +
                                    "        @Id_Usuario, @IdStatus)" +
                                    " ON DUPLICATE KEY UPDATE " +
                                    "    Email_Ajustador = VALUES(Email_Ajustador), " +
                                    "    Tel_Ajustador = VALUES(Tel_Ajustador), " +
                                    "    Fec_IniVigencia = VALUES(Fec_IniVigencia), " +
                                    "    Fec_FinVigencia = VALUES(Fec_FinVigencia), " +
                                    "    Fec_Ocurrencia = VALUES(Fec_Ocurrencia), " +
                                    "    Hora_Recepcion = VALUES(Hora_Recepcion), " +
                                    "    Hora_Ocurrencia = VALUES(Hora_Ocurrencia), " +
                                    "    Detalle_Reporte = VALUES(Detalle_Reporte), " +
                                    "    Calle = VALUES(Calle), " +
                                    "    Num_Exterior = VALUES(Num_Exterior), " +
                                    "    Num_Interior = VALUES(Num_Interior), " +
                                    "    Estado = VALUES(Estado), " +
                                    "    Delegacion = VALUES(Delegacion), " +
                                    "    Colonia = VALUES(Colonia), " +
                                    "    Codigo_Postal = VALUES(Codigo_Postal), " +
                                    "    Id_Usuario = VALUES(Id_Usuario), " +
                                    "    IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@Email_Ajustador", Email_Ajustador);
                    cmd.Parameters.AddWithValue("@Tel_Ajustador", Tel_Ajustador);
                    cmd.Parameters.AddWithValue("@Fec_IniVigencia", Fec_IniVigencia);
                    cmd.Parameters.AddWithValue("@Fec_FinVigencia", Fec_FinVigencia);
                    cmd.Parameters.AddWithValue("@Fec_Ocurrencia", Fec_Ocurrencia);
                    cmd.Parameters.AddWithValue("@Hora_Recepcion", Hora_Recepcion);
                    cmd.Parameters.AddWithValue("@Hora_Ocurrencia", Hora_Ocurrencia);
                    cmd.Parameters.AddWithValue("@Detalle_Reporte", Detalle_Reporte);
                    cmd.Parameters.AddWithValue("@Calle", Calle);
                    cmd.Parameters.AddWithValue("@Num_Exterior", Num_Exterior);
                    cmd.Parameters.AddWithValue("@Num_Interior", Num_Interior);
                    cmd.Parameters.AddWithValue("@Estado", Estado);
                    cmd.Parameters.AddWithValue("@Delegacion", Delegacion);
                    cmd.Parameters.AddWithValue("@Colonia", Colonia);
                    cmd.Parameters.AddWithValue("@Codigo_Postal", Codigo_Postal);
                    cmd.Parameters.AddWithValue("@Id_Usuario", Id_Usuario);
                    cmd.Parameters.AddWithValue("@IdStatus", IdStatus);
                });

                dbConn.Close();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Insertar_ITM_73_2()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Informacion General
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                string Nom_Lesionado = TxtNomLesionado.Text;
                string Fec_Nacimiento = TxtFecNacimiento.Text;
                string Sexo = TxtSexo.Text;
                string Email_Lesionado = TxtEmailLesionado.Text;
                string Tel_Celular = TxtTelLesionado.Text;
                string Edad_Lesionado = TxtEdadLesionado.Text;
                string RFC_Lesionado = TxtRFC_Lesionado.Text;
                string Tpo_Evento = ddlTipoEvento.SelectedValue;
                string Desc_Lesiones = TxtDescLesiones.Text;

                string Calle = TxtCalleLesionado.Text;
                string Num_Exterior = TxtNumExtLesionado.Text;
                string Num_Interior = TxtNumIntLesionado.Text;
                string Estado = ddlEstadoLesionado.SelectedValue;
                string Delegacion = ddlMunicipiosLesionado.SelectedValue;
                string Colonia = TxtColoniaLesionado.Text;
                string Codigo_Postal = TxtCPostalLesionado.Text;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @" INSERT INTO ITM_73_2 (Referencia, SubReferencia, Nom_Lesionado, Fec_Nacimiento, Sexo, Email_Lesionado, Tel_Celular, Edad, RFC_Lesionado, " +
                                    "                       Tpo_Evento, Desc_Lesiones, Calle, Num_Exterior, Num_Interior, Estado, Delegacion, Colonia, Codigo_Postal, " +
                                    "                       Id_Usuario, IdStatus)" +
                                    " VALUES (@Referencia, @SubReferencia, @Nom_Lesionado, @Fec_Nacimiento, @Sexo, @Email_Lesionado, @Tel_Celular, @Edad, @RFC_Lesionado, " +
                                    "        @Tpo_Evento, @Desc_Lesiones, @Calle, @Num_Exterior, @Num_Interior, @Estado, @Delegacion, @Colonia, @Codigo_Postal, " +
                                    "        @Id_Usuario, @IdStatus)" +
                                    " ON DUPLICATE KEY UPDATE " +
                                    "    Nom_Lesionado = VALUES(Nom_Lesionado), " +
                                    "    Fec_Nacimiento = VALUES(Fec_Nacimiento), " +
                                    "    Sexo = VALUES(Sexo), " +
                                    "    Email_Lesionado = VALUES(Email_Lesionado), " +
                                    "    Tel_Celular = VALUES(Tel_Celular), " +
                                    "    Edad = VALUES(Edad), " +
                                    "    RFC_Lesionado = VALUES(RFC_Lesionado), " +
                                    "    Tpo_Evento = VALUES(Tpo_Evento), " +
                                    "    Desc_Lesiones = VALUES(Desc_Lesiones), " +
                                    "    Calle = VALUES(Calle), " +
                                    "    Num_Exterior = VALUES(Num_Exterior), " +
                                    "    Num_Interior = VALUES(Num_Interior), " +
                                    "    Estado = VALUES(Estado), " +
                                    "    Delegacion = VALUES(Delegacion), " +
                                    "    Colonia = VALUES(Colonia), " +
                                    "    Codigo_Postal = VALUES(Codigo_Postal), " +
                                    "    Id_Usuario = VALUES(Id_Usuario), " +
                                    "    IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@Nom_Lesionado", Nom_Lesionado);
                    cmd.Parameters.AddWithValue("@Fec_Nacimiento", Fec_Nacimiento);
                    cmd.Parameters.AddWithValue("@Sexo", Sexo);
                    cmd.Parameters.AddWithValue("@Email_Lesionado", Email_Lesionado);
                    cmd.Parameters.AddWithValue("@Tel_Celular", Tel_Celular);
                    cmd.Parameters.AddWithValue("@Edad", Edad_Lesionado);
                    cmd.Parameters.AddWithValue("@RFC_Lesionado", RFC_Lesionado);
                    cmd.Parameters.AddWithValue("@Tpo_Evento", Tpo_Evento);
                    cmd.Parameters.AddWithValue("@Desc_Lesiones", Desc_Lesiones);
                    cmd.Parameters.AddWithValue("@Calle", Calle);
                    cmd.Parameters.AddWithValue("@Num_Exterior", Num_Exterior);
                    cmd.Parameters.AddWithValue("@Num_Interior", Num_Interior);
                    cmd.Parameters.AddWithValue("@Estado", Estado);
                    cmd.Parameters.AddWithValue("@Delegacion", Delegacion);
                    cmd.Parameters.AddWithValue("@Colonia", Colonia);
                    cmd.Parameters.AddWithValue("@Codigo_Postal", Codigo_Postal);
                    cmd.Parameters.AddWithValue("@Id_Usuario", Id_Usuario);
                    cmd.Parameters.AddWithValue("@IdStatus", IdStatus);
                });

                dbConn.Close();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Insertar_ITM_73_3()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Informacion General
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                string Nom_Responsable = TxtNomResponsable.Text;
                string Parentesco = TxtParentesco.Text;
                string Edad = TxtEdadResponsable.Text;
                string Tel_Responsable = TxtTelResponsable.Text;
                string RFC_Responsable = TxtRFC_Responsable.Text;
                string Email_Responsable = TxtEmailResponsable.Text;

                string Calle = TxtCalleResponsable.Text;
                string Num_Exterior = TxtNumExtResponsable.Text;
                string Num_Interior = TxtNumIntResponsable.Text;
                string Estado = ddlEstadoResponsable.SelectedValue;
                string Delegacion = ddlMunicipiosResponsable.SelectedValue;
                string Colonia = TxtColoniaResponsable.Text;
                string Codigo_Postal = TxtCPostalResponsable.Text;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @" INSERT INTO ITM_73_3 (Referencia, SubReferencia, Nom_Responsable, Parentesco, Edad, Tel_Responsable, RFC_Responsable, Email_Responsable, " +
                                    "                       Calle, Num_Exterior, Num_Interior, Estado, Delegacion, Colonia, Codigo_Postal, " +
                                    "                       Id_Usuario, IdStatus)" +
                                    " VALUES (@Referencia, @SubReferencia, @Nom_Responsable, @Parentesco, @Edad, @Tel_Responsable, @RFC_Responsable, @Email_Responsable, " +
                                    "        @Calle, @Num_Exterior, @Num_Interior, @Estado, @Delegacion, @Colonia, @Codigo_Postal, " +
                                    "        @Id_Usuario, @IdStatus)" +
                                    " ON DUPLICATE KEY UPDATE " +
                                    "    Nom_Responsable = VALUES(Nom_Responsable), " +
                                    "    Parentesco = VALUES(Parentesco), " +
                                    "    Edad = VALUES(Edad), " +
                                    "    Tel_Responsable = VALUES(Tel_Responsable), " +
                                    "    RFC_Responsable = VALUES(RFC_Responsable), " +
                                    "    Email_Responsable = VALUES(Email_Responsable), " +
                                    "    Calle = VALUES(Calle), " +
                                    "    Num_Exterior = VALUES(Num_Exterior), " +
                                    "    Num_Interior = VALUES(Num_Interior), " +
                                    "    Estado = VALUES(Estado), " +
                                    "    Delegacion = VALUES(Delegacion), " +
                                    "    Colonia = VALUES(Colonia), " +
                                    "    Codigo_Postal = VALUES(Codigo_Postal), " +
                                    "    Id_Usuario = VALUES(Id_Usuario), " +
                                    "    IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@Nom_Responsable", Nom_Responsable);
                    cmd.Parameters.AddWithValue("@Parentesco", Parentesco);
                    cmd.Parameters.AddWithValue("@Edad", Edad);
                    cmd.Parameters.AddWithValue("@Tel_Responsable", Tel_Responsable);
                    cmd.Parameters.AddWithValue("@RFC_Responsable", RFC_Responsable);
                    cmd.Parameters.AddWithValue("@Email_Responsable", Email_Responsable);
                    cmd.Parameters.AddWithValue("@Calle", Calle);
                    cmd.Parameters.AddWithValue("@Num_Exterior", Num_Exterior);
                    cmd.Parameters.AddWithValue("@Num_Interior", Num_Interior);
                    cmd.Parameters.AddWithValue("@Estado", Estado);
                    cmd.Parameters.AddWithValue("@Delegacion", Delegacion);
                    cmd.Parameters.AddWithValue("@Colonia", Colonia);
                    cmd.Parameters.AddWithValue("@Codigo_Postal", Codigo_Postal);
                    cmd.Parameters.AddWithValue("@Id_Usuario", Id_Usuario);
                    cmd.Parameters.AddWithValue("@IdStatus", IdStatus);
                });

                dbConn.Close();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }


        protected void Insertar_ITM_73_4()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Informacion General
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                int IdLinea = string.IsNullOrWhiteSpace(ddlLineaOcurrencia.SelectedValue) ? 0 : int.Parse(ddlLineaOcurrencia.SelectedValue);
                int IdEstacion = string.IsNullOrWhiteSpace(ddlEstacionOcurrencia.SelectedValue) ? 0 : int.Parse(ddlEstacionOcurrencia.SelectedValue);

                int IdZona = string.IsNullOrWhiteSpace(ddlZonas.SelectedValue) ? 0 : int.Parse(ddlZonas.SelectedValue);
                int IdHospital = string.IsNullOrWhiteSpace(ddlHospitales.SelectedValue) ? 0 : int.Parse(ddlHospitales.SelectedValue);
                int TpoAtencion = string.IsNullOrWhiteSpace(ddlTpoAtencion.SelectedValue) ? 0 : int.Parse(ddlTpoAtencion.SelectedValue);

                string Email_Atencion = TxtCorreoElectronico.Text.Trim();
                string Tel_Contacto_1 = TxtTelAtencionContacto1.Text.Trim();
                string Tel_Contacto_2 = TxtTelAtencionContacto2.Text.Trim();

                string FechaIngreso = TxtFechaIngreso.Text.Trim();
                string Hora_Ingreso = TxtHoraIngreso.Text.Trim();
                string FechaRecepcion = TxtFechaRecepcionNM.Text.Trim();
                string Hora_Recepcion = TxtHoraRecepcionNM.Text.Trim();

                string FechaAlta = TxtFechaAlta.Text.Trim();
                string Hora_Alta = TxtHoraAlta.Text.Trim();
                string FechaEnvio = TxtFechaEnvio.Text.Trim();
                string Hora_Envio = TxtHoraEnvio.Text.Trim();

                string FechaVigencia = TxtFechaVigencia.Text.Trim();

                decimal MontoAutorizado = 0;

                if (!string.IsNullOrWhiteSpace(TxtMontoAutorizado.Text.Trim()))
                {
                    decimal.TryParse(TxtMontoAutorizado.Text.Trim(), out MontoAutorizado);
                }

                string Calle = TxtCalleAtencion.Text;
                string Num_Exterior = TxtNumExtAtencion.Text;
                string Num_Interior = TxtNumIntAtencion.Text;
                string Estado = ddlEstadoAtencion.SelectedValue;
                string Delegacion = ddlMunicipiosAtencion.SelectedValue;
                string Colonia = TxtColoniaAtencion.Text;
                string Codigo_Postal = TxtCPostalAtencion.Text;

                string Diagnostico = TxtDiagnostico.Text.Trim();
                string Observaciones = TxtObservaciones_DA.Text.Trim();
                string Ubicacion = TxtRef_Ubicacion.Text.Trim();
                string PlanTratamiento = TxtPlanTratamiento.Text.Trim();

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @" INSERT INTO ITM_73_4 (Referencia, SubReferencia, IdLinea, IdEstacion, IdZona, IdHospital, " +
                                   "     TpoAtencion, Email_Atencion, Tel_Contacto_1, Tel_Contacto_2, FechaIngreso, Hora_Ingreso," +
                                   "     FechaRecepcion, Hora_Recepcion, FechaAlta, Hora_Alta, FechaEnvio, Hora_Envio, " +
                                   "     Calle, Num_Exterior, Num_Interior, Estado, Delegacion, Colonia, Codigo_Postal, " +
                                   "     FechaVigencia, MontoAutorizado, Diagnostico, Observaciones, Ubicacion, PlanTratamiento, " +
                                   "     Id_Usuario, IdStatus)" +
                                   " VALUES (@Referencia, @SubReferencia, @IdLinea, @IdEstacion, @IdZona, @IdHospital, " +
                                   "         @TpoAtencion, @Email_Atencion, @Tel_Contacto_1, @Tel_Contacto_2, @FechaIngreso, @Hora_Ingreso, " +
                                   "         @FechaRecepcion, @Hora_Recepcion, @FechaAlta, @Hora_Alta, @FechaEnvio, @Hora_Envio, " +
                                   "         @Calle, @Num_Exterior, @Num_Interior, @Estado, @Delegacion, @Colonia, @Codigo_Postal, " +
                                   "         @FechaVigencia, @MontoAutorizado, @Diagnostico, @Observaciones, @Ubicacion, @PlanTratamiento, " +
                                   "         @Id_Usuario, @IdStatus)" +
                                   " ON DUPLICATE KEY UPDATE " +
                                   "    IdLinea = VALUES(IdLinea), " +
                                   "    IdEstacion = VALUES(IdEstacion), " +
                                   "    IdZona = VALUES(IdZona), " +
                                   "    IdHospital = VALUES(IdHospital), " +
                                   "    TpoAtencion = VALUES(TpoAtencion), " +
                                   "    Email_Atencion = VALUES(Email_Atencion), " +
                                   "    Tel_Contacto_1 = VALUES(Tel_Contacto_1), " +
                                   "    Tel_Contacto_2 = VALUES(Tel_Contacto_2), " +
                                   "    FechaIngreso = VALUES(FechaIngreso), " +
                                   "    Hora_Ingreso = VALUES(Hora_Ingreso), " +
                                   "    FechaRecepcion = VALUES(FechaRecepcion), " +
                                   "    Hora_Recepcion = VALUES(Hora_Recepcion), " +
                                   "    FechaAlta = VALUES(FechaAlta), " +
                                   "    Hora_Alta = VALUES(Hora_Alta), " +
                                   "    FechaEnvio = VALUES(FechaEnvio), " +
                                   "    Hora_Envio = VALUES(Hora_Envio), " +
                                   "    Calle = VALUES(Calle), " +
                                   "    Num_Exterior = VALUES(Num_Exterior), " +
                                   "    Num_Interior = VALUES(Num_Interior), " +
                                   "    Estado = VALUES(Estado), " +
                                   "    Delegacion = VALUES(Delegacion), " +
                                   "    Colonia = VALUES(Colonia), " +
                                   "    Codigo_Postal = VALUES(Codigo_Postal), " +
                                   "    FechaVigencia = VALUES(FechaVigencia), " +
                                   "    MontoAutorizado = VALUES(MontoAutorizado), " +
                                   "    Diagnostico = VALUES(Diagnostico), " +
                                   "    Observaciones = VALUES(Observaciones), " +
                                   "    Ubicacion = VALUES(Ubicacion), " +
                                   "    PlanTratamiento = VALUES(PlanTratamiento), " +
                                   "    Id_Usuario = VALUES(Id_Usuario), " +
                                   "    IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@IdLinea", IdLinea);
                    cmd.Parameters.AddWithValue("@IdEstacion", IdEstacion);
                    cmd.Parameters.AddWithValue("@IdZona", IdZona);
                    cmd.Parameters.AddWithValue("@IdHospital", IdHospital);
                    cmd.Parameters.AddWithValue("@TpoAtencion", TpoAtencion);
                    cmd.Parameters.AddWithValue("@Email_Atencion", Email_Atencion);
                    cmd.Parameters.AddWithValue("@Tel_Contacto_1", Tel_Contacto_1);
                    cmd.Parameters.AddWithValue("@Tel_Contacto_2", Tel_Contacto_2);
                    cmd.Parameters.AddWithValue("@FechaIngreso", FechaIngreso);
                    cmd.Parameters.AddWithValue("@Hora_Ingreso", Hora_Ingreso);
                    cmd.Parameters.AddWithValue("@FechaRecepcion", FechaRecepcion);
                    cmd.Parameters.AddWithValue("@Hora_Recepcion", Hora_Recepcion);
                    cmd.Parameters.AddWithValue("@FechaAlta", FechaAlta);
                    cmd.Parameters.AddWithValue("@Hora_Alta", Hora_Alta);
                    cmd.Parameters.AddWithValue("@FechaEnvio", FechaEnvio);
                    cmd.Parameters.AddWithValue("@Hora_Envio", Hora_Envio);
                    cmd.Parameters.AddWithValue("@Calle", Calle);
                    cmd.Parameters.AddWithValue("@Num_Exterior", Num_Exterior);
                    cmd.Parameters.AddWithValue("@Num_Interior", Num_Interior);
                    cmd.Parameters.AddWithValue("@Estado", Estado);
                    cmd.Parameters.AddWithValue("@Delegacion", Delegacion);
                    cmd.Parameters.AddWithValue("@Colonia", Colonia);
                    cmd.Parameters.AddWithValue("@Codigo_Postal", Codigo_Postal);
                    cmd.Parameters.AddWithValue("@FechaVigencia", FechaVigencia);
                    cmd.Parameters.AddWithValue("@MontoAutorizado", MontoAutorizado);
                    cmd.Parameters.AddWithValue("@Diagnostico", Diagnostico);
                    cmd.Parameters.AddWithValue("@Observaciones", Observaciones);
                    cmd.Parameters.AddWithValue("@Ubicacion", Ubicacion);
                    cmd.Parameters.AddWithValue("@PlanTratamiento", PlanTratamiento);
                    cmd.Parameters.AddWithValue("@Id_Usuario", Id_Usuario);
                    cmd.Parameters.AddWithValue("@IdStatus", IdStatus);
                });

                dbConn.Close();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Insertar_ITM_73_5()
        {
            // PAQUETES MEDICOS
        }

        protected void Insertar_ITM_73_6()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Informacion General
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                string Alergias = TxtAlergias.Text;
                string Enfermedades = TxtEnfermedades.Text;
                string Medicamentos = TxtMedicamentos.Text;
                string Alcohol = TxtAlcohol.Text;
                string Sustancias = TxtSustancias.Text;
                string Observaciones = TxtObservaciones_PA.Text;
                string Diagnostico = TxtDiagnosticoPreliminar.Text;
                string Comentarios = TxtComentariosMedicos.Text;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @" INSERT INTO ITM_73_6 (Referencia, SubReferencia, Alergias, Enfermedades, Medicamentos, Alcohol, Sustancias, " +
                                    "                       Observaciones, Diagnostico, Comentarios, Id_Usuario, IdStatus)" +
                                    " VALUES (@Referencia, @SubReferencia, @Alergias, @Enfermedades, @Medicamentos, @Alcohol, @Sustancias, " +
                                    "        @Observaciones, @Diagnostico, @Comentarios, @Id_Usuario, @IdStatus)" +
                                    " ON DUPLICATE KEY UPDATE " +
                                    "    Alergias = VALUES(Alergias), " +
                                    "    Enfermedades = VALUES(Enfermedades), " +
                                    "    Medicamentos = VALUES(Medicamentos), " +
                                    "    Alcohol = VALUES(Alcohol), " +
                                    "    Sustancias = VALUES(Sustancias), " +
                                    "    Observaciones = VALUES(Observaciones), " +
                                    "    Diagnostico = VALUES(Diagnostico), " +
                                    "    Comentarios = VALUES(Comentarios), " +
                                    "    Id_Usuario = VALUES(Id_Usuario), " +
                                    "    IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@Alergias", Alergias);
                    cmd.Parameters.AddWithValue("@Enfermedades", Enfermedades);
                    cmd.Parameters.AddWithValue("@Medicamentos", Medicamentos);
                    cmd.Parameters.AddWithValue("@Alcohol", Alcohol);
                    cmd.Parameters.AddWithValue("@Sustancias", Sustancias);
                    cmd.Parameters.AddWithValue("@Observaciones", Observaciones);
                    cmd.Parameters.AddWithValue("@Diagnostico", Diagnostico);
                    cmd.Parameters.AddWithValue("@Comentarios", Comentarios);
                    cmd.Parameters.AddWithValue("@Id_Usuario", Id_Usuario);
                    cmd.Parameters.AddWithValue("@IdStatus", IdStatus);
                });

                dbConn.Close();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void GetSeccion_2()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "  WITH CTE AS ( SELECT ITM_22.Descripcion AS Descripcion_22, ITM_25.IdDocumento, " +
                                  "                ROW_NUMBER() OVER (ORDER BY ITM_25.IdPosicion) AS RowNumber " +
                                  "  FROM ITM_22 " +
                                  " INNER JOIN ITM_25 ON ITM_22.IdDocumento = ITM_25.IdDocumento " +
                                  " WHERE ITM_22.IdStatus = 1 AND ITM_25.IdProyecto =  " + Variables.wIdProyecto + " AND ITM_25.bSeleccion = 1 " +
                                  "   AND ITM_25.IdSeccion = 2 AND ITM_25.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  " ), " +
                                  " Seleccion_95 AS ( SELECT IdCategoria, bSeleccion " +
                                  "  FROM ITM_95 " +
                                  " WHERE IdProyecto = " + Variables.wIdProyecto + " AND IdSeccion = 2 AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND Referencia = '" + Variables.wRef + "' AND SubReferencia = " + Variables.wSubRef + " ) " +
                                  "SELECT COALESCE(CTE1.Descripcion_22, '') AS Columna1, " +
                                  "       COALESCE(S95_1.bSeleccion, 0) AS ChBoxSeccion_2_1, " +
                                  "       COALESCE(CTE2.Descripcion_22, '') AS Columna2, " +
                                  "       COALESCE(S95_2.bSeleccion, 0) AS ChBoxSeccion_2_2," +
                                  "       COALESCE(CTE3.Descripcion_22, '') AS Columna3, " +
                                  "       COALESCE(S95_3.bSeleccion, 0) AS ChBoxSeccion_2_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN Seleccion_95 S95_1 ON CTE1.IdDocumento = S95_1.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN Seleccion_95 S95_2 ON CTE2.IdDocumento = S95_2.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                  "  LEFT JOIN Seleccion_95 S95_3 ON CTE3.IdDocumento = S95_3.IdCategoria " +
                                  " ORDER BY CTE1.RowNumber ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    // grdSeccion_2.ShowHeaderWhenEmpty = true;
                    // grdSeccion_2.EmptyDataText = "No hay resultados.";
                }

                // grdSeccion_2.DataSource = dt;
                // grdSeccion_2.DataBind();

                dbConn.Close();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetSeccion_5()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "  WITH CTE AS ( SELECT ITM_24.Descripcion AS Descripcion_24, ITM_25.IdDocumento, " +
                                  "                ROW_NUMBER() OVER (ORDER BY ITM_25.IdPosicion) AS RowNumber " +
                                  "  FROM ITM_24 " +
                                  " INNER JOIN ITM_25 ON ITM_24.IdDocumento = ITM_25.IdDocumento " +
                                  " WHERE ITM_24.IdStatus = 1 AND ITM_25.IdProyecto =  " + Variables.wIdProyecto + " AND ITM_25.bSeleccion = 1 " +
                                  "   AND ITM_25.IdSeccion = 5 AND ITM_25.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  " ), " +
                                  " Seleccion_95 AS ( SELECT IdCategoria, bSeleccion " +
                                  "  FROM ITM_95 " +
                                  " WHERE IdProyecto = " + Variables.wIdProyecto + " AND IdSeccion = 5 AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND Referencia = '" + Variables.wRef + "' AND SubReferencia = " + Variables.wSubRef + " ) " +
                                  "SELECT COALESCE(CTE1.Descripcion_24, '') AS Columna1, " +
                                  "       COALESCE(S95_1.bSeleccion, 0) AS ChBoxSeccion_5_1, " +
                                  "       COALESCE(CTE2.Descripcion_24, '') AS Columna2, " +
                                  "       COALESCE(S95_2.bSeleccion, 0) AS ChBoxSeccion_5_2," +
                                  "       COALESCE(CTE3.Descripcion_24, '') AS Columna3, " +
                                  "       COALESCE(S95_3.bSeleccion, 0) AS ChBoxSeccion_5_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN Seleccion_95 S95_1 ON CTE1.IdDocumento = S95_1.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN Seleccion_95 S95_2 ON CTE2.IdDocumento = S95_2.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                  "  LEFT JOIN Seleccion_95 S95_3 ON CTE3.IdDocumento = S95_3.IdCategoria " +
                                  " ORDER BY CTE1.RowNumber ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    // grdSeccion_5.ShowHeaderWhenEmpty = true;
                    // grdSeccion_5.EmptyDataText = "No hay resultados.";
                }

                // grdSeccion_5.DataSource = dt;
                // grdSeccion_5.DataBind();

                dbConn.Close();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }


        protected void grdSeccion_2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Encuentra los CheckBoxes dentro de la fila
                CheckBox chBoxSeccion_2_1 = (CheckBox)e.Row.FindControl("ChBoxSeccion_2_1");
                CheckBox chBoxSeccion_2_2 = (CheckBox)e.Row.FindControl("ChBoxSeccion_2_2");
                CheckBox chBoxSeccion_2_3 = (CheckBox)e.Row.FindControl("ChBoxSeccion_2_3");

                // Obtén los datos de la fila actual
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                // Obtén los valores de las columnas y conviértelos a tipo entero
                string valorColumna1 = Convert.ToString(rowView["Columna1"]);
                string valorColumna2 = Convert.ToString(rowView["Columna2"]);
                string valorColumna3 = Convert.ToString(rowView["Columna3"]);

                // Obtén el valor de la columna Columna1
                int valorChBoxSeccion_2_1 = Convert.ToInt32(rowView["ChBoxSeccion_2_1"]);
                int valorChBoxSeccion_2_2 = Convert.ToInt32(rowView["ChBoxSeccion_2_2"]);
                int valorChBoxSeccion_2_3 = Convert.ToInt32(rowView["ChBoxSeccion_2_3"]);

                // Verifica si la columna Columna1 tiene datos y muestra u oculta el CheckBox
                if (!string.IsNullOrEmpty(valorColumna1))
                {
                    chBoxSeccion_2_1.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_2_1.Checked = valorChBoxSeccion_2_1 == 1;
                }
                else
                {
                    chBoxSeccion_2_1.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna2))
                {
                    chBoxSeccion_2_2.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_2_2.Checked = valorChBoxSeccion_2_2 == 1;
                }
                else
                {
                    chBoxSeccion_2_2.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna3))
                {
                    chBoxSeccion_2_3.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_2_3.Checked = valorChBoxSeccion_2_3 == 1;
                }
                else
                {
                    chBoxSeccion_2_3.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
            }
        }

        protected void grdSeccion_5_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Encuentra los CheckBoxes dentro de la fila
                CheckBox chBoxSeccion_5_1 = (CheckBox)e.Row.FindControl("ChBoxSeccion_5_1");
                CheckBox chBoxSeccion_5_2 = (CheckBox)e.Row.FindControl("ChBoxSeccion_5_2");
                CheckBox chBoxSeccion_5_3 = (CheckBox)e.Row.FindControl("ChBoxSeccion_5_3");

                // Obtén los datos de la fila actual
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                // Obtén los valores de las columnas y conviértelos a tipo entero
                string valorColumna1 = Convert.ToString(rowView["Columna1"]);
                string valorColumna2 = Convert.ToString(rowView["Columna2"]);
                string valorColumna3 = Convert.ToString(rowView["Columna3"]);

                // Obtén el valor de la columna Columna1
                int valorChBoxSeccion_5_1 = Convert.ToInt32(rowView["ChBoxSeccion_5_1"]);
                int valorChBoxSeccion_5_2 = Convert.ToInt32(rowView["ChBoxSeccion_5_2"]);
                int valorChBoxSeccion_5_3 = Convert.ToInt32(rowView["ChBoxSeccion_5_3"]);

                // Verifica si la columna Columna1 tiene datos y muestra u oculta el CheckBox
                if (!string.IsNullOrEmpty(valorColumna1))
                {
                    chBoxSeccion_5_1.Visible = true;    // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_5_1.Checked = valorChBoxSeccion_5_1 == 1;
                }
                else
                {
                    chBoxSeccion_5_1.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna2))
                {
                    chBoxSeccion_5_2.Visible = true;    // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_5_2.Checked = valorChBoxSeccion_5_2 == 1;
                }
                else
                {
                    chBoxSeccion_5_2.Visible = false;   // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna3))
                {
                    chBoxSeccion_5_3.Visible = true;    // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_5_3.Checked = valorChBoxSeccion_5_3 == 1;
                }
                else
                {
                    chBoxSeccion_5_3.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
            }
        }


        public int GetConsulta_Datos_Generales(string pReferencia, string pSubReferencia)
        {

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t0.IdAsunto, t0.SubReferencia, CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END as Referencia_Sub, " +
                           "       t0.NumSiniestro, t0.NumPoliza, t0.NomAjustador, " +
                           "       A.Email_Ajustador, A.Tel_Ajustador, A.Fec_IniVigencia, A.Fec_FinVigencia, A.Fec_Ocurrencia, A.Hora_Recepcion, A.Hora_Ocurrencia, " +
                           "       A.Detalle_Reporte, A.Calle, A.Num_Exterior, A.Num_Interior, A.Estado, A.Delegacion, A.Colonia, A.Codigo_Postal, " +
                           "       CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END as Seguro_Cia, " +
                           "       t0.IdRegimen, t0.IdEstStatus, t0.IdConclusion, " +
                           "       B.Nom_Lesionado, B.Fec_Nacimiento, B.Sexo, B.Email_Lesionado, B.Tel_Celular, B.Edad, B.RFC_Lesionado, B.Tpo_Evento, B.Desc_Lesiones, " +
                           "       B.Calle, B.Num_Exterior, B.Num_Interior, B.Estado, B.Delegacion, B.Colonia, B.Codigo_Postal, " +
                           "       C.Nom_Responsable, C.Parentesco, C.Edad, C.Tel_Responsable, C.RFC_Responsable, C.Email_Responsable, " +
                           "       C.Calle, C.Num_Exterior, C.Num_Interior, C.Estado, C.Delegacion, C.Colonia, C.Codigo_Postal, " +
                           "       D.IdLinea, D.IdEstacion, D.IdZona, D.IdHospital, D.TpoAtencion, D.Email_Atencion, D.Tel_Contacto_1, D.Tel_Contacto_2, " +
                           "       D.FechaIngreso, D.Hora_Ingreso, D.FechaRecepcion, D.Hora_Recepcion, D.FechaAlta, D.Hora_Alta, D.FechaEnvio, D.Hora_Envio, " +
                           "       D.Calle, D.Num_Exterior, D.Num_Interior, D.Estado, D.Delegacion, D.Colonia, D.Codigo_Postal, " +
                           "       D.FechaVigencia, D.MontoAutorizado, " +
                           "       D.Diagnostico, D.Observaciones, D.Ubicacion, D.PlanTratamiento, " +
                           "       F.Alergias, F.Enfermedades, F.Medicamentos, F.Alcohol, F.Sustancias, F.Observaciones," +
                           "       F.Diagnostico, F.Comentarios, " +
                           "       t0.IdEstatusCaso " +
                           "  FROM ITM_73 t0 " +
                           "  LEFT JOIN ITM_73_1 A ON t0.Referencia = A.Referencia AND t0.SubReferencia = A.SubReferencia " +
                           "  LEFT JOIN ITM_73_2 B ON t0.Referencia = B.Referencia AND t0.SubReferencia = B.SubReferencia " +
                           "  LEFT JOIN ITM_73_3 C ON t0.Referencia = C.Referencia AND t0.SubReferencia = C.SubReferencia " +
                           "  LEFT JOIN ITM_73_4 D ON t0.Referencia = D.Referencia AND t0.SubReferencia = D.SubReferencia " +
                           "  LEFT JOIN ITM_73_6 F ON t0.Referencia = F.Referencia AND t0.SubReferencia = F.SubReferencia " +
                           "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                           "  LEFT JOIN ITM_12 t3 ON t0.IdRespMedico = t3.IdRespMedico " +
                           "  LEFT JOIN ITM_14 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
                           "  LEFT JOIN ITM_78 t5 ON t0.IdProyecto = t5.IdProyecto " +
                           " WHERE t0.IdStatus IN (1) " +
                           "   AND t0.Referencia = '" + pReferencia + "'" +
                           "   AND t0.SubReferencia = '" + pSubReferencia + "'" +
                           " ORDER BY t0.IdAsunto DESC LIMIT 100";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                foreach (DataRow row in dt.Rows)
                {
                    // Informacion General
                    TxtSubReferencia.Text = Convert.ToString(row[2]);
                    TxtNumSiniestro.Text = Convert.ToString(row[3]);
                    // TxtNumPoliza.Text = Convert.ToString(row[4]);
                    TxtNumPoliza.Text = (row.IsNull(4) || string.IsNullOrWhiteSpace(row[4]?.ToString())) ? "INST0004" : row[4].ToString();
                    TxtNomAjustador.Text = Convert.ToString(row[5]);

                    TxtEmailAjustador.Text = Convert.ToString(row[6]);
                    TxtTelAjustador.Text = Convert.ToString(row[7]);

                    // TxtFechaIniVigencia.Text = Convert.ToString(row[8]);
                    TxtFechaIniVigencia.Text = (row.IsNull(8) || string.IsNullOrWhiteSpace(row[8]?.ToString())) ? "01/01/" + DateTime.Now.Year.ToString() : row[8].ToString();
                    // TxtFechaFinVigencia.Text = Convert.ToString(row[9]);
                    TxtFechaFinVigencia.Text = (row.IsNull(8) || string.IsNullOrWhiteSpace(row[9]?.ToString())) ? "31/12/" + DateTime.Now.Year.ToString() : row[9].ToString();
                    TxtFechaOcurrencia.Text = Convert.ToString(row[10]);

                    TxtHoraRecepcion.Text = string.IsNullOrEmpty(Convert.ToString(row[11])) ? "00:00" : Convert.ToString(row[11]);
                    TxtHoraOcurrencia.Text = string.IsNullOrEmpty(Convert.ToString(row[12])) ? "00:00" : Convert.ToString(row[12]);
                    TxtDetalleReporte.Text = Convert.ToString(row[13]);
                    //TxtCalle.Text = Convert.ToString(row[14]);
                    //TxtNumExterior.Text = Convert.ToString(row[15]);
                    //TxtNumInterior.Text = Convert.ToString(row[16]);
                    //ddlEstado.SelectedValue = Convert.ToString(row[17]);

                    //// Disparar el evento SelectedIndexChanged manualmente
                    //ddlEstado_SelectedIndexChanged(ddlEstado, EventArgs.Empty);

                    //ddlMunicipios.SelectedValue = Convert.ToString(row[18]);
                    //TxtColonia.Text = Convert.ToString(row[19]);
                    //TxtCodigoPostal.Text = Convert.ToString(row[20]);
                    TxtSeguro_Cia.Text = Convert.ToString(row[21]);
                    ddlTpoAsegurado.SelectedValue = Convert.ToString(row[22]);
                    ddlEstSiniestro.SelectedValue = Convert.ToString(row[23]);

                    // Disparar el evento SelectedIndexChanged manualmente
                    ddlEstSiniestro_SelectedIndexChanged(ddlEstSiniestro, EventArgs.Empty);
                    ddlConclusion.SelectedValue = Convert.ToString(row[24]);

                    TxtNomLesionado.Text = Convert.ToString(row[25]);
                    TxtFecNacimiento.Text = Convert.ToString(row[26]);
                    TxtSexo.Text = Convert.ToString(row[27]);
                    TxtEmailLesionado.Text = Convert.ToString(row[28]);
                    TxtTelLesionado.Text = Convert.ToString(row[29]);
                    TxtEdadLesionado.Text = Convert.ToString(row[30]);
                    TxtRFC_Lesionado.Text = Convert.ToString(row[31]);
                    ddlTipoEvento.SelectedValue = Convert.ToString(row[32]);
                    TxtDescLesiones.Text = Convert.ToString(row[33]);

                    TxtCalleLesionado.Text = Convert.ToString(row[34]);
                    TxtNumExtLesionado.Text = Convert.ToString(row[35]);
                    TxtNumIntLesionado.Text = Convert.ToString(row[36]);
                    ddlEstadoLesionado.SelectedValue = Convert.ToString(row[37]);

                    // Disparar el evento SelectedIndexChanged manualmente
                    ddlEstadoLesionado_SelectedIndexChanged(ddlEstadoLesionado, EventArgs.Empty);

                    ddlMunicipiosLesionado.SelectedValue = Convert.ToString(row[38]);
                    TxtColoniaLesionado.Text = Convert.ToString(row[39]);
                    TxtCPostalLesionado.Text = Convert.ToString(row[40]);

                    TxtNomResponsable.Text = Convert.ToString(row[41]);
                    TxtParentesco.Text = Convert.ToString(row[42]);
                    TxtEdadResponsable.Text = Convert.ToString(row[43]);
                    TxtTelResponsable.Text = Convert.ToString(row[44]);
                    TxtRFC_Responsable.Text = Convert.ToString(row[45]);
                    TxtEmailResponsable.Text = Convert.ToString(row[46]);

                    TxtCalleResponsable.Text = Convert.ToString(row[47]);
                    TxtNumExtResponsable.Text = Convert.ToString(row[48]);
                    TxtNumIntResponsable.Text = Convert.ToString(row[49]);
                    ddlEstadoResponsable.SelectedValue = Convert.ToString(row[50]);

                    // Disparar el evento SelectedIndexChanged manualmente
                    ddlEstadoResponsable_SelectedIndexChanged(ddlEstadoResponsable, EventArgs.Empty);

                    ddlMunicipiosResponsable.SelectedValue = Convert.ToString(row[51]);
                    TxtColoniaResponsable.Text = Convert.ToString(row[52]);
                    TxtCPostalResponsable.Text = Convert.ToString(row[53]);

                    // ddlLineaOcurrencia.SelectedValue = Convert.ToString(row[54]);
                    string valorLinea = row[54] != DBNull.Value ? Convert.ToInt32(row[54]).ToString() : "0";
                    if (ddlLineaOcurrencia.Items.FindByValue(valorLinea) != null)
                    {
                        ddlLineaOcurrencia.SelectedValue = valorLinea;

                        // Disparar el evento SelectedIndexChanged manualmente
                        ddlLineaOcurrencia_SelectedIndexChanged(ddlLineaOcurrencia, EventArgs.Empty);

                    }
                    else
                    {
                        // Opción: seleccionar un valor por defecto o dejarlo sin seleccionar
                        ddlLineaOcurrencia.SelectedIndex = -1;  // Ninguno seleccionado
                    }

                    ddlEstacionOcurrencia.SelectedValue = Convert.ToString(row[55]);

                    // ddlZonas.SelectedValue = Convert.ToString(row[56]);
                    string valorZonas = row[56] != DBNull.Value ? Convert.ToInt32(row[56]).ToString() : "0";
                    if (ddlZonas.Items.FindByValue(valorZonas) != null)
                    {
                        ddlZonas.SelectedValue = valorZonas;

                        // Disparar el evento SelectedIndexChanged manualmente
                        ddlZonas_SelectedIndexChanged(ddlZonas, EventArgs.Empty);

                    }
                    else
                    {
                        // Opción: seleccionar un valor por defecto o dejarlo sin seleccionar
                        ddlZonas.SelectedIndex = -1;    // Ninguno seleccionado
                    }

                    ddlHospitales.SelectedValue = Convert.ToString(row[57]);
                    ddlTpoAtencion.SelectedValue = Convert.ToString(row[58]);

                    TxtCorreoElectronico.Text = Convert.ToString(row[59]);
                    TxtTelAtencionContacto1.Text = Convert.ToString(row[60]);
                    TxtTelAtencionContacto2.Text = Convert.ToString(row[61]);

                    TxtFechaIngreso.Text = Convert.ToString(row[62]);
                    TxtHoraIngreso.Text = string.IsNullOrEmpty(Convert.ToString(row[63])) ? "00:00" : Convert.ToString(row[63]);
                    TxtFechaRecepcionNM.Text = Convert.ToString(row[64]);
                    TxtHoraRecepcionNM.Text = string.IsNullOrEmpty(Convert.ToString(row[65])) ? "00:00" : Convert.ToString(row[65]);

                    TxtFechaAlta.Text = Convert.ToString(row[66]);
                    TxtHoraAlta.Text = string.IsNullOrEmpty(Convert.ToString(row[67])) ? "00:00" : Convert.ToString(row[67]);
                    TxtFechaEnvio.Text = Convert.ToString(row[68]);
                    TxtHoraEnvio.Text = string.IsNullOrEmpty(Convert.ToString(row[69])) ? "00:00" : Convert.ToString(row[69]);

                    TxtCalleAtencion.Text = Convert.ToString(row[70]);
                    TxtNumExtAtencion.Text = Convert.ToString(row[71]);
                    TxtNumIntAtencion.Text = Convert.ToString(row[72]);

                    ddlEstadoAtencion.SelectedValue = Convert.ToString(row[73]);

                    // Disparar el evento SelectedIndexChanged manualmente
                    ddlEstadoAtencion_SelectedIndexChanged(ddlEstadoAtencion, EventArgs.Empty);

                    ddlMunicipiosAtencion.SelectedValue = Convert.ToString(row[74]);

                    TxtColoniaAtencion.Text = Convert.ToString(row[75]);
                    TxtCPostalAtencion.Text = Convert.ToString(row[76]);

                    TxtFechaVigencia.Text = Convert.ToString(row[77]);
                    TxtMontoAutorizado.Text = Convert.ToString(row[78]);
                    TxtMontoAutorizado.Text = string.IsNullOrEmpty(Convert.ToString(row[78])) ? "0.00" : Convert.ToString(row[78]);
                    TxtDiagnostico.Text = Convert.ToString(row[79]);
                    TxtObservaciones_DA.Text = Convert.ToString(row[80]);
                    TxtRef_Ubicacion.Text = Convert.ToString(row[81]);
                    TxtPlanTratamiento.Text = Convert.ToString(row[82]);

                    TxtAlergias.Text = Convert.ToString(row[83]);
                    TxtEnfermedades.Text = Convert.ToString(row[84]);
                    TxtMedicamentos.Text = Convert.ToString(row[85]);
                    TxtAlcohol.Text = Convert.ToString(row[86]);
                    TxtSustancias.Text = Convert.ToString(row[87]);
                    TxtObservaciones_PA.Text = Convert.ToString(row[88]);
                    TxtDiagnosticoPreliminar.Text = Convert.ToString(row[89]);
                    TxtComentariosMedicos.Text = Convert.ToString(row[90]);

                    ddlEstatusCaso.SelectedValue = Convert.ToString(row[91]);

                    return 0;
                }

                dbConn.Close();

                return 0;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }

            return -1;

        }


        protected void btnShowPanel1_Click(object sender, EventArgs e)
        {
            pnl1.Visible = !pnl1.Visible;   // Cambia la visibilidad del Panel 1 al contrario de su estado actual

            //if (pnl1.Visible)
            //{
            //    string flechaHaciaArriba = "\u25B2";
            //    btnShowPanel1.Text = flechaHaciaArriba; // Flecha hacia arriba
            //    pnl1.Visible = true;
            //}
            //else
            //{
            //    string flechaHaciaAbajo = "\u25BC";
            //    btnShowPanel1.Text = flechaHaciaAbajo; // Flecha hacia abajo
            //    pnl1.Visible = false;
            //}
        }

        protected void btnShowPanel2_Click(object sender, EventArgs e)
        {
            pnl2.Visible = !pnl2.Visible;   // Cambia la visibilidad del Panel 2 al contrario de su estado actual

            if (pnl2.Visible)
            {
                // string flechaHaciaArriba = "\u25B2";
                // btnShowPanel2.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl2.Visible = true;
            }
            else
            {
                // string flechaHaciaAbajo = "\u25BC";
                // btnShowPanel2.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl2.Visible = false;
            }
        }

        protected void btnShowPanel3_Click(object sender, EventArgs e)
        {
            pnl3.Visible = !pnl3.Visible;   // Cambia la visibilidad del Panel 3 al contrario de su estado actual

            if (pnl3.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel3.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl3.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel3.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl3.Visible = false;
            }
        }

        protected void btnShowPanel4_Click(object sender, EventArgs e)
        {
            pnl4.Visible = !pnl4.Visible;   // Cambia la visibilidad del Panel 4 al contrario de su estado actual

            if (pnl4.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel4.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl4.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel4.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl4.Visible = false;
            }
        }

        protected void btnShowPanel5_Click(object sender, EventArgs e)
        {
            //pnl5.Visible = !pnl5.Visible;   // Cambia la visibilidad del Panel 5 al contrario de su estado actual

            //if (pnl5.Visible)
            //{
            //    string flechaHaciaArriba = "\u25B2";
            //    btnShowPanel5.Text = flechaHaciaArriba; // Flecha hacia arriba
            //    pnl5.Visible = true;
            //}
            //else
            //{
            //    string flechaHaciaAbajo = "\u25BC";
            //    btnShowPanel5.Text = flechaHaciaAbajo; // Flecha hacia abajo
            //    pnl5.Visible = false;
            //}
        }


        protected void btnShowPanel6_Click(object sender, EventArgs e)
        {
            pnl6.Visible = !pnl6.Visible;   // Cambia la visibilidad del Panel 6 al contrario de su estado actual

            if (pnl6.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel6.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl6.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel6.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl6.Visible = false;
            }
        }

        protected void btnShowPanel7_Click(object sender, EventArgs e)
        {
            pnl7.Visible = !pnl7.Visible;   // Cambia la visibilidad del Panel 7 al contrario de su estado actual

            if (pnl7.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel7.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl7.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel7.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl7.Visible = false;
            }
        }

        protected void btnShowPanel8_Click(object sender, EventArgs e)
        {
            pnl8.Visible = !pnl8.Visible;   // Cambia la visibilidad del Panel 8 al contrario de su estado actual

            if (pnl8.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel8.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl8.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel8.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl8.Visible = false;
            }
        }

        protected void btnShowPanel9_Click(object sender, EventArgs e)
        {
            pnl9.Visible = !pnl9.Visible;   // Cambia la visibilidad del Panel 9 al contrario de su estado actual

            if (pnl9.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel9.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl9.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel9.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl9.Visible = false;
            }
        }

        protected void ddlTpoAsegurado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlEstSiniestro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEstSiniestro.SelectedValue != "0")
            {
                GetConclusion();
            }
        }

        protected void ddlConclusion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ImgDel_Documento_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void ddlLineaOcurrencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iLinea = Convert.ToInt16(ddlLineaOcurrencia.SelectedValue);
            GetEstaciones(iLinea);
        }

        protected void ddlEstacionOcurrencia_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlHospitales_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlZonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iZona = Convert.ToInt16(ddlZonas.SelectedValue);
            GetHospitales(iZona);
        }

        public void inhabilitar(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Enabled = false;
                else if (control is DropDownList)
                    ((DropDownList)control).Enabled = false;
                else if (control is RadioButtonList)
                    ((RadioButtonList)control).Enabled = false;
                else if (control is CheckBoxList)
                    ((CheckBoxList)control).Enabled = false;
                else if (control is RadioButton)
                    ((RadioButton)control).Enabled = false;
                //else if (control is CheckBox)
                //    ((CheckBox)control).Enabled = false;
                else if (control.HasControls())
                    //Esta linea detécta un Control que contenga otros Controles
                    //Así ningún control se quedará sin ser limpiado.
                    inhabilitar(control.Controls);
            }
        }

        protected void BtnGraba_Categorias_Click(object sender, EventArgs e)
        {
            if (ddlTpoAsegurado.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Tipo de Asegurado";
                mpeMensaje.Show();
                return;
            }
            else if (ddlEstSiniestro.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Estatus del Siniestro";
                mpeMensaje.Show();
                return;
            }
            else if (ddlConclusion.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Estatus Etapas";
                mpeMensaje.Show();
                return;
            }

            try
            {
                // Eliminar registros
                Delete_ITM_95();
                // Actualizar campo IdRegimen, IdConclusion, DomSiniestro 
                Update_ITM_73();
                ////// Insertar en ITM_95 (Tipo de Asegurado / Tpo de Estatus)
                Insert_ITM_95();

                // Agregar registros con los datos de cada cuaderno a cargar
                string sReferencia = Variables.wRef;
                int iSubReferencia = Variables.wSubRef;
                string IdAseguradora = Variables.wPrefijo_Aseguradora;
                int IdConclusion = Convert.ToInt32(ddlConclusion.SelectedValue);
                int IdRegimen = Convert.ToInt32(ddlTpoAsegurado.SelectedValue); ;

                if (Variables.wExiste == false)
                {
                    ////Add_tbDetalleCuadernos(sReferencia, iSubReferencia, IdAseguradora, IdConclusion, IdRegimen);
                }

                //Obtener_Valores_ChBox(grdSeccion_2, "ChBoxSeccion_2", 2, "ITM_22");
                //Obtener_Valores_ChBox(grdSeccion_5, "ChBoxSeccion_5", 5, "ITM_24");

                ////GetDocumentos(TxtSubReferencia.Text);

                LblMessage.Text = "Se han aplicado los cambios correctamente";
                this.mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }


        protected void Update_ITM_73()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string DomSiniestro = string.Empty;

                // Actualizar registro(s) tablas (ITM_03, ITM_46, ITM_47, ITM_70)
                //string strQuery = "UPDATE ITM_03 " +
                //                  "   SET IdRegimen = " + ddlTpoAsegurado.SelectedValue + "," +
                //                  "       IdConclusion = " + ddlConclusion.SelectedValue + " " +
                //                  " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END) = '" + TxtSubReferencia.Text + "' ; ";

                //strQuery += Environment.NewLine;

                //strQuery += "UPDATE ITM_46 " +
                //            "   SET IdConclusion = " + ddlConclusion.SelectedValue + " " +
                //            " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END) = '" + TxtSubReferencia.Text + "' ; ";

                //strQuery += Environment.NewLine;

                //strQuery += "UPDATE ITM_47 " +
                //            "   SET IdConclusion = " + ddlConclusion.SelectedValue + " " +
                //            " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END) = '" + TxtSubReferencia.Text + "' ; ";

                //strQuery += Environment.NewLine;

                string strQuery = "UPDATE ITM_73 " +
                                  "   SET IdRegimen = " + ddlTpoAsegurado.SelectedValue + ", " +
                                  "       IdConclusion = " + ddlConclusion.SelectedValue + ", " +
                                  "       IdEstStatus = " + ddlEstSiniestro.SelectedValue + ", " +
                                  "       DomSiniestro = '" + DomSiniestro + "' " +
                                  " WHERE Referencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = " + Variables.wSubRef + " ";

                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Delete_ITM_95()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_95)
                string strQuery = "DELETE FROM ITM_95 " +
                                  " WHERE Referencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = " + Variables.wSubRef + " " +
                                  "   AND IdSeccion NOT IN (3) ";

                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Insert_ITM_95()
        {
            try
            {

                int IdProyecto = Variables.wIdProyecto;
                string IdCliente = Variables.wPrefijo_Aseguradora;

                int IdTpoAsunto = Variables.wIdTpoAsunto;
                string IdUsuario = Variables.wUserLogon;

                int IdDocumento = 1;
                string strQuery = string.Empty;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                if (ddlTpoAsegurado.SelectedValue != "0")
                {

                    // Tipo de Asegurado
                    strQuery = "INSERT INTO ITM_95 (Referencia, SubReferencia, IdProyecto, IdCliente, IdTpoAsunto, IdSeccion, IdCategoria, IdDocumento, bSeleccion, IdUsuario) " +
                               "VALUES ('" + Variables.wRef + "', " + Variables.wSubRef + ", " + IdProyecto + ", '" + IdCliente + "', " + IdTpoAsunto + ", 1, " +
                               " " + ddlTpoAsegurado.SelectedValue + ", " + IdDocumento + ", 1, '" + IdUsuario + "'); " + "\n \n";

                    strQuery += Environment.NewLine;

                }

                if (ddlConclusion.SelectedValue != "0")
                {
                    Variables.wExiste = false;

                    int IdSeccion = 3;
                    int IdCategoria = Convert.ToInt32(ddlConclusion.SelectedValue);

                    // Validar si el Tpo. de Estatus existe no se inserta
                    int iExiste = Validar_Existe_Categoria(IdSeccion, IdCategoria);

                    if (iExiste == 0)   // No Existe
                    {
                        // Estatus
                        strQuery += "INSERT INTO ITM_95 (Referencia, SubReferencia, IdProyecto, IdCliente, IdTpoAsunto, IdSeccion, IdCategoria, IdDocumento, bSeleccion, IdUsuario) " +
                                    "VALUES ('" + Variables.wRef + "', " + Variables.wSubRef + ", " + IdProyecto + ", '" + IdCliente + "', " + IdTpoAsunto + ", 3, " +
                                    " " + ddlConclusion.SelectedValue + ", " + IdDocumento + ", 1, '" + IdUsuario + "'); " + "\n \n";
                    }
                    else
                    {
                        Variables.wExiste = true;
                    }

                }

                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

        }

        public int Validar_Existe_Categoria(int IdSeccion, int IdCategoria)
        {

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT Referencia, SubReferencia, IdSeccion, IdCategoria " +
                              "  FROM ITM_95 t0" +
                              " WHERE t0.Referencia = '" + Variables.wRef + "'" +
                              "   AND t0.SubReferencia = '" + Variables.wSubRef + "' " +
                              "   AND IdSeccion =  " + IdSeccion + " " +
                              "   AND IdCategoria = " + IdCategoria + " ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            dbConn.Close();

            if (dt.Rows.Count > 0)
            {
                return 1;
            }

            return 0;

        }

        protected void BtnCrear_Cuaderno_Click(object sender, EventArgs e)
        {
            // *
        }

        protected void Obtener_Valores_ChBox(GridView gridView, string checkBoxID, int IdSeccion, string sTabla)
        {
            //int IdProyecto = Variables.wIdProyecto;

            //string IdCliente = (string)Session["IdCliente"];
            //string IdTpoAsunto = (string)Session["IdTpoAsunto"];

            string Referencia = Variables.wRef;
            int SubReferencia = Variables.wSubRef;

            int IdProyecto = Variables.wIdProyecto;
            string IdTpoAsunto = Convert.ToString(Variables.wIdTpoAsunto);

            string IdCliente = Variables.wPrefijo_Aseguradora;
            string IdUsuario = Variables.wUserLogon;

            //// bool[] valoresDocs = new bool[18];      // Creamos un array de booleanos para almacenar los valores de los documentos
            // string[] valoresDocs = new string[18];

            // // Inicializar el array con cadenas vacías
            // for (int j = 0; j < valoresDocs.Length; j++)
            // {
            //     valoresDocs[j] = string.Empty;
            // }

            // Declarar una lista para almacenar los textos de los labels asociados a los CheckBoxes seleccionados
            List<string> selectedLabels = new List<string>();

            // Iterar sobre las filas de la GridView
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                // Obtener los CheckBoxes de cada fila
                GridViewRow row = gridView.Rows[i];

                CheckBox chkBox1 = (CheckBox)row.FindControl(checkBoxID + "_1");
                CheckBox chkBox2 = (CheckBox)row.FindControl(checkBoxID + "_2");
                CheckBox chkBox3 = (CheckBox)row.FindControl(checkBoxID + "_3");

                // Recuperar el texto de las celdas asociadas a cada CheckBox y reemplazar &nbsp; con una cadena vacía
                string labelText1 = row.Cells[0].Text.Trim() == "&nbsp;" ? "" : Server.HtmlDecode(row.Cells[0].Text.Trim());  // Asumiendo que el texto está en la primera celda (Columna1)
                string labelText2 = row.Cells[2].Text.Trim() == "&nbsp;" ? "" : Server.HtmlDecode(row.Cells[2].Text.Trim());  // Asumiendo que el texto está en la tercera celda (Columna2)
                string labelText3 = row.Cells[4].Text.Trim() == "&nbsp;" ? "" : Server.HtmlDecode(row.Cells[4].Text.Trim());  // Asumiendo que el texto está en la quinta celda (Columna3)

                // Almacenar los valores de los CheckBoxes en las columnas de la base de datos correspondientes solo si están seleccionados
                if (chkBox1 != null && chkBox1.Checked)
                {
                    selectedLabels.Add(labelText1);
                }
                if (chkBox2 != null && chkBox2.Checked)
                {
                    selectedLabels.Add(labelText2);
                }
                if (chkBox3 != null && chkBox3.Checked)
                {
                    selectedLabels.Add(labelText3);
                }

                //// Determinar las columnas correspondientes en la base de datos
                //int columnaBaseDatos = i / 6;   // Cada conjunto de 6 filas corresponde a una columna en la base de datos
                //int offset = i % 6;             // Determina el desplazamiento dentro de cada conjunto de 6 filas

                //// Almacenar los valores de los CheckBoxes en las columnas de la base de datos correspondientes
                //valoresDocs[columnaBaseDatos * 6 + offset] = row.Cells[0].Text.Trim() == "&nbsp;" ? "" : row.Cells[0].Text.Trim();            // chkBox1.Checked;
                //valoresDocs[columnaBaseDatos * 6 + offset + 6] = row.Cells[2].Text.Trim() == "&nbsp;" ? "" : row.Cells[2].Text.Trim();        // chkBox2.Checked;
                //valoresDocs[columnaBaseDatos * 6 + offset + 12] = row.Cells[4].Text.Trim() == "&nbsp;" ? "" : row.Cells[4].Text.Trim();       // chkBox3.Checked;

            }

            ActualizarDatosEnTabla(Referencia, SubReferencia, IdProyecto, IdCliente, IdSeccion, IdTpoAsunto, IdUsuario, sTabla, selectedLabels);
        }

        // Método para actualizar los datos en la tabla ITM_86
        private void ActualizarDatosEnTabla(string Referencia, int SubReferencia, int idProyecto, string idCliente, int idSeccion, string idTpoAsunto, string IdUsuario, string sTabla, List<string> selectedLabels)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Obtener el IdDocumento de ITM_82, ITM_84, ITM_85  para cada label
                Dictionary<string, int> idDocumentos = new Dictionary<string, int>();

                foreach (var label in selectedLabels)
                {
                    int idDocumento = GetIdDocumento(label, sTabla);
                    idDocumentos[label] = idDocumento;
                }

                // Implementa la lógica de la función aquí
                foreach (var kvp in idDocumentos)
                {
                    string label = kvp.Key;
                    int idDocumento = kvp.Value;

                    // Crear el comando SQL para la inserción
                    string strQuery = "INSERT INTO ITM_95 (Referencia, SubReferencia, IdCliente, IdProyecto, IdTpoAsunto, IdSeccion, " +
                                      "                    IdCategoria, IdDocumento, bSeleccion, IdUsuario )" +
                                      " VALUES (@Referencia, @SubReferencia, @idCliente, @idProyecto, @idTpoAsunto, @idSeccion, @IdCategoria, 1, 1, @IdUsuario) ";

                    //using (SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD))
                    int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                    {
                        // Agregar el parámetro y su valor
                        // Agregar los parámetros y sus valores
                        cmd.Parameters.AddWithValue("@Referencia", Referencia);
                        cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                        cmd.Parameters.AddWithValue("@idCliente", idCliente);
                        cmd.Parameters.AddWithValue("@idProyecto", idProyecto);
                        cmd.Parameters.AddWithValue("@idTpoAsunto", idTpoAsunto);
                        cmd.Parameters.AddWithValue("@idSeccion", idSeccion);
                        cmd.Parameters.AddWithValue("@IdCategoria", idDocumento);
                        cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);

                        // Ejecutar el comando
                        // cmd.ExecuteNonQuery();
                    });
                }

                dbConn.Close();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public int GetIdDocumento(string label, string Tabla)
        {
            int idDocumento = 0;

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Crear la consulta SQL para recuperar IdDocumento de ITM_82
                string query = $"SELECT IdDocumento FROM { Tabla } WHERE Descripcion = @Label";

                int rowsAffected = dbConn.ExecuteNonQueryWithParameters(query, cmd =>
                {
                    cmd.Parameters.AddWithValue("@Label", label);

                    // Ejecutar la consulta y obtener el resultado
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        idDocumento = Convert.ToInt32(result);
                    }

                });

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

            return idDocumento;
        }

        public void GetDiagnosticoFinal(string sValor)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t2.Id_ICD, Referencia, SubReferencia, t2.Cve_ICD, t1.Desc_ICD FROM ITM_31 AS t1, ITM_32 AS t2 " +
                                  " WHERE (CASE WHEN t2.SubReferencia >= 1 THEN CONCAT(t2.Referencia, '-', t2.SubReferencia) ELSE t2.Referencia END) = '" + sValor + "' " +
                                  "   AND t1.Cve_ICD = t2.Cve_ICD ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdDiagnosticoFinal.ShowHeaderWhenEmpty = true;
                    GrdDiagnosticoFinal.EmptyDataText = "No hay resultados.";
                }

                GrdDiagnosticoFinal.DataSource = dt;
                GrdDiagnosticoFinal.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdDiagnosticoFinal.HeaderRow.TableSection = TableRowSection.TableHeader;

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        
        public void GetTratamientosRealizados(string sValor)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t2.Id_CPT, Referencia, SubReferencia, t2.Cve_CPT, t1.Desc_CPT FROM ITM_30 AS t1, ITM_33 AS t2 " +
                                    " WHERE (CASE WHEN t2.SubReferencia >= 1 THEN CONCAT(t2.Referencia, '-', t2.SubReferencia) ELSE t2.Referencia END) = '" + sValor + "' " +
                                    "   AND t1.Cve_CPT = t2.Cve_CPT ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdTratamientos.ShowHeaderWhenEmpty = true;
                    GrdTratamientos.EmptyDataText = "No hay resultados.";
                }

                GrdTratamientos.DataSource = dt;
                GrdTratamientos.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdTratamientos.HeaderRow.TableSection = TableRowSection.TableHeader;

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }


        public void GetDocumentos(string sValor)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT * FROM (" +
                                  "SELECT t1.IdDoc_Categoria, t1.NomArchivo AS Descripcion, t3.TpoArchivo, t3.Fec_Entrega, " +
                                  "       (SELECT DocInterno FROM ITM_90 t4 WHERE t4.IdConsecutivo = t1.IdDocumento) AS DocInterno, " +
                                  "       COALESCE(t3.IdDescarga, 0) AS IdDescarga, " +
                                  "       t1.IdSeccion AS IdSeccion, t1.IdCategoria AS IdCategoria" +
                                  "  FROM ITM_88 AS t1 " +
                                  "  JOIN ITM_95 AS t2 ON t1.IdProyecto = t2.IdProyecto AND t1.IdSeccion = t2.IdSeccion " +
                                  "   AND t1.IdCliente = t2.IdCliente AND t1.IdCategoria = t2.IdCategoria " +
                                  "  LEFT JOIN ITM_47 AS t3 " +
                                  "    ON (CASE WHEN t3.SubReferencia >= 1 THEN CONCAT(t3.UsReferencia, '-', t3.SubReferencia) ELSE t3.UsReferencia END) = '" + sValor + "' " +
                                  "   AND t1.IdCliente = t3.IdAseguradora AND t1.IdTpoAsunto = t3.IdTpoAsunto " +
                                  "   AND t1.IdSeccion = t3.IdSeccion  AND t1.IdDocumento = t3.IdDocumento " +
                                  " WHERE (CASE WHEN t2.SubReferencia >= 1 THEN CONCAT(t2.Referencia, '-', t2.SubReferencia) ELSE t2.Referencia END) = '" + sValor + "' " +
                                  "   AND t1.IdProyecto = " + Variables.wIdProyecto + " " +
                                  "   AND t1.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "UNION ALL " +
                                  "SELECT t1.IdDocumento, t2.Descripcion, t1.TpoArchivo, t1.Fec_Entrega, t2.DocInterno, " +
                                  "COALESCE(t1.IdDescarga, 0) AS IdDescarga, t1.IdSeccion AS IdSeccion, t1.IdCategoria AS IdCategoria " +
                                  "  FROM ITM_47 AS t1 JOIN ITM_46 AS t2 " +
                                  "    ON (CASE WHEN t1.SubReferencia >= 1 THEN CONCAT(t1.UsReferencia, '-', t1.SubReferencia) ELSE t1.UsReferencia END) = '" + sValor + "' " +
                                  "   AND (CASE WHEN t2.SubReferencia >= 1 THEN CONCAT(t2.UsReferencia, '-', t2.SubReferencia) ELSE t2.UsReferencia END) = '" + sValor + "' " +
                                  "   AND t1.IdDocumento = t2.IdDocumento " +
                                  "  ) AS OrderedPart " +
                                  " ORDER BY IdSeccion, IdCategoria ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdDocumentos.ShowHeaderWhenEmpty = true;
                    GrdDocumentos.EmptyDataText = "No hay resultados.";
                }

                GrdDocumentos.DataSource = dt;
                GrdDocumentos.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdDocumentos.HeaderRow.TableSection = TableRowSection.TableHeader;

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void Add_ITM_32()
        {
            try
            {
                foreach (GridViewRow row in GrdICD.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        var chkbox = row.FindControl("ChBoxRow") as CheckBox;

                        if (chkbox.Checked)
                        {
                            // string IdDocumento = Server.HtmlDecode(Convert.ToString(row.Cells[1].Text));
                            // string Descripcion = Server.HtmlDecode(Convert.ToString(row.Cells[2].Text));

                            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                            dbConn.Open();

                            int Id_ICD = GetId_ICD_Max();

                            string sReferencia = Variables.wRef;
                            int sSubReferencia = Variables.wSubRef;
                            string Cve_ICD = Server.HtmlDecode(Convert.ToString(row.Cells[1].Text));
                            string Id_Usuario = Variables.wUserLogon;

                            string strQuery = "INSERT INTO ITM_32 (Id_ICD, Referencia, SubReferencia, Cve_ICD, Id_Usuario, IdStatus) " +
                                              "VALUES (" + Id_ICD + ", '" + sReferencia + "', " + sSubReferencia + ", '" + Cve_ICD + "', '" + Id_Usuario + "', 1)" + "\n \n";

                            int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                            dbConn.Close();
                        }
                    }
                }

                // GetDocumentos(IdProyecto, IdCategoria, IdSeccion);

                GetDiagnosticoFinal(TxtSubReferencia.Text);

                // inicializar control
                TxtBuscarICD.Text = string.Empty;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public int GetId_ICD_Max()
        {

            int Id_ICD_Max = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX( Id_ICD ), 0) + 1 Id_ICD " +
                              "  FROM ITM_32 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            foreach (DataRow row in dt.Rows)
            {
                Id_ICD_Max = Convert.ToInt32(row["Id_ICD"].ToString().Trim());
            }

            dbConn.Close();

            return Id_ICD_Max;

        }

        protected void Eliminar_ITM_32()
        {
            try
            {
                int index = Variables.wRenglon;

                string sReferencia = Variables.wRef;
                int iSubReferencia = Variables.wSubRef;

                int Id_ICD = Convert.ToInt32(GrdDiagnosticoFinal.Rows[index].Cells[0].Text);
                string Cve_ICD = Convert.ToString(GrdDiagnosticoFinal.Rows[index].Cells[1].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_32)
                string strQuery = "DELETE FROM ITM_32 " +
                                  " WHERE Id_ICD = " + Id_ICD + " " +
                                  "   AND Referencia = '" + sReferencia + "' " +
                                  "   AND SubReferencia = " + iSubReferencia + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se eliminó correctamente la clave ICD";
                mpeMensaje.Show();

                // GetDocumentos(IdProyecto, IdCategoria, IdSeccion);
                GetDiagnosticoFinal(TxtSubReferencia.Text);
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Add_ITM_33()
        {
            try
            {
                foreach (GridViewRow row in GrdCPT.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        var chkbox = row.FindControl("ChBoxRow") as CheckBox;

                        if (chkbox.Checked)
                        {
                            // string IdDocumento = Server.HtmlDecode(Convert.ToString(row.Cells[1].Text));
                            // string Descripcion = Server.HtmlDecode(Convert.ToString(row.Cells[2].Text));

                            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                            dbConn.Open();

                            int Id_CPT = GetId_CPT_Max();

                            string sReferencia = Variables.wRef;
                            int sSubReferencia = Variables.wSubRef;
                            string Cve_CPT = Server.HtmlDecode(Convert.ToString(row.Cells[1].Text));
                            string Id_Usuario = Variables.wUserLogon;

                            string strQuery = "INSERT INTO ITM_33 (Id_CPT, Referencia, SubReferencia, Cve_CPT, Id_Usuario, IdStatus) " +
                                              "VALUES (" + Id_CPT + ", '" + sReferencia + "', " + sSubReferencia + ", '" + Cve_CPT + "', '" + Id_Usuario + "', 1)" + "\n \n";

                            int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                            dbConn.Close();
                        }
                    }
                }

                GetTratamientosRealizados(TxtSubReferencia.Text);

                // inicializar control
                TxtBuscarCPT.Text = string.Empty;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public int GetId_CPT_Max()
        {

            int Id_CPT_Max = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX( Id_CPT ), 0) + 1 Id_CPT " +
                              "  FROM ITM_33 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            foreach (DataRow row in dt.Rows)
            {
                Id_CPT_Max = Convert.ToInt32(row["Id_CPT"].ToString().Trim());
            }

            dbConn.Close();

            return Id_CPT_Max;

        }

        protected void Eliminar_ITM_33()
        {
            try
            {
                int index = Variables.wRenglon;

                string sReferencia = Variables.wRef;
                int iSubReferencia = Variables.wSubRef;

                int Id_CPT = Convert.ToInt32(GrdTratamientos.Rows[index].Cells[0].Text);
                string Cve_CPT = Convert.ToString(GrdTratamientos.Rows[index].Cells[1].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_33)
                string strQuery = "DELETE FROM ITM_33 " +
                                  " WHERE Id_CPT = " + Id_CPT + " " +
                                  "   AND Referencia = '" + sReferencia + "' " +
                                  "   AND SubReferencia = " + iSubReferencia + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se eliminó correctamente la clave CPT";
                mpeMensaje.Show();

                // GetDocumentos(IdProyecto, IdCategoria, IdSeccion);
                GetTratamientosRealizados(TxtSubReferencia.Text);
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Datos_Seleccionados_Proveedor()
        {
            try
            {
                foreach (GridViewRow row in GrdDatosProveedor.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        var chkbox = row.FindControl("ChBoxRow") as CheckBox;

                        if (chkbox.Checked)
                        {
                            TxtNomEmpresa.Text = Server.HtmlDecode(Convert.ToString(row.Cells[2].Text));
                            TxtEmailEmpresa.Text = Server.HtmlDecode(Convert.ToString(row.Cells[3].Text));

                            TxtCalleProveedor.Text = Server.HtmlDecode(Convert.ToString(row.Cells[4].Text));
                            TxtNumExtProveedor.Text = Server.HtmlDecode(Convert.ToString(row.Cells[5].Text));
                            TxtNumIntProveedor.Text = Server.HtmlDecode(Convert.ToString(row.Cells[6].Text));
                            ddlEstadoProveedor.SelectedValue = Server.HtmlDecode(Convert.ToString(row.Cells[7].Text));

                            // Disparar el evento SelectedIndexChanged manualmente
                            ddlEstadoProveedor_SelectedIndexChanged(ddlEstadoProveedor, EventArgs.Empty);

                            ddlMunicipioProveedor.SelectedValue = Server.HtmlDecode(Convert.ToString(row.Cells[8].Text));
                            TxtColoniaProveedor.Text = Server.HtmlDecode(Convert.ToString(row.Cells[9].Text));
                            TxtCPostalProveedor.Text = Server.HtmlDecode(Convert.ToString(row.Cells[10].Text));

                            TxtTelContacto1.Text = Server.HtmlDecode(Convert.ToString(row.Cells[11].Text));
                            TxtTelContacto2.Text = Server.HtmlDecode(Convert.ToString(row.Cells[12].Text));

                        }
                    }
                }

                // GetDocumentos(IdProyecto, IdCategoria, IdSeccion);

                GetDiagnosticoFinal(TxtSubReferencia.Text);

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void btnShowPanel0_Click(object sender, EventArgs e)
        {
            pnl0.Visible = !pnl0.Visible;   // Cambia la visibilidad del Panel 0 al contrario de su estado actual

            if (pnl0.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel0.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl0.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel0.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl0.Visible = false;
            }
        }

        protected void ddlTipoEvento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string sEstado = ddlEstado.SelectedValue;
            //GetMunicipios(sEstado);
        }

        protected void ddlMunicipios_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void ddlEstadoLesionado_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sEstado = ddlEstadoLesionado.SelectedValue;
            GetMunicipiosLesionado(sEstado);
        }

        protected void ddlMunicipiosLesionado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlEstadoResponsable_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sEstado = ddlEstadoResponsable.SelectedValue;
            GetMunicipiosResponsable(sEstado);
        }

        protected void ddlMunicipiosResponsable_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlEstadoAtencion_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sEstado = ddlEstadoAtencion.SelectedValue;
            GetMunicipios_Atencion(sEstado);
        }

        protected void ddlMunicipiosAtencion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnShowPanel13_Click(object sender, EventArgs e)
        {
            //pnl13.Visible = !pnl13.Visible;   // Cambia la visibilidad del Panel 13 al contrario de su estado actual

            //if (pnl13.Visible)
            //{
            //    string flechaHaciaArriba = "\u25B2";
            //    btnShowPanel13.Text = flechaHaciaArriba; // Flecha hacia arriba
            //    pnl13.Visible = true;
            //}
            //else
            //{
            //    string flechaHaciaAbajo = "\u25BC";
            //    btnShowPanel13.Text = flechaHaciaAbajo; // Flecha hacia abajo
            //    pnl13.Visible = false;
            //}
        }

        protected void btnShowPanel14_Click(object sender, EventArgs e)
        {
            pnl14.Visible = !pnl14.Visible;   // Cambia la visibilidad del Panel 14 al contrario de su estado actual

            if (pnl14.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel14.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl14.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel14.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl14.Visible = false;
            }
        }

        protected void btnShowPanel15_Click(object sender, EventArgs e)
        {
            pnl15.Visible = !pnl15.Visible;   // Cambia la visibilidad del Panel 15 al contrario de su estado actual

            if (pnl15.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel15.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl15.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel15.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl15.Visible = false;
            }
        }

        protected void BtnAnularPnl3_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            inhabilitar(this.Controls);
            habilitar_controles();

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            btnEditarPnl3.Visible = true;
            btnActualizarPnl3.Visible = false;
            BtnAnularPnl3.Visible = false;
        }

        protected void btnEditarPnl3_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl4
            TxtNomLesionado.Enabled = true;
            TxtFecNacimiento.Enabled = true;
            TxtSexo.Enabled = true;
            TxtEmailLesionado.Enabled = true;
            TxtTelLesionado.Enabled = true;
            TxtEdadLesionado.Enabled = true;
            TxtRFC_Lesionado.Enabled = true;
            ddlTipoEvento.Enabled = true;
            TxtDescLesiones.Enabled = true;

            TxtCalleLesionado.Enabled = true;
            TxtNumExtLesionado.Enabled = true;
            TxtNumIntLesionado.Enabled = true;
            ddlEstadoLesionado.Enabled = true;
            ddlMunicipiosLesionado.Enabled = true;
            TxtColoniaLesionado.Enabled = true;
            TxtCPostalLesionado.Enabled = true;

            btnEditarPnl3.Visible = false;
            btnActualizarPnl3.Visible = true;
            BtnAnularPnl3.Visible = true;
        }

        protected void btnActualizarPnl3_Click(object sender, EventArgs e)
        {
            Insertar_ITM_73_2();

            inhabilitar(this.Controls);
            habilitar_controles();

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            btnEditarPnl3.Visible = true;
            btnActualizarPnl3.Visible = false;
            BtnAnularPnl3.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios correctamente";
            this.mpeMensaje.Show();
        }

        protected void btnActualizarPnl4_Click(object sender, EventArgs e)
        {
            Insertar_ITM_73_3();

            inhabilitar(this.Controls);
            habilitar_controles();

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            btnEditarPnl4.Visible = true;
            btnActualizarPnl4.Visible = false;
            BtnAnularPnl4.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios correctamente";
            this.mpeMensaje.Show();
        }

        protected void btnEditarPnl4_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl4
            TxtNomResponsable.Enabled = true;
            TxtParentesco.Enabled = true;
            TxtEdadResponsable.Enabled = true;
            TxtTelResponsable.Enabled = true;
            TxtRFC_Responsable.Enabled = true;
            TxtEmailResponsable.Enabled = true;

            TxtCalleResponsable.Enabled = true;
            TxtNumExtResponsable.Enabled = true;
            TxtNumIntResponsable.Enabled = true;
            ddlEstadoResponsable.Enabled = true;
            ddlMunicipiosResponsable.Enabled = true;
            TxtColoniaResponsable.Enabled = true;
            TxtCPostalResponsable.Enabled = true;

            btnEditarPnl4.Visible = false;
            btnActualizarPnl4.Visible = true;
            BtnAnularPnl4.Visible = true;
        }

        protected void BtnAnularPnl4_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            inhabilitar(this.Controls);
            habilitar_controles();

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            btnEditarPnl4.Visible = true;
            btnActualizarPnl4.Visible = false;
            BtnAnularPnl4.Visible = false;
        }


        protected void BtnAnularPnl6_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            inhabilitar(this.Controls);
            habilitar_controles();

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            btnEditarPnl6.Visible = true;
            btnActualizarPnl6.Visible = false;
            BtnAnularPnl6.Visible = false;
        }

        protected void btnActualizarPnl6_Click(object sender, EventArgs e)
        {
            Insertar_ITM_73_4();

            inhabilitar(this.Controls);
            habilitar_controles();

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            btnEditarPnl6.Visible = true;
            btnActualizarPnl6.Visible = false;
            BtnAnularPnl6.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios correctamente";
            this.mpeMensaje.Show();
        }

        protected void btnEditarPnl6_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl6
            ddlLineaOcurrencia.Enabled = true;
            ddlEstacionOcurrencia.Enabled = true;
            TxtRef_Ubicacion.Enabled = true;

            btnEditarPnl6.Visible = false;
            btnActualizarPnl6.Visible = true;
            BtnAnularPnl6.Visible = true;
        }

        protected void btnAgregarPnl7_Click(object sender, EventArgs e)
        {
            if (TxtNomEmpresa.Text == "" || TxtNomEmpresa.Text == null)
            {
                LblMessage.Text = "Seleccione el nombre de la empresa";
                mpeMensaje.Show();
                return;
            }
            else if (TxtEmailEmpresa.Text == "" || TxtEmailEmpresa.Text == null)
            {
                LblMessage.Text = "Seleccione el correo electrónico";
                mpeMensaje.Show();
                return;
            }

            string valorTexto = TxtProvMontoAutorizado.Text.Trim();

            if (string.IsNullOrEmpty(valorTexto))
            {
                // Opcional: si quieres validar que no esté vacío
                LblMessage.Text = "Ingrese un monto autorizado válido";
                mpeMensaje.Show();

                return; // O muestra mensaje si quieres
            }

            if (!decimal.TryParse(valorTexto, out decimal monto))
            {
                // Opcional: si quieres validar que sea número válido
                return; // O muestra mensaje si quieres
            }

            // Contar decimales en el texto original
            int decimales = 0;
            int indicePunto = valorTexto.IndexOf('.');

            if (indicePunto >= 0)
            {
                decimales = valorTexto.Length - indicePunto - 1;
            }

            if (decimales > 2)
            {
                LblMessage.Text = "monto autorizado, solo se permiten hasta 2 decimales";
                mpeMensaje.Show();
                return;
            }

            // string sDescripcion = TxtDescServicio.Text;

            int Envio_Ok = Add_tbProveedor();

            if (Envio_Ok == 0)
            {

                // inicializar controles
                ddlTpoServicio.SelectedValue = "0";
                TxtNomEmpresa.Text = string.Empty;
                TxtEmailEmpresa.Text = string.Empty;

                TxtCalleProveedor.Text = string.Empty;
                TxtNumExtProveedor.Text = string.Empty;
                TxtNumIntProveedor.Text = string.Empty;
                ddlEstadoProveedor.SelectedValue = "0";
                ddlMunicipioProveedor.SelectedValue = "0";
                TxtColoniaProveedor.Text = string.Empty;
                TxtCPostalProveedor.Text = string.Empty;

                TxtTelContacto1.Text = string.Empty;
                TxtTelContacto2.Text = string.Empty;
                TxtHoraSolicitud.Text = "00:00";
                TxtHoraArribo.Text = "00:00";
                TxtHoraSalida.Text = "00:00";
                TxtHoraLlegada.Text = "00:00";

                TxtProvMontoAutorizado.Text = string.Empty;
                TxtNumUnidad.Text = string.Empty;
                TxtResponsable.Text = string.Empty;

                GetDatos_Proveedor();
            }
        }

        protected void BtnAnularPnl7_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            // inhabilitar(this.Controls);
            
            // habilitar_controles();

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            // Inicializar Controles
            ddlTpoServicio.SelectedValue = "0";

            TxtNomEmpresa.Text = string.Empty;
            TxtEmailEmpresa.Text = string.Empty;

            TxtCalleProveedor.Text = string.Empty;
            TxtNumExtProveedor.Text = string.Empty;
            TxtNumIntProveedor.Text = string.Empty;

            ddlEstadoProveedor.SelectedValue = "0";
            ddlMunicipioProveedor.SelectedValue = "0";

            TxtColoniaProveedor.Text = string.Empty;
            TxtCPostalProveedor.Text = string.Empty;

            TxtTelContacto1.Text = string.Empty;
            TxtTelContacto2.Text = string.Empty;
            TxtHoraSolicitud.Text = "00:00";
            TxtHoraArribo.Text = "00:00";
            TxtHoraSalida.Text = "00:00";
            TxtHoraLlegada.Text = "00:00";

            TxtProvMontoAutorizado.Text = string.Empty;
            TxtNumUnidad.Text = string.Empty;
            TxtResponsable.Text = string.Empty;

            ddlTpoServicio.Enabled = true;

            TxtNomEmpresa.ReadOnly = false;
            TxtEmailEmpresa.ReadOnly = false;

            TxtCalleProveedor.ReadOnly = false;
            TxtNumExtProveedor.ReadOnly = false;
            TxtNumIntProveedor.ReadOnly = false;

            ddlEstadoProveedor.Enabled = true;
            ddlMunicipioProveedor.Enabled = true;

            TxtColoniaProveedor.ReadOnly = false;
            TxtCPostalProveedor.ReadOnly = false;

            TxtTelContacto1.ReadOnly = false;
            TxtTelContacto2.ReadOnly = false;
            TxtHoraSolicitud.ReadOnly = false;
            TxtHoraArribo.ReadOnly = false;
            TxtHoraSalida.ReadOnly = false;
            TxtHoraLlegada.ReadOnly = false;

            TxtProvMontoAutorizado.ReadOnly = false;
            TxtNumUnidad.ReadOnly = false;
            TxtResponsable.ReadOnly = false;

            btnEditarPnl7.Visible = true;
            btnActualizarPnl7.Visible = false;
            BtnAnularPnl7.Visible = false;

            btnEditarPnl7.Enabled = false;
            btnAgregarPnl7.Enabled = true;
        }

        protected void btnEditarPnl7_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl7
            TxtHoraSolicitud.ReadOnly = false;
            TxtHoraArribo.ReadOnly = false;
            TxtHoraSalida.ReadOnly = false;
            TxtHoraLlegada.ReadOnly = false;

            TxtProvMontoAutorizado.ReadOnly = false;
            TxtNumUnidad.ReadOnly = false;
            TxtResponsable.ReadOnly = false;

            btnEditarPnl7.Visible = false;
            btnActualizarPnl7.Visible = true;
            BtnAnularPnl7.Visible = true;
        }

        protected void btnActualizarPnl7_Click(object sender, EventArgs e)
        {

            if (TxtNomEmpresa.Text == "" || TxtNomEmpresa.Text == null)
            {
                LblMessage.Text = "Seleccione el nombre de la empresa";
                mpeMensaje.Show();
                return;
            }
            else if (TxtEmailEmpresa.Text == "" || TxtEmailEmpresa.Text == null)
            {
                LblMessage.Text = "Seleccione el correo electrónico";
                mpeMensaje.Show();
                return;
            }

            string valorTexto = TxtProvMontoAutorizado.Text.Trim();

            if (string.IsNullOrEmpty(valorTexto))
            {
                // Opcional: si quieres validar que no esté vacío
                LblMessage.Text = "Ingrese un monto autorizado válido";
                mpeMensaje.Show();

                return; // O muestra mensaje si quieres
            }

            if (!decimal.TryParse(valorTexto, out decimal monto))
            {
                // Opcional: si quieres validar que sea número válido
                return; // O muestra mensaje si quieres
            }

            // Contar decimales en el texto original
            int decimales = 0;
            int indicePunto = valorTexto.IndexOf('.');

            if (indicePunto >= 0)
            {
                decimales = valorTexto.Length - indicePunto - 1;
            }

            if (decimales > 2)
            {
                LblMessage.Text = "monto autorizado, solo se permiten hasta 2 decimales";
                mpeMensaje.Show();
                return;
            }

            Actualizar_ITM_35();

            // habilitar_controles();

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            // inicializar controles.
            ddlTpoServicio.SelectedValue = "0";

            TxtNomEmpresa.Text = string.Empty;
            TxtEmailEmpresa.Text = string.Empty;

            TxtCalleProveedor.Text = string.Empty;
            TxtNumExtProveedor.Text = string.Empty;
            TxtNumIntProveedor.Text = string.Empty;

            ddlEstadoProveedor.SelectedValue = "0";
            ddlMunicipioProveedor.SelectedValue = "0";

            TxtColoniaProveedor.Text = string.Empty;
            TxtCPostalProveedor.Text = string.Empty;

            TxtTelContacto1.Text = string.Empty;
            TxtTelContacto2.Text = string.Empty;
            TxtHoraSolicitud.Text = "00:00";
            TxtHoraArribo.Text = "00:00";
            TxtHoraSalida.Text = "00:00";
            TxtHoraLlegada.Text = "00:00";

            TxtProvMontoAutorizado.Text = string.Empty;
            TxtNumUnidad.Text = string.Empty;
            TxtResponsable.Text = string.Empty;

            ddlTpoServicio.Enabled = true;

            TxtNomEmpresa.ReadOnly = false;
            TxtEmailEmpresa.ReadOnly = false;

            TxtCalleProveedor.ReadOnly = false;
            TxtNumExtProveedor.ReadOnly = false;
            TxtNumIntProveedor.ReadOnly = false;

            ddlEstadoProveedor.Enabled = false;
            ddlMunicipioProveedor.Enabled = false;

            TxtColoniaProveedor.ReadOnly = false;
            TxtCPostalProveedor.ReadOnly = false;

            TxtTelContacto1.ReadOnly = false;
            TxtTelContacto2.ReadOnly = false;

            TxtHoraSolicitud.ReadOnly = false;
            TxtHoraArribo.ReadOnly = false;
            TxtHoraSalida.ReadOnly = false;
            TxtHoraLlegada.ReadOnly = false;

            TxtProvMontoAutorizado.ReadOnly = false;
            TxtNumUnidad.ReadOnly = false;
            TxtResponsable.ReadOnly = false;

            btnEditarPnl7.Enabled = false;
            btnAgregarPnl7.Enabled = true;

            btnEditarPnl7.Visible = true;
            btnActualizarPnl7.Visible = false;
            BtnAnularPnl7.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios correctamente";
            this.mpeMensaje.Show();
        }

        protected void Actualizar_ITM_35()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int index = Variables.wRenglon;

                int Id_Proveedor = Convert.ToInt32(GrdProveedores.Rows[index].Cells[0].Text); ;

                // Actualizar registro(s) tablas (ITM_35)
                string strQuery = "UPDATE ITM_35 " +
                                  "   SET Hora_Solicitud = '" + TxtHoraSolicitud.Text.Trim() + "', " +
                                  "       Hora_Arribo = '" + TxtHoraArribo.Text.Trim() + "', " +
                                  "       Hora_Salida = '" + TxtHoraSalida.Text.Trim() + "', " +
                                  "       Hora_Llegada = '" + TxtHoraLlegada.Text.Trim() + "', " +
                                  "       Monto_Autorizado = " + TxtProvMontoAutorizado.Text.Trim() + ", " +
                                  "       Num_Unidad = '" + TxtNumUnidad.Text.Trim() + "', " +
                                  "       Responsable = '" + TxtResponsable.Text.Trim() + "' " +
                                  " WHERE Id_Proveedor = " + Id_Proveedor + " " +
                                  "   AND Referencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = " + Variables.wSubRef + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizó correctamente el proveedor";
                mpeMensaje.Show();

                GetDatos_Proveedor();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Eliminar_ITM_35()
        {
            try
            {
                int index = Variables.wRenglon;

                string sReferencia = Variables.wRef;
                int iSubReferencia = Variables.wSubRef;

                int Id_Proveedor = Convert.ToInt32(GrdProveedores.Rows[index].Cells[0].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_35)
                string strQuery = "DELETE FROM ITM_35 " +
                                  " WHERE Id_Proveedor = " + Id_Proveedor + " " +
                                  "   AND Referencia = '" + sReferencia + "' " +
                                  "   AND SubReferencia = " + iSubReferencia + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se eliminó correctamente el proveedor";
                mpeMensaje.Show();

                GetDatos_Proveedor();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }


        protected void Actualizar_ITM_36()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int index = Variables.wRenglon;

                int Id_Estudio = Convert.ToInt32(GrdEstudios.Rows[index].Cells[0].Text); ;

                // Actualizar registro(s) tablas (ITM_36)
                string strQuery = "UPDATE ITM_36 " +
                                  "   SET Desc_Estudio = '" + TxtEstudiosRealizados.Text.Trim() + "' " +
                                  " WHERE Id_Estudio = " + Id_Estudio + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizó correctamente el estudio realizado";
                mpeMensaje.Show();

                GetDatos_Estudios();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Eliminar_ITM_36()
        {
            try
            {
                int index = Variables.wRenglon;

                string sReferencia = Variables.wRef;
                int iSubReferencia = Variables.wSubRef;

                int Id_Estudio = Convert.ToInt32(GrdEstudios.Rows[index].Cells[0].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_36)
                string strQuery = "DELETE FROM ITM_36 " +
                                  " WHERE Id_Estudio = " + Id_Estudio + " " +
                                  "   AND Referencia = '" + sReferencia + "' " +
                                  "   AND SubReferencia = " + iSubReferencia + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se eliminó correctamente el estudio realizado";
                mpeMensaje.Show();

                GetDatos_Estudios();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void ImgProveedor_Add_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            ddlTpoServicio.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[1].Text));
            TxtNomEmpresa.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[2].Text));
            TxtHoraSolicitud.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[3].Text));
            TxtHoraArribo.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[4].Text));
            TxtHoraSalida.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[5].Text));
            TxtHoraLlegada.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[6].Text));
            TxtProvMontoAutorizado.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[7].Text));

            TxtCalleProveedor.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[8].Text));
            TxtNumExtProveedor.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[9].Text));
            TxtNumIntProveedor.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[10].Text));
            ddlEstadoProveedor.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[11].Text));

            // Disparar el evento SelectedIndexChanged manualmente
            ddlEstadoProveedor_SelectedIndexChanged(ddlEstadoProveedor, EventArgs.Empty);

            ddlMunicipioProveedor.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[12].Text));
            TxtColoniaProveedor.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[13].Text));
            TxtCPostalProveedor.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[14].Text));

            TxtTelContacto1.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[15].Text));
            TxtTelContacto2.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[16].Text));

            TxtEmailEmpresa.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[17].Text));
            TxtNumUnidad.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[18].Text));
            TxtResponsable.Text = Server.HtmlDecode(Convert.ToString(GrdProveedores.Rows[index].Cells[19].Text));

            TxtNomEmpresa.ReadOnly = true;
            TxtCalleProveedor.ReadOnly = true;
            TxtNumExtProveedor.ReadOnly = true;
            TxtNumIntProveedor.ReadOnly = true;

            ddlEstadoProveedor.Enabled = false;
            ddlMunicipioProveedor.Enabled = false;

            TxtColoniaProveedor.ReadOnly = true;
            TxtCPostalProveedor.ReadOnly = true;

            TxtEmailEmpresa.ReadOnly = true;
            TxtTelContacto1.ReadOnly = true;
            TxtTelContacto2.ReadOnly = true;
            TxtHoraSolicitud.ReadOnly = true;
            TxtHoraArribo.ReadOnly = true;
            TxtHoraSalida.ReadOnly = true;
            TxtHoraLlegada.ReadOnly = true;

            TxtProvMontoAutorizado.ReadOnly = true;
            TxtNumUnidad.ReadOnly = true;
            TxtResponsable.ReadOnly = true;

            ddlTpoServicio.Enabled = false;

            BtnAnularPnl7.Visible = true;
            btnEditarPnl7.Enabled = true;
            btnAgregarPnl7.Enabled = false;
        }

        protected void ImgProveedor_Del_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            Variables.wCPT = false;
            Variables.wICD = false;
            Variables.wEstudios = false;
            Variables.wServicios = false;
            Variables.wProveedor = true;
            Variables.wPaquete = false;

            LblMessage_1.Text = "¿Desea eliminar el proveedor?";
            mpeMensaje_1.Show();
        }

        protected void BtnAnularPnl8_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            inhabilitar(this.Controls);
            habilitar_controles();

            // BtnICD.Enabled = false;
            // BtnCPT.Enabled = false;

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            btnEditarPnl8.Visible = true;
            btnActualizarPnl8.Visible = false;
            BtnAnularPnl8.Visible = false;
        }

        protected void btnEditarPnl8_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl8
            TxtAlergias.Enabled = true;
            TxtEnfermedades.Enabled = true;
            TxtMedicamentos.Enabled = true;
            TxtAlcohol.Enabled = true;
            TxtSustancias.Enabled = true;
            TxtObservaciones_PA.Enabled = true;
            TxtDiagnosticoPreliminar.Enabled = true;
            TxtComentariosMedicos.Enabled = true;

            btnEditarPnl8.Visible = false;
            btnActualizarPnl8.Visible = true;
            BtnAnularPnl8.Visible = true;
        }

        protected void btnActualizarPnl8_Click(object sender, EventArgs e)
        {
            Insertar_ITM_73_6();

            inhabilitar(this.Controls);
            habilitar_controles();

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            btnEditarPnl8.Visible = true;
            btnActualizarPnl8.Visible = false;
            BtnAnularPnl8.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios correctamente";
            this.mpeMensaje.Show();
        }

        protected void btnActualizarPnl13_Click(object sender, EventArgs e)
        {
            string valorTexto = TxtMontoAutorizado.Text.Trim();

            if (string.IsNullOrEmpty(valorTexto))
            {
                // Opcional: si quieres validar que no esté vacío
                LblMessage.Text = "Ingrese un monto autorizado válido";
                mpeMensaje.Show();

                return; // O muestra mensaje si quieres
            }

            if (!decimal.TryParse(valorTexto, out decimal monto))
            {
                // Opcional: si quieres validar que sea número válido
                return; // O muestra mensaje si quieres
            }

            // Contar decimales en el texto original
            int decimales = 0;
            int indicePunto = valorTexto.IndexOf('.');

            if (indicePunto >= 0)
            {
                decimales = valorTexto.Length - indicePunto - 1;
            }

            if (decimales > 2)
            {
                LblMessage.Text = "monto autorizado, solo se permiten hasta 2 decimales";
                mpeMensaje.Show();
                return;
            }

            Insertar_ITM_73_4();

            inhabilitar(this.Controls);
            habilitar_controles();

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            btnEditarPnl13.Visible = true;
            btnActualizarPnl13.Visible = false;
            BtnAnularPnl13.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios correctamente";
            this.mpeMensaje.Show();
        }

        protected void btnEditarPnl13_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl13
            ddlZonas.Enabled = true;
            ddlHospitales.Enabled = true;
            ddlTpoAtencion.Enabled = true;
            
            TxtCorreoElectronico.Enabled = true;
            TxtTelAtencionContacto1.Enabled = true;
            TxtTelAtencionContacto2.Enabled = true;

            TxtFechaIngreso.Enabled = true;
            TxtHoraIngreso.Enabled = true;
            TxtFechaRecepcionNM.Enabled = true;
            TxtHoraRecepcionNM.Enabled = true;

            TxtFechaAlta.Enabled = true;
            TxtHoraAlta.Enabled = true;
            TxtFechaEnvio.Enabled = true;
            TxtHoraEnvio.Enabled = true;

            TxtFechaVigencia.Enabled = true;
            TxtMontoAutorizado.Enabled = true;

            TxtCalleAtencion.Enabled = true;
            TxtNumExtAtencion.Enabled = true;
            TxtNumIntAtencion.Enabled = true;
            ddlEstadoAtencion.Enabled = true;
            ddlMunicipiosAtencion.Enabled = true;
            TxtColoniaAtencion.Enabled = true;
            TxtCPostalAtencion.Enabled = true;

            TxtDiagnostico.Enabled = true;
            TxtObservaciones_DA.Enabled = true;
            TxtPlanTratamiento.Enabled = true;

            btnEditarPnl13.Visible = false;
            btnActualizarPnl13.Visible = true;
            BtnAnularPnl13.Visible = true;
        }


        protected void BtnAnularPnl13_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            inhabilitar(this.Controls);
            habilitar_controles();

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            btnEditarPnl13.Visible = true;
            btnActualizarPnl13.Visible = false;
            BtnAnularPnl13.Visible = false;
        }


        protected void BtnAnularPnl16_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            // inhabilitar(this.Controls);
            habilitar_controles();

            ddlInstituciones.SelectedValue = "0";

            ddlPaquetes_MD.Items.Clear();
            ddlPaquetes_MD.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

            ddlInstituciones.Enabled = true;
            ddlPaquetes_MD.Enabled = true;

            TxtIdPaquete.Text = string.Empty;
            TxtMontoMinimo.Text = string.Empty;
            TxtMontoMaximo.Text = string.Empty;
            TxtMontoUtilizado.Text = string.Empty;
            TxtMontoRestante.Text = string.Empty;
            TxtMontoSuperado.Text = string.Empty;
            TxtObservaciones_PM.Text = string.Empty;

            TxtIdPaquete.ReadOnly = false;
            TxtMontoUtilizado.ReadOnly = false;
            TxtMontoRestante.ReadOnly = false;
            TxtMontoSuperado.ReadOnly = false;
            TxtObservaciones_PM.ReadOnly = false;

            btnEditarPnl16.Visible = true;
            btnActualizarPnl16.Visible = false;
            BtnAnularPnl16.Visible = false;

            btnEditarPnl16.Enabled = false;
            BtnAgregarPnl16.Enabled = true;
        }


        protected void btnEditarPnl16_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl16
            ddlInstituciones.Enabled = false;
            ddlPaquetes_MD.Enabled = false;

            //TxtIdPaquete.Enabled = true;
            //TxtMontoUtilizado.Enabled = true;
            //TxtMontoRestante.Enabled = true;
            //TxtMontoSuperado.Enabled = true;
            //TxtObservaciones_PM.Enabled = true;

            TxtIdPaquete.ReadOnly = false;
            TxtMontoUtilizado.ReadOnly = false;
            TxtMontoRestante.ReadOnly = false;
            TxtMontoSuperado.ReadOnly = false;
            TxtObservaciones_PM.ReadOnly = false;

            btnEditarPnl16.Visible = false;
            btnActualizarPnl16.Visible = true;
            BtnAnularPnl16.Visible = true;
        }

        protected void btnActualizarPnl16_Click(object sender, EventArgs e)
        {

            Actualizar_ITM_38();

            habilitar_controles();

            ddlInstituciones.Enabled = true;
            ddlPaquetes_MD.Enabled = true;

            // inicializar controles.
            ddlInstituciones.SelectedValue = "0";

            ddlPaquetes_MD.Items.Clear();
            ddlPaquetes_MD.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

            TxtIdPaquete.Text = string.Empty;
            TxtMontoMinimo.Text = string.Empty;
            TxtMontoMaximo.Text = string.Empty;
            TxtMontoUtilizado.Text = string.Empty;
            TxtMontoRestante.Text = string.Empty;
            TxtMontoSuperado.Text = string.Empty;
            TxtObservaciones_PM.Text = string.Empty;

            btnEditarPnl16.Enabled = false;
            BtnAgregarPnl16.Enabled = true;

            btnEditarPnl16.Visible = true;
            btnActualizarPnl16.Visible = false;
            BtnAnularPnl16.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios correctamente";
            this.mpeMensaje.Show();
        }

        protected void BtnAgregarPnl16_Click(object sender, EventArgs e)
        {
            if (ddlInstituciones.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Institución hospitalaria";
                mpeMensaje.Show();
                return;
            }
            else if (ddlPaquetes_MD.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Nombre del Paquete";
                mpeMensaje.Show();
                return;
            }
            else if (TxtIdPaquete.Text == "" || TxtIdPaquete.Text == null)
            {
                LblMessage.Text = "Capturar ID del Paquete";
                mpeMensaje.Show();
                return;
            }

            int sPaquete = Convert.ToInt32(ddlPaquetes_MD.SelectedValue);

            int Envio_Ok = Add_tbPaquete(Convert.ToInt32(ddlInstituciones.SelectedValue), sPaquete);

            if (Envio_Ok == 0)
            {

                // inicializar controles
                ddlInstituciones.SelectedValue = "0";

                ddlPaquetes_MD.Items.Clear();
                ddlPaquetes_MD.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                TxtIdPaquete.Text = string.Empty;
                TxtMontoMinimo.Text = string.Empty;
                TxtMontoMaximo.Text = string.Empty;
                TxtMontoUtilizado.Text = string.Empty;
                TxtMontoRestante.Text = string.Empty;
                TxtMontoSuperado.Text = string.Empty;
                TxtObservaciones_PM.Text = string.Empty;

                GetDatos_Paquetes();
            }
        }

        protected void BtnAgregarPnl14_Click(object sender, EventArgs e)
        {
            if (ddlServicios.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar tipo de servicio";
                mpeMensaje.Show();
                return;
            }
            else if (TxtFechaServicio.Text == "" || TxtFechaServicio.Text == null)
            {
                LblMessage.Text = "Capturar Fecha Servicio";
                mpeMensaje.Show();
                return;
            }
            else if (TxtHoraServicio.Text == "" || TxtHoraServicio.Text == null)
            {
                LblMessage.Text = "Capturar Hora Servicio";
                mpeMensaje.Show();
                return;
            }
            else if (TxtDescServicio.Text == "" || TxtDescServicio.Text == null)
            {
                LblMessage.Text = "Capturar Descripción Servicio";
                mpeMensaje.Show();
                return;
            }

            string sFechaServicio = TxtFechaServicio.Text;
            string sHoraServicio = TxtHoraServicio.Text;
            string sDescripcion = TxtDescServicio.Text;

            int Envio_Ok = Add_tbServicio(Convert.ToInt32(ddlServicios.SelectedValue), sFechaServicio, sHoraServicio, sDescripcion);

            if (Envio_Ok == 0)
            {

                // inicializar controles
                ddlServicios.SelectedValue = "0";
                TxtDescServicio.Text = string.Empty;
                TxtFechaServicio.Text = string.Empty;
                TxtHoraServicio.Text = "00:00";

                GetDatos_Servicios();
            }
        }

        protected void BtnAnularPnl14_Click(object sender, EventArgs e)
        {
            // habilitar_controles();

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            // Inicializar Controles
            ddlServicios.SelectedValue = "0";
            TxtFechaServicio.Text = string.Empty;
            TxtHoraServicio.Text = "00:00";
            TxtDescServicio.Text = string.Empty;

            ddlServicios.Enabled = true;
            TxtFechaServicio.ReadOnly = false;
            TxtHoraServicio.ReadOnly = false;
            TxtDescServicio.ReadOnly = false;

            btnEditarPnl14.Visible = true;
            btnActualizarPnl14.Visible = false;
            BtnAnularPnl14.Visible = false;

            btnEditarPnl14.Enabled = false;
            BtnAgregarPnl14.Enabled = true;
        }

        protected void btnEditarPnl14_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl14
            TxtFechaServicio.ReadOnly = false;
            TxtHoraServicio.ReadOnly = false;
            TxtDescServicio.ReadOnly = false;

            btnEditarPnl14.Visible = false;
            btnActualizarPnl14.Visible = true;
            BtnAnularPnl14.Visible = true;
        }

        protected void btnActualizarPnl14_Click(object sender, EventArgs e)
        {
            if (TxtFechaServicio.Text.Trim() == "" || TxtFechaServicio.Text == null)
            {
                LblMessage.Text = "Capturar Fecha Servicio";
                mpeMensaje.Show();
                return;
            }
            if (TxtHoraServicio.Text == "" || TxtHoraServicio.Text == null)
            {
                LblMessage.Text = "Capturar Hora Servicio";
                mpeMensaje.Show();
                return;
            }
            else if (TxtDescServicio.Text == "" || TxtDescServicio.Text == null)
            {
                LblMessage.Text = "Capturar Descripción Servicio";
                mpeMensaje.Show();
                return;
            }

            Actualizar_ITM_07();

            // habilitar_controles();

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            // inicializar controles.
            ddlServicios.Enabled = true;

            ddlServicios.SelectedValue = "0";
            TxtFechaServicio.Text = string.Empty;
            TxtHoraServicio.Text = "00:00";
            TxtDescServicio.Text = string.Empty;

            btnEditarPnl14.Enabled = false;
            BtnAgregarPnl14.Enabled = true;

            btnEditarPnl14.Visible = true;
            btnActualizarPnl14.Visible = false;
            BtnAnularPnl14.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios correctamente";
            this.mpeMensaje.Show();

        }

        protected void ImgServicio_Add_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            ddlServicios.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdServicios.Rows[index].Cells[1].Text));
            TxtFechaServicio.Text = Server.HtmlDecode(Convert.ToString(GrdServicios.Rows[index].Cells[3].Text));
            TxtHoraServicio.Text = Server.HtmlDecode(Convert.ToString(GrdServicios.Rows[index].Cells[4].Text));
            TxtDescServicio.Text = Server.HtmlDecode(Convert.ToString(GrdServicios.Rows[index].Cells[5].Text));

            ddlServicios.Enabled = false;
            TxtFechaServicio.ReadOnly = true;
            TxtHoraServicio.ReadOnly = true;
            TxtDescServicio.ReadOnly = true;

            BtnAnularPnl14.Visible = true;
            btnEditarPnl14.Enabled = true;
            BtnAgregarPnl14.Enabled = false;
        }

        protected void ImgServicio_Del_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            Variables.wCPT = false;
            Variables.wICD = false;
            Variables.wEstudios = false;
            Variables.wServicios = true;
            Variables.wProveedor = false;
            Variables.wPaquete = false;

            LblMessage_1.Text = "¿Desea eliminar el servicio?";
            mpeMensaje_1.Show();
        }

        protected void Actualizar_ITM_07()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int index = Variables.wRenglon;

                int IdConsecutivo = Convert.ToInt32(GrdServicios.Rows[index].Cells[0].Text); ;

                // Actualizar registro(s) tablas (ITM_07)
                string strQuery = "UPDATE ITM_07 " +
                                  "   SET FechaServicio = '" + TxtFechaServicio.Text.Trim() + "'," +
                                  "   Hora_Servicio = '" + TxtHoraServicio.Text.Trim() + "', " +
                                  "   Desc_Servicio = '" + TxtDescServicio.Text.Trim() + "' " +
                                  " WHERE Referencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = " + Variables.wSubRef + " " +
                                  "   AND IdConsecutivo = " + IdConsecutivo + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizó correctamente el servicio";
                mpeMensaje.Show();

                GetDatos_Servicios();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Eliminar_ITM_07()
        {
            try
            {
                int index = Variables.wRenglon;

                string sReferencia = Variables.wRef;
                int iSubReferencia = Variables.wSubRef;

                int IdConsecutivo = Convert.ToInt32(GrdServicios.Rows[index].Cells[0].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_07)
                string strQuery = "DELETE FROM ITM_07 " +
                                  " WHERE Referencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = " + Variables.wSubRef + " " +
                                  "   AND IdConsecutivo = " + IdConsecutivo + " ";

                                  //"   AND Referencia = '" + sReferencia + "' " +
                                  //"   AND SubReferencia = " + iSubReferencia + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se eliminó correctamente el servicio";
                mpeMensaje.Show();

                GetDatos_Servicios();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        public int Add_tbProveedor()
        {
            try
            {
                int Id_Proveedor = GetIdProveedorMax();

                int iTpo_Servicio = Convert.ToInt32(ddlTpoServicio.SelectedValue);
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;
                string Id_Usuario = Variables.wUserLogon;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = $"INSERT INTO ITM_35 (Id_Proveedor, Tpo_Servicio, Referencia, SubReferencia, " +
                                  $" Nom_Empresa, Email_Empresa, Calle, Num_Exterior, Num_Interior, Estado, Delegacion, " +
                                  $" Colonia, Codigo_Postal, Tel_Contacto_1, Tel_Contacto_2, Hora_Solicitud, " +
                                  $" Hora_Arribo, Hora_Salida, Hora_Llegada, Monto_Autorizado, Num_Unidad, " +
                                  $" Responsable, Id_Usuario, IdStatus) " +
                                  $"VALUES (" + Id_Proveedor + ", " + iTpo_Servicio + ", '" + Referencia + "', '" + SubReferencia + "', " +
                                  "'" + TxtNomEmpresa.Text.Trim() + "', '" + TxtEmailEmpresa.Text.Trim() + "', '" + TxtCalleProveedor.Text.Trim() + "', '" + TxtNumExtProveedor.Text.Trim() + "', " +
                                  "'" + TxtNumIntProveedor.Text.Trim() + "', '" + ddlEstadoProveedor.SelectedValue + "', '" + ddlMunicipioProveedor.SelectedValue + "', '" + TxtColoniaProveedor.Text.Trim() + "', " +
                                  "'" + TxtCPostalProveedor.Text.Trim() + "', '" + TxtTelContacto1.Text.Trim() + "', '" + TxtTelContacto2.Text.Trim() + "', " +
                                  "'" + TxtHoraSolicitud.Text.Trim() + "', '" + TxtHoraArribo.Text.Trim() + "', '" + TxtHoraSalida.Text.Trim() + "', '" + TxtHoraLlegada.Text.Trim() + "', " +
                                  "" + TxtProvMontoAutorizado.Text.Trim() + ", '" + TxtNumUnidad.Text.Trim() + "', '" + TxtResponsable.Text.Trim() + "', '" + Id_Usuario + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se agregó proveedor correctamente";
                mpeMensaje.Show();

                return 0;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }
            return -1;
        }

        public int GetIdProveedorMax()
        {

            int Id_ProveedorMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX( Id_Proveedor ), 0) + 1 Id_Proveedor " +
                              "  FROM ITM_35 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            foreach (DataRow row in dt.Rows)
            {
                Id_ProveedorMax = Convert.ToInt32(row["Id_Proveedor"].ToString().Trim());
            }

            dbConn.Close();

            return Id_ProveedorMax;

        }

        public int Add_tbEstudio(string pDescripcion)
        {
            try
            {
                int Id_Estudio = GetIdEstudioMax();
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = $"INSERT INTO ITM_36 (Id_Estudio, Referencia, SubReferencia, Desc_Estudio, IdStatus) " +
                                  $"VALUES (" + Id_Estudio + ", '" + Referencia + "', '" + SubReferencia + "', '" + pDescripcion + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se agregó el estudio realizado correctamente";
                mpeMensaje.Show();

                return 0;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }
            return -1;
        }

        public int GetIdEstudioMax()
        {

            int IdEstudioMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX( Id_Estudio ), 0) + 1 Id_Estudio " +
                              "  FROM ITM_36 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            foreach (DataRow row in dt.Rows)
            {
                IdEstudioMax = Convert.ToInt32(row["Id_Estudio"].ToString().Trim());
            }

            dbConn.Close();

            return IdEstudioMax;

        }

        public int Add_tbPaquete(int iId_Institucion, int iId_Paquete_Medico)
        {
            try
            {
                int iConsecutivo = GetIdPaqueteMax();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;
                string Id_Usuario = Variables.wUserLogon;

                decimal MontoUtilizado = 0;

                if (!string.IsNullOrWhiteSpace(TxtMontoUtilizado.Text.Trim()))
                {
                    decimal.TryParse(TxtMontoUtilizado.Text.Trim(), out MontoUtilizado);
                }

                decimal MontoRestante = 0;

                if (!string.IsNullOrWhiteSpace(TxtMontoRestante.Text.Trim()))
                {
                    decimal.TryParse(TxtMontoRestante.Text.Trim(), out MontoRestante);
                }

                decimal MontoSuperado = 0;

                if (!string.IsNullOrWhiteSpace(TxtMontoSuperado.Text.Trim()))
                {
                    decimal.TryParse(TxtMontoSuperado.Text.Trim(), out MontoSuperado);
                }

                string strQuery = $"INSERT INTO ITM_38 (Id_Consecutivo, Id_Institucion, Id_Paquete_Medico, Referencia, SubReferencia, " +
                                  $"                    ID_Paquete, Monto_Minimo, Monto_Maximo, Monto_Utilizado, Monto_Restante, " +
                                  $"                    Monto_Superado, Observaciones, Id_Usuario, IdStatus) " +
                                  $"VALUES (" + iConsecutivo + ", " + iId_Institucion + ", " + iId_Paquete_Medico + ", '" + Referencia + "', " + SubReferencia + ", '" + TxtIdPaquete.Text.Trim() + "', " +
                                  "" + TxtMontoMinimo.Text.Trim() + ", " + TxtMontoMaximo.Text.Trim() + ", " + MontoUtilizado + ", " + MontoRestante + ", " +
                                  "" + MontoSuperado + ", '" + TxtObservaciones_PM.Text.Trim() + "', '" + Id_Usuario + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se agregó paquete médico correctamente";
                mpeMensaje.Show();

                return 0;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }
            return -1;
        }

        public int GetIdPaqueteMax()
        {

            int IdConsecutivoMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX( Id_Consecutivo ), 0) + 1 Id_Consecutivo " +
                              "  FROM ITM_38 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["Id_Consecutivo"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        public int Add_tbServicio(int iIdServicio, string pFechaServicio, string pHoraServicio, string pDescripcion)
        {
            try
            {
                int iConsecutivo = GetIdConsecutivoMax();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = $"INSERT INTO ITM_07 (IdConsecutivo, Referencia, SubReferencia, IdServicio, FechaServicio, Hora_Servicio, Desc_Servicio, IdStatus) " +
                                  $"VALUES (" + iConsecutivo + ", '" + Variables.wRef + "', " + Variables.wSubRef + ", " + iIdServicio + ", " +
                                  "'" + pFechaServicio + "', '" + pHoraServicio + "', '" + pDescripcion + "', 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se agregó servicio correctamente";
                mpeMensaje.Show();

                return 0;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }
            return -1;
        }

        public int GetIdConsecutivoMax()
        {

            int IdConsecutivoMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX( IdConsecutivo ), 0) + 1 IdConsecutivo " +
                              "  FROM ITM_07 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdConsecutivo"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void Eliminar_tbServicio(string IdColumna, string Tabla, int iIdDocumento, int iIdTpoAsunto)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName; //LblUsuario.Text;

                // Eliminar registro tabla
                string strQuery = $"DELETE FROM { Tabla } " +
                                  $" WHERE { IdColumna } = " + iIdDocumento + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se eliminó correctamente el responsable";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    LblMessage.Text = "Responsable, se encuentra relacionado a un Asunto";
                }
                else
                {
                    LblMessage.Text = Convert.ToString(ex.Message);
                }

                mpeMensaje.Show();
            }
        }

        protected void Actualizar_tbServicio(string IdColumna, string Tabla, int iIdDocumento, int iIdTpoAsunto)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName; //LblUsuario.Text;

                // Eliminar registro tabla
                string strQuery = $"UPDATE { Tabla } " +
                                  $"   SET Descripcion = '" + TxtNomResponsable.Text.Trim() + "' " +
                                  $" WHERE { IdColumna } = " + iIdDocumento + " ";
                //$"   AND IdTpoAsunto = " + iIdTpoAsunto + "";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizó correctamente el servicio";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    LblMessage.Text = "Responsable, se encuentra relacionado a un Asunto";
                }
                else
                {
                    LblMessage.Text = Convert.ToString(ex.Message);
                }

                mpeMensaje.Show();
            }
        }

        public void GetDatos_Estudios()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_36
                string strQuery = "SELECT Id_Estudio, Referencia, SubReferencia, Desc_Estudio " +
                                  "  FROM ITM_36 " +
                                  " WHERE Referencia = '" + Variables.wRef + "' " +
                                  "   ANd SubReferencia = " + Variables.wSubRef + " " +
                                  "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdEstudios.ShowHeaderWhenEmpty = true;
                    GrdEstudios.EmptyDataText = "No hay resultados.";
                }

                GrdEstudios.DataSource = dt;
                GrdEstudios.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdServicios.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetDatos_Servicios()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_07
                string strQuery = "SELECT IdConsecutivo, t1.IdServicio, t1.FechaServicio, t1.Hora_Servicio, t0.Descripcion, Desc_Servicio " +
                                  "  FROM ITM_06 t0, ITM_07 t1 " +
                                  " WHERE Referencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = " + Variables.wSubRef + " " +
                                  "   AND t0.IdServicio = t1.IdServicio " +
                                  "   AND t1.IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdServicios.ShowHeaderWhenEmpty = true;
                    GrdServicios.EmptyDataText = "No hay resultados.";
                }

                GrdServicios.DataSource = dt;
                GrdServicios.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdServicios.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetDatos_Proveedor()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Datos de Proveedor = ITM_35
                string strQuery = "SELECT Id_Proveedor, Tpo_Servicio, Referencia, SubReferencia, Nom_Empresa," +
                                  "       Email_Empresa, Calle, Num_Exterior, Num_Interior, Estado, Delegacion, Colonia, Codigo_Postal, " +
                                  "       Tel_Contacto_1, Tel_Contacto_2, Hora_Solicitud, Hora_Arribo, Hora_Salida, Hora_Llegada, Monto_Autorizado, Num_Unidad, Responsable " +
                                  "  FROM ITM_35 " +
                                  " WHERE Referencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = " + Variables.wSubRef + " " +
                                  "   AND IdStatus = 1 ";

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

        public void GetDatos_Paquetes()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Datos de Proveedor = ITM_38
                string strQuery = "SELECT t1.Id_Consecutivo, t1.Id_Institucion, t1.Id_Paquete_Medico, t0.Descripcion, t1.Referencia, t1.SubReferencia, t1.ID_Paquete," +
                                  "       t1.Monto_Minimo, t1.Monto_Maximo, t1.Monto_Utilizado, t1.Monto_Restante, t1.Monto_Superado, t1.Observaciones " +
                                  "  FROM ITM_13 t0, ITM_38 t1 " +
                                  " WHERE t1.Referencia = '" + Variables.wRef + "' " +
                                  "   AND t1.SubReferencia = " + Variables.wSubRef + " " +
                                  "   AND t1.Id_Institucion = t0.Id_Institucion " +
                                  "   AND t1.IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdPaqueteMedico.ShowHeaderWhenEmpty = true;
                    GrdPaqueteMedico.EmptyDataText = "No hay resultados.";
                }

                GrdPaqueteMedico.DataSource = dt;
                GrdPaqueteMedico.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdPaqueteMedico.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnNine_Click(object sender, EventArgs e)
        {
            try
            {
                if (Variables.wIdTpoAsunto == 4 || Variables.wIdTpoAsunto == 5)
                {
                    Dictionary<string, TextBox> fechas = new Dictionary<string, TextBox>
                    {
                        { "Fecha Inicio Vigencia", TxtFechaIniVigencia },
                        { "Fecha Final Vigencia", TxtFechaFinVigencia },
                        { "Fecha de Ocurrencia", TxtFechaOcurrencia },
                        { "Fecha de Nacimiento", TxtFecNacimiento }
                    };

                    foreach (var fecha in fechas)
                    {
                        if (string.IsNullOrEmpty(fecha.Value.Text))
                        {
                            string mensaje = $"Por favor, complete {fecha.Key}.";

                            LblMessage.Text = mensaje;
                            this.mpeMensaje.Show();

                            return;
                        }
                    }

                    Genera_Documento("VF_PASE_MEDICO_RESP_QR.docx", "PMQRR");

                }
                else
                {
                    LblMessage.Text = "No existe plantilla para este tipo de asunto";
                    this.mpeMensaje.Show();
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }
        protected void BtnEight_Click(object sender, EventArgs e)
        {
            try
            {
                if (Variables.wIdTpoAsunto == 4 || Variables.wIdTpoAsunto == 5)
                {
                    Dictionary<string, TextBox> fechas = new Dictionary<string, TextBox>
                    {
                        { "Fecha Inicio Vigencia", TxtFechaIniVigencia },
                        { "Fecha Final Vigencia", TxtFechaFinVigencia },
                        { "Fecha de Ocurrencia", TxtFechaOcurrencia },
                        { "Fecha de Nacimiento", TxtFecNacimiento }
                    };

                    foreach (var fecha in fechas)
                    {
                        if (string.IsNullOrEmpty(fecha.Value.Text))
                        {
                            string mensaje = $"Por favor, complete {fecha.Key}.";

                            LblMessage.Text = mensaje;
                            this.mpeMensaje.Show();

                            return;
                        }
                    }

                    Genera_Documento("VF_PASE_MEDICO_BENEF_QR.docx", "PMQRB");

                }
                else
                {
                    LblMessage.Text = "No existe plantilla para este tipo de asunto";
                    this.mpeMensaje.Show();
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnSeven_Click(object sender, EventArgs e)
        {
            try
            {
                if (Variables.wIdTpoAsunto == 4 || Variables.wIdTpoAsunto == 5)
                {
                    Dictionary<string, TextBox> fechas = new Dictionary<string, TextBox>
                    {
                        { "Fecha Inicio Vigencia", TxtFechaIniVigencia },
                        { "Fecha Final Vigencia", TxtFechaFinVigencia },
                        { "Fecha de Ocurrencia", TxtFechaOcurrencia },
                        { "Fecha de Nacimiento", TxtFecNacimiento }
                    };

                    foreach (var fecha in fechas)
                    {
                        if (string.IsNullOrEmpty(fecha.Value.Text))
                        {
                            string mensaje = $"Por favor, complete {fecha.Key}.";

                            LblMessage.Text = mensaje;
                            this.mpeMensaje.Show();

                            return;
                        }
                    }

                    Genera_Documento("VF_RESP_PRE_PASE_MEDICO.docx", "PPRSTC");

                }
                else
                {
                    LblMessage.Text = "No existe plantilla para este tipo de asunto";
                    this.mpeMensaje.Show();
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnSix_Click(object sender, EventArgs e)
        {
            try
            {
                if (Variables.wIdTpoAsunto == 4 || Variables.wIdTpoAsunto == 5)
                {
                    Dictionary<string, TextBox> fechas = new Dictionary<string, TextBox>
                    {
                        { "Fecha Inicio Vigencia", TxtFechaIniVigencia },
                        { "Fecha Final Vigencia", TxtFechaFinVigencia },
                        { "Fecha de Ocurrencia", TxtFechaOcurrencia },
                        { "Fecha de Nacimiento", TxtFecNacimiento }
                    };

                    foreach (var fecha in fechas)
                    {
                        if (string.IsNullOrEmpty(fecha.Value.Text))
                        {
                            string mensaje = $"Por favor, complete {fecha.Key}.";

                            LblMessage.Text = mensaje;
                            this.mpeMensaje.Show();

                            return;
                        }
                    }

                    Genera_Documento("VF_BENEF_PRE_PASE_MEDICO.docx", "PPBSTC");

                }
                else
                {
                    LblMessage.Text = "No existe plantilla para este tipo de asunto";
                    this.mpeMensaje.Show();
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnFive_Click(object sender, EventArgs e)
        {
            try
            {
                if (Variables.wIdTpoAsunto == 4 || Variables.wIdTpoAsunto == 5)
                {
                    Dictionary<string, TextBox> fechas = new Dictionary<string, TextBox>
                    {
                        { "Fecha Inicio Vigencia", TxtFechaIniVigencia },
                        { "Fecha Final Vigencia", TxtFechaFinVigencia },
                        { "Fecha de Ocurrencia", TxtFechaOcurrencia },
                        { "Fecha de Nacimiento", TxtFecNacimiento }
                    };

                    foreach (var fecha in fechas)
                    {
                        if (string.IsNullOrEmpty(fecha.Value.Text))
                        {
                            string mensaje = $"Por favor, complete {fecha.Key}.";

                            LblMessage.Text = mensaje;
                            this.mpeMensaje.Show();

                            return;
                        }
                    }

                    Genera_Documento("VF_INFORME_PRELIMINAR.docx", "IPSTC");

                }
                else
                {
                    LblMessage.Text = "No existe plantilla para este tipo de asunto";
                    this.mpeMensaje.Show();
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnFour_Click(object sender, EventArgs e)
        {
            try
            {
                if (Variables.wIdTpoAsunto == 4 || Variables.wIdTpoAsunto == 5)
                {
                    Dictionary<string, TextBox> fechas = new Dictionary<string, TextBox>
                    {
                        { "Fecha Inicio Vigencia", TxtFechaIniVigencia },
                        { "Fecha Final Vigencia", TxtFechaFinVigencia },
                        { "Fecha de Ocurrencia", TxtFechaOcurrencia },
                        { "Fecha de Nacimiento", TxtFecNacimiento }
                    };

                    foreach (var fecha in fechas)
                    {
                        if (string.IsNullOrEmpty(fecha.Value.Text))
                        {
                            string mensaje = $"Por favor, complete {fecha.Key}.";

                            LblMessage.Text = mensaje;
                            this.mpeMensaje.Show();

                            return;
                        }
                    }

                    Genera_Documento("VF_DICTAMEN_MEDICO.docx", "DMSTC");

                }
                else
                {
                    LblMessage.Text = "No existe plantilla para este tipo de asunto";
                    this.mpeMensaje.Show();
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnThree_Click(object sender, EventArgs e)
        {
            try
            {
                if (Variables.wIdTpoAsunto == 4 || Variables.wIdTpoAsunto == 5)
                {
                    Dictionary<string, TextBox> fechas = new Dictionary<string, TextBox>
                    {
                        { "Fecha Inicio Vigencia", TxtFechaIniVigencia },
                        { "Fecha Final Vigencia", TxtFechaFinVigencia },
                        { "Fecha de Ocurrencia", TxtFechaOcurrencia },
                        { "Fecha de Nacimiento", TxtFecNacimiento }
                    };

                    foreach (var fecha in fechas)
                    {
                        if (string.IsNullOrEmpty(fecha.Value.Text))
                        {
                            string mensaje = $"Por favor, complete {fecha.Key}.";

                            LblMessage.Text = mensaje;
                            this.mpeMensaje.Show();

                            return;
                        }
                    }

                    Genera_Documento("VF_CARTA_AUTORIZACION.docx", "CASTC");

                }
                else
                {
                    LblMessage.Text = "No existe plantilla para este tipo de asunto";
                    this.mpeMensaje.Show();
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnTwo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Variables.wIdTpoAsunto == 4 || Variables.wIdTpoAsunto == 5)
                {
                    Dictionary<string, TextBox> fechas = new Dictionary<string, TextBox>
                    {
                        { "Fecha Inicio Vigencia", TxtFechaIniVigencia },
                        { "Fecha Final Vigencia", TxtFechaFinVigencia },
                        { "Fecha de Ocurrencia", TxtFechaOcurrencia },
                        { "Fecha de Nacimiento", TxtFecNacimiento }
                    };

                    foreach (var fecha in fechas)
                    {
                        if (string.IsNullOrEmpty(fecha.Value.Text))
                        {
                            string mensaje = $"Por favor, complete {fecha.Key}.";

                            LblMessage.Text = mensaje;
                            this.mpeMensaje.Show();

                            return;
                        }
                    }

                    Genera_Documento("VF_PRIMERA_RESPUESTA.docx", "PRSTC");

                }
                else
                {
                    LblMessage.Text = "No existe plantilla para este tipo de asunto";
                    this.mpeMensaje.Show();
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnOne_Click(object sender, EventArgs e)
        {
            try
            {
                if (Variables.wIdTpoAsunto == 4 || Variables.wIdTpoAsunto == 5) 
                {
                    Dictionary<string, TextBox> fechas = new Dictionary<string, TextBox>
                    {
                        { "Fecha Inicio Vigencia", TxtFechaIniVigencia },
                        { "Fecha Final Vigencia", TxtFechaFinVigencia },
                        { "Fecha de Ocurrencia", TxtFechaOcurrencia },
                        { "Fecha de Nacimiento", TxtFecNacimiento }
                    };

                    foreach (var fecha in fechas)
                    {
                        if (string.IsNullOrEmpty(fecha.Value.Text))
                        {
                            string mensaje = $"Por favor, complete {fecha.Key}.";

                            LblMessage.Text = mensaje;
                            this.mpeMensaje.Show();

                            return;
                        }
                    }

                    Genera_Documento("VF_PASE_MEDICO_STC_METRO.docx", "PMSTC");

                }
                else
                {
                    LblMessage.Text = "No existe plantilla para este tipo de asunto";
                    this.mpeMensaje.Show();
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        private void Genera_Documento(string sPlantilla, string sPrefijo)
        {
            try
            {
                string plantillaPath = Server.MapPath("~/itnowstorage/" + sPlantilla);

                // string documentoGeneradoPath = Server.MapPath("~/itnowstorage/InformePreliminar.docx");
                string Nom_Documento = sPrefijo + "_" + Variables.wRef + ".docx";
                string documentoGeneradoPath = Server.MapPath("~/itnowstorage/" + Nom_Documento);

                // Obtener de RIESGOS los campos seleccionados
                // string Sub_Ramo = Categorias_Riegos();      // "Variación de Voltaje";

                // Obtener la fecha del 1er Contacto
                string Fec_Contacto = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                // Obtener la fecha Actual
                string Fec_Solicitud = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecOcurrencia = DateTime.ParseExact(TxtFechaOcurrencia.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaOcurrencia = fecOcurrencia.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecNacimiento = DateTime.ParseExact(TxtFecNacimiento.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string Fec_Nacimiento = fecNacimiento.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                string abrev_Colonia = "Colonia";
                string abrev_CodigoP = "C.P.";

                //string Dom_Asegurado = TxtCalle.Text.Trim() + " " + TxtNumExterior.Text.Trim() + " " + TxtNumInterior.Text.Trim() + ", " + abrev_Colonia + " " + TxtColonia.Text.Trim()
                //                      + ", " + ddlMunicipios.SelectedItem + ", " + ddlEstado.SelectedItem + ", " + abrev_CodigoP + " " + TxtCodigoPostal.Text.Trim();

                //string Dom_Ocurrencia = TxtCalle.Text.Trim() + " " + TxtNumExterior.Text.Trim() + " " + TxtNumInterior.Text.Trim() + ", " + abrev_Colonia + " " + TxtColonia.Text.Trim()
                //                      + ", " + ddlMunicipios.SelectedItem + ", " + ddlEstado.SelectedItem + ", " + abrev_CodigoP + " " + TxtCodigoPostal.Text.Trim();

                string Dom_Ocurrencia = string.Empty;

                string Dom_Lesionado = TxtCalleLesionado.Text.Trim() + " " + TxtNumExtLesionado.Text.Trim() + " " + TxtNumIntLesionado.Text.Trim() + ", " + abrev_Colonia + " " + TxtColoniaLesionado.Text.Trim()
                                      + ", " + ddlMunicipiosLesionado.SelectedItem + ", " + ddlEstadoLesionado.SelectedItem + ", " + abrev_CodigoP + " " + TxtCPostalLesionado.Text.Trim();

                string Dom_Responsable = TxtCalleResponsable.Text.Trim() + " " + TxtNumExtResponsable.Text.Trim() + " " + TxtNumIntResponsable.Text.Trim() + ", " + abrev_Colonia + " " + TxtColoniaResponsable.Text.Trim()
                                      + ", " + ddlMunicipiosResponsable.SelectedItem + ", " + ddlEstadoResponsable.SelectedItem + ", " + abrev_CodigoP + " " + TxtCPostalResponsable.Text.Trim();


                // Obtener Paquete Medico
                // Iterar sobre las filas del GridView
                // Declaración de variables
                string Descripcion_1 = string.Empty, ID_Paquete_1 = string.Empty;
                string Descripcion_2 = string.Empty, ID_Paquete_2 = string.Empty;
                string Descripcion_3 = string.Empty, ID_Paquete_3 = string.Empty;
                string Descripcion_4 = string.Empty, ID_Paquete_4 = string.Empty;

                for (int i = 0; i < GrdPaqueteMedico.Rows.Count && i < 4; i++) // Máximo 4 filas
                {
                    GridViewRow row = GrdPaqueteMedico.Rows[i];

                    // Recuperar valores de las celdas
                    string Descripcion = Server.HtmlDecode(Convert.ToString(row.Cells[3].Text));       // Descripción
                    string ID_Paquete = Server.HtmlDecode(Convert.ToString(row.Cells[4].Text));        // ID_Paquete

                    // Asignar valores a las variables según el renglón
                    switch (i)
                    {
                        case 0:
                            Descripcion_1 = Descripcion;
                            ID_Paquete_1 = ID_Paquete;
                            break;
                        case 1:
                            Descripcion_2 = Descripcion;
                            ID_Paquete_2 = ID_Paquete;
                            break;
                        case 2:
                            Descripcion_3 = Descripcion;
                            ID_Paquete_3 = ID_Paquete;
                            break;
                        case 3:
                            Descripcion_4 = Descripcion;
                            ID_Paquete_4 = ID_Paquete;
                            break;
                    }
                }

                // Obtener Proveedores
                // Iterar sobre las filas del GridView
                string Nom_Empresa_1 = string.Empty, Direccion_1 = string.Empty, Observaciones_1 = string.Empty, Monto_Proveedor_1 = string.Empty, Monto_Proveedor_Letras_1 = string.Empty;
                string Hora_Solicitud_1 = string.Empty, Num_Unidad_1 = string.Empty, Responsable_Proveedor_1 = string.Empty;

                string Nom_Empresa_2 = string.Empty, Direccion_2 = string.Empty, Observaciones_2 = string.Empty;
                string Nom_Empresa_3 = string.Empty, Direccion_3 = string.Empty, Observaciones_3 = string.Empty;
                string Nom_Empresa_4 = string.Empty, Direccion_4 = string.Empty, Observaciones_4 = string.Empty;

                for (int i = 0; i < GrdProveedores.Rows.Count && i < 4; i++) // Máximo 4 filas
                {
                    GridViewRow row = GrdProveedores.Rows[i];

                    // Recuperar valores de las celdas
                    string Nom_Empresa = Server.HtmlDecode(Convert.ToString(row.Cells[2].Text));
                    string Hora_Solicitud = Server.HtmlDecode(Convert.ToString(row.Cells[3].Text));
                    string Monto_Proveedor = Server.HtmlDecode(Convert.ToString(row.Cells[7].Text));

                    // Quitar símbolo de moneda y comas, si vienen
                    Monto_Proveedor = Monto_Proveedor.Replace("$", "").Replace(",", "").Trim();

                    // Convertir a decimal
                    decimal importe;
                    string Monto_Proveedor_Letras = string.Empty;

                    if (decimal.TryParse(Monto_Proveedor, out importe))
                    {
                        Monto_Proveedor_Letras = NumeroALetrasConFraccionMN(importe);
                    }

                    string Direccion = Server.HtmlDecode(Convert.ToString(row.Cells[8].Text)) + " " + Server.HtmlDecode(Convert.ToString(row.Cells[9].Text)) + 
                        " " + Server.HtmlDecode(Convert.ToString(row.Cells[10].Text)) + "," + abrev_Colonia + " " + Server.HtmlDecode(Convert.ToString(row.Cells[13].Text))
                        + ", " + Server.HtmlDecode(Convert.ToString(row.Cells[12].Text)) + ", " + Server.HtmlDecode(Convert.ToString(row.Cells[11].Text)) + ", " + abrev_CodigoP + " " + Server.HtmlDecode(Convert.ToString(row.Cells[14].Text));          // Direccion

                    string Num_Unidad = Server.HtmlDecode(Convert.ToString(row.Cells[18].Text));
                    string Responsable_Proveedor = Server.HtmlDecode(Convert.ToString(row.Cells[19].Text));


                    string Observaciones = string.Empty;

                    // Asignar valores a las variables según el renglón
                    switch (i)
                    {
                        case 0:
                            Nom_Empresa_1 = Nom_Empresa;
                            Direccion_1 = Direccion;
                            Monto_Proveedor_1 = Monto_Proveedor;
                            Monto_Proveedor_Letras_1 = Monto_Proveedor_Letras;

                            Responsable_Proveedor_1 = Responsable_Proveedor;
                            Hora_Solicitud_1 = Hora_Solicitud;
                            Num_Unidad_1 = Num_Unidad;

                            Observaciones_1 = Observaciones;
                            break;
                        case 1:
                            Nom_Empresa_2 = Nom_Empresa;
                            Direccion_2 = Direccion;
                            Observaciones_2 = Observaciones;
                            break;
                        case 2:
                            Nom_Empresa_3 = Nom_Empresa;
                            Direccion_3 = Direccion;
                            Observaciones_3 = Observaciones;
                            break;
                        case 3:
                            Nom_Empresa_4 = Nom_Empresa;
                            Direccion_4 = Direccion;
                            Observaciones_4 = Observaciones;
                            break;
                    }
                }


                // Obtener Estudios Realizados
                // Iterar sobre las filas del GridView
                string Estudios_1 = string.Empty, Estudios_2 = string.Empty, Estudios_3 = string.Empty, Estudios_4 = string.Empty;

                for (int i = 0; i < GrdEstudios.Rows.Count && i < 4; i++) // Máximo 4 filas
                {
                    GridViewRow row = GrdEstudios.Rows[i];

                    // Recuperar valores de las celdas
                    string Estudios = Server.HtmlDecode(Convert.ToString(row.Cells[1].Text));       // Descripción

                    // Asignar valores a las variables según el renglón
                    switch (i)
                    {
                        case 0:
                            Estudios_1 = Estudios;
                            break;
                        case 1:
                            Estudios_2 = Estudios;
                            break;
                        case 2:
                            Estudios_3 = Estudios;
                            break;
                        case 3:
                            Estudios_4 = Estudios;
                            break;
                    }
                }


                // Obtener Diagnostico Final
                // Iterar sobre las filas del GridView
                string Desc_ICD_1 = string.Empty, Desc_ICD_2 = string.Empty, Desc_ICD_3 = string.Empty, Desc_ICD_4 = string.Empty;

                for (int i = 0; i < GrdDiagnosticoFinal.Rows.Count && i < 4; i++) // Máximo 4 filas
                {
                    GridViewRow row = GrdDiagnosticoFinal.Rows[i];

                    // Recuperar valores de las celdas
                    string Desc_ICD = Server.HtmlDecode(Convert.ToString(row.Cells[2].Text));       // Descripción

                    // Asignar valores a las variables según el renglón
                    switch (i)
                    {
                        case 0:
                            Desc_ICD_1 = Desc_ICD;
                            break;
                        case 1:
                            Desc_ICD_2 = Desc_ICD;
                            break;
                        case 2:
                            Desc_ICD_3 = Desc_ICD;
                            break;
                        case 3:
                            Desc_ICD_4 = Desc_ICD;
                            break;
                    }
                }

                // Tratamientos Realizados
                // Obtener Diagnostico Final
                // Iterar sobre las filas del GridView
                string Desc_CPT_1 = string.Empty, Desc_CPT_2 = string.Empty, Desc_CPT_3 = string.Empty, Desc_CPT_4 = string.Empty;

                for (int i = 0; i < GrdTratamientos.Rows.Count && i < 4; i++) // Máximo 4 filas
                {
                    GridViewRow row = GrdTratamientos.Rows[i];

                    // Recuperar valores de las celdas
                    string Desc_CPT = Server.HtmlDecode(Convert.ToString(row.Cells[2].Text));       // Descripción

                    // Asignar valores a las variables según el renglón
                    switch (i)
                    {
                        case 0:
                            Desc_CPT_1 = Desc_CPT;
                            break;
                        case 1:
                            Desc_CPT_2 = Desc_CPT;
                            break;
                        case 2:
                            Desc_CPT_3 = Desc_CPT;
                            break;
                        case 3:
                            Desc_CPT_4 = Desc_CPT;
                            break;
                    }
                }

                // Obtener Servicios Autorizados
                // Iterar sobre las filas del GridView
                string Nom_Servicio_1 = string.Empty, Desc_Servicio_1 = string.Empty;
                string Nom_Servicio_2 = string.Empty, Desc_Servicio_2 = string.Empty;
                string Nom_Servicio_3 = string.Empty, Desc_Servicio_3 = string.Empty;
                string Nom_Servicio_4 = string.Empty, Desc_Servicio_4 = string.Empty;

                for (int i = 0; i < GrdServicios.Rows.Count && i < 4; i++) // Máximo 4 filas
                {
                    GridViewRow row = GrdServicios.Rows[i];

                    // Recuperar valores de las celdas
                    string Nom_Servicio = Server.HtmlDecode(Convert.ToString(row.Cells[2].Text));       // Servicio
                    string Desc_Servicio = Server.HtmlDecode(Convert.ToString(row.Cells[3].Text));      // Descripción Servicio

                    // Asignar valores a las variables según el renglón
                    switch (i)
                    {
                        case 0:
                            Nom_Servicio_1 = Nom_Servicio;
                            Desc_Servicio_1 = Desc_Servicio;
                            break;
                        case 1:
                            Nom_Servicio_2 = Nom_Servicio;
                            Desc_Servicio_2 = Desc_Servicio;
                            break;
                        case 2:
                            Nom_Servicio_3 = Nom_Servicio;
                            Desc_Servicio_3 = Desc_Servicio;
                            break;
                        case 3:
                            Nom_Servicio_4 = Nom_Servicio;
                            Desc_Servicio_4 = Desc_Servicio;
                            break;
                    }
                }


                // Copiar la plantilla a un nuevo documento
                System.IO.File.Copy(plantillaPath, documentoGeneradoPath, true);

                // Abrir el documento generado
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documentoGeneradoPath, true))
                {
                    // Obtener el cuerpo del documento
                    DocumentFormat.OpenXml.Wordprocessing.Body body = wordDoc.MainDocumentPart.Document.Body;

                    // Buscar y reemplazar marcadores de posición con datos reales
                    ReplaceText(body, "Num_Referencia", TxtSubReferencia.Text);
                    ReplaceText(body, "Num_Poliza", TxtNumPoliza.Text);
                    ReplaceText(body, "Num_Siniestro", TxtNumSiniestro.Text);
                    ReplaceText(body, "Dom_Ocurrencia", Dom_Ocurrencia);
                    ReplaceText(body, "Fecha_Ingreso", TxtFechaIngreso.Text);       // en dd/MM/yyyy
                    ReplaceText(body, "Fecha_Vigencia", TxtFechaVigencia.Text);     // en dd/MM/yyyy

                    ReplaceText(body, "Linea_Ocurrencia", ddlLineaOcurrencia.SelectedItem.Text);
                    ReplaceText(body, "Estacion_Ocurrencia", ddlEstacionOcurrencia.SelectedItem.Text);

                    ReplaceText(body, "Fec_Solicitud", Fec_Solicitud);

                    ReplaceText(body, "Fec_Dia", fecOcurrencia.Day.ToString("00"));
                    ReplaceText(body, "Fec_Mes", fecOcurrencia.Month.ToString("00"));
                    ReplaceText(body, "Fec_Año", fecOcurrencia.Year.ToString());

                    ReplaceText(body, "Fec_IniVigencia", TxtFechaIniVigencia.Text);    // en dd/MM/yyyy
                    ReplaceText(body, "Fec_FinVigencia", TxtFechaFinVigencia.Text);    // en dd/MM/yyyy

                    ReplaceText(body, "Fec_Ocurrencia", FechaOcurrencia);               // en letras
                    ReplaceText(body, "Fecha_Ocurrencia", TxtFechaOcurrencia.Text);     // en dd/MM/yyyy
                    ReplaceText(body, "Hora_Ocurrencia", TxtHoraOcurrencia.Text);
                    ReplaceText(body, "Clasif_Lesiones", ddlTipoEvento.SelectedItem.Text);
                    ReplaceText(body, "Desc_Lesiones", TxtDescLesiones.Text);

                    ReplaceText(body, "Reaccion", TxtAlergias.Text);
                    ReplaceText(body, "Enfer_Previas", TxtEnfermedades.Text);
                    ReplaceText(body, "Medicamento", TxtMedicamentos.Text);
                    ReplaceText(body, "Alcohol", TxtAlcohol.Text);
                    ReplaceText(body, "Sustancias", TxtSustancias.Text);

                    if (ddlTpoAtencion.SelectedItem.Text == "Hospitalaria")
                    {
                        string Tpo_Ambulatoria = string.Empty;
                        ReplaceText(body, "Tpo_Ambulatoria", string.IsNullOrWhiteSpace(Tpo_Ambulatoria) ? "\u00A0" : Tpo_Ambulatoria);

                        ReplaceText(body, "Tpo_Hospital", "X");

                    } else if (ddlTpoAtencion.SelectedItem.Text == "Ambulatoria")
                    {
                        ReplaceText(body, "Tpo_Ambulatoria", "X");

                        string Tpo_Hospital = string.Empty;
                        ReplaceText(body, "Tpo_Hospital", string.IsNullOrWhiteSpace(Tpo_Hospital) ? "\u00A0" : Tpo_Hospital);
                    }

                    // Estudios_Realizados
                    ReplaceText(body, "Estudios_1", string.IsNullOrWhiteSpace(Estudios_1) ? "\u00A0" : Estudios_1);
                    ReplaceText(body, "Estudios_2", string.IsNullOrWhiteSpace(Estudios_2) ? "\u00A0" : Estudios_2);
                    ReplaceText(body, "Estudios_3", string.IsNullOrWhiteSpace(Estudios_3) ? "\u00A0" : Estudios_3);
                    ReplaceText(body, "Estudios_4", string.IsNullOrWhiteSpace(Estudios_4) ? "\u00A0" : Estudios_4);

                    // Diagnostico Final
                    ReplaceText(body, "Desc_ICD_1", string.IsNullOrWhiteSpace(Desc_ICD_1) ? "\u00A0" : Desc_ICD_1);
                    ReplaceText(body, "Desc_ICD_2", string.IsNullOrWhiteSpace(Desc_ICD_2) ? "\u00A0" : Desc_ICD_2);
                    ReplaceText(body, "Desc_ICD_3", string.IsNullOrWhiteSpace(Desc_ICD_3) ? "\u00A0" : Desc_ICD_3);
                    ReplaceText(body, "Desc_ICD_4", string.IsNullOrWhiteSpace(Desc_ICD_4) ? "\u00A0" : Desc_ICD_4);

                    // Tratamientos Realizados
                    ReplaceText(body, "Desc_CPT_1", string.IsNullOrWhiteSpace(Desc_CPT_1) ? "\u00A0" : Desc_CPT_1);
                    ReplaceText(body, "Desc_CPT_2", string.IsNullOrWhiteSpace(Desc_CPT_2) ? "\u00A0" : Desc_CPT_2);
                    ReplaceText(body, "Desc_CPT_3", string.IsNullOrWhiteSpace(Desc_CPT_3) ? "\u00A0" : Desc_CPT_3);
                    ReplaceText(body, "Desc_CPT_4", string.IsNullOrWhiteSpace(Desc_CPT_4) ? "\u00A0" : Desc_CPT_4);


                    // Servicios Autorizados
                    ReplaceText(body, "Nom_Servicio_1", string.IsNullOrWhiteSpace(Nom_Servicio_1) ? "\u00A0" : Nom_Servicio_1);
                    ReplaceText(body, "Desc_Servicio_1", string.IsNullOrWhiteSpace(Desc_Servicio_1) ? "\u00A0" : Desc_Servicio_1);

                    ReplaceText(body, "Nom_Servicio_2", string.IsNullOrWhiteSpace(Nom_Servicio_2) ? "\u00A0" : Nom_Servicio_2);
                    ReplaceText(body, "Desc_Servicio_2", string.IsNullOrWhiteSpace(Desc_Servicio_2) ? "\u00A0" : Desc_Servicio_2);

                    ReplaceText(body, "Nom_Servicio_3", string.IsNullOrWhiteSpace(Nom_Servicio_3) ? "\u00A0" : Nom_Servicio_3);
                    ReplaceText(body, "Desc_Servicio_3", string.IsNullOrWhiteSpace(Desc_Servicio_3) ? "\u00A0" : Desc_Servicio_3);

                    ReplaceText(body, "Nom_Servicio_4", string.IsNullOrWhiteSpace(Nom_Servicio_4) ? "\u00A0" : Nom_Servicio_4);
                    ReplaceText(body, "Desc_Servicio_4", string.IsNullOrWhiteSpace(Desc_Servicio_4) ? "\u00A0" : Desc_Servicio_4);

                    ReplaceText(body, "Observacion_Medica", TxtObservaciones_PA.Text);

                    ReplaceText(body, "Hospital_Atension", ddlHospitales.SelectedItem.Text);
                    ReplaceText(body, "Observaciones_Atencion", TxtObservaciones_DA.Text);

                    ReplaceText(body, "Nom_Completo", TxtNomLesionado.Text);
                    ReplaceText(body, "Fec_Nacimiento", Fec_Nacimiento);            // en letras
                    ReplaceText(body, "FecNacimiento", TxtFecNacimiento.Text);      // en dd/MM/yyyy
                    ReplaceText(body, "Edad_Responsable", TxtEdadLesionado.Text);
                    ReplaceText(body, "Sexo_Lesionado", TxtSexo.Text);
                    ReplaceText(body, "Tel_Lesionado", TxtTelLesionado.Text);
                    ReplaceText(body, "Email_Lesionado", TxtEmailLesionado.Text);
                    ReplaceText(body, "RFC_Lesionado", TxtRFC_Lesionado.Text);
                    ReplaceText(body, "Dom_Lesionado", Dom_Lesionado);

                    ReplaceText(body, "Nom_Responsable", TxtNomResponsable.Text);
                    ReplaceText(body, "Parentesco_Responsable", TxtParentesco.Text);
                 // ReplaceText(body, "Edad_Responsable", TxtEdadResponsable.Text);
                    ReplaceText(body, "Edad_Parentesco", TxtEdadResponsable.Text);
                    ReplaceText(body, "Tel_Responsable", TxtTelResponsable.Text);
                    ReplaceText(body, "Email_Responsable", TxtEmailResponsable.Text);
                    ReplaceText(body, "RFC_Responsable", TxtRFC_Responsable.Text);
                    ReplaceText(body, "Domicilio_Responsable", Dom_Responsable);

                    ReplaceText(body, "Monto_Autorizado", TxtMontoAutorizado.Text);

                    // Quitar símbolo de moneda y comas, si vienen
                    string Monto_Autorizado = TxtMontoAutorizado.Text.Replace("$", "").Replace(",", "").Trim();

                    // Convertir a decimal
                    decimal importe;
                    string Monto_Autorizado_Letras = string.Empty;

                    if (decimal.TryParse(Monto_Autorizado, out importe))
                    {
                        Monto_Autorizado_Letras = NumeroALetrasConFraccionMN(importe);
                    }

                    ReplaceText(body, "Monto_Letras_Autorizado", Monto_Autorizado_Letras);

                    ReplaceText(body, "Ate_Diagnostico", TxtDiagnostico.Text);
                    ReplaceText(body, "Desc_Lesiones", TxtDescLesiones.Text);
                    ReplaceText(body, "Tratamiento_Paquete", string.Empty);

                    // SOLICITUD DE SERVICIO (Tratamiento/Paquete)
                    ReplaceText(body, "Descripcion_1", Descripcion_1 ?? " ");
                    ReplaceText(body, "ID_Paquete_1", ID_Paquete_1);

                    ReplaceText(body, "Descripcion_2", Descripcion_2 ?? " ");
                    ReplaceText(body, "ID_Paquete_2", ID_Paquete_2);

                    ReplaceText(body, "Descripcion_3", Descripcion_3 ?? " ");
                    ReplaceText(body, "ID_Paquete_3", ID_Paquete_3);

                    ReplaceText(body, "Descripcion_4", Descripcion_4 ?? " ");
                    ReplaceText(body, "ID_Paquete_4", ID_Paquete_4);

                    // CENTRO MÉDICO DE REFERENCIA
                    ReplaceText(body, "Nom_Empresa_1", string.IsNullOrWhiteSpace(Nom_Empresa_1) ? "\u00A0" : Nom_Empresa_1);
                    ReplaceText(body, "Direccion_1", string.IsNullOrWhiteSpace(Direccion_1) ? "\u00A0" : Direccion_1);
                    ReplaceText(body, "Observaciones_1", string.IsNullOrWhiteSpace(Observaciones_1) ? "\u00A0" : Observaciones_1);

                    ReplaceText(body, "Proveedor_Solicitado", Nom_Empresa_1);
                    ReplaceText(body, "Responsable_Proveedor", Responsable_Proveedor_1);
                    ReplaceText(body, "Hora_Solicitud", Hora_Solicitud_1);
                    ReplaceText(body, "Num_Unidad", Num_Unidad_1);
                    ReplaceText(body, "Monto_Proveedor", Monto_Proveedor_1);
                    ReplaceText(body, "Monto_Letras_Proveedor", Monto_Proveedor_Letras_1);

                    ReplaceText(body, "Nom_Empresa_2", string.IsNullOrWhiteSpace(Nom_Empresa_2) ? "\u00A0" : Nom_Empresa_2);
                    ReplaceText(body, "Direccion_2", Direccion_2);
                    ReplaceText(body, "Observaciones_2", Observaciones_2);

                    ReplaceText(body, "Nom_Empresa_3", string.IsNullOrWhiteSpace(Nom_Empresa_3) ? "\u00A0" : Nom_Empresa_3);
                    ReplaceText(body, "Direccion_3", Direccion_3);
                    ReplaceText(body, "Observaciones_3", Observaciones_3);

                    ReplaceText(body, "Nom_Empresa_4", string.IsNullOrWhiteSpace(Nom_Empresa_4) ? "\u00A0" : Nom_Empresa_4);
                    ReplaceText(body, "Direccion_4", Direccion_4);
                    ReplaceText(body, "Observaciones_4", Observaciones_4);

                    // Guardar los cambios
                    wordDoc.MainDocumentPart.Document.Save();
                }

                LblMessage.Text = "El documento, se ha generado correctamente";
                this.mpeMensaje.Show();

                // Descargar el documento generado
                // Session["sFileName"] = "InformePreliminar.docx";

                Session["sFileName"] = Nom_Documento;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AbrirDescarga", string.Format("window.open('Descargas.aspx');"), true);

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        private void ReplaceText(DocumentFormat.OpenXml.Wordprocessing.Body body, string placeholder, string newText)
        {
            foreach (var textElement in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
            {
                if (textElement.Text.Trim().Contains(placeholder))
                {
                    textElement.Text = textElement.Text.Replace(placeholder, newText);
                }
            }
        }

        public static string NumeroALetrasConFraccionMN(decimal numero)
        {
            string letras = ConvertirNumero((int)Math.Floor(numero)).Trim();
            int centavos = (int)Math.Round((numero - Math.Floor(numero)) * 100);

            return $"{letras} PESOS {centavos:D2}/100 M.N.";
        }

        private static string ConvertirNumero(int numero)
        {
            string[] unidades = { "", "UNO", "DOS", "TRES", "CUATRO", "CINCO", "SEIS", "SIETE", "OCHO", "NUEVE" };
            string[] decenas = { "", "DIEZ", "VEINTE", "TREINTA", "CUARENTA", "CINCUENTA", "SESENTA", "SETENTA", "OCHENTA", "NOVENTA" };
            string[] centenas = { "", "CIENTO", "DOSCIENTOS", "TRESCIENTOS", "CUATROCIENTOS", "QUINIENTOS", "SEISCIENTOS", "SETECIENTOS", "OCHOCIENTOS", "NOVECIENTOS" };

            if (numero == 0)
                return "CERO";

            if (numero < 10)
                return unidades[numero];

            if (numero < 20)
            {
                switch (numero)
                {
                    case 10: return "DIEZ";
                    case 11: return "ONCE";
                    case 12: return "DOCE";
                    case 13: return "TRECE";
                    case 14: return "CATORCE";
                    case 15: return "QUINCE";
                    default: return "DIECI" + unidades[numero - 10];
                }
            }

            if (numero < 30)
            {
                if (numero == 20) return "VEINTE";
                return "VEINTI" + unidades[numero - 20];
            }

            if (numero < 100)
            {
                int decena = numero / 10;
                int unidad = numero % 10;
                return decenas[decena] + (unidad > 0 ? " Y " + unidades[unidad] : "");
            }

            if (numero < 1000)
            {
                int centena = numero / 100;
                int resto = numero % 100;
                if (numero == 100) return "CIEN";
                return centenas[centena] + (resto > 0 ? " " + ConvertirNumero(resto) : "");
            }

            if (numero < 1000000)
            {
                int miles = numero / 1000;
                int resto = numero % 1000;
                string milesTexto = miles == 1 ? "MIL" : ConvertirNumero(miles) + " MIL";
                return milesTexto + (resto > 0 ? " " + ConvertirNumero(resto) : "");
            }

            return "";
        }


        protected void ddlServicios_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlTpoAtencion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void habilitar_controles()
        {
            // ** CONFIGURACIÓN DE LA RECLAMACIÓN
            ddlTpoAsegurado.Enabled = true;
            ddlEstSiniestro.Enabled = true;
            ddlConclusion.Enabled = true;

            // Diagnostico Final
            TxtBuscarICD.Enabled = true;
            BtnICD.Enabled = true;

            // Tratamientos Realizados
            TxtBuscarCPT.Enabled = true;
            BtnCPT.Enabled = true;

            //Estudios Realizados
            TxtEstudiosRealizados.Enabled = true;

            // Datos de Proveedor
            ddlTpoServicio.Enabled = true;
            BtnProveedor.Enabled = true;
            TxtHoraSolicitud.Enabled = true;
            TxtHoraArribo.Enabled = true;
            TxtHoraSalida.Enabled = true;
            TxtHoraLlegada.Enabled = true;

            TxtProvMontoAutorizado.Enabled = true;
            TxtNumUnidad.Enabled = true;
            TxtResponsable.Enabled = true;

            //Servicio Autorizado
            ddlServicios.Enabled = true;
            TxtFechaServicio.Enabled = true;
            TxtHoraServicio.Enabled = true;
            TxtDescServicio.Enabled = true;

            // Paquetes Médicos
            ddlInstituciones.Enabled = true;
            ddlPaquetes_MD.Enabled = true;

            TxtIdPaquete.Enabled = true;
            TxtMontoUtilizado.Enabled = true;
            TxtMontoRestante.Enabled = true;
            TxtMontoSuperado.Enabled = true;
            TxtObservaciones_PM.Enabled = true;

        }
        public void habilitar_control_servicios()
        {
            ddlServicios.SelectedValue = "0";

            ddlServicios.Enabled = true;
            TxtFechaServicio.Enabled = true;
            TxtHoraServicio.Enabled = true;
            TxtDescServicio.Enabled = true;

            TxtFechaServicio.Text = string.Empty;
            TxtHoraServicio.Text = "00:00";
            TxtDescServicio.Text = string.Empty;

            btnEditarPnl14.Enabled = false;
        }

        public void habilitar_control_proveedores()
        {
            ddlTpoServicio.Enabled = true;

            TxtHoraSolicitud.Enabled = true;
            TxtHoraSolicitud.Text = "00:00";

            TxtHoraArribo.Enabled = true;
            TxtHoraArribo.Text = "00:00";

            TxtHoraSalida.Enabled = true;
            TxtHoraSalida.Text = "00:00";

            TxtHoraLlegada.Enabled = true;
            TxtHoraLlegada.Text = "00:00";

            TxtProvMontoAutorizado.Enabled = true;
            TxtProvMontoAutorizado.Text = "0.00";
            TxtNumUnidad.Enabled = true;
            TxtResponsable.Enabled = true;

            btnEditarPnl7.Enabled = false;
        }

        public void habilitar_control_paquetes()
        {
            ddlInstituciones.SelectedValue = "0";

            ddlPaquetes_MD.Items.Clear();
            ddlPaquetes_MD.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

            ddlInstituciones.Enabled = true;
            ddlPaquetes_MD.Enabled = true;

            TxtIdPaquete.Enabled = true;
            //TxtMontoMinimo.Enabled = true;
            //TxtMontoMaximo.Enabled = true;
            TxtMontoUtilizado.Enabled = true;
            TxtMontoRestante.Enabled = true;
            TxtMontoSuperado.Enabled = true;
            TxtObservaciones_PM.Enabled = true;

            TxtIdPaquete.Text = string.Empty;
            TxtMontoMinimo.Text = string.Empty;
            TxtMontoMaximo.Text = string.Empty;
            TxtMontoUtilizado.Text = string.Empty;
            TxtMontoRestante.Text = string.Empty;
            TxtMontoSuperado.Text = string.Empty;
            TxtObservaciones_PM.Text = string.Empty;

            btnEditarPnl16.Enabled = false;
        }

        protected void GrdServicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdServicios.PageIndex = e.NewPageIndex;
                GetDatos_Servicios();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdServicios_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdServicios_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdServicios_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdServicios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdServicios, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Width = Unit.Pixel(300);     // Servicio
                e.Row.Cells[3].Width = Unit.Pixel(150);     // FechaServicio
                e.Row.Cells[4].Width = Unit.Pixel(150);     // Hora_Servicio
                e.Row.Cells[5].Width = Unit.Pixel(800);     // Descripción
                e.Row.Cells[6].Width = Unit.Pixel(50);      // Editar
                e.Row.Cells[7].Width = Unit.Pixel(50);      // Eliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;     // IdConsecutivo
                e.Row.Cells[1].Visible = false;     // IdServicio
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;     // IdConsecutivo
                e.Row.Cells[1].Visible = false;     // IdServicio
            }
        }


        protected void BtnCPT_Click(object sender, EventArgs e)
        {
            CargarDatos_CPT(TxtBuscarCPT.Text.Trim());
            mpeNewCPT.Show();
        }

        protected void BtnICD_Click(object sender, EventArgs e)
        {
            CargarDatos_ICD(TxtBuscarICD.Text.Trim());
            mpeNewICD.Show();
        }

        private void CargarDatos_CPT(string filtro = "")
        {
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT Cve_CPT, Desc_CPT FROM ITM_30 WHERE Cve_CPT <> 'CPT' AND IdStatus = 1";

            if (!string.IsNullOrEmpty(filtro))
            {
                // Escapar comillas simples para evitar errores y reducir riesgo de SQL injection
                filtro = filtro.Replace("'", "''");
                strQuery += $" AND (Cve_CPT LIKE '%{filtro}%' OR Desc_CPT LIKE '%{filtro}%')";
            }

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            if (dt.Rows.Count == 0)
            {
                GrdCPT.ShowHeaderWhenEmpty = true;
                GrdCPT.EmptyDataText = "No hay resultados.";
            }

            GrdCPT.DataSource = dt;
            GrdCPT.DataBind();

            dbConn.Close();
        }


        private void CargarDatos_ICD(string filtro = "")
        {

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT Cve_ICD, Desc_ICD FROM ITM_31 WHERE IdStatus = 1";

            if (!string.IsNullOrEmpty(filtro))
            {
                // Escapar comillas simples para evitar errores y reducir riesgo de SQL injection
                filtro = filtro.Replace("'", "''");
                strQuery += $" AND (Cve_ICD LIKE '%{filtro}%' OR Desc_ICD LIKE '%{filtro}%')";
            }

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            if (dt.Rows.Count == 0)
            {
                GrdICD.ShowHeaderWhenEmpty = true;
                GrdICD.EmptyDataText = "No hay resultados.";
            }

            GrdICD.DataSource = dt;
            GrdICD.DataBind();

            dbConn.Close();

        }

        private void CargarDatos_Proveedor(string filtro = "")
        {

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT Tpo_Servicio, Nom_Empresa, Calle, Num_Exterior, Num_Interior, " +
                              "       Estado, Delegacion, Colonia, Codigo_Postal, Email_Empresa, " +
                              "       Tel_Contacto_1, Tel_Contacto_2 " +
                              "  FROM ITM_34 WHERE IdStatus = 1";

            if (!string.IsNullOrEmpty(filtro))
            {
                // Escapar comillas simples para evitar errores y reducir riesgo de SQL injection
                filtro = filtro.Replace("'", "''");
                strQuery += $" AND (Tpo_Servicio LIKE '%{filtro}%') ";
            }

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            if (dt.Rows.Count == 0)
            {
                GrdDatosProveedor.ShowHeaderWhenEmpty = true;
                GrdDatosProveedor.EmptyDataText = "No hay resultados.";
            }

            GrdDatosProveedor.DataSource = dt;
            GrdDatosProveedor.DataBind();

            dbConn.Close();

        }
        protected void GrdICD_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GrdICD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdICD.PageIndex = e.NewPageIndex;
                CargarDatos_ICD(TxtBuscarICD.Text.Trim());
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdCPT_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GrdCPT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdCPT.PageIndex = e.NewPageIndex;
                CargarDatos_CPT(TxtBuscarCPT.Text.Trim());
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void btnAgregar_ICD_Click(object sender, EventArgs e)
        {
            Add_ITM_32();
        }

        protected void btnClose_ICD_Click(object sender, EventArgs e)
        {

        }

        protected void btnAgregar_CPT_Click(object sender, EventArgs e)
        {
            Add_ITM_33();
        }

        protected void btnClose_CPT_Click(object sender, EventArgs e)
        {

        }

        protected void GrdDiagnosticoFinal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdDiagnosticoFinal.PageIndex = e.NewPageIndex;
                GetDiagnosticoFinal(TxtSubReferencia.Text);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdDiagnosticoFinal_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdDiagnosticoFinal_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdDiagnosticoFinal_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdDiagnosticoFinal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdDiagnosticoFinal, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Width = Unit.Pixel(150);     // Cve_ICD
                e.Row.Cells[2].Width = Unit.Pixel(1300);    // Desc_ICD
                e.Row.Cells[5].Width = Unit.Pixel(50);      // Eliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;     // Id_ICD
                e.Row.Cells[3].Visible = false;     // Referencia
                e.Row.Cells[4].Visible = false;     // SubReferencia
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;     // Id_ICD
                e.Row.Cells[3].Visible = false;     // Referencia
                e.Row.Cells[4].Visible = false;     // SubReferencia
            }
        }

        protected void ImgEliminar_ICD_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            Variables.wCPT = false;
            Variables.wICD = true;
            Variables.wEstudios = false;
            Variables.wServicios = false;
            Variables.wProveedor = false;
            Variables.wPaquete = false;

            LblMessage_1.Text = "¿Desea eliminar la clave ICD?";
            mpeMensaje_1.Show();
        }

        protected void ImgEliminar_CPT_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            Variables.wICD = false;
            Variables.wCPT = true;
            Variables.wEstudios = false;
            Variables.wServicios = false;
            Variables.wProveedor = false;
            Variables.wPaquete = false;

            LblMessage_1.Text = "¿Desea eliminar la clave CPT?";
            mpeMensaje_1.Show();
        }

        protected void GrdTratamientos_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdTratamientos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdTratamientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdTratamientos.PageIndex = e.NewPageIndex;
                GetTratamientosRealizados(TxtSubReferencia.Text);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdTratamientos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdTratamientos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdTratamientos, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Width = Unit.Pixel(150);     // Cve_CPT
                e.Row.Cells[2].Width = Unit.Pixel(1300);    // Desc_CPT
                e.Row.Cells[5].Width = Unit.Pixel(50);      // Eliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;     // Id_CPT
                e.Row.Cells[3].Visible = false;     // Referencia
                e.Row.Cells[4].Visible = false;     // SubReferencia
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;     // Id_CPT
                e.Row.Cells[3].Visible = false;     // Referencia
                e.Row.Cells[4].Visible = false;     // SubReferencia
            }
        }

        protected void BtnAnularPnl9_Click(object sender, EventArgs e)
        {
            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;

            // habilitar_controles();

            // Inicializar Controles
            TxtEstudiosRealizados.Text = string.Empty;
            TxtEstudiosRealizados.ReadOnly = false;

            btnEditarPnl9.Visible = true;
            btnActualizarPnl9.Visible = false;
            BtnAnularPnl9.Visible = false;

            btnEditarPnl9.Enabled = false;
            BtnAgregarPnl9.Enabled = true;
        }

        protected void btnEditarPnl9_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl9
            TxtEstudiosRealizados.ReadOnly = false;

            btnEditarPnl9.Visible = false;
            btnActualizarPnl9.Visible = true;
            BtnAnularPnl9.Visible = true;
        }

        protected void btnActualizarPnl9_Click(object sender, EventArgs e)
        {
            Actualizar_ITM_36();

            // ddlTpoAsegurado.Enabled = true;
            // ddlEstSiniestro.Enabled = true;
            // ddlConclusion.Enabled = true;
            // habilitar_controles();

            // inicializar controles.
            TxtEstudiosRealizados.Text = string.Empty;

            btnEditarPnl9.Enabled = false;
            BtnAgregarPnl9.Enabled = true;

            btnEditarPnl9.Visible = true;
            btnActualizarPnl9.Visible = false;
            BtnAnularPnl9.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios correctamente";
            this.mpeMensaje.Show();
        }

        protected void BtnAgregarPnl9_Click(object sender, EventArgs e)
        {
            if (TxtEstudiosRealizados.Text == "" || TxtEstudiosRealizados.Text == null)
            {
                LblMessage.Text = "Capturar Estudios Realizados";
                mpeMensaje.Show();
                return;
            }

            string sDescripcion = TxtEstudiosRealizados.Text;

            int Envio_Ok = Add_tbEstudio(sDescripcion);

            if (Envio_Ok == 0)
            {

                // inicializar controles
                TxtEstudiosRealizados.Text = string.Empty;

                GetDatos_Estudios();
            }
        }

        protected void ImgEstudios_Add_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            TxtEstudiosRealizados.Text = Server.HtmlDecode(Convert.ToString(GrdEstudios.Rows[index].Cells[1].Text));

            TxtEstudiosRealizados.ReadOnly = true;

            BtnAnularPnl9.Visible = true;
            btnEditarPnl9.Enabled = true;
            BtnAgregarPnl9.Enabled = false;
        }

        protected void ImgEstudios_Del_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            Variables.wCPT = false;
            Variables.wICD = false;
            Variables.wEstudios = true;
            Variables.wServicios = false;
            Variables.wProveedor = false;
            Variables.wPaquete = false;


            LblMessage_1.Text = "¿Desea eliminar el estudio realizado?";
            mpeMensaje_1.Show();
        }

        protected void GrdEstudios_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdEstudios.PageIndex = e.NewPageIndex;
                GetDatos_Estudios();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdEstudios_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdEstudios_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdEstudios_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdEstudios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdEstudios, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = Unit.Pixel(50);      // Id_Estudio
                e.Row.Cells[1].Width = Unit.Pixel(1000);    // Desc_Estudio
                e.Row.Cells[2].Width = Unit.Pixel(50);      // Editar
                e.Row.Cells[3].Width = Unit.Pixel(50);      // Eliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;     // Id_Estudio
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;     // Id_Estudio
            }
        }

        protected void btnShowPanel10_Click(object sender, EventArgs e)
        {
            pnl10.Visible = !pnl10.Visible;   // Cambia la visibilidad del Panel 10 al contrario de su estado actual

            if (pnl10.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel10.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl10.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel10.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl10.Visible = false;
            }
        }

        protected void btnShowPanel11_Click(object sender, EventArgs e)
        {
            pnl11.Visible = !pnl11.Visible;   // Cambia la visibilidad del Panel 11 al contrario de su estado actual

            if (pnl11.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel11.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl11.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel11.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl11.Visible = false;
            }
        }

        protected void btnShowPanel12_Click(object sender, EventArgs e)
        {
            pnl12.Visible = !pnl12.Visible;   // Cambia la visibilidad del Panel 12 al contrario de su estado actual

            if (pnl12.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel12.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl12.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel12.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl12.Visible = false;
            }
        }

        protected void btnShowPanel16_Click(object sender, EventArgs e)
        {
            pnl16.Visible = !pnl16.Visible;   // Cambia la visibilidad del Panel 16 al contrario de su estado actual

            if (pnl16.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel16.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl16.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel16.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl16.Visible = false;
            }
        }

        protected void GrdProveedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdProveedores.PageIndex = e.NewPageIndex;
                GetDatos_Proveedor();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdProveedores_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdProveedores_PreRender(object sender, EventArgs e)
        {

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
                e.Row.Cells[2].Width = Unit.Pixel(350);     // Nom_Empresa
                e.Row.Cells[3].Width = Unit.Pixel(150);     // Hora_Solicitud
                e.Row.Cells[4].Width = Unit.Pixel(150);     // Hora_Arribo
                e.Row.Cells[5].Width = Unit.Pixel(150);     // Hora_Salida
                e.Row.Cells[6].Width = Unit.Pixel(150);     // Hora_Llegada
                e.Row.Cells[7].Width = Unit.Pixel(150);     // Monto_Autorizado

                e.Row.Cells[20].Width = Unit.Pixel(50);     // Editar
                e.Row.Cells[21].Width = Unit.Pixel(50);     // Eliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;     // Id_Proveedor
                e.Row.Cells[1].Visible = false;     // Tpo_Servicio
                e.Row.Cells[8].Visible = false;     // Calle
                e.Row.Cells[9].Visible = false;     // Num_Exterior
                e.Row.Cells[10].Visible = false;    // Num_Interior
                e.Row.Cells[11].Visible = false;    // Estado
                e.Row.Cells[12].Visible = false;    // Delegacion
                e.Row.Cells[13].Visible = false;    // Colonia
                e.Row.Cells[14].Visible = false;    // Codigo_Postal
                e.Row.Cells[15].Visible = false;    // Tel_Contacto_1
                e.Row.Cells[16].Visible = false;    // Tel_Contacto_2
                e.Row.Cells[17].Visible = false;    // Email_Empresa
                e.Row.Cells[18].Visible = false;    // Num_Unidad
                e.Row.Cells[19].Visible = false;    // Responsable
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;     // Id_Proveedor
                e.Row.Cells[1].Visible = false;     // Tpo_Servicio
                e.Row.Cells[8].Visible = false;     // Calle
                e.Row.Cells[9].Visible = false;     // Num_Exterior
                e.Row.Cells[10].Visible = false;    // Num_Interior
                e.Row.Cells[11].Visible = false;    // Estado
                e.Row.Cells[12].Visible = false;    // Delegacion
                e.Row.Cells[13].Visible = false;    // Colonia
                e.Row.Cells[14].Visible = false;    // Codigo_Postal
                e.Row.Cells[15].Visible = false;    // Tel_Contacto_1
                e.Row.Cells[16].Visible = false;    // Tel_Contacto_2
                e.Row.Cells[17].Visible = false;    // Email_Empresa
                e.Row.Cells[18].Visible = false;    // Num_Unidad
                e.Row.Cells[19].Visible = false;    // Responsable
            }
        }

        protected void ddlMunicipioProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlEstadoProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sEstado = ddlEstadoProveedor.SelectedValue;
            GetMunicipiosProveedor(sEstado);
        }

        protected void ddlTpoServicio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnProveedor_Click(object sender, EventArgs e)
        {
            CargarDatos_Proveedor(ddlTpoServicio.SelectedValue);
            mpeNewProveedor.Show();
        }

        protected void btnAgregar_Proveedor_Click(object sender, EventArgs e)
        {
            Datos_Seleccionados_Proveedor();
        }

        protected void btnClose_Proveedor_Click(object sender, EventArgs e)
        {

        }

        protected void GrdDatosProveedor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdDatosProveedor.PageIndex = e.NewPageIndex;
                CargarDatos_Proveedor(ddlTpoServicio.SelectedValue);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdDatosProveedor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdDatosProveedor, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Width = Unit.Pixel(300);     // Nom_Empresa
                e.Row.Cells[3].Width = Unit.Pixel(300);     // Email_Empresa
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;     // Tpo_Servicio
                e.Row.Cells[4].Visible = false;     // Calle
                e.Row.Cells[5].Visible = false;     // Num_Exterior
                e.Row.Cells[6].Visible = false;     // Num_Interior
                e.Row.Cells[7].Visible = false;     // Estado
                e.Row.Cells[8].Visible = false;     // Delegacion
                e.Row.Cells[9].Visible = false;     // Colonia
                e.Row.Cells[10].Visible = false;    // Codigo_Postal
                e.Row.Cells[11].Visible = false;    // Tel_Contacto_1
                e.Row.Cells[12].Visible = false;    // Tel_Contacto_2
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;     // Tpo_Servicio
                e.Row.Cells[4].Visible = false;     // Calle
                e.Row.Cells[5].Visible = false;     // Num_Exterior
                e.Row.Cells[6].Visible = false;     // Num_Interior
                e.Row.Cells[7].Visible = false;     // Estado
                e.Row.Cells[8].Visible = false;     // Delegacion
                e.Row.Cells[9].Visible = false;     // Colonia
                e.Row.Cells[10].Visible = false;    // Codigo_Postal
                e.Row.Cells[11].Visible = false;    // Tel_Contacto_1
                e.Row.Cells[12].Visible = false;    // Tel_Contacto_2
            }
        }

        protected void ddlInstituciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Inicializar controles
            TxtIdPaquete.Text = string.Empty;
            TxtMontoMinimo.Text = string.Empty;
            TxtMontoMaximo.Text = string.Empty;
            TxtMontoUtilizado.Text = string.Empty;
            TxtMontoRestante.Text = string.Empty;
            TxtMontoSuperado.Text = string.Empty;
            TxtObservaciones_PM.Text = string.Empty;

            int iInstitucion = Convert.ToInt16(ddlInstituciones.SelectedValue);
            GetPaqueteMedico(iInstitucion);
        }

        protected void ddlPaquetes_MD_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iPaquete = Convert.ToInt16(ddlPaquetes_MD.SelectedValue);
            GetPaqueteMontos(iPaquete);
        }

        protected void GrdPaqueteMedico_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdPaqueteMedico.PageIndex = e.NewPageIndex;
                GetDatos_Paquetes();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdPaqueteMedico_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdPaqueteMedico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdPaqueteMedico, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Width = Unit.Pixel(300);     // Institución Hospitalaria
                e.Row.Cells[4].Width = Unit.Pixel(150);     // ID_Paquete
                e.Row.Cells[5].Width = Unit.Pixel(200);     // Monto_Minimo
                e.Row.Cells[6].Width = Unit.Pixel(200);     // Monto_Maximo
                e.Row.Cells[7].Width = Unit.Pixel(200);     // Monto_Utilizado
                e.Row.Cells[8].Width = Unit.Pixel(200);     // Monto_Restante
                e.Row.Cells[9].Width = Unit.Pixel(200);     // Monto_Superado
                e.Row.Cells[11].Width = Unit.Pixel(50);     // Editar
                e.Row.Cells[12].Width = Unit.Pixel(50);     // Eliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;     // Id_Consecutivo
                e.Row.Cells[1].Visible = false;     // Id_Institucion
                e.Row.Cells[2].Visible = false;     // Id_Paquete_Medico
                e.Row.Cells[10].Visible = false;    // Observaciones
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;     // Id_Consecutivo
                e.Row.Cells[1].Visible = false;     // Id_Institucion
                e.Row.Cells[2].Visible = false;     // Id_Paquete_Medico
                e.Row.Cells[10].Visible = false;    // Observaciones
            }
        }

        protected void GrdPaqueteMedico_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdPaqueteMedico_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void ImgPaquete_Add_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            ddlInstituciones.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdPaqueteMedico.Rows[index].Cells[1].Text));
            
            // Disparar el evento SelectedIndexChanged manualmente
            ddlInstituciones_SelectedIndexChanged(ddlInstituciones, EventArgs.Empty);

            ddlPaquetes_MD.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdPaqueteMedico.Rows[index].Cells[2].Text));
            TxtIdPaquete.Text = Server.HtmlDecode(Convert.ToString(GrdPaqueteMedico.Rows[index].Cells[4].Text));

            TxtMontoMinimo.Text = Server.HtmlDecode(Convert.ToString(GrdPaqueteMedico.Rows[index].Cells[5].Text));
            TxtMontoMaximo.Text = Server.HtmlDecode(Convert.ToString(GrdPaqueteMedico.Rows[index].Cells[6].Text));
            TxtMontoUtilizado.Text = Server.HtmlDecode(Convert.ToString(GrdPaqueteMedico.Rows[index].Cells[7].Text));
            TxtMontoRestante.Text = Server.HtmlDecode(Convert.ToString(GrdPaqueteMedico.Rows[index].Cells[8].Text));
            TxtMontoSuperado.Text = Server.HtmlDecode(Convert.ToString(GrdPaqueteMedico.Rows[index].Cells[9].Text));
            TxtObservaciones_PM.Text = Server.HtmlDecode(Convert.ToString(GrdPaqueteMedico.Rows[index].Cells[10].Text));

            ddlInstituciones.Enabled = false;
            ddlPaquetes_MD.Enabled = false;

            TxtIdPaquete.ReadOnly = true;
            TxtMontoUtilizado.ReadOnly = true;
            TxtMontoRestante.ReadOnly = true;
            TxtMontoSuperado.ReadOnly = true;
            TxtObservaciones_PM.ReadOnly = true;

            BtnAnularPnl16.Visible = true;
            btnEditarPnl16.Enabled = true;
            BtnAgregarPnl16.Enabled = false;
        }

        protected void ImgPaquete_Del_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            Variables.wCPT = false;
            Variables.wICD = false;
            Variables.wEstudios = false;
            Variables.wServicios = false;
            Variables.wProveedor = false;
            Variables.wPaquete = true;

            LblMessage_1.Text = "¿Desea eliminar el paquete médico?";
            mpeMensaje_1.Show();
        }

        protected void Actualizar_ITM_38()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int index = Variables.wRenglon;

                int Id_Consecutivo = Convert.ToInt32(GrdPaqueteMedico.Rows[index].Cells[0].Text);
                int Id_Institucion = Convert.ToInt32(GrdPaqueteMedico.Rows[index].Cells[1].Text);
                int Id_Paquete_Medico = Convert.ToInt32(GrdPaqueteMedico.Rows[index].Cells[2].Text);

                decimal MontoUtilizado = 0;

                if (!string.IsNullOrWhiteSpace(TxtMontoUtilizado.Text.Trim()))
                {
                    decimal.TryParse(TxtMontoUtilizado.Text.Trim(), out MontoUtilizado);
                }

                decimal MontoRestante = 0;

                if (!string.IsNullOrWhiteSpace(TxtMontoRestante.Text.Trim()))
                {
                    decimal.TryParse(TxtMontoRestante.Text.Trim(), out MontoRestante);
                }

                decimal MontoSuperado = 0;

                if (!string.IsNullOrWhiteSpace(TxtMontoSuperado.Text.Trim()))
                {
                    decimal.TryParse(TxtMontoSuperado.Text.Trim(), out MontoSuperado);
                }

                // Actualizar registro(s) tablas (ITM_38)
                string strQuery = "UPDATE ITM_38 " +
                                  "   SET ID_Paquete = '" + TxtIdPaquete.Text.Trim() + "', " +
                                  "       Monto_Utilizado = " + MontoUtilizado + ", " +
                                  "       Monto_Restante = " + MontoRestante + ", " +
                                  "       Monto_Superado = " + MontoSuperado + ", " +
                                  "       Observaciones = '" + TxtObservaciones_PM.Text.Trim() + "' " +
                                  " WHERE Id_Consecutivo = " + Id_Consecutivo + " " +
                                  "   AND Id_Institucion = " + Id_Institucion + " " +
                                  "   AND Id_Paquete_Medico = " + Id_Paquete_Medico + " ";


                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se actualizó correctamente el paquete médico";
                mpeMensaje.Show();

                GetDatos_Paquetes();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Eliminar_ITM_38()
        {
            try
            {
                int index = Variables.wRenglon;

                string sReferencia = Variables.wRef;
                int iSubReferencia = Variables.wSubRef;

                int Id_Consecutivo = Convert.ToInt32(GrdPaqueteMedico.Rows[index].Cells[0].Text);
                int Id_Institucion = Convert.ToInt32(GrdPaqueteMedico.Rows[index].Cells[1].Text);
                int Id_Paquete_Medico = Convert.ToInt32(GrdPaqueteMedico.Rows[index].Cells[2].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_38)
                string strQuery = "DELETE FROM ITM_38 " +
                                  " WHERE Id_Consecutivo = " + Id_Consecutivo + " " +
                                  "   AND Id_Institucion = " + Id_Institucion + " " +
                                  "   AND Id_Paquete_Medico = " + Id_Paquete_Medico + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se eliminó correctamente el paquete médico";
                mpeMensaje.Show();

                GetDatos_Paquetes();
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void ddlEstatusCaso_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}