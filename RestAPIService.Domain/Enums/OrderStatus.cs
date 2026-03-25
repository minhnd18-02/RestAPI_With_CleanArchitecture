using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Domain.Enums
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Shipping,
        Delivered,
        Completed,
        Cancelled
    }
}
