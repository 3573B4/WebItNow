using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow
{
    public partial class Menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                //
            }

        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }


        protected void MyMenu_MenuItemClick(object sender, MenuEventArgs e)
        {

        }

        protected void BtnClose_Click(object sender, EventArgs e)
        {

        }

    }
}