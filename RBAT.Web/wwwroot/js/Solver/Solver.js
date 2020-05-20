var seasonalModel;

function calculate() {
    if (document.getElementById("chkSeasonalModel").checked) {       
        getSeasonalData();        
    } else {
        runSolver();
    }
}

function calculateFromWebService() {
    $('#overlay').removeClass("d-none").addClass("d-block");
    var projectId = document.getElementById("ProjectID").value;
    var scenarioId = document.getElementById("ScenarioID").value;
    var saveConstraintsAsTXTFile = document.getElementById("chkSaveConstraintsAsTXTFile").checked;
    var saveConstraintsAsLINDOFile = document.getElementById("chkSaveConstraintsAsLINDOFile").checked;
    var saveOptimalSolutionsAsTXTFile = document.getElementById("chkSaveOptimalSolutionsAsTXTFile").checked;
    var saveResultsAsVolumes = document.getElementById("chkSaveResultsAsVolumes").checked;
    var isDebugMode = document.getElementById("chkDebugMode").checked;
    var saveComponentName = document.getElementById("chkSaveComponentName").checked;
    var sensitivityAnalysis = document.getElementById("txtSensitivityAnalysis").value;
    var aridityFactor = document.getElementById("txtAridityFactor").value;

    window.open("/Solver/RunSolverFromWebService?projectId=" + projectId + "&scenarioId=" + scenarioId + "&saveConstraintsAsTXTFile=" + saveConstraintsAsTXTFile + "&saveConstraintsAsLINDOFile=" + saveConstraintsAsLINDOFile + "&saveOptimalSolutionsAsTXTFile=" + saveOptimalSolutionsAsTXTFile + "&saveResultsAsVolumes=" + saveResultsAsVolumes + "&isDebugMode=" + isDebugMode + "&saveComponentName=" + saveComponentName + "&sensitivityAnalysis=" + sensitivityAnalysis + "&aridityFactor=" + aridityFactor, "_blank");
    $('#overlay').removeClass("d-block").addClass("d-none");
       // runSolverFromWebService();
}

function getSeasonalData() {
    $('#overlay').removeClass("d-none").addClass("d-block");
    $.ajax({
        type: "POST",
        url: "/Solver/GetSeasonalData",
        data: {
            projectId: document.getElementById("ProjectID").value,
            scenarioId: document.getElementById("ScenarioID").value,
            isSeasonalModel: document.getElementById("chkSeasonalModel").checked,
            startDate: document.getElementById("startDate").value,
            numberOfTimeIntervals: document.getElementById("txtNumberOfTimeIntervals").value,
            sensitivityAnalysis: document.getElementById("txtSensitivityAnalysis").value,
            aridityFactor: document.getElementById("txtAridityFactor").value
        },
        success: function (data) {
            if (data) {
                $('#tabs').tabs();
                $('.tab').on('click', function (e) {
                    $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
                });

                seasonalModel = data;
                loadTables();
                showHideSeasonalModel();
                $('#overlay').removeClass("d-block").addClass("d-none");
            }
        },
        error: function (error) {            
            $('#overlay').removeClass("d-block").addClass("d-none");
        }
    });    
}

function runSolver() {
    $('#overlay').removeClass("d-none").addClass("d-block");
    $.ajax({
        type: "POST",
        url: "/Solver/RunSolver",
        data: {
            projectId: document.getElementById("ProjectID").value,
            scenarioId: document.getElementById("ScenarioID").value,
            isSeasonalModel: document.getElementById("chkSeasonalModel").checked,
            saveConstraintsAsTXTFile: document.getElementById("chkSaveConstraintsAsTXTFile")?.checked,
            saveConstraintsAsLINDOFile: document.getElementById("chkSaveConstraintsAsLINDOFile")?.checked,
            saveOptimalSolutionsAsTXTFile: document.getElementById("chkSaveOptimalSolutionsAsTXTFile").checked,
            saveResultsAsVolumes: document.getElementById("chkSaveResultsAsVolumes").checked,
            isDebugMode: document.getElementById("chkDebugMode")?.checked,
            saveComponentName: document.getElementById("chkSaveComponentName").checked,
            sensitivityAnalysis: document.getElementById("txtSensitivityAnalysis").value,
            aridityFactor: document.getElementById("txtAridityFactor").value,
            seasonalModel: null
        },
        success: function (data) {
            if (data.success) {
                window.location = '/Solver/DownloadZip';
                showHideSeasonalModel();
                $('#overlay').removeClass("d-block").addClass("d-none");
            } else {
                $('#overlay').removeClass("d-block").addClass("d-none");
                alert(data.message);
            }
        }
    });    
}

