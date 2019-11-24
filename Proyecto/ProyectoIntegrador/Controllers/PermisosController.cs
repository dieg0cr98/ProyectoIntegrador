using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.Models;
using System.IO;
using ProyectoIntegrador.BaseDatos;
namespace ProyectoIntegrador.Controllers
{
    public class PermisosController : Controller
    {
        private string[] tipo = { "Total", "Parcial", "Ninguno" };

        private SeguridadController seguridad = new SeguridadController();




        /*
        * Efecto: Obtiene todos los valores requeridos en la vista de Permisos y los añade al viewbag.
        * Requiere: 
        * Modifica: Agrega permisos a los ViewBags
        * Retorna:  La vista Index de Permisos
        */
        public ActionResult Index()
        {

            var rol = seguridad.GetRoleUsuario(User);
            //Verifica que sea un jefe de calidad o soporte
            if (rol == 0)
            {
                ViewBag.permiso = true;
                ViewBag.proyecto = seguridad.getTablaSeguridadProyectoGeneral();
                ViewBag.req = seguridad.getTablaSeguridadRequerimientosGeneral();
                ViewBag.empleado = seguridad.getTablaSeguridadEmpleadosGeneral();
                ViewBag.cliente = seguridad.getTablaSeguridadClientesGeneral();
                ViewBag.equipo = seguridad.getTablaSeguridadEquipoGeneral();
                ViewBag.prueba = seguridad.getTablaSeguridadPruebasGeneral();
                return View();
            }
            else
            {
                ViewBag.permiso = false;
                return View();
            }
 
        }

        /*
        * Efecto: Carga la vista de permisos generales para Proyecto
        * Requiere: 
        * Modifica: Agrega permisos al ViewBag
        * Retorna:  La vista de permisos generales para Proyecto
        */
        public ActionResult ProyectoGeneral()
        {
            ViewBag.permisos = seguridad.getTablaSeguridadProyectoGeneral();
            ViewBag.saved = 0;
            return View("ProyectoGeneral");
        }




        /*
        * Efecto: Modificar permisos generales para Proyecto
        * Requiere: CS = Permiso Consultar Soporte/Calidad
        *           CL = Permiso Consultar Lider
        *           CT = Permiso Consultar Tester
        *           CC = Permiso Consultar Cliente
        *           AS = Permiso Agregar Soporte/Calidad
        *           AL = Permiso Agregar Lider
        *           AT = Permiso Agregar Tester
        *           AC = Permiso Agregar Cliente
        *           ES = Permiso Editar Soporte/Calidad
        *           EL = Permiso Editar Lider
        *           ET = Permiso Editar Tester
        *           EC = Permiso Editar Cliente
        *           BS = Permiso Eliminar Soporte/Calidad
        *           BL = Permiso Eliminar Lider
        *           BT = Permiso Eliminar Tester
        *           BC = Permiso Eliminar Cliente
        * Modifica: Permisos generales para Proyecto
        * Retorna:  La vista de permisos generales post para Proyecto
        */
        [HttpPost]
        public ActionResult ProyectoGeneral(string CS, string CL, string CT, string CC, string AS, string AL, string AT, string AC,
            string ES, string EL, string ET, string EC, string BS, string BL, string BT, string BC)
        {


            ////Cambia los permisos para los Jefe de Calidad / Soporte
            seguridad.setTablaSeguridadProyectoGeneral(0, new List<int>() { Array.IndexOf(tipo, CS) + 1, Array.IndexOf(tipo, AS) + 1, Array.IndexOf(tipo, ES) + 1, Array.IndexOf(tipo, BS) + 1 });

            ////Cambia los permisos para los Liders
            seguridad.setTablaSeguridadProyectoGeneral(1, new List<int>() { Array.IndexOf(tipo, CL) + 1, Array.IndexOf(tipo, AL) + 1, Array.IndexOf(tipo, EL) + 1, Array.IndexOf(tipo, BL) + 1 });

            ////Cambia los permisos para los Tester
            seguridad.setTablaSeguridadProyectoGeneral(2, new List<int>() { Array.IndexOf(tipo, CT) + 1, Array.IndexOf(tipo, AT) + 1, Array.IndexOf(tipo, ET) + 1, Array.IndexOf(tipo, BT) + 1 });

            ////Cambia los permisos para los Clientes
            seguridad.setTablaSeguridadProyectoGeneral(3, new List<int>() { Array.IndexOf(tipo, CC) + 1, Array.IndexOf(tipo, AC) + 1, Array.IndexOf(tipo, EC) + 1, Array.IndexOf(tipo, BC) + 1 });

            return RedirectToAction("ProyectoGeneralPost");

        }

