var table = $('#table').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "90vh",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "/NodePolicyGroup/GetAll",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "scenarioID": document.getElementById("ScenarioID").value
            });
        }
    },
    columnDefs: [
        {
            "targets": [0, 1, 2],
            "visible": false,
            "searchable": false
        }
    ],
    columns: [        
        { "data": "id" },
        { "data": "scenarioID" },
        { "data": "nodeTypeID" },
        { "data": "name" },
        { "data": "nodeType" },
        { "data": "numberOfZonesAboveIdealLevel" },
        { "data": "numberOfZonesBelowIdealLevel" },
        { "data": "zoneWeightsOffset" },
        {
            "data": "equalDeficits",
            "render": function (data) {
                return '<input type="checkbox" ' + (data === true ? 'checked' : '') + ' disabled />';
            }
        },
        {
            "data": "copyNodeLevelasFirstComponent",
            "render": function (data) {
                return '<input type="checkbox" ' + (data === true ? 'checked' : '') + ' disabled />';
            }
        }
    ],
    language: {
        "info": "Showing _TOTAL_ entries",
        "infoEmpty": "Showing 0 entries",
        "loadingRecords": "Please wait - loading..."
    },
    select: "single",
    stateSave: true,
    drawCallback: function (settings, json) {
        table.row(':eq(0)').select();
    }
}).on("user-select", function (e, dt, type, cell, originalEvent) {
    // I do not let the user deselect until done editing the row or editing is cancelled
    if ($("#table input[type=text]").length > 0) { return false; }

    return true;
});

$(document).ready(function () {
    table;
    npgAddDataTableButtonsBottom();
});

function npgAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(table, {
        buttons: [
            {
                text: "Add", className: "addButton",
                action: function (e, dt, bt, config) { npgAddRow(e, dt, bt, config); }
            },
            {
                text: "Delete", className: "deleteButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { npgRemoveRow(e, dt, bt, config); }
            },
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { npgEditRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#npgButtonsBottom'));

    document.getElementById('npgButtonsBottom').children[0].classList.remove('btn-group');

    npgSetButtons('normal');
}

function npgShowBadInputAlert() {
    var alertID = "#npgBadInputAlert";
    $(alertID).html('Please check your data.Only numbers can be imported.');
    npgShowAlert(alertID);
}

function npgShowSuccessfullySaved(text) {
    var alertID = "#npgSavedAlert";
    $(alertID).html(text);
    npgShowAlert(alertID);
}

function npgShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function npgAddRow(e, dt, bt, config) {
    $.ajax({
        url: "/NodePolicyGroup/Add",
        data: {
            ScenarioID: document.getElementById("ScenarioID").value
        }
    }).done(function (msg) {
        $("#npgDetailShowModal").html(msg);
        $("#npgAddModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function npgEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    $.ajax({
        url: "/NodePolicyGroup/Edit",
        data: {
            id: table.row(r).data()["id"]
        }
    }).done(function (msg) {
        $("#npgDetailShowModal").html(msg);
        $("#npgEditModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function npgRemoveRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();
    $.ajax({
        url: "/NodePolicyGroup/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            npgShowSuccessfullySaved('Record is successfully deleted');
            dt.draw();
        }
    });
}

function npgSetButtons(mode) {
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

function npgModalClose() {
    $('#npgModal').modal('hide');
}

table.on('select', function (e, dt, type, indexes) {
    if (type === 'row') {
        var r = $('#table > tbody > tr.selected').index();
        var previousRowId = $('#npgPreviousRow').val();
        if (r !== previousRowId) {
            $.ajax({
                type: "GET",
                url: "/NodePolicyGroup/NodePolicyGroupNode",
                data: {
                    nodePolicyGroupID: table.row(r).data()["id"]
                },
                success: function (data) {
                    $("#nodePolicyGroupNodeDiv").html(data);
                }
            });
        }
        $('#npgPreviousRow').val(r);
    }
});