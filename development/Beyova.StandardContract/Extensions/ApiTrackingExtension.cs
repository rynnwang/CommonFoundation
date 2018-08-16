using System.Text;
using Beyova.ApiTracking;

namespace Beyova.Api
{
    /// <summary>
    /// Class ApiTrackingExtension.
    /// </summary>
    public static class ApiTrackingExtension
    {
        /// <summary>
        /// APIs the event log to string.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns>System.String.</returns>
        public static string ApiEventLogToString(this ApiEventLog log)
        {
            var builder = new StringBuilder(512);

            if (log != null)
            {
                builder.AppendLineWithFormat("{0}: {1}", log.CreatedStamp.ToFullDateTimeString(), log.ApiFullName);
                builder.AppendLine(log.ToJson());
            }

            return builder.ToString();
        }

        /// <summary>
        /// APIs the message to string.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static string ApiMessageToString(this ApiMessage message)
        {
            var builder = new StringBuilder(512);

            if (message != null)
            {
                builder.AppendLineWithFormat("{0}: {1}", message.CreatedStamp.ToFullDateTimeString(), message.Category);
                builder.AppendLine(message.Message);
            }

            return builder.ToString();
        }

        /// <summary>
        /// APIs the trace log to string.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="log">The log.</param>
        /// <param name="level">The level.</param>
        /// <returns>System.String.</returns>
        private static void ApiTraceLogToString(StringBuilder builder, ApiTraceLogPiece log, int level)
        {
            if (builder != null && log != null)
            {
                builder.AppendIndent(level);
                builder.AppendLineWithFormat("Entry: {0}", log.EntryStamp.ToFullDateTimeString());
                builder.AppendIndent(level);
                builder.AppendLineWithFormat("Exit: {0}", log.ExitStamp.ToFullDateTimeString());
                builder.AppendIndent(level);
                builder.AppendLineWithFormat("Exception Key: {0}", log.ExceptionKey);
                builder.AppendIndent(level);

                foreach (var one in log.InnerTraces)
                {
                    builder.AppendLineWithFormat("Inner trace: ");
                    ApiTraceLogToString(builder, one, level + 1);
                }
            }
        }

        /// <summary>
        /// APIs the trace log to string.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns>System.String.</returns>
        public static string ApiTraceLogToString(this ApiTraceLog log)
        {
            StringBuilder builder = new StringBuilder();

            if (log != null)
            {
                builder.AppendLineWithFormat("Trace ID: {0}", log.TraceId);
                ApiTraceLogToString(builder, log, 0);
            }

            return builder.ToString();
        }
    }
}