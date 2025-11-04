using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;


// ddlServicio = proyecto. los datos que se muestran son los del
//                         proyecto tabla 
//ddlCaterogia = categoria 
//


namespace WebItNow_Peacock
{
    public partial class fwCatalog_SLA_Protocolos : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Session["DownloadsPath"] = GetDownloadFolderPath();

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
                    lblTitulo_Cat_Protocolos.Text = GetGlobalResourceObject("GlobalResources", "lblTitulo_Cat_Protocolos").ToString();

                    MostrarPanel(false); // Mostrar el panel principal por defecto

                    GetCiaSeguros();
                    //GetServicios();
                    GetServicioFk(0);
                    //GetCategoria();
                    GetCategoriaFk(0);
                    //GetEtapas();
                    GetProtocolos(0, 0);
                    GetTaskddlDependencia(0);
                    GetGrdChkBxPnlAddTask();
                    GetGrdChBoxTasksConx(0, 0);
                    GetTareas(0);
                    GetUnidadTiempo();
                    
                    //BtnInsertConxTask.Visible = false;
                    BtnModalAddTask.Enabled = false;

                    GetGrdChBoxAddProtocolos();

                    upGrdProtConx.Update();

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
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdOrden, Descripcion " +
                                  "  FROM ITM_67 " +
                                  " WHERE IdSeguros <> 'OTR'" +
                                  "   AND IdStatus = 1 " +
                                  " ORDER BY IdOrden";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlAseguradora.DataSource = dt;
                ddlAseguradoraAddProt.DataSource = dt;

                ddlAseguradora.DataValueField = "IdOrden";
                ddlAseguradora.DataTextField = "Descripcion";

                ddlAseguradoraAddProt.DataValueField = "IdOrden";
                ddlAseguradoraAddProt.DataTextField = "Descripcion";

                ddlAseguradora.DataBind();
                ddlAseguradoraAddProt.DataBind();

                // ddlAseguradora.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlAseguradora.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));
                // ddlAseguradoraAddProt.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlAseguradoraAddProt.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        // Obtener las lineas de negocios o los servicios tabla 96 (tentativamente)
        // sea por la llave foranea de la Aseguradora o toda al mismo tiempo
        protected void GetServicios()
        {
            try
            {
                /*
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdServicio, Nombre " +
                                  "  FROM ITM_96 " +
                                  " WHERE IdStatus = 1 ;";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlServicio.DataSource = dt;

                ddlServicio.DataValueField = "IdServicio";
                ddlServicio.DataTextField = "Nombre";

               /ddlServicio.DataBind();

                ddlServicio.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
                */
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

                ddlServicio.DataSource = dt;
                ddlServicioAddProt.DataSource = dt;

                ddlServicio.DataValueField = "IdProyecto";
                ddlServicioAddProt.DataValueField = "IdProyecto";

                ddlServicio.DataTextField = "Descripcion";
                ddlServicioAddProt.DataTextField = "Descripcion";

                ddlServicio.DataBind();
                ddlServicioAddProt.DataBind();

                //ddlServicio.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));
                ddlServicio.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "-1"));

