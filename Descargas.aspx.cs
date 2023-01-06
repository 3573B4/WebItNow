using System;
using System.IO;

using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow
{
    public partial class Descargas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["filePath"] != null)
                {
                    //string nombrearchivo = Request.QueryString["Fileid"].ToString();

                    //System.IO.FileStream fs = null;
                    //fs = System.IO.File.Open(Server.MapPath("txtGenerados/" + nombrearchivo + ".txt"), System.IO.FileMode.Open);
                    //byte[] txtbyte = new byte[fs.Length];
                    //fs.Read(txtbyte, 0, Convert.ToInt32(fs.Length));
                    //fs.Close();
                    //Response.AddHeader("Content-disposition", "attachment; filename=" + nombrearchivo + ".txt");
                    //Response.ContentType = "application/octet-stream";
                    //Response.BinaryWrite(txtbyte);
                    //Response.End();

                    string filePath = (string)Session["filePath"];

                    Response.ContentType = ContentType;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                    Response.WriteFile(filePath);
                    Response.End();
                }
            }
        }
    }
}