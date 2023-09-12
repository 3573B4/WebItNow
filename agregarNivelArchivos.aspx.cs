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
    public partial class agregarNivelArchivos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string error = "Error al borrar";
            this.Page.Response.Write("<script language='JavaScript'>var resultado=window.confirm('" + error + "');");
            this.Page.Response.Write("if(resultado==true){");

            // Código que borra el registro
            this.Page.Response.Write("}else{");
            this.Page.Response.Write("window.alert('Ha introducido cancelar');");
            this.Page.Response.Write("}</script>");

            if (!Page.IsPostBack)
            {
                GetAseguradora();
                ddlConclusion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));
                GetNivel1(0);
                GetNivel2(0);
                GetNivel3(0);
                GetNivel4(0);
                GetArchivosPaq(0);
            }

            //* * Agrega THEAD y TBODY a GridView.
            GrdNivel1.HeaderRow.TableSection = TableRowSection.TableHeader;
            GrdNivel2.HeaderRow.TableSection = TableRowSection.TableHeader;
            GrdNivel3.HeaderRow.TableSection = TableRowSection.TableHeader;
            GrdNivel4.HeaderRow.TableSection = TableRowSection.TableHeader;
            GridArchivosPaq.HeaderRow.TableSection = TableRowSection.TableHeader;

        }
        

        protected void GetAseguradora()
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT TOP (100) IdAseguradora, nomAseguradora " +
                                        " FROM Aseguradora " 
                                        //+ " WHERE IdStatus = 1 "
                                        ;
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlAseguradora.DataSource = dt;

                ddlAseguradora.DataValueField = "IdAseguradora";
                ddlAseguradora.DataTextField = "nomAseguradora";

                ddlAseguradora.DataBind();
                ddlAseguradora.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

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
                string sqlQuery = "SELECT IdProceso, nomProceso " + //conclusion
                                        " FROM Proceso " 
                                        + " WHERE IdAseguradora =" + ddlAseguradora.SelectedValue;
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlConclusion.DataSource = dt;

                ddlConclusion.DataValueField = "IdProceso";
                ddlConclusion.DataTextField = "nomProceso";

                ddlConclusion.DataBind();
                ddlConclusion.Items.Insert(0, new ListItem("-- Seleccionar --", "0"));

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

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

        protected void ddlAseguradora_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetConclusion();
        }

        protected void GetNivel1(int IdProcecso)  
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT IdNivel1, nomNivel1 " + 
                                        " FROM Nivel1 "
                                        + " WHERE IdProceso =" + IdProcecso;
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    GrdNivel1.ShowHeaderWhenEmpty = true;
                    GrdNivel1.EmptyDataText = "No hay resultados.";
                }

                GrdNivel1.DataSource = dt;
                GrdNivel1.DataBind();

                cmd.Dispose();
                da.Dispose();
                dt.Dispose();

                //* * Agrega THEAD y TBODY a GridView.
                GrdNivel1.HeaderRow.TableSection = TableRowSection.TableHeader;
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
            //string sNivelSelec = "IdNivel1";

            int IdNivel2 = 0;
            int IdNivel3 = 0;
            GetNivel1(IdProcecso);
            //GetNivel2(IdNivel1);
            GetNivel3(IdNivel2);
            GetNivel4(IdNivel3);
            GetArchivosPaq(IdProcecso);  //los archivos que correspondan al nivel 1
        }

        protected void GetArchivosPaq(int IdProcecso)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT A.IdArchivo, A.nomArchivo, A.Extencion  " +
                                        " FROM Archivo AS A " +
                                        "   INNER JOIN ArchivoUbicacion AS AU ON A.IdArchivo = AU.IdArchivo "
                                        + " WHERE AU.IdNivel1 = " + IdProcecso +
                                        "     AND A.Estatus = 1 ";
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
        protected void GetNivel2(int IdNivel1)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT IdNivel2, nomNivel2 " +
                                        " FROM Nivel2 "
                                        + " WHERE IdNivel1 =" + IdNivel1;
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    GrdNivel2.ShowHeaderWhenEmpty = true;
                    GrdNivel2.EmptyDataText = "No hay resultados.";
                }

                GrdNivel2.DataSource = dt;
                GrdNivel2.DataBind();

                cmd.Dispose();
                da.Dispose();
                dt.Dispose();

                //* * Agrega THEAD y TBODY a GridView.
                GrdNivel2.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GetNivel3(int IdNivel2)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT IdNivel3, nomNivel3 " +
                                        " FROM Nivel3 "
                                        + " WHERE IdNivel2 = " + IdNivel2;
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    GrdNivel3.ShowHeaderWhenEmpty = true; 
                    GrdNivel3.EmptyDataText = "No hay resultados.";
                }

                GrdNivel3.DataSource = dt;
                GrdNivel3.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdNivel3.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }
        protected void GetNivel4(int IdNivel3)
        {
            try
            {
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT IdNivel4, nomNivel4 " +
                                        " FROM Nivel4 "
                                        + " WHERE IdNivel3 = " + IdNivel3;
                SqlCommand cmd = new SqlCommand(sqlQuery, Conecta.ConectarBD);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    GrdNivel4.ShowHeaderWhenEmpty = true;  //cambiar el nombre del gridview
                    GrdNivel4.EmptyDataText = "No hay resultados.";
                }

                GrdNivel4.DataSource = dt;
                GrdNivel4.DataBind();

                //* * Agrega THEAD y TBODY a GridView.
                GrdNivel4.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            catch (System.Exception ex)
            {
                LblMessage.Text = ex.Message;
                mpeMensaje.Show();
            }
        }

        protected void GrdNivel1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
        }

        protected void GrdNivel1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
        }

        protected void GrdNivel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string sReferencia = GrdRef.SelectedRow.Cells[0].Text;
            int IdProcecso = Convert.ToInt32(ddlConclusion.SelectedValue);
            int iIdNivel1 = Convert.ToInt32(GrdNivel1.SelectedRow.Cells[0].Text);  //toma el Id del Ringlon 

            GetNivel2(iIdNivel1);  // pasa el parametro y va  buscar los niveles
            if(iIdNivel1 == 1)
            {
                GetArchivosPaq(IdProcecso);
            }
            else
            {
                // limpiar la gridvio 
                GetArchivosPaq(0);
            }
        }

        protected void GrdNivel1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdNivel1, "Select$" + e.Row.RowIndex.ToString()) + ";");
        }

        protected void GetArchivosPaqN(int IdNivel2)
        {
            try
            {
                
                ConexionBD Conecta = new ConexionBD();
                Conecta.Abrir();
                string sqlQuery = "SELECT A.IdArchivo, A.nomArchivo, A.Extencion  " +
                                        " FROM Archivo AS A " +
                                        "   INNER JOIN ArchivoUbicacion AS AU ON A.IdArchivo = AU.IdArchivo "
                                        + " WHERE AU.IdNivel2 = " + IdNivel2 +
                                        "     AND Estatus = 1 ";
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

        protected void GrdNivel2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Attributes.Add("OnClick", "" + Page.ClientScript.GetPostBackClientHyperlink(this.GrdNivel2, "Select$" + e.Row.RowIndex.ToString()) + ";");
        }

        protected void GrdNivel2_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void GrdNivel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string sReferencia = GrdRef.SelectedRow.Cells[0].Text;
            int iIdNivel2 = Convert.ToInt32(GrdNivel2.SelectedRow.Cells[0].Text);  //toma el Id del Ringlon 
            string sNivelSelec = "IdNivel2";
            GetNivel3(iIdNivel2);  // pasa el parametro y va  buscar los niveles
            // limpiar la gridvio 
            GetArchivosPaqN(iIdNivel2);
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {

        }

        protected void BtnAddAseguradora_Click(object sender, EventArgs e)
        {
            //LblMsgPnlAdd.Text = "Prueba";
            //mpeAddMensaje.Show();
            LblMessage.Text = "preuba";
            mpeMensaje.Show();
        }
    }
}