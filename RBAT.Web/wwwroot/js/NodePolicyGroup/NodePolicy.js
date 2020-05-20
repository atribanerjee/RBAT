$(document).ready(function () {
    loadNodePolicyGroup(document.getElementById("ScenarioID").value);
});

function loadNodePolicyGroup(scenarioID) {
    $.ajax({
        type: "GET",
        url: "/NodePolicyGroup/NodePolicyGroup",
        data: {
            scenarioID: scenarioID
        },
        success: function (data) {
            $("#nodePolicyGroupDiv").html(data);
        }
    });
}

function scenarioChanged() {
    table.draw();    
}