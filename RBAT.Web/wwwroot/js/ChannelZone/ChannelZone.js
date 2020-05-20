$(document).ready(function () {
    loadChannelPolicyGroup(document.getElementById("czScenarioID").value);            
});

function loadChannelPolicyGroup(scenarioID) {
    $.ajax({
        type: "GET",
        url: "/ChannelZone/ChannelPolicyGroup",
        data: {
            scenarioID: scenarioID
        },
        success: function (data) {
            $("#channelPolicyGroupDiv").html(data);
        }
    });
}