using System;

using System.Data;
using System.Data.SqlClient;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class fwAddNivel_Arch : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //Session["DownloadsPath"] = GetDownloadFolderPath();

            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    Variables.wUserName = Convert.ToString(Session["IdUsuario"]);
                    Variables.wPassword = Convert.ToString(Session["UsPassword"]);

                    if (Variables.wUserName == "" || Variables.wPassword == "")
                    {
                        Response.Redirect("Login.aspx", true);
                        return;
                    }

                    // Habilitar Controles
                    GetAseguradora();
                    GetConclusion();
                    GetRegimen();
                    GetTpoAsunto();

                    // GetRegimenDdlPnl();
                    GetDirectoriosDdlPnl();
                    GetTpoDoc();
                    GetSeccion();

                    GetArchivosPaq(0);
                    // Agrega THEAD y TBODY a GridView.
                    GridArchivosPaq.HeaderRow.TableSection = TableRowSection.TableHeader;

                    ddlAseguradora.Enabled = true;
                    ddlConclusion.Enabled = true;
                    ddlNivel1.Items.Insert(0, new ListItem("-- No Hay Nivel --", "0"));
                    ddlNivel1.Enabled = true;
                    BtnAddNivel1.Enabled = true;
                    ddlNivel2.Items.Insert(0, new ListItem("-- No Hay Nivel --", "0"));
                    ddlNivel2.Enabled = true;
                    BtnAddNivel2.Enabled = true;
                    //ddlNivel3.Items.Insert(0, new ListItem("-- No Hay Nivel --", "0"));
                    //ddlNivel3.Enabled = true;
                    //BtnAddNivel3.Enabled = true;
                    //ddlNivel4.Items.Insert(0, new ListItem("-- No Hay Nivel --", "0"));
                    //ddlNivel4.Enabled = true;
                    //BtnAddNivel4.Enabled = true;
                    BtnArchivos.Enabled = true;
                }

                // BtnAddAseguradora.Visible = false;
                // BtnAddConclusion.Visible = false;
                LblAddRegimen.Visible = false;
                BtnAddRegimen.Visible = false;
                LblAddCarpeta.Visible = false;
                BtnAddDirectorio.Visible = false;

                //GetArchivosPaq(0);
                //// Agrega THEAD y TBODY a GridView.
                //GridArchivosPaq.HeaderRow.TableSection = TableRowSection.TableHeader;

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.HResult.ToString();
                mpeMensaje.Show();
            }
        }

        protected void GetAseguradora()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT TOP (100) IdAseguradora, Descripcion " +
                                        " FROM ITM_48 "
                                        + " WHERE IdStatus = 1 "
                                        ;
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlAseguradora.DataSource = dt;

                ddlAseguradora.DataValueField = "IdAseguradora";
                ddlAseguradora.DataTextField = "Descripcion";

                ddlAseguradora.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ddlAseguradora.Items.Insert(0, new ListItem("-- No Hay Aseguradora --", "0"));
                }
                else
                {
                    ddlAseguradora.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                }

                Conecta.Cerrar();
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();

            }
            catch (System.Exception ex)
            {

                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetConclusion()  //Proceso
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT IdConclusion, Descripcion " + //conclusion
                                        " FROM ITM_10 "
                                        + " WHERE IdStatus = 1";
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlConclusion.DataSource = dt;

                ddlConclusion.DataValueField = "IdConclusion";
                ddlConclusion.DataTextField = "Descripcion";

                ddlConclusion.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ddlConclusion.Items.Insert(0, new ListItem("-- No Hay Proceso --", "0"));
                }
                else
                {
                    ddlConclusion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                }

                Conecta.Cerrar();
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();
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
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT IdRegimen, Descripcion " +
                                        " FROM ITM_49 " +
                                        " WHERE IdStatus = 1 ";
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlRegimen.DataSource = dt;

                ddlRegimen.DataValueField = "IdRegimen";
                ddlRegimen.DataTextField = "Descripcion";

                ddlRegimen.DataBind();
                ddlRegimen.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                Conecta.Cerrar();
                cmd.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetTpoAsunto()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT IdTpoAsunto, Descripcion " +
                                        " FROM ITM_66 " +
                                        " WHERE IdStatus = 1 ";
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlTpoAsunto.DataSource = dt;

                ddlTpoAsunto.DataValueField = "IdTpoAsunto";
                ddlTpoAsunto.DataTextField = "Descripcion";

                ddlTpoAsunto.DataBind();
                ddlTpoAsunto.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                Conecta.Cerrar();
                cmd.Dispose();
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

        protected void ddlAseguradora_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetConclusion();
            ddlConclusion.SelectedValue = "0";
            int IdProcecso = Convert.ToInt32(ddlConclusion.SelectedValue);
            int IdRegimen = Convert.ToInt32(ddlRegimen.SelectedValue);

            string sQNivel = "IdNivel1";

            GetNivel1Ddl(IdProcecso);
            GetNivel2Ddl(IdProcecso);
            // GetNivel3Ddl(IdProcecso);
            // GetNivel4Ddl(IdProcecso);

            GetArchivosPaqN(0, sQNivel, IdRegimen);
        }

        protected void GetNivel1Ddl(int IdProcecso)  //Proceso
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT IdNivel1, nomNivel1 " +
                                        " FROM ITM_53 "
                                        + " WHERE IdProceso = " + IdProcecso
                                        + " AND IdAseguradora = " + ddlAseguradora.SelectedValue;
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlNivel1.DataSource = dt;

                ddlNivel1.DataValueField = "IdNivel1";
                ddlNivel1.DataTextField = "nomNivel1";

                ddlNivel1.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ddlNivel1.Items.Insert(0, new ListItem("-- No Hay Nivel --", "0"));
                }
                else
                {
                    ddlNivel1.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                }

                Conecta.Cerrar();
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        protected void ddlConclusion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int IdProcecso = Convert.ToInt32(ddlConclusion.SelectedValue);
            int IdRegimen = Convert.ToInt32(ddlRegimen.SelectedValue);

            string sQNivel = "IdNivel1";

            GetNivel1Ddl(IdProcecso);
            GetNivel2Ddl(IdProcecso);
            //GetNivel3Ddl(IdProcecso);
            //GetNivel4Ddl(IdProcecso);

            GetArchivosPaqN(0, sQNivel, IdRegimen);
            //GetArchivosPaq(IdProcecso);  //los archivos que correspondan al nivel 1
        }

        protected void GetArchivosPaq(int IdProceso)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT A.IdArchivo, A.nomArchivo, B.Descripcion, A.IdDirectorio, A.IdRegimen, A.IdSeccion, R.Descripcion AS DrescRegimen " +
                                        " FROM ITM_58 AS A " +
                                          " INNER JOIN ITM_57 AS AU ON A.IdArchivo = AU.IdArchivo " +
                                          " INNER JOIN ITM_50 AS B ON A.IdTpoArchivo = B.IdTpoArchivo " +
                                          " INNER JOIN ITM_49 AS R ON A.IdRegimen = R.IdRegimen"
                                        + " WHERE AU.IdNivel1 = " + IdProceso +
                                        "     AND A.IdStatus = 1 " +
                                        " ";
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    GridArchivosPaq.ShowHeaderWhenEmpty = true;  //Cambiarle GridV
                    GridArchivosPaq.EmptyDataText = "No hay resultados.";
                }

                GridArchivosPaq.DataSource = dt;
                GridArchivosPaq.DataBind();

                Conecta.Cerrar();
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();

                //* * Agrega THEAD y TBODY a GridView.
                GridArchivosPaq.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        protected void GetNivel2Ddl(int IdProceso)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT IdNivel2, nomNivel2 " +
                                        " FROM ITM_54 "
                                        + " WHERE IdProceso =" + IdProceso
                                        + " AND IdAseguradora = " + ddlAseguradora.SelectedValue;
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlNivel2.DataSource = dt;

                ddlNivel2.DataValueField = "IdNivel2";
                ddlNivel2.DataTextField = "nomNivel2";

                ddlNivel2.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ddlNivel2.Items.Insert(0, new ListItem("-- No Hay Nivel --", "0"));
                }
                else
                {
                    ddlNivel2.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                }

                Conecta.Cerrar();
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetNivel3Ddl(int IdProceso)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT IdNivel3, nomNivel3 " +
                                        " FROM ITM_55 "
                                        + " WHERE IdProceso = " + IdProceso
                                        + " AND IdAseguradora = " + ddlAseguradora.SelectedValue;
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                //ddlNivel3.DataSource = dt;

                //ddlNivel3.DataValueField = "IdNivel3";
                //ddlNivel3.DataTextField = "nomNivel3";

                //ddlNivel3.DataBind();

                //if (dt.Rows.Count == 0)
                //{
                //    ddlNivel3.Items.Insert(0, new ListItem("-- No Hay Nivel --", "0"));
                //}
                //else
                //{
                //    ddlNivel3.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                //}

                Conecta.Cerrar();
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        protected void GetNivel4Ddl(int IdProceso)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT IdNivel4, nomNivel4 " +
                                        " FROM ITM_56 "
                                        + " WHERE IdProceso =" + IdProceso
                                        + " AND IdAseguradora = " + ddlAseguradora.SelectedValue;
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                //ddlNivel4.DataSource = dt;

                //ddlNivel4.DataValueField = "IdNivel4";
                //ddlNivel4.DataTextField = "nomNivel4";

                //ddlNivel4.DataBind();

                //if (dt.Rows.Count == 0)
                //{
                //    ddlNivel4.Items.Insert(0, new ListItem("-- No Hay Nivel --", "0"));
                //}
                //else
                //{
                //    ddlNivel4.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                //}

                Conecta.Cerrar();
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetArchivosPaqN(int IdNivel, string sQNivel, int IdRegimen)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT A.IdArchivo, A.nomArchivo, B.Descripcion, A.IdDirectorio, A.IdRegimen, A.IdSeccion, A.IdTpoArchivo, R.Descripcion AS DrescRegimen " +
                                        " FROM ITM_58 AS A " +
                                        "   INNER JOIN ITM_57 AS AU ON A.IdArchivo = AU.IdArchivo " +
                                          " INNER JOIN ITM_50 AS B ON A.IdTpoArchivo = B.IdTpoArchivo " +
                                          " INNER JOIN ITM_49 AS R ON A.IdRegimen = R.IdRegimen "
                                        + " WHERE AU." + sQNivel + "= " + IdNivel +
                                        "     AND A.IdRegimen = " + IdRegimen + " " +
                                        "     AND A.IdStatus = 1  ";

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    GridArchivosPaq.ShowHeaderWhenEmpty = true;  //Cambiarle GridV
                    GridArchivosPaq.EmptyDataText = "No hay resultados.";
                }

                GridArchivosPaq.DataSource = dt;
                GridArchivosPaq.DataBind();

                Conecta.Cerrar();
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();

                //* * Agrega THEAD y TBODY a GridView.
                GridArchivosPaq.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAddAseguradora_Click(object sender, EventArgs e)
        {
            Variables.wIdentificadorBtn = 1;
            LblTitlePnlAdd.Text = "Agregar Aseguradora";
            LblTitlePnlAdd.Visible = true;
            // LblPnlMsgFiscal.Visible = false;
            // ddlPnlMsgFiscal.Visible = false;
            LblMsgPnlAdd.Text = "Escribir el Nombre de la Aseguradora.";
            LblDdlPnlMsgAdd.Visible = false;
            ddlPnlMsAdd.Visible = false;
            LblDdlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Enabled = true;
            LblPnlMsgSeccion.Visible = false;
            ddlPnlMsgSeccion.Visible = false;
            ddlPnlMsgSeccion.Enabled = true;
            BtnPnlMsgAceptar.Visible = true;
            BtnClousePnlAdd.Visible = true;
            mpeAddMensaje.Show();
        }

        protected void ddlNivel1_SelectedIndexChanged(object sender, EventArgs e)
        {

            ddlNivel2.SelectedValue = "0";
            // ddlNivel3.SelectedValue = "0";
            // ddlNivel4.SelectedValue = "0";
            string sQNivel = "IdNivel1";
            int IdNivel = Convert.ToInt32(ddlNivel1.SelectedValue);
            int IdRegimen = 4;

            if (ddlNivel1.SelectedItem.Text == "492")
            {
                IdRegimen = Convert.ToInt32(ddlRegimen.SelectedValue);
            }

            GetArchivosPaqN(0, sQNivel, IdRegimen);
            GetArchivosPaqN(IdNivel, sQNivel, IdRegimen);
        }

        protected void ddlNivel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlNivel1.SelectedValue = "0";
            // ddlNivel3.SelectedValue = "0";
            // ddlNivel4.SelectedValue = "0";
            string sQNivel = "IdNivel2";
            int IdNivel = Convert.ToInt32(ddlNivel2.SelectedValue);
            int IdRegimen = 4;

            if (ddlNivel2.SelectedItem.Text == "492")
            {
                IdRegimen = Convert.ToInt32(ddlRegimen.SelectedValue);
            }

            GetArchivosPaqN(0, sQNivel, IdRegimen); ;
            GetArchivosPaqN(IdNivel, sQNivel, IdRegimen);
        }

        protected void ddlNivel3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlNivel1.SelectedValue = "0";
            ddlNivel2.SelectedValue = "0";
            // ddlNivel4.SelectedValue = "0";
            // int IdNivel = Convert.ToInt32(ddlNivel3.SelectedValue);

            // string sQNivel = "IdNivel3";
            // GetArchivosPaqN(0, sQNivel); ;
            // GetArchivosPaqN(IdNivel, sQNivel);
        }

        protected void ddlNivel4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlNivel1.SelectedValue = "0";
            ddlNivel2.SelectedValue = "0";
            // ddlNivel3.SelectedValue = "0";
            // int IdNivel = Convert.ToInt32(ddlNivel4.SelectedValue);
            // string sQNivel = "IdNivel4";
            // GetArchivosPaqN(0, sQNivel); ;
            // GetArchivosPaqN(IdNivel, sQNivel);
        }

        protected void BtnPnlMsgAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                string CajaTxtPnlAdd = TxtPnlMsgAdd.Text;
                int IdentificadorBtn = Convert.ToInt32(Variables.wIdentificadorBtn);
                string tablaConsultar = string.Empty;
                string columnaConsultar = string.Empty;
                string compararContra = string.Empty;
                int existeRegistro = 0;
                int iNivelInsert = 1;
                int iIdNivel = 0;
                string sIdNivel = string.Empty;

                if ((CajaTxtPnlAdd == string.Empty) || (CajaTxtPnlAdd == "") || (CajaTxtPnlAdd == null))
                {
                    LblMessage.Text = "Nombre de Archivo es obligatorio";
                    mpeMensaje.Show();
                    //break;
                    return;
                }
                //balida que este seleccionado una seguradora y una conclusion
                if (ddlAseguradora.SelectedValue == "0")
                {
                    TxtPnlMsgAdd.Text = string.Empty;
                    LblMessage.Text = "Seleccionar Aseguradora";
                    mpeMensaje.Show();
                    return;
                }
                if (ddlConclusion.SelectedValue == "0")
                {
                    TxtPnlMsgAdd.Text = string.Empty;
                    LblMessage.Text = "Seleccionar Tipo de Cuaderno";
                    mpeMensaje.Show();
                    return;
                }

                if (IdentificadorBtn == 1)
                {
                    tablaConsultar = "ITM_48";
                    columnaConsultar = "Descripcion";

                }
                else if (IdentificadorBtn == 2)
                {
                    tablaConsultar = "ITM_10";
                    columnaConsultar = "Descripcion";
                    compararContra = " AND IdAseguradora = " + ddlAseguradora.SelectedValue;
                    if (ddlAseguradora.SelectedValue == "0")
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar Aseguradora";
                        mpeMensaje.Show();
                        return;
                    }
                }
                else if (IdentificadorBtn == 3)
                {
                    tablaConsultar = "ITM_53";
                    columnaConsultar = "nomNivel1";
                    compararContra = " AND IdProceso = " + ddlConclusion.SelectedValue + " AND IdAseguradora = " + ddlAseguradora.SelectedValue;
                    if (ddlConclusion.SelectedValue == "0")
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar Tipo de Cuaderno";
                        mpeMensaje.Show();
                        return;
                    }
                }
                else if (IdentificadorBtn == 4)
                {
                    tablaConsultar = "ITM_54";
                    columnaConsultar = "nomNivel2";
                    compararContra = " AND IdProceso = " + ddlConclusion.SelectedValue + " AND IdAseguradora = " + ddlAseguradora.SelectedValue;
                    if (ddlConclusion.SelectedValue == "0")
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar Tipo de Cuaderno";
                        mpeMensaje.Show();
                        return;
                    }
                }
                else if (IdentificadorBtn == 5)
                {
                    tablaConsultar = "ITM_55";
                    columnaConsultar = "nomNivel3";
                    compararContra = " AND IdProceso = " + ddlConclusion.SelectedValue + " AND IdAseguradora = " + ddlAseguradora.SelectedValue;
                    if (ddlConclusion.SelectedValue == "0")
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar Tipo de Cuaderno";
                        mpeMensaje.Show();
                        return;
                    }
                }
                else if (IdentificadorBtn == 6)
                {
                    tablaConsultar = "ITM_56";
                    columnaConsultar = "nomNivel4";
                    compararContra = " AND IdProceso = " + ddlConclusion.SelectedValue + " AND IdAseguradora = " + ddlAseguradora.SelectedValue;
                    if (ddlConclusion.SelectedValue == "0")
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar Tipo de Cuaderno";
                        mpeMensaje.Show();
                        return;
                    }
                }
                else if (IdentificadorBtn == 7)
                {
                    tablaConsultar = "ITM_49";
                    columnaConsultar = "Descripcion";
                    compararContra = " AND IdAseguradora = " + ddlAseguradora.SelectedValue;
                    if (ddlAseguradora.SelectedValue == "0")
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar Aseguradora";
                        mpeMensaje.Show();
                        return;
                    }
                }
                else if (IdentificadorBtn == 8)
                {
                    tablaConsultar = "ITM_51";
                    columnaConsultar = "Nom_Directorio";
                    compararContra = " AND IdAseguradora = " + ddlAseguradora.SelectedValue;
                    if (ddlAseguradora.SelectedValue == "0")
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar Aseguradora";
                        mpeMensaje.Show();
                        return;
                    }
                }
                else if (IdentificadorBtn == 9)
                {
                    tablaConsultar = "ITM_58";
                    columnaConsultar = "nomArchivo";
                    if (ddlConclusion.SelectedValue == "0")
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar Tipo de Cuaderno";
                        mpeMensaje.Show();
                        return;
                    }
                    if (ddlRegimen.SelectedValue == "0")
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar Regimen Fiscal";
                        mpeMensaje.Show();
                        return;
                    }
                    if (ddlPnlMsAdd.SelectedValue == "0")
                    {
                        LblMessage.Text = "Seleccionar una Carpeta";
                        mpeMensaje.Show();
                        return;
                    }
                    if (ddlPnlMsgTpDoc.SelectedValue == "0")
                    {
                        LblMessage.Text = "Seleccionar Tipo de Documento";
                        mpeMensaje.Show();
                        return;
                    }
                    if (ddlNivel1.SelectedValue != "0")
                    {
                        compararContra = " AND AU.IdNivel1 = " + ddlNivel1.SelectedValue +
                                         " AND A.IdRegimen = " + ddlRegimen.SelectedValue +
                                         " AND A.IdTpoArchivo = " + ddlPnlMsgTpDoc.SelectedValue;
                        iNivelInsert = 1;
                        iIdNivel = Convert.ToInt32(ddlNivel1.SelectedValue);
                        sIdNivel = "IdNivel1";
                    }
                    else if (ddlNivel2.SelectedValue != "0")
                    {
                        compararContra = " AND AU.IdNivel2 = " + ddlNivel2.SelectedValue +
                                         " AND A.IdRegimen = " + ddlRegimen.SelectedValue +
                                         " AND A.IdTpoArchivo = " + ddlPnlMsgTpDoc.SelectedValue;
                        iNivelInsert = 2;
                        iIdNivel = Convert.ToInt32(ddlNivel2.SelectedValue);
                        sIdNivel = "IdNivel2";
                    }
                    //else if (ddlNivel3.SelectedValue != "0")
                    //{
                    //    compararContra = " AND AU.IdNivel3 = " + ddlNivel3.SelectedValue + 
                    //                     " AND A.IdRegimen = " + ddlPnlMsgFiscal.SelectedValue + 
                    //                     " AND A.IdTpoArchivo = " + ddlPnlMsgTpDoc.SelectedValue;
                    //    iNivelInsert = 3;
                    //    iIdNivel = Convert.ToInt32(ddlNivel3.SelectedValue);
                    //    sIdNivel = "IdNivel3";
                    //}
                    //else if (ddlNivel4.SelectedValue != "0")
                    //{
                    //    compararContra = " AND AU.IdNivel4 = " + ddlNivel4.SelectedValue + 
                    //                     " AND A.IdRegimen = " + ddlPnlMsgFiscal.SelectedValue + 
                    //                     " AND A.IdTpoArchivo = " + ddlPnlMsgTpDoc.SelectedValue;
                    //    iNivelInsert = 4;
                    //    iIdNivel = Convert.ToInt32(ddlNivel4.SelectedValue);
                    //    sIdNivel = "IdNivel4";
                    //}
                    else
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar un Nivel";
                        mpeMensaje.Show();
                        return;
                    }

                }
                else if (IdentificadorBtn == 10)
                {
                    tablaConsultar = "ITM_58";
                    columnaConsultar = "nomArchivo";
                    if (ddlConclusion.SelectedValue == "0")
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar Tipo de Cuaderno";
                        mpeMensaje.Show();
                        return;
                    }
                    if (ddlRegimen.SelectedValue == "0")
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar Regimen Fiscal";
                        mpeMensaje.Show();
                        return;
                    }
                    if (ddlPnlMsAdd.SelectedValue == "0")
                    {
                        LblMessage.Text = "Seleccionar una Carpeta";
                        mpeMensaje.Show();
                        return;
                    }
                    if (ddlNivel1.SelectedValue != "0")
                    {
                        compararContra = " AND AU.IdNivel1 = " + ddlNivel1.SelectedValue;
                        iNivelInsert = 1;
                        iIdNivel = Convert.ToInt32(ddlNivel1.SelectedValue);
                        sIdNivel = "IdNivel1";
                    }
                    else if (ddlNivel2.SelectedValue != "0")
                    {
                        compararContra = " AND AU.IdNivel2 = " + ddlNivel2.SelectedValue;
                        iNivelInsert = 2;
                        iIdNivel = Convert.ToInt32(ddlNivel2.SelectedValue);
                        sIdNivel = "IdNivel2";
                    }
                    //else if (ddlNivel3.SelectedValue != "0")
                    //{
                    //    compararContra = " AND AU.IdNivel3 = " + ddlNivel3.SelectedValue;
                    //    iNivelInsert = 3;
                    //    iIdNivel = Convert.ToInt32(ddlNivel3.SelectedValue);
                    //    sIdNivel = "IdNivel3";
                    //}
                    //else if (ddlNivel4.SelectedValue != "0")
                    //{
                    //    compararContra = " AND AU.IdNivel4 = " + ddlNivel4.SelectedValue;
                    //    iNivelInsert = 4;
                    //    iIdNivel = Convert.ToInt32(ddlNivel4.SelectedValue);
                    //    sIdNivel = "IdNivel4";
                    //}
                    else
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar un Nivel";
                        mpeMensaje.Show();
                        return;
                    }
                }
                else if (IdentificadorBtn == 11)
                {
                    tablaConsultar = "ITM_58";
                    columnaConsultar = "nomArchivo";
                    if (ddlConclusion.SelectedValue == "0")
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar Tipo de Cuaderno";
                        mpeMensaje.Show();
                        return;
                    }
                    if (ddlRegimen.SelectedValue == "0")
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar Regimen Fiscal";
                        mpeMensaje.Show();
                        return;
                    }
                    if (ddlPnlMsAdd.SelectedValue == "0")
                    {
                        LblMessage.Text = "Seleccionar una Carpeta";
                        mpeMensaje.Show();
                        return;
                    }
                    if (ddlNivel1.SelectedValue != "0")
                    {
                        compararContra = " AND AU.IdNivel1 = " + ddlNivel1.SelectedValue;
                        iNivelInsert = 1;
                        iIdNivel = Convert.ToInt32(ddlNivel1.SelectedValue);
                        sIdNivel = "IdNivel1";
                    }
                    else if (ddlNivel2.SelectedValue != "0")
                    {
                        compararContra = " AND AU.IdNivel2 = " + ddlNivel2.SelectedValue;
                        iNivelInsert = 2;
                        iIdNivel = Convert.ToInt32(ddlNivel2.SelectedValue);
                        sIdNivel = "IdNivel2";
                    }
                    //else if (ddlNivel3.SelectedValue != "0")
                    //{
                    //    compararContra = " AND AU.IdNivel3 = " + ddlNivel3.SelectedValue;
                    //    iNivelInsert = 3;
                    //    iIdNivel = Convert.ToInt32(ddlNivel3.SelectedValue);
                    //    sIdNivel = "IdNivel3";
                    //}
                    //else if (ddlNivel4.SelectedValue != "0")
                    //{
                    //    compararContra = " AND AU.IdNivel4 = " + ddlNivel4.SelectedValue;
                    //    iNivelInsert = 4;
                    //    iIdNivel = Convert.ToInt32(ddlNivel4.SelectedValue);
                    //    sIdNivel = "IdNivel4";
                    //}
                    else
                    {
                        TxtPnlMsgAdd.Text = string.Empty;
                        LblMessage.Text = "Seleccionar un Nivel";
                        mpeMensaje.Show();
                        return;
                    }
                }


                if ((IdentificadorBtn > 0) && (IdentificadorBtn <= 9))
                {
                    if ((IdentificadorBtn == 9) || (IdentificadorBtn == 11))
                    {
                        existeRegistro = ExisteRegistroArchivo(CajaTxtPnlAdd, tablaConsultar, columnaConsultar, compararContra);
                    }
                    else
                    {
                        existeRegistro = ExisteRegistro(CajaTxtPnlAdd, tablaConsultar, columnaConsultar, compararContra);
                    }
                }


                if (existeRegistro == 1)
                {
                    TxtPnlMsgAdd.Text = string.Empty;
                    LblMessage.Text = "Algo salio mal o ya existe " + CajaTxtPnlAdd;
                    mpeMensaje.Show();
                    //break;
                    return;
                }

                int IdRegimen = 3;

                if (ddlNivel1.SelectedItem.Text == "492" || ddlNivel2.SelectedItem.Text == "492")
                {
                    IdRegimen = Convert.ToInt32(ddlRegimen.SelectedValue);
                }

                switch (IdentificadorBtn)
                {
                    case 1:
                        InsertDB(IdentificadorBtn, CajaTxtPnlAdd, iNivelInsert, sIdNivel, iIdNivel, IdRegimen);
                        TxtPnlMsgAdd.Text = string.Empty;
                        GetAseguradora();
                        LblMessage.Text = "Se inserto Aseguradora correctamente";
                        mpeMensaje.Show();
                        break;
                    case 2:
                        InsertDB(IdentificadorBtn, CajaTxtPnlAdd, iNivelInsert, sIdNivel, iIdNivel, IdRegimen);
                        TxtPnlMsgAdd.Text = string.Empty;
                        //ddlPnlMsAdd.SelectedValue = "0";
                        GetConclusion();
                        LblMessage.Text = "Se inserto Tipo de Cuaderno correctamente";
                        mpeMensaje.Show();
                        break;
                    case 3:
                        InsertDB(IdentificadorBtn, CajaTxtPnlAdd, iNivelInsert, sIdNivel, iIdNivel, IdRegimen);
                        TxtPnlMsgAdd.Text = string.Empty;
                        //ddlPnlMsAdd.SelectedValue = "0";
                        GetNivel1Ddl(Convert.ToInt32(ddlConclusion.SelectedValue));
                        LblMessage.Text = "Se inserto el nivel 1 correctamente";
                        mpeMensaje.Show();
                        break;
                    case 4:
                        InsertDB(IdentificadorBtn, CajaTxtPnlAdd, iNivelInsert, sIdNivel, iIdNivel, IdRegimen);
                        TxtPnlMsgAdd.Text = string.Empty;
                        //ddlPnlMsAdd.SelectedValue = "0";
                        GetNivel2Ddl(Convert.ToInt32(ddlConclusion.SelectedValue));
                        LblMessage.Text = "Se inserto el nivel 2 correctamente";
                        mpeMensaje.Show();
                        break;
                    case 5:
                        InsertDB(IdentificadorBtn, CajaTxtPnlAdd, iNivelInsert, sIdNivel, iIdNivel, IdRegimen);
                        TxtPnlMsgAdd.Text = string.Empty;
                        //ddlPnlMsAdd.SelectedValue = "0";
                        GetNivel3Ddl(Convert.ToInt32(ddlConclusion.SelectedValue));
                        LblMessage.Text = "Se inserto el nivel 3 correctamente";
                        mpeMensaje.Show();
                        break;
                    case 6:
                        InsertDB(IdentificadorBtn, CajaTxtPnlAdd, iNivelInsert, sIdNivel, iIdNivel, IdRegimen);
                        TxtPnlMsgAdd.Text = string.Empty;
                        //ddlPnlMsAdd.SelectedValue = "0";
                        // GetNivel4Ddl(Convert.ToInt32(ddlConclusion.SelectedValue));
                        LblMessage.Text = "Se inserto el nivel 4 correctamente";
                        mpeMensaje.Show();
                        break;
                    case 7:
                        InsertDB(IdentificadorBtn, CajaTxtPnlAdd, iNivelInsert, sIdNivel, iIdNivel, IdRegimen);
                        TxtPnlMsgAdd.Text = string.Empty;
                        // GetRegimenDdlPnl();
                        LblMessage.Text = "Se inserto regimen fiscal correctamente";
                        mpeMensaje.Show();
                        break;
                    case 8:
                        InsertDB(IdentificadorBtn, CajaTxtPnlAdd, iNivelInsert, sIdNivel, iIdNivel, IdRegimen);
                        TxtPnlMsgAdd.Text = string.Empty;
                        GetDirectoriosDdlPnl();
                        LblMessage.Text = "Se inserto la carpeta correctamente";
                        mpeMensaje.Show();
                        break;
                    case 9:
                        InsertDB(IdentificadorBtn, CajaTxtPnlAdd, iNivelInsert, sIdNivel, iIdNivel, IdRegimen);
                        TxtPnlMsgAdd.Text = string.Empty;
                        GetArchivosPaqN(0, sIdNivel, IdRegimen); ;
                        GetArchivosPaqN(iIdNivel, sIdNivel, IdRegimen);
                        LblMessage.Text = "Se inserto el archivo correctamente";
                        mpeMensaje.Show();
                        break;
                    case 10:
                        InsertDB(IdentificadorBtn, CajaTxtPnlAdd, iNivelInsert, sIdNivel, iIdNivel, IdRegimen);
                        TxtPnlMsgAdd.Text = string.Empty;
                        GetArchivosPaqN(0, sIdNivel, IdRegimen);
                        GetArchivosPaqN(iIdNivel, sIdNivel, IdRegimen);
                        LblMessage.Text = "Se elimino el archivo correctamente";
                        mpeMensaje.Show();
                        break;
                    case 11:
                        InsertDB(IdentificadorBtn, CajaTxtPnlAdd, iNivelInsert, sIdNivel, iIdNivel, IdRegimen);
                        TxtPnlMsgAdd.Text = string.Empty;
                        GetArchivosPaqN(0, sIdNivel, IdRegimen); ;
                        GetArchivosPaqN(iIdNivel, sIdNivel, IdRegimen);
                        LblMessage.Text = "Se edito el archivo correctamente";
                        mpeMensaje.Show();
                        break;
                }
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void InsertDB(int IdentBtn, string CajaTxtPnlAdd, int iNivelInsert, string sIdNivel, int iIdNivel, int IdRegimen)
        {
            try
            {
                string sqlQuery;

                switch (IdentBtn)
                {
                    case 1:
                        sqlQuery = "INSERT INTO ITM_48 VALUES('" + CajaTxtPnlAdd + "');";
                        ejecucionBD(sqlQuery);
                        break;
                    case 2:
                        sqlQuery = "INSERT INTO ITM_10 VALUES('" + CajaTxtPnlAdd + "'," + ddlAseguradora.SelectedValue + ");";
                        ejecucionBD(sqlQuery);
                        break;
                    case 3:
                        sqlQuery = "INSERT INTO ITM_53 VALUES('" + CajaTxtPnlAdd + "'," + ddlConclusion.SelectedValue + "," + ddlAseguradora.SelectedValue + ");";
                        ejecucionBD(sqlQuery);
                        break;
                    case 4:
                        sqlQuery = "INSERT INTO ITM_54 VALUES('" + CajaTxtPnlAdd + "'," + ddlConclusion.SelectedValue + "," + ddlAseguradora.SelectedValue + ");";
                        ejecucionBD(sqlQuery);
                        break;
                    case 5:
                        sqlQuery = "INSERT INTO ITM_55 VALUES('" + CajaTxtPnlAdd + "'," + ddlConclusion.SelectedValue + "," + ddlAseguradora.SelectedValue + ");";
                        ejecucionBD(sqlQuery);
                        break;
                    case 6:
                        sqlQuery = "INSERT INTO ITM_56 VALUES('" + CajaTxtPnlAdd + "'," + ddlConclusion.SelectedValue + "," + ddlAseguradora.SelectedValue + ");";
                        ejecucionBD(sqlQuery);
                        break;
                    case 7:
                        sqlQuery = "INSERT INTO ITM_49 (Descripcion, IdStatus) VALUES('" + CajaTxtPnlAdd + "',1);";
                        ejecucionBD(sqlQuery);
                        break;
                    case 8:
                        sqlQuery = "INSERT INTO ITM_51 (Nom_Directorio, IdStatus, IdAseguradora) " +
                                               " VALUES('" + CajaTxtPnlAdd + "',1," + ddlAseguradora.SelectedValue + ");";
                        ejecucionBD(sqlQuery);
                        break;
                    case 9:
                        int IdArchivoMax = GetIdArchivoMax();
                        sqlQuery = "INSERT INTO ITM_58 (IdArchivo, nomArchivo, Nivel, IdStatus, IdDirectorio, IdRegimen, IdTpoArchivo, IdSeccion, IdAseguradora, IdConclusion) " +
                                  " VALUES(" + IdArchivoMax + ",'" + CajaTxtPnlAdd + "', " + iNivelInsert + ",1, " + ddlPnlMsAdd.SelectedValue + ", " +
                                  IdRegimen + ", " + ddlPnlMsgTpDoc.SelectedValue + ", " + ddlPnlMsgSeccion.SelectedValue + ", " +
                                  ddlAseguradora.SelectedValue + ", " + ddlConclusion.SelectedValue + ");";
                        ejecucionBD(sqlQuery);
                        sqlQuery = InsertArchivoUbic(IdArchivoMax, sIdNivel, iIdNivel);
                        ejecucionBD(sqlQuery);
                        break;
                    case 10:
                        int iIdArchivo = Convert.ToInt32(Variables.wIdArchivo);
                        sqlQuery = "DELETE FROM ITM_57 WHERE IdArchivo = " + iIdArchivo;
                        ejecucionBD(sqlQuery);
                        sqlQuery = "DELETE FROM ITM_58 WHERE IdArchivo = " + iIdArchivo;
                        ejecucionBD(sqlQuery);
                        break;
                    case 11:
                        iIdArchivo = Convert.ToInt32(Variables.wIdArchivo);
                        sqlQuery = "UPDATE ITM_58  SET nomArchivo = '" + CajaTxtPnlAdd + "', " +
                                                    " IdDirectorio = " + ddlPnlMsAdd.SelectedValue + ", " +
                                                       " IdRegimen = " + IdRegimen + "," +
                                                    " IdTpoArchivo = " + ddlPnlMsgTpDoc.SelectedValue + "," +
                                                       " IdSeccion = " + ddlPnlMsgSeccion.SelectedValue +
                                                 " WHERE IdArchivo = " + iIdArchivo;
                        ejecucionBD(sqlQuery);
                        break;
                }

            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        protected void ejecucionBD(string sqlQuery)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                Conecta.Cerrar();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnAddConclusion_Click(object sender, EventArgs e)
        {
            Variables.wIdentificadorBtn = 2;
            LblTitlePnlAdd.Text = "Agregar Tipo de Cuaderno";
            LblTitlePnlAdd.Visible = true;
            // LblPnlMsgFiscal.Visible = false;
            // ddlPnlMsgFiscal.Visible = false;
            LblDdlPnlMsgAdd.Text = "Tipo de Cuaderno / Proceso";
            LblDdlPnlMsgAdd.Visible = false;
            ddlPnlMsAdd.Visible = false;
            LblMsgPnlAdd.Text = "Escribir el Nombre del Tipo de Cuaderno.";
            LblMsgPnlAdd.Visible = true;
            LblDdlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Enabled = true;
            LblPnlMsgSeccion.Visible = false;
            ddlPnlMsgSeccion.Visible = false;
            ddlPnlMsgSeccion.Enabled = true;
            BtnPnlMsgAceptar.Visible = true;
            BtnClousePnlAdd.Visible = true;
            mpeAddMensaje.Show();
        }

        public int ExisteRegistro(string CajaTxtPnlAdd, string tablaConsultar, string columnaConsultar, string compararContra)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT * " +
                                   " FROM " + tablaConsultar +
                                  " WHERE " + columnaConsultar + " = '" + CajaTxtPnlAdd + "'" +
                                  compararContra;
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    Conecta.Cerrar();
                    cmd.Dispose();
                    da.Dispose();
                    dt.Dispose();
                    return 0;
                }
                Conecta.Cerrar();
                return 1;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
            return 1;
        }

        protected void BtnAddNivel1_Click(object sender, EventArgs e)
        {
            Variables.wIdentificadorBtn = 3;
            LblTitlePnlAdd.Text = "Agregar Nivel 1";
            LblTitlePnlAdd.Visible = true;
            // LblPnlMsgFiscal.Visible = false;
            // ddlPnlMsgFiscal.Visible = false;
            LblDdlPnlMsgAdd.Text = "Tipo de Cuaderno / Proceso";
            LblDdlPnlMsgAdd.Visible = false;
            ddlPnlMsAdd.Visible = false;
            LblMsgPnlAdd.Text = "Escribir el Nombre del Nivel 1.";
            LblMsgPnlAdd.Visible = true;
            LblDdlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Enabled = true;
            LblPnlMsgSeccion.Visible = false;
            ddlPnlMsgSeccion.Visible = false;
            ddlPnlMsgSeccion.Enabled = true;
            BtnPnlMsgAceptar.Visible = true;
            BtnClousePnlAdd.Visible = true;
            mpeAddMensaje.Show();
        }

        protected void BtnAddNivel2_Click(object sender, EventArgs e)
        {
            Variables.wIdentificadorBtn = 4;
            LblTitlePnlAdd.Text = "Agregar Nivel 2";
            LblTitlePnlAdd.Visible = true;
            // LblPnlMsgFiscal.Visible = false;
            // ddlPnlMsgFiscal.Visible = false;
            LblDdlPnlMsgAdd.Text = "Tipo de Cuaderno / Proceso";
            LblDdlPnlMsgAdd.Visible = false;
            ddlPnlMsAdd.Visible = false;
            LblMsgPnlAdd.Text = "Escribir el Nombre del Nivel 2.";
            LblMsgPnlAdd.Visible = true;
            LblDdlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Enabled = true;
            LblPnlMsgSeccion.Visible = false;
            ddlPnlMsgSeccion.Visible = false;
            ddlPnlMsgSeccion.Enabled = true;
            BtnPnlMsgAceptar.Visible = true;
            BtnClousePnlAdd.Visible = true;
            mpeAddMensaje.Show();
        }

        protected void BtnAddNivel3_Click(object sender, EventArgs e)
        {
            Variables.wIdentificadorBtn = 5;
            LblTitlePnlAdd.Text = "Agregar Nivel 3";
            LblTitlePnlAdd.Visible = true;
            // LblPnlMsgFiscal.Visible = false;
            // ddlPnlMsgFiscal.Visible = false;
            LblDdlPnlMsgAdd.Text = "Tipo de Cuaderno / Proceso";
            LblDdlPnlMsgAdd.Visible = false;
            ddlPnlMsAdd.Visible = false;
            LblMsgPnlAdd.Text = "Escribir el Nombre del Nivel 3.";
            LblMsgPnlAdd.Visible = true;
            LblDdlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Enabled = true;
            LblPnlMsgSeccion.Visible = false;
            ddlPnlMsgSeccion.Visible = false;
            ddlPnlMsgSeccion.Enabled = true;
            BtnPnlMsgAceptar.Visible = true;
            BtnClousePnlAdd.Visible = true;
            mpeAddMensaje.Show();
        }

        protected void BtnAddNivel4_Click(object sender, EventArgs e)
        {
            Variables.wIdentificadorBtn = 6;
            LblTitlePnlAdd.Text = "Agregar Nivel 4";
            LblTitlePnlAdd.Visible = true;
            // LblPnlMsgFiscal.Visible = false;
            // ddlPnlMsgFiscal.Visible = false;
            LblDdlPnlMsgAdd.Text = "Tipo de Cuaderno / Proceso";
            LblDdlPnlMsgAdd.Visible = false;
            ddlPnlMsAdd.Visible = false;
            LblMsgPnlAdd.Text = "Escribir el Nombre del Nivel 4.";
            LblMsgPnlAdd.Visible = true;
            LblDdlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Enabled = true;
            LblPnlMsgSeccion.Visible = false;
            ddlPnlMsgSeccion.Visible = false;
            ddlPnlMsgSeccion.Enabled = true;
            BtnPnlMsgAceptar.Visible = true;
            BtnClousePnlAdd.Visible = true;
            mpeAddMensaje.Show();
        }

        protected void BtnArchivos_Click(object sender, EventArgs e)
        {
            Variables.wIdentificadorBtn = 9;
            //GetRegimenDdlPnl();
            //GetDirectoriosDdlPnl();
            //GetTpoDoc();
            //GetSeccion();
            LblTitlePnlAdd.Text = "Agregar Archivo";
            LblTitlePnlAdd.Visible = true;
            // LblPnlMsgFiscal.Visible = true;
            // ddlPnlMsgFiscal.Enabled = true;
            // ddlPnlMsgFiscal.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
            // ddlPnlMsgFiscal.SelectedValue = "0";
            // ddlPnlMsgFiscal.Visible = true;
            LblDdlPnlMsgAdd.Text = "Indique la carpeta donde se va a guardar";
            LblDdlPnlMsgAdd.Visible = true;

            ddlPnlMsAdd.SelectedValue = "0";
            ddlPnlMsAdd.Visible = true;
            ddlPnlMsAdd.Enabled = true;

            LblMsgPnlAdd.Text = "Escribir Nombre del Archivo.";
            LblMsgPnlAdd.Visible = true;
            TxtPnlMsgAdd.Text = "";
            TxtPnlMsgAdd.Enabled = true;
            TxtPnlMsgAdd.Visible = true;
            LblDdlPnlMsgTpDoc.Visible = true;

            ddlPnlMsgTpDoc.SelectedValue = "0";
            ddlPnlMsgTpDoc.Visible = true;
            ddlPnlMsgTpDoc.Enabled = true;

            LblPnlMsgSeccion.Visible = true;
            ddlPnlMsgSeccion.SelectedValue = "0";
            ddlPnlMsgSeccion.Visible = true;
            ddlPnlMsgSeccion.Enabled = true;

            BtnPnlMsgAceptar.Visible = true;
            BtnClousePnlAdd.Visible = true;

            mpeAddMensaje.Show();
        }

        public int ExisteRegistroArchivo(string CajaTxtPnlAdd, string tablaConsultar, string columnaConsultar, string compararContra)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT A.* " +
                                   " FROM " + tablaConsultar + " AS A " +
                                  " INNER JOIN ITM_57 AS AU ON A.IdArchivo = AU.IdArchivo " +
                                  " WHERE A." + columnaConsultar + " = '" + CajaTxtPnlAdd + "'" +
                                  compararContra +
                                    " AND A.IdStatus = 1";
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    Conecta.Cerrar();
                    cmd.Dispose();
                    da.Dispose();
                    dt.Dispose();
                    return 0;
                }
                Conecta.Cerrar();
                return 1;
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
            return 1;
        }

        public int GetIdArchivoMax()
        {

            int IdArchivoMax = 0;

            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();
            string sqlQuery = "SELECT COALESCE(MAX(IdArchivo), 0) + 1 IdArchivo " +
                                " FROM ITM_58 ";

            SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    IdArchivoMax = Convert.ToInt32(dr["IdArchivo"].ToString().Trim());
                }
            }

            Conecta.Cerrar();
            cmd.Dispose();
            dr.Dispose();

            return IdArchivoMax;

        }

        protected string InsertArchivoUbic(int IdArchivoMax, string sIdNivel, int iIdNivel)
        {
            string QueryInsertArchivoUbic = string.Empty;
            QueryInsertArchivoUbic = "INSERT INTO ITM_57 (IdArchivo, " + sIdNivel + ") " +
                                         " VALUES (" + IdArchivoMax + ", " + iIdNivel + ")";
            return QueryInsertArchivoUbic;
        }

        protected void GridArchivosPaq_DataBound(object sender, EventArgs e)
        {

        }

        protected void GridArchivosPaq_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sReferencia = GridArchivosPaq.SelectedRow.Cells[0].Text;

        }

        protected void GridArchivosPaq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GridArchivosPaq, "Select$" + e.Row.RowIndex.ToString()) + ";");

            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[6].Visible = false;
                e.Row.Cells[7].Visible = false;
                e.Row.Cells[8].Visible = false;
                e.Row.Cells[9].Visible = false;
            }
        }

        protected void GetDirectoriosDdlPnl()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT Id_Directorio, Nom_Directorio " +
                                        " FROM ITM_51 " +
                                        " WHERE IdStatus = 1 "
                                        /*+ " WHERE IdAseguradora = " + ddlAseguradora.SelectedValue*/;

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlPnlMsAdd.DataSource = dt;

                ddlPnlMsAdd.DataValueField = "Id_Directorio";
                ddlPnlMsAdd.DataTextField = "Nom_Directorio";

                ddlPnlMsAdd.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ddlPnlMsAdd.Items.Insert(0, new ListItem("-- No Hay Directorio --", "0"));
                }
                else
                {
                    ddlPnlMsAdd.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                }

                Conecta.Cerrar();
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        //estee es el evento para el vento de eliminar
        protected void ImgRechazado_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
                int index = row.RowIndex;
                //GetRegimenDdlPnl();
                //GetTpoDoc();
                //GetSeccion();
                int iIdArchivo = Convert.ToInt32(GridArchivosPaq.Rows[index].Cells[0].Text);
                string sNomArchivo = GridArchivosPaq.Rows[index].Cells[1].Text;
                ddlPnlMsAdd.SelectedValue = GridArchivosPaq.Rows[index].Cells[6].Text;
                // ddlPnlMsgFiscal.SelectedValue = GridArchivosPaq.Rows[index].Cells[7].Text;
                ddlPnlMsgSeccion.SelectedValue = GridArchivosPaq.Rows[index].Cells[8].Text;
                ddlPnlMsgTpDoc.SelectedValue = GridArchivosPaq.Rows[index].Cells[9].Text;
                Variables.wIdArchivo = iIdArchivo;
                Variables.wIdentificadorBtn = 10;

                //GetDirectoriosDdlPnl();
                LblTitlePnlAdd.Text = "Eliminar Archivo";
                // LblPnlMsgFiscal.Visible = true;
                // ddlPnlMsgFiscal.Visible = true;
                // ddlPnlMsgFiscal.Enabled = false;
                LblTitlePnlAdd.Visible = true;
                LblDdlPnlMsgAdd.Text = "Indique la carpeta donde se va a guardar";
                LblDdlPnlMsgAdd.Visible = true;
                ddlPnlMsAdd.Visible = true;
                ddlPnlMsAdd.Enabled = false;
                LblMsgPnlAdd.Text = "Escribir el Nombre del Archivo.";
                LblMsgPnlAdd.Visible = true;
                TxtPnlMsgAdd.Text = sNomArchivo;
                TxtPnlMsgAdd.Enabled = false;
                LblDdlPnlMsgTpDoc.Visible = true;
                ddlPnlMsgTpDoc.Visible = true;
                ddlPnlMsgTpDoc.Enabled = false;
                LblPnlMsgSeccion.Visible = true;
                ddlPnlMsgSeccion.Visible = true;
                ddlPnlMsgSeccion.Enabled = false;
                BtnPnlMsgAceptar.Visible = true;
                BtnClousePnlAdd.Visible = true;
                mpeAddMensaje.Show();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetRegimenDdlPnl()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT IdRegimen, Descripcion " +
                                        " FROM ITM_49 " +
                                        " WHERE IdStatus =  1"
                                        /*+ " WHERE IdAseguradora = " + ddlAseguradora.SelectedValue*/;

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                //ddlPnlMsgFiscal.DataSource = dt;

                //ddlPnlMsgFiscal.DataValueField = "IdRegimen";
                //ddlPnlMsgFiscal.DataTextField = "Descripcion";

                //ddlPnlMsgFiscal.DataBind();

                //if (dt.Rows.Count == 0)
                //{
                //    ddlPnlMsgFiscal.Items.Insert(0, new ListItem("-- No Hay Directorio --", "0"));
                //}
                //else
                //{

                //    ddlPnlMsgFiscal.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                //    ddlPnlMsgFiscal.SelectedValue = "0";
                //}

                Conecta.Cerrar();
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        //este es el evento para el boton de editar
        protected void ImgAceptado_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)sender).NamingContainer);
                int index = row.RowIndex;
                // GetRegimenDdlPnl();
                // GetTpoDoc();
                // GetSeccion();
                int iIdArchivo = Convert.ToInt32(GridArchivosPaq.Rows[index].Cells[0].Text);
                string sNomArchivo = GridArchivosPaq.Rows[index].Cells[1].Text;
                ddlPnlMsAdd.SelectedValue = GridArchivosPaq.Rows[index].Cells[6].Text;
                //ddlPnlMsgFiscal.SelectedValue = GridArchivosPaq.Rows[index].Cells[7].Text;
                ddlPnlMsgSeccion.SelectedValue = GridArchivosPaq.Rows[index].Cells[8].Text;
                ddlPnlMsgTpDoc.SelectedValue = GridArchivosPaq.Rows[index].Cells[9].Text;

                Variables.wIdArchivo = iIdArchivo;
                Variables.wIdentificadorBtn = 11;

                //GetDirectoriosDdlPnl();
                LblTitlePnlAdd.Text = "Editar Archivo";
                // LblPnlMsgFiscal.Visible = true;
                // ddlPnlMsgFiscal.Visible = true;
                // ddlPnlMsgFiscal.Enabled = true;
                LblTitlePnlAdd.Visible = true;
                LblDdlPnlMsgAdd.Text = "Indique la carpeta a guardar";
                LblDdlPnlMsgAdd.Visible = true;
                ddlPnlMsAdd.Visible = true;
                ddlPnlMsAdd.Enabled = true;
                LblMsgPnlAdd.Text = "Escribir el Nombre del Archivo.";
                LblMsgPnlAdd.Visible = true;
                TxtPnlMsgAdd.Text = sNomArchivo;
                TxtPnlMsgAdd.Enabled = true;
                LblDdlPnlMsgTpDoc.Visible = true;
                ddlPnlMsgTpDoc.Visible = true;
                ddlPnlMsgTpDoc.Enabled = true;
                LblPnlMsgSeccion.Visible = true;
                ddlPnlMsgSeccion.Visible = true;
                ddlPnlMsgSeccion.Enabled = true;
                BtnPnlMsgAceptar.Visible = true;
                BtnClousePnlAdd.Visible = true;

                mpeAddMensaje.Show();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void BtnAddRegimen_Click(object sender, EventArgs e)
        {
            Variables.wIdentificadorBtn = 7;
            LblTitlePnlAdd.Text = "Agregar Regimen Fiscal";
            LblTitlePnlAdd.Visible = true;
            // LblPnlMsgFiscal.Visible = false;
            // ddlPnlMsgFiscal.Enabled = false;
            // ddlPnlMsgFiscal.SelectedValue = "0";
            // ddlPnlMsgFiscal.Visible = false;
            LblDdlPnlMsgAdd.Text = "Indique la carpeta donde se va a guardar";
            LblDdlPnlMsgAdd.Visible = false;
            ddlPnlMsAdd.Enabled = false;
            ddlPnlMsAdd.Visible = false;
            LblMsgPnlAdd.Text = "Escribir el Nombre del Regimen Fiscal.";
            LblMsgPnlAdd.Visible = true;
            TxtPnlMsgAdd.Text = "";
            TxtPnlMsgAdd.Enabled = true;
            TxtPnlMsgAdd.Visible = true;
            LblDdlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Enabled = true;
            BtnPnlMsgAceptar.Visible = true;
            BtnClousePnlAdd.Visible = true;
            mpeAddMensaje.Show();
        }

        protected void BtnAddDirectorio_Click(object sender, EventArgs e)
        {
            Variables.wIdentificadorBtn = 8;
            LblTitlePnlAdd.Text = "Agregar Carpeta";
            LblTitlePnlAdd.Visible = true;
            // LblPnlMsgFiscal.Visible = false;
            // ddlPnlMsgFiscal.Enabled = false;
            // ddlPnlMsgFiscal.SelectedValue = "0";
            // ddlPnlMsgFiscal.Visible = false;
            LblDdlPnlMsgAdd.Text = "Indique la carpeta donde se va a guardar";
            LblDdlPnlMsgAdd.Visible = false;
            ddlPnlMsAdd.Enabled = false;
            ddlPnlMsAdd.Visible = false;
            LblMsgPnlAdd.Text = "Escribir el Nombre del Carpeta.";
            LblMsgPnlAdd.Visible = true;
            TxtPnlMsgAdd.Text = "";
            TxtPnlMsgAdd.Enabled = true;
            TxtPnlMsgAdd.Visible = true;
            LblDdlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Visible = false;
            ddlPnlMsgTpDoc.Enabled = true;
            BtnPnlMsgAceptar.Visible = true;
            BtnClousePnlAdd.Visible = true;
            mpeAddMensaje.Show();
        }

        protected void GridArchivosPaq_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            //GridArchivosPaq.PageIndex = e.NewPageIndex;
            //getDocsUsuario();
        }

        protected void GetTpoDoc()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT IdTpoArchivo, Descripcion " +
                                        " FROM ITM_50 "
                                        + " WHERE IdStatus = 1 "
                                        ;
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlPnlMsgTpDoc.DataSource = dt;

                ddlPnlMsgTpDoc.DataValueField = "IdTpoArchivo";
                ddlPnlMsgTpDoc.DataTextField = "Descripcion";

                ddlPnlMsgTpDoc.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ddlPnlMsgTpDoc.Items.Insert(0, new ListItem("-- No Hay Extencion --", "0"));
                }
                else
                {
                    ddlPnlMsgTpDoc.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                }

                Conecta.Cerrar();
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetSeccion()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT IdSeccion, nomSeccion " +
                                        " FROM ITM_59 "
                                        + " WHERE IdStatus = 1 "
                                        ;
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlPnlMsgSeccion.DataSource = dt;

                ddlPnlMsgSeccion.DataValueField = "IdSeccion";
                ddlPnlMsgSeccion.DataTextField = "nomSeccion";

                ddlPnlMsgSeccion.DataBind();

                if (dt.Rows.Count == 0)
                {
                    ddlPnlMsgSeccion.Items.Insert(0, new ListItem("-- No Hay Seccion --", "0"));
                }
                else
                {
                    ddlPnlMsgSeccion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                }

                Conecta.Cerrar();
                cmd.Dispose();
                da.Dispose();
                dt.Dispose();
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }



        protected void ddlRegimen_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlTpoAsunto_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void BtnDelNivel1_Click(object sender, EventArgs e)
        {
            if (ddlNivel1.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccione el nivel 1 a eliminar";
                mpeMensaje.Show();

                return;
            }

            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Eliminar registro(s) tablas (ITM_53)
                string sqlQuery = "DELETE ITM_53 " +
                                  " WHERE IdNivel1 = " + ddlNivel1.SelectedValue + "";

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                int affectedRows = cmd.ExecuteNonQuery();

                Conecta.Cerrar();
                cmd.Dispose();

                GetNivel1Ddl(Convert.ToInt32(ddlConclusion.SelectedValue));

                LblMessage.Text = "Se elimino el nivel 1 correctamente";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

        protected void BtnDelNivel2_Click(object sender, EventArgs e)
        {
            if (ddlNivel2.SelectedValue == "0")
            {
                LblMessage.Text = "Seleccione el nivel 2 a eliminar";
                mpeMensaje.Show();

                return;
            }

            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();

                // Eliminar registro(s) tablas (ITM_54)
                string sqlQuery = "DELETE ITM_54 " +
                                  " WHERE IdNivel2 = " + ddlNivel2.SelectedValue + "";

                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                int affectedRows = cmd.ExecuteNonQuery();

                Conecta.Cerrar();
                cmd.Dispose();

                GetNivel2Ddl(Convert.ToInt32(ddlConclusion.SelectedValue));

                LblMessage.Text = "Se elimino el nivel 2 correctamente";
                mpeMensaje.Show();

            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                this.mpeMensaje.Show();
            }
        }

    }
}