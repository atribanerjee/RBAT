var cpgcTable = $('#cpgcTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "75vh",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "/ChannelPolicyGroup/GetChannelPolicyGroupChannels",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "channelPolicyGroupId": document.getElementById("channelPolicyGroupId").value
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
        { "data": "channelID" },
        {
            data: "isSelected",
            render: function (data, type, row) {
                if (row.isSelected) {
                    return '<input type="checkbox" checked>'
                }
                else {
                    return '<input type="checkbox" >'
                }               
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
});

$(document).ready(function () {
    cpgcTable;    
});

$('#cpgcTable').on('click', 'input[type="checkbox"]', function () {
    var $row = $(this).closest('tr');
    var data = cpgcTable.row($row).data();
    var thisChannel = this;
    $.ajax({
        url: "/ChannelPolicyGroup/ChannelPolicyGroupChannel",
        method: 'POST',
        data: {
            channelPolicyGroupId: document.getElementById("channelPolicyGroupId").value,
            channelId: data["channelID"],
            isChecked: this.checked
        },
        dataType: 'json',
        success: function (data, status, xhr) {
            if (data.type === "Error") {
                alert(data.message);
            }
            if (data.type === "ChannelError") {
                thisChannel.checked = false;
                alert(data.message);
            }
        }
    });
});