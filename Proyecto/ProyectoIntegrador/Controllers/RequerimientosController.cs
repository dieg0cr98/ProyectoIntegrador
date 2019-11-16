using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.BaseDatos;
using System.Data.SqlClient;
using System.Configuration;

namespace ProyectoIntegrador.Controllers
{
    // Esta clase será la encargada de administrar los requerimientos de los proyectos y todo lo asociado a ellos. Desde aquí y un par de triggers en la base de datos
    // también, se administran algunos atributos de la tabla tester y todas las tuplas de la tabla HistorialReqTester.
    public class RequerimientosController : Controller
    {
        private Gr03Proy2Entities6 db = new Gr03Proy2Entities6();
        private SeguridadController seguridad = new SeguridadController();
        private ProyectosController proyectos = new ProyectosController();
        private EmpleadosController empleados = new EmpleadosController();




        //Metodo para recuperar los requerimientos en los que participa un usuario.
        //Recibe int idProyecto (identificador del proyecto)        
        //       int rol (El usuario tiene que tener un rol asignado antes de llamar a este metodo)
        //              rol = 0 Soporte/Calidad
        //              rol = 1 Lider
        //              rol = 2 Tester
        //              rol = 3 Cliente
        //       string idUsuario (Es la cedula o identificador de un usuario) 
        //Devuelve una lista IEnumerable con los requerimientos. Null en caso de que no puede ver ninguno
        public IEnumerable<ProyectoIntegrador.BaseDatos.Requerimiento> GetRequerimientosUsuario(int idProyecto,int rol, string idUsuario)
        {

            if (rol < 0)//Si no tiene permisos
            {
                return null;
            }
            else
            {
                //Pueden ver todos solo los requerimientos en los que participa
                if (rol == 2)
                {
                    return db.Requerimiento.Where(r => r.idProyectoFK == idProyecto /*&& r.estado != "Cancelado"*/ && r.cedulaTesterFK == idUsuario).ToList();
                }
                else //Pueden ver todos
                {
                    return db.Requerimiento.Where(r => r.idProyectoFK == idProyecto /*&& r.estado != "Cancelado"*/).ToList();
                }
            }
        }

        // Efecto: Retorna los datos de todos los testers que se encuentran asociados a requerimientos en el proyecto, junto con sus cédulas, para poder mostrar sus
        // nombres a la hora de consultar los requerimientos.
        // Requiere: int idProyecto -> Id del proyecto que posee los requerimientos a los que están asociados los testers que buscamos.
        // Modifica: El viewbag, genera 2 listas conocidas como nombres y cedulas, que poseen los nombres y las cédulas de los testers asociados a los requerimientos. 
        public void GetDatosTester(int idProyecto)
        {
            List<string> nombres = new List<string>();
            List<string> cedulas = new List<string>();
            string nombre;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    da.SelectCommand = new SqlCommand("nombreTester", conn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@idProyecto", idProyecto);

                    DataSet ds = new DataSet();
                    da.Fill(ds, "Empleados");

                    DataTable dt = ds.Tables["Empleados"];

                    foreach (DataRow row in dt.Rows)
                    {
                        nombre = row.Field<string>("nombre") + " " + row.Field<string>("apellido1");
                        nombres.Add(nombre);
                        cedulas.Add(row.Field<string>("idEmpleadoPK"));
                    }
                }
            }
            ViewBag.nombreTesters = nombres;
            ViewBag.cedulaTesters = cedulas;
        }

        // Efecto: Método que depliega la consulta sobre los requerimientos del proyecto cuyo id llega como parámetro.
        // Requiere: int idProyecto -> Identificador del proyecto al que están asociados los requerimientos que deseamos consultar.
        //           int idRequerimiento -> Utilizado en caso de volver de una vista como agregar o modificar, logra abrir el requerimiento recien creado o modificado.
        // Modifica: La vista que el usuario mira en ese momento, muestra la vista de consultar requerimientos.
        public ActionResult Index(int idProyecto, int idRequerimiento)
        {

            //Se buscan los permisos del usuario que hizo la consulta
            ViewBag.permisosGenerales = seguridad.RequerimientosConsultar(User);

            //Se obtienen los datos de todos los requerimientos asociados al proyecto.
            var requerimiento = GetRequerimientosUsuario(idProyecto, seguridad.GetRoleUsuario(User), seguridad.IdUsuario(User)).Reverse();

            //Obtiene los datos que se utilizarán para desplegar el nombre de los testers asociados a los requerimientos
            GetDatosTester(idProyecto);

            //Se guarda la selección que se debe desplegar automáticamente a la hora de llamar la vista de consulta.
            ViewBag.seleccion = idRequerimiento;
            //Se guarda el id del proyecto asociado para ser usado al llamar el CRUD de cualquier requerimiento.
            ViewBag.idProyecto = idProyecto;
            //Obtiene el nombre del proyecto asociado para mostrarlo en la consulta.
            ViewBag.nombre = proyectos.GetNombreProyecto(idProyecto);

            return View(requerimiento.ToList());
        }

