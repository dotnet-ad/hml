using System.Reflection;
using NUnit.Framework;

namespace Hml.Tests
{
    public static class Extensions
    {
        public static MethodInfo GetPrivateMethod(this object o, string methodName)
        {
            var method = o.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null)
                Assert.Fail(string.Format("{0} method not found", methodName));

            return method;
        }

        public static TReturn InvokePrivateMethod<TReturn>(this object o, string methodName, params object[] args)
        {
            var method = o.GetPrivateMethod(methodName);
            return (TReturn)method.Invoke(o, args);
        }

        public static void InvokePrivateMethod(this object o, string methodName, params object[] args)
        {
            var method = o.GetPrivateMethod(methodName);
            method.Invoke(o, args);
        }
    }
}
