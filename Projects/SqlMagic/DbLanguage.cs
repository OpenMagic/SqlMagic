using System;
using System.Data;
using System.Linq;
using OpenMagic;

namespace SqlMagic
{
    /// <summary>
    /// Implements IDbLanguage. 
    /// 
    /// Where I'm reasonably confident most SQL providers use the same language them they have been implemented by this class. 
    /// Where I know each provider is unique then the method is abstract.
    /// </summary>
    public abstract class DbLanguage<TColumnLanguage> : IDbLanguage where TColumnLanguage : IDbColumnLanguage
    {
        private TColumnLanguage ColumnLanguage;

        public DbLanguage()
        {
            this.ColumnLanguage = (TColumnLanguage)Activator.CreateInstance(typeof(TColumnLanguage), this);
        }

        public virtual string CreateColumnDefinition(IColumnMetaData column)
        {
            return ColumnLanguage.Create(column);
        }

        public virtual string CreateTableCommandText(ITableMetaData table)
        {
            var columns = table.GetColumns(excludeId: false);
            var columnDefinitions = string.Join(", ", columns.Select(c => this.CreateColumnDefinition(c)));

            return string.Format("CREATE TABLE {0} ({1});", this.Quote(table.TableName), columnDefinitions);
        }

        public virtual string InsertCommandText(ITableMetaData table)
        {
            var columns = table.GetColumns(excludeId: true);

            return string.Format(
                "INSERT INTO {0} ({1}) VALUES ({2});",
                this.Quote(table.TableName),
                string.Join(", ", columns.Select(c => this.Quote(c.ColumnName))),
                string.Join(", ", columns.Select(c => this.ParameterName(c.ColumnName)))
            );
        }

        public abstract string LastIdCommandText(ITableMetaData table);

        public virtual string ParameterName(string value)
        {
            return "@" + value;
        }

        public virtual IDbCommand PrepareCreateTableCommand(ITableMetaData table, IDbCommand command)
        {
            command.CommandText = this.CreateTableCommandText(table);

            return command;
        }

        public virtual IDbCommand PrepareInsertCommand<T>(T row, ITableMetaData table, IDbCommand command)
        {
            command.CommandText = this.InsertCommandText(table);

            foreach (var column in table.GetColumns(excludeId: true))
            {
                var parameter = command.CreateParameter();

                parameter.ParameterName = this.ParameterName(column.ColumnName);
                parameter.Value = column.Property.GetValue(row, null);

                command.Parameters.Add(parameter);
            }

            return command;
        }

        public virtual IDbCommand PrepareLastIdCommand(ITableMetaData table, IDbCommand command)
        {
            command.CommandText = this.LastIdCommandText(table);

            return command;
        }

        public virtual string Quote(string value)
        {
            return string.Format("[{0}]", value);
        }
    }
}
