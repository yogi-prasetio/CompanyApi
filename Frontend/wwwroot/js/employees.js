$(document).ready(function () {
    var table = $('#tbl-employee').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "info": false,
        "autoWidth": true,
        "responsive": true,
        "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"],

        "ajax": {
            type: 'GET',
            url: "https://localhost:7034/api/Employee",
            dataType: 'json',
            dataSrc: 'data',
        },
        "columns": [
            { "data": null },
            { "data": "firstName" },
            { "data": "lastName" },
            { "data": "email" },
            { "data": "phoneNumber" },
            { "data": "address" },
            {
                "data": "status",
                render: function (data, type, row, meta) {
                    return row.status == true ? '<span class="badge badge-success p-1" >Active</span>' : '<span class="badge badge-danger p-1" >Resign</span>';
                }
            },
            { "data": "department_Name" },
            { "data": null },
        ],
        columnDefs: [
            {
                targets: [0],
                searchable: false,
                orderable: false,
            },
            {
                orderable: false,
                targets: [-1],
                render: function (data, type, row, meta) {
                    return `<button type="button" class="btn btn-warning text-white" onclick="return GetById(\'${row.nik}'\)" data-toggle="modal" data-tooltips="tooltip" data-placement="left" title="Edit Employee">
                                <i class="fas fa-pen"></i>
                            </button> &nbsp;
                            <button type="button" class="btn btn-danger" onclick="return Delete(\'${row.nik}'\)" data-tooltips="tooltip" data-placement="right" title="Delete Employee">
                                <i class="fas fa-trash"></i>
                            </button>`;
                }
            }
        ],
        "order": [[1, 'asc']],
    });

    table.on('draw.dt', function () {
        var PageInfo = $('#tbl-employee').DataTable().page.info();
        table.column(0, { page: 'current' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1 + PageInfo.start;
        });
    });

    //Add data department to select option
    var departmentSelect = document.getElementById('department_id');
    $.ajax({
        type: 'GET',
        url: "https://localhost:7034/api/Department",
        dataType: 'json',
        dataSrc: 'data',
    }).then(function (result) {
        // create the option and append to Select2
        for (let i = 0; i < result.data.length; i++) {
            var option = new Option(result.data[i].name, result.data[i].dept_Id, true, false);
            departmentSelect.add(option);
        }
    });

    var tbl_act_empl = $('#tbl-active-employee').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "info": false,
        "autoWidth": false,
        "responsive": true,
        "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"],

        "ajax": {
            type: 'GET',
            url: "https://localhost:7034/api/Employee/ActiveEmployee",
            dataType: 'json',
            dataSrc: 'data',
        },
        "columns": [
            { "data": null },
            { "data": "fulName" },
            { "data": "email" },
            { "data": "phoneNumber" },
            { "data": "address" },
            { "data": "departmentName" },
        ],
        columnDefs: [
            {
                targets: [0],
                searchable: false,
                orderable: false,
            },
        ],
        "order": [[1, 'asc']],
    });

    tbl_act_empl.on('draw.dt', function () {
        var PageInfo = $('#tbl-active-employee').DataTable().page.info();
        tbl_act_empl.column(0, { page: 'current' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1 + PageInfo.start;
        });
    });

    var tbl_resign = $('#tbl-resign-employee').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "info": false,
        "autoWidth": false,
        "responsive": true,
        "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"],

        "ajax": {
            type: 'GET',
            url: "https://localhost:7034/api/Employee/ResignEmployee",
            dataType: 'json',
            dataSrc: 'data',
        },
        "columns": [
            { "data": null },
            { "data": "fulName" },
            { "data": "email" },
            { "data": "phoneNumber" },
            { "data": "address" },
            { "data": "departmentName" },
        ],
        columnDefs: [
            {
                targets: [0],
                searchable: false,
                orderable: false,
            },
        ],
        "order": [[1, 'asc']],
    });

    tbl_resign.on('draw.dt', function () {
        var PageInfo = $('#tbl-resign-employee').DataTable().page.info();
        tbl_resign.column(0, { page: 'current' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1 + PageInfo.start;
        });
    });
})

var firstName = document.getElementById('first_name');
var lastName = document.getElementById('last_name');
var phoneNumber = document.getElementById('phone');
var address = document.getElementById('address');
var status = document.getElementById('status');
var department = document.getElementById('department_id');

var msg_first_name = document.getElementById('message_first_name');
var msg_last_name = document.getElementById('message_last_name');
var msg_phone = document.getElementById('message_phone');
var msg_address = document.getElementById('message_address');
var msg_status = document.getElementById('message_status');
var msg_department = document.getElementById('message_department');

function validation() {
    if ($('#first_name').val() == '') {
        $('#first_name').addClass("is-invalid");
        msg_first_name.innerHTML = "First Name can't be empty";
    } else {
        $('#first_name').removeClass("is-invalid");
        msg_first_name.innerHTML = '';
    }
    if ($('#last_name').val() == '') {
        $('#last_name').addClass("is-invalid");
        msg_last_name.innerHTML = "Last Name can't be empty";
    } else {
        $('#last_name').removeClass("is-invalid");
        msg_last_name.innerHTML = '';
    }
    if ($('#phone').val() == '') {
        $('#phone').addClass("is-invalid");
        msg_phone.innerHTML = "Phone Number can't be empty"
    } else {
        $('#phone').removeClass("is-invalid");
        msg_phone.innerHTML = '';
    }
    if ($('#address').val() == '') {
        $('#address').addClass("is-invalid");
        msg_address.innerHTML = "Address can't be empty";
    } else {
        $('#address').removeClass("is-invalid");
        msg_address.innerHTML = '';
    }
    if ($('#department_id').val() == null) {
        $('#department_id').addClass("is-invalid");
        msg_department.innerHTML = "Department can't be empty";
    } else {
        $('#department_id').removeClass("is-invalid");
        msg_department.innerHTML = '';
    }
}

