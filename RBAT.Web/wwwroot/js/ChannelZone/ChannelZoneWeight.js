var czwImportList;
var aboveIdealZone = Number(document.getElementById("czwNumberOfZonesAbove").value);
var belowIdealZone = Number(document.getElementById("czwNumberOfZonesBelow").value);

var czwTable = $('#czwTable').DataTable({
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
        "url": "/ChannelZoneWeight/FillGridFromDB",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "channelPolicyGroupID": document.getElementById("czwChannelPolicyGroupID").value,
                "importList": czwImportList
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
    stateSave: true,
    initComplete: function (settings, json) {
        czwSetVisibleDataTableButtons(czwTable.rows().count() === 0);
    }
}).on("user-select", function (e, dt, type, cell, originalEvent) {
    // I do not let the user deselect until done editing the row or editing is cancelled
    if ($("#czwTable input[type=text]").length > 0 || !$("#channelZoneWeightDiv .saveButton").hasClass("disabled")) { return false; }

    return true;
});

function czwAddDataTableButtons() {
    new $.fn.dataTable.Buttons(czwTable, {
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
                    czwPasteFromClipboard();
                }
            },
            {
                text: 'Save All', className: "saveButton",
                action: function (e, dt, node, config) {
                    czwSaveTable();
                }
            }
        ]
    }).container().appendTo($('#czwButtons'));

    document.getElementById('czwButtons').children[0].classList.remove('btn-group');
}

function czwSetVisibleDataTableButtons(visible) { 
    if (visible) {
        document.getElementById('czwButtons').style.display = "block";
    }
    else {
        document.getElementById('czwButtons').style.display = "none";
    }
}

function czwAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(czwTable, {
        buttons: [
            //{
            //    text: "Add", className: "addButton",
            //    action: function (e, dt, bt, config) { czwAddRow(e, dt, bt, config); }
            //},
            {
                text: "Delete", className: "deleteButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { czwRemoveRow(e, dt, bt, config); }
            },
            {
                text: "Delete All", className: "deleteAllButton",
                action: function (e, dt, bt, config) { czwRemoveAllRows(e, dt, bt, config); }
            },
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { czwEditRow(e, dt, bt, config); }
            },
            {
                text: "Update", className: "updateButton",
                action: function (e, dt, bt, config) { czwSaveRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#czwButtonsBottom'));

    document.getElementById('czwButtonsBottom').children[0].classList.remove('btn-group');

    czwSetButtons('normal');
}

$(document).ready(function () {
    czwSetColumnsVisibility();
    czwAddDataTableButtons();
    czwAddDataTableButtonsBottom();
});

function czwSetColumnsVisibility() {
    czwSetAllColumnsVisible();
    czwHideNotUsedZoneLevels();
    $("#czwTableWrapper").show();
}

function czwSetAllColumnsVisible() {
    for (var i = 1; i <= 17; i++) {
        var column = czwTable.column(i);
        column.visible(true);
    }
}

function czwHideNotUsedZoneLevels() {
    czwHideNotUsedZoneLevelsAboveIdeal();
    czwHideNotUsedZoneLevelsBelowIdeal();
}

function czwHideNotUsedZoneLevelsAboveIdeal() {
    for (var i = 1; i <= 6 - aboveIdealZone; i++) {
        var column = czwTable.column(i);
        column.visible(false);
    }
}

function czwHideNotUsedZoneLevelsBelowIdeal() {
    for (var i = 8 + belowIdealZone; i <= 17; i++) {
        var column = czwTable.column(i);
        column.visible(false);
    }
}

async function czwPasteFromClipboard() {
    var pasteList = await czwConvertClipboardDataToDoubleArray();

    if (czwvalidatePastedList(pasteList)) {
        $.ajax({
            url: "/ChannelZoneWeight/GetDateFromClipboard",
            data: {
                channelPolicyGroupID: document.getElementById("czwChannelPolicyGroupID").value,
                existingList: czwTable.data().toArray(),
                pasteList: pasteList
            },
            type: "POST",
            success: function (response) {
                if (!response.error) {
                    czwReloadTableAfterClipboardPaste(response);
                    czwSetButtons('paste');
                    czwTable.off('draw');
                }
            }
        });
    }
}

function czwvalidatePastedList(pasteList) {
    if (pasteList === null) {
        czwShowBadInputAlert('Please check your data. Looks like you didn\'t copy anything');
        return false;
    }

    return czwListHasOnlyOneElement(pasteList) &&
           czwListHasExpectedNumberOfColumns(pasteList) &&
           czwCheckIfPasteListContainsAllNumbers(pasteList);
}

function czwListHasExpectedNumberOfColumns(paste) {
    if (paste[0].ZoneLevelsAboveIdeal.length + paste[0].ZoneLevelsBelowIdeal.length + paste[0].IdealZone.length !== aboveIdealZone + belowIdealZone + 1) {        
        czwShowBadInputAlert('Please check your data. Copied number of columns must be equal to the grid number of columns on the form');
        return false;
    }

    return true;
}

function czwListHasOnlyOneElement(paste) {
    if (paste.length > 1) {
        czwShowBadInputAlert('Please check your data. Only one record can be imported for weights.');
        return false;
    }

    return true;
}

function czwCheckIfPasteListContainsAllNumbers(arrayDouble) {
    if (!arrayDouble.every(czwGetArrayFromObject)) {
        czwShowBadInputAlert('Please check your data. Only numbers can be imported.');
        return false;
    }

    return true;
}

async function czwConvertClipboardDataToDoubleArray() {
    var clipboardText = await navigator.clipboard.readText();
    clipboardText = clipboardText.replace(/\r\n([^\r\n]*)$/, '$1');
    var clipboardTextRows = clipboardText.split("\r\n");

    var data = [];
    for (var i = 0; i < clipboardTextRows.length; i++) {
        var clipboardTextColumns = clipboardTextRows[i].split("\t").map(parseFloat);

        data.push(
            {
                ZoneLevelsAboveIdeal: czwCreateArrayOfZoneLevelsAboveIdeal(clipboardTextColumns),
                IdealZone: [clipboardTextColumns[aboveIdealZone]],
                ZoneLevelsBelowIdeal: czwCreateArrayOfZoneLevelsBelowIdeal(clipboardTextColumns)
            }
        );
    }

    return data;
}

function czwCreateArrayOfZoneLevelsAboveIdeal(clipboardTextColumns) {
    var columnZoneLevelsAboveIdeal = [];
    for (var j = 0; j < aboveIdealZone; j++) {
        columnZoneLevelsAboveIdeal.push(clipboardTextColumns[j]);
    }
    return columnZoneLevelsAboveIdeal;
}

function czwCreateArrayOfZoneLevelsBelowIdeal(clipboardTextColumns) {
    var columnZoneLevelsBelowIdeal = [];
    for (var j = aboveIdealZone + 1; j < clipboardTextColumns.length; j++) {
        columnZoneLevelsBelowIdeal.push(clipboardTextColumns[j]);
    }
    return columnZoneLevelsBelowIdeal;
}

function czwGetArrayFromObject(n) {
    if (!n.ZoneLevelsAboveIdeal.every(czwIsNumber) || !n.IdealZone.every(czwIsNumber) || !n.ZoneLevelsBelowIdeal.every(czwIsNumber)) {
        return false;
    }
    else {
        return true;
    }
}

function czwIsNumber(n) {
    return !isNaN(parseFloat(n)) && !isNaN(n - 0);
}

function czwSaveTable() {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: "/ChannelZoneWeight/SaveAll",
        data: {
            channelPolicyGroupID: document.getElementById("czwChannelPolicyGroupID").value,
            listToSave: czwTable.data().toArray()
        },
        success: function (response) {
            czwShowSuccessfullySaved('Changes are successfully saved');
            czwSetButtons('normal');
            czwImportList = null;
            czwSetVisibleDataTableButtons(czwTable.rows().count() === 0);     
            czwTable.draw();
        }
    });
}

function czwShowBadInputAlert(text) {
    var alertID = "#czwBadInputAlert";
    $(alertID).html(text);
    czwShowAlert(alertID);
}

function czwShowSuccessfullySaved(text) {
    var alertID = "#czwSavedAlert";
    $(alertID).html(text);
    czwShowAlert(alertID);
}

function czwShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function czwReloadTableAfterClipboardPaste(response) {
    czwImportList = response.list;
    czwTable.draw();
}

//function czwAddRow(e, dt, bt, config) {
//    $.ajax({
//        url: "/ChannelZoneWeight/Add",
//        data: {
//            channelPolicyGroupID: document.getElementById("czwChannelPolicyGroupID").value
//        }
//    }).done(function (msg) {
//        $("#czwDetailShowModal").html(msg);
//        $("#czwAddModal").modal({
//            backdrop: 'static',
//            keyboard: false
//        });
//    });
//}

function czwEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($(czwTable.buttons(".editButton")[0].node).find("span").text() === "Cancel") {
        $(r).children("td").each(function (i, it) {
            var od = dt.cells(it).data()[0];
            $(it).html(od);
        });
        czwSetButtons('cancel');
    }
    else {
        $(r).children("td").each(function (i, it) {
            var h = $("<input type='text'>");
            h.val(it.innerText);
            $(it).html(h);
        });
        czwSetButtons('edit');
    }
}

