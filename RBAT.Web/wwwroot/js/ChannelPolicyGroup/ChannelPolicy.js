$(document).ready(function () {
    loadChannelPolicyGroup(document.getElementById("ScenarioID").value);
});

function loadChannelPolicyGroup(scenarioID) {
    $.ajax({
        type: "GET",
        url: "/ChannelPolicyGroup/ChannelPolicyGroup",
        data: {
            scenarioID: scenarioID
        },
        success: function (data) {
            $("#channelPolicyGroupDiv").html(data);
        }
    });
}

function scenarioChanged() {
    table.draw();    
}