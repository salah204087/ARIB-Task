using System.Net;

namespace Domain.Common;

public class OperationResult
{
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public bool Success { get; set; } = true;
    public string ErrorMessage { get; set; } = string.Empty;
    public string SuccessMessage { get; set; } = string.Empty;
}
