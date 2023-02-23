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
            GetTpoDoc();

            if (!IsPostBack)
            {

            }
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
                        ConexionBD Conecta = new ConexionBD();
                        Conecta.Abrir();

                        string sqlString = "INSERT INTO ITM_06 " +
                            " (IdTpoDocumento, Descripcion, DescrpBrev) " +
                            " VALUES ('"+ sIdTpoDoc + "','"+ TxtNameDoc.Text +"','"+ TxtAreaMensaje.Value +"')";
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
            ConexionBD Conectar = new ConexionBD();
            Conectar.Abrir();

            string sqlQuery = "SELECT IdTpoDocumento, Descripcion, DescrpBrev " +
                              "  FROM ITM_06 " +
                              " WHERE Status IN (1) ";

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
            TxtNameDoc.Text = Server.HtmlDecode(GrdTpoDocumento.SelectedRow.Cells[1].Text);
            TxtAreaMensaje.Value = Server.HtmlDecode(GrdTpoDocumento.SelectedRow.Cells[2].Text);
        }

    }
}