using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.BaseDatos;
using System.Data.Entity.Core.Objects;

namespace ProyectoIntegrador.Controllers
{
    public class TrabajaEnController : Controller
    {
        private Gr03Proy2Entities6 db = new Gr03Proy2Entities6();
        private SeguridadController seguridad = new SeguridadController();

        /*
         * Efecto: Obtiene todos los valores requeridos en la vista de equipo y los añade al viewbag.
         * Requiere: Un id de proyecto valido
         * Modifica: Agrega variables al ViewBag
         */
        private void GetDatosVistaEquipo(int idProyecto)
        {
            //Encuentra el proyecto asociado al id
            ViewBag.proyectoActual = db.Proyecto.Find(idProyecto);
            //Consigue los permisos del usuario actual.
            ViewBag.permisosActuales = seguridad.EquipoConsultar(User);

            //Selecciona todos los empelados que esten disponible y que sean tester
            ViewBag.testers = db.USP_GetTestersDisponibles().ToList();
            
            //Selecciona todos los empelados que esten disponible y que sean Lider
            ViewBag.lideres = db.USP_GetLideresDisponibles().ToList();

            //Procedimiento almacenado para encontrar integrantes del equipo asociados al proyecto
            ViewBag.equipoActual = db.USP_GetEquipo(idProyecto);
        }

        //-------------------------ActionResults--------------------------//
        /*
         * Efecto: Mustra la vista principal del equipo.
         * Requiere: --
         * Modifica: --
         */
        public ActionResult Index(int? idProyecto)
        {
            if (idProyecto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                int id = idProyecto ?? default(int);
                GetDatosVistaEquipo(id);
                var trabajaEn = db.TrabajaEn.Where(t => t.idProyectoFK == idProyecto);
                return View(trabajaEn.ToList());
            }
        }

        //---------------------------------------------------------------------------//
        //-----------------------------Rutinas del controlador-----------------------//

        /*
         * 
         */
        public List<Empleado> GetEmpleados()
        {
            return db.Empleado.ToList();
        }


