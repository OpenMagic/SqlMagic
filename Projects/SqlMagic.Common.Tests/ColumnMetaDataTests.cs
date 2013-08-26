using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenMagic.Reflection;
using SqlMagic.Common.Tests.TestHelpers;
using TestMagic;

namespace SqlMagic.Common.Tests
{
    public class ColumnMetaDataTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ShouldThrowArgumentNullExceptionWhenRowIsNull()
            {
                var mockProperty = (new Mock<PropertyInfo>()).Object;

                GWT.Given("constructor test")
                    .When(x => new ColumnMetaData(table: null, property: mockProperty))
                    .Then<ArgumentNullException>().ShouldBeThrown().ForParameter("table");
            }

            [TestMethod]
            public void ShouldThrowArgumentNullExceptionWhenPropertyIsNull()
            {
                var table = new TableMetaData<TestEntity>();

                GWT.Given("constructor test")
                    .When(x => new ColumnMetaData(table: table, property: null))
                    .Then<ArgumentNullException>().ShouldBeThrown().ForParameter("property");
            }
        }

        [TestClass]
        public class ColumnName
        {
            [TestMethod]
            public void ShouldBeColumnAttributeName()
            {
                // Given
                var table = new TableMetaData<TestEntity>();
                var property = Type<EntityWithOverrideColumnName>.Property(x => x.OverrideColumnName);

                // When
                var column = new ColumnMetaData(table, property);

                // Then
                column.ColumnName.Should().Be("Overridden");
            }

            [TestMethod]
            public void ShouldBePropertyNameWhenPropertyDoesNotHaveColumnAttribute()
            {
                // Given
                var table = new TableMetaData<TestEntity>();
                var property = Type<TestEntity>.Property(x => x.NullableString);

                // When
                var column = new ColumnMetaData(table, property);

                // Then
                column.ColumnName.Should().Be("NullableString");
            }

            [TestMethod]
            public void ShouldBePropertyNameWhenPropertyHasColumnAttributeButNameIsNotSpecified()
            {
                // Given
                var table = new TableMetaData<EntityWithColumnAttributeButNameIsNotSpecified>();
                var property = Type<EntityWithColumnAttributeButNameIsNotSpecified>.Property(x => x.Id);

                // When
                var column = new ColumnMetaData(table, property);

                // Then
                column.ColumnName.Should().Be("Id");
            }

            private class EntityWithColumnAttributeButNameIsNotSpecified
            {
                [Column(IsDbGenerated=true)]
                public int Id { get; set; }
            }

            private class EntityWithOverrideColumnName
            {
                [Column(Name = "Overridden")]
                public int OverrideColumnName { get; set; }
            }
        }

        [TestClass]
        public class IsIdColumn
        {
            [TestMethod]
            public void ShouldBeTrueWhenPropertyNameIsId()
            {
                // Given
                var table = new TableMetaData<TestEntity>();
                var property = Type<TestEntity>.Property(x => x.Id);

                // When
                var column = new ColumnMetaData(table, property);

                // Then
                column.IsIdColumn.Should().BeTrue();
            }

            [TestMethod]
            public void ShouldBeTrueWhenColumnAttributeIsPrimaryKeyIsTrue()
            {
                // Given
                var table = new TableMetaData<EntityWithIdIdentifiedByColumnAttribute>();
                var property = table.RowType.GetProperties().Single();

                // When
                var column = new ColumnMetaData(table, property);

                // Then
                column.IsIdColumn.Should().BeTrue();
            }

            [TestMethod]
            public void ShouldBeTrueWhenPropertyNameIsEntityNamePlusId()
            {
                // Given
                var table = new TableMetaData<EntityWithTableNamePlusIdAsIdProperty>();
                var property = table.RowType.GetProperties().Single();

                // When
                var column = new ColumnMetaData(table, property);

                // Then
                column.IsIdColumn.Should().BeTrue();
            }

            [TestMethod]
            public void ShouldBeFalseForAllOtherProperties()
            {
                // Given
                var table = new TableMetaData<TestEntity>();
                var property = Type<TestEntity>.Property(x => x.NullableString);

                // When
                var column = new ColumnMetaData(table, property);

                // Then
                column.IsIdColumn.Should().BeFalse();
            }

            private class EntityWithIdIdentifiedByColumnAttribute
            {
                [Column(IsPrimaryKey = true)]
                public int ThisIsTheIdProperty { get; set; }
            }

            private class EntityWithTableNamePlusIdAsIdProperty
            {
                public int EntityWithTableNamePlusIdAsIdPropertyId { get; set; }
            }
        }

        [TestClass]
        public class IsNullable
        {
            [TestMethod]
            public void ShouldBeFalseWhenPropertyIsIdColumn()
            {
                this.GetIsNullable(Type<TestEntity>.Property(x => x.Id)).Should().BeFalse();
            }

            [TestMethod]
            public void ShouldBeFalseWhenPropertyTypeIsInt()
            {
                this.GetIsNullable(Type<TestEntity>.Property(x => x.RequiredInt)).Should().BeFalse();
            }

            [TestMethod]
            public void ShouldBeTrueWhenPropertyTypeIsNullableInt()
            {
                this.GetIsNullable(Type<TestEntity>.Property(x => x.NullableInt)).Should().BeTrue();
            }

            [TestMethod]
            public void ShouldBeFalseWhenColumnAttributeCanBeNullIsFalse()
            {
                this.GetIsNullable(Type<TestEntity>.Property(x => x.RequiredString)).Should().BeFalse();
            }

            [TestMethod]
            public void ShouldBeTrueWhenPropertyTypeIsString()
            {
                this.GetIsNullable(Type<TestEntity>.Property(x => x.NullableString)).Should().BeTrue();
            }

            private bool GetIsNullable(PropertyInfo property)
            {
                var table = new TableMetaData<TestEntity>();
                var column = new ColumnMetaData(table, property);

                return column.IsNullable;
            }
        }

        [TestClass]
        public class Property
        {
            [TestMethod]
            public void ShouldBePropertyPassedToConstructor()
            {
                // Given
                var table = new TableMetaData<TestEntity>();
                var property = Type<TestEntity>.Property(x => x.Id);

                // When
                var column = new ColumnMetaData(table, property);

                // Then
                column.Property.ShouldBeEquivalentTo(property);
            }
        }

        [TestClass]
        public class Table
        {
            [TestMethod]
            public void ShouldBeTablePassedToConstructor()
            {
                // Given
                var table = new TableMetaData<TestEntity>();
                var property = Type<TestEntity>.Property(x => x.Id);

                // When
                var column = new ColumnMetaData(table, property);

                // Then
                column.Table.ShouldBeEquivalentTo(table);
            }
        }
    }
}
