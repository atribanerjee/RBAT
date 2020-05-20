var importList;

var srlTable = $('#seasonalReservoirLevelTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "50vh",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "/SeasonalReservoirLevel/GetAll",
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
        { "data": "nodeId" },
        { "data": "nodeName" },
        { "data": "storageLevel" }
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
    if ($("#seasonalReservoirLevelTable input[type=text]").length > 0) { return false; }

    return true;
});

$(document).ready(function () {
    srlAddDataTableButtons();
    srlAddDataTableButtonsBottom();
});

function srlAddDataTableButtons() {
    new $.fn.dataTable.Buttons(srlTable, {
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
                    srlPasteFromClipboard();
                }
            }
        ]
    }).container().appendTo($('#srlButtons'));

    document.getElementById('srlButtons').children[0].classList.remove('btn-group');
}

function srlAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(srlTable, {
        buttons: [
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { srlEditRow(e, dt, bt, config); }
            },
            {
                text: "Update", className: "updateButton",
                action: function (e, dt, bt, config) { srlSaveRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#srlButtonsBottom'));

    document.getElementById('srlButtonsBottom').children[0].classList.remove('btn-group');

    srlSetButtons('normal');
}

async function srlPasteFromClipboard() {
    var pasteList = await srlConvertClipboardDataToArray();    
    if (srlValidatePastedList(pasteList)) {
        $.ajax({
            url: "/SeasonalReservoirLevel/GetDataFromClipboard",
            data: {
                existingList: srlTable.data().toArray(),
                pasteList: pasteList
            },
            type: "POST",
            success: function (response) {
                if (!response.error) {
                    srlReloadTableAfterClipboardPaste(response);
                    srlSetButtons('paste');
                }
            }
        });
    }
}

async function srlConvertClipboardDataToArray() {
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

function srlValidatePastedList(pasteList) {
    if (pasteList === null) {        
        srlShowBadInputAlert('Please check your data. Only one column can be copied');
        return false;
    }

    return srlValidateSameNumberOfRows(pasteList);
}

function srlValidateSameNumberOfRows(pasteList) {
    if (srlTable.data().toArray().length !== pasteList.length) {
        srlShowBadInputAlert('Please check your data. You must provide same number of records as is presented in the grid');
        return false;
    }

    return true;
}

function srlReloadTableAfterClipboardPaste(response) {
    importList = response.list;
    srlTable.draw();
}

function srlShowBadInputAlert(text) {
    var alertID = "#srlBadInputAlert";
    $(alertID).html(text);
    srlShowAlert(alertID);
}

//function srlShowSuccessfullySaved(text) {
//    var alertID = "#srlSavedAlert";
//    $(alertID).html(text);
//    srlShowAlert(alertID);
//}

function srlShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function srlEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    var cellToModify = $(r).children("td")[1];

    if ($(srlTable.buttons(".editButton")[0].node).find("span").text() === "Cancel") {
        var od = dt.cells(cellToModify).data()[0];
        $(cellToModify).html(od);
        srlSetButtons('cancel');
    }
    else {
        var h = $("<input type='text'>");
        h.val(cellToModify.innerText);
        $(cellToModify).html(h);
        srlSetButtons('edit');
    }
}

function srlSaveRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    var cellToModify = $(r).children("td")[1];
    if (cellToModify.length === 0) { return; }

    var di = $("input", cellToModify).val();
    $(cellToModify).html(di);
    var cell = srlTable.cell(cellToModify._DT_CellIndex.row, cellToModify._DT_CellIndex.column);
    cell.data(di);

    srlSetButtons('normal');
}

function srlSetButtons(mode) {
    srlTable.buttons().enable(false);
    srlTable.buttons([".buttons-copy", ".buttons-excel", ".buttons-csv", ".buttons-pdf", ".buttons-print"]).enable(true);

    switch (mode) {
        case "paste":
            srlTable.buttons([".exportButton", ".pasteButton"]).enable(true);
            break;
        case "edit":
            srlTable.buttons([".editButton", ".updateButton"]).enable(true);
            $(srlTable.buttons(".editButton")[0].node).find("span").text('Cancel');
            break;
        case "cancel":
        case "normal":
            srlTable.buttons([".editButton", ".exportButton", ".pasteButton"]).enable(true);
            $(srlTable.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
    }
}