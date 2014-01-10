using System;
using System.Collections.Generic;
using NullGuard;
using OpenMagic;

namespace SqlMagic
{
    /// <summary>
    /// Implements <see cref="IDbColumnLanguage"/>.
    /// </summary>
    public abstract class DbColumnLanguage : IDbColumnLanguage
    {
        private static Dictionary<Type, Func<DbColumnLanguage, IColumnMetaData, string>> ColumnTypes;

        protected IDbLanguage DbLanguage;

        static DbColumnLanguage()
        {
            ColumnTypes = new Dictionary<Type, Func<DbColumnLanguage, IColumnMetaData, string>>();

            ColumnTypes.Add(typeof(Int32), (columnLanguage, columnMetaData) => columnLanguage.Int32Column(columnMetaData));
            ColumnTypes.Add(typeof(Int32?), (columnLanguage, columnMetaData) => columnLanguage.Int32Column(columnMetaData));
            ColumnTypes.Add(typeof(string), (columnLanguage, columnMetaData) => columnLanguage.StringColumn(columnMetaData));
        }

        public DbColumnLanguage(IDbLanguage dbLanguage)
        {
            this.DbLanguage = dbLanguage;
        }

        public virtual string Create(IColumnMetaData column)
        {
            var propertyType = column.Property.PropertyType;

            if (ColumnTypes.ContainsKey(propertyType))
            {
                return ColumnTypes[propertyType](this, column);
            }

            throw new NotSupportedException(string.Format("{0} is not supported.", propertyType));
        }

        [return: AllowNull]
        public virtual string GetNullDefinition(IColumnMetaData column)
        {
            if (column.IsNullable)
            {
                return null;
            }

            return "NOT NULL";
        }

        public abstract string Int32Column(IColumnMetaData column);
        public abstract string StringColumn(IColumnMetaData column);
    }
}
