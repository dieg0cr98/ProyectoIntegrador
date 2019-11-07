using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.BaseDatos;
using ProyectoIntegrador.Models;
using System.Data.SqlClient;

namespace ProyectoIntegrador.Controllers
{
    public class EmpleadosController : Controller
    {
        private Gr03Proy2Entities6 db = new Gr03Proy2Entities6();
         private HabilidadBlanda habilidadBlanda = new HabilidadBlanda();
        private HabilidadTecnica habilidadTecnica = new HabilidadTecnica();

       

        internal dynamic GetNombreEmpleado(string id)
        {
            return db.Empleado.Find(id).nombre +" "+ db.Empleado.Find(id).apellido1;
        }

        private SeguridadController seguridad = new SeguridadController();

        //Metodo que regresa el contexto de la tabla Empleado, para poder realizar joins
        //Retorna el contexto de la base de datos
        public DbSet<Empleado> GetTablaEmpleado()
        {
            return db.Empleado;
        }

        public List<Empleado> GetTestersDisponibles()
        {
            return db.Empleado.Where(e => e.tipoTrabajo == "Tester" && e.estado == "Disponible").ToList();
        }

        //Metodo que guarda/Eliminar en la tabla Empleado
        //Recibe Empleado empleado, con los datos para guardar
        //       int  tipo, flag para saber el tipo de accion
        //            tipo 0 = nuevo empleado
        //            tipo 1 = actualizar empleado
        //            tipo 2 = borrar empleado
        //Returna un bool, flag para saber si la operacion se pudo completar
        //           bool = false no se pudo realizar la operacion
        //           bool = true se pudo realizar la operacion
        public bool SetEmpleado(Empleado empleado, int tipo)
        {
            try
            {
                switch (tipo)
                {
                    case 0:
                        {
                            db.Empleado.Add(empleado);
                            db.SaveChanges();
                            break;
                        }
                    case 1:
                        {
                            db.Entry(empleado).State = EntityState.Modified;
                            db.SaveChanges();
                            break;
                        }
                    case 2:
                        {
                            db.Empleado.Remove(empleado);
                            db.SaveChanges();
                            break;
                        }
                }

                if (tipo == 0)
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

        //Metodo que retorna todos los Empleados
        //Retorna una lista con los empleados, de no tener la lista queda vacia
        public IEnumerable<ProyectoIntegrador.BaseDatos.Empleado> GetEmpleado()
        {
            return db.Empleado.ToList();
        }

        //Metodo que retorna un empleado especifico
        //Recibe un int idEmpleadoPK, llave primaria del empleado
        public Empleado GetEmpleado(int idEmpleadoPK)
        {
            return db.Empleado.Find(idEmpleadoPK);
        }

        public async System.Threading.Tasks.Task<ActionResult> Agregar()
        {
            //......Code........//
            await seguridad.AgregarUsuarioAsync("test2@gmail.com", "Lider");
            //......Code........//

            return RedirectToAction("Index");
        }

        //Metodo para recuperar los empleados que quisiera accesar un usuario.
        //Recibe int permiso (Tomados de la tabla de seguridad)
        //          permiso = 1 Puede ver todos los proyectos
        //           permiso = 2 Solo puede ver los proyectos en los que participa
        //           permiso = 3 No puede ver ninguno
        //       int rol (El usuario tiene que tener un rol asignado antes de llamar a este metodo)
        //              rol = 0 Soporte/Calidad
        //              rol = 1 Lider
        //              rol = 2 Tester
        //              rol = 3 Cliente
        //       string idUsuario (Es la cedula o identificador de un usuario)
        //Devuelve una lista IEnumerable con los proyectos. Null en caso de que no puede ver ninguno
        public IEnumerable<ProyectoIntegrador.BaseDatos.Empleado> GetEmpleadosUsuario(int permiso, int rol, string idUsuario)
        {
            if (permiso == 1)
            {
                var innerJoin =
                from e in db.Empleado //Selecciona la tabla de Empleado
                where e.estado != "Despedido" //Solo los empleados que no estan despedidos
                select e; //Selecciona todo los atributos del empleado
                return innerJoin.ToList();
            }
            else
            {
                //Solo puede ver los proyectos en los que participa
                if (permiso == 2)
                {

                    if (rol == 1)//El lider puede ver a los empleados que trabajan en un proyecto donde el es lider
                    {
                        SqlParameter[] param = new SqlParameter[] {
                        new SqlParameter("@cedulaLider", idUsuario)

                        };
                        return db.Database.SqlQuery<Empleado>("USP_GetEmpleadosDeLider @cedulaLider", param).ToList();
                    }
                    else if (rol == 2)
                    {
                        return db.Empleado.Where(e => e.idEmpleadoPK == idUsuario).ToList();
                    }
                    else
                    {
                        return null;
                    }

                }
                //El tester solo se puede ver su informacion
                else return null;

            }
        }


        // GET: Empleados
        public ActionResult Index()
        {
            var permisosGenerales = seguridad.EmpleadoConsultar(User);
            ViewBag.permisosEspecificos = permisosGenerales;
            ViewBag.cedEmpleado = permisosGenerales.Item2;

            //Verifica que el usuario este registrado
            if (permisosGenerales.Item1 >= 0)
            {
                //GetEmpleadosUsuario(int permiso, int rol, string idUsuario)
                return View(GetEmpleadosUsuario(permisosGenerales.Item3, permisosGenerales.Item1, permisosGenerales.Item2).Reverse());
            }
            else
            {
                return View();
            }
        }

        // GET: Empleados/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleado.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // GET: Empleados/Create
        public ActionResult Create()
        {
            var permisosGenerales = seguridad.EmpleadoConsultar(User);
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item5 == 1)
            {
                ViewBag.idEmpleadoPK = new SelectList(db.Tester, "idEmpleadoFK", "idEmpleadoFK");
                return View();
            }
            return RedirectToAction("Index");
        }

         // POST: Empleados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Create(string habilidadesTecnicas, string habilidadesBlandas, string idEmpleadoPK, string nombre, string apellido1, string apellido2, string correo, DateTime? fechaNacimiento, string provincia, string canton, string distrito, string direccion, string telefono, string estado, string tipoTrabajo)
        {
            var permisosGenerales = seguridad.EmpleadoConsultar(User);

            
            
            if (habilidadesTecnicas != null) {
                List<string> TagIds = habilidadesTecnicas.Split(',').ToList();
                foreach (string v in TagIds)
            {
                HabilidadTecnica habilidad = new HabilidadTecnica();
                habilidad.idEmpleadoFK = idEmpleadoPK;
                habilidad.habilidad = v;
                db.HabilidadTecnica.Add(habilidad);
            }
        }
            if (habilidadesBlandas != null) {
                List<string> TagBlanda = habilidadesBlandas.Split(',').ToList();
                foreach (string v in TagBlanda)
            {
                HabilidadBlanda habilidadB = new HabilidadBlanda();
                habilidadB.idEmpleadoFK = idEmpleadoPK;
                habilidadB.habilidad = v;
                db.HabilidadBlanda.Add(habilidadB);
            }
            }

            //Verifica que el usuario este registrado y que tenga permiso de crear = 1
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item5 == 1)
            {
                Empleado empleado = new Empleado();
                
                    empleado.idEmpleadoPK = idEmpleadoPK;
                    empleado.correo = correo;
                    empleado.nombre = nombre;
                    empleado.apellido1 = apellido1;
                    empleado.apellido2 = apellido2;
                    empleado.fechaNacimiento = (DateTime)fechaNacimiento;
                    empleado.estado = estado;
                    empleado.telefono = telefono;
                    empleado.provincia = provincia;
                    empleado.canton = canton;
                    empleado.distrito = distrito;
                    empleado.direccion = direccion;
                    empleado.tipoTrabajo = tipoTrabajo;

                    if (ModelState.IsValid)
                    {
                        db.Empleado.Add(empleado);
                        if (empleado.tipoTrabajo == "Tester")
                        {
                            Tester tester = new Tester();
                            tester.cantidadRequerimientos = 0;
                            tester.idEmpleadoFK = empleado.idEmpleadoPK;
                            db.Tester.Add(tester);
                        }
                        db.SaveChanges();
                        await seguridad.AgregarUsuarioAsync(correo, empleado.tipoTrabajo); //crea cuenta de usuario en el sistema
                        return RedirectToAction("Index");
                    }

                    ViewBag.idEmpleadoPK = new SelectList(db.Tester, "idEmpleadoFK", "idEmpleadoFK", empleado.idEmpleadoPK);
                    return View(empleado);

            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        // GET: Empleados/Edit/5
        public ActionResult Edit(string id)
        {
            var permisosGenerales = seguridad.EmpleadoConsultar(User);
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item4 == 1)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Empleado empleado = db.Empleado.Find(id);
                if (empleado == null)
                {
                    return HttpNotFound();
                }

                ViewBag.idEmpleadoPK = new SelectList(db.Tester, "idEmpleadoFK", "idEmpleadoFK", empleado.idEmpleadoPK);
                return View(empleado);
            }
            return RedirectToAction("Index");
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(string cedulaVieja, string idEmpleadoPK, string nombre, string apellido1, string apellido2,
      string correo, DateTime? fechaNacimiento, string provincia, string canton, string distrito, string direccion, string telefono, string estado, string tipoTrabajo)
        {
            var permisosGenerales = seguridad.EmpleadoConsultar(User);

            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item4 == 1)
            {
                Empleado empleado = db.Empleado.Find(cedulaVieja);

                empleado.nombre = nombre;
                empleado.apellido1 = apellido1;
                empleado.apellido2 = apellido2;
                empleado.fechaNacimiento = (DateTime)fechaNacimiento;
                empleado.estado = estado;
                empleado.telefono = telefono;
                empleado.provincia = provincia;
                empleado.canton = canton;
                empleado.distrito = distrito;
                empleado.direccion = direccion;
                empleado.tipoTrabajo = tipoTrabajo;
                empleado.correo = correo;

                SqlParameter[] param = new SqlParameter[] {
                new SqlParameter("@cedulaVieja", cedulaVieja),
                new SqlParameter("@cedulaNueva",idEmpleadoPK),
            };

                db.Entry(empleado).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                ViewBag.idEmpleadoPK = new SelectList(db.Tester, "idEmpleadoFK", "idEmpleadoFK", empleado.idEmpleadoPK);
                View(empleado);
                try
                {
                    ViewBag.idEmpleadoPK = db.Database.SqlQuery<Empleado>("USP_editEmployeeId @cedulaVieja, @cedulaNueva", param).ToList(); return View(empleado);
                }
                catch
                {
                    return RedirectToAction("Index");
                }
            }
            else return RedirectToAction("Index");

        }
        // GET: Empleados/Delete/5
        public ActionResult Delete(string id)
        {
            var permisosGenerales = seguridad.EmpleadoConsultar(User);
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item6 == 1)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Empleado empleado = db.Empleado.Find(id);
                if (empleado == null)
                {
                    return HttpNotFound();
                }
                return View(empleado);
            }
            return RedirectToAction("Index");
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var permisosGenerales = seguridad.EmpleadoConsultar(User);
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item6 == 1)
            {
                Empleado empleado = db.Empleado.Find(id);
                db.Empleado.Remove(empleado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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

        public async System.Threading.Tasks.Task<ActionResult> Eliminar(string id)
        {
            var permisosGenerales = seguridad.EmpleadoConsultar(User);
            Empleado empleado = db.Empleado.Find(id);
            //Verifica que el usuario este registrado y que tenga permiso de editar = 1
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item6 == 1 && empleado.estado != "Despedido")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                await seguridad.DeleteUsuarioAsync(empleado.correo); //crea cuenta de usuario en el sistema
                if (empleado == null)
                {
                    return HttpNotFound();
                }
                empleado.estado = "Despedido";
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return RedirectToAction("Index"); // se devuelve al index.

        }

        public Empleado getTesterAsociado(string cedulaTester)
        {
            return db.Empleado.Where(E => E.idEmpleadoPK == cedulaTester).FirstOrDefault();
        }


        public JsonResult CheckCedula(string cedulaPK, string oldcedulaPK)
        {
            
            //Hay que verificar si la nueva cedula ya existe en la base de datos
            if ((cedulaPK != oldcedulaPK) && (db.Empleado.Where(i => i.idEmpleadoPK == cedulaPK).FirstOrDefault() != null))
            {
                //Existe un empleado con esa cedula
                return new JsonResult { Data = false };
            }
            else
                return new JsonResult { Data = true };
        }
        public JsonResult CheckEmail(string email, string oldEmail)
        {
            //Hay que verificar si el nuevo correo ya existe en la base de datos
            if ((email != oldEmail) && (db.Empleado.Where(i => i.correo == email).FirstOrDefault() != null))
            {
                //Existe un empleado con ese correo
                return new JsonResult { Data = false };
            }
            else
                return new JsonResult { Data = true };
        }
    }
}
