var importList;
var controllerName = document.getElementById("controllerName").value;
var timeComponentType = document.getElementsByName("timeComponent")[0];
var radioButtonDiv = timeComponentType.closest("div");
var dateControl = document.getElementById('startImportDate');

tsdSetInitialRBChecked();


$("#showImportButton").click(function () {
    $(".import").removeClass("invisible").addClass("visible");
});

$("#cancelImportButton").click(function () {
    $(".import").removeClass("visible").addClass("invisible");
    $("#fileSelect").val("");
    $("#importButton").addClass("disabled");
});

$("#fileSelect").change(function () {
    $("#importButton").removeClass("disabled");
});

$("#importButton").click(async function () {
    $('#overlay').removeClass("d-none").addClass("d-block");
    let file = document.getElementById("fileSelect").files[0];
    let formData = new FormData();

    formData.append("file", file);
    formData.append("fileSelect", file, file.name);
    formData.append("projectID", document.getElementById("tsdProjectID").value);
    formData.append("elementID", document.getElementById("tsdElementID").value);
    formData.append("startDate", dateControl.value);
    formData.append("timeComponent", document.querySelector('input[name = "timeComponent"]:checked').value);

    try {
        const response = await fetch(controllerName + "/OnUploadAsync", {
            method: 'POST',
            body: formData,
            enctype: "multipart/form-data"
        });

        if (response.ok) {
            tsdEnableRadioButtonList(true);
            tsdSetButtons('normal');

            const result = await response.json();
            if (result.ok) {
                tsdShowSuccessfullySaved('Changes are successfully saved');
                importList = null;
                tsdTable.draw();
            } else if (result.message) {
                tsdShowBadInputAlert(result.message);
            }

            $('#overlay').removeClass("d-block").addClass("d-none");
        }
    } catch (error) {
        console.error('Error:', error);
        $('#overlay').removeClass("d-block").addClass("d-none");
    }
});

var tsdTable = $('#tsdTable').DataTable({
    dom: 'frtsi',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: 200,
    scrollCollapse: true,
    deferRender: true,
    scroller: {
        loadingIndicator: true
    },
    paging: true,
    ajax: {
        "url": controllerName + "/FillGridFromDB",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "projectID": document.getElementById("tsdProjectID").value,
                "elementID": document.getElementById("tsdElementID").value,
                "importList": importList,
                "timeComponent": document.querySelector('input[name = "timeComponent"]:checked').value
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
        { "data": "value" }
    ],
    language: {
        "info": "Showing _TOTAL_ entries",
        "infoEmpty": "Showing 0 entries",
        "loadingRecords": "Please wait - loading..."
    },
    select: "single",
    stateSave: false
}).on("user-select", function (e, dt, type, cell, originalEvent) {
    // I do not let the user deselect until done editing the row or editing is cancelled
    if ($("#tsdTable input[type=text]").length > 0 || !$("#tsdShowModal .saveButton").hasClass("disabled")) { return false; }

    return true;
});

function exportAllData() {
    $('#overlay').removeClass("d-none").addClass("d-block");

    $.ajax({
        type: 'POST',
        url: controllerName + "/GetAll",
        xhrFields: {
            responseType: 'blob'
        },
        data: {
            projectID: document.getElementById("tsdProjectID").value,
            elementID: document.getElementById("tsdElementID").value,
            timeComponent: document.querySelector('input[name = "timeComponent"]:checked').value
        },
        success: function (result, status, xhr) {
            // get file name
            var filename = "";
            var disposition = xhr.getResponseHeader('Content-Disposition');
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) {
                    filename = matches[1].replace(/['"]/g, '');
                }
            }

            // download the file
            var a = document.createElement('a');
            var url = window.URL.createObjectURL(result);
            a.href = url;
            a.download = filename;
            document.body.append(a);
            a.click();
            a.remove();
            window.URL.revokeObjectURL(url);
            $('#overlay').removeClass("d-block").addClass("d-none");
        },
        error: function (error) {
            console.log(error);
            $('#overlay').removeClass("d-block").addClass("d-none");
        }
    });
}