                //ddlServicioAddProt.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));
                ddlServicioAddProt.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "-1"));

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

            //try
            //{
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
            //}
            //catch (System.Exception ex)
            //{
            //    LblMessage.Text = ex.Message;
            //    mpeMensaje.Show();
            //    return sIdAseguradora = "0";
            //}
        }
        
        // Esta tabla la 66 es donde especifican si el servicio es
        // simple, complejo, civil, es una tabla sub categorias de 
        // los servicios.
        protected void GetCategoria()
        {
            try
            {
                /*
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdTpoAsunto, Descripcion " +
                                  "  FROM ITM_66 " +
                                  //" WHERE IdServicio_fk = IdServicio" +
                                  //"   AND IdServicio_fk =  " + IdServicio +
                                  " WHERE IdStatus = 1 ;";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlCategoria.DataSource = dt;

                ddlCategoria.DataValueField = "IdTpoAsunto";
                ddlCategoria.DataTextField = "Descripcion";

                ddlCategoria.DataBind();

                ddlCategoria.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
                */
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetCategoriaFk(int idServicioFk)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                /*
                string strQuery = "SELECT c.IdTpoAsunto, c.Descripcion " +
                                  "  FROM ITM_66 AS c " +
                             " INNER JOIN ITM_96 AS s " +
                                  "    ON c.IdServicio_fk = s.IdServicio " +
                                  " WHERE c.IdStatus = 1 " +
                                  "   AND c.IdServicio_fk =  " + idServicioFk +
                                  "   AND s.IdStatus = 1 ;";
                */
                string strQuery = "SELECT IdTpoAsunto, Descripcion " +
                                        " FROM ITM_66 " +
                                        " WHERE IdStatus IN (0, 1) ;";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlCategoria.DataSource = dt;
                ddlCategoriaAddProt.DataSource = dt;

                ddlCategoria.DataValueField = "IdTpoAsunto";
                ddlCategoriaAddProt.DataValueField = "IdTpoAsunto";

                ddlCategoria.DataTextField = "Descripcion";
                ddlCategoriaAddProt.DataTextField = "Descripcion";

                ddlCategoria.DataBind();
                ddlCategoriaAddProt.DataBind();

                //ddlCategoria.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));
                ddlCategoria.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "-1"));
                //ddlCategoriaAddProt.Items.Insert(0, new ListItem("-- Seleccionar --", "-1"));
                ddlCategoriaAddProt.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "-1"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetTareas(int IdSLA)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Consulta a las tablas : Estado de Documento (Expediente) = ITM_54
                string strQuery = "SELECT IdTarea, IdSLA, NomTarea, a.Descripcion as Descripcion, Plazo, DocInterno, " +
                                  //"  CASE WHEN UnidadTiempo = 1 THEN 'Minutos' ELSE 'Horas' END as UnidadTiempo,  " +
                                  "  CASE WHEN DocInterno = 1 THEN 'INTERNO' ELSE '' END as TpoInterno, b.UnidadTiempo " +
                                  "  FROM ITM_50 as a, ITM_54 as b " +
                                  " WHERE IdSLA = " + IdSLA + " " +
                                  "   AND a.IdUnidadTiempo = b.UnidadTiempo" +
                                  "   AND b.IdStatus = 1 ORDER BY IdTarea";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdTasks.ShowHeaderWhenEmpty = true;
                    GrdTasks.EmptyDataText = GetGlobalResourceObject("GlobalResources", "msg_NoResults").ToString();

                    //GrdTasks.EmptyDataText = "No hay resultados.";

                }

                GrdTasks.DataSource = dt;
                GrdTasks.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdTasks.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetEtapas()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdDocumento, Descripcion " +
                                  "  FROM ITM_83 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                //ddlConclusion.DataSource = dt;
                //ddlConclusionAddProt.DataSource = dt;

                //ddlConclusion.DataValueField = "IdDocumento";
                //ddlConclusion.DataTextField = "Descripcion";

                //ddlConclusionAddProt.DataValueField = "IdDocumento"; 
                //ddlConclusionAddProt.DataTextField = "Descripcion";

                //ddlConclusion.DataBind();
                //ddlConclusionAddProt.DataBind();

                //ddlConclusion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                //ddlConclusionAddProt.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetProtocolos(int iIdProyecto,int IdCategoria)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                /*
                string strQuery = "SELECT IdSLA, NomProtocolo " +
                                  "  FROM ITM_55 p " +
                                  " INNER JOIN ITM_67 e " +
                                  "    ON p.IdAseguradora_fk = e.IdOrden " +
                                  " WHERE p.IdStatus = 1 " +
                                  "   AND e.IdStatus = 1 " +
                                  "   AND p.IdAseguradora_fk = " + IdEtapa +";";
                */

                string strQuery = " SELECT r.IdRelacion, e.Descripcion, r.IdOrden " +
                                  "  FROM ITM_97 r " +
                                  " INNER JOIN ITM_83 e " +
                                  "    ON r.IdEtapa_fk = e.IdDocumento " +
                                  " WHERE r.IdAseguradora_fk = " + ddlAseguradora.SelectedValue +
                                  "   AND r.IdServicio_fk    = " + iIdProyecto +
                                  "   AND r.IdCategoria_fk = " + IdCategoria +
                                  "   AND r.bSeleccion    = 1 " +
                                  "   AND r.IdStatus      = 1 " +
                                  "   AND e.IdStatus      = 1 " +
                                  " ORDER BY r.IdOrden ASC; ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlProtocolos.DataSource = dt;

                ddlProtocolos.DataValueField = "IdRelacion";
                ddlProtocolos.DataTextField = "Descripcion";

                ddlProtocolos.DataBind();
                //ddlProtocolos.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlProtocolos.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                dbConn.Close();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetUnidadTiempo()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string strQuery = "SELECT IdUnidadTiempo, Descripcion " +
                                  "  FROM ITM_50 " +
                                  " WHERE IdStatus = 1 ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);
                ddlUnidadTiempo.DataSource = dt;

                ddlUnidadTiempo.DataValueField = "IdUnidadTiempo";
                ddlUnidadTiempo.DataTextField = "Descripcion";

                ddlUnidadTiempo.DataBind();
                //ddlUnidadTiempo.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                ddlUnidadTiempo.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                dbConn.Close();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }


        protected void BtnCerrar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAceptar_Click(object sender, EventArgs e)
        {
            if (Variables.wEliminar == 0)
            {
                // Eliminar Tareas
                Eliminar_ITM_54();
            } 
            else
            {
                // Eliminar protocolos (SLA)
                Eliminar_ITM_55();
            }

            TxtTarea.Text = string.Empty;
            TxtPlazo.Text = string.Empty;
            chkTaskInterno.Checked = false;

            Variables.wEliminar = 0;

            MostrarPanel(false);
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void GrdProcesos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GrdProcesos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdProcesos_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdProcesos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdProcesos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void BtnAnular_Click(object sender, EventArgs e)
        {
            // inicializar controles.
            ddlProtocolos.Enabled = true;

            TxtTarea.Text = string.Empty;
            TxtPlazo.Text = string.Empty;
            ddlUnidadTiempo.SelectedIndex = 0;
            chkTaskInterno.Checked = false;

            TxtTarea.ReadOnly = false;
            TxtPlazo.ReadOnly = false;
            ddlUnidadTiempo.Enabled = true;
            chkTaskInterno.Enabled = true;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void BtnEditar_Click(object sender, EventArgs e)
        {
            // ddlCliente.Enabled = false;
            ddlAseguradora.Enabled = false;
            ddlServicio.Enabled = false;
            ddlCategoria.Enabled = false;
            ddlProtocolos.Enabled = false;
            BtnModalAddTask.Enabled = true;
            //habilitar los checkbox de la grid

            TxtTarea.ReadOnly = false;
            TxtPlazo.ReadOnly = false;
            ddlUnidadTiempo.Enabled = true;
            chkTaskInterno.Enabled = true;

            BtnEditar.Visible = false;
            BtnGrabar.Visible = true;
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            //if (ddlCliente.SelectedValue == "0")
            //{
            //    LblMessage.Text = "Seleccionar Compañia de Seguros";
            //    mpeMensaje.Show();
            //    return;
            //}

            //ddlTpoAsegurado
            if (ddlProtocolos.SelectedValue == "0")
            {
                // LblMessage.Text = "Seleccionar Protocolo (SLA)";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Protocolo_SLA").ToString();
                mpeMensaje.Show();
                return;
            }
            if (TxtTarea.Text == "" || TxtTarea.Text == null)
            {
                //LblMessage.Text = "Capturar Descripción de la Tarea";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Capturar_Descripcion_Tarea").ToString();
                mpeMensaje.Show();
                return;
            }
            if (TxtPlazo.Text == "" || TxtPlazo.Text == null || TxtPlazo.Text.Trim() == "")
            {
                //LblMessage.Text = "Capturar Descripción del Plazo";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Capturar_Descripcion_Plazo").ToString();
                mpeMensaje.Show();
                return;
            }
            if (ddlUnidadTiempo.SelectedValue == "0")
            {
                //LblMessage.Text = "Seleccionar Unidad de Tiempo";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Unidad_Tiempo").ToString();
                mpeMensaje.Show();
                return;
            }

            Actualizar_ITM_54();

            // inicializar controles.
            //ddlCliente.Enabled = true;
            ddlAseguradora.Enabled = true;
            ddlServicio.Enabled = true;
            ddlCategoria.Enabled = true;
            ddlProtocolos.Enabled = true;
            // ddlCliente.SelectedIndex = 0;

            TxtTarea.Text = string.Empty;
            TxtPlazo.Text = string.Empty;
            ddlUnidadTiempo.SelectedIndex = 0;
            chkTaskInterno.Checked = false;

            TxtTarea.ReadOnly = false;
            TxtPlazo.ReadOnly = false;
            ddlUnidadTiempo.Enabled = true;
            chkTaskInterno.Enabled = true;

            BtnEditar.Visible = true;
            BtnGrabar.Visible = false;
            BtnAnular.Visible = false;

            BtnEditar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        protected void Actualizar_ITM_54()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                int index = Variables.wRenglon;

                int IdTarea = Convert.ToInt32(GrdTasks.Rows[index].Cells[1].Text);
                int IdSLA = Convert.ToInt32(ddlProtocolos.SelectedValue);

                int iDocInterno = 0;

                if (chkTaskInterno.Checked)
                {
                    iDocInterno = 1;
                }

                // Actualizar registro(s) tablas (ITM_54)
                string strQuery = "UPDATE ITM_54 " +
                                  "   SET NomTarea = '" + TxtTarea.Text.Trim() + "', " +
                                  "       Plazo = '" + TxtPlazo.Text.Trim() + "', " +
                                  "       UnidadTiempo = " + ddlUnidadTiempo.SelectedValue + ", " +
                                  "       DocInterno = " + iDocInterno + " " +
                                  " WHERE IdTarea = " + IdTarea + " " +
                                  "   AND IdSLA = " + IdSLA + " ";


                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                //LblMessage.Text = "Se actualizo tarea, correctamente";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Tarea_Actualizada").ToString();
                mpeMensaje.Show();

                GetTareas(Convert.ToInt32(ddlProtocolos.SelectedValue));
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }


        protected void Eliminar_ITM_54()
        {
            try
            {
                int index = Variables.wRenglon;

                int IdTarea = Convert.ToInt32(GrdTasks.Rows[index].Cells[1].Text); ;

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_54)
                string strQuery = "DELETE FROM ITM_54 " +
                                  " WHERE IdTarea = " + IdTarea + " ";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                //LblMessage.Text = "Se elimino tarea, correctamente";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Tarea_Eliminada").ToString();
                mpeMensaje.Show();

                GetTareas(Convert.ToInt32(ddlProtocolos.SelectedValue));
            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void Eliminar_ITM_55()
        {
            try
            {
                int index = Variables.wRenglon;

                int IdSLA = Convert.ToInt32(GrdProtocolos.Rows[index].Cells[0].Text);

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Eliminar registro(s) tablas (ITM_54)
                string strQuery = "DELETE FROM ITM_54 " +
                                  " WHERE IdSLA = '" + IdSLA + "'; ";

                strQuery += Environment.NewLine;

                // Eliminar registro(s) tablas (ITM_55)
                strQuery += "DELETE FROM ITM_55 " +
                                  " WHERE IdSLA = " + IdSLA + " ";


                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                //LblMessage.Text = "Se elimino protocolo, correctamente";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Protocolo_Eliminado").ToString();
                mpeMensaje.Show();
                //ver el flujo
                GetTareas(Convert.ToInt32(ddlProtocolos.SelectedValue));
                GetProtocolos(0, 0);

            }

            catch (System.Exception ex)
            {
                // Show(ex.Message);
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlAseguradora.SelectedValue == "0")
                {
                    //LblMessage.Text = "Seleccionar Aseguradora";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Aseguradora").ToString();
                    mpeMensaje.Show();
                    return;
                }
                /*
                if (ddlServicio.SelectedValue == "0")
                {
                    LblMessage.Text = "Seleccionar Servicio";
                    mpeMensaje.Show();
                    return;
                }
                */
                if (ddlCategoria.SelectedValue == "-1")
                {
                    //LblMessage.Text = "Seleccionar Categoría";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Tipo_Asunto").ToString();
                    mpeMensaje.Show();
                    return;
                }

                if (ddlProtocolos.SelectedValue == "0")
                {
                    //LblMessage.Text = "Seleccionar Protocolo (SLA)";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Protocolo_SLA").ToString();
                    mpeMensaje.Show();
                    return;
                }

                if (TxtTarea.Text.Trim() == "" || TxtTarea.Text == null)
                {
                    //LblMessage.Text = "Capturar Descripción de la Tarea";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Capturar_Descripcion_Tarea").ToString();
                    mpeMensaje.Show();
                    return;
                }

                if (TxtPlazo.Text == "" || TxtPlazo.Text == null)
                {
                    //LblMessage.Text = "Capturar Descripción del Plazo";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Capturar_Descripcion_Plazo").ToString();
                    mpeMensaje.Show();
                    return;
                }

                if (string.IsNullOrEmpty(TxtTimeSleep.Text.Trim()))
                {
                    //LblMessage.Text = "Capturar tiempo de espera";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Capturar_Tiempo_Espera").ToString();
                    mpeMensaje.Show();
                    return;
                }
                
                if (ddlUnidadTiempo.SelectedValue == "0")
                {
                    //LblMessage.Text = "Seleccionar Unidad de Tiempo";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Unidad_Tiempo").ToString();
                    mpeMensaje.Show();
                    return;
                }

                int iIdTarea = GetIdConsecutivoMax();

                int iDocInterno = 0;
                int iTaskAutoma = 0;

                if (chkTaskInterno.Checked)
                {
                    iDocInterno = 1;
                }
                if (chbxTaskAutom.Checked)
                {
                    iTaskAutoma = 1;
                }

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_54)
                string strQuery = "INSERT INTO ITM_54 (IdTarea, NomTarea, Plazo, PlazoSleep," +
                                                     " UnidadTiempo, DocInterno, TaskAutomatica, IdStatus) " +
                                  "VALUES (" + iIdTarea + ", '" + TxtTarea.Text.Trim() + "', " +
                                  "'" + TxtPlazo.Text.Trim() + "'," + TxtTimeSleep.Text.Trim() + ", " + 
                                  ddlUnidadTiempo.SelectedValue + ", " + iDocInterno + ", "+
                                  iTaskAutoma +", 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                GetTareas(Convert.ToInt32(ddlProtocolos.SelectedValue));
                GetGrdChkBxPnlAddTask();

                //LblMessage.Text = "Se agrego tarea, correctamente";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Tarea_Agregada").ToString();
                mpeMensaje.Show();

                // Inicializar Controles
                TxtTarea.Text = string.Empty;
                TxtPlazo.Text = string.Empty;
                TxtTimeSleep.Text = string.Empty;
                ddlUnidadTiempo.SelectedIndex = 0;
                chkTaskInterno.Checked = false;
                chbxTaskAutom.Checked = false;

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

            string strQuery = "SELECT COALESCE(MAX(IdTarea), 0) + 1 IdTarea " +
                                " FROM ITM_54 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);


            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdTarea"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        public int GetIdSLAMax()
        {

            int IdConsecutivoMax = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = "SELECT COALESCE(MAX(IdSLA), 0) + 1 IdSLA " +
                                " FROM ITM_55 ";

            DataTable dt = dbConn.ExecuteQuery(strQuery);


            foreach (DataRow row in dt.Rows)
            {
                IdConsecutivoMax = Convert.ToInt32(row["IdSLA"].ToString().Trim());
            }

            dbConn.Close();

            return IdConsecutivoMax;

        }

        protected void ImgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;

            TxtTarea.Text = Server.HtmlDecode(Convert.ToString(GrdTasks.Rows[index].Cells[0].Text));
            TxtPlazo.Text = Server.HtmlDecode(Convert.ToString(GrdTasks.Rows[index].Cells[3].Text));
            ddlUnidadTiempo.SelectedValue = Convert.ToString(GrdTasks.Rows[index].Cells[6].Text);
            chkTaskInterno.Checked = GrdTasks.Rows[index].Cells[2].Text == "1";

            ddlProtocolos.Enabled = false;
            TxtTarea.ReadOnly = true;
            TxtPlazo.ReadOnly = true;
            ddlUnidadTiempo.Enabled = false;
            chkTaskInterno.Enabled = false;

            BtnAnular.Visible = true;
            BtnEditar.Enabled = true;
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

            LblMessage_1.Text = "¿Desea eliminar la tarea?";
            mpeMensaje_1.Show();
        }

        protected void ddlAseguradora_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetServicioFk(Convert.ToInt32(ddlAseguradora.SelectedValue));
        }

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iIdProyecto = Convert.ToInt32(ddlServicio.SelectedValue);
            int iIdCategoria = Convert.ToInt32(ddlCategoria.SelectedValue);
            //if (iIdProyecto == -1)
            //{
            //    iIdProyecto = 0;
            //}
            //GetCategoriaFk(Convert.ToInt32(ddlServicio.SelectedValue));
            GetProtocolos(iIdProyecto, iIdCategoria);
        }
        protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iIdProyecto = Convert.ToInt32(ddlServicio.SelectedValue);
            int iIdCategoria = Convert.ToInt32(ddlCategoria.SelectedValue);
            //if (iIdProyecto == -1)
            //{
            //    iIdProyecto = 0;
            //}
            GetProtocolos(iIdProyecto,iIdCategoria);

        }
        protected void ddlProtocolos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iIdTask_fk = Convert.ToInt32(ddlTaskPadre.SelectedValue);
            
            //BtnModalAddTask.Enabled = false;
            BtnEditGrdTask.Enabled = true;
            //GetTareas(Convert.ToInt32(ddlProtocolos.SelectedValue));
            GetTaskddlDependencia(Convert.ToInt32(ddlProtocolos.SelectedValue));
            GetGrdChBoxTasksConx(Convert.ToInt32(ddlProtocolos.SelectedValue), iIdTask_fk);
            DesactivarCheckBoxes(grdTaskConx, false);
            
        }

        protected void ddlTaskPadre_SelectedIndexChanged(object sender, EventArgs e)
        {

            GetGrdChBoxTasksConx(Convert.ToInt32(ddlProtocolos.SelectedValue),
                                 Convert.ToInt32(ddlTaskPadre.SelectedValue));
            DesactivarCheckBoxes(grdTaskConx, false);
        }

        protected void ddlAseguradoraAddProt_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iIdAseguradora = Convert.ToInt32(ddlAseguradoraAddProt.SelectedValue);
            
            GetServicioFk(iIdAseguradora);
        }

        protected void ddlServicioAddProt_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iIdProyecto = Convert.ToInt32(ddlServicioAddProt.SelectedValue);
            int iIdCategoria = Convert.ToInt32(ddlCategoriaAddProt.SelectedValue);

            //if (iIdProyecto == -1 & iIdCategoria == 0)
            //{
            //    //BtnGrabarProts.Visible = true;
            //    BtnEditarConxProt.Visible = false;
            //    grdProtConx.Visible = false;
            //    ddlServicio.SelectedValue = "-1";
            //}
            //else
            //{
            //    if (iIdProyecto == -1)
            //    {
            //        iIdProyecto = 0;
            //    }
            //    //BtnGrabarProts.Visible = true;
            //    BtnEditarConxProt.Visible = true;
            //    grdProtConx.Visible = true;
            //}
            BtnEditarConxProt.Visible = true;
            grdProtConx.Visible = true;

            //GetCategoriaFk(Convert.ToInt32(ddlServicioAddProt.SelectedValue));
            GetGrdChBoxProtocolosConx(iIdProyecto, iIdCategoria);

            DesactivarCheckBoxes(grdProtConx, false);
            
            upGrdProtConx.Update();
        }

        protected void ddlCategoriaAddProt_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iIdProyecto = Convert.ToInt32(ddlServicioAddProt.SelectedValue);
            int iIdCategoria = Convert.ToInt32(ddlCategoriaAddProt.SelectedValue);
            
            //if(iIdProyecto == -1 & iIdCategoria == 0)
            //{
            //    //BtnGrabarProts.Visible = true;
            //    BtnEditarConxProt.Visible = false;
            //    grdProtConx.Visible = false;
            //    ddlServicio.SelectedValue = "-1";
            //}
            //else
            //{
            //    if (iIdProyecto == -1)
            //    {
            //        iIdProyecto = 0;
            //    }
            //    //BtnGrabarProts.Visible = true;
            //    BtnEditarConxProt.Visible = true;
            //    grdProtConx.Visible = true;
            //}

            BtnEditarConxProt.Visible = true;
            grdProtConx.Visible = true;

            GetGrdChBoxProtocolosConx(iIdProyecto,iIdCategoria);

            DesactivarCheckBoxes(grdProtConx, false);

            //Obtener_Valores_ChBox(grdProtConx, "");
        }

        protected void BtnCancelProtocolo_Click(object sender, EventArgs e)
        {

            GetServicioFk(0);
            GetCategoriaFk(0);
            GetProtocolos(0, 0);
            GetGrdChBoxTasksConx(0,0);
            //desactivar los checkbox
            DesactivarCheckBoxes(grdTaskConx, false);

            ddlAseguradora.SelectedValue = "0";
            ddlServicio.SelectedValue = "-1";
            ddlProtocolos.SelectedValue = "0";

            BtnGrabarProts.Visible = false;

            grdProtConx.Visible = false;

            BtnModalAddTask.Enabled = false;
            BtnSaveGrdTask.Visible = false;
            BtnEditGrdTask.Visible = true;
            BtnEditGrdTask.Enabled = false;

            MostrarPanel(false);
        }

        protected void BtnAddProtocolo_Click(object sender, EventArgs e)
        {

            Insert_ITM_55();

            //GetServicioFk(0);
            //GetCategoriaFk(0);
            //GetProtocolos(0);

            ddlAseguradora.SelectedValue = "0";
            ddlServicio.SelectedValue = "0";
            ddlProtocolos.SelectedValue = "0";

            MostrarPanel(true);
        }

        protected void btnMostrarPanel_Click(object sender, EventArgs e)
        {
            GetProSLA(0);
            GetServicioFk(0);
            GetCategoriaFk(0);
            
            BtnPnlAddProtocolos.Enabled = false;

            //inicializar ddlAseAddProt
            ddlAseguradoraAddProt.SelectedValue = "0";
            ddlServicioAddProt.SelectedValue = "-1";
            ddlCategoriaAddProt.SelectedValue = "-1";

            ddlAseguradoraAddProt.Enabled = true;
            ddlServicioAddProt.Enabled = true;
            ddlCategoriaAddProt.Enabled = true;

            MostrarPanel(true);
        }

        private void MostrarPanel(bool mostrarAgregarProtocolo)
        {
            PanelAgregarProtocolo.Visible = mostrarAgregarProtocolo;
            MainPanel.Visible = !mostrarAgregarProtocolo;
        }

        protected void GrdProtocolos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdProtocolos.PageIndex = e.NewPageIndex;
                GetProtocolos(0, 0);
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdProtocolos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdProtocolos_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdProtocolos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdProtocolos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdProtocolos, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Width = Unit.Pixel(200);     // Aseguradora
                e.Row.Cells[2].Width = Unit.Pixel(850);     // Nom Protocolo
                e.Row.Cells[3].Width = Unit.Pixel(50);      // Eliminar
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;    // IdSLA
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Visible = false;    // IdSLA
            }
        }

        public void Insert_ITM_55()
        {
            try
            {
                /*
                if (TxtSLA_Protocolo.Text == "" || TxtSLA_Protocolo.Text == null)
                {
                    LblMessage.Text = "Capturar Nombre del Protocolo (SLA)";
                    mpeMensaje.Show();
                    return;
                }
                */
                int iIdSLAMax = GetIdSLAMax();
                /*
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                // Insertar registro tabla (ITM_55)
                string strQuery = "INSERT INTO ITM_55 (IdSLA, NomProtocolo, IdStatus) " +
                                  "VALUES (" + iIdSLAMax + ", '" + TxtSLA_Protocolo.Text.Trim() + "', " +
                                  " 1)" + "\n \n";

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();
                */
                GetGrdChBoxAddProtocolos();
                //mpeNewsProtocolos.Show();
                //LblMessage.Text = "Se agrego Protocolo (SLA), correctamente";

                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Protocolo_Agregado").ToString();
                mpeMensaje.Show();

                // Inicializar Controles
                //TxtSLA_Protocolo.Text = string.Empty;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        public void GetProSLA(int IdEtapa)
        {
            try
            {

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                string IdSeguros = string.Empty;

                // Consulta a las tablas : Protocolos (SLA) = ITM_55
                string strQuery = "SELECT p.IdSLA, e.IdSeguros AS Aseguradora, p.NomProtocolo " +
                                  "  FROM ITM_55 p " +
                             " INNER JOIN ITM_67 e " +
                                  "    ON p.IdAseguradora_fk = e.IdOrden " +
                                  " WHERE p.IdStatus = 1 " +
                                  "   AND e.IdStatus = 1 " +
                                  "   AND p.IdAseguradora_fk = " + IdEtapa +
                                  " ORDER BY IdSLA ; " ;

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                if (dt.Rows.Count == 0)
                {
                    GrdProtocolos.ShowHeaderWhenEmpty = true;
                    GrdProtocolos.EmptyDataText = GetGlobalResourceObject("GlobalResources", "msg_NoResults").ToString();

                    //GrdProtocolos.EmptyDataText = "No hay resultados.";
                }

                GrdProtocolos.DataSource = dt;
                GrdProtocolos.DataBind();

                dbConn.Close();

                //* * Agrega THEAD y TBODY a GridView.
                GrdProtocolos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ImgEliminarSLA_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
            int index = row.RowIndex;

            Variables.wRenglon = row.RowIndex;
            Variables.wEliminar = 1;

            BtnAceptar.Visible = true;
            BtnCancelar.Visible = true;
            BtnCerrar.Visible = false;

            LblMessage_1.Text = "¿Desea eliminar el protocolo?";
            mpeMensaje_1.Show();
        }

        protected void GrdTasks_PreRender(object sender, EventArgs e)
        {

        }

        protected void GrdTasks_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdTasks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GrdTasks.PageIndex = e.NewPageIndex;
                GetTareas(Convert.ToInt32(ddlProtocolos.SelectedValue));
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdTasks_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GrdTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdTasks, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = Unit.Pixel(1000);    // NomTarea
                e.Row.Cells[3].Width = Unit.Pixel(400);     // Plazo
                e.Row.Cells[4].Width = Unit.Pixel(400);     // Descripcion
                e.Row.Cells[5].Width = Unit.Pixel(100);     // TpoInterno
                e.Row.Cells[7].Width = Unit.Pixel(50);      // Editar
                e.Row.Cells[8].Width = Unit.Pixel(50);      // Eliminar

            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;    // IdTarea
                e.Row.Cells[2].Visible = false;    // DocInterno
                e.Row.Cells[6].Visible = false;    // Unidad de Tiempo
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;    // IdTarea
                e.Row.Cells[2].Visible = false;    // DocInterno
                e.Row.Cells[6].Visible = false;    // Unidad de Tiempo
            }
        }

        protected void ddlUnidadTiempo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdChBoxAddProtocolos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;    // IdDocumento
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;    // IdDocumento
            }
        }

        protected void GetGrdChBoxAddProtocolos()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                //string IdTpoAsunto = (string)Session["IdTpoAsunto"];
                //string IdCliente = (string)Session["IdCliente"];

                string strQuery = " SELECT IdDocumento, Descripcion " +
                                    " FROM itm_83 " +
                                    " WHERE IdStatus = 1; ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);


                if (dt.Rows.Count == 0)
                {
                    grdChBoxAddProtocolos.ShowHeaderWhenEmpty = true;
                    grdChBoxAddProtocolos.EmptyDataText = GetGlobalResourceObject("GlobalResources", "msg_NoResults").ToString();

                    //grdChBoxAddProtocolos.EmptyDataText = "No hay resultados.";
                }

                grdChBoxAddProtocolos.DataSource = dt;
                grdChBoxAddProtocolos.DataBind();
                
                dbConn.Close();

                //Agrega THEAD y TBODY a GridView.
                //grdChBoxAddProtocolos.HeaderRow.TableSection = TableRowSection.TableHeader;
                grdChBoxAddProtocolos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        protected void GetGrdChBoxProtocolosConx(int iIdProyecto, int iIdCategoria)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                //string IdTpoAsunto = (string)Session["IdTpoAsunto"];
                //string IdCliente = (string)Session["IdCliente"];

                string strQuery = " WITH CTE AS ( SELECT ITM_83.Descripcion AS Descripcion_83, " +
                                   "  ROW_NUMBER() OVER (ORDER BY ITM_97.IdOrden) AS RowNumber " +
                                   " FROM ITM_83 " +
                                   " INNER JOIN ITM_97 ON ITM_83.IdDocumento = ITM_97.IdEtapa_fk " +
                                   " WHERE ITM_83.IdStatus = 1 AND ITM_97.IdStatus = 1 " +
                                   "  AND ITM_97.IdAseguradora_fk = " + Convert.ToInt32(ddlAseguradoraAddProt.SelectedValue) +
                                   "  AND ITM_97.IdServicio_fk = " + iIdProyecto +
                                   "  AND ITM_97.IdCategoria_fk = " + iIdCategoria + " )," +
                                   "      Protocolos AS ( SELECT IdEtapa_fk, bSeleccion, " +
                                   "  ROW_NUMBER() OVER (ORDER BY IdOrden) AS RowNumber " +
                                   " FROM ITM_97 " +
                                   " WHERE IdStatus = 1 " +
                                         " AND IdCategoria_fk = " + iIdCategoria +
                                         " AND IdServicio_fk = " + iIdProyecto +
                                         " AND IdAseguradora_fk = " + Convert.ToInt32(ddlAseguradoraAddProt.SelectedValue) + " ) " +
                                   " SELECT COALESCE(CTE1.Descripcion_83, '') AS Columna1, " +
                                   "        COALESCE(P1.bSeleccion, 0) AS ChBoxSeccion_1, " +
                                   "        COALESCE(CTE2.Descripcion_83, '') AS Columna2, " +
                                   "        COALESCE(P2.bSeleccion, 0) AS ChBoxSeccion_2, " +
                                   "        COALESCE(CTE3.Descripcion_83, '') AS Columna3, " +
                                   "        COALESCE(P3.bSeleccion, 0) AS ChBoxSeccion_3 " +
                                   "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                        " LEFT JOIN Protocolos P1 ON CTE1.RowNumber = P1.RowNumber " +
                                        " LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2" +
                                               " ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                        " LEFT JOIN Protocolos P2 ON CTE2.RowNumber = P2.RowNumber " +
                                        " LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3" +
                                               " ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                        " LEFT JOIN Protocolos P3 ON CTE3.RowNumber = P3.RowNumber " +
                                   "  ORDER BY P3.RowNumber; ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                dbConn.Close();

                if (dt.Rows.Count == 0)
                {
                    grdProtConx.ShowHeaderWhenEmpty = true;
                    grdProtConx.EmptyDataText = GetGlobalResourceObject("GlobalResources", "msg_NoResults").ToString();

                    //grdProtConx.EmptyDataText = "No hay resultados.";
                }

                grdProtConx.DataSource = dt;
                grdProtConx.DataBind();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
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

        protected void grdChBoxAddProtocolos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Quitar")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());

                // Aquí puedes acceder a la fila específica que fue seleccionada y ejecutar tu función
                GridViewRow row = grdChBoxAddProtocolos.Rows[rowIndex];
                int IdProtocolo = int.Parse(row.Cells[1].Text);

                // Ejecutar tu función aquí
                Del_tbConxProtocolos(IdProtocolo); //pulir esta funcion

                // Renumerar indice tabla ITM_86
                // Renumerar_ITM_86();

                // Inicializar_IdDoc(IdDocumento);
                GetGrdChBoxProtocolosConx(Convert.ToInt32(ddlServicioAddProt.SelectedValue),
                                          Convert.ToInt32(ddlCategoriaAddProt.SelectedValue));
                // GetDatosSeccion(grdSeccion_1, "ChBoxSeccion_1", Variables.wSeccion);
                
            }
        }

        protected void Del_tbConxProtocolos(int iIdProtocolo)
        {
            //cambiar el nombre iIdDocumento por iIdProtocolo

            int iIdAseguradora = Convert.ToInt32(ddlAseguradoraAddProt.SelectedValue);
            int iIdServicio = Convert.ToInt32(ddlServicioAddProt.SelectedValue);
            int iIdCategoria = Convert.ToInt32(ddlCategoriaAddProt.SelectedValue);
            //int iIdProtocolo

            try
            {
                
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                /*
                string strQuery = "UPDATE web_peacock.ITM_97 " +
                                  " SET IdStatus = 0 " +
                                  "  WHERE IdStatus = 1 " +
                                  "   AND IdAseguradora_fk = " + iIdAseguradora + " " +
                                  "   AND IdServicio_fk = " + iIdServicio + " " +
                                  "   AND IdCategoria_fk =  " + iIdCategoria + " " +
                                  "   AND IdEtapa_fk = " + iIdProtocolo + " ;";
                */

                string strQuery = " DELETE FROM ITM_97 " +
                                  "  WHERE IdStatus = 1 " +
                                  "   AND IdAseguradora_fk = " + iIdAseguradora +
                                  "   AND IdServicio_fk = " + iIdServicio +
                                  "   AND IdCategoria_fk =  " + iIdCategoria +
                                  "   AND IdEtapa_fk = " + iIdProtocolo + " ;";

                strQuery += Environment.NewLine;

                strQuery += "  WITH Renumbered AS ( " +
                            " SELECT IdOrden, ROW_NUMBER() OVER(ORDER BY IdOrden) AS NewOrden " +
                            "  FROM ITM_97 " +
                            " WHERE IdAseguradora_fk = " + iIdAseguradora +
                            "   AND IdServicio_fk = " + iIdServicio +
                            "   AND IdCategoria_fk =  " + iIdCategoria +
                            "   AND IdStatus = 1 ) " +
                            " UPDATE ITM_97 SET IdOrden = ( SELECT Renumbered.NewOrden FROM Renumbered WHERE Renumbered.IdOrden = ITM_97.IdOrden ) " +
                            " WHERE ITM_97.IdAseguradora_fk = " + iIdAseguradora +
                            "  AND ITM_97.IdServicio_fk = " + iIdServicio +
                            "  AND ITM_97.AND IdCategoria_fk =  " + iIdCategoria +
                            "  AND ITM_97.IdStatus = 1 ; ";

                


                // Insert en la tabla Estado de Documento
                MySqlDataReader dr = dbConn.ExecuteReaderQuery(strQuery);

                dbConn.Close();

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //SqlDataReader dr = cmd.ExecuteReader();

                //Conecta.Cerrar();

                //cmd.Dispose();
                //dr.Dispose();

            }

            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void btnAgregarProts_Click(object sender, EventArgs e)
        {
            

            int Envio_ok = AddProtocolo_ITM_97();
            if (Envio_ok == 0)
            {
                //funcion para GetProtocolos de la Grid con ChekBox
                GetGrdChBoxProtocolosConx(Convert.ToInt32(ddlServicioAddProt.SelectedValue),
                                          Convert.ToInt32(ddlCategoriaAddProt.SelectedValue));
            }
        }

        public int AddProtocolo_ITM_97()
        {
            try
            {
                int rowsAffected = 0;
                int iIdAseguradora = Convert.ToInt32(ddlAseguradoraAddProt.SelectedValue);
                int iIdServicio = Convert.ToInt32(ddlServicioAddProt.SelectedValue);
                int iIdCategoria = Convert.ToInt32(ddlCategoriaAddProt.SelectedValue);
                string strQuery = string.Empty;

                foreach (GridViewRow row in grdChBoxAddProtocolos.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        var chkbox = row.FindControl("ChBoxRow") as CheckBox;

                        if (chkbox.Checked)
                        {
                            string sIdProtocolo = Server.HtmlEncode(Convert.ToString(row.Cells[1].Text));

                            int iIdUltiPosicion = UltiPosicion_ITM_97(); //funcion para el consecutivo de la tabla
                            int iIdUltiPosOrder = UltiPosicionOrden_ITM_97(iIdAseguradora,iIdServicio,iIdCategoria);
                            if (iIdUltiPosOrder > 24) { return -1; }

                            //agregar funcion para actualizar si existe

                            // Abrir la conexión
                            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                            dbConn.Open();

                            strQuery = "INSERT INTO ITM_97 (IdRelacion, IdAseguradora_fk, IdServicio_fk, IdCategoria_fk, " +
                                                          " IdEtapa_fk, IdOrden, bSeleccion, IdStatus) " +
                                       "SELECT @IdRelacion, @IdAseguradora_fk, @IdServicio_fk, @IdCategoria_fk, " +
                                             " @IdEtapa_fk, @IdOrden, 1, 1 " +
                                       // "  FROM DUAL " +
                                       " WHERE NOT EXISTS ( " +
                                       "SELECT 1 FROM ITM_97 " +
                                       " WHERE IdRelacion = @IdRelacion AND IdAseguradora_fk = @IdAseguradora_fk " +
                                       "   AND IdServicio_fk = @IdServicio_fk AND IdCategoria_fk = @IdCategoria_fk " +
                                       "   AND IdEtapa_fk = @IdEtapa_fk);";

                            // Crear y configurar el comando SQL
                            using (MySqlConnection conn = dbConn.Connection)
                            {
                                using (MySqlCommand cmd = new MySqlCommand(strQuery, conn))
                                {
                                    // Agregar parámetros para evitar inyección SQL y mejorar la legibilidad del código
                                    cmd.Parameters.AddWithValue("@IdRelacion", iIdUltiPosicion);
                                    cmd.Parameters.AddWithValue("@IdAseguradora_fk", iIdAseguradora);
                                    cmd.Parameters.AddWithValue("@IdServicio_fk", iIdServicio);
                                    cmd.Parameters.AddWithValue("@IdCategoria_fk", iIdCategoria);
                                    cmd.Parameters.AddWithValue("@IdEtapa_fk", sIdProtocolo);
                                    cmd.Parameters.AddWithValue("@IdOrden", iIdUltiPosOrder);

                                    // Ejecutar el comando y obtener el número de filas afectadas
                                    rowsAffected = cmd.ExecuteNonQuery();

                                }

                                // La conexión se cerrará automáticamente al salir del bloque using
                                // dbConn.Close();
                            }

                        }
                    }
                }

                if (rowsAffected > 0)
                {
                    //mpeNewsProtocolos.Hide();
                    //LblMessage.Text = "Se agrego categoría, correctamente";

                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Categoria_Agregada").ToString();
                    mpeMensaje.Show();
                }

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

        public int UltiPosicion_ITM_97 () {
            int iMaxPosicion = 0;
            
            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = " SELECT COALESCE(MAX(IdRelacion), 0) + 1 IdRelacion " +
                                " FROM ITM_97 ;";

            MySqlDataReader dr = dbConn.ExecuteReaderQuery(strQuery);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    iMaxPosicion = Convert.ToInt32(dr["IdRelacion"].ToString().Trim());
                }
            }

            return iMaxPosicion;
        
        }

        public int UltiPosicionOrden_ITM_97(int idAseguradora, int idServicio, int idCategoria)
        {
            int iMaxPosicionOrden = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = " SELECT COALESCE(MAX(IdOrden), 0) + 1 IdOrden " +
                                " FROM ITM_97 " +
                                " WHERE IdAseguradora_fk = " + idAseguradora +
                                "  AND IdServicio_fk = " + idServicio +
                                "  AND IdCategoria_fk = " + idCategoria +
                                "  AND IdStatus = 1 ;";

            MySqlDataReader dr = dbConn.ExecuteReaderQuery(strQuery);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    iMaxPosicionOrden = Convert.ToInt32(dr["IdOrden"].ToString().Trim());
                }
            }

            return iMaxPosicionOrden;

        }

        protected void grdProtConx_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Encuentra los CheckBoxes dentro de la fila
                CheckBox ChBoxSeccion_1 = (CheckBox)e.Row.FindControl("ChBoxSeccion_1");
                CheckBox chBoxSeccion_2 = (CheckBox)e.Row.FindControl("ChBoxSeccion_2");
                CheckBox chBoxSeccion_3 = (CheckBox)e.Row.FindControl("ChBoxSeccion_3");

                // Obtén los datos de la fila actual
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                // Obtén los valores de las columnas y conviértelos a tipo entero
                string valorColumna1 = Convert.ToString(rowView["Columna1"]);
                string valorColumna2 = Convert.ToString(rowView["Columna2"]);
                string valorColumna3 = Convert.ToString(rowView["Columna3"]);

                // Obtén el valor de la columna Columna1
                int valorChBoxSeccion_1 = Convert.ToInt32(rowView["ChBoxSeccion_1"]);
                int valorChBoxSeccion_2 = Convert.ToInt32(rowView["ChBoxSeccion_2"]);
                int valorChBoxSeccion_3 = Convert.ToInt32(rowView["ChBoxSeccion_3"]);

                // Verifica si la columna Columna1 tiene datos y muestra u oculta el CheckBox
                if (!string.IsNullOrEmpty(valorColumna1))
                {
                    ChBoxSeccion_1.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    ChBoxSeccion_1.Checked = valorChBoxSeccion_1 == 1;
                }
                else
                {
                    ChBoxSeccion_1.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna2))
                {
                    chBoxSeccion_2.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_2.Checked = valorChBoxSeccion_2 == 1;
                }
                else
                {
                    chBoxSeccion_2.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna3))
                {
                    chBoxSeccion_3.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_3.Checked = valorChBoxSeccion_3 == 1;
                }
                else
                {
                    chBoxSeccion_3.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
            }
        }

        protected void BtnPnlAddProtocolos_Click(object sender, EventArgs e)
        {
            if (ddlAseguradoraAddProt.SelectedValue == "0")
            {
                //LblMessage.Text = "Seleccionar Aseguradora";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Aseguradora").ToString();
                //mpeNewsProtocolos.Hide();
                mpeMensaje.Show();
                return;
            }
            
            if (ddlServicioAddProt.SelectedValue == "-1")
            {
                //LblMessage.Text = "Seleccionar Proyecto";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Proyecto").ToString();
                mpeMensaje.Show();
                return;
            }
            
            if (ddlCategoriaAddProt.SelectedValue == "-1")
            {
                //LblMessage.Text = "Seleccionar tipo  de asunto";
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Tipo_Asunto").ToString();
                mpeMensaje.Show();
                return;
            }
            
            GetGrdChBoxAddProtocolos();
            mpeNewsProtocolos.Show();
        }

        protected void BtnGrabarProts_Click(object sender, EventArgs e)
        {
            BtnEditarConxProt.Visible = true;
            BtnGrabarProts.Visible = false;
            BtnPnlAddProtocolos.Enabled = false;
            BtnModalOrdenarProtocolo.Enabled = false;
            BtnCancelProtocolo.Enabled = true;

            ddlAseguradoraAddProt.Enabled = true;
            ddlServicioAddProt.Enabled = true;
            ddlCategoriaAddProt.Enabled = true;
            //actualizar la tabla ITM_97 los valores de CheckBox seleccionados
            Obtener_Valores_ChBox(grdProtConx, "ChBoxSeccion");
            //desactivar los checkbox
            DesactivarCheckBoxes(grdProtConx, false);
        }

        protected void BtnEditarConxProt_Click(object sender, EventArgs e)
        {
            if (ddlAseguradoraAddProt.SelectedValue == "0")
            {
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Aseguradora").ToString();
                //mpeNewsProtocolos.Hide();
                mpeMensaje.Show();
                return;
            }
            
            if (ddlServicioAddProt.SelectedValue == "-1")
            {
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Proyecto").ToString(); ;
                mpeMensaje.Show();
                return;
            }
            
            if (ddlCategoriaAddProt.SelectedValue == "-1")
            {
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Tipo_Asunto").ToString(); ;
                mpeMensaje.Show();
                return;
            }
            
            BtnEditarConxProt.Visible = false;
            BtnGrabarProts.Visible = true;
            BtnModalOrdenarProtocolo.Enabled = true;
            BtnCancelProtocolo.Enabled = false;
            
            ddlAseguradoraAddProt.Enabled = false;
            ddlServicioAddProt.Enabled = false;
            ddlCategoriaAddProt.Enabled = false;
            //desactivar el boton que saca el modal
            BtnPnlAddProtocolos.Enabled = true;
            //desactivar los ChecksBox de la Grid
            DesactivarCheckBoxes(grdProtConx, true);
        }

        protected void Obtener_Valores_ChBox(GridView gridView, string checkBoxID)
        {
            int iIdAseguradora = Convert.ToInt32(ddlAseguradoraAddProt.SelectedValue);
            int iIdServicio = Convert.ToInt32(ddlServicioAddProt.SelectedValue);
            int iIdCategoria = Convert.ToInt32(ddlCategoriaAddProt.SelectedValue);
            int iIdProtocolo = Convert.ToInt32(ddlProtocolos.SelectedValue);
            int iIdTaskPadre = Convert.ToInt32(ddlTaskPadre.SelectedValue);
            string sIdTaskQuery = string.Empty;
            //if (iIdServicio == -1)
            //{
            //    iIdServicio = 0;
            //}
            if (iIdTaskPadre == 0)
            {
                sIdTaskQuery = "IS NULL";
            }
            else
            {
                sIdTaskQuery = " = " + iIdTaskPadre;
            }
            
            bool[] valoresDocs = new bool[24];      // Creamos un array de booleanos para almacenar los valores de los documentos

            // Iterar sobre las filas de la GridView
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                // Obtener los CheckBoxes de cada fila
                GridViewRow row = gridView.Rows[i];

                CheckBox chkBox1 = (CheckBox)row.FindControl(checkBoxID + "_1");
                CheckBox chkBox2 = (CheckBox)row.FindControl(checkBoxID + "_2");
                CheckBox chkBox3 = (CheckBox)row.FindControl(checkBoxID + "_3");

                // Determinar las columnas correspondientes en la base de datos
                int columnaBaseDatos = i / 8;   // Cada conjunto de 6 filas corresponde a una columna en la base de datos
                int offset = i % 8;             // Determina el desplazamiento dentro de cada conjunto de 6 filas

                // Almacenar los valores de los CheckBoxes en las columnas de la base de datos correspondientes
                valoresDocs[columnaBaseDatos * 8 + offset] = chkBox1.Checked;
                valoresDocs[columnaBaseDatos * 8 + offset + 8] = chkBox2.Checked;
                valoresDocs[columnaBaseDatos * 8 + offset + 16] = chkBox3.Checked;

                // Almacenar los valores de los CheckBoxes en el array
                // valoresDocs[i * 3] = chkBox1.Checked;
                // valoresDocs[i * 3 + 1] = chkBox2.Checked;
                // valoresDocs[i * 3 + 2] = chkBox3.Checked;
            }

            if (gridView == grdProtConx)
            {
                ActualizarDatosEnTabla(iIdAseguradora, iIdServicio, iIdCategoria, valoresDocs);
            }
            if(gridView == grdTaskConx)
            {
                ActualizarDatosEnTabla98(iIdProtocolo, valoresDocs, sIdTaskQuery);
            }
            
        }

        private void ActualizarDatosEnTabla(int idAseguradora, int idServicio, int idCategoria, bool[] valoresDocs)
        {
            try
            {

                //ConexionBD Conecta = new ConexionBD();
                //Conecta.Abrir();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                StringBuilder posiciones = new StringBuilder(); // StringBuilder para construir la cadena

                // Recorremos el arreglo para encontrar los valores true y sus posiciones
                for (int i = 0; i < valoresDocs.Length; i++)
                {
                    if (valoresDocs[i]) // Si el valor en esta posición es true
                    {
                        if (posiciones.Length > 0)
                        {
                            posiciones.Append(", ");    // Agrega una coma y un espacio si ya hay elementos en la cadena
                        }
                        posiciones.Append(i + 1);       // Agrega la posición a la cadena
                    }
                }

                // Inicializar datos tabla ITM_86

                string strQuery = $@"UPDATE ITM_97 
                                            SET bSeleccion = 0 
                                          WHERE IdAseguradora_fk = { idAseguradora }
                                            AND IdServicio_fk = { idServicio }
                                            AND IdCategoria_fk = { idCategoria }
                                            AND IdStatus = 1 ;";

                if (posiciones.Length != 0)
                {

                    string posicionesStr = $"({posiciones})";

                    //int idPosicion = 1;

                    //// Generar la consulta SQL UPDATE
                    ///
                    //string strQuery = $@"UPDATE ITM_86 
                    //                        SET bSeleccion = 0 
                    //                      WHERE [IdProyecto] = { idProyecto }
                    //                        AND [IdCliente] = '{ idCliente }'
                    //                        AND [IdSeccion] = { idSeccion }
                    //                    AND [IdTpoAsunto] = { idTpoAsunto } ";

                    strQuery += Environment.NewLine;

                    strQuery += $@"UPDATE ITM_97 
                                      SET bSeleccion = 1 
                                    WHERE IdOrden IN { posicionesStr}
                                      AND IdAseguradora_fk = { idAseguradora }
                                      AND IdServicio_fk = { idServicio }
                                      AND IdCategoria_fk = { idCategoria }
                                      AND IdStatus = 1 ";

                    //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                    //int affectedRows = cmd.ExecuteNonQuery();

                }

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //int affectedRows = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        private void ActualizarDatosEnTabla98(int idRelacion, bool[] valoresDocs, string sIdTaskQuery)
        {
            try
            {

                //ConexionBD Conecta = new ConexionBD();
                //Conecta.Abrir();

                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                StringBuilder posiciones = new StringBuilder(); // StringBuilder para construir la cadena

                // Recorremos el arreglo para encontrar los valores true y sus posiciones
                for (int i = 0; i < valoresDocs.Length; i++)
                {
                    if (valoresDocs[i]) // Si el valor en esta posición es true
                    {
                        if (posiciones.Length > 0)
                        {
                            posiciones.Append(", ");    // Agrega una coma y un espacio si ya hay elementos en la cadena
                        }
                        posiciones.Append(i + 1);       // Agrega la posición a la cadena
                    }
                }

                // Inicializar datos tabla ITM_86

                string strQuery = $@"UPDATE ITM_98 
                                            SET bSeleccion = 0 
                                          WHERE IdRelacion = { idRelacion }
                                            AND IdTarea_fk {sIdTaskQuery}
                                            AND IdStatus = 1 ;";

                if (posiciones.Length != 0)
                {

                    string posicionesStr = $"({posiciones})";

                    //int idPosicion = 1;

                    //// Generar la consulta SQL UPDATE
                    ///
                    //string strQuery = $@"UPDATE ITM_86 
                    //                        SET bSeleccion = 0 
                    //                      WHERE [IdProyecto] = { idProyecto }
                    //                        AND [IdCliente] = '{ idCliente }'
                    //                        AND [IdSeccion] = { idSeccion }
                    //                    AND [IdTpoAsunto] = { idTpoAsunto } ";

                    strQuery += Environment.NewLine;

                    strQuery += $@"UPDATE ITM_98 
                                      SET bSeleccion = 1 
                                    WHERE IdOrden IN { posicionesStr}
                                      AND IdRelacion = { idRelacion }
                                      AND IdTarea_fk {sIdTaskQuery}
                                      AND IdStatus = 1 ";

                    //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                    //int affectedRows = cmd.ExecuteNonQuery();

                }

                int affectedRows = dbConn.ExecuteNonQuery(strQuery);

                dbConn.Close();

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //int affectedRows = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void ddlUnidTimeSleep_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnModalAddTask_Click(object sender, EventArgs e)
        {
            if (ddlAseguradora.SelectedValue == "0")
            {
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Aseguradora").ToString();
                //mpeNewsProtocolos.Hide();
                mpeMensaje.Show();
                return;
            }

            if (ddlServicio.SelectedValue == "-1")
            {
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Proyecto").ToString(); ;
                mpeMensaje.Show();
                return;
            }

            if (ddlCategoria.SelectedValue == "-1")
            {
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Tipo_Asunto").ToString();
                mpeMensaje.Show();
                return;
            }

            GetGrdChkBxPnlAddTask();
            mpeNewsTask.Show();
        }

        protected void grdChkBxPnlAddTask_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //aqui va la funcion de Quitar de la grd de conexion
            if (e.CommandName == "Quitar")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());

                // Aquí puedes acceder a la fila específica que fue seleccionada y ejecutar tu función
                GridViewRow row = grdChkBxPnlAddTask.Rows[rowIndex];
                int IdTarea = int.Parse(row.Cells[1].Text);

                int iIdTask_fk = Convert.ToInt32(ddlTaskPadre.SelectedValue);
                
                // Ejecutar tu función aquí
                Del_tbConxTasks(IdTarea, iIdTask_fk); //pulir esta funcion

                // Renumerar indice tabla ITM_86
                // Renumerar_ITM_86();

                // Inicializar_IdDoc(IdDocumento);
                GetGrdChBoxTasksConx(Convert.ToInt32(ddlProtocolos.SelectedValue), iIdTask_fk);
                // GetDatosSeccion(grdSeccion_1, "ChBoxSeccion_1", Variables.wSeccion);

            }
        }

        protected void Del_tbConxTasks(int IdTarea, int iIdTaskPadre)
        {
            //cambiar el nombre iIdDocumento por iIdProtocolo

            int iIdProtocolo = Convert.ToInt32(ddlProtocolos.SelectedValue);

            try
            {
                string sIdTaskQuery = string.Empty;

                if (iIdTaskPadre == 0)
                {
                    sIdTaskQuery = "IS NULL";
                }
                else
                {
                    sIdTaskQuery = " = " + iIdTaskPadre;
                }
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                /*
                string strQuery = "UPDATE web_peacock.ITM_98 " +
                                  " SET IdStatus = 0 " +
                                  "  WHERE IdStatus = 1 " +
                                  "   AND IdAseguradora_fk = " + iIdAseguradora + " " +
                                  "   AND IdServicio_fk = " + iIdServicio + " " +
                                  "   AND IdCategoria_fk =  " + iIdCategoria + " " +
                                  "   AND IdEtapa_fk = " + iIdProtocolo + " ;";
                */

                string strQuery = " DELETE FROM ITM_98 " +
                                  "  WHERE IdStatus = 1 " +
                                  "   AND IdRelacion = " + iIdProtocolo +
                                  "   AND IdTarea = " + IdTarea + 
                                  "   AND IdTarea_fk "+ sIdTaskQuery +" ;";

                strQuery += Environment.NewLine;

                strQuery += "  WITH Renumbered AS ( " +
                            "   SELECT IdOrden, ROW_NUMBER() OVER (ORDER BY IdOrden) AS NewId " +
                            "     FROM ITM_98 " +
                            "    WHERE IdRelacion = " + iIdProtocolo +
                            "      AND IdStatus = 1 " +
                            "      AND IdTarea_fk "+ sIdTaskQuery +" ) " +
                            " UPDATE ITM_98 SET IdOrden = ( " +
                            "   SELECT Renumbered.NewId " +
                            "     FROM Renumbered " +
                            "    WHERE Renumbered.IdOrden = ITM_98.IdOrden ) " +
                            " WHERE ITM_98.IdRelacion = " + iIdProtocolo +
                            "  AND ITM_98.IdStatus = 1  " +
                            "  AND ITM_98.IdTarea_fk "+ sIdTaskQuery +" ; ";

                // Insert en la tabla Estado de Documento
                MySqlDataReader dr = dbConn.ExecuteReaderQuery(strQuery);

                dbConn.Close();

                //SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

                //SqlDataReader dr = cmd.ExecuteReader();

                //Conecta.Cerrar();

                //cmd.Dispose();
                //dr.Dispose();

            }

            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void grdChkBxPnlAddTask_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;    // IdTarea
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Visible = false;    // IdTarea
            }
        }

        protected void GetGrdChkBxPnlAddTask()
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                //string IdTpoAsunto = (string)Session["IdTpoAsunto"];
                //string IdCliente = (string)Session["IdCliente"];

                string strQuery = " SELECT b.IdTarea, b.NomTarea, b.Plazo, a.Descripcion, " +
                                  "   CASE WHEN b.DocInterno = 1 THEN 'INTERNO' ELSE '' END AS TpoInterno, " +
                                  "   CASE WHEN b.TaskAutomatica = 1 THEN 'AUTOMATICA' ELSE '' END AS TskAuto, " +
                                  "      b.UnidadTiempo " +
                                    " FROM ITM_50 AS a INNER JOIN ITM_54 AS b " +
                                    "   ON a.IdUnidadTiempo = b.UnidadTiempo " +
                                    " WHERE b.IdStatus = 1 AND a.IdStatus = 1 ORDER BY b.IdTarea ASC; ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                dbConn.Close();

                if (dt.Rows.Count == 0)
                {
                    grdProtConx.ShowHeaderWhenEmpty = true;
                    grdProtConx.EmptyDataText = GetGlobalResourceObject("GlobalResources", "msg_NoResults").ToString();

                    //grdChkBxPnlAddTask.EmptyDataText = "No hay resultados.";
                }

                grdChkBxPnlAddTask.DataSource = dt;
                grdChkBxPnlAddTask.DataBind();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void grdTaskConx_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Encuentra los CheckBoxes dentro de la fila
                CheckBox ChBoxSeccion_1 = (CheckBox)e.Row.FindControl("ChBoxSeccion_1");
                CheckBox chBoxSeccion_2 = (CheckBox)e.Row.FindControl("ChBoxSeccion_2");
                CheckBox chBoxSeccion_3 = (CheckBox)e.Row.FindControl("ChBoxSeccion_3");

                // Obtén los datos de la fila actual
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                // Obtén los valores de las columnas y conviértelos a tipo entero
                string valorColumna1 = Convert.ToString(rowView["Columna1"]);
                string valorColumna2 = Convert.ToString(rowView["Columna2"]);
                string valorColumna3 = Convert.ToString(rowView["Columna3"]);

                // Obtén el valor de la columna Columna1
                int valorChBoxSeccion_1 = Convert.ToInt32(rowView["ChBoxSeccion_1"]);
                int valorChBoxSeccion_2 = Convert.ToInt32(rowView["ChBoxSeccion_2"]);
                int valorChBoxSeccion_3 = Convert.ToInt32(rowView["ChBoxSeccion_3"]);

                // Verifica si la columna Columna1 tiene datos y muestra u oculta el CheckBox
                if (!string.IsNullOrEmpty(valorColumna1))
                {
                    ChBoxSeccion_1.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    ChBoxSeccion_1.Checked = valorChBoxSeccion_1 == 1;
                }
                else
                {
                    ChBoxSeccion_1.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna2))
                {
                    chBoxSeccion_2.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_2.Checked = valorChBoxSeccion_2 == 1;
                }
                else
                {
                    chBoxSeccion_2.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
                if (!string.IsNullOrEmpty(valorColumna3))
                {
                    chBoxSeccion_3.Visible = true; // Mostrar el CheckBox si Columna1 tiene datos
                    chBoxSeccion_3.Checked = valorChBoxSeccion_3 == 1;
                }
                else
                {
                    chBoxSeccion_3.Visible = false; // Ocultar el CheckBox si Columna1 está vacía
                }
            }
        }

        protected void BtnInsertConxTask_Click(object sender, EventArgs e)
        {
            //if al ddlProtocolos
            if(ddlProtocolos.SelectedValue == "0")
            {
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Protocolo").ToString();
                mpeMensaje.Show();
                return;
            }
            int  iIdTask_fk = Convert.ToInt32(ddlTaskPadre.SelectedValue);
            
            int Envio_ok = AddTask_ITM_98();
            if (Envio_ok == 0)
            {
                //funcion para GetTaskGrdConx de la Grid con ChekBox
                GetGrdChBoxTasksConx(Convert.ToInt32(ddlProtocolos.SelectedValue),iIdTask_fk);
                GetTaskddlDependencia(Convert.ToInt32(ddlProtocolos.SelectedValue));

                ddlTaskPadre.SelectedValue = Convert.ToString(iIdTask_fk);
            }
            
        }

        public int AddTask_ITM_98()
        {
            try
            {
                int rowsAffected = 0;
                int iIdSLARelacion = Convert.ToInt32(ddlProtocolos.SelectedValue);
                string strQuery = string.Empty;
                int iIdTaskPadre = Convert.ToInt32(ddlTaskPadre.SelectedValue);
                string sIdTaskQuery = string.Empty;
                string sQueryColum = string.Empty;
                string sQueryVar = string.Empty;

                if (iIdTaskPadre == 0)
                {
                    sIdTaskQuery = "IS NULL";
                    sQueryColum = " ";
                    sQueryVar = " ";
                }
                else
                {
                    sIdTaskQuery = " = " + iIdTaskPadre;
                    sQueryColum = " IdTarea_fk, " ;
                    sQueryVar = " @IdTarea_fk, ";
                }

                foreach (GridViewRow row in grdChkBxPnlAddTask.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        var chkbox = row.FindControl("ChBoxRow") as CheckBox;

                        if (chkbox.Checked)
                        {
                            string sIdTarea = Server.HtmlEncode(Convert.ToString(row.Cells[1].Text));

                            int iIdUltiPosicion = UltiPosicion_ITM_98(); //funcion para el consecutivo de la tabla
                            int iIdUltiPosOrder = UltiPosicionOrden_ITM_98(iIdSLARelacion, sIdTaskQuery);
                            if (iIdUltiPosOrder > 24) { return -1; }

                            //agregar funcion para actualizar si existe

                            // Abrir la conexión
                            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                            dbConn.Open();

                            strQuery = "INSERT INTO ITM_98 (IdRelaciones, IdRelacion, IdTarea, " +
                                                          " IdOrden, bSeleccion, " + sQueryColum + " IdStatus) " +
                                       "SELECT @IdRelaciones, @IdRelacion, @IdTarea,  @IdOrden, 1, " +
                                       sQueryVar + " 1 " +
                                       // "  FROM DUAL " +
                                       " WHERE NOT EXISTS ( " +
                                       "SELECT 1 FROM ITM_98 " +
                                       " WHERE IdRelacion = @IdRelacion " +
                                       "   AND IdTarea_fk " + sIdTaskQuery +
                                       "   AND IdTarea = @IdTarea );";

                            // Crear y configurar el comando SQL
                            using (MySqlConnection conn = dbConn.Connection)
                            {
                                using (MySqlCommand cmd = new MySqlCommand(strQuery, conn))
                                {
                                    // Agregar parámetros para evitar inyección SQL y mejorar la legibilidad del código
                                    cmd.Parameters.AddWithValue("@IdRelaciones", iIdUltiPosicion);
                                    cmd.Parameters.AddWithValue("@IdRelacion", iIdSLARelacion);
                                    cmd.Parameters.AddWithValue("@IdTarea", sIdTarea);
                                    cmd.Parameters.AddWithValue("@IdOrden", iIdUltiPosOrder);
                                    cmd.Parameters.AddWithValue("@IdTarea_fk", iIdTaskPadre);

                                    // Ejecutar el comando y obtener el número de filas afectadas
                                    rowsAffected = cmd.ExecuteNonQuery();

                                }

                                // La conexión se cerrará automáticamente al salir del bloque using
                                // dbConn.Close();
                            }

                        }
                    }
                }

                if (rowsAffected > 0)
                {
                    //mpeNewsProtocolos.Hide();
                    // LblMessage.Text = "Se agrego las Tareas, correctamente";
                    LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Tarea_Agregada").ToString();
                    mpeMensaje.Show();
                }

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

        public int UltiPosicion_ITM_98()
        {
            int iMaxPosicion = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = " SELECT COALESCE(MAX(IdRelaciones), 0) + 1 IdRelaciones " +
                                " FROM ITM_98 ;";

            MySqlDataReader dr = dbConn.ExecuteReaderQuery(strQuery);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    iMaxPosicion = Convert.ToInt32(dr["IdRelaciones"].ToString().Trim());
                }
            }

            return iMaxPosicion;

        }

        public int UltiPosicionOrden_ITM_98(int iIdSLARelacion, string sIdTaskQuery)
        {
            int iMaxPosicionOrden = 0;

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = " SELECT COALESCE(MAX(IdOrden), 0) + 1 IdOrden " +
                                " FROM ITM_98 " +
                                " WHERE IdRelacion = " + iIdSLARelacion +
                                "   AND IdTarea_fk " + sIdTaskQuery +
                                "   AND IdStatus = 1 ;";

            MySqlDataReader dr = dbConn.ExecuteReaderQuery(strQuery);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    iMaxPosicionOrden = Convert.ToInt32(dr["IdOrden"].ToString().Trim());
                }
            }

            return iMaxPosicionOrden;

        }

        protected void GetGrdChBoxTasksConx(int iIdProtocolo, int iIdTaskPadre )
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                //string IdTpoAsunto = (string)Session["IdTpoAsunto"];
                //string IdCliente = (string)Session["IdCliente"];
                string sIdTaskQuery = string.Empty;

                if (iIdTaskPadre == 0)
                {
                    sIdTaskQuery = "IS NULL";
                }
                else
                {
                    sIdTaskQuery = " = " + iIdTaskPadre;
                }

                string strQuery = " WITH CTE AS ( SELECT ITM_54.NomTarea AS Tarea, " +
                                   "  ROW_NUMBER() OVER (ORDER BY ITM_98.IdOrden) AS RowNumber " +
                                   " FROM ITM_54 " +
                                   " INNER JOIN web_peacock.ITM_98 ON ITM_54.IdTarea = ITM_98.IdTarea " +
                                   " WHERE ITM_54.IdStatus = 1 AND ITM_98.IdStatus = 1 " +
                                   "  AND ITM_98.IdRelacion = " + iIdProtocolo + " " +
                                   "  AND ITM_98.IdTarea_fk " + sIdTaskQuery + " )," +
                                   "      Tareas AS ( SELECT IdTarea, bSeleccion, " +
                                   "  ROW_NUMBER() OVER (ORDER BY IdOrden) AS RowNumber " +
                                   " FROM ITM_98 " +
                                   " WHERE IdStatus = 1 " +
                                         " AND IdRelacion = " + iIdProtocolo + " " +
                                         " AND IdTarea_fk " + sIdTaskQuery + " ) " +
                                   " SELECT COALESCE(CTE1.Tarea, '') AS Columna1, " +
                                   "        COALESCE(T1.bSeleccion, 0) AS ChBoxSeccion_1, " +
                                   "        COALESCE(CTE2.Tarea, '') AS Columna2, " +
                                   "        COALESCE(T2.bSeleccion, 0) AS ChBoxSeccion_2, " +
                                   "        COALESCE(CTE3.Tarea, '') AS Columna3, " +
                                   "        COALESCE(T3.bSeleccion, 0) AS ChBoxSeccion_3 " +
                                   "  FROM (SELECT * FROM CTE WHERE RowNumber BETWEEN 1 AND 8) AS CTE1 " +
                                        " LEFT JOIN Tareas T1 ON CTE1.RowNumber = T1.RowNumber " +
                                        " LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 9 AND 16) AS CTE2 " +
                                               " ON CTE1.RowNumber = CTE2.RowNumber - 8 " +
                                        " LEFT JOIN Tareas T2 ON CTE2.RowNumber = T2.RowNumber " +
                                        " LEFT JOIN (SELECT * FROM CTE WHERE RowNumber BETWEEN 17 AND 24) AS CTE3 " +
                                               " ON CTE1.RowNumber = CTE3.RowNumber - 16 " +
                                        " LEFT JOIN Tareas T3 ON CTE3.RowNumber = T3.RowNumber " +
                                   "  ORDER BY T3.RowNumber ; ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                dbConn.Close();

                if (dt.Rows.Count == 0)
                {
                    grdTaskConx.ShowHeaderWhenEmpty = true;
                    grdTaskConx.EmptyDataText = GetGlobalResourceObject("GlobalResources", "msg_NoResults").ToString();

                    //grdTaskConx.EmptyDataText = "No hay resultados.";
                }

                grdTaskConx.DataSource = dt;
                grdTaskConx.DataBind();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnEditGrdTask_Click(object sender, EventArgs e)
        {
            if (ddlAseguradora.SelectedValue == "0")
            {
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Aseguradora").ToString();
                //mpeNewsProtocolos.Hide();
                mpeMensaje.Show();
                return;
            }

            if (ddlServicio.SelectedValue == "-1")
            {
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Proyecto").ToString(); ;
                mpeMensaje.Show();
                return;
            }

            if (ddlCategoria.SelectedValue == "-1")
            {
                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Seleccionar_Tipo_Asunto").ToString();
                mpeMensaje.Show();
                return;
            }

            BtnModalAddTask.Enabled = true;
            BtnSaveGrdTask.Visible = true;
            BtnModalOrdenarTask.Enabled = true;
            btnMostrarPanel.Enabled = false;

            ddlAseguradora.Enabled = false;
            ddlServicio.Enabled = false;
            ddlCategoria.Enabled = false;
            ddlProtocolos.Enabled = false;
            ddlTaskPadre.Enabled = false;

            BtnEditGrdTask.Visible = false;
            DesactivarCheckBoxes(grdTaskConx, true);
        }

        protected void BtnSaveGrdTask_Click(object sender, EventArgs e)
        {
            BtnEditGrdTask.Visible = true;
            BtnEditGrdTask.Enabled = true;
            BtnSaveGrdTask.Visible = false;
            BtnModalAddTask.Enabled = false;
            btnMostrarPanel.Enabled = true;
            BtnModalOrdenarTask.Enabled = false;

            ddlAseguradora.Enabled = true;
            ddlServicio.Enabled = true;
            ddlCategoria.Enabled = true;
            ddlProtocolos.Enabled = true;
            ddlTaskPadre.Enabled = true;

            //actualizar la tabla ITM_98 los valores de CheckBox seleccionados
            Obtener_Valores_ChBox(grdTaskConx, "ChBoxSeccion");
            //desactivar los checkbox
            DesactivarCheckBoxes(grdTaskConx, false);
        }

        protected void BtnModalOrdenarTask_Click(object sender, EventArgs e)
        {
            //funcion Get para traer las tareas.
            GetTaskOrden();
            mpeOrderTask.Show();
        }

        protected void GetTaskOrden()
        {

            int iIdRelacion = Convert.ToInt32(ddlProtocolos.SelectedValue);
            int iIdTaskPadre = Convert.ToInt32(ddlTaskPadre.SelectedValue);
            string sIdTaskQuery = string.Empty;

            if (iIdTaskPadre == 0)
            {
                sIdTaskQuery = "IS NULL";
            }
            else
            {
                sIdTaskQuery = " = " + iIdTaskPadre;
            }
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                //string IdTpoAsunto = (string)Session["IdTpoAsunto"];
                //string IdCliente = (string)Session["IdCliente"];

                string strQuery = " SELECT r.IdRelaciones, t.NomTarea" +
                                  "   FROM ITM_98 r INNER JOIN ITM_54 t " +
                                  "     ON r.IdTarea = t.IdTarea " +
                                  "  WHERE r.IdRelacion = " + iIdRelacion +
                                  "    AND r.idTarea_fk " + sIdTaskQuery +
                                  "    AND r.IdStatus = 1 AND t.IdStatus = 1 " +
                                  "  ORDER BY r.IdOrden ASC; ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                dbConn.Close();

                //lvOrderTaskItem.Items.Clear();

                if (dt.Rows.Count == 0)
                {
                    //lvOrderTaskItem.Items.Clear();
                    //lvOrderTaskItem. = "No hay resultados.";
                    //lvOrderTaskItem.EmptyDataTemplate = "no hay Tareas.";
                }

                lvOrderTaskItem.DataSource = dt;

                //lvOrderTaskItem.DataValueField = "IdRelaciones";
                //lvOrderTaskItem.DataTextField = "NomTarea";

                //lvOrderTaskItem.DataBind();
                lvOrderTaskItem.DataBind();
                // Registrar script para inicializar Sortable en el cliente luego del render
                // Si usas UpdatePanel, usa ScriptManager.RegisterStartupScript
                ScriptManager.RegisterStartupScript(this, this.GetType(), "initSortable", "initSortable();", true);

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void btnPnlGuardarOrdenTask_Click(object sender, EventArgs e)
        {
            try
            {
                string strQuery = string.Empty;
                string jsonOrden = hdnOrdenTask.Value;
                if (string.IsNullOrWhiteSpace(jsonOrden))
                {
                    mpeOrderTask.Show(); return;
                }

                // Deserializar
                var lista = JsonConvert.DeserializeObject<List<ItemOrden>>(jsonOrden);
                // Abrir la conexión
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();
                // Actualizar BD (usa transacción)
                using (MySqlConnection conn = dbConn.Connection)
                {
                    //conn.Open();
                    /*
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            */
                            foreach (var it in lista)
                            {
                                strQuery = " UPDATE ITM_98 " +
                                                  " SET IdOrden = @orden " +
                                                  "WHERE IdRelaciones = @id;";
                                using (MySqlCommand cmd = new MySqlCommand(strQuery, conn/*, tran*/))
                                {
                                    cmd.Parameters.AddWithValue("@orden", it.pos);
                                    cmd.Parameters.AddWithValue("@id", it.id);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            /*
                            tran.Commit();
                        }
                        catch
                        {
                            tran.Rollback(); // si algo falla
                            throw;           // se va al catch externo
                        }
                    }*/
                }
                // Opcional: recargar la lista o cerrar modal
                // Aquí vuelvo a cargar para que el usuario vea el cambio si quieres
                GetTaskOrden();
                int iIdTask_fk = Convert.ToInt32(ddlTaskPadre.SelectedValue);

                GetTaskddlDependencia(Convert.ToInt32(ddlProtocolos.SelectedValue));
                GetGrdChBoxTasksConx(Convert.ToInt32(ddlProtocolos.SelectedValue), iIdTask_fk);

                ddlTaskPadre.SelectedValue = Convert.ToString(iIdTask_fk);

                ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "cerrarModal", "$find('" + mpeOrderTask.ClientID + "').hide();", true);

                //mpeOrderTask.Show(); // si quieres dejarlo abierto
                //LblMessage.Text = "Se agrego el orden de las Tareas, correctamente";

                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Orden_Tareas_Agregado").ToString();
                mpeMensaje.Show();
            }
            catch (Exception ex)
            {
                /*
                    tran.Rollback();
                    // loguea el error o muéstralo (no exponer mensajes técnicos en producción)
                    throw; 
                */
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }
                
        }
        public class ItemOrden { 
            public int id { get; set; } 
            public int pos { get; set; } 
        }

        protected void BtnModalOrdenarProtocolo_Click(object sender, EventArgs e)
        {
            //funcion para cargar los protocolos
            GetProtocolosOrden();
            //funcion para abrir el modal para ordenar los protocolos
            mpeOrderSLA.Show();
        }

        protected void GetProtocolosOrden()
        {

            int iIdAseguradora = Convert.ToInt32(ddlAseguradoraAddProt.SelectedValue);
            int iIdServicio = Convert.ToInt32(ddlServicioAddProt.SelectedValue);
            int iIdCategoria = Convert.ToInt32(ddlCategoriaAddProt.SelectedValue);
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                //string IdTpoAsunto = (string)Session["IdTpoAsunto"];
                //string IdCliente = (string)Session["IdCliente"];

                string strQuery = " SELECT r.IdRelacion, e.Descripcion " +
                                  "   FROM ITM_97 r INNER JOIN ITM_83 e " +
                                  "     ON r.IdEtapa_fk = e.IdDocumento " +
                                  "  WHERE r.IdAseguradora_fk = " + iIdAseguradora +
                                  "    AND r.IdServicio_fk    = " + iIdServicio +
                                  "    AND r.IdCategoria_fk   = " + iIdCategoria +
                                  /* " AND r.bSeleccion = 1 " +  */
                                  "    AND r.IdStatus   = 1 " +
                                  "    AND e.IdStatus   = 1 ORDER BY r.IdOrden ASC; ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                dbConn.Close();

                //lvOrderTaskItem.Items.Clear();

                if (dt.Rows.Count == 0)
                {
                    //lvOrderTaskItem.Items.Clear();
                    //lvOrderTaskItem. = "No hay resultados.";
                    //lvOrderTaskItem.EmptyDataTemplate = "no hay Tareas.";
                }

                lvOrderSLAItem.DataSource = dt;

                lvOrderSLAItem.DataBind();
                // Registrar script para inicializar Sortable en el cliente luego del render
                // Si usas UpdatePanel, usa ScriptManager.RegisterStartupScript
                ScriptManager.RegisterStartupScript(this, this.GetType(), "initSortableP", "initSortableP();", true);

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void btnPnlGuardarOrdenSLA_Click(object sender, EventArgs e)
        {
            try
            {
                string strQuery = string.Empty;
                string jsonOrden = hdnOrdenSLA.Value;
                if (string.IsNullOrWhiteSpace(jsonOrden))
                {
                    mpeOrderSLA.Show(); return;
                }

                // Deserializar
                var lista = JsonConvert.DeserializeObject<List<ItemOrden>>(jsonOrden);
                // Abrir la conexión
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();
                // Actualizar BD (usa transacción)
                using (MySqlConnection conn = dbConn.Connection)
                {
                    //conn.Open();
                    /*
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            */
                    foreach (var it in lista)
                    {
                        strQuery = " UPDATE ITM_97 " +
                                          " SET IdOrden = @orden " +
                                          "WHERE IdRelacion = @id;";
                        using (MySqlCommand cmd = new MySqlCommand(strQuery, conn/*, tran*/))
                        {
                            cmd.Parameters.AddWithValue("@orden", it.pos);
                            cmd.Parameters.AddWithValue("@id", it.id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    /*
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback(); // si algo falla
                    throw;           // se va al catch externo
                }
            }*/
                }
                // Opcional: recargar la lista o cerrar modal
                // Aquí vuelvo a cargar para que el usuario vea el cambio si quieres
                GetProtocolosOrden();
                GetGrdChBoxProtocolosConx(Convert.ToInt32(ddlServicioAddProt.SelectedValue),
                                          Convert.ToInt32(ddlCategoriaAddProt.SelectedValue));

                ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "cerrarModal", "$find('" + mpeOrderSLA.ClientID + "').hide();", true);

                //mpeOrderTask.Show(); // si quieres dejarlo abierto
                //LblMessage.Text = "Se agrego el orden de las Tareas, correctamente";

                LblMessage.Text = GetGlobalResourceObject("GlobalResources", "msg_Orden_Tareas_Agregado").ToString();
                mpeMensaje.Show();
            }
            catch (Exception ex)
            {
                /*
                    tran.Rollback();
                    // loguea el error o muéstralo (no exponer mensajes técnicos en producción)
                    throw; 
                */
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
            finally
            {

            }
        }

        protected void GetTaskddlDependencia(int iIdRelacion)
        {
            try
            {
                ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
                dbConn.Open();

                //string IdTpoAsunto = (string)Session["IdTpoAsunto"];
                //string IdCliente = (string)Session["IdCliente"];

                string strQuery = " SELECT r.IdRelaciones, t.NomTarea" +
                                  "   FROM ITM_98 r INNER JOIN ITM_54 t " +
                                  "     ON r.IdTarea = t.IdTarea " +
                                  "  WHERE r.IdRelacion = " + iIdRelacion +
                                  "    AND r.IdTarea_fk IS NULL" +
                                  "    AND r.IdStatus = 1 " + /* " AND r.bSeleccion = 1 " + */
                                  "    AND t.IdStatus = 1 ORDER BY r.IdOrden ASC; ";

                DataTable dt = dbConn.ExecuteQuery(strQuery);

                ddlTaskPadre.DataSource = dt;

                ddlTaskPadre.DataValueField = "IdRelaciones";
                ddlTaskPadre.DataTextField = "NomTarea";

                ddlTaskPadre.DataBind();
                ddlTaskPadre.Items.Insert(0, new ListItem(GetGlobalResourceObject("GlobalResources", "ddl_Select").ToString(), "0"));

                //ddlTaskPadre.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                dbConn.Close();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }

        }

        //protected void grdChBoxAddProtocolos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    try
        //    {
        //        grdChBoxAddProtocolos.PageIndex = e.NewPageIndex;
        //        //funcion para cargar los protocolos en la tabla 83
        //        GetGrdChBoxAddProtocolos();
        //    }
        //    catch (Exception ex)
        //    {
        //        LblMessage.Text = ex.Message;
        //        mpeMensaje.Show();
        //    }
        //}

    }
}