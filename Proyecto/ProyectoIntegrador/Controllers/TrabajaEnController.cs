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
    public class TrabajaEnController : Controller
    {
        private Gr03Proy2Entities3 db = new Gr03Proy2Entities3();





        //-------------------------ActionResults--------------------------//
        /*
         * Efecto: Mustra la vista principal del equipo.
         * Requiere: --
         * Modifica: --
         */

        public ActionResult Index()
        {
            var trabajaEns = db.TrabajaEn.Include(t => t.Empleado).Include(t => t.Proyecto);
            return View(trabajaEns.ToList());
        }

        /*
         * Efecto: Muestra la vista para agregar empleados al equipo del proyecto.
         * Requiere: --
         * Modifica: --
         */
        public ActionResult Add()
        {
            ViewBag.idEmpleadoFK = new SelectList(db.Empleado, "idEmpleadoPK", "nombre");
            ViewBag.idProyectoFK = new SelectList(db.Proyecto, "idProyectoAID", "nombre");
            return View();
        }


        //---------------------------------------------------------------------------//
        //-----------------------------Rutinas del controlador-----------------------//

        /*Donde debería ir esto? 
        */
        public List<Empleado> GetEmpleados()
        {
            return db.Empleado.ToList();
        }

        // GET: TrabajaEn/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrabajaEn trabajaEn = db.TrabajaEn.Find(id);
            if (trabajaEn == null)
            {
                return HttpNotFound();
            }
            return View(trabajaEn);
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
