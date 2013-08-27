namespace SqlMagic
{
    public interface IDbColumnLanguage
    {
        string Create(IColumnMetaData column);
        string GetNullDefinition(IColumnMetaData column);
        string Int32Column(IColumnMetaData column);
        string StringColumn(IColumnMetaData column);
    }
}
