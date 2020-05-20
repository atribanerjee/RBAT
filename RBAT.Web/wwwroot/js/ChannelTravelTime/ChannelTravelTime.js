var importList;

var cttTable = $('#cttTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "200px",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "ChannelTravelTime/FillGridFromDB",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {                
                "elementID": document.getElementById("cttElementID").value,
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
        { "data": "flow" },
        { "data": "travelTime" }
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
    if ($("#cttTable input[type=text]").length > 0 || !$("#cttShowModal .saveButton").hasClass("disabled")) { return false; }

    return true;
});

function cttAddDataTableButtons() {
    new $.fn.dataTable.Buttons(cttTable, {
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
                    cttPasteFromClipboard()
                }
            },
            {
                text: 'Save All', className: "saveButton",
                action: function (e, dt, node, config) {
                    cttSaveTable()
                }
            }
        ]
    }).container().appendTo($('#cttButtons'));

    document.getElementById('cttButtons').children[0].classList.remove('btn-group');
}

function cttAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(cttTable, {
        buttons: [
            {
                text: "Add", className: "addButton",
                action: function (e, dt, bt, config) { cttAddRow(e, dt, bt, config); }
            },
            {
                text: "Delete", className: "deleteButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { cttRemoveRow(e, dt, bt, config); }
            },
            {
                text: "Delete All", className: "deleteAllButton",
                action: function (e, dt, bt, config) { cttRemoveAllRows(e, dt, bt, config); }
            },
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { cttEditRow(e, dt, bt, config); }
            },
            {
                text: "Update", className: "updateButton",
                action: function (e, dt, bt, config) { cttSaveRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#cttButtonsBottom'));

    document.getElementById('cttButtonsBottom').children[0].classList.remove('btn-group');

    cttSetButtons('normal');
}

$(document).ready(function () {
    cttTable;
    cttAddDataTableButtons();
    cttAddDataTableButtonsBottom();    
});

function cttSaveTable() {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: "ChannelTravelTime/SaveAll",
        data: {
            elementID: document.getElementById("cttElementID").value,            
            listToSave: cttTable.data().toArray()
        },
        success: function (response) {
            cttShowSuccessfullySaved('Changes are successfully saved');
            cttSetButtons('normal');
            importList = null;
            cttTable.draw();
        }
    });
}

async function cttPasteFromClipboard() {
    var pasteList = await cttConvertClipboardDataToTimeStorageCapacityObject();
   
    $.ajax({
        url: "ChannelTravelTime/GetDateFromClipboard",
        data: {               
            elementID: document.getElementById("cttElementID").value,                     
            existingList: cttTable.data().toArray(),
            pasteList: pasteList
        },
        type: "POST",
        success: function (response) {
            if (!response.error) {
                cttReloadTableAfterClipboardPaste(response)
                cttSetButtons('paste');
            }
        },
    });
}

async function cttConvertClipboardDataToTimeStorageCapacityObject() {
    var clipboardText = await navigator.clipboard.readText();
    clipboardText = clipboardText.replace(/\r\n([^\r\n]*)$/, '$1');
    var clipboardTextRows = clipboardText.split("\r\n");    

    var data = [];
    for (var i = 0; i < clipboardTextRows.length; i++) {
        var clipboardTextColumns = clipboardTextRows[i].split("\t").map(parseFloat);                                   
        data.push(
            {
                ChannelID: document.getElementById("cttElementID").value,                
                Flow: clipboardTextColumns[0],
                TravelTime: clipboardTextColumns[1]
            }
        );
    }

    return data;
}

function cttShowBadInputAlert() {
    var alertID = "#cttBadInputAlert";
    $(alertID).html('Please check your data.Only numbers can be imported.');
    cttShowAlert(alertID);
}

function cttShowSuccessfullySaved(text) {
    var alertID = "#cttSavedAlert";
    $(alertID).html(text);
    cttShowAlert(alertID);
}

function cttShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function cttReloadTableAfterClipboardPaste(response) {
    importList = response.list;
    cttTable.draw();
}

function cttAddRow(e, dt, bt, config) {
    $.ajax({
        url: "ChannelTravelTime/Add",
        data: {            
            elementID: document.getElementById("cttElementID").value            
        }
    }).done(function (msg) {
        $("#cttDetailShowModal").html(msg);
        $("#cttAddModal").modal({
            backdrop: 'static',
            keyboard: false
        });
    });
}

function cttEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($(cttTable.buttons(".editButton")[0].node).find("span").text() === "Cancel") {
        $(r).children("td").each(function (i, it) {
            var od = dt.cells(it).data()[0];
            $(it).html(od);
        });
        cttSetButtons('cancel');
    }
    else {
        $(r).children("td").each(function (i, it) {
            var h = $("<input type='text'>");
            h.val(it.innerText);
            $(it).html(h);
        });
        cttSetButtons('edit');
    }
}

function cttSetButtons(mode) {
    cttTable.buttons().enable(false);
    cttTable.buttons([".buttons-copy", ".buttons-excel", ".buttons-csv", ".buttons-pdf", ".buttons-print"]).enable(true);

    switch (mode) {
        case "paste":
            cttTable.buttons([".exportButton", ".pasteButton", ".saveButton"]).enable(true);
            break;
        case "edit":
            cttTable.buttons([".editButton", ".updateButton"]).enable(true);
            $(cttTable.buttons(".editButton")[0].node).find("span").text('Cancel');
            break;
        case "cancel":
        case "normal":
            cttTable.buttons([".editButton", ".addButton", ".deleteButton", ".deleteAllButton", ".exportButton", ".pasteButton"]).enable(true);
            $(cttTable.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
    }
}

function cttSaveRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($("input[type=text]", r).length === 0) { return; }

    $(r).children("td").each(function (i, it) {
        var di = $("input", it).val();
        $(it).html(di);
        var cell = cttTable.cell(it._DT_CellIndex.row, it._DT_CellIndex.column);
        cell.data(di);
    });

    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "ChannelTravelTime/Update",
        method: 'POST',
        data: { listToUpdate: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            cttShowSuccessfullySaved('Record is successfully saved')
            dt.draw();
        }
    });

    cttSetButtons('normal')
}

function cttRemoveRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: "ChannelTravelTime/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            cttShowSuccessfullySaved('Record is successfully deleted')
            dt.draw();
        }
    });
}

function cttRemoveAllRows(e, dt, bt, config) {
    if (cttTable.data().any()) {
        if (confirm("Are you sure you want to delete?")) {
            $.ajax({
                url: "ChannelTravelTime/Remove",
                method: 'POST',
                data: { listToRemove: cttTable.data().toArray() },
                dataType: 'json',
                success: function (data, status, xhr) {
                    cttShowSuccessfullySaved('Record is successfully deleted')
                    dt.draw();
                }
            });
        }
    }
}

function cttModalClose() {
    $('#cttModal').modal('hide');
}
