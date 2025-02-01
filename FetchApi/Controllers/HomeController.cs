using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace FetchApi.Controllers
{
    public class Item
    {
        public string ShortDescription { get; set; }
        public float Price { get; set; }

        public Item(string shortDescription, float price)
        {
            this.ShortDescription = shortDescription;
            this.Price = price;
        }
    }
    public class MorningReceipt
    {
        public string Retailer { get; set; }
        public string PurchaseDate { get; set; }
        public string PurchaseTime { get; set; }
        public double Total { get; set; }

        public List<Item> Items { get; set; }

        public MorningReceipt(string retailer, string PurchaseDate, string PurchaseTime, double total, List<Item> items)
        {
            this.Retailer = retailer;
            this.PurchaseDate = PurchaseDate;
            this.PurchaseTime = PurchaseTime;
            this.Total = total;
            this.Items = items;
        }
        public int CalculateTotalScore() // May need to create a custom model binder
        {
            int score = 0;
            score += this.AlphaNumericScore();
            score += this.TotalCostScore();
            score += this.IsMultipleOf25Score();
            score += this.EveryTwoItemsScore();
            score += this.TrimmedLengthScore();
            score += this.IsPurchaseDayOddScore();
            score += this.TimeOfPurchaseScore();

            return score;
        }


        public int AlphaNumericScore() => this.Retailer.Count(char.IsLetterOrDigit);

        public int TotalCostScore() => this.Total % 1 == 0 ? 50 : 0;

        public int IsMultipleOf25Score() => this.Total % 0.25 == 0 ? 25 : 0;

        public int EveryTwoItemsScore() => (int)(Math.Floor((double)(this.Items.Count / 2)) * 5);

        public int TrimmedLengthScore()
        {
            int sumLengthOfTrimmedItems = 0;
            foreach (Item item in this.Items)
            {
                int itemDescriptionLength = item.ShortDescription.Trim().Length;
                if (itemDescriptionLength % 3 == 0)
                {
                    sumLengthOfTrimmedItems += (int)(Math.Ceiling(item.Price * 0.2));
                }
            }
            return sumLengthOfTrimmedItems;
        }

        public int IsPurchaseDayOddScore()
        {
            DateTime date = DateTime.Parse(this.PurchaseDate);
            return date.Day % 2 == 0 ? 0 : 6;
        }

        public int TimeOfPurchaseScore()
        {
            TimeOnly purchaseTime = TimeOnly.Parse(this.PurchaseTime);
            int greaterThan2 = purchaseTime.CompareTo(TimeOnly.Parse("2:00PM"));
            int earlierThan4 = purchaseTime.CompareTo(TimeOnly.Parse("4:00PM"));
            return greaterThan2 > 0 && earlierThan4 < 0 ? 10 : 0;
        }
    }

    public class FetchController : Controller
    {
        IMemoryCache MemoryCache { get; set; }
        public Dictionary<int, int> MorningReceiptScores { get; set; }
        public FetchController(IMemoryCache memoryCache)
        {
            this.MemoryCache = memoryCache;
            this.MorningReceiptScores = new Dictionary<int, int>();
        }

        [Route("/receipts/process")]
        [HttpPost]
        public JsonResult FetchInterviewApi([FromBody] MorningReceipt morningReceipt) // May need to create a custom model binder
        {
            if (!ModelState.IsValid)
                return ModelBindingErrorLogger.LogErrorMessages(ModelState);

            string id = Guid.NewGuid().ToString();
            int score = morningReceipt.CalculateTotalScore();

            this.MemoryCache.Set(id, score);

            return new JsonResult(new KeyValuePair<string, string>("id", id));
        }

        [Route("/receipts/{id}/process")]
        [HttpGet]
        public JsonResult FetchInterviewApi(string Id)
        {
            this.MemoryCache.TryGetValue(Id, out var result);
            if (result != null)
                return new JsonResult(new KeyValuePair<string, int>("points", (int)result));
            else
                return new JsonResult("The provided Id was not found in the in-memory database.");
        }
    }

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
            return new JsonResult(new KeyValuePair<string, List<string>>("MODEL BINDING ERROR. Error messages:", messages));
        }
    }
}
