using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.BaseDatos;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ProyectoIntegrador.Models;
using System.Web.Security;


namespace ProyectoIntegrador.Controllers
{
    public class SeguridadController : Controller
    {

        private Gr03Proy2Entities6 db = new Gr03Proy2Entities6();

        /*Metodo que inserta el usario con un rol a las tabla de seguridad
         * Falta agregar algun tipo de control de errores
         * La clase es de tipo async porque los metodos son async. Los hilos ejecutan el metodo y no esperan a que este termine
         * El await significa que tiene que esperar a que este metodo termine para continuar
        */
        public async System.Threading.Tasks.Task AgregarUsuarioAsync(string correo, string rol, string contraseña = "Admin1$")
        {
            //Crea una varia de contexto Owin
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //Tomado de https://stackoverflow.com/questions/24001245/cant-get-usermanager-from-owincontext-in-apicontroller

            //Se crea un objeto RegisterViewModel el cual contiene los datos del correo y contraseña
            RegisterViewModel model = new RegisterViewModel
            {
                Email = correo,
                Password = contraseña //La contraseña por defecto es Admin1$
            };

            //Luego hay que insertar el usuario a las tablas
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };


            await manager.CreateAsync(user, model.Password);

            //Ahora hay que asignarle un rol al usuario, pero primero ocupa saber el id autogenerado por el sistema

            //Determina el id usando el correo del usuario
            var r = await manager.FindByNameAsync(correo);

            //Inserta el usuario a un rol.
            await manager.AddToRoleAsync(r.Id, rol);

        }

        /*Metodo que eliminar el usario de las tabla de seguridad
         * La clase es de tipo async porque los metodos son async. Los hilos ejecutan el metodo y no esperan a que este termine
         * El await significa que tiene que esperar a que este metodo termine para continuar
        */
        public async System.Threading.Tasks.Task DeleteUsuarioAsync(string correo)
        {
            //Crea una varia de contexto Owin
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //Tomado de https://stackoverflow.com/questions/24001245/cant-get-usermanager-from-owincontext-in-apicontroller

            var user = await manager.FindByNameAsync(correo);

            await manager.DeleteAsync(user);

        }


        /*Metodo para saber el rol del usuario logueado
        Recibe System.Security.Principal.IPrincipal user . Con los datos del usuario registrado actualmenten 
        Retorna un int. Posible valores:
            -1 El usuario no tiene rol o tiene uno distinto a los definidos
             0 Soporte y Calidad
             1 Lider
             2 Tester
             3 Cliente
      */
        public int GetRoleUsuario(System.Security.Principal.IPrincipal user)
        {


            int rol = -1;
            if (user.IsInRole("Lider"))
            {
                rol = 1;
            }
            else
            {
                if (user.IsInRole("Tester"))
                {
                    rol = 2;
                }
                else
                {
                    if (user.IsInRole("Cliente"))
                    {
                        rol = 3;
                    }
                    else
                    {
                        if (user.IsInRole("Soporte") || (user.IsInRole("Calidad")))
                        {
                            rol = 0;
                        }
                    }
                }
            }

            return rol;

        }


        //Metodo para recuperar el id del usuario registrado actualmente
        //Recibe como parametro un objeto tipo System.Security.Principal.IPrincipal con los datos del usuario logueado                      
        //Devuelve una string con el id del usuario. En caso de que no exista returna una string vacia
        public string IdUsuario(System.Security.Principal.IPrincipal user)
        {

            string idUsuario = "";
            int rol = GetRoleUsuario(user);

            if (rol >= 0) // Revisa si el usuario tiene un rol asignado
            {
                if (rol == 3)//Si es un cliente
                {

                    idUsuario = db.Cliente.Where(p => p.correo == user.Identity.Name).FirstOrDefault().cedulaPK;

                }
                else //Si no es cliente es un empleado
                {
                    idUsuario = db.Empleado.Where(p => p.correo == user.Identity.Name).FirstOrDefault().idEmpleadoPK;
                }

            }



            return idUsuario;
        }


