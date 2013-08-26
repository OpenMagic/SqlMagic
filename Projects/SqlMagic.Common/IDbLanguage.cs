using System.Data;
namespace SqlMagic.Common
{
    public interface IDbLanguage
    {
        string CreateDefinition(IColumnMetaData column);
        string ParameterName(string value);
        IDbCommand PrepareCreateTableCommand(ITableMetaData table, IDbCommand command);
        IDbCommand PrepareInsertCommand<T>(T row, ITableMetaData table, IDbCommand command);
        IDbCommand PrepareLastIdCommand(ITableMetaData table, IDbCommand command);
        string Quote(string value);
    }
}
