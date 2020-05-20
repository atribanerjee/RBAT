var deficitTable = $('#deficitTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "200px",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "ReportingTools/FillDeficitTableFromDB",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {                
                "elementID": document.getElementById("cocElementID").value
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

function deficitAddDataTableButtons() {
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

$(document).ready(function () {
    deficitTable;
    deficitAddDataTableButtons();
});