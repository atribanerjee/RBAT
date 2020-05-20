var cpgTable = $('#cpgTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "90vh",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "/ChannelPolicyGroup/GetAll",
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
        cpgTable.row(':eq(0)').select();
    }
})

$(document).ready(function () {
    cpgTable;   
});

function scenarioChanged() {
    cpgTable.draw();
}

cpgTable.on('select', function (e, dt, type, indexes) {
    if (type === 'row') {
        var r = $('#cpgTable > tbody > tr.selected').index();
        var previousRowId = $('#cpgPreviousRow').val();
        if (r !== previousRowId) {
            $.ajax({
                type: "GET",
                url: "/ChannelZone/ChannelPolicyGroupChannel",
                data: {
                    ChannelPolicyGroupID: cpgTable.row(r).data()["id"]
                },
                success: function (data) {
                    $("#channelPolicyGroupChannelDiv").html(data);
                }
            });
            czwImportList = null;
            $.ajax({
                type: "GET",
                url: "/ChannelZone/ChannelZoneWeight",
                data: {
                    ChannelPolicyGroupID: cpgTable.row(r).data()["id"]
                },
                success: function (data) {
                    $("#channelZoneWeightDiv").html(data);
                }
            });
        }
        $('#cpgPreviousRow').val(r);
    }
});