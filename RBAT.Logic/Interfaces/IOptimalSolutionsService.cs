using System;
using RBAT.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RBAT.Logic.Interfaces
{
    public interface IOptimalSolutionsService
    {
        IEnumerable<ChannelOptimalSolutions> GetChannels(int projectId, string scenarioName);

        IEnumerable<NodeOptimalSolutions> GetNodes(int projectId, string scenarioName);

        Task<IEnumerable<ChannelOptimalSolutionsData>> GetChannelChartData(long channelOptimalSolutionsId);

        Task<IEnumerable<ChannelOptimalSolutionsData>> GetChannelChartSteppedData(long channelOptimalSolutionsId);

        Task<IEnumerable<NodeOptimalSolutionsData>> GetNodeChartData(long nodeOptimalSolutionsId);

        Task<IEnumerable<NodeOptimalSolutionsData>> GetNodeChartSteppedData(long nodeOptimalSolutionsId);

        IEnumerable<string> GetScenariosByChannelId(int projectId, int channelId);

        IEnumerable<string> GetScenariosByNodeId(int projectId, int nodeId);

        Task<IEnumerable<ChannelOptimalSolutionsData>> GetChannelExceedenceChartData(long channelOptimalSolutionsId, string valueType, DateTime? startDate, DateTime? endDate);

        Task<IEnumerable<NodeOptimalSolutionsData>> GetNodeExceedenceChartData(long nodeOptimalSolutionsId, string valueType, DateTime? startDate, DateTime? endDate);

        List<List<string>> GetNodeDeficitTableData(int? projectId, string scenarioName, int? nodeId, string deficitType);

        List<List<string>> GetChannelDeficitTableData(int? projectId, string scenarioName, int? channelId, string deficitType);

        IList<string> GetScenariosNameByProjectID(int projectId);

        IEnumerable<ChannelOptimalSolutionsData> GetChannelSubsetData(long channelOptimalSolutionsId, int startPeriodDay, int startPeriodMonth, int? startSubsetYear, int endPeriodDay, int endPeriodMonth, int? endSubsetYear);

        IEnumerable<NodeOptimalSolutionsData> GetNodeSubsetData(long nodeOptimalSolutionsId, int startPeriodDay, int startPeriodMonth, int? startSubsetYear, int endPeriodDay, int endPeriodMonth, int? endSubsetYear);
    }
}
