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
    public class HabilidadTecnicasController : Controller
    {
        private Gr03Proy2Entities6 db = new Gr03Proy2Entities6();
        private EmpleadosController empleados = new EmpleadosController();

        public IEnumerable<ProyectoIntegrador.BaseDatos.HabilidadTecnica> GetHabilidadUsuario(string idUsuario)
        {
            var t = db.HabilidadTecnica.Where(h => h.idEmpleadoFK == idUsuario).ToList();
            if (t != null)
            {
                return t;
            }
            else
            {
                return null;
            }


        }
        // GET: HabilidadTecnicas
        public ActionResult Index(string id)
        {
            ViewBag.empl = db.Empleado.Find(id);
            ViewBag.nombre = empleados.GetNombreEmpleado(id);
            var habilidadTecnica = db.HabilidadTecnica.Where(h => h.idEmpleadoFK == id);
            return View(habilidadTecnica.ToList());
        }

        // GET: HabilidadTecnicas/Details/5
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

        // GET: HabilidadTecnicas/Create
        public ActionResult Create()
        {
            ViewBag.idEmpleadoFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre");
            return View();
        }

        // POST: HabilidadTecnicas/Create
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

        // GET: HabilidadTecnicas/Edit/5
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

        // POST: HabilidadTecnicas/Edit/5
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

        // GET: HabilidadTecnicas/Delete/5
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

        // POST: HabilidadTecnicas/Delete/5
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
