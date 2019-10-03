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
        private Gr03Proy2Entities2 db = new Gr03Proy2Entities2();

        // GET: Requerimientos
        public ActionResult Index()
        {
            var requerimiento = db.Requerimiento.Include(r => r.Empleado).Include(r => r.Proyecto);
            return View(requerimiento.ToList());
        }

        // GET: Requerimientos/Details/5
        public ActionResult Details(int? idRequerimiento, int? idProyecto)
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

            return View(requerimiento);
        }

        // GET: Requerimientos/Create
        public ActionResult Create()
        {
            ViewBag.cedulaTesterFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre");
            ViewBag.idProyectoFK = new SelectList(db.Proyecto, "idProyectoAID", "nombre");
            return View();
        }

        // POST: Requerimientos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idReqPK,idProyectoFK,cedulaTesterFK,nombre,complejidad,tiempoEstimado,tiempoReal,descripcion,fechaDeInicio,fechaDeFinalizacion,estado,horas")] Requerimiento requerimiento)
        {
            if (ModelState.IsValid)
            {
                db.Requerimiento.Add(requerimiento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cedulaTesterFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre", requerimiento.cedulaTesterFK);
            ViewBag.idProyectoFK = new SelectList(db.Proyecto, "idProyectoAID", "nombre", requerimiento.idProyectoFK);
            return View(requerimiento);
        }

        // GET: Requerimientos/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.cedulaTesterFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre", requerimiento.cedulaTesterFK);
            ViewBag.idProyectoFK = new SelectList(db.Proyecto, "idProyectoAID", "nombre", requerimiento.idProyectoFK);
            return View(requerimiento);
        }

        // POST: Requerimientos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idReqPK,idProyectoFK,cedulaTesterFK,nombre,complejidad,tiempoEstimado,tiempoReal,descripcion,fechaDeInicio,fechaDeFinalizacion,estado,horas")] Requerimiento requerimiento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requerimiento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cedulaTesterFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre", requerimiento.cedulaTesterFK);
            ViewBag.idProyectoFK = new SelectList(db.Proyecto, "idProyectoAID", "nombre", requerimiento.idProyectoFK);
            return View(requerimiento);
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