function runSolverFromWebService() {
    $('#overlay').removeClass("d-none").addClass("d-block");
    $.ajax({
        type: "POST",
        url: "/Solver/RunSolverFromWebService",
        data: {
            projectId: document.getElementById("ProjectID").value,
            scenarioId: document.getElementById("ScenarioID").value,
            isSeasonalModel: document.getElementById("chkSeasonalModel").checked,
            saveConstraintsAsTXTFile: document.getElementById("chkSaveConstraintsAsTXTFile")?.checked,
            saveConstraintsAsLINDOFile: document.getElementById("chkSaveConstraintsAsLINDOFile")?.checked,
            saveOptimalSolutionsAsTXTFile: document.getElementById("chkSaveOptimalSolutionsAsTXTFile").checked,
            saveResultsAsVolumes: document.getElementById("chkSaveResultsAsVolumes").checked,
            isDebugMode: document.getElementById("chkDebugMode")?.checked,
            saveComponentName: document.getElementById("chkSaveComponentName").checked,
            sensitivityAnalysis: document.getElementById("txtSensitivityAnalysis").value,
            aridityFactor: document.getElementById("txtAridityFactor").value,
            seasonalModel: null
        },
        success: function (data) {
            //if (data.success) {
            //    window.location = '/Solver/DownloadZip';
            //    $('#overlay').removeClass("d-block").addClass("d-none");
            //} else {
            //    $('#overlay').removeClass("d-block").addClass("d-none");
            //    alert(data.message);
            //}
        }
    });   
}

function showHideSeasonalModel() {
    var seasonalModelDiv = document.getElementById("seasonalModelDiv");

    if (document.getElementById("chkSeasonalModel").checked) {
        seasonalModelDiv.style.visibility = "visible";
    } else {
        seasonalModelDiv.style.visibility = "hidden";
    }
}

function runSolverWithSeasonalModel() {
    $('#overlay').removeClass("d-none").addClass("d-block");

    seasonalModel.startDate = document.getElementById("startDate").value,
    seasonalModel.numberOfTimeIntervals = document.getElementById("txtNumberOfTimeIntervals").value,
    seasonalModel.initialElevation = srlTable.columns(2).data().eq(0).toArray();
    seasonalModel.waterDemands = swdTable.data().toArray();
    seasonalModel.cumulativeVolumeDiversionLicenses = sdlTable.columns(2).data().eq(0).toArray();
    seasonalModel.maximalRateDiversionLicenses = sdlTable.columns(3).data().eq(0).toArray();
    seasonalModel.apportionmentAgreements = satTable.columns(2).data().eq(0).toArray();

    $.ajax({
        type: "POST",
        url: "/Solver/RunSolver",
        data: {
            projectId: document.getElementById("ProjectID").value,
            scenarioId: document.getElementById("ScenarioID").value,
            isSeasonalModel: document.getElementById("chkSeasonalModel").checked,
            saveConstraintsAsTXTFile: document.getElementById("chkSaveConstraintsAsTXTFile")?.checked,
            saveConstraintsAsLINDOFile: document.getElementById("chkSaveConstraintsAsLINDOFile")?.checked,
            saveOptimalSolutionsAsTXTFile: document.getElementById("chkSaveOptimalSolutionsAsTXTFile").checked,
            saveResultsAsVolumes: document.getElementById("chkSaveResultsAsVolumes").checked,
            isDebugMode: document.getElementById("chkDebugMode")?.checked,
            saveComponentName: document.getElementById("chkSaveComponentName").checked,
            sensitivityAnalysis: document.getElementById("txtSensitivityAnalysis").value,
            aridityFactor: document.getElementById("txtAridityFactor").value,
            seasonalModel: seasonalModel
        },
        success: function (data) {
            if (data) {
                window.location = '/Solver/DownloadZip';
                showHideSeasonalModel();
                $('#overlay').removeClass("d-block").addClass("d-none");
            }
        },
        error: function (error) {
            $('#overlay').removeClass("d-block").addClass("d-none");
        }
    });
}

function loadTables() {
    loadSeasonalReservoirLevel();
    loadSeasonalWaterDemand();
    loadSeasonalDiversionLicense();
    loadSeasonalApportionmentTarget();
}

function loadSeasonalReservoirLevel() {
    $.ajax({
        type: "GET",
        url: "/Solver/SeasonalReservoirLevel",
        success: function (data) {
            $("#seasonalReservoirLevelDiv").html(data);
        }
    });
}

function loadSeasonalWaterDemand() {
    $.ajax({
        type: "GET",
        url: "/Solver/SeasonalWaterDemand",
        success: function (data) {
            $("#seasonalWaterDemandDiv").html(data);
        }
    });
}

function loadSeasonalDiversionLicense() {
    $.ajax({
        type: "POST",
        url: "/Solver/SeasonalDiversionLicense",
        success: function (data) {
            $("#seasonalDiversionLicenseDiv").html(data);
        }
    });
}

function loadSeasonalApportionmentTarget() {
    $.ajax({
        type: "GET",
        url: "/Solver/SeasonalApportionmentTarget",
        success: function (data) {
            $("#seasonalApportionmentTargetDiv").html(data);
        }
    });
}