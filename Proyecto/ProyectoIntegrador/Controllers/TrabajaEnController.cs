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
        private Gr03Proy2Entities5 db = new Gr03Proy2Entities5();
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
            ViewBag.testers = db.Empleado.Where(p => p.estado == "Disponible" && p.tipoTrabajo == "Tester").ToList();
            //Selecciona todos los empelados que esten disponible y que sean Lider
            ViewBag.lideres = db.Empleado.Where(p => p.estado == "Disponible" && p.tipoTrabajo == "Lider").ToList();

            //Query para seleccionar integrantes del equipo asociados al proyecto
            var equipo = from P in db.Proyecto
                         join TB in db.TrabajaEn on P.idProyectoAID equals TB.idProyectoFK
                         join E in db.Empleado on TB.idEmpleadoFK equals E.idEmpleadoPK
                         where P.idProyectoAID == idProyecto && TB.estado == "Activo"
                         select E;
            ViewBag.equipoActual = equipo;
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

/*
 

        <p>
                            Equipo
                            <ul id="sortable2" class="connectedSortable " style="min-height:100px">
                            </ul>
                        </p>


       <p>
                            <ul id="sortable1" class="connectedSortable">
                                @foreach (var item in Model)
                                {
                                    <li class="ui-state-default" value="@item.idProyectoFK"> @item.idEmpleadoFK @item.idProyectoFK </li>
                                }
                            </ul>
                        </p> 

    -----------------------------------------------------
                        <a data-toggle="tooltip" onclick="my_loading(this,'30px','30px')" data-placement="top" title="Agregar Integrantes" href="/TrabajaEn/Add" class="btn">
                        <img src="~/Content/plus.svg" class="rounded float-right" style="max-height:30px ; max-width:30px">
                    </a>
                    <a data-toggle="tooltip" onclick="my_loading(this,'30px','30px')" data-placement="top" title="Eliminar Integrantes" href="/TrabajaEn/Delete" class="btn">
                        <img src="~/Content/delete.svg" class="rounded float-right" style="max-height:30px ; max-width:30px">
                    </a>
*/
