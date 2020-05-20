var table = $('#scenarioTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "50vh",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "/Scenario/GetAll",
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
        { "data": "projectID" },
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
    if ($("#scenarioTable input[type=text]").length > 0) { return false; }

    return true;
});

$(document).ready(function () {
    table;
    sAddDataTableButtonsBottom();
});

function sAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(table, {
        buttons: [
            {
                text: "Add", className: "addButton",
                action: function (e, dt, bt, config) { sAddRow(e, dt, bt, config); }
            },
            {
                text: "Copy", className: "copyButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { sCopyScenario(e, dt, bt, config); }
            },
            {
                text: "Delete", className: "deleteButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { sRemoveRow(e, dt, bt, config); }
            },
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { sEditRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#sButtonsBottom'));

    document.getElementById('sButtonsBottom').children[0].classList.remove('btn-group');

    sSetButtons('normal');
}

function sShowBadInputAlert() {
    var alertID = "#sBadInputAlert";
    $(alertID).html('Please check your data.Only numbers can be imported.');
    sShowAlert(alertID);
}

function sShowSuccessfullySaved(text) {
    var alertID = "#sSavedAlert";
    $(alertID).html(text);
    sShowAlert(alertID);
}

function sShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function sAddRow(e, dt, bt, config) {
    $.ajax({
        url: "/Scenario/Add",
        data: {
            ProjectID: document.getElementById("ProjectID").value
        }
    }).done(function (msg) {
        $("#sShowModal").html(msg);
        $("#sAddModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function sCopyScenario(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    $.ajax({
        type: "GET",
        url: "/Scenario/CopyScenario",
        data: {
            scenarioId: table.row(r).data()["id"]
        },
        dataType: 'json',
        success: function (data, status, xhr) {
            if (data.type === "Success") {
                sShowSuccessfullySaved('Scenario copied successfully');
                dt.draw();
            }
            else {
                sShowBadInputAlert('Scenario is not copied successfully');
            }
        },
        error: function (jqXhr, textStatus, errorThrown) {
            sShowBadInputAlert('Scenario is not copied successfully');
        }
    });
}

function sEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    $.ajax({
        url: "/Scenario/Edit",
        data: {
            id: table.row(r).data()["id"]
        }
    }).done(function (msg) {
        $("#sShowModal").html(msg);
        $("#sEditModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function sRemoveRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "/Scenario/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            sShowSuccessfullySaved('Record is successfully deleted');
            dt.draw();
        }
    });
}

function sSetButtons(mode) {
    table.buttons().enable(false);

    switch (mode) {
        case "edit":
            table.buttons([".editButton", ".copyButton"]).enable(true);
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

$('#scenarioTable tbody').on('click',
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

function format(d) {
    return '<table class="table-bordered" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px; width:100%">' +
        '<tr style="background: #e9ecef">' +        
        '<td><b>Calculation Time Step:</b></td>' +
        '<td>' +
        d.timeStepType +
        '</td>' +
        '<td><b>Data Time Step:</b></td>' +
        '<td>' +
        d.dataTimeStepType +
        '</td>' +
        '<td><b>Scenario Type:</b></td>' +
        '<td>' +
        d.scenarioType +
        '</td>' +
        '</tr>' +
        '<tr style="background: #e9ecef">' +
        '<td><b>Calculation Begins:</b></td>' +
        '<td>' +
        d.calculationBegins +
        '</td>' +
        '<td><b>Calculation Ends:</b></td>' +
        '<td>' +
        d.calculationEnds +
        '</td>' +       
        '<td></td>' +
        '<td></td>' +
        '</tr>' +
        '<tr style="background: #e9ecef">' +
        '<td colspan="6">' +
        ' <button type="button" class="btn btn-primary" onClick="window.open(\'/ChannelPolicyGroup/Index/' + d.id + '\');">Channel Policy Groups</button> ' +        
        ' <button type="button" class="btn btn-primary" onClick="window.open(\'/ChannelZone/Index/' + d.id + '\');">Channel Zones</button> ' +       
        ' <button type="button" class="btn btn-primary" onClick="window.open(\'/NodePolicyGroup/Index/' + d.id + '\');">Node Policy Groups</button> ' +
        ' <button type="button" class="btn btn-primary" onClick="window.open(\'/NodeZone/Index/' + d.id + '\');">Node Zones</button> ' +       
        '</td>' +
        '</tr>' +
        '</table>';
}