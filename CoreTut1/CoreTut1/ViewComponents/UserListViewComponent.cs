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
    [ViewComponent(Name = "UserList")]
    public class UserListViewComponent : ViewComponent
    {
        private IUnitOfWork _uow;
        private int pageSize = 15;

        public UserListViewComponent(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page = 1, List<SearchObj> searchParams = null, List<SortObj> sortParams = null)
        {
            var vm = new UserVM();
            var skipCount = (page == 1) ? 0 : (page - 1) * pageSize;
            var searchQuery = ListHelper.GenerateSearchQuery(searchParams);
            var sorting = ListHelper.GenerateSortQuery(sortParams);

            vm.Search = searchParams;
            vm.Sort = sortParams;
            vm.List = await _uow.Repository<AspNetUsers>()
                .Query(new EfConfig())
                .Where(searchQuery)
                .OrderBy(sorting)
                .Skip(skipCount)
                .Take(pageSize)
                .ToListAsync();

            ListHelper.Paging<AspNetUsers>(vm, pageSize, page, searchQuery, null);

            return View(vm);
        }
    }
}
