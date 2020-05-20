var table = $('#table').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "50vh",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "/ChannelPolicyGroup/GetAll",
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
        { "data": "channelTypeID" },
        { "data": "name" },
        { "data": "channelType" },
        { "data": "numberOfZonesAboveIdealLevel" },
        { "data": "numberOfZonesBelowIdealLevel" },
        { "data": "zoneWeightsOffset" }
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
    cpgAddDataTableButtonsBottom();
});

function cpgAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(table, {
        buttons: [
            {
                text: "Add", className: "addButton",
                action: function (e, dt, bt, config) { cpgAddRow(e, dt, bt, config); }
            },
            {
                text: "Delete", className: "deleteButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { cpgRemoveRow(e, dt, bt, config); }
            },
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { cpgEditRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#cpgButtonsBottom'));

    document.getElementById('cpgButtonsBottom').children[0].classList.remove('btn-group');

    cpgSetButtons('normal');
}

function cpgShowBadInputAlert() {
    var alertID = "#cpgBadInputAlert";
    $(alertID).html('Please check your data.Only numbers can be imported.');
    cpgShowAlert(alertID);
}

function cpgShowSuccessfullySaved(text) {
    var alertID = "#cpgSavedAlert";
    $(alertID).html(text);
    cpgShowAlert(alertID);
}

function cpgShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function cpgAddRow(e, dt, bt, config) {
    $.ajax({
        url: "/ChannelPolicyGroup/Add",
        data: {
            ScenarioID: document.getElementById("ScenarioID").value
        }
    }).done(function (msg) {
        $("#cpgDetailShowModal").html(msg);
        $("#cpgAddModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function cpgEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    $.ajax({
        url: "/ChannelPolicyGroup/Edit",
        data: {
            id: table.row(r).data()["id"]
        }
    }).done(function (msg) {
        $("#cpgDetailShowModal").html(msg);
        $("#cpgEditModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function cpgRemoveRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();
    $.ajax({
        url: "/ChannelPolicyGroup/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            cpgShowSuccessfullySaved('Record is successfully deleted');
            dt.draw();
        }
    });
}

function cpgSetButtons(mode) {
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

function cpgModalClose() {
    $('#cpgModal').modal('hide');
}

table.on('select', function (e, dt, type, indexes) {
    if (type === 'row') {
        var r = $('#table > tbody > tr.selected').index();
        var previousRowId = $('#cpgPreviousRow').val();
        if (r !== previousRowId) {
            $.ajax({
                type: "GET",
                url: "/ChannelPolicyGroup/ChannelPolicyGroupChannel",
                data: {
                    channelPolicyGroupID: table.row(r).data()["id"]
                },
                success: function (data) {
                    $("#channelPolicyGroupChannelDiv").html(data);
                }
            });
        }
        $('#cpgPreviousRow').val(r);
    }
});