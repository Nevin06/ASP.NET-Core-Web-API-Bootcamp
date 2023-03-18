using System.Runtime.Serialization;

namespace Courseproject.Business.Exceptions;

[Serializable]
public class AddressNotFoundException : Exception
{
    public int Id;

    public AddressNotFoundException()
    {
    }

    public AddressNotFoundException(int id)
    {
        this.Id = id;
    }

    public AddressNotFoundException(string? message) : base(message)
    {
    }

    public AddressNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected AddressNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}