function Insert() {
    validation();

    if ($('#first_name').val() != '' &&
        $('#last_name').val() != '' &&
        $('#phone').val() != '' && 
        $('#address').val() != '' &&
        $('#department_id').val() != null) {
        var formData = {
            nik: "",
            firstName: $("#first_name").val(),
            lastName: $("#last_name").val(),
            phoneNumber: $("#phone").val(),
            address: $("#address").val(),
            status: true,
            department_Id: $("#department_id").val()
        };

        $.ajax({
            type: 'POST',
            url: "https://localhost:7034/api/Employee",
            data: JSON.stringify(formData),
            contentType: 'application/json',
        }).then((result) => {
            if (result.status == 200) {
                Swal.fire({
                    title: 'Success!',
                    text: result.message,
                    icon: 'success',
                    showConfirmButton: false,
                    timer: 1500
                });
                $('#tbl-employee').DataTable().ajax.reload();
            } else {
                Swal.fire({
                    title: 'Failed!',
                    text: result.message,
                    icon: 'error',
                    showConfirmButton: false,
                    timer: 3000
                });
            }
            $("#formModal").modal('hide');
        });
    }
}

function GetById(nik) {
    $.ajax({
        type: 'GET',
        url: "https://localhost:7034/api/Employee/" + nik,
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            var row = result.data;

            $('#nik').val(row.nik);
            $('#first_name').val(row.firstName);
            $('#last_name').val(row.lastName);
            $('#phone').val(row.phoneNumber);
            $('#address').val(row.address);
            $('#status').val(row.status.toString());
            $('#department_id').val(row.department_Id);
            $('#formModal').modal('show');
        }
    })
    SetModal('edit');
}

function Update() {
    validation();

    if ($('#first_name').val() != '' &&
        $('#last_name').val() != '' &&
        $('#phone').val() != '' &&
        $('#address').val() != '' &&
        $('#department_id').val() != null) {

        var status;
        if ($("#status").val() == "true") {
            status = true;
        } else {
            status = false;
        }

        var formData = {
            nik: $("#nik").val(),
            firstName: $("#first_name").val(),
            lastName: $("#last_name").val(),
            phoneNumber: $("#phone").val(),
            address: $("#address").val(),
            status: status,
            department_Id: $("#department_id").val(),
        }

        $.ajax({
            type: 'PUT',
            url: "https://localhost:7034/api/Employee",
            data: JSON.stringify(formData),
            contentType: 'application/json',
        }).then((result) => {
            if (result.status == 200) {
                Swal.fire({
                    title: 'Success!',
                    text: result.message,
                    icon: 'success',
                    showConfirmButton: false,
                    timer: 1500
                });
                $('#tbl-employee').DataTable().ajax.reload();
            } else {
                Swal.fire({
                    title: 'Failed!',
                    text: result.message,
                    icon: 'error',
                    showConfirmButton: false,
                    timer: 3000
                });
            }
            $("#formModal").modal('hide');
        });
    }
}

function Delete(nik) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'DELETE',
                url: "https://localhost:7034/api/Employee/" + nik,
                dataType: 'json',
                contentType: 'application/json',
            }).then((response) => {
                if (response.status == 200) {
                    Swal.fire({
                        title: 'Success!',
                        text: response.message,
                        icon: 'success',
                        showConfirmButton: false,
                        timer: 1500
                    });
                    $('#tbl-employee').DataTable().ajax.reload();
                } else {
                    Swal.fire({
                        title: 'Failed!',
                        text: response.message,
                        icon: 'warning',
                        showConfirmButton: false,
                        timer: 3500
                    });
                }
            });
        }
    });
    debugger;
}
function SetModal(action) {
    var title = document.getElementById('formModalLabel');
    if (action == 'add') {
        title.innerHTML = "Add Employee";
        $('#nik_column').hide();
        $('#status_column').hide();
        $('#Update').hide();
        $('#first_name').val(""); //set value input name
        $('#last_name').val(""); //set value input name
        $('#phone').val(""); //set value input name
        $('#address').val(""); //set value input name
        $('#department_id').val(""); //set value input name

        msg_first_name.innerHTML = '';
        msg_last_name.innerHTML = '';
        msg_phone.innerHTML = '';
        msg_address.innerHTML = '';
        msg_department.innerHTML = '';

        $('#first_name').removeClass('is-invalid');
        $('#last_name').removeClass('is-invalid');
        $('#phone').removeClass('is-invalid');
        $('#address').removeClass('is-invalid');
        $('#department_id').removeClass('is-invalid');

        $('#Insert').show();
    } else if (action == 'edit') {
        title.innerHTML = "Edit Employee";
        $('#nik_column').show();
        $('#status_column').show();

        msg_first_name.innerHTML = '';
        msg_last_name.innerHTML = '';
        msg_phone.innerHTML = '';
        msg_address.innerHTML = '';
        msg_department.innerHTML = '';

        $('#first_name').removeClass('is-invalid');
        $('#last_name').removeClass('is-invalid');
        $('#phone').removeClass('is-invalid');
        $('#address').removeClass('is-invalid');
        $('#department_id').removeClass('is-invalid');
        $('#Insert').hide();
        $('#Update').show();
    }
}

$(document).ajaxComplete(function () {
    // Required for Bootstrap tooltips in DataTables
    $('[data-tooltips="tooltip"]').tooltip({
        trigger: 'hover'
    });
    $('[data-tooltip="tooltip"]').tooltip({
        trigger: 'hover'
    });
});