using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace WebItNow
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string script = "$(document).ready(function () { $('[id*=btnSubmit]').click(); });";
                ClientScript.RegisterStartupScript(this.GetType(), "load", script, true);
            }
        }

        protected void Submit(object sender, EventArgs e)
        {
            // Add Fake Delay to simulate long running process.
            System.Threading.Thread.Sleep(5000);
            this.LoadCustomers();
        }

        private void LoadCustomers()
        {
            string constr = "Data Source=.\\SQL2019;Initial Catalog=northwind;user id=sa;password=pass@123";
            string query = "SELECT CustomerId, ContactName, City FROM Customers WHERE Country = @Country OR @Country = ''";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@Country", ddlCountries.SelectedItem.Value);
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            gvCustomers.DataSource = dt;
                            gvCustomers.DataBind();
                        }
                    }
                }
            }
        }

    }
}