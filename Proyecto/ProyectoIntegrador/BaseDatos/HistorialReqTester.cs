//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoIntegrador.BaseDatos
{
    using System;
    using System.Collections.Generic;
    
    public partial class HistorialReqTester
    {
        public int idReqFK { get; set; }
        public int idProyectoFK { get; set; }
        public string idEmpleadoFK { get; set; }
        public int horas { get; set; }
        public Nullable<System.DateTime> fechaInicio { get; set; }
        public Nullable<System.DateTime> fechaFin { get; set; }
        public string activo { get; set; }
    
        public virtual Requerimiento Requerimiento { get; set; }
        public virtual Tester Tester { get; set; }
    }
}