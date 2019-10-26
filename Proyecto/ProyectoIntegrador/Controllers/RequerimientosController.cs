﻿using System;
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

        // Métodos comentados porque se utilizaran hasta el siguiente sprint
        // Método utilizado para obtener todos los testers disponibles en un proyecto
        private IEnumerable<Empleado> getTesters(int opc, int idProyecto, string cedulaTester)
        {
            IEnumerable<Empleado> testers = Enumerable.Empty<Empleado>();
            if (opc == 0)
            {
                testers =
                from p in db.Proyecto //Selecciona la tabla de Proyectos
                join t in db.TrabajaEn on p.idProyectoAID equals t.idProyectoFK //Hace join con la tabla TrabajaEn
                join e in db.Empleado on t.idEmpleadoFK equals e.idEmpleadoPK //Hace join con la tabla Empleado
                //join test in db.Tester on e.idEmpleadoPK equals test.idEmpleadoFK // Hace join con la tabla de testers
                where t.idProyectoFK == idProyecto && e.tipoTrabajo == "Tester" //&& test.cantidadRequerimientos < 10 // Selecciona los testers que trabajan en ese proyecto y todavía se les puede asignar requerimientos
                select e;
            }
            else 
            {
                testers =
                from p in db.Proyecto //Selecciona la tabla de Proyectos
                join t in db.TrabajaEn on p.idProyectoAID equals t.idProyectoFK //Hace join con la tabla TrabajaEn
                join e in db.Empleado on t.idEmpleadoFK equals e.idEmpleadoPK //Hace join con la tabla Empleado
                                                                              // join test in db.Tester on e.idEmpleadoPK equals test.idEmpleadoFK // Hace join con la tabla de testers
                where t.idProyectoFK == idProyecto && e.idEmpleadoPK != cedulaTester// && test.cantidadRequerimientos < 10  // Selecciona los testers que trabajan en ese proyecto y todavía se les puede asignar requerimientos
                select e;
            }


            return testers.ToList();
        }

        //Método que se encarga actualizar la cantidad de requerimientos que posee un tester
        private void actualiceTester(int tipo, string idTesterNuevo, string idTesterViejo)
        {
            if (tipo == 0)
            {
                Tester tester = db.Tester.Find(idTesterNuevo);
                tester.cantidadRequerimientos++;
                db.Entry(tester).State = EntityState.Modified;
            }
            else if (tipo == 1)
            {
                Tester testerViejo = db.Tester.Find(idTesterViejo);
                testerViejo.cantidadRequerimientos--;
                Tester testerNuevo = db.Tester.Find(idTesterNuevo);
                testerNuevo.cantidadRequerimientos++;
                db.Entry(testerViejo).State = EntityState.Modified;
                db.Entry(testerNuevo).State = EntityState.Modified;
            }
            else
            {
                Tester testerViejo = db.Tester.Find(idTesterViejo);
                testerViejo.cantidadRequerimientos--;
                db.Entry(testerViejo).State = EntityState.Modified;
            }

            db.SaveChanges();

        }


        // Método que depliega el la consulta sobre los requerimientos del proyecto cuyo id llega como parámetro
        public ActionResult Index(int idProyecto, int idRequerimiento)
        {
            //Se obtienen los datos de todos los requerimientos asociados al proyecto.
            var requerimiento = db.Requerimiento.Where(P => P.idProyectoFK == idProyecto);

            //Se buscan los permisos del usuario que hizo la consulta
            ViewBag.permisosGenerales = seguridad.RequerimientosConsultar(User);
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
            ViewBag.TestersDisponibles = getTesters(0, idProyecto, "");
            ViewBag.idProyecto = idProyecto;
            ViewBag.idRequerimiento = proyectos.GetCantidadRequerimientos(idProyecto) + 1;

            return View();
        }


        // Este método se encarga de recibir los datos del formulario con el que se crea un requerimiento. Primero verifica que sean válidos, de ser así
        // los envía a la BD y de no serlo, vuelve a la vista de crear con un error.
        [HttpPost]
        public ActionResult Create(int idRequerimiento, string nombre, string complejidad, string descripcion, string estado,
            int? duracionEstimada, DateTime? fechai, int idProyecto, string idTester)
        {
            //Crea la instancia de requerimiento que será agregada.
            Requerimiento requerimiento = new Requerimiento();
            requerimiento.nombre = nombre;
            requerimiento.complejidad = complejidad;
            requerimiento.idProyectoFK = idProyecto;
            requerimiento.idReqPK = idRequerimiento;
            requerimiento.descripcion = descripcion;
            //requerimiento.cedulaTesterFK = idTester;

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
             
            //Actualiza la cantidad de requerimientos que el tester tiene asignados.
            //actualiceTester(0, idTester, "");

            //Lo agrega a la BD
            db.Requerimiento.Add(requerimiento);
            db.SaveChanges();

            // Se cambia la nueva cantidad de requerimientos del proyecto para asignar correctamente el siguiente id.
            proyectos.SetCantidadRequerimientos(idProyecto, idRequerimiento);

            //Vuelve a la vista de consultar.
            return RedirectToAction("Index", new { idProyecto, idRequerimiento });
        }

        // Método que llama a la vista de modificar un requerimiento
        public ActionResult Edit(int idRequerimiento, int idProyecto)
        {
            Requerimiento requerimiento = db.Requerimiento.Find(idRequerimiento, idProyecto);

            if (requerimiento == null)
            {
                return HttpNotFound();
            }

            //Comentado pues se usará en el siguiente sprint
            if (requerimiento.cedulaTesterFK != null)
            {
                ViewBag.testerAsociado = empleados.getTesterAsociado(requerimiento.cedulaTesterFK);
                ViewBag.testersDisponibles = getTesters(1, idProyecto, requerimiento.cedulaTesterFK);
            }
            else
            {
                ViewBag.testerAsociado = null;
                ViewBag.testersDisponibles = getTesters(0, idProyecto, "");
            }
            ViewBag.idProyecto = idProyecto;
            ViewBag.idRequerimiento = idRequerimiento;
            return View(requerimiento);
        }

        // Método que recibe los cambios hechos a un requerimiento en el formulario de modificar y los valída antes de enviarlos a la BD.
        [HttpPost]
        public ActionResult Edit(int idProyecto, int idRequerimiento, string nombre, string complejidad, string descripcion, string estado, int duracionEstimada, 
            int? duracionReal, DateTime fechai, DateTime? fechaf, string estadoR, string detalleResultado, string idTester)
        {
            Requerimiento requerimiento = db.Requerimiento.Find(idRequerimiento, idProyecto);
            requerimiento.nombre = nombre;
            requerimiento.complejidad = complejidad;
            requerimiento.descripcion = descripcion;
            requerimiento.estado = estado;
            requerimiento.fechaDeFinalizacion = fechaf;
            requerimiento.fechaDeInicio = fechai;
            requerimiento.idProyectoFK = idProyecto;
            requerimiento.tiempoEstimado = duracionEstimada;
            requerimiento.tiempoReal = duracionReal;
            requerimiento.estadoResultado = estadoR;
            requerimiento.detallesResultado = detalleResultado;
            //requerimiento.cedulaTesterFK = idTester;

            //Método utilzado para quitarle un requerimiento a un tester y asignarselo a otro, en terminos de cantidad de requerimientos por tester.
            //if (requerimiento.cedulaTesterFK != idTester)
            //{
            //    actualiceTester(1, idTester, requerimiento.cedulaTesterFK);
            //}

            //requerimiento.cedulaTesterFK = idTester;

            db.Entry(requerimiento).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = 0 });
        }

        // Método que recibe la confirmación del usuario para eliminar un requerimiento.
        public ActionResult Eliminar(int idRequerimiento, int idProyecto)
        {
            Requerimiento requerimiento = db.Requerimiento.Find(idRequerimiento, idProyecto);
            //Comentado pues se usará en el siguiente sprint
            //actualiceTester(2, "", requerimiento.cedulaTesterFK);
            //db.Requerimiento.Remove(requerimiento);
            requerimiento.estado = "Cancelado";
            db.SaveChanges();
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
    }
}
