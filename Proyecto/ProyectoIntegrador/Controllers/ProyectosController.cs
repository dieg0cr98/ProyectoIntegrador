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


        //Metodo para recuperar los proyectos en los que participa un usuario.
        //Recibe int permiso (Tomados de la tabla de seguridad)
        //           permiso = 1 Puede ver todos los proyectos
        //           permiso = 2 Solo puede ver los proyectos en los que participa
        //           permiso = 3 No puede ver ninguno
        //       string rol (El usuario tiene que tener un rol asignado antes de llamar a este metodo)
        //              rol = 0 Soporte/Calidad
        //              rol = 1 Lider
        //              rol = 2 Tester
        //              rol = 3 Cliente
        //       string idUsuario (Es la cedula o identificador de un usuario) 
        //Devuelve una lista IEnumerable con los proyectos. Null en caso de que no puede ver ninguno
        public IEnumerable<ProyectoIntegrador.BaseDatos.Proyecto> GetProyectosUsuario(int permiso  , int rol, string idUsuario)
        {
            //Puede ver todos los proyectos
            if(permiso == 1)
            {
                return db.Proyecto.ToList();
            }
            else
            {
                //Solo puede ver los proyectos en los que participa
                if (permiso == 2)
                {
                    //Si es cliente
                    if (rol == 3)
                    {
                        //Selecciona todos los proyectos que tengan la cedula del cliente asociada
                        return db.Proyecto.Where(p => p.cedulaClienteFK == idUsuario);

                    }
                    else
                    {
                        var innerJoinQuery =
                        from p in db.Proyecto //Selecciona la tabla de Proyectos
                        join t in db.TrabajaEn on p.idProyectoAID equals t.idProyectoFK //Hace join con la tabla TrabajaEn
                        join e in db.Empleado on t.idEmpleadoFK equals e.idEmpleadoPK //Hace join con la tabla Empleado
                        where e.idEmpleadoPK == idUsuario //Solo los proyectos en donde el Empleado trabaja
                        select p; //Selecciona todo los atributos del proyecto

                        return innerJoinQuery.ToList();
                    }

                }
                //No tiene permisos
                else
                    return null;

            }


          



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


            return View(GetProyectosUsuario(permisosGenerales.Item2 , permisosGenerales.Item1, permisosGenerales.Item3));

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
            
            int rolUsurario = seguridad.GetRoleUsuario(User);
            //Verifica si el usuario tiene un rol valido
            if(rolUsurario >= 0)//Si tiene rol valido
            {
                //Selecciona todos los Clientes
                ViewBag.cliente = db.Cliente.ToList();

                //Selecciona todos los empelados que esten disponible y que sean Lider
                ViewBag.lider = db.Empleado.Where(p => p.estado == "Disponible" && p.tipoTrabajo == "Lider").ToList();


                return View();
            }
            else
            {
                return View();
            }

           
        }


        // POST: Proyectos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string nombre,string objetivo, TimeSpan duracionEstimada, string cedulaCliente, string cedulaLider)
        {
            


            Proyecto proyecto = new Proyecto();
            proyecto.nombre = nombre;
            proyecto.objetivo = objetivo;
            proyecto.duracionEstimada = duracionEstimada;
            proyecto.cedulaClienteFK = cedulaCliente;
        

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

                Empleado empleado = db.Empleado.Find(cedulaLider);
                empleado.estado = "Trabajando";
                db.Entry(empleado).State = EntityState.Modified;
                db.SaveChanges();

            }

                return RedirectToAction("Index");

        }



   
        // GET: Proyectos/Edit/5
        public ActionResult Edit(int? id)
        {


            int rolUsurario = seguridad.GetRoleUsuario(User);
            //Verifica si el usuario tiene un rol valido
            if (rolUsurario < 0)//Si tiene rol invalido
            {
                return View();
            }
            else
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


                var permisosGenerales = seguridad.ProyectoEditar(rolUsurario);
                ViewBag.permisos = permisosGenerales;

                //Si no tiene permisos para acceder a la vista editar
                if(permisosGenerales.Item1 == 3)
                {
                    return View();

                }
                else
                {

                    //Regresa una lista de los proyectos en los que participa el usuario
                    var proyectos = GetProyectosUsuario(permisosGenerales.Item1, rolUsurario , seguridad.IdUsuario(User));

                    //Busca si el proyecto que se quiere editar pertenece a esta persona
                    var verificacion = proyectos.Where(p => p.idProyectoAID == proyecto.idProyectoAID);

                    //Si tiene acceso a ese proyecto
                    if (verificacion.Any())
                    {
                        //Selecciona el cliente actual
                        ViewBag.clienteActual = db.Cliente.Find(proyecto.cedulaClienteFK);

                        //Case null 


                        //Selecciona todos los Clientes
                        ViewBag.cliente = db.Cliente.Where(c => c.cedulaPK != proyecto.cedulaClienteFK).ToList();


                        

                        //Busca la cedula lider actual del proyecto
                        TrabajaEn cedulaLider = db.TrabajaEn.Where(p => p.idProyectoFK == id && p.rol == "Lider" && p.estado == "Activo").FirstOrDefault();

                        ViewBag.liderActual = null;

                        if (cedulaLider != null)
                        {
                            ViewBag.liderActual = cedulaLider.Empleado;
                            //Selecciona todos los empelados que esten disponible y que sean Lider (Menos el lider actual) 
                            ViewBag.lider = db.Empleado.Where(p => p.idEmpleadoPK != cedulaLider.Empleado.idEmpleadoPK && p.estado == "Disponible" && p.tipoTrabajo == "Lider").ToList();

                        }
                        else
                        {
                            ViewBag.lider = db.Empleado.Where(p => p.estado == "Disponible" && p.tipoTrabajo == "Lider").ToList();
                        }
                        return View(proyecto);

                    }
                    else
                    {
                        return View();
                    }

                  

                }
                
            }

           

        }

        // POST: Proyectos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string id,string nombre, string objetivo, string estado, TimeSpan duracionEstimada,TimeSpan duracionReal, DateTime fechaInicio, DateTime fechaFinalizacion, int cantidadReq, string cedulaCliente, string cedulaLiderActual, string cedulaLider)
        {

           int inProyectoAID = Int32.Parse(id);


            Proyecto proyecto = db.Proyecto.Find(inProyectoAID);

            //Si selecciona un nuevo lider o eliminar al actual. Hay que actualizar datos
            if( !String.IsNullOrEmpty(cedulaLider))
            {

                //Si no existe un lider 
                if (String.IsNullOrEmpty(cedulaLiderActual))
                {
                    //Se selecciono un nuevo lider
                    if(cedulaLider != "-1")
                    {

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
                //Ya existe un lider
                else
                {
                    //hay que cambiar al lider
                    if (cedulaLider != cedulaLiderActual)
                    {
                        //Si aun no empieza el proyecto cambie el lider
                        if (proyecto.estado == "Preparacion")
                        {

                            //Elimina el empleado anterior
                            db.TrabajaEn.Remove(db.TrabajaEn.Find(inProyectoAID, cedulaLiderActual));
                            db.SaveChanges();

                            //Actualiza los datos del empleado
                            Empleado empleado = db.Empleado.Find(cedulaLiderActual);
                            empleado.estado = "Disponible";
                            db.Entry(empleado).State = EntityState.Modified;
                            db.SaveChanges();


                            //Se selecciono un nuevo lider
                            if(cedulaLider != "-1")
                            {
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


                            //Se selecciono un nuevo lider
                            if (cedulaLider != "-1")
                            {
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

                    }

                }



            }



            //Actualiza los datos del proyecto
            proyecto.nombre = nombre;
            proyecto.objetivo = objetivo;
            proyecto.estado = estado;
            proyecto.duracionEstimada = duracionEstimada;
            proyecto.duracionReal = duracionReal;
            proyecto.fechaInicio = fechaInicio;
            proyecto.fechaFinalizacion = fechaFinalizacion;
            proyecto.cantidadReq = cantidadReq;
            proyecto.cedulaClienteFK = cedulaCliente;


            //Guarda el proyecto con los nuevos valores
            db.Entry(proyecto).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");


        }

        // GET: Proyectos/Delete/5
        public ActionResult Delete(int? id)
        {
            int rolUsurario = seguridad.GetRoleUsuario(User);
            //Verifica si el usuario tiene un rol valido
            if (rolUsurario < 0)//Si tiene rol invalido
            {
                return View();
            }
            else
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


           
        }

        public ActionResult Eliminar(int id)
        {
            Proyecto proyecto = db.Proyecto.Find(id);
            db.Proyecto.Remove(proyecto);
            db.SaveChanges();
            return  RedirectToAction("Index");
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
