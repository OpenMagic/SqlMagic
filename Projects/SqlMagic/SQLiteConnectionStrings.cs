namespace SqlMagic
{
    /// <summary>
    /// Helper class to construct connection strings for SQLite.
    /// </summary>
    public static class SQLiteConnectionStrings
    {
        public static string Memory()
        {
            return "Data Source=:memory:;Version=3;New=True;";
        }
    }
}
