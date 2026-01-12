using DL;
using ML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Empleado
    {
        public static ML.Result GetAllEF()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JDiazPruebaGrupoOllamaniEntities context = new DL.JDiazPruebaGrupoOllamaniEntities())
                {         
                    var listEmpleados = context.EmpleadoCRUD("GetAll", null,null,null,null,null,null,null);

                    if (listEmpleados != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var itemEmployee in listEmpleados)
                        {
                            ML.Empleado empleadoObject = new ML.Empleado();
                            empleadoObject.IdEmpleado = itemEmployee.IdEmpleado;
                            empleadoObject.Nombre = itemEmployee.NombreEmpleado;
                            empleadoObject.ApellidoPaterno = itemEmployee.ApellidoPaterno;
                            empleadoObject.ApellidoMaterno = itemEmployee.ApellidoMaterno;
                            empleadoObject.Salario = itemEmployee.Salario;
                            empleadoObject.Activo = itemEmployee.Activo.Value;
                            empleadoObject.FechaRegistro = itemEmployee.FechaRegistro.Value;
                            empleadoObject.Departamento = new ML.Departamento();
                            empleadoObject.Departamento.IdDepartamento = itemEmployee.IdDepartamento;
                            empleadoObject.Departamento.Nombre = itemEmployee.NombreDepartamento;

                            result.Objects.Add(empleadoObject);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al obtener los Empleados";
                    }

                }
            }
            catch (Exception ex)
            {
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
                result.Correct = false;
            }
            return result;
        }

        public static ML.Result GetByIdEF(int IdEmpleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JDiazPruebaGrupoOllamaniEntities context = new DL.JDiazPruebaGrupoOllamaniEntities())
                {
                    var empleadoDB = context.EmpleadoCRUD("GetById", IdEmpleado, null, null, null, null, null, null).SingleOrDefault();

                    if (empleadoDB != null)
                    {
                        ML.Empleado empleado = new ML.Empleado
                        {
                            IdEmpleado = empleadoDB.IdEmpleado,
                            Nombre = empleadoDB.NombreEmpleado,
                            ApellidoPaterno = empleadoDB.ApellidoPaterno,
                            ApellidoMaterno = empleadoDB.ApellidoMaterno,
                            Salario = empleadoDB.Salario,
                            Activo = empleadoDB.Activo.Value,
                            FechaRegistro = empleadoDB.FechaRegistro.Value,
                            Departamento = new ML.Departamento
                            {
                                IdDepartamento = empleadoDB.IdDepartamento,
                                Nombre = empleadoDB.NombreDepartamento
                            }
                        };

                        result.Object = empleado;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontro el empleado con el Id solicitado";
                    }
                }
            }
            catch (Exception ex) {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result AddEF(ML.Empleado empleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JDiazPruebaGrupoOllamaniEntities context =
                       new DL.JDiazPruebaGrupoOllamaniEntities())
                {
                    int filasAfectadas = context.Database.ExecuteSqlCommand(
                        "EXEC EmpleadoCRUD @Accion = {0}, @IdEmpleado = {1}, @Nombre = {2}, " +
                        "@ApellidoPaterno = {3}, @ApellidoMaterno = {4}, @Salario = {5}, " +
                        "@Activo = {6}, @IdDepartamento = {7}",
                        "Add",
                        empleado.IdEmpleado,
                        empleado.Nombre,
                        empleado.ApellidoPaterno,
                        empleado.ApellidoMaterno,
                        empleado.Salario,
                        empleado.Activo,
                        empleado.Departamento.IdDepartamento
                    );

                    if (filasAfectadas > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se pudo agregar el empleado";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public static ML.Result UpdateEF(ML.Empleado empleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JDiazPruebaGrupoOllamaniEntities context =
                       new DL.JDiazPruebaGrupoOllamaniEntities())
                {
                    int filasAfectadas = context.Database.ExecuteSqlCommand(
                       "EXEC EmpleadoCRUD @Accion = {0}, @IdEmpleado = {1}, @Nombre = {2}, " +
                       "@ApellidoPaterno = {3}, @ApellidoMaterno = {4}, @Salario = {5}, " +
                       "@Activo = {6}, @IdDepartamento = {7}",
                       "Update",
                       empleado.IdEmpleado,
                       empleado.Nombre,
                       empleado.ApellidoPaterno,
                       empleado.ApellidoMaterno,
                       empleado.Salario,
                       empleado.Activo,
                       empleado.Departamento.IdDepartamento
                     );

                    if (filasAfectadas > 0)
                    { 
                        result.Correct = true;
                    }else {
                        result.Correct = false;
                            result.ErrorMessage = "No se actualizó el empleado";
                    }
            }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }


        public static ML.Result DeleteEF(int IdEmpleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JDiazPruebaGrupoOllamaniEntities context = new DL.JDiazPruebaGrupoOllamaniEntities())
                {
                    int filasAfectadas = context.Database.ExecuteSqlCommand(
                                  "EXEC EmpleadoCRUD @Accion = {0}, @IdEmpleado = {1}",
                                  "Delete",
                                  IdEmpleado
                              );
                    if (filasAfectadas > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se eliminó el empleado, verifique el Id.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result GetAllDQ()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection conexion = new SqlConnection(DL.Conexion.Get()))
                {
                    SqlCommand cmd = new SqlCommand("EmpleadoCRUD", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Accion", "GetAll");

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count > 0)
                    {
                        result.Objects = new List<object>();

                        foreach (DataRow row in table.Rows)
                        {
                            ML.Empleado empleado = new ML.Empleado();
                            empleado.Departamento = new ML.Departamento();

                            empleado.IdEmpleado = Convert.ToInt32(row["IdEmpleado"]);
                            empleado.Nombre = row["NombreEmpleado"].ToString();
                            empleado.ApellidoPaterno = row["ApellidoPaterno"].ToString();
                            empleado.ApellidoMaterno = row["ApellidoMaterno"].ToString();
                            empleado.Salario = Convert.ToDecimal(row["Salario"]);
                            empleado.Activo = Convert.ToBoolean(row["Activo"]);
                            empleado.FechaRegistro = Convert.ToDateTime(row["FechaRegistro"]);
                            empleado.Departamento.IdDepartamento = Convert.ToInt32(row["IdDepartamento"]);
                            empleado.Departamento.Nombre = row["NombreDepartamento"].ToString();

                            result.Objects.Add(empleado);
                        }

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay empleados registrados";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result GetByIdDQ(int IdEmpleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection conexion = new SqlConnection(DL.Conexion.Get()))
                {
                    using (SqlCommand cmd = new SqlCommand("EmpleadoCRUD", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Accion", "GetById");
                        cmd.Parameters.AddWithValue("@IdEmpleado", IdEmpleado);

                        conexion.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            ML.Empleado empleado = new ML.Empleado();
                            empleado.Departamento = new ML.Departamento();

                            empleado.IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]);
                            empleado.Nombre = reader["NombreEmpleado"].ToString();
                            empleado.ApellidoPaterno = reader["ApellidoPaterno"].ToString();
                            empleado.ApellidoMaterno = reader["ApellidoMaterno"].ToString();
                            empleado.Salario = Convert.ToDecimal(reader["Salario"]);
                            empleado.Activo = Convert.ToBoolean(reader["Activo"]);
                            empleado.FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]);

                            empleado.Departamento.IdDepartamento =
                                Convert.ToInt32(reader["IdDepartamento"]);
                            result.Object = empleado;
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "Empleado no encontrado";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result AddDQ(ML.Empleado empleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection conexion = new SqlConnection(DL.Conexion.Get()))
                {
                    using (SqlCommand cmd = new SqlCommand("EmpleadoCRUD", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Accion", "Add");
                        cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                        cmd.Parameters.AddWithValue("@ApellidoPaterno", empleado.ApellidoPaterno);
                        cmd.Parameters.AddWithValue("@ApellidoMaterno", empleado.ApellidoMaterno);
                        cmd.Parameters.AddWithValue("@Salario", empleado.Salario);
                        cmd.Parameters.AddWithValue("@Activo", empleado.Activo);
                        cmd.Parameters.AddWithValue("@IdDepartamento", empleado.Departamento.IdDepartamento);

                        conexion.Open();
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se pudo insertar el empleado.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result UpdateDQ(ML.Empleado empleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection conexion = new SqlConnection(DL.Conexion.Get()))
                {
                    using (SqlCommand cmd = new SqlCommand("EmpleadoCRUD", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Accion", "Update");
                        cmd.Parameters.AddWithValue("@IdEmpleado", empleado.IdEmpleado);
                        cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                        cmd.Parameters.AddWithValue("@ApellidoPaterno", empleado.ApellidoPaterno);
                        cmd.Parameters.AddWithValue("@ApellidoMaterno", empleado.ApellidoMaterno);
                        cmd.Parameters.AddWithValue("@Salario", empleado.Salario);
                        cmd.Parameters.AddWithValue("@Activo", empleado.Activo);
                        cmd.Parameters.AddWithValue("@IdDepartamento", empleado.Departamento.IdDepartamento);

                        conexion.Open();
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se pudo insertar el empleado.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result DeleteDQ(int IdEmpleado)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (SqlConnection conexion = new SqlConnection(DL.Conexion.Get()))
                {
                    using (SqlCommand cmd = new SqlCommand("EmpleadoCRUD", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Accion", "Delete");
                        cmd.Parameters.AddWithValue("@IdEmpleado", IdEmpleado);

                        conexion.Open();
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se eliminó el empleado.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }
    }
}