var czlImportList;
var aboveIdealZone = Number(document.getElementById("czlNumberOfZonesAbove").value);
var belowIdealZone = Number(document.getElementById("czlNumberOfZonesBelow").value); 

var czlTable = $('#czlTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollX: true,
    scrollY: "200px",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "/ChannelZoneLevel/FillGridFromDB",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "channelPolicyGroupID": document.getElementById("czlChannelPolicyGroupID").value,
                "channelID": document.getElementById("czlChannelID").value,
                "importList": czlImportList
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
        { "data": "year" },
        { "data": "timeComponentValue" },
        { "data": "zoneAboveIdeal6", "defaultContent": "" },
        { "data": "zoneAboveIdeal5", "defaultContent": "" },
        { "data": "zoneAboveIdeal4", "defaultContent": "" },
        { "data": "zoneAboveIdeal3", "defaultContent": "" },
        { "data": "zoneAboveIdeal2", "defaultContent": "" },
        { "data": "zoneAboveIdeal1", "defaultContent": "" },
        { "data": "idealZone", "defaultContent": "" },
        { "data": "zoneBelowIdeal1", "defaultContent": "" },
        { "data": "zoneBelowIdeal2", "defaultContent": "" },
        { "data": "zoneBelowIdeal3", "defaultContent": "" },
        { "data": "zoneBelowIdeal4", "defaultContent": "" },
        { "data": "zoneBelowIdeal5", "defaultContent": "" },
        { "data": "zoneBelowIdeal6", "defaultContent": "" },
        { "data": "zoneBelowIdeal7", "defaultContent": "" },
        { "data": "zoneBelowIdeal8", "defaultContent": "" },
        { "data": "zoneBelowIdeal9", "defaultContent": "" },
        { "data": "zoneBelowIdeal10", "defaultContent": "" }
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
    if ($("#czlTable input[type=text]").length > 0 || !$("#channelZoneLevelDiv .saveButton").hasClass("disabled")) { return false; }

    return true;
});

function czlAddDataTableButtons() {
    new $.fn.dataTable.Buttons(czlTable, {
        buttons: [
            {
                extend: 'collection',
                text: 'Export', className: "exportButton",
                buttons: [
                    'copy',
                    'excel',
                    'csv',
                    'pdf',
                    'print'
                ]
            },
            {
                text: 'Paste from Clipboard', className: "pasteButton",
                action: function (e, dt, node, config) {
                    czlPasteFromClipboard();
                }
            },
            {
                text: 'Save All', className: "saveButton",
                action: function (e, dt, node, config) {
                    czlSaveTable();
                }
            }
        ]
    }).container().appendTo($('#czlButtons'));

    document.getElementById('czlButtons').children[0].classList.remove('btn-group');
}

function czlAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(czlTable, {
        buttons: [
            //{
            //    text: "Add", className: "addButton",
            //    action: function (e, dt, bt, config) { czlAddRow(e, dt, bt, config); }
            //},
            {
                text: "Delete", className: "deleteButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { czlRemoveRow(e, dt, bt, config); }
            },
            {
                text: "Delete All", className: "deleteAllButton",
                action: function (e, dt, bt, config) { czlRemoveAllRows(e, dt, bt, config); }
            },
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { czlEditRow(e, dt, bt, config); }
            },
            {
                text: "Update", className: "updateButton",
                action: function (e, dt, bt, config) { czlSaveRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#czlButtonsBottom'));

    document.getElementById('czlButtonsBottom').children[0].classList.remove('btn-group');

    czlSetButtons('normal');
}

$(document).ready(function () {
    czlSetColumnsVisibility();
    czlAddDataTableButtons();
    czlAddDataTableButtonsBottom();    
    setVisibleChannelZoneLevelRecordedFlowStation();
    enableDisableRecordedFlowStation();
});

function czlSetColumnsVisibility() {
    czlSetAllColumnsVisible();
    czlHideNotUsedZoneLevels();
    $("#czlTableWrapper").show();
}

function czlSetAllColumnsVisible() {
    for (var i = 1; i <= 19; i++) {
        var column = czlTable.column(i);
        column.visible(true);
    }
}

function czlHideNotUsedZoneLevels() {
    czlHideNotUsedZoneLevelsAboveIdeal();
    czlHideNotUsedZoneLevelsBelowIdeal();
}

function czlHideNotUsedZoneLevelsAboveIdeal() {
    for (var i = 3; i <= 8 - aboveIdealZone; i++) {
        var column = czlTable.column(i);
        column.visible(false);
    }
}

function czlHideNotUsedZoneLevelsBelowIdeal() {
    for (var i = 10 + belowIdealZone; i <= 19; i++) {
        var column = czlTable.column(i);
        column.visible(false);
    }
}

async function czlPasteFromClipboard() {
    var pasteList = await czlConvertClipboardDataToDoubleArray();

    if (czlValidatePastedList(pasteList)) {
        $.ajax({
            url: "/ChannelZoneLevel/GetDateFromClipboard",
            data: {
                channelPolicyGroupID: document.getElementById("czlChannelPolicyGroupID").value,
                channelID: document.getElementById("czlChannelID").value,
                existingList: czlTable.data().toArray(),
                pasteList: pasteList
            },
            type: "POST",
            success: function (response) {
                if (!response.error) {
                    czlReloadTableAfterClipboardPaste(response);
                    czlSetButtons('paste');
                }
            }
        });
    }
}

function czlValidatePastedList(pasteList) {
    if (pasteList === null) {
        czlShowBadInputAlert('Please check your data. Looks like you didn\'t copy anything');
        return false;
    }

    return czlListHasExpectedNumberOfColumns(pasteList) &&
           czlCheckIfPasteListContainsAllNumbers(pasteList) &&
           czlCheckIfJulianDayIncreases(pasteList) &&
           czlCheckIfZonesValuesDecrease(pasteList);
}

function czlListHasExpectedNumberOfColumns(paste) {
    for (i = 0; i < paste.length; i++) { 
        if (paste[i].ZoneLevelsAboveIdeal.length + paste[i].ZoneLevelsBelowIdeal.length + paste[i].IdealZone.length + 2 !== aboveIdealZone + belowIdealZone + 3) {
            czlShowBadInputAlert('Please check your data. Copied number of columns must be equal to the grid number of columns on the form');
            return false;
        }
    }

    return true;
}

function czlCheckIfPasteListContainsAllNumbers(arrayDouble) {
    if (!arrayDouble.every(czlGetArrayFromObject)) {
        czlShowBadInputAlert('Please check your data. Only numbers can be imported.');
        return false;
    }
    
    return true;
}

function czlCheckIfJulianDayIncreases(paste) {
    for (i = 0; i < paste.length - 1; i++) {
        if (paste[i].Year === paste[i + 1].Year) {
            if (paste[i].TimeComponentValue >= paste[i + 1].TimeComponentValue) {
                czlShowBadInputAlert('Please check your data. Julian Day must be in increasing order');
                return false;
            }
        } else if (paste[i].Year > paste[i + 1].Year) {
            czlShowBadInputAlert('Please check your data. Julian Day must be in increasing order');
            return false;
        }
    }

    return true;
}

function czlCheckIfZonesValuesDecrease(paste) {
    for (i = 0; i < paste.length; i++) {
        for (j = 0; j < paste[i].ZoneLevelsAboveIdeal.length - 1; j++) {
            if (paste[i].ZoneLevelsAboveIdeal[j] < paste[i].ZoneLevelsAboveIdeal[j + 1]) {
                czlShowBadInputAlert('Please check your data. Zone Level values must be monotonous declining');
                return false;
            }
        }

        if (paste[i].ZoneLevelsAboveIdeal[aboveIdealZone - 1] < paste[i].IdealZone ||
            paste[i].IdealZone < paste[i].ZoneLevelsBelowIdeal[0]) {
            czlShowBadInputAlert('Please check your data. Zone Level values must be monotonous declining');
            return false;
        }

        for (j = 0; j < paste[i].ZoneLevelsBelowIdeal.length - 1; j++) {
            if (paste[i].ZoneLevelsBelowIdeal[j] < paste[i].ZoneLevelsBelowIdeal[j + 1]) {
                czlShowBadInputAlert('Please check your data. Zone Level values must be monotonous declining');
                return false;
            }
        } 
    }

    return true;
}

async function czlConvertClipboardDataToDoubleArray() {
    var clipboardText = await navigator.clipboard.readText();
    clipboardText = clipboardText.replace(/\r\n([^\r\n]*)$/, '$1');
    var clipboardTextRows = clipboardText.split("\r\n");

    var data = [];
    for (var i = 0; i < clipboardTextRows.length; i++) {
        var clipboardTextColumns = clipboardTextRows[i].split("\t").map(parseFloat);

        data.push(
            {                
                Year: clipboardTextColumns[0],
                TimeComponentValue: clipboardTextColumns[1],
                ZoneLevelsAboveIdeal: czlCreateArrayOfZoneLevelsAboveIdeal(clipboardTextColumns),
                IdealZone: [clipboardTextColumns[aboveIdealZone+2]],
                ZoneLevelsBelowIdeal: czlCreateArrayOfZoneLevelsBelowIdeal(clipboardTextColumns)
            }
        );
    }

    return data;
}

function czlCreateArrayOfZoneLevelsAboveIdeal(clipboardTextColumns) {
    var columnZoneLevelsAboveIdeal = [];
    for (var j = 2; j <= aboveIdealZone + 1; j++) {
        columnZoneLevelsAboveIdeal.push(clipboardTextColumns[j]);
    }
    return columnZoneLevelsAboveIdeal;
}

function czlCreateArrayOfZoneLevelsBelowIdeal(clipboardTextColumns) {
    var columnZoneLevelsBelowIdeal = [];
    for (var j = aboveIdealZone + 3; j < clipboardTextColumns.length; j++) {
        columnZoneLevelsBelowIdeal.push(clipboardTextColumns[j]);
    }
    return columnZoneLevelsBelowIdeal;
}

function czlGetArrayFromObject(n) {
    if (!n.ZoneLevelsAboveIdeal.every(czlIsNumber) || !n.IdealZone.every(czlIsNumber) || !n.ZoneLevelsBelowIdeal.every(czlIsNumber) ||
        !czlIsNumber(n.Year) || !czlIsNumber(n.TimeComponentValue)) { 
        return false;
    }
    else {
        return true;
    } 
}

function czlIsNumber(n) {
    return !isNaN(parseFloat(n)) && !isNaN(n - 0);
}

function czlSaveTable() {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: "/ChannelZoneLevel/SaveAll",
        data: {
            channelPolicyGroupID: document.getElementById("czlChannelPolicyGroupID").value,
            channelID: document.getElementById("czlChannelID").value,
            listToSave: czlTable.data().toArray()
        },
        success: function (response) {
            czlShowSuccessfullySaved('Changes are successfully saved');
            czlSetButtons('normal');
            czlImportList = null;
            czlTable.draw();
        }
    });
}

function czlShowBadInputAlert(text) {
    var alertID = "#czlBadInputAlert";
    $(alertID).html(text);
    czlShowAlert(alertID);
}

function czlShowSuccessfullySaved(text) {
    var alertID = "#czlSavedAlert";
    $(alertID).html(text);
    czlShowAlert(alertID);
}

function czlShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function czlReloadTableAfterClipboardPaste(response) {
    czlImportList = response.list;
    czlTable.draw();
}

//function czlAddRow(e, dt, bt, config) {
//    $.ajax({
//        url: "/ChannelZoneLevel/Add",
//        data: {
//            channelPolicyGroupID: document.getElementById("czlChannelPolicyGroupID").value
//        }
//    }).done(function (msg) {
//        $("#czlDetailShowModal").html(msg);
//        $("#czlAddModal").modal({
//            backdrop: 'static',
//            keyboard: false
//        });
//    });
//}

function czlEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($(czlTable.buttons(".editButton")[0].node).find("span").text() === "Cancel") {
        $(r).children("td").each(function (i, it) {
            var od = dt.cells(it).data()[0];
            $(it).html(od);
        });
        czlSetButtons('cancel');
    }
    else {
        $(r).children("td").each(function (i, it) {
            var h = $("<input type='text'>");
            h.val(it.innerText);
            $(it).html(h);
        });
        czlSetButtons('edit');
    }
}

