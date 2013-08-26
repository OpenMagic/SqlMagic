using System;
using System.Data;
using OpenMagic;

namespace SqlMagic.Common
{
    public class Database<TConnection, TLanguage> : IDatabase
        where TConnection : IDbConnection
        where TLanguage : IDbLanguage
    {
        private readonly TLanguage Language;

        public readonly TConnection Connection;

        public Database(TConnection connection, TLanguage language)
        {
            connection.MustNotBeNull("connection");
            language.MustNotBeNull("language");

            if (connection.State != ConnectionState.Open)
            {
                throw new ArgumentException(string.Format("Connection must be open. It is {0}.", connection.State.ToString().ToLower()), "connection");
            }

            this.Connection = connection;
            this.Language = language;
        }

        public void CreateTable<TRow>()
        {
            this.CreateTable(typeof(TRow));
        }

        public void CreateTable(Type rowType)
        {
            this.CreateTable(TableMetaData.Get(rowType));
        }

        public void CreateTable(TableMetaData table)
        {
            table.MustNotBeNull("table");

            using (var command = this.Connection.CreateCommand())
            {
                var result = this.Language.PrepareCreateTableCommand(table, command).ExecuteNonQuery();

                if (result != 0)
                {
                    throw new Exception(string.Format("CREATE TABLE returned {0}. Expected value to be 0.", result));
                }
            }
        }

        public void Insert<TRow>(TRow row)
        {
            this.Insert(row.MustNotBeNull("row"), TableMetaData.Get(row.GetType()));
        }

        public void Insert<TRow>(TRow row, TableMetaData table)
        {
            row.MustNotBeNull("row");
            table.MustNotBeNull("table");

            using (var command = this.Connection.CreateCommand())
            {
                this.Language.PrepareInsertCommand(row, table, command);

                var result = command.ExecuteNonQuery();

                if (result != 1)
                {
                    throw new Exception(string.Format("INSERT returned {0}. Expected value to be 1.", result));
                }
            }
        }

        public int GetLastId<TRow>()
        {
            return this.GetLastId(typeof(TRow));
        }

        public int GetLastId(Type rowType)
        {
            return this.GetLastId(TableMetaData.Get(rowType));
        }
        
        public int GetLastId(TableMetaData table)
        {
            table.MustNotBeNull("table");

            using (var command = this.Connection.CreateCommand())
            {
                return (int)this.Language.PrepareLastIdCommand(table, command).ExecuteScalar();
            }
        }
    }
}
