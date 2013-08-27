using System;
using System.Data.SQLite;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlMagic.Tests.TestHelpers;

namespace SqlMagic.SQLite.IntegrationTests
{
    public class DatabaseTests : IDisposable
    {
        protected Database<SQLiteConnection, SQLiteLanguage> Database;
        protected SQLiteConnection Connection;
        protected SQLiteLanguage Language;

        public DatabaseTests()
        {
            this.Connection = new SQLiteConnection(SQLiteConnectionStrings.Memory());
            this.Connection.Open();

            this.Language = new SQLiteLanguage();
            this.Database = new Database<SQLiteConnection, SQLiteLanguage>(this.Connection, this.Language);
        }

        public void Dispose()
        {
            // overly anal!
            this.Connection.Close();
            this.Connection.Dispose();

            this.Database = null;
            this.Connection = null;
            this.Language = null;
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
