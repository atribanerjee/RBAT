var importList;

var table = $('#channelTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "50vh",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "Channel/GetAll",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "channelTypeID": 0
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
        { "data": "channelTypeId" },
        { "data": "name" },        
        { "data": "channelTypeName" },        
        { "data": "percentReturnFlow" }        
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
    if ($("#channelTable input[type=text]").length > 0) { return false; }

    return true;
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

$(document).ready(function () {
    table;
    addDataTableButtonsBottom();
});

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
        url: "Channel/Add"
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
    $('#overlay').removeClass("d-none").addClass("d-block");

    $.ajax({
        url: "Channel/Edit",
        data: {
            id: table.row(r).data()["id"]
        }
    }).done(function (msg) {
        $("#showModal").html(msg);
        $("#myModal").modal({
            backdrop: 'static',
            keyboard: false
        });

        $('#overlay').removeClass("d-block").addClass("d-none");
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

function removeRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "Channel/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            showSuccessfullySaved('Record is successfully deleted');
            dt.draw();
        }
    });
}

$('#channelTable tbody').on('click',
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
    var tableDetails = '<table class="table-bordered" cellpadding="5" cellspacing="0" border="0" style="padding-left:50px; width:100%">' +
        '<tr style="background: #e9ecef">' +
        '<td colspan="2"></td>' +//<b>Recorded Flow Station:</b>
        //'<td>' +
        //d.recordedFlowStationName +
        //'</td>' +
        '<td><b>Description:</b></td>' +
        '<td>' +
        d.description +
        '</td>' +
        '<td><b>Upstream Node:</b></td>' +
        '<td>' +
        d.upstreamNodeName +
        '</td>' +
        '</tr>' +
        '<tr style="background: #e9ecef">' +
        '<td><b>Downstream Node:</b></td>' +
        '<td>' +
        d.downstreamNodeName +
        '</td>' +
        '<td><b>Upstream Node With Control Structure:</b></td>' +
        '<td>' +
        d.upstreamNodeWithControlStructureName +
        '</td>' +
        '<td><b>Upstream Channel With Control Structure:</b></td>' +
        '<td>' +
        d.upstreamChannelWithControlStructureName +
        '</td>' +
        '</tr>' +
        '<tr style="background: #e9ecef">';

    if (d.channelTypeId === 1 || d.channelTypeId === 2) {
        tableDetails = tableDetails +
            '<td><b>Overall Hydro Power Plant Efficiency:</b></td>' +
            '<td>' +
            d.overallHydroPowerPlantEfficiencyText +
            '</td>' +
            '<td><b>Constant Head Water Level:</b></td>' +
            '<td>' +
            d.constantHeadWaterLevelText +
            '</td>' +
            '<td><b>Upstream Reservoir Head Water Elevation:</b></td>' +
            '<td>' +
            d.upstreamReservoirHeadWaterElevationName +
            '</td>' +
            '</tr > ' +
            '<tr style="background: #e9ecef">' +
            '<td><b>Upstream Channel Head Water Elevation:</b></td>' +
            '<td>' +
            d.upstreamChannelHeadWaterElevationName +
            '</td>' +
            '<td><b>Constant Tail Water Level:</b></td>' +
            '<td>' +
            d.constantTailWaterLevelText +
            '</td>' +
            '<td><b>Downstream Reservoir Tail Water Elevation:</b></td>' +
            '<td>' +
            d.downstreamReservoirTailWaterElevationName +
            '</td>' +
            '</tr > ' +
            '<tr style="background: #e9ecef">' +
            '<td><b>Downstream Channel Tail Water Elevation:</b></td>' +
            '<td>' +
            d.downstreamChannelTailWaterElevationName +
            '</td>';
    }

    if (d.channelTypeId === 1) {
        tableDetails = tableDetails + '<td><b>Apportionment Flow Target:</b></td>' +
            '<td>' +
            d.apportionmentFlowTargetText +
            '</td>' +
            '<td><b>Reference Node:</b></td>' +
            '<td>' +
            d.referenceNodeName +
            '</td>' +
            '</tr>' +
            '<tr style="background: #e9ecef">' +
            '<td><b>Routing Coefficient A:</b></td>' +
            '<td>' +
            d.routingCoefficientAText +
            '</td>' +
            '<td><b>Routing Coefficient N:</b></td>' +
            '<td>' +
            d.routingCoefficientNText +
            '</td>' +
            '<td><b>Number Of Routing Phases:</b></td>' +
            '<td>' +
            d.numberOfRoutingPhasesText +
            '</td>' +
            '</tr>' +
            '<tr style="background: #e9ecef">'; 
    }
    else if (d.channelTypeId === 2) {
        tableDetails = tableDetails + '<td><b>Total Licensed Volume:</b></td>' +
            '<td>' +
            d.totalLicensedVolumeText +
            '</td>';
    }
    else {
        tableDetails = tableDetails + '<td><b>Return Flow Fraction:</b></td>' +
            '<td>' +
            d.percentReturnFlowText +
            '</td>';
    }

    tableDetails = tableDetails +
        '<td><b>Routing Option Use:</b></td>' +
        '<td>';

    if (d.routingOptionUse) {
        tableDetails = tableDetails + '<input type="checkbox" checked disabled />';
    }
    else {
        tableDetails = tableDetails + '<input type="checkbox" disabled />';
    }

    if (d.channelTypeId === 1) {
        tableDetails = tableDetails + '</td><td></td><td></td><td></td><td></td></tr>';
    }
    else if (d.channelTypeId === 2) {
        tableDetails = tableDetails + '</td></tr>';
    }
    else {
        tableDetails = tableDetails + '</td><td></td><td></td></tr>';
    }

    tableDetails = tableDetails + '<tr style="background: #e9ecef">' +
        '<td colspan="6">' +
        addDetailedButtons(d) +        
        '</td>' +
        '</tr>' +
        '</table>';

    return tableDetails;
}

