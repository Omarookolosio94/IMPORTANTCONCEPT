using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Customvalidation.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections;

namespace Customvalidation.Filters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorHashTable = new Hashtable();

                foreach (var x in context.ModelState.Keys)
                {
                  
                    var errors = new List<string>();

                    foreach(var v in context.ModelState[x].Errors)
                    {
                        errors.Add(v.ErrorMessage);
                    }

                    errorHashTable.Add(x, errors);
                }

                context.Result = new UnprocessableEntityObjectResult(
                    new ResponseModel<Hashtable>
                    {
                        Status = false,
                        StatusCode = StatusCodes.Status422UnprocessableEntity,
                        Message = "Error validating model",
                        Data = errorHashTable
                    });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) {

            if (!context.ModelState.IsValid)
            {
                // context.Result =  new UnprocessableEntityObjectResult(context.ModelState);

                var errorHashTable = new Hashtable();

                foreach (var x in context.ModelState.Keys)
                {

                    var errors = new List<string>();

                    foreach (var v in context.ModelState[x].Errors)
                    {
                        errors.Add(v.ErrorMessage);
                    }

                    errorHashTable.Add(x, errors);
                }

                context.Result = new BadRequestObjectResult(
                    new ResponseModel<Hashtable>
                    {
                        Status = false,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Error validating model",
                        Data = errorHashTable
                    });
            }
        }
    }
}

// add to model error
ModelState.AddModelError(nameof(createBookInputModel.Description), "Book description should contain book title!");
