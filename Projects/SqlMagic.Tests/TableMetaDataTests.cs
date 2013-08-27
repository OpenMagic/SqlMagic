using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMagic;
using FluentAssertions;
using SqlMagic.Tests.TestHelpers;

namespace SqlMagic.Tests
{
    public class TableMetaDataTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ShouldThrowArgumentNullExceptionWhenRowTypeIsNull()
            {
                GWT.Given("testing constructor")
                    .When(x => new TableMetaData(rowType: null))
                    .Then<ArgumentNullException>().ShouldBeThrown().ForParameter("rowType");
            }
        }

        [TestClass]
        public class GetTable
        {
            [TestMethod]
            public void ShouldThrowArgumentNullExceptionWhenRowTypeIsNull()
            {
                GWT.Given("testing static method")
                    .When(x => TableMetaData.GetTable(rowType: null))
                    .Then<ArgumentNullException>().ShouldBeThrown().ForParameter("rowType");
            }

            [TestMethod]
            public void ShouldReturnTableMetaDataForRowType()
            {
                // Given
                var rowType = typeof(TestEntity);

                // When
                var table = TableMetaData.GetTable(rowType);

                // Then
                table.Should().NotBeNull();
            }

            [TestMethod]
            public void ShouldUseACache()
            {
                // Given
                var rowType = typeof(TestEntity);

                // When
                var tableA = TableMetaData.GetTable(rowType);
                var tableB = TableMetaData.GetTable(rowType);

                // Then
                tableA.Should().BeSameAs(tableB);
            }
        }

        [TestClass]
        public class Columns
        {
            [TestMethod]
            public void ShouldReturnListOfColumnsMatchingThePropertiesOfRowTypePassedToConstructor()
            {
                // Given
                var table = new TableMetaData<TestEntity>();

                // When
                var columns = table.Columns;

                // Then
                var columnNames = from c in columns
                                  select c.ColumnName;

                columnNames.ShouldBeEquivalentTo(new string[] { "Id", "RequiredInt", "NullableInt", "RequiredString", "NullableString" });
            }
        }

        [TestClass]
        public class RowType
        {
            [TestMethod]
            public void ShouldReturnTheRowTypePassedToConstructor()
            {
                // Given
                var table = new TableMetaData<TestEntity>();

                // When
                var rowType = table.RowType;

                // Then
                rowType.Should().Be(typeof(TestEntity));
            }
        }

        [TestClass]
        public class TableName
        {
            [TestMethod]
            public void ShouldBeTableAttributeName()
            {
                // Given
                var table = new TableMetaData<TestEntity>();

                // When
                var tableName = table.TableName;

                // Then
                tableName.Should().Be("TestEntities");
            }

            [TestMethod]
            public void ShouldBePluralizedEntityName()
            {
                this.ShouldBePluralized<Exception>("Exceptions");
                this.ShouldBePluralized<Person>("People");
            }

            private void ShouldBePluralized<TEntity>(string expectedTableName) where TEntity : class
            {
                var table = new TableMetaData<TEntity>();
                var tableName = table.TableName;

                tableName.Should().Be(expectedTableName);
            }

            private class Person
            {
            }
        }

        [TestClass]
        public class GetColumns
        {
            [TestMethod]
            public void ShouldReturnAllColumnsWhenExcludeIdIsFalse()
            {
                // Given
                var table = new TableMetaData<TestEntity>();

                // When
                var columns = table.GetColumns(excludeId: false);

                // Then
                var columnNames = from c in columns
                                  select c.ColumnName;

                columnNames.ShouldBeEquivalentTo(new string[] { "Id", "RequiredInt", "NullableInt", "RequiredString", "NullableString" });
            }

            [TestMethod]
            public void ShouldReturnAllColumnsExceptIdWhenExcludeIdIsTrue()
            {
                // Given
                var table = new TableMetaData<TestEntity>();

                // When
                var columns = table.GetColumns(excludeId: true);

                // Then
                var columnNames = from c in columns
                                  select c.ColumnName;

                columnNames.ShouldBeEquivalentTo(new string[] { "RequiredInt", "NullableInt", "RequiredString", "NullableString" });
            }
        }

        [TestClass]
        public class GetIdColumn
        {
            [TestMethod]
            public void ShouldReturnIdColumn()
            {
                // Given
                var table = new TableMetaData<TestEntity>();

                // When
                var idColumn = table.GetIdColumn();

                // Then
                idColumn.ColumnName.Should().Be("Id");
            }
        }
    }
}
