var importList;
var nodeList;
var swdTable;

$(document).ready(function () {
    swdSetTableData();
});

function swdSetTableData() {
    getData(function (data) {
        var columns = [];        

        for (var i = 0; i < data.data.item1.length; i++) {
            columns.push({
                title: Object.values(data.data.item1)[i]
            });          
        }
 
        swdTable = $('#seasonalWaterDemandTable').DataTable({
            dom: 'lfrtip',
            ordering: false,
            searching: false,
            scrollY: "50vh",
            scrollCollapse: true,
            scrollX: true,
            paging: false,
            data: data.data.item2,
            columns: columns,
            language: {
                info: "Showing _TOTAL_ entries",
                infoEmpty: "Showing 0 entries",
                loadingRecords: "Please wait - loading..."
            },
            select: "single",
            stateSave: true
        });

        swdTable.on("user-select", function (e, dt, type, cell, originalEvent) {
            // I do not let the user deselect until done editing the row or editing is cancelled
            if ($("#seasonalWaterDemandTable input[type=text]").length > 0) { return false; }

            return true;
        });

        swdAddDataTableButtons();
        swdAddDataTableButtonsBottom();
    });
}

function getData(cb_func) {
    $.ajax({
        url: "/SeasonalWaterDemand/GetAll",
        type: "POST",
        datatype: "json",
        data: {
            seasonalModel: seasonalModel               
        },
        success: cb_func
    });
}

function swdAddDataTableButtons() {
    new $.fn.dataTable.Buttons(swdTable, {
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
                    swdPasteFromClipboard();
                }
            }
        ]
    }).container().appendTo($('#swdButtons'));

    document.getElementById('swdButtons').children[0].classList.remove('btn-group');
}

function swdAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(swdTable, {
        buttons: [
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { swdEditRow(e, dt, bt, config); }
            },
            {
                text: "Update", className: "updateButton",
                action: function (e, dt, bt, config) { swdSaveRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#swdButtonsBottom'));

    document.getElementById('swdButtonsBottom').children[0].classList.remove('btn-group');

    swdSetButtons('normal');
}

async function swdPasteFromClipboard() {
    var pasteList = await swdConvertClipboardDataToArray();
    if (swdValidatePastedList(pasteList)) {
        swdReloadTableAfterClipboardPaste(pasteList);       
    }
}

async function swdConvertClipboardDataToArray() {
    var clipboardText = await navigator.clipboard.readText();
    clipboardText = clipboardText.replace(/\r\n([^\r\n]*)$/, '$1');
    var clipboardTextRows = clipboardText.split("\r\n");
    
    var data = [];
    for (var i = 0; i < clipboardTextRows.length; i++) {
        var clipboardTextColumns = clipboardTextRows[i].split("\t").map(parseFloat);

        if (clipboardTextColumns === null || clipboardTextColumns.length !== swdTable.init().columns.length) {
            return null;
        }

        var record = [];
        for (var j = 0; j < clipboardTextColumns.length; j++) {
            record.push(clipboardTextColumns[j]);
        }

        data.push(record);
    }

    return data;
}

function swdValidatePastedList(pasteList) {
    if (pasteList === null) {
        swdShowBadInputAlert('Please check your data. You must provide same number of columns as is presented in the grid');
        return false;
    }

    return swdValidateSameNumberOfRows(pasteList);
}

function swdValidateSameNumberOfRows(pasteList) {
    if (swdTable.data().toArray().length !== pasteList.length) {
        swdShowBadInputAlert('Please check your data. You must provide same number of records as is presented in the grid');
        return false;
    }
    
    return true;
}

function swdReloadTableAfterClipboardPaste(pasteList) {
    swdTable.clear();
    swdTable.rows.add(pasteList).draw();
}

function swdShowBadInputAlert(text) {
    var alertID = "#swdBadInputAlert";
    $(alertID).html(text);
    swdShowAlert(alertID);
}

//function swdShowSuccessfullySaved(text) {
//    var alertID = "#swdSavedAlert";
//    $(alertID).html(text);
//    swdShowAlert(alertID);
//}

function swdShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function swdEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];    

    if ($(swdTable.buttons(".editButton")[0].node).find("span").text() === "Cancel") {
        $(r).children("td").each(function (i, it) {
            var od = dt.cells(it).data()[0];
            $(it).html(od);
        });        
        swdSetButtons('cancel');
    }
    else {
        $(r).children("td").each(function (i, it) {
            var h = $("<input type='text'>");
            h.val(it.innerText);
            $(it).html(h);
        });        
        swdSetButtons('edit');
    }
}

function swdSaveRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    var cellToModify = $(r).children("td")[1];
    if (cellToModify.length === 0) { return; }

    $(r).children("td").each(function (i, it) {
        var di = $("input", it).val();
        $(it).html(di);
        var cell = swdTable.cell(it._DT_CellIndex.row, it._DT_CellIndex.column);
        cell.data(di);
    });    

    swdSetButtons('normal');
}

function swdSetButtons(mode) {
    swdTable.buttons().enable(false);
    swdTable.buttons([".buttons-copy", ".buttons-excel", ".buttons-csv", ".buttons-pdf", ".buttons-print"]).enable(true);

    switch (mode) {
        case "paste":
            swdTable.buttons([".exportButton", ".pasteButton"]).enable(true);
            break;
        case "edit":
            swdTable.buttons([".editButton", ".updateButton"]).enable(true);
            $(swdTable.buttons(".editButton")[0].node).find("span").text('Cancel');
            break;
        case "cancel":
        case "normal":
            swdTable.buttons([".editButton", ".exportButton", ".pasteButton"]).enable(true);
            $(swdTable.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
    }
}