using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public class Conexion
    {
        public static string Get()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["JDiazPruebaGrupoOllamani"].ToString();
        }
    }
}
