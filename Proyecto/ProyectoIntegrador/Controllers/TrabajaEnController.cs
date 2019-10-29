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
        private Gr03Proy2Entities6 db = new Gr03Proy2Entities6();
        private SeguridadController seguridad = new SeguridadController();

        /*
         * Efecto: Obtiene todos los valores requeridos en la vista de equipo y los añade al viewbag.
         * Requiere: Un id de proyecto valido
         * Modifica: Agrega variables al ViewBag
         */
        private void GetDatosVistaEquipo(int idProyecto)
        {
            //Encuentra el proyecto asociado al id
            ViewBag.proyectoActual = db.Proyecto.Find(idProyecto);
            //Consigue los permisos del usuario actual.
            ViewBag.permisosActuales = seguridad.EquipoConsultar(User);

            //Selecciona todos los empelados que esten disponible y que sean tester
            ViewBag.testers = db.USP_GetTestersDisponibles().ToList();
            
            //Selecciona todos los empelados que esten disponible y que sean Lider
            ViewBag.lideres = db.USP_GetLideresDisponibles().ToList();

            //Procedimiento almacenado para encontrar integrantes del equipo asociados al proyecto
            ViewBag.equipoActual = db.USP_GetEquipo(idProyecto);
        }

        //-------------------------ActionResults--------------------------//
        /*
         * Efecto: Mustra la vista principal del equipo.
         * Requiere: --
         * Modifica: --
         */
        public ActionResult Index(int? idProyecto)
        {
            if (idProyecto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                int id = idProyecto ?? default(int);
                GetDatosVistaEquipo(id);
                var trabajaEn = db.TrabajaEn.Where(t => t.idProyectoFK == idProyecto);
                return View(trabajaEn.ToList());
            }
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

        /*
         * 
         */
        public List<Empleado> GetEmpleados()
        {
            return db.Empleado.ToList();
        }


        /*
         * Efecto: Agrega un integrante nuevo al equipo
         * Requiere: Parámetros válidos que no hagan conflicto con la base de datos
         * Modifica: El empleado que se integra al equipo y la tabla TrabajaEn
         */
        //deberia retornar json result para verificar
        public void AgregarIntegrante(int idProyecto, string idEmpleado, string rolEmpleado)
        {

            //System.Diagnostics.Debug.WriteLine(idProyecto + " " + idEmpleado + " " + rolEmpleado);
            TrabajaEn trabaja = new TrabajaEn();
            Empleado empleado = db.Empleado.Find(idEmpleado);
            empleado.estado = "Ocupado";
            trabaja.idProyectoFK = idProyecto;
            trabaja.idEmpleadoFK = empleado.idEmpleadoPK;
            trabaja.rol = rolEmpleado;
            trabaja.estado = "Activo";

            db.Entry(empleado).State = EntityState.Modified;
            db.TrabajaEn.Add(trabaja);
            db.SaveChanges();
        }


        public void QuitarIntegrante(int idProyecto, string idEmpleado, string rolEmpleado)
        {
        
            Empleado empleado = db.Empleado.Find(idEmpleado);
            Proyecto proyecto = db.Proyecto.Find(idProyecto);
            TrabajaEn trabaja = db.TrabajaEn.Find(idProyecto, idEmpleado);
            empleado.estado = "Disponible";
            db.Entry(empleado).State = EntityState.Modified;

            //Si el proyecto está activo guarda el empleado para cuestión de historial pero lo pone inactivo.
            if (proyecto.estado == "Activo")
            {
                trabaja.estado = "Inactivo";
                db.Entry(trabaja).State = EntityState.Modified;
            } else //Si no lo elimina del equipo.
            {
                db.TrabajaEn.Remove(trabaja);
            }
           
          
            db.SaveChanges();
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
