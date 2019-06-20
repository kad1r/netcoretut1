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
    public class PhotoController : BaseController
    {
        #region Constructor

        private IMemoryCache _cache;

        public PhotoController(IMemoryCache cache)
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
            return View(new PhotoVM());
        }

        public async Task<IActionResult> Form(int id)
        {
            var vm = new PhotoVM
            {
                ToolBar = new ToolbarAuth(),
                Photo = new Photos()
            };

            if (id > 0)
            {
                vm.Photo = await _uow.Repository<Photos>()
                    .Query(x => x.Id == id, new EfConfig())
                    .FirstOrDefaultAsync();
            }

            //await FormInit(vm);
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Form(PhotoVM vm)
        {
            //var qs = Request.UrlReferrer.Query.ToString();

            if (ModelState.IsValid)
            {
                if (vm.Photo.Id > 0)
                {
                    var existing = await _uow.Repository<Photos>()
                        .Query(x => x.Id == vm.Photo.Id, new EfConfig())
                        .FirstOrDefaultAsync();

                    if (existing != null)
                    {
                        try
                        {
                            _uow.Repository<Photos>().QuickUpdate(existing, vm.Photo);
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
                        _uow.Repository<Photos>().Add(vm.Photo);
                        await _uow.SaveChangesAsync();
                        TempData["PageStates"] = (int)PageStatesEnum.Created;

                        return Redirect(Url.Action("Form", new { id = vm.Photo.Id }));
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

        [HttpPost]
        public IActionResult RefreshComponent(Paging pm)
        {
            return ViewComponent("PhotoList", pm);
        }
    }
}