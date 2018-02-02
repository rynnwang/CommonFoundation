using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.UnitTestKit
{
    public class TestContainer : IDisposable
    {
        public ReadOnlyCollection<TestStep> Steps { get; protected set; }

        public ReadOnlyCollection<TestCase> Cases { get; protected set; }

        public TestContext Context { get; protected set; }

        public TestContainer(IList<TestCase> cases, IList<TestStep> steps,Dictionary<string, object> contextInitializeData=null)
        {
            this.Context = new TestContext();
            this.Cases = cases == null ? null : new ReadOnlyCollection<TestCase>(cases);
            this.Steps = steps == null ? null : new ReadOnlyCollection<TestStep>(steps);
        }

        public void Run(object startObject)
        {

        }

        protected void ValidateCaseSteps()
        {

        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
