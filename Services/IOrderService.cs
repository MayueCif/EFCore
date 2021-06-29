using EFCoreWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreWeb.Services
{
    public interface IOrderService
    {

        Task GetByIdAsync(int id);

        Task GetOrdersAsync(int customerId,int pageIndex,int pageSize);

        Task SaleTotalAsync();

        Task ProductReports();

        #region   Sql

        void GetByIdFromSql(int id);

        void GetOrdersSql(int customerId, int pageIndex, int pageSize);

        Task<decimal> SaleTotalSql();

        void CountAndAny();

        #endregion

    }
}
