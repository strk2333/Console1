using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using System.Diagnostics;
using System.Threading;

namespace UnitTestProject1.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var engine = Python.CreateEngine();
            var scope = engine.CreateScope();
            var source = engine.CreateScriptSourceFromFile("boo.py");
            dynamic obj = source.Execute(scope);
            var boo = scope.GetVariable<Func<object>>("boo");
            Debug.WriteLine(boo());
        }

        public void AsyncOperation(int data)
        {
            int i = 0;
            while (i++ < data)
            {
                Thread.Sleep(1000);
                Debug.WriteLine(string.Format("Running for {0} seconds, in thread id: {1}.", i, Thread.CurrentThread.ManagedThreadId));
            }
        }
        public delegate void AsyncOperationDelegate(int data);

        [TestMethod]
        public void RunBeginInvoke()
        {
            AsyncOperationDelegate d1 = new AsyncOperationDelegate(AsyncOperation);
            d1.BeginInvoke(3, null, null);
            int i = 0;
            while (i++ < 3)
            {
                Thread.Sleep(1000);
                Debug.WriteLine(string.Format("[BeginInvoke]Running for {0} seconds, in thread id: {1}.", i, Thread.CurrentThread.ManagedThreadId));
            }
        }
    }
}
