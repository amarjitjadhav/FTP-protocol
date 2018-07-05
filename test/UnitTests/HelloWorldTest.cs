using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class HelloWorldTest
    {
        [TestMethod]
        public void HelloWorld_GetHelloWorld_IsHelloWorld()
        {
            HelloWorld hello = new HelloWorld();
            Assert.AreEqual(hello.Hello(), "Hello World!");
            return;
        }
    }
}
