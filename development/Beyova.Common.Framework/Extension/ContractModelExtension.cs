using System.Diagnostics;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Class ContractModelExtension.
    /// </summary>
    public static partial class ContractModelExtension
    {
        /// <summary>
        /// To the type of the event log entry.
        /// </summary>
        /// <returns>EventLogEntryType.</returns>
        public static EventLogEntryType ToEventLogEntryType(this ExceptionCode exceptionCode)
        {
            EventLogEntryType result = EventLogEntryType.Information;

            if (exceptionCode != null)
            {
                switch (exceptionCode.Major)
                {
                    case ExceptionCode.MajorCode.OperationForbidden:
                    case ExceptionCode.MajorCode.NullOrInvalidValue:
                    case ExceptionCode.MajorCode.ServiceUnavailable:
                    case ExceptionCode.MajorCode.OperationFailure:
                    case ExceptionCode.MajorCode.NotImplemented:
                        result = EventLogEntryType.Error;
                        break;

                    case ExceptionCode.MajorCode.DataConflict:
                    case ExceptionCode.MajorCode.CreditNotAfford:
                    case ExceptionCode.MajorCode.ResourceNotFound:
                        result = EventLogEntryType.Warning;
                        break;

                    default:
                        break;
                }
            }

            return result;
        }
    }
}