using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.BaseDatos;

namespace ProyectoIntegrador.Controllers
{



    public class HomeController : Controller
    {



        public ActionResult Index()
        {



            return View();
        }

        public ActionResult About()
        {
            System.Diagnostics.Debug.WriteLine(Server.MapPath("~/Images"));
            //ViewBag.Message = "Your application description page.";
            //string dirImage = "C:\\Users\\dieg0\\Documents\\Consulta4.png";
            //Image imagen = Image.FromFile(dirImage);
            //var t = imageToByteArray(imagen);
            //var y = byteArrayToImage(t);
            //ViewBag.image = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(t));

            return View();
        }


            [HttpPost]
            public ActionResult About(HttpPostedFileBase file)
            {
                if (file != null && file.ContentLength > 0)
                    try
                    {
                    //string path = Path.Combine(Server.MapPath("~/Images"),
                    //                           Path.GetFileName(file.FileName));
                    //file.SaveAs(path);
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();



                    ViewBag.Message = "File uploaded successfully";
                    ViewBag.pic = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(data));
                }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                else
                {
                    ViewBag.Message = "You have not specified a file.";
                }
                return View();
            }


    public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        // convert image to byte array
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        //Byte array to photo
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}