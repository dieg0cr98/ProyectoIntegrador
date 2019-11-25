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


        // convert image to byte array
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

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
                ViewBag.nombreViejo = prueba.nombre;
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

                    string dirImage = "C:\\Users\\dieg0\\Documents\\Consulta4.png";
                    Image imagen2 = Image.FromFile(dirImage);
                    var t = imageToByteArray(imagen2);

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
                    prueba.imagen = t;
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

        // POST: Pruebas/Edit/5
        // Metodo para guardar los cambios realizados a una prueba.
        [HttpPost]
        public ActionResult Edit(int idProyecto, int idReq, int idPrueba,string resultadoFinal, string propositoPrueba,
        string entradaDatos, string resultadoEsperado, string flujoPrueba, string estado, string imagen, string descripcionError, string nombre, HttpPostedFileBase pic)
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


        public ActionResult AgregarImagen(int proyecto, int requerimiento, int prueba)
        {
            ViewBag.proyecto = proyecto;
            ViewBag.requerimiento = requerimiento;
            ViewBag.prueba = prueba;

            return View();
        }



        [HttpPost]
        public ActionResult AgregarImagen(int proyectoID, int requerimientoID, int pruebaID, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    //string path = Path.Combine(Server.MapPath("~/Images"),
                    //                           Path.GetFileName(file.FileName));
                    //file.SaveAs(path);
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();

                    Prueba prueba = db.Prueba.Find(proyectoID, requerimientoID, pruebaID);
                    prueba.imagen = data;

                    db.Entry(prueba).State = EntityState.Modified;
                    db.SaveChanges();

                    ViewBag.Message = "File uploaded successfully";
                    ViewBag.pic = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(data));

                    
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            ViewBag.proyecto = proyectoID;
            ViewBag.requerimiento = requerimientoID;
            ViewBag.prueba = pruebaID;

            return View();
        }
    }
}
