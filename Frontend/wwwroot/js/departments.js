$(document).ready(function () {   

    var table = $('#tbl-department').DataTable({
        "paging": true,
        "lengthMenu": [[5, 10, 25, 50, 100, -10],[5, 10, 25, 50, 100, 'Show All']],
        "lengthChange": true,
        "searching": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"],
        "processing": true,
        "serverSide": true,
        "ajax": {
            type: 'POST',
            url: "https://localhost:7034/api/Department/Paging",
            dataType: 'json',
            dataSrc: 'data',
        },
        "columns": [
            { "data": null },
            { "data": "name", name: "Name" },
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
                    return `<button type="button" class="btn btn-warning text-white" onclick="return GetById(\'${row.dept_Id}'\)" data-toggle="modal" data-tooltips="tooltip" data-placement="left" title="Edit Department">
                                <i class="fas fa-pen"></i>
                            </button>
                            <button type="button" class="btn btn-danger" onclick="return Delete(\'${row.dept_Id}'\)" data-tooltips="tooltip" data-placement="right" title="Delete Department">
                                <i class="fas fa-trash"></i>
                            </button>`;
                }
            }
        ],
        "order": [[1, 'asc']],
    });

    table.on('draw.dt', function () {
        var PageInfo = $('#tbl-department').DataTable().page.info();
        table.column(0, { page: 'current' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1 + PageInfo.start;
        });
    }).buttons().container().appendTo('#tbl-department_wrapper .col-md-6:eq(0)');

    $("#department").validate({
        rules: {
            name: {
                required: true,
            },
            action: "required"
        },
        messages: {
            name: {
                required: "Please enter some data",
            },
            action: "Please provide some data"
        }
    });
})

function validate() {
    var name = $('#name').val();
    var message = document.getElementById('message')

    if (name == '') {
        message.innerHTML = "Name can't be empty";
    } else {
        message.innerHTML = "";
    }
}

function Insert() {
    var name = $('#name').val();
    var message = document.getElementById('message')

    console.log(name);
    if (name == '') {
        message.innerHTML = "Name can't be empty";
    } else {
        message.innerHTML = "";
        var formData = { departmentID: "", name: $("#name").val() };

        $.ajax({
            type: 'POST',
            url: "https://localhost:7034/api/Department",
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
                $('#tbl-department').DataTable().ajax.reload();
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

function GetById(id) {
    $.ajax({
        type: 'GET',
        url: "https://localhost:7034/api/Department/" + id,
        contentType: 'application/json',
        dataType: 'json',
        success: function (result) {
            var row = result.data;

            $('#id').val(row.dept_Id);
            $('#name').val(row.name);
            $('#formModal').modal('show');
        }
    })
    SetModal('edit');
    validate();
}

function Update() {
    var name = $('#name').val();
    var message = document.getElementById('message')

    console.log(name);
    if (name == '') {
        message.innerHTML = "Name can't be empty";
    } else {
        var formData = { departmentID: $('#id').val(), name: $("#name").val() };

        $.ajax({
            type: 'PUT',
            url: "https://localhost:7034/api/Department",
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
                $('#tbl-department').DataTable().ajax.reload();
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

function Delete(id) {
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
                url: "https://localhost:7034/api/Department/" + id,
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
                    $('#tbl-department').DataTable().ajax.reload();
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
}
function SetModal(action) {
    var form_id = document.getElementById("id_column");
    var inp_id = document.getElementById("id");
    var inp_name = document.getElementById("name");
    var msg_name = document.getElementById("message");

    var btnAdd = document.getElementById("Insert");
    var btnUpdate = document.getElementById("Update");
    var title = document.getElementById("formModalLabel");

    if (action == 'add') {
        form_id.style.display = "none"; //set input id hidden
        inp_id.value = ""; //set value input id
        inp_name.value = ""; //set value input name

        btnUpdate.style.display = "none"; //set button Update hide
        btnAdd.style.display = "block"; //set button Add show
        title.innerHTML = "Add Department"; //set title modal
    } else if (action == 'edit') {
        form_id.style.display = "block"; //set input id hidden
        btnUpdate.style.display = "block"; //set button Update show        

        btnAdd.style.display = "none"; //set button Add hide
        title.innerHTML = "Edit Department"; //set title modal
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
