using System.Net;

namespace Domain.DTOs;

public class OperationResultDTO
{
    public HttpStatusCode StatusCode { get; set; }

    public string Message { get; set; }
}
