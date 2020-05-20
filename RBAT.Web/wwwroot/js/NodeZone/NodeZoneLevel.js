var nzlImportList;
var aboveIdealZone = Number(document.getElementById("nzlNumberOfZonesAbove").value);
var belowIdealZone = Number(document.getElementById("nzlNumberOfZonesBelow").value);
var isConsumptiveUseNode = document.getElementById('nzlNodeTypeID').value === '2'; //Consumptive Use Node

var nzlTable = $('#nzlTable').DataTable({
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
        "url": "/NodeZoneLevel/FillGridFromDB",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "nodePolicyGroupID": document.getElementById("nzlNodePolicyGroupID").value,
                "nodeID": document.getElementById("nzlNodeID").value,
                "importList": nzlImportList
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
    stateSave: true,
    initComplete: function (settings, json) {
        nzlSetVisibleDataTableButtons(nzlTable.rows().count() === 0);
    }
}).on("user-select", function (e, dt, type, cell, originalEvent) {
    // I do not let the user deselect until done editing the row or editing is cancelled
    if ($("#nzlTable input[type=text]").length > 0 || !$("#nodeZoneLevelDiv .saveButton").hasClass("disabled")) { return false; }

    return true;
});

function nzlAddDataTableButtons() {
    new $.fn.dataTable.Buttons(nzlTable, {
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
                    nzlPasteFromClipboard();
                }
            },
            {
                text: 'Save All', className: "saveButton",
                action: function (e, dt, node, config) {
                    nzlSaveTable();
                }
            }
        ]
    }).container().appendTo($('#nzlButtons'));

    document.getElementById('nzlButtons').children[0].classList.remove('btn-group');
}

function nzlAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(nzlTable, {
        buttons: [
            //{
            //    text: "Add", className: "addButton",
            //    action: function (e, dt, bt, config) { nzlAddRow(e, dt, bt, config); }
            //},
            {
                text: "Delete", className: "deleteButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { nzlRemoveRow(e, dt, bt, config); }
            },
            {
                text: "Delete All", className: "deleteAllButton",
                action: function (e, dt, bt, config) { nzlRemoveAllRows(e, dt, bt, config); }
            },
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { nzlEditRow(e, dt, bt, config); }
            },
            {
                text: "Update", className: "updateButton",
                action: function (e, dt, bt, config) { nzlSaveRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#nzlButtonsBottom'));

    document.getElementById('nzlButtonsBottom').children[0].classList.remove('btn-group');

    nzlSetButtons('normal');
}

function nzlSetVisibleDataTableButtons(visible) {
    if (isConsumptiveUseNode) {
        if (visible) {
            document.getElementById('nzlButtons').style.display = "block";
        }
        else {
            document.getElementById('nzlButtons').style.display = "none";
        }
    }
}

$(document).ready(function () {
    nzlSetColumnsVisibility();
    nzlAddDataTableButtons();
    nzlAddDataTableButtonsBottom();
    if (document.getElementById("nzlNodeTypeID").value === '1') {
        document.getElementById("divUseHistoricReservoirLevels").style.visibility = "visible";
    }
});

function nzlSetColumnsVisibility() {
    nzlSetAllColumnsVisible();
    nzlHideNotUsedZoneLevels();
    nzlHideColumnsForConsumptiveUse();
    $("#nzlTableWrapper").show();
}

function nzlSetAllColumnsVisible() {
    for (var i = 1; i <= 18; i++) {
        var column = nzlTable.column(i);
        column.visible(true);
    }
}

function nzlHideNotUsedZoneLevels() {
    nzlHideNotUsedZoneLevelsAboveIdeal();
    nzlHideNotUsedZoneLevelsBelowIdeal();
}

function nzlHideNotUsedZoneLevelsAboveIdeal() {
    for (var i = 3; i <= 8 - aboveIdealZone; i++) {
        var column = nzlTable.column(i);
        column.visible(false);
    }
}

function nzlHideNotUsedZoneLevelsBelowIdeal() {
    for (var i = 9 + belowIdealZone; i <= 18; i++) {
        var column = nzlTable.column(i);
        column.visible(false);
    }
}

function nzlHideColumnsForConsumptiveUse() {    
    nzlTable.column(1).visible(!isConsumptiveUseNode);
    nzlTable.column(2).visible(!isConsumptiveUseNode);
}

async function nzlPasteFromClipboard() {
    var pasteList = await nzlConvertClipboardDataToDoubleArray();

    if (nzlValidatePastedList(pasteList)) {
        $.ajax({
            url: "/NodeZoneLevel/GetDateFromClipboard",
            data: {
                nodePolicyGroupID: document.getElementById("nzlNodePolicyGroupID").value,
                nodeID: document.getElementById("nzlNodeID").value,
                nodeTypeID: document.getElementById("nzlNodeTypeID").value,
                existingList: nzlTable.data().toArray(),
                pasteList: pasteList
            },
            type: "POST",
            success: function (response) {
                if (!response.error) {
                    nzlReloadTableAfterClipboardPaste(response);
                    nzlSetButtons('paste');
                    nzlTable.off('draw');
                }
            }
        });
    }
}

function nzlValidatePastedList(pasteList) {
    if (pasteList === null) {
        nzlShowBadInputAlert('Please check your data. Looks like you didn\'t copy anything');
        return false;
    }

    if (isConsumptiveUseNode) {
        return nzlListHasOnlyOneRow(pasteList) &&
            nzlListHasExpectedNumberOfColumnsExcludingYearAndJulianDay(pasteList) &&
            nzlCheckIfPasteListContainsAllNumbers(pasteList);
    }

    return nzlListHasExpectedNumberOfColumns(pasteList) &&
           nzlCheckIfPasteListContainsAllNumbers(pasteList) &&
           nzlCheckIfJulianDayIncreases(pasteList) &&
           nzlCheckIfZonesValuesDecrease(pasteList);
}

function nzlListHasOnlyOneRow(paste) {
    if (paste.length !== 1) {
        nzlShowBadInputAlert('Please check your data. Only one row of data must be copied');
        return false;
    }

    return true;
}

function nzlListHasExpectedNumberOfColumnsExcludingYearAndJulianDay(paste) {
    for (i = 0; i < paste.length; i++) {
        if (paste[i].ZoneLevelsBelowIdeal.length !== belowIdealZone) {
            nzlShowBadInputAlert('Please check your data. Copied number of columns must be equal to the grid number of columns on the form');
            return false;
        }
    }

    return true;
}

function nzlListHasExpectedNumberOfColumns(paste) {
    for (i = 0; i < paste.length; i++) { 
        if (paste[i].ZoneLevelsAboveIdeal.length + paste[i].ZoneLevelsBelowIdeal.length + 2 !== aboveIdealZone + belowIdealZone + 2) {
            nzlShowBadInputAlert('Please check your data. Copied number of columns must be equal to the grid number of columns on the form');
            return false;
        }
    }

    return true;
}

function nzlCheckIfPasteListContainsAllNumbers(arrayDouble) {
    if (!arrayDouble.every(nzlGetArrayFromObject)) {
        nzlShowBadInputAlert('Please check your data. Only numbers can be imported.');
        return false;
    }
    
    return true;
}

function nzlCheckIfJulianDayIncreases(paste) {
    for (i = 0; i < paste.length - 1; i++) {
        if (paste[i].Year === paste[i + 1].Year) {
            if (paste[i].TimeComponentValue >= paste[i + 1].TimeComponentValue) {
                nzlShowBadInputAlert('Please check your data. Julian Day must be in increasing order');
                return false;
            }
        } else if (paste[i].Year > paste[i + 1].Year) {
            nzlShowBadInputAlert('Please check your data. Julian Day must be in increasing order');
            return false;
        }
    }

    return true;
}

function nzlCheckIfZonesValuesDecrease(paste) {
    for (i = 0; i < paste.length; i++) {
        for (j = 0; j < paste[i].ZoneLevelsAboveIdeal.length - 1; j++) {
            if (paste[i].ZoneLevelsAboveIdeal[j] < paste[i].ZoneLevelsAboveIdeal[j + 1]) {
                nzlShowBadInputAlert('Please check your data. Zone Level values must be monotonous declining');
                return false;
            }
        }

        if (paste[i].ZoneLevelsAboveIdeal[aboveIdealZone - 1] < paste[i].ZoneLevelsBelowIdeal[0]) {
            nzlShowBadInputAlert('Please check your data. Zone Level values must be monotonous declining');
            return false;
        }

        for (j = 0; j < paste[i].ZoneLevelsBelowIdeal.length - 1; j++) {
            if (paste[i].ZoneLevelsBelowIdeal[j] < paste[i].ZoneLevelsBelowIdeal[j + 1]) {
                nzlShowBadInputAlert('Please check your data. Zone Level values must be monotonous declining');
                return false;
            }
        } 
    }

    return true;
}

async function nzlConvertClipboardDataToDoubleArray() {
    var clipboardText = await navigator.clipboard.readText();
    clipboardText = clipboardText.replace(/\r\n([^\r\n]*)$/, '$1');
    var clipboardTextRows = clipboardText.split("\r\n");

    var data = [];
    for (var i = 0; i < clipboardTextRows.length; i++) {
        var clipboardTextColumns = clipboardTextRows[i].split("\t").map(parseFloat);

        if (isConsumptiveUseNode) {
            data.push({
                ZoneLevelsBelowIdeal: nzlCreateArrayOfZoneLevelsBelowIdealForConsumptiveUse(clipboardTextColumns)
            });
        } else {
            data.push({                
                Year: clipboardTextColumns[0],
                TimeComponentValue: clipboardTextColumns[1],
                ZoneLevelsAboveIdeal: nzlCreateArrayOfZoneLevelsAboveIdeal(clipboardTextColumns),
                ZoneLevelsBelowIdeal: nzlCreateArrayOfZoneLevelsBelowIdeal(clipboardTextColumns)
            });
        }
    }

    return data;
}

function nzlCreateArrayOfZoneLevelsBelowIdealForConsumptiveUse(clipboardTextColumns) {
    var columnZoneLevelsBelowIdeal = [];
    for (var j = aboveIdealZone; j < clipboardTextColumns.length; j++) {
        columnZoneLevelsBelowIdeal.push(clipboardTextColumns[j]);
    }
    return columnZoneLevelsBelowIdeal;
}

function nzlCreateArrayOfZoneLevelsAboveIdeal(clipboardTextColumns) {
    var columnZoneLevelsAboveIdeal = [];
    for (var j = 2; j <= aboveIdealZone + 1; j++) {
        columnZoneLevelsAboveIdeal.push(clipboardTextColumns[j]);
    }
    return columnZoneLevelsAboveIdeal;
}

function nzlCreateArrayOfZoneLevelsBelowIdeal(clipboardTextColumns) {
    var columnZoneLevelsBelowIdeal = [];
    for (var j = aboveIdealZone + 2; j < clipboardTextColumns.length; j++) {
        columnZoneLevelsBelowIdeal.push(clipboardTextColumns[j]);
    }
    return columnZoneLevelsBelowIdeal;
}

function nzlGetArrayFromObject(n) {
    if (isConsumptiveUseNode) {
        if (!n.ZoneLevelsBelowIdeal.every(nzlIsNumber)) {
            return false;
        }
    }
    else if (!n.ZoneLevelsAboveIdeal.every(nzlIsNumber) || !n.ZoneLevelsBelowIdeal.every(nzlIsNumber) ||
        !nzlIsNumber(n.Year) || !nzlIsNumber(n.TimeComponentValue)) { 
        return false;
    }
   
    return true;   
}

function nzlIsNumber(n) {
    return !isNaN(parseFloat(n)) && !isNaN(n - 0);
}

function nzlSaveTable() {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: "/NodeZoneLevel/SaveAll",
        data: {
            nodePolicyGroupID: document.getElementById("nzlNodePolicyGroupID").value,
            nodeID: document.getElementById("nzlNodeID").value,
            listToSave: nzlTable.data().toArray()
        },
        success: function (response) {
            nzlShowSuccessfullySaved('Changes are successfully saved');
            nzlSetButtons('normal');
            nzlImportList = null;
            nzlSetVisibleDataTableButtons(nzlTable.rows().count() === 0);
            nzlTable.draw();
        }
    });
}

function nzlShowBadInputAlert(text) {
    var alertID = "#nzlBadInputAlert";
    $(alertID).html(text);
    nzlShowAlert(alertID);
}

function nzlShowSuccessfullySaved(text) {
    var alertID = "#nzlSavedAlert";
    $(alertID).html(text);
    nzlShowAlert(alertID);
}

function nzlShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function nzlReloadTableAfterClipboardPaste(response) {
    nzlImportList = response.list;
    nzlTable.draw();
}

//function nzlAddRow(e, dt, bt, config) {
//    $.ajax({
//        url: "/NodeZoneLevel/Add",
//        data: {
//            nodePolicyGroupID: document.getElementById("nzlNodePolicyGroupID").value
//        }
//    }).done(function (msg) {
//        $("#nzlDetailShowModal").html(msg);
//        $("#nzlAddModal").modal({
//            backdrop: 'static',
//            keyboard: false
//        });
//    });
//}

function nzlEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($(nzlTable.buttons(".editButton")[0].node).find("span").text() === "Cancel") {
        $(r).children("td").each(function (i, it) {
            var od = dt.cells(it).data()[0];
            $(it).html(od);
        });
        nzlSetButtons('cancel');
    }
    else {
        $(r).children("td").each(function (i, it) {
            var h = $("<input type='text'>");
            h.val(it.innerText);
            $(it).html(h);
        });
        nzlSetButtons('edit');
    }
}

