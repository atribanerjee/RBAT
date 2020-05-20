using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RBAT.Logic;
using RBAT.Logic.Interfaces;
using RBAT.Web.Models.ReportingTools;

namespace RBAT.Web.Controllers
{
    [Authorize]
    public class ReportingToolsController : Controller
    {
        private readonly IScenarioService _scenarioService;
        private readonly IOptimalSolutionsService _optimalSolutionsService;
        private readonly DropDownService _dropDownService;

        public ReportingToolsController(IScenarioService scenarioService, IOptimalSolutionsService optimalSolutionsService, DropDownService dropDownService)
        {
            this._scenarioService = scenarioService;
            this._optimalSolutionsService = optimalSolutionsService;
            this._dropDownService = dropDownService;
        }

        public IActionResult Chart(int? projectId, int? channelId, int? nodeId)
        {
            var model = new ChartModel
            {
                ProjectId = projectId,
                ChannelId = channelId,
                NodeId = nodeId
            };

            return View(model);
        }

        public IActionResult NodesDeficitTables()
        {
            return View();
        }

        public IActionResult ChannelsDeficitTables()
        {
            return View();
        }

        public IActionResult DeficitTablesData(int? projectID, string scenarioName, int? componentID, bool isNodeComponent, string deficitType)
        {
            var model = new DeficitTableModel();
            if (isNodeComponent)
            {
                var list = this._optimalSolutionsService.GetNodeDeficitTableData(projectID, scenarioName, componentID, deficitType);
                model.ColumnValue = list;
            }
            else
            {
                var list = this._optimalSolutionsService.GetChannelDeficitTableData(projectID, scenarioName, componentID, deficitType);
                model.ColumnValue = list;
            }

            return View(model);
        }
        
        public JsonResult GetProjectDataForComponent(int projectId, int? channelId, int? nodeId)
        {
            var scenarios = this._optimalSolutionsService.GetScenariosNameByProjectID(projectId).ToProjectScenarioViewModelList();
            var componentScenarios = new List<string>();

            if (channelId.HasValue)
            {
                componentScenarios = this._optimalSolutionsService.GetScenariosByChannelId(projectId, channelId.Value).ToList();
            }
            else
            {
                componentScenarios = this._optimalSolutionsService.GetScenariosByNodeId(projectId, nodeId.Value).ToList();
            }

            var selectedScenario = componentScenarios.FirstOrDefault();

            var components = GetComponentViewModelListForProjectAndScenario(projectId, selectedScenario);

            var selectedComponent = new ComponentViewModel();
            if (channelId.HasValue)
            {
                selectedComponent = components.FirstOrDefault(x => x.ChannelId == channelId && x.Type == ComponentType.Channel);
            }
            else
            {
                selectedComponent = components.FirstOrDefault(x => x.NodeId == nodeId && !x.IsNetEvaporation && x.Type == ComponentType.Node);
            }

            return Json(new
                {
                    ProjectScenarios = scenarios,
                    SelectedScenario = selectedScenario,
                    ProjectComponents = components,
                    ComponentId = selectedComponent?.Id,
                    ComponentType = selectedComponent?.Type
                });
        }
        
        public JsonResult GetProjectScenarios(int? projectId)
        {
            var scenarios = new ProjectScenarioModelList();
            if (projectId.HasValue)
            {
                var projectScenarios = this._optimalSolutionsService.GetScenariosNameByProjectID(projectId.Value);
                // var projectScenarios = this._scenarioService.GetAllByProjectID(projectId.Value);
                scenarios = projectScenarios.ToProjectScenarioViewModelList();
            }
            return Json(scenarios);
        }

        [HttpPost]
        public JsonResult GetComponentsForProjectAndScenario(int projectId, string scenarioName)
        {
            var result = GetComponentViewModelListForProjectAndScenario(projectId, scenarioName);
            return Json(result);
        }

        private ComponentViewModelList GetComponentViewModelListForProjectAndScenario(int projectId, string scenarioName)
        {  
            var scenarioNodes = this._optimalSolutionsService.GetNodes(projectId, scenarioName);
            var result = scenarioNodes.ToComponentViewModelList();
            var scenarioChannels = this._optimalSolutionsService.GetChannels(projectId, scenarioName);
            var channelComponents = scenarioChannels.ToComponentViewModelList();
            result.AddRange(channelComponents);
            return result;
        }
        
