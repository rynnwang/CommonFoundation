namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class NullObjectException.
    /// </summary>
    public class NullObjectException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NullObjectException" /> class.
        /// </summary>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="friendlyHint">The friendly hint.</param>
        /// <param name="scene">The scene.</param>
        public NullObjectException(string objectIdentity, FriendlyHint friendlyHint = null, ExceptionScene scene = null)
            : base(string.Format("[{0}] is null.", objectIdentity), new ExceptionCode { Major = ExceptionCode.MajorCode.NullOrInvalidValue, Minor = "NullObject" }, null, null, hint: friendlyHint, scene: scene)
        {
        }

        #endregion Constructor
    }
}