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
            return View(requerimiento.ToList());
        }

        // GET: Requerimientos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requerimiento requerimiento = db.Requerimiento.Find(id);
            if (requerimiento == null)
            {
                return HttpNotFound();
            }
            return View(requerimiento);
        }

        // GET: Requerimientos/Create
        // GET: Requerimientos/Create
        public ActionResult Create(int idProyecto)
        {
            ViewBag.cedulaTesterFK = db.Empleado.Where(e => e.estado == "Disponible" && e.tipoTrabajo == "Tester");
            ViewBag.idProyectoFK = idProyecto;
            return View();
        }

        // POST: Requerimientos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(int idRequerimiento, string nombre, string complejidad, string descripcion, string estado,
            TimeSpan duracionEstimada, DateTime fechai, DateTime fechaf, int idProyecto, string idTester)
        {
            Requerimiento requerimiento = new Requerimiento();
            requerimiento.cedulaTesterFK = idTester;
            requerimiento.complejidad = complejidad;
            requerimiento.descripcion = descripcion;
            requerimiento.estado = estado;
            requerimiento.fechaDeFinalizacion = fechaf;
            requerimiento.fechaDeInicio = fechai;
            requerimiento.horas = TimeSpan.Parse("00:00");
            requerimiento.idProyectoFK = idProyecto;
            requerimiento.idReqPK = idRequerimiento;
            requerimiento.nombre = nombre;
            requerimiento.tiempoEstimado = duracionEstimada;
            requerimiento.tiempoReal = TimeSpan.Parse("00:00");

            db.Requerimiento.Add(requerimiento);
            db.SaveChanges();

            return RedirectToAction("Index", new { idProyecto = idProyecto });
            /*
            else {
                var requerimiento = db.Requerimiento.ToList();
                ViewBag.cedulaTesterFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre", requerimiento.cedulaTesterFK);
                ViewBag.idProyectoFK = new SelectList(db.Proyecto, "idProyectoAID", "nombre", requerimiento.idProyectoFK);
                return View(requerimiento);
            }
            */
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

            ViewBag.cedulaTesterFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre", requerimiento.cedulaTesterFK);
            //ViewBag.idProyectoFK = new SelectList(db.Proyecto, "idProyectoAID", "nombre", requerimiento.idProyectoFK);
            return View(requerimiento);
        }

        // POST: Requerimientos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(int idRequerimiento, string nombre, string complejidad, string descripcion, string estado,
            TimeSpan duracionEstimada, DateTime fechai, DateTime fechaf, int idProyecto, string idTester)
        {

            Requerimiento requerimiento = new Requerimiento();
            requerimiento.cedulaTesterFK = idTester;
            requerimiento.complejidad = complejidad;
            requerimiento.descripcion = descripcion;
            requerimiento.estado = estado;
            requerimiento.fechaDeFinalizacion = fechaf;
            requerimiento.fechaDeInicio = fechai;
            requerimiento.horas = TimeSpan.Parse("00:00");
            requerimiento.idProyectoFK = idProyecto;
            requerimiento.idReqPK = idRequerimiento;
            requerimiento.nombre = nombre;
            requerimiento.tiempoEstimado = duracionEstimada;
            requerimiento.tiempoReal = TimeSpan.Parse("00:00");

            db.Entry(requerimiento).State = EntityState.Modified;
            //db.Requerimiento.Add(requerimiento);
            db.SaveChanges();

            return RedirectToAction("Index", new { idProyecto = idProyecto });
            /*
            if (ModelState.IsValid)
            {
                db.Entry(requerimiento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cedulaTesterFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre", requerimiento.cedulaTesterFK);
            ViewBag.idProyectoFK = new SelectList(db.Proyecto, "idProyectoAID", "nombre", requerimiento.idProyectoFK);
            return View(requerimiento);
            */
        }

        // GET: Requerimientos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requerimiento requerimiento = db.Requerimiento.Find(id);
            if (requerimiento == null)
            {
                return HttpNotFound();
            }
            return View(requerimiento);
        }

        // POST: Requerimientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Requerimiento requerimiento = db.Requerimiento.Find(id);
            db.Requerimiento.Remove(requerimiento);
            db.SaveChanges();
            return RedirectToAction("Index");
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
