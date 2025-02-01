using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace FetchApi
{
    public static class ModelBindingErrorLogger
    {
        public static JsonResult LogErrorMessages(ModelStateDictionary modelState)
        {
            System.Diagnostics.Debug.WriteLine("MODEL BINDING ERROR");
            var messages = new List<string>();
            foreach (var keyModelStatePair in modelState)
            {
                var key = keyModelStatePair.Key;
                var modelErrors = keyModelStatePair.Value.Errors;
                if (modelErrors.Count > 0)
                {
                    var errorMessages = modelErrors.Select(error => error.ErrorMessage.ToString());
                    foreach (var errorMessage in errorMessages)
                    {
                        System.Diagnostics.Debug.WriteLine($"ModelState error: {errorMessage}");
                        messages.Add(errorMessage);
                    }
                }
            }
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
            result.Add("MODEL BINDING ERROR. Error messages:", messages);
            return new JsonResult(result);
        }
    }
}