        //----------------------------------------------------------------Tablas de seguridad------------------------------------------------//

        /* 1 Pueder hacer la accion para todos los proyectos(crud)
         * 2 Solo a los proyectos que pertenece
         * 3 No puede hacer la accion (crud)
        */
        private int[,] tablaSeguridadProyectoGeneral = new int[,] {
                       /*Soporte/Calidad , Lider , Tester , Cliente*/
         /*Consultar*/                   {1,2,2,2},
         /*Agregar*/                     {1,3,3,3},
         /*Editar*/                      {1,1,3,3},
         /*Eliminar*/                    {1,3,3,3}
         };


        /* 0 No puede editar el atributo
         * 1 Si puede editar el atributo
        */
        private int[,] tablaSeguridadProyectoEditar = new int[,] {
                                          /*Soporte/Calidad , Lider , Tester , Cliente*/
          /*idProyecto*/                                    {0,0,0,0},
          /*nombre*/                                        {1,0,0,0},
          /*objetivo*/                                      {1,0,0,0},
          /*Estado*/                                        {1,1,0,0},
          /*duracionEstimada*/                              {1,0,0,0},
          /*duracionReal*/                                  {1,0,0,0},
          /*FechaInicio*/                                   {1,0,0,0},
          /*FechaFinalizacion*/                             {1,1,0,0},
          /*Cantidad de requerimiento*/                     {0,0,0,0},
          /*cedulaCliente*/                                 {1,0,0,0},
          /*cedulaLider*/                                   {1,0,0,0}
          };


        /* 0 No puede editar el atributo
         * 1 Si puede editar el atributo
        */
        private int[,] tablaSeguridadProyectoAgregar = new int[,] {
                                          /*Soporte/Calidad , Lider , Tester , Cliente*/
          /*idProyecto*/                                    {0,0,0,0},
          /*nombre*/                                        {1,0,0,0},
          /*objetivo*/                                      {1,0,0,0},
          /*Estado*/                                        {0,0,0,0},
          /*duracionEstimada*/                              {1,0,0,0},
          /*duracionReal*/                                  {0,0,0,0},
          /*FechaInicio*/                                   {0,0,0,0},
          /*FechaFinalizacion*/                             {0,0,0,0},
          /*Cantidad de requerimiento*/                     {0,0,0,0},
          /*cedulaCliente*/                                 {1,0,0,0},
          /*cedulaLider*/                                   {1,0,0,0}
        };

        /*
       * Define los permisos para acceder los cruds de los equipos.
       * 1 = Todos los equipos.
       * 2 = Solo los que participa.
       * 3 = Ningún equipo.
       * Orden de usuarios: Soporte y Calidad, Lider, Tester, Cliente
       */
        private int[,] tablaSeguridadEquipoGeneral = new int[,] {
            {1,2,2,2}, //Consultar
            {1,2,3,3}, //Agregar
            {1,2,3,3}, //Editar
            {1,3,3,3} //Eliminar
        };

        /*
         * Define los permisos para agregar valores a los atributos de equipo.
         * 0 = No puede agregar al atributo
         * 1 = Puede agregar al atributo
         * Orden de usuarios: {Soporte y Calidad, Lider, Tester, Cliente}
         */
        private int[,] tablaSeguridadEquipoAgregar = new int[,] {
            {0,0,0,0}, //idProyectoFK
            {0,0,0,0}, //idEmpleadoFK
            {1,0,0,0}, //rol
            {1,1,0,0} //estado
        };

        /*
         * Define los permisos para editar  los valores de los atributos de equipo.
         * 0 = No puede editar el atributo
         * 1 = Puede editar el atributo
         * Orden de usuarios: {Soporte y Calidad, Lider, Tester, Cliente}
        */
        private int[,] tablaSeguridadEquipoEditar = new int[,] {
            {0,0,0,0}, //idProyectoFK
            {0,0,0,0}, //idEmpleadoFK
            {1,0,0,0}, //rol
            {1,1,0,0} //estado
        };

