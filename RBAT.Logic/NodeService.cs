using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RBAT.Core;
using RBAT.Core.Models;
using RBAT.Logic.Extensions;
using RBAT.Logic.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Logic
{

    public class NodeService : INodeService {

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INodePolicyGroupNodeService _nodePolicyGroupNodeService;

        public NodeService(IHttpContextAccessor contextAccessor, INodePolicyGroupNodeService nodePolicyGroupNodeService) {
            this._contextAccessor = contextAccessor;
            this._nodePolicyGroupNodeService = nodePolicyGroupNodeService;
        }
        public async Task<IList<Node>> GetNodesByNodeType(int nodeTypeId)
        {
            var userName = this._contextAccessor?.HttpContext?.User?.Identity.Name;
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var node = ctx.Nodes.Where(x => x.NodeTypeId == nodeTypeId)
                                    .OrderBy(x => x.Name);
                return await node.ToListAsync();
            }
        }

        public async Task<Node> GetNodeByID(int nodeId)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.Nodes.FirstOrDefaultAsync(x => x.Id == nodeId);
            }
        }

        public async Task Save(Node node)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.Nodes.AddAsync(node);
                ctx.SaveChanges();
            }
        }

        public async Task SaveAll(IList<Node> listToSave)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkInsertAsyncExtended(listToSave, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task UpdateAll(IList<Node> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await ctx.BulkUpdateAsyncExtended(listToUpdate, this._contextAccessor?.HttpContext?.User?.Identity.Name);
            }
        }

        public async Task RemoveAll(IList<Node> listToRemove)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                await UpdateNodePolicyGroupNodePriorities(listToRemove);
                await ctx.BulkDeleteAsync(listToRemove);
            }
        }

        private async Task UpdateNodePolicyGroupNodePriorities(IList<Node> nodesList)
        {
            if (nodesList != null)
            {
                using (var ctx = new RBATContext(this._contextAccessor))
                {
                    var nodePolicyGroups = ctx.NodePolicyGroupNodes.Where(p => p.NodeID == nodesList.First().Id).Select(p => p.NodePolicyGroup).ToList();

                    foreach (var g in nodePolicyGroups)
                    {
                        var priority = ctx.NodePolicyGroupNodes.Where(p => p.NodePolicyGroupID == g.Id)
                                                               .Where(p => p.NodeID == nodesList.First().Id)
                                                               .Select(p => p.Priority)
                                                               .First();

                        var nodePolicyGroupNodes = ctx.NodePolicyGroupNodes.Where(p => p.NodePolicyGroupID == g.Id)
                                                                           .Where(p => p.Priority > priority);

                        if (nodePolicyGroupNodes.Any())
                        {
                            await nodePolicyGroupNodes.ForEachAsync(c => c.Priority = c.Priority - 1);
                            await _nodePolicyGroupNodeService.ChangePriority(await nodePolicyGroupNodes.ToListAsync());
                        }
                    };
                }
            }
        }

        public void UpdateNodeMapData(int nodeId, string nodeMapData)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var nodeEntity = ctx.Nodes.FirstOrDefault(x => x.Id == nodeId);
                if (nodeEntity != null)
                {
                    nodeEntity.MapData = nodeMapData;
                    ctx.SaveChanges();
                }
            }
        }
    }
}
