using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NUnit.Framework;
using System.Data.SqlServerCe;

namespace TestProject2
{
    [TestFixture]
    public class UnitTest1
    {
        private SqlCeConnection _c;
        private SqlCeTransaction _t;
        private const string STR_TestDatabasesdf = "TestDatabase2.sdf";
        private const string STR_Testtxt = "test2.txt";


        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _c = new SqlCeConnection(String.Format("Data Source={0}", STR_TestDatabasesdf));

            _c.Open();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _c.Close();
        }

        [SetUp]
        public void SetUp()
        {
            _t = _c.BeginTransaction();
        }


        [TearDown]
        public void TearDown()
        {
            _t.Rollback();
        }


        public void Execute(string commandText)
        {
            var cmd1 = _c.CreateCommand();

            cmd1.CommandText = commandText;
            cmd1.CommandType = System.Data.CommandType.Text;
            cmd1.ExecuteNonQuery();
        }
        public SqlCeDataReader GetReader(string commandText)
        {
            var cmd2 = _c.CreateCommand();

            cmd2.CommandText = commandText;
            cmd2.CommandType = System.Data.CommandType.Text;

            return cmd2.ExecuteReader();
        }
        [Test]
        public void _2_Test_if_database_exists()
        {
            Execute(@"create table test(a int null)");
                        
            Execute(@"insert into test values(1)");

            SqlCeDataReader reader = GetReader("select * from test");

            bool read = reader.Read();

            Assert.IsTrue(read);

            do
            {

                int fieldCount = reader.FieldCount;

                Assert.AreEqual(fieldCount, 1);
                
                for (int i = 0; i < fieldCount; i++)
                {
                    Assert.AreEqual("a", reader.GetName(i));
                    Assert.AreEqual(1, reader.GetValue(i));
                }

            }
            while (reader.Read());


        }
        [Test]
        public void _3_Test_if_file_exists()
        {
            Assert.IsTrue(File.Exists(STR_Testtxt));
        }
    }
}
