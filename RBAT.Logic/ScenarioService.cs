using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RBAT.Core;
using RBAT.Core.Models;
using RBAT.Logic.Extensions;
using RBAT.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Logic
{

    public class ScenarioService : IScenarioService {

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public ScenarioService(IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager) {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<IList<Scenario>> GetAllByProjectID(int projectId)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                var scenario = ctx.Scenarios
                                  .Include(s => s.DataTimeStepType)
                                  .Include(s => s.TimeStepType)
                                  .Include(s => s.ScenarioType)
                                  .Where(s => s.ProjectID == projectId)
                                  .OrderBy(s => s.Name);

                return await scenario.ToListAsync();
            }
        }

        public async Task<Scenario> Get(long scenarioId)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                return await ctx.Scenarios.FirstOrDefaultAsync(x => x.Id == scenarioId);
            }
        }

        public async Task<Scenario> Get(long? scenarioId)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                if (scenarioId == null) {
                    var dropDownService = new DropDownService(_contextAccessor, _userManager);
                    var projects = dropDownService.ListProjects();
                    if (projects != null)
                        return await ctx.Scenarios.FirstOrDefaultAsync(x => x.ProjectID == projects.FirstOrDefault().Id);
                    else
                        return null;
                }
                else
                    return await ctx.Scenarios.FirstOrDefaultAsync(x => x.Id == scenarioId.GetValueOrDefault());

            }
        }

        public async Task Save(Scenario scenario)
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                await ctx.Scenarios.AddAsync(scenario);
                ctx.SaveChanges();
            }
        }

        public async Task SaveAll(IList<Scenario> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkInsertAsyncExtended(listToSave, _contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task UpdateAll(IList<Scenario> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsyncExtended(listToUpdate, _contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task RemoveAll(IList<Scenario> listToRemove)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                foreach (var scenario in listToRemove)
                {
                    var nodePolicyGroups = ctx.NodePolicyGroups.Where(g => g.ScenarioID == scenario.Id);

                    foreach (var nodePolicyGroup in nodePolicyGroups)
                    {
                        var nodePolicyGroupNodes = ctx.NodePolicyGroupNodes.Where(n => n.NodePolicyGroupID == nodePolicyGroup.Id);

                        foreach (var nodePolicyGroupNode in nodePolicyGroupNodes)
                        {
                            ctx.NodePolicyGroupNodes.Remove(nodePolicyGroupNode);
                        }

                        var nodeZoneLevels = ctx.NodeZoneLevels.Where(n => n.NodePolicyGroupID == nodePolicyGroup.Id);

                        foreach (var nodeZoneLevel in nodeZoneLevels)
                        {
                            ctx.NodeZoneLevels.Remove(nodeZoneLevel);
                        }
                    }

                    var channelOptimalSolutions = ctx.ChannelOptimalSolutions.Where(s => s.ScenarioID == scenario.Id);

                    foreach (var channelOptimalSolution in channelOptimalSolutions)
                    {
                        ctx.ChannelOptimalSolutions.Remove(channelOptimalSolution);
                    }

                    var nodeOptimalSolutions = ctx.NodeOptimalSolutions.Where(s => s.ScenarioID == scenario.Id);

                    foreach (var nodeOptimalSolution in nodeOptimalSolutions)
                    {
                        ctx.NodeOptimalSolutions.Remove(nodeOptimalSolution);
                    }

                    ctx.SaveChanges();

                    ctx.Scenarios.Remove(scenario);
                    ctx.SaveChanges();
                }                
            }
        }

        public async Task<bool> CopyScenario(long scenarioId)
        {
            try
            {
                var scenario = await GetScenario(scenarioId);

                if (scenario == null) return false;

                var newScenarioId = await CopyScenarioOnly(scenario.ProjectID, scenario.Id);

                if (newScenarioId == 0) return false;

                await CopyChannelPolicyGroups(scenario.Id, newScenarioId);
                await CopyNodePolicyGroups(scenario.Id, newScenarioId);
                await CopyCustomTimeSteps(scenario.Id, newScenarioId);
                await CopyStartingReservoirLevels(scenario.Id, newScenarioId);

                if (newScenarioId == 0) return false;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<Scenario> GetScenario(long scenarioId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.Scenarios.FirstOrDefaultAsync(s => s.Id == scenarioId);
            }
        }

        public async Task<bool> CopyScenarios(int projectId, int newProjectId, Dictionary<int, int> nodes = null, Dictionary<int, int> channels = null, string username = null)
        {            
            try
            {
                var scenarios = await GetScenarios(projectId);

                if (scenarios == null || !scenarios.Any()) return true;

                foreach (var s in scenarios)
                {
                    var newScenarioId = await CopyScenarioOnly(newProjectId, s.Id, username);

                    if (newScenarioId == 0) return false;

                    await CopyChannelPolicyGroups(s.Id, newScenarioId, channels, username);
                    await CopyNodePolicyGroups(s.Id, newScenarioId, nodes, username);
                    await CopyCustomTimeSteps(s.Id, newScenarioId, username);
                    await CopyStartingReservoirLevels(s.Id, newScenarioId, nodes, username);

                    if (newScenarioId == 0) return false;                        
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<List<Scenario>> GetScenarios(int projectId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.Scenarios.Where(s => s.ProjectID == projectId).ToListAsync();
            }
        }

        private async Task<long> CopyScenarioOnly(int newProjectId, long scenarioId, string username = null)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var scenario = await ctx.Scenarios.FirstOrDefaultAsync(s => s.Id == scenarioId);
                if (scenario == null) return 0;

                var newScenario = (Scenario)ctx.Entry(scenario).CurrentValues.ToObject();
                newScenario.Id = 0;
                newScenario.ProjectID = newProjectId;
                newScenario.Name = "Copy of " + scenario.Name;
                if (username != null) newScenario.UserName = username;
                ctx.Scenarios.Add(newScenario);

                if (ctx.SaveChanges() != 0) return newScenario.Id;

                return 0;
            }
        }

        private async Task CopyChannelPolicyGroups(long scenarioId, long newScenarioId, Dictionary<int, int> channels = null, string username = null)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var channelPolicyGroups = await ctx.ChannelPolicyGroups.Where(g => g.ScenarioID == scenarioId).ToListAsync();

                foreach (var g in channelPolicyGroups)
                {
                    var newChannelPolicyGroup = (ChannelPolicyGroup)ctx.Entry(g).CurrentValues.ToObject();
                    newChannelPolicyGroup.Id = 0;
                    newChannelPolicyGroup.ScenarioID = newScenarioId;
                    if (username != null) newChannelPolicyGroup.UserName = username;
                    ctx.ChannelPolicyGroups.Add(newChannelPolicyGroup);

                    if (ctx.SaveChanges() != 0)
                    {
                        await CopyChannelPolicyGroupChannels(g.Id, newChannelPolicyGroup.Id, channels, username);
                        await CopyChannelZoneLevels(g.Id, newChannelPolicyGroup.Id, channels, username);
                        await CopyChannelZoneWeights(g.Id, newChannelPolicyGroup.Id, username);
                    }
                };
            }
        }

        private async Task CopyChannelPolicyGroupChannels(long channelPolicyGroupId, long newchannelPolicyGroupId, Dictionary<int, int> channels = null, string username = null)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var channelPolicyGroupChannels = await ctx.ChannelPolicyGroupChannels.Where(c => c.ChannelPolicyGroupID == channelPolicyGroupId).ToListAsync();

                if (username == null) username = this._contextAccessor?.HttpContext?.User?.Identity.Name;

                await ctx.BulkInsertAsyncExtended(channelPolicyGroupChannels.Select(c =>
                {
                    c.ChannelPolicyGroupID = newchannelPolicyGroupId;
                    if (channels != null) c.ChannelID = channels[c.ChannelID];
                    if (username != null) c.UserName = username;
                    return c;
                }).ToList(), username);
            }
        }

        private async Task CopyChannelZoneLevels(long channelPolicyGroupId, long newchannelPolicyGroupId, Dictionary<int, int> channels = null, string username = null)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var channelZoneLevels = await ctx.ChannelZoneLevels.Where(c => c.ChannelPolicyGroupID == channelPolicyGroupId).ToListAsync();

                if (username == null) username = this._contextAccessor?.HttpContext?.User?.Identity.Name;

                await ctx.BulkInsertAsyncExtended(channelZoneLevels.Select(c =>
                {
                    c.ChannelPolicyGroupID = newchannelPolicyGroupId;
                    if (channels != null) c.ChannelID = channels[c.ChannelID];
                    if (username != null) c.UserName = username;
                    return c;
                }).ToList(), username);
            }
        }

        private async Task CopyChannelZoneWeights(long channelPolicyGroupId, long newchannelPolicyGroupId, string username = null)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var channelZoneWeights = await ctx.ChannelZoneWeights.Where(c => c.ChannelPolicyGroupID == channelPolicyGroupId).ToListAsync();

                if (username == null) username = this._contextAccessor?.HttpContext?.User?.Identity.Name;

                await ctx.BulkInsertAsyncExtended(channelZoneWeights.Select(c =>
                {
                    c.ChannelPolicyGroupID = newchannelPolicyGroupId;
                    if (username != null) c.UserName = username;
                    return c;
                }).ToList(), username);
            }
        }       

        private async Task CopyNodePolicyGroups(long scenarioId, long newScenarioId, Dictionary<int, int> nodes = null, string username = null)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var nodePolicyGroups = await ctx.NodePolicyGroups.Where(g => g.ScenarioID == scenarioId).ToListAsync();

                foreach (var g in nodePolicyGroups)
                {
                    var newNodePolicyGroup = (NodePolicyGroup)ctx.Entry(g).CurrentValues.ToObject();
                    newNodePolicyGroup.Id = 0;
                    newNodePolicyGroup.ScenarioID = newScenarioId;
                    if (username != null) newNodePolicyGroup.UserName = username;
                    ctx.NodePolicyGroups.Add(newNodePolicyGroup);

                    if (ctx.SaveChanges() != 0)
                    {
                        await CopyNodePolicyGroupNodes(g.Id, newNodePolicyGroup.Id, nodes, username);
                        await CopyNodeZoneLevels(g.Id, newNodePolicyGroup.Id, nodes, username);
                        await CopyNodeZoneWeights(g.Id, newNodePolicyGroup.Id, username);
                    }
                };
            }
        }

        private async Task CopyNodePolicyGroupNodes(long nodePolicyGroupId, long newNodePolicyGroupId, Dictionary<int, int> nodes = null, string username = null)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var nodePolicyGroupNodes = await ctx.NodePolicyGroupNodes.Where(n => n.NodePolicyGroupID == nodePolicyGroupId).ToListAsync();

                if (username == null) username = this._contextAccessor?.HttpContext?.User?.Identity.Name;

                await ctx.BulkInsertAsyncExtended(nodePolicyGroupNodes.Select(n =>
                {
                    n.NodePolicyGroupID = newNodePolicyGroupId;
                    if (nodes != null) n.NodeID = nodes[n.NodeID];
                    if (username != null) n.UserName = username;
                    return n;
                }).ToList(), username);
            }
        }

        private async Task CopyNodeZoneLevels(long nodePolicyGroupId, long newNodePolicyGroupId, Dictionary<int, int> nodes = null, string username = null)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var nodeZoneLevels = await ctx.NodeZoneLevels.Where(n => n.NodePolicyGroupID == nodePolicyGroupId).ToListAsync();

                if (username == null) username = this._contextAccessor?.HttpContext?.User?.Identity.Name;

                await ctx.BulkInsertAsyncExtended(nodeZoneLevels.Select(n =>
                {
                    n.NodePolicyGroupID = newNodePolicyGroupId;
                    if (nodes != null) n.NodeID = nodes[n.NodeID];
                    if (username != null) n.UserName = username;
                    return n;
                }).ToList(), username);
            }
        }

        private async Task CopyNodeZoneWeights(long nodePolicyGroupId, long newNodePolicyGroupId, string username = null)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var nodeZoneWeights = await ctx.NodeZoneWeights.Where(n => n.NodePolicyGroupID == nodePolicyGroupId).ToListAsync();

                if (username == null) username = this._contextAccessor?.HttpContext?.User?.Identity.Name;

                await ctx.BulkInsertAsyncExtended(nodeZoneWeights.Select(n =>
                {
                    n.NodePolicyGroupID = newNodePolicyGroupId;
                    if (username != null) n.UserName = username;
                    return n;
                }).ToList(), username);
            }
        }

        private async Task CopyCustomTimeSteps(long scenarioId, long newScenarioId, string username = null)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var customTimeSteps = await ctx.CustomTimeSteps.Where(s => s.ScenarioId == scenarioId).ToListAsync();

                if (username == null) username = this._contextAccessor?.HttpContext?.User?.Identity.Name;

                await ctx.BulkInsertAsyncExtended(customTimeSteps.Select(s =>
                {
                    s.ScenarioId = newScenarioId;
                    if (username != null) s.UserName = username;
                    return s;
                }).ToList(), username);
            }
        }

        private async Task CopyStartingReservoirLevels(long scenarioId, long newScenarioId, Dictionary<int, int> nodes = null, string username = null)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var startingReservoirLevels = await ctx.StartingReservoirLevels.Where(s => s.ScenarioId == scenarioId).ToListAsync();

                if (username == null) username = this._contextAccessor?.HttpContext?.User?.Identity.Name;

                await ctx.BulkInsertAsyncExtended(startingReservoirLevels.Select(s =>
                {
                    s.ScenarioId = newScenarioId;
                    if (nodes != null) s.NodeId = nodes[s.NodeId];
                    if (username != null) s.UserName = username;
                    return s;
                }).ToList(), username);
            }
        }
    }
}