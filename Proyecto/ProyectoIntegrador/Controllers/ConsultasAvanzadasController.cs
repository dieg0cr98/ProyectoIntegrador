using Newtonsoft.Json;
using ProyectoIntegrador.BaseDatos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoIntegrador.Controllers
{
    public class ConsultasAvanzadasController : Controller
    {
        private SeguridadController seguridad = new SeguridadController();
        private Gr03Proy2Entities6 db = new Gr03Proy2Entities6();
        private EmpleadosController empleados = new EmpleadosController();


        // GET: ConsultasAvanzadas
        public ActionResult Index()
        {
            

 
            return View();
        }


        public ActionResult Index2(int consulta)
        {
            
            ViewBag.consulta = consulta;
            return View();

            if (permiso == 1)
            {
                var t = db.USP_TestersDisponibleAsignado().ToList();
                var json = JsonConvert.SerializeObject(t);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult consulta3(int rol, int permiso, string idUsuario)
        {

            var t = proyecto.GetProyectosUsuario(permiso, rol, idUsuario);
            var json = JsonConvert.SerializeObject(t, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });


            return Json(json, JsonRequestBehavior.AllowGet);

        }
        public JsonResult consulta3LoadChart(int id)
        {

            var t = db.USP_CantidadReqATester(id);
            var json = JsonConvert.SerializeObject(t);

            return Json(json, JsonRequestBehavior.AllowGet);

        }


    }
}