        /*
        * Efecto: Carga la vista de permisos generales para Proyecto con un mensaje de confirmacion
        * Requiere: 
        * Modifica: Agrega permisos al ViewBag
        * Retorna:  La vista de permisos generales para Proyecto
        */    
        public ActionResult ProyectoGeneralPost()
        {
            ViewBag.permisos = seguridad.getTablaSeguridadProyectoGeneral();
            ViewBag.saved = 1;
            return View("ProyectoGeneral");
        }

        //--------------------------Equipo--------------------------//


        /*
        * Efecto: Carga la vista de permisos generales para Equipo
        * Requiere: 
        * Modifica: Agrega permisos al ViewBag
        * Retorna:  La vista de permisos generales para Equipo
        */
        public ActionResult EquipoGeneral()
        {
            ViewBag.permisos = seguridad.getTablaSeguridadEquipoGeneral();
            ViewBag.saved = 0;
            return View("EquipoGeneral");
        }


        /*
        * Efecto: Modificar permisos generales para Equipo
        * Requiere: CS = Permiso Consultar Soporte/Calidad
        *           CL = Permiso Consultar Lider
        *           CT = Permiso Consultar Tester
        *           CC = Permiso Consultar Cliente
        *           AS = Permiso Agregar Soporte/Calidad
        *           AL = Permiso Agregar Lider
        *           AT = Permiso Agregar Tester
        *           AC = Permiso Agregar Cliente
        *           ES = Permiso Editar Soporte/Calidad
        *           EL = Permiso Editar Lider
        *           ET = Permiso Editar Tester
        *           EC = Permiso Editar Cliente
        *           BS = Permiso Eliminar Soporte/Calidad
        *           BL = Permiso Eliminar Lider
        *           BT = Permiso Eliminar Tester
        *           BC = Permiso Eliminar Cliente
        * Modifica: Permisos generales para Equipo
        * Retorna:  La vista de permisos generales post para Equipo
        */
        [HttpPost]
        public ActionResult EquipoGeneral(string CS, string CL, string CT, string CC, string AS, string AL, string AT, string AC,
    string ES, string EL, string ET, string EC, string BS, string BL, string BT, string BC)
        {


            ////Cambia los permisos para los Jefe de Calidad / Soporte
            seguridad.setTablaSeguridadEquipoGeneral(0, new List<int>() { Array.IndexOf(tipo, CS) + 1, Array.IndexOf(tipo, AS) + 1, Array.IndexOf(tipo, ES) + 1, Array.IndexOf(tipo, BS) + 1 });

            ////Cambia los permisos para los Liders
            seguridad.setTablaSeguridadEquipoGeneral(1, new List<int>() { Array.IndexOf(tipo, CL) + 1, Array.IndexOf(tipo, AL) + 1, Array.IndexOf(tipo, EL) + 1, Array.IndexOf(tipo, BL) + 1 });

            ////Cambia los permisos para los Tester
            seguridad.setTablaSeguridadEquipoGeneral(2, new List<int>() { Array.IndexOf(tipo, CT) + 1, Array.IndexOf(tipo, AT) + 1, Array.IndexOf(tipo, ET) + 1, Array.IndexOf(tipo, BT) + 1 });

            ////Cambia los permisos para los Clientes
            seguridad.setTablaSeguridadEquipoGeneral(3, new List<int>() { Array.IndexOf(tipo, CC) + 1, Array.IndexOf(tipo, AC) + 1, Array.IndexOf(tipo, EC) + 1, Array.IndexOf(tipo, BC) + 1 });

            return RedirectToAction("EquipoGeneralPost");

        }

