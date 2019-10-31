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
using ProyectoIntegrador.Controllers;


namespace ProyectoIntegrador.Controllers
{
    public class ClientesController : Controller
    {
        private Gr03Proy2Entities6 db = new Gr03Proy2Entities6();
        private SeguridadController seguridad = new SeguridadController();

        // GET: Clientes

        public ActionResult Index()
        {
            var permisosGenerales = seguridad.ClienteConsultar(User);
            ViewBag.permisosEspecificos = permisosGenerales;

            ViewBag.cedCliente = permisosGenerales.Item2;

            //Verifica que el usuario este registrado
            if (permisosGenerales.Item1 >= 0)
            {
                //GetClientesVista(int permiso, int rol, string idUsuario)
                return View(GetClientesVista(permisosGenerales.Item3, permisosGenerales.Item1, permisosGenerales.Item2).Reverse());
            }
            else
            {
                return View();
            }
        }

        // GET: Clientes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            var permisosGenerales = seguridad.ClienteConsultar(User);

            //Verifica que el usuario este registrado y que tenga permiso de crear = 1
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item5 == 1)
            {
                return View();
            }
            return RedirectToAction("Index");
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(string cedulaPK, string nombre, string apellido1, string apellido2, string empresa, string provincia,
            string canton, string distrito, string direccionExacta, string telefono, string correo)
        {
            var permisosGenerales = seguridad.ClienteConsultar(User);

            //Verifica que el usuario este registrado y que tenga permiso de crear = 1
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item5 == 1)
            {
                Cliente cliente = new Cliente();
                if (ModelState.IsValid)
                {
                    cliente.nombre = nombre;
                    cliente.apellido1 = apellido1;
                    cliente.apellido2 = apellido2;
                    cliente.empresa = empresa;
                    cliente.provincia = provincia;
                    cliente.canton = canton;
                    cliente.distrito = distrito;
                    cliente.provincia = provincia;
                    cliente.direccionExacta = direccionExacta;
                    cliente.telefono = telefono;
                    cliente.cedulaPK = cedulaPK;
                    cliente.correo = correo;


                    db.Cliente.Add(cliente);
                    db.SaveChanges();

                    await seguridad.AgregarUsuarioAsync(correo, "Cliente"); //crea cuenta de usuario en el sistema

                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //Metodo para verificar si una cedula de cliente ya existe
        //Recibe string cedulaPK. Contiene la cedula que se quiere verificar
        //string oldName. Contiene la cedula Actual del cliente
        //Devuelve un JsonResult con un True si ya existe un cliente con esa cedula y un false si no
        public JsonResult CheckCedula(string cedulaPK, string oldcedulaPK)
        {
            //Hay que verificar si la nueva cedula ya existe en la base de datos
            if ((cedulaPK != oldcedulaPK) && (db.Cliente.Where(i => i.cedulaPK == cedulaPK).FirstOrDefault() != null))
            {
                //Existe un cliente con esa cedula
                return new JsonResult { Data = false };
            }
            else
                return new JsonResult { Data = true };
        }

        //Metodo para verificar si un correo de cliente ya existe
        //Recibe string email. Contiene el email que se quiere verificar
        //string oldEmail. Contiene el email Actual del cliente
        //Devuelve un JsonResult con un True si ya existe un cliente con esa cedula y un false si no
        public JsonResult CheckEmail(string email, string oldEmail)
        {
            //Hay que verificar si el nuevo correo ya existe en la base de datos
            if ((email != oldEmail) && (db.Cliente.Where(i => i.correo == email).FirstOrDefault() != null))
            {
                //Existe un cliente con ese correo
                return new JsonResult { Data = false };
            }
            else
                return new JsonResult { Data = true };
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(string id)
        {
            var permisosGenerales = seguridad.ClienteConsultar(User);

            //Verifica que el usuario este registrado y que tenga permiso de editar = 1
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item4 == 1)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Cliente cliente = db.Cliente.Find(id);
                if (cliente == null)
                {
                    return HttpNotFound();
                }
                return View(cliente);
            }
            return RedirectToAction("Index");
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string cedulaVieja, string cedulaPK, string nombre, string apellido1, string apellido2, string empresa, string provincia,
            string canton, string distrito, string direccionExacta, string telefono, string correo)
        {
            var permisosGenerales = seguridad.ClienteConsultar(User);

            //Verifica que el usuario este registrado y que tenga permiso de editar = 1
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item4 == 1)
            {
                Cliente cliente = db.Cliente.Find(cedulaVieja);

                cliente.nombre = nombre;
                cliente.apellido1 = apellido1;
                cliente.apellido2 = apellido2;
                cliente.empresa = empresa;
                cliente.provincia = provincia;
                cliente.canton = canton;
                cliente.distrito = distrito;
                cliente.provincia = provincia;
                cliente.direccionExacta = direccionExacta;
                cliente.telefono = telefono;
                cliente.cedulaPK = cedulaPK;
                cliente.correo = correo;

                if (ModelState.IsValid)
                {
                    db.Entry(cliente).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Cliente cliente = db.Cliente.Find(id);
            db.Cliente.Remove(cliente);
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

        //Metodo para eliminar directamente un cliente
        public async System.Threading.Tasks.Task<ActionResult> Eliminar(string id)
        {
            var permisosGenerales = seguridad.ClienteConsultar(User);

            //Verifica que el usuario este registrado y que tenga permiso de editar = 1
            if (permisosGenerales.Item1 >= 0 && permisosGenerales.Item6 == 1)
            {
                Cliente cliente = db.Cliente.Find(id); // Se busca el cliente en la bd

                await seguridad.DeleteUsuarioAsync(cliente.correo); //crea cuenta de usuario en el sistema

                db.Cliente.Remove(cliente); // se elimina cliente de la bd
                db.SaveChanges(); // se guardan los cambios
                return RedirectToAction("Index"); // se devuelve al index.
            }
            else
            {
                return RedirectToAction("Index"); // se devuelve al index.
            }
        }

        //Metodo para recuperar los clientes
        //Recibe int permiso (Tomados de la tabla de seguridad)
        //           permiso = 1 Puede ver todos los clientes
        //           permiso = 2 Solo puede ver los clientes asociados a su proyecto
        //           permiso = 3 No puede ver ninguno
        //       int rol (El usuario tiene que tener un rol asignado antes de llamar a este metodo)
        //              rol = 0 Soporte/Calidad
        //              rol = 1 Lider
        //              rol = 2 Tester
        //              rol = 3 Cliente
        //       string idUsuario (Es la cedula o identificador de un usuario) 
        //Devuelve una lista IEnumerable con los proyectos. Null en caso de que no puede ver ninguno
        public IEnumerable<ProyectoIntegrador.BaseDatos.Cliente> GetClientesVista(int permiso, int rol, string idUsuario)
        {

            //Puede ver todos los proyectos
            if (permiso == 1)
            {
                return db.Cliente.ToList();
            }
            else
            {
                //Solo puede ver los proyectos en los que participa
                if (permiso == 2)
                {
                    //Si es cliente
                    if (rol == 3)
                    {
                        //Selecciona solo los datos personales relacionados a dicho cliente
                        return db.Cliente.Where(c => c.cedulaPK == idUsuario);
                    }
                    else //es empleado
                    {
                        var innerJoinQuery =
                        from p in db.Proyecto //Selecciona la tabla de Proyectos
                        join t in db.TrabajaEn on p.idProyectoAID equals t.idProyectoFK //Hace join con la tabla TrabajaEn
                        join e in db.Empleado on t.idEmpleadoFK equals e.idEmpleadoPK //Hace join con la tabla Empleado
                        join c in db.Cliente on p.cedulaClienteFK equals c.cedulaPK //hace join con proyecto y cliente
                        where c.cedulaPK == idUsuario //Solo los clientes relacionados a su proyecto
                        select c; //Selecciona todo los atributos del proyecto
                        return innerJoinQuery.ToList();
                    }
                }
                //No tiene permisos
                else
                    return null;
            }
        }
    }
}
