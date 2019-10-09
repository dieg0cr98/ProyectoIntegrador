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
    public class TrabajaEnController : Controller
    {
        private Gr03Proy2Entities5 db = new Gr03Proy2Entities5();

        // GET: TrabajaEn
        public ActionResult Index()
        {
            var trabajaEn = db.TrabajaEn.Include(t => t.Empleado).Include(t => t.Proyecto);
            return View(trabajaEn.ToList());
        }

        // GET: TrabajaEn/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrabajaEn trabajaEn = db.TrabajaEn.Find(id);
            if (trabajaEn == null)
            {
                return HttpNotFound();
            }
            return View(trabajaEn);
        }

        // GET: TrabajaEn/Create
        public ActionResult Create()
        {
            ViewBag.idEmpleadoFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre");
            ViewBag.idProyectoFK = new SelectList(db.Proyecto, "idProyectoAID", "nombre");
            return View();
        }

        // POST: TrabajaEn/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProyectoFK,idEmpleadoFK,rol,estado")] TrabajaEn trabajaEn)
        {
            if (ModelState.IsValid)
            {
                db.TrabajaEn.Add(trabajaEn);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idEmpleadoFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre", trabajaEn.idEmpleadoFK);
            ViewBag.idProyectoFK = new SelectList(db.Proyecto, "idProyectoAID", "nombre", trabajaEn.idProyectoFK);
            return View(trabajaEn);
        }

        // GET: TrabajaEn/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrabajaEn trabajaEn = db.TrabajaEn.Find(id);
            if (trabajaEn == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEmpleadoFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre", trabajaEn.idEmpleadoFK);
            ViewBag.idProyectoFK = new SelectList(db.Proyecto, "idProyectoAID", "nombre", trabajaEn.idProyectoFK);
            return View(trabajaEn);
        }

        // POST: TrabajaEn/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProyectoFK,idEmpleadoFK,rol,estado")] TrabajaEn trabajaEn)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trabajaEn).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idEmpleadoFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre", trabajaEn.idEmpleadoFK);
            ViewBag.idProyectoFK = new SelectList(db.Proyecto, "idProyectoAID", "nombre", trabajaEn.idProyectoFK);
            return View(trabajaEn);
        }

        // GET: TrabajaEn/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrabajaEn trabajaEn = db.TrabajaEn.Find(id);
            if (trabajaEn == null)
            {
                return HttpNotFound();
            }
            return View(trabajaEn);
        }

        // POST: TrabajaEn/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrabajaEn trabajaEn = db.TrabajaEn.Find(id);
            db.TrabajaEn.Remove(trabajaEn);
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
