using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.BaseDatos;

namespace ProyectoIntegrador.Controllers
{



    public class HomeController : Controller
    {


        private Gr03Proy2Entities3 db = new Gr03Proy2Entities3();



        public ActionResult getDepartment()
        {
            System.Diagnostics.Debug.WriteLine("asdasdadaddasdasd");
            return Json(db.Empleado.Select(x => new
            {
                DepartmentID = x.idEmpleadoPK,
                DepartmentName = x.nombre
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {

            ViewBag.empleados = db.Empleado.ToList();
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}