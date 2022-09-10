#region Imports

using System;

#endregion

namespace Domain.Exceptions
{
    public class DbEntityValidationException : Exception
    {
        public DbEntityValidationException(string message) : base(message)
        {
        }
    }
}