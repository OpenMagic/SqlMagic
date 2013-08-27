using System;
using System.Data;
using System.Linq;
using OpenMagic;

namespace SqlMagic.Tests.TestHelpers.Fakes
{
    public class FakeDbLanguage : DbLanguage<FakeDbColumnLanguage>
    {
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
