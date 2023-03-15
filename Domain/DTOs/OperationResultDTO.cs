using System.Net;

namespace Domain.DTOs;

public class OperationResultDTO
{
    public HttpStatusCode StatusCode { get; set; }

    public string Message { get; set; }

}

public class OperationResultDTO<T> : OperationResultDTO
{
    public T Data { get; set; }
}
