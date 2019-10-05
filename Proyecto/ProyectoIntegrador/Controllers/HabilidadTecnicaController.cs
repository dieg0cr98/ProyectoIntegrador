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
    public class HabilidadTecnicaController : Controller
    {
        private Gr03Proy2Entities3 db = new Gr03Proy2Entities3();

        // GET: HabilidadTecnica
        public ActionResult Index()
        {
            var habilidadTecnicas = db.HabilidadTecnica.Include(h => h.Empleado);
            return View(habilidadTecnicas.ToList());
        }

        // GET: HabilidadTecnica/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HabilidadTecnica habilidadTecnica = db.HabilidadTecnica.Find(id);
            if (habilidadTecnica == null)
            {
                return HttpNotFound();
            }
            return View(habilidadTecnica);
        }

        // GET: HabilidadTecnica/Create
        public ActionResult Create()
        {
            ViewBag.idEmpleadoFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre");
            return View();
        }

        // POST: HabilidadTecnica/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEmpleadoFK,habilidad")] HabilidadTecnica habilidadTecnica)
        {
            if (ModelState.IsValid)
            {
                db.HabilidadTecnica.Add(habilidadTecnica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idEmpleadoFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre", habilidadTecnica.idEmpleadoFK);
            return View(habilidadTecnica);
        }

        // GET: HabilidadTecnica/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HabilidadTecnica habilidadTecnica = db.HabilidadTecnica.Find(id);
            if (habilidadTecnica == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEmpleadoFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre", habilidadTecnica.idEmpleadoFK);
            return View(habilidadTecnica);
        }

        // POST: HabilidadTecnica/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEmpleadoFK,habilidad")] HabilidadTecnica habilidadTecnica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(habilidadTecnica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idEmpleadoFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre", habilidadTecnica.idEmpleadoFK);
            return View(habilidadTecnica);
        }

        // GET: HabilidadTecnica/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HabilidadTecnica habilidadTecnica = db.HabilidadTecnica.Find(id);
            if (habilidadTecnica == null)
            {
                return HttpNotFound();
            }
            return View(habilidadTecnica);
        }

        // POST: HabilidadTecnica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HabilidadTecnica habilidadTecnica = db.HabilidadTecnica.Find(id);
            db.HabilidadTecnica.Remove(habilidadTecnica);
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
