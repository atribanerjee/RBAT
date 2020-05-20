var importList;

var satTable = $('#seasonalApportionmentTargetTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "50vh",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "/SeasonalApportionmentTarget/GetAll",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "seasonalModel": seasonalModel,
                "importList": importList
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
        { "data": "channelId" },
        { "data": "channelName" },
        { "data": "apportionmentFlowVolume" }
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
    if ($("#seasonalApportionmentTargetTable input[type=text]").length > 0) { return false; }

    return true;
});

$(document).ready(function () {
    satAddDataTableButtons();
    satAddDataTableButtonsBottom();
});

function satAddDataTableButtons() {
    new $.fn.dataTable.Buttons(satTable, {
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
                    satPasteFromClipboard();
                }
            }
        ]
    }).container().appendTo($('#satButtons'));

    document.getElementById('satButtons').children[0].classList.remove('btn-group');
}

function satAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(satTable, {
        buttons: [
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { satEditRow(e, dt, bt, config); }
            },
            {
                text: "Update", className: "updateButton",
                action: function (e, dt, bt, config) { satSaveRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#satButtonsBottom'));

    document.getElementById('satButtonsBottom').children[0].classList.remove('btn-group');

    satSetButtons('normal');
}

async function satPasteFromClipboard() {
    var pasteList = await satConvertClipboardDataToArray();    
    if (satValidatePastedList(pasteList)) {
        $.ajax({
            url: "/SeasonalApportionmentTarget/GetDataFromClipboard",
            data: {
                existingList: satTable.data().toArray(),
                pasteList: pasteList
            },
            type: "POST",
            success: function (response) {
                if (!response.error) {
                    satReloadTableAfterClipboardPaste(response);
                    satSetButtons('paste');
                }
            }
        });
    }
}

async function satConvertClipboardDataToArray() {
    var clipboardText = await navigator.clipboard.readText();
    clipboardText = clipboardText.replace(/\r\n([^\r\n]*)$/, '$1');
    var clipboardTextRows = clipboardText.split("\r\n");

    var data = [];
    for (var i = 0; i < clipboardTextRows.length; i++) {
        var clipboardTextColumns = clipboardTextRows[i].split("\t").map(parseFloat);

        if (clipboardTextColumns === null || clipboardTextColumns.length !== 1) {
            return null;
        }
        data.push(clipboardTextColumns[0]);
    }

    return data;
}

function satValidatePastedList(pasteList) {
    if (pasteList === null) {        
        satShowBadInputAlert('Please check your data. Only one column can be copied');
        return false;
    }

    return satValidateSameNumberOfRows(pasteList);
}

function satValidateSameNumberOfRows(pasteList) {
    if (satTable.data().toArray().length !== pasteList.length) {
        satShowBadInputAlert('Please check your data. You must provide same number of records as is presented in the grid');
        return false;
    }

    return true;
}

function satReloadTableAfterClipboardPaste(response) {
    importList = response.list;
    satTable.draw();
}

function satShowBadInputAlert(text) {
    var alertID = "#satBadInputAlert";
    $(alertID).html(text);
    satShowAlert(alertID);
}

//function satShowSuccessfullySaved(text) {
//    var alertID = "#satSavedAlert";
//    $(alertID).html(text);
//    satShowAlert(alertID);
//}

function satShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function satEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    var cellToModify = $(r).children("td")[1];

    if ($(satTable.buttons(".editButton")[0].node).find("span").text() === "Cancel") {
        var od = dt.cells(cellToModify).data()[0];
        $(cellToModify).html(od);
        satSetButtons('cancel');
    }
    else {
        var h = $("<input type='text'>");
        h.val(cellToModify.innerText);
        $(cellToModify).html(h);
        satSetButtons('edit');
    }
}

function satSaveRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    var cellToModify = $(r).children("td")[1];
    if (cellToModify.length === 0) { return; }

    var di = $("input", cellToModify).val();
    $(cellToModify).html(di);
    var cell = satTable.cell(cellToModify._DT_CellIndex.row, cellToModify._DT_CellIndex.column);
    cell.data(di);

    satSetButtons('normal');
}

function satSetButtons(mode) {
    satTable.buttons().enable(false);
    satTable.buttons([".buttons-copy", ".buttons-excel", ".buttons-csv", ".buttons-pdf", ".buttons-print"]).enable(true);

    switch (mode) {
        case "paste":
            satTable.buttons([".exportButton", ".pasteButton"]).enable(true);
            break;
        case "edit":
            satTable.buttons([".editButton", ".updateButton"]).enable(true);
            $(satTable.buttons(".editButton")[0].node).find("span").text('Cancel');
            break;
        case "cancel":
        case "normal":
            satTable.buttons([".editButton", ".exportButton", ".pasteButton"]).enable(true);
            $(satTable.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
    }
}