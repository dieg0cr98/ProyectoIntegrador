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
using System.Data.SqlClient;

namespace ProyectoIntegrador.Controllers
{


    public class ProyectosController : Controller
    {

        private SeguridadController seguridad = new SeguridadController();

        private Gr03Proy2Entities3 db = new Gr03Proy2Entities3();


        //Metodo que regresa el contexto de la tabla proyectos, para poder realizar joins
        //Retorna el contexto de la base de datos
        public DbSet<Proyecto> GetTablaProyecto()
        {
            return db.Proyecto;

        }



        //Metodo que guarda/Eliminar en la tabla Proyecto 
        //Recibe Proyecto proyecto, con los datos para guardar
        //       int  tipo, flag para saber el tipo de accion
        //            tipo 0 = nuevo proyecto
        //            tipo 1 = actualizar proyecto
        //            tipo 2 = borrar proyecto
        //Returna un bool, flag para saber si la operacion se pudo completar
        //           bool = false no se pudo realizar la operacion
        //           bool = true se pudo realizar la operacion
        public bool SetProyecto(Proyecto proyecto, int tipo)
        {

            try
            {

                switch(tipo)
                {

                    case 0:
                        {
                            db.Proyecto.Add(proyecto);
                            db.SaveChanges();
                            break;
                        }
                    case 1:
                        {
                            db.Entry(proyecto).State = EntityState.Modified;
                            db.SaveChanges();
                            break;
                        }
                    case 2:
                        {
                            db.Proyecto.Remove(proyecto);
                            db.SaveChanges();
                            break;
                        }

                }

                if(tipo == 0)
                {
                  

                }
                else
                {
                   
                }


                return true;

            }
            catch (SqlException ex)
            {
                List<string> errorMessages = new List<string>();
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                   
                    //errorMessages.Append("Index #" + i + "\n" +
                        //"Message: " + ex.Errors[i].Message + "\n" +
                        //"LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        //"Source: " + ex.Errors[i].Source + "\n" +
                        //"Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                Console.WriteLine(errorMessages.ToString());

                return false;
            }



   
        }


        //Metodo que retorna todos los proyectos
        //Retorna una lista con los proyectos, de no tener la lista queda vacia
        public IEnumerable<ProyectoIntegrador.BaseDatos.Proyecto> GetProyectos()
        {
            return db.Proyecto.ToList();

        }

