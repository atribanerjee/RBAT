
// Modified functionality of Dygraph.Export.AsPNG to save the image to the user's machine
Dygraph.Export.downloadAsPNG = function (dygraph) {
    "use strict";
    var img = document.createElement("img");
    img.setAttribute("class", "hidden");
    document.body.appendChild(img);

    var canvas = Dygraph.Export.asCanvas(dygraph);
    img.src = canvas.toDataURL();
    canvas.toBlob(function (blob) {
        var url = window.URL.createObjectURL(blob);
        var link = document.createElement("a");
        link.setAttribute("href", url);
        link.setAttribute("download", "Chart.png");
        link.style.visibility = 'hidden';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
        document.body.removeChild(img);
    });
};

var RbatApp = RbatApp || {};

RbatApp.Chart = RbatApp.Chart || (function () {

    var chart1 = new Dygraph(
        document.getElementById("chart1"),
        [[new Date(), 0]],
        {
            showRangeSelector: true,
            labelsDiv: "chartLegend",
            labels: ["Survey Date", ""],
            labelsSeparateLines: true,
            connectSeparatedPoints: true,
            legendFormatter: function (data) {
                return data.series.map(element => {
                    return `<span style="color:${element.color};"><div class="dygraph-legend-line"></div> ${element.labelHTML}: ${element.yHTML || '-'}</span>`;
                }).join('<br/>');
            }//,
            //strokeWidth: 4.0
        }
    );
    
    function resetChart() {
        seriesCollection = [];
        chart1.updateOptions({ 'file': [[new Date(), 0]], 'labels': ["Survey Date", ""], 'colors': [], series: null });
    }
 
    var seriesCollection = [];
    
    function refreshChart(chartType, projectData) {
        var labels = chartType === "Exceedence" ? ["Probability of Exceedence"] : ["Survey Date"];
        var colors = [];

        for (i = 0; i < seriesCollection.length; i++) {
            labels.push(seriesCollection[i].seriesName);
            colors.push(seriesCollection[i].colour);
        }

        resetYAxis(chartType);
        chart1.updateOptions({ 'file': projectData, 'labels': labels, 'colors': colors });     
    }

    // Add or remove the second Y axis, if needed
    function resetYAxis(chartType) {
        var seriesOptions = {}, i;
        var doubleYs = false;
        var firstSeries = seriesCollection[0];
        if (chartType === "TimeSeries") {
            for (i = 1; i < seriesCollection.length; i++) {
                if (seriesCollection[i].y2Checked) {
                    doubleYs = true;
                    seriesOptions[seriesCollection[i].seriesName] = {
                        axis: 'y2'
                    };
                }
            }
            if (doubleYs) {
                chart1.updateOptions({
                    series: seriesOptions
                });
            } else {
                chart1.updateOptions({
                    series: null
                });
            }
        } else {
            chart1.updateOptions({
                series: null
            });
        }
    } 

    function loadChartData(projectData, seriesInfo, chartType) {
        var data = [];
        var point = [];
        if (chartType !== "Exceedence") {
            for (i = 0; i < projectData.length; i++) {
                point = [new Date(projectData[i].x)];
                if (seriesInfo.length >= 1) {
                    point.push(projectData[i].y1);
                }
                if (seriesInfo.length >= 2) {
                    point.push(projectData[i].y2);
                }
                if (seriesInfo.length >= 3) {
                    point.push(projectData[i].y3);
                }
                if (seriesInfo.length >= 4) {
                    point.push(projectData[i].y4);
                }
                data.push(point);
            }
        }
        else {
            for (i = 0; i < projectData.length; i++) {
                point = [projectData[i].x];
                if (seriesInfo.length >= 1) {
                    point.push(projectData[i].y1);
                }
                if (seriesInfo.length >= 2) {
                    point.push(projectData[i].y2);
                }
                if (seriesInfo.length >= 3) {
                    point.push(projectData[i].y3);
                }
                if (seriesInfo.length >= 4) {
                    point.push(projectData[i].y4);
                }
                data.push(point);
            }
        }

        seriesCollection = seriesInfo;
        
        refreshChart(chartType, data);
    }
    
    function updateSeriesColor(seriesInfo) {
        var colours = [];
        var i,j;
        for (i = 0; i < seriesInfo.length; i++) {
            for (j = 0; j < seriesCollection.length; j++) {
                if (seriesCollection[j].seriesName === seriesInfo[i].seriesName && seriesCollection[j].colour !== seriesInfo[i].colour) {
                    seriesCollection[j].colour = seriesInfo[i].colour;
                }
            }
        }
        for (i = 0; i < seriesCollection.length; i++) {
            colours.push(seriesCollection[i].colour);
        }
        chart1.updateOptions({ 'colors': colours });
    }

    function getChartData() {
        return chart1.file_;
    }

    function getLabels() {
        return chart1.getLabels();
    }

    function downloadImage() {
        Dygraph.Export.downloadAsPNG(chart1);
    }
   
    return {
        loadChartData,
        updateSeriesColor,
        resetChart,
        getChartData,
        getLabels,
        downloadImage
    };
}());