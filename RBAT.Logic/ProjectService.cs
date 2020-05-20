using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

    public class ProjectService : IProjectService
    {

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<ProjectService> _logger;
        private readonly IChannelPolicyGroupChannelService _channelPolicyGroupChannelService;
        private readonly INodePolicyGroupNodeService _nodePolicyGroupNodeService;

        private Dictionary<int, int> _nodes;
        private Dictionary<int, int> _channels;
        private Dictionary<int, int> _alreadyCreatedClimateStations;

        public ProjectService(ILogger<ProjectService> logger, IHttpContextAccessor contextAccessor, IChannelPolicyGroupChannelService channelPolicyGroupChannelService, INodePolicyGroupNodeService nodePolicyGroupNodeService)
        {
            this._logger = logger;
            this._contextAccessor = contextAccessor;
            this._channelPolicyGroupChannelService = channelPolicyGroupChannelService;
            this._nodePolicyGroupNodeService = nodePolicyGroupNodeService;
        }

        public void AddProject(string name, string description, DateTime calculationBeginsDate, DateTime calculationEndsDate, int dataReadTypeId, int routingOptionTypeId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var project = new Project
                {
                    Name = name,
                    Description = description,
                    CalculationBegins = calculationBeginsDate,
                    CalculationEnds = calculationEndsDate,
                    DataStepTypeID = dataReadTypeId,
                    RoutingOptionTypeID = routingOptionTypeId
                };
                ctx.Projects.Add(project);
                ctx.SaveChanges();
            }
        }

        public List<ProjectNode> GetAllProjectNode(int projectId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var projectNodeList = new List<ProjectNode>();
                var nodeList = ctx.Nodes.AsQueryable()
                                  .Include(e => e.NodeType).AsQueryable()
                                  .OrderBy(e => e.Name);

                foreach (var node in nodeList)
                {
                    var projectNodeExist = ctx.ProjectNodes.Any(x => x.NodeId == node.Id && x.ProjectId == projectId);
                    projectNodeList.Add(new ProjectNode
                    {
                        Node = node,
                        NodeId = node.Id,
                        ProjectId = projectNodeExist ? projectId : 0
                    });
                }

                return projectNodeList;
            }
        }

        public List<ProjectChannel> GetAllProjectChannel(int projectId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var projectChannelList = new List<ProjectChannel>();
                var channelList = ctx.Channels.AsQueryable()
                                     .Include(e => e.ChannelType).AsQueryable()
                                     .OrderBy(e => e.Name);

                foreach (var channel in channelList)
                {
                    var projectChannelExist = ctx.ProjectChannels.Any(x => x.ChannelId == channel.Id && x.ProjectId == projectId);
                    projectChannelList.Add(new ProjectChannel
                    {
                        Channel = channel,
                        ChannelId = channel.Id,
                        ProjectId = projectChannelExist ? projectId : 0
                    });
                }

                return projectChannelList;
            }
        }

        public List<ProjectNode> GetProjectNodes(int projectId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var query = ctx.ProjectNodes
                    .Include(pn => pn.Node)
                    .Where(pn => pn.ProjectId == projectId)
                    .AsQueryable();

                return query.ToList();
            }
        }

        public List<ProjectChannel> GetProjectChannels(int projectId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var query = ctx.ProjectChannels
                    .Include(pn => pn.Channel)
                    .Where(pn => pn.ProjectId == projectId)
                    .AsQueryable();

                return query.ToList();            }
        }

        public Project GetProjectByID(int projectId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return ctx.Projects.FirstOrDefault(x => x.Id == projectId);
            }
        }

        public List<Project> GetProjects()
        {           
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var query = ctx.Projects.AsQueryable()
                    .Include(e => e.DataStepType)
                    .Include(e => e.RoutingOptionType).AsQueryable()
                    .OrderBy(e => e.Name);

                return query.ToList();
            }
        }
       
        public bool RemoveProject(List<Project> projectList)
        {
            try
            {
                using (var ctx = new RBATContext(this._contextAccessor))
                {
                    foreach (var project in projectList)
                    {
                        var projectEntity = ctx.Projects.FirstOrDefault(x => x.Id == project.Id);
                        if (projectEntity != null)
                        {
                            var scenarios = ctx.Scenarios.Where(s => s.ProjectID == project.Id);
                            foreach (var scenario in scenarios)
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
                            }

                            ctx.SaveChanges();

                            ctx.Projects.Remove(projectEntity);
                            ctx.SaveChanges();
                        }
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                this._logger.LogError(string.Format("Delete Project: An unexpected error occurred while deleting a project. {0}", e.Message));
            }

            return false;
        }

        /// <summary>
        /// Saves project node.
        /// </summary>
        /// <param name="projectId">The project Id.</param>
        /// <param name="nodeId">The node Id.</param>
        /// <param name="isChecked">True or false.</param>
        /// <returns></returns>
        public async Task<bool> SaveProjectNode(int projectId, int nodeId, bool isChecked)
        {
            try
            {
                using (var ctx = new RBATContext(this._contextAccessor))
                {
                    var projectNode = ctx.ProjectNodes.FirstOrDefault(x => x.NodeId == nodeId && x.ProjectId == projectId);

                    if (!isChecked)
                    {
                        if (projectNode != null)
                        {
                            var nodePolicyGroups = ctx.NodePolicyGroupNodes.Where(c => c.NodeID == nodeId).Select(c => c.NodePolicyGroup).ToList();

                            foreach (var nodePolicyGroup in nodePolicyGroups)
                            {
                                var priority = ctx.NodePolicyGroupNodes.Where(p => p.NodePolicyGroupID == nodePolicyGroup.Id)
                                                                       .Where(p => p.NodeID == nodeId)
                                                                       .Select(p => p.Priority)
                                                                       .First();

                                var nodePolicyGroupNodes = ctx.NodePolicyGroupNodes.Where(p => p.NodePolicyGroupID == nodePolicyGroup.Id)
                                                                                   .Where(p => p.Priority > priority);

                                if (nodePolicyGroupNodes.Any())
                                {
                                    await nodePolicyGroupNodes.ForEachAsync(c => c.Priority = c.Priority - 1);
                                    await _nodePolicyGroupNodeService.ChangePriority(await nodePolicyGroupNodes.ToListAsync());
                                }
                            };

                            if (nodePolicyGroups.Any()) {
                                var nodePolicyGroupChannel = ctx.NodePolicyGroupNodes.Where(n => n.NodeID == nodeId)
                                                                                     .Where(c => nodePolicyGroups.Contains(c.NodePolicyGroup));

                                if (nodePolicyGroupChannel != null && nodePolicyGroupChannel.Any())
                                    await nodePolicyGroupChannel.ForEachAsync(n => ctx.NodePolicyGroupNodes.Remove(n));

                                var nodeZoneLevel = ctx.NodeZoneLevels.Where(n => n.NodeID == nodeId)
                                                                      .Where(c => nodePolicyGroups.Contains(c.NodePolicyGroup));

                                if (nodeZoneLevel != null && nodeZoneLevel.Any())
                                    await nodeZoneLevel.ForEachAsync(n => ctx.NodeZoneLevels.Remove(n));
                            }

                            ctx.ProjectNodes.Remove(projectNode);
                            ctx.SaveChanges();
                        }
                    }

                    if (isChecked && projectNode == null)
                    {
                        ctx.ProjectNodes.Add(new ProjectNode
                        {
                            NodeId = nodeId,
                            ProjectId = projectId
                        });

                        ctx.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                this._logger.LogError(string.Format("Save Project Node: An unexpected error occurred while saving project node. {0}", e.Message));
                return false;
            }
        }

        public async Task<bool> SaveProjectChannel(int projectId, int channelId, bool isChecked)
        {
            try
            {
                using (var ctx = new RBATContext(this._contextAccessor))
                {
                    var projectChannel = ctx.ProjectChannels.FirstOrDefault(x => x.ChannelId == channelId && x.ProjectId == projectId);

                    if (!isChecked)
                    {
                        if (projectChannel != null)
                        {
                            var channelPolicyGroups = ctx.ChannelPolicyGroupChannels.Where(c => c.ChannelID == channelId).Select(c => c.ChannelPolicyGroup).ToList();

                            foreach (var channelPolicyGroup in channelPolicyGroups)
                            {
                                var priority = ctx.ChannelPolicyGroupChannels.Where(p => p.ChannelPolicyGroupID == channelPolicyGroup.Id)
                                                                             .Where(p => p.ChannelID == channelId)
                                                                             .Select(p => p.Priority)
                                                                             .First();

                                var channelPolicyGroupChannels = ctx.ChannelPolicyGroupChannels.Where(p => p.ChannelPolicyGroupID == channelPolicyGroup.Id)
                                                                                               .Where(p => p.Priority > priority);

                                if(channelPolicyGroupChannels.Any()) { 
                                    await channelPolicyGroupChannels.ForEachAsync(c => c.Priority = c.Priority - 1);
                                    await _channelPolicyGroupChannelService.ChangePriority(await channelPolicyGroupChannels.ToListAsync());
                                }
                            };

                            if (channelPolicyGroups.Any())
                            {
                                var channelPolicyGroupChannel = ctx.ChannelPolicyGroupChannels.Where(c => c.ChannelID == channelId)
                                                                                          .Where(c => channelPolicyGroups.Contains(c.ChannelPolicyGroup));

                                if (channelPolicyGroupChannel != null && channelPolicyGroupChannel.Any())
                                    await channelPolicyGroupChannel.ForEachAsync(c => ctx.ChannelPolicyGroupChannels.Remove(c));

                                var channelZoneLevel = ctx.ChannelZoneLevels.Where(c => c.ChannelID == channelId)
                                                                            .Where(c => channelPolicyGroups.Contains(c.ChannelPolicyGroup));

                                if (channelZoneLevel != null && channelZoneLevel.Any())
                                    await channelZoneLevel.ForEachAsync(c => ctx.ChannelZoneLevels.Remove(c));
                            }

                            ctx.ProjectChannels.Remove(projectChannel);
                            ctx.SaveChanges();
                        }
                    }

                    if (isChecked && projectChannel == null)
                    {
                        ctx.ProjectChannels.Add(new ProjectChannel
                        {
                            ChannelId = channelId,
                            ProjectId = projectId
                        });

                        ctx.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                this._logger.LogError(string.Format("Save Project Channel: An unexpected error occurred while saving project channel. {0}", e.Message));
                return false;
            }
        }

        public void UpdateProject(Project project)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var projectEntity = ctx.Projects.FirstOrDefault(x => x.Id == project.Id);
                if (projectEntity != null)
                {
                    projectEntity.Name = project.Name;
                    projectEntity.Description = project.Description;
                    projectEntity.DataStepTypeID = project.DataStepTypeID;
                    projectEntity.RoutingOptionTypeID = project.RoutingOptionTypeID;
                    projectEntity.CalculationBegins = project.CalculationBegins;
                    projectEntity.CalculationEnds = project.CalculationEnds;
                    ctx.SaveChanges();
                }
            }
        }
        
        public void UpdateProjectMapData(int projectId, string projectMapData)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var projectEntity = ctx.Projects.FirstOrDefault(x => x.Id == projectId);
                if (projectEntity != null)
                {
                    projectEntity.MapData = projectMapData;
                    ctx.SaveChanges();
                }
            }
        }

        public async Task<Tuple<int, Dictionary<int, int>, Dictionary<int, int>>> CopyProject(int projectId, string username = null)
        {            
            try
            {
                _nodes = new Dictionary<int, int>();
                _channels = new Dictionary<int, int>();
                _alreadyCreatedClimateStations = new Dictionary<int, int>();
                var isAdmin = _contextAccessor?.HttpContext?.User?.IsInRole("Admin") ?? false;
                var newProjectId = await CopyProjectOnly(projectId, username);

                if (newProjectId != 0)
                {
                    await CopyProjectNodes(projectId, newProjectId, isAdmin, username);
                    await CopyProjectChannels(projectId, newProjectId, isAdmin, username);                        
                    if (!isAdmin) await CopyTimeNaturalFlows(projectId, newProjectId);
                    return new Tuple<int, Dictionary<int, int>, Dictionary<int, int>>(newProjectId, _nodes, _channels);
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }            
        }        

        private async Task<int> CopyProjectOnly(int projectId, string username = null)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var project = await ctx.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

                if (project != null)
                {
                    var newProject = (Project)ctx.Entry(project).CurrentValues.ToObject();
                    newProject.Id = 0;
                    newProject.Name = "Copy of " + project.Name;
                    if (username != null) newProject.UserName = username;
                    ctx.Projects.Add(newProject);

                    if (ctx.SaveChanges() != 0) return newProject.Id;
                }

                return 0;
            }
        }

        private async Task CopyProjectNodes(int projectId, int newProjectId, bool isAdmin, string username = null)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var projectNodes = await ctx.ProjectNodes.Where(n => n.ProjectId == projectId).ToListAsync();                

                foreach (var n in projectNodes)
                {
                    var nodeId = (isAdmin) ? await CopyNode(projectId, newProjectId, n.NodeId, username) : 0;

                    var newProjectNode = (ProjectNode)ctx.Entry(n).CurrentValues.ToObject();
                    newProjectNode.Id = 0;
                    newProjectNode.ProjectId = newProjectId;
                    if (nodeId != 0) newProjectNode.NodeId = nodeId;
                    if (username != null) newProjectNode.UserName = username;
                    ctx.ProjectNodes.Add(newProjectNode);
                    ctx.SaveChanges();
                }
            }
        }

        private async Task<int> CopyNode(int projectId, int newProjectId, int nodeId, string username)
        {
            var node = await GetNode(nodeId);

            if (node != null)
            {
                var newNodeId = await CopyNodeOnly(node, username);                

                if (newNodeId != 0)
                {
                    _nodes.Add(node.Id, newNodeId);
                    await CopyTimeNaturalFlows(projectId, newProjectId, nodeId, newNodeId, username);
                    await CopyTimeHistoricLevels(nodeId, newNodeId, username);
                    await CopyTimeStorageCapacity(nodeId, newNodeId, username);
                    await CopyTimeWaterUses(nodeId, newNodeId, username);
                    await CopyNetEvaporations(nodeId, newNodeId, username);

                    return newNodeId;
                }
            }

            return 0;            
        }

        private async Task<Node> GetNode(int nodeId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.Nodes.FirstOrDefaultAsync(n => n.Id == nodeId);
            }
        }

        private async Task<int> CopyNodeOnly(Node node, string username)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var newNode = (Node)ctx.Entry(node).CurrentValues.ToObject();
                newNode.Id = 0;
                newNode.UserName = username;
                ctx.Nodes.Add(newNode);                

                if (ctx.SaveChanges() != 0) return newNode.Id;

                return 0;
            }
        }

        private async Task CopyProjectChannels(int projectId, int newProjectId, bool isAdmin, string username = null)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var projectChannels = await ctx.ProjectChannels.Where(c => c.ProjectId == projectId).ToListAsync();

                foreach (var c in projectChannels)
                {
                    var channelId = (isAdmin) ? await CopyChannel(c.ChannelId, username) : 0;

                    var newProjectChannel = (ProjectChannel)ctx.Entry(c).CurrentValues.ToObject();
                    newProjectChannel.Id = 0;
                    newProjectChannel.ProjectId = newProjectId;
                    if (channelId != 0) newProjectChannel.ChannelId = channelId;
                    if (username != null) newProjectChannel.UserName = username;
                    ctx.ProjectChannels.Add(newProjectChannel);
                    ctx.SaveChanges();
                }
            }
        }

        private async Task<int> CopyChannel(int channelId, string username)
        {
            var channel = await GetChannel(channelId);

            if (channel != null)
            {
                var newChannelId = await CopyChannelOnly(channel, username);
                
                if (newChannelId != 0)
                {
                    _channels.Add(channel.Id, newChannelId);
                    await CopyChannelOutflowCapacities(channelId, newChannelId, username);
                    await CopyChannelTravelTimes(channelId, newChannelId, username);

                    return newChannelId;
                }                
            }

            return 0;
        }

        private async Task<Channel> GetChannel(int channelId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.Channels.FirstOrDefaultAsync(c => c.Id == channelId);
            }
        }

        private async Task<int> CopyChannelOnly(Channel channel, string username)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var newChannel = (Channel)ctx.Entry(channel).CurrentValues.ToObject();
                newChannel.Id = 0;
                newChannel.UserName = username;
                newChannel.DownstreamNodeID = (newChannel.DownstreamNodeID != null) ? _nodes[newChannel.DownstreamNodeID.GetValueOrDefault()] : (int?)null;
                newChannel.ReferenceNodeID = (newChannel.ReferenceNodeID != null) ? _nodes[newChannel.ReferenceNodeID.GetValueOrDefault()] : (int?)null;
                newChannel.UpstreamNodeID = (newChannel.UpstreamNodeID != null) ? _nodes[newChannel.UpstreamNodeID.GetValueOrDefault()] : (int?)null;
                newChannel.UpstreamNodeWithControlStructureID = (newChannel.UpstreamNodeWithControlStructureID != null) ? _nodes[newChannel.UpstreamNodeWithControlStructureID.GetValueOrDefault()] : (int?)null;
                ctx.Channels.Add(newChannel);                

                if (ctx.SaveChanges() != 0) return newChannel.Id;

                return 0;
            }
        }

        private async Task CopyTimeNaturalFlows(int projectId, int newProjectId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var timeNaturalFlows = await ctx.TimeNaturalFlows.Where(t => t.ProjectID == projectId).ToListAsync();
                await ctx.BulkInsertAsyncExtended(timeNaturalFlows.Select(t => { t.ProjectID = newProjectId; return t; }).ToList(), this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        private async Task CopyTimeNaturalFlows(int projectId, int newProjectId, int nodeId, int newNodeId, string username)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var timeNaturalFlows = await ctx.TimeNaturalFlows.Where(t => t.ProjectID == projectId)
                                                             .Where(t => t.NodeID == nodeId)
                                                             .ToListAsync();
                await ctx.BulkInsertAsyncExtended(timeNaturalFlows.Select(t =>
                {
                    t.ProjectID = newProjectId;
                    t.NodeID = newNodeId;
                    t.UserName = username;
                    return t;
                }).ToList(), username);
            }
        }

        private async Task CopyTimeHistoricLevels(int nodeId, int newNodeId, string username)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var timeHistoricLevels = await ctx.TimeHistoricLevels.Where(t => t.NodeID == nodeId).ToListAsync();

                await ctx.BulkInsertAsyncExtended(timeHistoricLevels.Select(t =>
                {
                    t.NodeID = newNodeId;
                    t.UserName = username;
                    return t;
                }).ToList(), username);
            }
        }

        private async Task CopyTimeStorageCapacity(int nodeId, int newNodeId, string username)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var timeStorageCapacities = await ctx.TimeStorageCapacities.Where(t => t.NodeID == nodeId).ToListAsync();

                await ctx.BulkInsertAsyncExtended(timeStorageCapacities.Select(t =>
                {
                    t.NodeID = newNodeId;
                    t.UserName = username;
                    return t;
                }).ToList(), username);
            }
        }

        private async Task CopyTimeWaterUses(int nodeId, int newNodeId, string username)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var timeWaterUses = await ctx.TimeWaterUses.Where(t => t.NodeID == nodeId).ToListAsync();

                await ctx.BulkInsertAsyncExtended(timeWaterUses.Select(t =>
                {
                    t.NodeID = newNodeId;
                    t.UserName = username;
                    return t;
                }).ToList(), username);
            }
        }

        private async Task CopyNetEvaporations(int nodeId, int newNodeId, string username)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var netEvaporations = await ctx.NetEvaporations.Where(t => t.NodeID == nodeId).ToListAsync();                

                foreach (var e in netEvaporations)
                {
                    var newClimateStationId = await GetNewClimateStationId(e.ClimateStationID, username);
                    await CopyNetEvaporationOnly(e, newClimateStationId, newNodeId, username);
                }
            }
        }

        private async Task<int> GetNewClimateStationId(int climateStationId, string username)
        {
            if (!_alreadyCreatedClimateStations.ContainsKey(climateStationId))
            {
                var newClimateStationId = await CopyClimateStation(climateStationId, username);

                if (newClimateStationId != 0)
                {
                    await CopyTimeClimateData(climateStationId, newClimateStationId, username);
                }

                _alreadyCreatedClimateStations.Add(climateStationId, newClimateStationId);

                return newClimateStationId;
            }
            else
                return _alreadyCreatedClimateStations.GetValueOrDefault(climateStationId);
        }

        private async Task CopyNetEvaporationOnly(NetEvaporation e, int newClimateStationId, int newNodeId, string username)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var newNetEvaporation = (NetEvaporation)ctx.Entry(e).CurrentValues.ToObject();
                newNetEvaporation.Id = 0;
                newNetEvaporation.ClimateStationID = newClimateStationId;
                newNetEvaporation.NodeID = newNodeId;
                newNetEvaporation.UserName = username;
                ctx.NetEvaporations.Add(newNetEvaporation);
                ctx.SaveChanges();
            }
        }

        private async Task<int> CopyClimateStation(int climateStationId, string username)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var climateStation = await ctx.ClimateStations.FirstOrDefaultAsync(s => s.Id == climateStationId);

                if (climateStation != null)
                {
                    var newClimateStation = (ClimateStation)ctx.Entry(climateStation).CurrentValues.ToObject();
                    newClimateStation.Id = 0;
                    newClimateStation.UserName = username;
                    ctx.ClimateStations.Add(newClimateStation);

                    if (ctx.SaveChanges() != 0) return newClimateStation.Id;
                }

                return 0;
            }
        }

        private async Task CopyTimeClimateData(int climateStationId, int newClimateStationId, string username)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var timeClimateData = await ctx.TimeClimateDatas.Where(s => s.ClimateStationID == climateStationId).ToListAsync();

                await ctx.BulkInsertAsyncExtended(timeClimateData.Select(t =>
                {
                    t.ClimateStationID = newClimateStationId;
                    t.UserName = username;
                    return t;
                }).ToList(), username);
            }
        }

        private async Task CopyChannelOutflowCapacities(int channelId, int newChannelId, string username)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var channelOutflowCapacities = await ctx.ChannelOutflowCapacities.Where(c => c.ChannelID == channelId).ToListAsync();

                await ctx.BulkInsertAsyncExtended(channelOutflowCapacities.Select(t =>
                {
                    t.ChannelID = newChannelId;
                    t.UserName = username;
                    return t;
                }).ToList(), username);
            }
        }

        private async Task CopyChannelTravelTimes(int channelId, int newChannelId, string username)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var channelTravelTimes = await ctx.ChannelTravelTimes.Where(c => c.ChannelID == channelId).ToListAsync();

                await ctx.BulkInsertAsyncExtended(channelTravelTimes.Select(c =>
                {
                    c.ChannelID = newChannelId;
                    c.UserName = username;
                    return c;
                }).ToList(), username);               
            }
        }
    }
}
