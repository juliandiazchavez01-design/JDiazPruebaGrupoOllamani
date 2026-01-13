let departamentosGlobal = [];

$(document).ready(function () {
    $.getJSON(urlDepartamentoGetAll, function (data) {
        departamentosGlobal = data;

        initGridEmpleados();
    });
});

function initGridEmpleados() {
    const empleadoStore = new DevExpress.data.CustomStore({
        key: "IdEmpleado",

        load: function () {
            return $.getJSON(urlEmpleadoGetAll).then(res => {
                if (!res.Correct) {
                    DevExpress.ui.notify(res.Message, "error", 2000);
                    return [];
                }

                $.each(res.Objects, function (index, emp) {
                    if (emp.FechaRegistro) {
                        emp.FechaRegistro = new Date(parseInt(emp.FechaRegistro.replace(/\/Date\((\d+)\)\//, '$1')));
                    }
                });
                return res.Objects;
            });
        },

        insert: function (values) {
            return $.ajax({
                url: urlEmpleadoAdd,
                type: "POST",
                data: values
            });
        },

        update: function (key, values) {
            values.IdEmpleado = key;
                console.log("valors", values);
            return $.ajax({

                url: urlEmpleadoUpdate,
                type: "POST",
                success: function (result) {
                console.log(result, "Esto devuelve el success");
                },
                data: values  
            });
        },

        remove: function (key) {
            return $.ajax({
                url: urlEmpleadoDelete,
                type: "POST",
                data: { IdEmpleado: key }
            });
        }
    });

    $("#gridEmpleados").dxDataGrid({
        dataSource: empleadoStore,
        keyExpr: "IdEmpleado",

        showBorders: true,
        columnAutoWidth: true,

        paging: { pageSize: 10 },
        filterRow: { visible: true },
        searchPanel: { visible: true },

        onRowUpdating: function (e) {
            e.newData = $.extend(true, {}, e.oldData, e.newData);
            if (!e.newData.Departamento) {
                e.newData.Departamento = {
                    IdDepartamento: e.newData.Departamento?.IdDepartamento
                };
            }
        },

        onCellPrepared: function (e) {
            if (e.rowType === "header") {
                e.cellElement.css({
                    "font-weight": "bold",
                    "text-align": "center",
                    "background-color": "#f4f6f9"
                });
            }
        },
        editing: {
            mode: "popup",
            allowAdding: true,
            allowUpdating: true,
            allowDeleting: true,
            useIcons: true,

            onEditingStart: function (e) {
                if (!e.data.Departamento) {
                    e.data.Departamento = { IdDepartamento: null };
                }
            },

            texts: {
                addRow: "Agregar",
                editRow: "Editar",
                deleteRow: "Eliminar",
                saveRowChanges: "Guardar",
                cancelRowChanges: "Cancelar",
                confirmDeleteMessage: "¿Deseas eliminar el empleado?"
            }, popup: {
                title: "Empleado",
                showTitle: true,
                width: 600,
                height: "auto"
            },

            form: {
                colCount: 2,
                labelLocation: "top"
            },
            onInitNewRow: function (e) {
                e.data.Activo = true;
                e.data.Salario = 0;
                e.data.Departamento = { IdDepartamento: null };
            }
        },
        paging: { pageSize: 10 },
        filterRow: { visible: true },
        searchPanel: { visible: true },

        columns: [
            {
                dataField: "Nombre",
                caption: "Nombre",
                alignment: "center",
                editorType: "dxTextBox",
                validationRules: [
                    { type: "required", message: "El nombre es obligatorio" },
                    { type: "stringLength", min: 3, message: "Mínimo 3 caracteres" }
                ]
            },
            {
                dataField: "ApellidoPaterno",
                caption: "Apellido Paterno",
                alignment: "center",
                editorType: "dxTextBox",
                validationRules: [
                    { type: "required", message: "El Apellido Paterno es obligatorio" },
                    { type: "stringLength", min: 3, message: "Mínimo 3 caracteres" }
                ]
            },
            {
                dataField: "ApellidoMaterno",
                caption: "Apellido Materno",
                alignment: "center",
                editorType: "dxTextBox",
                validationRules: [
                    { type: "required", message: "El Apellido Materno es obligatorio" },
                    { type: "stringLength", min: 3, message: "Mínimo 3 caracteres" }
                ]
            },
            {
                dataField: "Salario",
                caption: "Salario",
                alignment: "center",
                editorType: "dxNumberBox",
                format: { type: "currency", precision: 2 },
                validationRules: [
                    { type: "required", message: "El salario es obligatorio" },
                    { type: "range", min: 0.01, message: "Debe ser mayor a 0" }
                ]
            },
            {
                dataField: "Activo",
                caption: "Activo",
                alignment: "center",
                dataType: "boolean"
            },
            {
                dataField: "FechaRegistro",
                caption: "Fecha Registro",
                alignment: "center",
                dataType: "date",
                format: "dd/MM/yyyy",
                allowEditing: false,
                formItem: {
                    visible: false
                }
            },
            {
                dataField: "Departamento.IdDepartamento",
                caption: "Departamento",
                alignment: "center",
                editorType: "dxSelectBox",
                lookup: {
                    dataSource: departamentosGlobal,
                    valueExpr: "IdDepartamento",
                    displayExpr: "Nombre"
                },
                validationRules: [{ type: "required", message: "El Departamento es obligatorio" }]
            }
        ]
    });
}