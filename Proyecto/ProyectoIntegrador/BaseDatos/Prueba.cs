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
    
    public partial class Prueba
    {
        public int idProyectoFK { get; set; }
        public int idReqFK { get; set; }
        public int idPruebaPK { get; set; }
        public string resultadoFinal { get; set; }
        public string propositoPrueba { get; set; }
        public string entradaDatos { get; set; }
        public string resultadoEsperado { get; set; }
        public string flujoPrueba { get; set; }
        public string estado { get; set; }
        public string descripcionError { get; set; }
    
        public virtual Requerimiento Requerimiento { get; set; }
    }
}