        /* 1 Pueder ver la acción para realizarla
         * 2 No puede verla porque no la puede realizar
        */
        private int[,] tablaSeguridadRequerimientosGeneral = new int[,] {
                       /*Soporte/Calidad , Lider , Tester , Cliente*/
         /*Consultar*/                   {1,1,1,1},
         /*Agregar*/                     {1,2,2,2},
         /*Editar*/                      {1,1,1,2},
         /*Eliminar*/                    {1,2,2,2}
         };

        //---------------------------------------------------------------------------------------------------------------------------//



        /*Metodo para acceder a los permisos del usuario en la vista de consultarProyectos
         * Retorna un Tuple<int,string,int,int,int>, con los valores:
         *              rol (0 Soporte/Calidad , 1 Lider , 2 Tester , 3 Cliente)
         *              permisoConsultar (valor recuperado en la tabla de tablaSeguridadProyectoGeneral)
         *              cedulaUsuario
         *              permisoEditar (valor recuperado en la tabla de tablaSeguridadProyectoGeneral)
         *              permisoAgregar (valor recuperado en la tabla de tablaSeguridadProyectoGeneral)
         *              permisoBorrar (valor recuperado en la tabla de tablaSeguridadProyectoGeneral)
        */
        public Tuple<int, int, string, int, int, int> ProyectoConsultar(System.Security.Principal.IPrincipal user)
        {

            int permisoConsultar = 3; //Por defecto no puede consultar
            string cedulaUsuario = "";
            int permisoEditar = 2;   //Por defecto no puede editar
            int permisoAgregar = 2;   //Por defecto no puede editar
            int permisoBorrar = 2;   //Por defecto no puede editar


            //Obtiene el rol del usuario
            int rol = GetRoleUsuario(user);

            if (rol >= 0)// Si el usuario tiene un rol asignado
            {
                //Obtine los permisos de la tabla de Seguridad
                permisoConsultar = tablaSeguridadProyectoGeneral[0, rol];
                permisoAgregar = tablaSeguridadProyectoGeneral[1, rol]; ;
                permisoEditar = tablaSeguridadProyectoGeneral[2, rol]; ;
                permisoBorrar = tablaSeguridadProyectoGeneral[3, rol]; ;

                if (permisoConsultar == 2)//Solo puede ver los proyectos a los cuales pertenece
                {
                    //Para que el controlador haga un filtro se ocupa pasar la cedula del usuario
                    cedulaUsuario = IdUsuario(user);
                }

            }

            return Tuple.Create(rol, permisoConsultar, cedulaUsuario, permisoEditar, permisoAgregar, permisoBorrar);
        }

        //Returna un Tuple <itn,int,list<int>> o Null en caso de que el rol no sea valido
        //                  int = rol
        //                  int = permiso de agregar
        //                  list<int> = permisos por cada atributo
        //
        public Tuple<int, int, List<int>> ProyectoAgregar(System.Security.Principal.IPrincipal user)
        {
            int rol = GetRoleUsuario(user);
            if (rol >= 0)
            {

                List<int> permisos = new List<int>();
                for (int i = 0; i < 11; i++)
                {
                    permisos.Add(tablaSeguridadProyectoAgregar[i, rol]);
                }

                return Tuple.Create(rol, tablaSeguridadProyectoGeneral[1, rol], permisos);

            }
            else return null;

        }



        //Returna un Tuple <itn,int,list<int>> o Null en caso de que el rol no sea valido
        //                  int = rol
        //                  int = permiso de agregar
        //                  list<int> = permisos por cada atributo
        //
        public Tuple<int, int, List<int>> ProyectoEditar(System.Security.Principal.IPrincipal user)
        {
            int rol = GetRoleUsuario(user);
            //Si tiene un rol asignado
            if (rol >= 0)
            {
                List<int> permisos = new List<int>();
                for (int i = 0; i < 11; i++)
                {
                    permisos.Add(tablaSeguridadProyectoEditar[i, rol]);
                }

                return Tuple.Create(rol, tablaSeguridadProyectoGeneral[2, rol], permisos);

            }
            else return null;
        }

