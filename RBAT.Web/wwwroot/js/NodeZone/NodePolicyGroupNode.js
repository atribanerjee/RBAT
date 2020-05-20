var npgnImportList;

if ($.fn.dataTable.isDataTable('#npgnTable')) {
    npgnTable = $('#npgnTable').DataTable();
    npgnTable.draw();
}
else {
    var npgnTable = $('#npgnTable').DataTable({
        dom: 'lfrtip',
        processing: true,
        serverSide: true,
        ordering: false,
        searching: false,
        scrollX: true,
        scrollY: "200px",
        scrollCollapse: true,
        paging: false,
        ajax: {
            "url": "/NodePolicyGroupNode/FillGridFromDB",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                return $.extend({}, d, {
                    "nodePolicyGroupID": document.getElementById("npgnNodePolicyGroupID").value,
                    "importList": npgnImportList
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
                "className": "dt-body-center"
            }
        ],
        columns: [
            { "data": "id" },
            { "data": null, "defaultContent": "&#8597;" },
            { "data": "name" }
        ],
        language: {
            "info": "Showing _TOTAL_ entries",
            "infoEmpty": "Showing 0 entries",
            "loadingRecords": "Please wait - loading..."
        },
        select: "single",
        stateSave: true,
        drawCallback: function (settings, json) {
            npgnTable.row(':eq(0)').select();
        },
        rowReorder: {
            dataSrc: 'id',
            update: false
        }
    });
}

npgnTable.on('select', function (e, dt, type, indexes) {
    if (type === 'row') {
        var r = $('#npgnTable > tbody > tr.selected').index();
        var previousRowId = $('#npgnPreviousRow').val();
        if (r !== previousRowId) {
            nzlImportList = null;
            $.ajax({
                type: "GET",
                url: "/NodeZone/NodeZoneLevel",
                data: {
                    nodePolicyGroupID: document.getElementById("npgnNodePolicyGroupID").value,                    
                    nodeID: npgnTable.row(r).data()["id"],
                    nodeTypeID: npgnTable.row(r).data()["nodeTypeId"]
                },
                success: function (data) {
                    $("#nodeZoneLevelDiv").html(data);
                }
            });
        }
        $('#npgnPreviousRow').val(r);
    }
});

npgnTable.on('row-reorder', function (e, details, changes) {
    var listToUpdate = [];
    for (var i = 0, ien = details.length; i < ien; i++) {
        listToUpdate.push({
            NodePolicyGroupID: document.getElementById("npgnNodePolicyGroupID").value,
            NodeID: details[i].oldData,
            Priority: details[i].newPosition + 1
        });
    }
    if (listToUpdate.length > 0) {
        $.ajax({
            type: "POST",
            url: "/NodePolicyGroupNode/UpdateAll",
            data: {
                listToUpdate: listToUpdate
            },
            success: function (data) {
                npgnTable.draw();
            }
        });
    }
});