        private async Task<List<KeyValuePair<DateTime, double?>>> GetSeriesData(SeriesSpec spec, ChartType chartType, List<DateTime> allDates)
        {
            var result = new List<KeyValuePair<DateTime, double?>>();
            if (chartType == ChartType.TimeSeries)
            {
                if (spec.Type == ComponentType.Node)
                {
                    var nodeData = (await this._optimalSolutionsService.GetNodeChartData(spec.ComponentId)).ToList();

                    result = nodeData
                        .Select((s) =>
                        {
                            var value = ResolveNodeValue(s, spec.ValueType);
                            return new KeyValuePair<DateTime, double?> (s.SurveyDate, value);
                        })
                        .ToList();
                }
                else
                {
                    var channelData = (await this._optimalSolutionsService.GetChannelChartData(spec.ComponentId)).ToList();
                    result = channelData
                        .Select(s => new KeyValuePair<DateTime, double?> ( s.SurveyDate, spec.ValueType == SolutionValueType.Optimal ? s.OptimalValue : s.IdealValue))
                        .ToList();

                }
                foreach (var item in result)
                {
                    allDates.Add(item.Key);
                }
            }
            else if (chartType == ChartType.Stepped)
            {
                if (spec.Type == ComponentType.Node)
                {
                    var allData = (await this._optimalSolutionsService.GetNodeChartSteppedData(spec.ComponentId)).ToList();
                    for (var i = 0; i < allData.Count(); i++)
                    {
                        //Do not go thru the last record
                        if (i + 1 == allData.Count())
                        {
                            break;
                        }

                        var value = ResolveNodeValue(allData[i + 1], spec.ValueType);
                        result.Add(new KeyValuePair<DateTime, double?> ( allData[i].SurveyDate, value));
                        result.Add(new KeyValuePair<DateTime, double?> ( allData[i + 1].SurveyDate, value));
                    };
                }
                else
                {
                    var allData = (await this._optimalSolutionsService.GetChannelChartSteppedData(spec.ComponentId)).ToList();
                    for (var i = 0; i < allData.Count(); i++)
                    {
                        //Do not go thru the last record
                        if (i + 1 == allData.Count())
                        {
                            break;
                        }

                        var value = spec.ValueType == SolutionValueType.Optimal ? allData[i + 1].OptimalValue : allData[i + 1].IdealValue;
                        result.Add(new KeyValuePair<DateTime, double?> (allData[i].SurveyDate, value));
                        result.Add(new KeyValuePair<DateTime, double?> (allData[i + 1].SurveyDate, value));
                    };
                }
            }
            
            return result;
        }

        private async Task<List<KeyValuePair<double, double?>>> GetExceedenceSeriesData(SeriesSpec spec, DateTime? startDate, DateTime? endDate, List<double> allXValues)
        {
           var data = this._optimalSolutionsService.GetChannelSubsetData(8100, 1, 3, 2000, 1, 3, 2002);

            var result = new List<KeyValuePair<double, double?>>();
            if (spec.Type == ComponentType.Node)
            {
                var allData = (await this._optimalSolutionsService.GetNodeExceedenceChartData(spec.ComponentId, spec.ValueType.ToString(), startDate, endDate)).ToList();
                var numberOfRecords = allData.Count() + 1;
                for (var i = 0; i < allData.Count(); i++)
                {
                    var value = ResolveNodeValue(allData[i], spec.ValueType);
                    var probabilityOfExceedence = ((double)(i) / (numberOfRecords + 1));  //P = 100 × (m ÷ (n + 1))
                    result.Add(new KeyValuePair<double, double?> 
                    (
                        Math.Round(probabilityOfExceedence, 4, MidpointRounding.AwayFromZero),
                        Math.Round(value.GetValueOrDefault(0), 4, MidpointRounding.AwayFromZero)
                    ));
                }
            }
            else
            {   
                var allData = (await this._optimalSolutionsService.GetChannelExceedenceChartData(spec.ComponentId, spec.ValueType.ToString(), startDate, endDate)).ToList();
                var numberOfRecords = allData.Count() + 1;
                for (var i = 0; i < allData.Count(); i++)
                {
                    var value = spec.ValueType == SolutionValueType.Optimal ? allData[i].OptimalValue : allData[i].IdealValue;
                    var probabilityOfExceedence = ((double)(i) / (numberOfRecords + 1));  //P =  (m ÷ (n + 1))
                    result.Add(new KeyValuePair<double, double?>
                    (
                        Math.Round(probabilityOfExceedence, 4, MidpointRounding.AwayFromZero),
                        Math.Round(value.GetValueOrDefault(0), 4, MidpointRounding.AwayFromZero)
                    ));
                }
            }
            foreach (var item in result)
            {
                allXValues.Add(item.Key);
            }

            return result;
        }
        
