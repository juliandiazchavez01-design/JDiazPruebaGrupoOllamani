using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class GetAllDTO
    {
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public decimal Salario { get; set; }
        public bool Activo{ get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; }
    }
}
