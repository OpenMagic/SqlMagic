using System;

namespace SqlMagic
{
    interface IDatabase
    {
        void CreateTable<TRow>();
        void CreateTable(Type rowType);
        void CreateTable(ITableMetaData table);

        int GetLastId<TRow>();
        int GetLastId(Type rowType);
        int GetLastId(ITableMetaData table);

        void Insert<TRow>(TRow row);
        void Insert<TRow>(TRow row, ITableMetaData table);
    }
}
