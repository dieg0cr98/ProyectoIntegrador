using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.BaseDatos;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ProyectoIntegrador.Models;
using System.Threading.Tasks;

namespace ProyectoIntegrador.Controllers
{


    public class ProyectosController : Controller
    {
     
        private  SeguridadController seguridad = new SeguridadController();


        public async System.Threading.Tasks.Task<ActionResult> Agregar()
        {
            //......Code........//
            await seguridad.AgregarUsuarioAsync("test2@gmail.com", "Lider");
            //......Code........//

            return RedirectToAction("Index");
        }




        


        //Metodo para obtener la vista principal de los clientes
        public ActionResult IndexCliente()
        {
            return RedirectToAction("Index", "Clientes", null);
        }

        //Metodo para obtener la vista principal de los empleados
        public ActionResult IndexEmpleado()
        {
            return RedirectToAction("Index", "Empleados", null);
        }



        private Gr03Proy2Entities2 db = new Gr03Proy2Entities2();

        // GET: Proyectos
        public ActionResult Index()
        {

            var permisosGenerales = seguridad.ProyectoConsultar(User);
            ViewBag.permisosEspecificos = permisosGenerales;
            
            if (permisosGenerales.Item2 == 1) //Puede ver todos los proyectos
            {
                return View(db.Proyecto.ToList());

            }
            else
            {
                if (permisosGenerales.Item2 == 2) //Puede ver solo los proyectos a los que pertenece
                {
                    if (permisosGenerales.Item1 == 3)//Se hace join con la tabla de Clientes
                    {
                        return View(db.Proyecto.Where(p => p.cedulaClienteFK == permisosGenerales.Item3));
                    }

                    else //Se hace join con la tabla de Empleados y TrabajaEn
                    {

                    System.Diagnostics.Debug.WriteLine("Cedula " + permisosGenerales.Item3);

                    
                    var innerJoinQuery =
                    from p in db.Proyecto //Selecciona la tabla de Proyectos
                    join t in db.TrabajaEn on p.idProyectoAID equals t.idProyectoFK //Hace join con la tabla TrabajaEn
                    join e in db.Empleado on t.idEmpleadoFK equals e.idEmpleadoPK //Hace join con la tabla Empleado
                    where e.idEmpleadoPK == permisosGenerales.Item3 //Solo los proyectos en donde el Empleado trabaja
                    select p; //Selecciona todo los atributos del proyecto
                        return View(innerJoinQuery.ToList());
                    }

                }

            }




            return View(db.Proyecto.ToList());
        }

        // GET: Proyectos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyecto proyecto = db.Proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }




        // GET: Proyectos/Create
        public ActionResult Create()
        {

            //Selecciona todos los Clientes
            ViewBag.cliente = db.Cliente.ToList();     
            
            //Selecciona todos los empelados que esten disponible y que sean Lider
            ViewBag.lider = db.Empleado.Where(p => p.estado == "Disponible" && p.tipoTrabajo == "Lider").ToList();
                
                
            return View();
        }


        // POST: Proyectos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string nombre,string objetivo, TimeSpan duracionEstimada, Cliente cedulaCliente, string cedulaLider)
        {



            Proyecto proyecto = new Proyecto();
            proyecto.nombre = nombre;
            proyecto.objetivo = objetivo;
            proyecto.duracionEstimada = duracionEstimada;
            proyecto.cedulaClienteFK = cedulaCliente.cedulaPK;
            System.Diagnostics.Debug.WriteLine(cedulaCliente.cedulaPK + "");

            db.Proyecto.Add(proyecto);
            db.SaveChanges();



            if (!String.IsNullOrEmpty(cedulaLider))
            {
                
                //Recupera el id autogenerado del proyecto creado anteriormente
                int idProyecto = db.Proyecto.Where(p => p.nombre == nombre).Select(p => p.idProyectoAID).FirstOrDefault();
                TrabajaEn trabaja = new TrabajaEn();
                trabaja.idEmpleadoFK = cedulaLider;
                trabaja.idProyectoFK = idProyecto;
                trabaja.estado = "Activo";
                trabaja.rol = "Lider";

                db.TrabajaEn.Add(trabaja);
                db.SaveChanges();

                //Empleado empleado = db.Empleado.Find(cedulaLider);
                //empleado.estado = "Trabajando";
                //db.Entry(empleado).State = EntityState.Modified;
                //db.SaveChanges();




            }

                return RedirectToAction("Index");

        }



   
        // GET: Proyectos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyecto proyecto = db.Proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }

            

            //Selecciona el cliente actual
            ViewBag.clienteActual = db.Cliente.Find(proyecto.cedulaClienteFK);

            //Selecciona todos los Clientes
            ViewBag.cliente = db.Cliente.Where(c => c.cedulaPK != proyecto.cedulaClienteFK).ToList();

           