function addDetailedButtons(d) {    
    var channelTypeID = d.channelTypeId;

    if (channelTypeID === 1) {
        return formatRiverReach(d);
    }
    else if (channelTypeID === 2) {
        return formatDiversionChannel(d);
    }
    else if (channelTypeID === 3) {
        return '';
    }
}

function formatRiverReach(d) {
    var buttons = ' <button type = "button" class="btn btn-primary" onClick = "openChannelTravelTime(' + d.id + ')" > Travel Time</button > ' +
                  ' <button type="button" class="btn btn-primary" onClick="openChannelOutflowCapacity(' + d.id + ')">Outflow Capacity</button>';

    //if (d.recordedFlowStationID !== null)
    //    buttons = buttons + ' <button type="button" class="btn btn-primary" onClick="openRecordedFlowStationData(' + d.recordedFlowStationID + ')">Recorded Flow </button>';
    //else
    //    buttons = buttons + ' <button type="button" class="btn btn-primary" disabled>Recorded Flow </button>';

    return buttons;
}

function formatDiversionChannel(d) {
    var buttons = ' <button type="button" class="btn btn-primary" onClick="openChannelOutflowCapacity(' + d.id + ')">Outflow Capacity</button>';

    //if (d.recordedFlowStationID !== null)
    //    buttons = buttons + ' <button type="button" class="btn btn-primary" onClick="openRecordedFlowStationData(' + d.recordedFlowStationID + ')">Recorded Flow </button>';
    //else
    //    buttons = buttons + ' <button type="button" class="btn btn-primary" disabled>Recorded Flow </button>';

    return buttons;
}

//function formatReturnFlow(d) {
//    var buttons;

//    if (d.recordedFlowStationID !== null)
//        buttons = ' <button type="button" class="btn btn-primary" onClick="openRecordedFlowStationData(' + d.recordedFlowStationID + ')">Recorded Flow </button>';
//    else
//        buttons = ' <button type="button" class="btn btn-primary" disabled>Recorded Flow </button>';

//    return buttons;
//}

function openChannelOutflowCapacity(channelID) {           
    $.ajax({
        url: "ChannelOutflowCapacity/Index",
        data: { channelID: channelID }
    }).done(function (msg) {
        $("#cocShowModal").html(msg);
        $("#cocModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function openChannelTravelTime(channelID) {
    $.ajax({
        url: "ChannelTravelTime/Index",
        data: { channelID: channelID }
    }).done(function (msg) {
        $("#cttShowModal").html(msg);
        $("#cttModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

//function openRecordedFlowStationData(recordedFlowStationID) {
//    $.ajax({
//        url: "TimeRecordedFlow/Index",
//        data: { recordedFlowStationID: recordedFlowStationID }
//    }).done(function (msg) {
//        $("#tsdShowModal").html(msg);
//        $("#tsdModal").modal({
//            backdrop: 'static',
//            keyboard: false
//        });
//    });
//}

Date.prototype.toDateInputValue = (function () {
    var local = new Date(this);
    local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
    return local.toJSON().slice(0, 10);
});