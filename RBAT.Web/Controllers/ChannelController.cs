using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RBAT.Web.Models.Channel;

namespace RBAT.Web.Controllers
{
    [Authorize]
    public class ChannelController : Controller
    {
        private readonly IChannelService _channelService;
        private readonly INodeService _nodeService;

        public ChannelController(IChannelService channelService, INodeService nodeService)
        {
            _channelService = channelService;
            _nodeService = nodeService;
        }

        /// <summary>
        /// Returns the model state errors as JSON.
        /// </summary>
        public JsonResult ModelStateErrors
        {
            get
            {
                var errorModel =
                    from x in ModelState.Keys
                    where ModelState[x].Errors.Count > 0
                    select new
                    {
                        key = x,
                        errors = ModelState[x].Errors.Select(y => y.ErrorMessage).ToArray()
                    };
                return Json(new { success = false, errors = errorModel });
            }
        }
        public IActionResult Index()
        {
            ViewBag.title = "Channel List";
            return View();
        }

        public IActionResult Add()
        {
            var model = new ChannelModel();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var channel = await _channelService.GetChannelByID(id);
                var model = channel.ToChannelModel();
                return View(model);
            }
            catch(Exception ex) {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> GetAll()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                IEnumerable<Channel> channels = await _channelService.GetAll();
                var model = channels.ToChannelViewModelList();
                return Json(new { draw, recordsFiltered = model.Count(), recordsTotal = model.Count(), data = model });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public async Task<IActionResult> AddNew(ChannelModel channelModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _channelService.Save(channelModel.ToChannelEntityModel());
                    return await Task.FromResult(Json(new { success = true }));
                }
                return ModelStateErrors;
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Remove(List<Channel> listToRemove)
        {
            try
            {
                await _channelService.RemoveAll(listToRemove);
                return await Task.FromResult(Json(new { }));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public async Task<IActionResult> Update(ChannelModel channelModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var listToUpdate = new List<Channel> { channelModel.ToChannelEntityModel() };
                    await _channelService.UpdateAll(listToUpdate);
                    return await Task.FromResult(Json(new { success = true }));
                }
                return ModelStateErrors;

            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }        
    }
}