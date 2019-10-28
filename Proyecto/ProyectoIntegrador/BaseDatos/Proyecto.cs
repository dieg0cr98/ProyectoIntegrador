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
    
    public partial class Proyecto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Proyecto()
        {
            this.Requerimiento = new HashSet<Requerimiento>();
            this.TrabajaEn = new HashSet<TrabajaEn>();
        }
    
        public int idProyectoAID { get; set; }
        public string nombre { get; set; }
        public string objetivo { get; set; }
        public string estado { get; set; }
        public int duracionReal { get; set; }
        public int duracionEstimada { get; set; }
        public System.DateTime fechaInicio { get; set; }
        public System.DateTime fechaFinalizacion { get; set; }
        public string cedulaClienteFK { get; set; }
        public int cantidadReq { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requerimiento> Requerimiento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TrabajaEn> TrabajaEn { get; set; }
    }
}
