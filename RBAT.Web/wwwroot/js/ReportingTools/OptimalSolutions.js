(function () {

    function getChartType() {
        return $("#chartType").val();
    }

    function getStartDate() {
        return $("#startDate").val();
    }

    function setStartDate(startDate) {
        $("#startDate").val(startDate);
    }

    function getEndDate() {
        return $("#endDate").val();
    }

    function setEndDate(endDate) {
        $("#endDate").val(endDate);
    }

    function getExceedenceChartControls() {
        return $(".exceedenceDate");
    }

    function getComponentType() {
        return $("#component :selected").data().type;
    }

    function getComponents() {
        return $("#component option");
    }

    function getProjectId() {
        return $('#projectList :selected').val();
    }

    function getProjectName() {
        return $('#projectList :selected').text();
    }

    function getProjectListElement() {
        return $('#projectList');
    }

    function getScenarioName() {
        return $('#scenarios :selected').val();
    }

    function getScenarioElement() {
        return $("#scenarios");
    }
    
    function getComponent() {
        return $("#component :selected").val();
    }

    function getComponentName() {
        return $("#component :selected").text();
    }

    function getComponentElement() {
        return $("#component");
    }

    function getIdealCheckbox() {
        return $("#idealCheckbox");
    }

    function getSimulatedCheckbox() {
        return $("#simulatedCheckbox");
    }

    function getAddButtonElement() {
        return $("#addButton");
    }

    function disableAddButton(disable) {
        getAddButtonElement().prop("disabled", disable);
    }

    function showProgress() {
        $('#overlay').removeClass("d-none").addClass("d-block");
    }

    function hideProgress() {
        $('#overlay').removeClass("d-block").addClass("d-none");
    }
   
    var selectedSeries = (function () {
        var series = [],

            getSeries = function () {
                return series;
            },

            addSeries = function (item) {
                series.push(item);
            },

            removeSeries = function (seriesName) {
                var i;
                for (i = 0; i < series.length; i++) {
                    if (series[i].seriesName === seriesName) {
                        series.splice(i, 1);
                        break;
                    }
                }
            },

            updateColour = function (seriesName, newColour) {
                var i;
                for (i = 0; i < series.length; i++) {
                    if (series[i].seriesName === seriesName) {
                        series[i].colour = newColour;
                        break;
                    }
                }
            },

            updateY2Checked = function (seriesName, checked) {
                var i;
                for (i = 0; i < series.length; i++) {
                    if (series[i].seriesName === seriesName) {
                        series[i].y2Checked = checked;
                        break;
                    }
                }
            }

        return {
            getSeries,
            addSeries,
            removeSeries,
            updateColour,
            updateY2Checked
        }
    }());

    function refreshComponents() {
        $.ajax({
            url: "GetComponentsForProjectAndScenario",
            method: "POST",
            data: {
                projectId: getProjectId(),
                scenarioName: getScenarioName()
            }
        }).done(function (result) {
            resetComponentsList(result);
        });
    }

    function resetList(newList, callback) {
        var i;
        var liElements = [];
        var listItem;
        var liElement;

        for (i = 0; i < newList.length; i++) {
            listItem = newList[i];
            liElement =
                '<option value="' + listItem.name + '">' + listItem.name + '</option>';
            liElements.push(liElement);
        }
        
        getScenarioElement().empty().append(liElements);
        getScenarioElement().change(refreshComponents);
        if (callback) {
            callback();
        }
    }
    
    function displayOnChart() {
        RbatApp.Chart.resetChart();
        var series = selectedSeries.getSeries();
        if (series.length > 0) {
            showProgress();
            $.ajax({
                type: 'POST',
                url: "GetChartData",
                data: {
                    seriesSpecs: series,
                    chartType: getChartType(),
                    startDate: getStartDate(),
                    endDate: getEndDate()
                }
            }).done(function (projectData) {
                hideProgress();
                if (projectData.length > 0) {
                    RbatApp.Chart.loadChartData(projectData, series, getChartType());
                }
            });
        }
    }
    
    function updateColor(event) {
        var seriesName = $(this).data("seriesname");
        var newColour = event.target.value;
        selectedSeries.updateColour(seriesName, newColour);
        var series = selectedSeries.getSeries();
        RbatApp.Chart.updateSeriesColor(series);
    }

    function resetComponentsList(solutions) {
        var i;
        var nodeOrChannel;
        var channelElement;
        var liElements = [];
        for (i = 0; i < solutions.length; i++) {
            nodeOrChannel = solutions[i];
            channelElement = '<option value="' + nodeOrChannel.id + '" data-type="' + nodeOrChannel.type + '">' + nodeOrChannel.name + '</option>';

            liElements.push(channelElement);
        }
        getComponentElement().empty().append(liElements);
    }

    function refreshScenarios() {
        $.ajax({
            url: "GetProjectScenarios",
            data: {
                projectId: getProjectId()
            }
        }).done(function (projectScenarios) {
            resetList(projectScenarios, refreshComponents);
        });
    }

    getProjectListElement().change(function () {
        refreshScenarios();
    });
    
    $("#chartType").change(function () {
        if (getChartType() === "Exceedence") {
            getExceedenceChartControls().show();
        } else {
            getExceedenceChartControls().hide();
            setStartDate("");
            setEndDate("");
        }
    });

    function shouldShowY2Checkbox(index) {
        return getChartType() === "TimeSeries" && index >= 1;
    }
    
    function refreshSeriesList() {
        var htmlItems = [];
        var i;
        var seriesData = selectedSeries.getSeries();
        var seriesItem;
        for (i = 0; i < seriesData.length; i++) {
            seriesItem = seriesData[i];

            var selectedComponentHtml =
                `<div class="row">` +
                    `<div class="col">` +
                        `<span class="text-danger">
                            <i class="fas fa-times mr-3 fa-lg cursor-pointer delete-button" data-seriesname="` + seriesItem.seriesName + `"></i>
                        </span>`
                     + `<input type="color" value="` + seriesItem.colour + `" class="mr-2" data-seriesname="` + seriesItem.seriesName + `">`
                     + seriesItem.seriesName +
                       (shouldShowY2Checkbox(i) ? `<input type="checkbox"` + (seriesItem.y2Checked ? ` checked` : ` `) + ` class="ml-3 mr-1 y2-checkbox" data-seriesname="` + seriesItem.seriesName + `">Y2` : ` `) +
                    `</div>
                </div>`;

            htmlItems.push(selectedComponentHtml);
        }
        $("#selectedComponents").html("");
        $("#selectedComponents").append(htmlItems);
        $(".delete-button").click(function () {
            disableAddButton(false);
            var seriesName = $(this).data("seriesname");
            selectedSeries.removeSeries(seriesName);
            refreshSeriesList();
        });
        $("input[type=color]").change(updateColor);
        $(".y2-checkbox").change(function () {
            var seriesName = $(this).data("seriesname");
            selectedSeries.updateY2Checked(seriesName, this.checked);
        });
    }
    
    function makeUpSeriesName(simulatedOrIdealCheckbox) {
        return (getComponentType() === 0 ? "NODE" : "CHANNEL") + " - " + getProjectName() + " - " +
            getScenarioName() + " - " + getComponentName() + " - " + simulatedOrIdealCheckbox.data().name;
    }

    function getSeriesSpecs(simulatedOrIdealCheckbox) {
        return {
            seriesName: makeUpSeriesName(simulatedOrIdealCheckbox),
            valueType: simulatedOrIdealCheckbox.val(),
            type: getComponentType(),
            componentId: getComponent(),
            colour: "#e66465",
            y2Checked: false
        };
    }

    $("#addButton").click(function () {
        if (!(getProjectId() && getScenarioName() && getComponent())) {
            return;
        }

        // Can't have more than 4 series at a time
        if (selectedSeries.getSeries().length === 3 && getSimulatedCheckbox().is(":checked") && getIdealCheckbox().is(":checked")) {
            return;
        }

        // If none of the value type checkboxes is checked, there are no series to add
        if (!getSimulatedCheckbox().is(":checked") && !getIdealCheckbox().is(":checked")) {
            return;
        }

        if (selectedSeries.getSeries().length >= 4) {
            disableAddButton(true);
            return;
        }
        
        if (getSimulatedCheckbox().is(":checked")) {
            var simulatedSeries = getSeriesSpecs(getSimulatedCheckbox());
            selectedSeries.addSeries(simulatedSeries);
        }

        if (getIdealCheckbox().is(":checked")) {
            var idealSeries = getSeriesSpecs(getIdealCheckbox());
            selectedSeries.addSeries(idealSeries);
        }

        refreshSeriesList();
       
        if (selectedSeries.getSeries().length >= 4) {
            disableAddButton(true);
            return;
        }

        // reset Simulated and Ideal checkboxes
        getSimulatedCheckbox().prop("checked", true);
        getIdealCheckbox().prop("checked", true);

    });
    
    $("#openSetupButton").click(function () {
        $('.collapse').collapse('show');
        $("#openSetupButton").addClass("hidden");
        $("#closeSetupButton").removeClass("hidden");
    });

    $("#closeSetupButton").click(function () {
        $('.collapse').collapse('hide');
        $("#closeSetupButton").addClass("hidden");
        $("#openSetupButton").removeClass("hidden");
    });

    $("#refreshButton").click(function () {
        displayOnChart();
    });

    $("#downloadButton").click(function () {
        var data = RbatApp.Chart.getChartData();
        var csvContent = "";
        var labels = RbatApp.Chart.getLabels();

        var labelsString = "";
        labels.forEach(function (label) {
            labelsString += (label + ",");
        });
        
        csvContent += labelsString + "\r\n";

        data.forEach(function (rowArray) {
            let row = rowArray.join(",");
            csvContent += row + "\r\n";
        });

        var filename = "ChartData.csv";
        
        // download the file
        var blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
        var link = document.createElement("a");
        var url = window.URL.createObjectURL(blob);
        link.setAttribute("href", url);
        link.setAttribute("download", filename);
        link.style.visibility = 'hidden';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
    });

    $("#downloadImageButton").click(function () {
        RbatApp.Chart.downloadImage();
    });

    function setComponent(componentId, componentType) {
        getComponents().each(function () {
            if ($(this).val() === componentId.toString() && $(this).data().type === componentType) {
                $(this).prop("selected", true);
            }
        });        
    }

    $('.collapse').collapse('hide');

    var projectId = $('#projectId').val();
    var nodeId = $("#nodeId").val();
    var channelId = $("#channelId").val();
    
    if (projectId) {
        getProjectListElement().val(projectId);
    }

    if (projectId && (nodeId || channelId)) {

        $.ajax({
            url: "GetProjectDataForComponent",
            data: {
                projectId,
                nodeId,
                channelId
            }
        }).done(function (result) {
            $("#openSetupButton").click();
            resetList(result.projectScenarios);
            getScenarioElement().val(result.selectedScenario);
            resetComponentsList(result.projectComponents);
            setComponent(result.componentId, result.componentType);
            $("#addButton").click();
            displayOnChart();
        });
    }
}());