        public int ProyectoEliminar(System.Security.Principal.IPrincipal user)
        {
            int rol = GetRoleUsuario(user);
            return (tablaSeguridadProyectoGeneral[3, rol]);

        }


        /*Metodo para acceder a los permisos del usuario en la vista general de equipo.
        * Retorna un Tuple<int,int,int,int>, con los valores:
        *              rol (0 Soporte/Calidad , 1 Lider , 2 Tester , 3 Cliente)
        *              permisoConsultar (valor recuperado en la tabla general de permisos para equipo)
        *              permisoAgregar (valor recuperado en la tabla general de permisos para equipo)
        *              permisoEditar  (valor recuperado en la tabla general de permisos para equipo)
        *              permisoBorrar (valor recuperado en la tabla general de permisos para equipo)
       */
        public Tuple<int, int, int, int, int> EquipoConsultar(System.Security.Principal.IPrincipal user)
        {
            int rol = GetRoleUsuario(user);

            int permisoConsultar = 3; //Por defecto no puede consultar
            int permisoAgregar = 2;   //Por defecto no puede editar
            int permisoEditar = 2;   //Por defecto no puede editar
            int permisoBorrar = 2;   //Por defecto no puede editar

            if (rol >= 0)// Si el usuario tiene un rol asignado
            {
                //Obtine los permisos de la tabla de Seguridad
                permisoConsultar = tablaSeguridadEquipoGeneral[0, rol];
                permisoAgregar = tablaSeguridadEquipoGeneral[1, rol]; ;
                permisoEditar = tablaSeguridadEquipoGeneral[2, rol]; ;
                permisoBorrar = tablaSeguridadEquipoGeneral[3, rol]; ;
            }
            return Tuple.Create(rol, permisoConsultar, permisoEditar, permisoAgregar, permisoBorrar);
        }

        /*Metodo para acceder a los permisos del usuario en la vista de consultarProyectos
         * Retorna un Tuple<int,string,int,int,int>, con los valores:
         *              rol (0 Soporte/Calidad , 1 Lider , 2 Tester , 3 Cliente)
         *              permisoConsultar (valor recuperado en la tabla de tablaSeguridadProyectoGeneral)
         *              cedulaUsuario
         *              permisoEditar (valor recuperado en la tabla de tablaSeguridadProyectoGeneral)
         *              permisoAgregar (valor recuperado en la tabla de tablaSeguridadProyectoGeneral)
         *              permisoBorrar (valor recuperado en la tabla de tablaSeguridadProyectoGeneral)
        */
        public Tuple<int, int, int, int, int> RequerimientosConsultar(System.Security.Principal.IPrincipal user)
        {

            int permisoConsultar = 2; //Por defecto no puede consultar
            int permisoEditar = 2;   //Por defecto no puede editar
            int permisoAgregar = 2;   //Por defecto no puede agregar
            int permisoBorrar = 2;   //Por defecto no puede eliminar


            //Obtiene el rol del usuario
            int rol = GetRoleUsuario(user);

            if (rol >= 0)// Si el usuario tiene un rol asignado
            {
                //Obtiene los permisos de la tabla de Seguridad
                permisoConsultar = tablaSeguridadRequerimientosGeneral[0, rol];
                permisoEditar = tablaSeguridadRequerimientosGeneral[2, rol]; ;
                permisoAgregar = tablaSeguridadRequerimientosGeneral[1, rol]; ;
                permisoBorrar = tablaSeguridadRequerimientosGeneral[3, rol]; ;
            }

            return Tuple.Create(rol, permisoConsultar, permisoEditar, permisoAgregar, permisoBorrar);
        }

    }
}