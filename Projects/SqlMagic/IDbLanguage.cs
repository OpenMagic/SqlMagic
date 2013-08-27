using System.Data;
namespace SqlMagic
{
    public interface IDbLanguage
    {
        string CreateColumnDefinition(IColumnMetaData column);

        string CreateTableCommandText(ITableMetaData table);

        string InsertCommandText(ITableMetaData table);

        string LastIdCommandText(ITableMetaData table);

        string ParameterName(string value);

        IDbCommand PrepareCreateTableCommand(ITableMetaData table, IDbCommand command);

        IDbCommand PrepareInsertCommand<T>(T row, ITableMetaData table, IDbCommand command);

        IDbCommand PrepareLastIdCommand(ITableMetaData table, IDbCommand command);

        string Quote(string value);
    }
}
