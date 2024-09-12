using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TextMatch.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        internal string GetModelErrors(ModelStateDictionary modelState)
        {
            var modelStateErrors = modelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage);            
            return string.Join(Environment.NewLine, modelStateErrors.ToArray());
        }
    }
}
