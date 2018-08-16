using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// 
    /// </summary>
    public static class CSharpCodeUtil
    {
        /// <summary>
        /// The variable specification regex. Start with _ or alphabet, and _+alphabet+numeric
        /// </summary>
        static Regex VariableSpecificationRegex = new Regex("^[_a-zA-Z]([_a-zA-Z0-9]+)?$", RegexOptions.Compiled);

        /// <summary>
        /// Potentiallies the meet variable specification.
        /// </summary>
        /// <param name="charactor">The charactor.</param>
        /// <returns></returns>
        public static bool PotentiallyMeetVariableSpecification(char charactor)
        {
            return charactor == '_' || (charactor >= '0' && charactor <= '9') || (charactor >= 'a' && charactor <= 'z') || (charactor >= 'A' && charactor <= 'Z');
        }

        /// <summary>
        /// Meets the variable specification.
        /// </summary>
        /// <param name="potentialVariableName">Name of the potential variable.</param>
        /// <returns></returns>
        public static bool MeetVariableSpecification(string potentialVariableName)
        {
            try
            {
                potentialVariableName.CheckEmptyString(nameof(potentialVariableName));
                return VariableSpecificationRegex.IsMatch(potentialVariableName);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { potentialVariableName });
            }
        }

        /// <summary>
        /// Checks the variable specification.
        /// </summary>
        /// <param name="potentialVariableName">Name of the potential variable.</param>
        public static void CheckVariableSpecification(string potentialVariableName)
        {
            if (!MeetVariableSpecification(potentialVariableName))
            {
                throw ExceptionFactory.CreateInvalidObjectException(nameof(potentialVariableName), data: new { potentialVariableName });
            }
        }
    }
}
