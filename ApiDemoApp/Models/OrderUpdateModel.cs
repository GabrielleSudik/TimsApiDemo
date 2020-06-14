using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDemoApp.Models
{
    //we created this class as the thing that gets returned
    //when an order is updated.

    public class OrderUpdateModel
    {
        public int Id { get; set; }

        public string OrderName { get; set; }
    }
}
