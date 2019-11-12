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
    /*
     * La clase TrabajaEnController actua como el controlador del módulo equipo. Esto implica que posee la lógica 
     * requerida para su correcto funcionamiento y el manejo de las interacciones con la vista.
     */
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

        //-------------------------Redireccionamiento de vistas--------------------------//
        /*
         * Efecto: Mustra la vista principal del equipo asociado a un proyecto.
         * Requiere: Un id de proyecto válido.
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
                var permisosGenerales = seguridad.EquipoConsultar(User);

                //Verifica usuario registrado
                if (permisosGenerales.Item1 >= 0)
                {
                    int id = idProyecto ?? default(int);
                    GetDatosVistaEquipo(id);
                    var trabajaEn = db.TrabajaEn.Where(t => t.idProyectoFK == idProyecto);
                    return View(trabajaEn.ToList());
                }
                else
                {
                    return View();
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Naughty");
                }
            }
        }

        //---------------------------------------------------------------------------//
        //-----------------------------Rutinas del controlador-----------------------//

        /*
         * Efecto: Agrega un integrante nuevo al equipo
         * Requiere: un id de proyecto, id de empleado y rol válidos.
         * Modifica: El empleado que se integra al equipo y la tabla TrabajaEn.
         * Retorna: Devuelve un JSON con el flag de éxito y el mensaje por desplegar en la vista.
         */
        public JsonResult AgregarIntegrante(int idProyecto, string idEmpleado, string rolEmpleado)
        {
            //System.Diagnostics.Debug.WriteLine(idProyecto + " " + idEmpleado + " " + rolEmpleado);
            Empleado empleado = db.Empleado.Find(idEmpleado);
            dynamic ret;
            switch (empleado.tipoTrabajo)
            {
                case "Lider":
                    {
                        //Declara el parámetro de retorno para el procedimiento de la base y lo castea.
                        ObjectParameter output = new ObjectParameter("liderFlag", typeof(Int32));
                        db.USP_EquipoCheckLider(idProyecto, output);
                        int numLider = (int)output.Value;

                        //Equipo sin lider, puede ser añadido
                        if (numLider == 0)  
                        {
                            empleado.estado = "Ocupado";

                            //Revisa si el empleado no existe dentro del proyecto de manera inactiva
                            TrabajaEn checkInactivo = db.TrabajaEn.Find(idProyecto, idEmpleado);
                            if (checkInactivo == null)
                            {
                                TrabajaEn trabaja = new TrabajaEn();
                                trabaja.idProyectoFK = idProyecto;
                                trabaja.idEmpleadoFK = empleado.idEmpleadoPK;
                                trabaja.rol = rolEmpleado;
                                trabaja.estado = "Activo";
                                db.Entry(empleado).State = EntityState.Modified;
                                db.TrabajaEn.Add(trabaja);
          
                            }
                            //Ya fue parte del equipo, lo marca como activo en vez de insertar.
                            else
                            {
                                checkInactivo.estado = "Activo";
                            }


                            db.SaveChanges();
                            ret = new
                            {
                                flag = 1,
                                msg = "Se ha agregado el líder exitosamente al equipo."
                            };
                        }
                        // El equipo ya tiene lider, no puede ser agregado otro.
                        else
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
                        //Declara el parámetro de retorno para el procedimiento de la base y lo castea.
                        ObjectParameter output = new ObjectParameter("testers", typeof(Int32));
                        db.USP_EquipoCheckTesters(idProyecto, output);
                        int numTesters = (int)output.Value;
                        System.Diagnostics.Debug.WriteLine("CANTIDAD TESTERS: " + numTesters);

                        //Hay menos testers que el máximo, puede ser agregado.
                        if (numTesters < 5)
                        {
                            empleado.estado = "Ocupado";

                            //Revisa si el empleado no existe dentro del proyecto de manera inactiva
                            TrabajaEn checkInactivo = db.TrabajaEn.Find(idProyecto, idEmpleado);
                            if (checkInactivo == null)
                            {
                                TrabajaEn trabaja = new TrabajaEn();
                                trabaja.idProyectoFK = idProyecto;
                                trabaja.idEmpleadoFK = empleado.idEmpleadoPK;
                                trabaja.rol = rolEmpleado;
                                trabaja.estado = "Activo";
                                db.Entry(empleado).State = EntityState.Modified;
                                db.TrabajaEn.Add(trabaja);
                            }
                            //Ya fue parte del equipo, lo marca como activo en vez de insertar.
                            else
                            {
                                checkInactivo.estado = "Activo";
                            }
                           

                            db.SaveChanges();
                            ret = new
                            {
                                flag = 1,
                                msg = "Se ha agregado el tester exitosamente al equipo."
                            };
                        }
                        //Ya no se pueden agregar mas testers al equipo
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

        /*
         * Efecto: Elimina a un integrante del equipo asociado a un proyecto específico.
         * Requiere: un id de proyecto, id de empleado y rol válidos.
         * Modifica: El empleado que se integra al equipo y la tabla TrabajaEn.
         * Retorna: Devuelve un JSON con el flag de éxito y el mensaje por desplegar en la vista.
         */
        public JsonResult QuitarIntegrante(int idProyecto, string idEmpleado, string rolEmpleado)
        {

            Empleado empleado = db.Empleado.Find(idEmpleado);
            Proyecto proyecto = db.Proyecto.Find(idProyecto);
            TrabajaEn trabaja = db.TrabajaEn.Find(idProyecto, idEmpleado);
            dynamic ret;


            //Si el proyecto está activo guarda el empleado para cuestión de historial pero lo pone inactivo.
            if (proyecto.estado == "Activo")
            {
                //Es lider
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
                    //Declara la variable de retorno para el procedimiento de la base y lo castea.
                    ObjectParameter output = new ObjectParameter("reqs", typeof(Int32));
                    db.USP_ContarRequerimientosTester(idEmpleado, output);
                    int reqs = (int)output.Value;

                    //No tiene requerimientos asignados, puede ser eliminado
                    if (reqs == 0) 
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
                //Es lider, se elimina de proyecto no activo
                if (empleado.tipoTrabajo == "Lider") 
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
                //Es tester, se revisan los requerimientos.
                else
                {
                    ObjectParameter output = new ObjectParameter("reqs", typeof(Int32));
                    db.USP_ContarRequerimientosTester(idEmpleado, output);
                    int reqs = (int)output.Value;

                    //No tiene requerimientos asignados, puede ser eliminado
                    if (reqs == 0) 
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
                    //Tiene requerimientos, no puede ser eliminado.
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

        /**
         * Efecto: Filtra los empleados de la vista de acuerdo a sus habilidades
         * Requiere: Una string de texto para usar como filtro
         * Modifica: N/A
         */
        public JsonResult filtrarEmpleadosVista(string filtro)
        {
            var ids = db.USP_GetTestersPorHabilidades(filtro);
            System.Diagnostics.Debug.WriteLine(ids.ToString());
            return Json(ids, JsonRequestBehavior.AllowGet);
        }

    }
}
