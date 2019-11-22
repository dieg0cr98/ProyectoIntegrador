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
        private SeguridadController seguridad = new SeguridadController();

        // GET: Pruebas
        //Metodo que devuelve las pruebas asociadas al requerimiento del proyecto que entran como parametros
        public ActionResult Index(int idProyecto, int idRequerimiento, int idPrueba = 0 , string mensaje = null)
        {
            //Sacamos permisos generales de la vista.
            var permisosGenerales = seguridad.PruebasPermisos(User);
            ViewBag.permisosGenerales = permisosGenerales; //asignamos los permisos al viewbag
            ViewBag.error = mensaje;
            //Verifica que el usuario este registrado y que tenga permisos de consultar pruebas.
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item3 <= 2)
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
            else
            {
                ViewBag.error = "No posee permisos para realizar esa acción.";
                return View();
            }
        }

        // Metodo que se llama al ingresar a la vista de editar prueba
        public ActionResult Edit(int idProyecto, int idRequerimiento, int idPrueba)
        {
            var permisosGenerales = seguridad.PruebasPermisos(User);
            
            //Verifica que el usuario este registrado y que tenga permiso de editar = 1
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item4 == 1)
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
            else
            {
                return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = idRequerimiento, idPrueba = idPrueba, mensaje = "No posee permisos para realizar esa acción." }); //Retorna a la vista
            }
        }

        // GET: Pruebas/Create
        //Metodo que se llama al ingresar a la vista de crear prueba
        public ActionResult Create(int idProyecto, int idRequerimiento)
        {
            var permisosGenerales = seguridad.PruebasPermisos(User);

            //Verifica que el usuario este registrado y que tenga permiso de agregar = 1
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item5 == 1)
            {
                //Se guarda id y nombre del proyecto en el viewbag
                ViewBag.idProyecto = idProyecto;
                ViewBag.nombreProyecto = db.Proyecto.Find(idProyecto).nombre;

                //Se guarda id y nombre del requerimiento en el viewbag
                ViewBag.idRequerimiento = idRequerimiento;
                Requerimiento req = db.Requerimiento.Find(idRequerimiento, idProyecto);
                ViewBag.nombreRequerimiento = req.nombre;

                return View();
            }
            else
            {
                return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = idRequerimiento, mensaje = "No posee permisos para realizar esa acción." }); //Retorna a la vista
            }
        }

        // POST: Pruebas/Create
        [HttpPost]
        public ActionResult Create(int idProyecto, int idReq, string resultadoFinal, string propositoPrueba,
            string entradaDatos, string resultadoEsperado, string flujoPrueba, string estado, string imagen, string descripcionError, string nombre)
        {
            var permisosGenerales = seguridad.PruebasPermisos(User);
            string mensaje = "";
            //Verifica que el usuario este registrado y que tenga permiso de agregar = 1
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item5 == 1)
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
                }
            }
            else
            {
                mensaje = "No posee permisos para realizar esa acción.";
            }
            return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = idReq, mensaje = mensaje }); //Retorna a la vista    
        }

        // POST: Pruebas/Edit/5
        // Metodo para guardar los cambios realizados a una prueba.
        [HttpPost]
        public ActionResult Edit(int idProy, int idReq, int idPrueba,string resultadoFinal, string propositoPrueba,
        string entradaDatos, string resultadoEsperado, string flujoPrueba, string estado, byte[] imagen, string descripcionError, string nombre)
        {
            var permisosGenerales = seguridad.PruebasPermisos(User);
            string mensaje = "";
            //Verifica que el usuario este registrado y que tenga permiso de editar = 1
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item4 == 1)
            {
                if (ModelState.IsValid)
                {
                    Prueba prueba = db.Prueba.Find(idProy, idReq, idPrueba);
                    //prueba.nombre = nombre;
                    prueba.resultadoFinal = resultadoFinal;
                    prueba.propositoPrueba = propositoPrueba;
                    prueba.entradaDatos = entradaDatos;
                    prueba.resultadoEsperado = resultadoEsperado;
                    prueba.flujoPrueba = flujoPrueba;
                    prueba.estado = estado;
                    prueba.imagen = imagen;
                    prueba.descripcionError = descripcionError;

                    db.Entry(prueba).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                mensaje = "No posee permisos para realizar esa acción.";
            }
            return RedirectToAction("Index", new { idProyecto = idProy, idRequerimiento = idReq, mensaje = mensaje }); //Retorna a la vista
        }

        // Método que recibe la confirmación del usuario para eliminar una prueba.
        public ActionResult Eliminar(int idProyecto, int idRequerimiento, int idPrueba)
        {
            var permisosGenerales = seguridad.PruebasPermisos(User);
            string mensaje = "";
            //Verifica que el usuario este registrado y que tenga permiso de editar = 1
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item6 == 1)
            {
                Prueba prueba = db.Prueba.Find(idProyecto, idRequerimiento, idPrueba); //Se saca la prueba de la base de datos
                db.Prueba.Remove(prueba); //Se elimina la prueba
                db.SaveChanges(); //Se guardan cambios
            }
            else
            {
                mensaje = "No posee permisos para realizar esa acción.";
            }
            return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = idRequerimiento, idPrueba = 0, mensaje = mensaje }); //Retorna a la vista
        }

        public JsonResult ReviseNombrePRueba(string name, string oldName, int idProyecto, int idRequerimiento)
        {
            //Hay que verificar si el nuevo nombre ya existe en la base de datos
            if ((name != oldName) && (db.Prueba.Where(p => p.nombre == name && p.idProyectoFK == idProyecto && p.idReqFK == idRequerimiento).FirstOrDefault() != null))
            {
                //Existe un proyecto con ese nombre
                return new JsonResult { Data = false };
            }
            else
                return new JsonResult { Data = true };

        }
    }
}
