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
    public partial class Menu_Dinamico : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //BindMenuControl();
        }

        protected void BindMenuControl()
        {

            ConexionBD Conecta = new ConexionBD();
            NewMethod(Conecta);

            // Consulta a la tabla : Usuarios = ITM_02
            string strQuery = "SELECT Id_Menu, Etiqueta_Menu, Id_Parent_Menu, Url_Menu " +
                              "  FROM ITM_41";

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
                        // this.MyMenu.Items.Add(miMenuItem);

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