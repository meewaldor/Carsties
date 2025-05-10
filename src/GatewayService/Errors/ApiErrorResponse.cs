using Microsoft.AspNetCore.Http;

namespace GatewayService.Errors
{
    public class ApiErrorResponse(int statusCode, string message, string? details = null)
    {
        public int StatusCode { get; set; } = statusCode;
        public string Message { get; set; } = message;
        public string? Details { get; set; } = details;
    }
}
