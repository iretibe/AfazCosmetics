using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
namespace API.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse() : base(400)
        {

        }

        public IEnumerable<string> Errors { get; set; }
    }
}