﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Gr03Proy2Entities6 : DbContext
    {
        public Gr03Proy2Entities6()
            : base("name=Gr03Proy2Entities6")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Empleado> Empleado { get; set; }
        public virtual DbSet<HabilidadBlanda> HabilidadBlanda { get; set; }
        public virtual DbSet<HabilidadTecnica> HabilidadTecnica { get; set; }
        public virtual DbSet<Proyecto> Proyecto { get; set; }
        public virtual DbSet<Prueba> Prueba { get; set; }
        public virtual DbSet<Requerimiento> Requerimiento { get; set; }
        public virtual DbSet<Tester> Tester { get; set; }
        public virtual DbSet<TrabajaEn> TrabajaEn { get; set; }
        public virtual DbSet<HistorialReqTester> HistorialReqTester { get; set; }
    
        public virtual ObjectResult<CA_TestersAsignadosDisponibles_Result> CA_TestersAsignadosDisponibles()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CA_TestersAsignadosDisponibles_Result>("CA_TestersAsignadosDisponibles");
        }
    }
}
