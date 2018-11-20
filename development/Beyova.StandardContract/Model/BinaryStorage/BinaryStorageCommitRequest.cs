using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class BinaryStorageCommitRequest.
    /// </summary>
    public class BinaryStorageCommitRequest : BinaryStorageIdentifier
    {
        /// <summary>
        /// Gets or sets the commit option.
        /// </summary>
        /// <value>The commit option.</value>
        [JsonProperty(PropertyName = "commitOption")]
        public BinaryStorageCommitOption? CommitOption { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageCommitRequest"/> class.
        /// </summary>
        public BinaryStorageCommitRequest() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStorageCommitRequest"/> class.
        /// </summary>
        /// <param name="storageIdentifier">The storage identifier.</param>
        public BinaryStorageCommitRequest(BinaryStorageIdentifier storageIdentifier) : base(storageIdentifier)
        {
        }
    }
}