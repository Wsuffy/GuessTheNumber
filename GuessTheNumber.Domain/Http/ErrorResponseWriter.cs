using System.Net;
using System.Text;
using GuessTheNumber.Domain.Http.Responses;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GuessTheNumber.Domain.Http
{
    public static class ErrorResponseWriter
    {
        public static async Task WriteResponseToContextAsync(this HttpContext context, ErrorResponse errorResponse,
            HttpStatusCode? statusCode = HttpStatusCode.Forbidden)
        {
            context.Response.StatusCode = (int)statusCode!;
            context.Response.ContentType = "application/json";
            var responseJson = JsonConvert.SerializeObject(errorResponse, JsonSerializerSettings);
            await context.Response.WriteAsync(responseJson,
                Encoding.UTF8,
                context.RequestAborted);
        }

        private static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
        };
    }
}