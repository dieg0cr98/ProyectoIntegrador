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
        private Gr03Proy2Entities6 db = new Gr03Proy2Entities6();

        // GET: Pruebas
        //Metodo que devuelve las pruebas asociadas al requerimiento del proyecto que entran como parametros
        public ActionResult Index(int idProyecto, int idRequerimiento, int idPrueba)
        {
            //Se sacan las pruebas de la base de datos
            var pruebas = db.Prueba.Where(P => P.idProyectoFK == idProyecto && P.idReqFK == idRequerimiento);

            //Se guarda la selección que se debe desplegar automáticamente a la hora de llamar la vista de consulta.
            ViewBag.seleccion = idPrueba;

            //Se guardan los id de proyecto y requerimientos para que sean usados en el CRUD de pruebas.
            ViewBag.idProyecto = idProyecto;
            ViewBag.idRequerimiento = idRequerimiento;

            return View(pruebas.ToList());
        }

        // Metodo que se llama al ingresar a la vista de editar prueba
        public ActionResult Edit(int idProyecto, int idRequerimiento, int idPrueba)
        {
            Prueba prueba = db.Prueba.Find(idProyecto,idRequerimiento,idPrueba); //Se saca la prueba de la base de datos

            if (prueba == null)
            {
                return HttpNotFound();
            }

            //Se guarda id y nombre del proyecto en el viewbag
            ViewBag.idProyecto = idProyecto;
            ViewBag.nombreProyecto = db.Proyecto.Find(idProyecto).nombre;

            //Se guarda id y nombre del requerimiento en el viewbag
            ViewBag.idRequerimiento = idRequerimiento;
            ViewBag.nombreRequerimiento = db.Requerimiento.Find(idRequerimiento).nombre;

            return View(prueba);
        }

        // GET: Pruebas/Create
        //Metodo que se llama al ingresar a la vista de crear prueba
        public ActionResult Create(int idProyecto, int idRequerimiento)
        {
            //Se guarda id y nombre del proyecto en el viewbag
            ViewBag.idProyecto = idProyecto;
            ViewBag.nombreProyecto = db.Proyecto.Find(idProyecto).nombre;

            //Se guarda id y nombre del requerimiento en el viewbag
            ViewBag.idRequerimiento = idRequerimiento;
            Requerimiento req = db.Requerimiento.Find(idRequerimiento);
            ViewBag.nombreRequerimiento = req.nombre;

            ViewBag.idPrueba = req.cantidadDePruebas + 1;

            //Ver si hace falta un trigger para sumar a la cantidad de pruebas en req, creo que si

            return View();
        }

        // POST: Pruebas/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "idProyectoFK,idReqFK,idPruebaPK,resultadoFinal,propositoPrueba,entradaDatos,resultadoEsperado,flujoPrueba,estado,imagen,descripcionError")] Prueba prueba)
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
        //CREO ESTE METODO NO HACE FALTA
        /*
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
        }*/

        // POST: Pruebas/Edit/5
        // Metodo para guardar los cambios realizados a una prueba.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "idProyectoFK,idReqFK,idPruebaPK,resultadoFinal,propositoPrueba,entradaDatos,resultadoEsperado,flujoPrueba,estado,imagen,descripcionError")] Prueba prueba)
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

        // Método que recibe la confirmación del usuario para eliminar una prueba.
        public ActionResult Eliminar(int idProyecto, int idRequerimiento, int idPrueba)
        {
            Prueba prueba = db.Prueba.Find(idProyecto, idRequerimiento, idPrueba); //Se saca la prueba de la base de datos
            db.Prueba.Remove(prueba); //Se elimina la prueba
            db.SaveChanges(); //Se guardan cambios
            return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = idRequerimiento, idPrueba = 0 }); //Retorna a la vista
        }
    }
}
