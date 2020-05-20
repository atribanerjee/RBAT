var table = $('#climateStationTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "50vh",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "ClimateStation/GetAll",
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
        { "data": "description" },
        {
            data: "eptype",
            render: function (data, type, row) {
                if (row.epType) {
                    return '<input type="checkbox" disabled checked class="editor-active">';
                }
                else {
                    return '<input type="checkbox" disabled class="editor-active">';
                }
                //return data;
            },
            className: "dt-body-center"
        }
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
    if ($("#climateStationTable input[type=text]").length > 0) { return false; }

    return true;
});

$(document).ready(function () {
    table;
    addDataTableButtonsBottom();
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
        url: "ClimateStation/Add"
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
        url: "ClimateStation/Edit",
        data: {
            id: table.row(r).data()["id"]            
        }
    }).done(function (msg) {
        $("#showModal").html(msg);
        $("#myModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function saveRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($("input[type=text]", r).length === 0) { return; }
    var epType = false;
    $(r).children("td").each(function (i, it) {

        var checkBox = it.getElementsByClassName("editor-active");
        if (checkBox.length > 0) {
            if (checkBox[0].checked) {
                epType = true;
            }
        }
        else {
            var di = $("input", it).val();
            $(it).html(di);
            var cell = table.cell(it._DT_CellIndex.row, it._DT_CellIndex.column);
            cell.data(di);
        }
    });

    var selectedRow = dt.rows(".selected").data().to$();
    //Set epType
    selectedRow[0].epType = epType;

    $.ajax({
        url: "ClimateStation/Update",
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

    $.ajax({
        url: "ClimateStation/Remove",
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

function format(d) {
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
        ' <button type="button" class="btn btn-primary" onClick="openTimeClimateData(' + d.id + ')">Climate Data</button> ' +
        '</td>' +
        '</tr>' +
        '</table>';
}

$('#climateStationTable tbody').on('click',
    'td.details-control',
    function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
        } else {
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });

function openTimeClimateData(climateStationID) {
    $.ajax({
        url: "TimeClimateData/Index",
        data: { climateStationID: climateStationID }
    }).done(function (msg) {
        $("#tsdShowModal").html(msg);
        $("#tsdModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

Date.prototype.toDateInputValue = function() {
    var local = new Date(this);
    local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
    return local.toJSON().slice(0, 10);
};