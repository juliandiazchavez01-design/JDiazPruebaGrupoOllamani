$('#btnAgregar').on('click', function () {
    obtenerDepartamentos(function (departamentos) {
        abrirSwalEmpleado({
            titulo: 'Agregar Empleado',
            departamentos: departamentos,
            url: urlEmpleadoAdd,
            textoBoton: 'Guardar'
        });
    });
});

$(document).on('click', '#btn-edit', function () {
    const idEmpleado = $(this).data('id');
    $.ajax({
        url: urlEmpleadoGetById,
        type: 'GET',
        data: { IdEmpleado: idEmpleado },
        success: function (response) {
            if (!response.Correct) {
                Swal.fire('Error', 'No se pudo obtener el empleado', 'error');
                return;
            }

            obtenerDepartamentos(function (departamentos) {
                abrirSwalEmpleado({
                    titulo: 'Actualizar Empleado',
                    empleado: response.Object,
                    departamentos: departamentos,
                    url: urlEmpleadoUpdate,
                    textoBoton: 'Actualizar'
                });
            });
        },
        error: function () {
            Swal.fire('Error', 'Error al obtener el empleado', 'error');
        }
    });
});

function obtenerDepartamentos(callback) {
    $.ajax({
        url: urlDepartamentoGetAll,
        type: 'GET',
        success: function (data) {
            callback(data);
        },
        error: function () {
            Swal.fire('Error', 'No se pudieron cargar los departamentos', 'error');
        }
    });
}

function abrirSwalEmpleado({ titulo, empleado = {}, departamentos, url, textoBoton }) {

    let opciones = '<option value="">Selecciona departamento</option>';
    $.each(departamentos, function (indice, dep) {
        const selected = empleado.Departamento?.IdDepartamento === dep.IdDepartamento ? 'selected' : '';
        opciones += `<option value="${dep.IdDepartamento}" ${selected}>${dep.Nombre}</option>`;
    });

    Swal.fire({
        title: titulo,
        width: '700px',
        html: generarFormulario(opciones, empleado),
        showCancelButton: true,
        confirmButtonText: textoBoton,
        focusConfirm: false,
        preConfirm: () => construirPayload(empleado.IdEmpleado)
    }).then(result => {
        if (result.isConfirmed) {
            guardarEmpleado(result.value, url);
        }
    });
}

function generarFormulario(opciones, empleado) {
    const activoChecked = empleado.Activo ? 'checked' : '';
    return `
    <div class="swal-grid">

    <div class="swal-group">
        <label>Nombre</label>
        <input id="nombre" class="swal2-input" placeholder="Julian" value="${empleado.Nombre || ''}">
    </div>

    <div class="swal-group">
        <label>Apellido Paterno</label>
        <input id="apellidoPaterno" class="swal2-input" placeholder="Díaz" value="${empleado.ApellidoPaterno || ''}">
    </div>

    <div class="swal-group">
        <label>Apellido Materno</label>
        <input id="apellidoMaterno" class="swal2-input" placeholder="Chávez" value="${empleado.ApellidoMaterno || ''}">
    </div>

    <div class="swal-group">
        <label>Salario</label>
        <input id="salario" type="number" class="swal2-input" placeholder="127770.50" value="${empleado.Salario || ''}">
    </div>

    <div class="swal-group swal-full">
        <label>Departamento</label>
        <select id="departamento" class="swal2-input">
            ${opciones}
        </select>
    </div>

      <div class="swal-checkbox-group swal-full">
            <label>Activo</label>
            <div class="swal-checkbox">
                <input type="checkbox" id="activo" ${activoChecked}>
                <span>Empleado activo</span>
            </div>
        </div>
    </div>
    `;
}

function construirPayload(idEmpleado = null) {
    const payload = {
        IdEmpleado: idEmpleado,
        Nombre: $('#nombre').val().trim(),
        ApellidoPaterno: $('#apellidoPaterno').val().trim(),
        ApellidoMaterno: $('#apellidoMaterno').val().trim(),
        Salario: parseFloat($('#salario').val()),
        Activo: $('#activo').is(':checked'),
        Departamento: {
            IdDepartamento: parseInt($('#departamento').val())
        }
    };

    if (!payload.Nombre || !payload.ApellidoPaterno || !payload.ApellidoPaterno || isNaN(payload.Salario) || !payload.Departamento.IdDepartamento) {
        Swal.showValidationMessage('Completa todos los campos obligatorios');
        return false;
    }
    return payload;
}

function guardarEmpleado(payload, url) {
    $.ajax({
        url: url,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(payload),
        success: function (data) {
            if (data.Correct) {
                Swal.fire({
                    icon: 'success',
                    title: 'Operación exitosa',
                    timer: 1500,
                    showConfirmButton: false
                });
                cargarEmpleados();
            } else {
                Swal.fire('Error', data.Message, 'error');
            }
        },
        error: function () {
            Swal.fire('Error', 'Ocurrió un error', 'error');
        }
    });
}