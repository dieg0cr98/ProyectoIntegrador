using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
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


        //Efecto: Recibe una imagen en formato Image y la convierte a formato byte array
        //Requiere: imageIn --> Una imagen en formato System.Drawing.Image
        //Modifica: Esta variable para covertirla en un arreglo de bytes.
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        //Metodo que devuelve las pruebas asociadas al requerimiento del proyecto que entran como parametros-
        //Requiere: int idProyecto -> identificador del proyecto al que estan asociadas las pruebas.
        //          int idRequerimiento -> identificador del requerimiento al que estan asociadas las pruebas.
        //          int idPrueba -> identificador de la prueba que deseamos ver seleccionada.
        //          string mensaje -> mensaje de error en caso de ocurrir alguno.
        //Modifica: La vista actual del usuario.
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
                IEnumerable<ProyectoIntegrador.BaseDatos.Prueba> pruebas = db.Prueba.Where(P => P.idProyectoFK == idProyecto && P.idReqFK == idRequerimiento).ToList();

                //Se guarda la selección que se debe desplegar automáticamente a la hora de llamar la vista de consulta.
                ViewBag.seleccion = idPrueba;

                //Se guarda id y nombre del proyecto en el viewbag
                ViewBag.idProyecto = idProyecto;
                ViewBag.nombreProyecto = db.Proyecto.Find(idProyecto).nombre;

                //Se guarda id y nombre del requerimiento en el viewbag
                ViewBag.idRequerimiento = idRequerimiento;
                ViewBag.nombreRequerimiento = db.Requerimiento.Find(idRequerimiento, idProyecto).nombre;


                return View(pruebas.Reverse());
            }
            else
            {
                ViewBag.error = "No posee permisos para realizar esa acción.";
                return View();
            }
        }

        // Metodo que se llama al ingresar a la vista de editar prueba
        //Requiere: int idProyecto -> identificador del proyecto al que estan asociadas las pruebas.
        //          int idRequerimiento -> identificador del requerimiento al que estan asociadas las pruebas.
        //          int idPrueba -> identificador de la prueba que deseamos ver seleccionada.
        //          string mensaje -> mensaje de error en caso de ocurrir alguno.
        //Modifica: La vista actual del usuario.
        public ActionResult Edit(int idProyecto, int idRequerimiento, int idPrueba, string mensaje = null)
        {
            var permisosGenerales = seguridad.PruebasPermisos(User);

            ViewBag.mensaje = mensaje;

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
                ViewBag.nombreViejo = prueba.nombre;
                ViewBag.idPrueba = idPrueba;

                return View(prueba);
            }
            else
            {
                return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = idRequerimiento, idPrueba = idPrueba, mensaje = "No posee permisos para realizar esa acción." }); //Retorna a la vista
            }
        }

        //Metodo que se llama al ingresar a la vista de crear prueba
        //Requiere: int idProyecto -> identificador del proyecto al que estan asociadas las pruebas.
        //          int idRequerimiento -> identificador del requerimiento al que estan asociadas las pruebas.
        //          int idPrueba -> identificador de la prueba que deseamos ver seleccionada.
        //          string mensaje -> mensaje de error en caso de ocurrir alguno.
        //Modifica: La vista actual del usuario.
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

        
        [HttpPost]
        public ActionResult Create(int idProyecto, int idReq, string resultadoFinal, string propositoPrueba,
            string entradaDatos, string resultadoEsperado, string flujoPrueba, string estado, string descripcionError, string nombre)
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
                    prueba.descripcionError = descripcionError;
                    db.Prueba.Add(prueba);
                    db.SaveChanges();
                }
            }
            else
            {
                mensaje = "No posee permisos para realizar esa acción.";
            }

            var Prueba = db.Prueba.Where(P => P.nombre == nombre && P.idProyectoFK == idProyecto && P.idReqFK == idReq).FirstOrDefault();
            if (Prueba == null)
            {
                return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = idReq, idPrueba = 0, mensaje = mensaje }); //Retorna a la vista    
            }
            else {
                return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = idReq, idPrueba = Prueba.idPruebaPK, mensaje = mensaje }); //Retorna a la vista    
            }
            
        }

        // Metodo para guardar los cambios realizados a una prueba.
        // Requiere: Todos los atributos de una prueba excepto la imagen, que se guarda por aparte.
        // Modifica: La instancia identificada por el idProyecto, idReq y idPrueba de la tabla prueba.
        [HttpPost]
        public ActionResult Edit(int idProyecto, int idReq, int idPrueba, string resultadoFinal, string propositoPrueba,
        string entradaDatos, string resultadoEsperado, string flujoPrueba, string estado, string descripcionError, string nombre, HttpPostedFileBase pic)
        {
            var permisosGenerales = seguridad.PruebasPermisos(User);
            string mensaje = "";
            //Verifica que el usuario este registrado y que tenga permiso de editar = 1
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item4 == 1)
            {
                if (ModelState.IsValid)
                {


                    Prueba prueba = db.Prueba.Find(idProyecto, idReq, idPrueba);
                    //prueba.nombre = nombre;
                    prueba.resultadoFinal = resultadoFinal;
                    prueba.propositoPrueba = propositoPrueba;
                    prueba.entradaDatos = entradaDatos;
                    prueba.resultadoEsperado = resultadoEsperado;
                    prueba.flujoPrueba = flujoPrueba;
                    prueba.estado = estado;                    
                    prueba.descripcionError = descripcionError;

                    db.Entry(prueba).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                mensaje = "No posee permisos para realizar esa acción.";
            }
            return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = idReq, idPrueba = idPrueba, mensaje = mensaje }); //Retorna a la vista
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

        // Efecto: Retorna verdadero si hay un nombre de prueba como el que deseamos asignar en este requerimiento.
        // Requiere: string name -> Nombre que deseamos poner a la prueba.
        //           string oldname -> Nombre que tenía el requerimiento.
        //           int idProyecto -> Proyecto al que está asociado la prueba que estamos deseando nombrar.
        //           int idRequerimiento -> Requerimiento al que está asociado esta prueba que deseamos mostrar.
        // Modifica: Nada
        public JsonResult ReviseNombrePrueba(string name, string oldName, int idProyecto, int idRequerimiento)
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


        //Efecto: Retorna la vista utilizada para agregar la imagen.
        //Requiere: La llave primaria de las pruebas, para encontrar la que se desea utilizar para asignar la imagen
        //Modifica: La vista actual del usuario.
        public ActionResult AgregarImagen(int proyecto, int requerimiento, int prueba)
        {
            var permisosGenerales = seguridad.PruebasPermisos(User);
            ViewBag.permisosGenerales = permisosGenerales; //asignamos los permisos al viewbag
            //Verifica que el usuario este registrado y que tenga permisos de consultar pruebas.
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item3 <= 2)
            {
                ViewBag.proyecto = proyecto;
                ViewBag.requerimiento = requerimiento;
                ViewBag.prueba = prueba;

                return View();
            }
            return RedirectToAction("Index", new { idProyecto = proyecto, idRequerimiento = requerimiento, idPrueba = prueba, mensaje = "Usted no posee permisos para agregar la imagen" }); //Retorna a la vista
        }


        //Efecto: Guarda la imagen ingresada por el usuario en la prueba seleccionada en la consulta.
        //Requiere: La llave primaria de las pruebas y la imagen que se desea asignar.
        //Modifica: La tabla de pruebas para agregar la imagen a la prueba solicitada.
        [HttpPost]
        public ActionResult AgregarImagen(int proyectoID, int requerimientoID, int pruebaID, HttpPostedFileBase file)
        {
            var permisosGenerales = seguridad.PruebasPermisos(User);
            ViewBag.permisosGenerales = permisosGenerales; //asignamos los permisos al viewbag
            //Verifica que el usuario este registrado y que tenga permisos de consultar pruebas.
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item3 <= 2)
            {
                if (file != null && file.ContentLength > 0)
                    try
                    {
                        MemoryStream target = new MemoryStream();
                        file.InputStream.CopyTo(target);
                        byte[] data = target.ToArray();

                        Prueba prueba = db.Prueba.Find(proyectoID, requerimientoID, pruebaID);
                        prueba.imagen = data;

                        db.Entry(prueba).State = EntityState.Modified;
                        db.SaveChanges();

                        ViewBag.Message = "Imagen guardada exitosamente";
                        ViewBag.pic = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(data));

                        return RedirectToAction("Index", new { idProyecto = proyectoID, idRequerimiento = requerimientoID, idPrueba = pruebaID }); //Retorna a la vista
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                else
                {
                    ViewBag.Message = "Ninguna Imagen seleccionada";
                }

                ViewBag.proyecto = proyectoID;
                ViewBag.requerimiento = requerimientoID;
                ViewBag.prueba = pruebaID;
            }
            return View();
        }


        // Efecto: Muestra la vista utilizada para modificar la imagen
        // Requiere: La llave primaria de las pruebas para determinar a cual prueba se le desea cambiar la imagen.
        // Modifica: La vista actual del sistema.
        public ActionResult ModificarImagen(int proyecto, int requerimiento, int prueba)
        {
            var permisosGenerales = seguridad.PruebasPermisos(User);
            ViewBag.permisosGenerales = permisosGenerales; //asignamos los permisos al viewbag
            //Verifica que el usuario este registrado y que tenga permisos de consultar pruebas.
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item3 <= 2)
            {
                Prueba p = db.Prueba.Find(proyecto, requerimiento, prueba);
                ViewBag.oldPic = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(p.imagen));
                ViewBag.proyecto = proyecto;
                ViewBag.requerimiento = requerimiento;
                ViewBag.prueba = prueba;

                return View("CambiarImagen");
            }

            return RedirectToAction("Index", new { idProyecto = proyecto, idRequerimiento = requerimiento, idPrueba = prueba, mensaje = "Usted no posee permisos para modificar la imagen" }); //Retorna a la vista
        }

        // Efecto: Modifica la imagen que el usuario tenía asignada en la BD para esta prueba
        // Requiere: La llave primaria de las pruebas y la imagen para determinar en que prueba asignar la imagen.
        // Modifica: La tabla pruebas.
        [HttpPost]
        public ActionResult ModificarImagen(int proyectoID, int requerimientoID, int pruebaID, HttpPostedFileBase file)
        {
            var permisosGenerales = seguridad.PruebasPermisos(User);
            ViewBag.permisosGenerales = permisosGenerales; //asignamos los permisos al viewbag
            //Verifica que el usuario este registrado y que tenga permisos de consultar pruebas.
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item3 <= 2)
            {
                Prueba prueba = db.Prueba.Find(proyectoID, requerimientoID, pruebaID);

                if (file != null && file.ContentLength > 0)
                    try
                    {
                        MemoryStream target = new MemoryStream();
                        file.InputStream.CopyTo(target);
                        byte[] data = target.ToArray();

                        prueba.imagen = data;

                        db.Entry(prueba).State = EntityState.Modified;
                        db.SaveChanges();

                        ViewBag.Message = "Imagen guardada exitosamente";
                        ViewBag.pic = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(data));


                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                else
                {
                    ViewBag.Message = "Debe escoger una imagen primero.";
                }


                ViewBag.proyecto = proyectoID;
                ViewBag.requerimiento = requerimientoID;
                ViewBag.prueba = pruebaID;
                ViewBag.oldPic = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(prueba.imagen));

            }
            return View("CambiarImagen");
        }

        // Efecto: Elimina la imagen que de la prueba que el usuario desea.
        // Requiere: La llave primaria para identificar cual imagen se desea eliminar.
        // Modifica: La tabla de pruebas.
        public ActionResult EliminarImagen(int? idProyecto, int? idReq, int? idPrueba) {

            var permisosGenerales = seguridad.PruebasPermisos(User);
            ViewBag.permisosGenerales = permisosGenerales; //asignamos los permisos al viewbag

            //Verifica que el usuario este registrado y que tenga los permisos necesarios.
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item3 <= 2)
            {
                Prueba prueba = db.Prueba.Where(p => p.idProyectoFK == idProyecto && p.idReqFK == idReq && p.idPruebaPK == idPrueba).FirstOrDefault();
                prueba.imagen = null;
                db.Entry(prueba).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = idReq, idPrueba = idPrueba }); //Retorna a la vista
        }
    }
}
