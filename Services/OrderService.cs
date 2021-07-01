using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using EFCoreWeb.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreWeb.Services
{

    [SimpleJob(RuntimeMoniker.CoreRt50, targetCount: 100)]
    [RPlotExporter]
    [HtmlExporter]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [MemoryDiagnoser]
    public class OrderService :IOrderService
    {
        private readonly DataDBContext _dataDBContext;

        public OrderService(DataDBContext dataDBContext)
        {
            _dataDBContext = dataDBContext;
        }

        [Benchmark]
        [Arguments(100)]
        public async Task GetByIdAsync(int id)
        {
            //var order1 = await _dataDBContext.Orders.FindAsync(1362);
            //var order = await _dataDBContext.Orders.Include(a => a.OrderItems).FirstOrDefaultAsync(a => a.Id == id);
            var order = await _dataDBContext.Orders.FindAsync(id);
            var itemCount = order.OrderItems.Count();
            Console.WriteLine($"GetByIdAsync:itemCount{itemCount}");
        }

   
        public async Task GetOrdersAsync(int customerId, int pageIndex, int pageSize)
        {

            var order = await _dataDBContext.Orders.Where(a => a.CustomerId == customerId)
                .OrderBy(a => a.CreatedTime)
                .Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .Include(a => a.OrderItems).Include(a => a.Customer)
                .TagWith("--Get Orders").AsSplitQuery().FirstOrDefaultAsync();

            var orders = await _dataDBContext.Orders.Where(a => a.CustomerId == customerId)
                .OrderBy(a => a.CreatedTime)
                .Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .Include(a => a.OrderItems).Include(a => a.Customer)
                .TagWith("--Get Orders").AsSplitQuery().ToListAsync();

            var count1 = 0;
            foreach (var _order in orders)
            {
                count1 += order.OrderItems.Count;
            }

            Console.WriteLine($"count1:{count1}");

        }


        Func<DataDBContext, decimal, IQueryable<int>> errorCompiled =
            EF.CompileQuery<DataDBContext, decimal, IQueryable<int>>(
            (ctx, total) => ctx.OrderItems.AsNoTracking().IgnoreAutoIncludes()
            .GroupBy(a => a.ProductId).Select(a => new
            {
                ProductId = a.Key,
                Quantity = a.Sum(b => b.Quantity),
                Price = a.Sum(b => b.Price),
            }).Where(a => a.Price > total).Select(a => a.ProductId));

        [Benchmark]
        public async Task ProductReports()
        {
            //var productIds = await _dataDBContext.OrderItems.IgnoreAutoIncludes().AsNoTracking()
            //    .GroupBy(a=>a.ProductId).Select(a => new {
            //        ProductId = a.Key,
            //        Quantity = a.Sum(b => b.Quantity),
            //        Price = a.Sum(b => b.Price),
            //    }).Where(a=>a.Price>100000).Select(a=>a.ProductId)
            //    .ToListAsync();
            var compiledProductReports = EF.CompileQuery(
                (DataDBContext ctx, decimal total)
                    => ctx.OrderItems.AsNoTracking().IgnoreAutoIncludes()
                .GroupBy(a => a.ProductId).Select(a => new
                {
                    ProductId = a.Key,
                    Quantity = a.Sum(b => b.Quantity),
                    Price = a.Sum(b => b.Price),
                }).Where(a => a.Price > total).Select(a => a.ProductId));

            var productIds = compiledProductReports(_dataDBContext, 100000).ToList();

        }

        [Benchmark]
        public async Task SaleTotalAsync()
        {
            var total = await _dataDBContext.Orders.SumAsync(a => a.OrderTotal);
        }


        [Benchmark]
        [Arguments(100)]
        public void GetByIdFromSql(int id)
        {
            var order = _dataDBContext.Orders.FromSqlInterpolated($"select * from orders where id={id}").FirstOrDefault();
            var itemCount = order.OrderItems.Count();
            Console.WriteLine($"GetByIdFromSql:itemCount{itemCount}");
        }

        public void GetOrdersSql(int customerId, int pageIndex, int pageSize) 
        {
            var orders = _dataDBContext.Orders.FromSqlInterpolated($"select * from orders where CustomerId={customerId} ORDER BY CreatedTime limit {(pageIndex-1)* pageSize},{pageSize}");
        }

        [Benchmark]
        public async Task<decimal> SaleTotalSql() {

            decimal totalSale=0;
            //获取数据库的上下文连接
            var conn = _dataDBContext.Database.GetDbConnection();
            try
            {    
                //打开数据库连接
                await conn.OpenAsync();
                //建立连接
                using (var command = conn.CreateCommand())
                {
                    var query = "select sum(OrderTotal) from orders";
                    command.CommandText = query;//赋值需要执行的SQL语句
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)//判断是否有返回行
                        {
                            while (await reader.ReadAsync())
                            {
                                totalSale = reader.GetDecimal(0);
                            }
                        }
                    }
                }
            }
            finally
            {  
                conn.Close();
            }
            return totalSale;
        }


        public void CountAndAny() 
        {
            using (MiniProfiler.Current.Step("EF Count"))
            {
                var b = _dataDBContext.Orders.Count(a=>a.Id==10)>0;
            }

            using (MiniProfiler.Current.Step("EF FirstOrDefault"))
            {
                var b = _dataDBContext.Orders.FirstOrDefault(a => a.Id == 10)!=null;
            }

            using (MiniProfiler.Current.Step("EF Any"))
            {
                var b = _dataDBContext.Orders.Any(a => a.Id == 10);
            }
        }

    }

}
