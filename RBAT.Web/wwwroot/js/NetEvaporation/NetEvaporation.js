var neTable = $('#neTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "200px",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "NetEvaporation/GetAllData",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "nodeID": document.getElementById("neNodeID").value
            });
        }
    },
    columnDefs: [
        {
            "targets": [0],
            "visible": false,
            "searchable": false
        },
        {
            "targets": [1],
            "visible": false,
            "searchable": true
        }
    ],
    columns: [
        { "data": "id" },
        { "data": "climateStationId" },
        { "data": "climateStationName" },
        { "data": "adjustmentFactor" }
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
    if ($("#neTable input[type=text]").length > 0) { return false; }

    return true;
});

$(document).ready(function () {
    neTable;
    neAddDataTableButtonsBottom();
});

function neAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(neTable, {
        buttons: [
            {
                text: "Add", className: "addButton",
                action: function (e, dt, bt, config) { neAddRow(e, dt, bt, config); }
            },
            {
                text: "Delete", className: "deleteButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { neRemoveRow(e, dt, bt, config); }
            },
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { neEditRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#neButtonsBottom'));

    document.getElementById('neButtonsBottom').children[0].classList.remove('btn-group');

    neSetButtons('normal');
}

function neShowBadInputAlert() {
    var alertID = "#badInputAlert";
    $(alertID).html('Please check your data.Only numbers can be imported.');
    neShowAlert(alertID);
}

function neShowSuccessfullySaved(text) {
    var alertID = "#savedAlert";
    $(alertID).html(text);
    neShowAlert(alertID);
}

function neShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function neAddRow(e, dt, bt, config) {
    $.ajax({
        url: "NetEvaporation/Add",
        data: {
            nodeID: document.getElementById("neNodeID").value
        }
    }).done(function (msg) {
        $("#neDetailShowModal").html(msg);
        $("#neAddModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function neEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];       

    $.ajax({
        url: "NetEvaporation/Edit",
        data: {
            id: neTable.row(r).data()["id"],
            climateStationID: neTable.row(r).data()["climateStationId"],
            adjustmentFactor: neTable.row(r).data()["adjustmentFactor"]
        }
    }).done(function (msg) {
        $("#neDetailShowModal").html(msg);
        $("#neAddModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function neRemoveRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "NetEvaporation/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {           
            neShowSuccessfullySaved('Record is successfully deleted');
            dt.draw();
        }
    });
}

function neSetButtons(mode) {
    neTable.buttons().enable(false);

    switch (mode) {
        case "cancel":
        case "normal":
            neTable.buttons([".editButton", ".addButton", ".deleteButton"]).enable(true);
            $(neTable.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
    }
}

function neModalClose() {
    $('#neModal').modal('hide');
}