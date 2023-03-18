using Courseproject.Common.Model;
using System.Runtime.Serialization;

namespace Courseproject.Business.Exceptions
{
    [Serializable]
    internal class DependentTeamsExistException : Exception
    {
        private List<Team> teams;

        public DependentTeamsExistException()
        {
        }

        public DependentTeamsExistException(List<Team> teams)
        {
            this.teams = teams;
        }

        public DependentTeamsExistException(string? message) : base(message)
        {
        }

        public DependentTeamsExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DependentTeamsExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}