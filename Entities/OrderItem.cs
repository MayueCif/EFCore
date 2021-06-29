using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreWeb.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        #region    实际业务中表会相对复杂随便增加几个字段 增加表的复杂性

        public string Property1 { get; set; }

        public string Property2 { get; set; }

        public string Property3 { get; set; }

        public string Property4 { get; set; }

        public string Property5 { get; set; }

        public string Property6 { get; set; }

        #endregion

        
    }
}
