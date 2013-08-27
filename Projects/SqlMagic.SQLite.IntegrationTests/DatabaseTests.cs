using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using SqlMagic.Tests.TestHelpers;

namespace SqlMagic.SQLite.IntegrationTests
{
    public class DatabaseTests : IDisposable
    {
        protected readonly Database<IDbConnection, IDbLanguage> Database;

        public DatabaseTests()
        {
        }

        public void  Dispose()
        {
 	        throw new NotImplementedException();
        }

        [TestClass]
        public class CreateTable : DatabaseTests
        {
            [TestMethod]
            public void ShouldAddTableToDatabase()
            {
                // Given
                var table = TableMetaData.GetTable<TestEntity>();

                // When
                this.Database.CreateTable(table);

                // Then
                Assert.Inconclusive("todo");
            }    
        }
    }
}
