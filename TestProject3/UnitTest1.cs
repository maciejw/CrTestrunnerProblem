using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Cfg.MappingSchema;
using NHibernate;

namespace TestProject3
{
    public class TestClassMap : ClassMapping<TestClass>
    {
        public TestClassMap()
        {
            Id(p => p.Id);
            Property(p => p.Property1);
        }
    }
    public class TestClass
    {
        public TestClass()
        {

        }
        public virtual int Id { get; set; }
        public virtual int Property1 { get; set; }
    }


    [TestFixture]
    public class UnitTest1
    {

        private ISession _s;
        private ITransaction _t;
        private ISessionFactory _sf;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {

            var mapper = new ModelMapper();

            mapper.AddMapping<TestClassMap>();

            HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            Configuration cfg = new Configuration();

            cfg.DataBaseIntegration(c =>
            {
                c.Dialect<NHibernate.Dialect.MsSqlCe40Dialect>();
                c.ConnectionString = @"Data Source=TestDatabase3.sdf";
                c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                c.SchemaAction = SchemaAutoAction.Create;
            });

            cfg.AddMapping(domainMapping);

            _sf = cfg.BuildSessionFactory();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _sf.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            _s = _sf.OpenSession();
            _t = _s.BeginTransaction();
        }


        [TearDown]
        public void TearDown()
        {
            _s.Close();
        }

        [Test]
        public void _4_db_with_nh()
        {
            var id = (int)_s.Save(new TestClass { Property1 = 5 });

            _s.Flush();

            _s.Clear();

            var test = _s.Get<TestClass>(id);

            Assert.IsNotNull(test);

            Assert.AreEqual(5, test.Property1);
        }
    }
}