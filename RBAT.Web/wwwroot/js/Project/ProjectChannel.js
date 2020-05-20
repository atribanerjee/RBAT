var RbatApp = RbatApp || {};

(function (namespace) {

var projectChannelTable = $('#projectChannelTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "200px",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "/Project/GetProjectChannels",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "projectId": document.getElementById("channelProjectId").value
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
        { "data": "channelId" },
        {
            data: "isSelected",
            render: function (data, type, row) {
                if (row.isSelected) {
                    return '<input type="checkbox" data-channelId=' + row.channelId + ' checked class="editor-active" onchange="RbatApp.ProjectChannelNamespace.doalert(this)">';
                }
                else {
                    return '<input type="checkbox"  data-channelId=' + row.channelId + ' class="editor-active" onchange="RbatApp.ProjectChannelNamespace.doalert(this)">';
                }
                //return data;
            },
            className: "dt-body-center"
        },
        { "data": "channelName" },
        { "data": "channelType" }
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
    projectChannelTable;
});

    namespace.doalert = function (checkboxElem) {
    $.ajax({
        url: "/Project/ProjectChannel",
        method: 'POST',
        data: {
            projectId: document.getElementById("channelProjectId").value,
            channelId: checkboxElem.getAttribute("data-channelId"),
            isChecked: checkboxElem.checked
        },
        dataType: 'json',
        success: function (data, status, xhr) {
            if (data.type !== "Success") {
                alert("Save Project Channel: An unexpected error occurred while saving project channel.");
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
})(RbatApp.ProjectChannelNamespace = RbatApp.ProjectChannelNamespace || {});