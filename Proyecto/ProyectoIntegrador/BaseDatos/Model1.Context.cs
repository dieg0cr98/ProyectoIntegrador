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
    
        public virtual ObjectResult<CA_TestersAsignados_Y_Disponibles_Result> CA_TestersAsignados_Y_Disponibles()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CA_TestersAsignados_Y_Disponibles_Result>("CA_TestersAsignados_Y_Disponibles");
        }
    
        public virtual ObjectResult<numeroEmpleados_Result> numeroEmpleados(ObjectParameter numeroEmpleados)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<numeroEmpleados_Result>("numeroEmpleados", numeroEmpleados);
        }
    
        public virtual ObjectResult<testers_Result> testers()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<testers_Result>("testers");
        }
    
        public virtual ObjectResult<testersActivos_Result> testersActivos()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<testersActivos_Result>("testersActivos");
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual ObjectResult<USP_TestersDisponibleAsignado_Result> USP_TestersDisponibleAsignado()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_TestersDisponibleAsignado_Result>("USP_TestersDisponibleAsignado");
        }
    }
}
