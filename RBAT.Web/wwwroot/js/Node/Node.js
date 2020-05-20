var table = $('#nodeTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "50vh",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "Node/GetAll",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "nodeTypeID": document.getElementById("NodeTypeID").value
            });
        }
    },
    columnDefs: [
        {
            "targets": [1],
            "visible": false,
            "searchable": false
        },
        {
            "targets": [2],
            "visible": false,
            "searchable": false
        }
    ],
    columns: [
        {
            "className": 'details-control',
            "orderable": false,
            "data": null,
            "defaultContent": ''
        },
        { "data": "id" },
        { "data": "nodeTypeId" },
        { "data": "name" },
        { "data": "description" },
        { "data": "sizeOfIrrigatedArea" },
        { "data": "landUseFactor" },
        { "data": "measuringUnit" }
    ],
    language: {
        "info": "Showing _TOTAL_ entries",
        "infoEmpty": "Showing 0 entries",
        "loadingRecords": "Please wait - loading..."
    },
    select: "single",
    stateSave: true,
    initComplete: function (settings, json) {
        var selectedNodeId = $("#selectedNodeId").val();
        if (selectedNodeId) {
            var rowIndex = -1;
            table.rows(function (idx, data, node) {
                if (data.id.toString() === selectedNodeId) {
                    rowIndex = idx;
                    return true;
                }
                return false;
            });
            if (rowIndex >= 0) {
                table.row(rowIndex).select();
                $($('#nodeTable tbody td.details-control')[rowIndex]).click();
            }
        } 
    }
}).on("user-select", function (e, dt, type, cell, originalEvent) {
    // I do not let the user deselect until done editing the row or editing is cancelled
    if ($("#nodeTable input[type=text]").length > 0) { return false; }

    return true;
});

$(document).ready(function () {
    table;
    addDataTableButtonsBottom();
    showHideColumns();
});

function addDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(table, {
        buttons: [
            {
                text: "Add", className: "addButton",
                action: function (e, dt, bt, config) { addRow(e, dt, bt, config); }
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
            }
        ]
    }).container().appendTo($('#buttonsBottom'));

    document.getElementById('buttonsBottom').children[0].classList.remove('btn-group');

    setButtons('normal');
}

function showHideColumns() {
    var nodeTypeId = document.getElementById("NodeTypeID").value;
    table.column(5).visible(nodeTypeId === '2');
    table.column(6).visible(nodeTypeId === '2');
    table.column(7).visible(nodeTypeId === '2');
    table.column(8).visible(nodeTypeId === '2');
}


function showBadInputAlert() {
    var alertID = "#badInputAlert";
    $(alertID).html('Please check your data.Only numbers can be imported.');
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
        url: "Node/Add",
        data: {            
            nodeTypeId: document.getElementById("NodeTypeID").value
        }
    }).done(function (msg) {
        $("#showModal").html(msg);
        $("#myModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function editRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    $.ajax({
        url: "Node/Edit",
        data: {
            id: table.row(r).data()["id"],
            name: table.row(r).data()["name"],
            description: table.row(r).data()["description"],
            nodeTypeId: table.row(r).data()["nodeTypeId"],
            sizeOfIrrigatedArea: table.row(r).data()["sizeOfIrrigatedArea"],
            landUseFactor: table.row(r).data()["landUseFactor"],
            measuringUnitId: table.row(r).data()["measuringUnitId"]
        }
    }).done(function (msg) {
        $("#showModal").html(msg);
        $("#myModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function removeRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "Node/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            showSuccessfullySaved('Record is successfully deleted');
            dt.draw();
        }
    });
}

function setButtons(mode) {
    table.buttons().enable(false);

    switch (mode) {
        case "edit":
            table.buttons([".editButton"]).enable(true);
            $(table.buttons(".editButton")[0].node).find("span").text('Cancel');
            break;
        case "cancel":
        case "normal":
            table.buttons([".editButton", ".addButton", ".deleteButton"]).enable(true);
            $(table.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
    }
}

function nodeTypeChanged() {
    table.draw();
    showHideColumns();
}

function formatReservoair(d) {
    return '<table class="table-bordered" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px; width:100%">' +
        '<tr style="background: #e9ecef">' +
        '<td style="background: #e9ecef">' +
        ' <button type="button" class="btn btn-primary" onClick="openTimeStorageCapacity(' + d.id + ')">Storage Capacity</button >' +
        ' <button type="button" class="btn btn-primary" onClick="openTimeHistoricLevel(' + d.id + ')">Historic Levels</button>' +
        ' <button type="button" class="btn btn-primary" onClick="openNetEvaporation(' + d.id + ')">Net Evaporation</button >' +
        ' <button type="button" class="btn btn-primary" onClick="openTimeNaturalFlow(' + d.id + ')">Natural Flow</button >' +
        '</td > ' +
        '</tr>' +
        '</table>';
}

function formatConsumptiveUse(d) {
    return '<table class="table-bordered" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px; width:100%">' +
        '<tr style="background: #e9ecef">' +
        '<td style="background: #e9ecef">' +
        ' <button type="button" class="btn btn-primary" onClick="openTimeWaterUse(' + d.id + ')">Water Use</button >' +
        '</td > ' +
        '</tr>' +
        '</table>';
}

function formatJunction(d) {
    return '<table class="table-bordered" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px; width:100%">' +
        '<tr style="background: #e9ecef">' +
        '<td style="background: #e9ecef">' +
        ' <button type="button" class="btn btn-primary" onClick="openTimeNaturalFlow(' + d.id + ')">Natural Flow</button >' +
        '</td > ' +
        '</tr>' +
        '</table>';
}

$('#nodeTable tbody').on('click',
    'td.details-control',
    function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        } else {
            addDetailedButtons(row);
            tr.addClass('shown');
        }
    });

function openTimeStorageCapacity(nodeID) {
    $.ajax({
        url: "TimeStorageCapacity/Index",
        data: { nodeID: nodeID }
    }).done(function (msg) {
        $("#tscShowModal").html(msg);
        $("#tscModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function openTimeHistoricLevel(nodeID) {
    $.ajax({
        url: "TimeHistoricLevel/Index",
        data: { nodeID: nodeID }
    }).done(function (msg) {
        $("#tsdShowModal").html(msg);
        $("#tsdModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function openNetEvaporation(nodeID) {
    $.ajax({
        url: "NetEvaporation/Index",
        data: { nodeID: nodeID }
    }).done(function (msg) {
        $("#neShowModal").html(msg);
        $("#neModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function openTimeNaturalFlow(nodeID) {
    $.ajax({
        url: "TimeNaturalFlow/Index",
        data: { nodeID: nodeID }
    }).done(function (msg) {
        $("#tsdShowModal").html(msg);
        $("#tsdModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function openTimeWaterUse(nodeID) {
    $.ajax({
        url: "TimeWaterUse/Index",
        data: { nodeID: nodeID }
    }).done(function (msg) {
        $("#tsdShowModal").html(msg);
        $("#tsdModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function addDetailedButtons(row) {
    var nodeTypeID = document.getElementById("NodeTypeID").value;

    if (nodeTypeID === "1") {
        row.child(formatReservoair(row.data())).show();
    }
    else if (nodeTypeID === "2") {
        row.child(formatConsumptiveUse(row.data())).show();
    }
    else if (nodeTypeID === "3") {
        row.child(formatJunction(row.data())).show();
    }
}

Date.prototype.toDateInputValue = function () {
    var local = new Date(this);
    local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
    return local.toJSON().slice(0, 10);
};