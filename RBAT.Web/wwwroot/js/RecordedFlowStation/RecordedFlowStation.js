var rfsTable = $('#rfsTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "50vh",
    scrollCollapse: true,
    paging: false,
    ajax: {        
        "url": "RecordedFlowStation/GetAll",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
            });
        }
    },
    columnDefs: [
        {
            "targets": [1],
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
        { "data": "name" },
        { "data": "description" }       
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
    if ($("#rfsTable input[type=text]").length > 0) { return false; }

    return true;
});

$(document).ready(function () {
    rfsTable;
    rfsAddDataTableButtonsBottom();
});

function rfsAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(rfsTable, {
        buttons: [
            {
                text: "Add", className: "addButton",
                action: function (e, dt, bt, config) { rfsAddRow(e, dt, bt, config); }
            },
            {
                text: "Delete", className: "deleteButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { rfsRemoveRow(e, dt, bt, config); }
            },
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { rfsEditRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#rfsButtonsBottom'));

    document.getElementById('rfsButtonsBottom').children[0].classList.remove('btn-group');

    rfsSetButtons('normal');
}

function rfsShowBadInputAlert() {
    var alertID = "#badInputAlert";
    $(alertID).html('Please check your data.Only numbers can be imported.');
    rfsShowAlert(alertID);
}

function rfsShowSuccessfullySaved(text) {
    var alertID = "#savedAlert";
    $(alertID).html(text);
    rfsShowAlert(alertID);
}

function rfsShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function rfsAddRow(e, dt, bt, config) {
    $.ajax({
        url: "RecordedFlowStation/Add"
    }).done(function (msg) {
        $("#rfsShowAddModal").html(msg);
        $("#rfsAddModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function rfsEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    $.ajax({
        url: "RecordedFlowStation/Edit",
        data: {
            id: rfsTable.row(r).data()["id"]
        }
    }).done(function (msg) {
        $("#rfsShowAddModal").html(msg);
        $("#rfsAddModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function rfsRemoveRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "RecordedFlowStation/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            rfsShowSuccessfullySaved('Record is successfully deleted');
            dt.draw();
        }
    });
}

function rfsSetButtons(mode) {
    rfsTable.buttons().enable(false);

    switch (mode) {
        case "edit":
            rfsTable.buttons([".editButton"]).enable(true);
            $(rfsTable.buttons(".editButton")[0].node).find("span").text('Cancel');
            break;
        case "cancel":
        case "normal":
            rfsTable.buttons([".editButton", ".addButton", ".deleteButton"]).enable(true);
            $(rfsTable.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
    }
}

function formatDetails(d) {
    return '<table class="table-bordered" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px; width:100%">' +
        '<tr style="background: #e9ecef">' +
        '<td><b>Latitude:</b></td>' +
        '<td>' +
        d.latitudeString +
        '</td>' +
        '<td><b>Longitude:</b></td>' +
        '<td>' +
        d.longitudeString +
        '</td>' +
        '<td><b>Altitude:</b></td>' +
        '<td>' +
        d.altitudeString +
        '</td>' +
        '</tr>' +
        '<tr style="background: #e9ecef">' +
        '<td colspan=6>' +
        ' <button type="button" class="btn btn-primary" onClick="openTimeRecordedFlow(' + d.id + ')">Recorded Flow</button >' +
        '</td > ' +
        '</tr>' +
        '</table>';
}

$('#rfsTable tbody').on('click',
    'td.details-control',
    function () {
        var tr = $(this).closest('tr');
        var row = rfsTable.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        } else {
            row.child(formatDetails(row.data())).show();
            tr.addClass('shown');
        }
    });

function openTimeRecordedFlow(recordedFlowStationID) {
    $.ajax({
        url: "TimeRecordedFlow/Index",
        data: { recordedFlowStationID: recordedFlowStationID }
    }).done(function (msg) {
        $("#tsdShowModal").html(msg);
        $("#tsdModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

Date.prototype.toDateInputValue = function () {
    var local = new Date(this);
    local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
    return local.toJSON().slice(0, 10);
};

function rfsModalClose() {
    $('#rfsModal').modal('hide');
};