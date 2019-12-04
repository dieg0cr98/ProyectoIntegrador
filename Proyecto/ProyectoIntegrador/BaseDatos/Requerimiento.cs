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
    using System.ComponentModel.DataAnnotations;

    public partial class Requerimiento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Requerimiento()
        {
            this.HistorialReqTester = new HashSet<HistorialReqTester>();
            this.Prueba = new HashSet<Prueba>();
        }
        
        [Key]
        public int idReqPK { get; set; }
        [Key]
        public int idProyectoFK { get; set; }
        public string cedulaTesterFK { get; set; }
        public string nombre { get; set; }
        public string complejidad { get; set; } = "Medio";
        public int tiempoEstimado { get; set; } = 0;
        public Nullable<int> tiempoReal { get; set; }
        public string descripcion { get; set; }
        public System.DateTime fechaDeInicio { get; set; }
        public Nullable<System.DateTime> fechaDeFinalizacion { get; set; } = DateTime.Now;
        public string estado { get; set; } = "Preparacion";
        public Nullable<bool> resultado { get; set; }
        public string detallesResultado { get; set; }
        public string estadoResultado { get; set; }
        public int cantidadDePruebas { get; set; }
    
        public virtual Empleado Empleado { get; set; }
        public virtual Proyecto Proyecto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistorialReqTester> HistorialReqTester { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prueba> Prueba { get; set; }
    }
}
