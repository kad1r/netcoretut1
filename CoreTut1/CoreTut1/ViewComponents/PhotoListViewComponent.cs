using Common.Dtos;
using Common.Helpers;
using CoreTut1.ViewModels;
using Data.Dtos;
using Data.Model;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace CoreTut1.ViewComponents
{
    [ViewComponent(Name = "PhotoList")]
    public class PhotoListViewComponent : ViewComponent
    {
        private IUnitOfWork _uow;
        private int pageSize = 15;

        public PhotoListViewComponent(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IViewComponentResult> InvokeAsync(Paging pm)
        {
            if (pm == null)
            {
                pm = new Paging();
            }

            var vm = new PhotoVM();
            var skipCount = (pm.Page == 1) ? 0 : (pm.Page - 1) * pageSize;
            var searchQuery = ListHelper.GenerateSearchQuery(pm.SearchParams);
            var sorting = ListHelper.GenerateSortQuery(pm.SortParams);

            vm.Search = pm.SearchParams;
            vm.Sort = pm.SortParams;
            vm.List = await _uow.Repository<Photos>()
                .Query(new EfConfig())
                .Include(x => x.Album)
                .Where(searchQuery)
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(pageSize)
                .ToListAsync();

            ListHelper.Paging<Photos>(vm, pageSize, pm.Page, searchQuery, null);

            return View(vm);
        }
    }
}
