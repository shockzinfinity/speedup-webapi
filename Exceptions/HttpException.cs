using System.Net;

namespace speedupApi.Exceptions
{
  public class HttpException : Exception
  {
    public int StatusCode { get; }
    public string MessageDetail { get; set; }

    public HttpException(HttpStatusCode statusCode, string message = null, string messageDetail = null) : base(message)
    {
      StatusCode = (int)statusCode;
      MessageDetail = messageDetail;
    }
  }
}
