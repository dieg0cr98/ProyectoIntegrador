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



namespace ProyectoIntegrador.Controllers
{
    public  class SeguridadController : Controller
    {
        private Gr03Proy2Entities2 db = new Gr03Proy2Entities2();

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

         int[,] tablaSeguridadProyectoGeneral = new int[,] {
             {1,2,2,2},
             {1,2,2,2},
             {1,1,2,2},
             {1,1,2,2}
         };

         int[,] tablaSeguridadProyectoEditar = new int[,] {
             {1,2,2,2},
             {1,2,2,2},
             {1,1,2,2},
             {1,1,2,2}
          };



        int[,] tablaSeguridadProyectoAgregar = new int[,] {
            {1,2,2,2},
            {1,2,2,2},
            {1,1,2,2},
            {1,1,2,2}
        };



        //Metodo para recuperar el id del usuario registrado actualmente
        public string idUsuario()
        {
            string email = User.Identity.Name;
            string idUsuario;
            if(User.IsInRole("Cliente"))
            {
                idUsuario = db.Cliente.Where(p => p.correo == email).FirstOrDefault().cedulaPK;

            }
            else
            {
                idUsuario = db.Empleado.Where(p => p.correo == email).FirstOrDefault().idEmpleadoPK;
            }
            
            return idUsuario;
        }
    }


}