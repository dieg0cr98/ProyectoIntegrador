using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.BaseDatos;
using ProyectoIntegrador.Models;

namespace ProyectoIntegrador.Controllers
{
    public class PruebasController : Controller
    {
        private Gr03Proy2Entities6 db = new Gr03Proy2Entities6();

        // GET: Pruebas
        //Metodo que devuelve las pruebas asociadas al requerimiento del proyecto que entran como parametros
        public ActionResult Index(int idProyecto, int idRequerimiento, int idPrueba = 0)
        {
            //Se sacan las pruebas de la base de datos
            var pruebas = db.Prueba.Where(P => P.idProyectoFK == idProyecto && P.idReqFK == idRequerimiento);

            //Se guarda la selección que se debe desplegar automáticamente a la hora de llamar la vista de consulta.
            ViewBag.seleccion = idPrueba;

            //Se guarda id y nombre del proyecto en el viewbag
            ViewBag.idProyecto = idProyecto;
            ViewBag.nombreProyecto = db.Proyecto.Find(idProyecto).nombre;

            //Se guarda id y nombre del requerimiento en el viewbag
            ViewBag.idRequerimiento = idRequerimiento;
            ViewBag.nombreRequerimiento = db.Requerimiento.Find(idRequerimiento, idProyecto).nombre;

            return View(pruebas.ToList());
        }

        // Metodo que se llama al ingresar a la vista de editar prueba
        public ActionResult Edit(int idProyecto, int idRequerimiento, int idPrueba)
        {
            Prueba prueba = db.Prueba.Find(idProyecto, idRequerimiento, idPrueba); //Se saca la prueba de la base de datos

            if (prueba == null)
            {
                return HttpNotFound();
            }

            //Se guarda id y nombre del proyecto en el viewbag
            ViewBag.idProyecto = idProyecto;
            ViewBag.nombreProyecto = db.Proyecto.Find(idProyecto).nombre;

            //Se guarda id y nombre del requerimiento en el viewbag
            ViewBag.idRequerimiento = idRequerimiento;
            ViewBag.nombreRequerimiento = db.Requerimiento.Find(idRequerimiento, idProyecto).nombre;

            ViewBag.idPrueba = idPrueba;

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
            Requerimiento req = db.Requerimiento.Find(idRequerimiento, idProyecto);
            ViewBag.nombreRequerimiento = req.nombre;

            ViewBag.idPrueba = req.cantidadDePruebas + 1;

            //Ver si hace falta un trigger para sumar a la cantidad de pruebas en req, creo que si

            return View();
        }

        // POST: Pruebas/Create
        [HttpPost]
        public ActionResult Create(int idProyecto, int idReq, string resultadoFinal, string propositoPrueba,
            string entradaDatos, string resultadoEsperado, string flujoPrueba, string estado, string imagen, string descripcionError, string nombre)
        {
            if (ModelState.IsValid)
            {
                Prueba prueba = new Prueba();
                prueba.idProyectoFK = idProyecto;
                prueba.idReqFK = idReq;
                prueba.nombre = nombre;
                prueba.resultadoFinal = resultadoFinal;
                prueba.propositoPrueba = propositoPrueba;
                prueba.entradaDatos = entradaDatos;
                prueba.resultadoEsperado = resultadoEsperado;
                prueba.flujoPrueba = flujoPrueba;
                prueba.estado = estado;
                prueba.imagen = Encoding.ASCII.GetBytes(imagen);
                prueba.descripcionError = descripcionError;
                db.Prueba.Add(prueba);
                db.SaveChanges();
                return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = idReq, idPrueba = prueba.idPruebaPK }); //Retorna a la vista
            }

            return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = idReq }); //Retorna a la vista
        }

        // POST: Pruebas/Edit/5
        // Metodo para guardar los cambios realizados a una prueba.
        [HttpPost]
        public ActionResult Edit(int idProyecto, int idReq, int idPrueba,string resultadoFinal, string propositoPrueba,
        string entradaDatos, string resultadoEsperado, string flujoPrueba, string estado, string imagen, string descripcionError, string nombre)
        {
            if (ModelState.IsValid)
            {
                Prueba prueba = db.Prueba.Find(idProyecto,idReq,idPrueba);
                //prueba.nombre = nombre;
                prueba.resultadoFinal = resultadoFinal;
                prueba.propositoPrueba = propositoPrueba;
                prueba.entradaDatos = entradaDatos;
                prueba.resultadoEsperado = resultadoEsperado;
                prueba.flujoPrueba = flujoPrueba;
                prueba.estado = estado;
                prueba.imagen = Encoding.ASCII.GetBytes(imagen);
                prueba.descripcionError = descripcionError;

                db.Entry(prueba).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = idReq, idPrueba = prueba.idPruebaPK }); //Retorna a la vista
            }

            return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = idReq }); //Retorna a la vista
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
