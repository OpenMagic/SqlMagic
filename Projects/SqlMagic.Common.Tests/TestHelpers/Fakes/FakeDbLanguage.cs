using System;
using System.Data;
using System.Linq;
using OpenMagic;

namespace SqlMagic.Common.Tests.TestHelpers.Fakes
{
    public class FakeDbLanguage : IDbLanguage
    {
        public string CreateDefinition(ColumnMetaData column)
        {
            column.MustNotBeNull("column");

            var propertyType = column.Property.PropertyType.FullName
                .Replace("System.", "")
                .Replace(", mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "");

            if (column.IsIdColumn)
            {
                return string.Format("{0} {1} PRIMARY KEY", this.Quote(column.ColumnName), propertyType);
            }

            return string.Format("{0} {1} {2}", this.Quote(column.ColumnName), propertyType, this.GetNullDefinition(column)).Trim();
        }

        private string GetNullDefinition(ColumnMetaData column)
        {
            if (column.IsNullable)
            {
                return null;
            }

            return "NOT NULL";
        }

        public string ParameterName(string value)
        {
            // Deliberately unusual to more clearly show when Database may have created a parameter name itself.
            return "#" + value;
        }

        public IDbCommand PrepareCreateTableCommand(TableMetaData table, IDbCommand command)
        {
            table.MustNotBeNull("table");

            var columns = table.Columns(excludeId: false);
            var columnDefinitions = string.Join(", ", columns.Select(c => this.CreateDefinition(c)));

            command.CommandText = string.Format("CREATE TABLE {0} ({1});", this.Quote(table.TableName), columnDefinitions);

            return command;
        }

        public IDbCommand PrepareInsertCommand<T>(T row, TableMetaData table, IDbCommand command)
        {
            row.MustNotBeNull("row");
            table.MustNotBeNull("table");
            command.MustNotBeNull("command");

            var columns = table.Columns(excludeId: true);

            command.CommandText = string.Format(
                "INSERT INTO {0} ({1}) VALUES ({2});",
                this.Quote(table.TableName),
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

            return command;
        }

        public IDbCommand PrepareLastIdCommand(TableMetaData table, IDbCommand command)
        {
            table.MustNotBeNull("table");
            command.MustNotBeNull("command");

            command.CommandText = "SELECT #GetLastId;";

            return command;
        }

        public string Quote(string value)
        {
            // Deliberately unusual to more clearly show when Database may have quoted a value itself.
            return string.Format("*{0}*", value);
        }


    }
}
