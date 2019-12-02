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




        /*
        * Efecto: Obtiene todos los valores requeridos en la vista de Consultas Avanzadas y los añade al viewbag.
        * Requiere: 
        * Modifica: Agrega permisos al ViewBag
        * Retorna:  La vista Index de Consultas Avanzadas
        */
        public ActionResult Index()
        {

            ViewBag.permisosConsultas = seguridad.permisosConsultasAvanzadas(User);

            return View();
        }



        /*
        * Efecto: Obtiene todos los valores requeridos en la primera consulta avanzada  y los devuelve en un Json.
        * Requiere: int permiso. 1= total , 2 = parcial , 3 = Ninguno
        * Modifica: 
        * Retorna:  JsonResult con los testers disponibles y asignados
        */
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

        /*
        * Efecto: Obtiene todos los valores requeridos en la tercera consulta avanzada  y los devuelve en un Json.
        * Requiere: int rol. 0 = Calidad/Soporte, 1= Lider, 2 = Tester, 3 = Cliente
        *           int permiso. 1= total , 2 = parcial , 3 = Ninguno
        *           string idUsuario. Identificacion del usuario
        * Modifica: 
        * Retorna:  JsonResult con los proyectos en los que participa un usuario
        */
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


        /*
        * Efecto: Obtiene todos los valores requeridos para cargar el grafico en la tercera consulta avanzada  y los devuelve en un Json.
        * Requiere: int id. Identificador del proyecto
        * Modifica: 
        * Retorna:  JsonResult con la cantidad de requerimientos asignados a cada tester en un proyecto especifico
        */
        public JsonResult consulta3LoadChart(int id)
        {

            var t = db.USP_CantidadReqATester(id);
            var json = JsonConvert.SerializeObject(t);

            return Json(json, JsonRequestBehavior.AllowGet);

        }


        /*
        * Efecto: Obtiene todos los valores requeridos en la cuarta consulta avanzada  y los devuelve en un Json.
        * Requiere: int rol. 0 = Calidad/Soporte, 1= Lider, 2 = Tester, 3 = Cliente
        *           int permiso. 1= total , 2 = parcial , 3 = Ninguno
        *           string idUsuario. Identificacion del usuario
        * Modifica: 
        * Retorna:  JsonResult con los proyectos en los que participa un usuario
        */
        public JsonResult consulta4(int rol, int permiso, string idUsuario)
        {
            //var t = proyecto.GetProyectosUsuario(permiso, rol, idUsuario);

            var t = db.USP_ObtenerProyectosUsuario(permiso, rol, idUsuario);

            var json = JsonConvert.SerializeObject(t, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });


            return Json(json, JsonRequestBehavior.AllowGet);
        }


        /*
        * Efecto: Obtiene todos los valores requeridos para cargar la tabla de testers en la cuarta consulta avanzada  y los devuelve en un Json.
        * Requiere: int id. Identificador del proyecto
        * Modifica: 
        * Retorna:  JsonResult con los tester asignados a un proyecto especifico
        */
        public JsonResult consulta4LoadTableTesters(int id)
        {

            var t = db.USP_CantidadReqATester(id);
            var json = JsonConvert.SerializeObject(t);

            return Json(json, JsonRequestBehavior.AllowGet);

        }


        /*
        * Efecto: Obtiene todos los valores requeridos para cargar el grafico con las duraciones de testers en la cuarta consulta avanzada  y los devuelve en un Json.
        * Requiere: int id. Identificador del proyecto
        * Modifica: 
        * Retorna:  JsonResult con las duraciones de los tester asignados a un proyecto especifico
        */
        public JsonResult consulta4LoadGraphTester(int id, string idTester)
        {

            var t = db.USP_DuracionReqTester(id, idTester);
            var json = JsonConvert.SerializeObject(t);

            return Json(json, JsonRequestBehavior.AllowGet);

        }


        /*
        * Efecto: Obtiene todos los valores requeridos en la quinta consulta avanzada  y los devuelve en un Json.
        * Requiere: int rol. 0 = Calidad/Soporte, 1= Lider, 2 = Tester, 3 = Cliente
        *           int permiso. 1= total , 2 = parcial , 3 = Ninguno
        *           string idUsuario. Identificacion del usuario
        * Modifica: 
        * Retorna:  JsonResult con las duraciones de los proyectos
        */
        public JsonResult consulta5(int rol, int permiso, string idUsuario)
        {
            var t = db.USP_DuracionesProyecto(permiso, rol, idUsuario);
            var json = JsonConvert.SerializeObject(t);

            return Json(json, JsonRequestBehavior.AllowGet);
        }


        /*
        * Efecto: Obtiene todos los valores requeridos en la sexta consulta avanzada  y los devuelve en un Json.
        * Requiere: int rol. 0 = Calidad/Soporte, 1= Lider, 2 = Tester, 3 = Cliente
        *           int permiso. 1= total , 2 = parcial , 3 = Ninguno
        *           string idUsuario. Identificacion del usuario
        * Modifica: 
        * Retorna:  JsonResult con los testers
        */
        public JsonResult consulta6()
        {
            var t = (from row in db.Empleado where row.tipoTrabajo == "Tester"  select new {row.idEmpleadoPK,row.nombre,row.apellido1,row.apellido2, row.estado });
            var json = JsonConvert.SerializeObject(t);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /*
        * Efecto: Obtiene todos los valores requeridos para cargar las fechas de inicio y fin de testers en la cuarta consulta avanzada  y los devuelve en un Json.
        * Requiere: int id. Identificador del proyecto
        * Modifica: 
        * Retorna:  JsonResult con las fechas de inicio y fin de testers asignados a un proyecto especifico
        */
        public ActionResult consulta6Index(string cedulaTester,string nombre)
        {
            ViewBag.datosTester = Tuple.Create(cedulaTester, nombre);
            ViewBag.listaProyectos = db.USP_FechaInioFinTesterProyecto(cedulaTester).ToList();
            return View();


        }


        /*
        * Efecto: Obtiene todos los valores requeridos para cargar las fechas de inicio y fin de los requerimiento asignados a un testers en un proyecto especifico en la cuarta consulta avanzada  y los devuelve en un Json.
        * Requiere: int id. Identificador del proyecto
        * Modifica: 
        * Retorna:  JsonResult con las fechas de inicio y fin de los requerimiento asignados a un testers en un proyecto especifico
        */
        public JsonResult consulta6Requerimiento(int idProyecto,string cedulaTester)
        {
            var t = db.USP_FechaInicioFinRequerimiento(idProyecto,cedulaTester);
            var json = JsonConvert.SerializeObject(t);

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /*
        * Efecto:
        * Requiere:
        * Modifica:
        * Retorna:
        */
        public JsonResult consulta7(int rol, int permiso, string idUsuario)
        {
            //System.Diagnostics.Debug.WriteLine(rol + " " + permiso + " " + idUsuario);
            var t = db.USP_ObtenerProyectosUsuario(permiso, rol, idUsuario);

            var json = JsonConvert.SerializeObject(t, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });


            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /**
      * Efecto:
      * Requiere:
      * Modifica:
      * Retorna:
      */
        public JsonResult consulta7CargarTesters(int id)
        {
            var t = db.USP_CantidadReqATester(id);
            var json = JsonConvert.SerializeObject(t);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /**
         * Efecto:
         * Requiere:
         * Modifica:
         * Retorna:
         */
        public JsonResult consulta7CompararReqsProyecto(int idProyecto, string idTester, string comp)
        {
            System.Diagnostics.Debug.WriteLine("\nSe entró al controlador para comparar estimaciones\n");
            System.Diagnostics.Debug.WriteLine(idProyecto + " " + idTester + " " + comp);
            var t = db.USP_CompararReqsProyecto(idProyecto, idTester, comp);
            var json = JsonConvert.SerializeObject(t);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        /*
        * Efecto: Obtiene todos los valores requeridos en la cuarta consulta avanzada  y los devuelve en un Json.
        * Requiere: int rol. 0 = Calidad/Soporte, 1= Lider, 2 = Tester, 3 = Cliente
        *           int permiso. 1= total , 2 = parcial , 3 = Ninguno
        *           string idUsuario. Identificacion del usuario
        * Modifica: 
        * Retorna:  JsonResult con los proyectos en los que participa un usuario
        */
        public JsonResult consulta8(int rol, int permiso, string idUsuario)
        {
            var t = db.USP_ObtenerProyectosActivosTerminadosUsuario(permiso, rol, idUsuario);

            var json = JsonConvert.SerializeObject(t, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Json(json, JsonRequestBehavior.AllowGet);
        }


        /*
        * Efecto: Obtiene todos los valores requeridos para cargar la tabla de testers en la cuarta consulta avanzada  y los devuelve en un Json.
        * Requiere: int id. Identificador del proyecto
        * Modifica: 
        * Retorna:  JsonResult con los tester asignados a un proyecto especifico
        */
        public JsonResult consulta8LoadTablePruebas(int id)
        {

            var t = db.USP_ObtenerPruebasRequerimient(id);
            var json = JsonConvert.SerializeObject(t);

            return Json(json, JsonRequestBehavior.AllowGet);

        }
        /*
               * Efecto: Obtiene todos los valores requeridos en la cuarta consulta avanzada  y los devuelve en un Json.
               * Requiere: int rol. 0 = Calidad/Soporte, 1= Lider, 2 = Tester, 3 = Cliente
               *           int permiso. 1= total , 2 = parcial , 3 = Ninguno
               *           string idUsuario. Identificacion del usuario
               * Modifica: 
               * Retorna:  JsonResult con los proyectos en los que participa un usuario
               */
        public JsonResult consulta9(int rol, int permiso, string idUsuario)
        {
            var t = db.USP_ObtenerProyectosActivosTerminadosUsuario(permiso, rol, idUsuario);

            var json = JsonConvert.SerializeObject(t, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Json(json, JsonRequestBehavior.AllowGet);
        }


        /*
        * Efecto: Obtiene todos los valores requeridos para cargar la tabla de testers en la cuarta consulta avanzada  y los devuelve en un Json.
        * Requiere: int id. Identificador del proyecto
        * Modifica: 
        * Retorna:  JsonResult con los tester asignados a un proyecto especifico
        */
        public JsonResult consulta9LoadTableRequerimientos(int id)
        {

            var t = db.USP_ObtenerRequerimientosProyecto(id);
            var json = JsonConvert.SerializeObject(t);

            return Json(json, JsonRequestBehavior.AllowGet);

        }
        /**
         * Efecto: Obtiene los proyectos relacionados al usuario.   
         * Requiere: El rol, permiso y id del usuario que realiza la consulta.
         * Modifica: N/A
         * Retorna: JSON con los proyectos asociados y sus líderes respectivos.
         */
        public JsonResult consulta10(int rol, int permiso, string idUsuario)
        {
            var t = db.USP_ObtenerProyectosActivosTerminadosUsuario(permiso, rol, idUsuario);

            var json = JsonConvert.SerializeObject(t, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /**
         * Efecto: Obtiene el estado de los requerimientos de un proyecto específico que esté activo o finalizado.
         * Requiere: ID del proyecto a consultar.
         * Modifica: N/A
         * Retorna: JSON con el conteo de requerimientos de acuerdo a su estado.
         */
        public JsonResult consulta10EstadoReqs(int idProyecto)
        {
            System.Diagnostics.Debug.WriteLine(idProyecto);
            var t = db.USP_EstadoRequerimientosProyecto(idProyecto);
            var json = JsonConvert.SerializeObject(t);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /*
        * Efecto: Obtiene todos los valores requeridos en la primera consulta avanzada  y los devuelve en un Json.
        * Requiere: int permiso. 1= total , 2 = parcial , 3 = Ninguno
        * Modifica: 
        * Retorna:  JsonResult con los testers disponibles y asignados
        */
        public JsonResult consulta11(int permiso)
        {


            if (permiso != 3)
            {
                var t = db.USP_obtenerRanking(5);
                var json = JsonConvert.SerializeObject(t);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
