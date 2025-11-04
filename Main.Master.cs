using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow_Peacock
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // (!Page.IsPostBack || Session["Idioma"] != null)
            if (!Page.IsPostBack)
            {
                string script = $@"
                    idLblExpira = '{LblExpira.ClientID}';
                    idLitSesionExpirada = '{litSesionExpirada.ClientID}';
                    idPnlExpira = '{pnlExpira.ClientID}';
                ";
                
                ScriptManager.RegisterStartupScript(this, GetType(), "InitExpiraIDs", script, true);

                string nom_page = Request.Url.Segments[Request.Url.Segments.Length - 1].ToUpper();

                lblMenuSpan.Text = GetGlobalResourceObject("GlobalResources", "lblMenu").ToString();
                    
                litSesionExpirada.Text = GetGlobalResourceObject("GlobalResources", "msgSesionExpirada").ToString();
                BtnClose_Expira.Text = GetGlobalResourceObject("GlobalResources", "btnCerrar").ToString();

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

                        LblMenuUsuario.Text = Convert.ToString(Session["IdUsuario"]);
                        BtnMenu.Visible = true;

                        LblMenuUsuario.Text = Convert.ToString(Session["IdUsuario"]);

                        if ((LblMenuUsuario.Text == "") || (LblMenuUsuario.Text == null))
                        {
                            Response.Redirect("Login.aspx");
                        }

                        // Supón que ModuloAcceso se obtiene de la sesión o de algún otro valor.
                        string ModuloAcceso = Session["ModuloAcceso"] as string;

                        if (ModuloAcceso == "1")
                        {
                            // Mostrar el menú para Siniestros
                            Menu_Dinamico.Visible = true;
                            // Puedes agregar más lógica para modificar el menú si es necesario
                        }
                        else if (ModuloAcceso == "2")
                        {
                            // Mostrar el menú para Gastos Médicos
                            Menu_Dinamico.Visible = true;
                            // Agregar lógica específica para Gastos Médicos
                        }
                        else
                        {
                            // Si no es un valor válido para ModuloAcceso, ocultar el menú
                            Menu_Dinamico.Visible = false;
                        }

                        BindMenuControl();
                        break;
                }


            }

        }

        protected void BindMenuControl()
        {

            ConexionBD_MySQL dbConn = new ConexionBD_MySQL(Variables.wUserName, Variables.wPassword);
            dbConn.Open();

            string strQuery = string.Empty;

            string IdUsuario = Convert.ToString(Session["IdUsuario"]);
            string UsPrivilegios = Convert.ToString(Session["UsPrivilegios"]);
            string ModuloAcceso = Convert.ToString(Session["ModuloAcceso"]);

            string idioma = Session["Idioma"]?.ToString() ?? "es-MX";
            string columnaEtiqueta = "Etiqueta_Menu"; // Español por defecto

            if (idioma == "en-US") columnaEtiqueta = "Etiqueta_Menu_EN";
            else if (idioma == "pt-BR") columnaEtiqueta = "Etiqueta_Menu_PT";


            if (ModuloAcceso == "1")
            {
                //strQuery = "Select t3.Id_Menu, t3.Etiqueta_Menu, t3.Id_Parent_Menu, t3.Url_Menu, t1.IdStatus, t2.IdPrivilegio " +
                //           "  From ITM_02 t1, ITM_09 t2, ITM_41 t3, ITM_43 t4" +
                //           " Where t1.IdPrivilegio = t2.IdPrivilegio " +
                //           "   And t3.Id_Menu = t4.Id_Menu " +
                //           "   And t2.IdPrivilegio = t4.Id_Privilegio " +
                //           "   And t1.IdUsuario = '" + IdUsuario + "' " +
                //           "   And t2.IdPrivilegio = " + UsPrivilegios + "";

                strQuery = $@"SELECT t3.Id_Menu, t3.{columnaEtiqueta}, t3.Id_Parent_Menu, t3.Url_Menu, t1.IdStatus, t2.IdPrivilegio
                                FROM ITM_02 t1
                                JOIN ITM_09 t2 ON t1.IdPrivilegio = t2.IdPrivilegio
                                JOIN ITM_43 t4 ON t2.IdPrivilegio = t4.Id_Privilegio
                                JOIN ITM_41 t3 ON t3.Id_Menu = t4.Id_Menu
                               WHERE t1.IdUsuario = '{IdUsuario}' 
                                 AND t2.IdPrivilegio = {UsPrivilegios}";
            }
            else if (ModuloAcceso == "2")
            {
                strQuery = "Select t3.Id_Menu, t3.Etiqueta_Menu, t3.Id_Parent_Menu, t3.Url_Menu, t1.IdStatus, t2.IdPrivilegio " +
                           "  From ITM_02 t1, ITM_09 t2, ITM_40 t3, ITM_42 t4" +
                           " Where t1.IdPrivilegio = t2.IdPrivilegio " +
                           "   And t3.Id_Menu = t4.Id_Menu " +
                           "   And t2.IdPrivilegio = t4.Id_Privilegio " +
                           "   And t1.IdUsuario = '" + IdUsuario + "' " +
                           "   And t2.IdPrivilegio = " + UsPrivilegios + "";
            }

            DataTable dt = dbConn.ExecuteQuery(strQuery);

            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToInt32(row[0]) == Convert.ToInt32(row[2]))
                    {
                        // MenuItem miMenuItem = new MenuItem(Convert.ToString(row[1]), Convert.ToString(row[0]), String.Empty, Convert.ToString(row[3]).Trim());
                        MenuItem miMenuItem = new MenuItem(Convert.ToString(row[columnaEtiqueta]), Convert.ToString(row[0]), String.Empty, Convert.ToString(row[3]).Trim());

                        this.Menu_Dinamico.Items.Add(miMenuItem);

                        AddChildItem(ref miMenuItem, dt, columnaEtiqueta);
                    }
                }

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                dbConn.Close();
            }
        }

        protected void AddChildItem(ref MenuItem miMenuItem, DataTable dtDataTable, string columnaEtiqueta)
        {
            foreach (DataRow drDataRow in dtDataTable.Rows)
            {
                if (Convert.ToInt32(drDataRow[2]) == Convert.ToInt32(miMenuItem.Value) && Convert.ToInt32(drDataRow[0]) != Convert.ToInt32(drDataRow[2]))
                {
                    // MenuItem miMenuItemChild = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[3]));
                    MenuItem miMenuItemChild = new MenuItem(Convert.ToString(drDataRow[columnaEtiqueta]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[3]));

                    miMenuItem.ChildItems.Add(miMenuItemChild);

                    AddChildItem(ref miMenuItemChild, dtDataTable, columnaEtiqueta);
                }
            }
        }

        protected void btnImgMenuCardLogout_Click(object sender, ImageClickEventArgs e)
        {
            Variables.wUserName = string.Empty;
            Variables.wPassword = string.Empty;

            Response.Redirect("Login.aspx");
        }

        protected void BtnHome_Click(object sender, ImageClickEventArgs e)
        {
            string ModuloAcceso = Convert.ToString(Session["ModuloAcceso"]);

            if (ModuloAcceso == "1")
            {
                // Siniestros
                Response.Redirect("Mnu_Dinamico.aspx", false);
                return;
            }
            else if (ModuloAcceso == "2")
            {
                // GastosMedicos
                Response.Redirect("fwGM_Mnu_Dinamico.aspx", false);
                return;
            }

            //Response.Redirect("Mnu_Dinamico.aspx", true);
            //return;
        }
    }
}