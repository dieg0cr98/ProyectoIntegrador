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


        private SeguridadController seguridad = new SeguridadController();
        //Metodo Get para obtener la vista
        //Devuelve la vista
        public ActionResult Index()
        {




            var rol = seguridad.GetRoleUsuario(User);
            //Verifica que sea un jefe de calidad o soporte
            if (rol == 0)
            {
                ViewBag.permiso = true;
                ViewBag.proyecto = seguridad.getTablaSeguridadProyectoGeneral();
                ViewBag.req = seguridad.tablaSeguridadRequerimientosGeneral;
                ViewBag.empleado = seguridad.tablaSeguridadEmpleados;
                ViewBag.cliente = seguridad.tablaSeguridadClientes;
                ViewBag.equipo = seguridad.tablaSeguridadEquipoGeneral;
                ViewBag.prueba = seguridad.tablaSeguridadRequerimientosGeneral;
                return View();
            }
            else
            {
                ViewBag.permiso = false;
                return View();
            }


            





        }

        public ActionResult ProyectoGeneral()
        {
            ViewBag.permisos = seguridad.getTablaSeguridadProyectoGeneral();
            ViewBag.saved = 0;
            return View("ProyectoGeneral");
        }


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


        public ActionResult ProyectoGeneralPost()
        {
            ViewBag.permisos = seguridad.getTablaSeguridadProyectoGeneral();
            ViewBag.saved = 1;
            return View("ProyectoGeneral");
        }


        private string[] tipo = { "Total", "Parcial", "Ninguno" };
    }
}