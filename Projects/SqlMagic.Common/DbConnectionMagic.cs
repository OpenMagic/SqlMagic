using System;
using System.Data;
using System.Linq;
using OpenMagic;

namespace SqlMagic.Common
{
    public abstract class DbConnectionMagic<TConnection> where TConnection : IDbConnection
    {
        public readonly TConnection Connection;

        public DbConnectionMagic(TConnection connection)
        {
            connection.MustNotBeNull("connection");

            if (connection.State != ConnectionState.Open)
            {
                throw new ArgumentException("Must be open connection.", "connection");
            }

            this.Connection = connection;
        }

        protected abstract string ColumnDefinition(ColumnMetaData c);

        public virtual void CreateTable(Type rowType)
        {
            var table = TableMetaData.Get(rowType);
            var columns = table.Columns(excludeId: false);
            var columnDefinitions = string.Join(", ", columns.Select(c => this.ColumnDefinition(c)));

            using (var command = this.Connection.CreateCommand())
            {
                command.CommandText = string.Format("CREATE TABLE {0} ({1});", this.Quote(table.TableName), columnDefinitions);

                var result = command.ExecuteNonQuery();

                if (result !=0)
                {
                    throw new Exception(string.Format("CREATE TABLE returned {0}. Expected value to be 0.", result));
                }
            }
        }
        
        public virtual void Insert(object row)
        {
            var metaData = TableMetaData.Get(row.GetType());
            var columns = metaData.Columns(excludeId: true);

            using (var command = this.Connection.CreateCommand())
            {
                command.CommandText = string.Format(
                    "INSERT INTO {0} ({1}) VALUES ({2});",
                    metaData.TableName,
                    string.Join(", ", columns.Select(c => this.Quote(c.ColumnName))),
                    string.Join(", ", columns.Select(c => this.ParameterName(c.ColumnName)))
                );

                foreach (var column in columns)
                {
                    var parameter = command.CreateParameter();

                    parameter.ParameterName = this.ParameterName(column.ColumnName);
                    parameter.Value = column.Property.GetValue(row, null);

                    command.Parameters.Add(parameter);
                }

                var result = command.ExecuteNonQuery();

                if (result != 1)
                {
                    throw new Exception(string.Format("INSERT returned {0}. Expected value to be 1.", result));
                }
            }
        }

        public virtual int GetLastId()
        {
            using (var command = this.Connection.CreateCommand())
            {
                command.CommandText = "SELECT last_insert_rowid();";

                return (int)command.ExecuteScalar();
            }
        }

        protected virtual string ParameterName(string name)
        {
            return string.Format("@{0}", name);
        }

        protected virtual string Quote(string name)
        {
            return string.Format("[{0}]", name);
        }
    }
}
