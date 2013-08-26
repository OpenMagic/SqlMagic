using System.Data;
namespace SqlMagic.Common
{
    public interface IDbLanguage
    {
        string CreateDefinition(IColumnMetaData column);
        string ParameterName(string value);
        IDbCommand PrepareCreateTableCommand(TableMetaData table, IDbCommand command);
        IDbCommand PrepareInsertCommand<T>(T row, TableMetaData table, IDbCommand command);
        IDbCommand PrepareLastIdCommand(TableMetaData table, IDbCommand command);
        string Quote(string value);
    }
}
