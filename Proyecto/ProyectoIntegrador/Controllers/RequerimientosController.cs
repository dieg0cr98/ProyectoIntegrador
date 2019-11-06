using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.BaseDatos;

namespace ProyectoIntegrador.Controllers
{
    public class RequerimientosController : Controller
    {
        private Gr03Proy2Entities6 db = new Gr03Proy2Entities6();
        private SeguridadController seguridad = new SeguridadController();
        private ProyectosController proyectos = new ProyectosController();
        private EmpleadosController empleados = new EmpleadosController();

        // Método que depliega el la consulta sobre los requerimientos del proyecto cuyo id llega como parámetro
        public ActionResult Index(int idProyecto, int idRequerimiento)
        {

            //Se buscan los permisos del usuario que hizo la consulta
            ViewBag.permisosGenerales = seguridad.RequerimientosConsultar(User);

            //Se obtienen los datos de todos los requerimientos asociados al proyecto.
            var requerimiento = db.Requerimiento.Where(P => P.idProyectoFK == idProyecto && P.estado != "Cancelado");

            //Se guarda la selección que se debe desplegar automáticamente a la hora de llamar la vista de consulta.
            ViewBag.seleccion = idRequerimiento;
            //Se guarda el id del proyecto asociado para ser usado al llamar el CRUD de cualquier requerimiento.
            ViewBag.idProyecto = idProyecto;
            //Obtiene el nombre del proyecto asociado para mostrarlo en la consulta.
            ViewBag.nombre = proyectos.GetNombreProyecto(idProyecto);

            return View(requerimiento.ToList());
        }

        //Método que despliega los datos de la vista utilizada para crear un requerimiento.
        public ActionResult Create(int idProyecto)
        {

            //Se buscan los permisos del usuario que hizo la consulta
            ViewBag.permisosAgregar = seguridad.RequerimientosAgregar(User);
            
            //Se solicitan todos los testers asignables al requerimiento
            ViewBag.TestersDisponibles = db.TestersAsignables(idProyecto).ToList();
            ViewBag.idProyecto = idProyecto;

            return View();
        }


        // Este método se encarga de recibir los datos del formulario con el que se crea un requerimiento. Primero verifica que sean válidos, de ser así
        // los envía a la BD y de no serlo, vuelve a la vista de crear con un error.
        [HttpPost]
        public ActionResult Create(string nombre, string complejidad, string descripcion, string estado,
            int? duracionEstimada, DateTime? fechai, int idProyecto, string idTester)
        {
            //Crea la instancia de requerimiento que será agregada.
            Requerimiento requerimiento = new Requerimiento();
            requerimiento.nombre = nombre;
            requerimiento.complejidad = complejidad;
            requerimiento.idProyectoFK = idProyecto;
            requerimiento.idReqPK = 0;
            requerimiento.descripcion = descripcion;

            //Valída los datos que podrían ser nulos.
            if (estado != "")
            {
                requerimiento.estado = estado;
            }

            if (duracionEstimada != null)
            {
                requerimiento.tiempoEstimado = (int)duracionEstimada;
            }

            if (fechai != null)
            {
                requerimiento.fechaDeInicio = (DateTime)fechai;
            }

            if (idTester != "")
            {
                requerimiento.cedulaTesterFK = idTester;
            }

            //Lo agrega a la BD
            db.Requerimiento.Add(requerimiento);
            db.SaveChanges();

            var idReq = db.Requerimiento.Where(R => R.nombre == requerimiento.nombre).FirstOrDefault();

            //Vuelve a la vista de consultar.
            return RedirectToAction("Index", new { idProyecto, idRequerimiento = idReq.idReqPK });
        }

        // Método que llama a la vista de modificar un requerimiento
        public ActionResult Edit(int idRequerimiento, int idProyecto)
        {
            Requerimiento requerimiento = db.Requerimiento.Find(idRequerimiento, idProyecto);

            if (requerimiento == null)
            {
                return HttpNotFound();
            }

            //Se solicitan los permisos del usuario para analizar su puede editar, o no!
            ViewBag.permisosEditar = seguridad.RequerimientosEditar(User);

            //Comentado pues se usará en el siguiente sprint
            ViewBag.testersDisponibles = db.TestersAsignables(idProyecto).ToList();
            ViewBag.idProyecto = idProyecto;
            ViewBag.idRequerimiento = idRequerimiento;
            ViewBag.testerAsociado = requerimiento.cedulaTesterFK;
            return View(requerimiento);
        }

        // Método que recibe los cambios hechos a un requerimiento en el formulario de modificar y los valída antes de enviarlos a la BD.
        [HttpPost]
        public ActionResult Edit(int idProyecto, int idRequerimiento, string nombre, string complejidad, string descripcion, string estado, int? duracionEstimada, 
            int? duracionReal, DateTime fechai, DateTime? fechaf, string resultado, string estadoR, string detalleResultado, string idTester)
        {
            Requerimiento requerimiento = db.Requerimiento.Find(idRequerimiento, idProyecto);
            requerimiento.nombre = nombre;
            requerimiento.complejidad = complejidad;
            requerimiento.descripcion = descripcion;
            requerimiento.fechaDeFinalizacion = fechaf;
            requerimiento.idProyectoFK = idProyecto;
            requerimiento.tiempoReal = duracionReal;
            requerimiento.detallesResultado = detalleResultado;

            if (estado != "")
            {
                requerimiento.estado = estado;
            }

            if (duracionEstimada != null)
            {
                requerimiento.tiempoEstimado = (int)duracionEstimada;
            }

            if (fechai != null)
            {
                requerimiento.fechaDeInicio = (DateTime)fechai;
            }

            if (idTester != "")
            {
                requerimiento.cedulaTesterFK = idTester;
            }
            else
            {
                requerimiento.cedulaTesterFK = null;
            }

            if (resultado != "")
            {
                if(resultado == "true")
                {
                    requerimiento.resultado = true;
                }
                else
                {
                    requerimiento.resultado = false;
                }
            }

            if (estadoR != "")
            {
                requerimiento.estadoResultado = estadoR;
            }

            db.Entry(requerimiento).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = requerimiento.idReqPK });
        }

        // Método que recibe la confirmación del usuario para eliminar un requerimiento.
        public ActionResult Eliminar(int idRequerimiento, int idProyecto)
        {
            if (Request.IsAuthenticated)
            {
                Requerimiento requerimiento = db.Requerimiento.Find(idRequerimiento, idProyecto);
                 db.Requerimiento.Remove(requerimiento);
                //requerimiento.estado = "Cancelado";
                //db.Entry(requerimiento).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = 0 });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Método que se encarga de verificar que el nombre del requerimiento que voy a ingresar, sea único.
        // Recibe string name. Contiene el nombre que se quiere verificar
        //        string oldName. Contiene el nombre actual del reqeurimiento (Se utiliza en caso de editar un requerimiento)
        // Devuelve un JsonResult con un True si ya existe un requerimiento y un false si no
        public JsonResult ReviseNombreRequerimiento(string name, string oldName)
        {
            //Hay que verificar si el nuevo nombre ya existe en la base de datos
            if ((name != oldName) && (db.Requerimiento.Where(r => r.nombre == name).FirstOrDefault() != null))
            {
                //Existe un proyecto con ese nombre
                return new JsonResult { Data = false };
            }
            else
                return new JsonResult { Data = true };

        }

        public JsonResult GetNombreTester(int idRequerimiento, int idProyecto)
        {
            return new JsonResult { Data = db.nombreTester(idProyecto, idRequerimiento) };
        }
    }
}
