$(document).on('click', '.btn-delete', function () {
var idEmpleado = $(this).data('id');

Swal.fire({
    title: '¿Estás seguro de Eliminar?',
    text: "¡Se eliminara el empleado!",
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Sí, eliminar',
    cancelButtonText: 'Cancelar'
}).then((result) => {
    if (result.isConfirmed) {
        $.ajax({
            url: urlEmpleadoDelete,
            type: 'POST',
            data: { IdEmpleado: idEmpleado },
            success: function (data) {
                if (data.Correct) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Empleado eliminado',
                        timer: 1500,
                        showConfirmButton: false
                    });
                    cargarEmpleados();
                } else {
                    Swal.fire('Error', data.Message, 'error');
                }
            },
            error: function (err) {
                Swal.fire('Error', 'Ocurrió un error al eliminar el empleado', 'error');
            }
        });
    }
});
});