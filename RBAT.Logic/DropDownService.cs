using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RBAT.Core;
using RBAT.Core.Models;
using RBAT.Logic.Extensions;
using RBAT.Logic.TransferModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Logic
{

    public class DropDownService
    {

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public DropDownService(IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public IList<TimeStepType> ListCalculationTimeSteps()
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return ctx.TimeStepTypes.Where(x => x.CalculationFlag).ToList();
            }
        }

        public IList<ChannelType> ListChannelTypes()
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return ctx.ChannelTypes.ToList();
            }
        }

        public IList<ChannelType> ListChannelTypesForPolicyGroup()
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return ctx.ChannelTypes.Where(c => c.Id != 3).ToList();
            }
        }

        public IList<NodeType> ListNodeTypesForPolicyGroup()
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return ctx.NodeTypes.Where(c => c.Id != 3).ToList();
            }
        }

        public IList<ClimateStation> ListClimateStations()
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var climateStations = from cs in ctx.ClimateStations
                                      orderby cs.Name
                                      select cs;

                return climateStations.ToList();
            }
        }

        public IList<TimeStepType> ListDataReadTypes()
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return ctx.TimeStepTypes.Where(x => x.DataFlag).ToList();
            }
        }

        public IList<Node> ListNodes()
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return ctx.Nodes.OrderBy(n => n.Name).ToList();
            }
        }

        public IList<Node> ListReservoirs(int projectID)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var reservoirs = from n in ctx.Nodes
                                 join pn in ctx.ProjectNodes on n.Id equals pn.NodeId
                                 where pn.ProjectId == projectID
                                 where n.NodeTypeId == 1 //reservoir
                                 orderby n.Name
                                 select n;

                return reservoirs.ToList();
            }
        }

        public IList<NodeType> ListNodeTypes()
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return ctx.NodeTypes.ToList();
            }
        }

        public IList<Project> ListProjects(int nodeID)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var projects = from p in ctx.Projects
                               join pn in ctx.ProjectNodes on p.Id equals pn.ProjectId
                               where pn.NodeId == nodeID
                               orderby p.Name
                               select p;

                return projects.ToList();
            }
        }

        public IList<RoutingOptionType> ListRoutingOptionTypes()
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return ctx.RoutingOptionTypes.ToList();
            }
        }

        public IList<Project> ListProjects()
        {
            using (var ctx = new RBATContext(_contextAccessor))
            {
                return ctx.Projects.OrderBy(p => p.Name).ToList();
            }
        }

        public IList<ScenarioType> ListScenarioTypes()
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return ctx.ScenarioTypes.ToList();
            }
        }

        public IList<Scenario> ListScenarios(int projectID)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return ctx.Scenarios.Where(s => s.ProjectID == projectID).OrderBy(s => s.Name).ToList();
            }
        }

        public IList<Node> ListNodeComponents(int projectID, string scenarioName)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var nodeList = ctx.NodeOptimalSolutions.Include(x => x.Node).Where(s => s.ProjectID == projectID && s.ScenarioName == scenarioName).Select(x => x.Node).AsQueryable();
                //Deficit Table only for Consumptive Use
                return nodeList.Where(x => x.NodeTypeId == 2).ToList();
            }
        }

        public IList<Channel> ListChannelComponents(int projectID, string scenarioName)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var channelList = ctx.ChannelOptimalSolutions.Include(x => x.Channel).Where(s => s.ProjectID == projectID && s.ScenarioName == scenarioName).Select(x => x.Channel)
                    .AsQueryable();
                //Deficit Table only for River reach
                return channelList.Where(x => x.ChannelTypeId == 1).ToList();
            }
        }

        public IList<Channel> ListNonReturnFlowChannels(int channelID)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return ctx.Channels.Where(c => c.ChannelTypeId != 3)
                                   .Where(c => c.Id != channelID)
                                   .OrderBy(c => c.Name).ToList();
            }
        }

        public IList<RecordedFlowStation> ListRecordedFlowStations()
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return ctx.RecordedFlowStations.OrderBy(r => r.Name).ToList();
            }
        }

        public IList<MeasuringUnit> ListMeasuringUnits()
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return ctx.MeasuringUnits.ToList();
            }
        }

        public IList<Node> ListReferenceNodes()
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                //var test =  ctx.Nodes.Include(n => n.TimeNaturalFlows)
                //          .Where(n => n.NodeTypeId != 2)
                //          .Where(n => n.TimeNaturalFlows.Any())
                //          .OrderBy(n => n.Name)
                //          .ToList();

                var nodeList = new List<Node>();
                var nodes = ctx.Nodes.Where(n => n.NodeTypeId != 2)
                                .OrderBy(n => n.Name)
                                .ToList();

                var nodeIDs = ctx.TimeNaturalFlows.GroupBy(x => x.NodeID).Select(y => new { id = y.Key });

                foreach (var node in nodes)
                {
                    if (nodeIDs.Any(x => x.id == node.Id))
                    {
                        nodeList.Add(node);
                    }
                }

                return nodeList.OrderBy(n => n.Name).ToList();

                //return ctx.Nodes.Include(n => n.TimeNaturalFlows)
                //                .Where(n => n.NodeTypeId != 2)
                //                .Where(n => n.TimeNaturalFlows.Any())
                //                .OrderBy(n => n.Name)
                //                .ToList();
            }
        }

        public IList<Node> ListReservoirs()
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return ctx.Nodes.Where(n => n.NodeTypeId == 1) //reservoir
                                .OrderBy(n => n.Name)
                                .ToList();
            }
        }

        public async Task<IList<ApplicationUser>> ListUsers()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");

            return _userManager.Users.Where(u => !admins.Contains(u))
                                     .OrderBy(u => u.UserName)
                                     .ToList();
        }

        public IList<ChannelZoneLevelModel> ListChannelZoneLevels(int channelPolicyGroupId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var channelPolicyGroup = ctx.ChannelPolicyGroups.Where(g => g.Id == channelPolicyGroupId).First();

                var channelZoneLevelList = new List<ChannelZoneLevelModel>();

                for (int i = channelPolicyGroup.NumberOfZonesAboveIdealLevel; i >= -channelPolicyGroup.NumberOfZonesBelowIdealLevel; i--)
                {
                    channelZoneLevelList.Add(new ChannelZoneLevelModel
                    {
                        Id = i,
                        Description = ((TransferModels.ChannelZoneLevel)i).GetDescription()
                    });
                }

                return channelZoneLevelList;
            }
        }
    }
}
