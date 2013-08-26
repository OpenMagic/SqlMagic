using System;

namespace SqlMagic.Common
{
    interface IDatabase
    {
        void CreateTable<TRow>();
        void CreateTable(Type rowType);
        void CreateTable(TableMetaData table);

        int GetLastId<TRow>();
        int GetLastId(Type rowType);
        int GetLastId(TableMetaData table);

        void Insert<TRow>(TRow row);
        void Insert<TRow>(TRow row, TableMetaData table);
    }
}