function tsdAddDataTableButtons() {
    new $.fn.dataTable.Buttons(tsdTable, {
        buttons: [
            {
                text: 'Export', className: "exportButton",
                action: function (e, dt, node, config) {
                    exportAllData();
                }
            },
            {
                text: 'Paste from Clipboard', className: "pasteButton",
                action: function (e, dt, node, config) {
                    tsdPasteFromClipboard();
                }
            },
            {
                text: 'Save All', className: "saveButton",
                action: function (e, dt, node, config) {
                    tsdSaveTable();
                }
            }
        ]
    }).container().appendTo($('#tsdButtons'));

    document.getElementById('tsdButtons').children[0].classList.remove('btn-group');
}

function tsdAddDataTableButtonsBottom() {
    new $.fn.dataTable.Buttons(tsdTable, {
        buttons: [
            {
                text: "Add", className: "addButton",
                action: function (e, dt, bt, config) { tsdAddRow(e, dt, bt, config); }
            },
            {
                text: "Delete", className: "deleteButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { tsdRemoveRow(e, dt, bt, config); }
            },
            {
                text: "Delete All", className: "deleteAllButton",
                action: function (e, dt, bt, config) { tsdRemoveAllRows(e, dt, bt, config); }
            },
            {
                text: "Edit", className: "editButton",
                extend: "selectedSingle",
                action: function (e, dt, bt, config) { tsdEditRow(e, dt, bt, config); }
            },
            {
                text: "Update", className: "updateButton",
                action: function (e, dt, bt, config) { tsdSaveRow(e, dt, bt, config); }
            }
        ]
    }).container().appendTo($('#tsdButtonsBottom'));

    document.getElementById('tsdButtonsBottom').children[0].classList.remove('btn-group');

    tsdSetButtons('normal');
}

$(document).ready(function () {
        tsdShowProjectRow();
        tsdShowRecordedFlowStationRow();
        tsdTable;
        tsdAddDataTableButtons();
        tsdAddDataTableButtonsBottom();
});

function tsdShowProjectRow() {
    if (controllerName === "TimeNaturalFlow") {
        document.getElementById("divProjectID").style.display = "block";
    }
}

function tsdShowRecordedFlowStationRow() {
    if (controllerName === "TimeRecordedFlow") {
        document.getElementById("divRecordedFlowStation").style.display = "block";
        document.getElementById("divNode").style.display = "none";
    }
    else {
        document.getElementById("divRecordedFlowStation").style.display = "none";
        document.getElementById("divNode").style.display = "block";
    }
}

function tsdSaveTable() {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: controllerName + "/SaveAll",
        data: {
            projectID: document.getElementById("tsdProjectID").value,
            elementID: document.getElementById("tsdElementID").value,
            startDate: dateControl.value,
            timeComponent: document.querySelector('input[name = "timeComponent"]:checked').value,
            listToSave: tsdTable.data().toArray()
        },
        success: function (response) {
            tsdShowSuccessfullySaved('Changes are successfully saved');
            tsdEnableRadioButtonList(true);
            tsdSetButtons('normal');
            importList = null;
            tsdTable.draw();
        }
    });
}

async function tsdPasteFromClipboard() {
    if ((controllerName === "TimeNaturalFlow" && document.getElementById("tsdProjectID").value) || controllerName !== "TimeNaturalFlow") {
        var pasteList = await tsdConvertClipboardDataToDoubleArray();

        if (pasteList !== null) {
            $.ajax({
                url: controllerName + "/GetDateFromClipboard",
                data: {
                    projectID: document.getElementById("tsdProjectID").value,
                    elementID: document.getElementById("tsdElementID").value,
                    startDate: dateControl.value,
                    timeComponent: document.querySelector('input[name = "timeComponent"]:checked').value,
                    existingList: tsdTable.data().toArray(),
                    pasteList: pasteList
                },
                type: "POST",
                success: function (response) {
                    if (!response.error) {
                        tsdReloadTableAfterClipboardPaste(response);
                        tsdSetButtons('paste');
                    }
                }
            });
        }
    } else {
        tsdShowBadInputAlert('You must provide a project in order to paste the data');
    }

}

