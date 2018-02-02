using System.Runtime.CompilerServices;
using Beyova.ExceptionSystem;

namespace Beyova.UnitTestKit
{
    /// <summary>
    /// Class TestInterruptException.
    /// </summary>
    public class TestInterruptException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestInterruptException" /> class.
        /// </summary>
        /// <param name="caseName">Name of the case.</param>
        /// <param name="stepSequence">The step sequence.</param>
        /// <param name="stepName">Name of the step.</param>
        /// <param name="data">The data.</param>
        /// <param name="assertName">Name of the assert.</param>
        internal TestInterruptException(string caseName, int stepSequence, string stepName, object data = null, [CallerMemberName] string assertName = null)
            : base(string.Format("Test case is interrupted by aasert [{0}] ,case [{1}] step-{2} [{3}]", assertName, caseName, stepSequence, stepName), new ExceptionCode
            {
                Major = ExceptionCode.MajorCode.Undefined
            }, data: data)
        {
        }
    }
}