        private double? LookUpSeriesValueByDate(DateTime date, List<KeyValuePair<DateTime, double?>> seriesData)
        {
            var val = seriesData.FirstOrDefault(s => s.Key == date);
            return (val.Equals(default(KeyValuePair<DateTime, double?>))) ? null : val.Value;
        }

        private double? LookUpSeriesValueByXValue(double xValue, List<KeyValuePair<double, double?>> seriesData)
        {
            var val = seriesData.FirstOrDefault(s => s.Key == xValue);
            return (val.Equals(default(KeyValuePair<double, double?>))) ? null : val.Value;
        }

        [HttpPost]
        public async Task<JsonResult> GetChartData(List<SeriesSpec> seriesSpecs, ChartType chartType, DateTime? startDate, DateTime? endDate)
        {
            int numberOfSeries = seriesSpecs.Count();
            
            if (chartType == ChartType.TimeSeries || chartType == ChartType.Stepped)
            {
                var result = new List<ChartPoint>();
                
                List<KeyValuePair<DateTime, double?>>[] allSeries = new List<KeyValuePair<DateTime, double?>>[4];
                allSeries[0] = new List<KeyValuePair<DateTime, double?>>();
                allSeries[1] = new List<KeyValuePair<DateTime, double?>>();
                allSeries[2] = new List<KeyValuePair<DateTime, double?>>();
                allSeries[3] = new List<KeyValuePair<DateTime, double?>>();

                var allDates = new List<DateTime>();
                    
                for (var i = 0; i < allSeries.Length; i++)
                {   
                    if (numberOfSeries >= i+1)
                    {
                        allSeries[i] = await GetSeriesData(seriesSpecs[i], chartType, allDates);
                    }
                }

                if (chartType == ChartType.TimeSeries)
                {
                    var distinctDates = allDates.Distinct().OrderBy(d => d);

                    foreach (var date in distinctDates)
                    {
                        var chartPoint = new ChartPoint
                        {
                            X = date.ToString(),
                            Y1 = LookUpSeriesValueByDate(date, allSeries[0]),
                            Y2 = LookUpSeriesValueByDate(date, allSeries[1]),
                            Y3 = LookUpSeriesValueByDate(date, allSeries[2]),
                            Y4 = LookUpSeriesValueByDate(date, allSeries[3])
                        };
                        result.Add(chartPoint);
                    }
                }
                else
                {
                    foreach(var series1 in allSeries[0])
                    {
                        var chartPoint = new ChartPoint
                        {
                            Date = series1.Key,
                            X = series1.Key.ToString(),
                            Y1 = series1.Value
                        };
                        result.Add(chartPoint);
                    }

                    foreach (var series2 in allSeries[1])
                    {
                        for (var i = 0; i < result.Count; i++)
                        {
                            var point = result[i];
                            if (point.Date == series2.Key && !point.Y2.HasValue)
                            {
                                point.Y2 = series2.Value;
                                break;
                            }
                            if (point.Date > series2.Key)
                            {
                                result.Insert(i, new ChartPoint
                                {
                                    Date = series2.Key,
                                    X = series2.Key.ToString(),
                                    Y1 = null,
                                    Y2 = series2.Value
                                });
                                break;
                            }
                            if (i+1 == result.Count && series2.Key >= point.Date)
                            {
                                result.Add(new ChartPoint
                                {
                                    Date = series2.Key,
                                    X = series2.Key.ToString(),
                                    Y1 = null,
                                    Y2 = series2.Value
                                });
                                break;
                            }
                        }
                    }
                    foreach (var series3 in allSeries[2])
                    {
                        for (var i = 0; i < result.Count; i++)
                        {
                            var point = result[i];
                            if (point.Date == series3.Key && !point.Y3.HasValue)
                            {
                                point.Y3 = series3.Value;
                                break;
                            }
                            if (point.Date > series3.Key)
                            {
                                result.Insert(i, new ChartPoint
                                {
                                    Date = series3.Key,
                                    X = series3.Key.ToString(),
                                    Y1 = null,
                                    Y2 = null,
                                    Y3 = series3.Value
                                });
                                break;
                            }
                            if (i + 1 == result.Count && series3.Key >= point.Date)
                            {
                                result.Add(new ChartPoint
                                {
                                    Date = series3.Key,
                                    X = series3.Key.ToString(),
                                    Y1 = null,
                                    Y2 = null,
                                    Y3 = series3.Value
                                });
                                break;
                            }
                        }
                    }
                    foreach (var series4 in allSeries[3])
                    {
                        for (var i = 0; i < result.Count; i++)
                        {
                            var point = result[i];
                            if (point.Date == series4.Key && !point.Y4.HasValue)
                            {
                                point.Y4 = series4.Value;
                                break;
                            }
                            if (point.Date > series4.Key)
                            {
                                result.Insert(i, new ChartPoint
                                {
                                    Date = series4.Key,
                                    X = series4.Key.ToString(),
                                    Y1 = null,
                                    Y2 = null,
                                    Y3 = null,
                                    Y4 = series4.Value
                                });
                                break;
                            }
                            if (i + 1 == result.Count && series4.Key >= point.Date)
                            {
                                result.Add(new ChartPoint
                                {
                                    Date = series4.Key,
                                    X = series4.Key.ToString(),
                                    Y1 = null,
                                    Y2 = null,
                                    Y3 = null,
                                    Y4 = series4.Value
                                });
                                break;
                            }
                        }
                    }
                }
                
                return Json(result);
            }
            else if (chartType == ChartType.Exceedence)
            {
                var result = new List<ExceedenceChartPoint>();
                
                List<KeyValuePair<double, double?>>[] allSeries = new List<KeyValuePair<double, double?>>[4];
                allSeries[0] = new List<KeyValuePair<double, double?>>();
                allSeries[1] = new List<KeyValuePair<double, double?>>();
                allSeries[2] = new List<KeyValuePair<double, double?>>();
                allSeries[3] = new List<KeyValuePair<double, double?>>();

                var allXValues = new List<double>();
                
                for (var i = 0; i < allSeries.Length; i++)
                {
                    if (numberOfSeries >= i + 1)
                    {
                        allSeries[i] = await GetExceedenceSeriesData(seriesSpecs[i], startDate, endDate, allXValues);
                    }
                }
                var distinctXValues = allXValues.Distinct().OrderBy(d => d);
                foreach (var xValue in distinctXValues)
                {
                    var chartPoint = new ExceedenceChartPoint
                    {
                        X = xValue,
                        Y1 = LookUpSeriesValueByXValue(xValue, allSeries[0]),
                        Y2 = LookUpSeriesValueByXValue(xValue, allSeries[1]),
                        Y3 = LookUpSeriesValueByXValue(xValue, allSeries[2]),
                        Y4 = LookUpSeriesValueByXValue(xValue, allSeries[3])
                    };
                    result.Add(chartPoint);
                }
                return Json(result);
            }
            else
            {
                throw new ArgumentException("Invalid chart type.");
            }
        }
        