function tsdValidatePastedList(clipboardArray) {
    if (clipboardArray === null) {
        tsdShowBadInputAlert('Please check your data. Looks like you didn\'t copy anything');
        return false;
    }

    return tsdListHasExpectedNumberOfColumns(clipboardArray) &&
        tsdCheckIfPasteListContainsAllNumbers(clipboardArray);
}

function tsdListHasExpectedNumberOfColumns(clipboardArray) {
    if (document.querySelector('input[name = "timeComponent"]:checked').value !== 'Custom' && clipboardArray.length !== 1) {
        tsdShowBadInputAlert('Please check your data. You must provide only one column of data');
        return false;
    }
    else if (document.querySelector('input[name = "timeComponent"]:checked').value === 'Custom' && clipboardArray.length !== 2) {
        tsdShowBadInputAlert('Please check your data. You must provide two columns of data');
        return false;
    }

    return true;
}

function tsdCheckIfPasteListContainsAllNumbers(clipboardArray) {
    if (!clipboardArray.every(tsdIsNumber)) {
        tsdShowBadInputAlert('Please check your data. Only numbers can be imported.');
        return false;
    }
    else {
        tsdEnableRadioButtonList(false);
        return true;
    }
}

function tsdIsNumber(n) {
    return !isNaN(parseFloat(n)) && !isNaN(n - 0);
}


function tsdSetInitialRBChecked() {
    timeComponentType.checked = true;
    //timeComponentType.closest("label").classList.add("active");
    tsdSetTableHeaderTimeComponentTypeValue();
}

function tsdReloadTableForSelectedTimeComponent() {
    $("#timeComponentGroup input").prop("disabled", true);
    $("#timeComponentGroup label").addClass("disabled");
    window.setTimeout(function () {
        importList = null;
        tsdSetTableHeaderTimeComponentTypeValue();
        tsdTable.draw();
        $("#timeComponentGroup input").prop("disabled", false);
        $("#timeComponentGroup label").removeClass("disabled");
    }, 500);
}

function tsdSetTableHeaderTimeComponentTypeValue() {
    document.getElementsByName("TimeComponentType").forEach(
        function (element, index, array) {
            element.innerText = document.querySelector('input[name = "timeComponent"]:checked').value;
        }
    );
}

async function tsdConvertClipboardDataToDoubleArray() {
    var clipboardText = await navigator.clipboard.readText();
    clipboardText = clipboardText.replace(/\r\n([^\r\n]*)$/, '$1');
    var clipboardTextRows = clipboardText.split("\r\n");

    var data = [];
    for (var i = 0; i < clipboardTextRows.length; i++) {
        var clipboardTextColumns = clipboardTextRows[i].split("\t").map(parseFloat);

        if (tsdValidatePastedList(clipboardTextColumns)) {
            if (document.querySelector('input[name = "timeComponent"]:checked').value !== 'Custom') {
                data.push(
                    {
                        TimeStep: null,
                        Value: clipboardTextColumns[0]
                    });
            } else {
                data.push(
                    {
                        TimeStep: clipboardTextColumns[0],
                        Value: clipboardTextColumns[1]
                    });
            }
        } else {
            return null;
        }
    }

    return data;
}

function tsdShowBadInputAlert(text) {
    var alertID = "#tsdBadInputAlert";
    $(alertID).html(text);
    tsdShowAlert(alertID);
}

function tsdShowSuccessfullySaved(text) {
    var alertID = "#tsdSavedAlert";
    $(alertID).html(text);
    tsdShowAlert(alertID);
}

function tsdShowAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}

function tsdEnableRadioButtonList(enable) {
    enable ? radioButtonDiv.style.pointerEvents = "auto" : radioButtonDiv.style.pointerEvents = "none";

    radioButtonDiv.querySelectorAll("label").forEach(
        function (element, index, array) {
            enable ? element.classList.remove("disabled") : element.classList.add("disabled");
        }
    );
}

function tsdReloadTableAfterClipboardPaste(response) {
    importList = response.list;
    tsdTable.draw();
}

