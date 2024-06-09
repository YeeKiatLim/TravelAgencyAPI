using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;

namespace TravelAgencyAPI.Controllers
{
    [ApiController]
    [Route("api / [controller]")]
    public class BookingController : ControllerBase
    {
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
            string tourName = "";
            double totalPersons = 0;
            double totalAmount = 0;
            if (request.QueryResult.Action == "book")
            {
                //Parse the intent params
                var requestParameters = request.QueryResult.Parameters;
                totalPersons = requestParameters.Fields["totalPersons"].NumberValue;
                totalAmount = totalPersons*100;
            };
            // Populate the response
            WebhookResponse response = new WebhookResponse
            {
                FulfillmentText = $"Thank you for choosing our hotel, your total amount will be { totalAmount} USD."
            };
            // Ask Protobuf to format the JSON to return.
            // Again, we don’t want to use Json.NET — it doesn’t know how to handle Struct
            // values etc.
            string responseJson = response.ToString();
            return Content(responseJson, "application/json");
        }
    }
}
