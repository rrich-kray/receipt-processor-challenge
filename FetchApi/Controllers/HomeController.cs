using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FetchApi.Controllers
{
    public class FetchController : Controller
    {
        private IMemoryCache Cache { get; set; }
        public FetchController(IMemoryCache memoryCache)
        {
            this.Cache = memoryCache;
        }

        [Route("/receipts/process")]
        [HttpPost]
        public JsonResult FetchInterviewApi([FromBody] MorningReceipt morningReceipt)
        {
            if (!ModelState.IsValid)
                return ModelBindingErrorLogger.LogErrorMessages(ModelState);

            string id = Guid.NewGuid().ToString();
            int score = morningReceipt.CalculateTotalScore();

            this.Cache.Set(id, score);

            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("id", id);
            return new JsonResult(result);
        }

        [Route("/receipts/{id}/process")]
        [HttpGet]
        public JsonResult FetchInterviewApi(string Id)
        {
            this.Cache.TryGetValue(Id, out var result);
            if (result != null)
            {
                Dictionary<string, int> resultDict = new Dictionary<string, int>();
                resultDict.Add("points", (int)result);
                return new JsonResult(resultDict);
            }
            else
                return new JsonResult("The provided Id was not found in the in-memory database.");
        }
    }
}
