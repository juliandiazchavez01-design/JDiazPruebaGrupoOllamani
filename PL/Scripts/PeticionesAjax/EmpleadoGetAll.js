function cargarEmpleados() {
    $.ajax({
        url: urlEmpleadoGetAll,
        type: 'GET',
        success: function (data) {
            var tbody = $('#tblEmpleado tbody');
            if (data.Correct) {
                tbody.empty();

                $.each(data.Objects, function (indice, empleado) {
                    var nombreCompleto = `${empleado.Nombre} ${empleado.ApellidoPaterno} ${empleado.ApellidoMaterno}`;
                    var status = empleado.Activo ? "Activo" : "Inactivo";
                    var salarioFormateado = `$${empleado.Salario.toFixed(2)}`;

                    var fecha = new Date(parseInt(empleado.FechaRegistro.replace(/\/Date\((\d+)\)\//, '$1')));
                    var day = fecha.getDate().toString().padStart(2, '0');
                    var month = (fecha.getMonth() + 1).toString().padStart(2, '0');
                    var year = fecha.getFullYear();
                    var fechaFormateada = `${day}/${month}/${year}`;
                    tbody.append(`
                          <tr>
                              <th scope="row">${nombreCompleto}</th>
                              <td>${salarioFormateado}</td>
                              <td>${status}</td>
                              <td>${fechaFormateada}</td>
                              <td>${empleado.Departamento.Nombre}</td>
                              <td>
                                  <button class="btn btn-warning" id="btn-edit" data-id="${empleado.IdEmpleado}">Editar</button>
                              </td>
                              <td>
                              <button class="btn btn-danger btn-delete" data-id="${empleado.IdEmpleado}">Eliminar</button>
                              </td>
                          </tr>
                        `);
                })
            } else {
                tbody.append(`
                    <div class="alert alert-danger" role="alert">
                        No se encontraron empleados!
                    </div>
                    `);
            }

        },
        error: function (err) {
            console.error("Error al cargar empleados:", err);
        }
    });
}