        // Efecto: Método que despliega los datos de la vista utilizada para crear un requerimiento.
        // Requiere: int idProyecto -> El identificadir del proyecto al que se va a asociar el requerimiento.
        // Modifica: La vista que está viendo el usuario. Se despliega el formulario para crear requerimientos.
        public ActionResult Create(int idProyecto)
        {

            //Se buscan los permisos del usuario que hizo la consulta
            ViewBag.permisosAgregar = seguridad.RequerimientosAgregar(User);
            
            //Se solicitan todos los testers asignables al requerimiento
            ViewBag.TestersDisponibles = db.TestersAsignables(idProyecto).ToList();
            ViewBag.idProyecto = idProyecto;

            return View();
        }


        // Efecto: Este método se encarga de recibir los datos del formulario con el que se crea un requerimiento. Primero verifica que sean válidos, de ser así
        // los envía a la BD y de no serlo, vuelve a la vista de crear con un error.
        // Requiere: string nombre -> Nombre del requerimiento.
        //           string complejidad -> Complejidad asignada al requerimiento.
        //           string descripción -> Breve descripción del requerimiento
        //           string estado -> Estado con el cual inicia el requerimiento.
        //           int? duracionEstimada -> Duración que se estima, durará el requerimiento en ser probado. 
        //           DateTime? fechai -> Fecha de inicio de pruebas del requerimiento.
        //           int idProyecto -> Parte de la llave primaria que identificará un nuevo requerimiento.
        //           string idTester -> Cédula que identifica al tester asociado a este requerimiento.
        // Modifica: Crea un requerimiento con los datos anteriormente mencionados y el id de requerimiento que se calcula por medio de un trigger.
        [HttpPost]
        public ActionResult Create(string nombre, string complejidad, string descripcion, string estado,
            int? duracionEstimada, DateTime? fechai, int idProyecto, string idTester)
        {
            //Crea la instancia de requerimiento que será agregada.
            Requerimiento requerimiento = new Requerimiento();
            requerimiento.nombre = nombre;
            requerimiento.complejidad = complejidad;
            requerimiento.idProyectoFK = idProyecto;
            requerimiento.idReqPK = 0;
            requerimiento.descripcion = descripcion;

            //Valída los datos que podrían ser nulos.
            if (estado != "")
            {
                requerimiento.estado = estado;
            }

            if (duracionEstimada != null)
            {
                requerimiento.tiempoEstimado = (int)duracionEstimada;
            }

            if (fechai != null)
            {
                requerimiento.fechaDeInicio = (DateTime)fechai;
            }

            if (idTester != "")
            {
                requerimiento.cedulaTesterFK = idTester;
            }

            //Lo agrega a la BD
            db.Requerimiento.Add(requerimiento);
            db.SaveChanges();

            var idReq = db.Requerimiento.Where(R => R.nombre == requerimiento.nombre).FirstOrDefault();

            //Vuelve a la vista de consultar.
            return RedirectToAction("Index", new { idProyecto, idRequerimiento = idReq.idReqPK });
        }

        // Efecto: Método que llama a la vista de modificar un requerimiento
        // Requiere:    int idRequerimiento -> Es parte de la llave primaria utilizada para encontrar el requerimiento que se desea editar.
        //              int idProyecto -> La otra parte de dicha llave primaria, con estos dos elementos se encuentra el elemento que el usuario desea editar.
        // Modifica: La vista actual del usuario, esta método le carga la vista de editar con los datos del requerimiento que este desea cambiar.
        public ActionResult Edit(int idRequerimiento, int idProyecto)
        {
            Requerimiento requerimiento = db.Requerimiento.Find(idRequerimiento, idProyecto);

            if (requerimiento == null)
            {
                return HttpNotFound();
            }

            //Se solicitan los permisos del usuario para analizar si puede editar o no
            ViewBag.permisosEditar = seguridad.RequerimientosEditar(User);

            //Se solicitan los testers disponibles para asignar, esto devuelve una lista con los nombres de estos.
            ViewBag.testersDisponibles = db.TestersAsignables(idProyecto).ToList();
            //Se pasa el id del proyecto por aparte junto con el id del requerimiento.
            ViewBag.idProyecto = idProyecto;
            ViewBag.idRequerimiento = idRequerimiento;
            //Se pasa la cedula del tester para poder identificarlo.
            ViewBag.testerAsociado = requerimiento.cedulaTesterFK;
            return View(requerimiento);
        }

