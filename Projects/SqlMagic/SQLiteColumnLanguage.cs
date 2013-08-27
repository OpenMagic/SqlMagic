namespace SqlMagic
{
    public class SQLiteColumnLanguage : DbColumnLanguage
    {
        public SQLiteColumnLanguage(IDbLanguage dbLanguage) : base(dbLanguage)
        {
        }

        private string ColumnDefinition(IColumnMetaData column, string columnType)
        {
            return string.Format("{0} {1} {2}", this.DbLanguage.Quote(column.ColumnName), columnType, this.GetNullDefinition(column)).Trim();
        }

        public override string Int32Column(IColumnMetaData column)
        {
            var columnDefinition = this.ColumnDefinition(column, "INTEGER");

            if (column.IsIdColumn)
            {
                columnDefinition += " PRIMARY KEY";
            }

            return columnDefinition;
        }

        public override string StringColumn(IColumnMetaData column)
        {
            return this.ColumnDefinition(column, "TEXT");
        }
    }
}