        //Metodo que retorna un proyecto especifico
        //Recibe un int idProyectoAID, llave primaria del proyecto
        public Proyecto GetProyecto(int idProyectoAID)
        {
            return db.Proyecto.Find(idProyectoAID);
        }



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
        //       int rol (El usuario tiene que tener un rol asignado antes de llamar a este metodo)
        //              rol = 0 Soporte/Calidad
        //              rol = 1 Lider
        //              rol = 2 Tester
        //              rol = 3 Cliente
        //       string idUsuario (Es la cedula o identificador de un usuario) 
        //Devuelve una lista IEnumerable con los proyectos. Null en caso de que no puede ver ninguno
        public IEnumerable<ProyectoIntegrador.BaseDatos.Proyecto> GetProyectosUsuario(int permiso, int rol, string idUsuario)
        {

            //Puede ver todos los proyectos
            if (permiso == 1)
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





        //Metodo que actualiza el lider de un proyecto
        //Recibe Proyecto proyecto, con los datos del proyecto
        //       string cedulaLider, llave primaria del nuevo lider
        //       string cedulaLiderActual, llave primaria del lider actual
        public void ActualizarTrabajaEN(Proyecto proyecto, string cedulaLider, string cedulaLiderActual)
        {
            int inProyectoAID = proyecto.idProyectoAID;
            //Si selecciona un nuevo lider o eliminar al actual. Hay que actualizar datos
            if (!String.IsNullOrEmpty(cedulaLider))
            {

                //Si no existe un lider 
                if (String.IsNullOrEmpty(cedulaLiderActual))
                {
                    //Se selecciono un nuevo lider
                    if (cedulaLider != "-1")
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
                            if (cedulaLider != "-1")
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

        }


        //Metodo para llenar los viewbag de edit con los datos de los clientes
        //Recibe un Proyecto proyecto, con los datos para realizar las busquedas
        private void CargarDatosClientesEdit(Proyecto proyecto)
        {
            //Si tiene un cliente asignado al proyecto
            if (proyecto.cedulaClienteFK != null)
            {
                //Selecciona el cliente actual
                ViewBag.clienteActual = db.Cliente.Find(proyecto.cedulaClienteFK);


                //Selecciona todos los Clientes (Sin tomar en cuenta al actual)
                ViewBag.cliente = db.Cliente.Where(c => c.cedulaPK != proyecto.cedulaClienteFK).ToList();

            }
            else
            {
                ViewBag.clienteActual = null;

                //Selecciona todos los Clientes (Sin tomar en cuenta al actual)
                ViewBag.cliente = db.Cliente.ToList();

            }

        }


        //Metodo para llenar los viewbag de edit con los datos de los lideres
        //Recibe un Proyecto proyecto, con los datos para realizar las busquedas
        private void CargarDatosLiderEdit(Proyecto proyecto)
        {

            //Busca la cedula lider actual del proyecto
            TrabajaEn cedulaLiderA = db.TrabajaEn.Where(p => p.idProyectoFK == proyecto.idProyectoAID && p.rol == "Lider" && p.estado == "Activo").FirstOrDefault();

            ViewBag.liderActual = null;

            if (cedulaLiderA != null)
            {
                ViewBag.liderActual = cedulaLiderA.Empleado;
                //Selecciona todos los empelados que esten disponible y que sean Lider (Menos el lider actual) 
                ViewBag.lider = db.Empleado.Where(p => p.idEmpleadoPK != cedulaLiderA.Empleado.idEmpleadoPK && p.estado == "Disponible" && p.tipoTrabajo == "Lider").ToList();

            }
            else
            {
                ViewBag.lider = db.Empleado.Where(p => p.estado == "Disponible" && p.tipoTrabajo == "Lider").ToList();
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





        //------------- ActionsResults -------------//


        // GET: Proyectos
        public ActionResult Index()
        {

            var permisosGenerales = seguridad.ProyectoConsultar(User);
            ViewBag.permisosEspecificos = permisosGenerales;
            return View(GetProyectosUsuario(permisosGenerales.Item2, permisosGenerales.Item1, permisosGenerales.Item3));

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

            var permisos = seguridad.ProyectoAgregar(User);

            if (permisos != null)
            {

                ViewBag.permisos = permisos;
                //Verifica si el usuario tiene permisos para crear
                if (permisos.Item2 != 0)
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

            else
            {
                return View();
            }

        }


        // POST: Proyectos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string nombre, string objetivo, TimeSpan duracionEstimada, string cedulaCliente, string cedulaLider)
        {



            Proyecto proyecto = new Proyecto();

            proyecto.objetivo = objetivo;
            proyecto.duracionEstimada = duracionEstimada;
            proyecto.cedulaClienteFK = cedulaCliente;



            //Verifica si el usuario cambio el nombre, ya que es unique
            if (nombre != proyecto.nombre)
            {

                //Hay que verificar si el nuevo nombre ya existe en la base de datos
                if (db.Proyecto.Where(p => p.nombre == nombre).FirstOrDefault() != null)
                {
                    //Existe un proyecto con ese nombre
                    ViewBag.error = "Ya existe un proyecto llamado: " + nombre;
                    ViewBag.permisos = seguridad.ProyectoAgregar(User);

                    //Selecciona todos los Clientes
                    ViewBag.cliente = db.Cliente.ToList();

                    //Selecciona todos los empelados que esten disponible y que sean Lider
                    ViewBag.lider = db.Empleado.Where(p => p.estado == "Disponible" && p.tipoTrabajo == "Lider").ToList();

                    return View(proyecto);
                }

            }


            proyecto.nombre = nombre;

            //Agrega el proyecto
            SetProyecto(proyecto, 0);

            ActualizarTrabajaEN(proyecto, cedulaLider, null);

            return RedirectToAction("Index");

        }




        // GET: Proyectos/Edit/5
        public ActionResult Edit(int? id)
        {



            //Revisa si el proyecto con el id ingresado existe
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyecto proyecto = db.Proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }



            var permisos = seguridad.ProyectoEditar(User);

            //Si tiene un rol permitido y puede editar
            if (permisos != null && permisos.Item2 != 3)
            {

                ViewBag.permisos = permisos;

                //Puede editar solo en los que el participa
                if (permisos.Item2 == 2)
                {

                    //Regresa una lista de los proyectos en los que participa el usuario
                    var proyectosList = GetProyectosUsuario(permisos.Item2, permisos.Item1, seguridad.IdUsuario(User));

                    //Busca si el proyecto que se quiere editar pertenece a esta persona
                    if (proyectosList.Where(p => p.idProyectoAID == proyecto.idProyectoAID).Any())
                    {
                        //Si no pertenece devuelve a la vista un null
                        return View();
                    }

                }


                //Llena los view bag
                CargarDatosClientesEdit(proyecto);
                CargarDatosLiderEdit(proyecto);


                return View(proyecto);


            }
            else
            {
                return View();
            }






        }


        

        // POST: Proyectos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string id, string nombre, string objetivo, string estado, TimeSpan duracionEstimada, TimeSpan duracionReal, DateTime fechaInicio, DateTime fechaFinalizacion, int cantidadReq, string cedulaCliente, string cedulaLiderActual, string cedulaLider)
        {

            int inProyectoAID = Int32.Parse(id);

            Proyecto proyecto = db.Proyecto.Find(inProyectoAID);

            //Actualiza los datos del proyecto
            proyecto.objetivo = objetivo;
            proyecto.estado = estado;
            proyecto.duracionEstimada = duracionEstimada;
            proyecto.duracionReal = duracionReal;
            proyecto.fechaInicio = fechaInicio;
            proyecto.fechaFinalizacion = fechaFinalizacion;
            proyecto.cantidadReq = cantidadReq;
            proyecto.cedulaClienteFK = cedulaCliente;

          
           
            //Verifica si el usuario cambio el nombre, ya que es unique
            if (nombre != proyecto.nombre)
            {
                
                //Hay que verificar si el nuevo nombre ya existe en la base de datos
                if(db.Proyecto.Where(p => p.nombre == nombre).FirstOrDefault() != null )
                {
                    //Existe un proyecto con ese nombre
                    ViewBag.error = "Ya existe un proyecto llamado: " + nombre;
                    ViewBag.permisos = seguridad.ProyectoEditar(User);

                    //Llena los view bag
                    CargarDatosClientesEdit(proyecto);
                    CargarDatosLiderEdit(proyecto);

                    return View(proyecto);
                }
            
            }

            proyecto.nombre = nombre;

            //Actualiza los datos del proyecto en la base de datos
            SetProyecto(proyecto, 1);

            //Actualiza/Agrega el lider al proyecto
            ActualizarTrabajaEN(proyecto, cedulaLider, cedulaLiderActual);


            return RedirectToAction("Index");

        }



        public ActionResult Eliminar(int id)
        {
            //Busca el proyecto
            Proyecto proyecto = GetProyecto(id);
            //Elimina el proyecto
            SetProyecto(proyecto, 2);
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
