namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TItem1">The type of the item.</typeparam>
    /// <typeparam name="TItem2">The type of the item2.</typeparam>
    /// <typeparam name="TOperator">The type of the operator.</typeparam>
    public interface IExpression<TItem1, TItem2, TOperator>
    {
        /// <summary>
        /// Gets or sets the item left.
        /// </summary>
        /// <value>
        /// The item left.
        /// </value>
        TItem1 ItemLeft { get; set; }

        /// <summary>
        /// Gets or sets the item right.
        /// </summary>
        /// <value>
        /// The item right.
        /// </value>
        TItem2 ItemRight { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        TOperator Operator { get; set; }
    }
}