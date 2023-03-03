using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow
{
    public partial class Catalog_TpoDocument : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getProcesos();
                ddlSubProceso.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

                Llenar_grdGeneral();
            }
            BtnCancelar.Visible = false;
            BtnUpdate.Enabled = false;
            grdTpoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader; //no va aqui por que no esta la gridView
        }

        protected void getProcesos()
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

        }

        protected void getSubProcesos(int iProceso)
        {
            try
            {
                //int gProceso = Convert.ToInt32(ddlSubProceso.SelectedValue);
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
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iProceso = Convert.ToInt32(ddlProceso.SelectedValue);
            getSubProcesos(iProceso);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            if (TxtNameDoc.Text == "" || TxtAreaMensaje.Value == "")
            {
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "* Estos campos son obligatorios";
            }
            else
            {
                if (TxtAreaMensaje.Value.Length <= 100 || TxtNameDoc.Text.Length <= 25)
                {

                    try
                    {

                        int idTpoDoc = Convert.ToInt32(ReturnIdTpoDoc());
                        //idTpoDoc = idTpoDoc + 1;
                        idTpoDoc += 1;
                        string sIdTpoDoc = Convert.ToString(idTpoDoc);
                        if (sIdTpoDoc.Length < 2)
                        {
                            sIdTpoDoc = "0" + sIdTpoDoc;
                        }

                        string sqlString = "INSERT INTO ITM_06 " +
                            " (IdTpoDocumento, Descripcion, DescrpBrev, Status, IdProceso, IdSubProceso) " + //para actualizar el status
                            " VALUES ('" + sIdTpoDoc + "', " +
                            " TRIM(' ' FROM '" + TxtNameDoc.Text + "')," +
                            " TRIM(' ' FROM '" + TxtAreaMensaje.Value + "')," +
                            " 1, " + ddlProceso.SelectedValue+", "+ ddlSubProceso.SelectedValue +
                            " )";

                        //if(rbtnActivo.Checked == true)
                        //{
                        //    sqlString = sqlString + ", 1)";
                        //}else if (rbtnDesactivo.Checked == true)
                        //{
                        //    sqlString = sqlString + ", 0)";
                        //}

                        ConexionBD Conecta = new ConexionBD();
                        Conecta.Abrir();
                        SqlCommand cmd = new SqlCommand(sqlString, Conecta.ConectarBD);

                        cmd.ExecuteReader();

                        GetTpoDoc();
                        //GetTpoDoc(iProceso, iSubProceso);
                        TxtNameDoc.Text = string.Empty;
                        TxtAreaMensaje.Value = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        LblMessage.Text = ex.Message;
                        mpeMensaje.Show();
                    }
                }
                else
                {
                    Lbl_Message.Visible = true;
                    Lbl_Message.Text = "* El texto debe tener 100 o menos caracteres \n y menos o igual a 25 caracteres";
                }
            }
        }

        private string ReturnIdTpoDoc()
        {
            string idTpoDoc = string.Empty;
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            string sqlString = "SELECT TOP 1 IdTpoDocumento " +
                                    " FROM ITM_06 " +
                                    " ORDER BY IdTpoDocumento DESC";
            SqlCommand cmd = new SqlCommand(sqlString, Conecta.ConectarBD);
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {

                while (dr.Read())
                {
                    idTpoDoc = dr["IdTpoDocumento"].ToString().Trim();
                }
            }
            else
            {
                return idTpoDoc;
            }

            cmd.Dispose();
            dr.Dispose();

            Conecta.Cerrar();

            return idTpoDoc;
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (TxtAreaMensaje.Value != "" || TxtNameDoc.Text != "")
            {
                if (TxtAreaMensaje.Value.Length <= 100 || TxtNameDoc.Text.Length <= 25)
                {
                    modGeneral funGenal = new modGeneral();
                    if (funGenal.valTextGrande(TxtAreaMensaje.Value) == true)
                    {
                        string sIdTpoDoc = TxtIdTpoDocumento.Text;

                        string sqlString = "UPDATE ITM_06 " +
                                            " SET Descripcion = TRIM(' ' FROM '" + TxtNameDoc.Text + "'), " +
                                                " DescrpBrev = TRIM(' ' FROM '" + TxtAreaMensaje.Value + "')," +
                                                " Status = 1, " +
                                                " IdProceso = " + ddlProceso.SelectedValue + ", " +
                                                " IdSubProceso = " + ddlSubProceso.SelectedValue +
                                                " WHERE IdTpoDocumento = '" + sIdTpoDoc + "'";

                        //if (rbtnActivo.Checked == true)
                        //{
                        //    sqlString = sqlString + ", status = 1 " +
                        //        " WHERE IdTpoDocumento = '" + sIdTpoDoc + "'";
                        //}
                        //else if (rbtnDesactivo.Checked == true)
                        //{
                        //    sqlString = sqlString + ", status = 0 " +
                        //        " WHERE IdTpoDocumento = '" + sIdTpoDoc + "'";
                        //}

                        ConexionBD Conecta = new ConexionBD();
                        Conecta.Abrir();

                        SqlCommand cmd = new SqlCommand(sqlString, Conecta.ConectarBD);

                        cmd.ExecuteReader();

                        GetTpoDoc();

                        TxtNameDoc.Text = string.Empty;
                        TxtAreaMensaje.Value = string.Empty;

                        BtnAgregar.Visible = true;
                        BtnUpdate.Enabled = false;
                        BtnCancelar.Visible = false;
                        

                    }
                    else
                    {
                        Lbl_Message.Visible = true;
                        Lbl_Message.Text = "* no accepta caracteres raros.";
                    }
                }
                else
                {
                    Lbl_Message.Visible = true;
                    Lbl_Message.Text = "* Excede los limites de caracteres.";
                }
            }
            else
            {
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "* Los campos son abligatorios.";
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

            ddlProceso.ClearSelection();
            ddlSubProceso.ClearSelection();
            TxtNameDoc.Text = string.Empty;
            TxtAreaMensaje.Value = string.Empty;
            getProcesos();
            BtnAgregar.Visible = true;
            BtnUpdate.Enabled = false;
            BtnCancelar.Visible = false;
            
        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }
        public void GetTpoDoc()
        {
            string iProceso = ddlProceso.SelectedValue;
            string iSubProceso = ddlSubProceso.SelectedValue;

            string sqlQuery = "SELECT IdTpoDocumento, Descripcion, DescrpBrev, IdProceso, IdSubProceso " +
                              "  FROM ITM_06 " +
                              " WHERE Status IN (1) " +
                              " AND IdProceso = " + iProceso +
                              " AND IdSubProceso = " + iSubProceso; //le ponia el valor del ddl status doc

            //if (rbtnActivo.Checked == true)
            //{
            //    sqlQuery = sqlQuery + " (1) ";
            //}
            //else if (rbtnDesactivo.Checked == true)
            //{
            //    sqlQuery = sqlQuery + " (0) ";
            //}

            ConexionBD Conectar = new ConexionBD();
            Conectar.Abrir();

            SqlCommand cmd = new SqlCommand(sqlQuery, Conectar.ConectarBD);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);


            if (dt.Rows.Count == 0)
            {
                grdTpoDocumento.ShowHeaderWhenEmpty = true;
                grdTpoDocumento.EmptyDataText = "No hay resultados.";
            }

            grdTpoDocumento.DataSource = dt;
            grdTpoDocumento.DataBind();

            //Conectar.Cerrar();

            ////* * Agrega THEAD y TBODY a GridView.
            grdTpoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void Llenar_grdGeneral()
        {
            try
            {
                string iProceso = ddlProceso.SelectedValue;
                string iSubProceso = ddlSubProceso.SelectedValue;

                string sqlQuery = "SELECT IdTpoDocumento, Descripcion, DescrpBrev, IdProceso, IdSubProceso " +
                                  "  FROM ITM_06 " +
                                  " WHERE Status IN (1) "; //le ponia el valor del ddl status doc

                //if (rbtnActivo.Checked == true)
                //{
                //    sqlQuery = sqlQuery + " (1) ";
                //}
                //else if (rbtnDesactivo.Checked == true)
                //{
                //    sqlQuery = sqlQuery + " (0) ";
                //}

                ConexionBD Conectar = new ConexionBD();
                Conectar.Abrir();

                SqlCommand cmd = new SqlCommand(sqlQuery, Conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);


                if (dt.Rows.Count == 0)
                {
                    grdTpoDocumento.ShowHeaderWhenEmpty = true;
                    grdTpoDocumento.EmptyDataText = "No hay resultados.";
                }

                grdTpoDocumento.DataSource = dt;
                grdTpoDocumento.DataBind();

                //Conectar.Cerrar();

                ////* * Agrega THEAD y TBODY a GridView.
                grdTpoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (Exception e)
            {
                LblMessage.Text = e.Message;
                mpeMensaje.Show();
            }
        }

        protected void grdTpoDocumento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.grdTpoDocumento, "Select$" + e.Row.RowIndex.ToString()) + ";");
        }

        protected void grdTpoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlProceso.ClearSelection();
            //ddlSubProceso.ClearSelection();
            TxtIdTpoDocumento.Text = Server.HtmlDecode(grdTpoDocumento.SelectedRow.Cells[0].Text);
            TxtNameDoc.Text = Server.HtmlDecode(grdTpoDocumento.SelectedRow.Cells[1].Text);
            TxtAreaMensaje.Value = Server.HtmlDecode(grdTpoDocumento.SelectedRow.Cells[2].Text);

            string sIdProceso = Server.HtmlDecode(grdTpoDocumento.SelectedRow.Cells[3].Text);
            //ddlProceso.Items.Insert(0, new ListItem(getProceso(sIdProceso), sIdProceso));

            string sIdSubProceso = Server.HtmlDecode(grdTpoDocumento.SelectedRow.Cells[4].Text);
            //ddlSubProceso.Items.Insert(0, new ListItem(getSubProceso(sIdSubProceso), sIdSubProceso));

            ddlProceso.SelectedValue = sIdProceso;
            int iIdProceso = Convert.ToInt32(sIdProceso);
            getSubProcesos(iIdProceso);
            ddlSubProceso.SelectedValue = sIdSubProceso;

            BtnAgregar.Visible = false;
            BtnUpdate.Enabled = true;
            BtnCancelar.Visible = true;
            grdTpoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void grdTpoDocumento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTpoDocumento.PageIndex = e.NewPageIndex;
            GetTpoDoc();
        }

        //protected string getProceso(string sIdProceso)
        //{
        //    string sProceso = string.Empty;

        //    ConexionBD Conecta = new ConexionBD();
        //    Conecta.Abrir();
        //    string sqlQuery = "SELECT Nombre " +
        //                            " FROM ITM_16 " +
        //                            " WHERE IdProceso = "+sIdProceso;
        //    SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);
        //    SqlDataReader dr = cmd.ExecuteReader();

        //    if (dr.HasRows)
        //    {

        //        while (dr.Read())
        //        {
        //            sProceso = dr["Nombre"].ToString().Trim();
        //        }
        //    }
        //    else
        //    {
        //        return sProceso;
        //    }

        //    cmd.Dispose();
        //    dr.Dispose();

        //    Conecta.Cerrar();

        //    return sProceso;
        //}

        //protected string getSubProceso(string sIdSubProceso)
        //{
        //    string sSubProceso = string.Empty;

        //    ConexionBD Conecta = new ConexionBD();
        //    Conecta.Abrir();
        //    string sqlQuery = "SELECT Descripcion " +
        //                            " FROM ITM_14 " +
        //                            " WHERE IdProceso = " + sIdSubProceso;
        //    SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);
        //    SqlDataReader dr = cmd.ExecuteReader();

        //    if (dr.HasRows)
        //    {

        //        while (dr.Read())
        //        {
        //            sSubProceso = dr["Descripcion"].ToString().Trim();
        //        }
        //    }
        //    else
        //    {
        //        return sSubProceso;
        //    }

        //    cmd.Dispose();
        //    dr.Dispose();

        //    Conecta.Cerrar();

        //    return sSubProceso;
        //}

        protected void ddlSubProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTpoDoc();
        }
    }
}