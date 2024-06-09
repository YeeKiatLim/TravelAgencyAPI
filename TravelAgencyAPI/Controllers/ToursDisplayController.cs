using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using TravelAgencyAPI.Models;

namespace TravelAgencyAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class ToursDisplayController : ControllerBase
    {
        private readonly ToursDbContext _context;

        public ToursDisplayController(ToursDbContext context)
        {
            _context = context;
        }

        // A Protobuf JSON parser configured to ignore unknown fields. This makes
        // the action robust against new fields being introduced by Dialogflow.
        private static readonly JsonParser jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
        [HttpPost]
        public ContentResult DialogAction()
        {
            // Parse the body of the request using the Protobuf JSON parser,
            // *not* Json.NET.
            WebhookRequest request;
            using (var reader = new StreamReader(Request.Body))
            {
                request = jsonParser.Parse<WebhookRequest>(reader);
            }
            string country= "";
            string tourType= "";
            var list = new List<Tour>();
            if (request.QueryResult.Action == "search")
            {
                //Parse the intent params
                var requestParameters = request.QueryResult.Parameters;
                country = requestParameters.Fields["countries"].StringValue;
                tourType = requestParameters.Fields["tour_types"].StringValue;

                IQueryable<Tour> result = _context.Tours;
                if (country != null)
                {
                    result = result.Where(x => x.Country.Contains(country));
                }
                if (tourType != null)
                {
                    result = result.Where(x => x.Type.Contains(tourType));
                }
                list = result.OrderByDescending(x => x.CreatedAt).ToList();
            };
            // Populate the response
            WebhookResponse response = new WebhookResponse
            {
                FulfillmentText = $"Test"
            };
            // Ask Protobuf to format the JSON to return.
            // Again, we don’t want to use Json.NET — it doesn’t know how to handle Struct
            // values etc.
            string responseJson = response.ToString();
            return Content(responseJson, "application/json");
        }
    }
}
