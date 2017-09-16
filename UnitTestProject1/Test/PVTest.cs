using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Scripting.Hosting;
using System.Threading;
using System.Diagnostics;

namespace UnitTestProject1.Test
{
    [TestClass]
    public class PVTest
    {
        static int i = 0;
        object o = i;
        //monitor
        Mutex mutex = new Mutex();
        Semaphore sema;

        [TestMethod]
        public void TestPV()
        {
            for (int i = 0; i < 3; i++)
            {
                ThreadProcess(i + 1);
            }
            //Task.WaitAll(tasks.ToArray());
        }

        public void ThreadProcess(int id)
        {
            new Thread(delegate ()
            {
                while (true)
                {
                    Monitor.Enter(o);
                    if (i < 30)
                    {
                        i++;
                        Debug.WriteLine("Thread id:" + id + " " + i);
                    }
                    else
                    {
                        break;
                    }
                    Monitor.Exit(o);
                }
            }).Start();
        }
    }
}
