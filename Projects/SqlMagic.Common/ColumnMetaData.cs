using System;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using OpenMagic;
using OpenMagic.Reflection;

namespace SqlMagic.Common
{
    public class ColumnMetaData
    {
        public readonly string ColumnName;
        public readonly bool IsIdColumn;
        public readonly bool IsNullable;
        public readonly PropertyInfo Property;
        public readonly TableMetaData Table;

        public ColumnMetaData(TableMetaData table, PropertyInfo property)
        {
            table.MustNotBeNull("table");
            property.MustNotBeNull("property");

            this.Table = table;
            this.Property = property;
            this.ColumnName = GetColumnName(property);
            this.IsIdColumn = GetIsIdColumn(table, property);

            if (!this.IsIdColumn)
            {
                this.IsNullable = this.GetIsNullable(property);
            }
        }

        private ColumnAttribute GetColumnAttribute(PropertyInfo property)
        {
            return property.GetCustomAttribute<ColumnAttribute>();
        }

        private bool GetIsIdColumn(TableMetaData table, PropertyInfo property)
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

            if (columnAttribute == null)
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
