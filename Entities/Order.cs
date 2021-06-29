using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreWeb.Entities
{
    public class Order
    {
        private ICollection<OrderItem> _orderItems;

        /// <summary>
        /// Gets or sets order items
        /// </summary>
        public virtual ICollection<OrderItem> OrderItems
        {
            get => _orderItems ?? (_orderItems = new List<OrderItem>());
            protected set => _orderItems = value;
        }

        //简单的订单实体，省略收件人等信息
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public int State { get; set; }

        public string OrderNo { get; set; }

        public decimal OrderTotal { get; set; }

        public DateTime PaymentTime { get; set; }

        public string ExpressNo { get; set; }

        public string Remarks { get; set; }

        public bool Deleted { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }


    }
}
