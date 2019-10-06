using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIntegrador.Models
{
    public class OrderOperatorViewModel
    {
        public BaseDatos.Proyecto proyecto { get; set; }
        public BaseDatos.Empleado empleado { get; set; }
    }
}