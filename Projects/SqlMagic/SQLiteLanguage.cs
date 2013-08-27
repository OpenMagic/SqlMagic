namespace SqlMagic
{
    public class SQLiteLanguage : DbLanguage<SQLiteColumnLanguage>
    {
        public override string LastIdCommandText(ITableMetaData table)
        {
            return "SELECT last_insert_rowid();";
        }
    }
}