        /*
        * Efecto: Carga la vista de permisos generales para Equipo con un mensaje de confirmacion
        * Requiere: 
        * Modifica: Agrega permisos al ViewBag
        * Retorna:  La vista de permisos generales para Equipo
        */
        public ActionResult EquipoGeneralPost()
        {
            ViewBag.permisos = seguridad.getTablaSeguridadEquipoGeneral();
            ViewBag.saved = 1;
            return View("EquipoGeneral");
        }


        //--------------------------Requerimiento--------------------------//


        /*
        * Efecto: Carga la vista de permisos generales para Requerimiento
        * Requiere: 
        * Modifica: Agrega permisos al ViewBag
        * Retorna:  La vista de permisos generales para Requerimiento
        */
        public ActionResult RequerimientoGeneral()
        {
            ViewBag.permisos = seguridad.getTablaSeguridadRequerimientosGeneral();
            ViewBag.saved = 0;
            return View("RequerimientoGeneral");
        }


        /*
        * Efecto: Modificar permisos generales para Requerimiento
        * Requiere: CS = Permiso Consultar Soporte/Calidad
        *           CL = Permiso Consultar Lider
        *           CT = Permiso Consultar Tester
        *           CC = Permiso Consultar Cliente
        *           AS = Permiso Agregar Soporte/Calidad
        *           AL = Permiso Agregar Lider
        *           AT = Permiso Agregar Tester
        *           AC = Permiso Agregar Cliente
        *           ES = Permiso Editar Soporte/Calidad
        *           EL = Permiso Editar Lider
        *           ET = Permiso Editar Tester
        *           EC = Permiso Editar Cliente
        *           BS = Permiso Eliminar Soporte/Calidad
        *           BL = Permiso Eliminar Lider
        *           BT = Permiso Eliminar Tester
        *           BC = Permiso Eliminar Cliente
        * Modifica: Permisos generales para Requerimiento
        * Retorna:  La vista de permisos generales post para Requerimiento
        */
        [HttpPost]
        public ActionResult RequerimientoGeneral(string CS, string CL, string CT, string CC, string AS, string AL, string AT, string AC,
    string ES, string EL, string ET, string EC, string BS, string BL, string BT, string BC)
        {


            ////Cambia los permisos para los Jefe de Calidad / Soporte
            seguridad.setTablaSeguridadRequerimientosGeneral(0, new List<int>() { Array.IndexOf(tipo, CS) + 1, Array.IndexOf(tipo, AS) + 1, Array.IndexOf(tipo, ES) + 1, Array.IndexOf(tipo, BS) + 1 });

            ////Cambia los permisos para los Liders
            seguridad.setTablaSeguridadRequerimientosGeneral(1, new List<int>() { Array.IndexOf(tipo, CL) + 1, Array.IndexOf(tipo, AL) + 1, Array.IndexOf(tipo, EL) + 1, Array.IndexOf(tipo, BL) + 1 });

            ////Cambia los permisos para los Tester
            seguridad.setTablaSeguridadRequerimientosGeneral(2, new List<int>() { Array.IndexOf(tipo, CT) + 1, Array.IndexOf(tipo, AT) + 1, Array.IndexOf(tipo, ET) + 1, Array.IndexOf(tipo, BT) + 1 });

            ////Cambia los permisos para los Clientes
            seguridad.setTablaSeguridadRequerimientosGeneral(3, new List<int>() { Array.IndexOf(tipo, CC) + 1, Array.IndexOf(tipo, AC) + 1, Array.IndexOf(tipo, EC) + 1, Array.IndexOf(tipo, BC) + 1 });

            return RedirectToAction("RequerimientoGeneralPost");

        }

