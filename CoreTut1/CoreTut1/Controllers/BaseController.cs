using Common.Helpers;
using Data.Model;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreTut1.Controllers
{
    public class BaseController : Controller
    {
        public int pageSize = 15;
        private bool redirect = false;
        private RedirectToRouteResult redirectRoute;
        private Dictionary<string, Microsoft.Extensions.Primitives.StringValues> qs = null;

        public string AppRoot { get { return HttpContext.RequestServices?.GetService<IHostingEnvironment>().ContentRootPath; } }
        public string WebRoot { get { return HttpContext.RequestServices?.GetService<IHostingEnvironment>().WebRootPath; } }
        public IConfiguration Configuration { get { return HttpContext.RequestServices?.GetService<IConfiguration>(); } }

        protected IUnitOfWork _uow { get; }

        public bool isDevelopment = ConfigurationHelper.IsDevelopment();
        public bool IsStaging = ConfigurationHelper.IsStaging();
        public bool IsProduction = ConfigurationHelper.IsProduction();

        public BaseController()
        {
            _uow = new UnitOfWork(new PhotographyContext());
        }

        protected override void Dispose(bool disposing)
        {
            _uow.Dispose();
            base.Dispose(disposing);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            BeReady();
            base.OnActionExecuting(context);
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            BeReady();
            return base.OnActionExecutionAsync(context, next);
        }

        public virtual void BeReady()
        {
        }
    }
}
