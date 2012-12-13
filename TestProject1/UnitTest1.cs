using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.IO;


namespace TestProject1
{
    [TestFixture]
    public class UnitTest1
    {
        private const string STR_Testtxt = "test1.txt";

        [Test]
        public void _1_test()
        {
            Assert.IsTrue(File.Exists(STR_Testtxt));

        }
    }
}
