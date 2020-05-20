var nzwImportList;
var aboveIdealZone = Number(document.getElementById("nzwNumberOfZonesAbove").value);
var belowIdealZone = Number(document.getElementById("nzwNumberOfZonesBelow").value);

var nzwTable = $('#nzwTable').DataTable({
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
        "url": "/NodeZoneWeight/FillGridFromDB",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "nodePolicyGroupID": document.getElementById("nzwNodePolicyGroupID").value,
                "importList": nzwImportList
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
        nzwSetVisibleDataTableButtons(nzwTable.rows().count() === 0);
    }
}).on("user-select", function (e, dt, type, cell, originalEvent) {
    // I do not let the user deselect until done editing the row or editing is cancelled
    if ($("#nzwTable input[type=text]").length > 0 || !$("#nodeZoneWeightDiv .saveButton").hasClass("disabled")) { return false; }

    return true;
});

function nzwAddDataTableButtons() {
    new $.fn.dataTable.Buttons(nzwTable, {
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
                    nzwPasteFromClipboard();
                }
            },
            {
                text: 'Save All', className: "saveButton",
                action: function (e, dt, node, config) {
                    nzwSaveTable();
                }
            }
        ]
    }).container().appendTo($('#nzwButtons'));

    document.getElementById('nzwButtons').children[0].classList.remove('btn-group');
}

function nzwSetVisibleDataTableButtons(visible) { 
    if (visible) {
        document.getElementById('nzwButtons').style.display = "block";
    }
    else {
        document.getElementById('nzwButtons').style.display = "none";
    }
}

function nzwAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(nzwTable, {
        buttons: [
            //{
            //    text: "Add", className: "addButton",
            //    action: function (e, dt, bt, config) { nzwAddRow(e, dt, bt, config); }
            //},
            {
                text: "Delete", className: "deleteButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { nzwRemoveRow(e, dt, bt, config); }
            },
            {
                text: "Delete All", className: "deleteAllButton",
                action: function (e, dt, bt, config) { nzwRemoveAllRows(e, dt, bt, config); }
            },
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { nzwEditRow(e, dt, bt, config); }
            },
            {
                text: "Update", className: "updateButton",
                action: function (e, dt, bt, config) { nzwSaveRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#nzwButtonsBottom'));

    document.getElementById('nzwButtonsBottom').children[0].classList.remove('btn-group');

    nzwSetButtons('normal');
}

$(document).ready(function () {
    nzwSetColumnsVisibility();
    nzwAddDataTableButtons();
    nzwAddDataTableButtonsBottom();
});

function nzwSetColumnsVisibility() {
    nzwSetAllColumnsVisible();
    nzwHideNotUsedZoneLevels();
    $("#nzwTableWrapper").show();
}

function nzwSetAllColumnsVisible() {
    for (var i = 1; i <= 16; i++) {
        var column = nzwTable.column(i);
        column.visible(true);
    }
}

function nzwHideNotUsedZoneLevels() {
    nzwHideNotUsedZoneLevelsAboveIdeal();
    nzwHideNotUsedZoneLevelsBelowIdeal();
}

function nzwHideNotUsedZoneLevelsAboveIdeal() {
    for (var i = 1; i <= 6 - aboveIdealZone; i++) {
        var column = nzwTable.column(i);
        column.visible(false);
    }
}

function nzwHideNotUsedZoneLevelsBelowIdeal() {
    for (var i = 7 + belowIdealZone; i <= 16; i++) {
        var column = nzwTable.column(i);
        column.visible(false);
    }
}

async function nzwPasteFromClipboard() {
    var pasteList = await nzwConvertClipboardDataToDoubleArray();

    if (nzwvalidatePastedList(pasteList)) {
        $.ajax({
            url: "/NodeZoneWeight/GetDateFromClipboard",
            data: {
                nodePolicyGroupID: document.getElementById("nzwNodePolicyGroupID").value,
                existingList: nzwTable.data().toArray(),
                pasteList: pasteList
            },
            type: "POST",
            success: function (response) {
                if (!response.error) {
                    nzwReloadTableAfterClipboardPaste(response);
                    nzwSetButtons('paste');
                    nzwTable.off('draw');
                }
            }
        });
    }
}

function nzwvalidatePastedList(pasteList) {
    if (pasteList === null) {
        nzwShowBadInputAlert('Please check your data. Looks like you didn\'t copy anything');
        return false;
    }

    return nzwListHasOnlyOneElement(pasteList) &&
           nzwListHasExpectedNumberOfColumns(pasteList) &&
           nzwCheckIfPasteListContainsAllNumbers(pasteList);
}

function nzwListHasExpectedNumberOfColumns(paste) {
    if (paste[0].ZoneLevelsAboveIdeal.length + paste[0].ZoneLevelsBelowIdeal.length !== aboveIdealZone + belowIdealZone) {        
        nzwShowBadInputAlert('Please check your data. Copied number of columns must be equal to the grid number of columns on the form');
        return false;
    }

    return true;
}

function nzwListHasOnlyOneElement(paste) {
    if (paste.length > 1) {
        nzwShowBadInputAlert('Please check your data. Only one record can be imported for weights.');
        return false;
    }

    return true;
}

function nzwCheckIfPasteListContainsAllNumbers(arrayDouble) {
    if (!arrayDouble.every(nzwGetArrayFromObject)) {
        nzwShowBadInputAlert('Please check your data. Only numbers can be imported.');
        return false;
    }

    return true;
}

async function nzwConvertClipboardDataToDoubleArray() {
    var clipboardText = await navigator.clipboard.readText();
    clipboardText = clipboardText.replace(/\r\n([^\r\n]*)$/, '$1');
    var clipboardTextRows = clipboardText.split("\r\n");

    var data = [];
    for (var i = 0; i < clipboardTextRows.length; i++) {
        var clipboardTextColumns = clipboardTextRows[i].split("\t").map(parseFloat);

        data.push(
            {
                ZoneLevelsAboveIdeal: nzwCreateArrayOfZoneLevelsAboveIdeal(clipboardTextColumns),
                ZoneLevelsBelowIdeal: nzwCreateArrayOfZoneLevelsBelowIdeal(clipboardTextColumns)
            }
        );
    }

    return data;
}

function nzwCreateArrayOfZoneLevelsAboveIdeal(clipboardTextColumns) {
    var columnZoneLevelsAboveIdeal = [];
    for (var j = 0; j < aboveIdealZone; j++) {
        columnZoneLevelsAboveIdeal.push(clipboardTextColumns[j]);
    }
    return columnZoneLevelsAboveIdeal;
}

function nzwCreateArrayOfZoneLevelsBelowIdeal(clipboardTextColumns) {
    var columnZoneLevelsBelowIdeal = [];
    for (var j = aboveIdealZone; j < clipboardTextColumns.length; j++) {
        columnZoneLevelsBelowIdeal.push(clipboardTextColumns[j]);
    }
    return columnZoneLevelsBelowIdeal;
}

function nzwGetArrayFromObject(n) {
    if (!n.ZoneLevelsAboveIdeal.every(nzwIsNumber) || !n.ZoneLevelsBelowIdeal.every(nzwIsNumber)) {
        return false;
    }
    else {
        return true;
    }
}

function nzwIsNumber(n) {
    return !isNaN(parseFloat(n)) && !isNaN(n - 0);
}

function nzwSaveTable() {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: "/NodeZoneWeight/SaveAll",
        data: {
            nodePolicyGroupID: document.getElementById("nzwNodePolicyGroupID").value,
            listToSave: nzwTable.data().toArray()
        },
        success: function (response) {
            nzwShowSuccessfullySaved('Changes are successfully saved');
            nzwSetButtons('normal');
            nzwImportList = null;
            nzwSetVisibleDataTableButtons(nzwTable.rows().count() === 0);
            nzwTable.draw();
        }
    });
}

function nzwShowBadInputAlert(text) {
    var alertID = "#nzwBadInputAlert";
    $(alertID).html(text);
    nzwShowAlert(alertID);
}

function nzwShowSuccessfullySaved(text) {
    var alertID = "#nzwSavedAlert";
    $(alertID).html(text);
    nzwShowAlert(alertID);
}

function nzwShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function nzwReloadTableAfterClipboardPaste(response) {
    nzwImportList = response.list;
    nzwTable.draw();
}

//function nzwAddRow(e, dt, bt, config) {
//    $.ajax({
//        url: "/NodeZoneWeight/Add",
//        data: {
//            nodePolicyGroupID: document.getElementById("nzwNodePolicyGroupID").value
//        }
//    }).done(function (msg) {
//        $("#nzwDetailShowModal").html(msg);
//        $("#nzwAddModal").modal({
//            backdrop: 'static',
//            keyboard: false
//        });
//    });
//}

function nzwEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($(nzwTable.buttons(".editButton")[0].node).find("span").text() === "Cancel") {
        $(r).children("td").each(function (i, it) {
            var od = dt.cells(it).data()[0];
            $(it).html(od);
        });
        nzwSetButtons('cancel');
    }
    else {
        $(r).children("td").each(function (i, it) {
            var h = $("<input type='text'>");
            h.val(it.innerText);
            $(it).html(h);
        });
        nzwSetButtons('edit');
    }
}

