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
    
    public partial class HabilidadBlanda
    {
        public string idEmpleadoFK { get; set; }
        public string habilidad { get; set; }
    
        public virtual Empleado Empleado { get; set; }
    }
}
