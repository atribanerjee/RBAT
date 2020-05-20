
var table = $('#projectTable').DataTable({
        dom: 'lfrtip',
        processing: true,
        serverSide: true,
        ordering: false,
        searching: false,
        scrollY: "200px",
        scrollCollapse: true,
        paging: false,
        ajax: {
            "url": "/Project/GetAllProjects",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                return $.extend({}, d, {
                    "typeID": 0
                });
            }
        },
        columnDefs: [
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            }
        ],
        columns: [
            { "data": "id" },
            { "data": "name" },
            { "data": "routingOptionType" },
            { "data": "dataReadType" },
            { "data": "calculationBegin" },
            { "data": "calculationEnd" }
        ],
        language: {
            "info": "Showing _TOTAL_ entries",
            "infoEmpty": "Showing 0 entries",
            "loadingRecords": "Please wait - loading..."
        },
        select: "single",
        stateSave: true
    }).on("user-select", function (e, dt, type, cell, originalEvent) {       
        // I do not let the user deselect until done editing the row or editing is cancelled
        if ($("#projectTable input[type=text]").length > 0) { return false; }
   
        return true;
        });

    $(document).ready(function () {
        table;
        addDataTableButtonsBottom();

        $('#projectTable tbody').on('click', 'tr', function () {
            var pos = table.row(this).index();
            var row = table.row(pos).data();
            $("#tabs").tabs('option', 'active', 0);

            $.ajax({
                type: "GET",
                url: "/Project/ProjectNode",
                data: {
                    projectId: row["id"]
                },
                success(data) {
                    $("#nodeDiv").html(data);                        
                  }
            });
         
        });
    });

    function addDataTableButtonsBottom() {
        new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    text: "Add", className: "addButton",
                    action: function (e, dt, bt, config) { addRow(e, dt, bt, config); }
                },
                {
                    text: "Copy", className: "copyButton",
                    extend: "selectedSingle",
                    action: function (e, dt, bt, config) { copyProject(e, dt, bt, config); }
                },
                {
                    text: "Delete", className: "deleteButton",
                    extend: "selectedSingle",
                    action: function (e, dt, bt, config) { removeRow(e, dt, bt, config); }
                },
                {
                    text: "Edit", className: "editButton",
                    extend: "selectedSingle",
                    action: function (e, dt, bt, config) { editRow(e, dt, bt, config); }
                },
                {
                    text: "Update", className: "updateButton",
                    action: function (e, dt, bt, config) { saveRow(e, dt, bt, config); }
                }
            ]
        }).container().appendTo($('#buttonsBottom'));

        document.getElementById('buttonsBottom').children[0].classList.remove('btn-group');

        setButtons('normal');
    }

    function showBadInputAlert() {
        var alertID = "#badInputAlert";
        $(alertID).html('Please check your data.Only numbers can be imported.');
        showAlert(alertID);
    }

    function showNotSaved(text) {
        var alertID = "#badInputAlert";
        $(alertID).html(text);
        showAlert(alertID);
    }

    function showSuccessfullySaved(text) {
        var alertID = "#savedAlert";
        $(alertID).html(text);
        showAlert(alertID);
    }

    function showAlert(alertID) {
        $(alertID).removeClass("in").show();
        $(alertID).delay(200).addClass("in").fadeOut(5000);
        $(alertID).show();
    }


    function addRow(e, dt, bt, config) {
        $.ajax({
            url:  "/Project/ProjectDetails"
        }).done(function (msg) {
            $("#showModal").html(msg);
            $("#myModal").modal({
                backdrop: 'static',
                keyboard: false
            });
        });
    }

    function copyProject(e, dt, bt, config) {
        var r = dt.rows(".selected").nodes()[0];        
        $.ajax({
            type: "GET",
            url: "/Project/CopyProject",
            data: {
                projectId: table.row(r).data()["id"]
            },
            dataType: 'json',
            beforeSend: function () {
                $('#overlay').removeClass("d-none").addClass("d-block");
            },
            success: function (data, status, xhr) {
                if (data.type === "Success") {
                    showSuccessfullySaved('Project copied successfully');
                    dt.draw();
                    $('#overlay').removeClass("d-block").addClass("d-none");
                }
                else if (data.type === "Details") {   
                    $('#overlay').removeClass("d-block").addClass("d-none")
                    $.ajax({
                        type: "GET",
                        url: "/Project/CopyProjectChooseUser",
                        data: {
                            projectId: table.row(r).data()["id"]
                        }
                    }).done(function (msg) {                        
                        $("#showModal").html(msg);
                        $("#copyProjectModal").modal({
                            backdrop: 'static',
                            keyboard: false
                        });
                    });
                }
                else {
                    $('#overlay').removeClass("d-block").addClass("d-none");
                    showNotSaved('Project is not copied successfully');
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $('#overlay').removeClass("d-block").addClass("d-none");
                showNotSaved('Project is not copied successfully');
            }
        });
    }

    function editRow(e, dt, bt, config) {
        var r = dt.rows(".selected").nodes()[0];
        $.ajax({
            type: "GET",
            url: "/Project/Edit",
            data: {
                id: table.row(r).data()["id"]
            }
        }).done(function (data) {
            $("#showModal").html(data);
            $("#modalEditProject").modal({
                backdrop: 'static',
                keyboard: false
            });
        });
    }

    function saveRow(e, dt, bt, config) {
        var r = dt.rows(".selected").projects()[0];
        if ($("input[type=text]", r).length === 0) { return; }

        $(r).children("td").each(function (i, it) {
            var di = $("input", it).val();
            $(it).html(di);
            var cell = table.cell(it._DT_CellIndex.row, it._DT_CellIndex.column);
            cell.data(di);
        });

        var selectedRow = dt.rows(".selected").data().to$();

        $.ajax({
            url: "/Project/UpdateProject",
            method: 'POST',
            data: { listToUpdate: selectedRow.toArray() },
            dataType: 'json',
            success: function (data, status, xhr) {
                showSuccessfullySaved('Record is successfully saved');
                dt.draw();
            }
        });

        setButtons('normal');
    }

    function removeRow(e, dt, bt, config) {
        var selectedRow = dt.rows(".selected").data().to$();
        $('#overlay').removeClass("d-none").addClass("d-block");
        $.ajax({
            url: "/Project/Remove",
            method: 'POST',
            data: { listToRemove: selectedRow.toArray() },
            dataType: 'json',
            success: function (data, status, xhr) {
                if (data.type === "Success") {
                    showSuccessfullySaved('Record is successfully deleted');
                    dt.draw();
                    $('#overlay').removeClass("d-block").addClass("d-none");
                }
                else {
                    $('#overlay').removeClass("d-block").addClass("d-none");
                    showSuccessfullySaved('The record cannot be deleted.');
                }
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $('#overlay').removeClass("d-block").addClass("d-none");
                alert('An error occurred... Look at the console (F12 or Ctrl+Shift+I, Console tab) for more information!');
                console.log('jqXHR:');
                console.log(jqXhr);
                console.log('textStatus:');
                console.log(textStatus);
                console.log('errorThrown:');
                console.log(errorThrown);
            }
        });
    }

    function setButtons(mode) {
        table.buttons().enable(false);

        switch (mode) {       
        case "edit":
            table.buttons([".editButton", ".updateButton", ".copyButton"]).enable(true);
            $(table.buttons(".editButton")[0].node).find("span").text('Cancel');
            break;
        case "cancel":
        case "normal":
            table.buttons([".editButton", ".addButton", ".deleteButton", ".copyButton"]).enable(true);
            $(table.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
        }
    }

function projectChanged() {
    table.draw();
}

function openScenario() {
    var r = $('table > tbody > tr.selected').index();   
    var projectId = table.row(r).data()["id"];
    location.href = "/Scenario/Index/" + projectId;
}

function openMap() {
    var r = $('table > tbody > tr.selected').index();
    var projectId = table.row(r).data()["id"];
    location.href = "/Map?projectId=" + projectId;
}

table.on('select', function (e, dt, type, indexes) {
    document.getElementById('openScenario').style.visibility = 'visible';
    document.getElementById('openMap').style.visibility = 'visible';
});

table.on('deselect', function (e, dt, type, indexes) {
    document.getElementById('openScenario').style.visibility = 'hidden';
    document.getElementById('openMap').style.visibility = 'hidden';
});


