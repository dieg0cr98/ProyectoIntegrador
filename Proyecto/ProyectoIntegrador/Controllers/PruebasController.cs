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
    public class PruebasController : Controller
    {
        private Gr03Proy2Entities2 db = new Gr03Proy2Entities2();

        // GET: Pruebas
        public ActionResult Index()
        {
            var prueba = db.Prueba.Include(p => p.Requerimiento);
            return View(prueba.ToList());
        }

        // GET: Pruebas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prueba prueba = db.Prueba.Find(id);
            if (prueba == null)
            {
                return HttpNotFound();
            }
            return View(prueba);
        }

        // GET: Pruebas/Create
        public ActionResult Create()
        {
            ViewBag.idReqFK = new SelectList(db.Requerimiento, "idReqPK", "cedulaTesterFK");
            return View();
        }

        // POST: Pruebas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idProyectoFK,idReqFK,idPruebaPK,resultadoFinal,propositoPrueba,entradaDatos,resultadoEsperado,flujoPrueba,estado,descripcionError")] Prueba prueba)
        {
            if (ModelState.IsValid)
            {
                db.Prueba.Add(prueba);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idReqFK = new SelectList(db.Requerimiento, "idReqPK", "cedulaTesterFK", prueba.idReqFK);
            return View(prueba);
        }

        // GET: Pruebas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prueba prueba = db.Prueba.Find(id);
            if (prueba == null)
            {
                return HttpNotFound();
            }
            ViewBag.idReqFK = new SelectList(db.Requerimiento, "idReqPK", "cedulaTesterFK", prueba.idReqFK);
            return View(prueba);
        }

        // POST: Pruebas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProyectoFK,idReqFK,idPruebaPK,resultadoFinal,propositoPrueba,entradaDatos,resultadoEsperado,flujoPrueba,estado,descripcionError")] Prueba prueba)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prueba).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idReqFK = new SelectList(db.Requerimiento, "idReqPK", "cedulaTesterFK", prueba.idReqFK);
            return View(prueba);
        }

        // GET: Pruebas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prueba prueba = db.Prueba.Find(id);
            if (prueba == null)
            {
                return HttpNotFound();
            }
            return View(prueba);
        }

        // POST: Pruebas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prueba prueba = db.Prueba.Find(id);
            db.Prueba.Remove(prueba);
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
