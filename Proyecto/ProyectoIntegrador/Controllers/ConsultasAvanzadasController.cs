using ProyectoIntegrador.BaseDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoIntegrador.Controllers
{
    public class ConsultasAvanzadasController : Controller
    {
        private SeguridadController seguridad = new SeguridadController();
        private Gr03Proy2Entities6 db = new Gr03Proy2Entities6();

        // GET: ConsultasAvanzadas
        public ActionResult Index()
        {

            var t = db.CA_TestersAsignados_Y_Disponibles();
           var y = t.ToList().First().nombre;

            return View();
        }
    }
}