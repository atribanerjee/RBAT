var cpgcImportList;

if ($.fn.dataTable.isDataTable('#cpgcTable')) {
    cpgcTable = $('#cpgcTable').DataTable();
    cpgcTable.draw();
}
else {
    var cpgcTable = $('#cpgcTable').DataTable({
        dom: 'lfrtip',
        processing: true,
        serverSide: true,
        ordering: false,
        searching: false,
        scrollX: true,
        scrollY: "90vh",
        scrollCollapse: true,
        paging: false,
        ajax: {
            "url": "/ChannelPolicyGroupChannel/FillGridFromDB",
            "type": "POST",
            "datatype": "json",
            "data": function (d) {
                return $.extend({}, d, {
                    "channelPolicyGroupID": document.getElementById("cpgcChannelPolicyGroupID").value,
                    "importList": cpgcImportList
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
            cpgcTable.row(':eq(0)').select();
        },
        rowReorder: {
            dataSrc: 'id',
            update: false
        }
    });
}

cpgcTable.on('select', function (e, dt, type, indexes) {
    if (type === 'row') {
        var r = $('#cpgcTable > tbody > tr.selected').index();
        var previousRowId = $('#cpgcPreviousRow').val();
        if (r !== previousRowId) {
            czlImportList = null;
            $.ajax({
                type: "GET",
                url: "/ChannelZone/ChannelZoneLevel",
                data: {
                    channelPolicyGroupID: document.getElementById("cpgcChannelPolicyGroupID").value,
                    channelID: cpgcTable.row(r).data()["id"]
                },
                success: function (data) {
                    $("#channelZoneLevelDiv").html(data);
                }
            });
        }
        $('#cpgcPreviousRow').val(r);
    }
});

cpgcTable.on('row-reorder', function (e, details, changes) {
    var listToUpdate = [];
    for (var i = 0, ien = details.length; i < ien; i++) {
        listToUpdate.push({
            ChannelPolicyGroupID: document.getElementById("cpgcChannelPolicyGroupID").value,
            ChannelID: details[i].oldData,
            Priority: details[i].newPosition + 1
        });
    }
    if (listToUpdate.length > 0) {
        $.ajax({
            type: "POST",
            url: "/ChannelPolicyGroupChannel/UpdateAll",
            data: {
                listToUpdate: listToUpdate
            },
            success: function (data) {
                cpgcTable.draw();
            }
        });
    }
});