function czlSetButtons(mode) {
    czlTable.buttons().enable(false);
    czlTable.buttons([".buttons-copy", ".buttons-excel", ".buttons-csv", ".buttons-pdf", ".buttons-print"]).enable(true);

    switch (mode) {
        case "paste":
            czlTable.buttons([".exportButton", ".pasteButton", ".saveButton"]).enable(true);
            break;
        case "edit":
            czlTable.buttons([".editButton", ".updateButton"]).enable(true);
            $(czlTable.buttons(".editButton")[0].node).find("span").text('Cancel');
            break;
        case "cancel":
        case "normal":
            czlTable.buttons([".editButton", ".deleteButton", ".deleteAllButton", ".exportButton", ".pasteButton"]).enable(true);
            $(czlTable.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
    }
}

function czlSaveRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($("input[type=text]", r).length === 0) { return; }

    $(r).children("td").each(function (i, it) {
        var di = $("input", it).val();
        $(it).html(di);
        var cell = czlTable.cell(it._DT_CellIndex.row, it._DT_CellIndex.column);
        cell.data(di);
    });

    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "/ChannelZoneLevel/Update",
        method: 'POST',
        data: { listToUpdate: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            czlShowSuccessfullySaved('Record is successfully saved');
            dt.draw();
        }
    });

    czlSetButtons('normal');
}

function czlRemoveRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "/ChannelZoneLevel/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            czlShowSuccessfullySaved('Record is successfully deleted');
            dt.draw();
        }
    });
}

function czlRemoveAllRows(e, dt, bt, config) {
    if (czlTable.data().any()) {
        if (confirm("Are you sure you want to delete?")) {
            $.ajax({
                url: "/ChannelZoneLevel/Remove",
                method: 'POST',
                data: { listToRemove: czlTable.data().toArray() },
                dataType: 'json',
                success: function (data, status, xhr) {
                    czlShowSuccessfullySaved('Record is successfully deleted');
                    dt.draw();
                } 
            });
        }
    }
}

function setVisibleChannelZoneLevelRecordedFlowStation() {
    var numberOfZonesAvailable = aboveIdealZone + belowIdealZone + 1;

    if (numberOfZonesAvailable === 2) {
        document.getElementById("Zone3Id").hidden = false;
        document.getElementById("RecordedFlowStation3Id").hidden = false;
    } else if (numberOfZonesAvailable >= 3) {
        document.getElementById("Zone2Id").hidden = false;
        document.getElementById("RecordedFlowStation2Id").hidden = false;
        document.getElementById("Zone3Id").hidden = false;
        document.getElementById("RecordedFlowStation3Id").hidden = false;
    }
}

function enableDisableRecordedFlowStation() {
    document.getElementById("RecordedFlowStation1Id").disabled = document.getElementById("Zone1Id").value === '';
    document.getElementById("RecordedFlowStation2Id").disabled = document.getElementById("Zone2Id").value === '';
    document.getElementById("RecordedFlowStation3Id").disabled = document.getElementById("Zone3Id").value === '';
}

function zoneOrRecordedFlowStationChanged() {    
    $.ajax({
        url: "/ChannelZoneLevelRecordedFlowStation/Save",
        method: 'POST',
        data: {
            "channelPolicyGroupId": document.getElementById("czlChannelPolicyGroupID").value,
            "channelId": document.getElementById("czlChannelID").value,
            "zone1Id": document.getElementById("Zone1Id").value,
            "zone2Id": document.getElementById("Zone2Id").value,
            "zone3Id": document.getElementById("Zone3Id").value,
            "recordedFlowStation1Id": document.getElementById("RecordedFlowStation1Id").value,
            "recordedFlowStation2Id": document.getElementById("RecordedFlowStation2Id").value,
            "recordedFlowStation3Id": document.getElementById("RecordedFlowStation3Id").value
        },
        dataType: 'json',
        success: function () {
            enableDisableRecordedFlowStation();
        }
    });
}