using System;
using System.Collections.Generic;

namespace SqlMagic.Common
{
    public interface ITableMetaData
    {
        IList<IColumnMetaData> Columns { get; set; }
        IEnumerable<IColumnMetaData> GetColumns(bool excludeId = false);
        IColumnMetaData GetIdColumn();
        Type RowType { get; set; }
        string TableName { get; set; }
    }
}
