using System;
using System.Data;
using System.Data.SqlClient;

using System.Web.UI;
using System.Web.UI.WebControls;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using System.Linq;

using System.Collections.Generic;
using System.IO;
using MySql.Data.MySqlClient;
using System.Web;
using System.Security.Cryptography;
using System.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;

namespace WebItNow_Peacock
{
    public partial class fwBitacora_Asunto_bk : System.Web.UI.Page
    {

        protected string sResp_Tecnico
        {
            get { return ViewState["sResp_Tecnico"] as string ?? string.Empty; }
            set { ViewState["sResp_Tecnico"] = value; }
        }

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
                
                sResp_Tecnico = string.Empty;

                Variables.wRef = sReferencia;
                Variables.wSubRef = Convert.ToInt32(SubReferencia);
                Variables.wIdProyecto = Convert.ToInt32(IdProyecto);
                Variables.wPrefijo_Aseguradora = CveCliente;
                Variables.wIdTpoAsunto = Convert.ToInt32(IdTpoAsunto);
                Variables.wExiste = false;

                // BindRepeater();

                inhabilitar(this.Controls);
                chkCopiarDatos.Enabled = false;
                pnlEstadoOcurrencia.Visible = false;        // Ocultar por defecto

                if (IdTpoAsunto == "1")
                {
                    // Aplica solo para NOTIFICACION
                    pnlEstadoOcurrencia.Visible = true;     // Vizualizar por defecto
                }

                // inhabilitar control Crear Cuaderno
                if (Convert.ToString(Session["UsPrivilegios"]) == "0")
                {
                    BtnCrear_Cuaderno.Enabled = false;
                    BtnRepFotografico.Enabled = false;
                }

                // GetConsulta_Datos(sReferencia);

                GetProductos();
                GetEstados();
                
                GetEstadosContratante();
                GetEstadosBienAsegurado();

                GetEstadosAsegurado1();
                GetEstadosAsegurado2();
                GetEstadosAsegurado3();

                GetStSiniestro();
                GetConclusion();
                GetRegimen();

                // GetSecciones();
                // GetCoberturas();

                string flechaHaciaAbajo = "\u25BC";
                string flechaHaciaArriba = "\u25B2";

                btnShowPanel0.Text = flechaHaciaAbajo;      // Flecha hacia arriba
                btnShowPanel1.Text = flechaHaciaArriba;     // Flecha hacia arriba
             // btnShowPanel2.Text = flechaHaciaAbajo;      // Flecha hacia abajo
                btnShowPanel3.Text = flechaHaciaAbajo;      // Flecha hacia abajo
                btnShowPanel4.Text = flechaHaciaAbajo;      // Flecha hacia abajo
                btnShowPanel5.Text = flechaHaciaArriba;     // Flecha hacia arriba
                btnShowPanel6.Text = flechaHaciaAbajo;      // Flecha hacia abajo
                btnShowPanel7.Text = flechaHaciaAbajo;      // Flecha hacia abajo
                btnShowPanel8.Text = flechaHaciaAbajo;      // Flecha hacia abajo
                btnShowPanel9.Text = flechaHaciaAbajo;      // Flecha hacia abajo
                btnShowPanel10.Text = flechaHaciaAbajo;     // Flecha hacia abajo
                btnShowPanel11.Text = flechaHaciaAbajo;     // Flecha hacia abajo
                btnShowPanel12.Text = flechaHaciaAbajo;     // Flecha hacia abajo
                btnShowPanel13.Text = flechaHaciaAbajo;     // Flecha hacia abajo
                btnShowPanel14.Text = flechaHaciaAbajo;     // Flecha hacia abajo
                btnShowPanel15.Text = flechaHaciaAbajo;     // Flecha hacia abajo
                btnShowPanel16.Text = flechaHaciaAbajo;     // Flecha hacia abajo
                btnShowPanel17.Text = flechaHaciaAbajo;     // Flecha hacia abajo
                btnShowPanel18.Text = flechaHaciaArriba;    // Flecha hacia arriba (AZTECA, BERKLEY)
                btnShowPanel20.Text = flechaHaciaAbajo;     // Flecha hacia abajo
                btnShowPanel21.Text = flechaHaciaAbajo;     // Flecha hacia abajo

                // Definir visibilidad inicial de pnlDependencia
                bool mostrarPanel = Variables.wPrefijo_Aseguradora == "AZT" || Variables.wPrefijo_Aseguradora == "BER";
                pnlDependencia.Visible = mostrarPanel;
                
                if(Variables.wPrefijo_Aseguradora == "AZT")
                {
                    pnl18.Visible = mostrarPanel;

                } else if (Variables.wPrefijo_Aseguradora == "BER")
                {
                    pnl19.Visible = mostrarPanel;

                }
                
                ddlTpoAsegurado.Enabled = true;
                ddlConclusion.Enabled = true;
                ddlEstSiniestro.Enabled = true;
                ddlCategorias.Enabled = true;

                // ddlSecciones.Enabled = true;
                ddlCoberturas.Enabled = true;
                // TxtDomSiniestro.Enabled = true;

                GetSeccion_2();     // RIESGOS
                GetSeccion_4();     // BIENES
                GetSeccion_5();     // OTROS DETALLES

                // Desactivar Secciones (RIESGOS, BIENES y OTROS DETALLES)
                DesactivarCheckBoxes(grdSeccion_2, false);
                DesactivarCheckBoxes(grdSeccion_4, false);
                DesactivarCheckBoxes(grdSeccion_5, false);

                // Obtener datos generales
                GetConsulta_Datos_Generales(sReferencia, SubReferencia);

                // Obtener datos coberturas tabla ITM_70_3_4
                GetAltaCoberturas();
                // GetConsulta_Datos_Coberturas(sReferencia, SubReferencia);

                // inhabilitar controles panel coberturas
                // SetControlsEnabled(false);

                // Obtener los documentos de las categorias solicitados
                GetDocumentos(TxtSubReferencia.Text);


                // Inhabilitar controles si ya existe un Alta de Cuaderno
                if (Convert.ToString(Session["UsPrivilegios"]) != "0")
                {
                    Validar_Alta_Notebook();
                }

                // GetCategorias();
                GetCategoriasDaños();

                CargarUnidades(ddlUnidadEdif_1);
                CargarUnidades(ddlUnidadEdif_2);
                CargarUnidades(ddlUnidadEdif_3);

                pnlVacio.Visible = true;
                pnlEdificio.Visible = false;
                pnlOtros.Visible = false;
                pnlEdifGrid.Visible = false;
                pnlOtrosGrid.Visible = false;
                pnlBotones.Visible = false;

                Inicializar_GrdEdificio();
                Inicializar_GrdOtrosDaños();

                // Actualizar CheckBox Seleccionados
                // Select_ITM_91(Variables.wRef, Variables.wSubRef);

            }
            else
            {
                // habilitar controles (COBERTURAS)
                // ddlSecciones.Enabled = true;
                ddlCoberturas.Enabled = true;

                TxtSeccion.Enabled = true;
                TxtNomCobertura.Enabled = true;
                TxtSeccion.Enabled = true;
                TxtRiesgo.Enabled = true;
                TxtSumaAsegurada.Enabled = true;
                TxtValSumaAsegurada.Enabled = true;
                TxtAgregado.Enabled = true;
                TxtValAgregado.Enabled = true;
                TxtSublimite.Enabled = true;
                TxtValSublimite.Enabled = true;
                TxtDeducible.Enabled = true;
                TxtValDeducible.Enabled = true;
                TxtCoaseguro.Enabled = true;
                TxtValCoaseguro.Enabled = true;
                TxtNotas.Enabled = true;
                chkAplicaSiniestro.Enabled = true;

                habilitar_controles_daños();

            }


            //chkDocInterno.Enabled = true;
        }

        private void BindRepeater()
        {
            // Define los textos que deseas mostrar
            string[] labels = { "EDIFICIO", "REMOCIÓN", "CONTENIDOS", "CONSECUENCIALES", "GASTOS EXTRA", "EQUIPO ELECTRONICO", "ROTURA DE MAQUINARIA", "ROBO", 
                                "DINERO Y VALORES", "RESPONSABILIDAD CIVIL", "TRANSPORTES", "CIBER", "LINEAS FINANCIERAS", "OTRAS" };

            // Vincula el array de textos al Repeater
            // Repeater1.DataSource = labels;
            // Repeater1.DataBind();
        }

        private void CargarUnidades(DropDownList ddl)
        {
            ddl.Items.Clear();

            // Opción inicial vacía con valor 0
            ddl.Items.Add(new ListItem("-- Seleccionar --", "0"));

            ddl.Items.Add(new ListItem("mm", "mm"));
            ddl.Items.Add(new ListItem("cm", "cm"));
            ddl.Items.Add(new ListItem("dm", "dm"));
            ddl.Items.Add(new ListItem("m", "m"));
            ddl.Items.Add(new ListItem("km", "km"));
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

        protected void GetEstadosContratante()
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

                ddlEstadoContratante.DataSource = dt;

                ddlEstadoContratante.DataValueField = "c_estado";
                ddlEstadoContratante.DataTextField = "d_estado";

                ddlEstadoContratante.DataBind();
                ddlEstadoContratante.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetEstadosBienAsegurado()
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

                ddlEstadoBienAsegurado.DataSource = dt;

                ddlEstadoBienAsegurado.DataValueField = "c_estado";
                ddlEstadoBienAsegurado.DataTextField = "d_estado";

                ddlEstadoBienAsegurado.DataBind();
                ddlEstadoBienAsegurado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetEstadosAsegurado1()
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

                ddlEstadoAsegurado1.DataSource = dt;

                ddlEstadoAsegurado1.DataValueField = "c_estado";
                ddlEstadoAsegurado1.DataTextField = "d_estado";

                ddlEstadoAsegurado1.DataBind();
                ddlEstadoAsegurado1.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetEstadosAsegurado2()
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

                ddlEstadoAsegurado2.DataSource = dt;

                ddlEstadoAsegurado2.DataValueField = "c_estado";
                ddlEstadoAsegurado2.DataTextField = "d_estado";

                ddlEstadoAsegurado2.DataBind();
                ddlEstadoAsegurado2.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }


        protected void GetEstadosAsegurado3()
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

                ddlEstadoAsegurado3.DataSource = dt;

                ddlEstadoAsegurado3.DataValueField = "c_estado";
                ddlEstadoAsegurado3.DataTextField = "d_estado";

                ddlEstadoAsegurado3.DataBind();
                ddlEstadoAsegurado3.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

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

        protected void GetMunicipiosContratante(string pEstado)
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

                ddlMunicipiosContratante.DataSource = dt;

                ddlMunicipiosContratante.DataValueField = "c_mnpio";
                ddlMunicipiosContratante.DataTextField = "D_mnpio";

                ddlMunicipiosContratante.DataBind();
                ddlMunicipiosContratante.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetMunicipiosBienAsegurado(string pEstado)
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

                ddlMunicipiosBienAsegurado.DataSource = dt;

                ddlMunicipiosBienAsegurado.DataValueField = "c_mnpio";
                ddlMunicipiosBienAsegurado.DataTextField = "D_mnpio";

                ddlMunicipiosBienAsegurado.DataBind();
                ddlMunicipiosBienAsegurado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetMunicipiosAsegurado1(string pEstado)
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

                ddlMunicipiosAsegurado1.DataSource = dt;

                ddlMunicipiosAsegurado1.DataValueField = "c_mnpio";
                ddlMunicipiosAsegurado1.DataTextField = "D_mnpio";

                ddlMunicipiosAsegurado1.DataBind();
                ddlMunicipiosAsegurado1.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetMunicipiosAsegurado2(string pEstado)
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

                ddlMunicipiosAsegurado2.DataSource = dt;

                ddlMunicipiosAsegurado2.DataValueField = "c_mnpio";
                ddlMunicipiosAsegurado2.DataTextField = "D_mnpio";

                ddlMunicipiosAsegurado2.DataBind();
                ddlMunicipiosAsegurado2.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetMunicipiosAsegurado3(string pEstado)
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

                ddlMunicipiosAsegurado3.DataSource = dt;

                ddlMunicipiosAsegurado3.DataValueField = "c_mnpio";
                ddlMunicipiosAsegurado3.DataTextField = "D_mnpio";

                ddlMunicipiosAsegurado3.DataBind();
                ddlMunicipiosAsegurado3.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

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

                //string strQuery = "SELECT C.IdDocumento, C.Descripcion " +
                //                  "  FROM ITM_86 AS A, ITM_87 AS B, ITM_83 AS C " +
                //                  " WHERE A.IdSeccion = B.IdCategoria " +
                //                  "   AND A.IdDocumento = C.IdDocumento " +
                //                  "   AND IdProyecto = " + Variables.wIdProyecto + " AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                //                  "   AND IdSeccion = 3 ";

                //if (Variables.wIdProyecto != 0)
                //{
                //    // strQuery += "   AND A.[IdTpoAsunto] = " + Variables.wIdTpoAsunto + " ";
                //}

                //strQuery += "   AND bSeleccion = 1 AND A.IdStatus = 1 " +
                //            " ORDER BY C.IdDocumento ";

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_83
                string strQuery = "SELECT a.IdEtapa, b.Descripcion " +
                                  "  FROM ITM_53 as a, ITM_83 as b " +
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
                                  "  FROM ITM_86 AS A, ITM_87 AS B, ITM_81 AS C " +
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

        public void GetSecciones()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_44
                string strQuery = "SELECT IdSeccion, Descripcion " +
                                        " FROM ITM_44 " +
                                        " WHERE IdStatus = 1";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                //ddlSecciones.DataSource = dt;

                //ddlSecciones.DataValueField = "IdSeccion";
                //ddlSecciones.DataTextField = "Descripcion";

                //ddlSecciones.DataBind();
                //ddlSecciones.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetCoberturas()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int iIdSeccion = 0;
                string strQuery = "SELECT IdCobertura, Descripcion " +
                                  "  FROM ITM_94 " +
                                  " WHERE IdSeguros = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND IdProducto = " + ddlProductos.SelectedValue + " " +
                                  "   AND IdSeccion = " + iIdSeccion + " " +
                                  "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlCoberturas.DataSource = dt;

                ddlCoberturas.DataValueField = "IdCobertura";
                ddlCoberturas.DataTextField = "Descripcion";

                ddlCoberturas.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ddlCoberturas.Items.Insert(0, new ListItem("-- No Cobertura(s) --", "0"));
                }
                else
                {
                    ddlCoberturas.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                }

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetProductos()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdProducto, Descripcion " +
                                        " FROM ITM_39 " +
                                        " WHERE IdSeguros = '" + Variables.wPrefijo_Aseguradora + "' " +
                                        "   AND IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlProductos.DataSource = dt;

                ddlProductos.DataValueField = "IdProducto";
                ddlProductos.DataTextField = "Descripcion";

                ddlProductos.DataBind();
                ddlProductos.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                //Conecta.Cerrar();
                //cmd.Dispose();
                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetCategorias()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdProyecto, IdCliente, IdSeccion, A.IdDocumento, B.IdCategoria, " +
                                  "       C.Descripcion AS DescCategoria, B.Descripcion AS DescSeccion " +
                                  "  FROM ITM_86 AS A, ITM_87 AS B, ITM_82 AS C " +
                                  " WHERE A.IdSeccion = B.IdCategoria " +
                                  "   AND A.IdDocumento = C.IdDocumento " +
                                  "   AND IdProyecto = " + Variables.wIdProyecto + " AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND IdSeccion = 2 ";

                if (Variables.wIdProyecto != 0)
                {
                    strQuery += "   AND A.IdTpoAsunto = " + Variables.wIdTpoAsunto + " ";
                }

                    strQuery += "   AND bSeleccion = 1 AND A.IdStatus = 1 " +
                                "UNION ALL " +
                                "SELECT IdProyecto, IdCliente, IdSeccion, A.IdDocumento, B.IdCategoria, " +
                                "       C.Descripcion AS DescCategoria, B.Descripcion AS DescSeccion " +
                                "   FROM ITM_86 AS A, ITM_87 AS B, ITM_84 AS C " +
                                " WHERE A.IdSeccion = B.IdCategoria " +
                                "   AND A.IdDocumento = C.IdDocumento " +
                                "   AND IdProyecto = " + Variables.wIdProyecto + " AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                "   AND IdSeccion = 4 ";

                if (Variables.wIdProyecto != 0)
                {
                    strQuery += "   AND A.IdTpoAsunto = " + Variables.wIdTpoAsunto + " ";
                }

                    strQuery += "   AND bSeleccion = 1 AND A.IdStatus = 1 " +
                                    "UNION ALL " +
                                    "SELECT IdProyecto, IdCliente, IdSeccion, A.IdDocumento, B.IdCategoria, " +
                                    "       C.Descripcion AS DescCategoria, B.Descripcion AS DescSeccion " +
                                    "  FROM ITM_86 AS A, ITM_87 AS B, ITM_85 AS C " +
                                    " WHERE A.IdSeccion = B.IdCategoria " +
                                    "   AND A.IdDocumento = C.IdDocumento " +
                                    "   AND IdProyecto = " + Variables.wIdProyecto + " AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                    "   AND IdSeccion = 5 ";

                if (Variables.wIdProyecto != 0)
                {
                    strQuery += "   AND A.IdTpoAsunto = " + Variables.wIdTpoAsunto + " ";
                }

                    strQuery += "   AND bSeleccion = 1 AND A.IdStatus = 1 " +
                                " ORDER BY IdSeccion, A.IdDocumento ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    //GrdCategorias.ShowHeaderWhenEmpty = true;
                    //GrdCategorias.EmptyDataText = "No hay resultados.";
                }

                //GrdCategorias.DataSource = dt;
                //GrdCategorias.DataBind();

                dbConn.Close();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }

        }

        protected void GetCategoriasDaños()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t0.IdDocumento, t0.Descripcion " +
                                  "  FROM ITM_84 t0, ITM_91 t1" +
                                  " WHERE t0.IdDocumento = t1.IdCategoria " +
                                  "   AND Referencia = '" + Variables.wRef + "' AND SubReferencia = " + Variables.wSubRef + " " +
                                  "   AND IdSeccion = 4 AND t0.IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlCategorias.DataSource = dt;

                ddlCategorias.DataValueField = "IdDocumento";
                ddlCategorias.DataTextField = "Descripcion";

                ddlCategorias.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ddlCategorias.Items.Insert(0, new ListItem("-- No Hay Categoria(s) --", "0"));
                }
                else
                {
                    ddlCategorias.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                }

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetAltaDaños_Edificio()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t0.IdEdificio, t0.IdCategoria, t0.Referencia, t0.SubReferencia, t0.AreaEdif, t0.NivelEdif, t0.CategoriaEdif, t0.ElementoEdif, t0.TipoEdif, " +
                                  "       t0.MaterialesEdif, t0.MedidaEdif_1, t0.UnidadEdif_1, t0.MedidaEdif_2, t0.UnidadEdif_2, t0.MedidaEdif_3, t0.UnidadEdif_3, " +
                                  "       t0.ObsEdif_1, t0.ObsEdif_2 " +
                                  "  FROM ITM_99_1 t0 " +
                                  " WHERE t0.Referencia = '" + Variables.wRef + "' " +
                                  "   AND t0.SubReferencia = " + Variables.wSubRef + " ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdEdificio.ShowHeaderWhenEmpty = true;
                    GrdEdificio.EmptyDataText = "No hay resultados.";
                }

                GrdEdificio.DataSource = dt;
                GrdEdificio.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdEdificio.HeaderRow.TableSection = TableRowSection.TableHeader;

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void GetAltaDaños_Otros(int pIdCategoria)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t0.IdOtros, t0.IdCategoria, t0.Referencia, t0.SubReferencia, t0.CategoriaOtros, t0.TpoOtros, t0.GenOtros, t0.DescOtros," +
                                  "       t0.CantOtros, t0.MarcaOtros, t0.ModOtros, t0.NumSerieOtros, t0.ObsOtros_1, t0.ObsOtros_2 " +
                                  "  FROM ITM_99_2 t0 " +
                                  " WHERE t0.Referencia = '" + Variables.wRef + "' " +
                                  "   AND t0.SubReferencia = " + Variables.wSubRef + " " +
                                  "   AND t0.IdCategoria = " + pIdCategoria + "";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdOtrosDaños.ShowHeaderWhenEmpty = true;
                    GrdOtrosDaños.EmptyDataText = "No hay resultados.";
                }

                GrdOtrosDaños.DataSource = dt;
                GrdOtrosDaños.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdOtrosDaños.HeaderRow.TableSection = TableRowSection.TableHeader;

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void GetAltaCoberturas()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t0.IdSeccion, t0.IdCobertura, t1.Descripcion AS DescCobertura, t0.Cob_Nombre, t0.Cob_Seccion, t0.Cob_Riesgo, t0.Cob_Suma, " +
                                  "       t0.Cob_ValSuma, t0.Cob_Agregado, t0.Cob_ValAgregado, t0.Cob_Sublimite, t0.Cob_ValSublimite, t0.Cob_Deducible, " +
                                  "       t0.Cob_ValDeducible, t0.Cob_Coaseguro, t0.Cob_ValCoaseguro, t0.Cob_Notas, t0.Aplica_Siniestro " +
                                  "  FROM ITM_70_3_4 t0 " +
                                  "  JOIN ITM_94 t1 ON t0.IdCobertura = t1.IdCobertura " +
                                  " WHERE t0.Referencia = '" + Variables.wRef + "' " +
                                  "   AND t0.SubReferencia = " + Variables.wSubRef + " ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdCoberturas.ShowHeaderWhenEmpty = true;
                    GrdCoberturas.EmptyDataText = "No hay resultados.";
                }

                GrdCoberturas.DataSource = dt;
                GrdCoberturas.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdCoberturas.HeaderRow.TableSection = TableRowSection.TableHeader;

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
                //Lbl_Message.Text = FnErrorMessage(ex.Message);
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
                                  "       t1.IdSeccion AS IdSeccion, t1.IdCategoria AS IdCategoria, " +
                                  "       t3.Url_Archivo, t3.Nom_Archivo" +
                                  "  FROM ITM_88 AS t1 " +
                                  "  JOIN ITM_91 AS t2 ON t1.IdProyecto = t2.IdProyecto AND t1.IdSeccion = t2.IdSeccion " +
                                  "   AND t1.IdCliente = t2.IdCliente AND t1.IdCategoria = t2.IdCategoria " +
                                  "  LEFT JOIN ITM_47 AS t3 " +
                                  "    ON (CASE WHEN t3.SubReferencia >= 1 THEN CONCAT(t3.UsReferencia, '-', t3.SubReferencia) ELSE t3.UsReferencia END) = '" + sValor + "' " +
                                  "   AND t1.IdCliente = t3.IdAseguradora AND t1.IdTpoAsunto = t3.IdTpoAsunto " +
                                  "   AND t1.IdSeccion = t3.IdSeccion  AND t1.IdDocumento = t3.IdDocumento " +
                                  " WHERE (CASE WHEN t2.SubReferencia >= 1 THEN CONCAT(t2.Referencia, '-', t2.SubReferencia) ELSE t2.Referencia END) = '" + sValor + "' " +
                                  "   AND t1.IdProyecto = " + Variables.wIdProyecto + " " +
                                  "   AND t1.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "UNION " +
                                  "SELECT t1.IdDocumento, t2.Descripcion, t1.TpoArchivo, t1.Fec_Entrega, t2.DocInterno, " +
                                  "COALESCE(t1.IdDescarga, 0) AS IdDescarga, t1.IdSeccion AS IdSeccion, t1.IdCategoria AS IdCategoria," +
                                  "         t1.Url_Archivo, t1.Nom_Archivo " +
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

        public int GetConsulta_Datos_Generales(string pReferencia, string pSubReferencia)
        {

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t0.IdAsunto, t0.SubReferencia, CASE WHEN t0.SubReferencia >= 1 THEN CONCAT(t0.Referencia, '-', t0.SubReferencia) ELSE t0.Referencia END as Referencia_Sub, " +
                           "       t0.NumSiniestro, t0.NumPoliza, t0.NumReporte, t0.NomAsegurado, " +
                           "       A.IdProducto, A.Fec_Ocurrencia, A.Fec_Reporte, A.Fec_Asignacion, A.Fec_Inspeccion, A.Hora_Asignacion, " +
                           "       A.Detalle_Reporte, A.Calle, A.Num_Exterior, A.Num_Interior, A.Estado, A.Delegacion, A.Colonia, A.Codigo_Postal, " +
                           "       t0.NomAsegurado, " +
                           "       CASE WHEN t0.IdSeguros = 'OTR' THEN t0.NomCliente ELSE t2.Descripcion END as Seguro_Cia, " +
                           "       t0.IdRegimen, t0.IdEstStatus, t0.IdConclusion, t0.EstOcurrencia, t0.DescMote, " +
                           "       B.Nom_Contacto_1, B.Tipo_Contacto_1, B.Tel1_Contacto_1, B.Tel2_Contacto_1, B.Tel3_Contacto_1, B.Tel4_Contacto_1," +
                           "       B.Email_Contacto_1, B.Detalle_Contacto_1, " +
                           "       B.Nom_Contacto_2, B.Tipo_Contacto_2, B.Tel1_Contacto_2, B.Tel2_Contacto_2, B.Tel3_Contacto_2, B.Tel4_Contacto_2," +
                           "       B.Email_Contacto_2, B.Detalle_Contacto_2, " +
                           "       B.Nom_Contacto_3, B.Tipo_Contacto_3, B.Tel1_Contacto_3, B.Tel2_Contacto_3, B.Tel3_Contacto_3, B.Tel4_Contacto_3," +
                           "       B.Email_Contacto_3, B.Detalle_Contacto_3, " +
                           "       B.Nom_Contacto_4, B.Tipo_Contacto_4, B.Tel1_Contacto_4, B.Tel2_Contacto_4, B.Tel3_Contacto_4, B.Tel4_Contacto_4," +
                           "       B.Email_Contacto_4, B.Detalle_Contacto_4, " +
                           "       E.Nom_Asegurado_1, E.RFC_Asegurado_1, E.Tipo_Asegurado_1, E.Calle_Asegurado_1, E.Colonia_Asegurado_1, E.Estado_Asegurado_1," +
                           "       E.Municipio_Asegurado_1, E.CPostal_Asegurado_1, " +
                           "       E.Nom_Asegurado_2, E.RFC_Asegurado_2, E.Tipo_Asegurado_2, E.Calle_Asegurado_2, E.Colonia_Asegurado_2, E.Estado_Asegurado_2," +
                           "       E.Municipio_Asegurado_2, E.CPostal_Asegurado_2, " +
                           "       E.Nom_Asegurado_3, E.RFC_Asegurado_3, E.Tipo_Asegurado_3, E.Calle_Asegurado_3, E.Colonia_Asegurado_3, E.Estado_Asegurado_3," +
                           "       E.Municipio_Asegurado_3, E.CPostal_Asegurado_3, " +
                           "       D.Nom_Contratante, D.RFC_Contratante, D.Tipo_Contratante, D.Calle_Contratante, D.Colonia_Contratante, D.Estado_Contratante," +
                           "       D.Municipio_Contratante, D.CPostal_Contratante, " +
                           "       C.TpoProducto, C.Fec_Emision, C.Fec_IniVigencia, C.Fec_FinVigencia, A.Fec_Contacto, C.Num_Certificado, C.TpoMoneda, C.TpoPlan, C.Plazo, " +
                           "       C.CanalVentas, C.Num_Renovacion, C.Giro, " +
                           "       F.Calle_BienAsegurado, F.Num_Exterior_BienAsegurado, F.Colonia_BienAsegurado, F.Estado_BienAsegurado, F.Municipio_BienAsegurado, F.CPostal_BienAsegurado, " +
                           "       F.TpoTecho_BienAsegurado, F.TpoVivienda_BienAsegurado, F.TpoMuro_BienAsegurado, F.Pisos_BienAsegurado, F.PisosDel_BienAsegurado, F.Locales_BienAsegurado, " +
                           "       F.Detalles_BienAsegurado, " +
                           "       G.Nom_Rep_1, G.Puesto_Rep_1, G.Nom_Rep_2, G.Puesto_Rep_2, G.Det_Division, G.Nom_Afectado_1, G.Nom_Afectado_2, G.Nom_Proveedor, G.Dependencia, " +
                           "       t0.NomBeneficiario, t3.Descripcion AS Resp_Tecnico, t0.Referencia_Anterior, t0.NomBenefPreferente, C.Hora_Emision, " +
                           "       C.FormaPago, C.PrimaTotalAnual, C.PrimaNeta, C.GastosExpedicion, C.RecargoPago, C.PorcentajeIVA, C.MontoIVA, C.PrimaTotal, C.PrimaFormaPago, PagoSubsecuente, " +
                           "       H.RegistroPPAQ, H.FecRegistroPPAQ, H.RegistroRESP, H.FecRegistroRESP, H.RegistroCGEN, H.FecRegistroCGEN, H.RegistroBADI, H.FecRegistroBADI, " +
                           "       H.RegistroCONDUSEF, H.FecRegistroCONDUSEF, D.Num_Exterior, D.Num_Interior, " +
                           "       E.Email_Asegurado_1, E.Tel_Asegurado_1, E.Cel_Asegurado_1, E.Num_Exterior_Asegurado_1, E.Num_Interior_Asegurado_1, " +
                           "       E.Email_Asegurado_2, E.Tel_Asegurado_2, E.Cel_Asegurado_2, E.Num_Exterior_Asegurado_2, E.Num_Interior_Asegurado_2, " +
                           "       E.Email_Asegurado_3, E.Tel_Asegurado_3, E.Cel_Asegurado_3, E.Num_Interior_Asegurado_3, E.Num_Exterior_Asegurado_3, " +
                           "       F.Num_Interior_BienAsegurado, D.Email_Contratante, D.Tel1_Contratante, D.Tel2_Contratante," +
                           "       A.Otros_General, D.Otros_Contratante, E.Otros_Asegurado_1, E.Otros_Asegurado_2, E.Otros_Asegurado_3, F.Otros_BienAsegurado, t2.DescripBrev " +
                           "  FROM ITM_70 t0 " +
                           "  LEFT JOIN ITM_70_1 A ON t0.Referencia = A.Referencia AND t0.SubReferencia = A.SubReferencia " +
                           "  LEFT JOIN ITM_70_2 B ON t0.Referencia = B.Referencia AND t0.SubReferencia = B.SubReferencia " +
                           "  LEFT JOIN ITM_70_3 C ON t0.Referencia = C.Referencia AND t0.SubReferencia = C.SubReferencia " +
                           "  LEFT JOIN ITM_70_3_1 D ON t0.Referencia = D.Referencia AND t0.SubReferencia = D.SubReferencia " +
                           "  LEFT JOIN ITM_70_3_2 E ON t0.Referencia = E.Referencia AND t0.SubReferencia = E.SubReferencia " +
                           "  LEFT JOIN ITM_70_3_3 F ON t0.Referencia = F.Referencia AND t0.SubReferencia = F.SubReferencia " +
                           "  LEFT JOIN ITM_70_3_5 H ON t0.Referencia = H.Referencia AND t0.SubReferencia = H.SubReferencia " +
                           "  LEFT JOIN ITM_70_4 G ON t0.Referencia = G.Referencia AND t0.SubReferencia = G.SubReferencia " +
                           "  JOIN ITM_66 t1 ON t0.IdTpoAsunto = t1.IdTpoAsunto " +
                           "  JOIN ITM_67 t2 ON t0.IdSeguros = t2.IdSeguros " +
                           "  LEFT JOIN ITM_68 t3 ON t0.IdRespTecnico = t3.IdRespTecnico " +
                           "  LEFT JOIN ITM_69 t4 ON t0.IdRespAdministrativo = t4.IdRespAdministrativo " +
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
                    TxtNumPoliza.Text = Convert.ToString(row[4]);
                    TxtNumReporte.Text = Convert.ToString(row[5]);
                    TxtNomAsegurado.Text = Convert.ToString(row[6]);
                    ddlProductos.SelectedValue = Convert.ToString(row[7]);

                    // Disparar el evento SelectedIndexChanged manualmente
                    ddlProductos_SelectedIndexChanged(ddlProductos, EventArgs.Empty);

                    TxtFechaOcurrencia.Text = Convert.ToString(row[8]);
                    TxtFechaReporte.Text = Convert.ToString(row[9]);
                    TxtFechaAsignacion.Text = Convert.ToString(row[10]);
                    TxtFechaInspeccion.Text = Convert.ToString(row[11]);
                    TxtHoraAsignacion.Text = string.IsNullOrEmpty(Convert.ToString(row[12])) ? "00:00" : Convert.ToString(row[12]);
                    TxtDetalleReporte.Text = Convert.ToString(row[13]);
                    TxtCalle.Text = Convert.ToString(row[14]);
                    TxtNumExterior.Text = Convert.ToString(row[15]);
                    TxtNumInterior.Text = Convert.ToString(row[16]);
                    ddlEstado.SelectedValue = Convert.ToString(row[17]);

                    // Disparar el evento SelectedIndexChanged manualmente
                    ddlEstado_SelectedIndexChanged(ddlEstado, EventArgs.Empty);

                    ddlMunicipios.SelectedValue = Convert.ToString(row[18]);
                    TxtColonia.Text = Convert.ToString(row[19]);
                    TxtCodigoPostal.Text = Convert.ToString(row[20]);
                    TxtNomAsegurado.Text = Convert.ToString(row[21]);
                    TxtSeguro_Cia.Text = Convert.ToString(row[22]);
                    ddlTpoAsegurado.SelectedValue = Convert.ToString(row[23]);
                    ddlEstSiniestro.SelectedValue = Convert.ToString(row[24]);

                    // Disparar el evento SelectedIndexChanged manualmente
                    ddlEstSiniestro_SelectedIndexChanged(ddlEstSiniestro, EventArgs.Empty);
                    ddlConclusion.SelectedValue = Convert.ToString(row[25]);

                    TxtEstOcurrencia.Text = Convert.ToString(row[26]);
                    TxtDescMote.Text = Convert.ToString(row[27]);

                    // Datos Personales
                    TxtNomContacto1.Text = Convert.ToString(row[28]);
                    TxtTpoContacto1.Text = Convert.ToString(row[29]);
                    TxtTel1_Contacto1.Text = Convert.ToString(row[30]);
                    TxtTel2_Contacto1.Text = Convert.ToString(row[31]);
                    TxtTel3_Contacto1.Text = Convert.ToString(row[32]);
                    TxtTel4_Contacto1.Text = Convert.ToString(row[33]);
                    TxtEmailContacto1.Text = Convert.ToString(row[34]);
                    TxtDetalleContacto1.Text = Convert.ToString(row[35]);

                    TxtNomContacto2.Text = Convert.ToString(row[36]);
                    TxtTpoContacto2.Text = Convert.ToString(row[37]);
                    TxtTel1_Contacto2.Text = Convert.ToString(row[38]);
                    TxtTel2_Contacto2.Text = Convert.ToString(row[39]);
                    TxtTel3_Contacto2.Text = Convert.ToString(row[40]);
                    TxtTel4_Contacto2.Text = Convert.ToString(row[41]);
                    TxtEmailContacto2.Text = Convert.ToString(row[42]);
                    TxtDetalleContacto2.Text = Convert.ToString(row[43]);

                    TxtNomContacto3.Text = Convert.ToString(row[44]);
                    TxtTpoContacto3.Text = Convert.ToString(row[45]);
                    TxtTel1_Contacto3.Text = Convert.ToString(row[46]);
                    TxtTel2_Contacto3.Text = Convert.ToString(row[47]);
                    TxtTel3_Contacto3.Text = Convert.ToString(row[48]);
                    TxtTel4_Contacto3.Text = Convert.ToString(row[49]);
                    TxtEmailContacto3.Text = Convert.ToString(row[50]);
                    TxtDetalleContacto3.Text = Convert.ToString(row[51]);

                    TxtNomContacto4.Text = Convert.ToString(row[52]);
                    TxtTpoContacto4.Text = Convert.ToString(row[53]);
                    TxtTel1_Contacto4.Text = Convert.ToString(row[54]);
                    TxtTel2_Contacto4.Text = Convert.ToString(row[55]);
                    TxtTel3_Contacto4.Text = Convert.ToString(row[56]);
                    TxtTel4_Contacto4.Text = Convert.ToString(row[57]);
                    TxtEmailContacto4.Text = Convert.ToString(row[58]);
                    TxtDetalleContacto4.Text = Convert.ToString(row[59]);

                    TxtNombreAsegurado1.Text = Convert.ToString(row[60]);
                    TxtRFC_Asegurado1.Text = Convert.ToString(row[61]);
                    TxtTpoAsegurado1.Text = Convert.ToString(row[62]);
                    TxtCalleAsegurado1.Text = Convert.ToString(row[63]);
                    TxtColoniaAsegurado1.Text = Convert.ToString(row[64]);
                    ddlEstadoAsegurado1.SelectedValue = Convert.ToString(row[65]);

                    // Disparar el evento SelectedIndexChanged manualmente
                    ddlEstadoAsegurado1_SelectedIndexChanged(ddlEstadoAsegurado1, EventArgs.Empty);

                    ddlMunicipiosAsegurado1.SelectedValue = Convert.ToString(row[66]);
                    TxtCPostalAsegurado1.Text = Convert.ToString(row[67]);

                    TxtNombreAsegurado2.Text = Convert.ToString(row[68]);
                    TxtRFC_Asegurado2.Text = Convert.ToString(row[69]);
                    TxtTpoAsegurado2.Text = Convert.ToString(row[70]);
                    TxtCalleAsegurado2.Text = Convert.ToString(row[71]);
                    TxtColoniaAsegurado2.Text = Convert.ToString(row[72]);
                    ddlEstadoAsegurado2.SelectedValue = Convert.ToString(row[73]);

                    // Disparar el evento SelectedIndexChanged manualmente
                    ddlEstadoAsegurado2_SelectedIndexChanged(ddlEstadoAsegurado2, EventArgs.Empty);

                    ddlMunicipiosAsegurado2.SelectedValue = Convert.ToString(row[74]);
                    TxtCPostalAsegurado2.Text = Convert.ToString(row[75]);

                    TxtNombreAsegurado3.Text = Convert.ToString(row[76]);
                    TxtRFC_Asegurado3.Text = Convert.ToString(row[77]);
                    TxtTpoAsegurado3.Text = Convert.ToString(row[78]);
                    TxtCalleAsegurado3.Text = Convert.ToString(row[79]);
                    TxtColoniaAsegurado3.Text = Convert.ToString(row[80]);
                    ddlEstadoAsegurado3.Text = Convert.ToString(row[81]);

                    // Disparar el evento SelectedIndexChanged manualmente
                    ddlEstadoAsegurado3_SelectedIndexChanged(ddlEstadoAsegurado3, EventArgs.Empty);

                    ddlMunicipiosAsegurado3.Text = Convert.ToString(row[82]);
                    TxtCPostalAsegurado3.Text = Convert.ToString(row[83]);

                    // Contratante
                    TxtNomContratante.Text = Convert.ToString(row[84]);
                    TxtRFC_Contratante.Text = Convert.ToString(row[85]);
                    TxtTpoContratante.Text = Convert.ToString(row[86]);
                    TxtCalleContratante.Text = Convert.ToString(row[87]);
                    TxtColoniaContratante.Text = Convert.ToString(row[88]);
                    ddlEstadoContratante.SelectedValue = Convert.ToString(row[89]);

                    // Disparar el evento SelectedIndexChanged manualmente
                    ddlEstadoContratante_SelectedIndexChanged(ddlEstadoContratante, EventArgs.Empty);

                    ddlMunicipiosContratante.SelectedValue = Convert.ToString(row[90]);
                    TxtCPostalContratante.Text = Convert.ToString(row[91]);

                    // Detalles Poliza
                    TxtTpoProducto.Text = Convert.ToString(row[92]);
                    TxtFechaEmision.Text = Convert.ToString(row[93]);
                    TxtFechaIniVigencia.Text = Convert.ToString(row[94]);
                    TxtFechaFinVigencia.Text = Convert.ToString(row[95]);
                    TxtFechaContacto.Text = Convert.ToString(row[96]);
                    TxtNumCertificado.Text = Convert.ToString(row[97]);
                    TxtTpoMoneda.Text = Convert.ToString(row[98]);
                    TxtTpoPlan.Text = Convert.ToString(row[99]);

                    TxtPlazo.Text = Convert.ToString(row[100]);
                    TxtCanalVentas.Text = Convert.ToString(row[101]);
                    TxtNumRenovacion.Text = Convert.ToString(row[102]);
                    TxtGiro_Asegurado.Text = Convert.ToString(row[103]);

                    // Bienes
                    TxtCalleBienAsegurado.Text = Convert.ToString(row[104]);
                    TxtNumExtBienAsegurado.Text = Convert.ToString(row[105]);
                    TxtColoniaBienAsegurado.Text = Convert.ToString(row[106]);
                    ddlEstadoBienAsegurado.Text = Convert.ToString(row[107]);

                    // Disparar el evento SelectedIndexChanged manualmente
                    ddlEstadoBienAsegurado_SelectedIndexChanged(ddlEstadoBienAsegurado, EventArgs.Empty);

                    ddlMunicipiosBienAsegurado.Text = Convert.ToString(row[108]);
                    TxtCodigoBienAsegurado.Text = Convert.ToString(row[109]);
                    TxtTpoTecho.Text = Convert.ToString(row[110]);
                    TxtTpoVivienda.Text = Convert.ToString(row[111]);
                    TxtTpoMuro.Text = Convert.ToString(row[112]);
                    TxtPisosBienAsegurado.Text = Convert.ToString(row[113]);
                    TxtPisosDelBienAsegurado.Text = Convert.ToString(row[114]);
                    TxtLocalesComerciales.Text = Convert.ToString(row[115]);
                    TxtDetallesBienAsegurado.Text = Convert.ToString(row[116]);

                    // Dependencia (AZTECA)
                    TxtNomRep_1.Text = Convert.ToString(row[117]);
                    TxtPtoRep_1.Text = Convert.ToString(row[118]);
                    TxtNomRep_2.Text = Convert.ToString(row[119]);
                    TxtPtoRep_2.Text = Convert.ToString(row[120]);
                    TxtDetDivision.Text = Convert.ToString(row[121]);
                    TxtNomAfectado_1.Text = Convert.ToString(row[122]);
                    // TxtNomAfectado_2.Text = Convert.ToString(row[123]);
                    TxtNomProveedor.Text = Convert.ToString(row[124]);
                    TxtDependencia.Text = Convert.ToString(row[125]);

                    // (BERKLEY)
                    TxtNomBeneficiario.Text = Convert.ToString(row[126]);
                    sResp_Tecnico = Convert.ToString(row[127]);

                    TxtAntReferencia.Text = Convert.ToString(row[128]);
                    TxtBenef_Preferente.Text = Convert.ToString(row[129]);
                    TxtHoraEmision.Text = string.IsNullOrEmpty(Convert.ToString(row[130])) ? "00:00" : Convert.ToString(row[130]);

                    TxtFormaPago.Text = Convert.ToString(row[131]);
                    TxtPrimaTotalAnual.Text = Convert.ToString(row[132]);
                    TxtPrimaNeta.Text = Convert.ToString(row[133]);
                    TxtGastosExpedicion.Text = Convert.ToString(row[134]);
                    TxtRecargoPago.Text = Convert.ToString(row[135]);
                    TxtPorcentajeIVA.Text = Convert.ToString(row[136]);
                    TxtMontoIVA.Text = Convert.ToString(row[137]);
                    TxtPrimaTotal.Text = Convert.ToString(row[138]);
                    TxtPrimaFormaPago.Text = Convert.ToString(row[139]);
                    TxtPagoSubsecuente.Text = Convert.ToString(row[140]);

                    // CONDUSEF
                    TxtRegistroPPAQ.Text = Convert.ToString(row[141]);
                    TxtFecRegistroPPAQ.Text = Convert.ToString(row[142]);
                    TxtRegistroRESP.Text = Convert.ToString(row[143]);
                    TxtFecRegistroRESP.Text = Convert.ToString(row[144]);
                    TxtRegistroCGEN.Text = Convert.ToString(row[145]);
                    TxtFecRegistroCGEN.Text = Convert.ToString(row[146]);
                    TxtRegistroBADI.Text = Convert.ToString(row[147]);
                    TxtFecRegistroBADI.Text = Convert.ToString(row[148]);
                    TxtRegistroCONDUSEF.Text = Convert.ToString(row[149]);
                    TxtFecRegistroCONDUSEF.Text = Convert.ToString(row[150]);

                    TxtNumExtContratante.Text = Convert.ToString(row[151]);
                    TxtNumIntContratante.Text = Convert.ToString(row[152]);

                    TxtEmailAsegurado1.Text = Convert.ToString(row[153]);
                    TxtTelefonoAsegurado1.Text = Convert.ToString(row[154]);
                    TxtCelularAsegurado1.Text = Convert.ToString(row[155]);

                    TxtNumExtAsegurado1.Text = Convert.ToString(row[156]);
                    TxtNumIntAsegurado1.Text = Convert.ToString(row[157]);

                    TxtEmailAsegurado2.Text = Convert.ToString(row[158]);
                    TxtTelefonoAsegurado2.Text = Convert.ToString(row[159]);
                    TxtCelularAsegurado2.Text = Convert.ToString(row[160]);

                    TxtNumExtAsegurado2.Text = Convert.ToString(row[161]);
                    TxtNumIntAsegurado2.Text = Convert.ToString(row[162]);

                    TxtEmailAsegurado3.Text = Convert.ToString(row[163]);
                    TxtTelefonoAsegurado3.Text = Convert.ToString(row[164]);
                    TxtCelularAsegurado3.Text = Convert.ToString(row[165]);

                    TxtNumExtAsegurado3.Text = Convert.ToString(row[166]);
                    TxtNumIntAsegurado3.Text = Convert.ToString(row[167]);

                    TxtNumIntBienAsegurado.Text = Convert.ToString(row[168]);

                    TxtEmailContratante.Text = Convert.ToString(row[169]);
                    TxtTelefonoContratante.Text = Convert.ToString(row[170]);
                    TxtCelularContratante.Text = Convert.ToString(row[171]);

                    TxtOtrosGeneral.Text = Convert.ToString(row[172]);
                    TxtOtrosContratante.Text = Convert.ToString(row[173]);
                    TxtOtrosAsegurado1.Text = Convert.ToString(row[174]);
                    TxtOtrosAsegurado2.Text = Convert.ToString(row[175]);
                    TxtOtrosAsegurado3.Text = Convert.ToString(row[176]);
                    TxtOtrosBienAsegurado.Text = Convert.ToString(row[177]);

                    Variables.wDesc_Aseguradora = Convert.ToString(row[178]);

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

        public int GetConsulta_Datos(string pReferencia)
        {

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t0.Referencia, t0.Num_Reporte, t0.Num_Siniestro, CONVERT(VARCHAR, t0.Fec_Ocurrencia, 103) as Fec_Ocurrencia, CONVERT(VARCHAR, t0.Fec_Reporte, 103) as Fec__Reporte, " +
                                  "CONVERT(VARCHAR, t0.Fec_Asignacion, 103) as Fec_Asignacion, t0.Riesgo, t0.Det_Reporte, t0.Tpo_Contratante, t0.Lugar_Siniestro, " +
                                  "t0.Asegurado_1, t0.Asegurado_2, t0.Asegurado_3, t0.Tel_Reporte, t0.Tel_Ofi_Reporte, t0.Tel_Cel_Reporte, t0.Tel_Ase_Poliza, " +
                                  "t0.Nom_Contratante, t0.Tel_Contratante, t0.Mail_Contratante, t0.Cel_Contratante, t0.Beneficiario, " +
                                  "t0.Calle_Bien_Asegurado, t0.Colonia_Bien_Asegurado, t0.Poblacion_Bien_Asegurado, t0.Estado_Bien_Asegurado, " +
                                  "t0.Municipio_Bien_Asegurado, t0.CPostal_Bien_Asegurado, t0.Tpo_Producto, t0.Tpo_Plan, t0.Tpo_Moneda, t0.Num_Poliza," +
                                  "t0.Num_Certificado,  CONVERT(VARCHAR, t0.Fec_Emision, 103) as Fec_Emision, CONVERT(VARCHAR, t0.Fec_Ini_Vigencia, 103) as Fec_Ini_Vigencia, " +
                                  "CONVERT(VARCHAR, t0.Fec_Fin_Vigencia, 103) as Fec_Fin_Vigencia, t0.Plazo, t0.Canal_Ventas," +
                                  "t0.Calle_Asegurado, t0.Colonia_Asegurado, t0.Poblacion_Asegurado, t0.Estado_Asegurado, t0.Municipio_Asegurado, " +
                                  "t0.CPostal_Asegurado, t0.Calle_Contratante, t0.Colonia_Contratante, t0.Poblacion_Contratante, " +
                                  "t0.Estado_Contratante, t0.Municipio_Contratante, t0.CPostal_Contratante, " +
                                  "t0.Tpo_Techo, t0.Tpo_Vivienda, t0.Tpo_Muro, t0.Pisos_Bien_Asegurado, t0.Pisos_Del_Bien_Asegurado, " +
                                  "t0.Locales_Comerciales, t0.Detalles_Bien_Asegurado, t0.Regla_Suma_Asegurada " +
                                  "  FROM ITM_Temporal as t0 " +
                                  " WHERE t0.Referencia = '" + pReferencia + "'";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                foreach (DataRow row in dt.Rows)
                {
                    // Informacion General
                    // TxtReferencia.Text = Convert.ToString(row[0]);
                    TxtNumReporte.Text = Convert.ToString(row[1]);
                    // TxtNumSiniestro.Text = Convert.ToString(row[2]);

                    TxtFechaOcurrencia.Text = row[3] != DBNull.Value && row[3] != null ? Convert.ToString(row[3]) : string.Empty;
                    TxtFechaReporte.Text = row[4] != DBNull.Value && row[4] != null ? Convert.ToString(row[4]) : string.Empty;
                    TxtFechaAsignacion.Text = row[5] != DBNull.Value && row[5] != null ? Convert.ToString(row[5]) : string.Empty;
                    
                    // TxtRiesgo.Text = Convert.ToString(row[6]);
                    TxtDetalleReporte.Text = Convert.ToString(row[7]);
                    // TxtTpoContratante.Text = Convert.ToString(row[8]);
                    // TxtDomSiniestro.Text = Convert.ToString(row[9]);

                    // Datos Personales
                    // TxtAsegurado_1.Text = Convert.ToString(row[10]);
                    // TxtAsegurado_2.Text = Convert.ToString(row[11]);
                    // TxtAsegurado_3.Text = Convert.ToString(row[12]);
                    // TxtTelReporte.Text = Convert.ToString(row[13]);
                    // TxtTelOficina.Text = Convert.ToString(row[14]);
                    // TxtCelReporte.Text = Convert.ToString(row[15]);
                    // TxtTelAsegPoliza.Text = Convert.ToString(row[16]);
                    // TxtNomContratante.Text = Convert.ToString(row[17]);
                    // TxtTelContratante.Text = Convert.ToString(row[18]);
                    // TxtMailContratante.Text = Convert.ToString(row[19]);
                    // TxtCelContratante.Text = Convert.ToString(row[20]);
                    // TxtBeneficiario.Text = Convert.ToString(row[21]);

                    // Informacion Póliza
                    // TxtCalleBienAsegurado.Text = Convert.ToString(row[22]);
                    // TxtColoniaBienAsegurado.Text = Convert.ToString(row[23]);
                    // TxtPoblacionBienAsegurado.Text = Convert.ToString(row[24]);
                    // TxtEstadoBienAsegurado.Text = Convert.ToString(row[25]);
                    // TxtMunicipioBienAsegurado.Text = Convert.ToString(row[26]);
                    // TxtCPostalBienAsegurado.Text = Convert.ToString(row[27]);

                    TxtTpoProducto.Text = Convert.ToString(row[28]);
                    TxtTpoPlan.Text = Convert.ToString(row[29]);
                    TxtTpoMoneda.Text = Convert.ToString(row[30]);
                    TxtNumPoliza.Text = Convert.ToString(row[31]);
                    TxtNumCertificado.Text = Convert.ToString(row[32]);

                    TxtFechaEmision.Text = row[33] != DBNull.Value && row[32] != null ? Convert.ToString(row[32]) : string.Empty;
                    TxtFechaIniVigencia.Text = row[34] != DBNull.Value && row[33] != null ? Convert.ToString(row[33]) : string.Empty;
                    TxtFechaFinVigencia.Text = row[35] != DBNull.Value && row[34] != null ? Convert.ToString(row[34]) : string.Empty;

                    TxtPlazo.Text = Convert.ToString(row[36]);
                    TxtCanalVentas.Text = Convert.ToString(row[37]);

                    TxtCalleAsegurado1.Text = Convert.ToString(row[38]);
                    TxtColoniaAsegurado1.Text = Convert.ToString(row[39]);
                    // TxtPoblacionAsegurado1.Text = Convert.ToString(row[40]);
                    // TxtEstadoAsegurado1.Text = Convert.ToString(row[41]);
                    // TxtMunicipioAsegurado1.Text = Convert.ToString(row[42]);
                    TxtCPostalAsegurado1.Text = Convert.ToString(row[43]);

                    TxtCalleContratante.Text = Convert.ToString(row[44]);
                    TxtColoniaContratante.Text = Convert.ToString(row[45]);
                    // TxtPoblacionContratante.Text = Convert.ToString(row[46]);
                    // TxtEstadoContratante.Text = Convert.ToString(row[47]);
                    // TxtMunicipioContratante.Text = Convert.ToString(row[48]);
                    TxtCPostalContratante.Text = Convert.ToString(row[49]);

                    TxtTpoTecho.Text = Convert.ToString(row[50]);
                    TxtTpoVivienda.Text = Convert.ToString(row[51]);
                    TxtTpoMuro.Text = Convert.ToString(row[52]);
                    TxtPisosBienAsegurado.Text = Convert.ToString(row[53]);
                    TxtPisosDelBienAsegurado.Text = Convert.ToString(row[54]);
                    TxtLocalesComerciales.Text = Convert.ToString(row[55]);
                    TxtDetallesBienAsegurado.Text = Convert.ToString(row[56]);
                    // TxtReglaSumaAsegurada.Text = Convert.ToString(row[57]);

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

        public void Validar_Alta_Notebook()
        {

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT UsReferencia FROM ITM_03 " +
                                  " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END) = '" + TxtSubReferencia.Text + "' ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count > 0)
                {
                    ddlTpoAsegurado.Enabled = false;

                    BtnCrear_Cuaderno.Enabled = false;
                    BtnRepFotografico.Enabled = true;

                    // BtnGraba_Categorias.Enabled = false;

                    // Desactivar los CheckBoxes
                    DesactivarCheckBoxes(grdSeccion_2, false);
                    DesactivarCheckBoxes(grdSeccion_4, false);
                    DesactivarCheckBoxes(grdSeccion_5, false);

                    UpdatePanel8.Update();
                    UpdatePanel10.Update();
                    UpdatePanel12.Update();

                    Variables.wExiste = true;
                } 
                else 
                {
                    BtnRepFotografico.Enabled = false;
                }

                dbConn.Close();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

        }

        public void DesactivarCheckBoxes(GridView gridView, bool habilitar)
        {
            foreach (GridViewRow row in gridView.Rows)
            {
                // Verifica si la fila es una fila de datos
                if (row.RowType == DataControlRowType.DataRow)
                {
                    // Itera sobre las celdas de la fila
                    foreach (TableCell cell in row.Cells)
                    {
                        // Verifica si la celda contiene un control CheckBox
                        foreach (Control control in cell.Controls)
                        {
                            CheckBox checkBox = control as CheckBox;
                            if (checkBox != null)
                            {
                                // Desactivar o activar los CheckBoxes según el parámetro "desactivar"
                                // checkBox.Enabled = habilitar;

                                if (habilitar)
                                {
                                    // Elimina la clase para habilitar visualmente los CheckBoxes
                                    checkBox.CssClass = checkBox.CssClass.Replace("disabled-checkbox", "");
                                }
                                else
                                {
                                    // Agrega la clase para deshabilitar visualmente los CheckBoxes
                                    checkBox.CssClass += " disabled-checkbox";      
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Update_Consulta_Datos(string pNum_Reporte)
        {
            // Actualizar ITM_Temporal
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string fechaOcurrencia = !string.IsNullOrEmpty(TxtFechaOcurrencia.Text) ? $"CONVERT(smalldatetime, '{TxtFechaOcurrencia.Text.Trim()}', 103)" : "Null";
                string fechaReporte = !string.IsNullOrEmpty(TxtFechaReporte.Text) ? $"CONVERT(smalldatetime, '{TxtFechaReporte.Text.Trim()}', 103)" : "Null";
                string fechaAsignacion = !string.IsNullOrEmpty(TxtFechaAsignacion.Text) ? $"CONVERT(smalldatetime, '{TxtFechaAsignacion.Text.Trim()}', 103)" : "Null";

                string fechaEmisionValue = !string.IsNullOrEmpty(TxtFechaEmision.Text) ? $"CONVERT(smalldatetime, '{TxtFechaEmision.Text.Trim()}', 103)" : "Null";
                string fechaIniVigenciaValue = !string.IsNullOrEmpty(TxtFechaIniVigencia.Text) ? $"CONVERT(smalldatetime, '{TxtFechaIniVigencia.Text.Trim()}', 103)" : "Null";
                string fechaFinVigenciaValue = !string.IsNullOrEmpty(TxtFechaFinVigencia.Text) ? $"CONVERT(smalldatetime, '{TxtFechaFinVigencia.Text.Trim()}', 103)" : "Null";

                string PoblacionAsegurado1 = string.Empty; // TxtPoblacionAsegurado1.Text.Trim();
                string EstadoAsegurado1 = string.Empty; // TxtEstadoAsegurado1.Text.Trim();
                string MunicipioAsegurado1 = string.Empty; // TxtMunicipioAsegurado1.Text.Trim();

                string PoblacionContratante = string.Empty; // TxtPoblacionContratante.Text.Trim();
                string EstadoContratante = string.Empty; // TxtEstadoContratante.Text.Trim();
                string MunicipioContratante = string.Empty; // TxtMunicipioContratante.Text.Trim()

                string strQuery = "UPDATE ITM_Temporal " +
                                  " Set Num_Siniestro = '" + TxtNumSiniestro.Text.Trim() + "', Fec_Ocurrencia  = " + fechaOcurrencia + ", " +
                                  " Fec_Reporte = " + fechaReporte + ", Fec_Asignacion = " + fechaAsignacion + ", " +
                                  // " Riesgo = '" + TxtRiesgo.Text.Trim() + "', " +
                                  " Det_Reporte = '" + TxtDetalleReporte.Text.Trim() + "', " +
                                  // " Tpo_Contratante = '" + TxtTpoContratante.Text + "', Lugar_Siniestro = '" + TxtDomSiniestro.Text.Trim() + "', " +
                                  // " Asegurado_1 = '" + TxtAsegurado_1.Text.Trim() + "', Asegurado_2 = '" + TxtAsegurado_2.Text.Trim() + "', " +
                                  // " Asegurado_3 = '" + TxtAsegurado_3.Text.Trim() + "', Tel_Reporte = '" + TxtTelReporte.Text.Trim() + "', " +
                                  // " Tel_Ofi_Reporte = '" + TxtTelOficina.Text.Trim() + "', Tel_Cel_Reporte = '" + TxtCelReporte.Text.Trim() + "', " +
                                  // " Tel_Ase_Poliza = '" + TxtTelAsegPoliza.Text.Trim() + "', Nom_Contratante = '" + TxtNomContratante.Text.Trim() + "', " +
                                  // " Tel_Contratante = '" + TxtTelContratante.Text.Trim() + "', Mail_Contratante = '" + TxtMailContratante.Text.Trim() + "', " +
                                  // " Cel_Contratante = '" + TxtCelContratante.Text.Trim() + "', Beneficiario = '" + TxtBeneficiario.Text.Trim() + "'," +
                                  // " Calle_Bien_Asegurado = '" + TxtCalleBienAsegurado.Text.Trim() + "', Colonia_Bien_Asegurado = '" + TxtColoniaBienAsegurado.Text.Trim() + "'," +
                                  // " Poblacion_Bien_Asegurado = '" + TxtPoblacionBienAsegurado.Text.Trim() + "',  Estado_Bien_Asegurado = '" + TxtEstadoBienAsegurado.Text.Trim() + "', " +
                                  // " Municipio_Bien_Asegurado = '" + TxtMunicipioBienAsegurado.Text.Trim() + "', CPostal_Bien_Asegurado = '" + TxtCPostalBienAsegurado.Text.Trim() + "'," +
                                  " Tpo_Producto = '" + TxtTpoProducto.Text.Trim() + "', Tpo_Plan = '" + TxtTpoPlan.Text.Trim() + "'," +
                                  " Tpo_Moneda = '" + TxtTpoMoneda.Text.Trim() + "', Num_Poliza = '" + TxtNumPoliza.Text.Trim() + "'," +
                                  " Num_Certificado = '" + TxtNumCertificado.Text.Trim() + "', Fec_Emision = " + fechaEmisionValue + ", " +
                                  " Fec_Ini_Vigencia = " + fechaIniVigenciaValue + ", Fec_Fin_Vigencia = " + fechaFinVigenciaValue + "," +
                                  " Plazo = '" + TxtPlazo.Text.Trim() + "', Canal_Ventas = '" + TxtCanalVentas.Text.Trim() + "', " +
                                  " Calle_Asegurado = '" + TxtCalleAsegurado1.Text.Trim() + "', Colonia_Asegurado = '" + TxtColoniaAsegurado1.Text.Trim() + "'," +
                                  " Poblacion_Asegurado = '" + PoblacionAsegurado1 + "', Estado_Asegurado = '" + EstadoAsegurado1 + "', " +
                                  " Municipio_Asegurado = '" + MunicipioAsegurado1 + "', CPostal_Asegurado = '" + TxtCPostalAsegurado1.Text.Trim() + "'," +
                                  " Calle_Contratante = '" + TxtCalleContratante.Text.Trim() + "', Colonia_Contratante = '" + TxtColoniaContratante.Text.Trim() + "'," +
                                  " Poblacion_Contratante = '" + PoblacionContratante + "', Estado_Contratante = '" + EstadoContratante + "', " +
                                  " Municipio_Contratante = '" + MunicipioContratante + "', CPostal_Contratante = '" + TxtCPostalContratante.Text.Trim() + "', " +
                                  " Tpo_Techo = '" + TxtTpoTecho.Text.Trim() + "', Tpo_Vivienda = '" + TxtTpoVivienda.Text.Trim() + "'," +
                                  " Tpo_Muro = '" + TxtTpoMuro.Text.Trim() + "', Pisos_Bien_Asegurado = '" + TxtPisosBienAsegurado.Text.Trim() + "', " +
                                  " Pisos_Del_Bien_Asegurado = '" + TxtPisosDelBienAsegurado.Text.Trim() + "', Locales_Comerciales = '" + TxtLocalesComerciales.Text.Trim() + "', " +
                                  " Detalles_Bien_Asegurado = '" + TxtDetallesBienAsegurado.Text.Trim() + "', " +
                                  // " Regla_Suma_Asegurada = '" + TxtReglaSumaAsegurada.Text.Trim() + "' " +
                                  " WHERE Num_Reporte = '" + pNum_Reporte + "'";

                // Actualizar en la tabla temporal

                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        public void habilitar(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Enabled = true;
                else if (control is DropDownList)
                    ((DropDownList)control).Enabled = true;
                else if (control is RadioButtonList)
                    ((RadioButtonList)control).Enabled = true;
                else if (control is CheckBoxList)
                    ((CheckBoxList)control).Enabled = true;
                else if (control is RadioButton)
                    ((RadioButton)control).Enabled = true;
                else if (control is CheckBox)
                    ((CheckBox)control).Enabled = true;
                else if (control.HasControls())
                    //Esta linea detécta un Control que contenga otros Controles
                    //Así ningún control se quedará sin ser limpiado.
                    habilitar(control.Controls);
            }

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

        public void habilitar_controles_daños()
        {
            ddlCategorias.Enabled = true;

            //  Edificio
            TxtAreaEdif.Enabled = true;
            TxtNivelEdif.Enabled = true;
            TxtCategoriaEdif.Enabled = true;
            TxtElementoEdif.Enabled = true;
            TxtTipoEdif.Enabled = true;
            TxtMaterialesEdif.Enabled = true;
            TxtMedidaEdif_1.Enabled = true;
            ddlUnidadEdif_1.Enabled = true;
            TxtMedidaEdif_2.Enabled = true;
            ddlUnidadEdif_2.Enabled = true;
            TxtMedidaEdif_3.Enabled = true;
            ddlUnidadEdif_3.Enabled = true;
            TxtObsEdif_1.Enabled = true;
            TxtObsEdif_2.Enabled = true;

            // Otros
            TxtCategoriaOtros.Enabled = true;
            TxtTpoOtros.Enabled = true;
            TxtGenOtros.Enabled = true;
            TxtDescOtros.Enabled = true;
            TxtCantOtros.Enabled = true;
            TxtMarcaOtros.Enabled = true;
            TxtModOtros.Enabled = true;
            TxtNumSerieOtros.Enabled = true;
            TxtObsOtros_1.Enabled = true;
            TxtObsOtros_2.Enabled = true;
        }

        public void habilitar_Config_Siniestro()
        {
            if (Variables.wExiste)
            {
                ddlTpoAsegurado.Enabled = false;
                ddlConclusion.Enabled = true;

                BtnCrear_Cuaderno.Enabled = false;
                BtnRepFotografico.Enabled = true;
            }
            else
            {
                ddlTpoAsegurado.Enabled = true;
                ddlConclusion.Enabled = true;

                BtnCrear_Cuaderno.Enabled = true;
                BtnRepFotografico.Enabled = false;

                // BtnGraba_Categorias.Enabled = false;

                // Desactivar los CheckBoxes
                DesactivarCheckBoxes(grdSeccion_2, true);
                DesactivarCheckBoxes(grdSeccion_4, true);
                DesactivarCheckBoxes(grdSeccion_5, true);

                UpdatePanel8.Update();
                UpdatePanel10.Update();
                UpdatePanel12.Update();
            }

        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Variables.wRef = string.Empty;
            Variables.wSubRef = 0;
            Variables.wIdProyecto = 0;
            Variables.wPrefijo_Aseguradora = string.Empty;
            Variables.wIdTpoAsunto = 0;

            Response.Redirect("fwReporte_Alta_Asunto.aspx", true);
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            habilitar(this.Controls);

            TxtNumReporte.Enabled = false;
            //btnEditar.Visible = false;
            //BtnGrabar.Visible = true;
        }

        protected void BtnInformePreliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Variables.wIdTpoAsunto == 2)
                {
                    Dictionary<string, TextBox> fechas = new Dictionary<string, TextBox>
                    {
                        { "Fecha de Ocurrencia", TxtFechaOcurrencia },
                        { "Fecha de Asignación", TxtFechaAsignacion },
                        { "Fecha de Inspección", TxtFechaInspeccion },
                        { "Fecha de Contacto", TxtFechaContacto },
                        { "Fecha de Inicio de Vigencia", TxtFechaIniVigencia },
                        { "Fecha de Fin de Vigencia", TxtFechaFinVigencia }
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

                    Informe_Preliminar();

                    // *** Envio de correo electronico ***

                    //string Url_OneDrive = @"\Users\Dell\OneDrive - INSURANCE CLAIMS & RISKS MEXICO\";
                    //Variables.wDesc_Aseguradora = "1.1 AJUSTE SIMPLE\\ZURICH-SANTANDER";

                    //string Nom_Documento = "IP_" + Variables.wRef + ".docx";
                    //string wAdjunto = Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + Variables.wRef + "\\" + Nom_Documento;

                    //string pAsunto = "Informe Preliminar " + Variables.wRef;
                    //string pBody = "A quien corresponda:";

                    //// Crear instancia de la clase EnvioEmail
                    //EnvioEmail envioEmail = new EnvioEmail();

                    //envioEmail.EnvioMensaje(TxtEmailContacto1.Text, pAsunto, pBody, wAdjunto);


                } 
                else
                {
                    LblMessage.Text = "No existe plantilla para este tipo de asunto";
                    this.mpeMensaje.Show();
                }

            } catch (Exception ex)
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

        private void ReplaceText_bk(DocumentFormat.OpenXml.Wordprocessing.Body body, string placeholder, string newText)
        {
            foreach (var textElement in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
            {
                if (textElement.Text.Contains(placeholder))
                {
                    // Reemplazar el marcador con el nuevo texto
                    string updatedText = textElement.Text.Replace(placeholder, newText);

                    // Si el texto contiene saltos de línea representados por #BR#
                    if (updatedText.Contains("#BR#"))
                    {
                        // Crear un nuevo párrafo
                        var paragraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph();

                        // Dividir el texto en líneas usando el marcador #BR#
                        var lines = updatedText.Split(new string[] { "#BR#" }, StringSplitOptions.None);

                        foreach (var line in lines)
                        {
                            // Crear un Run para cada línea
                            var run = new DocumentFormat.OpenXml.Wordprocessing.Run();
                            run.Append(new DocumentFormat.OpenXml.Wordprocessing.Text(line));

                            // Agregar el Run al párrafo
                            paragraph.Append(run);

                            // Añadir un salto de línea <w:br /> después de cada línea, excepto al final
                            if (line != lines.Last())
                            {
                                paragraph.Append(new DocumentFormat.OpenXml.Wordprocessing.Run(new DocumentFormat.OpenXml.Wordprocessing.Text("\r\n")));
                            }
                        }

                        // Eliminar el Run original que contenía el marcador de posición
                        textElement.Parent.Remove();

                        // Añadir el nuevo párrafo al contenedor adecuado (Body)
                        body.Append(paragraph);
                    }
                    else
                    {
                        // Si no hay saltos de línea, simplemente actualizamos el texto del Run original
                        textElement.Text = updatedText;
                    }
                }
            }
        }



        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            // Update_Consulta_Datos(TxtNumReporte.Text);

            LblMessage.Text = "Se han aplicado los cambios, correctamente";
            this.mpeMensaje.Show();

            inhabilitar(this.Controls);
            //btnEditar.Visible = true;
            //BtnGrabar.Visible = false;
        }

        private string Responsable_Tecnico()
        {
            string Resp_Tecnico = string.Empty;

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT t2.Descripcion AS Resp_Tecnico " +
                                  "  FROM ITM_70 t1 " +
                                  "  LEFT JOIN ITM_68 t2 ON t1.IdRespTecnico = t2.IdRespTecnico " +
                                  " WHERE t1.IdStatus = 1 AND t1.IdTpoProyecto = " + Variables.wIdProyecto + " " +
                                  "   AND t1.Referencia = '" + Variables.wRef + "' AND t1.SubReferencia = " + Variables.wSubRef + "";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                // Verificar si hay resultados
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Resp_Tecnico = row["Resp_Tecnico"].ToString();
                    }

                    // Concatenar los valores encontrados separados por comas
                    return Resp_Tecnico;
                }

                // Cerrar la conexión
                dbConn.Close();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

            return Resp_Tecnico;
        }

        private string Categorias_Riegos()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT ITM_82.Descripcion AS Descripcion_82 " +
                                  "  FROM ITM_82 " +
                                  " INNER JOIN ITM_86 ON ITM_82.IdDocumento = ITM_86.IdDocumento " +
                                  "   AND ITM_86.IdProyecto = " + Variables.wIdProyecto + " AND ITM_86.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND ITM_86.bSeleccion = 1 AND ITM_86.IdSeccion = 2  " +
                                  " INNER JOIN ITM_91 ON ITM_82.IdDocumento = ITM_91.IdCategoria " +
                                  "   AND ITM_91.IdProyecto = " + Variables.wIdProyecto + " AND ITM_91.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND ITM_91.IdSeccion = 2 " +
                                  " WHERE ITM_82.IdStatus = 1 AND ITM_91.Referencia = '" + Variables.wRef + "' AND ITM_91.SubReferencia = " + Variables.wSubRef + "; ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                // Verificar si hay resultados
                if (dt.Rows.Count > 0)
                {
                    // Construir cadena con los valores encontrados
                    List<string> descripciones = new List<string>();
                    foreach (DataRow row in dt.Rows)
                    {
                        descripciones.Add(row["Descripcion_82"].ToString());
                    }

                    // Concatenar los valores encontrados separados por comas
                    return string.Join(", ", descripciones);
                }

                // Cerrar la conexión
                dbConn.Close();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

            return "No existen categorías.";
        }

        private void Informe_Preliminar()
        {
            try 
            { 
                string plantillaPath = Server.MapPath("~/itnowstorage/IP_CALZA_SIDER.docx");

                // string documentoGeneradoPath = Server.MapPath("~/itnowstorage/InformePreliminar.docx");
                string Nom_Documento = "IP_" + Variables.wRef + ".docx";
                string documentoGeneradoPath = Server.MapPath("~/itnowstorage/" + Nom_Documento);

                // Obtener de RIESGOS los campos seleccionados
                string Sub_Ramo = Categorias_Riegos();      // "Variación de Voltaje";

                // Obtener la fecha del 1er Contacto
                string Fec_Contacto = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                string Fec_Solicitud = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecOcurrencia = DateTime.ParseExact(TxtFechaOcurrencia.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaOcurrencia = fecOcurrencia.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecAsignacion = DateTime.ParseExact(TxtFechaAsignacion.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaAsignacion = fecAsignacion.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecInspeccion = DateTime.ParseExact(TxtFechaInspeccion.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaInspeccion = fecInspeccion.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecContacto = DateTime.ParseExact(TxtFechaContacto.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaContacto = fecContacto.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecPolizaIni = DateTime.ParseExact(TxtFechaIniVigencia.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaPoliza_Ini = fecPolizaIni.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecPolizaFin = DateTime.ParseExact(TxtFechaFinVigencia.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaPoliza_Fin = fecPolizaFin.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                string abrev_Colonia = "Colonia";
                string abrev_CodigoP = "C.P.";

                string Dom_Asegurado = TxtCalle.Text.Trim() + " " + TxtNumExterior.Text.Trim() + " " + TxtNumInterior.Text.Trim() + ", " + abrev_Colonia + " " + TxtColonia.Text.Trim()
                                      + ", " + ddlMunicipios.SelectedItem + ", " + ddlEstado.SelectedItem + ", " + abrev_CodigoP + " " + TxtCodigoPostal.Text.Trim();

                // Declaración de variables
                string Cobertura_1 = string.Empty, Suma_Aseg_1 = string.Empty, Sublimite_1 = string.Empty, Deducible_1 = string.Empty, Coaseguro_1 = string.Empty;
                string Cobertura_2 = string.Empty, Suma_Aseg_2 = string.Empty, Sublimite_2 = string.Empty, Deducible_2 = string.Empty, Coaseguro_2 = string.Empty;
                string Cobertura_3 = string.Empty, Suma_Aseg_3 = string.Empty, Sublimite_3 = string.Empty, Deducible_3 = string.Empty, Coaseguro_3 = string.Empty;
                string Cobertura_4 = string.Empty, Suma_Aseg_4 = string.Empty, Sublimite_4 = string.Empty, Deducible_4 = string.Empty, Coaseguro_4 = string.Empty;

                // Obtener Coberturas
                // Iterar sobre las filas del GridView
                for (int i = 0; i < GrdCoberturas.Rows.Count && i < 4; i++) // Máximo 4 filas
                {
                    GridViewRow row = GrdCoberturas.Rows[i];

                    // Recuperar valores de las celdas
                    string cobertura = Server.HtmlDecode(Convert.ToString(row.Cells[4].Text));       // Cobertura
                    string sumaAseg = Server.HtmlDecode(Convert.ToString(row.Cells[6].Text));        // Suma Asegurada
                    string sublimite = Server.HtmlDecode(Convert.ToString(row.Cells[7].Text));       // Sublimite
                    string deducible = Server.HtmlDecode(Convert.ToString(row.Cells[8].Text));       // Deducible
                    string coaseguro = Server.HtmlDecode(Convert.ToString(row.Cells[9].Text));       // Coaseguro

                    // Asignar valores a las variables según el renglón
                    switch (i)
                    {
                        case 0:
                            Cobertura_1 = cobertura;
                            Suma_Aseg_1 = sumaAseg;
                            Sublimite_1 = sublimite;
                            Deducible_1 = deducible;
                            Coaseguro_1 = coaseguro;
                            break;
                        case 1:
                            Cobertura_2 = cobertura;
                            Suma_Aseg_2 = sumaAseg;
                            Sublimite_2 = sublimite;
                            Deducible_2 = deducible;
                            Coaseguro_2 = coaseguro;
                            break;
                        case 2:
                            Cobertura_3 = cobertura;
                            Suma_Aseg_3 = sumaAseg;
                            Sublimite_3 = sublimite;
                            Deducible_3 = deducible;
                            Coaseguro_3 = coaseguro;
                            break;
                        case 3:
                            Cobertura_4 = cobertura;
                            Suma_Aseg_4 = sumaAseg;
                            Sublimite_4 = sublimite;
                            Deducible_4 = deducible;
                            Coaseguro_4 = coaseguro;
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
                    ReplaceText(body, "Fec_Solicitud", Fec_Solicitud);
                    ReplaceText(body, "Num_Referencia", TxtSubReferencia.Text);
                    ReplaceText(body, "Num_Poliza", TxtNumPoliza.Text);
                    ReplaceText(body, "Num_Reporte", TxtNumReporte.Text);
                    ReplaceText(body, "Num_Siniestro", TxtNumSiniestro.Text);
                    ReplaceText(body, "Fec_Ocurrencia", FechaOcurrencia);
                    ReplaceText(body, "Fec_Notificacion", FechaAsignacion);
                    ReplaceText(body, "Fec_Asignacion", FechaAsignacion);
                    ReplaceText(body, "Fec_Contacto", FechaContacto);
                    ReplaceText(body, "Nom_Asegurado", TxtNomAsegurado.Text);
                    ReplaceText(body, "Dom_Asegurado", Dom_Asegurado);
                    ReplaceText(body, "Giro_Asegurado", TxtGiro_Asegurado.Text);
                    ReplaceText(body, "Fec_Poliza_Ini", FechaPoliza_Ini);
                    ReplaceText(body, "Fec_Poliza_Fin", FechaPoliza_Fin);
                    ReplaceText(body, "Tpo_Producto", TxtTpoProducto.Text);
                    ReplaceText(body, "Tpo_Moneda", TxtTpoMoneda.Text);
                    ReplaceText(body, "Tpo_Ramo", string.Empty);
                 // string Sub_RamoConSaltos = Sub_Ramo.Replace("\n", "#BR#");
                    ReplaceText(body, "Tpo_Sub_Ramo", Sub_Ramo);
                    ReplaceText(body, "Detalle_Reporte", TxtDetalleReporte.Text);
                    ReplaceText(body, "Fec_Inspeccion", FechaInspeccion);
                    ReplaceText(body, "Fec_Siniestro", FechaOcurrencia);

                    ReplaceText(body, "Cobertura_1", Cobertura_1);
                    ReplaceText(body, "Suma_Aseg_1", Suma_Aseg_1);
                    ReplaceText(body, "Sublimite_1", Sublimite_1);
                    ReplaceText(body, "Deducible_1", Deducible_1);
                    ReplaceText(body, "Dedu_1", Deducible_1);
                    ReplaceText(body, "Coaseguro_1", Coaseguro_1);

                    ReplaceText(body, "Cobertura_2", Cobertura_2);
                    ReplaceText(body, "Suma_Aseg_2", Suma_Aseg_2);
                    ReplaceText(body, "Sublimite_2", Sublimite_2);
                    ReplaceText(body, "Deducible_2", Deducible_2);
                    ReplaceText(body, "Dedu_2", Deducible_2);
                    ReplaceText(body, "Coaseguro_2", Coaseguro_2);

                    ReplaceText(body, "Cobertura_3", Cobertura_3);
                    ReplaceText(body, "Suma_Aseg_3", Suma_Aseg_3);
                    ReplaceText(body, "Sublimite_3", Sublimite_3);
                    ReplaceText(body, "Deducible_3", Deducible_3);
                    ReplaceText(body, "Dedu_3", Deducible_3);
                    ReplaceText(body, "Coaseguro_3", Coaseguro_3);

                    ReplaceText(body, "Cobertura_4", Cobertura_4);
                    ReplaceText(body, "Suma_Aseg_4", Suma_Aseg_4);
                    ReplaceText(body, "Sublimite_4", Sublimite_4);
                    ReplaceText(body, "Deducible_4", Deducible_4);
                    ReplaceText(body, "Dedu_4", Deducible_4);
                    ReplaceText(body, "Coaseguro_4", Coaseguro_4);

                    ReplaceText(body, "Fec_Inspeccion", FechaInspeccion);


                    // Guardar los cambios
                    wordDoc.MainDocumentPart.Document.Save();
                }

                LblMessage.Text = "Informe Preliminar, se a generado correctamente";
                this.mpeMensaje.Show();

                // Descargar el documento generado
                // Session["sFileName"] = "InformePreliminar.docx";

                Session["sFileName"] = Nom_Documento;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AbrirDescarga", string.Format("window.open('Descargas.aspx');"), true);

                // Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                // Response.AddHeader("Content-Disposition", "attachment; filename=DocumentoGenerado.docx");
                // Response.TransmitFile(documentoGeneradoPath);
                // Response.End();

                // Mostrar mensaje en el lado del cliente después de la descarga
                // ClientScript.RegisterStartupScript(this.GetType(), "downloadComplete", "alert('El Documento Word, se a generado correctamente.');", true);

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        private void Carta_Solicitud()
        {
            try
            {
                string plantillaPath = Server.MapPath("~/itnowstorage/CARTA_SOLICITUD.docx");

                // string documentoGeneradoPath = Server.MapPath("~/itnowstorage/CartaSolicitud.docx");

                string Nom_Documento = "CSD_" + Variables.wRef + ".docx";
                string documentoGeneradoPath = Server.MapPath("~/itnowstorage/" + Nom_Documento);

                // Obtener la fecha del día
                DateTime fechaActual = DateTime.Now;

                string DomSiniestro = TxtCalle.Text.Trim() + " " + TxtNumExterior.Text.Trim() + " " + TxtNumInterior.Text.Trim() + ", " + TxtColonia.Text.Trim()
                                      + ", " + ddlMunicipios.SelectedItem + ", " + TxtCodigoPostal.Text.Trim() + ", " + ddlEstado.SelectedItem ;

                // Formatear la fecha
                string Fec_Solicitud = fechaActual.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                List<string> documentosSolicitados = ObtenerDocumentosSolicitados();

                //// Usa string.Join directamente para concatenar los documentos con dos saltos de línea.
                //string Doc_Solicitados = string.Join(Environment.NewLine, documentosSolicitados);

                // string Doc_Solicitados = string.Empty;

                // Concatenar los valores en la variable Doc_Solicitados
                // foreach (var doc in documentosSolicitados)
                // {
                //     Doc_Solicitados += doc + Environment.NewLine;
                //     Doc_Solicitados += string.Join(Environment.NewLine, documentosSolicitados);
                // }

                List<string> elementosSeleccionados = ObtenerElementosSeleccionados();

                string Categorias_Seleccionadas = string.Join(", ", elementosSeleccionados);

                // Copiar la plantilla a un nuevo documento
                System.IO.File.Copy(plantillaPath, documentoGeneradoPath, true);

                // Abrir el documento generado
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documentoGeneradoPath, true))
                {

                    // Obtener el cuerpo del documento
                    DocumentFormat.OpenXml.Wordprocessing.Body body = wordDoc.MainDocumentPart.Document.Body;

                    // Verificar si el cuerpo existe, si no, lanzar una excepción
                    if (body == null)
                    {
                        throw new InvalidOperationException("La plantilla no tiene un cuerpo definido.");
                    }

                    // Buscar y reemplazar marcadores de posición con datos reales
                    ReplaceText(body, "Fec_Solicitud", Fec_Solicitud);
                    ReplaceText(body, "Num_Poliza", TxtNumPoliza.Text);
                    ReplaceText(body, "Num_Siniestro", TxtNumSiniestro.Text);
                    ReplaceText(body, "Nom_Asegurado", TxtNomAsegurado.Text);

                    ReplaceText(body, "Dom_Registrado", DomSiniestro);
                    ReplaceText(body, "Nom_Categorias", Categorias_Seleccionadas);

                    ReplaceTextWithNewLines(body, "Doc_Solicitados", documentosSolicitados, "Skeena", 10);

                    // ReplaceText(body, "Doc_Solicitados", Doc_Solicitados);

                    // Guardar los cambios
                    wordDoc.MainDocumentPart.Document.Save();
                }

                LblMessage.Text = "Carta Solicitud, se a generado correctamente";
                this.mpeMensaje.Show();

                // Descargar el documento generado
                // Session["sFileName"] = "CartaSolicitud.docx";
                Session["sFileName"] = Nom_Documento;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AbrirDescarga", string.Format("window.open('Descargas.aspx');"), true);

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        private void Convenio_Ajuste()
        {
            try
            {
                string sAseguradora = Variables.wPrefijo_Aseguradora + "_";
                string sDocumento = string.Empty;
                string sTpoDocumento = string.Empty;
                string Nom_Documento = string.Empty;

                if (Variables.wPrefijo_Aseguradora == "AZT")
                {

                    if (TxtNomAfectado_1.Text == "" && TxtNomProveedor.Text == "")
                    {
                        sTpoDocumento = "A_";       // Convenio pago a asegurado 
                        //sDocumento = "PA_CONVENIO_AJUSTE.docx";

                    } else if (TxtNomAfectado_1.Text != "" && TxtNomProveedor.Text == "")
                    {
                        sTpoDocumento = "T_";       // Convenio pago a tercero
                        //sDocumento = "PT_CONVENIO_AJUSTE.docx";

                    } else if (TxtNomAfectado_1.Text == "" && TxtNomProveedor.Text != "")
                    {
                        sTpoDocumento = "P_";       // Convenio pago a proveedor
                        //sDocumento = "PP_CONVENIO_AJUSTE.docx";
                    }

                }
                if (Variables.wPrefijo_Aseguradora == "BER")
                {

                    if (TxtNomBeneficiario.Text != "")
                    {
                        sTpoDocumento = "T_";       // Convenio pago a tercero
                        // sDocumento = "PT_CONVENIO_AJUSTE.docx";     // Convenio pago a tercero

                    }
                }

                sDocumento = sTpoDocumento + "CONVENIO_AJUSTE.docx";

                string plantillaPath = Server.MapPath("~/itnowstorage/" + sAseguradora + sDocumento);

                if (Variables.wPrefijo_Aseguradora == "AZT" || Variables.wPrefijo_Aseguradora == "BER")
                {
                    Nom_Documento = "CA" + sTpoDocumento + Variables.wRef + ".docx";
                }
                else
                {
                    // Convenio pago a asegurado
                    Nom_Documento = "CAA_" + Variables.wRef + ".docx";
                }
                
                string documentoGeneradoPath = Server.MapPath("~/itnowstorage/" + Nom_Documento);

                // Obtener de RIESGOS los campos seleccionados
                string Sub_Ramo = Categorias_Riegos();

                // Obtener Responsable Tecnico
                string Resp_Tecnico = Responsable_Tecnico();

                string Fec_Solicitud = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecOcurrencia = DateTime.ParseExact(TxtFechaOcurrencia.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaOcurrencia = fecOcurrencia.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecPolizaIni = DateTime.ParseExact(TxtFechaIniVigencia.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaPoliza_Ini = fecPolizaIni.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecPolizaFin = DateTime.ParseExact(TxtFechaFinVigencia.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaPoliza_Fin = fecPolizaFin.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                string DomSiniestro = TxtCalle.Text.Trim() + " " + TxtNumExterior.Text.Trim() + " " + TxtNumInterior.Text.Trim() + ", " + TxtColonia.Text.Trim()
                                      + ", " + ddlMunicipios.SelectedItem + ", " + TxtCodigoPostal.Text.Trim() + ", " + ddlEstado.SelectedItem;

                // Copiar la plantilla a un nuevo documento
                System.IO.File.Copy(plantillaPath, documentoGeneradoPath, true);

                // Abrir el documento generado
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(documentoGeneradoPath, true))
                {
                    // Obtener el cuerpo del documento
                    DocumentFormat.OpenXml.Wordprocessing.Body body = wordDoc.MainDocumentPart.Document.Body;

                    if (Variables.wPrefijo_Aseguradora == "BER")
                    {
                        ReplaceText(body, "Num_Referencia", TxtSubReferencia.Text);
                        ReplaceText(body, "Nom_Asegurado", TxtNomAsegurado.Text);
                        ReplaceText(body, "Num_Poliza", TxtNumPoliza.Text);
                        ReplaceText(body, "Num_Siniestro", TxtNumSiniestro.Text);
                        
                        ReplaceText(body, "Fec_Dia", fecOcurrencia.Day.ToString("00"));
                        ReplaceText(body, "Fec_Mes", fecOcurrencia.Month.ToString("00"));
                        ReplaceText(body, "Fec_Año", fecOcurrencia.Year.ToString());

                        ReplaceText(body, "Nom_Beneficiario", TxtNomBeneficiario.Text);
                        ReplaceText(body, "Det_Reporte", TxtDetalleReporte.Text);
                        ReplaceText(body, "Fec_Ocurrencia", FechaOcurrencia);

                        ReplaceText(body, "Resp_Tecnico", sResp_Tecnico);

                    }
                    else if (Variables.wPrefijo_Aseguradora == "AZT" || Variables.wPrefijo_Aseguradora == "BER")
                    {
                        DateTime fecSiniestro = DateTime.ParseExact(TxtFechaOcurrencia.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        string FechaSiniestro = fecSiniestro.ToString("dddd, dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                        ReplaceText(body, "Num_Referencia", TxtSubReferencia.Text);
                        ReplaceText(body, "Num_Siniestro", TxtNumSiniestro.Text);
                        ReplaceText(body, "Nom_Asegurado", TxtNomAsegurado.Text);
                        ReplaceText(body, "Nom_Dependencia", TxtDependencia.Text);
                        ReplaceText(body, "Nom_Responsable", TxtNomRep_1.Text);
                        ReplaceText(body, "Pto_Responsable", TxtPtoRep_1.Text);
                        ReplaceText(body, "Num_Poliza", TxtNumPoliza.Text);
                        ReplaceText(body, "Fec_Siniestro", FechaSiniestro);

                        ReplaceText(body, "Nom_Alcaldia", ddlMunicipios.SelectedItem.Text);
                        ReplaceText(body, "Nom_Representante", TxtNomRep_2.Text);
                        ReplaceText(body, "Pto_Representante", TxtPtoRep_2.Text);

                        ReplaceText(body, "Det_Reporte", TxtDetalleReporte.Text);
                        ReplaceText(body, "Div_Gubernamental", TxtDetDivision.Text);

                        ReplaceText(body, "Nom_Afectado", TxtNomAfectado_1.Text);
                        ReplaceText(body, "Nom_Proveedor", TxtNomProveedor.Text);

                        ReplaceText(body, "Dom_Siniestro", DomSiniestro);
                        ReplaceText(body, "Fec_Ocurrencia", FechaOcurrencia);
                    }
                    else
                    {
                        // Buscar y reemplazar marcadores de posición con datos reales
                        ReplaceText(body, "Fec_Solicitud", Fec_Solicitud);
                        ReplaceText(body, "Num_Referencia", TxtSubReferencia.Text);
                        ReplaceText(body, "Num_Poliza", TxtNumPoliza.Text);
                        ReplaceText(body, "Num_Siniestro", TxtNumSiniestro.Text);
                        ReplaceText(body, "Fec_Ocurrencia", FechaOcurrencia);
                        ReplaceText(body, "Tpo_Sub_Ramo", Sub_Ramo);
                        ReplaceText(body, "Nom_Asegurado", TxtNomAsegurado.Text);
                        ReplaceText(body, "Fec_Poliza_Ini", FechaPoliza_Ini);
                        ReplaceText(body, "Fec_Poliza_Fin", FechaPoliza_Fin);
                        ReplaceText(body, "Resp_Tecnico", Resp_Tecnico);
                    }

                    // Guardar los cambios
                    wordDoc.MainDocumentPart.Document.Save();
                }

                LblMessage.Text = "Convenio Ajuste, se a generado correctamente";
                this.mpeMensaje.Show();

                Session["sFileName"] = Nom_Documento;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AbrirDescarga", string.Format("window.open('Descargas.aspx');"), true);

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        private List<string> ObtenerDocumentosSolicitados()
        {
            List<string> documentosSolicitados = new List<string>();

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "WITH CTE_Union AS ( SELECT t1.NomArchivo AS DescripcionOriginal, t1.NomArchivo, 1 AS Grupo " +
                                  "     FROM ITM_88 AS t1 " +
                                  "     JOIN ITM_90 AS t4 ON t4.DocInterno = 0 AND t1.IdDocumento = t4.IdConsecutivo " +
                                  "     JOIN ITM_91 AS t2 ON t1.IdProyecto = t2.IdProyecto AND t1.IdSeccion = t2.IdSeccion " +
                                  "	     AND t1.IdCliente = t2.IdCliente AND t1.IdCategoria = t2.IdCategoria " +
                                  "     LEFT JOIN ITM_47 AS t3 ON (CASE WHEN t3.SubReferencia >= 1 THEN CONCAT(t3.UsReferencia, '-', t3.SubReferencia) ELSE t3.UsReferencia END) = '" + Variables.wRef + "' " +
                                  "      AND t1.IdCliente = t3.IdAseguradora AND t1.IdTpoAsunto = t3.IdTpoAsunto " +
                                  "      AND t1.IdSeccion = t3.IdSeccion  AND t1.IdDocumento = t3.IdDocumento " +
                                  "    WHERE (CASE WHEN t2.SubReferencia >= 1 THEN CONCAT(t2.Referencia, '-', t2.SubReferencia) ELSE t2.Referencia END) = '" + Variables.wRef + "'  " +
                                  "      AND t1.IdProyecto = " + Variables.wIdProyecto + " " +
                                  "      AND t1.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "      AND t3.IdDescarga = 0 " +
                                  "UNION ALL " +
                                  "   SELECT t2.Descripcion AS DescripcionOriginal, t2.Descripcion AS NomArchivo, 2 AS Grupo " +
                                  "     FROM ITM_47 AS t1 JOIN ITM_46 AS t2 ON t1.UsReferencia = t2.UsReferencia " +
                                  "      AND (CASE WHEN t2.SubReferencia >= 1 THEN CONCAT(t2.UsReferencia, '-', t2.SubReferencia) ELSE t2.UsReferencia END) = '" + Variables.wRef + "' " +
                                  "	     AND t1.SubReferencia = t2.SubReferencia AND t1.IdDocumento = t2.IdDocumento AND t2.DocInterno = 0 " +
                                  "    WHERE t1.IdDescarga = 0 ) " +
                                  "SELECT CONCAT(ROW_NUMBER() OVER (ORDER BY Grupo, NomArchivo), '. ', DescripcionOriginal) AS Descripcion " +
                                  "  FROM CTE_Union  ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                foreach (DataRow row in dt.Rows)
                {
                    documentosSolicitados.Add(row["Descripcion"].ToString());
                }

                dbConn.Close();
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

            return documentosSolicitados;
        }

        private void ReplaceTextWithNewLines_Bk(DocumentFormat.OpenXml.Wordprocessing.Body body, string placeholder, List<string> values)
        {
            foreach (var paragraph in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>())
            {
                foreach (var text in paragraph.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
                {
                    if (text.Text.Contains(placeholder))
                    {
                        // Crear un nuevo Run para reemplazar el texto del marcador de posición
                        DocumentFormat.OpenXml.Wordprocessing.Run run = new DocumentFormat.OpenXml.Wordprocessing.Run();

                        // Añadir cada valor en la lista con un salto de línea
                        foreach (var value in values)
                        {
                            run.Append(new DocumentFormat.OpenXml.Wordprocessing.Text(value));
                            run.Append(new DocumentFormat.OpenXml.Wordprocessing.Break());      // Añadir un salto de línea
                        }

                        // Reemplazar el marcador de posición con el nuevo Run
                        text.Text = string.Empty;           // Limpiar el texto del marcador
                        text.Parent.InsertAfterSelf(run);   // Insertar el nuevo Run después del marcador
                    }
                }
            }
        }

        private void ReplaceTextWithNewLines(DocumentFormat.OpenXml.Wordprocessing.Body body, string placeholder, List<string> values, string fontFamily, int fontSize)
        {
            foreach (var paragraph in body.Descendants<DocumentFormat.OpenXml.Wordprocessing.Paragraph>())
            {
                foreach (var text in paragraph.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
                {
                    if (text.Text.Contains(placeholder))
                    {
                        // Crear un nuevo Run para reemplazar el texto del marcador de posición
                        DocumentFormat.OpenXml.Wordprocessing.Run run = new DocumentFormat.OpenXml.Wordprocessing.Run();

                        // Crear propiedades de estilo para el Run
                        DocumentFormat.OpenXml.Wordprocessing.RunProperties runProperties = new DocumentFormat.OpenXml.Wordprocessing.RunProperties();

                        // Asignar tipo de letra (font family)
                        DocumentFormat.OpenXml.Wordprocessing.RunFonts runFonts = new DocumentFormat.OpenXml.Wordprocessing.RunFonts() { Ascii = fontFamily, HighAnsi = fontFamily };
                        runProperties.Append(runFonts);

                        // Asignar tamaño de fuente
                        DocumentFormat.OpenXml.Wordprocessing.FontSize fontSizeValue = new DocumentFormat.OpenXml.Wordprocessing.FontSize() { Val = $"{fontSize * 2}" };    // Multiplicar por 2 para el formato de OpenXML
                        runProperties.AppendChild(fontSizeValue);

                        // Añadir las propiedades de estilo al Run
                        run.Append(runProperties);

                        // Añadir cada valor en la lista con un salto de línea
                        foreach (var value in values)
                        {
                            // Crear un nuevo Text para cada valor con su estilo
                            run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(value));
                            run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Break()); // Añadir un salto de línea
                            run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Break()); // Añadir un salto de línea
                        }

                        // Reemplazar el marcador de posición con el nuevo Run
                        text.Text = string.Empty; // Limpiar el texto del marcador
                        text.Parent.InsertAfterSelf(run); // Insertar el nuevo Run después del marcador
                    }
                }
            }
        }


        private List<string> ObtenerElementosSeleccionados()
        {
            List<string> seleccionados = new List<string>();

            try
            {

                foreach (GridViewRow row in grdSeccion_2.Rows)
                {
                    // Accede a cada CheckBox en la fila
                    CheckBox chkBox1 = (CheckBox)row.FindControl("ChBoxSeccion_2_1");
                    CheckBox chkBox2 = (CheckBox)row.FindControl("ChBoxSeccion_2_2");
                    CheckBox chkBox3 = (CheckBox)row.FindControl("ChBoxSeccion_2_3");

                    // Accede a los valores de las columnas según tu lógica
                    //string columna1 = ((DataBoundLiteralControl)row.Cells[0].Controls[0]).Text.Trim();
                    //string columna2 = ((DataBoundLiteralControl)row.Cells[2].Controls[0]).Text.Trim();
                    //string columna3 = ((DataBoundLiteralControl)row.Cells[4].Controls[0]).Text.Trim();

                    string columna1 = row.Cells[0].Text.Trim() == "&nbsp;" ? "" : Server.HtmlDecode(row.Cells[0].Text.Trim());  // Asumiendo que el texto está en la primera celda (Columna1)
                    string columna2 = row.Cells[2].Text.Trim() == "&nbsp;" ? "" : Server.HtmlDecode(row.Cells[2].Text.Trim());  // Asumiendo que el texto está en la tercera celda (Columna2)
                    string columna3 = row.Cells[4].Text.Trim() == "&nbsp;" ? "" : Server.HtmlDecode(row.Cells[4].Text.Trim());  // Asumiendo que el texto está en la quinta celda (Columna3)

                    if (chkBox1 != null && chkBox1.Checked)
                    {
                        seleccionados.Add(columna1);
                    }
                    if (chkBox2 != null && chkBox2.Checked)
                    {
                        seleccionados.Add(columna2);
                    }
                    if (chkBox3 != null && chkBox3.Checked)
                    {
                        seleccionados.Add(columna3);
                    }
                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

            return seleccionados;
        }

        protected void btnShowPanel0_Click(object sender, EventArgs e)
        {
            pnl0.Visible = !pnl0.Visible;   // Cambia la visibilidad del Panel 1 al contrario de su estado actual

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

        protected void btnShowPanel1_Click(object sender, EventArgs e)
        {
            pnl1.Visible = !pnl1.Visible;   // Cambia la visibilidad del Panel 1 al contrario de su estado actual

            if (pnl1.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel1.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl1.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel1.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl1.Visible = false;
            }
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
            pnl5.Visible = !pnl5.Visible;   // Cambia la visibilidad del Panel 3 al contrario de su estado actual

            if (pnl5.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel5.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl5.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel5.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl5.Visible = false;
            }
        }

        protected void btnShowPanel6_Click(object sender, EventArgs e)
        {
            pnl6.Visible = !pnl6.Visible;   // Cambia la visibilidad del Panel 3 al contrario de su estado actual

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
            pnl7.Visible = !pnl7.Visible;   // Cambia la visibilidad del Panel 3 al contrario de su estado actual

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

        protected void btnShowPanel13_Click(object sender, EventArgs e)
        {
            pnl13.Visible = !pnl13.Visible;   // Cambia la visibilidad del Panel 13 al contrario de su estado actual

            if (pnl13.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel13.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl13.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel13.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl13.Visible = false;
            }
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

        protected void btnShowPanel17_Click(object sender, EventArgs e)
        {
            pnl17.Visible = !pnl17.Visible;   // Cambia la visibilidad del Panel 17 al contrario de su estado actual

            if (pnl17.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel17.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl17.Visible = true;

                // habilitar controles
                // ddlSecciones.Enabled = true;
                ddlCoberturas.Enabled = true;

                TxtSeccion.Enabled = true;
                TxtNomCobertura.Enabled = true;
                TxtSeccion.Enabled = true;
                TxtRiesgo.Enabled = true;
                TxtSumaAsegurada.Enabled = true;
                TxtValSumaAsegurada.Enabled = true;
                TxtAgregado.Enabled = true;
                TxtValAgregado.Enabled = true;
                TxtSublimite.Enabled = true;
                TxtValSublimite.Enabled = true;
                TxtDeducible.Enabled = true;
                TxtValDeducible.Enabled = true;
                TxtCoaseguro.Enabled = true;
                TxtValCoaseguro.Enabled = true;
                TxtNotas.Enabled = true;
                chkAplicaSiniestro.Enabled = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel17.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl17.Visible = false;
            }
        }

        protected void btnShowPanel18_Click(object sender, EventArgs e)
        {
            if (Variables.wPrefijo_Aseguradora == "AZT")
            {
                pnl18.Visible = !pnl18.Visible;   // Cambia la visibilidad del Panel 18 al contrario de su estado actual

                if (pnl18.Visible)
                {
                    string flechaHaciaArriba = "\u25B2";
                    btnShowPanel18.Text = flechaHaciaArriba; // Flecha hacia arriba
                    pnl18.Visible = true;
                }
                else
                {
                    string flechaHaciaAbajo = "\u25BC";
                    btnShowPanel18.Text = flechaHaciaAbajo; // Flecha hacia abajo
                    pnl18.Visible = false;
                }
            }

            if (Variables.wPrefijo_Aseguradora == "BER")
            {
                pnl19.Visible = !pnl19.Visible;   // Cambia la visibilidad del Panel 19 al contrario de su estado actual

                if (pnl19.Visible)
                {
                    string flechaHaciaArriba = "\u25B2";
                    btnShowPanel18.Text = flechaHaciaArriba; // Flecha hacia arriba
                    pnl19.Visible = true;
                }
                else
                {
                    string flechaHaciaAbajo = "\u25BC";
                    btnShowPanel18.Text = flechaHaciaAbajo; // Flecha hacia abajo
                    pnl19.Visible = false;
                }
            }
        }

        protected void btnShowPanel20_Click(object sender, EventArgs e)
        {
            pnl20.Visible = !pnl20.Visible;   // Cambia la visibilidad del Panel 20 al contrario de su estado actual

            if (pnl20.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel20.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl20.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel20.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl20.Visible = false;
            }
        }

        protected void ddlTpoAsegurado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlConclusion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdCategorias_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdCategorias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Width = Unit.Pixel(150);     // DescCategoria
                e.Row.Cells[4].Width = Unit.Pixel(150);     // DescSeccion
                e.Row.Cells[5].Width = Unit.Pixel(25);      // ChBoxRow
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;     // IdDocumento
                e.Row.Cells[1].Visible = false;     // IdSeccion
                e.Row.Cells[2].Visible = false;     // IdCategoria
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;     // IdDocumento
                e.Row.Cells[1].Visible = false;     // IdSeccion
                e.Row.Cells[2].Visible = false;     // IdCategoria
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
                Delete_ITM_91();
                // Actualizar campo IdRegimen, IdConclusion, DomSiniestro 
                Update_ITM_70();
                // Insertar en ITM_91 (Tipo de Asegurado / Tpo de Estatus)
                Insert_ITM_91();

                // Agregar registros con los datos de cada cuaderno a cargar
                string sReferencia = Variables.wRef;
                int iSubReferencia = Variables.wSubRef;
                string IdAseguradora = Variables.wPrefijo_Aseguradora;
                int IdConclusion = Convert.ToInt32(ddlConclusion.SelectedValue);
                int IdRegimen = Convert.ToInt32(ddlTpoAsegurado.SelectedValue);

                if (Variables.wExiste == false)
                {
                    Add_tbDetalleCuadernos(sReferencia, iSubReferencia, IdAseguradora, IdConclusion, IdRegimen);
                }

                // Obtener_Valores_ChBox(grdSeccion_2, "ChBoxSeccion_2", 2, "ITM_82");
                // Obtener_Valores_ChBox(grdSeccion_4, "ChBoxSeccion_4", 4, "ITM_84");
                // Obtener_Valores_ChBox(grdSeccion_5, "ChBoxSeccion_5", 5, "ITM_85");

                // bool estadoBoton = BtnCrear_Cuaderno.Enabled;   // Variable para validar si el cuaderno existe

                // if (estadoBoton)
                // {
                //    // Eliminar Secciones 2, 4 y 5 de la tabla ITM_47
                //    Delete_ITM_47();

                //    // Agregar Secciones 2, 4 y 5 de la tabla ITM_47
                //    Add_tbDetalleCategorias(sReferencia, iSubReferencia, IdAseguradora, IdConclusion, IdRegimen);
                // }

                GetDocumentos(TxtSubReferencia.Text);

                LblMessage.Text = "Se han aplicado los cambios, correctamente";
                this.mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
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
        private void ActualizarDatosEnTabla(string Referencia, int SubReferencia, int idProyecto, string idCliente, int idSeccion, string idTpoAsunto, string IdUsuario,  string sTabla, List<string> selectedLabels)
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
                    //string strQuery = "INSERT INTO ITM_91 (Referencia, SubReferencia, IdCliente, IdProyecto, IdTpoAsunto, IdSeccion, " +
                    //                  "                    IdCategoria, IdDocumento, bSeleccion, IdUsuario )" +
                    //                  " VALUES (@Referencia, @SubReferencia, @idCliente, @idProyecto, @idTpoAsunto, @idSeccion, @IdCategoria, 1, 1, @IdUsuario) ";

                    string strQuery = "INSERT INTO ITM_91 (Referencia, SubReferencia, IdCliente, IdProyecto, IdTpoAsunto, IdSeccion, IdCategoria, " +
                                      "                    IdDocumento, bSeleccion, IdUsuario) " +
                                      "SELECT @Referencia, @SubReferencia, @idCliente, @idProyecto, @idTpoAsunto, @idSeccion, @IdCategoria, " +
                                      "       1, 1, @IdUsuario " +
                                      "FROM DUAL " +
                                      "WHERE NOT EXISTS ( " +
                                      "    SELECT 1 FROM ITM_91 " +
                                      "    WHERE Referencia = @Referencia " +
                                      "      AND SubReferencia = @SubReferencia " +
                                      "      AND IdSeccion = @idSeccion " +
                                      "      AND IdCategoria = @IdCategoria );";


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
                        Variables.wExiste = false;
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

        protected void BtnRegresa_Categorias_Click(object sender, EventArgs e)
        {
            Variables.wRef = string.Empty;
            Variables.wSubRef = 0;
            Variables.wIdProyecto = 0;
            Variables.wPrefijo_Aseguradora = string.Empty;
            Variables.wIdTpoAsunto = 0;

            Response.Redirect("fwReporte_Alta_Asunto.aspx", true);
        }

        protected void Select_ITM_91(string Referencia, int SubReferencia)
        {

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT IdSeccion, IdDocumento, bSeleccion FROM ITM_91 " +
                              "WHERE Referencia = @Referencia AND SubReferencia = @SubReferencia";

            // Usar la nueva función para ejecutar la consulta
            DataTable dt = dbConn.ExecuteQueryWithParameters(strQuery, cmd =>
            {
                cmd.Parameters.AddWithValue("@Referencia", Referencia);
                cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
            });


            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dbRow in dt.Rows)
                {
                    int idSeccion = Convert.ToInt32(dbRow["IdSeccion"]);
                    int idDocumento = Convert.ToInt32(dbRow["IdDocumento"]);
                    bool bSeleccion = Convert.ToBoolean(dbRow["bSeleccion"]);

                }
            }

            dbConn.Close();
        }

        protected void Update_ITM_70()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string DomSiniestro = string.Empty;

                // Actualizar registro(s) tablas (ITM_03, ITM_46, ITM_47, ITM_70)
                string strQuery = "UPDATE ITM_03 " +
                                  "   SET IdRegimen = " + ddlTpoAsegurado.SelectedValue + "," +
                                  "       IdConclusion = " + ddlConclusion.SelectedValue + " " +
                                  " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END) = '" + TxtSubReferencia.Text + "' ; ";

                strQuery += Environment.NewLine;

                strQuery += "UPDATE ITM_46 " +
                            "   SET IdConclusion = " + ddlConclusion.SelectedValue + " " +
                            " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END) = '" + TxtSubReferencia.Text + "' ; ";

                strQuery += Environment.NewLine;

                strQuery += "UPDATE ITM_47 " +
                            "   SET IdConclusion = " + ddlConclusion.SelectedValue + " " +
                            " WHERE (CASE WHEN SubReferencia >= 1 THEN CONCAT(UsReferencia, '-', SubReferencia) ELSE UsReferencia END) = '" + TxtSubReferencia.Text + "' ; ";

                strQuery += Environment.NewLine;

                strQuery += "UPDATE ITM_70 " +
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

        protected void Delete_Doc_Conclusion(int IdSeccion, int IdCategoria)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Verificar si hay registros en ITM_47 con IdDescarga = 0
                string checkQuery = "SELECT COUNT(*) FROM ITM_47 " +
                                    " WHERE UsReferencia = '" + Variables.wRef + "' " +
                                    "   AND SubReferencia = " + Variables.wSubRef + " " +
                                    "   AND IdSeccion = " + IdSeccion + " " +
                                    "   AND IdCategoria = " + IdCategoria + " " +
                                    "   AND IdDescarga = 1;";

                int count = 0;

                using (var reader = dbConn.ExecuteReaderQuery(checkQuery))
                {
                    if (reader.Read()) // Lee el resultado del COUNT
                    {
                        count = reader.GetInt32(0);
                    }

                    // Cerrar el DataReader
                    reader.Close();
                }

                if (count == 0)
                {

                    // Eliminar registro(s) tablas (ITM_91)
                    string strQuery = "DELETE FROM ITM_91 " +
                                    " WHERE Referencia = '" + Variables.wRef + "' " +
                                    "   AND SubReferencia = " + Variables.wSubRef + " " +
                                    "   AND IdSeccion = " + IdSeccion + " " +
                                    "   AND IdCategoria = " + IdCategoria + "; ";

                    strQuery += Environment.NewLine;

                    // Eliminar registro(s) tablas (ITM_47)
                    strQuery += "DELETE FROM ITM_47 " +
                                        " WHERE UsReferencia = '" + Variables.wRef + "' " +
                                        "   AND SubReferencia = " + Variables.wSubRef + " " +
                                        "   AND IdSeccion = " + IdSeccion + " " +
                                        "   AND IdCategoria = " + IdCategoria + " ";

                    int result = dbConn.ExecuteNonQuery(strQuery);

                    dbConn.Close();

                    LblMessage.Text = "Se elimino documento(s), correctamente";
                    mpeMensaje.Show();
                }
                else
                {
                    LblMessage.Text = "No se elimino documento(s), ya existen archivo(s)";
                    mpeMensaje.Show();
                }

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Insert_ITM_91()
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
                    strQuery = "INSERT INTO ITM_91 (Referencia, SubReferencia, IdProyecto, IdCliente, IdTpoAsunto, IdSeccion, IdCategoria, IdDocumento, bSeleccion, IdUsuario) " +
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
                        strQuery += "INSERT INTO ITM_91 (Referencia, SubReferencia, IdProyecto, IdCliente, IdTpoAsunto, IdSeccion, IdCategoria, IdDocumento, bSeleccion, IdUsuario) " +
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

        public void Add_tbDetalleCategorias(String pReferencia, int pSubReferencia, string pIdAseguradora, int pIdConclusion, int pIdRegimen)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Obtener los registros que corresponden al Tpo. de Cuaderno
                string strQuery = "SELECT t1.IdDoc_Categoria, t1.IdCliente, t1.IdProyecto, t1.IdTpoAsunto, t1.IdSeccion, " +
                                  "       t1.IdCategoria, t1.IdDocumento, t1.NomArchivo, t1.IdUsuario, t1.IdStatus " +
                                  " FROM ITM_88 AS t1, ITM_91 AS t2 " +
                                  " WHERE t2.Referencia = '" + pReferencia + "' " +
                                  "   AND t2.SubReferencia = " + pSubReferencia + " " +
                                  "   AND t1.IdProyecto = " + Variables.wIdProyecto + "" +
                                  "   AND t1.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND t1.IdSeccion IN (2, 4, 5) " +
                                  "   AND t1.IdProyecto = t2.IdProyecto AND t1.IdCliente = t2.IdCliente " +
                                  "   AND t1.IdSeccion = t2.IdSeccion AND t1.IdCategoria = t2.IdCategoria ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                foreach (DataRow row in dt.Rows)
                {
                    int IdTpoAsunto = Convert.ToInt32(row[3]);      // ITM_88
                    int IdSeccion = Convert.ToInt32(row[4]);        // ITM_88
                    int IdCategoria = Convert.ToInt32(row[5]);      // ITM_88
                    int IdDocumento = Convert.ToInt32(row[6]);      // ITM_88

                    int IdConclusion = Convert.ToInt32(ddlConclusion.SelectedValue);
                    int IdTpoDocumento = 1;
                    string sTpoArchivo = "PDF";     // PDF, MSG, XML
                    int IdDirectorio = 1;           // Pendiente

                    int IdDescarga = 0;
                    int IdStatus = 1;
                    int IdProyecto = Variables.wIdProyecto;

                    string sqlQuery = "INSERT INTO ITM_47 (UsReferencia, SubReferencia, IdProyecto, IdAseguradora, IdConclusion, IdTpoAsunto, IdTpoDocumento, TpoArchivo, IdSeccion, IdCategoria, IdDocumento, IdUsuario, Id_Directorio, Url_Archivo, Fec_Entrega, Fec_Rechazo, IdDescarga, IdStatus ) " +
                                      "VALUES ('" + pReferencia + "', " + pSubReferencia + ", " + IdProyecto + ", '" + pIdAseguradora + "', " + IdConclusion + ", " + IdTpoAsunto + ", " + IdTpoDocumento + ",  '" + sTpoArchivo + "', " + IdSeccion + ", " + IdCategoria + ", " + IdDocumento + ", NULL, " + IdDirectorio + ", NULL, NULL, NULL, " + IdDescarga + ", " + IdStatus + "); ";

                    // Insert en la tabla Estado de Documento
                    // MySqlDataReader reader = dbConn.ExecuteReaderQuery(sqlQuery);
                    int affectedRows = dbConn.ExecuteNonQuery(sqlQuery);
                }

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }
        }

        public void Add_tbDetCategoriaSeccion(String pReferencia, int pSubReferencia, string pIdAseguradora, int pIdConclusion, int pIdRegimen, int pIdSeccion)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Obtener los registros que corresponden al Tpo. de Cuaderno
                string strQuery = "SELECT t1.IdDoc_Categoria, t1.IdCliente, t1.IdProyecto, t1.IdTpoAsunto, t1.IdSeccion, " +
                                  "       t1.IdCategoria, t1.IdDocumento, t1.NomArchivo, t1.IdUsuario, t1.IdStatus " +
                                  " FROM ITM_88 AS t1, ITM_91 AS t2 " +
                                  " WHERE t2.Referencia = '" + pReferencia + "' " +
                                  "   AND t2.SubReferencia = " + pSubReferencia + " " +
                                  "   AND t1.IdProyecto = " + Variables.wIdProyecto + "" +
                                  "   AND t1.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND t1.IdSeccion = " + pIdSeccion + " " +
                                  "   AND t1.IdProyecto = t2.IdProyecto AND t1.IdCliente = t2.IdCliente " +
                                  "   AND t1.IdSeccion = t2.IdSeccion AND t1.IdCategoria = t2.IdCategoria " +
                                  "   AND NOT EXISTS ( " +
                                  "       SELECT 1 FROM ITM_47 AS t47 " +
                                  "       WHERE t47.UsReferencia = '" + pReferencia + "' " +
                                  "         AND t47.SubReferencia = " + pSubReferencia + " " +
                                  "         AND t47.IdSeccion = t2.IdSeccion " +
                                  "         AND t47.IdCategoria = t2.IdCategoria " +
                                  "   )";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                foreach (DataRow row in dt.Rows)
                {
                    int IdTpoAsunto = Convert.ToInt32(row[3]);      // ITM_88
                    int IdSeccion = Convert.ToInt32(row[4]);        // ITM_88
                    int IdCategoria = Convert.ToInt32(row[5]);      // ITM_88
                    int IdDocumento = Convert.ToInt32(row[6]);      // ITM_88

                    int IdConclusion = Convert.ToInt32(ddlConclusion.SelectedValue);
                    int IdTpoDocumento = 1;
                    string sTpoArchivo = "PDF";     // PDF, MSG, XML
                    int IdDirectorio = 1;           // Pendiente

                    int IdDescarga = 0;
                    int IdStatus = 1;
                    int IdProyecto = Variables.wIdProyecto;

                    string sqlQuery = "INSERT INTO ITM_47 (UsReferencia, SubReferencia, IdProyecto, IdAseguradora, IdConclusion, IdTpoAsunto, IdTpoDocumento, TpoArchivo, IdSeccion, IdCategoria, IdDocumento, IdUsuario, Id_Directorio, Url_Archivo, Fec_Entrega, Fec_Rechazo, IdDescarga, IdStatus ) " +
                                      "VALUES ('" + pReferencia + "', " + pSubReferencia + ", " + IdProyecto + ", '" + pIdAseguradora + "', " + IdConclusion + ", " + IdTpoAsunto + ", " + IdTpoDocumento + ",  '" + sTpoArchivo + "', " + IdSeccion + ", " + IdCategoria + ", " + IdDocumento + ", NULL, " + IdDirectorio + ", NULL, NULL, NULL, " + IdDescarga + ", " + IdStatus + "); ";

                    // Insert en la tabla Estado de Documento
                    int affectedRows = dbConn.ExecuteNonQuery(sqlQuery);
                }

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }
        }


        public void Add_tbDetalleCuadernos(String pReferencia, int pSubReferencia, string pIdAseguradora, int pIdConclusion, int pIdRegimen)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Obtener los registros que corresponden al Tpo. de Cuaderno
                string strQuery = "SELECT t1.IdDoc_Categoria, t1.IdCliente, t1.IdProyecto, t1.IdTpoAsunto, t1.IdSeccion, " +
                                  "       t1.IdCategoria, t1.IdDocumento, t1.NomArchivo, t1.IdUsuario, t1.IdStatus " +
                                  " FROM ITM_88 AS t1, ITM_91 AS t2 " +
                                  " WHERE t2.Referencia = '" + pReferencia + "' " +
                                  "   AND t2.SubReferencia = " + pSubReferencia + " " +
                                  "   AND t1.IdProyecto = " + Variables.wIdProyecto + "" +
                                  "   AND t1.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND t1.IdSeccion = 3 AND t1.IdCategoria = '" + pIdConclusion + "' " +
                                  "   AND t1.IdProyecto = t2.IdProyecto AND t1.IdCliente = t2.IdCliente " +
                                  "   AND t1.IdSeccion = t2.IdSeccion AND t1.IdCategoria = t2.IdCategoria ";
                // "   AND t1.IdTpoAsunto = t2.IdTpoAsunto ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                foreach (DataRow row in dt.Rows)
                {
                    int IdTpoAsunto = Convert.ToInt32(row[3]);      // ITM_88
                    int IdSeccion = Convert.ToInt32(row[4]);        // ITM_88
                    int IdCategoria = Convert.ToInt32(row[5]);      // ITM_88
                    int IdDocumento = Convert.ToInt32(row[6]);      // ITM_88

                    int IdConclusion = Convert.ToInt32(ddlConclusion.SelectedValue);
                    int IdTpoDocumento = 1;
                    string sTpoArchivo = "PDF";     // PDF, MSG, XML
                    int IdDirectorio = 1;           // Pendiente

                    int IdDescarga = 0;
                    int IdStatus = 1;
                    int IdProyecto = Variables.wIdProyecto;

                    string sqlQuery = "INSERT INTO ITM_47 (UsReferencia, SubReferencia, IdProyecto, IdAseguradora, IdConclusion, IdTpoAsunto, IdTpoDocumento, TpoArchivo, IdSeccion, IdCategoria, IdDocumento, IdUsuario, Id_Directorio, Url_Archivo, Fec_Entrega, Fec_Rechazo, IdDescarga, IdStatus ) " +
                                      "VALUES ('" + pReferencia + "', " + pSubReferencia + ", " + IdProyecto + ", '" + pIdAseguradora + "', " + IdConclusion + ", " + IdTpoAsunto + ", " + IdTpoDocumento + ",  '" + sTpoArchivo + "', " + IdSeccion + ", " + IdCategoria + ", " + IdDocumento + ", NULL, " + IdDirectorio + ", NULL, NULL, NULL, " + IdDescarga + ", " + IdStatus + "); ";

                    // Insert en la tabla Estado de Documento
                    // MySqlDataReader reader = dbConn.ExecuteReaderQuery(sqlQuery);
                    int affectedRows = dbConn.ExecuteNonQuery(sqlQuery);
                }

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }
        }

        public int Validar_Existe_Categoria(int IdSeccion, int IdCategoria)
        {

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT Referencia, SubReferencia, IdSeccion, IdCategoria " +
                              "  FROM ITM_91 t0" +
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

        protected void Delete_ITM_91()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_91)
                string strQuery = "DELETE FROM ITM_91 " +
                                  " WHERE Referencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = " + Variables.wSubRef + " " +
                                  "   AND IdSeccion NOT IN (2, 3, 4, 5) ";

                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Delete_Seccion(int IdSeccion)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "START TRANSACTION; " +

                                  "CREATE TEMPORARY TABLE bloques_a_eliminar AS " +
                                  "SELECT t91.Referencia, t91.SubReferencia, t91.IdSeccion, t91.IdCategoria " +
                                  "FROM ITM_91 t91 " +
                                  "LEFT JOIN ITM_47 t47 " +
                                  "       ON t47.UsReferencia = t91.Referencia " +
                                  "      AND t47.SubReferencia = t91.SubReferencia " +
                                  "      AND t47.IdSeccion = t91.IdSeccion " +
                                  "      AND t47.IdCategoria = t91.IdCategoria " +
                                  "WHERE t91.Referencia = '" + Variables.wRef + "' " +
                                  "  AND t91.SubReferencia = " + Variables.wSubRef + " " +
                                  "  AND t91.IdSeccion = " + IdSeccion + " " +
                                  "GROUP BY t91.Referencia, t91.SubReferencia, t91.IdSeccion, t91.IdCategoria " +
                                  "HAVING SUM(CASE WHEN t47.IdDescarga = 1 THEN 1 ELSE 0 END) = 0; " +

                                  "-- Eliminar de ITM_91 " +
                                  "DELETE t91 " +
                                  "FROM ITM_91 t91 " +
                                  "INNER JOIN bloques_a_eliminar b " +
                                  "  ON t91.Referencia = b.Referencia " +
                                  " AND t91.SubReferencia = b.SubReferencia " +
                                  " AND t91.IdSeccion = b.IdSeccion " +
                                  " AND t91.IdCategoria = b.IdCategoria; " +

                                  "-- Eliminar de ITM_47 " +
                                  "DELETE t47 " +
                                  "FROM ITM_47 t47 " +
                                  "INNER JOIN bloques_a_eliminar b " +
                                  "  ON t47.UsReferencia = b.Referencia " +
                                  " AND t47.SubReferencia = b.SubReferencia " +
                                  " AND t47.IdSeccion = b.IdSeccion " +
                                  " AND t47.IdCategoria = b.IdCategoria; " +

                                  "DROP TEMPORARY TABLE bloques_a_eliminar; " +

                                  "COMMIT;";

                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

        }

        protected void Delete_Seccion_ITM_91(int IdSeccion)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_91)
                //string strQuery = "DELETE FROM ITM_91 " +
                //                  " WHERE Referencia = '" + Variables.wRef + "' " +
                //                  "   AND SubReferencia = " + Variables.wSubRef + " " +
                //                  "   AND IdSeccion = " + IdSeccion + " ";

                string strQuery = "DELETE t91 " +
                                  "FROM ITM_91 AS t91 " +
                                  "INNER JOIN ( " +
                                  "    SELECT t91_sub.Referencia, t91_sub.SubReferencia, t91_sub.IdSeccion, t91_sub.IdCategoria " +
                                  "    FROM ITM_91 AS t91_sub " +
                                  "    LEFT JOIN ITM_47 AS t47 " +
                                  "           ON t47.UsReferencia = t91_sub.Referencia " +
                                  "          AND t47.SubReferencia = t91_sub.SubReferencia " +
                                  "          AND t47.IdSeccion = t91_sub.IdSeccion " +
                                  "          AND t47.IdCategoria = t91_sub.IdCategoria " +
                                  "          AND t47.IdDescarga = 1 " +
                                  "    WHERE t91_sub.Referencia = '" + Variables.wRef + "' " +
                                  "      AND t91_sub.SubReferencia = " + Variables.wSubRef + " " +
                                  "      AND t91_sub.IdSeccion = " + IdSeccion + " " +
                                  "    GROUP BY t91_sub.Referencia, t91_sub.SubReferencia, t91_sub.IdSeccion, t91_sub.IdCategoria " +
                                  "    HAVING COUNT(t47.IdCategoria) = 0 " +    // Ningún registro con IdDescarga = 1
                                  ") AS bloque " +
                                  "ON t91.Referencia = bloque.Referencia " +
                                  "AND t91.SubReferencia = bloque.SubReferencia " +
                                  "AND t91.IdSeccion = bloque.IdSeccion " +
                                  "AND t91.IdCategoria = bloque.IdCategoria ";

                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Delete_ITM_47()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_47)
                string strQuery = "DELETE FROM ITM_47 " +
                                  " WHERE UsReferencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = " + Variables.wSubRef + " " +
                                  "   AND IdSeccion IN (2, 4, 5) ";

                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Delete_Seccion_ITM_47(int IdSeccion)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_47)
                //string strQuery = "DELETE FROM ITM_47 " +
                //                  " WHERE UsReferencia = '" + Variables.wRef + "' " +
                //                  "   AND SubReferencia = " + Variables.wSubRef + " " +
                //                  "   AND IdSeccion = " + IdSeccion + " ";

                string strQuery = "DELETE t47 " +
                                       "FROM ITM_47 AS t47 " +
                                       "LEFT JOIN ITM_47 AS t47_check " +
                                       "  ON t47_check.UsReferencia = t47.UsReferencia " +
                                       " AND t47_check.SubReferencia = t47.SubReferencia " +
                                       " AND t47_check.IdSeccion = t47.IdSeccion " +
                                       " AND t47_check.IdCategoria = t47.IdCategoria " +
                                       " AND t47_check.IdDescarga = 1 " +
                                       "WHERE t47.UsReferencia = '" + Variables.wRef + "' " +
                                       "  AND t47.SubReferencia = " + Variables.wSubRef + " " +
                                       "  AND t47.IdSeccion = " + IdSeccion + " " +
                                       "  AND t47_check.IdCategoria IS NULL";


                int result = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
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

        protected void grdSeccion_4_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Encuentra los CheckBoxes dentro de la fila
                CheckBox chBoxSeccion_4_1 = (CheckBox)e.Row.FindControl("ChBoxSeccion_4_1");
                CheckBox chBoxSeccion_4_2 = (CheckBox)e.Row.FindControl("ChBoxSeccion_4_2");
                CheckBox chBoxSeccion_4_3 = (CheckBox)e.Row.FindControl("ChBoxSeccion_4_3");

                // Obtén los datos de la fila actual
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                // Obtén los valores de las columnas y conviértelos a tipo entero
                string valorColumna1 = Convert.ToString(rowView["Columna1"]);
                string valorColumna2 = Convert.ToString(rowView["Columna2"]);
                string valorColumna3 = Convert.ToString(rowView["Columna3"]);

                // Obtén el valor de la columna Columna1
                int valorChBoxSeccion_4_1 = Convert.ToInt32(rowView["ChBoxSeccion_4_1"]);
                int valorChBoxSeccion_4_2 = Convert.ToInt32(rowView["ChBoxSeccion_4_2"]);
                int valorChBoxSeccion_4_3 = Convert.ToInt32(rowView["ChBoxSeccion_4_3"]);

                // Verifica si la columna Columna1 tiene datos y muestra u oculta el CheckBox
                if (!string.IsNullOrEmpty(valorColumna1))
                {
                    chBoxSeccion_4_1.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_4_1.Checked = valorChBoxSeccion_4_1 == 1;
                }
                else
                {
                    chBoxSeccion_4_1.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna2))
                {
                    chBoxSeccion_4_2.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_4_2.Checked = valorChBoxSeccion_4_2 == 1;
                }
                else
                {
                    chBoxSeccion_4_2.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna3))
                {
                    chBoxSeccion_4_3.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_4_3.Checked = valorChBoxSeccion_4_3 == 1;
                }
                else
                {
                    chBoxSeccion_4_3.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
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

        protected void GetSeccion_2()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "  WITH CTE AS ( SELECT ITM_82.Descripcion AS Descripcion_82, ITM_86.IdDocumento, " +
                                  "                ROW_NUMBER() OVER (ORDER BY ITM_86.IdPosicion) AS RowNumber " +
                                  "  FROM ITM_82 " +
                                  " INNER JOIN ITM_86 ON ITM_82.IdDocumento = ITM_86.IdDocumento " +
                                  " WHERE ITM_82.IdStatus = 1 AND ITM_86.IdProyecto =  " + Variables.wIdProyecto + " AND ITM_86.bSeleccion = 1 " +
                                  "   AND ITM_86.IdSeccion = 2 AND ITM_86.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  " ), " +
                                  " Seleccion_91 AS ( SELECT IdCategoria, bSeleccion " +
                                  "  FROM ITM_91 " +
                                  " WHERE IdProyecto = " + Variables.wIdProyecto + " AND IdSeccion = 2 AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND Referencia = '" + Variables.wRef + "' AND SubReferencia = " + Variables.wSubRef + " ) " +
                                  "SELECT COALESCE(CTE1.Descripcion_82, '') AS Columna1, " +
                                  "       COALESCE(S91_1.bSeleccion, 0) AS ChBoxSeccion_2_1, " +
                                  "       COALESCE(CTE2.Descripcion_82, '') AS Columna2, " +
                                  "       COALESCE(S91_2.bSeleccion, 0) AS ChBoxSeccion_2_2," +
                                  "       COALESCE(CTE3.Descripcion_82, '') AS Columna3, " +
                                  "       COALESCE(S91_3.bSeleccion, 0) AS ChBoxSeccion_2_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN Seleccion_91 S91_1 ON CTE1.IdDocumento = S91_1.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN Seleccion_91 S91_2 ON CTE2.IdDocumento = S91_2.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                  "  LEFT JOIN Seleccion_91 S91_3 ON CTE3.IdDocumento = S91_3.IdCategoria " +
                                  " ORDER BY CTE1.RowNumber ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    grdSeccion_2.ShowHeaderWhenEmpty = true;
                    grdSeccion_2.EmptyDataText = "No hay resultados.";
                }

                grdSeccion_2.DataSource = dt;
                grdSeccion_2.DataBind();

                dbConn.Close();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetSeccion_4()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "  WITH CTE AS ( SELECT ITM_84.Descripcion AS Descripcion_84, ITM_86.IdDocumento, " +
                                  "                ROW_NUMBER() OVER (ORDER BY ITM_86.IdPosicion) AS RowNumber " +
                                  "  FROM ITM_84 " +
                                  " INNER JOIN ITM_86 ON ITM_84.IdDocumento = ITM_86.IdDocumento " +
                                  " WHERE ITM_84.IdStatus = 1 AND ITM_86.IdProyecto =  " + Variables.wIdProyecto + " AND ITM_86.bSeleccion = 1 " +
                                  "   AND ITM_86.IdSeccion = 4 AND ITM_86.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  " ), " +
                                  " Seleccion_91 AS ( SELECT IdCategoria, bSeleccion " +
                                  "  FROM ITM_91 " +
                                  " WHERE IdProyecto = " + Variables.wIdProyecto + " AND IdSeccion = 4 AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND Referencia = '" + Variables.wRef + "' AND SubReferencia = " + Variables.wSubRef + " ) " +
                                  "SELECT COALESCE(CTE1.Descripcion_84, '') AS Columna1, " +
                                  "       COALESCE(S91_1.bSeleccion, 0) AS ChBoxSeccion_4_1, " +
                                  "       COALESCE(CTE2.Descripcion_84, '') AS Columna2, " +
                                  "       COALESCE(S91_2.bSeleccion, 0) AS ChBoxSeccion_4_2," +
                                  "       COALESCE(CTE3.Descripcion_84, '') AS Columna3, " +
                                  "       COALESCE(S91_3.bSeleccion, 0) AS ChBoxSeccion_4_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN Seleccion_91 S91_1 ON CTE1.IdDocumento = S91_1.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN Seleccion_91 S91_2 ON CTE2.IdDocumento = S91_2.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                  "  LEFT JOIN Seleccion_91 S91_3 ON CTE3.IdDocumento = S91_3.IdCategoria " +
                                  " ORDER BY CTE1.RowNumber ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    grdSeccion_4.ShowHeaderWhenEmpty = true;
                    grdSeccion_4.EmptyDataText = "No hay resultados.";
                }

                grdSeccion_4.DataSource = dt;
                grdSeccion_4.DataBind();

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

                string strQuery = "  WITH CTE AS ( SELECT ITM_85.Descripcion AS Descripcion_85, ITM_86.IdDocumento, " +
                                  "                ROW_NUMBER() OVER (ORDER BY ITM_86.IdPosicion) AS RowNumber " +
                                  "  FROM ITM_85 " +
                                  " INNER JOIN ITM_86 ON ITM_85.IdDocumento = ITM_86.IdDocumento " +
                                  " WHERE ITM_85.IdStatus = 1 AND ITM_86.IdProyecto =  " + Variables.wIdProyecto + " AND ITM_86.bSeleccion = 1 " +
                                  "   AND ITM_86.IdSeccion = 5 AND ITM_86.IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  " ), " +
                                  " Seleccion_91 AS ( SELECT IdCategoria, bSeleccion " +
                                  "  FROM ITM_91 " +
                                  " WHERE IdProyecto = " + Variables.wIdProyecto + " AND IdSeccion = 5 AND IdCliente = '" + Variables.wPrefijo_Aseguradora + "' " +
                                  "   AND Referencia = '" + Variables.wRef + "' AND SubReferencia = " + Variables.wSubRef + " ) " +
                                  "SELECT COALESCE(CTE1.Descripcion_85, '') AS Columna1, " +
                                  "       COALESCE(S91_1.bSeleccion, 0) AS ChBoxSeccion_5_1, " +
                                  "       COALESCE(CTE2.Descripcion_85, '') AS Columna2, " +
                                  "       COALESCE(S91_2.bSeleccion, 0) AS ChBoxSeccion_5_2," +
                                  "       COALESCE(CTE3.Descripcion_85, '') AS Columna3, " +
                                  "       COALESCE(S91_3.bSeleccion, 0) AS ChBoxSeccion_5_3 " +
                                  "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                  "  LEFT JOIN Seleccion_91 S91_1 ON CTE1.IdDocumento = S91_1.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                  "  LEFT JOIN Seleccion_91 S91_2 ON CTE2.IdDocumento = S91_2.IdCategoria " +
                                  "  LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                  "  LEFT JOIN Seleccion_91 S91_3 ON CTE3.IdDocumento = S91_3.IdCategoria " +
                                  " ORDER BY CTE1.RowNumber ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    grdSeccion_5.ShowHeaderWhenEmpty = true;
                    grdSeccion_5.EmptyDataText = "No hay resultados.";
                }

                grdSeccion_5.DataSource = dt;
                grdSeccion_5.DataBind();

                dbConn.Close();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdDocumentos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //GrdDocumentos.PageIndex = e.NewPageIndex;

            //GetDocumentos(TxtSubReferencia.Text);
        }

        protected void GrdDocumentos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdDocumentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AbrirArchivo")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                // Recuperar llaves
                string urlArchivo = GrdDocumentos.DataKeys[index]["Url_Archivo"].ToString();
                string nomArchivo = GrdDocumentos.DataKeys[index]["Nom_Archivo"].ToString();

                string carpetaTemp = Server.MapPath("~/ArchivoTemporal/");

                //Limpiar todos los archivos existentes
                foreach (var archivo in Directory.GetFiles(carpetaTemp))
                {
                    try { File.Delete(archivo); }
                    catch { }
                }

                // foreach (var archivo in Directory.GetFiles(carpetaTemp))
                // {
                //     try
                //     {
                //         if (File.GetCreationTime(archivo) < DateTime.Now.AddMinutes(-10))
                //             File.Delete(archivo);
                //     }
                //     catch { }
                // }

                // Obtener la ruta de OneDrive o ubicación base
                Session["Url_OneDrive"] = Get_Url_OneDrive();
                string Url_OneDrive = Url_OneDrive_Path();

                // Ruta original del archivo
                string rutaOriginal = Path.Combine(Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + TxtSubReferencia.Text + "\\" + urlArchivo, nomArchivo);
                rutaOriginal = Server.HtmlDecode(rutaOriginal);

                // Nombre único para evitar colisiones
                string nombreArchivoTemp = Guid.NewGuid().ToString() + Path.GetExtension(rutaOriginal);

                // Ruta física en la carpeta temporal
                string rutaDestino = Path.Combine(carpetaTemp, nombreArchivoTemp);

                // Copiar el archivo a la carpeta temporal
                File.Copy(rutaOriginal, rutaDestino, true);

                // Codificar ruta para pasarla al handler
                string rutaParam = HttpUtility.UrlEncode(rutaDestino);

                // Construir URL hacia fwFileHandler
                string rutaHandler = ResolveUrl("~/fwFileHandler.ashx?path=" + rutaParam);

                // Abrir en nueva pestaña
                string script = $"window.open('{rutaHandler}', '_blank');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AbrirArchivo", script, true);
            }

        }

        protected void GrdDocumentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Recuperar valor IdDescarga
                string idDescarga = DataBinder.Eval(e.Row.DataItem, "IdDescarga").ToString();

                // Buscar el link en esta fila
                LinkButton lnk = (LinkButton)e.Row.FindControl("lnkTpoArchivo");

                if (idDescarga == "0")
                {
                    // Si no hay descarga, dejar solo texto plano
                    lnk.Enabled = false;
                    lnk.OnClientClick = ""; // prevenir scripts
                    lnk.CssClass = "text-muted"; // opcional, lo muestra gris

                    // alternativa: reemplazar el link por texto puro
                    lnk.Text = lnk.Text;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chBoxRow = (CheckBox)e.Row.FindControl("ChBoxRow");
                if (chBoxRow != null)
                {
                    // Añadir atributo onclick para prevenir cambios
                    chBoxRow.Attributes["onclick"] = "return false;";
                }
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[3].Visible = false;     // DocInterno
                e.Row.Cells[4].Visible = false;     // Url_Archivo
                e.Row.Cells[5].Visible = false;     // Nom_Archivo
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Visible = false;     // DocInterno
                e.Row.Cells[4].Visible = false;     // Url_Archivo
                e.Row.Cells[5].Visible = false;     // Nom_Archivo
            }
        }

        protected void BtnCartaSolicitud_Click(object sender, EventArgs e)
        {
            try
            {

                Dictionary<string, TextBox> fechas = new Dictionary<string, TextBox>
                    {
                        { "Fecha de Asignación", TxtFechaAsignacion },
                        { "Fecha de Reporte", TxtFechaReporte }
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

                Carta_Solicitud();

                // Crear instancia de la clase EnvioEmail
                EnvioEmail envioEmail = new EnvioEmail();

                string sPredeterminado = string.Empty;

                // Llamar al método CorreoElectronico
                string sEmail = envioEmail.CorreoElectronico(Variables.wRef);
                string sUsuario = envioEmail.CorreoElectronico_User(Variables.wUserName);

                // string Url_OneDrive = @"\Users\Dell\OneDrive - INSURANCE CLAIMS & RISKS MEXICO\";

                DateTime fecAsignacion = DateTime.ParseExact(TxtFechaAsignacion.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaAsignacion = fecAsignacion.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                DateTime fecReporte = DateTime.ParseExact(TxtFechaReporte.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string FechaReporte = fecAsignacion.ToString("dd 'de' MMMM 'de' yyyy", new System.Globalization.CultureInfo("es-ES"));

                string sAsunto = "SOLICITUD DE DOCUMENTOS | NOTIFICACIÓN " + TxtNumSiniestro.Text + " | " + TxtNomAsegurado.Text + " | " +  Variables.wRef;

                //sPredeterminado = "Estimado Ajustador: \n \n" +
                //                  "Por el presente estamos asignándole el Siniestro Tradicional que nos fue instruido \n" +
                //                  FechaAsignacion + ", cuyos detalles se encuentran a continuación: \n \n" +
                //                  "• " + Variables.wRef + " \n\n";

                sPredeterminado = @"
                                    <html>
                                    <body>
                                        <p>Estimado Ajustador:</p>
                                        <p>
                                            Por el presente estamos asignándole el Siniestro Tradicional que nos fue instruido <br />
                                            " + FechaAsignacion + @", cuyos detalles se encuentran a continuación:
                                        </p>
                                        <p>
                                            • &nbsp;&nbsp; " + Variables.wRef + @"
                                        </p>
                                    </body>
                                    </html>";

                //sPredeterminado += "1.            DATOS DEL SINIESTRO \n \n" +
                //                   "• Fecha de reporte: " + FechaReporte + " \n" +
                //                   "• Aseguradora: " + TxtSeguro_Cia.Text + " \n" +
                //                   "• Asegurado: " + TxtNomAsegurado.Text + " \n" +
                //                   "• Notificación: " + TxtNumSiniestro.Text + " \n" +
                //                   "• POLIZA: " + TxtNumPoliza.Text + " \n \n";

                sPredeterminado += @"
                                    <html>
                                    <body>
                                        <p><b>1.            DATOS DEL SINIESTRO</b></p>
                                        <p>
                                            • &nbsp;&nbsp; Fecha de reporte: " + FechaReporte + @"<br />
                                            • &nbsp;&nbsp; Aseguradora: " + TxtSeguro_Cia.Text + @"<br />
                                            • &nbsp;&nbsp; Asegurado: " + TxtNomAsegurado.Text + @"<br />
                                            • &nbsp;&nbsp; Notificación: " + TxtNumSiniestro.Text + @"<br />
                                            • &nbsp;&nbsp; POLIZA: " + TxtNumPoliza.Text + @"<br />
                                        </p>
                                    </body>
                                    </html>";

                //sPredeterminado += "2.            DESCRIPCIÓN DEL EVENTO: \n \n" +
                //                   "• SE DESCONOCE \n \n";

                sPredeterminado += @"
                                    <html>
                                    <body>
                                        <p><b>2.            DESCRIPCIÓN DEL EVENTO:</b></p>
                                        <p>• &nbsp;&nbsp; SE DESCONOCE</p>
                                    </body>
                                    </html>";

                //sPredeterminado += "3.            DATOS DE CONTACTO \n \n" +
                //                   "• " + TxtNomAsegurado.Text + " \n" +
                //                   "• " + TxtTel1_Contacto1.Text + " \n" +
                //                   "• " + TxtEmailContacto1.Text + " \n \n";


                sPredeterminado += @"
                                    <html>
                                    <body>
                                        <p><b>3.            DATOS DE CONTACTO</b></p>
                                        <p>
                                            • &nbsp;&nbsp; " + TxtNomAsegurado.Text + @"<br />
                                            • &nbsp;&nbsp; " + TxtTel1_Contacto1.Text + @"<br />
                                            • &nbsp;&nbsp; " + TxtEmailContacto1.Text + @"<br />
                                        </p>
                                    </body>
                                    </html>";

                //sPredeterminado += "Quedamos atentos a cualquier duda o comentario y nos reiteramos a tus órdenes. \n \n" +
                //                   "Saludos. \n";

                sPredeterminado += @"
                                    <html>
                                    <body>
                                        <p>Quedamos atentos a cualquier duda o comentario y nos reiteramos a tus órdenes.</p>
                                        <p>Saludos.</p>
                                    </body>
                                    </html>";

                string sBody = sPredeterminado;

                // *** Envio de correo electronico ***
                //string Nom_Documento = "CSD_" + Variables.wRef + ".docx";
                //string wAdjunto = Url_OneDrive + Variables.wDesc_Aseguradora + "\\" + Variables.wRef + "\\" + Nom_Documento;

                //envioEmail.EnvioMensaje(TxtEmailContacto1.Text, sAsunto, sBody, wAdjunto);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnCrear_Cuaderno_Click(object sender, EventArgs e)
        {
            LblMessage_3.Text = "¿Desea crear el cuaderno? <br />  (Verifique de que sus documentos sean correctos).";
            mpeMensaje_3.Show();
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sEstado = ddlEstado.SelectedValue;
            GetMunicipios(sEstado);
        }

        protected void ddlEstadoContratante_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sEstado = ddlEstadoContratante.SelectedValue;
            GetMunicipiosContratante(sEstado);
        }

        protected void ddlEstadoAsegurado1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sEstado = ddlEstadoAsegurado1.SelectedValue;
            GetMunicipiosAsegurado1(sEstado);
        }

        protected void ddlEstadoAsegurado2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sEstado = ddlEstadoAsegurado2.SelectedValue;
            GetMunicipiosAsegurado2(sEstado);
        }

        protected void ddlEstadoAsegurado3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sEstado = ddlEstadoAsegurado3.SelectedValue;
            GetMunicipiosAsegurado3(sEstado);
        }

        protected void ddlEstadoBienAsegurado_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sEstado = ddlEstadoBienAsegurado.SelectedValue;
            GetMunicipiosBienAsegurado(sEstado);
        }

        protected void ddlMunicipios_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void ddlMunicipiosContratante_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlMunicipiosAsegurado1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlMunicipiosAsegurado2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlMunicipiosAsegurado3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlMunicipiosBienAsegurado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnAddCodigoPostal_Click(object sender, EventArgs e)
        {

        }


        protected void BtnAnularPnl2_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            inhabilitar(this.Controls);

            // habilitar (Configuracion Siniestro)
            habilitar_Config_Siniestro();

            btnEditarPnl2.Visible = true;
            btnActualizarPnl2.Visible = false;
            BtnAnularPnl2.Visible = false;
        }

        protected void btnEditarPnl2_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl2
            TxtAntReferencia.Enabled = true;
            TxtNumSiniestro.Enabled = true;
            TxtNumReporte.Enabled = true;
            TxtNumPoliza.Enabled = true;
            TxtNomAsegurado.Enabled = true;
            TxtBenef_Preferente.Enabled = true;
            TxtEstOcurrencia.Enabled = true;
            TxtDescMote.Enabled = true;
            TxtFechaOcurrencia.Enabled = true;
            TxtFechaReporte.Enabled = true;
            TxtFechaAsignacion.Enabled = true;
            TxtFechaInspeccion.Enabled = true;
            TxtHoraAsignacion.Enabled = true;
            TxtFechaContacto.Enabled = true;
            TxtDetalleReporte.Enabled = true;
            TxtCalle.Enabled = true;
            TxtNumExterior.Enabled = true;
            TxtNumInterior.Enabled = true;
            ddlProductos.Enabled = true;
            ddlEstado.Enabled = true;
            ddlMunicipios.Enabled = true;
            TxtColonia.Enabled = true;
            TxtCodigoPostal.Enabled = true;
            TxtOtrosGeneral.Enabled = true;

            if (Variables.wPrefijo_Aseguradora == "AZT")
            {
                TxtNomRep_1.Enabled = true;
                TxtPtoRep_1.Enabled = true;
                TxtNomRep_2.Enabled = true;
                TxtPtoRep_2.Enabled = true;
                TxtDetDivision.Enabled = true;
                TxtNomAfectado_1.Enabled = true;
                // TxtNomAfectado_2.Enabled = true;
                TxtNomProveedor.Enabled = true;
                TxtDependencia.Enabled = true;
            }
            if (Variables.wPrefijo_Aseguradora == "BER")
            {
                TxtNomBeneficiario.Enabled = true;
            }

            btnEditarPnl2.Visible = false;
            btnActualizarPnl2.Visible = true;
            BtnAnularPnl2.Visible = true;
        }

        protected void btnActualizarPnl2_Click(object sender, EventArgs e)
        {
            //if (Page.IsValid)
            //{

            //}

            if ( TxtNomAfectado_1.Text != "" && TxtNomProveedor.Text != "")
            {
                LblMessage.Text = "Capturar solo Nombre Afectado ó Proveedor.";
                this.mpeMensaje.Show();

                return;
            }

            string input = TxtHoraAsignacion.Text;
            if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^(?:[01]\d|2[0-3]):[0-5]\d$"))
            {
                // Maneja la entrada inválida.
                // Por ejemplo, muestra un mensaje de error al usuario.
                LblMessage.Text = "Formato Hora inválido. Use hh:mm.";
                this.mpeMensaje.Show();
            } else
            {
                Actualizar_ITM_70();

                Actualizar_Datos_Generales();

                if (Variables.wPrefijo_Aseguradora == "AZT" || Variables.wPrefijo_Aseguradora == "BER")
                {
                    Actualizar_Datos_Dependencia();
                }

                inhabilitar(this.Controls);

                // habilitar (Configuracion Siniestro)
                habilitar_Config_Siniestro();

                btnEditarPnl2.Visible = true;
                btnActualizarPnl2.Visible = false;
                BtnAnularPnl2.Visible = false;

                LblMessage.Text = "Se han aplicado los cambios, correctamente";
                this.mpeMensaje.Show();
            }

        }

        protected void Actualizar_ITM_70()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Actualizar registro tabla (ITM_70)
                string strQuery = "UPDATE ITM_70 " +
                                  "   SET Referencia_Anterior = '" + TxtAntReferencia.Text.Trim() + "', " +
                                  "       NumSiniestro = '" + TxtNumSiniestro.Text.Trim() + "', " +
                                  "       NumPoliza =  '" + TxtNumPoliza.Text.Trim() + "', " +
                                  "       NumReporte =  '" + TxtNumReporte.Text.Trim() + "', " +
                                  "       NomAsegurado = '" + TxtNomAsegurado.Text.Trim() + "', " +
                                  "       NomBeneficiario = '" + TxtNomBeneficiario.Text.Trim() + "', " +
                                  "       EstOcurrencia = '" + TxtEstOcurrencia.Text.Trim() + "', " +
                                  "       DescMote = '" + TxtDescMote.Text.Trim() + "', " +
                                  "       NomBenefPreferente = '" + TxtBenef_Preferente.Text.Trim() + "' " +
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

        protected void Actualizar_Datos_Generales()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Informacion General
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;
                string IdProducto = ddlProductos.SelectedValue;
                string NomAsegurado = TxtNomAsegurado.Text;
                string Fec_Ocurrencia = TxtFechaOcurrencia.Text;
                string Fec_Reporte = TxtFechaReporte.Text;
                string Fec_Asignacion = TxtFechaAsignacion.Text;
                string Fec_Inspeccion = TxtFechaInspeccion.Text;
                string Fec_Contacto = TxtFechaContacto.Text;
                string Hora_Asignacion = TxtHoraAsignacion.Text;
                string Detalle_Reporte = TxtDetalleReporte.Text;
                string Calle = TxtCalle.Text;
                string Num_Exterior = TxtNumExterior.Text;
                string Num_Interior = TxtNumInterior.Text;
                string Estado = ddlEstado.SelectedValue;
                string Delegacion = ddlMunicipios.SelectedValue;
                string Colonia = TxtColonia.Text;
                string Codigo_Postal = TxtCodigoPostal.Text;
                string Otros_General = TxtOtrosGeneral.Text;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @" INSERT INTO ITM_70_1 (Referencia, SubReferencia, IdProducto, Fec_Ocurrencia, Fec_Reporte, Fec_Asignacion, Fec_Inspeccion, Fec_Contacto, Hora_Asignacion, " +
                                    "                       Detalle_Reporte, Calle, Num_Exterior, Num_Interior, Estado, Delegacion, Colonia, Codigo_Postal, Otros_General, " +
                                    "                       Id_Usuario, IdStatus)" +
                                    " VALUES (@Referencia, @SubReferencia, @IdProducto, @Fec_Ocurrencia, @Fec_Reporte, @Fec_Asignacion, @Fec_Inspeccion, @Fec_Contacto, @Hora_Asignacion, " +
                                    "        @Detalle_Reporte, @Calle, @Num_Exterior, @Num_Interior, @Estado, @Delegacion, @Colonia, @Codigo_Postal, @Otros_General, " +
                                    "        @Id_Usuario, @IdStatus)" +
                                    " ON DUPLICATE KEY UPDATE " +
                                    "    IdProducto = VALUES(IdProducto), " +
                                    "    Fec_Ocurrencia = VALUES(Fec_Ocurrencia), " +
                                    "    Fec_Reporte = VALUES(Fec_Reporte), " +
                                    "    Fec_Asignacion = VALUES(Fec_Asignacion), " +
                                    "    Fec_Inspeccion = VALUES(Fec_Inspeccion), " +
                                    "    Fec_Contacto = VALUES(Fec_Contacto), " +
                                    "    Hora_Asignacion = VALUES(Hora_Asignacion), " +
                                    "    Detalle_Reporte = VALUES(Detalle_Reporte), " +
                                    "    Calle = VALUES(Calle), " +
                                    "    Num_Exterior = VALUES(Num_Exterior), " +
                                    "    Num_Interior = VALUES(Num_Interior), " +
                                    "    Estado = VALUES(Estado), " +
                                    "    Delegacion = VALUES(Delegacion), " +
                                    "    Colonia = VALUES(Colonia), " +
                                    "    Codigo_Postal = VALUES(Codigo_Postal), " +
                                    "    Otros_General = VALUES(Otros_General), " +
                                    "    Id_Usuario = VALUES(Id_Usuario), " +
                                    "    IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@IdProducto", IdProducto);
                    cmd.Parameters.AddWithValue("@Fec_Ocurrencia", Fec_Ocurrencia);
                    cmd.Parameters.AddWithValue("@Fec_Reporte", Fec_Reporte);
                    cmd.Parameters.AddWithValue("@Fec_Asignacion", Fec_Asignacion);
                    cmd.Parameters.AddWithValue("@Fec_Inspeccion", Fec_Inspeccion);
                    cmd.Parameters.AddWithValue("@Fec_Contacto", Fec_Contacto);
                    cmd.Parameters.AddWithValue("@Hora_Asignacion", Hora_Asignacion);
                    cmd.Parameters.AddWithValue("@Detalle_Reporte", Detalle_Reporte);
                    cmd.Parameters.AddWithValue("@Calle", Calle);
                    cmd.Parameters.AddWithValue("@Num_Exterior", Num_Exterior);
                    cmd.Parameters.AddWithValue("@Num_Interior", Num_Interior);
                    cmd.Parameters.AddWithValue("@Estado", Estado);
                    cmd.Parameters.AddWithValue("@Delegacion", Delegacion);
                    cmd.Parameters.AddWithValue("@Colonia", Colonia);
                    cmd.Parameters.AddWithValue("@Codigo_Postal", Codigo_Postal);
                    cmd.Parameters.AddWithValue("@Otros_General", Otros_General);
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

        protected void Actualizar_Datos_Dependencia()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Informacion General
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                string Nom_Rep_1 = TxtNomRep_1.Text;
                string Puesto_Rep_1 = TxtPtoRep_1.Text;
                string Nom_Rep_2 = TxtNomRep_2.Text;
                string Puesto_Rep_2 = TxtPtoRep_2.Text;
                string Det_Division = TxtDetDivision.Text;
                string Nom_Afectado_1 = TxtNomAfectado_1.Text;
                string Nom_Afectado_2 = string.Empty;
                string Nom_Proveedor = TxtNomProveedor.Text;
                string Dependencia = TxtDependencia.Text;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @" INSERT INTO ITM_70_4 (Referencia, SubReferencia, Nom_Rep_1, Puesto_Rep_1, Nom_Rep_2, Puesto_Rep_2, Det_Division, Nom_Afectado_1, Nom_Afectado_2, " +
                                    "                       Nom_Proveedor, Dependencia, Id_Usuario, IdStatus)" +
                                    " VALUES (@Referencia, @SubReferencia, @Nom_Rep_1, @Puesto_Rep_1, @Nom_Rep_2, @Puesto_Rep_2, @Det_Division, @Nom_Afectado_1,  @Nom_Afectado_2, " +
                                    "         @Nom_Proveedor, @Dependencia, @Id_Usuario, @IdStatus)" +
                                    " ON DUPLICATE KEY UPDATE " +
                                    "    Nom_Rep_1 = VALUES(Nom_Rep_1), " +
                                    "    Puesto_Rep_1 = VALUES(Puesto_Rep_1), " +
                                    "    Nom_Rep_2 = VALUES(Nom_Rep_2), " +
                                    "    Puesto_Rep_2 = VALUES(Puesto_Rep_2), " +
                                    "    Det_Division = VALUES(Det_Division), " +
                                    "    Nom_Afectado_1 = VALUES(Nom_Afectado_1), " +
                                    "    Nom_Afectado_2 = VALUES(Nom_Afectado_2), " +
                                    "    Nom_Proveedor = VALUES(Nom_Proveedor), " +
                                    "    Dependencia = VALUES(Dependencia), " +
                                    "    Id_Usuario = VALUES(Id_Usuario), " +
                                    "    IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@Nom_Rep_1", Nom_Rep_1);
                    cmd.Parameters.AddWithValue("@Puesto_Rep_1", Puesto_Rep_1);
                    cmd.Parameters.AddWithValue("@Nom_Rep_2", Nom_Rep_2);
                    cmd.Parameters.AddWithValue("@Puesto_Rep_2", Puesto_Rep_2);
                    cmd.Parameters.AddWithValue("@Det_Division", Det_Division);
                    cmd.Parameters.AddWithValue("@Nom_Afectado_1", Nom_Afectado_1);
                    cmd.Parameters.AddWithValue("@Nom_Afectado_2", Nom_Afectado_2);
                    cmd.Parameters.AddWithValue("@Nom_Proveedor", Nom_Proveedor);
                    cmd.Parameters.AddWithValue("@Dependencia", Dependencia);
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

        protected void BtnAnularPnl3_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            inhabilitar(this.Controls);

            // habilitar (Configuracion Siniestro)
            habilitar_Config_Siniestro();

            btnEditarPnl3.Visible = true;
            btnActualizarPnl3.Visible = false;
            BtnAnularPnl3.Visible = false;
        }

        protected void btnEditarPnl3_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl3
            // Contacto 1
            TxtNomContacto1.Enabled = true;
            TxtTpoContacto1.Enabled = true;
            TxtTel1_Contacto1.Enabled = true;
            TxtTel2_Contacto1.Enabled = true;
            TxtTel3_Contacto1.Enabled = true;
            TxtTel4_Contacto1.Enabled = true;
            TxtEmailContacto1.Enabled = true;
            TxtDetalleContacto1.Enabled = true;
            // Contacto 2
            TxtNomContacto2.Enabled = true;
            TxtTpoContacto2.Enabled = true;
            TxtTel1_Contacto2.Enabled = true;
            TxtTel2_Contacto2.Enabled = true;
            TxtTel3_Contacto2.Enabled = true;
            TxtTel4_Contacto2.Enabled = true;
            TxtEmailContacto2.Enabled = true;
            TxtDetalleContacto2.Enabled = true;
            // Contacto 3
            TxtNomContacto3.Enabled = true;
            TxtTpoContacto3.Enabled = true;
            TxtTel1_Contacto3.Enabled = true;
            TxtTel2_Contacto3.Enabled = true;
            TxtTel3_Contacto3.Enabled = true;
            TxtTel4_Contacto3.Enabled = true;
            TxtEmailContacto3.Enabled = true;
            TxtDetalleContacto3.Enabled = true;
            // Contacto 4
            TxtNomContacto4.Enabled = true;
            TxtTpoContacto4.Enabled = true;
            TxtTel1_Contacto4.Enabled = true;
            TxtTel2_Contacto4.Enabled = true;
            TxtTel3_Contacto4.Enabled = true;
            TxtTel4_Contacto4.Enabled = true;
            TxtEmailContacto4.Enabled = true;
            TxtDetalleContacto4.Enabled = true;

            btnEditarPnl3.Visible = false;
            btnActualizarPnl3.Visible = true;
            BtnAnularPnl3.Visible = true;
        }

        protected void btnActualizarPnl3_Click(object sender, EventArgs e)
        {
            Actualizar_Datos_Personales();

            inhabilitar(this.Controls);

            // habilitar (Configuracion Siniestro)
            habilitar_Config_Siniestro();

            btnEditarPnl3.Visible = true;
            btnActualizarPnl3.Visible = false;
            BtnAnularPnl3.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios, correctamente";
            this.mpeMensaje.Show();
        }

        protected void Actualizar_Datos_Personales()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Datos Personales
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                string Nom_Contacto_1 = TxtNomContacto1.Text;
                string Tipo_Contacto_1 = TxtTpoContacto1.Text;
                string Tel1_Contacto_1 = TxtTel1_Contacto1.Text;
                string Tel2_Contacto_1 = TxtTel2_Contacto1.Text;
                string Tel3_Contacto_1 = TxtTel3_Contacto1.Text;
                string Tel4_Contacto_1 = TxtTel4_Contacto1.Text;
                string Email_Contacto_1 = TxtEmailContacto1.Text;
                string Detalle_Contacto_1 = TxtDetalleContacto1.Text;

                string Nom_Contacto_2 = TxtNomContacto2.Text;
                string Tipo_Contacto_2 = TxtTpoContacto2.Text;
                string Tel1_Contacto_2 = TxtTel1_Contacto2.Text;
                string Tel2_Contacto_2 = TxtTel2_Contacto2.Text;
                string Tel3_Contacto_2 = TxtTel3_Contacto2.Text;
                string Tel4_Contacto_2 = TxtTel4_Contacto2.Text;
                string Email_Contacto_2 = TxtEmailContacto2.Text;
                string Detalle_Contacto_2 = TxtDetalleContacto2.Text;

                string Nom_Contacto_3 = TxtNomContacto3.Text;
                string Tipo_Contacto_3 = TxtTpoContacto3.Text;
                string Tel1_Contacto_3 = TxtTel1_Contacto3.Text;
                string Tel2_Contacto_3 = TxtTel2_Contacto3.Text;
                string Tel3_Contacto_3 = TxtTel3_Contacto3.Text;
                string Tel4_Contacto_3 = TxtTel4_Contacto3.Text;
                string Email_Contacto_3 = TxtEmailContacto3.Text;
                string Detalle_Contacto_3 = TxtDetalleContacto3.Text;

                string Nom_Contacto_4 = TxtNomContacto4.Text;
                string Tipo_Contacto_4 = TxtTpoContacto4.Text;
                string Tel1_Contacto_4 = TxtTel1_Contacto4.Text;
                string Tel2_Contacto_4 = TxtTel2_Contacto4.Text;
                string Tel3_Contacto_4 = TxtTel3_Contacto4.Text;
                string Tel4_Contacto_4 = TxtTel4_Contacto4.Text;
                string Email_Contacto_4 = TxtEmailContacto4.Text;
                string Detalle_Contacto_4 = TxtDetalleContacto4.Text;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @"INSERT INTO ITM_70_2 (Referencia, SubReferencia, " +
                                 "           Nom_Contacto_1, Tipo_Contacto_1, Tel1_Contacto_1, Tel2_Contacto_1, Tel3_Contacto_1, Tel4_Contacto_1, " +
                                 "           Email_Contacto_1, Detalle_Contacto_1, " +
                                 "           Nom_Contacto_2, Tipo_Contacto_2, Tel1_Contacto_2, Tel2_Contacto_2, Tel3_Contacto_2, Tel4_Contacto_2, " +
                                 "           Email_Contacto_2, Detalle_Contacto_2, " +
                                 "           Nom_Contacto_3, Tipo_Contacto_3, Tel1_Contacto_3, Tel2_Contacto_3, Tel3_Contacto_3, Tel4_Contacto_3, " +
                                 "           Email_Contacto_3, Detalle_Contacto_3, " +
                                 "           Nom_Contacto_4, Tipo_Contacto_4, Tel1_Contacto_4, Tel2_Contacto_4, Tel3_Contacto_4, Tel4_Contacto_4, " +
                                 "           Email_Contacto_4, Detalle_Contacto_4, Id_Usuario, IdStatus) " +
                                 "  VALUES (@Referencia, @SubReferencia, " +
                                 "          @Nom_Contacto_1, @Tipo_Contacto_1, @Tel1_Contacto_1, @Tel2_Contacto_1, @Tel3_Contacto_1, @Tel4_Contacto_1, " +
                                 "          @Email_Contacto_1, @Detalle_Contacto_1, " +
                                 "          @Nom_Contacto_2, @Tipo_Contacto_2, @Tel1_Contacto_2, @Tel2_Contacto_2, @Tel3_Contacto_2, @Tel4_Contacto_2, " +
                                 "          @Email_Contacto_2, @Detalle_Contacto_2, " +
                                 "          @Nom_Contacto_3, @Tipo_Contacto_3, @Tel1_Contacto_3, @Tel2_Contacto_3, @Tel3_Contacto_3, @Tel4_Contacto_3, " +
                                 "          @Email_Contacto_3, @Detalle_Contacto_3, " +
                                 "          @Nom_Contacto_4, @Tipo_Contacto_4, @Tel1_Contacto_4, @Tel2_Contacto_4, @Tel3_Contacto_4, @Tel4_Contacto_4, " +
                                 "          @Email_Contacto_4, @Detalle_Contacto_4, @Id_Usuario, @IdStatus) " +
                                 "  ON DUPLICATE KEY UPDATE " +
                                 "Nom_Contacto_1 = VALUES(Nom_Contacto_1), " +
                                 "Tipo_Contacto_1 = VALUES(Tipo_Contacto_1), " +
                                 "Tel1_Contacto_1 = VALUES(Tel1_Contacto_1), " +
                                 "Tel2_Contacto_1 = VALUES(Tel2_Contacto_1), " +
                                 "Tel3_Contacto_1 = VALUES(Tel3_Contacto_1), " +
                                 "Tel4_Contacto_1 = VALUES(Tel4_Contacto_1), " +
                                 "Email_Contacto_1 = VALUES(Email_Contacto_1), " +
                                 "Detalle_Contacto_1 = VALUES(Detalle_Contacto_1), " +
                                 "Nom_Contacto_2 = VALUES(Nom_Contacto_2), " +
                                 "Tipo_Contacto_2 = VALUES(Tipo_Contacto_2), " +
                                 "Tel1_Contacto_2 = VALUES(Tel1_Contacto_2), " +
                                 "Tel2_Contacto_2 = VALUES(Tel2_Contacto_2), " +
                                 "Tel3_Contacto_2 = VALUES(Tel3_Contacto_2), " +
                                 "Tel4_Contacto_2 = VALUES(Tel4_Contacto_2), " +
                                 "Email_Contacto_2 = VALUES(Email_Contacto_2), " +
                                 "Detalle_Contacto_2 = VALUES(Detalle_Contacto_2), " +
                                 "Nom_Contacto_3 = VALUES(Nom_Contacto_3), " +
                                 "Tipo_Contacto_3 = VALUES(Tipo_Contacto_3), " +
                                 "Tel1_Contacto_3 = VALUES(Tel1_Contacto_3), " +
                                 "Tel2_Contacto_3 = VALUES(Tel2_Contacto_3), " +
                                 "Tel3_Contacto_3 = VALUES(Tel3_Contacto_3), " +
                                 "Tel4_Contacto_3 = VALUES(Tel4_Contacto_3), " +
                                 "Email_Contacto_3 = VALUES(Email_Contacto_3), " +
                                 "Detalle_Contacto_3 = VALUES(Detalle_Contacto_3), " +
                                 "Nom_Contacto_4 = VALUES(Nom_Contacto_4), " +
                                 "Tipo_Contacto_4 = VALUES(Tipo_Contacto_4), " +
                                 "Tel1_Contacto_4 = VALUES(Tel1_Contacto_4), " +
                                 "Tel2_Contacto_4 = VALUES(Tel2_Contacto_4), " +
                                 "Tel3_Contacto_4 = VALUES(Tel3_Contacto_4), " +
                                 "Tel4_Contacto_4 = VALUES(Tel4_Contacto_4), " +
                                 "Email_Contacto_4 = VALUES(Email_Contacto_4), " +
                                 "Detalle_Contacto_4 = VALUES(Detalle_Contacto_4), " +
                                 "Id_Usuario = VALUES(Id_Usuario), " +
                                 "IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@Nom_Contacto_1", Nom_Contacto_1);
                    cmd.Parameters.AddWithValue("@Tipo_Contacto_1", Tipo_Contacto_1);
                    cmd.Parameters.AddWithValue("@Tel1_Contacto_1", Tel1_Contacto_1);
                    cmd.Parameters.AddWithValue("@Tel2_Contacto_1", Tel2_Contacto_1);
                    cmd.Parameters.AddWithValue("@Tel3_Contacto_1", Tel3_Contacto_1);
                    cmd.Parameters.AddWithValue("@Tel4_Contacto_1", Tel4_Contacto_1);
                    cmd.Parameters.AddWithValue("@Email_Contacto_1", Email_Contacto_1);
                    cmd.Parameters.AddWithValue("@Detalle_Contacto_1", Detalle_Contacto_1);

                    cmd.Parameters.AddWithValue("@Nom_Contacto_2", Nom_Contacto_2);
                    cmd.Parameters.AddWithValue("@Tipo_Contacto_2", Tipo_Contacto_2);
                    cmd.Parameters.AddWithValue("@Tel1_Contacto_2", Tel1_Contacto_2);
                    cmd.Parameters.AddWithValue("@Tel2_Contacto_2", Tel2_Contacto_2);
                    cmd.Parameters.AddWithValue("@Tel3_Contacto_2", Tel3_Contacto_2);
                    cmd.Parameters.AddWithValue("@Tel4_Contacto_2", Tel4_Contacto_2);
                    cmd.Parameters.AddWithValue("@Email_Contacto_2", Email_Contacto_2);
                    cmd.Parameters.AddWithValue("@Detalle_Contacto_2", Detalle_Contacto_2);

                    cmd.Parameters.AddWithValue("@Nom_Contacto_3", Nom_Contacto_3);
                    cmd.Parameters.AddWithValue("@Tipo_Contacto_3", Tipo_Contacto_3);
                    cmd.Parameters.AddWithValue("@Tel1_Contacto_3", Tel1_Contacto_3);
                    cmd.Parameters.AddWithValue("@Tel2_Contacto_3", Tel2_Contacto_3);
                    cmd.Parameters.AddWithValue("@Tel3_Contacto_3", Tel3_Contacto_3);
                    cmd.Parameters.AddWithValue("@Tel4_Contacto_3", Tel4_Contacto_3);
                    cmd.Parameters.AddWithValue("@Email_Contacto_3", Email_Contacto_3);
                    cmd.Parameters.AddWithValue("@Detalle_Contacto_3", Detalle_Contacto_3);

                    cmd.Parameters.AddWithValue("@Nom_Contacto_4", Nom_Contacto_4);
                    cmd.Parameters.AddWithValue("@Tipo_Contacto_4", Tipo_Contacto_4);
                    cmd.Parameters.AddWithValue("@Tel1_Contacto_4", Tel1_Contacto_4);
                    cmd.Parameters.AddWithValue("@Tel2_Contacto_4", Tel2_Contacto_4);
                    cmd.Parameters.AddWithValue("@Tel3_Contacto_4", Tel3_Contacto_4);
                    cmd.Parameters.AddWithValue("@Tel4_Contacto_4", Tel4_Contacto_4);
                    cmd.Parameters.AddWithValue("@Email_Contacto_4", Email_Contacto_4);
                    cmd.Parameters.AddWithValue("@Detalle_Contacto_4", Detalle_Contacto_4);

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


        protected void BtnAnularPnl9_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            inhabilitar(this.Controls);

            // habilitar (Configuracion Siniestro)
            habilitar_Config_Siniestro();

            btnEditarPnl9.Visible = true;
            btnActualizarPnl9.Visible = false;
            BtnAnularPnl9.Visible = false;
        }

        protected void btnEditarPnl9_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl9
            // Contratante
            TxtTpoProducto.Enabled = true;
            TxtFechaEmision.Enabled = true;
            TxtHoraEmision.Enabled = true;
            TxtFechaIniVigencia.Enabled = true;
            TxtFechaFinVigencia.Enabled = true;
         // TxtFechaContacto.Enabled = true;
            TxtNumCertificado.Enabled = true;
            TxtTpoMoneda.Enabled = true;
            TxtTpoPlan.Enabled = true;
            TxtPlazo.Enabled = true;
            TxtCanalVentas.Enabled = true;
            TxtNumRenovacion.Enabled = true;
            TxtGiro_Asegurado.Enabled = true;

            TxtFormaPago.Enabled = true;
            TxtPrimaTotalAnual.Enabled = true;
            TxtPrimaNeta.Enabled = true;
            TxtGastosExpedicion.Enabled = true;
            TxtRecargoPago.Enabled = true;
            TxtPorcentajeIVA.Enabled = true;
            TxtMontoIVA.Enabled = true;
            TxtPrimaTotal.Enabled = true;
            TxtPrimaFormaPago.Enabled = true;
            TxtPagoSubsecuente.Enabled = true;

            TxtRegistroPPAQ.Enabled = true;
            TxtFecRegistroPPAQ.Enabled = true;
            TxtRegistroRESP.Enabled = true;
            TxtFecRegistroRESP.Enabled = true;
            TxtRegistroCGEN.Enabled = true;
            TxtFecRegistroCGEN.Enabled = true;
            TxtRegistroBADI.Enabled = true;
            TxtFecRegistroBADI.Enabled = true;
            TxtRegistroCONDUSEF.Enabled = true;
            TxtFecRegistroCONDUSEF.Enabled = true;

            btnEditarPnl9.Visible = false;
            btnActualizarPnl9.Visible = true;
            BtnAnularPnl9.Visible = true;
        }

        protected void btnActualizarPnl9_Click(object sender, EventArgs e)
        {
            Actualizar_Datos_Poliza();
            Actualizar_Datos_Poliza_CONDUSEF();

            inhabilitar(this.Controls);

            // habilitar (Configuracion Siniestro)
            habilitar_Config_Siniestro();

            btnEditarPnl9.Visible = true;
            btnActualizarPnl9.Visible = false;
            BtnAnularPnl9.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios, correctamente";
            this.mpeMensaje.Show();
        }

        protected void Actualizar_Datos_Poliza()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Datos Personales
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                string TpoProducto = TxtTpoProducto.Text;
                string Fec_Emision = TxtFechaEmision.Text;
                string Fec_IniVigencia = TxtFechaIniVigencia.Text;
                string Fec_FinVigencia = TxtFechaFinVigencia.Text;
                string Hora_Emision = TxtHoraEmision.Text;
                string Num_Certificado = TxtNumCertificado.Text;
                string TpoMoneda = TxtTpoMoneda.Text;
                string TpoPlan = TxtTpoPlan.Text;
                string Plazo = TxtPlazo.Text;
                string CanalVentas = TxtCanalVentas.Text;
                string Num_Renovacion = TxtNumRenovacion.Text;
                string Giro_Asegurado = TxtGiro_Asegurado.Text;

                string FormaPago = TxtFormaPago.Text;
                string PrimaTotalAnual = TxtPrimaTotalAnual.Text;
                string PrimaNeta = TxtPrimaNeta.Text;
                string GastosExpedicion = TxtGastosExpedicion.Text;
                string RecargoPago = TxtRecargoPago.Text;
                string PorcentajeIVA = TxtPorcentajeIVA.Text;
                string MontoIVA = TxtMontoIVA.Text;
                string PrimaTotal = TxtPrimaTotal.Text;
                string PrimaFormaPago = TxtPrimaFormaPago.Text;
                string PagoSubsecuente = TxtPagoSubsecuente.Text;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @"INSERT INTO ITM_70_3 (Referencia, SubReferencia, TpoProducto, Fec_Emision, Hora_Emision, Fec_IniVigencia, Fec_FinVigencia, " +
                                   "                      Num_Certificado, TpoMoneda, TpoPlan, Plazo, CanalVentas, Num_Renovacion, Giro, " +
                                   "                      FormaPago, PrimaTotalAnual, PrimaNeta, GastosExpedicion, RecargoPago, PorcentajeIVA, " +
                                   "                      MontoIVA, PrimaTotal, PrimaFormaPago, PagoSubsecuente, Id_Usuario, IdStatus) " +
                                   "VALUES (@Referencia, @SubReferencia, @TpoProducto, @Fec_Emision, @Hora_Emision, @Fec_IniVigencia, @Fec_FinVigencia, @Num_Certificado, @TpoMoneda, @TpoPlan, @Plazo, @CanalVentas, @Num_Renovacion, @Giro, " +
                                   "                      @FormaPago, @PrimaTotalAnual, @PrimaNeta, @GastosExpedicion, @RecargoPago, @PorcentajeIVA, " +
                                   "                      @MontoIVA, @PrimaTotal, @PrimaFormaPago, @PagoSubsecuente, @Id_Usuario, @IdStatus) " +
                                   " ON DUPLICATE KEY UPDATE " +
                                   " TpoProducto = VALUES(TpoProducto), " +
                                   " Fec_Emision = VALUES(Fec_Emision), " +
                                   " Hora_Emision = VALUES(Hora_Emision), " +
                                   " Fec_IniVigencia = VALUES(Fec_IniVigencia), " +
                                   " Fec_FinVigencia = VALUES(Fec_FinVigencia), " +
                                   " Num_Certificado = VALUES(Num_Certificado), " +
                                   " TpoMoneda = VALUES(TpoMoneda), " +
                                   " TpoPlan = VALUES(TpoPlan), " +
                                   " Plazo = VALUES(Plazo), " +
                                   " CanalVentas = VALUES(CanalVentas), " +
                                   " Num_Renovacion = VALUES(Num_Renovacion), " +
                                   " Giro = VALUES(Giro)," +
                                   " FormaPago = VALUES(FormaPago)," +
                                   " PrimaTotalAnual = VALUES(PrimaTotalAnual)," +
                                   " PrimaNeta = VALUES(PrimaNeta)," +
                                   " GastosExpedicion = VALUES(GastosExpedicion)," +
                                   " RecargoPago = VALUES(RecargoPago)," +
                                   " PorcentajeIVA = VALUES(PorcentajeIVA)," +
                                   " MontoIVA = VALUES(MontoIVA)," +
                                   " PrimaTotal = VALUES(PrimaTotal)," +
                                   " PrimaFormaPago = VALUES(PrimaFormaPago)," +
                                   " PagoSubsecuente = VALUES(PagoSubsecuente)," +
                                   " Id_Usuario = VALUES(Id_Usuario), " +
                                   " IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@TpoProducto", TpoProducto);
                    cmd.Parameters.AddWithValue("@Fec_Emision", Fec_Emision);
                    cmd.Parameters.AddWithValue("@Hora_Emision", Hora_Emision);
                    cmd.Parameters.AddWithValue("@Fec_IniVigencia", Fec_IniVigencia);
                    cmd.Parameters.AddWithValue("@Fec_FinVigencia", Fec_FinVigencia);
                    cmd.Parameters.AddWithValue("@Num_Certificado", Num_Certificado);
                    cmd.Parameters.AddWithValue("@TpoMoneda", TpoMoneda);
                    cmd.Parameters.AddWithValue("@TpoPlan", TpoPlan);
                    cmd.Parameters.AddWithValue("@Plazo", Plazo);
                    cmd.Parameters.AddWithValue("@CanalVentas", CanalVentas);
                    cmd.Parameters.AddWithValue("@Num_Renovacion", Num_Renovacion);
                    cmd.Parameters.AddWithValue("@Giro", Giro_Asegurado);
                    cmd.Parameters.AddWithValue("@FormaPago", FormaPago);
                    cmd.Parameters.AddWithValue("@PrimaTotalAnual", PrimaTotalAnual);
                    cmd.Parameters.AddWithValue("@PrimaNeta", PrimaNeta);
                    cmd.Parameters.AddWithValue("@GastosExpedicion", GastosExpedicion);
                    cmd.Parameters.AddWithValue("@RecargoPago", RecargoPago);
                    cmd.Parameters.AddWithValue("@PorcentajeIVA", PorcentajeIVA);
                    cmd.Parameters.AddWithValue("@MontoIVA", MontoIVA);
                    cmd.Parameters.AddWithValue("@PrimaTotal", PrimaTotal);
                    cmd.Parameters.AddWithValue("@PrimaFormaPago", PrimaFormaPago);
                    cmd.Parameters.AddWithValue("@PagoSubsecuente", PagoSubsecuente);
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

        protected void Actualizar_Datos_Poliza_CONDUSEF()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Datos Personales
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                string RegistroPPAQ = TxtRegistroPPAQ.Text;
                string FecRegistroPPAQ = TxtFecRegistroPPAQ.Text;
                string RegistroRESP = TxtRegistroRESP.Text;
                string FecRegistroRESP = TxtFecRegistroRESP.Text;
                string RegistroCGEN = TxtRegistroCGEN.Text;
                string FecRegistroCGEN = TxtFecRegistroCGEN.Text;
                string RegistroBADI = TxtRegistroBADI.Text;
                string FecRegistroBADI = TxtFecRegistroBADI.Text;
                string RegistroCONDUSEF = TxtRegistroCONDUSEF.Text;
                string FecRegistroCONDUSEF = TxtFecRegistroCONDUSEF.Text;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @"INSERT INTO ITM_70_3_5 (Referencia, SubReferencia, RegistroPPAQ, FecRegistroPPAQ, RegistroRESP, FecRegistroRESP, RegistroCGEN, " +
                                   "                       FecRegistroCGEN, RegistroBADI, FecRegistroBADI, RegistroCONDUSEF, FecRegistroCONDUSEF, Id_Usuario, IdStatus) " +
                                   "VALUES (@Referencia, @SubReferencia, @RegistroPPAQ, @FecRegistroPPAQ, @RegistroRESP, @FecRegistroRESP, @RegistroCGEN, @FecRegistroCGEN, @RegistroBADI, " +
                                   "        @FecRegistroBADI, @RegistroCONDUSEF, @FecRegistroCONDUSEF, @Id_Usuario, @IdStatus)  " +
                                   " ON DUPLICATE KEY UPDATE " +
                                   " RegistroPPAQ = VALUES(RegistroPPAQ), " +
                                   " FecRegistroPPAQ = VALUES(FecRegistroPPAQ), " +
                                   " RegistroRESP = VALUES(RegistroRESP), " +
                                   " FecRegistroRESP = VALUES(FecRegistroRESP), " +
                                   " RegistroCGEN = VALUES(RegistroCGEN), " +
                                   " FecRegistroCGEN = VALUES(FecRegistroCGEN), " +
                                   " RegistroBADI = VALUES(RegistroBADI), " +
                                   " FecRegistroBADI = VALUES(FecRegistroBADI), " +
                                   " RegistroCONDUSEF = VALUES(RegistroCONDUSEF), " +
                                   " FecRegistroCONDUSEF = VALUES(FecRegistroCONDUSEF), " +
                                   " Id_Usuario = VALUES(Id_Usuario), " +
                                   " IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@RegistroPPAQ", RegistroPPAQ);
                    cmd.Parameters.AddWithValue("@FecRegistroPPAQ", FecRegistroPPAQ);
                    cmd.Parameters.AddWithValue("@RegistroRESP", RegistroRESP);
                    cmd.Parameters.AddWithValue("@FecRegistroRESP", FecRegistroRESP);
                    cmd.Parameters.AddWithValue("@RegistroCGEN", RegistroCGEN);
                    cmd.Parameters.AddWithValue("@FecRegistroCGEN", FecRegistroCGEN);
                    cmd.Parameters.AddWithValue("@RegistroBADI", RegistroBADI);
                    cmd.Parameters.AddWithValue("@FecRegistroBADI", FecRegistroBADI);
                    cmd.Parameters.AddWithValue("@RegistroCONDUSEF", RegistroCONDUSEF);
                    cmd.Parameters.AddWithValue("@FecRegistroCONDUSEF", FecRegistroCONDUSEF);
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

        protected void BtnAnularPnl10_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            inhabilitar(this.Controls);

            // habilitar (Configuracion Siniestro)
            habilitar_Config_Siniestro();

            btnEditarPnl10.Visible = true;
            btnActualizarPnl10.Visible = false;
            BtnAnularPnl10.Visible = false;
        }

        protected void btnEditarPnl10_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl10
            // Contratante
            TxtNomContratante.Enabled = true;
            TxtRFC_Contratante.Enabled = true;
            TxtTpoContratante.Enabled = true;
            TxtEmailContratante.Enabled = true;
            TxtTelefonoContratante.Enabled = true;
            TxtCelularContratante.Enabled = true;

            TxtCalleContratante.Enabled = true;
            TxtNumExtContratante.Enabled = true;
            TxtNumIntContratante.Enabled = true;

            // TxtPoblacionContratante.Enabled = true;
            // TxtEstadoContratante.Enabled = true;
            // TxtMunicipioContratante.Enabled = true;

            ddlEstadoContratante.Enabled = true;
            ddlMunicipiosContratante.Enabled = true;
            TxtColoniaContratante.Enabled = true;
            TxtCPostalContratante.Enabled = true;
            TxtOtrosContratante.Enabled = true;

            btnEditarPnl10.Visible = false;
            btnActualizarPnl10.Visible = true;
            BtnAnularPnl10.Visible = true;
        }

        protected void btnActualizarPnl10_Click(object sender, EventArgs e)
        {
            Actualizar_Datos_Contratante();

            inhabilitar(this.Controls);

            // habilitar (Configuracion Siniestro)
            habilitar_Config_Siniestro();

            btnEditarPnl10.Visible = true;
            btnActualizarPnl10.Visible = false;
            BtnAnularPnl10.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios, correctamente";
            this.mpeMensaje.Show();
        }

        protected void Actualizar_Datos_Contratante()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Datos Personales
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                string Nom_Contratante = TxtNomContratante.Text;
                string RFC_Contratante = TxtRFC_Contratante.Text;
                string Tipo_Contratante = TxtTpoContratante.Text;
                string Email_Contratante = TxtEmailContratante.Text;
                string Tel1_Contratante = TxtTelefonoContratante.Text;
                string Tel2_Contratante = TxtCelularContratante.Text;
                string Calle_Contratante = TxtCalleContratante.Text;
                string Num_Exterior = TxtNumExtContratante.Text;
                string Num_Interior = TxtNumIntContratante.Text;
                string Colonia_Contratante = TxtColoniaContratante.Text;

                string Estado_Contratante = ddlEstadoContratante.SelectedValue;
                string Municipio_Contratante = ddlMunicipiosContratante.SelectedValue;

                string CPostal_Contratante = TxtCPostalContratante.Text;
                string Otros_Contratante = TxtOtrosContratante.Text;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @"INSERT INTO ITM_70_3_1 (Referencia, SubReferencia, Nom_Contratante, RFC_Contratante, Tipo_Contratante, Email_Contratante, Tel1_Contratante, Tel2_Contratante, 
                                                            Calle_Contratante, Num_Exterior, Num_Interior, Colonia_Contratante, Estado_Contratante, Municipio_Contratante, CPostal_Contratante, Otros_Contratante, Id_Usuario, IdStatus) " +
                                   "VALUES (@Referencia, @SubReferencia, @Nom_Contratante, @RFC_Contratante, @Tipo_Contratante, @Email_Contratante, @Tel1_Contratante, @Tel2_Contratante, @Calle_Contratante, " +
                                   "        @Num_Exterior, @Num_Interior, @Colonia_Contratante, @Estado_Contratante, @Municipio_Contratante, @CPostal_Contratante, @Otros_Contratante, @Id_Usuario, @IdStatus) " +
                                   " ON DUPLICATE KEY UPDATE " +
                                   " Nom_Contratante = VALUES(Nom_Contratante), " +
                                   " RFC_Contratante = VALUES(RFC_Contratante), " +
                                   " Tipo_Contratante = VALUES(Tipo_Contratante), " +
                                   " Email_Contratante = VALUES(Email_Contratante), " +
                                   " Tel1_Contratante = VALUES(Tel1_Contratante), " +
                                   " Tel2_Contratante = VALUES(Tel2_Contratante), " +
                                   " Calle_Contratante = VALUES(Calle_Contratante), " +
                                   " Num_Exterior = VALUES(Num_Exterior), " +
                                   " Num_Interior = VALUES(Num_Interior), " +
                                   " Colonia_Contratante = VALUES(Colonia_Contratante), " +
                                   " Estado_Contratante = VALUES(Estado_Contratante), " +
                                   " Municipio_Contratante = VALUES(Municipio_Contratante), " +
                                   " CPostal_Contratante = VALUES(CPostal_Contratante), " +
                                   " Otros_Contratante = VALUES(Otros_Contratante), " +
                                   " Id_Usuario = VALUES(Id_Usuario), " +
                                   " IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@Nom_Contratante", Nom_Contratante);
                    cmd.Parameters.AddWithValue("@RFC_Contratante", RFC_Contratante);
                    cmd.Parameters.AddWithValue("@Tipo_Contratante", Tipo_Contratante);
                    cmd.Parameters.AddWithValue("@Email_Contratante", Email_Contratante);
                    cmd.Parameters.AddWithValue("@Tel1_Contratante", Tel1_Contratante);
                    cmd.Parameters.AddWithValue("@Tel2_Contratante", Tel2_Contratante);
                    cmd.Parameters.AddWithValue("@Calle_Contratante", Calle_Contratante);
                    cmd.Parameters.AddWithValue("@Num_Exterior", Num_Exterior);
                    cmd.Parameters.AddWithValue("@Num_Interior", Num_Interior);
                    cmd.Parameters.AddWithValue("@Colonia_Contratante", Colonia_Contratante);
                    cmd.Parameters.AddWithValue("@Estado_Contratante", Estado_Contratante);
                    cmd.Parameters.AddWithValue("@Municipio_Contratante", Municipio_Contratante);
                    cmd.Parameters.AddWithValue("@CPostal_Contratante", CPostal_Contratante);
                    cmd.Parameters.AddWithValue("@Otros_Contratante", Otros_Contratante);
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

        protected void BtnAnularPnl11_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            inhabilitar(this.Controls);
            chkCopiarDatos.Enabled = false;

            // habilitar (Configuracion Siniestro)
            habilitar_Config_Siniestro();

            btnEditarPnl11.Visible = true;
            btnActualizarPnl11.Visible = false;
            BtnAnularPnl11.Visible = false;
        }

        protected void btnEditarPnl11_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl11
            chkCopiarDatos.Enabled = true;

            // Asegurado 1
            TxtNombreAsegurado1.Enabled = true;
            TxtRFC_Asegurado1.Enabled = true;
            TxtTpoAsegurado1.Enabled = true;
            TxtEmailAsegurado1.Enabled = true;
            TxtTelefonoAsegurado1.Enabled = true;
            TxtCelularAsegurado1.Enabled = true;
            TxtCalleAsegurado1.Enabled = true;
            TxtNumExtAsegurado1.Enabled = true;
            TxtNumIntAsegurado1.Enabled = true;

            // TxtPoblacionAsegurado1.Enabled = true;
            // TxtEstadoAsegurado1.Enabled = true;
            // TxtMunicipioAsegurado1.Enabled = true;

            ddlEstadoAsegurado1.Enabled = true;
            ddlMunicipiosAsegurado1.Enabled = true;
            TxtColoniaAsegurado1.Enabled = true;
            TxtCPostalAsegurado1.Enabled = true;
            TxtOtrosAsegurado1.Enabled = true;

            // Asegurado 2
            TxtNombreAsegurado2.Enabled = true;
            TxtRFC_Asegurado2.Enabled = true;
            TxtTpoAsegurado2.Enabled = true;
            TxtEmailAsegurado2.Enabled = true;
            TxtTelefonoAsegurado2.Enabled = true;
            TxtCelularAsegurado2.Enabled = true;
            TxtCalleAsegurado2.Enabled = true;
            TxtNumExtAsegurado2.Enabled = true;
            TxtNumIntAsegurado2.Enabled = true;

            // TxtPoblacionAsegurado2.Enabled = true;
            // TxtEstadoAsegurado2.Enabled = true;
            // TxtMunicipioAsegurado2.Enabled = true;

            ddlEstadoAsegurado2.Enabled = true;
            ddlMunicipiosAsegurado2.Enabled = true;
            TxtColoniaAsegurado2.Enabled = true;
            TxtCPostalAsegurado2.Enabled = true;
            TxtOtrosAsegurado2.Enabled = true;

            // Asegurado 3
            TxtNombreAsegurado3.Enabled = true;
            TxtRFC_Asegurado3.Enabled = true;
            TxtTpoAsegurado3.Enabled = true;
            TxtEmailAsegurado3.Enabled = true;
            TxtTelefonoAsegurado3.Enabled = true;
            TxtCelularAsegurado3.Enabled = true;
            TxtCalleAsegurado3.Enabled = true;
            TxtNumExtAsegurado3.Enabled = true;
            TxtNumIntAsegurado3.Enabled = true;

            // TxtPoblacionAsegurado3.Enabled = true;
            // TxtEstadoAsegurado3.Enabled = true;
            // TxtMunicipioAsegurado3.Enabled = true;

            ddlEstadoAsegurado3.Enabled = true;
            ddlMunicipiosAsegurado3.Enabled = true;
            TxtColoniaAsegurado3.Enabled = true;
            TxtCPostalAsegurado3.Enabled = true;
            TxtOtrosAsegurado3.Enabled = true;

            btnEditarPnl11.Visible = false;
            btnActualizarPnl11.Visible = true;
            BtnAnularPnl11.Visible = true;
        }

        protected void btnActualizarPnl11_Click(object sender, EventArgs e)
        {
            Actualizar_Datos_Asegurados();

            inhabilitar(this.Controls);
            chkCopiarDatos.Enabled = false;

            // habilitar (Configuracion Siniestro)
            habilitar_Config_Siniestro();

            btnEditarPnl11.Visible = true;
            btnActualizarPnl11.Visible = false;
            BtnAnularPnl11.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios, correctamente";
            this.mpeMensaje.Show();
        }

        protected void Actualizar_Datos_Asegurados()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Datos Personales
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                string Nom_Asegurado_1 = TxtNombreAsegurado1.Text;
                string RFC_Asegurado_1 = TxtRFC_Asegurado1.Text;
                string Tipo_Asegurado_1 = TxtTpoAsegurado1.Text;
                string Email_Asegurado_1 = TxtEmailAsegurado1.Text;
                string Tel_Asegurado_1 = TxtTelefonoAsegurado1.Text;
                string Cel_Asegurado_1 = TxtCelularAsegurado1.Text;
                string Calle_Asegurado_1 = TxtCalleAsegurado1.Text;
                string Num_Exterior_Asegurado_1 = TxtNumExtAsegurado1.Text;
                string Num_Interior_Asegurado_1 = TxtNumIntAsegurado1.Text;
                string Colonia_Asegurado_1 = TxtColoniaAsegurado1.Text;
                string Estado_Asegurado_1 = ddlEstadoAsegurado1.SelectedValue;
                string Municipio_Asegurado_1 = ddlMunicipiosAsegurado1.SelectedValue;
                string CPostal_Asegurado_1 = TxtCPostalAsegurado1.Text;
                string Otros_Asegurado_1 = TxtOtrosAsegurado1.Text;

                string Nom_Asegurado_2 = TxtNombreAsegurado2.Text;
                string RFC_Asegurado_2 = TxtRFC_Asegurado2.Text;
                string Tipo_Asegurado_2 = TxtTpoAsegurado2.Text;
                string Email_Asegurado_2 = TxtEmailAsegurado2.Text;
                string Tel_Asegurado_2 = TxtTelefonoAsegurado2.Text;
                string Cel_Asegurado_2 = TxtCelularAsegurado2.Text;
                string Calle_Asegurado_2 = TxtCalleAsegurado2.Text;
                string Num_Exterior_Asegurado_2 = TxtNumExtAsegurado2.Text;
                string Num_Interior_Asegurado_2 = TxtNumIntAsegurado2.Text;
                string Colonia_Asegurado_2 = TxtColoniaAsegurado2.Text;
                string Estado_Asegurado_2 = ddlEstadoAsegurado2.SelectedValue;
                string Municipio_Asegurado_2 = ddlMunicipiosAsegurado2.SelectedValue;
                string CPostal_Asegurado_2 = TxtCPostalAsegurado2.Text;
                string Otros_Asegurado_2 = TxtOtrosAsegurado2.Text;

                string Nom_Asegurado_3 = TxtNombreAsegurado3.Text;
                string RFC_Asegurado_3 = TxtRFC_Asegurado3.Text;
                string Tipo_Asegurado_3 = TxtTpoAsegurado3.Text;
                string Email_Asegurado_3 = TxtEmailAsegurado3.Text;
                string Tel_Asegurado_3 = TxtTelefonoAsegurado3.Text;
                string Cel_Asegurado_3 = TxtCelularAsegurado3.Text;
                string Calle_Asegurado_3 = TxtCalleAsegurado3.Text;
                string Num_Exterior_Asegurado_3 = TxtNumExtAsegurado3.Text;
                string Num_Interior_Asegurado_3 = TxtNumIntAsegurado3.Text;
                string Colonia_Asegurado_3 = TxtColoniaAsegurado3.Text;
                string Estado_Asegurado_3 = ddlEstadoAsegurado3.SelectedValue;
                string Municipio_Asegurado_3 = ddlMunicipiosAsegurado3.SelectedValue;
                string CPostal_Asegurado_3 = TxtCPostalAsegurado3.Text;
                string Otros_Asegurado_3 = TxtOtrosAsegurado3.Text;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @"INSERT INTO ITM_70_3_2 (Referencia, SubReferencia,  " +
                                   "Nom_Asegurado_1, RFC_Asegurado_1, Tipo_Asegurado_1, Email_Asegurado_1, Tel_Asegurado_1, Cel_Asegurado_1, Calle_Asegurado_1, Num_Exterior_Asegurado_1, Num_Interior_Asegurado_1, Colonia_Asegurado_1, Estado_Asegurado_1, Municipio_Asegurado_1, CPostal_Asegurado_1, Otros_Asegurado_1, " +
                                   "Nom_Asegurado_2, RFC_Asegurado_2, Tipo_Asegurado_2, Email_Asegurado_2, Tel_Asegurado_2, Cel_Asegurado_2, Calle_Asegurado_2, Num_Exterior_Asegurado_2, Num_Interior_Asegurado_2, Colonia_Asegurado_2, Estado_Asegurado_2, Municipio_Asegurado_2, CPostal_Asegurado_2, Otros_Asegurado_2, " +
                                   "Nom_Asegurado_3, RFC_Asegurado_3, Tipo_Asegurado_3, Email_Asegurado_3, Tel_Asegurado_3, Cel_Asegurado_3, Calle_Asegurado_3, Num_Exterior_Asegurado_3, Num_Interior_Asegurado_3, Colonia_Asegurado_3, Estado_Asegurado_3, Municipio_Asegurado_3, CPostal_Asegurado_3, Otros_Asegurado_3, " +
                                   "Id_Usuario, IdStatus) " +
                                   "VALUES (@Referencia, @SubReferencia, " +
                                   "        @Nom_Asegurado_1, @RFC_Asegurado_1, @Tipo_Asegurado_1, @Email_Asegurado_1, @Tel_Asegurado_1, @Cel_Asegurado_1, @Calle_Asegurado_1, @Num_Exterior_Asegurado_1, @Num_Interior_Asegurado_1, @Colonia_Asegurado_1, @Estado_Asegurado_1, @Municipio_Asegurado_1, @CPostal_Asegurado_1, @Otros_Asegurado_1, " +
                                   "        @Nom_Asegurado_2, @RFC_Asegurado_2, @Tipo_Asegurado_2, @Email_Asegurado_2, @Tel_Asegurado_2, @Cel_Asegurado_2, @Calle_Asegurado_2, @Num_Exterior_Asegurado_2, @Num_Interior_Asegurado_2, @Colonia_Asegurado_2, @Estado_Asegurado_2, @Municipio_Asegurado_2, @CPostal_Asegurado_2, @Otros_Asegurado_2, " +
                                   "        @Nom_Asegurado_3, @RFC_Asegurado_3, @Tipo_Asegurado_3, @Email_Asegurado_3, @Tel_Asegurado_3, @Cel_Asegurado_3, @Calle_Asegurado_3, @Num_Exterior_Asegurado_3, @Num_Interior_Asegurado_3, @Colonia_Asegurado_3, @Estado_Asegurado_3, @Municipio_Asegurado_3, @CPostal_Asegurado_3, @Otros_Asegurado_3, " +
                                   "        @Id_Usuario, @IdStatus) " +
                                   " ON DUPLICATE KEY UPDATE  " +
                                   "    Nom_Asegurado_1 = VALUES(Nom_Asegurado_1), " +
                                   "    RFC_Asegurado_1 = VALUES(RFC_Asegurado_1), " +
                                   "    Tipo_Asegurado_1 = VALUES(Tipo_Asegurado_1), " +
                                   "    Email_Asegurado_1 = VALUES(Email_Asegurado_1), " +
                                   "    Tel_Asegurado_1 = VALUES(Tel_Asegurado_1), " +
                                   "    Cel_Asegurado_1 = VALUES(Cel_Asegurado_1), " +
                                   "    Calle_Asegurado_1 = VALUES(Calle_Asegurado_1), " +
                                   "    Num_Exterior_Asegurado_1 = VALUES(Num_Exterior_Asegurado_1), " +
                                   "    Num_Interior_Asegurado_1 = VALUES(Num_Interior_Asegurado_1), " +
                                   "    Colonia_Asegurado_1 = VALUES(Colonia_Asegurado_1), " +
                                   "    Estado_Asegurado_1 = VALUES(Estado_Asegurado_1), " +
                                   "    Municipio_Asegurado_1 = VALUES(Municipio_Asegurado_1), " +
                                   "    CPostal_Asegurado_1 = VALUES(CPostal_Asegurado_1), " +
                                   "    Otros_Asegurado_1 = VALUES(Otros_Asegurado_1), " +
                                   "    Nom_Asegurado_2 = VALUES(Nom_Asegurado_2), " +
                                   "    RFC_Asegurado_2 = VALUES(RFC_Asegurado_2), " +
                                   "    Tipo_Asegurado_2 = VALUES(Tipo_Asegurado_2), " +
                                   "    Email_Asegurado_2 = VALUES(Email_Asegurado_2), " +
                                   "    Tel_Asegurado_2 = VALUES(Tel_Asegurado_2), " +
                                   "    Cel_Asegurado_2 = VALUES(Cel_Asegurado_2), " +
                                   "    Calle_Asegurado_2 = VALUES(Calle_Asegurado_2), " +
                                   "    Num_Exterior_Asegurado_2 = VALUES(Num_Exterior_Asegurado_2), " +
                                   "    Num_Interior_Asegurado_2 = VALUES(Num_Interior_Asegurado_2), " +
                                   "    Colonia_Asegurado_2 = VALUES(Colonia_Asegurado_2), " +
                                   "    Estado_Asegurado_2 = VALUES(Estado_Asegurado_2), " +
                                   "    Municipio_Asegurado_2 = VALUES(Municipio_Asegurado_2), " +
                                   "    CPostal_Asegurado_2 = VALUES(CPostal_Asegurado_2), " +
                                   "    Otros_Asegurado_2 = VALUES(Otros_Asegurado_2), " +
                                   "    Nom_Asegurado_3 = VALUES(Nom_Asegurado_3), " +
                                   "    RFC_Asegurado_3 = VALUES(RFC_Asegurado_3), " +
                                   "    Tipo_Asegurado_3 = VALUES(Tipo_Asegurado_3), " +
                                   "    Email_Asegurado_3 = VALUES(Email_Asegurado_3), " +
                                   "    Tel_Asegurado_3 = VALUES(Tel_Asegurado_3), " +
                                   "    Cel_Asegurado_3 = VALUES(Cel_Asegurado_3), " +
                                   "    Calle_Asegurado_3 = VALUES(Calle_Asegurado_3), " +
                                   "    Num_Exterior_Asegurado_3 = VALUES(Num_Exterior_Asegurado_3), " +
                                   "    Num_Interior_Asegurado_3 = VALUES(Num_Interior_Asegurado_3), " +
                                   "    Colonia_Asegurado_3 = VALUES(Colonia_Asegurado_3), " +
                                   "    Estado_Asegurado_3 = VALUES(Estado_Asegurado_3), " +
                                   "    Municipio_Asegurado_3 = VALUES(Municipio_Asegurado_3), " +
                                   "    CPostal_Asegurado_3 = VALUES(CPostal_Asegurado_3), " +
                                   "    Otros_Asegurado_3 = VALUES(Otros_Asegurado_3), " +
                                   "    Id_Usuario = VALUES(Id_Usuario), " +
                                   "    IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@Nom_Asegurado_1", Nom_Asegurado_1);
                    cmd.Parameters.AddWithValue("@RFC_Asegurado_1", RFC_Asegurado_1);
                    cmd.Parameters.AddWithValue("@Tipo_Asegurado_1", Tipo_Asegurado_1);
                    cmd.Parameters.AddWithValue("@Email_Asegurado_1", Email_Asegurado_1);
                    cmd.Parameters.AddWithValue("@Tel_Asegurado_1", Tel_Asegurado_1);
                    cmd.Parameters.AddWithValue("@Cel_Asegurado_1", Cel_Asegurado_1);
                    cmd.Parameters.AddWithValue("@Calle_Asegurado_1", Calle_Asegurado_1);
                    cmd.Parameters.AddWithValue("@Num_Exterior_Asegurado_1", Num_Exterior_Asegurado_1);
                    cmd.Parameters.AddWithValue("@Num_Interior_Asegurado_1", Num_Interior_Asegurado_1);
                    cmd.Parameters.AddWithValue("@Colonia_Asegurado_1", Colonia_Asegurado_1);
                    cmd.Parameters.AddWithValue("@Estado_Asegurado_1", Estado_Asegurado_1);
                    cmd.Parameters.AddWithValue("@Municipio_Asegurado_1", Municipio_Asegurado_1);
                    cmd.Parameters.AddWithValue("@CPostal_Asegurado_1", CPostal_Asegurado_1);
                    cmd.Parameters.AddWithValue("@Otros_Asegurado_1", Otros_Asegurado_1);

                    cmd.Parameters.AddWithValue("@Nom_Asegurado_2", Nom_Asegurado_2);
                    cmd.Parameters.AddWithValue("@RFC_Asegurado_2", RFC_Asegurado_2);
                    cmd.Parameters.AddWithValue("@Tipo_Asegurado_2", Tipo_Asegurado_2);
                    cmd.Parameters.AddWithValue("@Email_Asegurado_2", Email_Asegurado_2);
                    cmd.Parameters.AddWithValue("@Tel_Asegurado_2", Tel_Asegurado_2);
                    cmd.Parameters.AddWithValue("@Cel_Asegurado_2", Cel_Asegurado_2);
                    cmd.Parameters.AddWithValue("@Calle_Asegurado_2", Calle_Asegurado_2);
                    cmd.Parameters.AddWithValue("@Num_Exterior_Asegurado_2", Num_Exterior_Asegurado_2);
                    cmd.Parameters.AddWithValue("@Num_Interior_Asegurado_2", Num_Interior_Asegurado_2);
                    cmd.Parameters.AddWithValue("@Colonia_Asegurado_2", Colonia_Asegurado_2);
                    cmd.Parameters.AddWithValue("@Estado_Asegurado_2", Estado_Asegurado_2);
                    cmd.Parameters.AddWithValue("@Municipio_Asegurado_2", Municipio_Asegurado_2);
                    cmd.Parameters.AddWithValue("@CPostal_Asegurado_2", CPostal_Asegurado_2);
                    cmd.Parameters.AddWithValue("@Otros_Asegurado_2", Otros_Asegurado_2);

                    cmd.Parameters.AddWithValue("@Nom_Asegurado_3", Nom_Asegurado_3);
                    cmd.Parameters.AddWithValue("@RFC_Asegurado_3", RFC_Asegurado_3);
                    cmd.Parameters.AddWithValue("@Tipo_Asegurado_3", Tipo_Asegurado_3);
                    cmd.Parameters.AddWithValue("@Email_Asegurado_3", Email_Asegurado_3);
                    cmd.Parameters.AddWithValue("@Tel_Asegurado_3", Tel_Asegurado_3);
                    cmd.Parameters.AddWithValue("@Cel_Asegurado_3", Cel_Asegurado_3);
                    cmd.Parameters.AddWithValue("@Calle_Asegurado_3", Calle_Asegurado_3);
                    cmd.Parameters.AddWithValue("@Num_Exterior_Asegurado_3", Num_Exterior_Asegurado_3);
                    cmd.Parameters.AddWithValue("@Num_Interior_Asegurado_3", Num_Interior_Asegurado_3);
                    cmd.Parameters.AddWithValue("@Colonia_Asegurado_3", Colonia_Asegurado_3);
                    cmd.Parameters.AddWithValue("@Estado_Asegurado_3", Estado_Asegurado_3);
                    cmd.Parameters.AddWithValue("@Municipio_Asegurado_3", Municipio_Asegurado_3);
                    cmd.Parameters.AddWithValue("@CPostal_Asegurado_3", CPostal_Asegurado_3);
                    cmd.Parameters.AddWithValue("@Otros_Asegurado_3", Otros_Asegurado_3);

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

        protected void BtnAnularPnl15_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            inhabilitar(this.Controls);

            // habilitar (Configuracion Siniestro)
            habilitar_Config_Siniestro();

            btnEditarPnl15.Visible = true;
            btnActualizarPnl15.Visible = false;
            BtnAnularPnl15.Visible = false;
        }

        protected void btnEditarPnl15_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl15
            // Contratante
            TxtCalleBienAsegurado.Enabled = true;
            TxtNumExtBienAsegurado.Enabled = true;
            TxtNumIntBienAsegurado.Enabled = true;

            // TxtPoblacionBienAsegurado.Enabled = true;
            // TxtEstadoBienAsegurado.Enabled = true;
            // TxtMunicipioBienAsegurado.Enabled = true;

            ddlEstadoBienAsegurado.Enabled = true;
            ddlMunicipiosBienAsegurado.Enabled = true;
            TxtColoniaBienAsegurado.Enabled = true;
            TxtCodigoBienAsegurado.Enabled = true;
            TxtOtrosBienAsegurado.Enabled = true;

            TxtTpoTecho.Enabled = true;
            TxtTpoVivienda.Enabled = true;
            TxtTpoMuro.Enabled = true;
            TxtPisosBienAsegurado.Enabled = true;
            TxtPisosDelBienAsegurado.Enabled = true;
            TxtLocalesComerciales.Enabled = true;
            TxtDetallesBienAsegurado.Enabled = true;

            btnEditarPnl15.Visible = false;
            btnActualizarPnl15.Visible = true;
            BtnAnularPnl15.Visible = true;
        }

        protected void btnActualizarPnl15_Click(object sender, EventArgs e)
        {
            Actualizar_Datos_BienAsegurado();

            inhabilitar(this.Controls);

            // habilitar (Configuracion Siniestro)
            habilitar_Config_Siniestro();

            btnEditarPnl15.Visible = true;
            btnActualizarPnl15.Visible = false;
            BtnAnularPnl15.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios, correctamente";
            this.mpeMensaje.Show();
        }

        protected void Actualizar_Datos_BienAsegurado()
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Datos Personales
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                string Calle_BienAsegurado = TxtCalleBienAsegurado.Text;
                string Num_Exterior_BienAsegurado = TxtNumExtBienAsegurado.Text;
                string Num_Interior_BienAsegurado = TxtNumIntBienAsegurado.Text;
                string Colonia_BienAsegurado = TxtColoniaBienAsegurado.Text;
                string Estado_BienAsegurado = ddlEstadoBienAsegurado.SelectedValue;
                string Municipio_BienAsegurado = ddlMunicipiosBienAsegurado.SelectedValue;
                string CPostal_BienAsegurado = TxtCodigoBienAsegurado.Text;
                string Otros_BienAsegurado = TxtOtrosBienAsegurado.Text;
                string TpoTecho_BienAsegurado = TxtTpoTecho.Text;
                string TpoVivienda_BienAsegurado = TxtTpoVivienda.Text;
                string TpoMuro_BienAsegurado = TxtTpoMuro.Text;
                string Pisos_BienAsegurado = TxtPisosBienAsegurado.Text;
                string PisosDel_BienAsegurado = TxtPisosDelBienAsegurado.Text;
                string Locales_BienAsegurado = TxtLocalesComerciales.Text;
                string Detalles_BienAsegurado = TxtDetallesBienAsegurado.Text;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @"INSERT INTO ITM_70_3_3 (Referencia, SubReferencia, Calle_BienAsegurado, Num_Exterior_BienAsegurado, Num_Interior_BienAsegurado, Colonia_BienAsegurado, Estado_BienAsegurado, Municipio_BienAsegurado, 
                                             CPostal_BienAsegurado, Otros_BienAsegurado, TpoTecho_BienAsegurado, TpoVivienda_BienAsegurado, TpoMuro_BienAsegurado, Pisos_BienAsegurado, PisosDel_BienAsegurado, Locales_BienAsegurado, Detalles_BienAsegurado, Id_Usuario, IdStatus) " +
                                   "VALUES (@Referencia, @SubReferencia, @Calle_BienAsegurado, @Num_Exterior_BienAsegurado, @Num_Interior_BienAsegurado, @Colonia_BienAsegurado, @Estado_BienAsegurado, @Municipio_BienAsegurado, @CPostal_BienAsegurado, " +
                                   "        @Otros_BienAsegurado, @TpoTecho_BienAsegurado, @TpoVivienda_BienAsegurado, @TpoMuro_BienAsegurado, @Pisos_BienAsegurado, @PisosDel_BienAsegurado, @Locales_BienAsegurado, @Detalles_BienAsegurado, @Id_Usuario, @IdStatus) " +
                                   " ON DUPLICATE KEY UPDATE " +
                                   " Calle_BienAsegurado = VALUES(Calle_BienAsegurado), " +
                                   " Num_Exterior_BienAsegurado = VALUES(Num_Exterior_BienAsegurado), " +
                                   " Num_Interior_BienAsegurado = VALUES(Num_Interior_BienAsegurado), " +
                                   " Colonia_BienAsegurado = VALUES(Colonia_BienAsegurado), " +
                                   " Estado_BienAsegurado = VALUES(Estado_BienAsegurado), " +
                                   " Municipio_BienAsegurado = VALUES(Municipio_BienAsegurado), " +
                                   " CPostal_BienAsegurado = VALUES(CPostal_BienAsegurado), " +
                                   " Otros_BienAsegurado = VALUES(Otros_BienAsegurado), " +
                                   " TpoTecho_BienAsegurado = VALUES(TpoTecho_BienAsegurado), " +
                                   " TpoVivienda_BienAsegurado = VALUES(TpoVivienda_BienAsegurado), " +
                                   " TpoMuro_BienAsegurado = VALUES(TpoMuro_BienAsegurado), " +
                                   " Pisos_BienAsegurado = VALUES(Pisos_BienAsegurado), " +
                                   " PisosDel_BienAsegurado = VALUES(PisosDel_BienAsegurado), " +
                                   " Locales_BienAsegurado = VALUES(Locales_BienAsegurado), " +
                                   " Detalles_BienAsegurado = VALUES(Detalles_BienAsegurado), " +
                                   " Id_Usuario = VALUES(Id_Usuario), " +
                                   " IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@Calle_BienAsegurado", Calle_BienAsegurado);
                    cmd.Parameters.AddWithValue("@Num_Exterior_BienAsegurado", Num_Exterior_BienAsegurado);
                    cmd.Parameters.AddWithValue("@Num_Interior_BienAsegurado", Num_Interior_BienAsegurado);
                    cmd.Parameters.AddWithValue("@Colonia_BienAsegurado", Colonia_BienAsegurado);
                    cmd.Parameters.AddWithValue("@Estado_BienAsegurado", Estado_BienAsegurado);
                    cmd.Parameters.AddWithValue("@Municipio_BienAsegurado", Municipio_BienAsegurado);
                    cmd.Parameters.AddWithValue("@CPostal_BienAsegurado", CPostal_BienAsegurado);
                    cmd.Parameters.AddWithValue("@Otros_BienAsegurado", Otros_BienAsegurado);
                    cmd.Parameters.AddWithValue("@TpoTecho_BienAsegurado", TpoTecho_BienAsegurado);
                    cmd.Parameters.AddWithValue("@TpoVivienda_BienAsegurado", TpoVivienda_BienAsegurado);
                    cmd.Parameters.AddWithValue("@TpoMuro_BienAsegurado", TpoMuro_BienAsegurado);
                    cmd.Parameters.AddWithValue("@Pisos_BienAsegurado", Pisos_BienAsegurado);
                    cmd.Parameters.AddWithValue("@PisosDel_BienAsegurado", PisosDel_BienAsegurado);
                    cmd.Parameters.AddWithValue("@Locales_BienAsegurado", Locales_BienAsegurado);
                    cmd.Parameters.AddWithValue("@Detalles_BienAsegurado", Detalles_BienAsegurado);
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

        protected void BtnAnularPnl17_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            // inhabilitar(this.Controls);

            btnEditarPnl17.Visible = true;
            btnActualizarPnl17.Visible = false;
            BtnAnularPnl17.Visible = false;

            // inicializar controles.
            // ddlSecciones.SelectedValue = "0";
            // ddlSecciones.Enabled = true;

            TxtSeccion.Text = string.Empty;
            TxtSeccion.ReadOnly = false;

            ddlCoberturas.SelectedValue = "0";
            ddlCoberturas.Enabled = true;

            TxtSumaAsegurada.Text = string.Empty;
            TxtSumaAsegurada.ReadOnly = false;

            TxtValSumaAsegurada.Text = string.Empty;
            TxtValSumaAsegurada.ReadOnly = false;

            TxtAgregado.Text = string.Empty;
            TxtAgregado.ReadOnly = false;

            TxtValAgregado.Text = string.Empty;
            TxtValAgregado.ReadOnly = false;

            TxtNomCobertura.Text = string.Empty;
            TxtNomCobertura.ReadOnly = false;

            TxtSeccion.Text = string.Empty;
            TxtSeccion.ReadOnly = false;

            TxtRiesgo.Text = string.Empty;
            TxtRiesgo.ReadOnly = false;

            TxtSublimite.Text = string.Empty;
            TxtSublimite.ReadOnly = false;

            TxtValSublimite.Text = string.Empty;
            TxtValSublimite.ReadOnly = false;

            TxtDeducible.Text = string.Empty;
            TxtDeducible.ReadOnly = false;

            TxtValDeducible.Text = string.Empty;
            TxtValDeducible.ReadOnly = false;

            TxtCoaseguro.Text = string.Empty;
            TxtCoaseguro.ReadOnly = false;

            TxtValCoaseguro.Text = string.Empty;
            TxtValCoaseguro.ReadOnly = false;

            TxtNotas.Text = string.Empty;
            TxtNotas.ReadOnly = false;

            chkAplicaSiniestro.Checked = false;
            chkAplicaSiniestro.Enabled = true;

            btnEditarPnl17.Visible = true;

            // BtnGrabar.Visible = false;
            BtnAnularPnl17.Visible = false;

            btnEditarPnl17.Enabled = false;
            BtnAgregarPnl17.Enabled = true;


        }

        protected void btnEditarPnl17_Click(object sender, EventArgs e)
        {
            // habilitar controles Pnl15
            // Coberturas
            // ddlCoberturas.Enabled = true;

            //TxtNomCobertura.Enabled = true;
            //TxtRiesgo.Enabled = true;
            //TxtSumaAsegurada.Enabled = true;
            //TxtSublimite.Enabled = true;
            //TxtDeducible.Enabled = true;
            //TxtCoaseguro.Enabled = true;

            // SetControlsEnabled(true);

            TxtNomCobertura.ReadOnly = false;
            TxtSeccion.ReadOnly = false;
            TxtRiesgo.ReadOnly = false;
            TxtSumaAsegurada.ReadOnly = false;
            TxtValSumaAsegurada.ReadOnly = false;
            TxtAgregado.ReadOnly = false;
            TxtValAgregado.ReadOnly = false;
            TxtSublimite.ReadOnly = false;
            TxtValSublimite.ReadOnly = false;
            TxtDeducible.ReadOnly = false;
            TxtValDeducible.ReadOnly = false;
            TxtCoaseguro.ReadOnly = false;
            TxtValCoaseguro.ReadOnly = false;
            TxtNotas.ReadOnly = false;
            chkAplicaSiniestro.Enabled = true;

            btnEditarPnl17.Visible = false;
            btnActualizarPnl17.Visible = true;
            BtnAnularPnl17.Visible = true;

        }

        protected void btnActualizarPnl17_Click(object sender, EventArgs e)
        {
            Actualizar_Datos_Cobertura();

            // inhabilitar(this.Controls);
            // SetControlsEnabled(false);

            //btnEditarPnl17.Visible = true;
            //btnActualizarPnl17.Visible = false;
            //BtnAnularPnl17.Visible = false;

            //btnEditarPnl17.Enabled = false;
            //BtnAgregarPnl17.Enabled = true;

            GetAltaCoberturas();

            // inicializar controles.
            // ddlSecciones.SelectedValue = "0";
            // ddlSecciones.Enabled = true;

            ddlCoberturas.SelectedValue = "0";
            ddlCoberturas.Enabled = true;

            TxtSeccion.Text = string.Empty;
            TxtSeccion.ReadOnly = false;

            TxtSumaAsegurada.Text = string.Empty;
            TxtSumaAsegurada.ReadOnly = false;

            TxtValSumaAsegurada.Text = string.Empty;
            TxtValSumaAsegurada.ReadOnly = false;

            TxtAgregado.Text = string.Empty;
            TxtAgregado.ReadOnly = false;

            TxtValAgregado.Text = string.Empty;
            TxtValAgregado.ReadOnly = false;

            TxtNomCobertura.Text = string.Empty;
            TxtNomCobertura.ReadOnly = false;

            TxtSeccion.Text = string.Empty;
            TxtSeccion.ReadOnly = false;

            TxtRiesgo.Text = string.Empty;
            TxtRiesgo.ReadOnly = false;

            TxtSublimite.Text = string.Empty;
            TxtSublimite.ReadOnly = false;

            TxtValSublimite.Text = string.Empty;
            TxtValSublimite.ReadOnly = false;

            TxtDeducible.Text = string.Empty;
            TxtDeducible.ReadOnly = false;

            TxtValDeducible.Text = string.Empty;
            TxtValDeducible.ReadOnly = false;

            TxtCoaseguro.Text = string.Empty;
            TxtCoaseguro.ReadOnly = false;

            TxtValCoaseguro.Text = string.Empty;
            TxtValCoaseguro.ReadOnly = false;

            TxtNotas.Text = string.Empty;
            TxtNotas.ReadOnly = false;

            chkAplicaSiniestro.Checked = false;
            chkAplicaSiniestro.Enabled = false;

            btnEditarPnl17.Visible = true;
            btnActualizarPnl17.Visible = false;
            BtnAnularPnl17.Visible = false;

            btnEditarPnl17.Enabled = false;
            BtnAgregarPnl17.Enabled = true;

            LblMessage.Text = "Se actualizo cobertura, correctamente";
            this.mpeMensaje.Show();
        }

        protected void BtnAgregarPnl17_Click(object sender, EventArgs e)
        {
            //if (ddlSecciones.SelectedValue == "0")
            //{
            //    LblMessage.Text = "Seleccionar Sección";
            //    mpeMensaje.Show();
            //    return;
            //}
            if (ddlCoberturas.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Cobertura";
                mpeMensaje.Show();
                return;
            }
            //else if (TxtNomCobertura.Text == "" || TxtNomCobertura.Text == null)
            //{
            //    LblMessage.Text = "Capturar Nombre Cobertura";
            //    mpeMensaje.Show();
            //    return;
            //}

            //string sDescripcion = TxtNomCategoria.Text;
            //string sTabla = Variables.wTabla;

            //int Envio_Ok = Add_tbDocumentos(sTabla, sDescripcion);
            Actualizar_Datos_Cobertura();

            // inhabilitar(this.Controls);

            btnEditarPnl17.Visible = true;
            btnActualizarPnl17.Visible = false;
            BtnAnularPnl17.Visible = false;

            // inicializar controles
            // ddlSecciones.SelectedValue = "0";
            // ddlSecciones.Enabled = true;

            ddlCoberturas.SelectedValue = "0";
            ddlCoberturas.Enabled = true;

            TxtNomCobertura.Text = string.Empty;
            TxtSeccion.Text = string.Empty;
            TxtRiesgo.Text = string.Empty;
            TxtSumaAsegurada.Text = string.Empty;
            TxtValSumaAsegurada.Text = string.Empty;
            TxtAgregado.Text = string.Empty;
            TxtValAgregado.Text = string.Empty;
            TxtSublimite.Text = string.Empty;
            TxtValSublimite.Text = string.Empty;
            TxtDeducible.Text = string.Empty;
            TxtValDeducible.Text = string.Empty;
            TxtCoaseguro.Text = string.Empty;
            TxtValCoaseguro.Text = string.Empty;
            TxtNotas.Text = string.Empty;

            chkAplicaSiniestro.Checked = false;

            GetAltaCoberturas();

            LblMessage.Text = "Se agrego cobertura, correctamente";
            this.mpeMensaje.Show();

        }

        private void SetControlsEnabled(bool enable)
        {
            //foreach (RepeaterItem item in Repeater1.Items)
            //{
            //    // Encontrar y habilitar/deshabilitar el CheckBox
            //    CheckBox checkBox = item.FindControl("CheckBox") as CheckBox;
            //    if (checkBox != null)
            //    {
            //        checkBox.Enabled = enable;
            //    }

            //    // Encontrar y habilitar/deshabilitar los TextBox
            //    TextBox textBox1 = item.FindControl("TextBox1") as TextBox;
            //    TextBox textBox2 = item.FindControl("TextBox2") as TextBox;
            //    TextBox textBox3 = item.FindControl("TextBox3") as TextBox;
            //    TextBox textBox4 = item.FindControl("TextBox4") as TextBox;
            //    TextBox textBox5 = item.FindControl("TextBox5") as TextBox;
            //    TextBox textBox6 = item.FindControl("TextBox6") as TextBox;

            //    if (textBox1 != null)
            //    {
            //        textBox1.Enabled = enable;
            //    }

            //    if (textBox2 != null)
            //    {
            //        textBox2.Enabled = enable;
            //    }

            //    if (textBox3 != null)
            //    {
            //        textBox3.Enabled = enable;
            //    }

            //    if (textBox4 != null)
            //    {
            //        textBox4.Enabled = enable;
            //    }

            //    if (textBox5 != null)
            //    {
            //        textBox5.Enabled = enable;
            //    }

            //    if (textBox6 != null)
            //    {
            //        textBox6.Enabled = enable;
            //    }
            //}
        }

        protected void Actualizar_Datos_Cobertura_bk()
        {
            // Lista para almacenar los valores de cada fila
            List<Dictionary<string, object>> listaFilas = new List<Dictionary<string, object>>();

            //foreach (RepeaterItem item in Repeater1.Items)
            //{
            //    // Diccionario para almacenar los valores de la fila actual
            //    Dictionary<string, object> filaValores = new Dictionary<string, object>();

            //    // Acceder al CheckBox
            //    CheckBox checkBox = item.FindControl("CheckBox") as CheckBox;
            //    if (checkBox != null)
            //    {
            //        filaValores["CheckBox"] = checkBox.Checked;
            //    }

            //    // Acceder a los TextBox
            //    TextBox textBox1 = item.FindControl("TextBox1") as TextBox;
            //    TextBox textBox2 = item.FindControl("TextBox2") as TextBox;
            //    TextBox textBox3 = item.FindControl("TextBox3") as TextBox;
            //    TextBox textBox4 = item.FindControl("TextBox4") as TextBox;
            //    TextBox textBox5 = item.FindControl("TextBox5") as TextBox;
            //    TextBox textBox6 = item.FindControl("TextBox6") as TextBox;

            //    if (textBox1 != null)
            //    {
            //        filaValores["TextBox1"] = textBox1.Text;
            //    }

            //    if (textBox2 != null)
            //    {
            //        filaValores["TextBox2"] = textBox2.Text;
            //    }

            //    if (textBox3 != null)
            //    {
            //        filaValores["TextBox3"] = textBox3.Text;
            //    }

            //    if (textBox4 != null)
            //    {
            //        filaValores["TextBox4"] = textBox4.Text;
            //    }

            //    if (textBox5 != null)
            //    {
            //        filaValores["TextBox5"] = textBox5.Text;
            //    }

            //    if (textBox6 != null)
            //    {
            //        filaValores["TextBox6"] = textBox6.Text;
            //    }

            //    // Añadir el diccionario de la fila actual a la lista
            //    listaFilas.Add(filaValores);
            //}

            bool Cob_Habilitado_1 = false;
            string Cob_Nombre_1 = string.Empty;
            string Cob_Riesgo_1 = string.Empty;
            decimal Cob_Suma_1 = 0;
            string Cob_Sublimite_1 = string.Empty;
            string Cob_Deducible_1 = string.Empty;
            string Cob_Coaseguro_1 = string.Empty;

            bool Cob_Habilitado_2 = false;
            string Cob_Nombre_2 = string.Empty;
            string Cob_Riesgo_2 = string.Empty;
            decimal Cob_Suma_2 = 0;
            string Cob_Sublimite_2 = string.Empty;
            string Cob_Deducible_2 = string.Empty;
            string Cob_Coaseguro_2 = string.Empty;

            bool Cob_Habilitado_3 = false;
            string Cob_Nombre_3 = string.Empty;
            string Cob_Riesgo_3 = string.Empty;
            decimal Cob_Suma_3 = 0;
            string Cob_Sublimite_3 = string.Empty;
            string Cob_Deducible_3 = string.Empty;
            string Cob_Coaseguro_3 = string.Empty;

            bool Cob_Habilitado_4 = false;
            string Cob_Nombre_4 = string.Empty;
            string Cob_Riesgo_4 = string.Empty;
            decimal Cob_Suma_4 = 0;
            string Cob_Sublimite_4 = string.Empty;
            string Cob_Deducible_4 = string.Empty;
            string Cob_Coaseguro_4 = string.Empty;

            bool Cob_Habilitado_5 = false;
            string Cob_Nombre_5 = string.Empty;
            string Cob_Riesgo_5 = string.Empty;
            decimal Cob_Suma_5 = 0;
            string Cob_Sublimite_5 = string.Empty;
            string Cob_Deducible_5 = string.Empty;
            string Cob_Coaseguro_5 = string.Empty;

            bool Cob_Habilitado_6 = false;
            string Cob_Nombre_6 = string.Empty;
            string Cob_Riesgo_6 = string.Empty;
            decimal Cob_Suma_6 = 0;
            string Cob_Sublimite_6 = string.Empty;
            string Cob_Deducible_6 = string.Empty;
            string Cob_Coaseguro_6 = string.Empty;

            bool Cob_Habilitado_7 = false;
            string Cob_Nombre_7 = string.Empty;
            string Cob_Riesgo_7 = string.Empty;
            decimal Cob_Suma_7 = 0;
            string Cob_Sublimite_7 = string.Empty;
            string Cob_Deducible_7 = string.Empty;
            string Cob_Coaseguro_7 = string.Empty;

            bool Cob_Habilitado_8 = false;
            string Cob_Nombre_8 = string.Empty;
            string Cob_Riesgo_8 = string.Empty;
            decimal Cob_Suma_8 = 0;
            string Cob_Sublimite_8 = string.Empty;
            string Cob_Deducible_8 = string.Empty;
            string Cob_Coaseguro_8 = string.Empty;

            bool Cob_Habilitado_9 = false;
            string Cob_Nombre_9 = string.Empty;
            string Cob_Riesgo_9 = string.Empty;
            decimal Cob_Suma_9 = 0;
            string Cob_Sublimite_9 = string.Empty;
            string Cob_Deducible_9 = string.Empty;
            string Cob_Coaseguro_9 = string.Empty;

            bool Cob_Habilitado_10 = false;
            string Cob_Nombre_10 = string.Empty;
            string Cob_Riesgo_10 = string.Empty;
            decimal Cob_Suma_10 = 0;
            string Cob_Sublimite_10 = string.Empty;
            string Cob_Deducible_10 = string.Empty;
            string Cob_Coaseguro_10 = string.Empty;

            bool Cob_Habilitado_11 = false;
            string Cob_Nombre_11 = string.Empty;
            string Cob_Riesgo_11 = string.Empty;
            decimal Cob_Suma_11 = 0;
            string Cob_Sublimite_11 = string.Empty;
            string Cob_Deducible_11 = string.Empty;
            string Cob_Coaseguro_11 = string.Empty;

            bool Cob_Habilitado_12 = false;
            string Cob_Nombre_12 = string.Empty;
            string Cob_Riesgo_12 = string.Empty;
            decimal Cob_Suma_12 = 0;
            string Cob_Sublimite_12 = string.Empty;
            string Cob_Deducible_12 = string.Empty;
            string Cob_Coaseguro_12 = string.Empty;

            bool Cob_Habilitado_13 = false;
            string Cob_Nombre_13 = string.Empty;
            string Cob_Riesgo_13 = string.Empty;
            decimal Cob_Suma_13 = 0;
            string Cob_Sublimite_13 = string.Empty;
            string Cob_Deducible_13 = string.Empty;
            string Cob_Coaseguro_13 = string.Empty;

            bool Cob_Habilitado_14 = false;
            string Cob_Nombre_14 = string.Empty;
            string Cob_Riesgo_14 = string.Empty;
            decimal Cob_Suma_14 = 0;
            string Cob_Sublimite_14 = string.Empty;
            string Cob_Deducible_14 = string.Empty;
            string Cob_Coaseguro_14 = string.Empty;

            // Procesar cada fila almacenada
            int filaIndex = 1;
            foreach (var fila in listaFilas)
            {

                if (filaIndex == 1)
                {
                    Cob_Habilitado_1 = (bool)fila["CheckBox"];
                    Cob_Nombre_1 = fila["TextBox1"] as string;
                    Cob_Riesgo_1 = fila["TextBox2"] as string;
                    Cob_Suma_1 = fila["TextBox3"] != null && decimal.TryParse(fila["TextBox3"].ToString(), out var tempCobSuma) ? tempCobSuma : 0;
                    Cob_Sublimite_1 = fila["TextBox4"] as string;
                    Cob_Deducible_1 = fila["TextBox5"] as string;
                    Cob_Coaseguro_1 = fila["TextBox6"] as string;
                }
                else if (filaIndex == 2)
                {
                    Cob_Habilitado_2 = (bool)fila["CheckBox"];
                    Cob_Nombre_2 = fila["TextBox1"] as string;
                    Cob_Riesgo_2 = fila["TextBox2"] as string;
                    Cob_Suma_2 = fila["TextBox3"] != null && decimal.TryParse(fila["TextBox3"].ToString(), out var tempCobSuma) ? tempCobSuma : 0;
                    Cob_Sublimite_2 = fila["TextBox4"] as string;
                    Cob_Deducible_2 = fila["TextBox5"] as string;
                    Cob_Coaseguro_2 = fila["TextBox6"] as string;
                }
                else if (filaIndex == 3)
                {
                    Cob_Habilitado_3 = (bool)fila["CheckBox"];
                    Cob_Nombre_3 = fila["TextBox1"] as string;
                    Cob_Riesgo_3 = fila["TextBox2"] as string;
                    Cob_Suma_3 = fila["TextBox3"] != null && decimal.TryParse(fila["TextBox3"].ToString(), out var tempCobSuma) ? tempCobSuma : 0;
                    Cob_Sublimite_3 = fila["TextBox4"] as string;
                    Cob_Deducible_3 = fila["TextBox5"] as string;
                    Cob_Coaseguro_3 = fila["TextBox6"] as string;
                }
                else if (filaIndex == 4)
                {
                    Cob_Habilitado_4 = (bool)fila["CheckBox"];
                    Cob_Nombre_4 = fila["TextBox1"] as string;
                    Cob_Riesgo_4 = fila["TextBox2"] as string;
                    Cob_Suma_4 = fila["TextBox3"] != null && decimal.TryParse(fila["TextBox3"].ToString(), out var tempCobSuma) ? tempCobSuma : 0;
                    Cob_Sublimite_4 = fila["TextBox4"] as string;
                    Cob_Deducible_4 = fila["TextBox5"] as string;
                    Cob_Coaseguro_4 = fila["TextBox6"] as string;
                }
                else if (filaIndex == 5)
                {
                    Cob_Habilitado_5= (bool)fila["CheckBox"];
                    Cob_Nombre_5 = fila["TextBox1"] as string;
                    Cob_Riesgo_5 = fila["TextBox2"] as string;
                    Cob_Suma_5 = fila["TextBox3"] != null && decimal.TryParse(fila["TextBox3"].ToString(), out var tempCobSuma) ? tempCobSuma : 0;
                    Cob_Sublimite_5 = fila["TextBox4"] as string;
                    Cob_Deducible_5 = fila["TextBox5"] as string;
                    Cob_Coaseguro_5 = fila["TextBox6"] as string;
                }
                else if (filaIndex == 6)
                {
                    Cob_Habilitado_6= (bool)fila["CheckBox"];
                    Cob_Nombre_6 = fila["TextBox1"] as string;
                    Cob_Riesgo_6 = fila["TextBox2"] as string;
                    Cob_Suma_6 = fila["TextBox3"] != null && decimal.TryParse(fila["TextBox3"].ToString(), out var tempCobSuma) ? tempCobSuma : 0;
                    Cob_Sublimite_6 = fila["TextBox4"] as string;
                    Cob_Deducible_6 = fila["TextBox5"] as string;
                    Cob_Coaseguro_6 = fila["TextBox6"] as string;
                }
                else if (filaIndex == 7)
                {
                    Cob_Habilitado_7 = (bool)fila["CheckBox"];
                    Cob_Nombre_7 = fila["TextBox1"] as string;
                    Cob_Riesgo_7 = fila["TextBox2"] as string;
                    Cob_Suma_7 = fila["TextBox3"] != null && decimal.TryParse(fila["TextBox3"].ToString(), out var tempCobSuma) ? tempCobSuma : 0;
                    Cob_Sublimite_7 = fila["TextBox4"] as string;
                    Cob_Deducible_7 = fila["TextBox5"] as string;
                    Cob_Coaseguro_7 = fila["TextBox6"] as string;
                }

                else if (filaIndex == 8)
                {
                    Cob_Habilitado_8 = (bool)fila["CheckBox"];
                    Cob_Nombre_8 = fila["TextBox1"] as string;
                    Cob_Riesgo_8 = fila["TextBox2"] as string;
                    Cob_Suma_8 = fila["TextBox3"] != null && decimal.TryParse(fila["TextBox3"].ToString(), out var tempCobSuma) ? tempCobSuma : 0;
                    Cob_Sublimite_8 = fila["TextBox4"] as string;
                    Cob_Deducible_8 = fila["TextBox5"] as string;
                    Cob_Coaseguro_8 = fila["TextBox6"] as string;
                }
                else if (filaIndex == 9)
                {
                    Cob_Habilitado_9 = (bool)fila["CheckBox"];
                    Cob_Nombre_9 = fila["TextBox1"] as string;
                    Cob_Riesgo_9 = fila["TextBox2"] as string;
                    Cob_Suma_9 = fila["TextBox3"] != null && decimal.TryParse(fila["TextBox3"].ToString(), out var tempCobSuma) ? tempCobSuma : 0;
                    Cob_Sublimite_9 = fila["TextBox4"] as string;
                    Cob_Deducible_9 = fila["TextBox5"] as string;
                    Cob_Coaseguro_9 = fila["TextBox6"] as string;
                }
                else if (filaIndex == 10)
                {
                    Cob_Habilitado_10 = (bool)fila["CheckBox"];
                    Cob_Nombre_10 = fila["TextBox1"] as string;
                    Cob_Riesgo_10 = fila["TextBox2"] as string;
                    Cob_Suma_10 = fila["TextBox3"] != null && decimal.TryParse(fila["TextBox3"].ToString(), out var tempCobSuma) ? tempCobSuma : 0;
                    Cob_Sublimite_10 = fila["TextBox4"] as string;
                    Cob_Deducible_10 = fila["TextBox5"] as string;
                    Cob_Coaseguro_10 = fila["TextBox6"] as string;
                }
                else if (filaIndex == 11)
                {
                    Cob_Habilitado_11 = (bool)fila["CheckBox"];
                    Cob_Nombre_11 = fila["TextBox1"] as string;
                    Cob_Riesgo_11 = fila["TextBox2"] as string;
                    Cob_Suma_11 = fila["TextBox3"] != null && decimal.TryParse(fila["TextBox3"].ToString(), out var tempCobSuma) ? tempCobSuma : 0;
                    Cob_Sublimite_11 = fila["TextBox4"] as string;
                    Cob_Deducible_11 = fila["TextBox5"] as string;
                    Cob_Coaseguro_11 = fila["TextBox6"] as string;
                }
                else if (filaIndex == 12)
                {
                    Cob_Habilitado_12 = (bool)fila["CheckBox"];
                    Cob_Nombre_12 = fila["TextBox1"] as string;
                    Cob_Riesgo_12 = fila["TextBox2"] as string;
                    Cob_Suma_12 = fila["TextBox3"] != null && decimal.TryParse(fila["TextBox3"].ToString(), out var tempCobSuma) ? tempCobSuma : 0;
                    Cob_Sublimite_12 = fila["TextBox4"] as string;
                    Cob_Deducible_12 = fila["TextBox5"] as string;
                    Cob_Coaseguro_12 = fila["TextBox6"] as string;
                }
                else if (filaIndex == 13)
                {
                    Cob_Habilitado_13 = (bool)fila["CheckBox"];
                    Cob_Nombre_13 = fila["TextBox1"] as string;
                    Cob_Riesgo_13 = fila["TextBox2"] as string;
                    Cob_Suma_13 = fila["TextBox3"] != null && decimal.TryParse(fila["TextBox3"].ToString(), out var tempCobSuma) ? tempCobSuma : 0;
                    Cob_Sublimite_13 = fila["TextBox4"] as string;
                    Cob_Deducible_13 = fila["TextBox5"] as string;
                    Cob_Coaseguro_13 = fila["TextBox6"] as string;
                }
                else if (filaIndex == 14)
                {
                    Cob_Habilitado_14 = (bool)fila["CheckBox"];
                    Cob_Nombre_14 = fila["TextBox1"] as string;
                    Cob_Riesgo_14 = fila["TextBox2"] as string;
                    Cob_Suma_14 = fila["TextBox3"] != null && decimal.TryParse(fila["TextBox3"].ToString(), out var tempCobSuma) ? tempCobSuma : 0;
                    Cob_Sublimite_14 = fila["TextBox4"] as string;
                    Cob_Deducible_14 = fila["TextBox5"] as string;
                    Cob_Coaseguro_14 = fila["TextBox6"] as string;
                }

                this.mpeMensaje.Show();

                filaIndex++;
            }

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Datos Personales
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                string Id_Usuario = Variables.wUserName;
                int IdStatus = 1;

                // Definir la consulta MERGE
                string strQuery = @" MERGE INTO ITM_70_3_4 AS target " +
                                   " USING (SELECT @Referencia AS Referencia, @SubReferencia AS SubReferencia, " +
                                   "        @Cob_Habilitado_1 AS Cob_Habilitado_1, @Cob_Nombre_1 AS Cob_Nombre_1, @Cob_Riesgo_1 AS Cob_Riesgo_1, @Cob_Suma_1 AS Cob_Suma_1," +
                                   "        @Cob_Sublimite_1 AS Cob_Sublimite_1, @Cob_Deducible_1 AS Cob_Deducible_1, @Cob_Coaseguro_1 AS Cob_Coaseguro_1, " +
                                   "        @Cob_Habilitado_2 AS Cob_Habilitado_2, @Cob_Nombre_2 AS Cob_Nombre_2, @Cob_Riesgo_2 AS Cob_Riesgo_2, @Cob_Suma_2 AS Cob_Suma_2," +
                                   "        @Cob_Sublimite_2 AS Cob_Sublimite_2, @Cob_Deducible_2 AS Cob_Deducible_2, @Cob_Coaseguro_2 AS Cob_Coaseguro_2, " +
                                   "        @Cob_Habilitado_3 AS Cob_Habilitado_3, @Cob_Nombre_3 AS Cob_Nombre_3, @Cob_Riesgo_3 AS Cob_Riesgo_3, @Cob_Suma_3 AS Cob_Suma_3," +
                                   "        @Cob_Sublimite_3 AS Cob_Sublimite_3, @Cob_Deducible_3 AS Cob_Deducible_3, @Cob_Coaseguro_3 AS Cob_Coaseguro_3, " +
                                   "        @Cob_Habilitado_4 AS Cob_Habilitado_4, @Cob_Nombre_4 AS Cob_Nombre_4, @Cob_Riesgo_4 AS Cob_Riesgo_4, @Cob_Suma_4 AS Cob_Suma_4," +
                                   "        @Cob_Sublimite_4 AS Cob_Sublimite_4, @Cob_Deducible_4 AS Cob_Deducible_4, @Cob_Coaseguro_4 AS Cob_Coaseguro_4, " +
                                   "        @Cob_Habilitado_5 AS Cob_Habilitado_5, @Cob_Nombre_5 AS Cob_Nombre_5, @Cob_Riesgo_5 AS Cob_Riesgo_5, @Cob_Suma_5 AS Cob_Suma_5," +
                                   "        @Cob_Sublimite_5 AS Cob_Sublimite_5, @Cob_Deducible_5 AS Cob_Deducible_5, @Cob_Coaseguro_5 AS Cob_Coaseguro_5, " +
                                   "        @Cob_Habilitado_6 AS Cob_Habilitado_6, @Cob_Nombre_6 AS Cob_Nombre_6, @Cob_Riesgo_6 AS Cob_Riesgo_6, @Cob_Suma_6 AS Cob_Suma_6," +
                                   "        @Cob_Sublimite_6 AS Cob_Sublimite_6, @Cob_Deducible_6 AS Cob_Deducible_6, @Cob_Coaseguro_6 AS Cob_Coaseguro_6, " +
                                   "        @Cob_Habilitado_7 AS Cob_Habilitado_7, @Cob_Nombre_7 AS Cob_Nombre_7, @Cob_Riesgo_7 AS Cob_Riesgo_7, @Cob_Suma_7 AS Cob_Suma_7," +
                                   "        @Cob_Sublimite_7 AS Cob_Sublimite_7, @Cob_Deducible_7 AS Cob_Deducible_7, @Cob_Coaseguro_7 AS Cob_Coaseguro_7, " +
                                   "        @Cob_Habilitado_8 AS Cob_Habilitado_8, @Cob_Nombre_8 AS Cob_Nombre_8, @Cob_Riesgo_8 AS Cob_Riesgo_8, @Cob_Suma_8 AS Cob_Suma_8," +
                                   "        @Cob_Sublimite_8 AS Cob_Sublimite_8, @Cob_Deducible_8 AS Cob_Deducible_8, @Cob_Coaseguro_8 AS Cob_Coaseguro_8, " +
                                   "        @Cob_Habilitado_9 AS Cob_Habilitado_9, @Cob_Nombre_9 AS Cob_Nombre_9, @Cob_Riesgo_9 AS Cob_Riesgo_9, @Cob_Suma_9 AS Cob_Suma_9," +
                                   "        @Cob_Sublimite_9 AS Cob_Sublimite_9, @Cob_Deducible_9 AS Cob_Deducible_9, @Cob_Coaseguro_9 AS Cob_Coaseguro_9, " +
                                   "        @Cob_Habilitado_10 AS Cob_Habilitado_10, @Cob_Nombre_10 AS Cob_Nombre_10, @Cob_Riesgo_10 AS Cob_Riesgo_10, @Cob_Suma_10 AS Cob_Suma_10," +
                                   "        @Cob_Sublimite_10 AS Cob_Sublimite_10, @Cob_Deducible_10 AS Cob_Deducible_10, @Cob_Coaseguro_10 AS Cob_Coaseguro_10, " +
                                   "        @Cob_Habilitado_11 AS Cob_Habilitado_11, @Cob_Nombre_11 AS Cob_Nombre_11, @Cob_Riesgo_11 AS Cob_Riesgo_11, @Cob_Suma_11 AS Cob_Suma_11," +
                                   "        @Cob_Sublimite_11 AS Cob_Sublimite_11, @Cob_Deducible_11 AS Cob_Deducible_11, @Cob_Coaseguro_11 AS Cob_Coaseguro_11, " +
                                   "        @Cob_Habilitado_12 AS Cob_Habilitado_12, @Cob_Nombre_12 AS Cob_Nombre_12, @Cob_Riesgo_12 AS Cob_Riesgo_12, @Cob_Suma_12 AS Cob_Suma_12," +
                                   "        @Cob_Sublimite_12 AS Cob_Sublimite_12, @Cob_Deducible_12 AS Cob_Deducible_12, @Cob_Coaseguro_12 AS Cob_Coaseguro_12, " +
                                   "        @Cob_Habilitado_13 AS Cob_Habilitado_13, @Cob_Nombre_13 AS Cob_Nombre_13, @Cob_Riesgo_13 AS Cob_Riesgo_13, @Cob_Suma_13 AS Cob_Suma_13," +
                                   "        @Cob_Sublimite_13 AS Cob_Sublimite_13, @Cob_Deducible_13 AS Cob_Deducible_13, @Cob_Coaseguro_13 AS Cob_Coaseguro_13, " +
                                   "        @Cob_Habilitado_14 AS Cob_Habilitado_14, @Cob_Nombre_14 AS Cob_Nombre_14, @Cob_Riesgo_14 AS Cob_Riesgo_14, @Cob_Suma_14 AS Cob_Suma_14," +
                                   "        @Cob_Sublimite_14 AS Cob_Sublimite_14, @Cob_Deducible_14 AS Cob_Deducible_14, @Cob_Coaseguro_14 AS Cob_Coaseguro_14, " +
                                   "        @Id_Usuario AS Id_Usuario, @IdStatus AS IdStatus ) AS source " +
                                   " ON (target.Referencia = source.Referencia AND target.SubReferencia = source.SubReferencia) " +
                                   " WHEN MATCHED THEN " +
                                   " UPDATE SET  " +
                                   "        Cob_Habilitado_1 = source.Cob_Habilitado_1, " +
                                   "        Cob_Nombre_1 = source.Cob_Nombre_1, " +
                                   "        Cob_Riesgo_1 = source.Cob_Riesgo_1, " +
                                   "        Cob_Suma_1 = source.Cob_Suma_1, " +
                                   "        Cob_Sublimite_1 = source.Cob_Sublimite_1, " +
                                   "        Cob_Deducible_1 = source.Cob_Deducible_1, " +
                                   "        Cob_Coaseguro_1 = source.Cob_Coaseguro_1, " +
                                   "        Cob_Habilitado_2 = source.Cob_Habilitado_2, " +
                                   "        Cob_Nombre_2 = source.Cob_Nombre_2, " +
                                   "        Cob_Riesgo_2 = source.Cob_Riesgo_2, " +
                                   "        Cob_Suma_2 = source.Cob_Suma_2, " +
                                   "        Cob_Sublimite_2 = source.Cob_Sublimite_2, " +
                                   "        Cob_Deducible_2 = source.Cob_Deducible_2, " +
                                   "        Cob_Coaseguro_2 = source.Cob_Coaseguro_2, " +
                                   "        Cob_Habilitado_3 = source.Cob_Habilitado_3, " +
                                   "        Cob_Nombre_3 = source.Cob_Nombre_3, " +
                                   "        Cob_Riesgo_3 = source.Cob_Riesgo_3, " +
                                   "        Cob_Suma_3 = source.Cob_Suma_3, " +
                                   "        Cob_Sublimite_3 = source.Cob_Sublimite_3, " +
                                   "        Cob_Deducible_3 = source.Cob_Deducible_3, " +
                                   "        Cob_Coaseguro_3 = source.Cob_Coaseguro_3, " +
                                   "        Cob_Habilitado_4 = source.Cob_Habilitado_4, " +
                                   "        Cob_Nombre_4 = source.Cob_Nombre_4, " +
                                   "        Cob_Riesgo_4 = source.Cob_Riesgo_4, " +
                                   "        Cob_Suma_4 = source.Cob_Suma_4, " +
                                   "        Cob_Sublimite_4 = source.Cob_Sublimite_4, " +
                                   "        Cob_Deducible_4 = source.Cob_Deducible_4, " +
                                   "        Cob_Coaseguro_4 = source.Cob_Coaseguro_4, " +
                                   "        Cob_Habilitado_5 = source.Cob_Habilitado_5, " +
                                   "        Cob_Nombre_5 = source.Cob_Nombre_5, " +
                                   "        Cob_Riesgo_5 = source.Cob_Riesgo_5, " +
                                   "        Cob_Suma_5 = source.Cob_Suma_5, " +
                                   "        Cob_Sublimite_5 = source.Cob_Sublimite_5, " +
                                   "        Cob_Deducible_5 = source.Cob_Deducible_5, " +
                                   "        Cob_Coaseguro_5 = source.Cob_Coaseguro_5, " +
                                   "        Cob_Habilitado_6 = source.Cob_Habilitado_6, " +
                                   "        Cob_Nombre_6 = source.Cob_Nombre_6, " +
                                   "        Cob_Riesgo_6 = source.Cob_Riesgo_6, " +
                                   "        Cob_Suma_6 = source.Cob_Suma_6, " +
                                   "        Cob_Sublimite_6 = source.Cob_Sublimite_6, " +
                                   "        Cob_Deducible_6 = source.Cob_Deducible_6, " +
                                   "        Cob_Coaseguro_6 = source.Cob_Coaseguro_6, " +
                                   "        Cob_Habilitado_7 = source.Cob_Habilitado_7, " +
                                   "        Cob_Nombre_7 = source.Cob_Nombre_7, " +
                                   "        Cob_Riesgo_7 = source.Cob_Riesgo_7, " +
                                   "        Cob_Suma_7 = source.Cob_Suma_7, " +
                                   "        Cob_Sublimite_7 = source.Cob_Sublimite_7, " +
                                   "        Cob_Deducible_7 = source.Cob_Deducible_7, " +
                                   "        Cob_Coaseguro_7 = source.Cob_Coaseguro_7, " +
                                   "        Cob_Habilitado_8 = source.Cob_Habilitado_8, " +
                                   "        Cob_Nombre_8 = source.Cob_Nombre_8, " +
                                   "        Cob_Riesgo_8 = source.Cob_Riesgo_8, " +
                                   "        Cob_Suma_8 = source.Cob_Suma_8, " +
                                   "        Cob_Sublimite_8 = source.Cob_Sublimite_8, " +
                                   "        Cob_Deducible_8 = source.Cob_Deducible_8, " +
                                   "        Cob_Coaseguro_8 = source.Cob_Coaseguro_8, " +
                                   "        Cob_Habilitado_9 = source.Cob_Habilitado_9, " +
                                   "        Cob_Nombre_9 = source.Cob_Nombre_9, " +
                                   "        Cob_Riesgo_9 = source.Cob_Riesgo_9, " +
                                   "        Cob_Suma_9 = source.Cob_Suma_9, " +
                                   "        Cob_Sublimite_9 = source.Cob_Sublimite_9, " +
                                   "        Cob_Deducible_9 = source.Cob_Deducible_9, " +
                                   "        Cob_Coaseguro_9 = source.Cob_Coaseguro_9, " +
                                   "        Cob_Habilitado_10 = source.Cob_Habilitado_10, " +
                                   "        Cob_Nombre_10 = source.Cob_Nombre_10, " +
                                   "        Cob_Riesgo_10 = source.Cob_Riesgo_10, " +
                                   "        Cob_Suma_10 = source.Cob_Suma_10, " +
                                   "        Cob_Sublimite_10 = source.Cob_Sublimite_10, " +
                                   "        Cob_Deducible_10 = source.Cob_Deducible_10, " +
                                   "        Cob_Coaseguro_10 = source.Cob_Coaseguro_10, " +

                                   "        Cob_Habilitado_11 = source.Cob_Habilitado_11, " +
                                   "        Cob_Nombre_11 = source.Cob_Nombre_11, " +
                                   "        Cob_Riesgo_11 = source.Cob_Riesgo_11, " +
                                   "        Cob_Suma_11 = source.Cob_Suma_11, " +
                                   "        Cob_Sublimite_11 = source.Cob_Sublimite_11, " +
                                   "        Cob_Deducible_11 = source.Cob_Deducible_11, " +
                                   "        Cob_Coaseguro_11 = source.Cob_Coaseguro_11, " +

                                   "        Cob_Habilitado_12 = source.Cob_Habilitado_12, " +
                                   "        Cob_Nombre_12 = source.Cob_Nombre_12, " +
                                   "        Cob_Riesgo_12 = source.Cob_Riesgo_12, " +
                                   "        Cob_Suma_12 = source.Cob_Suma_12, " +
                                   "        Cob_Sublimite_12 = source.Cob_Sublimite_12, " +
                                   "        Cob_Deducible_12 = source.Cob_Deducible_12, " +
                                   "        Cob_Coaseguro_12 = source.Cob_Coaseguro_12, " +

                                   "        Cob_Habilitado_13 = source.Cob_Habilitado_13, " +
                                   "        Cob_Nombre_13 = source.Cob_Nombre_13, " +
                                   "        Cob_Riesgo_13 = source.Cob_Riesgo_13, " +
                                   "        Cob_Suma_13 = source.Cob_Suma_13, " +
                                   "        Cob_Sublimite_13 = source.Cob_Sublimite_13, " +
                                   "        Cob_Deducible_13 = source.Cob_Deducible_13, " +
                                   "        Cob_Coaseguro_13 = source.Cob_Coaseguro_13, " +

                                   "        Cob_Habilitado_14 = source.Cob_Habilitado_14, " +
                                   "        Cob_Nombre_14 = source.Cob_Nombre_14, " +
                                   "        Cob_Riesgo_14 = source.Cob_Riesgo_14, " +
                                   "        Cob_Suma_14 = source.Cob_Suma_14, " +
                                   "        Cob_Sublimite_14 = source.Cob_Sublimite_14, " +
                                   "        Cob_Deducible_14 = source.Cob_Deducible_14, " +
                                   "        Cob_Coaseguro_14 = source.Cob_Coaseguro_14 " +
                                   " WHEN NOT MATCHED THEN " +
                                   " INSERT (Referencia, SubReferencia, " +
                                   "         Cob_Habilitado_1, Cob_Nombre_1, Cob_Riesgo_1, Cob_Suma_1, Cob_Sublimite_1, Cob_Deducible_1, Cob_Coaseguro_1," +
                                   "         Cob_Habilitado_2, Cob_Nombre_2, Cob_Riesgo_2, Cob_Suma_2, Cob_Sublimite_2, Cob_Deducible_2, Cob_Coaseguro_2," +
                                   "         Cob_Habilitado_3, Cob_Nombre_3, Cob_Riesgo_3, Cob_Suma_3, Cob_Sublimite_3, Cob_Deducible_3, Cob_Coaseguro_3," +
                                   "         Cob_Habilitado_4, Cob_Nombre_4, Cob_Riesgo_4, Cob_Suma_4, Cob_Sublimite_4, Cob_Deducible_4, Cob_Coaseguro_4," +
                                   "         Cob_Habilitado_5, Cob_Nombre_5, Cob_Riesgo_5, Cob_Suma_5, Cob_Sublimite_5, Cob_Deducible_5, Cob_Coaseguro_5," +
                                   "         Cob_Habilitado_6, Cob_Nombre_6, Cob_Riesgo_6, Cob_Suma_6, Cob_Sublimite_6, Cob_Deducible_6, Cob_Coaseguro_6," +
                                   "         Cob_Habilitado_7, Cob_Nombre_7, Cob_Riesgo_7, Cob_Suma_7, Cob_Sublimite_7, Cob_Deducible_7, Cob_Coaseguro_7," +
                                   "         Cob_Habilitado_8, Cob_Nombre_8, Cob_Riesgo_8, Cob_Suma_8, Cob_Sublimite_8, Cob_Deducible_8, Cob_Coaseguro_8," +
                                   "         Cob_Habilitado_9, Cob_Nombre_9, Cob_Riesgo_9, Cob_Suma_9, Cob_Sublimite_9, Cob_Deducible_9, Cob_Coaseguro_9," +
                                   "         Cob_Habilitado_10, Cob_Nombre_10, Cob_Riesgo_10, Cob_Suma_10, Cob_Sublimite_10, Cob_Deducible_10, Cob_Coaseguro_10," +
                                   "         Cob_Habilitado_11, Cob_Nombre_11, Cob_Riesgo_11, Cob_Suma_11, Cob_Sublimite_11, Cob_Deducible_11, Cob_Coaseguro_11," +
                                   "         Cob_Habilitado_12, Cob_Nombre_12, Cob_Riesgo_12, Cob_Suma_12, Cob_Sublimite_12, Cob_Deducible_12, Cob_Coaseguro_12," +
                                   "         Cob_Habilitado_13, Cob_Nombre_13, Cob_Riesgo_13, Cob_Suma_13, Cob_Sublimite_13, Cob_Deducible_13, Cob_Coaseguro_13," +
                                   "         Cob_Habilitado_14, Cob_Nombre_14, Cob_Riesgo_14, Cob_Suma_14, Cob_Sublimite_14, Cob_Deducible_14, Cob_Coaseguro_14," +
                                   "         Id_Usuario, IdStatus  ) " +
                                   " VALUES (source.Referencia, source.SubReferencia, " +
                                   "         source.Cob_Habilitado_1, source.Cob_Nombre_1, source.Cob_Riesgo_1, source.Cob_Suma_1, source.Cob_Sublimite_1, source.Cob_Deducible_1, source.Cob_Coaseguro_1, " +
                                   "         source.Cob_Habilitado_2, source.Cob_Nombre_2, source.Cob_Riesgo_2, source.Cob_Suma_2, source.Cob_Sublimite_2, source.Cob_Deducible_2, source.Cob_Coaseguro_2, " +
                                   "         source.Cob_Habilitado_3, source.Cob_Nombre_3, source.Cob_Riesgo_3, source.Cob_Suma_3, source.Cob_Sublimite_3, source.Cob_Deducible_3, source.Cob_Coaseguro_3, " +
                                   "         source.Cob_Habilitado_4, source.Cob_Nombre_4, source.Cob_Riesgo_4, source.Cob_Suma_4, source.Cob_Sublimite_4, source.Cob_Deducible_4, source.Cob_Coaseguro_4, " +
                                   "         source.Cob_Habilitado_5, source.Cob_Nombre_5, source.Cob_Riesgo_5, source.Cob_Suma_5, source.Cob_Sublimite_5, source.Cob_Deducible_5, source.Cob_Coaseguro_5, " +
                                   "         source.Cob_Habilitado_6, source.Cob_Nombre_6, source.Cob_Riesgo_6, source.Cob_Suma_6, source.Cob_Sublimite_6, source.Cob_Deducible_6, source.Cob_Coaseguro_6, " +
                                   "         source.Cob_Habilitado_7, source.Cob_Nombre_7, source.Cob_Riesgo_7, source.Cob_Suma_7, source.Cob_Sublimite_7, source.Cob_Deducible_7, source.Cob_Coaseguro_7, " +
                                   "         source.Cob_Habilitado_8, source.Cob_Nombre_8, source.Cob_Riesgo_8, source.Cob_Suma_8, source.Cob_Sublimite_8, source.Cob_Deducible_8, source.Cob_Coaseguro_8, " +
                                   "         source.Cob_Habilitado_9, source.Cob_Nombre_9, source.Cob_Riesgo_9, source.Cob_Suma_9, source.Cob_Sublimite_9, source.Cob_Deducible_9, source.Cob_Coaseguro_9, " +
                                   "         source.Cob_Habilitado_10, source.Cob_Nombre_10, source.Cob_Riesgo_10, source.Cob_Suma_10, source.Cob_Sublimite_10, source.Cob_Deducible_10, source.Cob_Coaseguro_10, " +
                                   "         source.Cob_Habilitado_11, source.Cob_Nombre_11, source.Cob_Riesgo_11, source.Cob_Suma_11, source.Cob_Sublimite_11, source.Cob_Deducible_11, source.Cob_Coaseguro_11, " +
                                   "         source.Cob_Habilitado_12, source.Cob_Nombre_12, source.Cob_Riesgo_12, source.Cob_Suma_12, source.Cob_Sublimite_12, source.Cob_Deducible_12, source.Cob_Coaseguro_12, " +
                                   "         source.Cob_Habilitado_13, source.Cob_Nombre_13, source.Cob_Riesgo_13, source.Cob_Suma_13, source.Cob_Sublimite_13, source.Cob_Deducible_13, source.Cob_Coaseguro_13, " +
                                   "         source.Cob_Habilitado_14, source.Cob_Nombre_14, source.Cob_Riesgo_14, source.Cob_Suma_14, source.Cob_Sublimite_14, source.Cob_Deducible_14, source.Cob_Coaseguro_14, " +
                                   "         source.Id_Usuario, source.IdStatus );";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@Cob_Habilitado_1", Cob_Habilitado_1);
                    cmd.Parameters.AddWithValue("@Cob_Nombre_1", Cob_Nombre_1);
                    cmd.Parameters.AddWithValue("@Cob_Riesgo_1", Cob_Riesgo_1);
                    cmd.Parameters.AddWithValue("@Cob_Suma_1", Cob_Suma_1);
                    cmd.Parameters.AddWithValue("@Cob_Sublimite_1", Cob_Sublimite_1);
                    cmd.Parameters.AddWithValue("@Cob_Deducible_1", Cob_Deducible_1);
                    cmd.Parameters.AddWithValue("@Cob_Coaseguro_1", Cob_Coaseguro_1);

                    cmd.Parameters.AddWithValue("@Cob_Habilitado_2", Cob_Habilitado_2);
                    cmd.Parameters.AddWithValue("@Cob_Nombre_2", Cob_Nombre_2);
                    cmd.Parameters.AddWithValue("@Cob_Riesgo_2", Cob_Riesgo_2);
                    cmd.Parameters.AddWithValue("@Cob_Suma_2", Cob_Suma_2);
                    cmd.Parameters.AddWithValue("@Cob_Sublimite_2", Cob_Sublimite_2);
                    cmd.Parameters.AddWithValue("@Cob_Deducible_2", Cob_Deducible_2);
                    cmd.Parameters.AddWithValue("@Cob_Coaseguro_2", Cob_Coaseguro_2);

                    cmd.Parameters.AddWithValue("@Cob_Habilitado_3", Cob_Habilitado_3);
                    cmd.Parameters.AddWithValue("@Cob_Nombre_3", Cob_Nombre_3);
                    cmd.Parameters.AddWithValue("@Cob_Riesgo_3", Cob_Riesgo_3);
                    cmd.Parameters.AddWithValue("@Cob_Suma_3", Cob_Suma_3);
                    cmd.Parameters.AddWithValue("@Cob_Sublimite_3", Cob_Sublimite_3);
                    cmd.Parameters.AddWithValue("@Cob_Deducible_3", Cob_Deducible_3);
                    cmd.Parameters.AddWithValue("@Cob_Coaseguro_3", Cob_Coaseguro_3);

                    cmd.Parameters.AddWithValue("@Cob_Habilitado_4", Cob_Habilitado_4);
                    cmd.Parameters.AddWithValue("@Cob_Nombre_4", Cob_Nombre_4);
                    cmd.Parameters.AddWithValue("@Cob_Riesgo_4", Cob_Riesgo_4);
                    cmd.Parameters.AddWithValue("@Cob_Suma_4", Cob_Suma_4);
                    cmd.Parameters.AddWithValue("@Cob_Sublimite_4", Cob_Sublimite_4);
                    cmd.Parameters.AddWithValue("@Cob_Deducible_4", Cob_Deducible_4);
                    cmd.Parameters.AddWithValue("@Cob_Coaseguro_4", Cob_Coaseguro_4);

                    cmd.Parameters.AddWithValue("@Cob_Habilitado_5", Cob_Habilitado_5);
                    cmd.Parameters.AddWithValue("@Cob_Nombre_5", Cob_Nombre_5);
                    cmd.Parameters.AddWithValue("@Cob_Riesgo_5", Cob_Riesgo_5);
                    cmd.Parameters.AddWithValue("@Cob_Suma_5", Cob_Suma_5);
                    cmd.Parameters.AddWithValue("@Cob_Sublimite_5", Cob_Sublimite_5);
                    cmd.Parameters.AddWithValue("@Cob_Deducible_5", Cob_Deducible_5);
                    cmd.Parameters.AddWithValue("@Cob_Coaseguro_5", Cob_Coaseguro_5);

                    cmd.Parameters.AddWithValue("@Cob_Habilitado_6", Cob_Habilitado_6);
                    cmd.Parameters.AddWithValue("@Cob_Nombre_6", Cob_Nombre_6);
                    cmd.Parameters.AddWithValue("@Cob_Riesgo_6", Cob_Riesgo_6);
                    cmd.Parameters.AddWithValue("@Cob_Suma_6", Cob_Suma_6);
                    cmd.Parameters.AddWithValue("@Cob_Sublimite_6", Cob_Sublimite_6);
                    cmd.Parameters.AddWithValue("@Cob_Deducible_6", Cob_Deducible_6);
                    cmd.Parameters.AddWithValue("@Cob_Coaseguro_6", Cob_Coaseguro_6);

                    cmd.Parameters.AddWithValue("@Cob_Habilitado_7", Cob_Habilitado_7);
                    cmd.Parameters.AddWithValue("@Cob_Nombre_7", Cob_Nombre_7);
                    cmd.Parameters.AddWithValue("@Cob_Riesgo_7", Cob_Riesgo_7);
                    cmd.Parameters.AddWithValue("@Cob_Suma_7", Cob_Suma_7);
                    cmd.Parameters.AddWithValue("@Cob_Sublimite_7", Cob_Sublimite_7);
                    cmd.Parameters.AddWithValue("@Cob_Deducible_7", Cob_Deducible_7);
                    cmd.Parameters.AddWithValue("@Cob_Coaseguro_7", Cob_Coaseguro_7);

                    cmd.Parameters.AddWithValue("@Cob_Habilitado_8", Cob_Habilitado_8);
                    cmd.Parameters.AddWithValue("@Cob_Nombre_8", Cob_Nombre_8);
                    cmd.Parameters.AddWithValue("@Cob_Riesgo_8", Cob_Riesgo_8);
                    cmd.Parameters.AddWithValue("@Cob_Suma_8", Cob_Suma_8);
                    cmd.Parameters.AddWithValue("@Cob_Sublimite_8", Cob_Sublimite_8);
                    cmd.Parameters.AddWithValue("@Cob_Deducible_8", Cob_Deducible_8);
                    cmd.Parameters.AddWithValue("@Cob_Coaseguro_8", Cob_Coaseguro_8);

                    cmd.Parameters.AddWithValue("@Cob_Habilitado_9", Cob_Habilitado_9);
                    cmd.Parameters.AddWithValue("@Cob_Nombre_9", Cob_Nombre_9);
                    cmd.Parameters.AddWithValue("@Cob_Riesgo_9", Cob_Riesgo_9);
                    cmd.Parameters.AddWithValue("@Cob_Suma_9", Cob_Suma_9);
                    cmd.Parameters.AddWithValue("@Cob_Sublimite_9", Cob_Sublimite_9);
                    cmd.Parameters.AddWithValue("@Cob_Deducible_9", Cob_Deducible_9);
                    cmd.Parameters.AddWithValue("@Cob_Coaseguro_9", Cob_Coaseguro_9);

                    cmd.Parameters.AddWithValue("@Cob_Habilitado_10", Cob_Habilitado_10);
                    cmd.Parameters.AddWithValue("@Cob_Nombre_10", Cob_Nombre_10);
                    cmd.Parameters.AddWithValue("@Cob_Riesgo_10", Cob_Riesgo_10);
                    cmd.Parameters.AddWithValue("@Cob_Suma_10", Cob_Suma_10);
                    cmd.Parameters.AddWithValue("@Cob_Sublimite_10", Cob_Sublimite_10);
                    cmd.Parameters.AddWithValue("@Cob_Deducible_10", Cob_Deducible_10);
                    cmd.Parameters.AddWithValue("@Cob_Coaseguro_10", Cob_Coaseguro_10);

                    cmd.Parameters.AddWithValue("@Cob_Habilitado_11", Cob_Habilitado_11);
                    cmd.Parameters.AddWithValue("@Cob_Nombre_11", Cob_Nombre_11);
                    cmd.Parameters.AddWithValue("@Cob_Riesgo_11", Cob_Riesgo_11);
                    cmd.Parameters.AddWithValue("@Cob_Suma_11", Cob_Suma_11);
                    cmd.Parameters.AddWithValue("@Cob_Sublimite_11", Cob_Sublimite_11);
                    cmd.Parameters.AddWithValue("@Cob_Deducible_11", Cob_Deducible_11);
                    cmd.Parameters.AddWithValue("@Cob_Coaseguro_11", Cob_Coaseguro_11);

                    cmd.Parameters.AddWithValue("@Cob_Habilitado_12", Cob_Habilitado_12);
                    cmd.Parameters.AddWithValue("@Cob_Nombre_12", Cob_Nombre_12);
                    cmd.Parameters.AddWithValue("@Cob_Riesgo_12", Cob_Riesgo_12);
                    cmd.Parameters.AddWithValue("@Cob_Suma_12", Cob_Suma_12);
                    cmd.Parameters.AddWithValue("@Cob_Sublimite_12", Cob_Sublimite_12);
                    cmd.Parameters.AddWithValue("@Cob_Deducible_12", Cob_Deducible_12);
                    cmd.Parameters.AddWithValue("@Cob_Coaseguro_12", Cob_Coaseguro_12);

                    cmd.Parameters.AddWithValue("@Cob_Habilitado_13", Cob_Habilitado_13);
                    cmd.Parameters.AddWithValue("@Cob_Nombre_13", Cob_Nombre_13);
                    cmd.Parameters.AddWithValue("@Cob_Riesgo_13", Cob_Riesgo_13);
                    cmd.Parameters.AddWithValue("@Cob_Suma_13", Cob_Suma_13);
                    cmd.Parameters.AddWithValue("@Cob_Sublimite_13", Cob_Sublimite_13);
                    cmd.Parameters.AddWithValue("@Cob_Deducible_13", Cob_Deducible_13);
                    cmd.Parameters.AddWithValue("@Cob_Coaseguro_13", Cob_Coaseguro_13);

                    cmd.Parameters.AddWithValue("@Cob_Habilitado_14", Cob_Habilitado_14);
                    cmd.Parameters.AddWithValue("@Cob_Nombre_14", Cob_Nombre_14);
                    cmd.Parameters.AddWithValue("@Cob_Riesgo_14", Cob_Riesgo_14);
                    cmd.Parameters.AddWithValue("@Cob_Suma_14", Cob_Suma_14);
                    cmd.Parameters.AddWithValue("@Cob_Sublimite_14", Cob_Sublimite_14);
                    cmd.Parameters.AddWithValue("@Cob_Deducible_14", Cob_Deducible_14);
                    cmd.Parameters.AddWithValue("@Cob_Coaseguro_14", Cob_Coaseguro_14);

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

        protected void Actualizar_Daños_Edificio_bk()
        {
            int? IdEdificio = null; // null = insert, valor = update

            // Si tienes IdEdificio de algún control o variable, asignarlo así:
            IdEdificio = Variables.wIdAsunto > 0 ? Variables.wIdAsunto : (int?)null;

            string AreaEdif = TxtAreaEdif.Text;
            string NivelEdif = TxtNivelEdif.Text;
            string CategoriaEdif = TxtCategoriaEdif.Text;
            string ElementoEdif = TxtElementoEdif.Text;
            string TipoEdif = TxtTipoEdif.Text;
            string MaterialesEdif = TxtMaterialesEdif.Text;
            string MedidaEdif_1 = TxtMedidaEdif_1.Text;
            string UnidadEdif_1 = ddlUnidadEdif_1.SelectedValue;
            string MedidaEdif_2 = TxtMedidaEdif_2.Text;
            string UnidadEdif_2 = ddlUnidadEdif_2.SelectedValue;
            string MedidaEdif_3 = TxtMedidaEdif_3.Text;
            string UnidadEdif_3 = ddlUnidadEdif_3.SelectedValue;
            string ObsEdif_1 = TxtObsEdif_1.Text;
            string ObsEdif_2 = TxtObsEdif_2.Text;

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Datos Personales
                int IdCategoria = int.Parse(ddlCategorias.SelectedValue);
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @"INSERT INTO ITM_99_1 (IdEdificio, IdCategoria, Referencia, SubReferencia, AreaEdif, NivelEdif, CategoriaEdif, ElementoEdif, TipoEdif,
                                                                    MaterialesEdif, MedidaEdif_1, UnidadEdif_1, MedidaEdif_2, UnidadEdif_2, MedidaEdif_3, UnidadEdif_3, 
                                                                    ObsEdif_1, ObsEdif_2, Id_Usuario, IdStatus) " +
                                   "VALUES (@IdEdificio, @IdCategoria, @Referencia, @SubReferencia, @AreaEdif, @NivelEdif, @CategoriaEdif, @ElementoEdif, @TipoEdif, " +
                                   "        @MaterialesEdif, @MedidaEdif_1, @UnidadEdif_1, @MedidaEdif_2, @UnidadEdif_2, @MedidaEdif_3, @UnidadEdif_3, " +
                                   "        @ObsEdif_1, @ObsEdif_2, @Id_Usuario, @IdStatus) " +
                                   " ON DUPLICATE KEY UPDATE " +
                                   " AreaEdif = VALUES(AreaEdif), " +
                                   " NivelEdif = VALUES(NivelEdif), " +
                                   " CategoriaEdif = VALUES(CategoriaEdif), " +
                                   " ElementoEdif = VALUES(ElementoEdif), " +
                                   " TipoEdif = VALUES(TipoEdif), " +
                                   " MaterialesEdif = VALUES(MaterialesEdif), " +
                                   " MedidaEdif_1 = VALUES(MedidaEdif_1), " +
                                   " UnidadEdif_1 = VALUES(UnidadEdif_1), " +
                                   " MedidaEdif_2 = VALUES(MedidaEdif_2), " +
                                   " UnidadEdif_2 = VALUES(UnidadEdif_2), " +
                                   " MedidaEdif_3 = VALUES(MedidaEdif_3), " +
                                   " UnidadEdif_3 = VALUES(UnidadEdif_3), " +
                                   " ObsEdif_1 = VALUES(ObsEdif_1), " +
                                   " ObsEdif_2 = VALUES(ObsEdif_2), " +
                                   " Id_Usuario = VALUES(Id_Usuario), " +
                                   " IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@IdEdificio", IdEdificio.HasValue ? IdEdificio.Value : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdCategoria", IdCategoria);
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@AreaEdif", AreaEdif);
                    cmd.Parameters.AddWithValue("@NivelEdif", NivelEdif);
                    cmd.Parameters.AddWithValue("@CategoriaEdif", CategoriaEdif);
                    cmd.Parameters.AddWithValue("@ElementoEdif", ElementoEdif);
                    cmd.Parameters.AddWithValue("@TipoEdif", TipoEdif);
                    cmd.Parameters.AddWithValue("@MaterialesEdif", MaterialesEdif);
                    cmd.Parameters.AddWithValue("@MedidaEdif_1", MedidaEdif_1);
                    cmd.Parameters.AddWithValue("@UnidadEdif_1", UnidadEdif_1);
                    cmd.Parameters.AddWithValue("@MedidaEdif_2", MedidaEdif_2);
                    cmd.Parameters.AddWithValue("@UnidadEdif_2", UnidadEdif_2);
                    cmd.Parameters.AddWithValue("@MedidaEdif_3", MedidaEdif_3);
                    cmd.Parameters.AddWithValue("@UnidadEdif_3", UnidadEdif_3);
                    cmd.Parameters.AddWithValue("@ObsEdif_1", ObsEdif_1);
                    cmd.Parameters.AddWithValue("@ObsEdif_2", ObsEdif_2);
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

        protected void Actualizar_Daños_Edificio(int? IdEdificio = null)
        {
            int IdCategoria = int.Parse(ddlCategorias.SelectedValue);
            string Referencia = Variables.wRef;
            int SubReferencia = Variables.wSubRef;

            string AreaEdif = TxtAreaEdif.Text;
            string NivelEdif = TxtNivelEdif.Text;
            string CategoriaEdif = TxtCategoriaEdif.Text;
            string ElementoEdif = TxtElementoEdif.Text;
            string TipoEdif = TxtTipoEdif.Text;
            string MaterialesEdif = TxtMaterialesEdif.Text;
            string MedidaEdif_1 = TxtMedidaEdif_1.Text;
            string UnidadEdif_1 = ddlUnidadEdif_1.SelectedValue;
            string MedidaEdif_2 = TxtMedidaEdif_2.Text;
            string UnidadEdif_2 = ddlUnidadEdif_2.SelectedValue;
            string MedidaEdif_3 = TxtMedidaEdif_3.Text;
            string UnidadEdif_3 = ddlUnidadEdif_3.SelectedValue;
            string ObsEdif_1 = TxtObsEdif_1.Text;
            string ObsEdif_2 = TxtObsEdif_2.Text;

            string Id_Usuario = Variables.wUserLogon;
            int IdStatus = 1;

            try
            {
                using (ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword))
                {
                    dbConn.Open();
                    string strQuery;

                    if (IdEdificio.HasValue)
                    {
                        // ACTUALIZAR registro existente
                        strQuery = @"UPDATE ITM_99_1
                                        SET AreaEdif = @AreaEdif,
                                            NivelEdif = @NivelEdif,
                                            CategoriaEdif = @CategoriaEdif,
                                            ElementoEdif = @ElementoEdif,
                                            TipoEdif = @TipoEdif,
                                            MaterialesEdif = @MaterialesEdif,
                                            MedidaEdif_1 = @MedidaEdif_1,
                                            UnidadEdif_1 = @UnidadEdif_1,
                                            MedidaEdif_2 = @MedidaEdif_2,
                                            UnidadEdif_2 = @UnidadEdif_2,
                                            MedidaEdif_3 = @MedidaEdif_3,
                                            UnidadEdif_3 = @UnidadEdif_3,
                                            ObsEdif_1 = @ObsEdif_1,
                                            ObsEdif_2 = @ObsEdif_2
                                      WHERE IdEdificio = @IdEdificio; ";

                        dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                        {
                            cmd.Parameters.AddWithValue("@IdEdificio", IdEdificio.Value);
                            cmd.Parameters.AddWithValue("@IdCategoria", IdCategoria);
                            cmd.Parameters.AddWithValue("@Referencia", Referencia);
                            cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                            cmd.Parameters.AddWithValue("@AreaEdif", AreaEdif);
                            cmd.Parameters.AddWithValue("@NivelEdif", NivelEdif);
                            cmd.Parameters.AddWithValue("@CategoriaEdif", CategoriaEdif);
                            cmd.Parameters.AddWithValue("@ElementoEdif", ElementoEdif);
                            cmd.Parameters.AddWithValue("@TipoEdif", TipoEdif);
                            cmd.Parameters.AddWithValue("@MaterialesEdif", MaterialesEdif);
                            cmd.Parameters.AddWithValue("@MedidaEdif_1", MedidaEdif_1);
                            cmd.Parameters.AddWithValue("@UnidadEdif_1", UnidadEdif_1);
                            cmd.Parameters.AddWithValue("@MedidaEdif_2", MedidaEdif_2);
                            cmd.Parameters.AddWithValue("@UnidadEdif_2", UnidadEdif_2);
                            cmd.Parameters.AddWithValue("@MedidaEdif_3", MedidaEdif_3);
                            cmd.Parameters.AddWithValue("@UnidadEdif_3", UnidadEdif_3);
                            cmd.Parameters.AddWithValue("@ObsEdif_1", ObsEdif_1);
                            cmd.Parameters.AddWithValue("@ObsEdif_2", ObsEdif_2);
                        });
                    }
                    else
                    {
                        // INSERTAR nuevo registro (sin IdEdificio)
                        strQuery = @"INSERT INTO ITM_99_1 (IdCategoria, Referencia, SubReferencia, AreaEdif, NivelEdif, CategoriaEdif, ElementoEdif, TipoEdif,
                                                                     MaterialesEdif, MedidaEdif_1, UnidadEdif_1, MedidaEdif_2, UnidadEdif_2, MedidaEdif_3, UnidadEdif_3,
                                                                     ObsEdif_1, ObsEdif_2, Id_Usuario, IdStatus)
                                                             VALUES (@IdCategoria, @Referencia, @SubReferencia, @AreaEdif, @NivelEdif, @CategoriaEdif, @ElementoEdif, @TipoEdif,
                                                                     @MaterialesEdif, @MedidaEdif_1, @UnidadEdif_1, @MedidaEdif_2, @UnidadEdif_2, @MedidaEdif_3, @UnidadEdif_3,
                                                                     @ObsEdif_1, @ObsEdif_2, @Id_Usuario, @IdStatus); ";

                        dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                        {
                            cmd.Parameters.AddWithValue("@IdCategoria", IdCategoria);
                            cmd.Parameters.AddWithValue("@Referencia", Referencia);
                            cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                            cmd.Parameters.AddWithValue("@AreaEdif", AreaEdif);
                            cmd.Parameters.AddWithValue("@NivelEdif", NivelEdif);
                            cmd.Parameters.AddWithValue("@CategoriaEdif", CategoriaEdif);
                            cmd.Parameters.AddWithValue("@ElementoEdif", ElementoEdif);
                            cmd.Parameters.AddWithValue("@TipoEdif", TipoEdif);
                            cmd.Parameters.AddWithValue("@MaterialesEdif", MaterialesEdif);
                            cmd.Parameters.AddWithValue("@MedidaEdif_1", MedidaEdif_1);
                            cmd.Parameters.AddWithValue("@UnidadEdif_1", UnidadEdif_1);
                            cmd.Parameters.AddWithValue("@MedidaEdif_2", MedidaEdif_2);
                            cmd.Parameters.AddWithValue("@UnidadEdif_2", UnidadEdif_2);
                            cmd.Parameters.AddWithValue("@MedidaEdif_3", MedidaEdif_3);
                            cmd.Parameters.AddWithValue("@UnidadEdif_3", UnidadEdif_3);
                            cmd.Parameters.AddWithValue("@ObsEdif_1", ObsEdif_1);
                            cmd.Parameters.AddWithValue("@ObsEdif_2", ObsEdif_2);
                            cmd.Parameters.AddWithValue("@Id_Usuario", Id_Usuario);
                            cmd.Parameters.AddWithValue("@IdStatus", IdStatus);
                        });
                    }

                    dbConn.Close();
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }


        protected void Actualizar_Daños_Otros(int? IdOtros = null)
        {
            int IdCategoria = int.Parse(ddlCategorias.SelectedValue);
            string Referencia = Variables.wRef;
            int SubReferencia = Variables.wSubRef;

            string CategoriaOtros = TxtCategoriaOtros.Text;
            string TpoOtros = TxtTpoOtros.Text;
            string GenOtros = TxtGenOtros.Text;
            string DescOtros = TxtDescOtros.Text;
            string CantOtros = TxtCantOtros.Text;
            string MarcaOtros = TxtMarcaOtros.Text;
            string ModOtros = TxtModOtros.Text;
            string NumSerieOtros = TxtNumSerieOtros.Text;
            string ObsOtros_1 = TxtObsOtros_1.Text;
            string ObsOtros_2 = TxtObsOtros_2.Text;

            string Id_Usuario = Variables.wUserLogon;
            int IdStatus = 1;

            try
            {
                using (ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword))
                {
                    dbConn.Open();
                    string strQuery;

                    if (IdOtros.HasValue)
                    {
                        // ACTUALIZAR registro existente
                        strQuery = @"UPDATE ITM_99_2
                                        SET CategoriaOtros = @CategoriaOtros,
                                            TpoOtros = @TpoOtros,
                                            GenOtros = @GenOtros,
                                           DescOtros = @DescOtros,
                                           CantOtros = @CantOtros,
                                          MarcaOtros = @MarcaOtros,
                                            ModOtros = @ModOtros,
                                       NumSerieOtros = @NumSerieOtros,
                                          ObsOtros_1 = @ObsOtros_1,
                                          ObsOtros_2 = @ObsOtros_2
                                      WHERE IdOtros = @IdOtros; ";

                        dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                        {
                            cmd.Parameters.AddWithValue("@IdOtros", IdOtros.Value);
                            cmd.Parameters.AddWithValue("@CategoriaOtros", CategoriaOtros);
                            cmd.Parameters.AddWithValue("@TpoOtros", TpoOtros);
                            cmd.Parameters.AddWithValue("@GenOtros", GenOtros);
                            cmd.Parameters.AddWithValue("@DescOtros", DescOtros);
                            cmd.Parameters.AddWithValue("@CantOtros", CantOtros);
                            cmd.Parameters.AddWithValue("@MarcaOtros", MarcaOtros);
                            cmd.Parameters.AddWithValue("@ModOtros", ModOtros);
                            cmd.Parameters.AddWithValue("@NumSerieOtros", NumSerieOtros);
                            cmd.Parameters.AddWithValue("@ObsOtros_1", ObsOtros_1);
                            cmd.Parameters.AddWithValue("@ObsOtros_2", ObsOtros_2);
                        });
                    }
                    else
                    {
                        // INSERTAR nuevo registro (sin IdOtros)
                        strQuery = @"INSERT INTO ITM_99_2 (IdCategoria, Referencia, SubReferencia, CategoriaOtros, TpoOtros, GenOtros, DescOtros, CantOtros, MarcaOtros,
                                                                  ModOtros, NumSerieOtros, ObsOtros_1, ObsOtros_2, Id_Usuario, IdStatus)
                                     VALUES (@IdCategoria, @Referencia, @SubReferencia, @CategoriaOtros, @TpoOtros, @GenOtros, @DescOtros, @CantOtros, @MarcaOtros, @ModOtros,  @NumSerieOtros,
                                             @ObsOtros_1, @ObsOtros_2, @Id_Usuario, @IdStatus); ";

                        dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                        {
                            // Agregar los parámetros y sus valores
                            cmd.Parameters.AddWithValue("@IdCategoria", IdCategoria);
                            cmd.Parameters.AddWithValue("@Referencia", Referencia);
                            cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                            cmd.Parameters.AddWithValue("@CategoriaOtros", CategoriaOtros);
                            cmd.Parameters.AddWithValue("@TpoOtros", TpoOtros);
                            cmd.Parameters.AddWithValue("@GenOtros", GenOtros);
                            cmd.Parameters.AddWithValue("@DescOtros", DescOtros);
                            cmd.Parameters.AddWithValue("@CantOtros", CantOtros);
                            cmd.Parameters.AddWithValue("@MarcaOtros", MarcaOtros);
                            cmd.Parameters.AddWithValue("@ModOtros", ModOtros);
                            cmd.Parameters.AddWithValue("@NumSerieOtros", NumSerieOtros);
                            cmd.Parameters.AddWithValue("@ObsOtros_1", ObsOtros_1);
                            cmd.Parameters.AddWithValue("@ObsOtros_2", ObsOtros_2);
                            cmd.Parameters.AddWithValue("@Id_Usuario", Id_Usuario);
                            cmd.Parameters.AddWithValue("@IdStatus", IdStatus);
                        });
                    }

                    dbConn.Close();
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Actualizar_Daños_Otros_bk()
        {
            string CategoriaOtros = TxtCategoriaOtros.Text;
            string TpoOtros = TxtTpoOtros.Text;
            string GenOtros = TxtGenOtros.Text;
            string DescOtros = TxtDescOtros.Text;
            string CantOtros = TxtCantOtros.Text;
            string MarcaOtros = TxtMarcaOtros.Text;
            string ModOtros = TxtModOtros.Text;
            string NumSerieOtros = TxtNumSerieOtros.Text;
            string ObsOtros_1 = TxtObsOtros_1.Text;
            string ObsOtros_2 = TxtObsOtros_2.Text;

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Datos Personales
                int IdCategoria = int.Parse(ddlCategorias.SelectedValue);
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @"INSERT INTO ITM_99_2 (IdCategoria, Referencia, SubReferencia, CategoriaOtros, TpoOtros, GenOtros, DescOtros, CantOtros, MarcaOtros, ModOtros, NumSerieOtros, 
                                                            ObsOtros_1, ObsOtros_2, Id_Usuario, IdStatus) " +
                                   "VALUES (@IdCategoria, @Referencia, @SubReferencia, @CategoriaOtros, @TpoOtros, @GenOtros, @DescOtros, @CantOtros, @MarcaOtros, @ModOtros,  @NumSerieOtros, " +
                                   "        @ObsOtros_1, @ObsOtros_2, @Id_Usuario, @IdStatus) " +
                                   " ON DUPLICATE KEY UPDATE " +
                                   " CategoriaOtros = VALUES(CategoriaOtros), " +
                                   " TpoOtros = VALUES(TpoOtros), " +
                                   " GenOtros = VALUES(GenOtros), " +
                                   " DescOtros = VALUES(DescOtros), " +
                                   " CantOtros = VALUES(CantOtros), " +
                                   " MarcaOtros = VALUES(MarcaOtros), " +
                                   " ModOtros = VALUES(ModOtros), " +
                                   " NumSerieOtros = VALUES(NumSerieOtros), " +
                                   " ObsOtros_1 = VALUES(ObsOtros_1), " +
                                   " ObsOtros_2 = VALUES(ObsOtros_2), " +
                                   " Id_Usuario = VALUES(Id_Usuario), " +
                                   " IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@IdCategoria", IdCategoria);
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@CategoriaOtros", CategoriaOtros);
                    cmd.Parameters.AddWithValue("@TpoOtros", TpoOtros);
                    cmd.Parameters.AddWithValue("@GenOtros", GenOtros);
                    cmd.Parameters.AddWithValue("@DescOtros", DescOtros);
                    cmd.Parameters.AddWithValue("@CantOtros", CantOtros);
                    cmd.Parameters.AddWithValue("@MarcaOtros", MarcaOtros);
                    cmd.Parameters.AddWithValue("@ModOtros", ModOtros);
                    cmd.Parameters.AddWithValue("@NumSerieOtros", NumSerieOtros);
                    cmd.Parameters.AddWithValue("@ObsOtros_1", ObsOtros_1);
                    cmd.Parameters.AddWithValue("@ObsOtros_2", ObsOtros_2);
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

        protected void Actualizar_Datos_Cobertura()
        {
            string Cob_Seccion = string.Empty;
            string Cob_Nombre = string.Empty;
            string Cob_Riesgo = string.Empty;
            string Cob_Suma = string.Empty;
            decimal Cob_ValSuma = 0;
            string Cob_Agregado = string.Empty;
            decimal Cob_ValAgregado = 0;
            string Cob_Sublimite = string.Empty;
            decimal Cob_ValSublimite = 0;
            string Cob_Deducible = string.Empty;
            decimal Cob_ValDeducible = 0;
            string Cob_Coaseguro = string.Empty;
            decimal Cob_ValCoaseguro = 0;
            string Cob_Notas = string.Empty;

            int Aplica_Siniestro = 0;

            if (chkAplicaSiniestro.Checked)
            {
                Aplica_Siniestro = 1;
            }

            // Procesar cada fila almacenada
            bool Cob_Habilitado = true;

            Cob_Nombre = TxtNomCobertura.Text;
            Cob_Seccion = TxtSeccion.Text;
            Cob_Riesgo = TxtRiesgo.Text;
            Cob_Suma = TxtSumaAsegurada.Text;   // decimal.TryParse(TxtSumaAsegurada.Text, out var resultado) ? resultado : 0; ;
            Cob_ValSuma = decimal.TryParse(TxtValSumaAsegurada.Text, out var resValSuma) ? resValSuma : 0; ;
            Cob_Agregado = TxtAgregado.Text;
            Cob_ValAgregado = decimal.TryParse(TxtValAgregado.Text, out var resValAgregado) ? resValAgregado : 0; ;
            Cob_Sublimite = TxtSublimite.Text;
            Cob_ValSublimite = decimal.TryParse(TxtValSublimite.Text, out var resValSublimite) ? resValSublimite : 0; ;
            Cob_Deducible = TxtDeducible.Text;
            Cob_ValDeducible = decimal.TryParse(TxtValDeducible.Text, out var resValDeducible) ? resValDeducible : 0; ;
            Cob_Coaseguro = TxtCoaseguro.Text;
            Cob_ValCoaseguro = decimal.TryParse(TxtValCoaseguro.Text, out var resValCoaseguro) ? resValCoaseguro : 0; ;
            Cob_Notas = TxtNotas.Text;

            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Inserta / Actualiza Datos Personales
                int IdCobertura = int.Parse(ddlCoberturas.SelectedValue);
             // int IdSeccion = int.Parse(ddlSecciones.SelectedValue);
                int IdSeccion = 0;
                string Referencia = Variables.wRef;
                int SubReferencia = Variables.wSubRef;

                string Id_Usuario = Variables.wUserLogon;
                int IdStatus = 1;

                string strQuery = @"INSERT INTO ITM_70_3_4 (Referencia, SubReferencia, IdSeccion, IdCobertura, Cob_Habilitado, Cob_Nombre, Cob_Seccion, Cob_Riesgo, Cob_Suma, Cob_ValSuma, Cob_Agregado, Cob_ValAgregado, 
                                                            Cob_Sublimite, Cob_ValSublimite, Cob_Deducible, Cob_ValDeducible, Cob_Coaseguro, Cob_ValCoaseguro, Cob_Notas, Aplica_Siniestro, Id_Usuario, IdStatus) " +
                                   "VALUES (@Referencia, @SubReferencia, @IdSeccion, @IdCobertura, @Cob_Habilitado, @Cob_Nombre, @Cob_Seccion, @Cob_Riesgo, @Cob_Suma, @Cob_ValSuma, @Cob_Agregado, @Cob_ValAgregado, " +
                                   "        @Cob_Sublimite, @Cob_ValSublimite, @Cob_Deducible, @Cob_ValDeducible, @Cob_Coaseguro, @Cob_ValCoaseguro, @Cob_Notas, @Aplica_Siniestro, @Id_Usuario, @IdStatus) " +
                                   " ON DUPLICATE KEY UPDATE " +
                                   " Cob_Habilitado = VALUES(Cob_Habilitado), " +
                                   " Cob_Nombre = VALUES(Cob_Nombre), " +
                                   " Cob_Seccion = VALUES(Cob_Seccion), " +
                                   " Cob_Riesgo = VALUES(Cob_Riesgo), " +
                                   " Cob_Suma = VALUES(Cob_Suma), " +
                                   " Cob_ValSuma = VALUES(Cob_ValSuma), " +
                                   " Cob_Agregado = VALUES(Cob_Agregado), " +
                                   " Cob_ValAgregado = VALUES(Cob_ValAgregado), " +
                                   " Cob_Sublimite = VALUES(Cob_Sublimite), " +
                                   " Cob_ValSublimite = VALUES(Cob_ValSublimite), " +
                                   " Cob_Deducible = VALUES(Cob_Deducible), " +
                                   " Cob_ValDeducible = VALUES(Cob_ValDeducible), " +
                                   " Cob_Coaseguro = VALUES(Cob_Coaseguro), " +
                                   " Cob_ValCoaseguro = VALUES(Cob_ValCoaseguro), " +
                                   " Cob_Notas = VALUES(Cob_Notas), " +
                                   " Aplica_Siniestro = VALUES(Aplica_Siniestro), " +
                                   " Id_Usuario = VALUES(Id_Usuario), " +
                                   " IdStatus = VALUES(IdStatus); ";

                int result = dbConn.ExecuteNonQueryWithParameters(strQuery, cmd =>
                {
                    // Agregar los parámetros y sus valores
                    cmd.Parameters.AddWithValue("@Referencia", Referencia);
                    cmd.Parameters.AddWithValue("@SubReferencia", SubReferencia);
                    cmd.Parameters.AddWithValue("@IdSeccion", IdSeccion);
                    cmd.Parameters.AddWithValue("@IdCobertura", IdCobertura);
                    cmd.Parameters.AddWithValue("@Cob_Habilitado", Cob_Habilitado);
                    cmd.Parameters.AddWithValue("@Cob_Nombre", Cob_Nombre);
                    cmd.Parameters.AddWithValue("@Cob_Seccion", Cob_Seccion);
                    cmd.Parameters.AddWithValue("@Cob_Riesgo", Cob_Riesgo);
                    cmd.Parameters.AddWithValue("@Cob_Suma", Cob_Suma);
                    cmd.Parameters.AddWithValue("@Cob_ValSuma", Cob_ValSuma);
                    cmd.Parameters.AddWithValue("@Cob_Agregado", Cob_Agregado);
                    cmd.Parameters.AddWithValue("@Cob_ValAgregado", Cob_ValAgregado);
                    cmd.Parameters.AddWithValue("@Cob_Sublimite", Cob_Sublimite);
                    cmd.Parameters.AddWithValue("@Cob_ValSublimite", Cob_ValSublimite);
                    cmd.Parameters.AddWithValue("@Cob_Deducible", Cob_Deducible);
                    cmd.Parameters.AddWithValue("@Cob_ValDeducible", Cob_ValDeducible);
                    cmd.Parameters.AddWithValue("@Cob_Coaseguro", Cob_Coaseguro);
                    cmd.Parameters.AddWithValue("@Cob_ValCoaseguro", Cob_ValCoaseguro);
                    cmd.Parameters.AddWithValue("@Cob_Notas", Cob_Notas);
                    cmd.Parameters.AddWithValue("@Aplica_Siniestro", Aplica_Siniestro);
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

        public int GetConsulta_Datos_Coberturas(string pReferencia, string pSubReferencia)
        {

            try
            {

                // Define los textos que deseas mostrar
                string[] labels = { "EDIFICIO", "REMOCIÓN", "CONTENIDOS", "CONSECUENCIALES", "GASTOS EXTRA", "EQUIPO ELECTRONICO", "ROTURA DE MAQUINARIA", "ROBO",
                        "DINERO Y VALORES", "RESPONSABILIDAD CIVIL", "TRANSPORTES", "CIBER", "LINEAS FINANCIERAS", "OTRAS" };

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SET LANGUAGE Spanish " +
                                  "  SELECT Cob_Habilitado_1, Cob_Nombre_1, Cob_Riesgo_1, Cob_Suma_1, Cob_Sublimite_1, Cob_Deducible_1, Cob_Coaseguro_1," +
                                  "         Cob_Habilitado_2, Cob_Nombre_2, Cob_Riesgo_2, Cob_Suma_2, Cob_Sublimite_2, Cob_Deducible_2, Cob_Coaseguro_2," +
                                  "         Cob_Habilitado_3, Cob_Nombre_3, Cob_Riesgo_3, Cob_Suma_3, Cob_Sublimite_3, Cob_Deducible_3, Cob_Coaseguro_3," +
                                  "         Cob_Habilitado_4, Cob_Nombre_4, Cob_Riesgo_4, Cob_Suma_4, Cob_Sublimite_4, Cob_Deducible_4, Cob_Coaseguro_4," +
                                  "         Cob_Habilitado_5, Cob_Nombre_5, Cob_Riesgo_5, Cob_Suma_5, Cob_Sublimite_5, Cob_Deducible_5, Cob_Coaseguro_5," +
                                  "         Cob_Habilitado_6, Cob_Nombre_6, Cob_Riesgo_6, Cob_Suma_6, Cob_Sublimite_6, Cob_Deducible_6, Cob_Coaseguro_6," +
                                  "         Cob_Habilitado_7, Cob_Nombre_7, Cob_Riesgo_7, Cob_Suma_7, Cob_Sublimite_7, Cob_Deducible_7, Cob_Coaseguro_7," +
                                  "         Cob_Habilitado_8, Cob_Nombre_8, Cob_Riesgo_8, Cob_Suma_8, Cob_Sublimite_8, Cob_Deducible_8, Cob_Coaseguro_8," +
                                  "         Cob_Habilitado_9, Cob_Nombre_9, Cob_Riesgo_9, Cob_Suma_9, Cob_Sublimite_9, Cob_Deducible_9, Cob_Coaseguro_9," +
                                  "         Cob_Habilitado_10, Cob_Nombre_10, Cob_Riesgo_10, Cob_Suma_10, Cob_Sublimite_10, Cob_Deducible_10, Cob_Coaseguro_10," +
                                  "         Cob_Habilitado_11, Cob_Nombre_11, Cob_Riesgo_11, Cob_Suma_11, Cob_Sublimite_11, Cob_Deducible_11, Cob_Coaseguro_11," +
                                  "         Cob_Habilitado_12, Cob_Nombre_12, Cob_Riesgo_12, Cob_Suma_12, Cob_Sublimite_12, Cob_Deducible_12, Cob_Coaseguro_12," +
                                  "         Cob_Habilitado_13, Cob_Nombre_13, Cob_Riesgo_13, Cob_Suma_13, Cob_Sublimite_13, Cob_Deducible_13, Cob_Coaseguro_13," +
                                  "         Cob_Habilitado_14, Cob_Nombre_14, Cob_Riesgo_14, Cob_Suma_14, Cob_Sublimite_14, Cob_Deducible_14, Cob_Coaseguro_14" +
                                  "   FROM ITM_70_3_4 t0 " +
                                  "  WHERE t0.IdStatus IN (1) " +
                                  "    AND t0.Referencia = '" + pReferencia + "'" +
                                  "    AND t0.SubReferencia = '" + pSubReferencia + "'";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                dbConn.Close();

                // Prepara un nuevo DataTable para vincular al Repeater
                DataTable dtCoberturas = new DataTable();
                dtCoberturas.Columns.Add("Label", typeof(string));
                dtCoberturas.Columns.Add("Cob_Habilitado", typeof(bool));
                dtCoberturas.Columns.Add("Cob_Nombre", typeof(string));
                dtCoberturas.Columns.Add("Cob_Riesgo", typeof(string));
                dtCoberturas.Columns.Add("Cob_Suma", typeof(string));
                dtCoberturas.Columns.Add("Cob_Sublimite", typeof(string));
                dtCoberturas.Columns.Add("Cob_Deducible", typeof(string));
                dtCoberturas.Columns.Add("Cob_Coaseguro", typeof(string));

                if (dt.Rows.Count > 0)
                {
                    // Llena el DataTable con los datos de la base de datos
                    for (int i = 0; i < labels.Length; i++)
                    {
                        DataRow row = dtCoberturas.NewRow();
                        row["Label"] = labels[i];

                        row["Cob_Habilitado"] = dt.Rows[0]["Cob_Habilitado_" + (i + 1)] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["Cob_Habilitado_" + (i + 1)]) : false;
                        row["Cob_Nombre"] = dt.Rows[0]["Cob_Nombre_" + (i + 1)] != DBNull.Value ? dt.Rows[0]["Cob_Nombre_" + (i + 1)].ToString() : "";
                        row["Cob_Riesgo"] = dt.Rows[0]["Cob_Riesgo_" + (i + 1)] != DBNull.Value ? dt.Rows[0]["Cob_Riesgo_" + (i + 1)].ToString() : "";
                        row["Cob_Suma"] = dt.Rows[0]["Cob_Suma_" + (i + 1)] != DBNull.Value ? dt.Rows[0]["Cob_Suma_" + (i + 1)].ToString() : "";
                        row["Cob_Sublimite"] = dt.Rows[0]["Cob_Sublimite_" + (i + 1)] != DBNull.Value ? dt.Rows[0]["Cob_Sublimite_" + (i + 1)].ToString() : "";
                        row["Cob_Deducible"] = dt.Rows[0]["Cob_Deducible_" + (i + 1)] != DBNull.Value ? dt.Rows[0]["Cob_Deducible_" + (i + 1)].ToString() : "";
                        row["Cob_Coaseguro"] = dt.Rows[0]["Cob_Coaseguro_" + (i + 1)] != DBNull.Value ? dt.Rows[0]["Cob_Coaseguro_" + (i + 1)].ToString() : "";

                        dtCoberturas.Rows.Add(row);
                    }
                }
                else
                {
                    // Si no hay datos, llena el DataTable con valores predeterminados
                    foreach (var label in labels)
                    {
                        DataRow row = dtCoberturas.NewRow();
                        row["Label"] = label;
                        row["Cob_Habilitado"] = false;
                        row["Cob_Nombre"] = "";
                        row["Cob_Riesgo"] = "";
                        row["Cob_Suma"] = "";
                        row["Cob_Sublimite"] = "";
                        row["Cob_Deducible"] = "";
                        row["Cob_Coaseguro"] = "";

                        dtCoberturas.Rows.Add(row);
                    }
                }

                // Vincula el nuevo DataTable al Repeater
                // Repeater1.DataSource = dtCoberturas;
                // Repeater1.DataBind();

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

        protected void ddlCoberturas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdCoberturas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GrdCoberturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdCoberturas_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdCoberturas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdCoberturas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdCoberturas, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = Unit.Pixel(25);      // ImgEditar
                e.Row.Cells[1].Width = Unit.Pixel(25);      // ImgEliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[2].Visible = false;         // IdSeccion
                e.Row.Cells[3].Visible = false;         // IdCobertura
                e.Row.Cells[5].Visible = false;         // Cob_Nombre
                e.Row.Cells[6].Visible = false;         // Cob_Seccion
                e.Row.Cells[7].Visible = false;         // Cob_Riesgo
                e.Row.Cells[8].Visible = false;         // Cob_Suma
                e.Row.Cells[9].Visible = false;         // Cob_Agregado
                e.Row.Cells[10].Visible = false;        // Cob_Sublimite
                e.Row.Cells[11].Visible = false;        // Cob_Deducible
                e.Row.Cells[12].Visible = false;        // Cob_Coaseguro
                e.Row.Cells[13].Visible = false;        // Cob_Notas
                e.Row.Cells[19].Visible = false;        // Aplica_Siniestro 
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].Visible = false;         // IdSeccion
                e.Row.Cells[3].Visible = false;         // IdCobertura
                e.Row.Cells[5].Visible = false;         // Cob_Nombre
                e.Row.Cells[6].Visible = false;         // Cob_Seccion
                e.Row.Cells[7].Visible = false;         // Cob_Riesgo
                e.Row.Cells[8].Visible = false;         // Cob_Suma
                e.Row.Cells[9].Visible = false;         // Cob_Agregado
                e.Row.Cells[10].Visible = false;        // Cob_Sublimite
                e.Row.Cells[11].Visible = false;        // Cob_Deducible
                e.Row.Cells[12].Visible = false;        // Cob_Coaseguro
                e.Row.Cells[13].Visible = false;        // Cob_Notas
                e.Row.Cells[19].Visible = false;        // Aplica_Siniestro
            }
        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            // ddlSecciones.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[2].Text));
            
            // Disparar el evento SelectedIndexChanged manualmente
            // ddlSecciones_SelectedIndexChanged(ddlSecciones, EventArgs.Empty);

            ddlCoberturas.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[3].Text));
            
            TxtNomCobertura.Text = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[5].Text));
            TxtSeccion.Text = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[6].Text));
            TxtRiesgo.Text = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[7].Text));

            TxtSumaAsegurada.Text = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[8].Text));
            TxtAgregado.Text = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[9].Text));
            TxtSublimite.Text = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[10].Text));
            TxtDeducible.Text = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[11].Text));
            TxtCoaseguro.Text = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[12].Text));
            TxtNotas.Text = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[13].Text));

            TxtValSumaAsegurada.Text = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[14].Text));
            TxtValAgregado.Text = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[15].Text));
            TxtValSublimite.Text = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[16].Text));
            TxtValDeducible.Text = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[17].Text));
            TxtValCoaseguro.Text = Server.HtmlDecode(Convert.ToString(GrdCoberturas.Rows[index].Cells[18].Text));

            string valorCelda = Server.HtmlDecode(GrdCoberturas.Rows[index].Cells[19].Text).Trim();

            bool aplica = false;    // valor por defecto

            if (!string.IsNullOrEmpty(valorCelda) && valorCelda != "&nbsp;")
            {
                // Manejar 0/1
                if (valorCelda == "1")
                    aplica = true;
                else if (valorCelda == "0")
                    aplica = false;
                // Manejar True/False (por si el Grid devuelve texto)
                else if (valorCelda.Equals("true", StringComparison.OrdinalIgnoreCase))
                    aplica = true;
                else if (valorCelda.Equals("false", StringComparison.OrdinalIgnoreCase))
                    aplica = false;
            }

            chkAplicaSiniestro.Checked = aplica;

            // ddlSecciones.Enabled = false;
            ddlCoberturas.Enabled = false;

            TxtSeccion.ReadOnly = true;
            TxtSumaAsegurada.ReadOnly = true;
            TxtValSumaAsegurada.ReadOnly = true;
            TxtAgregado.ReadOnly = true;
            TxtValAgregado.ReadOnly = true;
            TxtNomCobertura.ReadOnly = true;
            TxtSeccion.ReadOnly = true;
            TxtRiesgo.ReadOnly = true;
            TxtSublimite.ReadOnly = true;
            TxtValSublimite.ReadOnly = true;
            TxtDeducible.ReadOnly = true;
            TxtValDeducible.ReadOnly = true;
            TxtCoaseguro.ReadOnly = true;
            TxtValCoaseguro.ReadOnly = true;
            TxtNotas.ReadOnly = true;
            chkAplicaSiniestro.Enabled = false;

            BtnAnularPnl17.Visible = true;
            btnEditarPnl17.Enabled = true;
            BtnAgregarPnl17.Enabled = false;
        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {

            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            LblMessage_1.Text = "¿Desea eliminar la cobertura ?";
            mpeMensaje_1.Show();

        }

        protected void Eliminar_tbCoberturas(int iIdSeccion, int iIdCobertura)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName;

                // Eliminar registro tabla
                string strQuery = "DELETE FROM ITM_70_3_4 " +
                                  " WHERE Referencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = '" + Variables.wSubRef + "' " +
                                  "   AND IdSeccion = " + iIdSeccion + " " +
                                  "   AND IdCobertura = " + iIdCobertura + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino cobertura, correctamente";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        
        protected void Eliminar_tbEdificio(int iIdEdificio, int iIdCategoria)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName;

                // Eliminar registro tabla
                string strQuery = "DELETE FROM ITM_99_1 " +
                                  " WHERE Referencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = '" + Variables.wSubRef + "' " +
                                  "   AND IdEdificio = " + iIdEdificio + " " +
                                  "   AND IdCategoria = " + iIdCategoria + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino categoria, correctamente";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        protected void Eliminar_tbDañosOtros(int iIdOtros, int iIdCategoria)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string sUsuario = Variables.wUserName;

                // Eliminar registro tabla
                string strQuery = "DELETE FROM ITM_99_2 " +
                                  " WHERE Referencia = '" + Variables.wRef + "' " +
                                  "   AND SubReferencia = '" + Variables.wSubRef + "' " +
                                  "   AND IdOtros = " + iIdOtros + " " +
                                  "   AND IdCategoria = " + iIdCategoria + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                LblMessage.Text = "Se elimino categoria, correctamente";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {

            int index = Variables.wRenglon;

            int iIdSeccion = int.Parse(Server.HtmlDecode(GrdCoberturas.Rows[index].Cells[2].Text));
            int iIdCobertura = int.Parse(Server.HtmlDecode(GrdCoberturas.Rows[index].Cells[3].Text));

            Eliminar_tbCoberturas(iIdSeccion, iIdCobertura);

            GetAltaCoberturas();

        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void chkCopiarDatos_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCopiarDatos.Checked)
            {
                TxtNombreAsegurado1.Text = TxtNomContratante.Text;
                TxtRFC_Asegurado1.Text = TxtRFC_Contratante.Text;
                TxtTpoAsegurado1.Text = TxtTpoContratante.Text;
                TxtEmailAsegurado1.Text = TxtEmailContratante.Text;
                TxtTelefonoAsegurado1.Text = TxtTelefonoContratante.Text;
                TxtCelularAsegurado1.Text = TxtCelularContratante.Text;
                TxtCalleAsegurado1.Text = TxtCalleContratante.Text;
                TxtNumExtAsegurado1.Text = TxtNumExtContratante.Text;
                TxtNumIntAsegurado1.Text = TxtNumIntContratante.Text;

                ddlEstadoAsegurado1.SelectedValue = ddlEstadoContratante.SelectedValue;
                // Disparar el evento SelectedIndexChanged manualmente
                ddlEstadoAsegurado1_SelectedIndexChanged(ddlEstadoAsegurado1, EventArgs.Empty);

                ddlMunicipiosAsegurado1.SelectedValue = ddlMunicipiosContratante.SelectedValue;

                TxtColoniaAsegurado1.Text = TxtColoniaContratante.Text;
                TxtCPostalAsegurado1.Text = TxtCPostalContratante.Text;
                TxtOtrosAsegurado1.Text = TxtOtrosContratante.Text;

            }
        }

        protected void BtnAceptar_Del_Doc_Click(object sender, EventArgs e)
        {
            int IdSeccion = 3;
            int IdCategoria = Convert.ToInt32(ddlConclusion.SelectedValue);

            Delete_Doc_Conclusion(IdSeccion, IdCategoria);

            GetDocumentos(TxtSubReferencia.Text);
        }

        protected void BtnCancelar_Del_Doc_Click(object sender, EventArgs e)
        {

        }

        protected void ImgDel_Documento_Click(object sender, ImageClickEventArgs e)
        {
            LblMessage_2.Text = "¿Desea eliminar documento(s) estatus ?";
            mpeMensaje_2.Show();
        }

        protected void BtnConvenioAjuste_Click(object sender, EventArgs e)
        {
            try
            {
                if (Variables.wIdTpoAsunto == 2)
                {
                    Dictionary<string, TextBox> fechas = new Dictionary<string, TextBox>
                    {
                        { "Fecha de Ocurrencia", TxtFechaOcurrencia },
                        { "Fecha de Asignación", TxtFechaAsignacion },
                        { "Fecha de Inspección", TxtFechaInspeccion },
                        { "Fecha de Contacto", TxtFechaContacto },
                        { "Fecha de Inicio de Vigencia", TxtFechaIniVigencia },
                        { "Fecha de Fin de Vigencia", TxtFechaFinVigencia }
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

                    Convenio_Ajuste();

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

        protected void BtnAceptar_CrearCuaderno_Click(object sender, EventArgs e)
        {
            Response.Redirect("fwDocument_Notebook.aspx?Ref=" + Variables.wRef + "&SubRef=" + Variables.wSubRef + "&Create=" + "1", true);
        }

        protected void BtnCancelar_CrearCuaderno_Click(object sender, EventArgs e)
        {

        }

        protected void ddlEstSiniestro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlEstSiniestro.SelectedValue != "0")
            {
                GetConclusion();
            }

        }

        protected void ddlProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetCoberturas();
        }

        protected void ddlSecciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            // GetCoberturas();
        }


        public string Get_Url_OneDrive()
        {
            string urlOneDrive = string.Empty;
            string strQuery = "SELECT Url_OneDrive FROM ITM_01 WHERE Id = @Valor";

            try
            {
                // Abrir conexión utilizando tu clase personalizada
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                using (MySqlCommand command = new MySqlCommand(strQuery, dbConn.Connection))    // Usar la conexión de dbConn
                {
                    // Agregar el parámetro a la consulta
                    command.Parameters.AddWithValue("@Valor", "01");

                    // Ejecutar la consulta
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        urlOneDrive = result.ToString();
                    }
                }

                dbConn.Close(); // Asegúrate de cerrar la conexión
            }
            catch (Exception ex)
            {
                // Manejo de errores
                urlOneDrive = "Error: " + ex.Message;
            }

            return urlOneDrive;
        }

        public string Url_OneDrive_Path()
        {
            try
            {
                string Url_OneDrive = string.Empty;

                switch (Variables.wIdTpoAsunto)
                {
                    case 1:     // NOTIFICACION
                        Url_OneDrive = (string)Session["Url_OneDrive"];

                        break;

                    case 2:     // SIMPLE   
                        Url_OneDrive = (string)Session["Url_OneDrive"] + "1.1 AJUSTE SIMPLE" + "\\";

                        if (Variables.wIdTpoProyecto == 1)
                        {
                            Url_OneDrive = (string)Session["Url_OneDrive"] + "1.1 AJUSTE SIMPLE" + "\\" + "PROYECTOS ESPECIALES" + "\\";
                        }

                        break;

                    case 3:     // COMPLEJO
                        Url_OneDrive = (string)Session["Url_OneDrive"] + "1.2 AJUSTE - COMPLEX" + "\\";

                        if (Variables.wIdTpoProyecto == 1)
                        {
                            Url_OneDrive = (string)Session["Url_OneDrive"] + "1.2 AJUSTE - COMPLEX" + "\\" + "PROYECTOS ESPECIALES" + "\\";
                        }

                        break;

                    case 4:     // LITIGIO
                        Url_OneDrive = (string)Session["Url_OneDrive"] + "2. LITIGIO" + "\\";

                        if (Variables.wIdTpoProyecto == 1)
                        {
                            Url_OneDrive = (string)Session["Url_OneDrive"] + "2. LITIGIO" + "\\" + "PROYECTOS ESPECIALES" + "\\";
                        }

                        break;

                    default:
                        // code block
                        break;
                }

                return Url_OneDrive;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }

            return string.Empty;
        }


        protected void BtnRepFotografico_Click(object sender, EventArgs e)
        {
            string referencia = TxtSubReferencia.Text;
            string url = "https://158.23.56.14:8181/SistemaReportesSiniestros?Referencia=" + referencia;

            string script = "window.open('" + url + "', '_blank');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "AbrirNuevaPestana", script, true);
        }

        public (string token, string passwordTemporal) GenerarYGuardarToken(int idAsunto, int horasValidez = 24)
        {
            // Token seguro (64 hex chars)
            byte[] tokenData = new byte[32];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(tokenData);

            string token = BitConverter.ToString(tokenData).Replace("-", "").ToLower();

            // Contraseña temporal de 6 dígitos
            string passwordTemporal = new Random().Next(100000, 999999).ToString();

            // Usando tu clase personalizada para conexión
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);

            try
            {
                dbConn.Open();

                string query = @"INSERT INTO ITM_Tokens (IdAsunto, Token, PasswordTemporal, FechaExpira, Usado)
                                 VALUES (@IdAsunto, @Token, @Password, DATE_ADD(NOW(), INTERVAL @Horas HOUR), 0);";

                using (MySqlCommand cmd = new MySqlCommand(query, dbConn.Connection))
                {
                    cmd.Parameters.AddWithValue("@IdAsunto", idAsunto);
                    cmd.Parameters.AddWithValue("@Token", token);
                    cmd.Parameters.AddWithValue("@Password", passwordTemporal);
                    cmd.Parameters.AddWithValue("@Horas", horasValidez);

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                dbConn.Close();
            }

            return (token, passwordTemporal);
        }

        protected void btnShowPanel21_Click(object sender, EventArgs e)
        {
            pnl21.Visible = !pnl21.Visible;   // Cambia la visibilidad del Panel 1 al contrario de su estado actual

            if (pnl21.Visible)
            {
                string flechaHaciaArriba = "\u25B2";
                btnShowPanel21.Text = flechaHaciaArriba; // Flecha hacia arriba
                pnl21.Visible = true;
            }
            else
            {
                string flechaHaciaAbajo = "\u25BC";
                btnShowPanel21.Text = flechaHaciaAbajo; // Flecha hacia abajo
                pnl21.Visible = false;
            }
        }

        protected void BtnAnularPnl4_Click(object sender, EventArgs e)
        {
            // Inicializar Controles
            ddlCategorias.Enabled = true;

            // Edificio
            TxtAreaEdif.Text = string.Empty;
            TxtAreaEdif.ReadOnly = false;

            TxtNivelEdif.Text = string.Empty;
            TxtNivelEdif.ReadOnly = false;

            TxtCategoriaEdif.Text = string.Empty;
            TxtCategoriaEdif.ReadOnly = false;

            TxtElementoEdif.Text = string.Empty;
            TxtElementoEdif.ReadOnly = false;

            TxtTipoEdif.Text = string.Empty;
            TxtTipoEdif.ReadOnly = false;

            TxtMaterialesEdif.Text = string.Empty;
            TxtMaterialesEdif.ReadOnly = false;

            TxtMedidaEdif_1.Text = string.Empty;
            TxtMedidaEdif_1.ReadOnly = false;

            ddlUnidadEdif_1.SelectedValue = "0";
            ddlUnidadEdif_1.Enabled = true;

            TxtMedidaEdif_2.Text = string.Empty;
            TxtMedidaEdif_2.ReadOnly = false;

            ddlUnidadEdif_2.SelectedValue = "0";
            ddlUnidadEdif_2.Enabled = true;

            TxtMedidaEdif_3.Text = string.Empty;
            TxtMedidaEdif_3.ReadOnly = false;

            ddlUnidadEdif_3.SelectedValue = "0";
            ddlUnidadEdif_3.Enabled = true;

            TxtObsEdif_1.Text = string.Empty;
            TxtObsEdif_1.ReadOnly = false;

            TxtObsEdif_2.Text = string.Empty;
            TxtObsEdif_2.ReadOnly = false;

            // Otros
            TxtCategoriaOtros.Text = string.Empty;
            TxtCategoriaOtros.ReadOnly = false;

            TxtTpoOtros.Text = string.Empty;
            TxtTpoOtros.ReadOnly = false;

            TxtGenOtros.Text = string.Empty;
            TxtGenOtros.ReadOnly = false;

            TxtDescOtros.Text = string.Empty;
            TxtDescOtros.ReadOnly = false;

            TxtCantOtros.Text = string.Empty;
            TxtCantOtros.ReadOnly = false;

            TxtMarcaOtros.Text = string.Empty;
            TxtMarcaOtros.ReadOnly = false;

            TxtModOtros.Text = string.Empty;
            TxtModOtros.ReadOnly = false;

            TxtNumSerieOtros.Text = string.Empty;
            TxtNumSerieOtros.ReadOnly = false;

            TxtObsOtros_1.Text = string.Empty;
            TxtObsOtros_1.ReadOnly = false;

            TxtObsOtros_2.Text = string.Empty;
            TxtObsOtros_2.ReadOnly = false;

            btnEditarPnl4.Visible = true;
            btnActualizarPnl4.Visible = false;
            BtnAnularPnl4.Visible = false;

            btnEditarPnl4.Enabled = false;
            BtnAgregarPnl4.Enabled = true;
        }

        protected void btnEditarPnl4_Click(object sender, EventArgs e)
        {
            // habilitar controles pnl4
            // Categoria EDIFICIO
            TxtAreaEdif.ReadOnly = false;
            TxtNivelEdif.ReadOnly = false;
            TxtCategoriaEdif.ReadOnly = false;
            TxtElementoEdif.ReadOnly = false;
            TxtTipoEdif.ReadOnly = false;
            TxtMaterialesEdif.ReadOnly = false;
            TxtMedidaEdif_1.ReadOnly = false;
            ddlUnidadEdif_1.Enabled = true;
            TxtMedidaEdif_2.ReadOnly = false;
            ddlUnidadEdif_2.Enabled = true;
            TxtMedidaEdif_3.ReadOnly = false;
            ddlUnidadEdif_3.Enabled = true;
            TxtObsEdif_1.ReadOnly = false;
            TxtObsEdif_2.ReadOnly = false;

            // Categoria OTRA(S)
            TxtCategoriaOtros.ReadOnly = false;
            TxtTpoOtros.ReadOnly = false;
            TxtGenOtros.ReadOnly = false;
            TxtDescOtros.ReadOnly = false;
            TxtCantOtros.ReadOnly = false;
            TxtMarcaOtros.ReadOnly = false;
            TxtModOtros.ReadOnly = false;
            TxtNumSerieOtros.ReadOnly = false;
            TxtObsOtros_1.ReadOnly = false;
            TxtObsOtros_2.ReadOnly = false;

            ddlCategorias.Enabled = false;

            btnEditarPnl4.Visible = false;
            btnActualizarPnl4.Visible = true;
            BtnAnularPnl4.Visible = true;
        }

        protected void btnActualizarPnl4_Click(object sender, EventArgs e)
        {
            if (ddlCategorias.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Categoría";
                mpeMensaje.Show();
                return;
            }

            // Validamos la opción seleccionada
            if (ddlCategorias.SelectedIndex == 0)
            {

            }
            else if (ddlCategorias.SelectedItem.Text.ToUpper() == "EDIFICIO")
            {
                Actualizar_Daños_Edificio(Variables.wIdAsunto);

                inicializar_daños_edificio();

                GetAltaDaños_Edificio();
            }
            else
            {
                Actualizar_Daños_Otros(Variables.wIdAsunto);

                inicializar_daños_otros();

                GetAltaDaños_Otros(Convert.ToInt32(ddlCategorias.SelectedValue));
            }

            // inicializar controles.
            ddlCategorias.Enabled = true;

            btnEditarPnl4.Visible = true;
            btnActualizarPnl4.Visible = false;
            BtnAnularPnl4.Visible = false;

            BtnAgregarPnl4.Enabled = true;
            btnEditarPnl4.Enabled = false;

            LblMessage.Text = "Se han aplicado los cambios, correctamente";
            this.mpeMensaje.Show();
        }

        protected void ddlCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Ocultamos todos los paneles primero
            pnlVacio.Visible = pnlEdificio.Visible = pnlOtros.Visible =
            pnlEdifGrid.Visible = pnlOtrosGrid.Visible = pnlBotones.Visible = false;

            // Validamos la opción seleccionada
            if (ddlCategorias.SelectedIndex == 0)
            {
                // Nada seleccionado
                pnlVacio.Visible = true;
            }
            else if (ddlCategorias.SelectedItem.Text.ToUpper() == "EDIFICIO")
            {
                GetAltaDaños_Edificio();

                pnlEdificio.Visible = true;
                pnlEdifGrid.Visible = true;
                pnlBotones.Visible = true;
            }
            else
            {
                GetAltaDaños_Otros(Convert.ToInt32(ddlCategorias.SelectedValue));

                // Todo lo demás
                pnlOtros.Visible = true;
                pnlOtrosGrid.Visible = true;
                pnlBotones.Visible = true;

            }
        }

        protected void GrdEdificio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GrdEdificio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
        }

        protected void GrdEdificio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //    e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdEdificio, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[15].Width = Unit.Pixel(25);     // ImgEditar
                e.Row.Cells[16].Width = Unit.Pixel(25);     // ImgEliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;             // IdEdificio
                e.Row.Cells[1].Visible = false;             // IdCategoria
                e.Row.Cells[2].Visible = false;             // Referencia
                e.Row.Cells[3].Visible = false;             // SubReferencia
                e.Row.Cells[16].Visible = false;            // ObsEdif_1
                e.Row.Cells[17].Visible = false;            // ObsEdif_2
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;             // IdEdificio
                e.Row.Cells[1].Visible = false;             // IdCategoria
                e.Row.Cells[2].Visible = false;             // Referencia
                e.Row.Cells[3].Visible = false;             // SubReferencia
                e.Row.Cells[16].Visible = false;            // ObsEdif_1
                e.Row.Cells[17].Visible = false;            // ObsEdif_2
            }
        }

        protected void GrdEdificio_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdEdificio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ImgDaños_Edif_Del_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnDañosEdif_Aceptar.Visible = true;
            BtnDañosEdif_Cancel.Visible = true;
            BtnDañosEdif_Cerrar.Visible = false;

            LblMessage_4.Text = "¿Desea eliminar la categoría ?";
            mpeMensaje_4.Show();
        }

        protected void ImgDaños_Edif_Add_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            Variables.wIdAsunto = Convert.ToInt32(GrdEdificio.Rows[index].Cells[0].Text);
            ddlCategorias.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdEdificio.Rows[index].Cells[1].Text));

            TxtAreaEdif.Text = Server.HtmlDecode(Convert.ToString(GrdEdificio.Rows[index].Cells[4].Text));
            TxtNivelEdif.Text = Server.HtmlDecode(Convert.ToString(GrdEdificio.Rows[index].Cells[5].Text));
            TxtCategoriaEdif.Text = Server.HtmlDecode(Convert.ToString(GrdEdificio.Rows[index].Cells[6].Text));
            TxtElementoEdif.Text = Server.HtmlDecode(Convert.ToString(GrdEdificio.Rows[index].Cells[7].Text));
            TxtTipoEdif.Text = Server.HtmlDecode(Convert.ToString(GrdEdificio.Rows[index].Cells[8].Text));
            TxtMaterialesEdif.Text = Server.HtmlDecode(Convert.ToString(GrdEdificio.Rows[index].Cells[9].Text));

            TxtMedidaEdif_1.Text = Server.HtmlDecode(Convert.ToString(GrdEdificio.Rows[index].Cells[10].Text));
            ddlUnidadEdif_1.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdEdificio.Rows[index].Cells[11].Text));
            TxtMedidaEdif_2.Text = Server.HtmlDecode(Convert.ToString(GrdEdificio.Rows[index].Cells[12].Text));
            ddlUnidadEdif_2.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdEdificio.Rows[index].Cells[13].Text));
            TxtMedidaEdif_3.Text = Server.HtmlDecode(Convert.ToString(GrdEdificio.Rows[index].Cells[14].Text));
            ddlUnidadEdif_3.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdEdificio.Rows[index].Cells[15].Text));

            TxtObsEdif_1.Text = Server.HtmlDecode(Convert.ToString(GrdEdificio.Rows[index].Cells[16].Text));
            TxtObsEdif_2.Text = Server.HtmlDecode(Convert.ToString(GrdEdificio.Rows[index].Cells[17].Text));

            ddlCategorias.Enabled = false;

            TxtAreaEdif.ReadOnly = true;
            TxtNivelEdif.ReadOnly = true;
            TxtCategoriaEdif.ReadOnly = true;
            TxtElementoEdif.ReadOnly = true;
            TxtTipoEdif.ReadOnly = true;
            TxtMaterialesEdif.ReadOnly = true;

            TxtMedidaEdif_1.ReadOnly = true;
            ddlUnidadEdif_1.Enabled = false;
            TxtMedidaEdif_2.ReadOnly = true;
            ddlUnidadEdif_2.Enabled = false;
            TxtMedidaEdif_3.ReadOnly = true;
            ddlUnidadEdif_3.Enabled = false;

            TxtObsEdif_1.ReadOnly = true;
            TxtObsEdif_2.ReadOnly = true;

            BtnAnularPnl4.Visible = true;
            btnEditarPnl4.Enabled = true;
            BtnAgregarPnl4.Enabled = false;
        }

        protected void GrdOtrosDaños_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GrdOtrosDaños_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdOtrosDaños_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //    e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdOtrosDaños, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[13].Width = Unit.Pixel(25);     // ImgEditar
                e.Row.Cells[14].Width = Unit.Pixel(25);     // ImgEliminar
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;             // IdOtros
                e.Row.Cells[1].Visible = false;             // IdCategoria
                e.Row.Cells[2].Visible = false;             // Referencia
                e.Row.Cells[3].Visible = false;             // SubReferencia
                e.Row.Cells[12].Visible = false;            // ObsOtros_1
                e.Row.Cells[13].Visible = false;            // ObsOtros_2
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;             // IdOtros
                e.Row.Cells[1].Visible = false;             // IdCategoria
                e.Row.Cells[2].Visible = false;             // Referencia
                e.Row.Cells[3].Visible = false;             // SubReferencia
                e.Row.Cells[12].Visible = false;            // ObsOtros_1
                e.Row.Cells[13].Visible = false;            // ObsOtros_2
            }
        }

        protected void GrdOtrosDaños_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdOtrosDaños_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ImgDaños_Otros_Add_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            Variables.wIdAsunto = Convert.ToInt32(GrdOtrosDaños.Rows[index].Cells[0].Text);
            ddlCategorias.SelectedValue = Server.HtmlDecode(Convert.ToString(GrdOtrosDaños.Rows[index].Cells[1].Text));

            TxtCategoriaOtros.Text = Server.HtmlDecode(Convert.ToString(GrdOtrosDaños.Rows[index].Cells[4].Text));
            TxtTpoOtros.Text = Server.HtmlDecode(Convert.ToString(GrdOtrosDaños.Rows[index].Cells[5].Text));
            TxtGenOtros.Text = Server.HtmlDecode(Convert.ToString(GrdOtrosDaños.Rows[index].Cells[6].Text));

            TxtDescOtros.Text = Server.HtmlDecode(Convert.ToString(GrdOtrosDaños.Rows[index].Cells[7].Text));

            TxtCantOtros.Text = Server.HtmlDecode(Convert.ToString(GrdOtrosDaños.Rows[index].Cells[8].Text));
            TxtMarcaOtros.Text = Server.HtmlDecode(Convert.ToString(GrdOtrosDaños.Rows[index].Cells[9].Text));
            TxtModOtros.Text = Server.HtmlDecode(Convert.ToString(GrdOtrosDaños.Rows[index].Cells[10].Text));
            TxtNumSerieOtros.Text = Server.HtmlDecode(Convert.ToString(GrdOtrosDaños.Rows[index].Cells[11].Text));

            TxtObsOtros_1.Text = Server.HtmlDecode(Convert.ToString(GrdOtrosDaños.Rows[index].Cells[12].Text));
            TxtObsOtros_2.Text = Server.HtmlDecode(Convert.ToString(GrdOtrosDaños.Rows[index].Cells[13].Text));

            ddlCategorias.Enabled = false;

            TxtCategoriaOtros.ReadOnly = true;
            TxtTpoOtros.ReadOnly = true;
            TxtGenOtros.ReadOnly = true;
            TxtDescOtros.ReadOnly = true;

            TxtCantOtros.ReadOnly = true;
            TxtMarcaOtros.ReadOnly = true;
            TxtModOtros.ReadOnly = true;
            TxtNumSerieOtros.ReadOnly = true;

            TxtObsOtros_1.ReadOnly = true;
            TxtObsOtros_2.ReadOnly = true;

            BtnAnularPnl4.Visible = true;
            btnEditarPnl4.Enabled = true;
            BtnAgregarPnl4.Enabled = false;
        }

        protected void ImgDaños_Otros_Del_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            BtnDañosOtros_Aceptar.Visible = true;
            BtnDañosOtros_Cancel.Visible = true;
            BtnDañosOtros_Cerrar.Visible = false;

            LblMessage_5.Text = "¿Desea eliminar la categoría ?";
            mpeMensaje_5.Show();
        }

        private void Inicializar_GrdEdificio()
        {
            // Crea un DataTable vacío con la estructura necesaria
            DataTable dt = DataTableVacio_Edificio();

            // Verifica si el DataTable tiene filas
            if (dt.Rows.Count == 0)
            {
                // Mostrar el mensaje de "No hay resultados"
                GrdEdificio.ShowHeaderWhenEmpty = true;
                GrdEdificio.EmptyDataText = "No hay resultados.";
            }

            // Enlaza el DataTable (vacío o lleno) al GridView
            GrdEdificio.DataSource = dt;
            GrdEdificio.DataBind();
        }

        private DataTable DataTableVacio_Edificio()
        {
            DataTable dt = new DataTable();

            // Define las columnas del DataTable
            dt.Columns.Add("IdEdificio", typeof(string));
            dt.Columns.Add("AreaEdif", typeof(string));
            dt.Columns.Add("NivelEdif", typeof(string));
            dt.Columns.Add("CategoriaEdif", typeof(string));
            dt.Columns.Add("ElementoEdif", typeof(string));
            dt.Columns.Add("TipoEdif", typeof(string));
            dt.Columns.Add("MaterialesEdif", typeof(string));
            dt.Columns.Add("MedidaEdif_1", typeof(string));
            dt.Columns.Add("UnidadEdif_1", typeof(string));
            dt.Columns.Add("MedidaEdif_2", typeof(string));
            dt.Columns.Add("UnidadEdif_2", typeof(string));
            dt.Columns.Add("MedidaEdif_3", typeof(string));
            dt.Columns.Add("UnidadEdif_3", typeof(string));
            // Agrega más columnas según sea necesario

            return dt;
        }

        private void Inicializar_GrdOtrosDaños()
        {
            // Crea un DataTable vacío con la estructura necesaria
            DataTable dt = DataTableVacio_OtrosDaños();

            // Verifica si el DataTable tiene filas
            if (dt.Rows.Count == 0)
            {
                // Mostrar el mensaje de "No hay resultados"
                GrdOtrosDaños.ShowHeaderWhenEmpty = true;
                GrdOtrosDaños.EmptyDataText = "No hay resultados.";
            }

            // Enlaza el DataTable (vacío o lleno) al GridView
            GrdOtrosDaños.DataSource = dt;
            GrdOtrosDaños.DataBind();
        }

        private DataTable DataTableVacio_OtrosDaños()
        {
            DataTable dt = new DataTable();

            // Define las columnas del DataTable
            dt.Columns.Add("IdOtros", typeof(string));
            dt.Columns.Add("CategoriaOtros", typeof(string));
            dt.Columns.Add("TpoOtros", typeof(string));
            dt.Columns.Add("GenOtros", typeof(string));
            dt.Columns.Add("DescOtros", typeof(string));
            dt.Columns.Add("CantOtros", typeof(string));
            dt.Columns.Add("MarcaOtros", typeof(string));
            dt.Columns.Add("ModOtros", typeof(string));
            dt.Columns.Add("NumSerieOtros", typeof(string));
            // Agrega más columnas según sea necesario

            return dt;
        }

        protected void BtnAgregarPnl4_Click(object sender, EventArgs e)
        {
            Variables.wIdAsunto = 0;

            if (ddlCategorias.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccionar Categoría";
                mpeMensaje.Show();
                return;
            }

            // Validamos la opción seleccionada
            if (ddlCategorias.SelectedIndex == 0)
            {

            }
            else if (ddlCategorias.SelectedItem.Text.ToUpper() == "EDIFICIO")
            {
                Actualizar_Daños_Edificio();

                ddlUnidadEdif_1.Enabled = true;
                ddlUnidadEdif_2.Enabled = true;
                ddlUnidadEdif_3.Enabled = true;

                inicializar_daños_edificio();

                GetAltaDaños_Edificio();
            }
            else
            {
                Actualizar_Daños_Otros();

                inicializar_daños_otros();


                GetAltaDaños_Otros(Convert.ToInt32(ddlCategorias.SelectedValue));
            }

            LblMessage.Text = "Se agrego categoria, correctamente";
            this.mpeMensaje.Show();

            btnEditarPnl4.Visible = true;
            btnActualizarPnl4.Visible = false;
            BtnAnularPnl4.Visible = false;

            ddlCategorias.Enabled = true;

        }

        protected void BtnAnularRiesgos_Click(object sender, EventArgs e)
        {
            DesactivarCheckBoxes(grdSeccion_2, false);
            UpdatePanel10.Update();

            BtnEditarRiesgos.Visible = true;
            BtnActualizarRiesgos.Visible = false;
            BtnAnularRiesgos.Visible = false;
        }

        protected void BtnEditarRiesgos_Click(object sender, EventArgs e)
        {
            // Activar CheckBoxes (RIESGOS)
            DesactivarCheckBoxes(grdSeccion_2, true);
            UpdatePanel10.Update();

            BtnEditarRiesgos.Visible = false;
            BtnActualizarRiesgos.Visible = true;
            BtnAnularRiesgos.Visible = true;
        }

        protected void BtnActualizarRiesgos_Click(object sender, EventArgs e)
        {
            // Agregar registros con los datos de cada cuaderno a cargar
            string sReferencia = Variables.wRef;
            int iSubReferencia = Variables.wSubRef;
            string IdAseguradora = Variables.wPrefijo_Aseguradora;
            int IdConclusion = Convert.ToInt32(ddlConclusion.SelectedValue);
            int IdRegimen = Convert.ToInt32(ddlTpoAsegurado.SelectedValue);

            // Eliminar registros
            Delete_Seccion_ITM_91(2);

            // Actualizar Riesgos;
            Obtener_Valores_ChBox(grdSeccion_2, "ChBoxSeccion_2", 2, "ITM_82");

            // Eliminar Secciones (2) RIESGOS de la tabla ITM_47
            Delete_Seccion_ITM_47(2);

            // Agregar Secciones (2) RIESGOS de la tabla ITM_47
            Add_tbDetCategoriaSeccion(sReferencia, iSubReferencia, IdAseguradora, IdConclusion, IdRegimen, 2);

            GetSeccion_2();     // RIESGOS

            DesactivarCheckBoxes(grdSeccion_2, false);
            UpdatePanel10.Update();

            GetDocumentos(TxtSubReferencia.Text);

            BtnEditarRiesgos.Visible = true;
            BtnActualizarRiesgos.Visible = false;
            BtnAnularRiesgos.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios, correctamente";
            this.mpeMensaje.Show();
        }

        protected void BtnAnularBienes_Click(object sender, EventArgs e)
        {
            DesactivarCheckBoxes(grdSeccion_4, false);
            UpdatePanel12.Update();

            BtnEditarBienes.Visible = true;
            BtnActualizarBienes.Visible = false;
            BtnAnularBienes.Visible = false;
        }

        protected void BtnEditarBienes_Click(object sender, EventArgs e)
        {
            // Activar CheckBoxes (BIENES)
            DesactivarCheckBoxes(grdSeccion_4, true);
            UpdatePanel12.Update();

            BtnEditarBienes.Visible = false;
            BtnActualizarBienes.Visible = true;
            BtnAnularBienes.Visible = true;
        }

        protected void BtnActualizarBienes_Click(object sender, EventArgs e)
        {
            // Agregar registros con los datos de cada cuaderno a cargar
            string sReferencia = Variables.wRef;
            int iSubReferencia = Variables.wSubRef;
            string IdAseguradora = Variables.wPrefijo_Aseguradora;
            int IdConclusion = Convert.ToInt32(ddlConclusion.SelectedValue);
            int IdRegimen = Convert.ToInt32(ddlTpoAsegurado.SelectedValue);

            // Eliminar registros
            Delete_Seccion_ITM_91(4);

            Obtener_Valores_ChBox(grdSeccion_4, "ChBoxSeccion_4", 4, "ITM_84");

            // Eliminar Secciones (4) BIENES de la tabla ITM_47
            Delete_Seccion_ITM_47(4);

            // Agregar Secciones (4) BIENES de la tabla ITM_47
            Add_tbDetCategoriaSeccion(sReferencia, iSubReferencia, IdAseguradora, IdConclusion, IdRegimen, 4);

            GetSeccion_4();     // BIENES

            DesactivarCheckBoxes(grdSeccion_4, false);
            UpdatePanel12.Update();

            GetCategoriasDaños();
            GetDocumentos(TxtSubReferencia.Text);

            BtnEditarBienes.Visible = true;
            BtnActualizarBienes.Visible = false;
            BtnAnularBienes.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios, correctamente";
            this.mpeMensaje.Show();
        }

        protected void BtnAnularOtros_Click(object sender, EventArgs e)
        {
            DesactivarCheckBoxes(grdSeccion_5, false);
            UpdatePanel8.Update();

            BtnEditarOtros.Visible = true;
            BtnActualizarOtros.Visible = false;
            BtnAnularOtros.Visible = false;
        }

        protected void BtnEditarOtros_Click(object sender, EventArgs e)
        {
            // Activar CheckBoxes (OTROS DETALLES)
            DesactivarCheckBoxes(grdSeccion_5, true);
            UpdatePanel8.Update();

            BtnEditarOtros.Visible = false;
            BtnActualizarOtros.Visible = true;
            BtnAnularOtros.Visible = true;
        }

        protected void BtnActualizarOtros_Click(object sender, EventArgs e)
        {
            // Agregar registros con los datos de cada cuaderno a cargar
            string sReferencia = Variables.wRef;
            int iSubReferencia = Variables.wSubRef;
            string IdAseguradora = Variables.wPrefijo_Aseguradora;
            int IdConclusion = Convert.ToInt32(ddlConclusion.SelectedValue);
            int IdRegimen = Convert.ToInt32(ddlTpoAsegurado.SelectedValue);

            // Eliminar registros
            Delete_Seccion_ITM_91(5);

            Obtener_Valores_ChBox(grdSeccion_5, "ChBoxSeccion_5", 5, "ITM_85");

            // Eliminar Secciones (5) OTROS DETALLES de la tabla ITM_47
            Delete_Seccion_ITM_47(5);

            // Agregar Secciones (5) OTROS DETALLES de la tabla ITM_47
            Add_tbDetCategoriaSeccion(sReferencia, iSubReferencia, IdAseguradora, IdConclusion, IdRegimen, 5);

            GetSeccion_5();     // OTROS DETALLES

            DesactivarCheckBoxes(grdSeccion_5, false);
            UpdatePanel8.Update();

            GetDocumentos(TxtSubReferencia.Text);

            BtnEditarOtros.Visible = true;
            BtnActualizarOtros.Visible = false;
            BtnAnularOtros.Visible = false;

            LblMessage.Text = "Se han aplicado los cambios, correctamente";
            this.mpeMensaje.Show();
        }

        protected void BtnDañosEdif_Cancel_Click(object sender, EventArgs e)
        {

        }

        protected void BtnDañosEdif_Aceptar_Click(object sender, EventArgs e)
        {
            int index = Variables.wRenglon;

            int iIdEdificio = int.Parse(Server.HtmlDecode(GrdEdificio.Rows[index].Cells[0].Text));
            int iIdCategoria = int.Parse(Server.HtmlDecode(GrdEdificio.Rows[index].Cells[1].Text));

            Eliminar_tbEdificio(iIdEdificio, iIdCategoria);

            GetAltaDaños_Edificio();

            inicializar_daños_edificio();

            btnEditarPnl4.Visible = true;
            btnActualizarPnl4.Visible = false;
            BtnAnularPnl4.Visible = false;

            btnEditarPnl4.Enabled = false;
            BtnAgregarPnl4.Enabled = true;
        }

        protected void BtnDañosEdif_Cerrar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnDañosOtros_Aceptar_Click(object sender, EventArgs e)
        {
            int index = Variables.wRenglon;

            int iIdOtros = int.Parse(Server.HtmlDecode(GrdOtrosDaños.Rows[index].Cells[0].Text));
            int iIdCategoria = int.Parse(Server.HtmlDecode(GrdOtrosDaños.Rows[index].Cells[1].Text));

            Eliminar_tbDañosOtros(iIdOtros, iIdCategoria);

            GetAltaDaños_Otros(Convert.ToInt32(iIdCategoria));

            inicializar_daños_otros();

            btnEditarPnl4.Visible = true;
            btnActualizarPnl4.Visible = false;
            BtnAnularPnl4.Visible = false;

            btnEditarPnl4.Enabled = false;
            BtnAgregarPnl4.Enabled = true;
        }

        protected void inicializar_daños_edificio()
        {
            TxtAreaEdif.Text = string.Empty;
            TxtNivelEdif.Text = string.Empty;
            TxtCategoriaEdif.Text = string.Empty;
            TxtElementoEdif.Text = string.Empty;
            TxtTipoEdif.Text = string.Empty;
            TxtMaterialesEdif.Text = string.Empty;

            TxtMedidaEdif_1.Text = string.Empty;
            ddlUnidadEdif_1.SelectedValue = "0";
            TxtMedidaEdif_2.Text = string.Empty;
            ddlUnidadEdif_2.SelectedValue = "0";
            TxtMedidaEdif_3.Text = string.Empty;
            ddlUnidadEdif_3.SelectedValue = "0";

            TxtObsEdif_1.Text = string.Empty;
            TxtObsEdif_2.Text = string.Empty;
        }

        protected void inicializar_daños_otros()
        {
            TxtCategoriaOtros.Text = string.Empty;
            TxtTpoOtros.Text = string.Empty;
            TxtGenOtros.Text = string.Empty;

            TxtDescOtros.Text = string.Empty;

            TxtCantOtros.Text = string.Empty;
            TxtMarcaOtros.Text = string.Empty;
            TxtModOtros.Text = string.Empty;
            TxtNumSerieOtros.Text = string.Empty;

            TxtObsOtros_1.Text = string.Empty;
            TxtObsOtros_2.Text = string.Empty;
        }

        protected void BtnDañosOtros_Cancel_Click(object sender, EventArgs e)
        {

        }

        protected void BtnDañosOtros_Cerrar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnEnviarWhatsApp_Click(object sender, EventArgs e)
        {
            try
            {
                int idAsunto = 2019;                                        // o selecciona desde UI
                                                                            // int idAsunto = int.Parse(txtIdAsunto.Text.Trim());           // o selecciona desde UI
                                                                            // string telefono = txtTelefono.Text.Trim();                   // número destino e.g. 521555555555
                string email = "martin.baltierra@gmail.com";                // correo del asegurado                                         txtEmail.Text.Trim();
                string referencia = TxtSubReferencia.Text.Trim();           // opcional, para incluir en email


                // 1) Generar token y password
                var tm = new TokenManager();
                var (token, passwordTemporal) = tm.GenerarYGuardarToken(idAsunto);

                //// 2) Crear link (solo token en querystring)
                //string baseUrl = System.Configuration.ConfigurationManager.AppSettings["APP_BASE_URL"].TrimEnd('/');
                //string link = $"{baseUrl}/fwLandingAcceso.aspx?token={HttpUtility.UrlEncode(token)}";


                //// 3) Enviar WhatsApp (link)
                //string to = "+5215533316980";
                //string body = link;

                //WhatsAppSender.EnviarWhatsApp(to, body);

                Page.RegisterAsyncTask(new PageAsyncTask(async () =>
                {
                    // 2) Crear link (solo token en querystring)
                    string baseUrl = System.Configuration.ConfigurationManager.AppSettings["APP_BASE_URL"].TrimEnd('/');
                    string urlConToken = $"{baseUrl}/fwLandingAcceso.aspx?token={token}";

                    // 3) Enviar WhatsApp (link)
                    string telefonoAsegurado = "5215533316980";
                    string mensaje = $"Hola, por favor sube la documentación de tu siniestro en el siguiente link: {urlConToken}";

                    string respuesta = await WhatsAppSender.EnviarWhatsAppMeta(telefonoAsegurado, mensaje);

                    LblMessage.Text = "Mensaje enviado: " + respuesta;
                    this.mpeMensaje.Show();

                }));

                // WhatsAppSender.SendMessage(to, body);

                // WhatsAppSender.EnviarWhatsApp(to, body);

                // await WhatsAppSender.EnviarLinkAsync(telefono, link);

                // 4) Enviar email con contraseña temporal
                EmailSender.EnviarContrasena(email, passwordTemporal, referencia);

                // 5) Mostrar mensaje al admin
                //lblStatus.Text = "Link y contraseña enviados correctamente.";

                LblMessage.Text = "Link y contraseña enviados correctamente.";
                this.mpeMensaje.Show();
            }
            catch (Exception ex)
            {
                // lblStatus.Text = "Error al enviar: " + ex.Message;
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

    }

}