        /*
        * Efecto: Carga la vista de permisos generales para Requerimiento con un mensaje de confirmacion
        * Requiere: 
        * Modifica: Agrega permisos al ViewBag
        * Retorna:  La vista de permisos generales para Requerimiento
        */
        public ActionResult RequerimientoGeneralPost()
        {
            ViewBag.permisos = seguridad.getTablaSeguridadRequerimientosGeneral();
            ViewBag.saved = 1;
            return View("RequerimientoGeneral");
        }


        //--------------------------Empleado--------------------------//

        /*
        * Efecto: Carga la vista de permisos generales para Empleado
        * Requiere: 
        * Modifica: Agrega permisos al ViewBag
        * Retorna:  La vista de permisos generales para Empleado
        */
        public ActionResult EmpleadoGeneral()
        {
            ViewBag.permisos = seguridad.getTablaSeguridadEmpleadosGeneral();
            ViewBag.saved = 0;
            return View("EmpleadoGeneral");
        }

        /*
        * Efecto: Modificar permisos generales para Empleado
        * Requiere: CS = Permiso Consultar Soporte/Calidad
        *           CL = Permiso Consultar Lider
        *           CT = Permiso Consultar Tester
        *           CC = Permiso Consultar Cliente
        *           AS = Permiso Agregar Soporte/Calidad
        *           AL = Permiso Agregar Lider
        *           AT = Permiso Agregar Tester
        *           AC = Permiso Agregar Cliente
        *           ES = Permiso Editar Soporte/Calidad
        *           EL = Permiso Editar Lider
        *           ET = Permiso Editar Tester
        *           EC = Permiso Editar Cliente
        *           BS = Permiso Eliminar Soporte/Calidad
        *           BL = Permiso Eliminar Lider
        *           BT = Permiso Eliminar Tester
        *           BC = Permiso Eliminar Cliente
        * Modifica: Permisos generales para Empleado
        * Retorna:  La vista de permisos generales post para Empleado
        */
        [HttpPost]
        public ActionResult EmpleadoGeneral(string CS, string CL, string CT, string CC, string AS, string AL, string AT, string AC,
    string ES, string EL, string ET, string EC, string BS, string BL, string BT, string BC)
        {


            ////Cambia los permisos para los Jefe de Calidad / Soporte
            seguridad.setTablaSeguridadEmpleadosGeneral(0, new List<int>() { Array.IndexOf(tipo, CS) + 1, Array.IndexOf(tipo, AS) + 1, Array.IndexOf(tipo, ES) + 1, Array.IndexOf(tipo, BS) + 1 });

            ////Cambia los permisos para los Liders
            seguridad.setTablaSeguridadEmpleadosGeneral(1, new List<int>() { Array.IndexOf(tipo, CL) + 1, Array.IndexOf(tipo, AL) + 1, Array.IndexOf(tipo, EL) + 1, Array.IndexOf(tipo, BL) + 1 });

            ////Cambia los permisos para los Tester
            seguridad.setTablaSeguridadEmpleadosGeneral(2, new List<int>() { Array.IndexOf(tipo, CT) + 1, Array.IndexOf(tipo, AT) + 1, Array.IndexOf(tipo, ET) + 1, Array.IndexOf(tipo, BT) + 1 });

            ////Cambia los permisos para los Clientes
            seguridad.setTablaSeguridadEmpleadosGeneral(3, new List<int>() { Array.IndexOf(tipo, CC) + 1, Array.IndexOf(tipo, AC) + 1, Array.IndexOf(tipo, EC) + 1, Array.IndexOf(tipo, BC) + 1 });

            return RedirectToAction("EmpleadoGeneralPost");

        }

