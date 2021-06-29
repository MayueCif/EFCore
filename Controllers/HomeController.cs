using EFCoreWeb.Models;
using EFCoreWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EFCoreWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        #region  view
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion

        #region   ef test

        public async Task Get() {

            using (MiniProfiler.Current.Step("EF Method"))
            {
                await _orderService.GetByIdAsync(1360);
            }

            using (MiniProfiler.Current.Step("Sql Method"))
            {
                _orderService.GetByIdFromSql(1362);
            }

        }


        public async Task QueryByCustomerId()
        {
            using (MiniProfiler.Current.Step("EF Method"))
            {
                await _orderService.GetOrdersAsync(1487, 1, 20);
            }

            //using (MiniProfiler.Current.Step("Sql Method"))
            //{
            //    _orderService.GetOrdersSql(1487, 1, 20);
            //}
        }

        
        public async Task QueryProductReports() {
            using (MiniProfiler.Current.Step("EF Method"))
            {
                await _orderService.ProductReports();
            }
        }


        public async Task GetSaleTotal()
        {

            using (MiniProfiler.Current.Step("EF Method"))
            {
                await _orderService.SaleTotalAsync();
            }

            using (MiniProfiler.Current.Step("Sql Method"))
            {
                await _orderService.SaleTotalSql();
            }
            
        }

        public async Task ForeachOrders() 
        {
            using (MiniProfiler.Current.Step("EF Method"))
            {
                await _orderService.SaleTotalAsync();
            }
        }

        public void CountAndAny()
        {
            _orderService.CountAndAny();
        }

        #endregion
    }
}
