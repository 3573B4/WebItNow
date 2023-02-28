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
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request.Browser.IsMobileDevice)
                MasterPageFile = "~/Site.Mobile.Master";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                //GetTpoDoc();
                GetProceso();
                GetSubProceso();
            }
            GetTpoDoc();
            BtnCancelar.Visible = false;
            BtnUpdate.Enabled = false;
            //GrdTpoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void BtnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {
            
            if(TxtNameDoc.Text == "" || TxtAreaMensaje.Value == "")
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
                            " (IdTpoDocumento, Descripcion, DescrpBrev) " + //para actualizar el status
                            " VALUES ('"+ sIdTpoDoc + "', " +
                            " TRIM(' ' FROM '"+ TxtNameDoc.Text +"')," +
                            " TRIM(' ' FROM '"+ TxtAreaMensaje.Value +"')" +
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
                        TxtNameDoc.Text = string.Empty;
                        TxtAreaMensaje.Value = string.Empty;
                    }
                    catch(Exception ex)
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

        public void GetTpoDoc()
        {
            
            string sqlQuery = "SELECT IdTpoDocumento, Descripcion, DescrpBrev " +
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
                GrdTpoDocumento.ShowHeaderWhenEmpty = true;
                GrdTpoDocumento.EmptyDataText = "No hay resultados.";
            }

            GrdTpoDocumento.DataSource = dt;
            GrdTpoDocumento.DataBind();

            // Conectar.Cerrar();

            //* * Agrega THEAD y TBODY a GridView.
            GrdTpoDocumento.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void GrdTpoDocumento_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdTpoDocumento, "Select$" + e.Row.RowIndex.ToString()) + ";");
        }

        protected void GrdTpoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            TxtIdTpoDocumento.Text = Server.HtmlDecode(GrdTpoDocumento.SelectedRow.Cells[0].Text);
            TxtNameDoc.Text = Server.HtmlDecode(GrdTpoDocumento.SelectedRow.Cells[1].Text);
            TxtAreaMensaje.Value = Server.HtmlDecode(GrdTpoDocumento.SelectedRow.Cells[2].Text);

            BtnAgregar.Visible = false;
            BtnUpdate.Enabled = true;
            BtnCancelar.Visible = true;
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            if(TxtAreaMensaje.Value != "" || TxtNameDoc.Text != "")
            {
                if(TxtAreaMensaje.Value.Length <= 100 || TxtNameDoc.Text.Length <= 25)
                {
                    modGeneral funGenal = new modGeneral();
                    if(funGenal.valTextGrande(TxtAreaMensaje.Value) == true)
                    {
                        string sIdTpoDoc = TxtIdTpoDocumento.Text;
                        
                        string sqlString = "UPDATE ITM_06 " +
                                            " SET Descripcion = TRIM(' ' FROM '"+ TxtNameDoc.Text +"'), " + 
                                                " DescrpBrev = TRIM(' ' FROM '" + TxtAreaMensaje.Value + "') " +
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

        protected void GrdTpoDocumento_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdTpoDocumento.PageIndex = e.NewPageIndex;
            GetTpoDoc();
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            TxtNameDoc.Text = string.Empty;
            TxtAreaMensaje.Value = string.Empty;

            BtnAgregar.Visible = true;
            BtnUpdate.Enabled = false;
            BtnCancelar.Visible = false;
        }

        protected void ddlstatusActivado_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string option = ddlstatusActivado.SelectedValue;
            GetTpoDoc();
        }

        public void GetProceso()
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdProceso, Nombre " +
                                    " FROM ITM_16 " +
                                    " WHERE IdStatus = 1 ";

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlProceso.DataSource = dt;

                ddlProceso.DataValueField = "IdProceso";
                ddlProceso.DataTextField = "Nombre";

                ddlProceso.DataBind();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        public void GetSubProceso()
        {
            try
            {
                ConexionBD conectar = new ConexionBD();
                conectar.Abrir();

                // Consulta a la tabla Tipo de Documento
                string sqlQuery = "SELECT IdSubProceso, Descripcion " +
                                    " FROM ITM_14 ";

                SqlCommand cmd = new SqlCommand(sqlQuery, conectar.ConectarBD);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlSubProceso.DataSource = dt;

                ddlSubProceso.DataValueField = "IdSubProceso";
                ddlSubProceso.DataTextField = "Descripcion";

                ddlSubProceso.DataBind();
            }
            catch (Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
    }
}