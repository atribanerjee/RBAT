using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RBAT.Web.Controllers
{
    public class NodePolicyGroupNodeController : Controller
    {        
        private readonly INodePolicyGroupNodeService _nodePolicyGroupNodeService;

        public NodePolicyGroupNodeController(INodePolicyGroupNodeService nodePolicyGroupNodeService)
        {
            _nodePolicyGroupNodeService = nodePolicyGroupNodeService;
        }      

        public async Task<IActionResult> FillGridFromDB(int nodePolicyGroupID, List<Node> importList)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var data = (importList != null && importList.Any()) ? importList : await _nodePolicyGroupNodeService.Get(nodePolicyGroupID);
                int recordsTotal = data.Count;
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> UpdateAll(List<NodePolicyGroupNode> listToUpdate)
        {
            try
            {
                await _nodePolicyGroupNodeService.ChangePriority(listToUpdate);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }       
    }
}