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
    public class ChannelPolicyGroupChannelController : Controller
    {        
        private readonly IChannelPolicyGroupChannelService _channelPolicyGroupChannelService;

        public ChannelPolicyGroupChannelController(IChannelPolicyGroupChannelService channelPolicyGroupChannelService)
        {
            _channelPolicyGroupChannelService = channelPolicyGroupChannelService;
        }
       
        public async Task<IActionResult> FillGridFromDB(int channelPolicyGroupID, List<Channel> importList)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var data = (importList != null && importList.Any()) ? importList : await _channelPolicyGroupChannelService.Get(channelPolicyGroupID);
                int recordsTotal = data.Count;
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> UpdateAll(List<ChannelPolicyGroupChannel> listToUpdate)
        {
            try
            {
                await _channelPolicyGroupChannelService.ChangePriority(listToUpdate);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }       
    }
}