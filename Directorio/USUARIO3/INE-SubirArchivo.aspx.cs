using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebItNow
{
    public partial class SubirArchivo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                //Lbl_Message.Text = "selecciono un archivo";
                //Obtener la extesion y el tamaño para delimitar si es necesario
                //string ext = System.IO.Path.GetExtension(FileUpload1.FileName);
                //ext = ext.ToLower();

                ////El tamaño esta en bytes
                //int tamArch = FileUpload1.PostedFile.ContentLength;

                ////podemos llevar a cabo la verificacion de extension y de tamaño
                //if(/*ext==".png" && */tamArch <= 1048576)
                //{
                    FileUpload1.SaveAs(Server.MapPath("./Directorio/" + FileUpload1.FileName));
                    Lbl_Message.Text = "EL archivo se subio exitosamente";
                //}
                //else
                //{
                //    Lbl_Message.Text = "Ocurrio un error al subir el archivo";
                //}

            }
            else {
                Lbl_Message.Text = "Debe seleccionar un archivo";
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {

        }
    }
}