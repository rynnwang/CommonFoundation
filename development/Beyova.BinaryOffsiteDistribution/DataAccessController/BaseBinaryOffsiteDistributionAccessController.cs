namespace Beyova
{
    /// <summary>
    /// Class BinaryOffsiteDistributionAccessController.
    /// </summary>
    public abstract class BaseBinaryOffsiteDistributionAccessController<T> : BaseDataAccessController<T>
    {
        #region Constants

        /// <summary>
        /// The column_ host region
        /// </summary>
        protected const string column_HostRegion = "HostRegion";

        /// <summary>
        /// The column_ country
        /// </summary>
        protected const string column_Country = "Country";

        /// <summary>
        /// The column_ upload credential expired stamp
        /// </summary>
        protected const string column_UploadCredentialExpiredStamp = "UploadCredentialExpiredStamp";

        #endregion Constants

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBinaryOffsiteDistributionAccessController{T}" /> class.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        public BaseBinaryOffsiteDistributionAccessController(string sqlConnection)
            : base(sqlConnection)
        {
        }

        #endregion Constructor

    }
}