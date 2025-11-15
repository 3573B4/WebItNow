using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwReporte_Estatus_Asunto : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Session["DownloadsPath"] = GetDownloadFolderPath();

            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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
                    lblTitulo_Cat_Etapas.Text = GetGlobalResourceObject("GlobalResources", "lblTitulo_Cat_Etapas").ToString();

                    MostrarPanel(false); // Mostrar el panel principal por defecto

                    GetAseguradoras();
                    GetServicioFk(0);
                    GetCategoriaFk();


                }
                catch (Exception ex)
                {
                    LblMessage.Text = ex.Message;
                    this.mpeMensaje.Show();
                }
            }
        }

        private void MostrarPanel(bool mostrarAgregarProtocolo)
        {
            ConsultPanel.Visible = mostrarAgregarProtocolo;
            MainPanel.Visible = !mostrarAgregarProtocolo;
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void GetAseguradoras()
        {
            try
            {
                ddlAseguradora.Items.Clear(); // Limpia cualquier valor previo

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdOrden, Descripcion " +
                                  "  FROM ITM_67 " +
                                  " WHERE IdSeguros <> 'OTR'" +
                                  "   AND IdStatus = 1 " +
                                  " ORDER BY IdOrden";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlAseguradora.DataSource = dt;
                
                ddlAseguradora.DataValueField = "IdOrden";
                ddlAseguradora.DataTextField = "Descripcion";

                ddlAseguradora.DataBind();

                // ddlAseguradora.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlAseguradora.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        //filtrando Proyecto por numero de aseguradora antes servicio
        protected void GetServicioFk(int aseguradoraFk)
        {
            
            try
            {
                string sIdAseguradora = GetsAseguradoraFk(aseguradoraFk);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();


                string strQuery = "SELECT IdProyecto, Descripcion " +
                                  "  FROM ITM_78 " +
                                  " WHERE IdCliente = '" + sIdAseguradora + "'" +
                                  "   AND IdStatus = 1 ; ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlProyecto.DataSource = dt;

                ddlProyecto.DataValueField = "IdProyecto";
                ddlProyecto.DataTextField = "Descripcion";

                ddlProyecto.DataBind();

                // ddlProyecto.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));
                ddlProyecto.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "-1"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        //obtener el String de la aseguradora
        public string GetsAseguradoraFk(int aseguradoraFk)
        {
            string sIdAseguradora = string.Empty;

            
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();


            string strQuery = "SELECT IdSeguros " +
                              "  FROM itm_67 " +
                              " WHERE IdOrden = " + aseguradoraFk +
                              "   AND IdStatus = 1 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            foreach (DataRow row in dt.Rows)
            {
                sIdAseguradora = row["IdSeguros"].ToString().Trim();
            }

            dbConn.Close();

            return sIdAseguradora;
            
        }

        protected void GetCategoriaFk()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdTpoAsunto, Descripcion " +
                                        " FROM ITM_66 " +
                                        " WHERE IdStatus IN (0, 1) ;";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlCategoria.DataSource = dt;

                ddlCategoria.DataValueField = "IdTpoAsunto";
                ddlCategoria.DataTextField = "Descripcion";
                
                ddlCategoria.DataBind();

                //ddlCategoria.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));
                ddlCategoria.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "-1"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ddlAseguradora_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iIdAseguradora = Convert.ToInt32(ddlAseguradora.SelectedValue);

            GetServicioFk(iIdAseguradora);
        }

        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetEtapas();
        }

        protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetEtapas();
        }

        protected void GetEtapas()
        {

            int iIdAseguradora = Convert.ToInt32(ddlAseguradora.SelectedValue);
            int iIdProyecto = Convert.ToInt32(ddlProyecto.SelectedValue);
            int iIdCategoria = Convert.ToInt32(ddlCategoria.SelectedValue);
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);

                using (MySqlConnection conn = dbConn.Connection)
                {

                    conn.Open();
                    string strQuery = @"
                                    SELECT r.IdRelacion, r.IdEtapa_fk,
                                           e.Descripcion AS NombreEtapa,
                                           COALESCE(t.NumeroReferencias, 0) AS NumeroReferencias
                                      FROM ITM_97 r INNER JOIN ITM_83 e 
                                        ON r.IdEtapa_fk = e.IdDocumento
                                      LEFT JOIN ( 
                                                  SELECT IdRelacionEtapa, IdEtapa,
                                                         COUNT(*) AS NumeroReferencias
                                                    FROM ITM_100
                                                   WHERE IdStatus = 1 AND IdRelacionTareas IS NULL
                                                     AND IdTarea IS NULL AND IdSubTarea IS NULL
                                                   GROUP BY IdRelacionEtapa, IdEtapa
                                                ) t 
                                        ON t.IdRelacionEtapa = r.IdRelacion
                                       AND t.IdEtapa = r.IdEtapa_fk
                                     WHERE r.bSeleccion = 1 AND r.IdStatus = 1 AND e.IdStatus = 1
                                       AND r.IdAseguradora_fk = @IdAseguradora
                                       AND r.IdServicio_fk = @IdServicio
                                       AND r.IdCategoria_fk = @IdCategoria
                                     ORDER BY r.IdOrden ASC LIMIT 24; ";

                    using (MySqlCommand cmd = new MySqlCommand(strQuery, conn))
                    {
                        // Parámetros seguros
                        cmd.Parameters.AddWithValue("@IdAseguradora", iIdAseguradora);
                        cmd.Parameters.AddWithValue("@IdServicio", iIdProyecto);
                        cmd.Parameters.AddWithValue("@IdCategoria", iIdCategoria);

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count == 0)
                        {
                            //lvOrderTaskItem.Items.Clear();
                            //lvOrderTaskItem. = "No hay resultados.";
                            //lvOrderTaskItem.EmptyDataTemplate = "no hay Tareas.";
                        }

                        lvEtapas.DataSource = dt;

                        lvEtapas.DataBind();

                    }

                }

                //DataTable dt = dbConn.ExecuteQuery(strQuery);
                //dbConn.Close();
                //lvOrderTaskItem.Items.Clear();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnRegresarPnlMain_Click(object sender, EventArgs e)
        {
            MostrarPanel(false); // Mostrar el panel principal
        }

        protected void lvEtapas_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "VerReferencias")
            {
                string idEtapa = e.CommandArgument.ToString();

                // 👉 Aquí puedes usar idEtapa para traer las referencias
                // Ejemplo temporal:
                lblIdEtapaSelect.Text = "Etapa seleccionada: " + idEtapa;
                CargarReferenciasEjemplo();

                // Mostramos el panel de consulta
                MostrarPanel(true);
            }
        }

        private void CargarReferenciasEjemplo()
        {
            // Creamos tabla en memoria
            DataTable dt = new DataTable();
            dt.Columns.Add("Referencia_Siniestro");
            dt.Columns.Add("NumSiniestro");
            dt.Columns.Add("NumPoliza");
            dt.Columns.Add("NomProyecto");
            dt.Columns.Add("Fecha_Asignacion", typeof(DateTime));
            dt.Columns.Add("Seguro_Cia");
            dt.Columns.Add("Tpo_Asunto");
            dt.Columns.Add("NomAsegurado");
            dt.Columns.Add("Resp_Tecnico");
            dt.Columns.Add("Resp_Administrativo");
            dt.Columns.Add("Referencia_Anterior");

            // Agregamos tres filas de ejemplo (fijos)
            dt.Rows.Add("REF-001", "SIN-4521", "POL-2024-01", "Proyecto Reforma",
                        new DateTime(2024, 9, 15), "AXA Seguros", "Reparación",
                        "Juan Pérez", "Ing. Ramírez", "Lic. González", "REF-000");

            dt.Rows.Add("REF-002", "SIN-8794", "POL-2024-02", "Proyecto Durango",
                        new DateTime(2024, 10, 1), "GNP Seguros", "Evaluación",
                        "María López", "Ing. Hernández", "Lic. Castillo", "REF-001");

            dt.Rows.Add("REF-003", "SIN-9630", "POL-2024-03", "Proyecto Insurgentes",
                        new DateTime(2024, 10, 5), "Mapfre", "Seguimiento",
                        "Carlos Méndez", "Ing. Ortega", "Lic. Jiménez", "REF-002");

            // Asignamos la fuente de datos
            GrdReferenciasEtapa.DataSource = dt;
            GrdReferenciasEtapa.DataBind();
        }


    }
}