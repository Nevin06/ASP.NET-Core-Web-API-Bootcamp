using Courseproject.Common.Model;
using System.Runtime.Serialization;

namespace Courseproject.Business.Exceptions
{
    [Serializable]
    internal class DependentAddressExistException : Exception
    {
        private Address address;

        public DependentAddressExistException()
        {
        }

        public DependentAddressExistException(Address address)
        {
            this.address = address;
        }

        public DependentAddressExistException(string? message) : base(message)
        {
        }

        public DependentAddressExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DependentAddressExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}