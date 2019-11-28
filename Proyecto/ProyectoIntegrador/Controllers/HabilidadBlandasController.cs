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
    public class HabilidadBlandasController : Controller
    {
        private Gr03Proy2Entities6 db = new Gr03Proy2Entities6();
        private EmpleadosController empleados = new EmpleadosController();
        // GET: HabilidadBlandas


        public IEnumerable<ProyectoIntegrador.BaseDatos.HabilidadBlanda> GetHabilidadUsuario(string idUsuario)
        {
            if (db.HabilidadBlanda.Where(h => h.idEmpleadoFK == idUsuario).ToList() != null)
            {
                return db.HabilidadBlanda.Where(h => h.idEmpleadoFK == idUsuario).ToList();
            }
            else
            {
                return null;
            }
            

            }
            public ActionResult Index(string id)
        {
            ViewBag.nombre = empleados.GetNombreEmpleado(id);
            ViewBag.empl = db.Empleado.Find(id);
            var hblandas = GetHabilidadUsuario(id);
            return View(hblandas.ToList());
        }

        // GET: HabilidadBlandas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HabilidadBlanda habilidadBlanda = db.HabilidadBlanda.Find(id);
            if (habilidadBlanda == null)
            {
                return HttpNotFound();
            }
            return View(habilidadBlanda);
        }

        // GET: HabilidadBlandas/Create
        public ActionResult Create()
        {
            ViewBag.idEmpleadoFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre");
            return View();
        }

        // POST: HabilidadBlandas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idEmpleadoFK,habilidad")] HabilidadBlanda habilidadBlanda)
        {
            if (ModelState.IsValid)
            {
                db.HabilidadBlanda.Add(habilidadBlanda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idEmpleadoFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre", habilidadBlanda.idEmpleadoFK);
            return View(habilidadBlanda);
        }

        // GET: HabilidadBlandas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HabilidadBlanda habilidadBlanda = db.HabilidadBlanda.Find(id);
            if (habilidadBlanda == null)
            {
                return HttpNotFound();
            }
            ViewBag.idEmpleadoFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre", habilidadBlanda.idEmpleadoFK);
            return View(habilidadBlanda);
        }

        // POST: HabilidadBlandas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idEmpleadoFK,habilidad")] HabilidadBlanda habilidadBlanda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(habilidadBlanda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idEmpleadoFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre", habilidadBlanda.idEmpleadoFK);
            return View(habilidadBlanda);
        }

        // GET: HabilidadBlandas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HabilidadBlanda habilidadBlanda = db.HabilidadBlanda.Find(id);
            if (habilidadBlanda == null)
            {
                return HttpNotFound();
            }
            return View(habilidadBlanda);
        }

        // POST: HabilidadBlandas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            HabilidadBlanda habilidadBlanda = db.HabilidadBlanda.Find(id);
            db.HabilidadBlanda.Remove(habilidadBlanda);
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
