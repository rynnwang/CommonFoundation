namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TItemLeft">The type of the item left.</typeparam>
    /// <typeparam name="TItemRight">The type of the item right.</typeparam>
    /// <typeparam name="TOperator">The type of the operator.</typeparam>
    public interface IExpression<TItemLeft, TItemRight, TOperator>
    {
        /// <summary>
        /// Gets or sets the item left.
        /// </summary>
        /// <value>
        /// The item left.
        /// </value>
        TItemLeft ItemLeft { get; set; }

        /// <summary>
        /// Gets or sets the item right.
        /// </summary>
        /// <value>
        /// The item right.
        /// </value>
        TItemRight ItemRight { get; set; }

        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        TOperator Operator { get; set; }
    }
}