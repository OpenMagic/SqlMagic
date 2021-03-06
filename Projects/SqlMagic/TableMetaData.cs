﻿using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using Inflector;
using OpenMagic;

namespace SqlMagic
{
    public class TableMetaData : ITableMetaData
    {
        private static readonly Dictionary<Type, ITableMetaData> Tables = new Dictionary<Type, ITableMetaData>();

        public TableMetaData(Type rowType)
        {
            this.RowType = rowType;
            this.TableName = GetTableName(rowType);
            this.Columns = new List<IColumnMetaData>(
                from c in rowType.GetProperties()
                select new ColumnMetaData(this, c)
            );
        }

        public static ITableMetaData GetTable<TTable>()
        {
            return GetTable(typeof(TTable));
        }

        public static ITableMetaData GetTable(Type rowType)
        {
            ITableMetaData metaData = null;

            if (!Tables.TryGetValue(rowType, out metaData))
            {
                metaData = new TableMetaData(rowType);
                Tables.Add(rowType, metaData);
            }

            return metaData;
        }

        public IList<IColumnMetaData> Columns { get; set; }

        public Type RowType { get; set; }

        public string TableName { get; set; }

        public IEnumerable<IColumnMetaData> GetColumns(bool excludeId = false)
        {
            if (excludeId)
            {
                return from c in this.Columns
                       where !c.IsIdColumn
                       select c;
            }
            else
            {
                return this.Columns;
            }
        }

        public IColumnMetaData GetIdColumn()
        {
            return this.Columns.Single(c => c.IsIdColumn);
        }

        private string GetTableName(Type rowType)
        {
            var tableAttribute = rowType.GetCustomAttributes(typeof(TableAttribute), true).Cast<TableAttribute>().SingleOrDefault();

            if (tableAttribute == null)
            {
                return rowType.Name.Pluralize();
            }

            return tableAttribute.Name;
        }
    }
}
