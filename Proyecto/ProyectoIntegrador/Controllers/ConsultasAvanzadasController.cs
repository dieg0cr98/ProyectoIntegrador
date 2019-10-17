using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProyectoIntegrador.BaseDatos;
using ASPNET_MVC_Samples.Models;

namespace ProyectoIntegrador.Controllers
{
    public class ConsultasAvanzadasController : Controller
    {


        private SeguridadController seguridad = new SeguridadController();

        private Gr03Proy2Entities5 db = new Gr03Proy2Entities5();

        // GET: ConsultasAvanzadas
        public ActionResult Index()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            List<DataPoint> dataPoints2 = new List<DataPoint>();

            int proyectosTotal = db.Proyecto.Count();
            ViewBag.total = proyectosTotal;
            int proyectosPreparación = db.Proyecto.Where(p => p.estado == "Preparación").Count();
            int proyectosActivo = db.Proyecto.Where(p => p.estado == "Activo").Count();
            int proyectosCancelado = db.Proyecto.Where(p => p.estado == "Cancelado").Count();
            int proyectosTerminado = db.Proyecto.Where(p => p.estado == "Terminado").Count();



            dataPoints2.Add(new DataPoint("Preparación", ((proyectosPreparación * 100) / proyectosTotal)));
            dataPoints2.Add(new DataPoint("Activo", ((proyectosActivo * 100) / proyectosTotal)));
            dataPoints2.Add(new DataPoint("Cancelado", ((proyectosCancelado * 100) / proyectosTotal)));
            dataPoints2.Add(new DataPoint("Terminado", ((proyectosTerminado * 100) / proyectosTotal)));

            dataPoints.Add(new DataPoint("Preparación", proyectosPreparación));
            dataPoints.Add(new DataPoint("Activo", proyectosActivo ));
            dataPoints.Add(new DataPoint("Cancelado", proyectosCancelado));
            dataPoints.Add(new DataPoint("Terminado", proyectosTerminado ));

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);

            return View();
        }
    }
}