        // Efecto: Método que recibe los cambios hechos a un requerimiento en el formulario de modificar y los valída antes de enviarlos a la BD.
        // Requiere: Todos los atributos de la entidad requerimiento. Aunque los únicos que no se editan, son el id del proyecto y del requerimiento.
        // Modifica: La instancia de requerimiento en la base de datos identificada por dichos id.
        [HttpPost]
        public ActionResult Edit(int idProyecto, int idRequerimiento, string nombre, string complejidad, string descripcion, string estado, int? duracionEstimada, 
            int? duracionReal, DateTime fechai, DateTime? fechaf, string resultado, string estadoR, string detalleResultado, string idTester)
        {
            Requerimiento requerimiento = db.Requerimiento.Find(idRequerimiento, idProyecto);
            requerimiento.nombre = nombre;
            requerimiento.complejidad = complejidad;
            requerimiento.descripcion = descripcion;
            requerimiento.fechaDeFinalizacion = fechaf;
            requerimiento.idProyectoFK = idProyecto;
            requerimiento.tiempoReal = duracionReal;
            requerimiento.detallesResultado = detalleResultado;

            if (estado != "")
            {
                requerimiento.estado = estado;
            }

            if (duracionEstimada != null)
            {
                requerimiento.tiempoEstimado = (int)duracionEstimada;
            }

            if (fechai != null)
            {
                requerimiento.fechaDeInicio = (DateTime)fechai;
            }

            if (idTester != "")
            {
                requerimiento.cedulaTesterFK = idTester;
            }
            else
            {
                requerimiento.cedulaTesterFK = null;
            }

            if (resultado != "")
            {
                if(resultado == "true")
                {
                    requerimiento.resultado = true;
                }
                else
                {
                    requerimiento.resultado = false;
                }
            }

            if (estadoR != "")
            {
                requerimiento.estadoResultado = estadoR;
            }

            db.Entry(requerimiento).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = requerimiento.idReqPK });
        }

        // Efecto: Método que recibe la confirmación del usuario para eliminar un requerimiento de la base de datos. Esto es, si ya se verificó que el usuario 
        // posee los permisos para eliminarlos y además, está cancelado ya.
        // Requiere:    int idRequerimiento -> Es parte de la llave primaria utilizada para encontrar el requerimiento que se desea eliminar.
        //              int idProyecto -> La otra parte de dicha llave primaria, con estos dos elementos se encuentra el elemento que el usuario desea eliminar y se elimina.
        // Modifica: Se modifica la BD, elminando el requerimiento que se encuentra por medio de los atributos pasados como parametros.
        public ActionResult Eliminar(int idRequerimiento, int idProyecto)
        {
            if (Request.IsAuthenticated)
            {
                Requerimiento requerimiento = db.Requerimiento.Find(idRequerimiento, idProyecto);
                db.Requerimiento.Remove(requerimiento);
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { idProyecto = idProyecto, idRequerimiento = 0 });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Método que se encarga de verificar que el nombre del requerimiento que voy a ingresar, sea único.
        // Recibe string name. Contiene el nombre que se quiere verificar
        //        string oldName. Contiene el nombre actual del reqeurimiento (Se utiliza en caso de editar un requerimiento)
        // Devuelve un JsonResult con un True si ya existe un requerimiento y un false si no
        public JsonResult ReviseNombreRequerimiento(string name, string oldName, int idProyecto)
        {
            //Hay que verificar si el nuevo nombre ya existe en la base de datos
            if ((name != oldName) && (db.Requerimiento.Where(r => r.nombre == name && r.idProyectoFK == idProyecto).FirstOrDefault() != null))
            {
                //Existe un proyecto con ese nombre
                return new JsonResult { Data = false };
            }
            else
                return new JsonResult { Data = true };

        }
    }
}
