using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Service.Product.enums
{
    public class Payment
    {
        public int Id { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
        public PaymentStatus Status { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }


}
