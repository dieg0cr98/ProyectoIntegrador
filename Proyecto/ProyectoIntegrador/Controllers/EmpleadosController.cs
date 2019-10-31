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

        public List<Empleado> GetTestersDisponibles()
        {
            return db.Empleado.Where(e => e.tipoTrabajo == "Tester" && e.estado == "Disponible").ToList();
        }


        // GET: Empleados
        public ActionResult Index()
        {
            var Empleado =
            from e in db.Empleado //Selecciona la tabla de Empledo
            where e.estado != "Despedido" //Solo los empleados que no estan despedidos
            select e; //Selecciona todo los atributos del empleado



            //= db.Empleado.Include(e => e.Tester);
            return View(Empleado.ToList());
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
            ViewBag.idEmpleadoPK = new SelectList(db.Tester, "idEmpleadoFK", "idEmpleadoFK");
            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(string idEmpleadoPK, string nombre, string apellido1, string apellido2,
            string correo, DateTime? fechaNacimiento, string provincia, string canton, string distrito, string direccion, string telefono, string estado, string tipoTrabajo)
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
                if (empleado.tipoTrabajo == "Tester") {
                Tester tester = new Tester();
                tester.cantidadRequerimientos = 0;
                tester.idEmpleadoFK = empleado.idEmpleadoPK;
                db.Tester.Add(tester);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idEmpleadoPK = new SelectList(db.Tester, "idEmpleadoFK", "idEmpleadoFK", empleado.idEmpleadoPK);
            return View(empleado);
        }
        // GET: Empleados/Edit/5
        public ActionResult Edit(string id)
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

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
           [HttpPost]
         public ActionResult Edit(string cedulaVieja, string idEmpleadoPK, string nombre, string apellido1, string apellido2,
         string correo, DateTime? fechaNacimiento, string provincia, string canton, string distrito, string direccion, string telefono, string estado, string tipoTrabajo)
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
            ViewBag.idEmpleadoPK = new SelectList(db.Tester, "idEmpleadoFK", "idEmpleadoFK", empleado.idEmpleadoPK);
            View(empleado);
            try
            {
                db.Database.SqlQuery<Empleado>("USP_editEmployeeId @cedulaVieja, @cedulaNueva", param).ToList();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Empleados/Delete/5
        public ActionResult Delete(string id)
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

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Empleado empleado = db.Empleado.Find(id);
            db.Empleado.Remove(empleado);
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

        public ActionResult Eliminar(string id)
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
            empleado.estado = "Despedido";
            db.SaveChanges();
            return RedirectToAction("Index");
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
                //Existe un cliente con esa cedula
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
                //Existe un cliente con ese correo
                return new JsonResult { Data = false };
            }
            else
                return new JsonResult { Data = true };
        }
    }
}
