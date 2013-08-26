using System;
using System.Reflection;

namespace SqlMagic.Common
{
    public interface IColumnMetaData
    {
        string ColumnName { get; set; }
        bool IsIdColumn { get; set; }
        bool IsNullable { get; set; }
        PropertyInfo Property { get; set; }
        TableMetaData Table { get; set; }
    }
}
