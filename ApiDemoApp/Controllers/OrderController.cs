using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDemoApp.Controllers
{
    [Route("api/[controller]")] //this is how you know the name of the path
                                //like https://localhost:5001/api/order
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IFoodData _foodData;
        private readonly IOrderData _orderData;

        //create the ctor:
        public OrderController(IFoodData foodData, IOrderData orderData)
        {
            _foodData = foodData;
            _orderData = orderData;
        }

        //create methods:

        [HttpPost]
        [ValidateModel] //see notes in ValidateModelAttribute.cs
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //those last two lines say what possible responses might come from this method.
        public async Task<IActionResult> Post(OrderModel order)
        {
            //you often don't HAVE to return stuff.
            //but having something returned makes troubleshooting easier.
            //esp for Http which will return standard error codes.

            var food = await _foodData.GetFood();

            order.Total = order.Quantity * food.Where(x => x.Id == order.FoodId).First().Price;

            int id = await _orderData.CreateOrder(order);

            //return Ok(id); //sends back both the OK plus the id of the newly created order.
            //but that's ALL it returns. just a number. So... do this instead:
            return Ok(new { Id = id }); //now you get { "id": 11004 }. Much nicer.
        }

        [HttpGet("{id}")] //"{id}" is what gets added to the end of the path
                          //ie, it's the URL parameter like https://localhost:5001/api/order/11004
        [ValidateModel] //see notes in ValidateModelAttribute.cs
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //OMG CTRL-J will show the intellisense
        public async Task<IActionResult> Get(int id)
        {
            if (id == 0)
            {
                return BadRequest(); //400
            }

            var order = await _orderData.GetOrderById(id);

            if (order != null)
            {
                var food = await _foodData.GetFood();

                var output = new
                {
                    Order = order,
                    ItemPurchased = food.Where(x => x.Id == order.FoodId).FirstOrDefault()?.Title
                }; //this is an anonymous object, to be used in the return OK.

                return Ok(output); //200
            }
            else
            {
                return NotFound(); //404
            }
        }
    }
}




