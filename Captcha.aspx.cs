using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WebItNow_Peacock
{
    public partial class Captcha : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "image/gif";

            using (Bitmap objBitmap = new Bitmap(130, 80))
            using (Graphics objGraphics = Graphics.FromImage(objBitmap))
            {
                objGraphics.Clear(Color.White);
                Random objRandom = new Random();

                // Dibujos aleatorios
                objGraphics.DrawLine(Pens.Black, objRandom.Next(0, 50), objRandom.Next(10, 30), objRandom.Next(0, 200), objRandom.Next(0, 50));
                objGraphics.DrawRectangle(Pens.Blue, objRandom.Next(0, 20), objRandom.Next(0, 20), objRandom.Next(50, 80), objRandom.Next(0, 20));
                objGraphics.DrawLine(Pens.Blue, objRandom.Next(0, 20), objRandom.Next(10, 50), objRandom.Next(100, 200), objRandom.Next(0, 80));

                // Fondo con HatchStyle aleatorio
                HatchStyle[] aHatchStyles = new HatchStyle[]
                {
                    HatchStyle.BackwardDiagonal, HatchStyle.Cross, HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal,
                    HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical, HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross
                };
                using (Brush objBrush = new HatchBrush(aHatchStyles[objRandom.Next(aHatchStyles.Length)],
                    Color.FromArgb(objRandom.Next(100, 255), objRandom.Next(100, 255), objRandom.Next(100, 255)), Color.White))
                {
                    objGraphics.FillRectangle(objBrush, 0, 0, objBitmap.Width, objBitmap.Height);
                }

                // Texto del captcha
                string captchaText = string.Format("{0:X}", objRandom.Next(1000000, 9999999));
                Session["CaptchaVerify"] = captchaText.ToLower();

                using (Font objFont = new Font("Courier New", 15, FontStyle.Bold))
                {
                    objGraphics.DrawString(captchaText, objFont, Brushes.Black, 20, 20);
                }

                objBitmap.Save(Response.OutputStream, ImageFormat.Gif);
            }

            Response.End();
        }
    }
}
