using System;
using System.Data;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestMagic;
using SqlMagic.Tests.TestHelpers;
using SqlMagic.Tests.TestHelpers.Fakes;

namespace SqlMagic.Tests
{
    public class DatabaseTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ShouldThrowArgumentNullExceptionWhenConnectionIsNull()
            {
                var language = new FakeDbLanguage();

                GWT.Given("testing constructor")
                    .When(x => new Database<IDbConnection, IDbLanguage>(connection: null, language: language))
                    .Then<ArgumentNullException>().ShouldBeThrown().ForParameter("connection");
            }

            [TestMethod]
            public void ShouldThrowArgumentNullExceptionWhenLanguageIsNull()
            {
                var connection = new FakeDbConnection();

                GWT.Given("testing constructor")
                    .When(x => new Database<IDbConnection, IDbLanguage>(connection: connection, language: null))
                    .Then<ArgumentNullException>().ShouldBeThrown().ForParameter("language");
            }

            [TestMethod]
            public void ShouldThrowArgumentExceptionWhenConnectionIsNotOpen()
            {
                // Given
                var connection = new FakeDbConnection(openConnection: false);
                var language = new FakeDbLanguage();

                // When
                Action action = () => new Database<IDbConnection, IDbLanguage>(connection, language);

                // Then
                action.ShouldThrow<ArgumentException>().WithMessage("Connection must be open. It is closed.\r\nParameter name: connection");
            }
        }

        [TestClass]
        public class CreateTable
        {
            [TestMethod]
            public void ShouldThrowArgumentNullExceptionWhenTableIsNull()
            {
                var connection = new FakeDbConnection(openConnection: true);
                var language = new FakeDbLanguage();
                TableMetaData nullTable = null;

                GWT.Given(new Database<IDbConnection, IDbLanguage>(connection, language))
                    .When(d => d.CreateTable(nullTable))
                    .Then<ArgumentNullException>().ShouldBeThrown().ForParameter("table");
            }

            [TestMethod]
            public void ShouldCreateTable()
            {
                // Given
                var connection = new FakeDbConnection(openConnection: true);
                var language = new FakeDbLanguage();
                var database = new Database<FakeDbConnection, FakeDbLanguage>(connection, language);
                var table = new TableMetaData<TestEntity>();

                // When
                database.CreateTable(table);

                // Then
                var command = connection.Commands.Single();

                command.CommandText.Should().Be(FormatCommandText(
                    @"CREATE TABLE *TestEntities* (
                        *Id* Int32 NOT NULL PRIMARY KEY, 
                        *RequiredInt* Int32 NOT NULL, 
                        *NullableInt* Nullable`1[[Int32]], 
                        *RequiredString* String NOT NULL, 
                        *NullableString* String
                    );"
                ));
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenCreateTableCommandDoesNotReturnZero()
            {
                // Given
                var connection = new FakeDbConnection(openConnection: true);
                var language = new FakeDbLanguage();
                var database = new Database<FakeDbConnection, FakeDbLanguage>(connection, language);
                var table = new TableMetaData<TestEntity>();

                connection.CreateCommandFactory = () => new FakeDbCommand() { ReturnValueForExecuteNonQuery = 1 };

                // When
                Action action = () => database.CreateTable(table);

                // Then
                action.ShouldThrow<Exception>().WithMessage("CREATE TABLE returned 1. Expected value to be 0.");
            }
        }

        [TestClass]
        public class Insert
        {
            [TestMethod]
            public void ShouldThrowArgumentNullExceptionWhenRowIsNull()
            {
                var connection = new FakeDbConnection(openConnection: true);
                var language = new FakeDbLanguage();
                var table = new TableMetaData<TestEntity>();
                TestEntity nullRow = null;

                GWT.Given(new Database<IDbConnection, IDbLanguage>(connection, language))
                    .When(d => d.Insert(row: nullRow))
                    .Then<ArgumentNullException>().ShouldBeThrown().ForParameter("row");
            }

            [TestMethod]
            public void ShouldThrowArgumentNullExceptionWhenTableIsNull()
            {
                var connection = new FakeDbConnection(openConnection: true);
                var language = new FakeDbLanguage();
                var row = new TestEntity();

                GWT.Given(new Database<IDbConnection, IDbLanguage>(connection, language))
                    .When(d => d.Insert(row: row, table: null))
                    .Then<ArgumentNullException>().ShouldBeThrown().ForParameter("table");
            }

            [TestMethod]
            public void ShouldInsertRow()
            {
                // Given
                var connection = new FakeDbConnection() { CreateCommandFactory = () => new FakeDbCommand() { ReturnValueForExecuteNonQuery = 1 } };
                var language = new FakeDbLanguage();
                var database = new Database<FakeDbConnection, FakeDbLanguage>(connection, language);
                var row = new TestEntity();

                // When
                database.Insert(row);

                // Then
                var command = connection.Commands.Single();

                command.CommandText.Should().Be(FormatCommandText(
                    @"INSERT INTO *TestEntities* (
                        *RequiredInt*, 
                        *NullableInt*, 
                        *RequiredString*, 
                        *NullableString*
                    ) 
                    VALUES (
                        #RequiredInt, 
                        #NullableInt, 
                        #RequiredString, 
                        #NullableString
                    );"
                ));

                var parameterNames = from p in command.Parameters.Cast<IDbDataParameter>()
                                     select p.ParameterName;

                parameterNames.ShouldBeEquivalentTo(new string[] { "#RequiredInt", "#NullableInt", "#RequiredString", "#NullableString" });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenInsertTableCommandDoesNotReturnOne()
            {
                // Given
                var connection = new FakeDbConnection() { CreateCommandFactory = () => new FakeDbCommand() { ReturnValueForExecuteNonQuery = 0 } };
                var language = new FakeDbLanguage();
                var database = new Database<FakeDbConnection, FakeDbLanguage>(connection, language);
                var row = new TestEntity();

                // When
                Action action = () => database.Insert(row);

                // Then
                action.ShouldThrow<Exception>().WithMessage("INSERT returned 0. Expected value to be 1.");
            }
        }

        [TestClass]
        public class GetLastId
        {
            [TestMethod]
            public void ShouldIdAfterPreviousInsert()
            {
                // Given
                var expectedLastId = 123;
                var connection = new FakeDbConnection() { CreateCommandFactory = () => new FakeDbCommand() { ReturnValueForExecuteScalar = expectedLastId } };
                var language = new FakeDbLanguage();
                var database = new Database<FakeDbConnection, FakeDbLanguage>(connection, language);

                // When
                var lastId = database.GetLastId<TestEntity>();

                // Then
                var command = connection.Commands.Single();

                lastId.Should().Be(expectedLastId);
                command.CommandText.Should().Be("SELECT #GetLastId;");
            }
        }

        private static string FormatCommandText(string commandText)
        {
            var formatted = commandText.Replace("\r\n", "");

            while (formatted.Contains("  "))
            {
                formatted = formatted.Replace("  ", " ");
            }

            formatted = formatted.Replace("( ", "(").Replace(" )", ")");

            return formatted;
        }
    }
}
