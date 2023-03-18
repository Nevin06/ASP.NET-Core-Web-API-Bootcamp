using Courseproject.Common.Model;
using System.Runtime.Serialization;

namespace Courseproject.Business.Exceptions
{
    [Serializable]
    internal class DependentJobExistException : Exception
    {
        private Job job;

        public DependentJobExistException()
        {
        }

        public DependentJobExistException(Job job)
        {
            this.job = job;
        }

        public DependentJobExistException(string? message) : base(message)
        {
        }

        public DependentJobExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DependentJobExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}