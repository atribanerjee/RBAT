var table = $('#startingReservoirLevelTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "50vh",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "/StartingReservoirLevel/GetAll",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "projectID": document.getElementById("ProjectID").value
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
        { "data": "scenarioName" },
        { "data": "nodeName" },
        { "data": "initialElevation" }
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
    if ($("#startingReservoirLevelTable input[type=text]").length > 0) { return false; }

    return true;
});

$(document).ready(function () {
    table;
    srlAddDataTableButtonsBottom();
});

function srlAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(table, {
        buttons: [
            {
                text: "Add", className: "addButton",
                action: function (e, dt, bt, config) { srlAddRow(e, dt, bt, config); }
            },
            {
                text: "Delete", className: "deleteButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { srlRemoveRow(e, dt, bt, config); }
            },
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { srlEditRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#srlButtonsBottom'));

    document.getElementById('srlButtonsBottom').children[0].classList.remove('btn-group');

    srlSetButtons('normal');
}

function srlShowBadInputAlert() {
    var alertID = "#srlBadInputAlert";
    $(alertID).html('Please check your data. Only numbers can be imported.');
    srlShowAlert(alertID);
}

function srlShowSuccessfullySaved(text) {
    var alertID = "#srlSavedAlert";
    $(alertID).html(text);
    srlShowAlert(alertID);
}

function srlShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function srlAddRow(e, dt, bt, config) {
    $.ajax({
        url: "/StartingReservoirLevel/Add",
        data: {
            ProjectID: document.getElementById("ProjectID").value
        }
    }).done(function (msg) {
        $("#srlShowModal").html(msg);
        $("#srlAddModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function srlEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    $.ajax({
        url: "/StartingReservoirLevel/Edit",
        data: {
            id: table.row(r).data()["id"]
        }
    }).done(function (msg) {
        $("#srlShowModal").html(msg);
        $("#srlEditModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function srlRemoveRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "/StartingReservoirLevel/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            srlShowSuccessfullySaved('Record is successfully deleted');
            dt.draw();
        }
    });
}

function srlSetButtons(mode) {
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

function projectChanged() {
    table.draw();
}