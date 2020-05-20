var importList;

var cocTable = $('#cocTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "200px",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "ChannelOutflowCapacity/FillGridFromDB",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {                
                "elementID": document.getElementById("cocElementID").value,
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
        { "data": "elevation" },
        { "data": "maximumOutflow" }
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
    if ($("#cocTable input[type=text]").length > 0 || !$("#cocShowModal .saveButton").hasClass("disabled")) { return false; }

    return true;
});

function cocAddDataTableButtons() {
    new $.fn.dataTable.Buttons(cocTable, {
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
                    cocPasteFromClipboard();
                }
            },
            {
                text: 'Save All', className: "saveButton",
                action: function (e, dt, node, config) {
                    cocSaveTable();
                }
            }
        ]
    }).container().appendTo($('#cocButtons'));

    document.getElementById('cocButtons').children[0].classList.remove('btn-group');
}

function cocAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(cocTable, {
        buttons: [
            {
                text: "Add", className: "addButton",
                action: function (e, dt, bt, config) { cocAddRow(e, dt, bt, config); }
            },
            {
                text: "Delete", className: "deleteButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { cocRemoveRow(e, dt, bt, config); }
            },
            {
                text: "Delete All", className: "deleteAllButton",
                action: function (e, dt, bt, config) { cocRemoveAllRows(e, dt, bt, config); }
            },
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { cocEditRow(e, dt, bt, config); }
            },
            {
                text: "Update", className: "updateButton",
                action: function (e, dt, bt, config) { cocSaveRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#cocButtonsBottom'));

    document.getElementById('cocButtonsBottom').children[0].classList.remove('btn-group');

    cocSetButtons('normal');
}

$(document).ready(function () {
    cocTable;
    cocAddDataTableButtons();
    cocAddDataTableButtonsBottom();    
});

function cocSaveTable() {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: "ChannelOutflowCapacity/SaveAll",
        data: {
            elementID: document.getElementById("cocElementID").value,            
            listToSave: cocTable.data().toArray()
        },
        success: function (response) {
            cocShowSuccessfullySaved('Changes are successfully saved');
            cocSetButtons('normal');
            importList = null;
            cocTable.draw();
        }
    });
}

async function cocPasteFromClipboard() {
    var pasteList = await cocConvertClipboardDataToTimeStorageCapacityObject();
   
    $.ajax({
        url: "ChannelOutflowCapacity/GetDateFromClipboard",
        data: {               
            elementID: document.getElementById("cocElementID").value,                     
            existingList: cocTable.data().toArray(),
            pasteList: pasteList
        },
        type: "POST",
        success: function (response) {
            if (!response.error) {
                cocReloadTableAfterClipboardPaste(response);
                cocSetButtons('paste');
            }
        }
    });
}

async function cocConvertClipboardDataToTimeStorageCapacityObject() {
    var clipboardText = await navigator.clipboard.readText();
    clipboardText = clipboardText.replace(/\r\n([^\r\n]*)$/, '$1');
    var clipboardTextRows = clipboardText.split("\r\n");    

    var data = [];
    for (var i = 0; i < clipboardTextRows.length; i++) {
        var clipboardTextColumns = clipboardTextRows[i].split("\t").map(parseFloat);                                   
        data.push(
            {
                ChannelID: document.getElementById("cocElementID").value,                
                Elevation: clipboardTextColumns[0],
                MaximumOutflow: clipboardTextColumns[1]
            }
        );
    }

    return data;
}

function cocShowBadInputAlert() {
    var alertID = "#cocBadInputAlert";
    $(alertID).html('Please check your data.Only numbers can be imported.');
    cocShowAlert(alertID);
}

function cocShowSuccessfullySaved(text) {
    var alertID = "#cocSavedAlert";
    $(alertID).html(text);
    cocShowAlert(alertID);
}

function cocShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function cocReloadTableAfterClipboardPaste(response) {
    importList = response.list;
    cocTable.draw();
}

function cocAddRow(e, dt, bt, config) {
    $.ajax({
        url: "ChannelOutflowCapacity/Add",
        data: {            
            elementID: document.getElementById("cocElementID").value            
        }
    }).done(function (msg) {
        $("#cocDetailShowModal").html(msg);
        $("#cocAddModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function cocEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($(cocTable.buttons(".editButton")[0].node).find("span").text() === "Cancel") {
        $(r).children("td").each(function (i, it) {
            var od = dt.cells(it).data()[0];
            $(it).html(od);
        });
        cocSetButtons('cancel');
    }
    else {
        $(r).children("td").each(function (i, it) {
            var h = $("<input type='text'>");
            h.val(it.innerText);
            $(it).html(h);
        });
        cocSetButtons('edit');
    }
}

function cocSetButtons(mode) {
    cocTable.buttons().enable(false);
    cocTable.buttons([".buttons-copy", ".buttons-excel", ".buttons-csv", ".buttons-pdf", ".buttons-print"]).enable(true);

    switch (mode) {
        case "paste":
            cocTable.buttons([".exportButton", ".pasteButton", ".saveButton"]).enable(true);
            break;
        case "edit":
            cocTable.buttons([".editButton", ".updateButton"]).enable(true);
            $(cocTable.buttons(".editButton")[0].node).find("span").text('Cancel');
            break;
        case "cancel":
        case "normal":
            cocTable.buttons([".editButton", ".addButton", ".deleteButton", ".deleteAllButton", ".exportButton", ".pasteButton"]).enable(true);
            $(cocTable.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
    }
}

function cocSaveRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($("input[type=text]", r).length === 0) { return; }

    $(r).children("td").each(function (i, it) {
        var di = $("input", it).val();
        $(it).html(di);
        var cell = cocTable.cell(it._DT_CellIndex.row, it._DT_CellIndex.column);
        cell.data(di);
    });

    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "ChannelOutflowCapacity/Update",
        method: 'POST',
        data: { listToUpdate: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            cocShowSuccessfullySaved('Record is successfully saved');
            dt.draw();
        }
    });

    cocSetButtons('normal');
}

function cocRemoveRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "ChannelOutflowCapacity/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            cocShowSuccessfullySaved('Record is successfully deleted');
            dt.draw();
        }
    });
}

function cocRemoveAllRows(e, dt, bt, config) {
    if (cocTable.data().any()) {
        if (confirm("Are you sure you want to delete?")) {
            $.ajax({
                url: "ChannelOutflowCapacity/Remove",
                method: 'POST',
                data: { listToRemove: cocTable.data().toArray() },
                dataType: 'json',
                success: function (data, status, xhr) {
                    cocShowSuccessfullySaved('Record is successfully deleted');
                    dt.draw();
                }
            });
        }
    }
}

function cocModalClose() {
    $('#cocModal').modal('hide');
}