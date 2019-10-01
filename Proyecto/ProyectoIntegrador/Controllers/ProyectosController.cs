using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.BaseDatos;

namespace ProyectoIntegrador.Controllers
{
    public class OrderOperatorViewModel
    {
    
    }


    public class ProyectosController : Controller
    {

   


        //Metodo para obtener la vista principal de los clientes
        public ActionResult IndexCliente()
        {
            return RedirectToAction("Index", "Clientes", null);
        }


        private Gr03Proy2Entities2 db = new Gr03Proy2Entities2();

        // GET: Proyectos
        public ActionResult Index()
        {
            var proyecto = db.Proyecto.Include(p => p.Cliente);
            return View(proyecto.ToList());
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
        public ActionResult Create(string nombre,string objetivo, TimeSpan duracionEstimada, string cedulaCliente, string cedulaLider)
        {
            Proyecto proyecto = new Proyecto();
            proyecto.nombre = nombre;
            proyecto.objetivo = objetivo;
            proyecto.duracionEstimada = duracionEstimada;
            proyecto.cedulaClienteFK = cedulaCliente;

            db.Proyecto.Add(proyecto);
            db.SaveChanges();

            if( !String.IsNullOrEmpty(cedulaLider) )
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

            

            }









            //Selecciona todos los Clientes
            ViewBag.cliente = db.Cliente.ToList();

            //Selecciona todos los empelados que esten disponible y que sean Lider
            ViewBag.lider = db.Empleado.Where(p => p.estado == "Disponible" && p.tipoTrabajo == "Lider").ToList();


            return View();
        }



        //// POST: Proyectos/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Proyecto proyecto)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Hola this is ");
        //        db.Proyecto.Add(proyecto);


        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.cedulaClienteFK = new SelectList(db.Cliente, "cedulaPK", "nombre", proyecto.cedulaClienteFK);
        //    return View(proyecto);
        //}

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
            ViewBag.cedulaClienteFK = new SelectList(db.Cliente, "cedulaPK", "nombre", proyecto.cedulaClienteFK);
            return View(proyecto);
        }

        // POST: Proyectos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProyectoAID,nombre,objetivo,estado,duracionReal,duracionEstimada,fechaInicio,fechaFinalizacion,cedulaClienteFK,cantidadReq")] Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proyecto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cedulaClienteFK = new SelectList(db.Cliente, "cedulaPK", "nombre", proyecto.cedulaClienteFK);
            return View(proyecto);
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
