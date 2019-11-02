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
        private ProyectosController proyecto = new ProyectosController();





        // GET: ConsultasAvanzadas
        public ActionResult Index()
        {
            

 
            return View();
        }


        public ActionResult Index2(int consulta)
        {
            
            ViewBag.consulta = consulta;
            return View();
        }

        public JsonResult consulta1()
        {

            var t = db.USP_TestersDisponibleAsignado().ToList();
            var json = JsonConvert.SerializeObject(t);
            //var t2 = db.testersActivos().ToList();
            //var t = empleados.GetTestersDisponibles();
            //var tuple = Tuple.Create(t, t2);

            //var json = JsonConvert.SerializeObject(tuple);

            return Json(json, JsonRequestBehavior.AllowGet);

        }

        public JsonResult consulta3()
        {

            var t = db.Proyecto.ToList();
            var json = JsonConvert.SerializeObject(t, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            //var t2 = db.testersActivos().ToList();
            //var t = empleados.GetTestersDisponibles();
            //var tuple = Tuple.Create(t, t2);

            //var json = JsonConvert.SerializeObject(tuple);

            return Json(json, JsonRequestBehavior.AllowGet);

        }
        public JsonResult consulta3LoadChart(int id)
        {

            var t = db.Requerimiento.Where(r => r.idProyectoFK == id).ToList();
            var json = JsonConvert.SerializeObject(t, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Json(json, JsonRequestBehavior.AllowGet);

        }
        




    }
}