using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RBAT.Core;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Logic
{
    public class OptimalSolutionsService : IOptimalSolutionsService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly ILogger<ProjectService> _logger;

        public OptimalSolutionsService(ILogger<ProjectService> logger, IHttpContextAccessor contextAccessor)
        {
            this._logger = logger;
            this._contextAccessor = contextAccessor;
        }

        public IEnumerable<ChannelOptimalSolutions> GetChannels(int projectId, string scenarioName)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var scenarioChannels = ctx.ChannelOptimalSolutions
                    .Include(s => s.Channel)
                    .Where(s => s.ProjectID == projectId && s.ScenarioName == scenarioName)
                    .OrderBy(c => c.ChannelID)
                    .AsQueryable()
                    .ToList();

                return scenarioChannels;
            }
        }

        public IEnumerable<NodeOptimalSolutions> GetNodes(int projectId, string scenarioName)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var scenarioNodes = ctx.NodeOptimalSolutions
                    .Include(s => s.Node)
                    .Where(s => s.ProjectID == projectId && s.ScenarioName == scenarioName)
                    .OrderBy(c => c.NodeID)
                    .AsQueryable()
                    .ToList();

                return scenarioNodes;
            }
        }

        public async Task<IEnumerable<ChannelOptimalSolutionsData>> GetChannelChartData(long channelOptimalSolutionsId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var chartData = ctx.ChannelOptimalSolutionsData
                    .Where(s => s.ChannelOptimalSolutionsID == channelOptimalSolutionsId)
                    .OrderBy(x => x.SurveyDate)
                    .AsQueryable();

                return await chartData.ToListAsync();
            }
        }

        public async Task<IEnumerable<ChannelOptimalSolutionsData>> GetChannelChartSteppedData(long channelOptimalSolutionsId)
        {
            var chartDataList = new List<ChannelOptimalSolutionsData>();

            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var optimalSolutionsData = await ctx.ChannelOptimalSolutionsData
                    .Where(s => s.ChannelOptimalSolutionsID == channelOptimalSolutionsId).OrderBy(x => x.SurveyDate).ToListAsync();

                var channelOptimalSolution = ctx.ChannelOptimalSolutions
                    .Where(s => s.Id == channelOptimalSolutionsId).FirstOrDefault();

                if (channelOptimalSolution != null)
                {
                    if (optimalSolutionsData.Any())
                    {
                        var first = new ChannelOptimalSolutionsData();
                        first.OptimalValue = optimalSolutionsData.First().OptimalValue;
                        first.IdealValue = optimalSolutionsData.First().IdealValue;
                        first.SurveyDate = channelOptimalSolution.SimulationStartDate.GetValueOrDefault(optimalSolutionsData.First().SurveyDate);

                        chartDataList.Insert(0, first);

                        foreach (var item in optimalSolutionsData)
                        {
                            chartDataList.Add(new ChannelOptimalSolutionsData { IdealValue = item.IdealValue, OptimalValue = item.OptimalValue, SurveyDate = item.SurveyDate });
                        }
                    }
                }
            }

            return chartDataList;
        }

        public async Task<IEnumerable<NodeOptimalSolutionsData>> GetNodeChartData(long nodeOptimalSolutionsId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var chartData = ctx.NodeOptimalSolutionsData
                    .Where(s => s.NodeOptimalSolutionsID == nodeOptimalSolutionsId).OrderBy(x => x.SurveyDate)
                    .AsQueryable();

                return await chartData.ToListAsync();
            }
        }

        public async Task<IEnumerable<NodeOptimalSolutionsData>> GetNodeChartSteppedData(long nodeOptimalSolutionsId)
        {
            var chartDataList = new List<NodeOptimalSolutionsData>();

            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var optimalSolutionsData = await ctx.NodeOptimalSolutionsData
                    .Where(s => s.NodeOptimalSolutionsID == nodeOptimalSolutionsId).OrderBy(x => x.SurveyDate).ToListAsync();

                var nodeOptimalSolution = ctx.NodeOptimalSolutions
                    .Where(s => s.Id == nodeOptimalSolutionsId).FirstOrDefault();

                if (nodeOptimalSolution != null)
                {
                    if (optimalSolutionsData.Any())
                    {
                        var first = new NodeOptimalSolutionsData();
                        first.OptimalValue = optimalSolutionsData.First().OptimalValue;
                        first.IdealValue = optimalSolutionsData.First().IdealValue;
                        first.SurveyDate = nodeOptimalSolution.SimulationStartDate.GetValueOrDefault(optimalSolutionsData.First().SurveyDate);

                        chartDataList.Insert(0, first);

                        foreach (var item in optimalSolutionsData)
                        {
                            chartDataList.Add(new NodeOptimalSolutionsData { IdealValue = item.IdealValue, OptimalValue = item.OptimalValue, SurveyDate = item.SurveyDate });
                        }
                    }
                }
            }

            return chartDataList;
        }

        public IEnumerable<string> GetScenariosByChannelId(int projectId, int channelId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var scenarios = ctx.ChannelOptimalSolutions
                    .Include(s => s.Channel)
                    .Where(s => s.ProjectID == projectId && channelId == s.ChannelID)
                    .OrderBy(c => c.ScenarioName)
                    .AsQueryable()
                    .Select(s => s.ScenarioName)
                    .ToList();

                return scenarios;
            }
        }

        public IEnumerable<string> GetScenariosByNodeId(int projectId, int nodeId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var scenarios = ctx.NodeOptimalSolutions
                    .Include(s => s.Node)
                    .Where(s => s.ProjectID == projectId && nodeId == s.NodeID)
                    .OrderBy(c => c.ScenarioName)
                    .AsQueryable()
                    .Select(s => s.ScenarioName)
                    .ToList();

                return scenarios;
            }
        }

        public async Task<IEnumerable<ChannelOptimalSolutionsData>> GetChannelExceedenceChartData(long channelOptimalSolutionsId, string valueType, DateTime? startDate, DateTime? endDate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var channelOptimalSolutionsData = ctx.ChannelOptimalSolutionsData
                    .Where(s => s.ChannelOptimalSolutionsID == channelOptimalSolutionsId).OrderBy(x => x.SurveyDate)
                    .AsQueryable();

                //Get all data between start and end date for each year from start to end year
                if (startDate != null && endDate != null)
                {
                    var chartData = new List<ChannelOptimalSolutionsData>();

                    var year = 0;
                    var startDateNew = startDate;
                    var endDateNew = endDate;
                    foreach (var data in channelOptimalSolutionsData)
                    {
                        if (data.SurveyDate.Year != year)
                        {
                            year = data.SurveyDate.Year;
                            startDateNew = new DateTime(year, startDate.Value.Month, startDate.Value.Day);
                            endDateNew = new DateTime(year, endDate.Value.Month, endDate.Value.Day);
                        }

                        if (data.SurveyDate >= startDateNew && data.SurveyDate <= endDateNew)
                        {
                            chartData.Add(data);
                        }
                    }

                    if (valueType.ToLower() == "optimal")
                    {
                        chartData = new List<ChannelOptimalSolutionsData>(
                            chartData.OrderByDescending(o => o.OptimalValue));
                    }
                    else if (valueType.ToLower() == "ideal")
                    {
                        chartData = new List<ChannelOptimalSolutionsData>(
                            chartData.OrderByDescending(o => o.IdealValue));
                    }

                    return chartData;
                }

                if (valueType.ToLower() == "optimal")
                {
                    channelOptimalSolutionsData = channelOptimalSolutionsData.OrderByDescending(o => o.OptimalValue);
                }
                else if (valueType.ToLower() == "ideal")
                {
                    channelOptimalSolutionsData = channelOptimalSolutionsData.OrderByDescending(o => o.IdealValue);
                }

                return await channelOptimalSolutionsData.ToListAsync();
            }
        }

        public async Task<IEnumerable<NodeOptimalSolutionsData>> GetNodeExceedenceChartData(long nodeOptimalSolutionsId, string valueType, DateTime? startDate, DateTime? endDate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var nodeOptimalSolutionsData = ctx.NodeOptimalSolutionsData
                    .Where(s => s.NodeOptimalSolutionsID == nodeOptimalSolutionsId).OrderBy(x => x.SurveyDate)
                    .AsQueryable();

                if (startDate != null && endDate != null)
                {
                    var chartData = new List<NodeOptimalSolutionsData>();

                    var year = 0;
                    var startDateNew = startDate;
                    var endDateNew = endDate;
                    foreach (var data in nodeOptimalSolutionsData)
                    {
                        if (data.SurveyDate.Year != year)
                        {
                            year = data.SurveyDate.Year;
                            startDateNew = new DateTime(year, startDate.Value.Month, startDate.Value.Day);
                            endDateNew = new DateTime(year, endDate.Value.Month, endDate.Value.Day);
                        }

                        if (data.SurveyDate >= startDateNew && data.SurveyDate <= endDateNew)
                        {
                            chartData.Add(data);
                        }
                    }

                    if (valueType.ToLower() == "optimal")
                    {
                        chartData = new List<NodeOptimalSolutionsData>(chartData.OrderByDescending(o => o.OptimalValue));
                    }
                    else if (valueType.ToLower() == "ideal")
                    {
                        chartData = new List<NodeOptimalSolutionsData>(chartData.OrderByDescending(o => o.IdealValue));
                    }

                    return chartData.ToList();
                }

                if (valueType.ToLower() == "optimal")
                {
                    nodeOptimalSolutionsData = nodeOptimalSolutionsData.OrderByDescending(o => o.OptimalValue);
                }
                //else if (valueType == "Optimal Net Evaporation")
                //{
                //    chartData = chartData.OrderBy(o => o.OptimalNetEvaporation);
                //}
                else if (valueType.ToLower() == "ideal")
                {
                    nodeOptimalSolutionsData = nodeOptimalSolutionsData.OrderByDescending(o => o.IdealValue);
                }
                //else if (valueType == "Ideal Net Evaporation")
                //{
                //    chartData = chartData.OrderBy(o => o.IdealNetEvaporation);
                //}

                return  await nodeOptimalSolutionsData.ToListAsync();
            }
        }

        public List<List<string>> GetNodeDeficitTableData(int? projectId, string scenarioName, int? nodeId, string deficitType)
        {
            
            var deficitTable = new List<List<string>>();
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                if (deficitType == "annualDeficit")
                {
                    var nodeOptimalSolutionsList = ctx.NodeOptimalSolutions.Include(x => x.Node).Where(x => x.ProjectID == projectId && x.ScenarioName == scenarioName && x.Node.NodeTypeId == 2);

                    //Write first row
                    var dataNodeGrouped = nodeOptimalSolutionsList.GroupBy(x => x.Node.Name);
                    var maxCountOfColumns = dataNodeGrouped.Max(g => g.Count());

                    var row = new List<string>
                    {
                        "Year"
                    };

                    foreach (var group in dataNodeGrouped)
                    {
                        row.Add(group.Key);
                    }

                    deficitTable.Add(row);

                    //write all rows
                    var nodeOptimalSolutionFirst = nodeOptimalSolutionsList.First();
                    var nodeOptimalSolutionsData = ctx.NodeOptimalSolutionsData.Where(x => x.NodeOptimalSolutionsID == nodeOptimalSolutionFirst.Id).OrderBy(x => x.SurveyDate);

                    var dataYearGrouped = nodeOptimalSolutionsData.GroupBy(x => x.SurveyDate.Year);
                    foreach (var group in dataYearGrouped)
                    {
                        var rowData = new List<string>();
                        rowData.Add(group.Key.ToString());

                        foreach (var groupNode in dataNodeGrouped)
                        {
                            var nodeOptimalSolution = nodeOptimalSolutionsList.FirstOrDefault(x => x.Node.Name == groupNode.Key);
                            if (nodeOptimalSolution != null)
                            {
                                var data = ctx.NodeOptimalSolutionsData.Where(x => x.NodeOptimalSolutionsID == nodeOptimalSolution.Id && x.SurveyDate.Year == group.Key);

                                var idealValueSum = data.Sum(x => x.IdealValue);
                                var optimalValueSum = data.Sum(x => x.OptimalValue);

                                if (idealValueSum == null || idealValueSum.Value == 0)
                                {
                                    rowData.Add("0");
                                }
                                else
                                {
                                    var annualDeficitValue = Math.Round((idealValueSum.GetValueOrDefault() - optimalValueSum.GetValueOrDefault()) / idealValueSum.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero);
                                    rowData.Add(annualDeficitValue.ToString(CultureInfo.InvariantCulture));
                                }
                            }

                        }

                        deficitTable.Add(rowData);
                    }

                    return ResolveDeficitTable(deficitTable);
                }

                //Deficit tables are only for Consumptive Use, don't need to include IsNetEvaporation
                var nodeOptimalSolutions = ctx.NodeOptimalSolutions.FirstOrDefault(x => x.ProjectID == projectId && x.ScenarioName == scenarioName && x.NodeID == nodeId);

                if (nodeOptimalSolutions != null)
                {
                    var data = ctx.NodeOptimalSolutionsData
                        .Where(x => x.NodeOptimalSolutionsID == nodeOptimalSolutions.Id).OrderBy(x => x.SurveyDate);

                    if (!data.Any())
                    {
                        return null;
                    }

                    var dataGrouped = data.GroupBy(x => x.SurveyDate.Year);
                    var maxCountOfColumns = dataGrouped.Max(g => g.Count());

                   var row = new List<string>
                    {
                        "Year"
                    };

                    for (int i = 1; i <= maxCountOfColumns; i++)
                    {
                        row.Add(i.ToString());
                    }

                    deficitTable.Add(row);

                    foreach (var group in dataGrouped)
                    {
                        var rowData = new List<string>();
                        rowData.Add(group.Key.ToString());

                        foreach (var value in group.ToList())
                        {
                            var a = value.IdealValue.GetValueOrDefault(0);
                            var b = value.OptimalValue.GetValueOrDefault(0);

                            if (deficitType == "absoluteDeficit")
                            {
                                var absoluteValue = Math.Round(a - b, 2, MidpointRounding.AwayFromZero);
                                rowData.Add(absoluteValue.ToString(CultureInfo.InvariantCulture));
                            }
                            if (deficitType == "relativeDeficit")
                            {
                                if (a == 0)
                                {
                                    rowData.Add("0");
                                }
                                else
                                {
                                    var relativeValue = Math.Round((a - b) / a, 2, MidpointRounding.AwayFromZero);
                                    rowData.Add(relativeValue.ToString(CultureInfo.InvariantCulture));
                                }
                            }
                        }

                        deficitTable.Add(rowData);
                    }
                }
            }
            return ResolveDeficitTable(deficitTable);
        }

        //Add columns to match number of columns in the first row
        private List<List<string>> ResolveDeficitTable(List<List<string>> deficitTable)
        {
            if (deficitTable == null || deficitTable.Count == 0)
            {
                return deficitTable;
            }

            var firstRowNumberOfColumns = deficitTable.First().Count;

            foreach (var row in deficitTable.Skip(1))
            {
                var toAdd = firstRowNumberOfColumns - row.Count;
                for (int i = 0; i < toAdd; i++)
                {
                    row.Add("");
                }
            }

            return deficitTable;
        }

        public List<List<string>> GetChannelDeficitTableData(int? projectId, string scenarioName, int? channelId, string deficitType)
        {
            var deficitTable = new List<List<string>>();
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var channelOptimalSolutions = ctx.ChannelOptimalSolutions.FirstOrDefault(x => x.ProjectID == projectId && x.ScenarioName == scenarioName && x.ChannelID == channelId);
                if (channelOptimalSolutions != null)
                {
                    var data = ctx.ChannelOptimalSolutionsData
                        .Where(x => x.ChannelOptimalSolutionsID == channelOptimalSolutions.Id).OrderBy(x => x.SurveyDate);

                    if (!data.Any())
                    {
                        return null;
                    }

                    var dataGrouped = data.GroupBy(x => x.SurveyDate.Year);
                    var maxCountOfColumns = dataGrouped.Max(g => g.Count());

                    var row = new List<string>
                    {
                        "Year"
                    };

                    for (int i = 1; i <= maxCountOfColumns; i++)
                    {
                        row.Add(i.ToString());
                    }

                    deficitTable.Add(row);

                    foreach (var group in dataGrouped)
                    {
                        var rowData = new List<string>();
                        rowData.Add(group.Key.ToString());

                        foreach (var value in group.ToList())
                        {
                            var a = value.IdealValue.GetValueOrDefault(0);
                            var b = value.OptimalValue.GetValueOrDefault(0);
                            if (deficitType == "absoluteDeficit")
                            {
                                var absoluteValue = Math.Round(a - b, 2, MidpointRounding.AwayFromZero);
                                rowData.Add(absoluteValue.ToString(CultureInfo.InvariantCulture));
                            }
                            if (deficitType == "relativeDeficit")
                            {
                                if (a == 0)
                                {
                                    rowData.Add("0");
                                }
                                else
                                {
                                    var relativeValue = Math.Round((a - b) / a, 2, MidpointRounding.AwayFromZero);
                                    rowData.Add(relativeValue.ToString(CultureInfo.InvariantCulture));
                                }
                            }
                        }

                        deficitTable.Add(rowData);
                    }
                }
            }
            return ResolveDeficitTable(deficitTable);
        }

        public IList<string> GetScenariosNameByProjectID(int projectId)
        {
            var scenariosNames = new List<string>();
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var nodeScenarios = ctx.NodeOptimalSolutions
                    .Where(x => x.ProjectID == projectId && x.ScenarioName != null)
                    .Select(s => s.ScenarioName);

                scenariosNames.AddRange(nodeScenarios);
             
                var channelScenarios = ctx.ChannelOptimalSolutions
                    .Where(x => x.ProjectID == projectId && x.ScenarioName != null)
                    .Select(s => s.ScenarioName);
             
                scenariosNames.AddRange(channelScenarios);
            }

            return scenariosNames.Distinct().ToList();
        }

        public IEnumerable<ChannelOptimalSolutionsData> GetChannelSubsetData(long channelOptimalSolutionsId, int startPeriodDay, int startPeriodMonth, int? startSubsetYear, int endPeriodDay, int endPeriodMonth, int? endSubsetYear)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var channelOptimalSolutionsData = ctx.ChannelOptimalSolutionsData
                    .Where(s => s.ChannelOptimalSolutionsID == channelOptimalSolutionsId).OrderBy(x => x.SurveyDate)
                    .AsQueryable();

                var subsetData = new List<ChannelOptimalSolutionsData>();

                var yearTemp = 0;
                var startDateNew = channelOptimalSolutionsData.Min(x => x.SurveyDate);
                var endDateNew = channelOptimalSolutionsData.Max(x => x.SurveyDate);
              
                //is start year is provided, consider solver result from start year
                if (startSubsetYear != null)
                {
                    startDateNew = new DateTime(startSubsetYear.Value, 1, 1);
                }

                //is end year is provided, consider solver result up to end year
                if (endSubsetYear != null)
                {
                    endDateNew = new DateTime(endSubsetYear.Value, 12, 31);
                }

                //check is end period is in following year
                var startDateTest = new DateTime(startDateNew.Year, startPeriodMonth, startPeriodDay);
                var endDateTest = new DateTime(startDateNew.Year, endPeriodMonth, endPeriodDay);

                var isEndPeriodInNextYear = false;
                if (endDateTest < startDateTest)
                {
                    isEndPeriodInNextYear = true;
                }

                var dataSubsetFromSolver = channelOptimalSolutionsData.Where(x => x.SurveyDate >= startDateNew && x.SurveyDate <= endDateNew);

                foreach (var data in dataSubsetFromSolver)
                {
                    if (data.SurveyDate == null)
                    {
                        continue;
                    }

                    var dataYear = data.SurveyDate.Year;

                    if (dataYear != yearTemp)
                    {
                        yearTemp = dataYear;
                        startDateNew = new DateTime(dataYear, startPeriodMonth, startPeriodDay);
                        endDateNew = new DateTime(dataYear, endPeriodMonth, endPeriodDay);
                    }

                    if (!isEndPeriodInNextYear)
                    {
                        if (data.SurveyDate >= startDateNew && data.SurveyDate <= endDateNew)
                        {
                            subsetData.Add(data);
                        }
                    }
                    else
                    {
                        if (data.SurveyDate >= startDateNew && data.SurveyDate <= new DateTime(data.SurveyDate.Year, 12, 31))
                        {
                            subsetData.Add(data);
                        }

                        if (data.SurveyDate >= new DateTime(data.SurveyDate.Year, 1, 1) && data.SurveyDate <= endDateNew)
                        {
                            subsetData.Add(data);
                        }
                    }
                }
                return  subsetData;
            }
        }

        public IEnumerable<NodeOptimalSolutionsData> GetNodeSubsetData(long nodeOptimalSolutionsId, int startPeriodDay, int startPeriodMonth, int? startSubsetYear, int endPeriodDay, int endPeriodMonth, int? endSubsetYear)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var nodeOptimalSolutionsData = ctx.NodeOptimalSolutionsData
                    .Where(s => s.NodeOptimalSolutionsID == nodeOptimalSolutionsId).OrderBy(x => x.SurveyDate)
                    .AsQueryable();

                var subsetData = new List<NodeOptimalSolutionsData>();

                var yearTemp = 0;
                var startDateNew = nodeOptimalSolutionsData.Min(x => x.SurveyDate);
                var endDateNew = nodeOptimalSolutionsData.Max(x => x.SurveyDate);

                //is start year is provided, consider solver result from start year
                if (startSubsetYear != null)
                {
                    startDateNew = new DateTime(startSubsetYear.Value, 1, 1);
                }

                //is end year is provided, consider solver result up to end year
                if (endSubsetYear != null)
                {
                    endDateNew = new DateTime(endSubsetYear.Value, 12, 31);
                }

                //check is end period is in following year
                var startDateTest = new DateTime(startDateNew.Year, startPeriodMonth, startPeriodDay);
                var endDateTest = new DateTime(startDateNew.Year, endPeriodMonth, endPeriodDay);

                var isEndPeriodInNextYear = false;
                if (endDateTest < startDateTest)
                {
                    isEndPeriodInNextYear = true;
                }

                var dataSubsetFromSolver = nodeOptimalSolutionsData.Where(x => x.SurveyDate >= startDateNew && x.SurveyDate <= endDateNew);

                foreach (var data in dataSubsetFromSolver)
                {
                    if (data.SurveyDate == null)
                    {
                        continue;
                    }

                    var dataYear = data.SurveyDate.Year;

                    if (dataYear != yearTemp)
                    {
                        yearTemp = dataYear;
                        startDateNew = new DateTime(dataYear, startPeriodMonth, startPeriodDay);
                        endDateNew = new DateTime(dataYear, endPeriodMonth, endPeriodDay);
                    }

                    if (!isEndPeriodInNextYear)
                    {
                        if (data.SurveyDate >= startDateNew && data.SurveyDate <= endDateNew)
                        {
                            subsetData.Add(data);
                        }
                    }
                    else
                    {
                        if (data.SurveyDate >= startDateNew && data.SurveyDate <= new DateTime(data.SurveyDate.Year, 12, 31))
                        {
                            subsetData.Add(data);
                        }

                        if (data.SurveyDate >= new DateTime(data.SurveyDate.Year, 1, 1) && data.SurveyDate <= endDateNew)
                        {
                            subsetData.Add(data);
                        }
                    }
                }
                return subsetData;
            }
        }
    }
}
