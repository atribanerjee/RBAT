var importList;
var dateControl = document.getElementById('surveyDate');

var tscTable = $('#tscTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "200px",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "TimeStorageCapacity/FillGridFromDB",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {                
                "elementID": document.getElementById("tscElementID").value,
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
        { "data": "id" },
        { "data": "surveyDate" },
        { "data": "elevation" },
        { "data": "area" },
        { "data": "volume" }
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
    if ($("#tscTable input[type=text]").length > 0 || !$("#tscShowModal .saveButton").hasClass("disabled")) { return false; }

    return true;
});

function tscAddDataTableButtons() {
    new $.fn.dataTable.Buttons(tscTable, {
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
                ],
            },
            {
                text: 'Paste from Clipboard', className: "pasteButton",
                action: function (e, dt, node, config) {
                    tscPasteFromClipboard();
                }
            },
            {
                text: 'Save All', className: "saveButton",
                action: function (e, dt, node, config) {
                    tscSaveTable();
                }
            }
        ]
    }).container().appendTo($('#tscButtons'));

    document.getElementById('tscButtons').children[0].classList.remove('btn-group');
}

function tscAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(tscTable, {
        buttons: [
            {
                text: "Add", className: "addButton",
                action: function (e, dt, bt, config) { tscAddRow(e, dt, bt, config); }
            },
            {
                text: "Delete", className: "deleteButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { tscRemoveRow(e, dt, bt, config); }
            },
            {
                text: "Delete All", className: "deleteAllButton",
                action: function (e, dt, bt, config) { tscRemoveAllRows(e, dt, bt, config); }
            },
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { tscEditRow(e, dt, bt, config); }
            },
            {
                text: "Update", className: "updateButton",
                action: function (e, dt, bt, config) { tscSaveRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#tscButtonsBottom'));

    document.getElementById('tscButtonsBottom').children[0].classList.remove('btn-group');

    tscSetButtons('normal');
}

$(document).ready(function () {
    tscTable;
    tscAddDataTableButtons();
    tscAddDataTableButtonsBottom();
});

function tscSaveTable() {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: "TimeStorageCapacity/SaveAll",
        data: {
            elementID: document.getElementById("tscElementID").value,            
            listToSave: tscTable.data().toArray()
        },
        success: function (response) {
            tscShowSuccessfullySaved('Changes are successfully saved');
            tscSetButtons('normal');
            importList = null;
            tscTable.draw();
        }
    });
}

async function tscPasteFromClipboard() {
    var pasteList = await tscConvertClipboardDataToTimeStorageCapacityObject();
   
    $.ajax({
        url: "TimeStorageCapacity/GetDateFromClipboard",
        data: {               
            elementID: document.getElementById("tscElementID").value,
            surveyDate: dateControl.value,                
            existingList: tscTable.data().toArray(),
            pasteList: pasteList
        },
        type: "POST",
        success: function (response) {
            if (!response.error) {
                tscReloadTableAfterClipboardPaste(response)
                tscSetButtons('paste');
            }
        },
    });
}

async function tscConvertClipboardDataToTimeStorageCapacityObject() {
    var clipboardText = await navigator.clipboard.readText();
    clipboardText = clipboardText.replace(/\r\n([^\r\n]*)$/, '$1');
    var clipboardTextRows = clipboardText.split("\r\n");    

    var data = [];
    for (var i = 0; i < clipboardTextRows.length; i++) {
        var clipboardTextColumns = clipboardTextRows[i].split("\t").map(parseFloat);                                   
        data.push(
            {
                NodeID: document.getElementById("tscElementID").value,
                SurveyDate: dateControl.value,
                Elevation: clipboardTextColumns[0],
                Area: clipboardTextColumns[1],
                Volume: clipboardTextColumns[2]
            }
        );
    }

    return data;
}

function tscShowBadInputAlert() {
    var alertID = "#tscBadInputAlert";
    $(alertID).html('Please check your data.Only numbers can be imported.');
    tscShowAlert(alertID);
}

function tscShowSuccessfullySaved(text) {
    var alertID = "#tscSavedAlert";
    $(alertID).html(text);
    tscShowAlert(alertID);
}

function tscShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function tscReloadTableAfterClipboardPaste(response) {
    importList = response.list;
    tscTable.draw();
}

function tscAddRow(e, dt, bt, config) {
    $.ajax({
        url: "TimeStorageCapacity/Add",
        data: {            
            elementID: document.getElementById("tscElementID").value            
        }
    }).done(function (msg) {
        $("#tscDetailShowModal").html(msg);
        $("#tscAddModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function tscEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($(tscTable.buttons(".editButton")[0].node).find("span").text() === "Cancel") {
        $(r).children("td").each(function (i, it) {
            var od = dt.cells(it).data()[0];
            $(it).html(od);
        });
        tscSetButtons('cancel');
    }
    else {
        $(r).children("td").each(function (i, it) {
            var h = $("<input type='text'>");
            h.val(it.innerText);
            $(it).html(h);
        });
        tscSetButtons('edit');
    }
}

function tscSetButtons(mode) {
    tscTable.buttons().enable(false);
    tscTable.buttons([".buttons-copy", ".buttons-excel", ".buttons-csv", ".buttons-pdf", ".buttons-print"]).enable(true);

    switch (mode) {
        case "paste":
            tscTable.buttons([".exportButton", ".pasteButton", ".saveButton"]).enable(true);
            break;
        case "edit":
            tscTable.buttons([".editButton", ".updateButton"]).enable(true);
            $(tscTable.buttons(".editButton")[0].node).find("span").text('Cancel');
            break;
        case "cancel":
        case "normal":
            tscTable.buttons([".editButton", ".addButton", ".deleteButton", ".deleteAllButton", ".exportButton", ".pasteButton"]).enable(true);
            $(tscTable.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
    }
}

function tscSaveRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($("input[type=text]", r).length === 0) { return; }

    $(r).children("td").each(function (i, it) {
        var di = $("input", it).val();
        $(it).html(di);
        var cell = tscTable.cell(it._DT_CellIndex.row, it._DT_CellIndex.column);
        cell.data(di);
    });

    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "TimeStorageCapacity/Update",
        method: 'POST',
        data: { listToUpdate: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            tscShowSuccessfullySaved('Record is successfully saved')
            dt.draw();
        }
    });

    tscSetButtons('normal')
}

function tscRemoveRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "TimeStorageCapacity/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            tscShowSuccessfullySaved('Record is successfully deleted')
            dt.draw();
        }
    });
}

function tscRemoveAllRows(e, dt, bt, config) {
    if (tscTable.data().any()) {
        if (confirm("Are you sure you want to delete?")) {
            $.ajax({
                url: "TimeStorageCapacity/Remove",
                method: 'POST',
                data: { listToRemove: tscTable.data().toArray() },
                dataType: 'json',
                success: function (data, status, xhr) {
                    tscShowSuccessfullySaved('Record is successfully deleted')
                    dt.draw();
                }
            });
        }
    }
}

function tscModalClose() {
    $('#tscModal').modal('hide');
}
