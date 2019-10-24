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


namespace ProyectoIntegrador.Controllers
{
    public class ClientesController : Controller
    {
        private Gr03Proy2Entities6 db = new Gr03Proy2Entities6();

        // GET: Clientes
        public ActionResult Index()
        {
            return View(db.Cliente.ToList());
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
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string cedulaPK, string nombre, string apellido1, string apellido2, string empresa, string provincia,
            string canton, string distrito, string direccionExacta, string telefono, string correo)
         //[Bind(Include = "cedulaPK,nombre,apellido1,apellido2,empresa,provincia,canton,distrito,direccionExacta,telefono,correo")] Cliente cliente)
        {
            Cliente cliente = new Cliente();
            
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
                db.Cliente.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cliente);
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

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string cedulaVieja, string cedulaPK, string nombre, string apellido1, string apellido2, string empresa, string provincia,
            string canton, string distrito, string direccionExacta, string telefono, string correo)
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
            return View(cliente);
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
        public ActionResult Eliminar(string id)
        {
            Cliente cliente = db.Cliente.Find(id); // Se busca el cliente en la bd
            db.Cliente.Remove(cliente); // se elimina cliente de la bd
            db.SaveChanges(); // se guardan los cambios
            return RedirectToAction("Index"); // se devuelve al index.
        }
    }
}
