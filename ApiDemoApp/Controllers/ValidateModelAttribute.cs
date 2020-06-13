using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDemoApp.Controllers
{
    public class ValidateModelAttribute : ActionFilterAttribute
        //something key: the whole class is called ValidateModelAttribute
        //but "ValidateModel" is more like the name,
        //and "Attribute" is more like the type.
        //so when you decorate other methods to use this,
        //you decorate with "[ValidateModel]" only.
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid == false)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }

    //a whole lot in this class is just stuff Tim knows what to type.
    //but the gist is: if whatever model isn't valid, 
    //the user will get an error message (ie, a BadRequest message).

    //Compare to your UI projects. In the methods, you have if statements
    //like "if model isn't valid, return the same blank page."
    //This validator takes the place of that.
}
