$(document).ready(function () {
    loadNodePolicyGroup(document.getElementById("nzScenarioID").value);            
});

function loadNodePolicyGroup(scenarioID) {
    $.ajax({
        type: "GET",
        url: "/NodeZone/NodePolicyGroup",
        data: {
            scenarioID: scenarioID
        },
        success: function (data) {
            $("#nodePolicyGroupDiv").html(data);
        }
    });
}