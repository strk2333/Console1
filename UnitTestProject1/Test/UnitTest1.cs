using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using System.Diagnostics;

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
    }
}
