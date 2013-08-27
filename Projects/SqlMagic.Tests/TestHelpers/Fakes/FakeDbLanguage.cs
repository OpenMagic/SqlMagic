using System;
using System.Data;
using System.Linq;
using OpenMagic;

namespace SqlMagic.Tests.TestHelpers.Fakes
{
    public class FakeDbLanguage : DbLanguage
    {
        public override string CreateColumnDefinition(IColumnMetaData column)
        {
            column.MustNotBeNull("column");

            var propertyType = column.Property.PropertyType.FullName
                .Replace("System.", "")
                .Replace(", mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "");

            if (column.IsIdColumn)
            {
                return string.Format("{0} {1} PRIMARY KEY", this.Quote(column.ColumnName), propertyType);
            }

            return string.Format("{0} {1} {2}", this.Quote(column.ColumnName), propertyType, this.GetNullDefinition(column)).Trim();
        }

        private string GetNullDefinition(IColumnMetaData column)
        {
            if (column.IsNullable)
            {
                return null;
            }

            return "NOT NULL";
        }

        public override string LastIdCommandText(ITableMetaData table)
        {
            // Deliberately unusual to more clearly show when Database may have created a parameter name itself.
            return "SELECT #GetLastId;";
        }

        public override string ParameterName(string value)
        {
            // Deliberately unusual to more clearly show when Database may have created a parameter name itself.
            return "#" + value;
        }

        public override string Quote(string value)
        {
            // Deliberately unusual to more clearly show when Database may have quoted a value itself.
            return string.Format("*{0}*", value);
        }
    }
}
