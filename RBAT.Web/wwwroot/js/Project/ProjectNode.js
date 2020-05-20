
var projectNodeTable = $('#projectNodeTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "200px",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "/Project/GetProjectNodes",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "projectId": document.getElementById("nodeProjectId").value
            });
        }
    },
    columnDefs: [
        {
            "targets": [0],
            "visible": false,
            "searchable": false
        },
        {
            "targets": [1],
            "width": 80
        }
    ],
    columns: [
        { "data": "nodeId" },
        {
            data: "isSelected",
            render: function (data, type, row) {
                if (row.isSelected) {
                    return '<input type="checkbox" data-nodeId=' + row.nodeId + ' checked class="editor-active" onchange="doalert(this)">';
                }
                else {
                    return '<input type="checkbox"  data-nodeId=' + row.nodeId + ' class="editor-active" onchange="doalert(this)">';
                }
                //return data;
            },
            className: "dt-body-center"
        },
        { "data": "nodeName" },
        { "data": "nodeType" }
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
    if ($("#projectTable input[type=text]").length > 0) { return false; }

    return true;
});

$(document).ready(function () {
    projectNodeTable;
});

function doalert(checkboxElem) {
    $.ajax({
        url: "/Project/ProjectNode",
        method: 'POST',
        data: {
            projectId: document.getElementById("nodeProjectId").value,
            nodeId: checkboxElem.getAttribute("data-nodeId"),
            isChecked: checkboxElem.checked
        },
        dataType: 'json',
        success: function (data, status, xhr) {
            if (data.type !== "Success") {
                alert("Save Project Node: An unexpected error occurred while saving project node.");
            }
        }
    });
}

function showSuccessfullySaved(text) {
    var alertID = "#savedAlert";
    $(alertID).html(text);
    showAlert(alertID);
}

function showAlert(alertID) {
    $(alertID).removeClass("in").show();
    $(alertID).delay(200).addClass("in").fadeOut(5000);
    $(alertID).show();
}
