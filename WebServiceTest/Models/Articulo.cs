using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServiceTest.Models
{
    public class Articulo
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public double Disponible { get; set; }
        public int CodigoGrupo { get; set; }
        public string Frecuencia { get; set; }
        public double CantidadFrecuencia { get; set; }
        public List<Almacen> Almacenes { get; set; }
    }
}