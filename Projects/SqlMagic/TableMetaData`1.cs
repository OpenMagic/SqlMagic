namespace SqlMagic
{
    public class TableMetaData<TRow> : TableMetaData where TRow : class
    {
        public TableMetaData()
            : base(typeof(TRow))
        {
        }
    }
}
