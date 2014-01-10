using System;
using System.Data.Linq.Mapping;
using System.Reflection;
using OpenMagic;
using OpenMagic.Reflection;

namespace SqlMagic
{
    public class ColumnMetaData : IColumnMetaData
    {
        public ColumnMetaData(ITableMetaData table, PropertyInfo property)
        {
            this.Table = table;
            this.Property = property;
            this.ColumnName = GetColumnName(property);
            this.IsIdColumn = GetIsIdColumn(table, property);

            if (!this.IsIdColumn)
            {
                this.IsNullable = this.GetIsNullable(property);
            }
        }

        public string ColumnName { get; set; }
        public bool IsIdColumn { get; set; }
        public bool IsNullable { get; set; }
        public PropertyInfo Property { get; set; }
        public ITableMetaData Table { get; set; }

        private ColumnAttribute GetColumnAttribute(PropertyInfo property)
        {
            return property.GetCustomAttribute<ColumnAttribute>();
        }

        private bool GetIsIdColumn(ITableMetaData table, PropertyInfo property)
        {
            var columnAttribute = this.GetColumnAttribute(property);

            if (columnAttribute == null)
            {
                return property.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase) || property.Name.Equals(table.RowType.Name + "Id", StringComparison.InvariantCultureIgnoreCase);
            }

            return columnAttribute.IsPrimaryKey;
        }

        private string GetColumnName(PropertyInfo property)
        {
            var columnAttribute = this.GetColumnAttribute(property);

            if (columnAttribute == null || string.IsNullOrWhiteSpace(columnAttribute.Name))
            {
                return property.Name;
            }

            return columnAttribute.Name;
        }

        private bool GetIsNullable(PropertyInfo property)
        {
            var columnAttribute = this.GetColumnAttribute(property);

            if (columnAttribute != null)
            {
                return columnAttribute.IsPrimaryKey;
            }

            // hack but it works!
            if (property.PropertyType.Name.Equals("Nullable`1"))
            {
                return true;
            }

            return property.PropertyType.Equals(typeof(string));
        }
    }
}