function nzwSetButtons(mode) {
    nzwTable.buttons().enable(false);
    nzwTable.buttons([".buttons-copy", ".buttons-excel", ".buttons-csv", ".buttons-pdf", ".buttons-print"]).enable(true);

    switch (mode) {
        case "paste":
            nzwTable.buttons([".exportButton", ".pasteButton", ".saveButton"]).enable(true);
            break;
        case "edit":
            nzwTable.buttons([".editButton", ".updateButton"]).enable(true);
            $(nzwTable.buttons(".editButton")[0].node).find("span").text('Cancel');
            break;
        case "cancel":
        case "normal":
            nzwTable.buttons([".editButton", ".deleteButton", ".deleteAllButton", ".exportButton", ".pasteButton"]).enable(true);
            $(nzwTable.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
    }
}

function nzwSaveRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($("input[type=text]", r).length === 0) { return; }

    $(r).children("td").each(function (i, it) {
        var di = $("input", it).val();
        $(it).html(di);
        var cell = nzwTable.cell(it._DT_CellIndex.row, it._DT_CellIndex.column);
        cell.data(di);
    });

    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "/NodeZoneWeight/Update",
        method: 'POST',
        data: { listToUpdate: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            nzwShowSuccessfullySaved('Record is successfully saved');
            dt.draw();
        }
    });

    nzwSetButtons('normal');
}

function nzwRemoveRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "/NodeZoneWeight/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            nzwShowSuccessfullySaved('Record is successfully deleted');
            dt.draw();
        }
    });
}

function nzwRemoveAllRows(e, dt, bt, config) {
    if (nzwTable.data().any()) {
        if (confirm("Are you sure you want to delete?")) {
            $.ajax({
                url: "/NodeZoneWeight/Remove",
                method: 'POST',
                data: { listToRemove: nzwTable.data().toArray() },
                dataType: 'json',
                success: function (data, status, xhr) {
                    nzwShowSuccessfullySaved('Record is successfully deleted');
                    dt.draw();
                }
            });
        }
    }
}

nzwTable.on('draw', function () {
    nzwSetVisibleDataTableButtons(nzwTable.rows().count() === 0);
});