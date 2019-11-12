﻿using Newtonsoft.Json;
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

            ViewBag.permisosConsultas = seguridad.permisosConsultasAvanzadas(User);

            return View();
        }




        public JsonResult consulta1(int permiso)
        {

           
            if (permiso != 3)
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

            //var t = proyecto.GetProyectosUsuario(permiso, rol, idUsuario);

            var t = db.USP_ObtenerProyectosUsuario(permiso,rol,idUsuario);

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

        public JsonResult consulta4(int rol, int permiso,string idUsuario)
        {
            //var t = proyecto.GetProyectosUsuario(permiso, rol, idUsuario);

            var t = db.USP_ObtenerProyectosUsuario(permiso, rol, idUsuario);

            var json = JsonConvert.SerializeObject(t, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });


            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult consulta4LoadTableTesters(int id)
        {

            var t = db.USP_CantidadReqATester(id);
            var json = JsonConvert.SerializeObject(t);

            return Json(json, JsonRequestBehavior.AllowGet);

        }

        public JsonResult consulta4LoadGraphTester(int id, string idTester)
        {

            var t = db.USP_DuracionReqTester(id, idTester);
            var json = JsonConvert.SerializeObject(t);

            return Json(json, JsonRequestBehavior.AllowGet);

        }

        public JsonResult consulta5(int rol, int permiso, string idUsuario)
        {
            var t = db.USP_DuracionesProyecto(permiso, rol, idUsuario);
            var json = JsonConvert.SerializeObject(t);

            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public JsonResult consulta6()
        {
            var t = (from row in db.Empleado where row.tipoTrabajo == "Tester"  select new {row.idEmpleadoPK,row.nombre,row.apellido1,row.apellido2, row.estado });
            var json = JsonConvert.SerializeObject(t);

            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public ActionResult indexTester(string cedulaTester,string nombre)
        {
            ViewBag.datosTester = Tuple.Create(cedulaTester, nombre);
            ViewBag.listaProyectos = db.USP_FechaInioFinTesterProyecto(cedulaTester).ToList();
            return View();


        }

        public JsonResult consulta6Proyecto(string cedulaTester)
        {
            var t = db.USP_FechaInioFinTesterProyecto(cedulaTester).ToList();
            var json = JsonConvert.SerializeObject(t);

            return Json(json, JsonRequestBehavior.AllowGet);
        }


        public JsonResult consulta6Requerimiento(int idProyecto,string cedulaTester)
        {
            var t = db.USP_FechaInicioFinRequerimiento(idProyecto,cedulaTester);
            var json = JsonConvert.SerializeObject(t);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}