using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using TracerLib.Tracer;

namespace TracerTest
{
    [TestClass]
    public class UnitTest
    {
        private ITracer tracer;

        [TestInitialize]
        public void Init()
        {
            tracer = new Tracer();
        }

        [TestMethod]
        public void MethodNameCheck()
        {
            TracerTest();

            string expectedMethodName = "TracerTest";
            string receivedMethodName = tracer.GetTraceResult().GetThreadTraces()[Thread.CurrentThread.ManagedThreadId].MethodInfo[0].MethodName;

            Assert.AreEqual(expectedMethodName, receivedMethodName);
        }

        [TestMethod]
        public void MethodClassCheck()
        {
            TracerTest();

            string expectedClassName = "UnitTest";
            string receivedClassName = tracer.GetTraceResult().GetThreadTraces()[Thread.CurrentThread.ManagedThreadId].MethodInfo[0].ClassName;

            Assert.AreEqual(expectedClassName, receivedClassName);
        }

        [TestMethod]
        public void ThreadCountCheck()
        {
            List<Thread> threads = new List<Thread>();

            int expectedThreadCount = 2;
            for (int i = 0; i < expectedThreadCount; i++)
            {
                threads.Add(new Thread(TracerTest));
                threads[i].Start();
                threads[i].Join();
            }

            int receivedThreadCount = tracer.GetTraceResult().GetThreadTraces().Count;
            Assert.AreEqual(expectedThreadCount, receivedThreadCount);
        }

        [TestMethod]
        public void MethodCountCheck()
        {
            int expectedMethodCount = 3;
            for (int i = 0; i < expectedMethodCount; i++)
            {
                TracerTest();
            }

            int receivedMethodCount = tracer.GetTraceResult().GetThreadTraces()[Thread.CurrentThread.ManagedThreadId].MethodInfo.Count;
            Assert.AreEqual(expectedMethodCount, receivedMethodCount);
        }

        [TestMethod]
        public void MethodTimeCheck()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            TracerTest();
            stopwatch.Stop();

            long expectedTime = stopwatch.ElapsedMilliseconds/10;
            long receivedTime = (long)tracer.GetTraceResult().GetThreadTraces()[Thread.CurrentThread.ManagedThreadId].MethodInfo[0].Time/10;

            Assert.AreEqual(expectedTime, receivedTime);
        }

        void TracerTest()
        {
            tracer.StartTrace();
            Thread.Sleep(50);
            tracer.StopTrace();
        }
    }
}
