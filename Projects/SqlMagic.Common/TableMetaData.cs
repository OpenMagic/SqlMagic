using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using OpenMagic;
using OpenMagic.Reflection;

namespace SqlMagic.Common
{
    public class TableMetaData
    {
        private static readonly Dictionary<Type, TableMetaData> Rows = new Dictionary<Type, TableMetaData>();

        private readonly IColumnMetaData[] AllColumns;

        public readonly Type RowType;
        public readonly string TableName;

        public TableMetaData(Type rowType)
        {
            rowType.MustNotBeNull("rowType");

            this.RowType = rowType;
            this.TableName = GetTableName(rowType);
            this.AllColumns = (
                from c in rowType.GetProperties()
                select new ColumnMetaData(this, c)
            ).ToArray();
        }

        public static TableMetaData Get(Type rowType)
        {
            rowType.MustNotBeNull("rowType");

            TableMetaData metaData = null;

            if (!Rows.TryGetValue(rowType, out metaData))
            {
                metaData = new TableMetaData(rowType);
                Rows.Add(rowType, metaData);
            }

            return metaData;
        }

        public IEnumerable<IColumnMetaData> Columns(bool excludeId = false)
        {
            if (excludeId)
            {
                return from c in this.AllColumns
                       where !c.IsIdColumn
                       select c;
            }
            else
            {
                return this.AllColumns;
            }
        }

        private string GetTableName(Type rowType)
        {
            var tableAttribute = rowType.GetCustomAttributes(typeof(TableAttribute), true).Cast<TableAttribute>().SingleOrDefault();

            if (tableAttribute == null)
            {
                // todo: smarter pluralization. in the mean time use TableAttribute if this is not sufficient.
                return rowType.Name + "s";
            }

            return tableAttribute.Name;
        }

        public IColumnMetaData IdColumn()
        {
            return this.AllColumns.Single(c => c.IsIdColumn);
        }
    }
}
