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
using System.IO;
using System.Data.Entity;

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

        /*
         * Efecto: Metodo para saber el rol del usuario logueado
         * Requiere: System.Security.Principal.IPrincipal user . Con los datos del usuario registrado actualmenten
         * Retorna:  un int. Posible valores:
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




        /*
        * Efecto: Metodo para recuperar el id del usuario registrado actualmente
        * Requiere: Recibe como parametro un objeto tipo System.Security.Principal.IPrincipal con los datos del usuario logueado   
        * Modifica:
        * Retorna:string con el id del usuario. En caso de que no exista returna una string vacia
        */
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



        //----------------------------------------------------------------Proyecto------------------------------------------------//
        /* 1 Pueder hacer la accion para todos los proyectos(crud)
         * 2 Solo a los proyectos que pertenece
         * 3 No puede hacer la accion (crud)
        */
        public int[,] tablaSeguridadProyectoGeneral = new int[,] {
                       /*Soporte/Calidad , Lider , Tester , Cliente*/
         /*Consultar*/                   {1,2,2,2},
         /*Agregar*/                     {1,3,3,3},
         /*Editar*/                      {1,1,3,3},
         /*Eliminar*/                    {1,3,3,3}
         };


        /*
        * Efecto: 
        * Requiere: 
        * Modifica:
        * Retorna:  
        */

        /*
        * Efecto: Metodo Set para modificar datos de la tabla SeguridadProyectoGeneral
        * Requiere: int rol. 0 = Jefe Calidad/Soporte, 1 = Lider, 2 = Tester , 3 = Cliente
        * Modifica: datos de la tabla SeguridadProyectoGeneral 
        * Retorna:  
        */
        public void setTablaSeguridadProyectoGeneral(int rol, List<int> permisos)
        {
            SeguridadProyectoGeneral tabla = db.SeguridadProyectoGeneral.Find(rol);
            tabla.Consultar = permisos[0];
            tabla.Agregar = permisos[1];
            tabla.Editar = permisos[2];
            tabla.Eliminar = permisos[3];


            db.Entry(tabla).State = EntityState.Modified;
            db.SaveChanges();

        }

        /*
        * Efecto: Metodo Get para obtner la tabla SeguridadProyectoGeneral
        * Requiere: 
        * Modifica:
        * Retorna:  matriz int 4x4 con los permisos
        */
        public int[,] getTablaSeguridadProyectoGeneral()
        {

            int[,] permisos = new int[4, 4];



            for (int x = 0; x < 4; x++)
            {
                SeguridadProyectoGeneral tabla = db.SeguridadProyectoGeneral.Find(x);
                permisos[0, x] = tabla.Consultar;
                permisos[1, x] = tabla.Agregar;
                permisos[2, x] = tabla.Editar;
                permisos[3, x] = tabla.Eliminar;
            }

            return permisos;


        }


        /*
         * Efecto: Metodo Set para modificar datos de la tabla SeguridadProyectoGeneral
         * Requiere: int rol. 0 = Jefe Calidad/Soporte, 1 = Lider, 2 = Tester , 3 = Cliente
         * Modifica: datos de la tabla SeguridadProyectoGeneral 
         * Retorna:  
         */
        public void setTablaSeguridadProyectoEditar(int rol, List<int> permisos)
        {
            SeguridadProyectoEditar tabla = db.SeguridadProyectoEditar.Find(rol);
            tabla.Rol = rol;
            tabla.Nombre = permisos[0];
            tabla.Objetivo = permisos[1];
            tabla.Estado = permisos[2];
            tabla.DuracionEstimada = permisos[3];
            tabla.DuracionReal = permisos[4];
            tabla.FechaInicio = permisos[5];
            tabla.FechaFin = permisos[6];
            tabla.CedulaCliente = permisos[7];
            tabla.CedulaLider = permisos[8];


            db.Entry(tabla).State = EntityState.Modified;
            db.SaveChanges();

        }

        /*
        * Efecto: Metodo Get para obtner la tabla SeguridadProyectoGeneral
        * Requiere: 
        * Modifica:
        * Retorna:  matriz int 4x4 con los permisos
        */
        public int[,] getTablaSeguridadProyectoEditar()
        {

            int[,] permisos = new int[9, 4];



            for (int x = 0; x < 4; x++)
            {
                SeguridadProyectoEditar tabla = db.SeguridadProyectoEditar.Find(x);
                permisos[0,x] =  tabla.Nombre;
                permisos[1, x] = tabla.Objetivo;
                permisos[2, x] = tabla.Estado;
                permisos[3, x] = tabla.DuracionEstimada;
                permisos[4, x] = tabla.DuracionReal;
                permisos[5, x] = tabla.FechaInicio;
                permisos[6, x] = tabla.FechaFin;
                permisos[7, x] = tabla.CedulaCliente;
                permisos[8, x] = tabla.CedulaLider;

            }

            return permisos;


        }



        /* 0 No puede editar el atributo
         * 1 Si puede editar el atributo
        */
        public int[,] tablaSeguridadProyectoEditar = new int[,] {
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
        public int[,] tablaSeguridadProyectoAgregar = new int[,] {
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

                var tabla = getTablaSeguridadProyectoGeneral();
                permisoConsultar = tabla[0, rol];
                permisoAgregar = tabla[1, rol];
                permisoEditar = tabla[2, rol];
                permisoBorrar = tabla[3, rol];

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
            var tabla = getTablaSeguridadProyectoGeneral();
            if (rol >= 0)
            {

                List<int> permisos = new List<int>();
                for (int i = 0; i < 11; i++)
                {
                    permisos.Add(tablaSeguridadProyectoAgregar[i, rol]);
                }

                return Tuple.Create(rol, tabla[1, rol], permisos);

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
            var tabla = getTablaSeguridadProyectoGeneral();
            var tabla2 = getTablaSeguridadProyectoEditar();
            //Si tiene un rol asignado
            if (rol >= 0)
            {
                List<int> permisos = new List<int>();
                for (int i = 0; i < 9; i++)
                {
                    permisos.Add(tabla2[i, rol]);
                }

                return Tuple.Create(rol, tabla[2, rol], permisos);

            }
            else return null;
        }

        public int ProyectoEliminar(System.Security.Principal.IPrincipal user)
        {
            int rol = GetRoleUsuario(user);
            return (tablaSeguridadProyectoGeneral[3, rol]);

        }




        //----------------------------------------------------------------Tablas de Equipo------------------------------------------------//

        /*
       * Define los permisos para acceder los cruds de los equipos.
       * 1 = Todos los equipos.
       * 2 = Solo los que participa.
       * 3 = Ningún equipo.
       * Orden de usuarios: Soporte y Calidad, Lider, Tester, Cliente
       */
        public int[,] tablaSeguridadEquipoGeneral = new int[,] {
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
        public int[,] tablaSeguridadEquipoAgregar = new int[,] {
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
        public int[,] tablaSeguridadEquipoEditar = new int[,] {
            {0,0,0,0}, //idProyectoFK
            {0,0,0,0}, //idEmpleadoFK
            {1,0,0,0}, //rol
            {1,1,0,0} //estado
        };


        /*
        * Efecto: Metodo Set para modificar datos de la tabla SeguridadEquipoGeneral
        * Requiere: int rol. 0 = Jefe Calidad/Soporte, 1 = Lider, 2 = Tester , 3 = Cliente
        * Modifica: datos de la tabla SeguridadEquipoGeneral
        * Retorna:  
        */
        public void setTablaSeguridadEquipoGeneral(int rol, List<int> permisos)
        {
            SeguridadEquipoGeneral tabla = db.SeguridadEquipoGeneral.Find(rol);
            tabla.Consultar = permisos[0];
            tabla.Agregar = permisos[1];
            tabla.Editar = permisos[2];
            tabla.Eliminar = permisos[3];


            db.Entry(tabla).State = EntityState.Modified;
            db.SaveChanges();

        }

        /*
        * Efecto: Metodo Get para obtner la tabla SeguridadEquipoGeneral
        * Requiere: 
        * Modifica:
        * Retorna:  matriz int 4x4 con los permisos
        */
        public int[,] getTablaSeguridadEquipoGeneral()
        {

            int[,] permisos = new int[4, 4];



            for (int x = 0; x < 4; x++)
            {
                SeguridadEquipoGeneral tabla = db.SeguridadEquipoGeneral.Find(x);
                permisos[0, x] = tabla.Consultar;
                permisos[1, x] = tabla.Agregar;
                permisos[2, x] = tabla.Editar;
                permisos[3, x] = tabla.Eliminar;
            }

            return permisos;


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
                var tabla = getTablaSeguridadEquipoGeneral();
                //Obtine los permisos de la tabla de Seguridad
                permisoConsultar = tabla[0, rol];
                permisoAgregar = tabla[1, rol]; ;
                permisoEditar = tabla[2, rol]; ;
                permisoBorrar = tabla[3, rol]; ;
            }
            return Tuple.Create(rol, permisoConsultar, permisoEditar, permisoAgregar, permisoBorrar);
        }




        //----------------------------------------------------------------Tablas de Requerimientos------------------------------------------------//


        /* 1 Pueder ver la acción para realizarla
         * 2 No puede verla porque no la puede realizar
        */
        public int[,] tablaSeguridadRequerimientosGeneral = new int[,] {
                       /*Soporte/Calidad , Lider , Tester , Cliente*/
         /*Consultar*/                   {1,1,1,1},
         /*Agregar*/                     {1,1,2,2},
         /*Editar*/                      {1,1,1,2},
         /*Eliminar*/                    {1,2,2,2}
         };

        /* 0 No puede editar el atributo
         * 1 Si puede editar el atributo
        */
        public int[,] tablaSeguridadRequerimientosEditar = new int[,] {
                                          /*Soporte/Calidad , Lider , Tester , Cliente*/
          /*idRekPK*/                                 {0,0,0,0},
          /*idProyectoFK*/                            {0,0,0,0},
          /*cedulaTesterFK*/                          {1,1,0,0},
          /*nombre*/                                  {1,1,0,0},
          /*complejidad*/                             {1,1,0,0},
          /*tiempoEstimado*/                          {1,1,0,0},
          /*tiempoReal*/                              {1,1,1,0},
          /*descripcíon*/                             {1,1,0,0},
          /*fechaDeInicio*/                           {1,1,0,0},
          /*fechaDeFinalizacion*/                     {1,1,1,0},
          /*estado*/                                  {1,1,1,0},
          /*resultado*/                               {1,1,1,0},
          /*estadoResultado*/                         {1,1,1,0},
          /*detallesResultado*/                       {1,1,1,0},
          /*cantidadResultados*/                      {0,0,0,0}
          };


        /* 0 No puede editar el atributo
         * 1 Si puede editar el atributo
        */
        public int[,] tablaSeguridadRequerimientosAgregar = new int[,] {
                                          /*Soporte/Calidad , Lider , Tester , Cliente*/
          /*idRekPK*/                                 {0,0,0,0},
          /*idProyectoFK*/                            {1,0,0,0},
          /*cedulaTesterFK*/                          {1,1,0,0},
          /*nombre*/                                  {1,1,0,0},
          /*complejidad*/                             {1,1,0,0},
          /*tiempoEstimado*/                          {1,1,0,0},
          /*tiempoReal*/                              {0,0,0,0},
          /*descripcíon*/                             {1,1,0,0},
          /*fechaDeInicio*/                           {1,1,0,0},
          /*fechaDeFinalizacion*/                     {0,0,0,0},
          /*estado*/                                  {1,1,0,0},
          /*resultado*/                               {0,0,0,0},
          /*estadoResultado*/                         {0,0,0,0},
          /*detallesResultado*/                       {0,0,0,0},
          /*cantidadResultados*/                      {0,0,0,0}
        };

        /*
        * Efecto: Metodo Set para modificar datos de la tabla SeguridadRequerimientosGeneral
        * Requiere: int rol. 0 = Jefe Calidad/Soporte, 1 = Lider, 2 = Tester , 3 = Cliente
        * Modifica: datos de la tabla SeguridadRequerimientosGeneral
        * Retorna:  
        */
        public void setTablaSeguridadRequerimientosGeneral(int rol, List<int> permisos)
        {
            SeguridadRequerimientosGeneral tabla = db.SeguridadRequerimientosGeneral.Find(rol);
            tabla.Consultar = permisos[0];
            tabla.Agregar = permisos[1];
            tabla.Editar = permisos[2];
            tabla.Eliminar = permisos[3];


            db.Entry(tabla).State = EntityState.Modified;
            db.SaveChanges();

        }

        /*
        * Efecto: Metodo Get para obtner la tabla SeguridadRequerimientosGeneral
        * Requiere: 
        * Modifica:
        * Retorna:  matriz int 4x4 con los permisos
        */
        public int[,] getTablaSeguridadRequerimientosGeneral()
        {

            int[,] permisos = new int[4, 4];



            for (int x = 0; x < 4; x++)
            {
                SeguridadRequerimientosGeneral tabla = db.SeguridadRequerimientosGeneral.Find(x);
                permisos[0, x] = tabla.Consultar;
                permisos[1, x] = tabla.Agregar;
                permisos[2, x] = tabla.Editar;
                permisos[3, x] = tabla.Eliminar;
            }

            return permisos;


        }

        /*Metodo para acceder a los permisos del usuario en la vista de consultarProyectos
         * Retorna un Tuple<int,string,int,int,int>, con los valores:
         *              rol (0 Soporte/Calidad , 1 Lider , 2 Tester , 3 Cliente)
         *              permisoConsultar (valor recuperado en la tabla de tablaSeguridadRequerimientosGeneral)
         *              cedulaUsuario
         *              permisoEditar (valor recuperado en la tabla de tablaSeguridadRequerimientosGeneral)
         *              permisoAgregar (valor recuperado en la tabla de tablaSeguridadRequerimientosGeneral)
         *              permisoBorrar (valor recuperado en la tabla de tablaSeguridadRequerimientosGeneral)
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
                var tabla = getTablaSeguridadRequerimientosGeneral();
                //Obtiene los permisos de la tabla de Seguridad
                permisoConsultar = tabla[0, rol];
                permisoEditar = tabla[2, rol]; ;
                permisoAgregar = tabla[1, rol]; ;
                permisoBorrar = tabla[3, rol]; ;
            }

            return Tuple.Create(rol, permisoConsultar, permisoEditar, permisoAgregar, permisoBorrar);
        }

        //Returna un Tuple <itn,int,list<int>> o Null en caso de que el rol no sea valido
        //                  int = rol
        //                  int = permiso de agregar
        //                  list<int> = permisos por cada atributo
        //
        public Tuple<int, int, List<int>> RequerimientosAgregar(System.Security.Principal.IPrincipal user)
        {
            int rol = GetRoleUsuario(user);
            var tabla = getTablaSeguridadRequerimientosGeneral();
            if (rol >= 0)
            {
                
                List<int> permisos = new List<int>();
                for (int i = 0; i < 15; i++)
                {
                    permisos.Add(tablaSeguridadRequerimientosAgregar[i, rol]);
                }

                return Tuple.Create(rol, tabla[1, rol], permisos);

            }
            else return null;

        }



        //Returna un Tuple <itn,int,list<int>> o Null en caso de que el rol no sea valido
        //                  int = rol
        //                  int = permiso de editar
        //                  list<int> = permisos por cada atributo
        //
        public Tuple<int, int, List<int>> RequerimientosEditar(System.Security.Principal.IPrincipal user)
        {
            int rol = GetRoleUsuario(user);
            var tabla = getTablaSeguridadRequerimientosGeneral();
            //Si tiene un rol asignado
            if (rol >= 0)
            {
                List<int> permisos = new List<int>();
                for (int i = 0; i < 15; i++)
                {
                    permisos.Add(tablaSeguridadRequerimientosEditar[i, rol]);
                }

                return Tuple.Create(rol, tabla[2, rol], permisos);

            }
            else return null;
        }


        //----------------------------------------------------------------Tablas de Clientes------------------------------------------------//


        /* 1 Pueder hacer la accion para todos los clientes(crud)
         * 2 Solo a los clientes que pertenece
         * 3 No puede hacer la accion 
        */
        public int[,] tablaSeguridadClientes = new int[,] {
                       /*Soporte/Calidad , Lider , Tester , Cliente*/
         /*Consultar*/                   {1,2,2,2},
         /*Agregar*/                     {1,3,3,3},
         /*Editar*/                      {1,3,3,3},
         /*Eliminar*/                    {1,3,3,3}
         };


        /*
        * Efecto: Metodo Set para modificar datos de la tabla SeguridadClientesGeneral
        * Requiere: int rol. 0 = Jefe Calidad/Soporte, 1 = Lider, 2 = Tester , 3 = Cliente
        * Modifica: datos de la tabla SeguridadClientesGeneral
        * Retorna:  
        */
        public void setTablaSeguridadClientesGeneral(int rol, List<int> permisos)
        {
            SeguridadClientesGeneral tabla = db.SeguridadClientesGeneral.Find(rol);
            tabla.Consultar = permisos[0];
            tabla.Agregar = permisos[1];
            tabla.Editar = permisos[2];
            tabla.Eliminar = permisos[3];


            db.Entry(tabla).State = EntityState.Modified;
            db.SaveChanges();

        }

        /*
        * Efecto: Metodo Get para obtner la tabla SeguridadClientesGeneral
        * Requiere: 
        * Modifica:
        * Retorna:  matriz int 4x4 con los permisos
        */
        public int[,] getTablaSeguridadClientesGeneral()
        {

            int[,] permisos = new int[4, 4];



            for (int x = 0; x < 4; x++)
            {
                SeguridadClientesGeneral tabla = db.SeguridadClientesGeneral.Find(x);
                permisos[0, x] = tabla.Consultar;
                permisos[1, x] = tabla.Agregar;
                permisos[2, x] = tabla.Editar;
                permisos[3, x] = tabla.Eliminar;
            }

            return permisos;


        }



        /*Metodo para acceder a los permisos del usuario en la vista de consultarClientes
 * Retorna un Tuple<int,string,int,int,int>, con los valores:
 *              rol (0 Soporte/Calidad , 1 Lider , 2 Tester , 3 Cliente)
 *              permisoConsultar (valor recuperado en la tabla de tablaSeguridadClientes)
 *              cedulaUsuario
 *              permisoEditar (valor recuperado en la tabla de tablaSeguridadClientes)
 *              permisoAgregar (valor recuperado en la tabla de tablaSeguridadClientes)
 *              permisoBorrar (valor recuperado en la tabla de tablaSeguridadClientes)
*/
        public Tuple<int, string, int, int, int, int> ClienteConsultar(System.Security.Principal.IPrincipal user)
        {

            int permisoConsultar = 3; //Por defecto no puede consultar
            string cedulaUsuario = "";
            int permisoEditar = 3;   //Por defecto no puede editar
            int permisoAgregar = 3;   //Por defecto no puede editar
            int permisoBorrar = 3;   //Por defecto no puede editar


            //Obtiene el rol del usuario
            int rol = GetRoleUsuario(user);

            if (rol >= 0)// Si el usuario tiene un rol asignado
            {
                var tabla = getTablaSeguridadClientesGeneral();
                //Obtine los permisos de la tabla de Seguridad
                permisoConsultar = tabla[0, rol];
                permisoAgregar = tabla[1, rol]; ;
                permisoEditar = tabla[2, rol]; ;
                permisoBorrar = tabla[3, rol]; ;

                if (permisoConsultar == 2)//Solo puede ver los proyectos a los cuales pertenece
                {
                    //Para que el controlador haga un filtro se ocupa pasar la cedula del usuario
                    cedulaUsuario = IdUsuario(user);
                }

            }

            return Tuple.Create(rol, cedulaUsuario, permisoConsultar, permisoEditar, permisoAgregar, permisoBorrar);
        }


        //----------------------------------------------------------------Tablas de Empleados------------------------------------------------//
        /* 1 Pueder hacer la accion para todos los empleados(CRUD)
             * 2 Solo a los empleados que trabajan en un proyecto donde el es el lider
             * 3 No puede hacer la accion 
            */
        public int[,] tablaSeguridadEmpleados = new int[,] {
                       /*Soporte/Calidad , Lider , Tester , Cliente*/
         /*Consultar*/                   {1,2,2,3},
         /*Agregar*/                     {1,3,3,3},
         /*Editar*/                      {1,3,3,3},
         /*Eliminar*/                    {1,3,3,3}
         };


        /*
        * Efecto: Metodo Set para modificar datos de la tabla SeguridadEmpleadosGeneral
        * Requiere: int rol. 0 = Jefe Calidad/Soporte, 1 = Lider, 2 = Tester , 3 = Cliente
        * Modifica: datos de la tabla SeguridadEmpleadosGeneral
        * Retorna:  
        */
        public void setTablaSeguridadEmpleadosGeneral(int rol, List<int> permisos)
        {
            SeguridadEmpleadosGeneral tabla = db.SeguridadEmpleadosGeneral.Find(rol);
            tabla.Consultar = permisos[0];
            tabla.Agregar = permisos[1];
            tabla.Editar = permisos[2];
            tabla.Eliminar = permisos[3];


            db.Entry(tabla).State = EntityState.Modified;
            db.SaveChanges();

        }

        /*
        * Efecto: Metodo Get para obtner la tabla SeguridadEmpleadosGeneral
        * Requiere: 
        * Modifica:
        * Retorna:  matriz int 4x4 con los permisos
        */
        public int[,] getTablaSeguridadEmpleadosGeneral()
        {

            int[,] permisos = new int[4, 4];



            for (int x = 0; x < 4; x++)
            {
                SeguridadEmpleadosGeneral tabla = db.SeguridadEmpleadosGeneral.Find(x);
                permisos[0, x] = tabla.Consultar;
                permisos[1, x] = tabla.Agregar;
                permisos[2, x] = tabla.Editar;
                permisos[3, x] = tabla.Eliminar;
            }

            return permisos;


        }


        /*Metodo para acceder a los permisos del usuario en la vista de consultarClientes
      * Retorna un Tuple<int,string,int,int,int>, con los valores:
      *              rol (0 Soporte/Calidad , 1 Lider , 2 Tester , 3 Cliente)
      *              permisoConsultar (valor recuperado en la tabla de tablaSeguridadClientes)
      *              cedulaUsuario
      *              permisoEditar (valor recuperado en la tabla de tablaSeguridadClientes)
      *              permisoAgregar (valor recuperado en la tabla de tablaSeguridadClientes)
      *              permisoBorrar (valor recuperado en la tabla de tablaSeguridadClientes)
     */
        public Tuple<int, string, int, int, int, int> EmpleadoConsultar(System.Security.Principal.IPrincipal user)
        {

            int permisoConsultar = 3; //Por defecto no puede consultar
            string cedulaUsuario = "";
            int permisoEditar = 3;   //Por defecto no puede editar
            int permisoAgregar = 3;   //Por defecto no puede editar
            int permisoBorrar = 3;   //Por defecto no puede editar


            //Obtiene el rol del usuario
            int rol = GetRoleUsuario(user);

            if (rol >= 0)// Si el usuario tiene un rol asignado
            {

                var tabla = getTablaSeguridadEmpleadosGeneral();
                //Obtine los permisos de la tabla de Seguridad
                permisoConsultar = tabla[0, rol];
                permisoAgregar = tabla[1, rol]; ;
                permisoEditar = tabla[2, rol]; ;
                permisoBorrar = tabla[3, rol]; ;

                if (permisoConsultar == 2)//Solo puede ver los proyectos a los cuales pertenece
                {
                    //Para que el controlador haga un filtro se ocupa pasar la cedula del usuario
                    cedulaUsuario = IdUsuario(user);
                }

            }

            return Tuple.Create(rol, cedulaUsuario, permisoConsultar, permisoEditar, permisoAgregar, permisoBorrar);
        }

        //----------------------------------------------------------------Tablas de Pruebas------------------------------------------------//

        /*
        * Efecto: Metodo Set para modificar datos de la tabla SeguridadPruebasGeneral
        * Requiere: int rol. 0 = Jefe Calidad/Soporte, 1 = Lider, 2 = Tester , 3 = Cliente
        * Modifica: datos de la tabla SeguridadPruebasGeneral
        * Retorna:  
        */
        public void setTablaSeguridadPruebasGeneral(int rol, List<int> permisos)
        {
            SeguridadPruebasGeneral tabla = db.SeguridadPruebasGeneral.Find(rol);
            tabla.Consultar = permisos[0];
            tabla.Agregar = permisos[1];
            tabla.Editar = permisos[2];
            tabla.Eliminar = permisos[3];


            db.Entry(tabla).State = EntityState.Modified;
            db.SaveChanges();

        }

        /*
        * Efecto: Metodo Get para obtner la tabla SeguridadPruebasGeneral
        * Requiere: 
        * Modifica:
        * Retorna:  matriz int 4x4 con los permisos
        */
        public int[,] getTablaSeguridadPruebasGeneral()
        {

            int[,] permisos = new int[4, 4];



            for (int x = 0; x < 4; x++)
            {
                SeguridadPruebasGeneral tabla = db.SeguridadPruebasGeneral.Find(x);
                permisos[0, x] = tabla.Consultar;
                permisos[1, x] = tabla.Agregar;
                permisos[2, x] = tabla.Editar;
                permisos[3, x] = tabla.Eliminar;
            }

            return permisos;


        }



        /*Metodo para acceder a los permisos del usuario en la vista de consultarClientes
* Retorna un Tuple<int,string,int,int,int>, con los valores:
*              rol (0 Soporte/Calidad , 1 Lider , 2 Tester , 3 Cliente)
*              permisoConsultar (valor recuperado en la tabla de tablaSeguridadClientes)
*              cedulaUsuario
*              permisoEditar (valor recuperado en la tabla de tablaSeguridadClientes)
*              permisoAgregar (valor recuperado en la tabla de tablaSeguridadClientes)
*              permisoBorrar (valor recuperado en la tabla de tablaSeguridadClientes)
*/
        public Tuple<int, string, int, int, int, int> PruebasPermisos(System.Security.Principal.IPrincipal user)
        {

            int permisoConsultar = 3; //Por defecto no puede consultar
            string cedulaUsuario = "";
            int permisoEditar = 3;   //Por defecto no puede editar
            int permisoAgregar = 3;   //Por defecto no puede editar
            int permisoBorrar = 3;   //Por defecto no puede editar


            //Obtiene el rol del usuario
            int rol = GetRoleUsuario(user);

            if (rol >= 0)// Si el usuario tiene un rol asignado
            {

                var tabla = getTablaSeguridadPruebasGeneral();
                //Obtine los permisos de la tabla de Seguridad
                permisoConsultar = tabla[0, rol];
                permisoAgregar = tabla[1, rol]; ;
                permisoEditar = tabla[2, rol]; ;
                permisoBorrar = tabla[3, rol]; ;

                if (permisoConsultar == 2)//Solo puede ver los proyectos a los cuales pertenece
                {
                    //Para que el controlador haga un filtro se ocupa pasar la cedula del usuario
                    cedulaUsuario = IdUsuario(user);
                }

            }

            return Tuple.Create(rol, cedulaUsuario, permisoConsultar, permisoEditar, permisoAgregar, permisoBorrar);
        }

        //----------------------------------------------------------------Tablas de Consultas Avanzadas------------------------------------------------//



        //----Consultas Avanzadas--//

        /* 1 Pueder realizar la consulta
         * 2 Solo los datos en los que participa
         * 3 No puede realizar la consulta
        */
        public int[,] tablaSeguridadProyectoGeneralConsultas = new int[,] {
                           /*Soporte/Calidad , Lider , Tester , Cliente*/
             /*Consulta1*/                   {1,2,3,3},
             /*Consulta2*/                   {1,2,3,3},
             /*Consulta3*/                   {1,2,3,3},
             /*Consulta4*/                   {1,2,3,3},
             /*Consulta5*/                   {1,2,3,3},
             /*Consulta6*/                   {1,3,3,3},
             /*Consulta7*/                   {1,3,3,3},
             /*Consulta8*/                   {1,3,3,2},
             /*Consulta9*/                   {1,3,3,2},
             /*Consulta10*/                  {1,3,3,2}
        };



            /*Metodo para acceder a las consultas avanzadas
             * Retorna un Tuple<int,List<int>>, con los valores:
             *              rol (0 Soporte/Calidad , 1 Lider , 2 Tester , 3 Cliente)
             *              permisos (valor recuperado en la tabla de tablaSeguridadProyectoGeneralConsultas)
             *              idUsuario (cedula)
            */
            public Tuple<int, List<int>, string> permisosConsultasAvanzadas(System.Security.Principal.IPrincipal user)
            {
                int consulta1 = 3; // Por defecto no puede realizar la consulta
                int consulta2 = 3; // Por defecto no puede realizar la consulta
                int consulta3 = 3; // Por defecto no puede realizar la consulta
                int consulta4 = 3; // Por defecto no puede realizar la consulta
                int consulta5 = 3; // Por defecto no puede realizar la consulta
                int consulta6 = 3; // Por defecto no puede realizar la consulta
                int consulta7 = 3; // Por defecto no puede realizar la consulta
                int consulta8 = 3; // Por defecto no puede realizar la consulta
                int consulta9 = 3; // Por defecto no puede realizar la consulta
                int consulta10 = 3; // Por defecto no puede realizar la consulta
                string cedulaUsuario = "";
                List<int> permisos = new List<int>() { consulta1, consulta2, consulta3, consulta4, consulta5, consulta6, consulta7, consulta8, consulta9, consulta10 };


                //Obtiene el rol del usuario
                int rol = GetRoleUsuario(user);

                if (rol >= 0)// Si el usuario tiene un rol asignado
                {
                    cedulaUsuario = IdUsuario(user);
                    //Obtiene los permisos de la tabla de Seguridad
                    for (int x = 0; x < 10; x++)
                    {
                        permisos[x] = tablaSeguridadProyectoGeneralConsultas[x, rol];
                    }

                }

                return Tuple.Create(rol, permisos, cedulaUsuario);
            }



    }
}
