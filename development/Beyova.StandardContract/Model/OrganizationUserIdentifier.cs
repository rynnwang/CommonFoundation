using System;

namespace Beyova
{
    /// <summary>
    /// class OrganizationUserIdentifier
    /// </summary>
    public class OrganizationUserIdentifier : OrganizationUserIdentifier<string, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationUserIdentifier" /> class.
        /// </summary>
        public OrganizationUserIdentifier() : base() { }

        /// <summary>
        /// To the union identifier.
        /// </summary>
        /// <param name="seperator">The seperator.</param>
        /// <returns></returns>
        public override string ToUnionId(char seperator = '\\')
        {
            return ToUnionId(seperator, string.IsNullOrWhiteSpace);
        }
    }

    /// <summary>
    /// class OrganizationUserIdentifier
    /// </summary>
    /// <typeparam name="TOrganizationId">The type of the organization identifier.</typeparam>
    /// <typeparam name="TUserId">The type of the user identifier.</typeparam>
    public abstract class OrganizationUserIdentifier<TOrganizationId, TUserId>
    {
        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        public TOrganizationId OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public TUserId UserId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationUserIdentifier{TOrganizationId, TUserId}"/> class.
        /// </summary>
        public OrganizationUserIdentifier() { }

        /// <summary>
        /// To the union identifier.
        /// </summary>
        /// <param name="seperator">The seperator.</param>
        /// <returns></returns>
        public virtual string ToUnionId(char seperator = '\\')
        {
            return ToUnionId(seperator, x => { return x != null; });
        }

        /// <summary>
        /// To the union identifier.
        /// </summary>
        /// <param name="seperator">The seperator.</param>
        /// <param name="oganizationIdValidator">The oganization identifier validator.</param>
        /// <returns></returns>
        protected string ToUnionId(char seperator, Func<TOrganizationId, bool> oganizationIdValidator)
        {
            return oganizationIdValidator(OrganizationId) ? UserId?.ToString() : string.Format("{0}{1}{2}", OrganizationId, seperator, UserId);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ToUnionId();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var idObject = obj as OrganizationUserIdentifier<TOrganizationId, TUserId>;
            var organizationId = idObject == null ? default(TOrganizationId) : idObject.OrganizationId;
            var userId = idObject == null ? default(TUserId) : idObject.UserId;
            return OrganizationId.MeaningfulEquals(organizationId) && UserId.MeaningfulEquals(userId);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return OrganizationId?.GetHashCode() ?? 0 + UserId?.GetHashCode() ?? 0;
        }
    }
}