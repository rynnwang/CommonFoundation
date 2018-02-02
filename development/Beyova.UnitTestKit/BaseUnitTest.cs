using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.UnitTestKit
{
    /// <summary>
    /// Class BaseUnitTest.
    /// </summary>
    public abstract class BaseUnitTest
    {
        /// <summary>
        /// Sets the user information.
        /// </summary>
        /// <param name="credential">The credential.</param>
        protected void SetContextUserInfo(ICredential credential)
        {
            ContextHelper.ApiContext.CurrentCredential = credential;
        }

        /// <summary>
        /// Sets the context user information.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        protected void SetContextUserInfo(Guid? userKey)
        {
            ContextHelper.ApiContext.CurrentCredential = new BaseCredential { Key = userKey };
        }

        /// <summary>
        /// Should not null or empty.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="stringName">Name of the string.</param>
        protected void ShouldNotNullOrEmpty(string stringObject, string stringName)
        {
            stringObject.CheckEmptyString(stringName);
        }

        /// <summary>
        /// Should has item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        protected void ShouldHasItem<T>(IEnumerable<T> items)
        {
            if (!items.HasItem())
            {
                throw ExceptionFactory.CreateInvalidObjectException(nameof(items));
            }
        }
    }
}
