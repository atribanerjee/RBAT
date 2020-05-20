var npgTable = $('#npgTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "50vh",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "/NodePolicyGroup/GetAll",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "scenarioID": document.getElementById("ScenarioID").value
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
            "visible": false,
            "searchable": false
        }
    ],
    columns: [
        { "data": "id" },
        { "data": "scenarioID" },
        { "data": "name" },
        { "data": "numberOfZonesAboveIdealLevel" },
        { "data": "numberOfZonesBelowIdealLevel" },
        { "data": "zoneWeightsOffset" }
    ],
    language: {
        "info": "Showing _TOTAL_ entries",
        "infoEmpty": "Showing 0 entries",
        "loadingRecords": "Please wait - loading..."
    },
    select: "single",
    stateSave: true,
    drawCallback: function (settings, json) {
        npgTable.row(':eq(0)').select();
    }
});

$(document).ready(function () {
    npgTable;   
});

function scenarioChanged() {
    npgTable.draw();
}

npgTable.on('select', function (e, dt, type, indexes) {
    if (type === 'row') {
        var r = $('#npgTable > tbody > tr.selected').index();
        var previousRowId = $('#npgPreviousRow').val();
        if (r !== previousRowId) {
            $.ajax({
                type: "GET",
                url: "/NodeZone/NodePolicyGroupNode",
                data: {
                    NodePolicyGroupID: npgTable.row(r).data()["id"]
                },
                success: function (data) {
                    $("#nodePolicyGroupNodeDiv").html(data);
                }
            });
            nzwImportList = null;
            $.ajax({
                type: "GET",
                url: "/NodeZone/NodeZoneWeight",
                data: {
                    NodePolicyGroupID: npgTable.row(r).data()["id"]
                },
                success: function (data) {
                    $("#nodeZoneWeightDiv").html(data);
                }
            });
        }
        $('#npgPreviousRow').val(r);
    }
});