        /*
        * Efecto: Carga la vista de permisos generales para Empleado con un mensaje de confirmacion
        * Requiere: 
        * Modifica: Agrega permisos al ViewBag
        * Retorna:  La vista de permisos generales para Empleado
        */
        public ActionResult EmpleadoGeneralPost()
        {
            ViewBag.permisos = seguridad.getTablaSeguridadEmpleadosGeneral();
            ViewBag.saved = 1;
            return View("EmpleadoGeneral");
        }


        //--------------------------Cliente--------------------------//

        /*
        * Efecto: Carga la vista de permisos generales para Cliente
        * Requiere: 
        * Modifica: Agrega permisos al ViewBag
        * Retorna:  La vista de permisos generales para Cliente
        */
        public ActionResult ClienteGeneral()
        {
            ViewBag.permisos = seguridad.getTablaSeguridadClientesGeneral();
            ViewBag.saved = 0;
            return View("ClienteGeneral");
        }

        /*
        * Efecto: Modificar permisos generales para Cliente
        * Requiere: CS = Permiso Consultar Soporte/Calidad
        *           CL = Permiso Consultar Lider
        *           CT = Permiso Consultar Tester
        *           CC = Permiso Consultar Cliente
        *           AS = Permiso Agregar Soporte/Calidad
        *           AL = Permiso Agregar Lider
        *           AT = Permiso Agregar Tester
        *           AC = Permiso Agregar Cliente
        *           ES = Permiso Editar Soporte/Calidad
        *           EL = Permiso Editar Lider
        *           ET = Permiso Editar Tester
        *           EC = Permiso Editar Cliente
        *           BS = Permiso Eliminar Soporte/Calidad
        *           BL = Permiso Eliminar Lider
        *           BT = Permiso Eliminar Tester
        *           BC = Permiso Eliminar Cliente
        * Modifica: Permisos generales para Cliente
        * Retorna:  La vista de permisos generales post para Cliente
        */
        [HttpPost]
        public ActionResult ClienteGeneral(string CS, string CL, string CT, string CC, string AS, string AL, string AT, string AC,
    string ES, string EL, string ET, string EC, string BS, string BL, string BT, string BC)
        {


            ////Cambia los permisos para los Jefe de Calidad / Soporte
            seguridad.setTablaSeguridadClientesGeneral(0, new List<int>() { Array.IndexOf(tipo, CS) + 1, Array.IndexOf(tipo, AS) + 1, Array.IndexOf(tipo, ES) + 1, Array.IndexOf(tipo, BS) + 1 });

            ////Cambia los permisos para los Liders
            seguridad.setTablaSeguridadClientesGeneral(1, new List<int>() { Array.IndexOf(tipo, CL) + 1, Array.IndexOf(tipo, AL) + 1, Array.IndexOf(tipo, EL) + 1, Array.IndexOf(tipo, BL) + 1 });

            ////Cambia los permisos para los Tester
            seguridad.setTablaSeguridadClientesGeneral(2, new List<int>() { Array.IndexOf(tipo, CT) + 1, Array.IndexOf(tipo, AT) + 1, Array.IndexOf(tipo, ET) + 1, Array.IndexOf(tipo, BT) + 1 });

            ////Cambia los permisos para los Clientes
            seguridad.setTablaSeguridadClientesGeneral(3, new List<int>() { Array.IndexOf(tipo, CC) + 1, Array.IndexOf(tipo, AC) + 1, Array.IndexOf(tipo, EC) + 1, Array.IndexOf(tipo, BC) + 1 });

            return RedirectToAction("ClienteGeneralPost");

        }

