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
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual ObjectResult<testersActivos_Result> testersActivos()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<testersActivos_Result>("testersActivos");
        }
    
        public virtual ObjectResult<USP_GetEquipo_Result> USP_GetEquipo(Nullable<int> id_proyecto)
        {
            var id_proyectoParameter = id_proyecto.HasValue ?
                new ObjectParameter("id_proyecto", id_proyecto) :
                new ObjectParameter("id_proyecto", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_GetEquipo_Result>("USP_GetEquipo", id_proyectoParameter);
        }
    
        public virtual ObjectResult<USP_GetLideresDisponibles_Result> USP_GetLideresDisponibles()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_GetLideresDisponibles_Result>("USP_GetLideresDisponibles");
        }
    
        public virtual ObjectResult<USP_GetTestersDisponibles_Result> USP_GetTestersDisponibles()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_GetTestersDisponibles_Result>("USP_GetTestersDisponibles");
        }
    
        public virtual ObjectResult<string> HabilidadesEmpleado(string id)
        {
            var idParameter = id != null ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("HabilidadesEmpleado", idParameter);
        }
    
        public virtual ObjectResult<TestersAsignables_Result> TestersAsignables(Nullable<int> idProyecto)
        {
            var idProyectoParameter = idProyecto.HasValue ?
                new ObjectParameter("idProyecto", idProyecto) :
                new ObjectParameter("idProyecto", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<TestersAsignables_Result>("TestersAsignables", idProyectoParameter);
        }
    
        public virtual int USP_editEmployeeId(string cedulaVieja, string cedulaNueva)
        {
            var cedulaViejaParameter = cedulaVieja != null ?
                new ObjectParameter("cedulaVieja", cedulaVieja) :
                new ObjectParameter("cedulaVieja", typeof(string));
    
            var cedulaNuevaParameter = cedulaNueva != null ?
                new ObjectParameter("cedulaNueva", cedulaNueva) :
                new ObjectParameter("cedulaNueva", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("USP_editEmployeeId", cedulaViejaParameter, cedulaNuevaParameter);
        }
    
        public virtual int USP_EquipoCheckLider(Nullable<int> idProyecto, ObjectParameter liderFlag)
        {
            var idProyectoParameter = idProyecto.HasValue ?
                new ObjectParameter("idProyecto", idProyecto) :
                new ObjectParameter("idProyecto", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("USP_EquipoCheckLider", idProyectoParameter, liderFlag);
        }
    
        public virtual ObjectResult<USP_TestersDisponibleAsignado_Result> USP_TestersDisponibleAsignado()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<USP_TestersDisponibleAsignado_Result>("USP_TestersDisponibleAsignado");
        }
    }
}
