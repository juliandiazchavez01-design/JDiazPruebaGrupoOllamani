using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public decimal Salario { get; set; }
        public bool Activo{ get; set; }
        public DateTime FechaRegistro { get; set; }
        public ML.Departamento Departamento { get; set; }
       public List<object> Empleados { get; set; }
    }
}