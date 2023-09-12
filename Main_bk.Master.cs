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
    public partial class Main_bk : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                string nom_page = Request.Url.Segments[Request.Url.Segments.Length - 1].ToUpper();

                switch (nom_page)
                {
                    case "ACCESS":
                        break;
                    case "UPLOAD_FILES":
                        break;
                    case "UPLOAD_FILES_1":
                        break;
                    case "LOGIN":
                        break;
                    case "CAPTCHA":
                        break;
                    case "PAGE_MESSAGE":
                        break;
                    default:
                        lblMenu.Visible = true;

                        BindMenuControl();
                        break;
                }
            }
        }

        protected void BindMenuControl()
        {

            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            // Consulta a la tabla : Usuarios = ITM_02
            //string strQuery = "SELECT Id_Menu, Etiqueta_Menu, Id_Parent_Menu, Url_Menu " +
            //                  "  FROM ITM_41";

            string IdUsuario = Convert.ToString(Session["IdUsuario"]);
            string UsPrivilegios = Convert.ToString(Session["UsPrivilegios"]);

            string strQuery = "Select t3.Id_Menu, t3.Etiqueta_Menu, t3.Id_Parent_Menu, t3.Url_Menu, t1.IdStatus, t2.IdPrivilegio" +
                              "  From ITM_01 t1, ITM_09 t2, ITM_41 t3, ITM_43 t4" +
                              " Where t1.IdPrivilegio = t2.IdPrivilegio " +
                              "   And t3.Id_Menu = t4.Id_Menu" +
                              "   And t2.IdPrivilegio = t4.Id_Privilegio" +
                              "   And t1.IdUsuario = '" + IdUsuario + "'" +
                              "   And t2.IdPrivilegio = " + UsPrivilegios + "";

            SqlCommand cmd = new SqlCommand(strQuery, Conecta.ConectarBD);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);

            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToInt32(row[0]) == Convert.ToInt32(row[2]))
                    {
                        MenuItem miMenuItem = new MenuItem(Convert.ToString(row[1]), Convert.ToString(row[0]), String.Empty, Convert.ToString(row[3]));
                        this.Menu_Dinamico.Items.Add(miMenuItem);

                        AddChildItem(ref miMenuItem, dt);
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                cmd.Dispose();
                Conecta.Cerrar();
            }
        }

        private static void NewMethod(ConexionBD Conecta)
        {
            Conecta.Abrir();
        }

        protected void AddChildItem(ref MenuItem miMenuItem, DataTable dtDataTable)
        {
            foreach (DataRow drDataRow in dtDataTable.Rows)
            {
                if (Convert.ToInt32(drDataRow[2]) == Convert.ToInt32(miMenuItem.Value) && Convert.ToInt32(drDataRow[0]) != Convert.ToInt32(drDataRow[2]))
                {
                    MenuItem miMenuItemChild = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[3]));
                    miMenuItem.ChildItems.Add(miMenuItemChild);

                    AddChildItem(ref miMenuItemChild, dtDataTable);
                }
            }
        }

    }

}