        /*
        * Efecto: Carga la vista de permisos generales para Cliente con un mensaje de confirmacion
        * Requiere: 
        * Modifica: Agrega permisos al ViewBag
        * Retorna:  La vista de permisos generales para Cliente
        */
        public ActionResult ClienteGeneralPost()
        {
            ViewBag.permisos = seguridad.getTablaSeguridadClientesGeneral();
            ViewBag.saved = 1;
            return View("ClienteGeneral");
        }

        //--------------------------Prueba--------------------------//


        /*
        * Efecto: Carga la vista de permisos generales para Prueba
        * Requiere: 
        * Modifica: Agrega permisos al ViewBag
        * Retorna:  La vista de permisos generales para Prueba
        */
        public ActionResult PruebaGeneral()
        {
            ViewBag.permisos = seguridad.getTablaSeguridadPruebasGeneral();
            ViewBag.saved = 0;
            return View("PruebaGeneral");
        }


        /*
        * Efecto: Modificar permisos generales para Prueba
        * Requiere: CS = Permiso Consultar Soporte/Calidad
        *           CL = Permiso Consultar Lider
        *           CT = Permiso Consultar Tester
        *           CC = Permiso Consultar Cliente
        *           AS = Permiso Agregar Soporte/Calidad
        *           AL = Permiso Agregar Lider
        *           AT = Permiso Agregar Tester
        *           AC = Permiso Agregar Cliente
        *           ES = Permiso Editar Soporte/Calidad
        *           EL = Permiso Editar Lider
        *           ET = Permiso Editar Tester
        *           EC = Permiso Editar Cliente
        *           BS = Permiso Eliminar Soporte/Calidad
        *           BL = Permiso Eliminar Lider
        *           BT = Permiso Eliminar Tester
        *           BC = Permiso Eliminar Cliente
        * Modifica: Permisos generales para Prueba
        * Retorna:  La vista de permisos generales post para Prueba
        */
        [HttpPost]
        public ActionResult PruebaGeneral(string CS, string CL, string CT, string CC, string AS, string AL, string AT, string AC,
    string ES, string EL, string ET, string EC, string BS, string BL, string BT, string BC)
        {


            ////Cambia los permisos para los Jefe de Calidad / Soporte
            seguridad.setTablaSeguridadPruebasGeneral(0, new List<int>() { Array.IndexOf(tipo, CS) + 1, Array.IndexOf(tipo, AS) + 1, Array.IndexOf(tipo, ES) + 1, Array.IndexOf(tipo, BS) + 1 });

            ////Cambia los permisos para los Liders
            seguridad.setTablaSeguridadPruebasGeneral(1, new List<int>() { Array.IndexOf(tipo, CL) + 1, Array.IndexOf(tipo, AL) + 1, Array.IndexOf(tipo, EL) + 1, Array.IndexOf(tipo, BL) + 1 });

            ////Cambia los permisos para los Tester
            seguridad.setTablaSeguridadPruebasGeneral(2, new List<int>() { Array.IndexOf(tipo, CT) + 1, Array.IndexOf(tipo, AT) + 1, Array.IndexOf(tipo, ET) + 1, Array.IndexOf(tipo, BT) + 1 });

            ////Cambia los permisos para los Clientes
            seguridad.setTablaSeguridadPruebasGeneral(3, new List<int>() { Array.IndexOf(tipo, CC) + 1, Array.IndexOf(tipo, AC) + 1, Array.IndexOf(tipo, EC) + 1, Array.IndexOf(tipo, BC) + 1 });

            return RedirectToAction("PruebaGeneralPost");

        }

        /*
        * Efecto: Carga la vista de permisos generales para Prueba con un mensaje de confirmacion
        * Requiere: 
        * Modifica: Agrega permisos al ViewBag
        * Retorna:  La vista de permisos generales para Prueba
        */
        public ActionResult PruebaGeneralPost()
        {
            ViewBag.permisos = seguridad.getTablaSeguridadPruebasGeneral();
            ViewBag.saved = 1;
            return View("PruebaGeneral");
        }




    }
}