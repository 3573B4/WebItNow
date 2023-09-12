using System;
using System.Web;
using System.Web.Util;
using System.Drawing;
using System.Drawing.Imaging;

namespace CatalogView
{
	/// <summary>
	/// Summary description for ThumbGen.
	/// </summary>
	public class ThumbGen : IHttpHandler
	{
		public void ProcessRequest (HttpContext context)
		{
			if (context.Request["path"]!=null)
				//we have a pth try process image
			{
				
				string img=(string)context.Request["path"];
				//here is path for application dir
				string path1=context.Server.MapPath(context.Request.ApplicationPath);
				path1=context.Server.MapPath(img);
				//img=path1+"\\"+img; //full path to image
				img=path1;
				System.Drawing.Image.GetThumbnailImageAbort myCallback =
				new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
				//get bitmap from file
				Bitmap bitmap = new Bitmap(img);
				// process image, create thumbnail. Try use to reduce size, instead of GetThubmnail
				ImageFormat imgformat=bitmap.RawFormat;
				//we will use fixed size for picture, in our case 69x90
				//but easy to add parameters such as width and calculate height
				
				/////////////////this code borrowed from Rick Strahl, http://west-wind.com/weblog
				//works better than NET method GetThumbnail() when need transparent gif
				//Bitmap bmpOut=new Bitmap(70,90);
				//Graphics g=Graphics.FromImage(bmpOut);
				//g.InterpolationMode=System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				//g.FillRectangle(Brushes.White,0,0,69,90);
				//g.DrawImage(bitmap,0,0,69,90);
				///////////////////////////////////////////////////
				// ALternatively you can use 
				System.Drawing.Image imgOut=bitmap.GetThumbnailImage(58,76, myCallback, IntPtr.Zero);
				bitmap.Dispose();
				//but seems creating Black background for transparent GIF's
				//////////////////return our image back to stream
				context.Response.ContentType="image/jpeg";
				imgOut.Save(context.Response.OutputStream, ImageFormat.Jpeg);
				//bmpOut.Save(context.Response.OutputStream, ImageFormat.Jpeg);
				//bmpOut.Dispose();
				imgOut.Dispose();
			}
			
			//added Oct21, 2005. Try return 
			if (context.Request["Dir"]!=null)
			{
			//can we return array to client? Lets try.
				string curGallery=(string) context.Request["Dir"];
				string photodir=System.Configuration.ConfigurationSettings.AppSettings["photofolder"]; //or get from config file
				string[] photos=System.IO.Directory.GetFiles(context.Server.MapPath(photodir+"/"+curGallery),"*.jpg"); 
				int i=System.IO.Directory.GetParent(context.Server.MapPath(photodir)).FullName.Length;
				//
				//get files names/ relative path
				//just get actual files names for thumbnails and photos
				//just removing extra path, leaving only catalog and file name and the same time
				//convert string array to XMl document for returning back to client
				string photoxml="<gallery>";
				for (int ix=0; ix<photos.Length; ix++)
				{
					photos[ix]=photos[ix].Substring(i+1);
					photos[ix]=photos[ix].Replace("\\","/");
					photoxml=photoxml+"<photos>"+photos[ix]+"</photos>";
				}
				photoxml=photoxml+"</gallery>";
				context.Response.Clear();
				context.Response.ContentType="text/xml";
				//need fromat data to return back to client
				context.Response.Write(photoxml);
				context.Response.End();

			}


			
		}




		public bool IsReusable
		{
			get {return true;}
		}
		public bool ThumbnailCallback()
		{
			return false;
		}
	}
}
