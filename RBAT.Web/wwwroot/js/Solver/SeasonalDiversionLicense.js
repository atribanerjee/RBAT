var importList;

var sdlTable = $('#seasonalDiversionLicenseTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "50vh",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "/SeasonalDiversionLicense/GetAll",
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
        { "data": "waterLicenseVolume" },
        { "data": "maximalRateDiversionLicenses" }                   
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
    if ($("#seasonalDiversionLicenseTable input[type=text]").length > 0) { return false; }

    return true;
});

$(document).ready(function () {
    sdlAddDataTableButtons();
    sdlAddDataTableButtonsBottom();
});

function sdlAddDataTableButtons() {
    new $.fn.dataTable.Buttons(sdlTable, {
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
                    sdlPasteFromClipboard();
                }
            }
        ]
    }).container().appendTo($('#sdlButtons'));

    document.getElementById('sdlButtons').children[0].classList.remove('btn-group');
}

function sdlAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(sdlTable, {
        buttons: [
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { sdlEditRow(e, dt, bt, config); }
            },
            {
                text: "Update", className: "updateButton",
                action: function (e, dt, bt, config) { sdlSaveRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#sdlButtonsBottom'));

    document.getElementById('sdlButtonsBottom').children[0].classList.remove('btn-group');

    sdlSetButtons('normal');
}

async function sdlPasteFromClipboard() {
    var pasteList = await sdlConvertClipboardDataToArray();    
    if (sdlValidatePastedList(pasteList)) {
        $.ajax({
            url: "/SeasonalDiversionLicense/GetDataFromClipboard",
            data: {
                existingList: sdlTable.data().toArray(),
                pasteList: pasteList
            },
            type: "POST",
            success: function (response) {
                if (!response.error) {
                    sdlReloadTableAfterClipboardPaste(response);
                    sdlSetButtons('paste');
                }
            }
        });
    }
}

async function sdlConvertClipboardDataToArray() {
    var clipboardText = await navigator.clipboard.readText();
    clipboardText = clipboardText.replace(/\r\n([^\r\n]*)$/, '$1');
    var clipboardTextRows = clipboardText.split("\r\n");
    
    var data = [];
    for (var i = 0; i < clipboardTextRows.length; i++) {
        var clipboardTextColumns = clipboardTextRows[i].split("\t").map(parseFloat);

        if (clipboardTextColumns === null || clipboardTextColumns.length !== 2) {
            return null;
        }

        data.push(
            {
                waterLicenseVolume: clipboardTextColumns[0],
                maximalRateDiversionLicenses: clipboardTextColumns[1]
            }
        );        
    }

    return data;
}

function sdlValidatePastedList(pasteList) {
    if (pasteList === null) {        
        sdlShowBadInputAlert('Please check your data. Only two columns can be copied');
        return false;
    }

    return sdlValidateSameNumberOfRows(pasteList);
}

function sdlValidateSameNumberOfRows(pasteList) {
    if (sdlTable.data().toArray().length !== pasteList.length) {
        sdlShowBadInputAlert('Please check your data. You must provide same number of records as is presented in the grid');
        return false;
    }

    return true;
}

function sdlReloadTableAfterClipboardPaste(response) {
    importList = response.list;
    sdlTable.draw();
}

function sdlShowBadInputAlert(text) {
    var alertID = "#sdlBadInputAlert";
    $(alertID).html(text);
    sdlShowAlert(alertID);
}

//function sdlShowSuccessfullySaved(text) {
//    var alertID = "#sdlSavedAlert";
//    $(alertID).html(text);
//    sdlShowAlert(alertID);
//}

function sdlShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function sdlEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];

    if ($(sdlTable.buttons(".editButton")[0].node).find("span").text() === "Cancel") {
        $(r).children("td").each(function (i, it) {            
            var od = dt.cells(it).data()[0];
            $(it).html(od);
        });
        sdlSetButtons('cancel');
    }
    else {
        $(r).children("td").each(function (i, it) {
            if (i > 0) {
                var h = $("<input type='text'>");
                h.val(it.innerText);
                $(it).html(h);
            }
        });  
        sdlSetButtons('edit');
    }
}

function sdlSaveRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    var cellToModify = $(r).children("td")[1];
    if (cellToModify.length === 0) { return; }

    $(r).children("td").each(function (i, it) {
        var di = $("input", it).val();
        $(it).html(di);
        var cell = sdlTable.cell(it._DT_CellIndex.row, it._DT_CellIndex.column);
        cell.data(di);
    });

    sdlSetButtons('normal');
}

function sdlSetButtons(mode) {
    sdlTable.buttons().enable(false);
    sdlTable.buttons([".buttons-copy", ".buttons-excel", ".buttons-csv", ".buttons-pdf", ".buttons-print"]).enable(true);

    switch (mode) {
        case "paste":
            sdlTable.buttons([".exportButton", ".pasteButton"]).enable(true);
            break;
        case "edit":
            sdlTable.buttons([".editButton", ".updateButton"]).enable(true);
            $(sdlTable.buttons(".editButton")[0].node).find("span").text('Cancel');
            break;
        case "cancel":
        case "normal":
            sdlTable.buttons([".editButton", ".exportButton", ".pasteButton"]).enable(true);
            $(sdlTable.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
    }
}