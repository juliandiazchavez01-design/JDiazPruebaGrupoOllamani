using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Departamento
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JDiazPruebaGrupoOllamaniEntities context = new DL.JDiazPruebaGrupoOllamaniEntities())
                {
                    var listaDepartamento = context.DepartamentoGetAll().ToList();
                    result.Objects = new List<object>();
                    foreach (var itemDepartament in listaDepartamento)
                    {
                        ML.Departamento departamentObject = new ML.Departamento();
                        departamentObject.IdDepartamento = itemDepartament.IdDepartamento;
                        departamentObject.Nombre = itemDepartament.Nombre;

                        result.Objects.Add(departamentObject);
                    }
                    result.Correct = true;
                }            
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}