function tsdAddRow(e, dt, bt, config) {
    if ((controllerName === "TimeNaturalFlow" && document.getElementById("tsdProjectID").value) || controllerName !== "TimeNaturalFlow") {
        $.ajax({
            url: controllerName + "/Add",
            data: {
                projectID: document.getElementById("tsdProjectID").value,
                elementID: document.getElementById("tsdElementID").value,
                timeComponentType: document.querySelector('input[name = "timeComponent"]:checked').value
            }
        }).done(function (msg) {
            $("#tsdDetailShowModal").html(msg);
            $("#tsdAddModal").modal({
                backdrop: 'static',
                keyboard: false
            });
        });
    } else {
        tsdShowBadInputAlert('You must provide a project in order to add the data');
    }
}

function tsdEditRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($(tsdTable.buttons(".editButton")[0].node).find("span").text() === "Cancel") {
        $(r).children("td").each(function (i, it) {
            var od = dt.cells(it).data()[0];
            $(it).html(od);
        });
        tsdSetButtons('cancel');
    }
    else {
        $(r).children("td").each(function (i, it) {
            var h = $("<input type='text'>");
            h.val(it.innerText);
            $(it).html(h);
        });
        tsdSetButtons('edit');
    }
}

function tsdSetButtons(mode) {
    tsdTable.buttons().enable(false);
    tsdTable.buttons([".buttons-copy", ".buttons-excel", ".buttons-csv", ".buttons-pdf", ".buttons-print"]).enable(true);

    switch (mode) {
        case "paste":
            tsdTable.buttons([".exportButton", ".pasteButton", ".saveButton"]).enable(true);
            break;
        case "edit":
            tsdTable.buttons([".editButton", ".updateButton"]).enable(true);
            $(tsdTable.buttons(".editButton")[0].node).find("span").text('Cancel');
            break;
        case "cancel":
        case "normal":
            tsdTable.buttons([".editButton", ".addButton", ".deleteButton", ".deleteAllButton", ".exportButton", ".pasteButton"]).enable(true);
            $(tsdTable.buttons(".editButton")[0].node).find("span").text('Edit');
            break;
    }
}

function tsdSaveRow(e, dt, bt, config) {
    var r = dt.rows(".selected").nodes()[0];
    if ($("input[type=text]", r).length === 0) { return; }

    $(r).children("td").each(function (i, it) {
        var di = $("input", it).val();
        $(it).html(di);
        var cell = tsdTable.cell(it._DT_CellIndex.row, it._DT_CellIndex.column);
        cell.data(di);
    });

    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: controllerName + "/Update",
        method: 'POST',
        data: { listToUpdate: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            tsdShowSuccessfullySaved('Record is successfully saved');
            dt.draw();
        }
    });

    tsdSetButtons('normal');
}

function tsdRemoveRow(e, dt, bt, config) {
    var selectedRow = dt.rows(".selected").data().to$();

    $.ajax({
        url: controllerName + "/Remove",
        method: 'POST',
        data: { listToRemove: selectedRow.toArray() },
        dataType: 'json',
        success: function (data, status, xhr) {
            tsdShowSuccessfullySaved('Record is successfully deleted');
            dt.draw();
        }
    });
}

function tsdRemoveAllRows(e, dt, bt, config) {
    if (tsdTable.data().any()) {
        if (confirm("Are you sure you want to delete?")) {
            $('#overlay').removeClass("d-none").addClass("d-block");
            $.ajax({
                url: controllerName + "/RemoveAll",
                method: 'POST',
                data: {
                    "projectID": document.getElementById("tsdProjectID").value,
                    "elementID": document.getElementById("tsdElementID").value,
                    "timeComponent": document.querySelector('input[name = "timeComponent"]:checked').value
                },
                dataType: 'json',
                success: function (data, status, xhr) {
                    tsdShowSuccessfullySaved('Records are successfully deleted');
                    dt.draw();
                    $('#overlay').removeClass("d-block").addClass("d-none");
                }
            });
        }
    }
}

function tsdProjectChanged() {
    tsdTable.draw();
}

function tsdModalClose() {
    $('#tsdModal').modal('hide');
}