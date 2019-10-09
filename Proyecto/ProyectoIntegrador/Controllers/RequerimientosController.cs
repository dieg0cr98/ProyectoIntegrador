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
        private Gr03Proy2Entities3 db = new Gr03Proy2Entities3();

        // GET: Requerimientos
        public ActionResult Index(int idProyecto)
        {
            var requerimiento = db.Requerimiento.Where(P => P.idProyectoFK == idProyecto);
            ViewBag.idProyecto = idProyecto;
            ViewBag.datosProyecto = db.Proyecto.Where(N => N.idProyectoAID == idProyecto);
                                    
            return View(requerimiento.ToList());
        }

        //Método utilizado para obtener todos los testers disponibles en un proyecto
        private IQueryable<Empleado> getTesters(int idProyecto)
        {
            var availableTestersjoinQuery =
                from p in db.Proyecto //Selecciona la tabla de Proyectos
                join t in db.TrabajaEn on p.idProyectoAID equals t.idProyectoFK //Hace join con la tabla TrabajaEn
                join e in db.Empleado on t.idEmpleadoFK equals e.idEmpleadoPK //Hace join con la tabla Empleado
                join test in db.Tester on e.idEmpleadoPK equals test.idEmpleadoFK // Hace join con la tabla de testers
                where t.idProyectoFK == idProyecto && test.cantidadRequerimientos < 10 // Selecciona los testers que trabajan en ese proyecto y todavía se les puede asignar requerimientos
                select e;
            return availableTestersjoinQuery;
        }

        //Método que se va a encargar de actualizar la cantidad de requerimientos que posee un tester
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
            else {
                Tester testerViejo = db.Tester.Find(idTesterViejo);
                testerViejo.cantidadRequerimientos--;
                db.Entry(testerViejo).State = EntityState.Modified;
            }
            
            db.SaveChanges();

        }

        // GET: Requerimientos/Create
        // GET: Requerimientos/Create
        public ActionResult Create(int idProyecto)
        {
            //ViewBag.cedulaTesterFK = db.Empleado.Where(e => e.estado == "Disponible" && e.tipoTrabajo == "Tester");
            ViewBag.testers = getTesters(idProyecto).ToList();
            ViewBag.idProyectoFK = idProyecto;
            return View();
        }

        // POST: Requerimientos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(int idRequerimiento, string nombre, string complejidad, string descripcion, string estado,
            TimeSpan duracionEstimada, TimeSpan duracionReal, DateTime fechai, DateTime fechaf, int idProyecto, string idTester)
        {
            //Crea la instancia de requerimiento que será agregada si pasa las pruebas necesarias.
            Requerimiento requerimiento = new Requerimiento();
            requerimiento.complejidad = complejidad;
            requerimiento.descripcion = descripcion;
            requerimiento.cedulaTesterFK = idTester;
            requerimiento.estado = estado;
            requerimiento.fechaDeFinalizacion = fechaf;
            requerimiento.fechaDeInicio = fechai;
            requerimiento.horas = TimeSpan.Parse("00:00");
            requerimiento.idProyectoFK = idProyecto;
            requerimiento.tiempoEstimado = duracionEstimada;
            requerimiento.tiempoReal = duracionReal;

            /*
            //Actualiza la cantidad de requerimientos que el tester tiene asignados para 
            actualiceTester(0, idTester, "");
            */
            //Revisa que no exista un requerimiento con el id ingresado por el usuario
            if (db.Requerimiento.Where(i => i.idReqPK == idRequerimiento).FirstOrDefault() != null) {
                ViewBag.error = "Ya existe un requerimiento con el id: " + idRequerimiento;
                ViewBag.cedulaTesterFK = getTesters(idProyecto).ToList();
                ViewBag.idProyectoFK = idProyecto;
                return View(requerimiento);
            }

            requerimiento.idReqPK = idRequerimiento;

            //Revisa que no exista un requerimiento con el nombre ingresado por el usuario
            if (db.Requerimiento.Where(i => i.nombre == nombre).FirstOrDefault() != null)
            {
                ViewBag.error = "Ya existe un requerimiento llamado: " + nombre;
                ViewBag.cedulaTesterFK = getTesters(idProyecto).ToList();
                ViewBag.idProyectoFK = idProyecto;
                return View(requerimiento);
            }

            requerimiento.nombre = nombre;

            //Lo agrega a la BD
            db.Requerimiento.Add(requerimiento);
            db.SaveChanges();

            //Vuelve a la vista de consultar
            return RedirectToAction("Index", new { idProyecto = idProyecto });
        }

        // GET: Requerimientos/Edit/5
        public ActionResult Edit(int? idRequerimiento, int? idProyecto)
        {
            if (idRequerimiento == null || idProyecto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Requerimiento requerimiento = db.Requerimiento.Find(idRequerimiento, idProyecto);

            if (requerimiento == null)
            {
                return HttpNotFound();
            }

            ViewBag.cedulaTesterFK = db.Empleado.Where(e => e.tipoTrabajo == "Tester");
            //ViewBag.testerAsociado = db.Empleado.Where(e => e.idEmpleadoPK == requerimiento.cedulaTesterFK);
            return View(requerimiento);
        }

        // POST: Requerimientos/Edit/5
        [HttpPost]
        public ActionResult Edit(int idRequerimiento, string nombre, string complejidad, string descripcion, string estado,
            TimeSpan duracionEstimada, DateTime fechai, DateTime fechaf, int idProyecto, string idTester)
        {

            Requerimiento requerimiento = db.Requerimiento.Find(idRequerimiento, idProyecto);
            requerimiento.complejidad = complejidad;
            requerimiento.descripcion = descripcion;
            requerimiento.estado = estado;
            requerimiento.fechaDeFinalizacion = fechaf;
            requerimiento.fechaDeInicio = fechai;
            requerimiento.horas = TimeSpan.Parse("00:00");
            requerimiento.idProyectoFK = idProyecto;
            requerimiento.tiempoEstimado = duracionEstimada;
            requerimiento.tiempoReal = TimeSpan.Parse("00:00");
            /*
            //Método utilzado para quitarle un requerimiento a un tester y asignarselo a otro, en terminos de cantidad de requerimientos por tester.
            if (requerimiento.cedulaTesterFK != idTester)
            {
                actualiceTester(1, idTester, requerimiento.cedulaTesterFK);
            }
            */
            requerimiento.cedulaTesterFK = idTester;

            if (db.Requerimiento.Where(i => i.idReqPK == idRequerimiento).FirstOrDefault() != null)
            {
                ViewBag.error = "Ya existe un requerimiento con el id: " + idRequerimiento;
                ViewBag.tipoError = 1;
                ViewBag.cedulaTesterFK = getTesters(idProyecto).ToList();
                ViewBag.idProyectoFK = idProyecto;
                return View(requerimiento);
            }

            requerimiento.idReqPK = idRequerimiento;

            //Revisa que no exista un requerimiento con el nombre ingresado por el usuario
            if (db.Requerimiento.Where(i => i.nombre == nombre).FirstOrDefault() != null)
            {
                ViewBag.error = "Ya existe un requerimiento llamado: " + nombre;
                ViewBag.tipoError = 2;
                ViewBag.cedulaTesterFK = getTesters(idProyecto).ToList();
                ViewBag.idProyectoFK = idProyecto;
                return View(requerimiento);
            }

            requerimiento.nombre = nombre;

            db.Entry(requerimiento).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { idProyecto = idProyecto });
        }

        // POST: Requerimientos/Delete/
        public ActionResult Eliminar(int idRequerimiento, int idProyecto)
        {
            Requerimiento requerimiento = db.Requerimiento.Find(idRequerimiento, idProyecto);
            //actualiceTester(2, "", requerimiento.cedulaTesterFK);
            db.Requerimiento.Remove(requerimiento);
            db.SaveChanges();
            return RedirectToAction("Index", new { idProyecto = idProyecto });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
