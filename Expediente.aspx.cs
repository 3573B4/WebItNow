using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;

using System.Configuration;
using System.Web.Services;
using System.Web.Script.Services;
using AjaxControlToolkit;
using Azure.Storage.Files.Shares;
using System.IO;
using Azure;

namespace WebItNow
{
    public partial class Expediente : System.Web.UI.Page
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
                inhabilitar(this.Controls);
                TxtReferencia.Enabled = true;

                BtnActualizar.Enabled = false;

                //BtnEnviarPoliza.Enabled = false;
                //BtnEnviarCorreo.Enabled = false;

                // Carga de Catalogos Aseguradora, Asegurado, Grupo
                getAseguradora();
                getDivision();
                getEvento();
                getMoneda();

                ddlAtencion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                getProcesos();
                
                ddlSubProceso.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlCobertura.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlRiesgo.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                Carga_Asegurado();
                Carga_SegundoAsegurado();
                Carga_Grupo();
                Carga_Estado();

                ddlDelegacion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                Carga_Estado_Afectado();
                ddlDelegacionAfectada.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                Carga_Cliente();
                Carga_Oficina();
                Carga_Local();
                Carga_Ajustador();
                Carga_Corredor();
                Carga_Agente();

                ddlAjustador.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlCorredor.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlAgente.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                TxtReferencia.Enabled = true;
            }

        }

        public void getAseguradora()
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdAseguradora, Nombre " +
                                    "FROM ITM_19 WHERE IdStatus != 0 ";

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlAseguradora.DataSource = dt;

                ddlAseguradora.DataValueField = "IdAseguradora";
                ddlAseguradora.DataTextField = "Nombre";

                ddlAseguradora.DataBind();
                ddlAseguradora.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void getDivision()
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdDivision, Descripcion " +
                                    "FROM ITM_21 ";

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlDivision.DataSource = dt;

                ddlDivision.DataValueField = "IdDivision";
                ddlDivision.DataTextField = "Descripcion";

                ddlDivision.DataBind();
                ddlDivision.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void getEvento()
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdEvento, Descripcion " +
                                    "FROM ITM_22 WHERE IdStatus != 0 ";

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlEventCatastrofico.DataSource = dt;

                ddlEventCatastrofico.DataValueField = "IdEvento";
                ddlEventCatastrofico.DataTextField = "Descripcion";

                ddlEventCatastrofico.DataBind();
                ddlEventCatastrofico.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void getMoneda()
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdMoneda, CveMoneda " +
                                    "FROM ITM_28 ";

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlMoneda.DataSource = dt;

                ddlMoneda.DataValueField = "IdMoneda";
                ddlMoneda.DataTextField = "CveMoneda";

                ddlMoneda.DataBind();
                ddlMoneda.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void getAsistente(string idAseguradora, int idDivision)
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdAsistente, Nombre " +
                                    " FROM ITM_29 WHERE IdStatus != 0 " +
                                    " AND IdAseguradora = '" + idAseguradora + "' " +
                                    " AND IdDivision = " + idDivision + " ";

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlAtencion.DataSource = dt;

                ddlAtencion.DataValueField = "IdAsistente";
                ddlAtencion.DataTextField = "Nombre";

                ddlAtencion.DataBind();
                ddlAtencion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();

                conectar.Cerrar();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void getProcesos()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT IdProceso, Nombre " +
                                        " FROM ITM_16 " +
                                        " WHERE IdStatus = 1 ";
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlProceso.DataSource = dt;

                ddlProceso.DataValueField = "IdProceso";
                ddlProceso.DataTextField = "Nombre";

                ddlProceso.DataBind();
                ddlProceso.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();

                Conecta.Cerrar();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        protected void getSubProcesos(int iProceso)
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdSubProceso, Descripcion " +
                                    " FROM ITM_14 " +
                                    " WHERE IdProceso = " + iProceso;

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlSubProceso.DataSource = dt;

                ddlSubProceso.DataValueField = "IdSubProceso";
                ddlSubProceso.DataTextField = "Descripcion";

                ddlSubProceso.DataBind();
                ddlSubProceso.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();

                conectar.Cerrar();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void getCobertura(int iProceso, int iSubProceso)
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdCobertura, Descripcion " +
                                    " FROM ITM_13 " +
                                    " WHERE IdProceso = " + iProceso +
                                    " AND IdSubproceso = " + iSubProceso;

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlCobertura.DataSource = dt;

                ddlCobertura.DataValueField = "IdCobertura";
                ddlCobertura.DataTextField = "Descripcion";

                ddlCobertura.DataBind();
                ddlCobertura.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                cmd.Dispose();
                da.Dispose();
                dt.Dispose();

                conectar.Cerrar();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void Carga_Asegurado()
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();
            string sqlQuery = "SELECT IdAsegurado, Nombre " +
                                    " FROM ITM_20 " +
                                    " WHERE IdStatus = 1 ";

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlAsegurado.DataSource = dt;

            ddlAsegurado.DataValueField = "IdAsegurado";
            ddlAsegurado.DataTextField = "Nombre";

            ddlAsegurado.DataBind();
            ddlAsegurado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

        }

        protected void Carga_SegundoAsegurado()
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();
            string sqlQuery = "SELECT IdAsegurado, Nombre " +
                                    " FROM ITM_20 " +
                                    " WHERE IdStatus = 1 ";

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlSegundoAsegurado.DataSource = dt;

            ddlSegundoAsegurado.DataValueField = "IdAsegurado";
            ddlSegundoAsegurado.DataTextField = "Nombre";

            ddlSegundoAsegurado.DataBind();
            ddlSegundoAsegurado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
        }

        protected void Carga_Grupo()
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();
            string sqlQuery = "SELECT IdGrupo, Descripcion " +
                                    " FROM ITM_24 " +
                                    " WHERE IdStatus = 1 ";

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlGrupo.DataSource = dt;

            ddlGrupo.DataValueField = "IdGrupo";
            ddlGrupo.DataTextField = "Descripcion";

            ddlGrupo.DataBind();
            ddlGrupo.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
        }

        protected void Carga_Estado()
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();
            string sqlQuery = "SELECT IdEstado, Nombre_Estado " +
                                    " FROM ITM_26 " +
                                    " WHERE IdStatus = 1 ";

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlEstado.DataSource = dt;

            ddlEstado.DataValueField = "IdEstado";
            ddlEstado.DataTextField = "Nombre_Estado";

            ddlEstado.DataBind();
            ddlEstado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

        }

        protected void Carga_Delegacion(int iEstado)
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();
            string sqlQuery = "SELECT IdMunicipio, Nombre_Municipio " +
                                    " FROM ITM_27 " +
                                    " WHERE IdStatus = 1 " +
                                    "   AND IdEstado = " + iEstado;

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlDelegacion.DataSource = dt;

            ddlDelegacion.DataValueField = "IdMunicipio";
            ddlDelegacion.DataTextField = "Nombre_Municipio";

            ddlDelegacion.DataBind();
            ddlDelegacion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

        }

        protected void Carga_Estado_Afectado()
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();
            string sqlQuery = "SELECT IdEstado, Nombre_Estado " +
                                    " FROM ITM_26 " +
                                    " WHERE IdStatus = 1 ";

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlEstadoAfectado.DataSource = dt;

            ddlEstadoAfectado.DataValueField = "IdEstado";
            ddlEstadoAfectado.DataTextField = "Nombre_Estado";

            ddlEstadoAfectado.DataBind();
            ddlEstadoAfectado.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

        }

        protected void Carga_Delegacion_Afectado(int iEstado)
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();
            string sqlQuery = "SELECT IdMunicipio, Nombre_Municipio " +
                                    " FROM ITM_27 " +
                                    " WHERE IdStatus = 1 " +
                                    "   AND IdEstado = " + iEstado;

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlDelegacionAfectada.DataSource = dt;

            ddlDelegacionAfectada.DataValueField = "IdMunicipio";
            ddlDelegacionAfectada.DataTextField = "Nombre_Municipio";

            ddlDelegacionAfectada.DataBind();
            ddlDelegacionAfectada.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

        }

        protected void Carga_Cliente()
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();
            string sqlQuery = "SELECT IdCliente, Cliente " +
                                    " FROM ITM_05 " +
                                    " WHERE IdStatus = 1 ";

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlCliente.DataSource = dt;

            ddlCliente.DataValueField = "IdCliente";
            ddlCliente.DataTextField = "Cliente";

            ddlCliente.DataBind();
            ddlCliente.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
        }

        protected void Carga_Oficina()
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();
            string sqlQuery = "SELECT IdOficina, Oficina " +
                                    " FROM ITM_30 " +
                                    " WHERE IdStatus = 1 ";

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlOficina.DataSource = dt;

            ddlOficina.DataValueField = "IdOficina";
            ddlOficina.DataTextField = "Oficina";

            ddlOficina.DataBind();
            ddlOficina.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
        }

        protected void Carga_Local()
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();
            string sqlQuery = "SELECT IdLocal, Descripcion " +
                                    " FROM ITM_31 " +
                                    " WHERE IdStatus = 1 ";

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlLocal.DataSource = dt;

            ddlLocal.DataValueField = "IdLocal";
            ddlLocal.DataTextField = "Descripcion";

            ddlLocal.DataBind();
            ddlLocal.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
        }

        protected void Carga_Ajustador()
        {

        }

        protected void Carga_Corredor()
        {

        }

        protected void Carga_Agente()
        {

        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idAseguradora = ddlAseguradora.SelectedValue;
            int idDivision = Convert.ToInt32(ddlDivision.SelectedValue);

            getAsistente(idAseguradora, idDivision);
        }

        protected void ddlProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iProceso = Convert.ToInt32(ddlProceso.SelectedValue);
            getSubProcesos(iProceso);
            ddlGetRiesgo(iProceso);
        }

        protected void ddlGetRiesgo(int iProceso)
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdRiesgo, Descripcion, IdProceso  " +
                                    " FROM ITM_23 " +
                                    " WHERE IdProceso = " + iProceso;

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlRiesgo.DataSource = dt;

                ddlRiesgo.DataValueField = "IdRiesgo";
                ddlRiesgo.DataTextField = "Descripcion";

                ddlRiesgo.DataBind();
                ddlRiesgo.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();

                conectar.Cerrar();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ddlSubProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iProceso = Convert.ToInt32(ddlProceso.SelectedValue);
            int iSubProceso = Convert.ToInt32(ddlSubProceso.SelectedValue);
            getCobertura(iProceso, iSubProceso);
        }

        protected void ddlAsegurado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlSegundoAsegurado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iEstado = Convert.ToInt32(ddlEstado.SelectedValue);
            Carga_Delegacion(iEstado);
        }

        protected void ddlDelegacion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlEstadoAfectado_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iEstado = Convert.ToInt32(ddlEstadoAfectado.SelectedValue);
            Carga_Delegacion_Afectado(iEstado);
        }

        protected void ddlDelegacionAfectada_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlLocal_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlOficina_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlAjustador_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlCorredor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlAgente_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon();

         // Response.Redirect("Menu.aspx", true);
            Response.Redirect("Mnu_Dinamico.aspx", true);
        }

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            string sReferencia = TxtReferencia.Text;

            // Busqueda Referencia ITM_02
            int iReferencia = Val_Referencia(sReferencia);

            if (iReferencia == 0)
            {
                int iExiste = Select_Siniestro(sReferencia);

                if (iExiste == 0)
                {
                    habilitar(this.Controls);
                    Variables.wExiste = true;
                }
                else
                {
                    Limpia(this.Controls);
                    InicializarGrid();

                    habilitar(this.Controls);

                    TxtReferencia.Text = sReferencia;
                    Variables.wExiste = false;
                }

                BtnDocumentos.Enabled = true;
                BtnActualizar.Enabled = true;
                BtnNotas.Enabled = true;
                BtnFotos.Enabled = true;

                //BtnEnviarPoliza.Enabled = true;
                //BtnEnviarCorreo.Enabled = true;
            }
            else
            {
                BtnDocumentos.Enabled = false;
                BtnActualizar.Enabled = false;
                BtnNotas.Enabled = false;
                BtnFotos.Enabled = false;

                //BtnEnviarPoliza.Enabled = false;
                //BtnEnviarCorreo.Enabled = false;

                Variables.wExiste = false;

                Limpia(this.Controls);
                InicializarGrid();

                inhabilitar(this.Controls);

                TxtReferencia.Enabled = true;

                LblMessage.Text = "Referencia proporcionada no es correcta, favor de ingresarla nuevamente";
                this.mpeMensaje.Show();
            }
        }

        protected void BtnActualizar_Click(object sender, EventArgs e)
        {

            int iOk = Campos_Obligatorios();

            if (iOk != 0)
            {
                return;
            }

            string sReferencia = TxtReferencia.Text;
            string sTpoDocumento = "10";

            try
            {

                if (Variables.wExiste)
                {
                    // Actualiza Asegurado ITM_32
                    Update_DetalleAseguradora(sReferencia, ddlAseguradora.SelectedValue, Convert.ToInt32(ddlDivision.SelectedValue), TxtAnalista.Text, Convert.ToInt32(ddlAtencion.SelectedValue), Convert.ToInt32(ddlEventCatastrofico.SelectedValue),
                        Convert.ToInt32(ddlProceso.SelectedValue), Convert.ToInt32(ddlSubProceso.SelectedValue), Convert.ToInt32(ddlCobertura.SelectedValue), Convert.ToInt32(ddlRiesgo.SelectedValue),
                        TxtNumSiniestro.Text, TxtNumReporte.Text, TxtFechaAseguradora.Text, TxtFechaSiniestro.Text, TxtRefAsegurado.Text, TxtReporto.Text, TxtFechaAsignacion.Text,
                        TxtHorAsignacion.Text, TxtNumPoliza.Text, TxtIniciaVigencia.Text, TxtTerminaVigencia.Text, Convert.ToInt32(TxtEstimacion.Text), Convert.ToInt32(ddlMoneda.SelectedValue),
                        TxtCausaSiniestro.Text);

                    // Actualiza Asegurado ITM_33
                    Update_DetalleAsegurado(sReferencia);

                    // Actualiza Asegurado ITM_34
                    Update_DetalleDireccion(sReferencia);

                    // Actualiza Asegurado ITM_35
                    Update_DetalleDireccion_Afectada(sReferencia);

                    // Actualiza Asegurado ITM_36
                    Update_DetalleAjustador(sReferencia);


                    Update_tbDetalleArchivos(sReferencia, sTpoDocumento);
                }
                else
                {
                    // Agregar Asegurado ITM_32
                    Add_tbDetalleAseguradora(sReferencia, ddlAseguradora.SelectedValue, ddlDivision.SelectedValue, TxtAnalista.Text, Convert.ToInt32(ddlAtencion.SelectedValue), Convert.ToInt32(ddlEventCatastrofico.SelectedValue),
                        Convert.ToInt32(ddlProceso.SelectedValue), Convert.ToInt32(ddlSubProceso.SelectedValue), Convert.ToInt32(ddlCobertura.SelectedValue), Convert.ToInt32(ddlRiesgo.SelectedValue),
                        TxtNumSiniestro.Text, TxtNumReporte.Text, TxtFechaAseguradora.Text, TxtFechaSiniestro.Text, TxtRefAsegurado.Text, TxtReporto.Text, TxtFechaAsignacion.Text,
                        TxtHorAsignacion.Text, TxtNumPoliza.Text, TxtIniciaVigencia.Text, TxtTerminaVigencia.Text, Convert.ToInt32(TxtEstimacion.Text), Convert.ToInt32(ddlMoneda.SelectedValue),
                        TxtCausaSiniestro.Text);

                    // Agregar Asegurado ITM_33
                    int iIdAsegurado = Convert.ToInt32(ddlAsegurado.SelectedValue);
                    int iIdAsegurado_Segundo = Convert.ToInt32(ddlSegundoAsegurado.SelectedValue);
                    int iIdGrupo = Convert.ToInt32(ddlGrupo.SelectedValue);

                    Add_tbDetalleAsegurado(sReferencia, iIdAsegurado, iIdAsegurado_Segundo, iIdGrupo, TxtTelefono_1.Text, TxtTelefono_2.Text, TxtCelular.Text);

                    // Agregar Direccion ITM_34
                    int iIdEstado = Convert.ToInt32(ddlEstado.SelectedValue);
                    int iIdMunicipio = Convert.ToInt32(ddlDelegacion.SelectedValue);

                    Add_tbDetalleDireccion(sReferencia, TxtCodigoPostal.Text, iIdEstado, iIdMunicipio, TxtColonia.Text, TxtCalle.Text);

                    // Agregar Direccion Afectada ITM_35
                    int iIdEstado_Afectado = Convert.ToInt32(ddlEstadoAfectado.SelectedValue);
                    int iIdMunicipio_Afectado = Convert.ToInt32(ddlDelegacionAfectada.SelectedValue);

                    Add_tbDetalleDireccion_Afectada(sReferencia, TxtCodigoPostalAfectado.Text, iIdEstado_Afectado, iIdMunicipio_Afectado, TxtColonia_Afectada.Text, TxtCalle_Afectada.Text,
                                                    TxtObservaciones.Text, TxtEntrevistar.Text, TxtPuesto_1.Text, TxtTelefono_1.Text, TxtEmail_1.Text, TxtDocumentara.Text,
                                                    TxtPuesto_2.Text, TxtTelefono_2.Text, TxtEmail_2.Text, TxtLegal.Text);

                    // Agregar Ajustador ITM_36
                    int iIdLocal = Convert.ToInt32(ddlLocal.SelectedValue);
                    int iIdCliente = Convert.ToInt32(ddlCliente.SelectedValue);
                    int iIdOficina = Convert.ToInt32(ddlOficina.SelectedValue);
                    int iIdAjustador = Convert.ToInt32(ddlAjustador.SelectedValue);
                    int iIdCorredor = Convert.ToInt32(ddlCorredor.SelectedValue);
                    int iIdAgente = Convert.ToInt32(ddlAgente.SelectedValue);

                    Add_tbDetalleAjustador(sReferencia, iIdLocal, iIdCliente, iIdOficina, iIdAjustador, iIdCorredor, iIdAgente, TxtObservaciones_2.Text, TxtAntecedentes.Text, TxtCausa.Text, TxtBienes.Text);

                    int Envio_Ok = Add_tbDetalleArchivos(sReferencia);

                    if (Envio_Ok == 0)
                    {
                        LblMessage.Text = "Su registro fue creado exitosamente";
                        this.mpeMensaje.Show();
                    }

                }

                Limpia(this.Controls);
                InicializarGrid();

                inhabilitar(this.Controls);

                BtnDocumentos.Enabled = false;
                BtnActualizar.Enabled = false;
                BtnNotas.Enabled = false;
                BtnFotos.Enabled = false;

                //BtnEnviarPoliza.Enabled = false;
                //BtnEnviarCorreo.Enabled = false;

                TxtReferencia.Enabled = true;

            }
            catch(Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        public int Add_tbDetalleAseguradora(string pReferencia, string sIdAseguradora, string sIdDivision, string sAnalista, int iIdAtencion_A, int iEvntCatastrofico,
                                                int iProceso, int iSubProceso, int iIdCovertura, int iIdRiesgo, string NumSiniestro, string NumReporte, string sFechaReporteAseguradora, string FechaSiniestro,
                                                string sRefAsegurado, string QuienReporto, string FechAsignacion, string HoraAsignacion, string NumPoliza, string IniciaVigencia, string TerminaVigencia,
                                                int sEstimacion, int iMoneda, string sCausaSiniestro)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = " INSERT INTO ITM_32(Referencia, IdAseguradora, Analista, IdEvento, " +
                                                      " IdProceso, IdCobertura, IdDivision, IdAtencion_A, IdSubproceso, " +
                                                      " IdRiesgo, Num_Siniestro, Num_Reporte, Fec_Reporte_Asegurado, Fec_Siniestro, " +
                                                      " Referencia_Asegurado, Reporto, Fec_Asignacion, Hora_Asignacion, " +
                                                      " Num_Poliza, Fec_Ini_Vigencia, Fec_Fin_Vigencia, Estimacion, IdMoneda, Causa_Siniestro ) " +
                                            //falta la variable de la Referencia
                                            " VALUES('" + pReferencia + "', '" + sIdAseguradora + "', '" + sAnalista + "', " + iEvntCatastrofico + ", " +
                                                     iProceso + ", " + iIdCovertura + ", " + sIdDivision + ", " + iIdAtencion_A + ", " + iSubProceso + ", " +
                                                     iIdRiesgo + " , '" + NumSiniestro + "', '" + NumReporte + "', CONVERT(smalldatetime, '" + sFechaReporteAseguradora + "', 103), CONVERT(smalldatetime, '" + FechaSiniestro + "', 103), " +
                                                     " '" + sRefAsegurado + "', '" + QuienReporto + "', CONVERT(smalldatetime, '" + FechAsignacion + "', 103), '" + HoraAsignacion + "', " +
                                                     " '" + NumPoliza + "', CONVERT(smalldatetime, '" + IniciaVigencia + "', 103), CONVERT(smalldatetime, '" + TerminaVigencia + "', 103), " + sEstimacion + ", " + iMoneda + ", '" + sCausaSiniestro + "') ";

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                cmd.ExecuteReader();
                cmd.Dispose();
                Conecta.Cerrar();

                return 0;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
            finally
            {

            }
            return -1;

        }

        public int Add_tbDetalleAsegurado(string pReferencia, int pIdAsegurado, int pIdAsegurado_Segundo, int pIdGrupo, string pTelefono_1, string pTelefono_2, string pCelular)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Insert en la tabla Estado de Documento
                SqlCommand cmd1 = new SqlCommand("Insert into ITM_33 (Referencia, IdAsegurado, IdAsegurado_Segundo, IdGrupo, Telefono_1, Telefono_2, Celular) " +
                                    "Values ('" + pReferencia + "', " + pIdAsegurado + ", " + pIdAsegurado_Segundo + ", " + pIdGrupo + ", " +
                                    "" + pTelefono_1 + ", " + pTelefono_2 + ", " + pCelular + ")", Conecta.ConectarBD);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

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

        public int Add_tbDetalleDireccion(string pReferencia, string pCodigoPostal, int pIdEstado, int pIdMunicipio, string pColonia, string pCalle)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Insert en la tabla Estado de Documento
                SqlCommand cmd1 = new SqlCommand("Insert into ITM_34 (Referencia, Codigo_Postal, IdEstado, IdMunicipio, Colonia, Calle) " +
                                    "Values ('" + pReferencia + "', '" + pCodigoPostal + "', " + pIdEstado + ", " + pIdMunicipio + ", " +
                                    "'" + pColonia + "', '" + pCalle + "')", Conecta.ConectarBD);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

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

        public int Add_tbDetalleDireccion_Afectada(string pReferencia, string pCodigoPostal, int pIdEstado, int pIdMunicipio, string pColonia, string pCalle,
                                                   string pObservaciones, string pEntrevistar, string pPuesto, string pTelefono, string pEmail, string pDocumentador,
                                                   string pPuesto_2, string pTelefono_2, string pEmail_2, string pRep_Legal)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Insert en la tabla Estado de Documento
                SqlCommand cmd1 = new SqlCommand("Insert into ITM_35 (Referencia, Codigo_Postal, IdEstado, IdMunicipio, Colonia, Calle, " +
                                    "Observaciones, Entrevistar_A, Puesto_1, Telefono_1, Email_1, Documentador, Puesto_2, Telefono_2, Email_2, Rep_Legal) " +
                                    "Values ('" + pReferencia + "', '" + pCodigoPostal + "', " + pIdEstado + ", " + pIdMunicipio + ", " +
                                    "'" + pColonia + "', '" + pCalle + "', '" + pObservaciones + "', '" + pEntrevistar + "', " +
                                    "'" + pPuesto + "', '" + pTelefono + "', '" + pEmail + "', '" + pDocumentador + "', " +
                                    "'" + pPuesto_2 + "', '" + pTelefono_2 + "', '" + pEmail_2 + "', '" + pRep_Legal + "')", Conecta.ConectarBD);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

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

        public int Add_tbDetalleAjustador(string pReferencia, int pIdLocal, int pIdCliente, int pIdOficina, int pIdAjustador, int pIdCorredor, int pIdAgente,
                                           string pObservacion, string pAntecedentes, string pCausa, string pBienes)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Insert en la tabla Estado de Documento
                SqlCommand cmd1 = new SqlCommand("Insert into ITM_36 (Referencia, IdLocal, IdCliente, IdOficina, IdAjustador, IdCorredor, IdAgente, " +
                                                 "Observaciones, Antecedentes, Causa, Bienes) " +
                                    "Values ('" + pReferencia + "', " + pIdLocal + ", " + pIdCliente + ", " + pIdOficina + ", " +
                                    "" + pIdAjustador + ", " + pIdCorredor + ", " + pIdAgente + ", " +
                                    "'" + pObservacion + "', '" + pAntecedentes + "', '" + pCausa + "', '" + pBienes + "')", Conecta.ConectarBD);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

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

        public int Add_tbDetalleArchivos(string pReferencia)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string strQuery = "Insert into ITM_37 (Referencia, IdTpoDocumento, Descripcion, Url_Imagen, Url_Path_Final, Nom_Imagen, Nom_Imagen_Final, Fec_Entrega, Entregado)" +
                                  "Values ('" + pReferencia + "', '10', 'Correo de Asignación', Null, Null, Null, Null, Null, 0)" + "\n \n";

                strQuery += strQuery = "Insert into ITM_37 (Referencia, IdTpoDocumento, Descripcion, Url_Imagen, Url_Path_Final, Nom_Imagen, Nom_Imagen_Final, Fec_Entrega, Entregado) " +
                                      "Values ('" + pReferencia + "', '14', 'Póliza', Null, Null, Null, Null, Null, 0)";

                // Insert en la tabla Estado de Documento
                SqlCommand cmd1 = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

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

        public int Val_Referencia(string pReferencia)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                NewMethod(Conecta);

                string strQuery = "SELECT UsReferencia From ITM_02 as a" +
                                  " WHERE UsReferencia = '" + pReferencia + "'";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    return 0;
                }
                cmd.Dispose();
                Conecta.Cerrar();
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

        public int Select_Siniestro(string pReferencia)
        {

            try
            {
                ConexionBD Conecta = new ConexionBD();
                NewMethod(Conecta);

                string strQuery = "SELECT b.IdAsegurado, b.IdAsegurado_Segundo, b.IdGrupo, b.Telefono_1, b.Telefono_2, b.Celular, " +
                                         "c.Codigo_Postal, c.IdEstado, c.IdMunicipio, c.Colonia, c.Calle, " +
                                         "d.Codigo_Postal, d.IdEstado, d.IdMunicipio, d.Colonia, d.Calle, d.Observaciones, d.Entrevistar_A, " +
                                         "d.Puesto_1, d.Telefono_1, d.Email_1, d.Documentador, d.Puesto_2, d.Telefono_2, d.Email_2, d.Rep_Legal, " +
                                         "e.IdLocal, e.IdCliente, e.IdOficina, e.IdAjustador, e.IdCorredor, e.IdAgente, e.Observaciones, e.Antecedentes, e.Causa, e.Bienes," +
                                         "a.IdAseguradora, a.Analista, a.IdEvento, a.IdProceso, a.IdCobertura, a.IdDivision, a.IdAtencion_A, a.IdSubproceso," +
                                         "a.IdRiesgo, a.Num_Siniestro, a.Num_Reporte, CONVERT(VARCHAR, a.Fec_Reporte_Asegurado, 103) as Fec_Reporte_Asegurado, CONVERT(VARCHAR, a.Fec_Siniestro, 103) as Fec_Siniestro, a.Referencia_Asegurado, a.Reporto, " +
                                         "CONVERT(VARCHAR, a.Fec_Asignacion, 103) as Fec_Asignacion, a.Hora_Asignacion, a.Num_Poliza, CONVERT(VARCHAR, a.Fec_Ini_Vigencia, 103) as Fec_Ini_Vigencia, CONVERT(VARCHAR, a.Fec_Fin_Vigencia, 103) as Fec_Fin_Vigencia, a.Estimacion, " +
                                         "a.IdMoneda, a.Causa_Siniestro, t1.UsReferencia" +
                                  "  FROM ITM_02 as t1, ITM_32 as a, ITM_33 as b, ITM_34 as c, ITM_35 as d, ITM_36 as e " +
                                  " WHERE t1.UsReferencia = '" + pReferencia + "'" +
                                  "   AND t1.UsReferencia = a.Referencia " +
                                  "   AND t1.UsReferencia = b.Referencia" +
                                  "   AND t1.UsReferencia = c.Referencia" +
                                  "   AND t1.UsReferencia = d.Referencia" +
                                  "   AND t1.UsReferencia = e.Referencia";

                SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    // Asegurado
                    ddlAsegurado.SelectedValue = Convert.ToString(row[0]);
                    TxtBusqNombre.Text = ddlAsegurado.SelectedItem.Text;

                    ddlSegundoAsegurado.SelectedValue = Convert.ToString(row[1]);
                    ddlGrupo.SelectedValue = Convert.ToString(row[2]);
                    TxtTelefono_1.Text = Convert.ToString(row[3]);
                    TxtTelefono_2.Text = Convert.ToString(row[4]);
                    TxtCelular.Text = Convert.ToString(row[5]);

                    // Dirección
                    TxtCodigoPostal.Text = Convert.ToString(row[6]);
                    ddlEstado.SelectedValue = Convert.ToString(row[7]);

                    Carga_Delegacion(Convert.ToInt32(row[7]));
                    ddlDelegacion.SelectedValue = Convert.ToString(row[8]);

                    TxtColonia.Text = Convert.ToString(row[9]);
                    TxtCalle.Text = Convert.ToString(row[10]);

                    // Dirección Afectada
                    TxtCodigoPostalAfectado.Text = Convert.ToString(row[11]);
                    ddlEstadoAfectado.SelectedValue = Convert.ToString(row[12]);

                    Carga_Delegacion_Afectado(Convert.ToInt32(row[12]));
                    ddlDelegacionAfectada.SelectedValue = Convert.ToString(row[13]);

                    TxtColonia_Afectada.Text = Convert.ToString(row[14]);
                    TxtCalle_Afectada.Text = Convert.ToString(row[15]);
                    TxtObservaciones.Text = Convert.ToString(row[16]);
                    TxtEntrevistar.Text = Convert.ToString(row[17]);
                    TxtPuesto_1.Text = Convert.ToString(row[18]);
                    TxtTel_1.Text = Convert.ToString(row[19]);
                    TxtEmail_1.Text = Convert.ToString(row[20]);
                    TxtDocumentara.Text = Convert.ToString(row[21]);
                    TxtPuesto_2.Text = Convert.ToString(row[22]);
                    TxtTel_2.Text = Convert.ToString(row[23]);
                    TxtEmail_2.Text = Convert.ToString(row[24]);
                    TxtLegal.Text = Convert.ToString(row[25]);

                    // Ajustador
                    ddlLocal.SelectedValue = Convert.ToString(row[26]);
                    ddlCliente.SelectedValue = Convert.ToString(row[27]);
                    ddlOficina.SelectedValue = Convert.ToString(row[28]);
                    ddlAjustador.SelectedValue = Convert.ToString(row[29]);
                    ddlCorredor.SelectedValue = Convert.ToString(row[30]);
                    ddlAgente.SelectedValue = Convert.ToString(row[31]);
                    TxtObservaciones_2.Text = Convert.ToString(row[32]);
                    TxtAntecedentes.Text = Convert.ToString(row[33]);
                    TxtCausa.Text = Convert.ToString(row[34]);
                    TxtBienes.Text = Convert.ToString(row[35]);

                    // Aseguradora
                    ddlAseguradora.SelectedValue = Convert.ToString(row[36]);
                    TxtAnalista.Text = Convert.ToString(row[37]);
                    ddlEventCatastrofico.SelectedValue = Convert.ToString(row[38]);

                    ddlProceso.SelectedValue = Convert.ToString(row[39]);

                    getCobertura(Convert.ToInt32(row[39]), Convert.ToInt32(row[43]));
                    ddlCobertura.SelectedValue = Convert.ToString(row[40]);
                    
                    ddlDivision.SelectedValue = Convert.ToString(row[41]);

                    getAsistente(Convert.ToString(row[36]), Convert.ToInt32(row[41]));
                    ddlAtencion.SelectedValue = Convert.ToString(row[42]);

                    getSubProcesos(Convert.ToInt32(row[39]));
                    ddlSubProceso.SelectedValue = Convert.ToString(row[43]);

                    ddlGetRiesgo(Convert.ToInt32(row[39]));
                    ddlRiesgo.SelectedValue = Convert.ToString(row[44]);
                    
                    TxtNumSiniestro.Text = Convert.ToString(row[45]);
                    TxtNumReporte.Text = Convert.ToString(row[46]);
                    TxtFechaAseguradora.Text = Convert.ToString(row[47]);
                    TxtFechaSiniestro.Text = Convert.ToString(row[48]);
                    TxtRefAsegurado.Text = Convert.ToString(row[49]);
                    TxtReporto.Text = Convert.ToString(row[50]);
                    TxtFechaAsignacion.Text = Convert.ToString(row[51]);
                    TxtHorAsignacion.Text = Convert.ToString(row[52]);
                    TxtNumPoliza.Text = Convert.ToString(row[53]);
                    TxtIniciaVigencia.Text = Convert.ToString(row[54]);
                    TxtTerminaVigencia.Text = Convert.ToString(row[55]);
                    TxtEstimacion.Text = Convert.ToString(row[56]);
                    ddlMoneda.SelectedValue = Convert.ToString(row[57]);
                    TxtCausaSiniestro.Text = Convert.ToString(row[58]);

                    TxtReferencia.Text = Convert.ToString(row[59]);

                    return 0;
                }   

                cmd.Dispose();
                Conecta.Cerrar();

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


        protected void Update_DetalleAseguradora(string pReferencia, string sIdAseguradora, int sIdDivision, string sAnalista, int iIdAtencion_A, int iEvntCatastrofico,
                                                int iProceso, int iSubProceso, int iIdCovertura, int iIdRiesgo, string NumSiniestro, string NumReporte, string sFechaReporteAseguradora, string FechaSiniestro,
                                                string sRefAsegurado, string QuienReporto, string FechAsignacion, string HoraAsignacion, string NumPoliza, string IniciaVigencia, string TerminaVigencia,
                                                int sEstimacion, int iMoneda, string sCausaSiniestro)
        {
            try
            {

                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = " UPDATE ITM_32 SET IdAseguradora =  '" + sIdAseguradora + "', Analista = '" + sAnalista + "', IdEvento = " + iEvntCatastrofico + ", " +
                                                    " IdProceso = " + iProceso + ", IdCobertura =  " + iIdCovertura + ", IdDivision = " + sIdDivision + ", IdAtencion_A = " + iIdAtencion_A + ", IdSubproceso = " + iSubProceso + ", " +
                                                    " IdRiesgo = " + iIdRiesgo + ", Num_Siniestro = '" + NumSiniestro + "', Num_Reporte = '" + NumReporte + "', Fec_Reporte_Asegurado =  CONVERT(smalldatetime, '" + sFechaReporteAseguradora + "', 103), Fec_Siniestro = CONVERT(smalldatetime, '" + FechaSiniestro + "', 103), " +
                                                    " Referencia_Asegurado = '" + sRefAsegurado + "', Reporto = '" + QuienReporto + "', Fec_Asignacion = CONVERT(smalldatetime, '" + FechAsignacion + "', 103), Hora_Asignacion = '" + HoraAsignacion + "', " +
                                                    " Num_Poliza = '" + NumPoliza + "', Fec_Ini_Vigencia = CONVERT(smalldatetime, '" + IniciaVigencia + "', 103), Fec_Fin_Vigencia = CONVERT(smalldatetime, '" + TerminaVigencia + "', 103), Estimacion = " + sEstimacion + ", " +
                                                    " IdMoneda = " + iMoneda + ", Causa_Siniestro = '" + sCausaSiniestro + "' " +
                                                    " WHERE Referencia LIKE '%' + '" +  pReferencia + "' + '%' ";

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                cmd.ExecuteReader();
                cmd.Dispose();
                Conecta.Cerrar();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }


        public void Update_DetalleAsegurado(string pReferencia)
        {
            // Agregar Asegurado ITM_33
            int iIdAsegurado = Convert.ToInt32(ddlAsegurado.SelectedValue);
            int iIdAsegurado_Segundo = Convert.ToInt32(ddlSegundoAsegurado.SelectedValue);
            int iIdGrupo = Convert.ToInt32(ddlGrupo.SelectedValue);

            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string strQuery = "Update ITM_33 " +
                                    "Set IdAsegurado = " + iIdAsegurado + ", " +
                                    "IdAsegurado_Segundo = " + iIdAsegurado_Segundo + ", " +
                                    "IdGrupo = " + iIdGrupo + ", " +
                                    "Telefono_1 = '" + TxtTelefono_1.Text + "', " +
                                    "Telefono_2 = '" + TxtTelefono_2.Text + "', " +
                                    "Celular = '" + TxtCelular.Text + "' " +
                                 "Where Referencia = '" + pReferencia + "'";

                // Insert en la tabla Estado de Documento
                SqlCommand cmd1 = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        public void Update_DetalleDireccion(string pReferencia)
        {
            // Agregar Asegurado ITM_34
            int iIdEstado = Convert.ToInt32(ddlEstado.SelectedValue);
            int iIdDelegacion = Convert.ToInt32(ddlDelegacion.SelectedValue);

            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string strQuery = "Update ITM_34 " +
                                    "Set Codigo_Postal = '" + TxtCodigoPostal.Text + "', " +
                                    "IdEstado = " + iIdEstado + ", " +
                                    "IdMunicipio = " + iIdDelegacion + ", " +
                                    "Colonia = '" + TxtColonia.Text + "', " +
                                    "Calle = '" + TxtCalle.Text + "' " +
                                 "Where Referencia = '" + pReferencia + "'";

                // Insert en la tabla Estado de Documento
                SqlCommand cmd1 = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        public void Update_DetalleDireccion_Afectada(string pReferencia)
        {
            // Agregar Asegurado ITM_34
            int iIdEstadoA = Convert.ToInt32(ddlEstadoAfectado.SelectedValue);
            int iIdDelegacionA = Convert.ToInt32(ddlDelegacionAfectada.SelectedValue);

            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string strQuery = "Update ITM_35 " +
                                    "Set Codigo_Postal = '" + TxtCodigoPostalAfectado.Text + "', " +
                                    "IdEstado = " + iIdEstadoA + ", " +
                                    "IdMunicipio = " + iIdDelegacionA + ", " +
                                    "Colonia = '" + TxtColonia_Afectada.Text + "', " +
                                    "Calle = '" + TxtCalle_Afectada.Text + "', " +
                                    "Observaciones = '" + TxtObservaciones.Text + "', " +
                                    "Entrevistar_A = '" + TxtEntrevistar.Text + "', " +
                                    "Puesto_1 = '" + TxtPuesto_1.Text + "', " +
                                    "Telefono_1 = '" + TxtTel_1.Text + "', " +
                                    "Email_1 = '" + TxtEmail_1.Text + "', " +
                                    "Documentador = '" + TxtDocumentara.Text + "', " +
                                    "Puesto_2 = '" + TxtPuesto_2.Text + "', " +
                                    "Telefono_2 = '" + TxtTel_2.Text + "', " +
                                    "Email_2 = '" + TxtEmail_2.Text + "', " +
                                    "Rep_Legal = '" + TxtLegal.Text + "' " +
                                 "Where Referencia = '" + pReferencia + "'";

                // Insert en la tabla Estado de Documento
                SqlCommand cmd1 = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        public void Update_DetalleAjustador(string pReferencia)
        {
            // Agregar Asegurado ITM_36
            int iIdLocal = Convert.ToInt32(ddlLocal.SelectedValue);
            int iIdCliente = Convert.ToInt32(ddlCliente.SelectedValue);
            int iIdOficina = Convert.ToInt32(ddlLocal.SelectedValue);
            int iIdAjustador = Convert.ToInt32(ddlAjustador.SelectedValue);
            int iIdCorredor = Convert.ToInt32(ddlCorredor.SelectedValue);
            int iIdAgente = Convert.ToInt32(ddlAgente.SelectedValue);

            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string strQuery = "Update ITM_36 " +
                                    "Set IdLocal = " + iIdLocal + ", " +
                                    "IdCliente = " + iIdCliente + ", " +
                                    "IdOficina = " + iIdOficina + ", " +
                                    "IdAjustador = " + iIdAjustador + ", " +
                                    "IdCorredor = " + iIdCorredor + ", " +
                                    "IdAgente = " + iIdAgente + ", " +
                                    "Observaciones = '" + TxtObservaciones_2.Text + "', " +
                                    "Antecedentes = '" + TxtAntecedentes.Text + "', " +
                                    "Causa = '" + TxtCausa.Text + "', " +
                                    "Bienes = '" + TxtBienes.Text + "' " +
                                 "Where Referencia = '" + pReferencia + "'";

                // Insert en la tabla Estado de Documento
                SqlCommand cmd1 = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        public void Update_tbDetalleArchivos(string pReferencia, string pTpoDocumento)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                string pUrl_Imagen = "";
                string pNom_Imagen = "";
                int iEntregado = 1;

                string strQuery = "Update ITM_37 " +
                                    "Set Url_Imagen = '" + pUrl_Imagen + "', " +
                                    "Url_Path_Final = NULL, " +
                                    "Nom_Imagen = '" + pNom_Imagen + "', " +
                                    "Nom_Imagen_Final = NULL, " +
                                    "Fec_Entrega = GETDATE(), " +
                                    "Entregado = " + iEntregado + " " +
                                 "Where Referencia = '" + pReferencia + "'" +
                                 "And IdTpoDocumento = '" + pTpoDocumento + "'";

                // Insert en la tabla Estado de Documento
                SqlCommand cmd1 = new SqlCommand(strQuery, Conecta.ConectarBD);

                SqlDataReader dr1 = cmd1.ExecuteReader();

                cmd1.Dispose();
                dr1.Dispose();

                Conecta.Cerrar();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }

        public void Limpia(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
                // else if (control is DropDownList)
                //     ((DropDownList)control).Items.Clear();
                else if (control is DropDownList)
                    ((DropDownList)control).SelectedIndex = 0;
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
                else if (control is CheckBox)
                    ((CheckBox)control).Enabled = false;
                else if (control.HasControls())
                    //Esta linea detécta un Control que contenga otros Controles
                    //Así ningún control se quedará sin ser limpiado.
                    inhabilitar(control.Controls);
            }

        }

        public void InicializarGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("UsReferencia", Type.GetType("System.String"));
            dt.Columns.Add("Num_Siniestro", Type.GetType("System.String"));
            dt.Columns.Add("Num_Reporte", Type.GetType("System.String"));
            dt.Columns.Add("Nombre", Type.GetType("System.String"));
            dt.Columns.Add("Num_Poliza", Type.GetType("System.String"));

            grdPanelBusqueda.DataSource = dt;
            grdPanelBusqueda.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }

        //protected void Button2_Click(object sender, EventArgs e)
        //{
        //    MultiView1.ActiveViewIndex = 1;
        //}

        protected void BtnEnviarPoliza_Click(object sender, EventArgs e)
        {
            try
            {
                string sReferencia = TxtReferencia.Text;

                // Acceda al archivo usando el nombre del archivo de entrada HTML.
                HttpPostedFile postedFile = Request.Files["oFilePoliza"];

                string nomFile = postedFile.FileName;

                if (postedFile.FileName != "")
                {
                    this.UploadToAzure(nomFile, sReferencia, "Polizas");
                }
                else
                {
                    LblMessage.Text = "Debe seleccionar un archivo";
                    mpeMensaje.Show();
                    return;
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
                return;
            }
        }

        protected void BtnEnviarCorreo_Click(object sender, EventArgs e)
        {
            try
            {
                string sReferencia = TxtReferencia.Text;

                // Acceda al archivo usando el nombre del archivo de entrada HTML.
                HttpPostedFile postedFile = Request.Files["oFileCorreo"];

                string nomFile = postedFile.FileName;

                if (postedFile.FileName != "")
                {
                    this.UploadToAzure(nomFile, sReferencia, "Correos");
                }
                else
                {
                    LblMessage.Text = "Debe seleccionar un archivo";
                    mpeMensaje.Show();
                    return;
                }
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
                return;
            }
        }

        public int Campos_Obligatorios()
        {
            try
            {

                if (ddlAseguradora.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar Aseguradora";
                    mpeMensaje.Show();
                    return -1;
                }
                if (ddlDivision.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar tipo de Division";
                    mpeMensaje.Show();
                    return -1;
                }
                if (TxtAnalista.Text == "" || TxtAnalista == null)
                {
                    LblMessage.Text = "Capturar nombre de Analista";
                    mpeMensaje.Show();
                    return -1;
                }
                if (ddlAtencion.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccione una Atencion";
                    mpeMensaje.Show();
                    return -1;
                }
                if (ddlEventCatastrofico.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccione un Evento Catastrofico";
                    mpeMensaje.Show();
                    return -1;
                }
                if (ddlProceso.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar el Ramo";
                    mpeMensaje.Show();
                    return -1;
                }
                if (ddlSubProceso.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar el Subramo";
                    mpeMensaje.Show();
                    return -1;
                }
                if (ddlCobertura.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar tipo de Cobertura";
                    mpeMensaje.Show();
                    return -1;
                }
                if (ddlRiesgo.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar tipo de Riesgo";
                    mpeMensaje.Show();
                    return -1;
                }
                modGeneral general = new modGeneral();

                if (TxtNumSiniestro.Text == "" || TxtNumSiniestro.Text == null)
                {
                    if (general.valTxtNumeros(TxtNumSiniestro.Text))
                    {
                        LblMessage.Text = "Capturar numero de Siniestro";
                        mpeMensaje.Show();
                        return -1;
                    }
                }
                if (TxtNumReporte.Text == "" || TxtNumReporte.Text == null || general.valTxtNumeros(TxtNumReporte.Text))
                {
                    LblMessage.Text = "Capturar Numero de Reporte";
                    mpeMensaje.Show();
                    return -1;
                }
                if (TxtFechaAseguradora.Text == "")
                {
                    LblMessage.Text = "Seleccionar fecha del reporte a aseguradora";
                    mpeMensaje.Show();
                    return -1;
                }
                if (TxtFechaSiniestro.Text == "")
                {
                    LblMessage.Text = "Seleccionar fecha del siniestro";
                    mpeMensaje.Show();
                    return -1;
                }
                if (TxtRefAsegurado.Text == "" || TxtRefAsegurado.Text == null || general.valTxtNumeros(TxtRefAsegurado.Text))
                {
                    LblMessage.Text = "Capturar referencia del Asegurado";
                    mpeMensaje.Show();
                    return -1;
                }
                if (TxtReporto.Text == "" || TxtReporto.Text == null)
                {
                    LblMessage.Text = "Capturar quien reporto?";
                    mpeMensaje.Show();
                    return -1;
                }
                if (TxtFechaAsignacion.Text == "")
                {
                    LblMessage.Text = "Seleccionar la fecha de asignacion";
                    mpeMensaje.Show();
                    return -1;
                }
                if (TxtHorAsignacion.Text == "" || TxtHorAsignacion.Text == null || general.valTxtHora(TxtHorAsignacion.Text))
                {
                    LblMessage.Text = "Capturar la hora de Asignacion";
                    mpeMensaje.Show();
                    return -1;
                }
                if (TxtNumPoliza.Text == "" || TxtNumPoliza.Text == null || general.valTxtNumeros(TxtNumPoliza.Text))
                {
                    LblMessage.Text = "Capturar numero de poliza";
                    mpeMensaje.Show();
                    return -1;
                }
                if (TxtIniciaVigencia.Text == "" || TxtIniciaVigencia.Text == null)
                {
                    LblMessage.Text = "Seleccionar fecha inicio de vigencia";
                    mpeMensaje.Show();
                    return -1;
                }
                if (TxtTerminaVigencia.Text == "" || TxtTerminaVigencia.Text == null)
                {
                    LblMessage.Text = "Seleccionar Fecha termino de vigencia";
                    mpeMensaje.Show();
                    return -1;
                }
                if (TxtCausaSiniestro.Text == "" || TxtCausaSiniestro == null)
                {
                    LblMessage.Text = "Capturar causa de siniestro";
                    mpeMensaje.Show();
                    return -1;
                }
                if (TxtEstimacion.Text == "" || TxtEstimacion.Text == null || general.valTxtNumeros(TxtEstimacion.Text))
                {
                    LblMessage.Text = "Capturar monto estimado";
                    mpeMensaje.Show();
                    return -1;
                }
                if (ddlMoneda.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar tipo de cambio/moneda";
                    mpeMensaje.Show();
                    return -1;
                }

                return 0;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
            finally
            {

            }

            return -1;
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Limpia(this.Controls);
            InicializarGrid();

            inhabilitar(this.Controls);

            BtnActualizar.Enabled = false;
            BtnDocumentos.Enabled = false;
            BtnNotas.Enabled = false;
            BtnFotos.Enabled = false;

            //BtnEnviarPoliza.Enabled = false;
            //BtnEnviarCorreo.Enabled = false;

            TxtReferencia.Enabled = true;
        }

        public void getGrdBusquedaRef(string pReferencia)
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                string strQuery = "SELECT V1.UsReferencia , V1.Nombre , V1.Num_Siniestro, V1.Num_Poliza, V1.Num_Reporte " +
                                    " FROM " +
                                    " ( " +
                                    "   SELECT e.UsReferencia, " +
                                    "   (SELECT Nombre FROM ITM_20 as t1, ITM_33 as t2 WHERE t1.IdAsegurado = t2.IdAsegurado and t2.Referencia = e.UsReferencia ) AS Nombre, " +
                                    "   (SELECT Num_Siniestro FROM ITM_32 WHERE e.UsReferencia = Referencia ) Num_Siniestro," +
                                    "   (SELECT Num_Poliza FROM ITM_32 WHERE e.UsReferencia = Referencia ) Num_Poliza , " +
                                    "   (SELECT Num_Reporte FROM ITM_32 WHERE e.UsReferencia = Referencia ) Num_Reporte " +
                                    "   FROM ITM_02 as e " +
                                    "   WHERE UsReferencia LIKE '%" + pReferencia + "%' " +
                                    "   UNION ALL " +
                                    "       SELECT a.Referencia, " +
                                    "       (SELECT Nombre FROM ITM_20 as t1, ITM_33 as t2 WHERE t1.IdAsegurado = t2.IdAsegurado and t2.Referencia Like '%" + pReferencia + "%' ) AS Nombre," +
                                    "       a.Num_Siniestro , a.Num_Poliza, a.Num_Reporte " +
                                    "       FROM ITM_02 as e INNER JOIN ITM_32 as a " +
                                    "       ON e.UsReferencia = a.Referencia " +
                                    "       AND e.UsReferencia LIKE '%" + pReferencia + "%'" +
                                    " ) V1 " +
                                    "GROUP BY UsReferencia, Nombre, Num_Siniestro, Num_Poliza, Num_Reporte";

                SqlCommand cmd = new SqlCommand(strQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    grdPanelBusqueda.ShowHeaderWhenEmpty = true;
                    grdPanelBusqueda.EmptyDataText = "No hay resultados.";
                }

                grdPanelBusqueda.DataSource = dt;
                grdPanelBusqueda.DataBind();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void grdPanelBusqueda_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cargaDocumento")
            {
                string sRef = Convert.ToString(e.CommandArgument);
                TxtReferencia.Text = sRef;
            }
        }

        protected void grdPanelBusqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPanelBusqueda.PageIndex = e.NewPageIndex;
            getGrdBusquedaRef(TxtReferencia.Text);
        }

        protected void grdPanelBusqueda_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.grdPanelBusqueda, "Select$" + e.Row.RowIndex.ToString()) + ";");
        }

        protected void grdPanelBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtReferencia.Text = Server.HtmlDecode(grdPanelBusqueda.SelectedRow.Cells[0].Text);
        }

        public void UploadToAzure(string sFilename, string sDirName, string sTpoDocumento)
        {

            string oInputFile = "";

            string ConnectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");
            string AccountName = ConfigurationManager.AppSettings.Get("StorageAccountName");

            try
            {
                // Get a reference to a share and then create it
                ShareClient share = new ShareClient(ConnectionString, AccountName);

                // Get a reference to a directory and create it
                ShareDirectoryClient directory = share.GetDirectoryClient(AccountName);

                // Get a reference to a subdirectory not located on root level
                directory = directory.GetSubdirectoryClient(sDirName + "/" + sTpoDocumento);

                if (!directory.Exists())
                {
                    directory = directory.GetSubdirectoryClient("../");
                    directory.CreateSubdirectory(sTpoDocumento);
                    directory = directory.GetSubdirectoryClient(sTpoDocumento);
                }

                // Get a reference to a file and upload it
                ShareFileClient file = directory.GetFileClient(sFilename);

                if (file.Exists())
                {
                    //Variables.wExiste = true;

                    LblMessage.Text = "El documento ya existe";
                    mpeMensaje.Show();
                    return;
                }
                else
                {

                    if (sTpoDocumento == "Polizas")
                    {
                        oInputFile = "oFilePoliza";

                    }else if (sTpoDocumento == "Correos")
                    {
                        oInputFile = "oFileCorreo";
                    }else
                    {

                    }

                    //Access the File using the Name of HTML INPUT File.
                    HttpPostedFile postedFile = Request.Files[oInputFile];

                    file.Create(postedFile.ContentLength);

                    int blockSize = 64 * 1024;
                    long offset = 0;    // Definir desplazamiento de rango http.

                    BinaryReader reader = new BinaryReader(postedFile.InputStream);
                    while (true)
                    {
                        byte[] buffer = reader.ReadBytes(blockSize);
                        if (buffer.Length == 0)
                            break;

                        MemoryStream uploadChunk = new MemoryStream();
                        uploadChunk.Write(buffer, 0, buffer.Length);
                        uploadChunk.Position = 0;

                        HttpRange httpRange = new HttpRange(offset, buffer.Length);
                        var resp = file.UploadRange(httpRange, uploadChunk);
                        offset += buffer.Length;    // Cambia el desplazamiento por el número de bytes ya escritos.
                    }

                    reader.Close();

                    //string folderName = Convert.ToString(Session["TpoDocumento"]);

                    //ConexionBD Conectar = new ConexionBD();
                    //Conectar.Abrir();

                    //// Actualizar la tabla Estado de Documento
                    //string sqlUpDate = "UPDATE ITM_04 " +
                    //                    "    SET IdStatus = '2'," +
                    //                        " Url_Imagen = '" + sDirName + "'," +
                    //                        " Nom_Imagen = '" + sFilename + "'," +
                    //                        "  Fec_Envio = GETDATE() " +
                    //                    " WHERE Referencia LIKE '%' + '" + sReferencia + "'  + '%' " +
                    //                    " AND IdTipoDocumento = '" + folderName + "'";

                    //SqlCommand cmd = new SqlCommand(sqlUpDate, Conectar.ConectarBD);

                    //cmd.ExecuteReader();
                }

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnNotas_Click(object sender, EventArgs e)
        {
            Session["Referencia"] = TxtReferencia.Text;
            Session["Asegurado"] = TxtBusqNombre.Text;

            Response.Redirect("Document_Notas.aspx");
        }

        protected void BtnDocumentos_Click(object sender, EventArgs e)
        {
            Session["Referencia"] = TxtReferencia.Text;
            Session["Asegurado"] = TxtBusqNombre.Text;
            Session["Proceso"] = "4"; //ddlProceso.SelectedValue

            Response.Redirect("Document_Files.aspx");
        }

        protected void BtnFotos_Click(object sender, EventArgs e)
        {
            Session["Referencia"] = TxtReferencia.Text;
            Session["Asegurado"] = TxtBusqNombre.Text;

            Response.Redirect("Document_Fotos.aspx");
        }

        protected void TxtReferencia_TextChanged(object sender, EventArgs e)
        {
            try
            {
                InicializarGrid();
                getGrdBusquedaRef(TxtReferencia.Text);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
    }
}