function nzlSetButtons(mode) {
    nzlTable.buttons().enable(false);
    nzlTable.buttons([".buttons-copy", ".buttons-excel", ".buttons-csv", ".buttons-pdf", ".buttons-print"]).enable(true);

    switch (mode) {
        case "paste":
            nzlTable.buttons([".exportButton", ".pasteButton", ".saveButton"]).enable(true);
            break;
        case "edit":
            nzlTable.buttons([".editButton", ".updateButton"]).enable(true);
            $(nzlTable.buttons(".editButton")[0].node).find("span").text('Cancel');
            break;
        case "cancel":
        case "normal":
            nzlTable.buttons([".editButton", ".deleteButton", ".deleteAllButton", ".exportButton", ".pasteButton"]).enable(true);
            $(nzlTable.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
    }
}

function nzlSaveRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($("input[type=text]", r).length === 0) { return; }

    $(r).children("td").each(function (i, it) {
        var di = $("input", it).val();
        $(it).html(di);
        var cell = nzlTable.cell(it._DT_CellIndex.row, it._DT_CellIndex.column);
        cell.data(di);
    });

    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "/NodeZoneLevel/Update",
        method: 'POST',
        data: { listToUpdate: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            nzlShowSuccessfullySaved('Record is successfully saved');
            dt.draw();
        }
    });

    nzlSetButtons('normal');
}

function nzlRemoveRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "/NodeZoneLevel/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            nzlShowSuccessfullySaved('Record is successfully deleted');
            dt.draw();
        }
    });
}

function nzlRemoveAllRows(e, dt, bt, config) {
    if (nzlTable.data().any()) {
        if (confirm("Are you sure you want to delete?")) {
            $.ajax({
                url: "/NodeZoneLevel/Remove",
                method: 'POST',
                data: { listToRemove: nzlTable.data().toArray() },
                dataType: 'json',
                success: function (data, status, xhr) {
                    nzlShowSuccessfullySaved('Record is successfully deleted');
                    dt.draw();
                }
            });
        }
    }
}

nzlTable.on('draw', function () {
    nzlSetVisibleDataTableButtons(nzlTable.rows().count() === 0);
});

function useHistoricReservoirLevelsChanged() {
    $.ajax({
        url: "/NodeZoneLevelHistoricReservoirLevel/Save",
        method: 'POST',
        data: {
            "nodePolicyGroupId": document.getElementById("nzlNodePolicyGroupID").value,
            "nodeId": document.getElementById("nzlNodeID").value,
            "useHistoricReservoirLevels": document.getElementById("chkUseHistoricReservoirLevels").checked            
        },
        dataType: 'json'
    });
}