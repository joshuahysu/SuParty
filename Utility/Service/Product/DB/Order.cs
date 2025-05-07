using SuParty.Data.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Service.Product.enums
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public UserData User { get; set; } = null!;

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }



}