        /*
         * Efecto: Agrega un integrante nuevo al equipo
         * Requiere: Parámetros válidos que no hagan conflicto con la base de datos
         * Modifica: El empleado que se integra al equipo y la tabla TrabajaEn
         */
        //deberia retornar json result para verificar
        //[HttpGet]
        public JsonResult AgregarIntegrante(int idProyecto, string idEmpleado, string rolEmpleado)
        {

            //System.Diagnostics.Debug.WriteLine(idProyecto + " " + idEmpleado + " " + rolEmpleado);
            Empleado empleado = db.Empleado.Find(idEmpleado);
            //empleado.estado = "Ocupado";
            dynamic ret;
            switch (empleado.tipoTrabajo)
            {
                case "Lider":
                    {
                        ObjectParameter output = new ObjectParameter("liderFlag", typeof(Int32));
                        db.USP_EquipoCheckLider(idProyecto, output);
                        int numLider = (int)output.Value;
                        System.Diagnostics.Debug.WriteLine("TIENE LIDER: "  + numLider);
                        if (numLider == 0) //Equipo sin lider, puede ser añadido
                        {
                            empleado.estado = "Ocupado";
                            TrabajaEn trabaja = new TrabajaEn();
                            trabaja.idProyectoFK = idProyecto;
                            trabaja.idEmpleadoFK = empleado.idEmpleadoPK;
                            trabaja.rol = rolEmpleado;
                            trabaja.estado = "Activo";
                            db.Entry(empleado).State = EntityState.Modified;
                            db.TrabajaEn.Add(trabaja);
                            db.SaveChanges();
                            ret = new
                            {
                               flag = 1, 
                               msg = "Se ha agregado el líder exitosamente al equipo."
                            };
                        }  
                        else //Ya tiene lider
                        {
                            ret = new
                            {
                                flag = -1,
                                msg = "No puede haber más de un líder en el equipo."
                            };
                            return Json(ret, JsonRequestBehavior.AllowGet);
                        }
                      break;
                    }
                    
                case "Tester":
                    {
                        ObjectParameter output = new ObjectParameter("testers", typeof(Int32));
                        db.USP_EquipoCheckTesters(idProyecto, output);
                        int numTesters = (int) output.Value;
                        System.Diagnostics.Debug.WriteLine("CANTIDAD TESTERS: " + numTesters);
                        if (numTesters < 5)
                        {
                            empleado.estado = "Ocupado";
                            TrabajaEn trabaja = new TrabajaEn();
                            trabaja.idProyectoFK = idProyecto;
                            trabaja.idEmpleadoFK = empleado.idEmpleadoPK;
                            trabaja.rol = rolEmpleado;
                            trabaja.estado = "Activo";
                            db.Entry(empleado).State = EntityState.Modified;
                            db.TrabajaEn.Add(trabaja);
                            db.SaveChanges();
                            ret = new
                            {
                                flag = 1,
                                msg = "Se ha agregado el tester exitosamente al equipo."
                            };
                        } 
                        else
                        {
                            ret = new
                            {
                                flag = -1,
                                msg = "No se pueden agregar más testers al equipo."
                            };
                        }
                    }
                    break;
                default:
                    ret = new
                    {
                        flag = -1,
                        msg = "No se ha podido agregar el integrante al equipo."
                    };
                    break;
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }


        public JsonResult QuitarIntegrante(int idProyecto, string idEmpleado, string rolEmpleado)
        {
            
            Empleado empleado = db.Empleado.Find(idEmpleado);
            Proyecto proyecto = db.Proyecto.Find(idProyecto);
            TrabajaEn trabaja = db.TrabajaEn.Find(idProyecto, idEmpleado);
            dynamic ret;


            //Si el proyecto está activo guarda el empleado para cuestión de historial pero lo pone inactivo.
            if (proyecto.estado == "Activo")
            {
                //Si el proyecto esta activo.
                if (empleado.tipoTrabajo == "Lider")
                {
                    ret = new
                    {
                        flag = -1,
                        msg = "No se puede sacar al líder de un proyecto activo."
                    };
                }
                //Es un tester, revisar reqs
                else
                {
                    ObjectParameter output = new ObjectParameter("reqs", typeof(Int32));
                    db.USP_ContarRequerimientosTester(idEmpleado, output);
                    int reqs = (int)output.Value;

                    if (reqs == 0) //No tiene requerimientos asignados, puede ser eliminado
                    {
                        empleado.estado = "Disponible";
                        db.Entry(empleado).State = EntityState.Modified;

                        trabaja.estado = "Inactivo";
                        db.Entry(trabaja).State = EntityState.Modified;

                        ret = new
                        {
                            flag = 1,
                            msg = "Se ha sacado exitosamente al tester del equipo."
                        };
                        db.SaveChanges();
                    }
                    else //Tiene reqs
                    {
                        ret = new
                        {
                            flag = -1,
                            msg = "No se puede sacar a un empleado con requerimientos activos asignados."
                        };
                    }
                }
         
            }
            //Proyecto no activo, lo borra del equipo.
            else
            {
                
                if (empleado.tipoTrabajo == "Lider") //Es lider, se elimina de proyecto no activo
                {
                    db.TrabajaEn.Remove(trabaja);
                    empleado.estado = "Disponible";
                    db.Entry(empleado).State = EntityState.Modified;
                    ret = new
                    {
                        flag = 1,
                        msg = "Se ha sacado exitosamente al líder del equipo."
                    };
                    db.SaveChanges();
                }
                else //Es tester, se revisan los requerimientos.
                {
                    ObjectParameter output = new ObjectParameter("reqs", typeof(Int32));
                    db.USP_ContarRequerimientosTester(idEmpleado, output);
                    int reqs = (int)output.Value;

                     if (reqs == 0) //No tiene requerimientos asignados, puede ser eliminado
                     {
                        db.TrabajaEn.Remove(trabaja);
                        empleado.estado = "Disponible";
                        db.Entry(empleado).State = EntityState.Modified;
                        ret = new
                        {
                            flag = 1,
                            msg = "Se ha sacado exitosamente al tester del equipo."
                        };
                        db.SaveChanges();
                    }
                    else
                    {
                        ret = new
                        {
                            flag = -1,
                            msg = "No se puede sacar a un empleado con requerimientos activos asignados."
                        };
                    }
                }
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
    }
}
