using System.Text.Json;

namespace speedupApi.Exceptions
{
  public class ExceptionMessage
  {
    public string Message { get; set; }
    public ExceptionMessage() { }
    public ExceptionMessage(string message)
    {
      Message = message;
    }

    public override string ToString()
    {
      return JsonSerializer.Serialize(new { message = new string(Message) });
    }
  }
}
