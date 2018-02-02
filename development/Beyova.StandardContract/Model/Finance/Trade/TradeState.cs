using System;

namespace Beyova
{
    /// <summary>
    /// Enum TradeState.
    /// Common flow: Created -%gt; Confirmed (-%gt; ) -%gt; Completed (-%gt; RefundRequested -%gt; RefundCompleted ) -%gt; Closed
    /// </summary>
    public enum TradeState
    {
        Unknown = 0,
        Created,
        Confirmed,
        Completed,
        RefundRequested,
        RefundCompleted,
        Closed
    }
}