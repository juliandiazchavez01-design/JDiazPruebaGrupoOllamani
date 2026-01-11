function confirmDeleteLink(url) {
    Swal.fire({
        title: '¿Eliminar?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sí'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = url;
        }
    });
}