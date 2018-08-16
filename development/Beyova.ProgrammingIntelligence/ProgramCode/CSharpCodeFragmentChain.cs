//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Text;

//namespace Beyova.ProgrammingIntelligence
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    internal class CSharpCodeFragmentChain
//    {
//        /// <summary>
//        /// Gets or sets the items.
//        /// </summary>
//        /// <value>
//        /// The items.
//        /// </value>
//        public List<CSharpCodeFragment> Items { get; protected set; }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="CSharpCodeFragmentChain"/> class.
//        /// </summary>
//        /// <param name="fullName">The full name.</param>
//        public CSharpCodeFragmentChain(string fullName)
//        {
//            Items = InitializeByFullName(fullName);
//        }

//        /// <summary>
//        /// Initializes the full name of the by.
//        /// </summary>
//        /// <param name="fullName">The full name.</param>
//        /// <returns></returns>
//        protected List<CSharpCodeFragment> InitializeByFullName(string fullName)
//        {
//            List<CSharpCodeFragment> result = new List<CSharpCodeFragment>();

//            try
//            {
//                fullName.CheckEmptyString(nameof(fullName));

//                Items = new List<CSharpCodeFragment>();
//                LookUpFragment(Items, fullName, 0, 0);
//            }
//            catch (Exception ex)
//            {
//                throw ex.Handle(new { fullName });
//            }

//            return result;
//        }

//        protected int LookUpFragment(List<CSharpCodeFragment> chain, string originalName, int position, int level)
//        {
//            int startPosition = position;
//            // Keep simple in this phase. Ignore generic types in either types or methods.

//            //TODO: Generic Type

//            while (position < originalName.Length)
//            {
//                var currentChar = originalName[position];
//                //bool hasLeftBrace = false;
//                if (CSharpCodeUtil.PotentiallyMeetVariableSpecification(currentChar))
//                {
//                    position++;
//                }
//                else if (currentChar == '.')
//                {
//                    chain.Add(new CSharpCodeFragment
//                    {
//                        Name = originalName.Substring(startPosition, position - startPosition)
//                    });

//                    position++;
//                    // reset start position to new value.
//                    startPosition = position;
//                }
//                else if (currentChar == '<')
//                {
//                    List<CSharpCodeFragment> subChain = new List<CSharpCodeFragment>();
//                    position++;
//                    position = LookUpFragment(subChain, originalName, position, level + 1);

//                    throw new NotImplementedException("Generic Type in fragment is not supported yet.");
//                }
//                else if (currentChar == '>')
//                {
//                    if (level < 1)
//                    {
//                        throw ExceptionFactory.CreateInvalidObjectException(nameof(originalName), data: new { originalName, position }, reason: "Missing '<'");
//                    }

//                    chain.Add(new CSharpCodeFragment
//                    {
//                        Name = originalName.Substring(startPosition, position - startPosition)
//                    });
//                    position++;
//                    return position;
//                }
//                else if (currentChar == ',')
//                {
//                    // Jump out of loop, to add potential fragment and return new position.
//                    break;
//                }
//                else
//                {
//                    throw ExceptionFactory.CreateInvalidObjectException(nameof(originalName), data: new { originalName, position, currentChar });
//                }
//            }

//            if (startPosition < position)
//            {
//                chain.Add(new CSharpCodeFragment
//                {
//                    Name = originalName.Substring(startPosition)
//                });
//            }

//            if (position > originalName.Length && level > 0)
//            {
//                throw ExceptionFactory.CreateInvalidObjectException(nameof(originalName), data: new { originalName, position }, reason: "Missing '>'");
//            }

//            return position;
//        }

//        //public MethodInfo AsMethod()
//        //{

//        //}

//        //public Type AsType()
//        //{

//        //}

//        //public CSharpCodeFragmentChainRecognization TryRecognize()
//        //{

//        //}
//    }
//}
