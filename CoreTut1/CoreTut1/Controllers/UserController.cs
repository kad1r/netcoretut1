using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Common.Dtos;
using Common.Enums;
using Common.Helpers;
using CoreTut1.ViewModels;
using Data.Dtos;
using Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CoreTut1.Controllers
{
    public class UserController : BaseController
    {
        #region Constructor

        private IMemoryCache _cache;

        public UserController(IMemoryCache cache)
        {
            _cache = cache;
        }

        protected override void Dispose(bool disposing)
        {
            _uow.Dispose();
            base.Dispose(disposing);
        }

        #endregion Constructor

        public IActionResult Index()
        {
            return View(new UserVM());
        }

        public async Task<IActionResult> Form(string id)
        {
            var vm = new UserVM
            {
                ToolBar = new ToolbarAuth(),
                AspNetUser = new AspNetUsers()
            };

            if (id != null)
            {
                vm.AspNetUser = await _uow.Repository<AspNetUsers>()
                    .Query(x => x.Id == id, new EfConfig())
                    .FirstOrDefaultAsync();
            }

            //await FormInit(vm);
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Form(UserVM vm)
        {
            //var qs = Request.UrlReferrer.Query.ToString();

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(vm.AspNetUser.Id))
                {
                    var existing = await _uow.Repository<AspNetUsers>()
                        .Query(x => x.Id == vm.AspNetUser.Id, new EfConfig())
                        .FirstOrDefaultAsync();

                    if (existing != null)
                    {
                        try
                        {
                            _uow.Repository<AspNetUsers>().QuickUpdate(existing, vm.AspNetUser);
                            await _uow.SaveChangesAsync();
                            TempData["PageStates"] = (int)PageStatesEnum.Updated;
                        }
                        catch (Exception ex)
                        {
                            TempData["PageStates"] = (int)PageStatesEnum.NotUpdated;
                            var msg = ex.Message;
                            // error log
                        }
                    }
                }
                else
                {
                    try
                    {
                        _uow.Repository<AspNetUsers>().Add(vm.AspNetUser);
                        await _uow.SaveChangesAsync();
                        TempData["PageStates"] = (int)PageStatesEnum.Created;

                        return Redirect(Url.Action("Form", new { id = vm.AspNetUser.Id }));
                        //return Redirect(Url.Action("Form", new { id = vm.XmlRule.Id }) + qs);
                    }
                    catch (Exception ex)
                    {
                        TempData["PageStates"] = (int)PageStatesEnum.NotCreated;
                        var msg = ex.Message;
                        // error log
                    }
                }
            }

            //await FormInit(vm);
            return View(vm);
        }
    }
}