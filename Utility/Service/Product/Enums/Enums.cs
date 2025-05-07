using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Service.Product.enums
{
    public enum OrderStatus
    {
        Pending,
        Paid,
        Shipped,
        Completed,
        Cancelled
    }

    public enum PaymentMethod
    {
        CreditCard,
        BankTransfer,
        Paypal,
        LinePay,
        Other
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed,
        Refunded
    }

}
