using System;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Mezm.Owin.Razor.Tests
{
    public static class ExceptionUtil
    {
        public static void UnwrapTaskException(Func<Task> action)
        {
            try
            {
                action().Wait();
            }
            catch (AggregateException aex)
            {
                aex.Handle(HandleTaskException);
            }

            Assert.Fail();
        }

        private static bool HandleTaskException(Exception ex)
        {
            throw ex;
        }
    }
}