function czwSetButtons(mode) {
    czwTable.buttons().enable(false);
    czwTable.buttons([".buttons-copy", ".buttons-excel", ".buttons-csv", ".buttons-pdf", ".buttons-print"]).enable(true);

    switch (mode) {
        case "paste":
            czwTable.buttons([".exportButton", ".pasteButton", ".saveButton"]).enable(true);
            break;
        case "edit":
            czwTable.buttons([".editButton", ".updateButton"]).enable(true);
            $(czwTable.buttons(".editButton")[0].node).find("span").text('Cancel');
            break;
        case "cancel":
        case "normal":
            czwTable.buttons([".editButton", ".deleteButton", ".deleteAllButton", ".exportButton", ".pasteButton"]).enable(true);
            $(czwTable.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
    }
}

function czwSaveRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($("input[type=text]", r).length === 0) { return; }

    $(r).children("td").each(function (i, it) {
        var di = $("input", it).val();
        $(it).html(di);
        var cell = czwTable.cell(it._DT_CellIndex.row, it._DT_CellIndex.column);
        cell.data(di);
    });

    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "/ChannelZoneWeight/Update",
        method: 'POST',
        data: { listToUpdate: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            czwShowSuccessfullySaved('Record is successfully saved');
            dt.draw();
        }
    });

    czwSetButtons('normal');
}

function czwRemoveRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "/ChannelZoneWeight/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            czwShowSuccessfullySaved('Record is successfully deleted');
            dt.draw();
        }
    });
}

function czwRemoveAllRows(e, dt, bt, config) {
    if (czwTable.data().any()) {
        if (confirm("Are you sure you want to delete?")) {
            $.ajax({
                url: "/ChannelZoneWeight/Remove",
                method: 'POST',
                data: { listToRemove: czwTable.data().toArray() },
                dataType: 'json',
                success: function (data, status, xhr) {
                    czwShowSuccessfullySaved('Record is successfully deleted');
                    dt.draw();
                }
            });
        }
    }
}

czwTable.on('draw', function () {
    czwSetVisibleDataTableButtons(czwTable.rows().count() === 0);
});