            //Busca la cedula lider actual del proyecto
            TrabajaEn cedulaLider = db.TrabajaEn.Where(p => p.idProyectoFK == id && p.rol == "Lider" && p.estado == "Activo").FirstOrDefault();
            
            ViewBag.liderActual = null;

            if(cedulaLider != null)
            {
                ViewBag.liderActual = cedulaLider.Empleado;
                //Selecciona todos los empelados que esten disponible y que sean Lider (Menos el lider actual) 
                ViewBag.lider = db.Empleado.Where(p => p.idEmpleadoPK != cedulaLider.Empleado.idEmpleadoPK && p.estado == "Disponible" && p.tipoTrabajo == "Lider").ToList();
                
            }
            else
            {
                ViewBag.lider = db.Empleado.ToList();
            }
            return View(proyecto);

        }

        // POST: Proyectos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string id,string nombre, string objetivo, string estado, TimeSpan duracionEstimada,TimeSpan duracionReal, DateTime fechaInicio, DateTime fechaFinalizacion, string cedulaCliente, string cedulaLiderActual, string cedulaLider)
        {

           int inProyectoAID = Int32.Parse(id);


            Proyecto proyecto = db.Proyecto.Find(inProyectoAID);
            if (cedulaLider != cedulaLiderActual) //hay que cambiar al lider
            {

                if (proyecto.estado == "Preparacion")//Si aun no empieza el proyecto cambie el lider
                {
                    //Elimina el empleado anterior
                    db.TrabajaEn.Remove(db.TrabajaEn.Find(inProyectoAID, cedulaLiderActual));
                    db.SaveChanges();

                    //Actualiza los datos del empleado
                    Empleado empleado = db.Empleado.Find(cedulaLiderActual);
                    empleado.estado = "Disponible";
                    db.Entry(empleado).State = EntityState.Modified;
                    db.SaveChanges();


                    //Agrega un nuevo Empleado
                    TrabajaEn trabaja = new TrabajaEn();
                    trabaja.idEmpleadoFK = cedulaLider;
                    trabaja.idProyectoFK = inProyectoAID;
                    trabaja.estado = "Activo";
                    trabaja.rol = "Lider";

                    db.TrabajaEn.Add(trabaja);
                    db.SaveChanges();


                    Empleado empleado2 = db.Empleado.Find(cedulaLider);
                    empleado2.estado = "Trabajando";
                    db.Entry(empleado2).State = EntityState.Modified;
                    db.SaveChanges();

                }
                //Si el proyecto ya esta activo, se guarda el lider actual con estado "Inactivo"
                else
                {

                    //Actualiza los datos del empleado en la tabla TrabajaEn
                    TrabajaEn liderAnterior = db.TrabajaEn.Find(inProyectoAID, cedulaLiderActual);
                    liderAnterior.estado = "Inactivo";
                    db.Entry(liderAnterior).State = EntityState.Modified;
                    db.SaveChanges();

                    //Actualiza los datos del empleado en la tabla de empleados
                    Empleado empleado = db.Empleado.Find(cedulaLiderActual);
                    empleado.estado = "Disponible";
                    db.Entry(empleado).State = EntityState.Modified;
                    db.SaveChanges();


                    //Agrega un nuevo Empleado
                    //Verifica si el empleado ya trabajo en el proyecto
                    TrabajaEn lider = db.TrabajaEn.Find(inProyectoAID, cedulaLider);
                    if (lider == null)//Si aun no ha trabajado lo agrega
                    {
                        TrabajaEn trabaja = new TrabajaEn();
                        trabaja.idEmpleadoFK = cedulaLider;
                        trabaja.idProyectoFK = inProyectoAID;
                        trabaja.estado = "Activo";
                        trabaja.rol = "Lider";

                        db.TrabajaEn.Add(trabaja);
                        db.SaveChanges();



                    }
                    else //Si ya trabajo, actualiza el estado
                    {
                        lider.estado = "Activo";
                        db.Entry(lider).State = EntityState.Modified;



                    }

                    //Actualiza el estado del empleado a trabajando
                    Empleado empleado2 = db.Empleado.Find(cedulaLider);
                    empleado2.estado = "Trabajando";
                    db.Entry(empleado2).State = EntityState.Modified;
                    db.SaveChanges();




                }
            }

            proyecto.nombre = nombre;
            proyecto.objetivo = objetivo;
            proyecto.estado = estado;
            proyecto.duracionEstimada = duracionEstimada;
            proyecto.duracionReal = duracionReal;
            proyecto.fechaInicio = fechaInicio;
            proyecto.fechaFinalizacion = fechaFinalizacion;
            proyecto.cedulaClienteFK = cedulaCliente;





            db.Entry(proyecto).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");


        }

        // GET: Proyectos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyecto proyecto = db.Proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }

        // POST: Proyectos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proyecto proyecto = db.Proyecto.Find(id);
            db.Proyecto.Remove(proyecto);
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
