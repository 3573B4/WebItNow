using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

using System.Data;
using System.Text;

using System.Data.OleDb;
using System.Configuration;

using System.Runtime.InteropServices;
//using Excel = Microsoft.Office.Interop.Excel;

namespace WebFONAC
{
    public partial class Carga_Nomina : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Session["UserId"] = 1;

                this.Form.Attributes.Add("autocomplete", "off");
                // Permisos Usuario
                string UsPrivilegios = System.Web.HttpContext.Current.Session["UsPrivilegios"] as String;

                if (UsPrivilegios == "User")
                {
                    BtnPrimerCarga.Enabled = false;
                    BtnSegundaCarga.Enabled = false;
                    BtnTercerCarga.Enabled = false;
                    BtnDefinitivo.Enabled = false;
                    BtnLimpiar.Enabled = false;
                }

                Carga_Totales_Nomina();
            }

            //if (Session["UserId"] == null)
            //{
            //    LblMessage.Text = "La sesión ha expirado.";
            //    this.mpeMensaje.Show();

            //    Response.Redirect("Acceso.aspx");
            //}

        }
        protected void BtnPrimerCarga_Click(object sender, EventArgs e)
        {
            Subir_Archivo();

            if (FileUpload1.PostedFile.ContentLength > 0)
            { 
                Carga_Arch_Nomina();
                Totales_Nomina_1();
                Inicializar_Carga_Nomina();

                // inhabilitar Botón Primer Carga
                BtnPrimerCarga.Enabled = false;
            }
        }
        protected void BtnSegundaCarga_Click(object sender, EventArgs e)
        {
            Subir_Archivo();
            if (FileUpload1.PostedFile.ContentLength > 0)
            {
                Carga_Arch_Nomina();
                Totales_Nomina_2();
                Inicializar_Carga_Nomina();

                // inhabilitar Botón Segunda Carga
                BtnSegundaCarga.Enabled = false;
            }
            ////Access the File using the Name of HTML INPUT File.
            //HttpPostedFile postedFile = Request.Files["oFile"];

            ////Check if File is available.
            //if (postedFile != null && postedFile.ContentLength > 0)
            //{
            //    //Save the File.
            //    string filePath = Server.MapPath("~/Uploads/") + Path.GetFileName(postedFile.FileName);
            //    postedFile.SaveAs(filePath);
            //    lblMessage.Visible = true;
            //}

        }
        protected void BtnTercerCarga_Click(object sender, EventArgs e)
        {
            Subir_Archivo();
            if (FileUpload1.PostedFile.ContentLength > 0)
            {
                Carga_Arch_Nomina();
                Totales_Nomina_3();
                Inicializar_Carga_Nomina();

                // inhabilitar Botón Tercer Carga
                BtnTercerCarga.Enabled = false;
            }
        }
        protected void BtnDefinitivo_Click(object sender, EventArgs e)
        {
            Subir_Archivo();
            if (FileUpload1.PostedFile.ContentLength > 0)
            {
                Carga_Arch_Nomina();
                Totales_Nomina_4();
                Inicializar_Carga_Nomina();

                // inhabilitar Botón Definitivo Carga
                BtnDefinitivo.Enabled = false;
            }
        }
        protected void BtnLimpiar_Click(object sender, EventArgs e)
        {

            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            MySqlCommand cmd1 = new MySqlCommand("Delete From totales_nomina", Conecta.ConectarBD);
            MySqlDataReader dr1 = cmd1.ExecuteReader();


            cmd1.Dispose();
            dr1.Dispose();

            Conecta.Cerrar();

            // habilita botones de carga
            BtnPrimerCarga.Enabled = true;
            BtnSegundaCarga.Enabled = true;
            BtnTercerCarga.Enabled = true;
            BtnDefinitivo.Enabled = true;

            Lbl_Mensaje.Text = string.Empty;

            Limpia(this.Controls);
        }
        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Menu.aspx");
        }
        protected void Carga_Totales_Nomina()
        {

            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            MySqlCommand cmd1 = new MySqlCommand("Select * From totales_nomina", Conecta.ConectarBD);
            MySqlDataReader dr1 = cmd1.ExecuteReader();

            while (dr1.Read())
            {
                int Id_Carga = int.Parse(dr1["Id_Carga"].ToString().Trim());

                if (Id_Carga == 1)
                {
                    Txt_Casos_B1.Text = dr1["Casos_B"].ToString().Trim();
                    Txt_Aportaciones_B1.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr1["Aportaciones_B"].ToString().Trim()));
                    Txt_Casos_C1.Text = dr1["Casos_C"].ToString().Trim();
                    Txt_Aportaciones_C1.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr1["Aportaciones_C"].ToString().Trim()));

                    BtnPrimerCarga.Enabled = false;
                }
                else if (Id_Carga == 2)
                {
                    Txt_Casos_B2.Text = dr1["Casos_B"].ToString().Trim();
                    Txt_Aportaciones_B2.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr1["Aportaciones_B"].ToString().Trim()));
                    Txt_Casos_C2.Text = dr1["Casos_C"].ToString().Trim();
                    Txt_Aportaciones_C2.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr1["Aportaciones_C"].ToString().Trim()));

                    BtnSegundaCarga.Enabled = false;
                }
                else if (Id_Carga == 3)
                {
                    Txt_Casos_B3.Text = dr1["Casos_B"].ToString().Trim();
                    Txt_Aportaciones_B3.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr1["Aportaciones_B"].ToString().Trim()));
                    Txt_Casos_C3.Text = dr1["Casos_C"].ToString().Trim();
                    Txt_Aportaciones_C3.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr1["Aportaciones_C"].ToString().Trim()));

                    BtnTercerCarga.Enabled = false;
                }
                else if (Id_Carga == 4)
                {
                    Txt_Casos_B4.Text = dr1["Casos_B"].ToString().Trim();
                    Txt_Aportaciones_B4.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr1["Aportaciones_B"].ToString().Trim()));
                    Txt_Casos_C4.Text = dr1["Casos_C"].ToString().Trim();
                    Txt_Aportaciones_C4.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr1["Aportaciones_C"].ToString().Trim()));

                    BtnDefinitivo.Enabled = false;
                }
                else
                {

                }
            }

            cmd1.Dispose();
            dr1.Dispose();

            Conecta.Cerrar();
        }
        protected void Carga_Arch_Nomina()
        {
            //int Id_Carga
            //string fileName = "carga_nom" + Id_Carga + ".xls";
            string fileName = FileUpload1.PostedFile.FileName;
          //string path1 = @"~\ArchNomina\";
            string path = HttpContext.Current.Server.MapPath("/ArchNomina/");
            string strExcelFile = path + fileName;

            //string strExcelFile = @"C:\MySQL\carga_nom1.xls";
            string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strExcelFile + ";Extended Properties=Excel 8.0";

            // Crea el objeto de conexión
            OleDbConnection oledbConn = new OleDbConnection(connString);
            try
            {
                // Conexión abierta
                oledbConn.Open();

                // Cree el objeto OleDbCommand y seleccione los datos de la hoja de trabajo Hoja1
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Hoja1$]", oledbConn);

                // Crear nuevo OleDbDataAdapter
                OleDbDataAdapter oleda = new OleDbDataAdapter();

                oleda.SelectCommand = cmd;

                // Cree un DataSet que contendrá los datos extraídos de la hoja de trabajo
                DataSet ds = new DataSet();

                // Rellene el DataSet de los datos extraídos de la hoja de trabajo.
                oleda.Fill(ds, "Carga_Nomina");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string strQuery = "Insert into carga_nomina Values ";

                    foreach (DataTable table in ds.Tables)
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            Variables.wNum_Emp = int.Parse(dr["NUM_EMP"].ToString());
                            Variables.wRfc = dr["RFC"].ToString();
                            Variables.wCodigo = dr["CODIGO"].ToString();
                            Variables.wAgrup = dr["AGRUP"].ToString();
                            Variables.wD_21_SDO = decimal.Parse(dr["D_21_SDO"].ToString());

                            strQuery += "(" + Variables.wNum_Emp + ",'" + Variables.wRfc + "','" + Variables.wCodigo + "','" + Variables.wAgrup + "'," + Variables.wD_21_SDO + "),";

                        }
                    }

                    ConexionBD Conecta = new ConexionBD();
                    Conecta.Abrir();

                    strQuery = strQuery.TrimEnd(',');

                    MySqlCommand cmd1 = new MySqlCommand(strQuery, Conecta.ConectarBD);
                    MySqlDataReader dr1 = cmd1.ExecuteReader();

                    cmd1.Dispose();
                    dr1.Dispose();

                    Conecta.Cerrar();
                }

            }
            catch (OleDbException exception)
            {
                // MessageBox.Show("ERROR: " + hoja + " " + path + " " + exception.Message);
                LblMessage.Text = exception.Message;
            }
            finally
            {
                // Cerrar Conexion
                oledbConn.Close();
            }

        }
        protected void Totales_Nomina_1()
        {

            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            string strTipoB = "BASE";
            MySqlCommand cmd1 = new MySqlCommand("Select Count(*) Casos_B , Sum(D_21_SDO) Aportaciones_B " +
                                            "From carga_nomina " +
                                            "Where AGRUP = '" + strTipoB + "'"
                                            , Conecta.ConectarBD);
            MySqlDataReader dr1 = cmd1.ExecuteReader();

            if (dr1.HasRows)
            {
                while (dr1.Read())
                {
                    if (dr1[0].ToString().Trim() != "0")
                    {
                        Txt_Casos_B1.Text = dr1[0].ToString().Trim();
                        Txt_Aportaciones_B1.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr1[1].ToString().Trim()));
                    }
                }
            }

            cmd1.Dispose();
            dr1.Dispose();

            string strTipoC = "CONF";
            MySqlCommand cmd2 = new MySqlCommand("Select Count(*) Casos_C , Sum(D_21_SDO) Aportaciones_C " +
                                            "From carga_nomina " +
                                            "Where AGRUP = '" + strTipoC + "'"
                                            , Conecta.ConectarBD);
            MySqlDataReader dr2 = cmd2.ExecuteReader();

            if (dr2.HasRows)
            {
                while (dr2.Read())
                {
                    if (dr2[0].ToString().Trim() != "0")
                    {
                        Txt_Casos_C1.Text = dr2[0].ToString().Trim();
                        Txt_Aportaciones_C1.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr2[1].ToString().Trim()));
                    }
                }
            }
            cmd2.Dispose();
            dr2.Dispose();

            Variables.wId_Carga = 1;
            Variables.wCasos_B = int.Parse(Txt_Casos_B1.Text);
            Variables.wAportaciones_B = decimal.Parse(Txt_Aportaciones_B1.Text);
            Variables.wCasos_C = int.Parse(Txt_Casos_C1.Text);
            Variables.wAportaciones_C = decimal.Parse(Txt_Aportaciones_C1.Text);

            MySqlCommand cmd8 = new MySqlCommand("Insert into totales_nomina (Id_Carga, Casos_B, Aportaciones_B, Casos_C, Aportaciones_C) " +
                "values (" + Variables.wId_Carga + ", " + Variables.wCasos_B + "," + Variables.wAportaciones_B + "," + Variables.wCasos_C + "," + Variables.wAportaciones_C + ")",
                Conecta.ConectarBD);
            MySqlDataReader dr8 = cmd8.ExecuteReader();

            cmd8.Dispose();
            dr8.Dispose();

            Conecta.Cerrar();

        }
        protected void Totales_Nomina_2()
        {

            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            string strTipoB = "BASE";
            MySqlCommand cmd1 = new MySqlCommand("Select Count(*) Casos_B , Sum(D_21_SDO) Aportaciones_B " +
                                            "From carga_nomina " +
                                            "Where AGRUP = '" + strTipoB + "'"
                                            , Conecta.ConectarBD);
            MySqlDataReader dr1 = cmd1.ExecuteReader();

            if (dr1.HasRows)
            {
                while (dr1.Read())
                {
                    if (dr1[0].ToString().Trim() != "0")
                    {
                        Txt_Casos_B2.Text = dr1[0].ToString().Trim();
                        Txt_Aportaciones_B2.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr1[1].ToString().Trim()));
                    }
                }
            }

            cmd1.Dispose();
            dr1.Dispose();

            string strTipoC = "CONF";
            MySqlCommand cmd2 = new MySqlCommand("Select Count(*) Casos_C , Sum(D_21_SDO) Aportaciones_C " +
                                            "From carga_nomina " +
                                            "Where AGRUP = '" + strTipoC + "'"
                                            , Conecta.ConectarBD);
            MySqlDataReader dr2 = cmd2.ExecuteReader();

            if (dr2.HasRows)
            {
                while (dr2.Read())
                {
                    if (dr2[0].ToString().Trim() != "0")
                    {
                        Txt_Casos_C2.Text = dr2[0].ToString().Trim();
                        Txt_Aportaciones_C2.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr2[1].ToString().Trim()));
                    }
                }
            }
            cmd2.Dispose();
            dr2.Dispose();

            Variables.wId_Carga = 2;
            Variables.wCasos_B = int.Parse(Txt_Casos_B2.Text);
            Variables.wAportaciones_B = decimal.Parse(Txt_Aportaciones_B2.Text);
            Variables.wCasos_C = int.Parse(Txt_Casos_C2.Text);
            Variables.wAportaciones_C = decimal.Parse(Txt_Aportaciones_C2.Text);

            MySqlCommand cmd3 = new MySqlCommand("Insert into totales_nomina (Id_Carga, Casos_B, Aportaciones_B, Casos_C, Aportaciones_C) " +
                "values (" + Variables.wId_Carga + ", " + Variables.wCasos_B + "," + Variables.wAportaciones_B + "," + Variables.wCasos_C + "," + Variables.wAportaciones_C + ")",
                Conecta.ConectarBD);
            MySqlDataReader dr3 = cmd3.ExecuteReader();

            cmd3.Dispose();
            dr3.Dispose();

            Conecta.Cerrar();

        }
        protected void Totales_Nomina_3()
        {

            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            string strTipoB = "BASE";
            MySqlCommand cmd1 = new MySqlCommand("Select Count(*) Casos_B , Sum(D_21_SDO) Aportaciones_B " +
                                            "From carga_nomina " +
                                            "Where AGRUP = '" + strTipoB + "'"
                                            , Conecta.ConectarBD);
            MySqlDataReader dr1 = cmd1.ExecuteReader();

            if (dr1.HasRows)
            {
                while (dr1.Read())
                {
                    if (dr1[0].ToString().Trim() != "0")
                    {
                        Txt_Casos_B3.Text = dr1[0].ToString().Trim();
                        Txt_Aportaciones_B3.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr1[1].ToString().Trim()));
                    }
                }
            }

            cmd1.Dispose();
            dr1.Dispose();

            string strTipoC = "CONF";
            MySqlCommand cmd2 = new MySqlCommand("Select Count(*) Casos_C , Sum(D_21_SDO) Aportaciones_C " +
                                            "From carga_nomina " +
                                            "Where AGRUP = '" + strTipoC + "'"
                                            , Conecta.ConectarBD);
            MySqlDataReader dr2 = cmd2.ExecuteReader();

            if (dr2.HasRows)
            {
                while (dr2.Read())
                {
                    if (dr2[0].ToString().Trim() != "0")
                    {
                        Txt_Casos_C3.Text = dr2[0].ToString().Trim();
                        Txt_Aportaciones_C3.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr2[1].ToString().Trim()));
                    }
                }
            }
            cmd2.Dispose();
            dr2.Dispose();

            Variables.wId_Carga = 3;
            Variables.wCasos_B = int.Parse(Txt_Casos_B3.Text);
            Variables.wAportaciones_B = decimal.Parse(Txt_Aportaciones_B3.Text);
            Variables.wCasos_C = int.Parse(Txt_Casos_C3.Text);
            Variables.wAportaciones_C = decimal.Parse(Txt_Aportaciones_C3.Text);

            MySqlCommand cmd3 = new MySqlCommand("Insert into totales_nomina (Id_Carga, Casos_B, Aportaciones_B, Casos_C, Aportaciones_C) " +
                "values (" + Variables.wId_Carga + ", " + Variables.wCasos_B + "," + Variables.wAportaciones_B + "," + Variables.wCasos_C + "," + Variables.wAportaciones_C + ")",
                Conecta.ConectarBD);
            MySqlDataReader dr3 = cmd3.ExecuteReader();

            cmd3.Dispose();
            dr3.Dispose();

            Conecta.Cerrar();

        }
        protected void Totales_Nomina_4()
        {

            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            string strTipoB = "BASE";
            MySqlCommand cmd1 = new MySqlCommand("Select Count(*) Casos_B , Sum(D_21_SDO) Aportaciones_B " +
                                            "From carga_nomina " +
                                            "Where AGRUP = '" + strTipoB + "'"
                                            , Conecta.ConectarBD);
            MySqlDataReader dr1 = cmd1.ExecuteReader();

            if (dr1.HasRows)
            {
                while (dr1.Read())
                {
                    if (dr1[0].ToString().Trim() != "0")
                    {
                        Txt_Casos_B4.Text = dr1[0].ToString().Trim();
                        Txt_Aportaciones_B4.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr1[1].ToString().Trim()));
                    }
                }
            }

            cmd1.Dispose();
            dr1.Dispose();

            string strTipoC = "CONF";
            MySqlCommand cmd2 = new MySqlCommand("Select Count(*) Casos_C , Sum(D_21_SDO) Aportaciones_C " +
                                            "From carga_nomina " +
                                            "Where AGRUP = '" + strTipoC + "'"
                                            , Conecta.ConectarBD);
            MySqlDataReader dr2 = cmd2.ExecuteReader();

            if (dr2.HasRows)
            {
                while (dr2.Read())
                {
                    if (dr2[0].ToString().Trim() != "0")
                    {
                        Txt_Casos_C4.Text = dr2[0].ToString().Trim();
                        Txt_Aportaciones_C4.Text = string.Format("{0:###,###,###,##0.00##}", Decimal.Parse(dr2[1].ToString().Trim()));
                    }
                }
            }
            cmd2.Dispose();
            dr2.Dispose();

            Variables.wId_Carga = 4;
            Variables.wCasos_B = int.Parse(Txt_Casos_B4.Text);
            Variables.wAportaciones_B = decimal.Parse(Txt_Aportaciones_B4.Text);
            Variables.wCasos_C = int.Parse(Txt_Casos_C4.Text);
            Variables.wAportaciones_C = decimal.Parse(Txt_Aportaciones_C4.Text);

            MySqlCommand cmd3 = new MySqlCommand("Insert into totales_nomina (Id_Carga, Casos_B, Aportaciones_B, Casos_C, Aportaciones_C) " +
                "values (" + Variables.wId_Carga + ", " + Variables.wCasos_B + "," + Variables.wAportaciones_B + "," + Variables.wCasos_C + "," + Variables.wAportaciones_C + ")",
                Conecta.ConectarBD);
            MySqlDataReader dr3 = cmd3.ExecuteReader();

            cmd3.Dispose();
            dr3.Dispose();

            Conecta.Cerrar();

        }
        protected void Inicializar_Carga_Nomina()
        {
            ConexionBD Conecta = new ConexionBD();
            Conecta.Abrir();

            MySqlCommand cmd1 = new MySqlCommand("Delete From carga_nomina", Conecta.ConectarBD);
            MySqlDataReader dr1 = cmd1.ExecuteReader();


            cmd1.Dispose();
            dr1.Dispose();

            Conecta.Cerrar();

        }
        public void Limpia(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
                else if (control is DropDownList)
                    ((DropDownList)control).Items.Clear();
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
        protected void Subir_Archivo()
        {
            Lbl_Mensaje.Text = string.Empty;

            if (FileUpload1.HasFile)
            {
                try
                {
                  //FileUpload1.SaveAs(HttpContext.Current.Server.MapPath("/ArchNomina/") + FileUpload1.FileName);
                    FileUpload1.SaveAs(HttpContext.Current.Server.MapPath(".") + "/ArchNomina/" + FileUpload1.FileName);
                    Lbl_Mensaje.Text = "Archivo " + FileUpload1.PostedFile.FileName + " cargado correctamente.";
                }
                catch (Exception ex)
                {
                    // error message
                    Lbl_Mensaje.Text = ex.Message;
                    Lbl_Mensaje.Text = "Se produjo algún problema al cargar el archivo. Por favor intente mas tarde.";
                }
            }
            else
            {
                // advertencia mensaje
                Lbl_Mensaje.Text = "Por favor, elija un archivo para cargar.";
            }
        }
    }
}