        public class ChartPoint
        {
            public string X { get; set; }
            public DateTime Date { get; set; }
            public double? Y1 { get; set; }
            public double? Y2 { get; set; }
            public double? Y3 { get; set; }
            public double? Y4 { get; set; }
        }

        public class ExceedenceChartPoint
        {
            public double X { get; set; }
            public double? Y1 { get; set; }
            public double? Y2 { get; set; }
            public double? Y3 { get; set; }
            public double? Y4 { get; set; }
        }
        
        private double? ResolveNodeValue(Core.Models.NodeOptimalSolutionsData nodeOptimalSolution, SolutionValueType valueType)
        {
            double? value = null;
            if (valueType == SolutionValueType.Optimal)
            {
                value = Math.Round(nodeOptimalSolution.OptimalValue.GetValueOrDefault(0), 4, MidpointRounding.AwayFromZero);
            }
            //else if (valueType == "Optimal Net Evaporation")
            //{
            //    value = nodeOptimalSolution.OptimalNetEvaporation;
            //}
            else if (valueType == SolutionValueType.Ideal)
            {
                value = Math.Round(nodeOptimalSolution.IdealValue.GetValueOrDefault(0), 4, MidpointRounding.AwayFromZero);
            }
            //else if (valueType == "Ideal Net Evaporation")
            //{
            //    value = nodeOptimalSolution.IdealNetEvaporation;
            //}
            return value;
        }

        //public async Task<IActionResult> FillDeficitTableFromDB(int projectId, int scenarioId, int nodeId)
        //{
        //    try
        //    {
        //        var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
        //        var data =  await _optimalSolutionsService.GetAll(elementID);
        //        int recordsTotal = data.Count;
        //        return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { error = ex.Message });
        //    }
        //}
        public ActionResult ScenarioList(int projectID)
        {
            var scenarioList = this._dropDownService.ListScenarios(projectID);
            return Json(new { scenarioList });
        }

        public ActionResult ComponentsList(int projectID, string scenarioName, bool isNode)
        {
            if (isNode)
            {
                var nodeList = this._dropDownService.ListNodeComponents(projectID, scenarioName);
                return Json(new { nodeList });
            }
            else
            {
                var channelList = this._dropDownService.ListChannelComponents(projectID, scenarioName);
                return Json(new { channelList });
            }
        }
    }
}