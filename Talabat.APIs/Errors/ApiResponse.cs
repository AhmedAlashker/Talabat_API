
namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statuscode, string? message = null)
        {
            StatusCode = statuscode;
            Message = message ?? GetDefailtMessageForStatusCode(statuscode);
        }

        private string? GetDefailtMessageForStatusCode(int statuscode)
        {
            return statuscode switch
            {
                400 => "A Bad Request, you have made",
                401 => "Unautorized",
                404 => "Resource was not found",
                500 => "Errors are the path to the dark side. Errors lead to anger. Hare leades to career change",
                _ => null,
            };
        }
    }
}
