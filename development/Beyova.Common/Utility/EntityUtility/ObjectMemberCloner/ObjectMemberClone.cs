using System;

namespace Beyova
{
    /// <summary>
    /// Class ObjectMemberClone
    /// </summary>
    public class ObjectMemberClone
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeepEquality"/> class.
        /// </summary>
        protected ObjectMemberClone()
        {
        }

        /// <summary>
        /// Members the shadow clone.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object1">The object1.</param>
        /// <param name="object2">The object2.</param>
        public static void MemberShadowClone<T>(T object1, T object2)
        {
            var cloner = CreateMemberShadowClone<T>();
            cloner.ShadowClone(object1, object2);
        }

        /// <summary>
        /// Creates the member shadow clone.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static IObjectMemberClone<T> CreateMemberShadowClone<T>()
        {
            return ObjectMemberCloneGenerator.CreateMemberClone<T>();
        }
    }
}