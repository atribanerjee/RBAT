using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RBAT.Core;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Logic
{
    public class NodePolicyGroupNodeService : INodePolicyGroupNodeService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public NodePolicyGroupNodeService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<IList<Node>> Get(int nodePolicyGroupID)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                return await ctx.NodePolicyGroupNodes
                                .Include(c => c.Node)
                                .Where(c => c.NodePolicyGroupID == nodePolicyGroupID)
                                .OrderBy(c => c.Priority)
                                .Select(c => c.Node)                                
                                .ToListAsync();
            }
        }

        public async Task ChangePriority(IList<NodePolicyGroupNode> listToUpdate)
        {
            using (var ctx = new RBATContext(this._contextAccessor))
            {
                var config = new BulkConfig
                {
                    PropertiesToInclude = new List<string> {
                        nameof(NodePolicyGroupNode.Priority)
                    },
                    UpdateByProperties = new List<string> {
                        nameof(NodePolicyGroupNode.NodePolicyGroupID),
                        nameof(NodePolicyGroupNode.NodeID)
                    }
                };
                await ctx.BulkUpdateAsync(listToUpdate, config);
            }
        }
    }    
}
