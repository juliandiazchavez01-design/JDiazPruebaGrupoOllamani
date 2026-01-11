using ML;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL.Controllers
{
    public class EmpleadoController : Controller
    {
        public ActionResult GetAll()
        {
            ML.Empleado empleado = new ML.Empleado();
            empleado.Empleados = new List<object>();
            ML.Result result = BL.Empleado.GetAllEF();
            if (result.Correct && result.Objects != null)
            {
                empleado.Empleados = result.Objects;
            }
            return View(empleado);
        }

        [HttpGet]
        public ActionResult Formulario(int IdEmpleado)
        {
            ML.Empleado empleado = new ML.Empleado();
            empleado.Departamento = new ML.Departamento();

            ML.Result resultDepartamento = BL.Departamento.GetAll();
            if (resultDepartamento.Correct && resultDepartamento.Objects != null)
            {
                empleado.Departamento.Departamentos = resultDepartamento.Objects;
            }
            if (IdEmpleado > 0)
            {
                ML.Result resultGetByIdEmpleado = BL.Empleado.GetByIdEF(IdEmpleado);
                if (resultGetByIdEmpleado.Correct)
                {
                    empleado = (ML.Empleado)resultGetByIdEmpleado.Object;
                    empleado.Departamento.Departamentos = resultDepartamento.Objects;
                }
                else
                {
                    return View("Error");
                }
            }
            return View(empleado);
        }

        [HttpPost]
        public ActionResult Formulario(ML.Empleado empleado)
        {
            if (empleado.IdEmpleado == 0)
            {
                ML.Result result = BL.Empleado.AddEF(empleado);
                if (result.Correct)
                {
                    TempData["SwalTitle"] = "Éxito";
                    TempData["SwalText"] = "Empleado agregado correctamente";
                    TempData["SwalIcon"] = "success";
                }
                else
                {
                    TempData["SwalTitle"] = "Error";
                    TempData["SwalText"] = result.ErrorMessage;
                    TempData["SwalIcon"] = "error";
                }
            }
            else
            {
                ML.Result resultUpdate = BL.Empleado.UpdateEF(empleado);
                if (resultUpdate.Correct)
                {
                    TempData["SwalTitle"] = "Éxito";
                    TempData["SwalText"] = "Empleado actualizado correctamente";
                    TempData["SwalIcon"] = "success";
                }
                else
                {
                    TempData["SwalTitle"] = "Error";
                    TempData["SwalText"] = resultUpdate.ErrorMessage;
                    TempData["SwalIcon"] = "error";
                }
            }
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        public ActionResult Delete(int IdEmpleado)
        {
            ML.Result result = BL.Empleado.DeleteEF(IdEmpleado);
            if (result.Correct)
            {
                TempData["SwalTitle"] = "Eliminado";
                TempData["SwalText"] = "Empleado eliminado correctamente";
                TempData["SwalIcon"] = "success";
            }
            else
            {
                TempData["SwalTitle"] = "Error";
                TempData["SwalText"] = "No se pudo eliminar el empleado";
                TempData["SwalIcon"] = "error";
            }
            return RedirectToAction("GetAll");
        }
    }
}