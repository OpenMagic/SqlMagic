namespace SqlMagic.Tests.TestHelpers.Fakes
{
    public class FakeDbColumnLanguage : DbColumnLanguage
    {
        public FakeDbColumnLanguage(IDbLanguage dbLanguage)
            : base(dbLanguage)
        {
        }

        private string ColumnDefinition(IColumnMetaData column)
        {
            var propertyType = column.Property.PropertyType.FullName
                .Replace("System.", "")
                .Replace(", mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "");

            return string.Format("{0} {1} {2}", this.DbLanguage.Quote(column.ColumnName), propertyType, this.GetNullDefinition(column)).Trim();
        }

        public override string Int32Column(IColumnMetaData column)
        {
            var columnDefinition = this.ColumnDefinition(column);

            if (column.IsIdColumn)
            {
                columnDefinition += " PRIMARY KEY";
            }

            return columnDefinition;
        }

        public override string StringColumn(IColumnMetaData column)
        {
            return this.ColumnDefinition(column);
        }
    }
}
