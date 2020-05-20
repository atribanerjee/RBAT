var npgnTable = $('#npgnTable').DataTable({
    dom: 'lfrtip',
    processing: true,
    serverSide: true,
    ordering: false,
    searching: false,
    scrollY: "90vh",
    scrollCollapse: true,
    paging: false,
    ajax: {
        "url": "/NodePolicyGroup/GetNodePolicyGroupNodes",
        "type": "POST",
        "datatype": "json",
        "data": function (d) {
            return $.extend({}, d, {
                "nodePolicyGroupId": document.getElementById("nodePolicyGroupId").value
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
        { "data": "nodeID" },
        {
            data: "isSelected",
            render: function (data, type, row) {
                if (row.isSelected) {
                    return '<input type="checkbox" checked>';
                }
                else {
                    return '<input type="checkbox" >';
                }               
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
});

$(document).ready(function () {
    npgnTable;    
});

$('#npgnTable').on('click', 'input[type="checkbox"]', function () {
    var $row = $(this).closest('tr');
    var data = npgnTable.row($row).data();
    var thisNode = this;
    $.ajax({
        url: "/NodePolicyGroup/NodePolicyGroupNode",
        method: 'POST',
        data: {
            nodePolicyGroupId: document.getElementById("nodePolicyGroupId").value,
            nodeId: data["nodeID"],
            isChecked: this.checked
        },
        dataType: 'json',
        success: function (data, status, xhr) {
            if (data.type === "Error") {
                alert(data.message);
            }

            if (data.type === "NodeError") {
                thisNode.checked = false;
                alert(data.message);
            }
        }
    });
});