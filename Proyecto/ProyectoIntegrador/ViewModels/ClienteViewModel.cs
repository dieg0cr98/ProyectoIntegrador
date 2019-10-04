using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIntegrador.ViewModels
{
    public class ClienteViewModel
    {
        //Metodo que retorna el resultado de los Clientes luego de consulta.
        public List<string> resultadoConsulta { get; set; }

        //Metodo que retornar la cantidad de Clientes encontrados.
        public int cantidadEncontrados { get; set; }
    }
}